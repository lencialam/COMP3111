using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using HKeInvestWebApplication.Models;
using System.Data;
using System.Data.SqlClient;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HKeInvestData myHKeInvestData = new HKeInvestData();
                HKeInvestCode myHKeInvestCode = new HKeInvestCode();

                string sql = "select count(*) from Client where firstname='" + myHKeInvestCode.checkSQLqueryForReservePun(FirstName.Text.Trim()).Trim() + "' and lastname='" 
                    + myHKeInvestCode.checkSQLqueryForReservePun(LastName.Text.Trim()).Trim() + "' and dateOfBirth=CONVERT(date,'" + DateOfBirth.Text.Trim() + "',103) and email='" 
                    + myHKeInvestCode.checkSQLqueryForReservePun(Email.Text.Trim()).Trim() + "' and HKIDPassportNumber='" 
                    + myHKeInvestCode.checkSQLqueryForReservePun(HKID.Text.Trim()).Trim() + "' and accountNumber='" + AccountNumber.Text.Trim() + "' AND IsPrimaryHolder = 'true'";
                //***********Why not use the external function instead?
                //DataTable dtClient = myHKeInvestData.getData(sql);
                //int tablesize = 0;
                //foreach(DataRow row in dtClient.Rows)
                //{
                //    tablesize = tablesize + 1;
                //}
                //if (tablesize==0)
                //{
                //    ErrorMessage.Text = "The information of the client is not correct.";
                //}
                //***********Why not use the external function instead?
                decimal tablesize = myHKeInvestData.getAggregateValue(sql);
                if (tablesize == 0 || tablesize == -1) {
                    ErrorMessage.Text = "The information of the client is not correct.";
                }
                else
                {
                    sql = "SELECT count(*) FROM Account WHERE accountNumber = '" + AccountNumber.Text.Trim() + "' AND userName IS NULL";
                    decimal existing = myHKeInvestData.getAggregateValue(sql);

                    if (existing == 1)
                    {
                        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                        var user = new ApplicationUser() { UserName = UserName.Text, Email = Email.Text };
                        IdentityResult result = manager.Create(user, Password.Text);
                        if (result.Succeeded)
                        {
                            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                            //string code = manager.GenerateEmailConfirmationToken(user.Id);
                            //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                            //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                            string code = manager.GenerateEmailConfirmationToken(user.Id);
                            string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                            string subject = "Confirm your account";
                            string body = "Dear Client<br><br>Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.<br><br>Regards,<br>COMP3111 Team 115";
                            myHKeInvestCode.sendEmail(Email.Text.Trim(), subject, body);

                            //Assign the user to the Client role.
                            IdentityResult roleResult = manager.AddToRole(user.Id, "Client");
                            if (!roleResult.Succeeded)
                            {
                                ErrorMessage.Text = roleResult.Errors.FirstOrDefault();
                            }

                            //Update the username for the user's account in the Account table
                            UpdateClientRecord(AccountNumber.Text.Trim(), UserName.Text.Trim());

                            signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);

                        }
                        else
                        {
                            ErrorMessage.Text = result.Errors.FirstOrDefault();
                        }
                    }
                    else {
                        ErrorMessage.Text = "This client already has an online account.";
                    }
                }
            }
        }


        private void UpdateClientRecord(string accountnum, string username)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            SqlTransaction trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData("UPDATE Account SET userName='" + username + "' where accountNumber='" + accountnum + "'", trans);
            myHKeInvestData.commitTransaction(trans);
        }


        protected void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            string accountnum = AccountNumber.Text.Trim();

            string firstletter = null;
            string secondletter = null;
            string lastnameupp = LastName.Text.Trim();

            for (int i = 0; i < lastnameupp.Length; i++)
            {
                if (Char.IsLetter(lastnameupp[i]))
                {
                    if (firstletter == null)
                        firstletter = lastnameupp[i].ToString().Trim().ToUpper();
                    else if (secondletter == null)
                        secondletter = lastnameupp[i].ToString().Trim().ToUpper();
                }
            }

            //Check the length of the account number is correct or not
            if (accountnum.Length != 10)
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "The length of the account number is not correct.";
            }
            else if (lastnameupp.Length == 0)
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "The account number does not match the client's last name.";
            }
            else if(!Char.IsLetter(accountnum[0]) || !Char.IsLetter(accountnum[1]))
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "The account number is not valid";
            }
            else
            {
                //Check whether the account number begins with the correct initials of the client's last name
                if (secondletter==null)
                {
                    if (!accountnum[0].ToString().Trim().Equals(firstletter) || !accountnum[1].ToString().Trim().Equals(firstletter))
                    {
                        args.IsValid = false;
                        CustomValidator1.ErrorMessage = "The account number does not match the client's last name.";
                    }
                }
                else
                {
                    if (!accountnum[0].ToString().Trim().Equals(firstletter) || !accountnum[1].ToString().Trim().Equals(secondletter))
                    {
                        args.IsValid = false;
                        CustomValidator1.ErrorMessage = "The account number does not match the client's last name.";
                    }
                }


                //Check whether the account number's last 8 characters are all digits
                if (args.IsValid)
                {
                    string substring = accountnum.Substring(2);
                    if (!substring.All(c => char.IsDigit(c)))
                    {
                        args.IsValid = false;
                        CustomValidator1.ErrorMessage = "The account number should end with 8 digits.";
                    }
                }
            }
        }
    }
}