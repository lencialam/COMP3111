using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using System.Data.SqlClient;

namespace HKeInvestWebApplication.Code_File
{
    //**********************************************************
    //*  THE CODE IN THIS CLASS CAN BE MODIFIED AND ADDED TO.  *
    //**********************************************************
    public class HKeInvestCode
    {
        public string getDataType(string value)
        {
            // Returns the data type of value. Tests for more types can be added if needed.
            if (value != null)
            {
                int n; decimal d; DateTime dt;
                if (decimal.TryParse(value, out d)) { return "System.Decimal"; }
                else if (int.TryParse(value, out n)) { return "System.Int32"; }
                else if (DateTime.TryParse(value, out dt)) { return "System.DataTime"; }
            }
            return "System.String";
        }

        public string getSortDirection(System.Web.UI.StateBag viewState, string sortExpression)
        {
            // If the GridView is sorted for the first time or sorting is being done on a new column, 
            // then set the sort direction to "ASC" in ViewState.
            if (viewState["SortDirection"] == null || viewState["SortExpression"].ToString() != sortExpression)
            {
                viewState["SortDirection"] = "ASC";
            }
            // Othewise if the same column is clicked for sorting more than once, then toggle its SortDirection.
            else if (viewState["SortDirection"].ToString() == "ASC")
            {
                viewState["SortDirection"] = "DESC";
            }
            else if (viewState["SortDirection"].ToString() == "DESC")
            {
                viewState["SortDirection"] = "ASC";
            }
            return viewState["SortDirection"].ToString();
        }

        public DataTable unloadGridView(GridView gv)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                dt.Columns.Add(((BoundField)gv.Columns[i]).DataField);
            }

            // For correct sorting, set the data type of each DataTable column based on the values in the GridView.
            gv.SelectedIndex = 0;
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                
                if (gv.Columns[i].HeaderText.ToString() == "Code")
                {
                    dt.Columns[i].DataType = Type.GetType("System.String");
                }
                else if (gv.Columns[i].HeaderText.ToString() == "Reference No.")
                {
                    dt.Columns[i].DataType = Type.GetType("System.String");
                }
                else if (gv.Columns[i].HeaderText.ToString() == "Date of Submission")
                {
                    dt.Columns[i].DataType = Type.GetType("System.DateTime");
                }
                else if (gv.Columns[i].HeaderText.ToString() == "Total Executed Dollar Amount (in HKD)")
                {
                    dt.Columns[i].DataType = Type.GetType("System.Decimal");
                }
                else
                {
                    dt.Columns[i].DataType = Type.GetType(getDataType(gv.SelectedRow.Cells[i].Text.ToString()));
                }

            }

            // Load the GridView data into the DataTable.
            foreach (GridViewRow row in gv.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < gv.Columns.Count; j++)
                {
                    if (gv.Columns[j].HeaderText.ToString() == "Date of Submission")
                    {
                        dr[((BoundField)gv.Columns[j]).DataField.ToString().Trim()] = DateTime.ParseExact(row.Cells[j].Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else {
                        dr[((BoundField)gv.Columns[j]).DataField.ToString().Trim()] = row.Cells[j].Text;
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public int getColumnIndexByName(GridView gv, string columnName)
        {
            // Helper method to get GridView column index by a column's DataField name.
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (((BoundField)gv.Columns[i]).DataField.ToString().Trim() == columnName.Trim())
                { return i; }
            }
            MessageBox.Show("Column '" + columnName + "' was not found \n in the GridView '" + gv.ID.ToString() + "'.");
            return -1;
        }

        public decimal convertCurrency(string fromCurrency, string fromCurrencyRate, string toCurrency, string toCurrencyRate, decimal value)
        {
            if (fromCurrency == toCurrency)
            {
                return value;
            }
            else
            {
                return (Convert.ToDecimal(fromCurrencyRate) / Convert.ToDecimal(toCurrencyRate) * value);
            }
        }

        public void getCurrencyToSession(System.Web.SessionState.HttpSessionState session)
        {
            ExternalFunctions myExternalFunctions = new ExternalFunctions();
            DataTable dtCurrency = myExternalFunctions.getCurrencyData();
            session["kengdie"] = dtCurrency;
        }

        public void sendEmail(string emailTo, string subject, string body)
        {
            string emailFrom = "comp311_team115@cse.ust.hk";
            string password = "team115#";

            //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
            //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.cse.ust.hk");

            mail.From = new MailAddress(emailFrom);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            // Can set to false, if you are sending pure text.
            mail.IsBodyHtml = true;
            mail.Body = body;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(emailFrom, password);
            SmtpServer.EnableSsl = true;

            try
            {
                SmtpServer.Send(mail);
                MessageBox.Show("Mail Sent");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void updateAlert()
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();
            string sql = "SELECT count(*) FROM Alert";
            decimal d = myHKeInvestData.getAggregateValue(sql);
            if (d != 0 && d != -1)
            {
                sql = "SELECT * FROM Alert";
                DataTable dtSecuritiesAlert = myHKeInvestData.getData(sql);
                foreach (DataRow row in dtSecuritiesAlert.Rows)
                {
                    decimal lastPrice = Convert.ToDecimal(row["lastPrice"]);
                    string type = row["type"].ToString().Trim();
                    string code = row["code"].ToString().Trim();
                    decimal currentPrice = myExternalFunctions.getSecuritiesPrice(type, code);
                    decimal trend = currentPrice - lastPrice;
                    SqlTransaction trans = myHKeInvestData.beginTransaction();
                    sql = "UPDATE Alert SET lastPrice = '" + currentPrice + "', trend = '" + trend + "' WHERE type = '" + type + "' AND code = '" + code + "'";
                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);
                }
            }
        }

        public void checkAlert()
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();

            string sql = "SELECT count(*) FROM Alert";
            decimal d = myHKeInvestData.getAggregateValue(sql);
            if (d != 0 && d != -1)
            {
                sql = "SELECT * FROM Alert";
                DataTable dtSecuritiesAlert = myHKeInvestData.getData(sql);
                foreach (DataRow row in dtSecuritiesAlert.Rows)
                {
                    decimal currentPrice = Convert.ToDecimal(row["lastPrice"]);
                    decimal trend = Convert.ToDecimal(row["trend"]);
                    string type = row["type"].ToString().Trim();
                    string code = row["code"].ToString().Trim();
                    if (trend > 0)
                    {
                        sql = "SELECT accountNumber, name, base, highValue FROM SecurityHolding WHERE type = '" + type + "' AND code = '" + code + "' AND highValue IS NOT NULL";
                        DataTable dtHighTable = myHKeInvestData.getData(sql);
                        foreach (DataRow highrow in dtHighTable.Rows)
                        {
                            decimal highValue = Convert.ToDecimal(highrow["highValue"]);
                            string name = highrow["name"].ToString().Trim();
                            string securityBase = highrow["base"].ToString().Trim();
                            string accountNumber = highrow["accountNumber"].ToString().Trim();
                            if (highValue <= currentPrice && highValue>(currentPrice-trend))
                            {
                                sql = "SELECT email, clientTitle, lastName FROM Client WHERE accountNumber = '" + accountNumber + "' AND IsPrimaryHolder = 'true'";
                                DataTable dtClientTable = myHKeInvestData.getData(sql);

                                string emailTo = dtClientTable.Rows[0]["email"].ToString().Trim();
                                string clientTitle = "Mr.";
                                string clientTitlestr = dtClientTable.Rows[0]["clientTitle"].ToString().Trim();
                                if (clientTitlestr == "mrs")
                                {
                                    clientTitle = "Mrs.";
                                }
                                else if (clientTitlestr == "ms")
                                {
                                    clientTitle = "Ms.";
                                }
                                else if (clientTitlestr == "dr")
                                {
                                    clientTitle = "Dr.";
                                }
                                string lastName = dtClientTable.Rows[0]["lastName"].ToString().Trim();
                                string subject = "[Important!] Alert For Security Holdings";
                                string body = "Dear " + clientTitle + " " + lastName + ",<br><br>Your security holding " + type + " " + code + " " + name + " has passed through the high value you set.<br>This alert is triggered at " + securityBase + currentPrice + "<br><br>Regards,<br>COMP3111 Team 115";
                                sendEmail(emailTo, subject, body);

                                //delete this alert since it's triggered
                                SqlTransaction trans = myHKeInvestData.beginTransaction();
                                sql = "UPDATE SecurityHolding SET highValue = NULL WHERE accountNumber = '" + accountNumber + "' AND type = '" + type + "' AND code = '" + code + "'";
                                myHKeInvestData.setData(sql, trans);
                                myHKeInvestData.commitTransaction(trans);
                            }
                        }
                    }
                    else if (trend < 0)
                    {
                        sql = "SELECT accountNumber, name, lowValue, base FROM SecurityHolding WHERE type = '" + type + "' AND code = '" + code + "' AND lowValue IS NOT NULL";
                        DataTable dtLowTable = myHKeInvestData.getData(sql);
                        foreach (DataRow lowrow in dtLowTable.Rows)
                        {
                            decimal lowValue = Convert.ToDecimal(lowrow["lowValue"]);
                            string name = lowrow["name"].ToString().Trim();
                            string securityBase = lowrow["base"].ToString().Trim();
                            string accountNumber = lowrow["accountNumber"].ToString().Trim();
                            if (lowValue >= currentPrice && lowValue < (currentPrice - trend))
                            {
                                sql = "SELECT email, clientTitle, lastName FROM Client WHERE accountNumber = '" + accountNumber + "'";
                                DataTable dtClientTable = myHKeInvestData.getData(sql);

                                string emailTo = dtClientTable.Rows[0]["email"].ToString().Trim();
                                string clientTitle = dtClientTable.Rows[0]["clientTitle"].ToString().Trim();
                                string lastName = dtClientTable.Rows[0]["lastName"].ToString().Trim();
                                string subject = "[Important!] Alert For Security Holdings";
                                string body = "Dear " + clientTitle + " " + lastName + ",<br><br>Your security holding " + type + " " + code + " " + name + " has passed through the low value you set.<br>This alert is triggered at " + securityBase + currentPrice + "<br><br>Regards,<br>COMP3111 Team 115";
                                sendEmail(emailTo, subject, body);

                                //delete this alert since it's triggered
                                SqlTransaction trans = myHKeInvestData.beginTransaction();
                                sql = "UPDATE SecurityHolding SET lowValue = NULL WHERE accountNumber = '" + accountNumber + "' AND type = '" + type + "' AND code = '" + code + "'";
                                myHKeInvestData.setData(sql, trans);
                                myHKeInvestData.commitTransaction(trans);
                            }
                        }
                    }
                    //check if there's any other alert for this security holding
                    sql = "SELECT count(*) FROM SecurityHolding WHERE type = '" + type + "' AND code = '" + code + "' AND (highValue IS NOT NULL OR lowValue IS NOT NULL)";
                    decimal alertCount = myHKeInvestData.getAggregateValue(sql);
                    if (alertCount == 0 || alertCount == -1)
                    {
                        SqlTransaction trans = myHKeInvestData.beginTransaction();
                        sql = "DELETE FROM Alert WHERE type = '" + type + "' AND code = '" + code + "'";
                        myHKeInvestData.setData(sql, trans);
                        myHKeInvestData.commitTransaction(trans);
                    }
                }
            }
        }

        public decimal GetMonetaryValue(string AccountNumber, string Security_Type)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();
            // Secrurity_Type can be bond,stock,unit trust,all
            // if it is all, it's the sum of the 3 types (but not including the free_balance)
            if (Security_Type.Equals("all")) { return GetMonetaryValue(AccountNumber, "bond") + GetMonetaryValue(AccountNumber, "stock") + GetMonetaryValue(AccountNumber, "unit trust"); }

            string sql = "select code,shares,base from SecurityHolding where accountNumber = '" + AccountNumber + "' and type='" + Security_Type + "' AND shares<>0";
            DataTable dtHolding = myHKeInvestData.getData(sql);
            if (dtHolding == null) { return -1; } // If the DataSet is null, a SQL error occurred.
            if (dtHolding.Rows.Count == 0) { return 0; } //Not holding any security of this type
            decimal sum = 0m;
            foreach (DataRow row in dtHolding.Rows)
            {
                string securityCode = row["code"].ToString().Trim();
                decimal shares = Convert.ToDecimal(row["shares"]);
                decimal price = myExternalFunctions.getSecuritiesPrice(Security_Type, securityCode);
                string baseCurrency = row["base"].ToString().Trim();
                price=myHKeInvestCode.convertCurrency(baseCurrency.ToString().Trim(), myExternalFunctions.getCurrencyRate(baseCurrency).ToString().Trim(), "HKD", myExternalFunctions.getCurrencyRate("HKD").ToString().Trim(), price);
                sum = sum + price * shares;
            }
            return sum;
        }

        public double GetBalance(string AccountNumber)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            double balance = 0;
            string sql = " SELECT Balance From Account where AccountNumber = '" + AccountNumber + "'";
            DataTable dtBalance = myHKeInvestData.getData(sql);

            if (dtBalance.Rows.Count == 1)
            {
                foreach (DataRow row in dtBalance.Rows)
                {
                    balance = Convert.ToDouble(row["Balance"]);
                }

            }
            return balance;
        }

        public double GetAsset(string AccountNumber)
        {

            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();

            double Asset = myHKeInvestCode.GetBalance(AccountNumber);

            Asset += Convert.ToDouble(myHKeInvestCode.GetMonetaryValue(AccountNumber, "all"));

            return Asset;
        }

        //This returns a positive fee value with at most 2 significant figures. (If input is invalid, return -1)
        public double GetFee(string AccountNumber, string OrderType, string SecurityType, string StockType, double Amount)
        {   
            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();
            double Asset = myHKeInvestCode.GetAsset(AccountNumber);
            double fee = 0;
            double rate = 0;

            if (SecurityType == "stock")
            {
                if (Asset < 1000000)
                {
                    if (StockType == "market")
                    {
                        rate = 0.004;
                    }
                    else if (StockType == "limit" || StockType == "stop")
                    {
                        rate = 0.006;
                    }
                    else if (StockType == "stop limit")
                    {
                        rate = 0.008;
                    }
                }
                else
                {
                    if (StockType == "market")
                    {
                        rate = 0.002;
                    }
                    else if (StockType == "limit" || StockType == "stop")
                    {
                        rate = 0.004;
                    }
                    else if (StockType == "stop limit")
                    {
                        rate = 0.006;
                    }
                }
            }
            else
            {
                if (Asset < 500000)
                {
                    if (OrderType == "buy")
                    {
                        rate = 0.05;
                    }
                    else if (OrderType == "sell")
                    {
                        return 100;
                    }
                }
                else
                {
                    if (OrderType == "buy")
                    {
                        rate = 0.03;
                    }
                    else if (OrderType == "sell")
                    {
                        return 50;
                    }
                }
            }

            if(rate == 0)
            {
                return -1;//Exptional flow
            }

            
            
            fee = Math.Round(rate * Amount, 2);
            

            if (SecurityType == "stock")
            {
                if (Asset < 1000000 && fee < 150)
                {
                    return 150;
                }
                else if (Asset >= 1000000 && fee < 100)
                {
                    return 100;
                }
            }


            return fee;
        }

        public void changeBalance(string AccountNumber, double amount)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            double balance = myHKeInvestCode.GetBalance(AccountNumber);

            balance += amount;
            balance = Math.Round(balance, 2);

            SqlTransaction trans = myHKeInvestData.beginTransaction();
            string sql = "UPDATE Account SET Balance = " + balance + " WHERE AccountNumber = '" + AccountNumber + "'";
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }


        public void changeSecurityHolding(string AccountNumber, string Code, string Type, string Shares)
        {

            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();

            string sql = "SELECT * FROM SecurityHolding WHERE AccountNumber = '" + AccountNumber + "'and Code = " + Code + " and Type = '" + Type + "'";
            //sql = "SELECT COUNT(*) FROM SecurityHolding WHERE AccountNumber = '" + AccountNumber + "'and Code = " + Code + " and Type = '" + Type + "'";
            // d = myHKeInvestData.getAggregateValue(sql);
            DataTable dtSecurity = myHKeInvestData.getData(sql);

            DataTable dtname = myExternalFunctions.getSecuritiesByCode(Type, Code);
            string name = dtname.Rows[0]["name"].ToString().Trim();
            string Base = "HKD";
            if (!Type.Equals("stock"))
            {
                Base = dtname.Rows[0]["base"].ToString().Trim();
            }

            SqlTransaction trans = myHKeInvestData.beginTransaction();
            if (dtSecurity.Rows.Count == 0 || dtSecurity == null)//NO SECURITY INSIDE 
            {
                sql = "INSERT INTO SecurityHolding(AccountNumber,Code,Type,Name,Shares,Base,HighValue,LowValue) VALUES( '" + AccountNumber + "'," + Code + ",'" + Type + "','" + name + "'," + Shares + ",'" + Base + "',null,null)";
            }
            else {
                Shares = (Convert.ToDecimal(Shares) + Convert.ToDecimal(dtSecurity.Rows[0]["Shares"].ToString().Trim())).ToString();
                sql = "UPDATE SecurityHolding SET Shares = " + Shares + " WHERE AccountNumber = '" + AccountNumber + "' and Code = " + Code + " and Type = '" + Type + "'";
            }
            myHKeInvestData.setData(sql, trans);
            myHKeInvestData.commitTransaction(trans);
        }

        public decimal GetPendingValue(string AccountNumber)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();

            decimal pendingvalue = 0;
            string sql = "SELECT B.DollarAmount from [Order] O, BUTBuying B where B.OrderNumber = O.OrderNumber and O.CurrentStatus = 'pending' and O.AccountNumber = '" +AccountNumber + "'";
            pendingvalue = pendingvalue + myHKeInvestData.getAggregateValue(sql);

            sql = "SELECT B.OrderNumber, B.SharesToBuy, O.SecurityCode from[Order] O, StockBuying B where B.OrderNumber = O.OrderNumber and O.CurrentStatus = 'pending' and O.AccountNumber = '" + AccountNumber + "'";
            DataTable dtStock1 = myHKeInvestData.getData(sql);
            foreach (DataRow row in dtStock1.Rows)
            {
                string code = row["SecurityCode"].ToString();
                decimal price = myExternalFunctions.getSecuritiesPrice("stock", code);
                pendingvalue = pendingvalue + price * Convert.ToDecimal(row["SharesToBuy"].ToString());
            }

            sql = "SELECT B.OrderNumber, B.SharesToBuy, O.SecurityCode from[Order] O, StockBuying B where B.OrderNumber = O.OrderNumber and O.CurrentStatus = 'partial' and O.AccountNumber = '" + AccountNumber + "'";
            DataTable dtStock2 = myHKeInvestData.getData(sql);
            foreach (DataRow row in dtStock2.Rows)
            {
                decimal sharestobuy = Convert.ToDecimal(row["SharesToBuy"].ToString());
                string code = row["SecurityCode"].ToString();
                decimal price = myExternalFunctions.getSecuritiesPrice("stock", code);
                
                sql = "SELECT sum(executeshares) from [Transaction] T where OrderNumber = '"+row["OrderNumber"].ToString().Trim() +"'";
                decimal executedshares = myHKeInvestData.getAggregateValue(sql);

                pendingvalue = pendingvalue + price * (sharestobuy - executedshares);
            }

            return pendingvalue;
        }




        public string RefreshStatus()
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();

            string sql = "select OrderNumber,AccountNumber,OrderType,DateOfSubmission,CurrentStatus,SecurityCode,SecurityType from \"Order\" where CurrentStatus = 'partial' OR CurrentStatus = 'pending'";
            string AccountNumber = "";
            string ReferenceNumber = "";
            string OldStatus = "";
            string NewStatus = "";

            DataTable dtOrder = myHKeInvestData.getData(sql);// Get All Pending or Partial order reference number.

            if (dtOrder == null)
            {
                return "Error occured in connecting local database";
            }

            if (dtOrder.Rows.Count == 0)
            {
                return "No active order";
            }

            foreach (DataRow row in dtOrder.Rows)
            {
                AccountNumber = row["AccountNumber"].ToString().Trim();// Get AccountNumber number from datatable
                ReferenceNumber = row["OrderNumber"].ToString().Trim();// Get reference number from datatable
                OldStatus = row["CurrentStatus"].ToString().Trim();//Get old order status
                NewStatus = myExternalFunctions.getOrderStatus(ReferenceNumber).ToString().Trim();//Get new order status

                //Step 1: Update new transactions
                DataTable dtTrans = myExternalFunctions.getOrderTransaction(ReferenceNumber);
                if (dtTrans != null)
                {

                    //if exist transactions

                    foreach (DataRow Tran in dtTrans.Rows)
                    {   //CHECK: transaction already in database
                        sql = "SELECT count(TransactionNumber) from \"Transaction\" where OrderNumber ='" + ReferenceNumber + "'and TransactionNumber ='" + Tran["transactionNumber"].ToString().Trim() + "'";
                        if (myHKeInvestData.getAggregateValue(sql) == 1)
                        {
                            continue;
                        }
                        else
                        {   // if not, insert into database
                            sql = "INSERT INTO \"Transaction\"(OrderNumber,TransactionNumber,ExecuteDate,ExecuteShares,ExecutePrice) VALUES ('" + Tran["referenceNumber"].ToString().Trim().PadLeft(8, '0') + "','" + Tran["transactionNumber"].ToString().Trim() + "','" + Tran["executeDate"].ToString().Trim() + "'," + Tran["executeShares"].ToString().Trim() + "," + Tran["executePrice"].ToString().Trim() + ")";
                            SqlTransaction tr = myHKeInvestData.beginTransaction();
                            myHKeInvestData.setData(sql, tr);
                            myHKeInvestData.commitTransaction(tr);
                            //change balance and SecurityHolding
                            // get base 
                            string type = row["SecurityType"].ToString().Trim();
                            string Base = "HKD";
                            if (!type.Equals("stock"))
                            {
                                DataTable dtBase = myExternalFunctions.getSecuritiesByCode(type, row["SecurityCode"].ToString().Trim());
                                Base = dtBase.Rows[0]["base"].ToString().Trim();
                            }
                            decimal value = myHKeInvestCode.convertCurrency(Base, Convert.ToString(myExternalFunctions.getCurrencyRate(Base)), "HKD", myExternalFunctions.getCurrencyRate("HKD").ToString(), Convert.ToDecimal(Tran["executeShares"]) * Convert.ToDecimal(Tran["executePrice"]));

                            if (row[2].ToString().Trim() == "buy")//if buy
                            {
                                myHKeInvestCode.changeBalance(AccountNumber, -(double)value);

                                myHKeInvestCode.changeSecurityHolding(AccountNumber, row["SecurityCode"].ToString().Trim(), row["SecurityType"].ToString().Trim(), Tran["executeShares"].ToString().Trim());
                            }
                            else if (row[2].ToString().Trim() == "sell") //if sell
                            {
                                myHKeInvestCode.changeBalance(AccountNumber, (double)value);

                                myHKeInvestCode.changeSecurityHolding(AccountNumber, row["SecurityCode"].ToString().Trim(), row["SecurityType"].ToString().Trim(), "-" + Tran["executeShares"].ToString().Trim());
                            }


                        }
                    }
                }
                //Step 2: Update status
                if (NewStatus != OldStatus)
                {
                    sql = "UPDATE \"Order\" SET CurrentStatus = '" + NewStatus + "' WHERE AccountNumber = '" + AccountNumber + "'and OrderNumber = '" + ReferenceNumber + "'";
                   SqlTransaction tr = myHKeInvestData.beginTransaction();
                    myHKeInvestData.setData(sql, tr);
                    myHKeInvestData.commitTransaction(tr);
                }




                //Step 3: if order is complete, deduct fee from report and generate invoice.
                if ((NewStatus == "completed" || NewStatus == "cancelled") && NewStatus != OldStatus)
                {
                    double amount = 0;
                    string stockType = "";
                    double fee = 0;
                    if (dtTrans != null)
                    {
                        foreach (DataRow Tran in dtTrans.Rows)
                        {
                            string type = row["SecurityType"].ToString().Trim();
                            string Base = "HKD";
                            if (!type.Equals("stock"))
                            {
                                DataTable dtBase = myExternalFunctions.getSecuritiesByCode(type, row["SecurityCode"].ToString().Trim());
                                Base = dtBase.Rows[0]["base"].ToString().Trim();
                            }
                            decimal value = myHKeInvestCode.convertCurrency(Base, Convert.ToString(myExternalFunctions.getCurrencyRate(Base)), "HKD", myExternalFunctions.getCurrencyRate("HKD").ToString(), Math.Round(Convert.ToDecimal(Tran["executeShares"]) * Convert.ToDecimal(Tran["executePrice"]), 2));
                            amount += (double)value;
                        }
                        if (row["SecurityType"].ToString().Trim() == "stock")
                        {
                            if (row["OrderType"].ToString().Trim() == "buy")
                            {
                                sql = "select * from stockbuying where ordernumber = '" + ReferenceNumber + "'";
                                stockType = myHKeInvestData.getData(sql).Rows[0]["BuyingType"].ToString().Trim();
                            }
                            else
                            {
                                sql = "select * from stockselling where ordernumber = '" + ReferenceNumber + "'";
                                stockType = myHKeInvestData.getData(sql).Rows[0]["SellingType"].ToString().Trim();
                            }
                        }
                        fee = GetFee(AccountNumber, row["OrderType"].ToString().Trim(), row["SecurityType"].ToString().Trim(), stockType, amount);
                    }

                    sql = "UPDATE \"Order\" SET Fee = '" + fee + "' WHERE Ordernumber = '" + ReferenceNumber + "'";
                    SqlTransaction trans = myHKeInvestData.beginTransaction();
                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);
                    myHKeInvestCode.changeBalance(AccountNumber, -fee);
                    generateInvoice(AccountNumber, ReferenceNumber, fee);


                }


            }

            return "Status Updated";
        }

        public void generateInvoice(string AccountNumber, string ReferenceNumber, double fee)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            ExternalFunctions myExternalFunctions = new ExternalFunctions();


            //fetch order information and transaction information
            string sql = "select * from \"order\" where accountnumber = '" + AccountNumber + "' and OrderNumber = '" + ReferenceNumber + "'";
            DataTable dtOrder = myHKeInvestData.getData(sql);
            sql = "select * from \"transaction\" where OrderNumber = '" + ReferenceNumber + "'";
            DataTable dtTrans = myHKeInvestData.getData(sql);
            sql = "select email from client where accountnumber = '" + AccountNumber + "' and IsPrimaryHolder = 'true'";
            DataTable dtemail = myHKeInvestData.getData(sql);


            if (dtOrder.Rows.Count == 0 || dtemail.Rows.Count == 0 || dtTrans.Rows.Count == 0)
            {
                return;
            }




            //initialize body
            string body = "Your order has been processed, below is your order information: \n\n ";
            string type = dtOrder.Rows[0]["SecurityType"].ToString().Trim();
            string Code = dtOrder.Rows[0]["SecurityCode"].ToString().Trim();
            DataTable dtname = myExternalFunctions.getSecuritiesByCode(type, Code);
            string name = dtname.Rows[0]["name"].ToString().Trim();
            string stockordertype = "";
            decimal shares = 0;
            decimal dollar = 0;

            foreach (DataRow row in dtTrans.Rows)
            {
                shares += Convert.ToDecimal(row["executeShares"]);
                dollar += Convert.ToDecimal(row["executePrice"])*Convert.ToDecimal(row["executeShares"]);
            }





            if (dtOrder.Rows[0]["OrderType"].ToString().Trim() == "stock")
            {
                sql = "select BuyingType from stockbuying where ordernumber = '" + ReferenceNumber + "'";
                DataTable dtStock = myHKeInvestData.getData(sql);
                if (dtStock.Rows.Count == 0)
                {
                    sql = "select SellingType from stockselling where ordernumber = '" + ReferenceNumber + "'";
                    dtStock = myHKeInvestData.getData(sql);
                }
                stockordertype = dtStock.Rows[0][0].ToString().Trim();
            }


            //record order info
            body += ("Account Number: " + AccountNumber + "\t OrderNumber: " + ReferenceNumber + "\n");
            body += ("Order Type: " + type + "\t SecurityCode: " + Code + "\n");
            body += ("Security Name: " + name + "\n");

            if (stockordertype != "")//if an stock order
            {
                body += ("Stock Order Type: " + stockordertype + "\n");
            }

            string Base = "HKD";
            if (!type.Equals("stock"))
            {
                DataTable dtBase = myExternalFunctions.getSecuritiesByCode(type, Code);
                Base = dtBase.Rows[0]["base"].ToString();
            }

            body += ("Total Shares: " + shares + "\t Total Monetary Amount: " + Base + Math.Round(dollar,2).ToString().Trim() + "\n");
            body += ("Date Of Submission " + Convert.ToDateTime(dtOrder.Rows[0]["DateOfSubmission"]).ToString("dd/MM/yyyy") + "\t Charged Fee: HKD" + fee + "\n\n");


            //record transaction info  
            foreach (DataRow row in dtTrans.Rows)
            {

                body += ("Transaction Number: " + row["transactionNumber"].ToString().Trim().PadLeft(8,'0') + "\t Execution Date: " + (Convert.ToDateTime(row["executeDate"])).ToString("dd/MM/yyyy") + "\n");
                body += ("Executed Quantity: " + row["executeShares"].ToString().Trim() + "\t Execution Price: " + row["executePrice"].ToString().Trim() + "\n\n");

            }



            myHKeInvestCode.sendEmail(dtemail.Rows[0]["Email"].ToString().Trim(), "HKeInvest order invoice", body);
        }


        //Function to duplicate the ' in the string to prevent SQL query error
        public string checkAndAddApostrophe(string input)
        {
            string trimInput = input.Trim();

            //Replace ' with '' in the string
            trimInput = trimInput.Replace("'", "''");

            return trimInput;
        }
       
        public string checkSQLqueryForReservePun(string input)
        {
            string triminput = input.Trim();
            int length = triminput.Length;


            //Replace [ with [[]
            for(int i=0;i< length;i++)
            {
                if (triminput[i]=='[')
                {
                    string triminput1 = triminput.Substring(0, i);
                    string triminput2 = (i+1<length)?triminput.Substring(i+1):null;
                    triminput = triminput1 + "[" + triminput[i] + "]" + triminput2;
                    length = length + 2;
                    i+=2;
                }   
            }

            //Replace ' with ['] in the string
            triminput = triminput.Replace("'", "''");

            //Replace % with [%]
            triminput = triminput.Replace("%", "[%]");

            //Replace _ with [_]
            triminput = triminput.Replace("_", "[_]");

            return triminput;
        }
    }
}
