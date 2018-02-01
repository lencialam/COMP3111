using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string lastnameupp = LastName.Text.ToUpper().Trim();
            string accountnum = AccountNumber.Text.Trim();

            //Check if the account number contains the first character of the client's last name
            if (accountnum.Length==0||lastnameupp.Length==0||!accountnum.Contains(lastnameupp[0]))
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "The account number does not match the client's last name.";
            }
            // Check if the account number contains the second character of the client's last name
            else if (accountnum.Contains(lastnameupp[1]))
            { 
                //Check whether the length of the string is correct
                if (accountnum.Length != 10)
                {
                    args.IsValid = false;
                    CustomValidator1.ErrorMessage = "The account number starting with 2 letter should contain the letters followed by 8 digits.";
                }
                else
                {
                    string numsubstring = accountnum.Substring(2);
                    if (!numsubstring.All(c=>char.IsDigit(c)))
                    {
                        args.IsValid = false;
                        CustomValidator1.ErrorMessage = "The account number starting with 2 letter should contain the letters followed by 8 digits.";
                    }
                }
            }
            else
            {
                //Check whether the length of the string is correct
                if (accountnum.Length != 9)
                {
                    args.IsValid = false;
                    CustomValidator1.ErrorMessage = "The account number starting with 1 letter should contain the letter followed by 8 digits.";
                }
                else
                {
                    string numsubstring = accountnum.Substring(2);
                    if (!numsubstring.All(c => char.IsDigit(c)))
                    {
                        args.IsValid = false;
                        CustomValidator1.ErrorMessage = "The account number starting with 1 letter should contain the letter followed by 8 digits.";
                    }
                }
            }
        }
    }
}