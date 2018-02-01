using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication
{
    public partial class SecuritySearching : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the available currencies to populate the DropDownList.
                DataTable dtCurrency = myExternalFunctions.getCurrencyData();
                foreach (DataRow row in dtCurrency.Rows)
                {
                    ddlCurrencyForList.Items.Add(row["currency"].ToString().Trim());
                    ddlCurrencyForIndividual.Items.Add(row["currency"].ToString().Trim());
                    string currency = row["currency"].ToString().Trim();
                    ViewState[currency] = row["rate"].ToString().Trim();
                }

                //Hide the div for inputing security code and security name
                SecurityNameDiv.Disabled = true;
                SecurityNameDiv.Visible = false;
                SecurityCodeDiv.Disabled = true;
                SecurityCodeDiv.Visible = false;

                //Hide the div for the individual security details
                IndividualSecurityDetailDiv.Disabled = true;
                IndividualSecurityDetailDiv.Visible = false;

                //Hide the div for the security list
                SecurityDetailDiv.Disabled = true;
                SecurityDetailDiv.Visible = false;

            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (SecurityType.SelectedIndex == 0)
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "You must select the Security Type.";
            }
        }

        protected void SearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if 'Type only' is selected
            if (SearchCriteria.SelectedIndex == 0)
            {
                SecurityNameDiv.Disabled = true;
                SecurityNameDiv.Visible = false;

                SecurityCodeDiv.Disabled = true;
                SecurityCodeDiv.Visible = false;
            }
            //if 'Type+name' is selected
            else if (SearchCriteria.SelectedIndex == 1)
            {
                SecurityNameDiv.Disabled = false;
                SecurityNameDiv.Visible = true;

                SecurityCodeDiv.Disabled = true;
                SecurityCodeDiv.Visible = false;
            }
            //if 'Type+code' is selected
            else if (SearchCriteria.SelectedIndex == 2)
            {
                SecurityNameDiv.Disabled = true;
                SecurityNameDiv.Visible = false;

                SecurityCodeDiv.Disabled = false;
                SecurityCodeDiv.Visible = true;
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                //Record the security type in view state
                ViewState["SecurityType"] = SecurityType.SelectedValue;
                ViewState["SecurityTypeIndex"] = SecurityType.SelectedIndex.ToString().Trim();

                //Hide the extra fields in the detail view the the stock for the currency convertion
                showHideHiddenFields(false);

                //Set the 'Convert to Currency' dropdown list to be default
                ddlCurrencyForIndividual.SelectedIndex = 0;
                ddlCurrencyForList.SelectedIndex = 0;

                //Set to hide the div for showing the individual security detail as well as the security list first
                IndividualSecurityDetailDiv.Visible = false;
                IndividualSecurityDetailDiv.Disabled = true;
                SecurityDetailDiv.Visible = false;
                SecurityDetailDiv.Disabled = true;

                //Set the column for value in convert currency to be invisible
                gvSecurityDetails.Columns[myHKeInvestCode.getColumnIndexByName(gvSecurityDetails, "convertValue")].Visible = false;

                //If search by type only or by type & (partial) name is selected
                if (SearchCriteria.SelectedIndex == 0 || SearchCriteria.SelectedIndex == 1)
                {
                    SecurityDetailDiv.Visible = true;
                    SecurityDetailDiv.Disabled = false;
                    SearchError.Visible = false;
                    DataTable dtSecurities;
                    if (SearchCriteria.SelectedIndex == 0)
                    {
                        dtSecurities = myExternalFunctions.getSecuritiesData(SecurityType.SelectedValue.Trim());
                    }
                    else
                    {
                        dtSecurities = myExternalFunctions.getSecuritiesByName(SecurityType.SelectedValue.Trim(),myHKeInvestCode.checkSQLqueryForReservePun(SecurityName.Text.Trim()));
                    }

                    //If no securities has been found
                    if (dtSecurities==null||dtSecurities.Rows.Count<1)
                    {
                        SearchError.Visible = true;
                        SearchError.Text = "There is no security that could meet this criteria in the database. Please double check the information you provide.";
                        SecurityDetailDiv.Visible = false;
                        SecurityDetailDiv.Disabled = true;
                        return;
                    }

                    /*
                    dtSecurities.Columns.Add("type", typeof(String));
                    int dtrows = 0;
                    
                    foreach(DataRow row in dtSecurities.Rows)
                    {
                        dtSecurities.Rows[dtrows]["type"] = SecurityType.SelectedValue.Trim();
                        dtrows++;
                    }
                    */

                    int baseColumnIndex = myHKeInvestCode.getColumnIndexByName(gvSecurityDetails, "base");
                    gvSecurityDetails.Columns[baseColumnIndex].Visible = true;
                    if (SecurityType.SelectedIndex == 1)
                    {
                        dtSecurities.Columns["close"].ColumnName = "price";
                        int priceColumnIndex = myHKeInvestCode.getColumnIndexByName(gvSecurityDetails, "price");
                        gvSecurityDetails.Columns[priceColumnIndex].HeaderText = "Most recent closing price(HKD)";
                        dtSecurities.Columns.Add("base", typeof(String));
                        int dtrows = 0;
                        foreach (DataRow row in dtSecurities.Rows)
                        {
                            dtSecurities.Rows[dtrows]["base"] = "HKD";
                            dtrows++;
                        }
                    }
                    else
                    {
                        int priceColumnIndex = myHKeInvestCode.getColumnIndexByName(gvSecurityDetails, "price");
                        gvSecurityDetails.Columns[priceColumnIndex].HeaderText = "Price in base currency";
                    }

                    gvSecurityDetails.DataSource = dtSecurities;
                    gvSecurityDetails.DataBind();
                    SearchError.Visible = false;
                    if (SecurityType.SelectedIndex == 1)
                    {
                        gvSecurityDetails.Columns[baseColumnIndex].Visible = false;
                    }
                    dtSecurities.Columns.Add("convertValue", typeof(String));
                    Session["SecurityList"] = dtSecurities;
                }

                //If search an individual security is chosen
                else if (SearchCriteria.SelectedIndex == 2)
                {
                    IndividualSecurityDetailDiv.Visible = true;
                    IndividualSecurityDetailDiv.Disabled = false;
                    SearchError.Visible = false;
                    DataTable dtSecurity = myExternalFunctions.getSecuritiesByCode(SecurityType.SelectedValue.Trim(), SecurityCode.Text.Trim());
                    if (dtSecurity == null||dtSecurity.Rows.Count<1)
                    {
                        SearchError.Visible = true;
                        SearchError.Text = "There is no security that could meet this criteria in the database. Please double check the information you provide.";
                        IndividualSecurityDetailDiv.Visible = false;
                        IndividualSecurityDetailDiv.Disabled = true;
                        return;
                    }
                    if (SecurityType.SelectedIndex == 1)
                    {
                        dtSecurity.Columns.Add("base", typeof(String));
                        int dtrows = 0;
                        foreach (DataRow row in dtSecurity.Rows)
                        {
                            dtSecurity.Rows[dtrows]["base"] = "HKD";
                            dtrows++;
                        }
                        StockDetailsView.DataSource = dtSecurity;
                        StockDetailsView.DataBind();
                        StockDetailsView.Visible = true;
                        BondDetailView.Visible = false;
                        UnitTrustDetailView.Visible = false;
                        dtSecurity.Columns.Add("closeForeign", typeof(String));
                        dtSecurity.Columns.Add("changeDollarForeign", typeof(String));
                        dtSecurity.Columns.Add("highForeign", typeof(String));
                        dtSecurity.Columns.Add("lowForeign", typeof(String));
                    }
                    else if (SecurityType.SelectedIndex == 2)
                    {
                        BondDetailView.DataSource = dtSecurity;
                        BondDetailView.DataBind();
                        StockDetailsView.Visible = false;
                        BondDetailView.Visible = true;
                        UnitTrustDetailView.Visible = false;
                        dtSecurity.Columns.Add("priceForeign", typeof(String));
                        dtSecurity.Columns.Add("sizeForeign", typeof(String));
                    }
                    else if (SecurityType.SelectedIndex == 3)
                    {
                        UnitTrustDetailView.DataSource = dtSecurity;
                        UnitTrustDetailView.DataBind();
                        StockDetailsView.Visible = false;
                        BondDetailView.Visible = false;
                        UnitTrustDetailView.Visible = true;
                        dtSecurity.Columns.Add("priceForeign", typeof(String));
                        dtSecurity.Columns.Add("sizeForeign", typeof(String));
                    }
                    dtSecurity.Columns.Add("convertValue", typeof(String));
                    Session["IndividualSecurity"] = dtSecurity;
                    ViewState["SecurityType"] = SecurityType.SelectedValue;
                    ViewState["SecurityTypeIndex"] = SecurityType.SelectedIndex.ToString().Trim();
                }
            }
        }

        protected void gvSecurityDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Set the currency to convert to to be default value
            ddlCurrencyForIndividual.SelectedIndex = 0;

            string type = ViewState["SecurityType"].ToString().Trim();
            string code = gvSecurityDetails.SelectedRow.Cells[0].Text.Trim();
            DataTable dtSecurityDetail = myExternalFunctions.getSecuritiesByCode(type, code);
            showHideHiddenFields(false);
            if (ViewState["SecurityTypeIndex"].ToString().Trim() == "1")
            {
                dtSecurityDetail.Columns.Add("base", typeof(String));
                int dtrows = 0;
                foreach (DataRow row in dtSecurityDetail.Rows)
                {
                    dtSecurityDetail.Rows[dtrows]["base"] = "HKD";
                    dtrows++;
                }
                StockDetailsView.DataSource = dtSecurityDetail;
                StockDetailsView.DataBind();
                StockDetailsView.Visible = true;
                BondDetailView.Visible = false;
                UnitTrustDetailView.Visible = false;
                dtSecurityDetail.Columns.Add("closeForeign", typeof(String));
                dtSecurityDetail.Columns.Add("changeDollarForeign", typeof(String));
                dtSecurityDetail.Columns.Add("highForeign", typeof(String));
                dtSecurityDetail.Columns.Add("lowForeign", typeof(String));
            }
            else if (ViewState["SecurityTypeIndex"].ToString().Trim() == "2")
            {
                BondDetailView.DataSource = dtSecurityDetail;
                BondDetailView.DataBind();
                StockDetailsView.Visible = false;
                BondDetailView.Visible = true;
                UnitTrustDetailView.Visible = false;
                dtSecurityDetail.Columns.Add("priceForeign", typeof(String));
                dtSecurityDetail.Columns.Add("sizeForeign", typeof(String));
            }
            else if (ViewState["SecurityTypeIndex"].ToString().Trim() == "3")
            {
                UnitTrustDetailView.DataSource = dtSecurityDetail;
                UnitTrustDetailView.DataBind();
                StockDetailsView.Visible = false;
                BondDetailView.Visible = false;
                UnitTrustDetailView.Visible = true;
                dtSecurityDetail.Columns.Add("priceForeign", typeof(String));
                dtSecurityDetail.Columns.Add("sizeForeign", typeof(String));
            }
            IndividualSecurityDetailDiv.Visible = true;
            IndividualSecurityDetailDiv.Disabled = false;
            Session["IndividualSecurity"] = dtSecurityDetail;
        }

        protected void ddlCurrencyForList_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Get the column index for the converted value in the grid view
            int convertValueIndex = myHKeInvestCode.getColumnIndexByName(gvSecurityDetails, "convertValue");

            //Record the selected To currency
            string toCurrency = ddlCurrencyForList.SelectedValue;

            //Do nothing if no currency is selected
            if (toCurrency == "0")
            {
                gvSecurityDetails.Columns[convertValueIndex].Visible = false;
                return;
            }

            //Unload the gridview into the datatable
            gvSecurityDetails.Columns[convertValueIndex].Visible = true;

            //Copy the code for unloadgridview from the session state
            DataTable dtSecurity = (DataTable)Session["SecurityList"];

            //Compute the value in the selected currency
            int dtRows = 0;
            foreach (DataRow row in dtSecurity.Rows)
            {
                string fromCurrency = row["base"].ToString().Trim();
                if (String.IsNullOrEmpty(row["price"].ToString().Trim()))
                {
                    dtSecurity.Rows[dtRows]["convertValue"] = null;
                }
                else
                {
                    decimal value = Convert.ToDecimal(row["price"]);
                    //sql = "select rate from CurrencyRate where currency ='" + fromCurrency + "'";
                    //string fromCurrencyRate = myHKeInvestData.getAggregateValue(sql).ToString().Trim();
                    decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                    dtSecurity.Rows[dtRows]["convertValue"] = Math.Round(convertedValue,2);
                }
                dtRows = dtRows + 1;
            }

            // Change the header text of the convertedValue column to indicate the currency. 
            gvSecurityDetails.Columns[convertValueIndex].HeaderText = "Value in " + toCurrency;

            // Bind the DataTable to the GridView.
            gvSecurityDetails.DataSource = dtSecurity;
            gvSecurityDetails.DataBind();
        }

        protected void showHideHiddenFields(bool toShow)
        {
            if (ViewState["SecurityTypeIndex"].ToString().Trim() == "1")
            {
                StockDetailsView.Fields[3].Visible = toShow;
                StockDetailsView.Fields[5].Visible = toShow;
                StockDetailsView.Fields[9].Visible = toShow;
                StockDetailsView.Fields[11].Visible = toShow;
            }
            else if (ViewState["SecurityTypeIndex"].ToString().Trim() == "2")
            {
                BondDetailView.Fields[5].Visible = toShow;
                BondDetailView.Fields[7].Visible = toShow;
            }
            else if (ViewState["SecurityTypeIndex"].ToString().Trim() == "3")
            {
                UnitTrustDetailView.Fields[5].Visible = toShow;
                UnitTrustDetailView.Fields[7].Visible = toShow;
            }
        }

        protected void ddlCurrencyForIndividual_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Record the selected To currency
            string toCurrency = ddlCurrencyForIndividual.SelectedValue;

            //Do nothing if no currency is selected
            if (toCurrency == "0")
            {
                showHideHiddenFields(false);
                return;
            }

            if (ViewState["SecurityTypeIndex"].ToString().Trim() == "1")
            {
                DataTable dtStock = (DataTable)Session["IndividualSecurity"];
                int dtRows = 0;
                foreach (DataRow row in dtStock.Rows)
                {
                    string fromCurrency = row["base"].ToString().Trim();
                    if (String.IsNullOrEmpty(row["close"].ToString().Trim()))
                    {
                        dtStock.Rows[dtRows]["closeForeign"] = null;
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(row["close"]);
                        decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                        dtStock.Rows[dtRows]["closeForeign"] = Math.Round(convertedValue,2);
                    }

                    if (String.IsNullOrEmpty(row["changeDollar"].ToString().Trim()))
                    {
                        dtStock.Rows[dtRows]["changeDollarForeign"] = null;
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(row["changeDollar"]);
                        decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                        dtStock.Rows[dtRows]["changeDollarForeign"] = Math.Round(convertedValue,2);
                    }

                    if (String.IsNullOrEmpty(row["high"].ToString().Trim()))
                    {
                        dtStock.Rows[dtRows]["highForeign"] = null;
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(row["high"]);
                        decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                        dtStock.Rows[dtRows]["highForeign"] = Math.Round(convertedValue,2);
                    }

                    if (String.IsNullOrEmpty(row["low"].ToString().Trim()))
                    {
                        dtStock.Rows[dtRows]["lowForeign"] = null;
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(row["low"]);
                        decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                        dtStock.Rows[dtRows]["lowForeign"] = Math.Round(convertedValue,2);
                    }
                    dtRows = dtRows + 1;
                }
                // Change the header text of the convertedValue column to indicate the currency. 
                StockDetailsView.Fields[3].HeaderText = "Most recent closing price per share(" + toCurrency + ")";
                StockDetailsView.Fields[5].HeaderText = "The last trading day change in price(" + toCurrency + ")";
                StockDetailsView.Fields[9].HeaderText = "The high price during the preceding 52 weeks(" + toCurrency + ")";
                StockDetailsView.Fields[11].HeaderText = "The low price during the preceding 52 weeks(" + toCurrency + ")";

                showHideHiddenFields(true);

                // Bind the DataTable to the GridView.
                StockDetailsView.DataSource = dtStock;
                StockDetailsView.DataBind();
            }
            else if (ViewState["SecurityTypeIndex"].ToString().Trim() == "2")
            {
                DataTable dtBond = (DataTable)Session["IndividualSecurity"];
                int dtRows = 0;
                foreach (DataRow row in dtBond.Rows)
                {
                    string fromCurrency = row["base"].ToString().Trim();
                    if (String.IsNullOrEmpty(row["size"].ToString().Trim()))
                    {
                        dtBond.Rows[dtRows]["sizeForeign"] = null;
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(row["size"]);
                        decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                        dtBond.Rows[dtRows]["sizeForeign"] = Math.Round(convertedValue,2);
                    }
                    if (String.IsNullOrEmpty(row["price"].ToString().Trim()))
                    {
                        dtBond.Rows[dtRows]["priceForeign"] = null;
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(row["price"]);
                        decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                        dtBond.Rows[dtRows]["priceForeign"] = Math.Round(convertedValue,2);
                    }
                    dtRows++;
                }

                // Change the header text of the convertedValue column to indicate the currency. 
                BondDetailView.Fields[5].HeaderText = "Total monetary value(" + toCurrency + ")";
                BondDetailView.Fields[7].HeaderText = "Price per share(" + toCurrency + ")";

                showHideHiddenFields(true);

                // Bind the DataTable to the GridView.
                BondDetailView.DataSource = dtBond;
                BondDetailView.DataBind();
            }
            else if (ViewState["SecurityTypeIndex"].ToString().Trim() == "3")
            {
                DataTable dtUnitTrust = (DataTable)Session["IndividualSecurity"];
                int dtRows = 0;
                foreach (DataRow row in dtUnitTrust.Rows)
                {
                    string fromCurrency = row["base"].ToString().Trim();
                    if (String.IsNullOrEmpty(row["size"].ToString().Trim()))
                    {
                        dtUnitTrust.Rows[dtRows]["sizeForeign"] = null;
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(row["size"]);
                        decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                        dtUnitTrust.Rows[dtRows]["sizeForeign"] = Math.Round(convertedValue,2);
                    }
                    if (String.IsNullOrEmpty(row["price"].ToString().Trim()))
                    {
                        dtUnitTrust.Rows[dtRows]["priceForeign"] = null;
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(row["price"]);
                        decimal convertedValue = myHKeInvestCode.convertCurrency(fromCurrency.ToString().Trim(), ViewState[fromCurrency].ToString().Trim(), toCurrency.ToString().Trim(), ViewState[toCurrency].ToString().Trim(), value);
                        dtUnitTrust.Rows[dtRows]["priceForeign"] = Math.Round(convertedValue,2);
                    }
                    dtRows++;
                }

                // Change the header text of the convertedValue column to indicate the currency. 
                UnitTrustDetailView.Fields[5].HeaderText = "Total monetary value(" + toCurrency + ")";
                UnitTrustDetailView.Fields[7].HeaderText = "Price per share(" + toCurrency + ")";

                showHideHiddenFields(true);

                // Bind the DataTable to the GridView.
                UnitTrustDetailView.DataSource = dtUnitTrust;
                UnitTrustDetailView.DataBind();
            }

        }
    }
}