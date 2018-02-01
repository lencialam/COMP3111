using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using Microsoft.AspNet.Identity;
using System.Windows.Forms;
using System.Data.SqlClient;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication
{
    public partial class SetAlert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();

            string username = Context.User.Identity.GetUserName().Trim();
            string sql = "SELECT count(*) FROM Account WHERE userName = '" + username + "'";
            decimal d = myHKeInvestData.getAggregateValue(sql);
            if (d != 0 && d != -1)
            {
                sql = "SELECT accountNumber FROM Account WHERE userName = '" + username + "'";
                string accountNumber = myHKeInvestData.getData(sql).Rows[0]["accountNumber"].ToString().Trim();
                Session["AccountNumber"] = accountNumber;
            }
            
        }

        protected void SetAlert_Click(object sender, EventArgs e)
        {
            if(!Page.IsValid){ return; }
            HKeInvestData myHKeInvestData = new HKeInvestData();

            string username = Context.User.Identity.GetUserName().Trim();
            string sql = "SELECT accountNumber FROM Account WHERE userName = '" + username + "'";
            string accountNumber = myHKeInvestData.getData(sql).Rows[0]["accountNumber"].ToString().Trim();

            string typestr = SecurityType.SelectedValue.Trim();
            string codestr = SecurityCode.Text.Trim();
            string lowValue = (string.IsNullOrEmpty(LowValue.Text.Trim())) ? "NULL" : "'" + LowValue.Text.Trim() + "'";
            string highValue = (string.IsNullOrEmpty(HighValue.Text.Trim())) ? "NULL" : "'" + HighValue.Text.Trim() + "'";
            int highOrLow = AlertType.SelectedIndex;

            //check whether this security holding exists
            sql = "SELECT count(*) FROM SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" + accountNumber + "' AND shares <>0";
            decimal d = myHKeInvestData.getAggregateValue(sql);
            if (d == 0 || d == -1)
            {
                MessageBox.Show("No Such Security Holding is Found!");
            }
            else
            {
                //when setting low value alert
                if (highOrLow == 0 && lowValue != "NULL")
                {
                    //check if there is a high value alert for this security
                    sql = "SELECT count(*) FROM SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" 
                        + accountNumber + "' AND highValue IS NOT NULL";
                    decimal countHigh = myHKeInvestData.getAggregateValue(sql);
                    bool canSet = true;
                    //if high value exists
                    if (countHigh > 0)
                    {
                        sql = "SELECT highValue FROM SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" 
                            + accountNumber + "'";
                        decimal highValueInDB = Convert.ToDecimal(myHKeInvestData.getData(sql).Rows[0]["highValue"]);
                        if (highValueInDB <= Convert.ToDecimal(LowValue.Text.ToString().Trim()))
                        {
                            canSet = false;
                            MessageBox.Show("Low value for alert cannot be higher than its high value!");
                        }
                    }
                    if (canSet)
                    {
                        SqlTransaction trans = myHKeInvestData.beginTransaction();
                        sql = "UPDATE SecurityHolding SET lowValue = " + lowValue + " WHERE type = '" + typestr + "' AND code = '"
                        + codestr + "' AND accountNumber = '" + accountNumber + "'";
                        myHKeInvestData.setData(sql, trans);
                        myHKeInvestData.commitTransaction(trans);
                        MessageBox.Show("Alert is Set!");
                    }
                }
                //when setting a high value alert
                else if (highOrLow == 1 && highValue != "NULL")
                {
                    //check if there is a low value alert for this security
                    sql = "SELECT count(*) FROM SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" 
                        + accountNumber + "' AND lowValue IS NOT NULL";
                    decimal countLow = myHKeInvestData.getAggregateValue(sql);
                    bool canSet = true;
                    //if low value exists
                    if (countLow > 0)
                    {
                        sql = "SELECT lowValue FROM SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" 
                            + accountNumber + "'";
                        decimal lowValueInDB = Convert.ToDecimal(myHKeInvestData.getData(sql).Rows[0]["lowValue"]);
                        if (lowValueInDB >= Convert.ToDecimal(HighValue.Text.ToString().Trim()))
                        {
                            canSet = false;
                            MessageBox.Show("High value for alert cannot be lower than its low value!");
                        }
                    }
                    if (canSet)
                    {
                        SqlTransaction trans = myHKeInvestData.beginTransaction();
                        sql = "UPDATE SecurityHolding SET highValue = " + highValue + " WHERE type = '" + typestr + "' AND code = '"
                        + codestr + "' AND accountNumber = '" + accountNumber + "'";
                        myHKeInvestData.setData(sql, trans);
                        myHKeInvestData.commitTransaction(trans);
                        MessageBox.Show("Alert is Set!");
                    }
                }
                //check if the security is in the Alert table
                sql = "SELECT count(*) FROM Alert WHERE type = '" + typestr + "' AND code = '" + codestr + "'";
                decimal count = myHKeInvestData.getAggregateValue(sql);
                //if not, insert it
                if (count == 0 || count == -1)
                {
                    ExternalFunctions myExternalFunctions = new ExternalFunctions();
                    decimal currentPrice = myExternalFunctions.getSecuritiesPrice(typestr, codestr);

                    SqlTransaction trans = myHKeInvestData.beginTransaction();
                    sql = "INSERT INTO Alert Values ('" + typestr + "','" + codestr + "','" + currentPrice + "', '0.00')";
                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);
                }
                Response.Redirect("SetAlert.aspx");
            }
        }

        protected void DeleteAlert_Click(object sender, EventArgs e)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();

            string username = Context.User.Identity.GetUserName().Trim();
            string sql = "SELECT accountNumber FROM Account WHERE userName = '" + username + "'";
            string accountNumber = myHKeInvestData.getData(sql).Rows[0]["accountNumber"].ToString().Trim();
            int highOrLow = AlertType.SelectedIndex;

            string typestr = SecurityType.SelectedValue.Trim();
            string codestr = SecurityCode.Text.Trim();
            sql = "SELECT count(*) FROM SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" + accountNumber + "'";
            decimal d = myHKeInvestData.getAggregateValue(sql);
            if (d == 0 || d == -1)
            {
                MessageBox.Show("No Such Security Holding is Found!");
            }
            else
            {
                if (highOrLow == 0)
                {
                    sql = "SELECT count(*) FROM  SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" + accountNumber + "' AND lowValue IS NOT NULL";
                    decimal countLow = myHKeInvestData.getAggregateValue(sql);
                    if (countLow == 0 || countLow == -1)
                    {
                        MessageBox.Show("No low value alert found for this security holding!");
                    }
                    else
                    {
                        SqlTransaction trans = myHKeInvestData.beginTransaction();
                        sql = "UPDATE SecurityHolding SET lowValue = NULL WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" + accountNumber + "'";
                        myHKeInvestData.setData(sql, trans);
                        myHKeInvestData.commitTransaction(trans);
                        string msg = "Low Value Alert for " + typestr + " " + codestr + " is deleted!";
                        MessageBox.Show(msg);
                    }
                }
                else
                {
                    sql = "SELECT count(*) FROM  SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" + accountNumber + "' AND highValue IS NOT NULL";
                    decimal countHigh = myHKeInvestData.getAggregateValue(sql);
                    if (countHigh == 0 || countHigh == -1)
                    {
                        MessageBox.Show("No high value alert found for this security holding!");
                    }
                    else
                    {
                        SqlTransaction trans = myHKeInvestData.beginTransaction();
                        sql = "UPDATE SecurityHolding SET highValue = NULL WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND accountNumber = '" + accountNumber + "'";
                        myHKeInvestData.setData(sql, trans);
                        myHKeInvestData.commitTransaction(trans);
                        string msg = "High Value Alert for " + typestr + " " + codestr + " is deleted!";
                        MessageBox.Show(msg);
                    }
                }
                Response.Redirect("SetAlert.aspx");
            }
            //check if there's any other alert for this security holding
            sql = "SELECT count(*) FROM SecurityHolding WHERE type = '" + typestr + "' AND code = '" + codestr + "' AND (highValue IS NOT NULL OR lowValue IS NOT NULL)";
            decimal alertCount = myHKeInvestData.getAggregateValue(sql);
            if (alertCount == 0 || alertCount == -1)
            {
                SqlTransaction trans = myHKeInvestData.beginTransaction();
                sql = "DELETE FROM Alert WHERE type = '" + typestr + "' AND code = '" + codestr + "'";
                myHKeInvestData.setData(sql, trans);
                myHKeInvestData.commitTransaction(trans);
            }
        }

        protected void AlertType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AlertType.SelectedIndex == 0)
            {
                LowValueDiv.Disabled = false;
                LowValueDiv.Visible = true;
                HighValueDiv.Disabled = true;
                HighValueDiv.Visible = false;

            }
            else
            {
                LowValueDiv.Disabled = true;
                LowValueDiv.Visible = false;
                HighValueDiv.Disabled = false;
                HighValueDiv.Visible = true;
            }
        }

        protected void SecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SecurityType.SelectedIndex == 2)
            {
                LowValue.MaxLength = 7;
                HighValue.MaxLength = 7;
                revLowValue.ValidationExpression = "^([0-9]{0,4})(\\.[0-9]{1,2})?$";
                revHighValue.ValidationExpression = "^([0-9]{0,4})(\\.[0-9]{1,2})?$";
                revLowValue.ErrorMessage = "Low Value of a unit trust must be a decimal number with at most 4 digits before decimal point and a precision of at most two.";
                revHighValue.ErrorMessage = "High Value of a unit trust must be a decimal number with at most 4 digits before decimal point and a precision of at most two.";

            }
            else {
                LowValue.MaxLength = 6;
                HighValue.MaxLength = 6;
                revLowValue.ValidationExpression = "^([0-9]{0,3})(\\.[0-9]{1,2})?$";
                revHighValue.ValidationExpression = "^([0-9]{0,3})(\\.[0-9]{1,2})?$";
                revLowValue.ErrorMessage = "Low Value of a unit trust must be a decimal number with at most 3 digits before decimal point and a precision of at most two.";
                revHighValue.ErrorMessage = "High Value of a unit trust must be a decimal number with at most 3 digits before decimal point and a precision of at most two.";
            }
        }
    }
}