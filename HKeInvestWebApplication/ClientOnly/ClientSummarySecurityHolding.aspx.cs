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
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class ClientSummarySecurityHolding : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // Get the available currencies to populate the DropDownList.
                DataTable dtCurrency = myExternalFunctions.getCurrencyData();
                foreach (DataRow row in dtCurrency.Rows)
                {
                    ddlCurrency.Items.Add(row["currency"].ToString().Trim());
                    string currency = row["currency"].ToString().Trim();
                    ViewState[currency] = row["rate"].ToString().Trim();
                }
                //Show the client name and account number in the front
                string username = Context.User.Identity.GetUserName().Trim();
                string sql = "select Client.firstName,Client.lastName,Account.accountNumber from Client, Account where Client.accountNumber=Account.accountNumber AND Account.userName='" + username + "'";
                DataTable dtClient = myHKeInvestData.getData(sql);
                string clientName = "Client(s): ";
                int i = 1;
                foreach (DataRow row in dtClient.Rows)
                {
                    clientName = clientName + row["lastName"] + ", " + row["firstName"];
                    if (dtClient.Rows.Count != i)
                    {
                        clientName = clientName + "and ";
                    }
                    i = i + 1;
                    ViewState[username] = row["accountNumber"].ToString().Trim();
                }
                lblClientName.Text = clientName;
                ClientAccountNum.Text = ViewState[username].ToString().Trim();
                lblClientName.Visible = true;
            }
        }

        protected void Check_Click(object sender, EventArgs e)
        {
            // Reset visbility of controls and initialize values.
            lblClientName.Visible = false;
            lblTotalValue.Visible = false;
            lblFreeBalance.Visible = false;
            gvSummary.Visible = false;
            links.Visible = false;
            string sql = "";

            string accountNumber = ClientAccountNum.Text; // Set the account number from a web form control


            sql = "select firstName,lastName from Client where accountNumber = '" + accountNumber + "'";
            DataTable dtClient = myHKeInvestData.getData(sql);
            if (dtClient == null) { return; } // If the DataSet is null, a SQL error occurred.

            // Show the client name(s) on the web page.
            string clientName = "Client(s): ";
            int i = 1;
            foreach (DataRow r in dtClient.Rows)
            {
                clientName = clientName + r["lastName"].ToString().Trim() + ", " + r["firstName"];
                if (dtClient.Rows.Count != i)
                {
                    clientName = clientName + "and ";
                }
                i = i + 1;
            }
            lblClientName.Text = clientName;
            lblClientName.Visible = true;

            lblTotalValue.Visible = true;
            lblFreeBalance.Visible = true;

            sql = "select balance from account where accountNumber = '" + accountNumber + "'";
            DataTable dtbalance = myHKeInvestData.getData(sql);
            if (dtbalance == null) { return; } // If the DataSet is null, a SQL error occurred.
            // Show the balance on the web page.
            lblFreeBalance.Text = "The account free balance (in HKD):" + dtbalance.Rows[0]["balance"];
            ViewState["Total_Free_Balance"] = dtbalance.Rows[0]["balance"];

            decimal bond_monetary = myHKeInvestCode.GetMonetaryValue(accountNumber, "bond");
            decimal stock_monetary = myHKeInvestCode.GetMonetaryValue(accountNumber, "stock");
            decimal ut_monetary = myHKeInvestCode.GetMonetaryValue(accountNumber, "unit trust");
            decimal total = Math.Round(bond_monetary + stock_monetary + ut_monetary, 2);
            ViewState["Total_Monetary_Value"] = total;
            lblTotalValue.Text = "The total monetary value of securities held (in HKD):" + total.ToString();

            //Compute data for last excuted order for each type.
            // For bond
            string last_bond_date = "N/A";
            string last_bond_value = "N/A";
            sql = "Select TOP 1 * From(Select convert (VARCHAR(10),DateOfSubmission,103) as Date, TransactionNumber, SecurityCode, ExecuteDate, ExecuteShares, ExecutePrice From \"Order\", \"Transaction\" " +
                "Where \"Order\".AccountNumber = '" + accountNumber + "' AND \"Order\".OrderNumber = \"Transaction\".OrderNumber AND \"Order\".SecurityType = 'bond') AS temp ORDER BY ExecuteDate DESC, TransactionNumber DESC";
            DataTable dtLastBond = myHKeInvestData.getData(sql);

            if (dtLastBond.Rows.Count != 0)  // if it == 0, not executed bond order.
            {
                decimal shares = Convert.ToDecimal(dtLastBond.Rows[0]["ExecuteShares"]);
                decimal price = Convert.ToDecimal(dtLastBond.Rows[0]["ExecutePrice"]);

                DataTable dtBond = myExternalFunctions.getSecuritiesByCode("bond", dtLastBond.Rows[0]["SecurityCode"].ToString());
                string baseCurrency = dtBond.Rows[0]["base"].ToString().Trim();
                price = myHKeInvestCode.convertCurrency(baseCurrency, ViewState[baseCurrency].ToString().Trim(), "HKD", ViewState["HKD"].ToString().Trim(), price);
                decimal value = Math.Round(shares * price, 2);
                last_bond_value = value.ToString();
                last_bond_date = dtLastBond.Rows[0]["Date"].ToString().Trim();
            }

            // For unit_trust
            string last_ut_date = "N/A";
            string last_ut_value = "N/A";
            sql = "Select TOP 1 * From(Select convert (VARCHAR(10),DateOfSubmission,103) as Date, TransactionNumber, SecurityCode, ExecuteDate, ExecuteShares, ExecutePrice From \"Order\", \"Transaction\" " +
                "Where \"Order\".AccountNumber = '" + accountNumber + "' AND \"Order\".OrderNumber = \"Transaction\".OrderNumber AND \"Order\".SecurityType = 'unit trust') AS temp ORDER BY ExecuteDate DESC, TransactionNumber DESC";
            DataTable dtLastUT = myHKeInvestData.getData(sql);

            if (dtLastUT.Rows.Count != 0)  // if it == 0, not executed bond order.
            {
                decimal shares = Convert.ToDecimal(dtLastUT.Rows[0]["ExecuteShares"]);
                decimal price = Convert.ToDecimal(dtLastUT.Rows[0]["ExecutePrice"]);

                DataTable dtUT = myExternalFunctions.getSecuritiesByCode("unit trust", dtLastUT.Rows[0]["SecurityCode"].ToString());
                string baseCurrency = dtUT.Rows[0]["base"].ToString().Trim();
                price = myHKeInvestCode.convertCurrency(baseCurrency, ViewState[baseCurrency].ToString().Trim(), "HKD", ViewState["HKD"].ToString().Trim(), price);
                decimal value = Math.Round(shares * price, 2);
                last_ut_value = value.ToString();
                last_ut_date = dtLastUT.Rows[0]["Date"].ToString().Trim();
            }

            //For Stock
            //Yet to be completed
            string last_stock_date = "N/A";
            string last_stock_value = "N/A";
            sql = "Select TOP 1 * From(Select convert (VARCHAR(10),DateOfSubmission,103) as Date, TransactionNumber, SecurityCode, ExecuteDate, ExecuteShares, ExecutePrice From \"Order\", \"Transaction\" " +
                "Where \"Order\".AccountNumber = '" + accountNumber + "' AND \"Order\".OrderNumber = \"Transaction\".OrderNumber AND \"Order\".SecurityType = 'stock'" +
                " AND (\"Order\".CurrentStatus = 'completed' or \"Order\".CurrentStatus = 'cancelled')" +
                ") AS temp ORDER BY ExecuteDate DESC, TransactionNumber DESC";
            DataTable dtLastStock = myHKeInvestData.getData(sql);

            if (dtLastStock.Rows.Count != 0)  // if it == 0, not executed bond order.
            {
                decimal shares = Convert.ToDecimal(dtLastStock.Rows[0]["ExecuteShares"]);
                decimal price = Convert.ToDecimal(dtLastStock.Rows[0]["ExecutePrice"]);

                DataTable dtStock = myExternalFunctions.getSecuritiesByCode("stock", dtLastStock.Rows[0]["SecurityCode"].ToString());
                decimal value = Math.Round(shares * price, 2);
                last_stock_value = value.ToString();
                last_stock_date = dtLastStock.Rows[0]["Date"].ToString().Trim();
            }


            // Create new DataTable.
            DataTable dtSummary = new DataTable();

            // Declare DataColumn and DataRow variables.
            DataColumn column;
            DataRow row;

            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "type";
            dtSummary.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "value";
            dtSummary.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "convertedvalue";
            dtSummary.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "date";
            dtSummary.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "value_of_last";
            dtSummary.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "convertedvalue_of_last";
            dtSummary.Columns.Add(column);

            // Create new DataRow objects and add to DataTable. 
            row = dtSummary.NewRow();
            row["type"] = "Bond";
            row["value"] = bond_monetary;
            row["date"] = last_bond_date;
            row["value_of_last"] = last_bond_value;
            dtSummary.Rows.Add(row);

            row = dtSummary.NewRow();
            row["type"] = "Stock";
            row["value"] = stock_monetary;
            row["date"] = last_stock_date;
            row["value_of_last"] = last_stock_value;
            dtSummary.Rows.Add(row);
            row = dtSummary.NewRow();

            row["type"] = "Unit Trust";
            row["value"] = ut_monetary;
            row["date"] = last_ut_date;
            row["value_of_last"] = last_ut_value;
            dtSummary.Rows.Add(row);

            // Bind the DataTable to the GridView.
            gvSummary.DataSource = dtSummary;
            gvSummary.DataBind();
            gvSummary.Visible = true;
            links.Visible = true;
            gvSummary.Columns[myHKeInvestCode.getColumnIndexByName(gvSummary, "convertedvalue")].Visible = false;
            gvSummary.Columns[myHKeInvestCode.getColumnIndexByName(gvSummary, "convertedvalue_of_last")].Visible = false;
            ddlCurrency.Visible = true;
            ViewState["dtSummary"] = dtSummary;
            return;
        }
        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the index value of the convertedValue column in the GridView using the helper method "getColumnIndexByName".
            int convertedValueIndex = myHKeInvestCode.getColumnIndexByName(gvSummary, "convertedvalue");
            int convertedValueOfLastIndex = myHKeInvestCode.getColumnIndexByName(gvSummary, "convertedvalue_of_last");

            // Get the currency to convert to from the ddlCurrency dropdownlist.
            // Hide the converted currency column if no currency is selected.
            string toCurrency = ddlCurrency.SelectedValue;
            if (toCurrency == "0")
            {
                gvSummary.Columns[convertedValueIndex].Visible = false;
                gvSummary.Columns[convertedValueOfLastIndex].Visible = false;
                lblTotalConvertedValue.Visible = false;
                lblConvertedFreeBalance.Visible = false;
                return;
            }

            // Make the convertedValue column visible and create a DataTable from the GridView.
            // Since a GridView cannot be updated directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
            gvSummary.Columns[convertedValueIndex].Visible = true;
            gvSummary.Columns[convertedValueOfLastIndex].Visible = true;
            DataTable dtSummary = (DataTable)ViewState["dtSummary"];

            // ***********************************************************************************************************
            // TODO: For each row in the DataTable, get the base currency of the security, convert the current value to  *
            //       the selected currency and assign the converted value to the convertedValue column in the DataTable. *
            // ***********************************************************************************************************

            //string sql = "select rate from CurrencyRate where currency = '" + toCurrency + "'";
            //string toCurrencyRate = myHKeInvestData.getAggregateValue(sql).ToString().Trim();

            int dtRows = 0;
            foreach (DataRow row in dtSummary.Rows)
            {
                if (row["value"].ToString().Equals("N/A"))
                {
                    dtSummary.Rows[dtRows]["convertedvalue"] = "N/A";
                }
                else {
                    decimal value = Convert.ToDecimal(row["value"]);
                    decimal convertedValue = myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                    dtSummary.Rows[dtRows]["convertedvalue"] = Math.Round(convertedValue,2);
                }

                if (row["value_of_last"].ToString().Equals("N/A"))
                {
                    dtSummary.Rows[dtRows]["convertedvalue_of_last"] = "N/A";
                }
                else {
                    decimal valueoflast = Convert.ToDecimal(row["value_of_last"]);
                    decimal convertedValueOfLast = myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), valueoflast);
                    dtSummary.Rows[dtRows]["convertedvalue_of_last"] = Math.Round(convertedValueOfLast,2);
                }
                dtRows = dtRows + 1;
            }

            // Change the header text of the convertedValue column to indicate the currency. 
            gvSummary.Columns[convertedValueIndex].HeaderText = "Total Monetary Value in " + toCurrency;
            gvSummary.Columns[convertedValueOfLastIndex].HeaderText = "Total Monetary Value of Last Order in " + toCurrency;

            // Bind the DataTable to the GridView.
            gvSummary.DataSource = dtSummary;
            gvSummary.DataBind();

            lblTotalConvertedValue.Visible = true;
            lblConvertedFreeBalance.Visible = true;

            decimal tmv = Convert.ToDecimal(ViewState["Total_Monetary_Value"].ToString().Trim());
            decimal tfb = Convert.ToDecimal(ViewState["Total_Free_Balance"].ToString().Trim());
            decimal tcmv = myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), tmv);
            decimal tcfb = myHKeInvestCode.convertCurrency("HKD", ViewState["HKD"].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), tfb);

            lblTotalConvertedValue.Text = "The total monetary value of securities held in " + toCurrency + " is: " + Math.Round(tcmv, 2).ToString();
            lblConvertedFreeBalance.Text = "The account free balance in " + toCurrency + " is: " + Math.Round(tcfb, 2).ToString();
        }

        protected void Stock_Click(object sender, EventArgs e)
        {
            //link to SecurityHoldingDetails and set type = stock automatically.
            Session["type"] = "stock";
            Response.Redirect("ClientSecurityHoldingsDetails.aspx");
        }

        protected void Bond_Click(object sender, EventArgs e)
        {
            //link to SecurityHoldingDetails and set type = bond automatically.
            Session["type"] = "bond";
            Response.Redirect("ClientSecurityHoldingsDetails.aspx");
        }

        protected void UT_Click(object sender, EventArgs e)
        {
            //link to SecurityHoldingDetails and set type = unit trust automatically.
            Session["type"] = "unit trust";
            Response.Redirect("ClientSecurityHoldingsDetails.aspx");
        }
    }
}