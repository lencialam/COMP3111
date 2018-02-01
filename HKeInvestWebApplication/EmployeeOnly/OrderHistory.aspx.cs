using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Globalization;

namespace HKeInvestWebApplication.EmployeeOnly
{
    public partial class OrderHistory : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string today = (DateTime.Today).ToString("dd/MM/yyyy");
                DateFrom.Text = today;
                DateTo.Text = today;

                // Get the available currencies to populate the DropDownList.
                DataTable dtCurrency = myExternalFunctions.getCurrencyData();
                foreach (DataRow row in dtCurrency.Rows)
                {
                    string currency = row["currency"].ToString().Trim();
                    ViewState[currency] = row["rate"].ToString().Trim();
                }

            }
        }

        protected void Check_Click(object sender, EventArgs e)
        {

            if (!Page.IsValid) { return; }

            // Reset visbility of controls and initialize values.
            Filtering.Visible = false;
            gvOrder.Visible = false;
            trans.Visible = false;
            lblAccountMessage.Visible = false;
            lblDateError.Visible = false;
            lblResultMessage.Visible = false;
            string sql = "";

            DateTime DF = DateTime.ParseExact("01/01/1973", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime DT = DateTime.ParseExact("31/12/9999", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (!string.IsNullOrWhiteSpace(DateFrom.Text))
            {
                DF = DateTime.ParseExact(DateFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrWhiteSpace(DateTo.Text))
            {
                DT = DateTime.ParseExact(DateTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            if (DateTime.Compare(DF, DT) > 0) //DateFrom greater (later) than DateTo
            {
                lblDateError.Text = "Start Date should not be later than End Date";
                lblDateError.Visible = true;
                return;
            }

            string accountNumber = txtAccountNumber.Text; // Set the account number from a web form control

            // Check if an account number has been specified.
            if (accountNumber == "")
            {
                lblAccountMessage.Text = "Please specify an account number.";
                lblAccountMessage.Visible = true;
                return;
            }

            sql = "select accountNumber from Account where accountNumber = '" + accountNumber + "'";
            DataTable dtAccount = myHKeInvestData.getData(sql);
            if (dtAccount == null) { return; } // If the DataSet is null, a SQL error occurred.
            // If no result is returned by the SQL statement, then display a message.
            if (dtAccount.Rows.Count == 0)
            {
                lblAccountMessage.Text = "No such account number.";
                lblAccountMessage.Visible = true;
                return;
            }

            lblAccountMessage.Text = "The account number: " + accountNumber;
            lblAccountMessage.Visible = true;

            //initialized sql for quering
            sql = "Select OrderNumber, 'b/s' as buysell, SecurityType, SecurityCode, 'name' as name, DateOfSubmission, CurrentStatus, 'N/A' as TotalShares, 'N/A' as TotalAmount, Fee from \"Order\" WHERE ";

            //Add something to the WHERE part of SQL about the security
            if (FilterByType.Checked)
            {
                string SType = ddlSecurityType.SelectedValue.ToString();
                string SCode = txtSecurityCode.Text.ToString();
                DataTable temp = myExternalFunctions.getSecuritiesByCode(SType, SCode);
                if (temp == null)
                {
                    lblResultMessage.Text = "No matching for this security type and the corresponding code, please check it.";
                    lblResultMessage.Visible = true;
                    Filtering.Visible = true;
                    gvOrder.Visible = true;
                    trans.Visible = true;
                    return;
                }
                string name = temp.Rows[0]["name"].ToString();
                sql = sql + "SecurityType = '" + SType + "' AND SecurityCode = '" + SCode + "' AND ";
            }

            //Add something to the WHERE part of SQL about the order type
            if ((OrderStatus.SelectedIndex == -1))
            //When no orderstatus are specified (meaning all), not filtering bytype
            {
                //DO NOTHING
            }
            else
            {
                string short_sql = "";
                if (OrderStatus.Items[0].Selected) short_sql = short_sql + "OR CurrentStatus = 'completed' ";
                if (OrderStatus.Items[1].Selected) short_sql = short_sql + "OR CurrentStatus = 'partial' ";
                if (OrderStatus.Items[2].Selected) short_sql = short_sql + "OR CurrentStatus = 'pending' ";
                if (OrderStatus.Items[3].Selected) short_sql = short_sql + "OR CurrentStatus = 'cancelled' ";
                short_sql = " (" + short_sql.Substring(2) + ") AND ";
                sql = sql + short_sql;
            }

            //Finish the SQL with the necessary information.
            sql = sql +
                "AccountNumber = '" + accountNumber + "' and DateOfSubmission >= convert(datetime, '" + DF.ToString("dd/MM/yyyy") + "', 103)AND DateOfSubmission <= convert(datetime, '" + DT.ToString("dd/MM/yyyy") + "', 103) order by DateOfSubmission DESC";



            DataTable dtOrder = myHKeInvestData.getData(sql);
            if (dtOrder == null) { return; } // If the DataSet is null, a SQL error occurred.
            // If no result is returned, then display a message that no active orders are associatied with the account
            if (dtOrder.Rows.Count == 0)
            {
                lblResultMessage.Text = "There is no orders associated with this account, satifying thses specified criteria.";
                lblResultMessage.Visible = true;
                Filtering.Visible = true;
                gvOrder.Visible = false;
                trans.Visible = false;
                return;
            }

            DataTable dtTDetails = new DataTable();
            List<int> deletedindex = new List<int>();
            int dtRow = 0;
            foreach (DataRow row in dtOrder.Rows)
            {
                string OrderNumber = row["OrderNumber"].ToString().Trim();
                string securityType = row["SecurityType"].ToString().Trim();
                string securityCode = row["SecurityCode"].ToString().Trim();
                DataTable dtname = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);
                string name = dtname.Rows[0]["name"].ToString();
                dtOrder.Rows[dtRow]["name"] = name;
                bool whether_BUT = false;

                //Assigning buy/sell
                if (securityType == "stock")
                {
                    sql = "select OrderNumber from StockBuying where OrderNumber = '" + OrderNumber + "'";
                    DataTable dtStockBuy = myHKeInvestData.getData(sql);
                    if (dtStockBuy.Rows.Count != 0) //i.e. this order is indeed a stock buy order.
                    {
                        if (BuySell.Items[2].Selected) { deletedindex.Add(dtRow); dtRow = dtRow + 1; continue; }
                        dtOrder.Rows[dtRow]["buysell"] = "Buy";
                    }
                    else
                    {
                        sql = "select OrderNumber from StockSelling where OrderNumber = '" + OrderNumber + "'";
                        DataTable dtStockSell = myHKeInvestData.getData(sql);
                        if (dtStockSell.Rows.Count != 0) //i.e. this order is indeed a stock sell order.
                        {
                            if (BuySell.Items[1].Selected) { deletedindex.Add(dtRow); dtRow = dtRow + 1; continue; }
                            dtOrder.Rows[dtRow]["buysell"] = "Sell";
                        }
                    }
                }
                else if (securityType == "bond" || securityType == "unit trust")
                {
                    whether_BUT = true;
                    sql = "select OrderNumber from BUTBuying where OrderNumber = '" + OrderNumber + "'";
                    DataTable dtBUTBuy = myHKeInvestData.getData(sql);
                    if (dtBUTBuy.Rows.Count != 0) //i.e. this order is indeed a BUT buy order.
                    {
                        if (BuySell.Items[2].Selected) { deletedindex.Add(dtRow); dtRow = dtRow + 1; continue; }
                        dtOrder.Rows[dtRow]["buysell"] = "Buy";
                    }
                    else
                    {
                        sql = "select OrderNumber from BUTSelling where OrderNumber = '" + OrderNumber + "'";
                        DataTable dtBUTSell = myHKeInvestData.getData(sql);
                        if (dtBUTSell.Rows.Count != 0) //i.e. this order is indeed a BUT sell order.
                        {
                            if (BuySell.Items[1].Selected) { deletedindex.Add(dtRow); dtRow = dtRow + 1; continue; }
                            dtOrder.Rows[dtRow]["buysell"] = "Sell";
                        }
                    }
                }

                //Assign totalshares and totalAmount

                sql = "select count(*) as cnt, sum(executeshares) as totalshares, sum(executeshares * executeprice) as totalamount from \"transaction\" where ordernumber = '" + OrderNumber + "'";
                DataTable dtTransactionInfo = myHKeInvestData.getData(sql);
                if (dtTransactionInfo.Rows[0]["cnt"].ToString().Equals("0")) //No transaction involved
                {
                    dtOrder.Rows[dtRow]["TotalShares"] = "0";
                    dtOrder.Rows[dtRow]["TotalAmount"] = "0";
                    dtOrder.Rows[dtRow]["Fee"] = "0";
                }
                else {
                    if (securityType == "stock")
                    {
                        dtOrder.Rows[dtRow]["TotalShares"] = dtTransactionInfo.Rows[0]["totalshares"];
                        dtOrder.Rows[dtRow]["TotalAmount"] = dtTransactionInfo.Rows[0]["totalamount"];
                    }
                    else if (securityType == "bond" || securityType == "unit trust")
                    {
                        string baseCurrency = dtname.Rows[0]["base"].ToString();
                        decimal totalvalue = Convert.ToDecimal(dtTransactionInfo.Rows[0]["totalamount"]);
                        totalvalue = myHKeInvestCode.convertCurrency(baseCurrency, ViewState[baseCurrency].ToString().Trim(), "HKD", ViewState["HKD"].ToString().Trim(), totalvalue);
                        dtOrder.Rows[dtRow]["TotalShares"] = dtTransactionInfo.Rows[0]["totalshares"];
                        dtOrder.Rows[dtRow]["TotalAmount"] = (Math.Round(totalvalue, 2)).ToString();
                    }
                }


                if (whether_BUT)
                {
                    string basecurrency = dtname.Rows[0]["base"].ToString();
                    sql = "SELECT OrderNumber, TransactionNumber=REPLACE(STR(TransactionNumber, 8), SPACE(1), '0'), ExecuteDate,ExecuteShares,ExecutePrice, '" + basecurrency + "' AS BaseCurrency FROM \"Transaction\" WHERE OrderNumber = '" + OrderNumber + "'";
                }
                else { sql = "SELECT OrderNumber, TransactionNumber=REPLACE(STR(TransactionNumber, 8), SPACE(1), '0'), ExecuteDate,ExecuteShares,ExecutePrice, 'HKD' AS BaseCurrency FROM \"Transaction\" WHERE OrderNumber = '" + OrderNumber + "'"; }
                DataTable dtTransactionDetail = myHKeInvestData.getData(sql);
                dtTDetails.Merge(dtTransactionDetail);
                dtRow = dtRow + 1;
            }
            gvTransaction.DataSource = dtTDetails;
            gvTransaction.DataBind();
            gvTransaction.Visible = true;


            //Delete all the records that should be deleted...
            for (int i = deletedindex.Count - 1; i >= 0; i--)
            {
                dtOrder.Rows[deletedindex[i]].Delete();
            }
            dtOrder.AcceptChanges();

            // Set the initial sort expression and sort direction for sorting the GridView in ViewState.
            ViewState["SortExpression"] = "dateofsubmission";
            ViewState["SortDirection"] = "DESC";

            // Bind the DataTable to the GridView.
            if (dtOrder.Rows.Count == 0)
            {
                lblResultMessage.Text = "There is no orders associated with this account, satifying thses specified criteria.";
                lblResultMessage.Visible = true;
            }
            gvOrder.DataSource = dtOrder;
            gvOrder.DataBind();
            gvOrder.Visible = true;
            trans.Visible = true;
            Filtering.Visible = true; //Let the user be able to filter.

            if (dtTDetails.Rows.Count == 0) { trans.Visible = false; }


            return;
        }


        protected void gvOrder_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Since a GridView cannot be sorted directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
            // Create a DataTable from the GridView.
            DataTable dtOrder = myHKeInvestCode.unloadGridView(gvOrder);

            // Set the sort expression in ViewState for correct toggling of sort direction,
            // Sort the DataTable and bind it to the GridView.
            string sortExpression = e.SortExpression.ToLower();
            dtOrder.DefaultView.Sort = sortExpression + " " + myHKeInvestCode.getSortDirection(ViewState, e.SortExpression);
            dtOrder.AcceptChanges();
            ViewState["SortExpression"] = sortExpression;

            // Bind the DataTable to the GridView.
            gvOrder.DataSource = dtOrder.DefaultView;
            gvOrder.DataBind();
        }

    }
}