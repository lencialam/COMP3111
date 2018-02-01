using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class ClientShowActiveOrders : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {   
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
            lblAccountNumber.Visible = false;
            gvActiveOrders.Visible = false;
            lblResultMessage.Visible = false;
            string sql = "";

            string accountNumber = ClientAccountNum.Text; // Set the account number from a web form control


            sql = "select accountNumber from Account where accountNumber = '" + accountNumber + "'";
            DataTable dtAccount = myHKeInvestData.getData(sql);
            if (dtAccount == null) { return; } // If the DataSet is null, a SQL error occurred.


            sql = "Select OrderNumber, 'b/s' as buysell, SecurityType, SecurityCode, 'name' as name, DateOfSubmission, CurrentStatus" +
                ",'' as QuantityOfShares_Stock, '' as ExpiryDate, '' as StockOrderType, '' as HBLS_Price, '' as StopPrice, '' as DollarAmount, '' as QuantityOfShares_BUT" +
                " from \"Order\" Where AccountNumber = '" + accountNumber +
                "' and(CurrentStatus = 'pending' or CurrentStatus = 'partial') order by convert(datetime, DateOfSubmission, 103) ASC";
            DataTable dtActiveOrders = myHKeInvestData.getData(sql);
            if (dtActiveOrders == null) { return; } // If the DataSet is null, a SQL error occurred.
            // If no result is returned, then display a message that no active orders are associatied with the account
            if (dtActiveOrders.Rows.Count == 0)
            {
                lblResultMessage.Text = "There is no active orders associated with this account.";
                lblResultMessage.Visible = true;
                gvActiveOrders.Visible = false;
                return;
            }

            int dtRow = 0;
            foreach (DataRow row in dtActiveOrders.Rows)
            {
                string OrderNumber = row["OrderNumber"].ToString().Trim();
                string securityType = row["SecurityType"].ToString().Trim();
                string securityCode = row["SecurityCode"].ToString().Trim();
                DataTable dtname = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);
                string name = dtname.Rows[0]["name"].ToString();
                dtActiveOrders.Rows[dtRow]["name"] = name;
                if (securityType == "stock")
                {
                    sql = "select SharesToBuy, ExpiryDate, BuyingType, HighestBuyingPrice, StopPrice from StockBuying where OrderNumber = '" + OrderNumber + "'";
                    DataTable dtStockBuy = myHKeInvestData.getData(sql);
                    if (dtStockBuy.Rows.Count != 0) //i.e. this order is indeed a stock buy order.
                    {
                        dtActiveOrders.Rows[dtRow]["buysell"] = "Buy";
                        dtActiveOrders.Rows[dtRow]["QuantityOfShares_Stock"] = dtStockBuy.Rows[0]["SharesToBuy"].ToString().Trim();
                        dtActiveOrders.Rows[dtRow]["StockOrderType"] = dtStockBuy.Rows[0]["BuyingType"].ToString().Trim();
                        dtActiveOrders.Rows[dtRow]["HBLS_Price"] = dtStockBuy.Rows[0]["HighestBuyingPrice"].ToString().Trim();
                        dtActiveOrders.Rows[dtRow]["StopPrice"] = dtStockBuy.Rows[0]["StopPrice"].ToString().Trim();
                        
                        int days = Int32.Parse(dtStockBuy.Rows[0]["ExpiryDate"].ToString());
                        dtActiveOrders.Rows[dtRow]["ExpiryDate"] = (Convert.ToDateTime(dtActiveOrders.Rows[dtRow]["DateOfSubmission"]).AddDays(days)).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        sql = "select SharesToSell, ExpiryDate, SellingType, LowestSellingPrice, StopPrice from StockSelling where OrderNumber = '" + OrderNumber + "'";
                        DataTable dtStockSell = myHKeInvestData.getData(sql);
                        if (dtStockSell.Rows.Count != 0) //i.e. this order is indeed a stock sell order.
                        {
                            dtActiveOrders.Rows[dtRow]["buysell"] = "Sell";
                            dtActiveOrders.Rows[dtRow]["QuantityOfShares_Stock"] = dtStockSell.Rows[0]["SharesToSell"].ToString().Trim();
                            dtActiveOrders.Rows[dtRow]["StockOrderType"] = dtStockSell.Rows[0]["SellingType"].ToString().Trim();
                            dtActiveOrders.Rows[dtRow]["HBLS_Price"] = dtStockSell.Rows[0]["LowestSellingPrice"].ToString().Trim();
                            dtActiveOrders.Rows[dtRow]["StopPrice"] = dtStockSell.Rows[0]["StopPrice"].ToString().Trim();

                            int days = Int32.Parse(dtStockSell.Rows[0]["ExpiryDate"].ToString());
                            dtActiveOrders.Rows[dtRow]["ExpiryDate"] = (Convert.ToDateTime(dtActiveOrders.Rows[dtRow]["DateOfSubmission"]).AddDays(days)).ToString("dd/MM/yyyy");
                        }
                    }
                }
                else if (securityType == "bond" || securityType == "unit trust")
                {
                    sql = "select DollarAmount from BUTBuying where OrderNumber = '" + OrderNumber + "'";
                    DataTable dtBUTBuy = myHKeInvestData.getData(sql);
                    if (dtBUTBuy.Rows.Count != 0) //i.e. this order is indeed a BUT buy order.
                    {
                        dtActiveOrders.Rows[dtRow]["buysell"] = "Buy";
                        dtActiveOrders.Rows[dtRow]["DollarAmount"] = dtBUTBuy.Rows[0]["DollarAmount"].ToString().Trim();
                    }
                    else
                    {
                        sql = "select SharesToSell from BUTSelling where OrderNumber = '" + OrderNumber + "'";
                        DataTable dtBUTSell = myHKeInvestData.getData(sql);
                        if (dtBUTSell.Rows.Count != 0) //i.e. this order is indeed a BUT sell order.
                        {
                            dtActiveOrders.Rows[dtRow]["buysell"] = "Sell";
                            dtActiveOrders.Rows[dtRow]["QuantityOfShares_BUT"] = dtBUTSell.Rows[0]["SharesToSell"].ToString().Trim();
                        }
                    }
                }

                dtRow = dtRow + 1;
            }
            // Bind the DataTable to the GridView.
            gvActiveOrders.DataSource = dtActiveOrders;
            gvActiveOrders.DataBind();
            gvActiveOrders.Visible = true;
        }
    }
}