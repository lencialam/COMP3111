using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Data;
using HKeInvestWebApplication.Code_File;
using System.Data.SqlClient;


namespace HKeInvestWebApplicationLencia
{
    public partial class ManageClientInfo : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Hide the modificable area by default
                modifyInfoDiv.Visible = false;
                modifyInfoDiv.Disabled = true;
                msgDiv.Visible = false;
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

        protected void Check_Click(object sender, EventArgs e)
        {
            //common
            string accountNumber = "";
            string accountType = "";
            string sourceOfFundstr = "";
            int sourceOfFundIndex = 0;
            string investmentObjectivestr = "";
            int investmentObjectiveIndex = 0;
            string investmentKnowledgestr = "";
            int investmentKnowledgeIndex = 0;
            string investmentExperiencestr = "";
            int investmentExperienceIndex = 0;
            string annualIncomestr = "";
            int annualIncomeIndex = 0;
            string liquiditystr = "";
            int liquidityIndex = 0;
            int earnOnBalance = 0;

            //initializing for primary holder
            string dateOfBirth = "";
            string hkid = "";
            string firstName = "";
            string lastName = "";
            string countryOfCitizenship = "";
            string countryOfResidence = "";
            string passportCountry = "";

            string emailstr = "";
            string clientTitlestr = "";
            int clientTitleIndex = 0;
            string homePhonestr = "";
            string homeFaxstr = "";
            string businessPhonestr = "";
            string mobilePhonestr = "";
            string employmentStatusstr = "";
            int employmentStatusIndex = 0;
            string specificOccupationstr = "";
            string yearsWithEmployerstr = "";
            string employerPhonestr = "";
            string natureOfBusinessstr = "";
            int hiredByFinancialstr = 0;
            int publicTradedCompanystr = 0;
            string employerNamestr = "";
            string buildingstr = "";
            string streetstr = "";
            string districtstr = "";

            //initializing for coholder
            string codateOfBirth = "";
            string cohkid = "";
            string cofirstName = "";
            string colastName = "";
            string cocountryOfCitizenship = "";
            string cocountryOfResidence = "";
            string copassportCountry = "";

            string coemailstr = "";
            string coclientTitlestr = "";
            int coclientTitleIndex = 0;
            string cohomePhonestr = "";
            string cohomeFaxstr = "";
            string cobusinessPhonestr = "";
            string comobilePhonestr = "";
            string coemploymentStatusstr = "";
            int coemploymentStatusIndex = 0;
            string cospecificOccupationstr = "";
            string coyearsWithEmployerstr = "";
            string coemployerPhonestr = "";
            string conatureOfBusinessstr = "";
            int cohiredByFinancialstr = 0;
            int copublicTradedCompanystr = 0;
            string coemployerNamestr = "";
            string cobuildingstr = "";
            string costreetstr = "";
            string codistrictstr = "";

            accountNumber = AccountNumberBox.Text.Trim();
            ViewState["accountNumber"] = accountNumber;

            HKeInvestData myHKeInvestData = new HKeInvestData();

            string sql = "select count(*) from Account where accountNumber='" + accountNumber + "'";
            decimal d = myHKeInvestData.getAggregateValue(sql);
            bool isValid = true;
            if (d == 0 || d == -1)
            {
                isValid = false;

                //Hide the modificable area and show the search error message
                modifyInfoDiv.Visible = false;
                modifyInfoDiv.Disabled = true;
                msgDiv.Visible = true;
                SearchError.Text = "There is no such an account in the system.";
            }
            if (isValid)
            {
                //Show the modificable area and hid the search error message
                modifyInfoDiv.Visible = true;
                modifyInfoDiv.Disabled = false;
                msgDiv.Visible = false;

                sql = "select * from Client, Account where Client.accountNumber = Account.accountNumber AND Account.accountNumber = '" + accountNumber + "' AND IsPrimaryHolder = 'true'";
                DataTable dtInformation = myHKeInvestData.getData(sql);

                //things can't be changed by client
                accountType = dtInformation.Rows[0]["accountType"].ToString().Trim();

                if (accountType == "individual")
                {
                    ClientinfoCClientDiv.Disabled = true;
                    ClientinfoCClientDiv.Visible = false;
                    EmployinfoCClientDiv.Disabled = true;
                    EmployinfoCClientDiv.Visible = false;
                    DisclosureCClientDiv.Disabled = true;
                    DisclosureCClientDiv.Visible = false;
                }
                dateOfBirth = Convert.ToDateTime(dtInformation.Rows[0]["dateOfBirth"]).ToString("dd/MM/yyyy");
                //dateOfBirth = dtInformation.Rows[0]["dateOfBirth"].ToShortDateString().ToString();
                hkid = dtInformation.Rows[0]["HKIDPassportNumber"].ToString().Trim();
                firstName = dtInformation.Rows[0]["firstName"].ToString().Trim();
                lastName = dtInformation.Rows[0]["lastName"].ToString().Trim();
                countryOfCitizenship = dtInformation.Rows[0]["countryOfCitizenship"].ToString().Trim();
                countryOfResidence = dtInformation.Rows[0]["countryOfResidence"].ToString().Trim();
                passportCountry = dtInformation.Rows[0]["passportCountry"].ToString().Trim();
                //For showing the user name
                if(String.IsNullOrEmpty(dtInformation.Rows[0]["userName"].ToString().Trim()))
                {
                    UserNameLabel.Text = "Not registered";
                    UserNameNullErrorDiv.Visible = true;
                }
                else
                {
                    UserNameLabel.Text = dtInformation.Rows[0]["userName"].ToString().Trim();
                    UserNameNullErrorDiv.Visible = false;
                }

                if (passportCountry == "")
                {
                    HKIDLabelDiv.Disabled = false;
                    HKIDLabelDiv.Visible = true;
                    PassportNumberDiv.Disabled = true;
                    PassportNumberDiv.Visible = false;
                }
                else
                {
                    HKIDLabelDiv.Disabled = true;
                    HKIDLabelDiv.Visible = false;
                    PassportNumberDiv.Disabled = false;
                    PassportNumberDiv.Visible = true;
                }


                //things can be changed
                emailstr = dtInformation.Rows[0]["email"].ToString().Trim();
                clientTitlestr = dtInformation.Rows[0]["clientTitle"].ToString().Trim();
                if (clientTitlestr == "mrs")
                {
                    clientTitleIndex = 1;
                }
                else if (clientTitlestr == "ms")
                {
                    clientTitleIndex = 2;
                }
                else if (clientTitlestr == "dr")
                {
                    clientTitleIndex = 3;
                }

                homePhonestr = dtInformation.Rows[0]["homePhone"].ToString().Trim();
                homeFaxstr = dtInformation.Rows[0]["homeFax"].ToString().Trim();
                businessPhonestr = dtInformation.Rows[0]["businessPhone"].ToString().Trim();
                mobilePhonestr = dtInformation.Rows[0]["mobilePhone"].ToString().Trim();

                employmentStatusstr = dtInformation.Rows[0]["EmploymentStatus"].ToString().Trim();
                PClientEmploydetailDiv.Disabled = true;
                PClientEmploydetailDiv.Visible = false;
                if (employmentStatusstr == "selfemployed")
                {
                    employmentStatusIndex = 1;
                }
                else if (employmentStatusstr == "retired")
                {
                    employmentStatusIndex = 2;
                }
                else if (employmentStatusstr == "student")
                {
                    employmentStatusIndex = 3;
                }
                else if (employmentStatusstr == "notemployed")
                {
                    employmentStatusIndex = 4;
                }
                else if (employmentStatusstr == "homemaker")
                {
                    employmentStatusIndex = 5;
                }
                else
                {
                    PClientEmploydetailDiv.Disabled = false;
                    PClientEmploydetailDiv.Visible = true;
                }
                specificOccupationstr = dtInformation.Rows[0]["specificOccupation"].ToString().Trim();
                yearsWithEmployerstr = dtInformation.Rows[0]["yearsWithEmployer"].ToString().Trim();
                employerPhonestr = dtInformation.Rows[0]["employerPhone"].ToString().Trim();
                natureOfBusinessstr = dtInformation.Rows[0]["natureOfBusiness"].ToString().Trim();
                if (Convert.ToBoolean(dtInformation.Rows[0]["HiredByFinancial"]) == true)
                {
                    hiredByFinancialstr = 1;
                }
                if (Convert.ToBoolean(dtInformation.Rows[0]["PublicTradedCompany"]) == true)
                {
                    publicTradedCompanystr = 1;
                }
                employerNamestr = dtInformation.Rows[0]["employerName"].ToString().Trim();
                buildingstr = dtInformation.Rows[0]["building"].ToString().Trim();
                streetstr = dtInformation.Rows[0]["street"].ToString().Trim();
                districtstr = dtInformation.Rows[0]["district"].ToString().Trim();

                investmentObjectivestr = dtInformation.Rows[0]["investmentObjective"].ToString().Trim();
                if (investmentObjectivestr == "income")
                {
                    investmentObjectiveIndex = 1;
                }
                else if (investmentObjectivestr == "growth")
                {
                    investmentObjectiveIndex = 2;
                }
                else if (investmentObjectivestr == "speculation")
                {
                    investmentObjectiveIndex = 3;
                }

                investmentKnowledgestr = dtInformation.Rows[0]["investmentKnowledge"].ToString().Trim();
                if (investmentKnowledgestr == "limited")
                {
                    investmentKnowledgeIndex = 1;
                }
                else if (investmentKnowledgestr == "good")
                {
                    investmentKnowledgeIndex = 2;
                }
                else if (investmentKnowledgestr == "extensive")
                {
                    investmentKnowledgeIndex = 3;
                }

                annualIncomestr = dtInformation.Rows[0]["annualIncome"].ToString().Trim();
                if (annualIncomestr == "20001")
                {
                    annualIncomeIndex = 1;
                }
                else if (annualIncomestr == "200001")
                {
                    annualIncomeIndex = 2;
                }
                else if (annualIncomestr == "2000001")
                {
                    annualIncomeIndex = 3;
                }

                liquiditystr = dtInformation.Rows[0]["Liquidity"].ToString().Trim();
                if (liquiditystr == "100001")
                {
                    liquidityIndex = 1;
                }
                else if (liquiditystr == "1000001")
                {
                    liquidityIndex = 2;
                }
                else if (liquiditystr == "10000001")
                {
                    liquidityIndex = 3;
                }


                if (Convert.ToBoolean(dtInformation.Rows[0]["EarnOnBalance"]) == true)
                {
                    earnOnBalance = 1;
                }
                investmentExperiencestr = dtInformation.Rows[0]["investmentExperience"].ToString().Trim();
                if (investmentExperiencestr == "limited")
                {
                    investmentExperienceIndex = 1;
                }
                else if (investmentExperiencestr == "good")
                {
                    investmentExperienceIndex = 2;
                }
                else if (investmentExperiencestr == "extensive")
                {
                    investmentExperienceIndex = 3;
                }

                sourceOfFundstr = dtInformation.Rows[0]["SourceOfFund"].ToString().Trim();
                if (sourceOfFundstr == "salary")
                {
                    sourceOfFundIndex = 0;
                    DescribeOtherFundDiv.Disabled = true;
                    DescribeOtherFundDiv.Visible = false;
                }
                else if (sourceOfFundstr == "investment")
                {
                    sourceOfFundIndex = 1;
                    DescribeOtherFundDiv.Disabled = true;
                    DescribeOtherFundDiv.Visible = false;
                }
                else if (sourceOfFundstr == "family")
                {
                    sourceOfFundIndex = 2;
                    DescribeOtherFundDiv.Disabled = true;
                    DescribeOtherFundDiv.Visible = false;
                }
                else
                {
                    sourceOfFundIndex = 3;
                    DescribeOtherFundDiv.Disabled = false;
                    DescribeOtherFundDiv.Visible = true;
                    DescribeOtherFund.Text = sourceOfFundstr;
                }


                if (accountType != "individual")
                {

                    //Initialize panels
                    ClientinfoCClientDiv.Disabled = false;
                    ClientinfoCClientDiv.Visible = true;
                    EmployinfoCClientDiv.Disabled = false;
                    EmployinfoCClientDiv.Visible = true;
                    DisclosureCClientDiv.Disabled = false;
                    DisclosureCClientDiv.Visible = true;

                    sql = "select count(*) from Client where accountNumber = '" + accountNumber + "' AND IsPrimaryHolder = 'false'";
                    decimal cod = myHKeInvestData.getAggregateValue(sql);
                    bool coisValid = true;
                    if (cod == 0 || cod == -1)
                    {
                        coisValid = false;
                    }
                    if (coisValid)
                    {
                        sql = "select * from Client, Account where Client.accountNumber = Account.accountNumber AND Account.accountNumber = '" + accountNumber + "' AND IsPrimaryHolder = 'false'";
                        DataTable dtCoInformation = myHKeInvestData.getData(sql);

                        //things can't be changed by client
                        codateOfBirth = Convert.ToDateTime(dtCoInformation.Rows[0]["dateOfBirth"]).ToString("dd/MM/yyyy");
                        cohkid = dtCoInformation.Rows[0]["HKIDPassportNumber"].ToString().Trim();
                        cofirstName = dtCoInformation.Rows[0]["firstName"].ToString().Trim();
                        colastName = dtCoInformation.Rows[0]["lastName"].ToString().Trim();
                        cocountryOfCitizenship = dtCoInformation.Rows[0]["countryOfCitizenship"].ToString().Trim();
                        cocountryOfResidence = dtCoInformation.Rows[0]["countryOfResidence"].ToString().Trim();
                        copassportCountry = dtCoInformation.Rows[0]["passportCountry"].ToString().Trim();
                        if (copassportCountry == "")
                        {
                            CHKIDDiv.Disabled = false;
                            CHKIDDiv.Visible = true;
                            CPassportNumberDiv.Disabled = true;
                            CPassportNumberDiv.Visible = false;
                        }
                        else
                        {
                            CHKIDDiv.Disabled = true;
                            CHKIDDiv.Visible = false;
                            CPassportNumberDiv.Disabled = false;
                            CPassportNumberDiv.Visible = true;
                        }

                        //things can be changed
                        coemailstr = dtCoInformation.Rows[0]["email"].ToString().Trim();
                        coclientTitlestr = dtCoInformation.Rows[0]["clientTitle"].ToString().Trim();
                        if (coclientTitlestr == "mrs")
                        {
                            coclientTitleIndex = 1;
                        }
                        else if (coclientTitlestr == "ms")
                        {
                            coclientTitleIndex = 2;
                        }
                        else if (coclientTitlestr == "dr")
                        {
                            coclientTitleIndex = 3;
                        }

                        cohomePhonestr = dtCoInformation.Rows[0]["homePhone"].ToString().Trim();
                        cohomeFaxstr = dtCoInformation.Rows[0]["homeFax"].ToString().Trim();
                        cobusinessPhonestr = dtCoInformation.Rows[0]["businessPhone"].ToString().Trim();
                        comobilePhonestr = dtCoInformation.Rows[0]["mobilePhone"].ToString().Trim();

                        coemploymentStatusstr = dtCoInformation.Rows[0]["EmploymentStatus"].ToString().Trim();
                        CClientEmploydetailDiv.Disabled = true;
                        CClientEmploydetailDiv.Visible = false;
                        if (coemploymentStatusstr == "selfemployed")
                        {
                            coemploymentStatusIndex = 1;
                        }
                        else if (coemploymentStatusstr == "retired")
                        {
                            coemploymentStatusIndex = 2;
                        }
                        else if (coemploymentStatusstr == "student")
                        {
                            coemploymentStatusIndex = 3;
                        }
                        else if (coemploymentStatusstr == "notemployed")
                        {
                            coemploymentStatusIndex = 4;
                        }
                        else if (coemploymentStatusstr == "homemaker")
                        {
                            coemploymentStatusIndex = 5;
                        }
                        else
                        {
                            CClientEmploydetailDiv.Disabled = false;
                            CClientEmploydetailDiv.Visible = true;
                        }
                        cospecificOccupationstr = dtCoInformation.Rows[0]["specificOccupation"].ToString().Trim();
                        coyearsWithEmployerstr = dtCoInformation.Rows[0]["yearsWithEmployer"].ToString().Trim();
                        coemployerPhonestr = dtCoInformation.Rows[0]["employerPhone"].ToString().Trim();
                        conatureOfBusinessstr = dtCoInformation.Rows[0]["natureOfBusiness"].ToString().Trim();
                        if (Convert.ToBoolean(dtCoInformation.Rows[0]["HiredByFinancial"]) == true)
                        {
                            cohiredByFinancialstr = 1;
                        }
                        if (Convert.ToBoolean(dtCoInformation.Rows[0]["PublicTradedCompany"]) == true)
                        {
                            copublicTradedCompanystr = 1;
                        }
                        coemployerNamestr = dtCoInformation.Rows[0]["employerName"].ToString().Trim();
                        cobuildingstr = dtCoInformation.Rows[0]["building"].ToString().Trim();
                        costreetstr = dtCoInformation.Rows[0]["street"].ToString().Trim();
                        codistrictstr = dtCoInformation.Rows[0]["district"].ToString().Trim();
                    }
                }



                //accountNumber = dtInformation.Rows[0]["accountNumber"].ToString().Trim();
                if (accountType == "individual")
                {
                    accountTypeLabel.Text = "Individual";
                }
                else if(accountType=="common")
                {
                    accountTypeLabel.Text = "Joint Tenants in Common";
                }
                else
                {
                    accountTypeLabel.Text = "Joint Tenants with Rights of Survivorship";
                }
                InvestmentObjective.Items[investmentObjectiveIndex].Selected = true;
                InvestmentKnowledge.Items[investmentKnowledgeIndex].Selected = true;
                AnnualIncome.Items[annualIncomeIndex].Selected = true;
                LiquidNetWorth.Items[liquidityIndex].Selected = true;
                SweepFreeCreditBalance.Items[earnOnBalance].Selected = true;
                InvestmentExperience.Items[investmentExperienceIndex].Selected = true;
                SourceOfFund.Items[sourceOfFundIndex].Selected = true;

                //For Primary Holder, things that can't be changed
                DateOfBirthLabel.Text = dateOfBirth;
                HKIDLabel.Text = hkid;
                PassportNumber.Text = hkid;
                FirstName.Text = firstName;
                LastName.Text = lastName;
                CountryOfCitizenship.Text = countryOfCitizenship;
                CountryOfResidence.Text = countryOfResidence;
                PassportCountryOfIssue.Text = passportCountry;

                //For Primary Holder, things that can be changed
                Email.Text = emailstr;
                PClientTitle.Items[clientTitleIndex].Selected = true;
                HomePhone.Text = homePhonestr;
                HomeFax.Text = homeFaxstr;
                BusinessPhone.Text = businessPhonestr;
                MobilePhone.Text = mobilePhonestr;
                EmploymentStatus.Items[employmentStatusIndex].Selected = true;
                SpecificOccupation.Text = specificOccupationstr;
                YearsWithEmployer.Text = yearsWithEmployerstr;
                EmployerPhone.Text = employerPhonestr;
                NatureOfBusiness.Text = natureOfBusinessstr;
                EmployedFinancialInstitution.Items[hiredByFinancialstr].Selected = true;
                DirectorOfTradedCompany.Items[publicTradedCompanystr].Selected = true;
                EmployerName.Text = employerNamestr;
                Building.Text = buildingstr;
                Street.Text = streetstr;
                District.Text = districtstr;

                if (accountType != "individual")
                {
                    //For Co Holder, things that can't be changed
                    CDateOfBirthLabel.Text = codateOfBirth;
                    CHKIDLabel.Text = cohkid;
                    CPassportNumber.Text = cohkid;
                    CFirstName.Text = cofirstName;
                    CLastName.Text = colastName;
                    CCountryOfCitizenship.Text = cocountryOfCitizenship;
                    CCountryOfResidence.Text = cocountryOfResidence;
                    CPassportCountryOfIssue.Text = copassportCountry;

                    //For Co Holder, things that can be changed
                    CEmail.Text = coemailstr;
                    CClientTitle.Items[coclientTitleIndex].Selected = true;
                    CHomePhone.Text = cohomePhonestr;
                    CHomeFax.Text = cohomeFaxstr;
                    CBusinessPhone.Text = cobusinessPhonestr;
                    CMobilePhone.Text = comobilePhonestr;
                    CEmploymentStatus.Items[coemploymentStatusIndex].Selected = true;
                    CSpecificOccupation.Text = cospecificOccupationstr;
                    CYearsWithEmployer.Text = coyearsWithEmployerstr;
                    CEmployerPhone.Text = coemployerPhonestr;
                    CNatureOfBusiness.Text = conatureOfBusinessstr;
                    CEmployedFinancialInstitution.Items[cohiredByFinancialstr].Selected = true;
                    CDirectorOfTradedCompany.Items[copublicTradedCompanystr].Selected = true;
                    CEmployerName.Text = coemployerNamestr;
                    CBuilding.Text = cobuildingstr;
                    CStreet.Text = costreetstr;
                    CDistrict.Text = codistrictstr;
                }
                //Set the error message and the successful message hidden
                errormsg.Visible = false;
                successmsg.Visible = false;
        }
    }

        protected void Apply_Click(object sender, EventArgs e)
        {
            string accountNumber = ViewState["accountNumber"].ToString();
            HKeInvestData myHKeInvestData = new HKeInvestData();
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();
            string sql = "";
            sql = "SELECT accountType FROM Account WHERE accountNumber = '" + accountNumber + "'";
            DataTable dtInformation = myHKeInvestData.getData(sql);
            string accountType = dtInformation.Rows[0]["accountType"].ToString().Trim();

            if (Page.IsValid)
            {
                //Use SQL to create the account in the database
                string sourceoffund = (SourceOfFund.Items[3].Selected) ? myHKeInvestCode.checkSQLqueryForReservePun(DescribeOtherFund.Text.Trim()) : SourceOfFund.SelectedValue.Trim();
                SqlTransaction trans = myHKeInvestData.beginTransaction();
                sql = "UPDATE Account SET InvestmentObjective = '" + InvestmentObjective.SelectedValue.Trim() + "', InvestmentKnowledge = '"
                    + InvestmentKnowledge.SelectedValue.Trim() + "', AnnualIncome = '" + AnnualIncome.SelectedValue.Trim() + "',Liquidity = '"
                    + LiquidNetWorth.SelectedValue.Trim() + "',EarnOnBalance = '" + SweepFreeCreditBalance.SelectedValue.Trim() + "',InvestmentExperience = '"
                    + InvestmentExperience.SelectedValue.Trim() + "',SourceOfFund = '" + sourceoffund + "' WHERE accountNumber = '" + accountNumber + "'";
                myHKeInvestData.setData(sql, trans);
                myHKeInvestData.commitTransaction(trans);


                //Use SQL to create the client(s) in the database
                if (accountType == "individual")
                {
                    string building = (string.IsNullOrEmpty(Building.Text.Trim())) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(Building.Text.Trim()) + "'";
                    string homePhone = (string.IsNullOrEmpty(HomePhone.Text.Trim())) ? "NULL" : "'" + HomePhone.Text.Trim() + "'";
                    string homeFax = (string.IsNullOrEmpty(HomeFax.Text.Trim())) ? "NULL" : "'" + HomeFax.Text.Trim() + "'";
                    string busiPhone = (string.IsNullOrEmpty(BusinessPhone.Text.Trim())) ? "NULL" : "'" + BusinessPhone.Text.Trim() + "'";
                    string mobile = (string.IsNullOrEmpty(MobilePhone.Text.Trim())) ? "NULL" : "'" + MobilePhone.Text.Trim() + "'";

                    //Check whether the Client is employed
                    string specificoccupation = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(SpecificOccupation.Text.Trim()) + "'" : "NULL";
                    string yearswithemployer = (EmploymentStatus.Items[0].Selected) ? "'" + YearsWithEmployer.Text.Trim() + "'" : "NULL";
                    string employername = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(EmployerName.Text.Trim()) + "'" : "NULL";
                    string employerphone = (EmploymentStatus.Items[0].Selected) ? "'" + EmployerPhone.Text.Trim() + "'" : "NULL";
                    string natureofbusiness = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(NatureOfBusiness.Text.Trim()) + "'" : "NULL";

                    trans = myHKeInvestData.beginTransaction();
                    sql = "UPDATE Client SET firstName = '" + myHKeInvestCode.checkSQLqueryForReservePun(FirstName.Text.Trim()) + "', lastName = '" + myHKeInvestCode.checkSQLqueryForReservePun(LastName.Text.Trim()) 
                        + "', countryOfCitizenship = '" + myHKeInvestCode.checkSQLqueryForReservePun(CountryOfCitizenship.Text.Trim()) + "', countryOfResidence = '" 
                        + myHKeInvestCode.checkSQLqueryForReservePun(CountryOfResidence.Text.Trim()) + "', Email = '"
                        + myHKeInvestCode.checkSQLqueryForReservePun(Email.Text.Trim()) + "', ClientTitle = '" + PClientTitle.SelectedItem.Value.Trim()
                        + "', HomePhone = " + homePhone + ", HomeFax = " + homeFax + ", BusinessPhone = " + busiPhone + ", MobilePhone = "
                        + mobile + ", Building = " + building + ", Street = '" + myHKeInvestCode.checkSQLqueryForReservePun(Street.Text.Trim()) + "', District = '" 
                        + myHKeInvestCode.checkSQLqueryForReservePun(District.Text.Trim())
                        + "', EmploymentStatus = '" + EmploymentStatus.SelectedValue.Trim() + "', SpecificOccupation = " + specificoccupation
                        + ", YearsWithEmployer = " + yearswithemployer + ", EmployerPhone = " + employerphone + ", NatureOfBusiness = "
                        + natureofbusiness + ", HiredByFinancial = '" + EmployedFinancialInstitution.SelectedValue.Trim() + "',PublicTradedCompany = '"
                        + DirectorOfTradedCompany.SelectedValue.Trim() + "', EmployerName = " + employername + " WHERE accountNumber = '" + accountNumber
                        + "' AND IsPrimaryHolder = 'true'";

                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);

                    if (PassportNumberDiv.Disabled == false)
                    {
                        trans = myHKeInvestData.beginTransaction();
                        sql = "UPDATE Client SET HKIDPassportNumber = '" + PassportNumber.Text.Trim() + "', passportCountry = '" + PassportCountryOfIssue.Text.Trim()
                            + "' WHERE accountNumber = '" + accountNumber + "' AND IsPrimaryHolder = 'true'";
                        myHKeInvestData.setData(sql, trans);
                        myHKeInvestData.commitTransaction(trans);
                    }
                }
                else
                {
                    //For primary account holder
                    string building = (string.IsNullOrEmpty(Building.Text.Trim())) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(Building.Text.Trim()) + "'";
                    string homePhone = (string.IsNullOrEmpty(HomePhone.Text.Trim())) ? "NULL" : "'" + HomePhone.Text.Trim() + "'";
                    string homeFax = (string.IsNullOrEmpty(HomeFax.Text.Trim())) ? "NULL" : "'" + HomeFax.Text.Trim() + "'";
                    string busiPhone = (string.IsNullOrEmpty(BusinessPhone.Text.Trim())) ? "NULL" : "'" + BusinessPhone.Text.Trim() + "'";
                    string mobile = (string.IsNullOrEmpty(MobilePhone.Text.Trim())) ? "NULL" : "'" + MobilePhone.Text.Trim() + "'";

                    //Check whether the Client is employed
                    string specificoccupation = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(SpecificOccupation.Text.Trim()) + "'" : "NULL";
                    string yearswithemployer = (EmploymentStatus.Items[0].Selected) ? "'" + YearsWithEmployer.Text.Trim() + "'" : "NULL";
                    string employername = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(EmployerName.Text.Trim()) + "'" : "NULL";
                    string employerphone = (EmploymentStatus.Items[0].Selected) ? "'" + EmployerPhone.Text.Trim() + "'" : "NULL";
                    string natureofbusiness = (EmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(NatureOfBusiness.Text.Trim()) + "'" : "NULL";

                    //Check whether the Client is using passport


                    trans = myHKeInvestData.beginTransaction();
                    sql = "UPDATE Client SET firstName = '" + myHKeInvestCode.checkSQLqueryForReservePun(FirstName.Text.Trim()) + "', lastName = '" + myHKeInvestCode.checkSQLqueryForReservePun(LastName.Text.Trim()) 
                        + "', countryOfCitizenship = '"
                        + myHKeInvestCode.checkSQLqueryForReservePun(CountryOfCitizenship.Text.Trim()) + "', countryOfResidence = '" + myHKeInvestCode.checkSQLqueryForReservePun(CountryOfResidence.Text.Trim())
                        + "', Email = '" + myHKeInvestCode.checkSQLqueryForReservePun(Email.Text.Trim()) + "', ClientTitle = '" + PClientTitle.SelectedItem.Value.Trim()
                        + "', HomePhone = " + homePhone + ", HomeFax = " + homeFax + ", BusinessPhone = " + busiPhone + ", MobilePhone = "
                        + mobile + ", Building = " + building + ", Street = '" + myHKeInvestCode.checkSQLqueryForReservePun(Street.Text.Trim()) + "', District = '" + myHKeInvestCode.checkSQLqueryForReservePun(District.Text.Trim())
                        + "', EmploymentStatus = '" + EmploymentStatus.SelectedValue.Trim() + "', SpecificOccupation = " + specificoccupation
                        + ", YearsWithEmployer = " + yearswithemployer + ", EmployerPhone = " + employerphone + ", NatureOfBusiness = "
                        + natureofbusiness + ", HiredByFinancial = '" + EmployedFinancialInstitution.SelectedValue.Trim() + "',PublicTradedCompany = '"
                        + DirectorOfTradedCompany.SelectedValue.Trim() + "', EmployerName = " + employername + " WHERE accountNumber = '" + accountNumber
                        + "' AND IsPrimaryHolder = 'true'";

                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);

                    if (PassportNumberDiv.Disabled == false)
                    {
                        trans = myHKeInvestData.beginTransaction();
                        sql = "UPDATE Client SET HKIDPassportNumber = '" + PassportNumber.Text.Trim() + "', passportCountry = '" + PassportCountryOfIssue.Text.Trim()
                            + "' WHERE accountNumber = '" + accountNumber + "' AND IsPrimaryHolder = 'true'";
                        myHKeInvestData.setData(sql, trans);
                        myHKeInvestData.commitTransaction(trans);
                    }

                    //For Co-Account holder

                    string cbuilding = (string.IsNullOrEmpty(CBuilding.Text.Trim())) ? "NULL" : "'" + myHKeInvestCode.checkSQLqueryForReservePun(CBuilding.Text.Trim()) + "'";
                    string chomePhone = (string.IsNullOrEmpty(CHomePhone.Text.Trim())) ? "NULL" : "'" + CHomePhone.Text.Trim() + "'";
                    string chomeFax = (string.IsNullOrEmpty(CHomeFax.Text.Trim())) ? "NULL" : "'" + CHomeFax.Text.Trim() + "'";
                    string cbusiPhone = (string.IsNullOrEmpty(CBusinessPhone.Text.Trim())) ? "NULL" : "'" + CBusinessPhone.Text.Trim() + "'";
                    string cmobile = (string.IsNullOrEmpty(CMobilePhone.Text.Trim())) ? "NULL" : "'" + CMobilePhone.Text.Trim() + "'";

                    //Check whether the Client is employed
                    string cspecificoccupation = (CEmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(CSpecificOccupation.Text.Trim()) + "'" : "NULL";
                    string cyearswithemployer = (CEmploymentStatus.Items[0].Selected) ? "'" + CYearsWithEmployer.Text.Trim() + "'" : "NULL";
                    string cemployername = (CEmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(CEmployerName.Text.Trim()) + "'" : "NULL";
                    string cemployerphone = (CEmploymentStatus.Items[0].Selected) ? "'" + CEmployerPhone.Text.Trim() + "'" : "NULL";
                    string cnatureofbusiness = (CEmploymentStatus.Items[0].Selected) ? "'" + myHKeInvestCode.checkSQLqueryForReservePun(CNatureOfBusiness.Text.Trim()) + "'" : "NULL";

                    trans = myHKeInvestData.beginTransaction();
                    sql = "UPDATE Client SET firstName = '" + myHKeInvestCode.checkSQLqueryForReservePun(CFirstName.Text.Trim()) + "', lastName = '" + myHKeInvestCode.checkSQLqueryForReservePun(CLastName.Text.Trim()) + "', countryOfCitizenship = '"
                        + myHKeInvestCode.checkSQLqueryForReservePun(CCountryOfCitizenship.Text.Trim()) + "', countryOfResidence = '" + myHKeInvestCode.checkSQLqueryForReservePun(CCountryOfResidence.Text.Trim())
                        + "', Email = '" + myHKeInvestCode.checkSQLqueryForReservePun(CEmail.Text.Trim()) + "', ClientTitle = '" + CClientTitle.SelectedItem.Value.Trim()
                        + "', HomePhone = " + chomePhone + ",HomeFax = " + chomeFax + ", BusinessPhone = " + cbusiPhone + ", MobilePhone = "
                        + cmobile + ", Building = " + cbuilding + ", Street = '" + myHKeInvestCode.checkSQLqueryForReservePun(CStreet.Text.Trim()) + "', District = '" + myHKeInvestCode.checkSQLqueryForReservePun(CDistrict.Text.Trim())
                        + "', EmploymentStatus = '" + CEmploymentStatus.SelectedValue.Trim() + "', SpecificOccupation = " + cspecificoccupation
                        + ", YearsWithEmployer = " + cyearswithemployer + ", EmployerPhone = " + cemployerphone + ", NatureOfBusiness = "
                        + cnatureofbusiness + ", HiredByFinancial = '" + CEmployedFinancialInstitution.SelectedValue.Trim() + "',PublicTradedCompany = '"
                        + CDirectorOfTradedCompany.SelectedValue.Trim() + "', EmployerName = " + cemployername + " WHERE accountNumber = '" + accountNumber
                        + "' AND IsPrimaryHolder = 'false'";

                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);

                    if (CPassportNumberDiv.Disabled == false)
                    {
                        trans = myHKeInvestData.beginTransaction();
                        sql = "UPDATE Client SET HKIDPassportNumber = '" + CPassportNumber.Text.Trim() + "', passportCountry = '" + CPassportCountryOfIssue.Text.Trim()
                            + "' WHERE accountNumber = '" + accountNumber + "' AND IsPrimaryHolder = 'false'";
                        myHKeInvestData.setData(sql, trans);
                        myHKeInvestData.commitTransaction(trans);
                    }
                }
                Label40.Text = "Account Information Updated! Account number is: " + accountNumber;
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

    }
}