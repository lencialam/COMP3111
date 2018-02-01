<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ManageClientInfo.aspx.cs" Inherits="HKeInvestWebApplicationLencia.ManageClientInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Your Information</h2>
    <hr />
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="AccountNumberBox" Text="Account Number" CssClass=" control-label col-md-2"></asp:Label>
            <asp:TextBox ID="AccountNumberBox" runat="server" CssClass="col-md-4 form-control" MaxLength="10"></asp:TextBox>
            <asp:RegularExpressionValidator ID="ANvalidator" runat="server" ControlToValidate="AccountNumberBox" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide valid Account Number" ValidationExpression="^[A-Z]{2}[0-9]{8}$"></asp:RegularExpressionValidator>
            <div class="col-md-2">
                <asp:Button ID="CheckUserName" runat="server" Text="Check" CssClass="btn button-default" OnClick="Check_Click" CausesValidation="False" />
            </div>
        </div>
        <div class="form-group" runat="server" id="msgDiv">
            <asp:Label ID="SearchError" runat="server" CssClass="text-danger"></asp:Label>
        </div>
        <div>
            <hr />
        </div>

        <div class="form-horizontal" runat="server" id="modifyInfoDiv">
            <h4>1. Account Type</h4>
            <div class="form-group">
                <asp:Label AssociatedControlID="accountTypeLabel" runat="server" Text="Account Type" CssClass="control-label col-md-2"></asp:Label>
                <asp:Label ID="accountTypeLabel" runat="server" CssClass="col-md-4"></asp:Label>
            </div>
            <div class="form-group">
                <asp:Label AssociatedControlID="UserNameLabel" runat="server" Text="User Name" CssClass="control-label col-md-2"></asp:Label>
                <asp:Label ID="UserNameLabel" runat="server" Cssclass="col-md-4"></asp:Label>
            </div>
            <div class="form-group" id="UserNameNullErrorDiv" runat="server" visible="false">
                <asp:Label runat="server" Text="No login account has been registered for this Account yet. Please be careful if you need to change the last name of the primary holder." CssClass="text-danger"></asp:Label>
            </div>
            <div class="form-group">
                <hr />
            </div>

            <h4>2. Client Information</h4>

            <div class="form-horizontal">
                <h5>Primary Account Holder</h5>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="PClientTitle" Text="Title" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="PClientTitle" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                            <asp:ListItem Value="mr">Mr.</asp:ListItem>
                            <asp:ListItem Value="mrs">Mrs.</asp:ListItem>
                            <asp:ListItem Value="ms">Ms.</asp:ListItem>
                            <asp:ListItem Value="dr">Dr.</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="rfvPClientTitle" runat="server" ControlToValidate="PClientTitle" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Title for the Primary Holder is required."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="FirstName" Text="First Name" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="FirstName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="First Name of Primary Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label runat="server" AssociatedControlID="LastName" Text="Last Name" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="LastName" runat="server" CssClass="col-md-4 form-control"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ControlToValidate="LastName" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid last name which should contain at least one letter" ValidationExpression="^(?=.*[a-zA-Z]).+$"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="LastName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Last Name of Primary Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="DateOfBirthLabel" Text="Date of Birth" CssClass="control-label col-md-2"></asp:Label>
                    <asp:Label ID="DateOfBirthLabel" runat="server" CssClass="col-md-4"></asp:Label>
                    <asp:Label runat="server" AssociatedControlID="Email" Text="Email" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="Email" runat="server" TextMode="Email" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Email" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Email address of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" Text="Home address" CssClass="control-label col-md-2 "></asp:Label>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="Building" Text="Building(if any)" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">

                        <asp:TextBox ID="Building" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                    <asp:Label runat="server" AssociatedControlID="Street" Text="Street" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="Street" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Street" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Street of the Primary Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="District" Text="District" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="District" runat="server" CssClass="form-control" MaxLength="19"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="District" CssClass="text-danger" EnableClientScript="False" ErrorMessage="District of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="HomePhone" Text="Home phone" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="HomePhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="HomePhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Home Phone." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="HomePhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="At least one of Home phone, Business phone and Mobile phone should be provided." OnServerValidate="CustomValidator1_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                    </div>
                    <asp:Label runat="server" AssociatedControlID="HomeFax" Text="Home fax" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="HomeFax" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="HomeFax" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Home Fax." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="BusinessPhone" Text="Business phone" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="BusinessPhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="BusinessPhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Business Phone." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                    </div>
                    <asp:Label ID="Label13" runat="server" AssociatedControlID="MobilePhone" Text="Mobile phone" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="MobilePhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="MobilePhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Mobile Phone." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="CountryOfCitizenship" Text="Country of citizenship" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CountryOfCitizenship" runat="server" CssClass="col-md-4 form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CountryOfCitizenship" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Country of Citizenship of Primary Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label runat="server" AssociatedControlID="CountryOfResidence" Text="Country of legal residence" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CountryOfResidence" runat="server" CssClass="col-md-4 form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CountryOfResidence" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Country of Legal Residence of Primary Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" Text="HKID/Passport Number" AssociatedControlID="HKIDLabel" CssClass="control-label col-md-2"></asp:Label>
                    <div id="HKIDLabelDiv" visible="False" runat="server">
                        <asp:Label ID="HKIDLabel" runat="server" CssClass="col-md-4"></asp:Label>
                    </div>
                    <div id="PassportNumberDiv" runat="server">
                        <div class="col-md-4">
                            <asp:TextBox ID="PassportNumber" runat="server" CssClass="form-control col-md-4" MaxLength="8"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="PassportNumber" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Passport number of of Primary Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="PassportCountryOfIssue" Text="Passport country of issue" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="PassportCountryOfIssue" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="PassportCountryOfIssue" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Passport Country of Issue of Primary Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

            <div runat="server" id="ClientinfoCClientDiv" class="form-horizontal">
                <h5>Co-Account Holder</h5>
                <div class="form-group">
                    <asp:Label ID="Label18" runat="server" AssociatedControlID="CClientTitle" Text="Title" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="CClientTitle" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                            <asp:ListItem Value="mr">Mr.</asp:ListItem>
                            <asp:ListItem Value="mrs">Mrs.</asp:ListItem>
                            <asp:ListItem Value="ms">Ms.</asp:ListItem>
                            <asp:ListItem Value="dr">Dr.</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="CClientTitle" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="You must select a Title for the Co-Account Holder."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="CFirstName" Text="First Name" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CFirstName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="First Name of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label runat="server" AssociatedControlID="CLastName" Text="Last Name" CssClass="control-label col-md-2 "></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CLastName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CLastName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Last Name of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="CDateOfBirthLabel" Text="Date of Birth (dd/mm/yyyy)" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:Label ID="CDateOfBirthLabel" runat="server"></asp:Label>
                    </div>
                    <asp:Label runat="server" AssociatedControlID="CEmail" Text="Email" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CEmail" runat="server" TextMode="Email" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="CEmail" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Email address of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" Text="Home address" CssClass="control-label col-md-2"></asp:Label>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="CBuilding" Text="Building(if any)" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CBuilding" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                    <asp:Label ID="Label24" runat="server" AssociatedControlID="CStreet" Text="Street" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CStreet" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="CStreet" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Street of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label25" runat="server" AssociatedControlID="CDistrict" Text="District" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CDistrict" runat="server" CssClass="form-control" MaxLength="19"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="CDistrict" CssClass="text-danger" EnableClientScript="False" ErrorMessage="District of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label26" runat="server" AssociatedControlID="CHomePhone" Text="Home phone" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CHomePhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="CHomePhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Home Phone." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="CHomePhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="At least one of Home phone, Business phone and Mobile phone should be provided." OnServerValidate="CustomValidator2_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                    </div>
                    <asp:Label ID="Label27" runat="server" AssociatedControlID="CHomeFax" Text="Home fax" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CHomeFax" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="CHomeFax" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Home Phone." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label28" runat="server" AssociatedControlID="CBusinessPhone" Text="Business phone" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CBusinessPhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="CBusinessPhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Business Phone." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                    </div>
                    <asp:Label ID="Label29" runat="server" AssociatedControlID="CMobilePhone" Text="Mobile phone" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CMobilePhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="CMobilePhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Mobile Phone." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label30" runat="server" AssociatedControlID="CCountryOfCitizenship" Text="Country of citizenship" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CCountryOfCitizenship" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CCountryOfCitizenship" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Country of Citizenship of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label ID="Label31" runat="server" AssociatedControlID="CCountryOfResidence" Text="Country of legal residence" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CCountryOfResidence" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CCountryOfResidence" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Country of Legal Residence of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label32" runat="server" AssociatedControlID="CHKIDLabel" Text="HKID/Passport Number" CssClass="control-label col-md-2"></asp:Label>
                    <div id="CHKIDDiv" runat="server" visible="False">
                        <asp:Label ID="CHKIDLabel" runat="server" CssClass="col-md-4"></asp:Label>
                    </div>
                    <div id="CPassportNumberDiv" runat="server">
                        <div class="col-md-4">
                            <asp:TextBox ID="CPassportNumber" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="CPassportNumber" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Passport number of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="CPassportCountryOfIssue" Text="Passport country of issue" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="CPassportCountryOfIssue" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="CPassportCountryOfIssue" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Passport Country of Issue of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <hr />
            </div>

            <h4>3. Employment Information</h4>

            <div class="form-horizontal">
                <h5>Primary Account Holder</h5>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="EmploymentStatus" Text="Employment Status" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-6">
                        <asp:RadioButtonList ID="EmploymentStatus" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CssClass="table-responsive" OnSelectedIndexChanged="EmploymentStatus_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="employed">Employed</asp:ListItem>
                            <asp:ListItem Value="selfemployed">Self-employed</asp:ListItem>
                            <asp:ListItem Value="retired">Retired</asp:ListItem>
                            <asp:ListItem Value="student">Student</asp:ListItem>
                            <asp:ListItem Value="notemployed">Not Employed</asp:ListItem>
                            <asp:ListItem Value="homemaker">Homemaker</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ControlToValidate="EmploymentStatus" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="You must select an Employment Status for the Primary holder."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div runat="server" id="PClientEmploydetailDiv">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="SpecificOccupation" Text="Specific occupation" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="SpecificOccupation" runat="server" CssClass="form-control " MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="SpecificOccupation" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Specific occupation of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="YearsWithEmployer" Text="Years with employer" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="YearsWithEmployer" runat="server" CssClass="form-control " MaxLength="2"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="YearsWithEmployer" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid length of the period." ValidationExpression="^[0-9]{1,2}$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="YearsWithEmployer" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Years with employer of issue of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="EmployerName" Text="Employer name" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="EmployerName" runat="server" CssClass="form-control " MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="EmployerName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Employer name of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="EmployerPhone" Text="Employer phone" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="EmployerPhone" runat="server" CssClass="form-control " MaxLength="8"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="EmployerPhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Employer phone number." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="EmployerPhone" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Employer phone of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="NatureOfBusiness" Text="Nature of business" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="NatureOfBusiness" runat="server" CssClass="form-control " MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="NatureOfBusiness" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Nature of business of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

            <div runat="server" id="EmployinfoCClientDiv" class="form-horizontal">
                <h5>Co-Account Holder</h5>
                <div class="form-group">
                    <asp:Label ID="Label52" runat="server" AssociatedControlID="CEmploymentStatus" Text="Employment Status" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-6">
                        <asp:RadioButtonList ID="CEmploymentStatus" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CssClass="table-responsive" AutoPostBack="True" OnSelectedIndexChanged="CEmploymentStatus_SelectedIndexChanged">
                            <asp:ListItem Value="employed">Employed</asp:ListItem>
                            <asp:ListItem Value="selfemployed">Self-employed</asp:ListItem>
                            <asp:ListItem Value="retired">Retired</asp:ListItem>
                            <asp:ListItem Value="student">Student</asp:ListItem>
                            <asp:ListItem Value="notemployed">Not Employed</asp:ListItem>
                            <asp:ListItem Value="homemaker">Homemaker</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ControlToValidate="CEmploymentStatus" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="You must select an Employment Status for Co-Account Holder."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div runat="server" id="CClientEmploydetailDiv">
                    <div class="form-group">
                        <asp:Label ID="Label53" runat="server" AssociatedControlID="CSpecificOccupation" Text="Specific occupation" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="CSpecificOccupation" runat="server" CssClass="form-control " MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ControlToValidate="CSpecificOccupation" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Specific occupation of Co-Account Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label ID="Label54" runat="server" AssociatedControlID="CYearsWithEmployer" Text="Years with employer" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="CYearsWithEmployer" runat="server" CssClass="form-control " MaxLength="2"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="CYearsWithEmployer" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid length of the period." ValidationExpression="^[0-9]{1,2}$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="CYearsWithEmployer" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Years with employer of issue of Co-Account Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label55" runat="server" AssociatedControlID="CEmployerName" Text="Employer name" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="CEmployerName" runat="server" CssClass="form-control " MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="CEmployerName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Employer name of Co-Account Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label ID="Label56" runat="server" AssociatedControlID="CEmployerPhone" Text="Employer phone" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="CEmployerPhone" runat="server" CssClass="form-control " MaxLength="8"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="CEmployerPhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Employer Phone Number." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ControlToValidate="CEmployerPhone" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Employer phone of Co-Account Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label57" runat="server" AssociatedControlID="CNatureOfBusiness" Text="Nature of business" CssClass="control-label col-md-2"></asp:Label>
                        <div class="col-md-4">
                            <asp:TextBox ID="CNatureOfBusiness" runat="server" CssClass="form-control " MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="CNatureOfBusiness" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Nature of business of Co-Account Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <hr />
            </div>

            <h4>4. Disclosures and Regulatory information</h4>

            <div class="form-horizontal">
                <h5>Primary Account Holder</h5>
                <div class="form-horizontal">
                    <h6>Are you employed by a registered securities broker/dealer, investment advisor, bank or other financial institution?</h6>
                </div>
                <div class="form-group">
                    <div class="col-md-4">

                        <asp:RadioButtonList ID="EmployedFinancialInstitution" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes (you must submit a compliance letter with this application)</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="EmployedFinancialInstitution" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-horizontal">
                    <h6>Are you a director, 10% shareholder or policy-making officer of a publicly traded company?</h6>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="DirectorOfTradedCompany" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="DirectorOfTradedCompany" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div runat="server" id="DisclosureCClientDiv" class="form-horizontal">
                <h5>Co-Account Holder</h5>
                <div class="form-horizontal">
                    <h6>Are you employed by a registered securities broker/dealer, investment advisor, bank or other financial institution?</h6>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="CEmployedFinancialInstitution" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes (you must submit a compliance letter with this application)</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator49" runat="server" ControlToValidate="CEmployedFinancialInstitution" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-horizontal">
                    <h6>Are you a director, 10% shareholder or policy-making officer of a publicly traded company?</h6>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="CDirectorOfTradedCompany" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator50" runat="server" ControlToValidate="CDirectorOfTradedCompany" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div class="form-horizontal">
                <h6>Describe the primary source of funds deposited to this account?</h6>
            </div>
            <div class="form-group">
                <div class="col-md-4">

                    <asp:RadioButtonList ID="SourceOfFund" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive" AutoPostBack="True" OnSelectedIndexChanged="SourceOfFund_SelectedIndexChanged">
                        <asp:ListItem Value="salary">salary/wage/savings</asp:ListItem>
                        <asp:ListItem Value="investment">investment/capital gains</asp:ListItem>
                        <asp:ListItem Value="family">family/relatives/inheritance</asp:ListItem>
                        <asp:ListItem Value="other">Other(describe below)</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ControlToValidate="SourceOfFund" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                </div>
                <div runat="server" class="col-md-12" id="DescribeOtherFundDiv">
                    <asp:TextBox ID="DescribeOtherFund" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator53" runat="server" ControlToValidate="DescribeOtherFund" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please describe the course of funds."></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <hr />
            </div>


            <h4>5. Investment Profile</h4>

            <h6>Investment objective for this account:</h6>
            <div class="form-group">
                <div class="col-md-12">

                    <asp:RadioButtonList ID="InvestmentObjective" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                        <asp:ListItem Value="capital">capital preservation</asp:ListItem>
                        <asp:ListItem>income</asp:ListItem>
                        <asp:ListItem>growth</asp:ListItem>
                        <asp:ListItem>speculation</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="InvestmentObjective" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                </div>
            </div>

            <h6>Investment knowledge:</h6>
            <div class="form-group">
                <div class="col-md-12">

                    <asp:RadioButtonList ID="InvestmentKnowledge" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                        <asp:ListItem>none</asp:ListItem>
                        <asp:ListItem>limited</asp:ListItem>
                        <asp:ListItem>good</asp:ListItem>
                        <asp:ListItem>extensive</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="InvestmentKnowledge" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                </div>
            </div>

            <h6>Investment experience:</h6>
            <div class="form-group">
                <div class="col-md-12">

                    <asp:RadioButtonList ID="InvestmentExperience" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                        <asp:ListItem>none</asp:ListItem>
                        <asp:ListItem>limited</asp:ListItem>
                        <asp:ListItem>good</asp:ListItem>
                        <asp:ListItem>extensive</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="InvestmentExperience" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                </div>
            </div>

            <h6>Annual income:</h6>
            <div class="form-group">
                <div class="col-md-12">

                    <asp:RadioButtonList ID="AnnualIncome" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                        <asp:ListItem Value="20000">under HK$20,000</asp:ListItem>
                        <asp:ListItem Value="20001">HK$20,001-HK$200,000</asp:ListItem>
                        <asp:ListItem Value="200001">HK$200,001-HK$2,000,000</asp:ListItem>
                        <asp:ListItem Value="2000001">more than HK$2,000,000</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator57" runat="server" ControlToValidate="AnnualIncome" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                </div>
            </div>

            <h6>Approximate liquid net worth(cash and securities):</h6>
            <div class="form-group">
                <div class="col-md-12">

                    <asp:RadioButtonList ID="LiquidNetWorth" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                        <asp:ListItem Value="100000">under HK$100,000</asp:ListItem>
                        <asp:ListItem Value="100001">HK$100,001-HK$1,000,000</asp:ListItem>
                        <asp:ListItem Value="1000001">HK$1,000,001-HK$10,000,000</asp:ListItem>
                        <asp:ListItem Value="10000001">more than HK$10,000,000</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator58" runat="server" ControlToValidate="LiquidNetWorth" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <hr />
            </div>


            <h4>6. Account Feature</h4>

            <h6>Earn Income on Your Cash Balance</h6>
            <div class="form-group">
                <div class="col-md-12">
                    <asp:RadioButtonList ID="SweepFreeCreditBalance" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                        <asp:ListItem Value="0">No</asp:ListItem>
                        <asp:ListItem Value="1">Yes, sweep my Free Credit Balance into the Fund</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator59" runat="server" ControlToValidate="SweepFreeCreditBalance" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <hr />
            </div>

            <div class="form-horizontal" id="successmsg" runat="server">
                <asp:Label ID="Label40" runat="server" Text="Account Created!" CssClass="bg-success"></asp:Label>
            </div>

            <div class="form-horizontal" id="errormsg" runat="server">
                <asp:Label ID="Label41" runat="server" Text="Please double check the information you provide." CssClass="alert-danger"></asp:Label>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button ID="Apply" runat="server" Text="Apply" CssClass="btn button-default" OnClick="Apply_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
