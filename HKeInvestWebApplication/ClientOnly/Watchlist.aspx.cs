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
    public partial class Watchlist : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Hide the msgdiv
                msgDiv.Visible = false;


                //Get the currency table using the session state
                if (Session["kengdie"] == null)
                    myHKeInvestCode.getCurrencyToSession(Session);

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
                DataTable dtCurrency = (DataTable)Session["kengdie"];
                foreach (DataRow row in dtCurrency.Rows)
                {
                    ddlCurrency.Items.Add(row["currency"].ToString().Trim());
                    string currency = row["currency"].ToString().Trim();
                    ViewState[currency] = row["rate"].ToString().Trim();
                }


                //Obtain and display all the securities in the watchlist first
                string acctNum = ViewState["acctNum"].ToString().Trim();
                sql = "select SecurityCode, SecurityType, SecurityName from WatchlistSecurity where AccountNumber='" + acctNum + "'";
                DataTable dtAllWatchSecurities = myHKeInvestData.getData(sql);
                //If there is no security in the watchlist
                if (dtAllWatchSecurities == null)
                {
                    msgDiv.Visible = true;
                    errormsg.Text = "There is no available securities in your Watchlist.";
                    errormsg.Visible = true;
                    successmsg.Visible = false;
                }
                else
                {
                    dtAllWatchSecurities.Columns.Add("price", typeof(string));
                    dtAllWatchSecurities.Columns.Add("base", typeof(string));
                    dtAllWatchSecurities.Columns.Add("convertedValue", typeof(string));
                    dtAllWatchSecurities.Columns["SecurityCode"].ColumnName = "code";
                    dtAllWatchSecurities.Columns["SecurityType"].ColumnName = "type";
                    dtAllWatchSecurities.Columns["SecurityName"].ColumnName = "name";

                    for (int i = 0; i < dtAllWatchSecurities.Rows.Count; i++)
                    {
                        string type = dtAllWatchSecurities.Rows[i]["type"].ToString().Trim();
                        string code = dtAllWatchSecurities.Rows[i]["code"].ToString().Trim();
                        dtAllWatchSecurities.Rows[i]["convertedValue"] = null;
                        DataTable dtSecurity = myExternalFunctions.getSecuritiesByCode(type, code);
                        if (dtSecurity != null)
                        {
                            dtAllWatchSecurities.Rows[i]["name"] = dtSecurity.Rows[0]["name"];
                            if (type == "stock")
                            {
                                dtAllWatchSecurities.Rows[i]["base"] = "HKD";
                                dtAllWatchSecurities.Rows[i]["price"] = dtSecurity.Rows[0]["close"];
                            }
                            else
                            {
                                dtAllWatchSecurities.Rows[i]["base"] = dtSecurity.Rows[0]["base"];
                                dtAllWatchSecurities.Rows[i]["price"] = dtSecurity.Rows[0]["price"];
                            }
                        }
                    }

                    //Bind the datasource to the gridview
                    gvSecurityDetails.DataSource = dtAllWatchSecurities;
                    gvSecurityDetails.DataBind();

                    //Store the current datasource into viewstate
                    ViewState["CurrentWatchlist"] = dtAllWatchSecurities;
                }
            }
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            int convertValueIndex = myHKeInvestCode.getColumnIndexByName(gvSecurityDetails, "convertedValue");

            //Record the selected To currency
            string toCurrency = ddlCurrency.SelectedValue;

            //Do nothing if no currency is selected
            if (toCurrency == "0")
            {
                gvSecurityDetails.Columns[convertValueIndex].Visible = false;
                return;
            }

            //Get the current watchlist from the viewstate
            DataTable dtSecurity = (DataTable)ViewState["CurrentWatchlist"];
            int dtRows = 0;
            foreach (DataRow row in dtSecurity.Rows)
            {
                string fromCurrency = row["base"].ToString().Trim();
                if (String.IsNullOrEmpty(row["price"].ToString().Trim()))
                {
                    dtSecurity.Rows[dtRows]["convertedValue"] = null;
                }
                else
                {
                    decimal value = Convert.ToDecimal(row["price"]);
                    decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                    dtSecurity.Rows[dtRows]["convertedValue"] = convertedValue;
                }
                dtRows = dtRows + 1;
            }

            //Upload the datatable to the gridview and show the converted value
            gvSecurityDetails.Columns[convertValueIndex].Visible = true;
            gvSecurityDetails.Columns[convertValueIndex].HeaderText = "Value in " + toCurrency;
            gvSecurityDetails.DataSource = dtSecurity;
            gvSecurityDetails.DataBind();
            msgDiv.Visible = false;
        }

        protected void SecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obtain and display all the securities in the watchlist first
            string acctNum = ViewState["acctNum"].ToString().Trim();
            string sql;
            if (SecurityType.SelectedIndex == 0)
            {
                sql = "select SecurityCode, SecurityType, SecurityName from WatchlistSecurity where AccountNumber='" + acctNum + "'";
            }
            else if (SecurityType.SelectedIndex == 1)
            {
                sql = "select SecurityCode, SecurityType, SecurityName from WatchlistSecurity where AccountNumber='" + acctNum + "' AND SecurityType='stock'";
            }
            else if (SecurityType.SelectedIndex == 2)
            {
                sql = "select SecurityCode, SecurityType, SecurityName from WatchlistSecurity where AccountNumber='" + acctNum + "' AND SecurityType='bond'";
            }
            else
            {
                sql = "select SecurityCode, SecurityType, SecurityName from WatchlistSecurity where AccountNumber='" + acctNum + "' AND SecurityType='unit trust'";
            }

            DataTable dtWatchlist = myHKeInvestData.getData(sql);
            //If there is no security in the watchlist
            if (dtWatchlist.Rows.Count == 0)
            {
                msgDiv.Visible = true;
                if (SecurityType.SelectedIndex == 0)
                {
                    errormsg.Text = "There is no available securities in your Watchlist.";
                }
                else
                {
                    errormsg.Text = "There is no available " + SecurityType.SelectedValue.ToString() + " in your Watchlist.";
                }
                errormsg.Visible = true;
                successmsg.Visible = false;
                return;
            }

            dtWatchlist.Columns.Add("price", typeof(string));
            dtWatchlist.Columns.Add("base", typeof(string));
            dtWatchlist.Columns.Add("convertedValue", typeof(string));
            dtWatchlist.Columns["SecurityCode"].ColumnName = "code";
            dtWatchlist.Columns["SecurityType"].ColumnName = "type";
            dtWatchlist.Columns["SecurityName"].ColumnName = "name";

            for (int i = 0; i < dtWatchlist.Rows.Count; i++)
            {
                string type = dtWatchlist.Rows[i]["type"].ToString().Trim();
                string code = dtWatchlist.Rows[i]["code"].ToString().Trim();
                dtWatchlist.Rows[i]["convertedValue"] = null;
                DataTable dtSecurity = myExternalFunctions.getSecuritiesByCode(type, code);
                if (dtSecurity != null)
                {
                    dtWatchlist.Rows[i]["name"] = dtSecurity.Rows[0]["name"];
                    if (type == "stock")
                    {
                        dtWatchlist.Rows[i]["base"] = "HKD";
                        dtWatchlist.Rows[i]["price"] = dtSecurity.Rows[0]["close"];
                    }
                    else
                    {
                        dtWatchlist.Rows[i]["base"] = dtSecurity.Rows[0]["base"];
                        dtWatchlist.Rows[i]["price"] = dtSecurity.Rows[0]["price"];
                    }
                }
            }

            //Bind the datasource to the gridview
            gvSecurityDetails.DataSource = dtWatchlist;
            gvSecurityDetails.DataBind();

            //Store the current datasource into viewstate
            ViewState["CurrentWatchlist"] = dtWatchlist;

            //Hide the msg div and the column for showing the converted value in different currency
            msgDiv.Visible = false;
            int convertValueIndex = myHKeInvestCode.getColumnIndexByName(gvSecurityDetails, "convertedValue");
            gvSecurityDetails.Columns[convertValueIndex].Visible = false;
        }

        protected void gvSecurityDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = gvSecurityDetails.SelectedRow.Cells[0].Text.Trim();
            string code = gvSecurityDetails.SelectedRow.Cells[1].Text.Trim();
            string acctNum = ViewState["acctNum"].ToString().Trim();

            //Check if the security is still in the Client's watchlist
            string sql = "select count(*) from WatchlistSecurity where AccountNumber='" + acctNum + "' AND SecurityCode='" + code + "' AND SecurityType='" + type + "'";
            decimal tablesize = myHKeInvestData.getAggregateValue(sql);
            if (tablesize == 0 || tablesize == -1)
            {
                msgDiv.Visible = true;
                errormsg.Visible = true;
                successmsg.Visible = false;
                errormsg.Text = "The Security is no longer in your Watchlist.";
                return;
            }

            //Remove the security from the watchlist
            sql = "Delete from WatchlistSecurity where AccountNumber='" + acctNum + "' AND SecurityCode='" + code + "' AND SecurityType='" + type + "'";
            SqlTransaction trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);

            msgDiv.Visible = true;
            errormsg.Visible = false;
            successmsg.Visible = true;
            successmsg.Text = "The security has been removed";

            DataTable dtWatchlist = (DataTable)ViewState["CurrentWatchlist"];


            //Create a new datatable to record the latest watchlist
            DataTable dtNewWatchlist = new DataTable();
            dtNewWatchlist.Clear();

            dtNewWatchlist.Columns.Add("price", typeof(string));
            dtNewWatchlist.Columns.Add("base", typeof(string));
            dtNewWatchlist.Columns.Add("convertedValue", typeof(string));
            dtNewWatchlist.Columns.Add("code",typeof(string));
            dtNewWatchlist.Columns.Add("type",typeof(string));
            dtNewWatchlist.Columns.Add("name",typeof(string));

            int numOfRows = dtWatchlist.Rows.Count;

            for (int i=0;i<numOfRows;i++)
            {
                if (dtWatchlist.Rows[i]["type"].ToString().Trim().Equals(type) && dtWatchlist.Rows[i]["code"].ToString().Trim().Equals(code))
                {
                    //dtWatchlist.Rows[i].Delete();
                }
                else
                {
                    DataRow dr = dtNewWatchlist.NewRow();
                    dr["type"] = dtWatchlist.Rows[i]["type"].ToString().Trim();
                    dr["code"] = dtWatchlist.Rows[i]["code"].ToString().Trim();
                    dr["name"] = dtWatchlist.Rows[i]["name"].ToString().Trim();
                    dr["base"] = dtWatchlist.Rows[i]["base"].ToString().Trim();
                    dr["price"] = dtWatchlist.Rows[i]["price"].ToString().Trim();
                    dr["convertedValue"] = dtWatchlist.Rows[i]["convertedValue"].ToString().Trim();
                    dtNewWatchlist.Rows.Add(dr);
                }
            }

            gvSecurityDetails.DataSource = dtNewWatchlist;
            gvSecurityDetails.DataBind();
            ViewState["CurrentWatchlist"] = dtNewWatchlist;
        }
    }
}