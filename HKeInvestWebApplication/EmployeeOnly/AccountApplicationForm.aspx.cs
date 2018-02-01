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
using System.Globalization;

namespace HKeInvestWebApplication
{
    public partial class AccountApplicationForm : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Set Default value for Textbox of Passport of issue as well as the Checkbox for Account Feature
                SweepFreeCreditBalance.Items[0].Selected = true;
                HKIDorPassport.Items[0].Selected = true;
                CHKIDorPassport.Items[0].Selected = true;

                //Set the error message and the successful message hidden
                errormsg.Visible = false;
                successmsg.Visible = false;

                //Disable the Co-Accout holder's information and hide the form
                ClientinfoCClientDiv.Disabled = true;
                ClientinfoCClientDiv.Visible = false;
                EmployinfoCClientDiv.Disabled = true;
                EmployinfoCClientDiv.Visible = false;
                DisclosureCClientDiv.Disabled = true;
                DisclosureCClientDiv.Visible = false;

                //Description of other funds is disabled and hidden
                DescribeOtherFundDiv.Disabled = true;
                DescribeOtherFundDiv.Visible = false;

                //The details for the Employment is disabled and hidden
                PClientEmploydetailDiv.Disabled = true;
                PClientEmploydetailDiv.Visible = false;

                CClientEmploydetailDiv.Disabled = true;
                CClientEmploydetailDiv.Visible = false;

                //The textboxes for the cheque value and the amount of the transfer is disabled and hidden
                ChequeDiv.Visible = false;
                ChequeDiv.Disabled = true;

                TransferDiv.Visible = false;
                TransferDiv.Disabled = true;

                //The div for showing the textbox for passport country is hidden
                passportcountrydiv.Visible = false;
                passportcountrydiv.Disabled = true;
                cpassportcountrydiv.Visible = false;
                cpassportcountrydiv.Disabled = true;
            }
        }


        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InitialDeposit.Items[0].Selected)
            {
                ChequeDiv.Visible = true;
                ChequeDiv.Disabled = false;
            }
            else
            {
                ChequeDiv.Visible = false;
                ChequeDiv.Disabled = true;
            }
            if (InitialDeposit.Items[1].Selected)
            {
                TransferDiv.Visible = true;
                TransferDiv.Disabled = false;
            }
            else
            {
                TransferDiv.Visible = false;
                TransferDiv.Disabled = true;
            }
        }

        protected void AccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AccountType.Items[0].Selected)
            {
                ClientinfoCClientDiv.Disabled = true;
                ClientinfoCClientDiv.Visible = false;
                EmployinfoCClientDiv.Disabled = true;
                EmployinfoCClientDiv.Visible = false;
                DisclosureCClientDiv.Disabled = true;
                DisclosureCClientDiv.Visible = false;
            }
            else
            {
                ClientinfoCClientDiv.Disabled = false;
                ClientinfoCClientDiv.Visible = true;
                EmployinfoCClientDiv.Disabled = false;
                EmployinfoCClientDiv.Visible = true;
                DisclosureCClientDiv.Disabled = false;
                DisclosureCClientDiv.Visible = true;
            }
        }

        protected void SourceOfFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SourceOfFund.Items[3].Selected)
            {
                DescribeOtherFundDiv.Disabled = false;
                DescribeOtherFundDiv.Visible = true;
            }
            else
            {
                DescribeOtherFundDiv.Disabled = true;
                DescribeOtherFundDiv.Visible = false;
            }
        }


        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(HomePhone.Text.Trim()) && string.IsNullOrEmpty(BusinessPhone.Text.Trim()) && string.IsNullOrEmpty(MobilePhone.Text.Trim()))
                args.IsValid = false;
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(CHomePhone.Text.Trim()) && string.IsNullOrEmpty(CBusinessPhone.Text.Trim()) && string.IsNullOrEmpty(CMobilePhone.Text.Trim()))
                args.IsValid = false;
        }

        protected void cvDOB_not_future_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string DOB = DateOfBirth.Text.Trim();
            string format = "dd/MM/yyyy";
            DateTime dd;
            if (DateTime.TryParseExact(DOB, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dd)) { }
            else
            {
                return;
            }
            if (dd> DateTime.Today)
            {
                args.IsValid = false;
            }
        }
        protected void cvcoDOB_not_future_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string DOB = CDateOfBirth.Text.Trim();
            string format = "dd/MM/yyyy";
            DateTime dd;
            if (DateTime.TryParseExact(DOB, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dd)) { }
            else
            {
                return;
            }
            if (dd > DateTime.Today)
            {
                args.IsValid = false;
            }
        }

        protected void EmploymentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EmploymentStatus.Items[0].Selected)
            {
                PClientEmploydetailDiv.Disabled = false;
                PClientEmploydetailDiv.Visible = true;
            }
            else
            {
                PClientEmploydetailDiv.Disabled = true;
                PClientEmploydetailDiv.Visible = false;
            }
        }

        protected void CEmploymentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CEmploymentStatus.Items[0].Selected)
            {
                CClientEmploydetailDiv.Disabled = false;
                CClientEmploydetailDiv.Visible = true;
            }
            else
            {
                CClientEmploydetailDiv.Disabled = true;
                CClientEmploydetailDiv.Visible = false;
            }
        }

        protected void Apply_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HKeInvestData myHKeInvestData = new HKeInvestData();
                HKeInvestCode myHKeInvestCode = new HKeInvestCode();            
                string sql;

                //Check whether the primary account holder (and the co-account holder has already got an account in the system
                if (AccountType.Items[0].Selected)
                {
                    sql = "SELECT HKIDPassportNumber from Client where HKIDPassportNumber ='" + myHKeInvestCode.checkSQLqueryForReservePun(HKID.Text.Trim()) + "'";
                    DataTable dtClient = myHKeInvestData.getData(sql);
                    if(dtClient.Rows.Count!=0)
                    {
                        Label41.Text = "The Primary holder has already had an account in the system.";
                        errormsg.Visible = true;
                        successmsg.Visible = false;
                        return;
                    }
                }
                else
                {
                    sql = "SELECT HKIDPassportNumber from Client where HKIDPassportNumber ='" + myHKeInvestCode.checkSQLqueryForReservePun(HKID.Text.Trim()) + "'";
                    DataTable dtClient = myHKeInvestData.getData(sql);
                    sql = "SELECT HKIDPassportNumber from Client where HKIDPassportNumber ='" + myHKeInvestCode.checkSQLqueryForReservePun(CHKID.Text.Trim()) + "'";
                    DataTable dtCClient = myHKeInvestData.getData(sql);
                    if (dtClient.Rows.Count != 0 && dtCClient.Rows.Count!=0)
                    {
                        Label41.Text = "The Primary holder and Co-Account holder have already had accounts in the system.";
                        errormsg.Visible = true;
                        successmsg.Visible = false;
                        return;
                    }
                    else if(dtClient.Rows.Count != 0)
                    {
                        Label41.Text = "The Primary holder has already had an account in the system.";
                        errormsg.Visible = true;
                        successmsg.Visible = false;
                        return;
                    }
                    else if (dtCClient.Rows.Count != 0)
                    {
                        Label41.Text = "The Co-Account holder has already had an account in the system.";
                        errormsg.Visible = true;
                        successmsg.Visible = false;
                        return;
                    }
                }
                

                string acctNumInitials;


                //Get the initials of the account number
                string firstletter=null;
                string secondletter=null;
                string lastname = LastName.Text.Trim();

                for(int i=0;i<lastname.Length;i++)
                {
                    if(Char.IsLetter(lastname[i]))
                    {
                        if (firstletter == null)
                            firstletter = lastname[i].ToString().Trim().ToUpper();
                        else if (secondletter == null)
                            secondletter = lastname[i].ToString().Trim().ToUpper();
                    }
                }
                
                if(secondletter==null)
                {
                    acctNumInitials = firstletter + firstletter;
                }
                else
                {
                    acctNumInitials = firstletter + secondletter;
                }

                //Get the initials of the account number
                /*
                if (LastName.Text.Trim().Length == 1)
                {
                    acctNumInitials = myHKeInvestCode.checkSQLqueryForReservePun(LastName.Text.Trim().ToUpper()) + myHKeInvestCode.checkSQLqueryForReservePun(LastName.Text.Trim().ToUpper());
                }
                else
                {
                    acctNumInitials = myHKeInvestCode.checkSQLqueryForReservePun(LastName.Text.Trim().Substring(0, 2).ToUpper());
                }
                */

                //Get the number of account numbers which shared the same initials, then generate the proper account number for the client
                sql = "select AccountNumber from Account where accountNumber like '" + acctNumInitials + "%'";
                DataTable dtAccountNumber = myHKeInvestData.getData(sql);
                int tablesize = 0;
                foreach (DataRow row in dtAccountNumber.Rows)
                {
                    tablesize = tablesize + 1;
                }
                string acctNumDigit = (tablesize + 1).ToString("00000000");
                string acctNum = acctNumInitials + acctNumDigit;

                //Get the initial deposit
                double amount = 0;
                if (InitialDeposit.Items[0].Selected)
                {
                    if (InitialDeposit.Items[1].Selected)
                        amount = Convert.ToDouble(Cheque.Text.Trim()) + Convert.ToDouble(Transfer.Text.Trim());
                    else
                        amount = Convert.ToDouble(Cheque.Text.Trim());
                }
                else
                {
                    amount = Convert.ToDouble(Transfer.Text.Trim());
                }

                //Use SQL to create the account in the database
                string sourceoffund = (SourceOfFund.Items[3].Selected) ? myHKeInvestCode.checkSQLqueryForReservePun(DescribeOtherFund.Text.Trim()) : SourceOfFund.SelectedValue.Trim();
                SqlTransaction trans = myHKeInvestData.beginTransaction();
                sql = "INSERT INTO Account(AccountNumber,AccountType, Balance,InvestmentObjective, InvestmentKnowledge, AnnualIncome,Liquidity,EarnOnBalance,InvestmentExperience,SourceOfFund) VALUES ('"
                    + acctNum + "','" + AccountType.SelectedValue.Trim() + "','" + amount + "', '" + InvestmentObjective.SelectedValue.Trim() + "','" + InvestmentKnowledge.SelectedValue.Trim() + "','"
                    + AnnualIncome.SelectedValue.Trim() + "','" + LiquidNetWorth.SelectedValue.Trim() + "','" + SweepFreeCreditBalance.SelectedValue.Trim() + "','" + InvestmentExperience.SelectedValue.Trim()
                    + "','" + sourceoffund + "')";

                myHKeInvestData.setData(sql, trans);
                myHKeInvestData.commitTransaction(trans);


                //Use SQL to create the client(s) in the database
                if (AccountType.Items[0].Selected)
                {
                    string building = (string.IsNullOrEmpty(Building.Text.Trim())) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(Building.Text.Trim()) + "'";
                    string homePhone = (string.IsNullOrEmpty(HomePhone.Text.Trim())) ? "NULL" : "'" + HomePhone.Text.Trim() + "'";
                    string homeFax = (string.IsNullOrEmpty(HomeFax.Text.Trim())) ? "NULL" : "'" + HomeFax.Text.Trim() + "'";
                    string busiPhone = (string.IsNullOrEmpty(BusinessPhone.Text.Trim())) ? "NULL" : "'" + BusinessPhone.Text.Trim() + "'";
                    string mobile = (string.IsNullOrEmpty(MobilePhone.Text.Trim())) ? "NULL" : "'" + MobilePhone.Text.Trim() + "'";

                    //Check whether the Client use HKID or Passport
                    string passportcountry = (HKIDorPassport.Items[0].Selected) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(PassportCountryOfIssue.Text.Trim()) + "'";

                    //Check whether the Client is employed
                    string specificoccupation = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(SpecificOccupation.Text.Trim()) + "'" : "NULL";
                    string yearswithemployer = (EmploymentStatus.Items[0].Selected) ? "'" + YearsWithEmployer.Text.Trim() + "'" : "NULL";
                    string employername = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(EmployerName.Text.Trim()) + "'" : "NULL";
                    string employerphone = (EmploymentStatus.Items[0].Selected) ? "'" + EmployerPhone.Text.Trim() + "'" : "NULL";
                    string natureofbusiness = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(NatureOfBusiness.Text.Trim()) + "'" : "NULL";

                    trans = myHKeInvestData.beginTransaction();
                    sql = "INSERT INTO Client (FirstName, LastName, DateOfBirth, Email, HKIDPassportNumber, AccountNumber, ClientTitle, HomePhone,HomeFax, BusinessPhone, MobilePhone, CountryOfCitizenship, CountryOfResidence, PassportCountry, Building, Street, District, EmploymentStatus, SpecificOccupation, YearsWithEmployer, EmployerPhone, NatureOfBusiness, HiredByFinancial,PublicTradedCompany, EmployerName,IsPrimaryHolder) VALUES ('"
                        + myHKeInvestCode.checkSQLqueryForReservePun(FirstName.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(LastName.Text.Trim()) + "',CONVERT(date,'" + DateOfBirth.Text.Trim() + "',103), '" + myHKeInvestCode.checkSQLqueryForReservePun(Email.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(HKID.Text.Trim()) + "','"
                        + acctNum + "','" + PClientTitle.SelectedItem.Value.Trim() + "'," + homePhone + "," + homeFax + "," + busiPhone + "," + mobile + ",'" + myHKeInvestCode.checkSQLqueryForReservePun(CountryOfCitizenship.Text.Trim()) + "','"
                        + myHKeInvestCode.checkSQLqueryForReservePun(CountryOfLegalResidence.Text.Trim()) + "'," + passportcountry + "," + building + ",'" + myHKeInvestCode.checkSQLqueryForReservePun(Street.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(District.Text.Trim()) + "','"
                        + EmploymentStatus.SelectedValue.Trim() + "'," + specificoccupation + "," + yearswithemployer + "," + employerphone + ","
                        + natureofbusiness + ",'" + EmployedFinancialInstitution.SelectedValue.Trim() + "','" + DirectorOfTradedCompany.SelectedValue.Trim() + "',"
                        + employername + ",'" + "1" + "')";

                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);
                }
                else
                {
                    //For primary account holder

                    string building = (string.IsNullOrEmpty(Building.Text.Trim())) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(Building.Text.Trim()) + "'";
                    string homePhone = (string.IsNullOrEmpty(HomePhone.Text.Trim())) ? "NULL" : "'" + HomePhone.Text.Trim() + "'";
                    string homeFax = (string.IsNullOrEmpty(HomeFax.Text.Trim())) ? "NULL" : "'" + HomeFax.Text.Trim() + "'";
                    string busiPhone = (string.IsNullOrEmpty(BusinessPhone.Text.Trim())) ? "NULL" : "'" + BusinessPhone.Text.Trim() + "'";
                    string mobile = (string.IsNullOrEmpty(MobilePhone.Text.Trim())) ? "NULL" : "'" + MobilePhone.Text.Trim() + "'";

                    //Check whether the Client use HKID or Passport
                    string passportcountry = (HKIDorPassport.Items[0].Selected) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(PassportCountryOfIssue.Text.Trim()) + "'";

                    //Check whether the Client is employed
                    string specificoccupation = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(SpecificOccupation.Text.Trim()) + "'" : "NULL";
                    string yearswithemployer = (EmploymentStatus.Items[0].Selected) ? "'" + YearsWithEmployer.Text.Trim() + "'" : "NULL";
                    string employername = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(EmployerName.Text.Trim()) + "'" : "NULL";
                    string employerphone = (EmploymentStatus.Items[0].Selected) ? "'" + EmployerPhone.Text.Trim() + "'" : "NULL";
                    string natureofbusiness = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(NatureOfBusiness.Text.Trim()) + "'" : "NULL";

                    trans = myHKeInvestData.beginTransaction();
                    sql = "INSERT INTO Client (FirstName, LastName, DateOfBirth, Email, HKIDPassportNumber, AccountNumber, ClientTitle, HomePhone,HomeFax, BusinessPhone, MobilePhone, CountryOfCitizenship, CountryOfResidence, PassportCountry, Building, Street, District, EmploymentStatus, SpecificOccupation, YearsWithEmployer, EmployerPhone, NatureOfBusiness, HiredByFinancial,PublicTradedCompany, EmployerName,IsPrimaryHolder) VALUES ('"
                        + myHKeInvestCode.checkSQLqueryForReservePun(FirstName.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(LastName.Text.Trim()) + "',CONVERT(date,'" + DateOfBirth.Text.Trim() + "',103), '" + myHKeInvestCode.checkSQLqueryForReservePun(Email.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(HKID.Text.Trim()) + "','"
                        + acctNum + "','" + PClientTitle.SelectedItem.Value.Trim() + "'," + homePhone + "," + homeFax + "," + busiPhone + "," + mobile + ",'" + myHKeInvestCode.checkSQLqueryForReservePun(CountryOfCitizenship.Text.Trim()) + "','"
                        + myHKeInvestCode.checkSQLqueryForReservePun(CountryOfLegalResidence.Text.Trim()) + "'," + passportcountry + "," + building + ",'" + myHKeInvestCode.checkSQLqueryForReservePun(Street.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(District.Text.Trim()) + "','"
                        + EmploymentStatus.SelectedValue.Trim() + "'," + specificoccupation + "," + yearswithemployer + "," + employerphone + ","
                        + natureofbusiness + ",'" + EmployedFinancialInstitution.SelectedValue.Trim() + "','" + DirectorOfTradedCompany.SelectedValue.Trim() + "',"
                        + employername + ",'" + "1" + "')";

                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);


                    //For Co-Account holder

                    string cbuilding = (string.IsNullOrEmpty(CBuilding.Text.Trim())) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(CBuilding.Text.Trim()) + "'";
                    string chomePhone = (string.IsNullOrEmpty(CHomePhone.Text.Trim())) ? "NULL" : "'" + CHomePhone.Text.Trim() + "'";
                    string chomeFax = (string.IsNullOrEmpty(CHomeFax.Text.Trim())) ? "NULL" : "'" + CHomeFax.Text.Trim() + "'";
                    string cbusiPhone = (string.IsNullOrEmpty(CBusinessPhone.Text.Trim())) ? "NULL" : "'" + CBusinessPhone.Text.Trim() + "'";
                    string cmobile = (string.IsNullOrEmpty(CMobilePhone.Text.Trim())) ? "NULL" : "'" + CMobilePhone.Text.Trim() + "'";

                    //Check whether the Client use HKID or Passport
                    string cpassportcountry = (CHKIDorPassport.Items[0].Selected) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(CPassportCountryOfIssue.Text.Trim()) + "'";

                    //Check whether the Client is employed
                    string cspecificoccupation = (CEmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(CSpecificOccupation.Text.Trim()) + "'" : "NULL";
                    string cyearswithemployer = (CEmploymentStatus.Items[0].Selected) ? "'" + CYearsWithEmployer.Text.Trim() + "'" : "NULL";
                    string cemployername = (CEmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(CEmployerName.Text.Trim()) + "'" : "NULL";
                    string cemployerphone = (CEmploymentStatus.Items[0].Selected) ? "'" + CEmployerPhone.Text.Trim() + "'" : "NULL";
                    string cnatureofbusiness = (CEmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(CNatureOfBusiness.Text.Trim()) + "'" : "NULL";

                    trans = myHKeInvestData.beginTransaction();
                    sql = "INSERT INTO Client (FirstName, LastName, DateOfBirth, Email, HKIDPassportNumber, AccountNumber, ClientTitle, HomePhone,HomeFax, BusinessPhone, MobilePhone, CountryOfCitizenship, CountryOfResidence, PassportCountry, Building, Street, District, EmploymentStatus, SpecificOccupation, YearsWithEmployer, EmployerPhone, NatureOfBusiness, HiredByFinancial,PublicTradedCompany, EmployerName,IsPrimaryHolder) VALUES ('"
                        + myHKeInvestCode.checkSQLqueryForReservePun(CFirstName.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(CLastName.Text.Trim()) + "',CONVERT(date,'" + CDateOfBirth.Text.Trim() + "',103), '" + myHKeInvestCode.checkSQLqueryForReservePun(CEmail.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(CHKID.Text.Trim()) + "','"
                        + acctNum + "','" + CClientTitle.SelectedItem.Value.Trim() + "'," + chomePhone + "," + chomeFax + "," + cbusiPhone + "," + cmobile + ",'" + myHKeInvestCode.checkSQLqueryForReservePun(CCountryOfCitizenship.Text.Trim()) + "','"
                        + myHKeInvestCode.checkSQLqueryForReservePun(CCountryOfLegalResidence.Text.Trim()) + "'," + cpassportcountry + "," + cbuilding + ",'" + myHKeInvestCode.checkSQLqueryForReservePun(CStreet.Text.Trim()) + "','" + myHKeInvestCode.checkSQLqueryForReservePun(CDistrict.Text.Trim()) + "','"
                        + CEmploymentStatus.SelectedValue.Trim() + "'," + cspecificoccupation + "," + cyearswithemployer + "," + cemployerphone + ","
                        + cnatureofbusiness + ",'" + CEmployedFinancialInstitution.SelectedValue.Trim() + "','" + CDirectorOfTradedCompany.SelectedValue.Trim() + "',"
                        + cemployername + ",'" + "0" + "')";

                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);
                }
                Label40.Text = "Account Created! Account number is: " + acctNum;
                errormsg.Visible = false;
                successmsg.Visible = true;
            }
            else
            {
                Label41.Text = "Please double check the information you provide.";
                errormsg.Visible = true;
                successmsg.Visible = false;
            }
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (InitialDeposit.Items[0].Selected)
            {
                args.IsValid = true;
            }
            else if (InitialDeposit.Items[1].Selected)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(!(InitialDeposit.Items[0].Selected || InitialDeposit.Items[1].Selected))
            {
                args.IsValid = true;
            }
            else if(InitialDeposit.Items[0].Selected&&InitialDeposit.Items[1].Selected)
            {
                if(string.IsNullOrEmpty(Cheque.Text.Trim())||string.IsNullOrEmpty(Transfer.Text.Trim()))
                {
                    args.IsValid = true;
                }
                else if (decimal.Parse(Cheque.Text.Trim())+decimal.Parse(Transfer.Text.Trim())<20000)
                {
                    args.IsValid = false;
                    CustomValidator4.ErrorMessage = "A HK$20,000 minimum deposit is required to open an account. ";
                }        
            }
            else if(InitialDeposit.Items[0].Selected)
            {
                if (string.IsNullOrEmpty(Cheque.Text.Trim()))
                {
                    args.IsValid = true;
                }
                else if (decimal.Parse(Cheque.Text.Trim())<20000)
                {
                    args.IsValid = false;
                    CustomValidator4.ErrorMessage = "A HK$20,000 minimum deposit is required to open an account. ";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Transfer.Text.Trim()))
                {
                    args.IsValid = true;
                }
                else if (decimal.Parse(Transfer.Text.Trim())<20000)
                {
                    args.IsValid = false;
                    CustomValidator4.ErrorMessage = "A HK$20,000 minimum deposit is required to open an account. ";
                }
            }

        }

        protected void HKIDorPassport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(HKIDorPassport.Items[0].Selected)
            {
                passportcountrydiv.Visible = false;
                passportcountrydiv.Disabled = true;
            }
            else
            {
                passportcountrydiv.Visible = true;
                passportcountrydiv.Disabled = false;
            }
        }

        protected void CHKIDorPassport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CHKIDorPassport.Items[0].Selected)
            {
                cpassportcountrydiv.Visible = false;
                cpassportcountrydiv.Disabled = true;
            }
            else
            {
                cpassportcountrydiv.Visible = true;
                cpassportcountrydiv.Disabled = false;
            }
        }

        protected void CustomValidator5_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(HKID.Text.Trim().Equals(CHKID.Text.Trim()))
            {
                args.IsValid = false;
                CustomValidator5.ErrorMessage = "The primary account holder could not be co-account holder.";
            }
        }
    }
}
