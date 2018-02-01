using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication
{
    public partial class ProfitLoss : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get the username and the account number of the client and store them in the viewstate
                string username = Context.User.Identity.GetUserName().Trim();
                ViewState["username"] = username;
                string sql = "select accountNumber from Account where userName='" + username + "'";
                DataTable dtAcctNum = myHKeInvestData.getData(sql);
                if (dtAcctNum.Rows.Count == 1)
                {
                    ViewState["acctNum"] = dtAcctNum.Rows[0]["accountNumber"].ToString().Trim();
                }


                // Get the available currencies to populate the DropDownList.
                DataTable dtCurrency = myExternalFunctions.getCurrencyData();
                foreach (DataRow row in dtCurrency.Rows)
                {
                    ddlCurrency.Items.Add(row["currency"].ToString().Trim());
                    string currency = row["currency"].ToString().Trim();
                    ViewState[currency] = row["rate"].ToString().Trim();
                }

                //Hide the dropdownlist for security type
                SecurityTypeDiv.Disabled = true;
                SecurityTypeDiv.Visible = false;

                //Hide the dropdownlist for currency
                ddlCurrencyDiv.Visible = false;

                //Hide all the gridviews
                SecurityListDiv.Visible = false;
                IndividualSecurityPLDiv.Visible = false;
                AllSecurityPLDiv.Visible = false;
                OneTypeSecurityPLDiv.Visible = false;

                //Hide the message div
                msgDiv.Visible = false;
            }
        }

        protected void TrackingType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (TrackingType.SelectedIndex!=0)
            {
                //Hide all the gridview div
                SecurityListDiv.Visible = false;
                IndividualSecurityPLDiv.Visible = false;
                AllSecurityPLDiv.Visible = false;
                OneTypeSecurityPLDiv.Visible = false;
                ddlCurrencyDiv.Visible = false;
                ddlCurrency.SelectedIndex = 0;
                showHiddenField(false);
                ViewState["Tracking"] = TrackingType.SelectedIndex;

                if (TrackingType.SelectedIndex == 1)
                {
                    SecurityTypeDiv.Disabled = true;
                    SecurityTypeDiv.Visible = false;

                    AllSecurityPLDiv.Visible = true;

                    //Not Finish
                    
                    //Code for getting the profit/loss for all securities
                    string acctNum = ViewState["acctNum"].ToString().Trim();
                    string sql = "select SecurityType as type, SecurityCode as code from \"Order\" where AccountNumber='" + acctNum + "' group by SecurityType, SecurityCode";
                    DataTable dtSecurityHolding = myHKeInvestData.getData(sql);

                    //If the Client have no security holdings
                    if (dtSecurityHolding == null || dtSecurityHolding.Rows.Count < 1)
                    {
                        msgDiv.Visible = true;
                        errormsg.Visible = true;
                        successmsg.Visible = false;

                        errormsg.Text = "You have no previous orders in your account";
                        AllSecurityPLDiv.Visible = false;
                        ddlCurrencyDiv.Visible = false;
                        return;
                    }

                    decimal buyTotal = 0;
                    decimal sellTotal = 0;
                    decimal fee = 0;
                    decimal currentValue = myHKeInvestCode.GetMonetaryValue(acctNum,"all");
                    decimal PL = 0;
                    decimal PLP = 0;

                    for(int i=0;i<dtSecurityHolding.Rows.Count;i++)
                    {
                        string type = dtSecurityHolding.Rows[i]["type"].ToString().Trim();
                        string code = dtSecurityHolding.Rows[i]["code"].ToString().Trim();
                        buyTotal += getBuyTotal(acctNum, type, code);
                        sellTotal += getSellTotal(acctNum, type, code);
                        fee += getFee(acctNum, type, code);
                    }

                    DataTable dtPL = new DataTable();
                    dtPL.Clear();
                    dtPL.Columns.Add("buy", typeof(string));
                    dtPL.Columns.Add("sell", typeof(string));
                    dtPL.Columns.Add("fee", typeof(string));
                    dtPL.Columns.Add("PL", typeof(string));
                    dtPL.Columns.Add("PLP", typeof(string));
                    DataRow dr = dtPL.NewRow();
                    dr["buy"] = Convert.ToString(Math.Round(buyTotal,2));
                    dr["sell"] = Convert.ToString(Math.Round(sellTotal,2));
                    dr["fee"] = Convert.ToString(Math.Round(fee,2));
                    PL = currentValue + sellTotal - buyTotal - fee;
                    PLP = (PL / (buyTotal+fee))*100;
                    dr["PL"] = Convert.ToString(Math.Round(PL,2));
                    dr["PLP"] = Convert.ToString(Math.Round(PLP, 2)) + "%";
                    dtPL.Rows.Add(dr);

                    gvAllPL.DataSource = dtPL;
                    gvAllPL.DataBind();

                    ddlCurrencyDiv.Visible = true;
                    msgDiv.Visible = false;

                    dtPL.Columns.Add("buyforeign", typeof(string));
                    dtPL.Columns.Add("sellforeign", typeof(string));
                    dtPL.Columns.Add("feeforeign", typeof(string));
                    dtPL.Columns.Add("PLforeign", typeof(string));

                    ViewState["SecuritiesPL"] = dtPL;
                    ViewState["Tracking"] = 1;
                }
                else if (TrackingType.SelectedIndex == 2 || TrackingType.SelectedIndex == 3)
                {
                    SecurityTypeDiv.Disabled = false;
                    SecurityTypeDiv.Visible = true;
                    SecurityType.SelectedIndex = 0;
                    ddlCurrencyDiv.Visible = false;
                }
            }

        }

        protected void SecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (ViewState["Tracking"].ToString().Trim().Equals("2"))
                {
                    //Show the div for the gridview and hide others
                    OneTypeSecurityPLDiv.Visible = true;
                    SecurityListDiv.Visible = false;
                    IndividualSecurityPLDiv.Visible = false;
                    AllSecurityPLDiv.Visible = false;
                    ddlCurrencyDiv.Visible = false;
                    ddlCurrency.SelectedIndex = 0;
                    showHiddenField(false);

                    //Code for getting the profit and loss for all of one type of securities 
                    string acctNum = ViewState["acctNum"].ToString().Trim();
                    string type = SecurityType.SelectedValue.Trim();

                    string sql = "select SecurityType as type, SecurityCode as code from \"Order\" where SecurityType='"+type+"' AND AccountNumber='" + acctNum + "' group by SecurityType, SecurityCode";
                    DataTable dtSecurityHolding = myHKeInvestData.getData(sql);

                    //If the Client have no security holdings
                    if (dtSecurityHolding == null || dtSecurityHolding.Rows.Count < 1)
                    {
                        msgDiv.Visible = true;
                        errormsg.Visible = true;
                        successmsg.Visible = false;

                        errormsg.Text = "You have no previous orders for "+SecurityType.SelectedValue+" in your account";
                        OneTypeSecurityPLDiv.Visible = false;
                        ddlCurrencyDiv.Visible = false;
                        return;
                    }

                    decimal buyTotal = 0;
                    decimal sellTotal = 0;
                    decimal fee = 0;
                    decimal currentValue = myHKeInvestCode.GetMonetaryValue(acctNum, type);
                    decimal PL = 0;
                    decimal PLP = 0;

                    for (int i = 0; i < dtSecurityHolding.Rows.Count; i++)
                    {
                        string code = dtSecurityHolding.Rows[i]["code"].ToString().Trim();
                        buyTotal += getBuyTotal(acctNum, type, code);
                        sellTotal += getSellTotal(acctNum, type, code);
                        fee += getFee(acctNum, type, code);
                    }

                    DataTable dtPL = new DataTable();
                    dtPL.Clear();
                    dtPL.Columns.Add("buy", typeof(string));
                    dtPL.Columns.Add("sell", typeof(string));
                    dtPL.Columns.Add("fee", typeof(string));
                    dtPL.Columns.Add("PL", typeof(string));
                    dtPL.Columns.Add("type", typeof(string));
                    dtPL.Columns.Add("PLP", typeof(string));
                    DataRow dr = dtPL.NewRow();
                    dr["buy"] = Convert.ToString(Math.Round(buyTotal, 2));
                    dr["sell"] = Convert.ToString(Math.Round(sellTotal, 2));
                    dr["fee"] = Convert.ToString(Math.Round(fee, 2));
                    PL = currentValue + sellTotal - buyTotal - fee;
                    PLP = (PL / (buyTotal+fee)) * 100;
                    dr["PL"] = Convert.ToString(Math.Round(PL, 2));
                    dr["PLP"] = Convert.ToString(Math.Round(PLP, 2)) + "%";
                    dr["type"] = type;
                    dtPL.Rows.Add(dr);

                    gvOneTypePL.DataSource = dtPL;
                    gvOneTypePL.DataBind();

                    msgDiv.Visible = false;
                    ddlCurrencyDiv.Visible = true;

                    dtPL.Columns.Add("buyforeign", typeof(string));
                    dtPL.Columns.Add("sellforeign", typeof(string));
                    dtPL.Columns.Add("feeforeign", typeof(string));
                    dtPL.Columns.Add("PLforeign", typeof(string));

                    ViewState["SecuritiesPL"] = dtPL;
                }
                else if (ViewState["Tracking"].ToString().Trim().Equals("3"))
                {
                    //Hide the unnecessary fields and divs
                    OneTypeSecurityPLDiv.Visible = false;
                    IndividualSecurityPLDiv.Visible = false;
                    AllSecurityPLDiv.Visible = false;
                    ddlCurrencyDiv.Visible = false;
                    ddlCurrency.SelectedIndex = 0;
                    showHiddenField(false);

                    //Display the list for the security holding of the Client
                    SecurityListDiv.Visible = true;
                    string acctNum = ViewState["acctNum"].ToString().Trim();
                    string type = SecurityType.SelectedValue;
                    string sql = "select type, code, name from SecurityHolding where type='" + type +"' AND accountNumber='" + acctNum + "'";

                    DataTable dtSecurityList = myHKeInvestData.getData(sql);
                    if (dtSecurityList == null || dtSecurityList.Rows.Count < 1)
                    {
                        msgDiv.Visible = true;
                        errormsg.Visible = true;
                        successmsg.Visible = false;

                        errormsg.Text = "You have no previous order for "+SecurityType.SelectedValue+" in your account";
                        SecurityListDiv.Visible = false;
                        ddlCurrencyDiv.Visible = false;
                        return;
                    }
                    gvSecurityList.DataSource = dtSecurityList;
                    gvSecurityList.DataBind();

                    msgDiv.Visible = false;  
                }
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (SecurityType.SelectedIndex == 0)
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "You must select a Security Type.";
                SecurityListDiv.Visible = false;
                IndividualSecurityPLDiv.Visible = false;
                AllSecurityPLDiv.Visible = false;
                OneTypeSecurityPLDiv.Visible = false;
                ddlCurrencyDiv.Visible = false;
                ddlCurrency.SelectedIndex = 0;
                showHiddenField(false);
            }
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (TrackingType.SelectedIndex == 0)
            {
                args.IsValid = false;
                CustomValidator2.ErrorMessage = "You must select a Profit/Loss Tracking type.";
                SecurityListDiv.Visible = false;
                IndividualSecurityPLDiv.Visible = false;
                AllSecurityPLDiv.Visible = false;
                OneTypeSecurityPLDiv.Visible = false;
                ddlCurrencyDiv.Visible = false;
                ddlCurrency.SelectedIndex = 0;
                showHiddenField(false);
            }
        }


        protected decimal getBuyTotal(string acctNum, string type, string code)
        {
            string sql;
            decimal buyTotal = 0;
            string tablename=null;
            string securityType=null;
            string baseCurr = "HKD";

            if (type.Trim().Equals("stock"))
            {
                securityType = "stock";
                tablename = "StockBuying";
                baseCurr = "HKD";
            }
            else if (type.Equals("bond"))
            {
                securityType = "bond";
                tablename = "BUTBuying";
                sql = "select base from SecurityHolding where accountNumber='" + acctNum + "' AND type='bond' AND code='" + code + "'";
                DataTable dtBond = myHKeInvestData.getData(sql);
                if(dtBond!=null&&dtBond.Rows.Count>0)
                {
                    baseCurr = dtBond.Rows[0]["base"].ToString().Trim();
                }
            }
            else if (type.Trim().Equals("unit trust"))
            {
                securityType = "unit trust";
                tablename = "BUTBuying";
                sql = "select base from SecurityHolding where accountNumber='" + acctNum + "' AND type='unit trust' AND code='" + code + "'";
                DataTable dtUnitTrust = myHKeInvestData.getData(sql);
                if (dtUnitTrust != null&&dtUnitTrust.Rows.Count>0)
                {
                    baseCurr = dtUnitTrust.Rows[0]["base"].ToString().Trim();
                }
            }

            sql = "select \"Transaction\".ExecuteShares, \"Transaction\".ExecutePrice from \"Order\", \"Transaction\", " + tablename + " where \"Order\".AccountNumber='"
            + acctNum + "' AND \"Order\".SecurityType='" + securityType + "' AND \"Order\".SecurityCode='" + code + "' AND \"Order\".OrderNumber=" + tablename
            + ".OrderNumber AND \"Order\".OrderNumber=\"Transaction\".OrderNumber";
            DataTable dtTransaction = myHKeInvestData.getData(sql);

            if (dtTransaction != null&&dtTransaction.Rows.Count>0)
            {
                for (int i = 0; i < dtTransaction.Rows.Count; i++)
                {
                    decimal share = Convert.ToDecimal(dtTransaction.Rows[i]["ExecuteShares"].ToString().Trim());
                    decimal price = Convert.ToDecimal(dtTransaction.Rows[i]["ExecutePrice"].ToString().Trim());
                    decimal convertedprice = myHKeInvestCode.convertCurrency(baseCurr, ViewState[baseCurr.Trim()].ToString().Trim(), "HKD", ViewState["HKD"].ToString().Trim(), price);
                    buyTotal += share * convertedprice;
                }
            }

            return buyTotal;
        }

        protected decimal getSellTotal(string acctNum, string type, string code)
        {
            string sql;
            decimal sellTotal = 0;
            string tablename=null;
            string securityType=null;
            string baseCurr = "HKD";

            if (type.Trim().Equals("stock"))
            {
                securityType = "stock";
                tablename = "StockSelling";
            }
            else if (type.Equals("bond"))
            {
                securityType = "bond";
                tablename = "BUTSelling";
                sql = "select base from SecurityHolding where accountNumber='" + acctNum + "' AND type='bond' AND code='" + code + "'";
                DataTable dtBond = myHKeInvestData.getData(sql);
                if (dtBond != null && dtBond.Rows.Count > 0)
                {
                    baseCurr = dtBond.Rows[0]["base"].ToString().Trim();
                }
            }
            else if (type.Trim().Equals("unit trust"))
            {
                securityType = "unit trust";
                tablename = "BUTSelling";
                sql = "select base from SecurityHolding where accountNumber='" + acctNum + "' AND type='unit trust' AND code='" + code + "'";
                DataTable dtUnitTrust = myHKeInvestData.getData(sql);
                if (dtUnitTrust != null && dtUnitTrust.Rows.Count > 0)
                {
                    baseCurr = dtUnitTrust.Rows[0]["base"].ToString().Trim();
                }
            }
            sql = "select \"Transaction\".ExecuteShares, \"Transaction\".ExecutePrice from \"Order\", \"Transaction\", " + tablename + " where \"Order\".AccountNumber='"
            + acctNum + "' AND \"Order\".SecurityType='" + securityType + "' AND \"Order\".SecurityCode='" + code + "' AND \"Order\".OrderNumber=" + tablename
            + ".OrderNumber AND \"Order\".OrderNumber=\"Transaction\".OrderNumber";
            DataTable dtTransaction = myHKeInvestData.getData(sql);

            if (dtTransaction != null&&dtTransaction.Rows.Count>0)
            {
                for (int i = 0; i < dtTransaction.Rows.Count; i++)
                {
                    decimal share = Convert.ToDecimal(dtTransaction.Rows[i]["ExecuteShares"].ToString().Trim());
                    decimal price = Convert.ToDecimal(dtTransaction.Rows[i]["ExecutePrice"].ToString().Trim());
                    decimal convertedprice = myHKeInvestCode.convertCurrency(baseCurr.Trim(), ViewState[baseCurr.Trim()].ToString().Trim(), "HKD", ViewState["HKD"].ToString().Trim(), price);
                    sellTotal += share * convertedprice;
                }
            }

            return sellTotal;
        }

        protected decimal getFee(string acctNum, string type, string code)
        {
            string securityType = type.Trim();
            string sql = "select fee from \"Order\" where AccountNumber='" + acctNum + "' AND SecurityCode='" + code + "' AND SecurityType='"
            + securityType + "' AND fee IS NOT NULL";

            DataTable dtFee = myHKeInvestData.getData(sql);
            decimal fee = 0;

            /*
            string baseCurr = "HKD";
            if (type != "stock")
            {
                sql = "select base form SecurityHolding where accountNumber='" + acctNum + "' AND type='" + securityType + "' AND code='" + code + "'";
                DataTable dtBase = myHKeInvestData.getData(sql);
                if(dtBase!=null)
                {
                    baseCurr = dtBase.Rows[0]["base"].ToString().Trim();
                }
            }
            */

            if (dtFee != null&&dtFee.Rows.Count>0)
            {
                for (int i = 0; i < dtFee.Rows.Count; i++)
                {
                    //decimal temp = Convert.ToDecimal(dtFee.Rows[i]["fee"].ToString().Trim());
                    fee += Convert.ToDecimal(dtFee.Rows[i]["fee"].ToString().Trim());
                }
            }
            return fee;
        }

        protected void gvSecurityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            showHiddenField(false);

            string type = SecurityType.SelectedValue;
            string code = gvSecurityList.SelectedRow.Cells[1].Text.Trim();
            string acctNum = ViewState["acctNum"].ToString().Trim();

            string sql = "select type, code, name, Shares from SecurityHolding where accountNumber='" + acctNum + "' AND type='" + type + "' AND code='" + code + "'";
            DataTable dtSecurity = myHKeInvestData.getData(sql);

            dtSecurity.Columns.Add("buy", typeof(string));
            dtSecurity.Columns.Add("sell", typeof(string));
            dtSecurity.Columns.Add("fee", typeof(string));
            dtSecurity.Columns.Add("PL", typeof(string));
            dtSecurity.Columns.Add("PLP", typeof(string));

            decimal buy = getBuyTotal(acctNum, type, code);
            decimal sell = getSellTotal(acctNum, type, code);
            decimal fee = getFee(acctNum, type, code);

            //Get the current value of the security
            string baseCurr = "HKD";
            string needBase = "";
            if(!type.Trim().Equals("stock"))
            {
                needBase = " base,";
            }
            sql = "select"+needBase+" shares from SecurityHolding where accountNumber='" + acctNum + "' AND type='" + type + "' AND code='" + code + "'";
            DataTable dtCurrentValue = myHKeInvestData.getData(sql);

            decimal share = Convert.ToDecimal(dtCurrentValue.Rows[0]["shares"]);
            decimal price = (share!=0)?myExternalFunctions.getSecuritiesPrice(type, code):0;
            if (!type.Trim().Equals("stock"))
            {
                baseCurr = dtCurrentValue.Rows[0]["base"].ToString().Trim();
            }
            price = myHKeInvestCode.convertCurrency(baseCurr, ViewState[baseCurr].ToString().Trim(), "HKD", ViewState["HKD"].ToString().Trim(), price);

            decimal currentValue = (share!=0)?price * share:0;
            decimal PL = currentValue + sell - buy - fee;
            decimal PLP = (PL / (buy+ fee)) * 100;

            dtSecurity.Rows[0]["buy"] = Math.Round(buy,2).ToString();
            dtSecurity.Rows[0]["sell"] = Math.Round(sell,2).ToString();
            dtSecurity.Rows[0]["fee"] = Math.Round(fee,2).ToString();
            dtSecurity.Rows[0]["PL"] = Math.Round(PL,2).ToString();
            dtSecurity.Rows[0]["PLP"] = Math.Round(PLP, 2).ToString()+"%";


            dvIndividualPL.DataSource = dtSecurity;
            dvIndividualPL.DataBind();

            //Hide the unnecessary fields
            msgDiv.Visible = false;
            ddlCurrencyDiv.Visible = true;
            IndividualSecurityPLDiv.Visible = true;

            dtSecurity.Columns.Add("buyforeign", typeof(string));
            dtSecurity.Columns.Add("sellforeign", typeof(string));
            dtSecurity.Columns.Add("feeforeign", typeof(string));
            dtSecurity.Columns.Add("PLforeign", typeof(string));

            ViewState["SecurityPL"] = dtSecurity;
        }

        protected void showHiddenField(bool show)
        {
            if(show)
            {
                if(ViewState["Tracking"].ToString().Trim().Equals("1"))
                {
                    gvAllPL.Columns[1].Visible = true;
                    gvAllPL.Columns[3].Visible = true;
                    gvAllPL.Columns[5].Visible = true;
                    gvAllPL.Columns[7].Visible = true;
                }
                else if(ViewState["Tracking"].ToString().Trim().Equals("2"))
                {
                    gvOneTypePL.Columns[2].Visible = true;
                    gvOneTypePL.Columns[4].Visible = true;
                    gvOneTypePL.Columns[6].Visible = true;
                    gvOneTypePL.Columns[8].Visible = true;
                }
                else if(ViewState["Tracking"].ToString().Trim().Equals("3"))
                {
                    dvIndividualPL.Fields[5].Visible = true;
                    dvIndividualPL.Fields[7].Visible = true;
                    dvIndividualPL.Fields[9].Visible = true;
                    dvIndividualPL.Fields[11].Visible = true;
                }
            }
            else
            {
                //Hide the hidden field for foreign currency 
                gvAllPL.Columns[1].Visible = false;
                gvAllPL.Columns[3].Visible = false;
                gvAllPL.Columns[5].Visible = false;
                gvAllPL.Columns[7].Visible = false;

                gvOneTypePL.Columns[2].Visible = false;
                gvOneTypePL.Columns[4].Visible = false;
                gvOneTypePL.Columns[6].Visible = false;
                gvOneTypePL.Columns[8].Visible = false;

                dvIndividualPL.Fields[5].Visible=false;
                dvIndividualPL.Fields[7].Visible=false;
                dvIndividualPL.Fields[9].Visible=false;
                dvIndividualPL.Fields[11].Visible=false;
            }
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            string toCurrency = ddlCurrency.SelectedValue.Trim();

            if (ddlCurrency.SelectedIndex == 0)
            {
                //If no currency is selected
                showHiddenField(false);
                return;
            }
            else
            {
                if (ViewState["Tracking"].ToString().Trim().Equals("1"))
                {
                    DataTable dtPL = (DataTable)ViewState["SecuritiesPL"];
                    dtPL.Rows[0]["buyforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["buy"])), 2);
                    dtPL.Rows[0]["sellforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["sell"])), 2);
                    dtPL.Rows[0]["feeforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["fee"])),2);
                    dtPL.Rows[0]["PLforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["PL"])), 2);

                    gvAllPL.Columns[1].HeaderText = "Total dollar amount from buying" + "(" + toCurrency + ")";
                    gvAllPL.Columns[3].HeaderText = "Total amount from selling" + "(" + toCurrency + ")";
                    gvAllPL.Columns[5].HeaderText = "Total fees paid" + "(" + toCurrency + ")";
                    gvAllPL.Columns[7].HeaderText = "Total profit/loss" + "(" + toCurrency + ")";

                    //Function to show the hidden field
                    showHiddenField(true);

                    gvAllPL.DataSource = dtPL;
                    gvAllPL.DataBind();


                }
                else if (ViewState["Tracking"].ToString().Trim().Equals("2"))
                {
                    DataTable dtPL = (DataTable)ViewState["SecuritiesPL"];
                    dtPL.Rows[0]["buyforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["buy"])), 2);
                    dtPL.Rows[0]["sellforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["sell"])), 2);
                    dtPL.Rows[0]["feeforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["fee"])), 2);
                    dtPL.Rows[0]["PLforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["PL"])), 2);

                    gvOneTypePL.Columns[2].HeaderText = "Total dollar amount from buying" + "(" + toCurrency + ")";
                    gvOneTypePL.Columns[4].HeaderText = "Total amount from selling" + "(" + toCurrency + ")";
                    gvOneTypePL.Columns[6].HeaderText = "Total fees paid" + "(" + toCurrency + ")";
                    gvOneTypePL.Columns[8].HeaderText = "Total profit/loss" + "(" + toCurrency + ")";

                    //Function to show the hidden field
                    showHiddenField(true);

                    gvOneTypePL.DataSource = dtPL;
                    gvOneTypePL.DataBind();
                }
                else if (ViewState["Tracking"].ToString().Trim().Equals("3"))
                {
                    DataTable dtPL = (DataTable)ViewState["SecurityPL"];
                    dtPL.Rows[0]["buyforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["buy"])), 2);
                    dtPL.Rows[0]["sellforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["sell"])), 2);
                    dtPL.Rows[0]["feeforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["fee"])), 2);
                    dtPL.Rows[0]["PLforeign"] = Math.Round(myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency, ViewState[toCurrency].ToString().Trim(), Convert.ToDecimal(dtPL.Rows[0]["PL"])), 2);

                    dvIndividualPL.Fields[5].HeaderText = "Total dollar amount from buying" + "(" + toCurrency + ")";
                    dvIndividualPL.Fields[7].HeaderText = "Total amount from selling" + "(" + toCurrency + ")";
                    dvIndividualPL.Fields[9].HeaderText = "Total fees paid" + "(" + toCurrency + ")";
                    dvIndividualPL.Fields[11].HeaderText = "Profit/Loss" + "(" + toCurrency + ")";

                    //Function to show the hidden field
                    showHiddenField(true);

                    dvIndividualPL.DataSource = dtPL;
                    dvIndividualPL.DataBind();
                }
            }
        }
    }
}