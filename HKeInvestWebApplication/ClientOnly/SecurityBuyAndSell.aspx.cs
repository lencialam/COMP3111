using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class SecurityBuyAndSell : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                lblTotal.Visible = false;
                Total.Visible = false;
                lblStockOrderType.Visible = false;
                StockOrderType.Visible = false;
                lblStopPrice.Visible = false;
                StopPrice.Visible = false;
                lblLimitPrice.Visible = false;
                LimitPrice.Visible = false;
                lblExpiryDate.Visible = false;
                cvStock.Visible = false;
                AllOrNone.Visible = false;
                lblAllOrNone.Visible = false;
                ExpiryDate.Visible = false;


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
                lblAccountNumber.Text = ViewState[username].ToString().Trim();
                lblClientName.Visible = true;
            }

        }

        protected void OrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Reset invisible controls 
            lblTotal.Visible = false;
            Total.Visible = false;
            lblStockOrderType.Visible = false;
            StockOrderType.Visible = false;
            lblStopPrice.Visible = false;
            StopPrice.Visible = false;
            lblLimitPrice.Visible = false;
            LimitPrice.Visible = false;
            lblExpiryDate.Visible = false;
            ExpiryDate.Visible = false;
            cvStock.Visible = false;
            AllOrNone.Visible = false;
            lblAllOrNone.Visible = false;

            // Reset visible controls
            SecurityType.SelectedIndex = 0;
            SecurityCode.Text = "";

            //reset validators 
            rfvAllOrNone.Enabled = false;
            cvStock.Enabled = false;
            cvExpiryDate.Enabled = false;
            rvTotal.Enabled = false;
            rfvTotal.Enabled = false;
            rvLimitPrice.Enabled = false;
            rvStopPrice.Enabled = false;




            return;

        }

        protected void SecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Reset invisible controls 
            lblTotal.Visible = false;
            Total.Visible = false;
            lblStockOrderType.Visible = false;
            StockOrderType.Visible = false;
            lblStopPrice.Visible = false;
            StopPrice.Visible = false;
            lblLimitPrice.Visible = false;
            LimitPrice.Visible = false;
            lblExpiryDate.Visible = false;
            ExpiryDate.Visible = false;
            cvStock.Visible = false;
            AllOrNone.Visible = false;
            lblAllOrNone.Visible = false;

            // Reset Validator
            rfvAllOrNone.Enabled = false;
            cvStock.Enabled = false;
            cvExpiryDate.Enabled = false;
            rvTotal.Enabled = true;
            rfvTotal.Enabled = true;
            rvLimitPrice.Enabled = false;
            rvStopPrice.Enabled = false;

            // Reset visible control
            SecurityCode.Text = "";

            //check validity of OrderType
            if (OrderType.SelectedIndex == 0)
            {
                return;
            }


            //UI visibility changes
            switch (SecurityType.SelectedIndex)
            {
                case 0:
                    return;
                case 1:
                    switch (OrderType.SelectedIndex)
                    {
                        case 0:
                            return;
                        case 1:
                            lblTotal.Text = "Total amount to buy (in HKD)";
                            rvTotal.ValidationExpression = "\\d+(?:\\.\\d{1,2})?";
                            rvTotal.ErrorMessage = "Amount should be a non-zero integer or decimal value up to two digits.";
                            break;
                        case 2:
                            lblTotal.Text = "Total amount to sell (in shares)";
                            rvTotal.ValidationExpression = "\\d+(?:\\.\\d{1,2})?";
                            rvTotal.ErrorMessage = "Share should be a non-zero integer or decimal value up to two digits.";
                            break;
                    }
                    break;
                case 2:
                    switch (OrderType.SelectedIndex)
                    {
                        case 0:
                            return;
                        case 1:
                            lblTotal.Text = "Total amount to buy (in HKD)";
                            rvTotal.ValidationExpression = "\\d+(?:\\.\\d{1,2})?";
                            rvTotal.ErrorMessage = "Amount should be a non-zero integer or decimal value up to two digits.";
                            break;
                        case 2:
                            lblTotal.Text = "Total amount to sell (in shares)";
                            rvTotal.ValidationExpression = "^[1-9]+[0-9]*$";
                            rvTotal.ErrorMessage = "Amount should be an integer.";
                            break;
                    }
                    break;
                case 3:
                    lblStockOrderType.Visible = true;
                    StockOrderType.Visible = true;
                    lblStopPrice.Visible = true;
                    StopPrice.Visible = true;
                    lblLimitPrice.Visible = true;
                    LimitPrice.Visible = true;
                    lblExpiryDate.Visible = true;
                    ExpiryDate.Visible = true;
                    cvStock.Visible = true;
                    AllOrNone.Visible = true;
                    lblAllOrNone.Visible = true;

                    rfvAllOrNone.Enabled = true;
                    cvStock.Enabled = true;
                    cvExpiryDate.Enabled = true;

                    switch (OrderType.SelectedIndex)
                    {
                        case 0:
                            return;
                        case 1:
                            lblTotal.Text = "Total amount to buy (in shares, multiple of 100)";
                            rvTotal.ValidationExpression = "^[1-9]+[0-9]*00$";
                            rvTotal.ErrorMessage = "Amount should be an integer and multiple of 100.";
                            break;
                        case 2:
                            lblTotal.Text = "Total amount to sell (in shares)";
                            rvTotal.ValidationExpression = "^[1-9]+[0-9]*$";
                            rvTotal.ErrorMessage = "Amount should be an integer.";
                            break;
                    }

                    break;
            }

            lblTotal.Visible = true;
            Total.Visible = true;

            return;
        }

        protected void cvOrderType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(OrderType.SelectedIndex == 0)
            {
                args.IsValid = false;
            }
        }

        protected void cvSecurityType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (SecurityType.SelectedIndex == 0)
            {
                args.IsValid = false;
            }
        }


        protected void Submit_OnClick(object sender, EventArgs e)
        {
            Submit.Enabled = false;
            Submit_Click();
            Submit.Enabled = true;
                    
        }

        protected void Submit_Click()
        {
            if (!Page.IsValid)
            {
                return;
            }
                
            //initialize
            string sql = "";
            decimal SellLimit = 0;
            string ReferenceNumber = "";
            Submit.Enabled = false;

            //Check if security exists
            DataTable dtSec = myExternalFunctions.getSecuritiesByCode(SecurityType.SelectedValue, SecurityCode.Text);
            if(dtSec == null)
            {
                MessageBox.Show(new Form { TopMost = true }, "Order was rejected. Invalid security code.");
                Submit.Enabled = true;

                return;
            }
            //if "buy" is selected, check condition:
            //Account balance >= buying amount in current value(MARKET PRICE)
            if(OrderType.SelectedIndex == 1)
            {
                decimal pendingvalue = myHKeInvestCode.GetPendingValue(lblAccountNumber.Text.Trim());
                sql = "select balance from account where AccountNumber = '" + lblAccountNumber.Text + "'";
                DataTable dtbalance = myHKeInvestData.getData(sql);
                if (dtbalance != null)
                {
                    decimal balance = Convert.ToDecimal(dtbalance.Rows[0][0]);// Get current balance of the account 
                    if((SecurityType.SelectedIndex == 1||SecurityType.SelectedIndex == 2)&& (Convert.ToDecimal(Total.Text)> (balance - pendingvalue)))//Input amount as HKD
                    {
                        MessageBox.Show(new Form { TopMost = true }, "Order was rejected. Insufficient balance.");
                        Submit.Enabled = true;
                        return;
                    }else if(SecurityType.SelectedIndex == 3)
                    {
                        decimal price = myExternalFunctions.getSecuritiesPrice(SecurityType.SelectedValue, SecurityCode.Text);
                        price = Math.Round(price * Convert.ToDecimal(Total.Text));
                        if(price > (balance - pendingvalue))
                        {
                            MessageBox.Show(new Form { TopMost = true }, "Order was rejected. Insufficient balance.");
                            Submit.Enabled = true;
                            return;
                        }
                        
                    }
                    
                }
            }

            //if "sell" is selected, check condition:
            //Sum of shares in active orders <= numbers of shares of the security held by the account. 
            if (OrderType.SelectedIndex == 2)
            {


                sql = "select sum(Shares) from SecurityHolding where AccountNumber = '" + lblAccountNumber.Text + "' and Type = '" + SecurityType.SelectedValue + "' and Code = '" + SecurityCode.Text + "' ";
                SellLimit = myHKeInvestData.getAggregateValue(sql);

                if (SecurityType.SelectedIndex == 3)
                {
                    sql = "select sum(SharesToSell) from \"Order\" ,StockSelling where \"Order\".Ordernumber = StockSelling.Ordernumber AND AccountNumber = '" + lblAccountNumber.Text + "' and SecurityType = '" + SecurityType.SelectedValue + "' and SecurityCode = '" + SecurityCode.Text + "' and (CurrentStatus = 'pending' or CurrentStatus = 'partial')";
                    SellLimit -= myHKeInvestData.getAggregateValue(sql);
                }
                else
                {
                    sql = "select sum(SharesToSell) from \"Order\" ,BUTSelling where \"Order\".Ordernumber = BUTSelling.Ordernumber AND AccountNumber = '" + lblAccountNumber.Text + "' and SecurityType = '" + SecurityType.SelectedValue + "' and SecurityCode = '" + SecurityCode.Text + "' and (CurrentStatus = 'pending' or CurrentStatus = 'partial')";
                    SellLimit -= myHKeInvestData.getAggregateValue(sql);
                }

                if (Convert.ToDecimal(Total.Text)> SellLimit)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Order was rejected. Sum of shares in active orders must not exceed numbers of shares of the security held by the account.");
                    Submit.Enabled = true;
                    return;
                }
                

            }

            switch (OrderType.SelectedIndex)
            {
                case 1:
                    switch (SecurityType.SelectedIndex)
                    {
                        case 1:
                            ReferenceNumber = myExternalFunctions.submitBondBuyOrder(SecurityCode.Text.Trim(),Total.Text.Trim());
                            break;
                        
                        case 2:
                            ReferenceNumber = myExternalFunctions.submitUnitTrustBuyOrder(SecurityCode.Text.Trim(), Total.Text.Trim());
                            break;

                        case 3:
                            ReferenceNumber = myExternalFunctions.submitStockBuyOrder(SecurityCode.Text.Trim(), Total.Text.Trim(),StockOrderType.SelectedValue, ExpiryDate.SelectedValue, AllOrNone.SelectedValue, LimitPrice.Text.Trim(), StopPrice.Text.Trim());
                            break;
                    }
                    break;



                case 2:
                    switch (SecurityType.SelectedIndex)
                    {
                        case 1:
                            ReferenceNumber = myExternalFunctions.submitBondSellOrder(SecurityCode.Text.Trim(), Total.Text.Trim());
                            break;

                        case 2:
                            ReferenceNumber = myExternalFunctions.submitUnitTrustSellOrder(SecurityCode.Text.Trim(), Total.Text.Trim());
                            break;

                        case 3:
                            ReferenceNumber = myExternalFunctions.submitStockSellOrder(SecurityCode.Text.Trim(), Total.Text.Trim(), StockOrderType.SelectedValue, ExpiryDate.SelectedValue, AllOrNone.SelectedValue, LimitPrice.Text.Trim(), StopPrice.Text.Trim());
                            break;


                    }
                    break;
            }
            if(ReferenceNumber == null)
            {
                MessageBox.Show(new Form { TopMost = true }, "Your order was not able to sumbit, please check your input.");
                Submit.Enabled = true;

                return;
            }
            else
            {

                //Record order into database.
                SqlTransaction trans = myHKeInvestData.beginTransaction();
                string limitprice = "";
                string stopprice = "";
                if (StockOrderType.SelectedIndex == 1 || StockOrderType.SelectedIndex == 3)
                {
                    limitprice = "null";
                }
                else
                {
                    limitprice = LimitPrice.Text.Trim();
                }

                if (StockOrderType.SelectedIndex == 1 || StockOrderType.SelectedIndex == 2)
                {
                    stopprice = "null";
                }
                else
                {
                    stopprice = StopPrice.Text.Trim();
                }


                sql = "INSERT INTO \"Order\"(AccountNumber,OrderNumber,SecurityType,DateOfSubmission,CurrentStatus,SecurityCode,OrderType) VALUES('" + lblAccountNumber.Text.Trim() + "','" + ReferenceNumber
                    + "','" + SecurityType.Text.Trim() + "', convert(datetime,'" + DateTime.Now.ToString("dd/MM/yyyy") + "',103), '" + myExternalFunctions.getOrderStatus(ReferenceNumber) + "','"
                    + SecurityCode.Text.Trim() + "','"+ OrderType.SelectedValue.Trim() + "')";
                myHKeInvestData.setData(sql, trans);
                if (OrderType.SelectedIndex == 1 && SecurityType.SelectedIndex == 1 || SecurityType.SelectedIndex == 2)
                {
                    sql = "INSERT INTO BUTBuying(OrderNumber,DollarAmount) VALUES('" + ReferenceNumber
                    + "','" + Total.Text.Trim() + "')";
                }
                else if (OrderType.SelectedIndex == 2 && SecurityType.SelectedIndex == 1 || SecurityType.SelectedIndex == 2)
                {
                    sql = "INSERT INTO BUTSelling(OrderNumber,SharesToSell) VALUES('" + ReferenceNumber
                    + "','" + Total.Text.Trim() + "')";
                }
                else if (OrderType.SelectedIndex == 1 && SecurityType.SelectedIndex == 3)
                {
                    sql = "INSERT INTO StockBuying(OrderNumber,SharesToBuy,ExpiryDate,AllOrNone,BuyingType,HighestBuyingPrice,StopPrice) VALUES('" + ReferenceNumber
                    + "','" + Total.Text.Trim() + "','" + ExpiryDate.SelectedValue.Trim() + "','" + AllOrNone.SelectedValue.Trim() + "','" + StockOrderType.Text.Trim() + "'," + limitprice + "," + stopprice + ")";
                }
                else if (OrderType.SelectedIndex == 2 && SecurityType.SelectedIndex == 3)
                {
                    sql = "INSERT INTO StockSelling(OrderNumber,SharesToSell,ExpiryDate,AllOrNone,SellingType,LowestSellingPrice,StopPrice)VALUES('" + ReferenceNumber
                    + "','" + Total.Text.Trim() + "','" + ExpiryDate.SelectedValue.Trim() + "','" + AllOrNone.SelectedValue.Trim() + "','" + StockOrderType.Text.Trim() + "'," + limitprice + "," + stopprice + ")";
                }



                myHKeInvestData.setData(sql, trans);
                myHKeInvestData.commitTransaction(trans);


                if (ReferenceNumber != null)
                {
                    MessageBox.Show(new Form { TopMost = true }, "Your order: " + ReferenceNumber + " has been successfully submitted. Please record your reference number. You will recieve an invoice while your order finished.");
                }
                else
                {
                    MessageBox.Show(new Form { TopMost = true }, "Your order was not able to sumbit, please check your input.");
                }
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void cvStock_ServerValidate(object source, ServerValidateEventArgs args)
        {   
            if (SecurityType.SelectedIndex == 3)
            {
                if (StockOrderType.SelectedIndex == 0)
                {
                    args.IsValid = false;
                    cvStock.ErrorMessage = "Stock order type is required.";
                }
                else if (StockOrderType.SelectedIndex == 2 && LimitPrice.Text == "")
                {
                    args.IsValid = false;
                    cvStock.ErrorMessage = "Limit price is required if limit order is selected.";
                }
                else if (StockOrderType.SelectedIndex == 3 && StopPrice.Text == "")
                {
                    args.IsValid = false;
                    cvStock.ErrorMessage = "Limit price is required if stop order is selected.";
                }
                else if (StockOrderType.SelectedIndex == 4)
                {
                    if ((StopPrice.Text == "" || LimitPrice.Text == ""))
                    {
                        args.IsValid = false;
                        cvStock.ErrorMessage = "Both stop, limit price are required if stop limit order is selected.";
                    }
                    else if(OrderType.SelectedIndex == 1 && Convert.ToDecimal(StopPrice.Text) > Convert.ToDecimal(LimitPrice.Text))
                    {
                        args.IsValid = false;
                        cvStock.ErrorMessage = "Stop price should be lower than or equal to limit price in buy orders.";
                    }else if(OrderType.SelectedIndex == 2 && Convert.ToDecimal(StopPrice.Text) < Convert.ToDecimal(LimitPrice.Text))
                    {
                        args.IsValid = false;
                        cvStock.ErrorMessage = "Stop price should be higher than or equal to limit price in buy orders.";
                    }
                }


            }
            return;
        }

        protected void cvExpiryDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (SecurityType.SelectedIndex == 3)
            {
               if(ExpiryDate.SelectedIndex == 0)
                {
                    args.IsValid = false;
                    cvExpiryDate.ErrorMessage = "Expiry date is required.";
                }

            }
            return;
        }

        protected void StockOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //initialize validators
            rvLimitPrice.Enabled = false;
            rvStopPrice.Enabled = false;

            switch (StockOrderType.SelectedIndex)
            {
                case 1:
                    return;
                case 2:
                    rvStopPrice.Enabled = true;
                    break;
                case 3:
                    rvLimitPrice.Enabled = true;
                    break;
                case 4:
                    rvLimitPrice.Enabled = true;
                    rvStopPrice.Enabled = true;
                    break;
            }
        }
    }
}