<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountApplicationForm.aspx.cs" Inherits="HKeInvestWebApplication.AccountApplicationForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Account Application Form</h3>
    <hr />
    <div class="form-horizontal">
        <h4>1. Account Type</h4>
        <div class="form-group">
            <div class="col-md-12">
                <asp:RadioButtonList ID="AccountType" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive" AutoPostBack="True" OnSelectedIndexChanged="AccountType_SelectedIndexChanged">
                    <asp:ListItem Value="individual">Individual</asp:ListItem>
                    <asp:ListItem Value="survivorship">Joint Tenants with Rights of Survivorship</asp:ListItem>
                    <asp:ListItem Value="common">Joint Tenants in Common</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="AccountType" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="You must select one of the Account type."></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <hr />
        </div>

        <h4>2. Client Information</h4>

        <div class="form-horizontal">
            <h5>Primary Account Holder</h5>
            <div class="form-group">
                <asp:Label ID="Label8" runat="server" AssociatedControlID="PClientTitle" Text="Title" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="PClientTitle" runat="server" RepeatDirection="Horizontal" CssClass="table-responsive">
                        <asp:ListItem Value="mr">Mr.</asp:ListItem>
                        <asp:ListItem Value="mrs">Mrs.</asp:ListItem>
                        <asp:ListItem Value="ms">Ms.</asp:ListItem>
                        <asp:ListItem Value="dr">Dr.</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ControlToValidate="PClientTitle" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Title for the Primary Holder is required."></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" AssociatedControlID="FirstName" Text="First Name" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="FirstName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FirstName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="First Name of the Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <asp:Label ID="Label2" runat="server" AssociatedControlID="LastName" Text="Last Name" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="LastName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ControlToValidate="LastName" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid last name which should contain at least one letter" ValidationExpression="^(?=.*[a-zA-Z]).+$"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="LastName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Last Name of the Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label5" runat="server" AssociatedControlID="DateOfBirth" Text="Date of Birth (dd/mm/yyyy)" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="DateOfBirth" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="DateOfBirth" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Date of Birth of Primary holder is not valid." ValidationExpression="^(((0[1-9]|1\d|2[0-8])\/(0[1-9]|1[012])|(29|30)\/(0[13456789]|1[012])|31\/(0[13578]|1[02]))\/\d{4}|29\/02\/((\d{2})(0[48]|[2468][048]|[13579][26])|(([02468][048]|[13579][26])00)))$" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DateOfBirth" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Date of Birth of the Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvDOB_not_future" runat="server" ControlToValidate="DateOfBirth" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Date Of Birth should not be in the future." OnServerValidate="cvDOB_not_future_ServerValidate"></asp:CustomValidator>
                </div>
                <asp:Label ID="Label6" runat="server" AssociatedControlID="Email" Text="Email" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Email" runat="server" TextMode="Email" CssClass="form-control" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Email" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Email address of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label49" runat="server" Text="Home address" CssClass="control-label col-md-2 "></asp:Label>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" AssociatedControlID="Building" Text="Building(if any)" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Building" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                </div>
                <asp:Label ID="Label4" runat="server" AssociatedControlID="Street" Text="Street" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Street" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Street" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Street of the Primary Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label10" runat="server" AssociatedControlID="District" Text="District" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="District" runat="server" CssClass="form-control" MaxLength="19"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="District" CssClass="text-danger" EnableClientScript="False" ErrorMessage="District of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label9" runat="server" AssociatedControlID="HomePhone" Text="Home phone" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="HomePhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="HomePhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Home Phone." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="HomePhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="At least one of Home phone, Business phone and Mobile phone should be provided." OnServerValidate="CustomValidator1_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                </div>
                <asp:Label ID="Label11" runat="server" AssociatedControlID="HomeFax" Text="Home fax" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="HomeFax" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="HomeFax" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Home Fax." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label12" runat="server" AssociatedControlID="BusinessPhone" Text="Business phone" CssClass="control-label col-md-2"></asp:Label>
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
                <asp:Label ID="Label14" runat="server" AssociatedControlID="CountryOfCitizenship" Text="Country of citizenship" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="CountryOfCitizenship" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="CountryOfCitizenship" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Country of citizenship for Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <asp:Label ID="Label15" runat="server" AssociatedControlID="CountryOfLegalResidence" Text="Country of legal residence" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="CountryOfLegalResidence" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="CountryOfLegalResidence" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Country of legal residence for Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label16" runat="server" AssociatedControlID="HKID" Text="HKID/Passport Number" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="HKIDorPassport" runat="server" CssClass="table-responsive" OnSelectedIndexChanged="HKIDorPassport_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem>HKID</asp:ListItem>
                        <asp:ListItem>Passport</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:TextBox ID="HKID" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ControlToValidate="HKID" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid HKID/Passport Number." ValidationExpression="(?i:^([a-zA-Z0-9]{1,8}))"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="HKID" CssClass="text-danger" EnableClientScript="False" ErrorMessage="HKID/Passport number of Primary holer is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div id="passportcountrydiv" runat="server">
                    <asp:Label ID="Label17" runat="server" AssociatedControlID="PassportCountryOfIssue" Text="Passport country of issue" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="PassportCountryOfIssue" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="PassportCountryOfIssue" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Passport country of issue of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
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
                <asp:Label ID="Label19" runat="server" AssociatedControlID="CFirstName" Text="First Name" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="CFirstName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="CFirstName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="First Name of the Co-Account holder holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <asp:Label ID="Label20" runat="server" AssociatedControlID="CLastName" Text="Last Name" CssClass="control-label col-md-2 "></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="CLastName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator20" runat="server" ControlToValidate="CLastName" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid last name which should contain at least one letter" ValidationExpression="^(?=.*[a-zA-Z]).+$"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="CLastName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Last Name of Co-Account Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label21" runat="server" AssociatedControlID="CDateOfBirth" Text="Date of Birth (dd/mm/yyyy)" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="CDateOfBirth" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="CDateOfBirth" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Date of Birth of Co-Account Holder is not valid." ValidationExpression="^(((0[1-9]|1\d|2[0-8])\/(0[1-9]|1[012])|(29|30)\/(0[13456789]|1[012])|31\/(0[13578]|1[02]))\/\d{4}|29\/02\/((\d{2})(0[48]|[2468][048]|[13579][26])|(([02468][048]|[13579][26])00)))$" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="CDateOfBirth" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Date of Birth of Co-Account Holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvcoDOB_not_future" runat="server" ControlToValidate="CDateOfBirth" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Date Of Birth should not be in the future." OnServerValidate="cvcoDOB_not_future_ServerValidate"></asp:CustomValidator>

                </div>
                <asp:Label ID="Label22" runat="server" AssociatedControlID="CEmail" Text="Email" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="CEmail" runat="server" TextMode="Email" CssClass="form-control" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="CEmail" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Email address of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label61" runat="server" Text="Home address" CssClass="control-label col-md-2"></asp:Label>
            </div>
            <div class="form-group">
                <asp:Label ID="Label23" runat="server" AssociatedControlID="CBuilding" Text="Building(if any)" CssClass="control-label col-md-2"></asp:Label>
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
                    <asp:TextBox ID="CCountryOfCitizenship" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="CCountryOfCitizenship" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Country of citizenship of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <asp:Label ID="Label31" runat="server" AssociatedControlID="CCountryOfLegalResidence" Text="Country of legal residence" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="CCountryOfLegalResidence" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="CCountryOfLegalResidence" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Country of legal residence of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label32" runat="server" AssociatedControlID="CHKID" Text="HKID/Passport Number" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="CHKIDorPassport" runat="server" CssClass="table-responsive" AutoPostBack="True" OnSelectedIndexChanged="CHKIDorPassport_SelectedIndexChanged">
                        <asp:ListItem>HKID</asp:ListItem>
                        <asp:ListItem>Passport</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:TextBox ID="CHKID" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="CHKID" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid HKID/Passport Number." ValidationExpression="(?i:^([a-zA-Z0-9]{1,8}))"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="CustomValidator5" runat="server" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" OnServerValidate="CustomValidator5_ServerValidate"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="CHKID" CssClass="text-danger" EnableClientScript="False" ErrorMessage="HKID/Passport number of Co-Account holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div runat="server" id="cpassportcountrydiv">
                    <asp:Label ID="Label33" runat="server" AssociatedControlID="CPassportCountryOfIssue" Text="Passport country of issue" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="CPassportCountryOfIssue" runat="server" CssClass="form-control " MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="CPassportCountryOfIssue" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Passport country of issue is required." Display="Dynamic"></asp:RequiredFieldValidator>
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
                <asp:Label ID="Label34" runat="server" AssociatedControlID="EmploymentStatus" Text="Employment Status" CssClass="control-label col-md-2"></asp:Label>
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
                    <asp:Label ID="Label35" runat="server" AssociatedControlID="SpecificOccupation" Text="Specific occupation" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="SpecificOccupation" runat="server" CssClass="form-control " MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="SpecificOccupation" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Specific occupation of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label ID="Label36" runat="server" AssociatedControlID="YearsWithEmployer" Text="Years with employer" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="YearsWithEmployer" runat="server" CssClass="form-control " MaxLength="2"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="YearsWithEmployer" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid length of the period." ValidationExpression="^[0-9]{1,2}$"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="YearsWithEmployer" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Years with employer of issue of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label37" runat="server" AssociatedControlID="EmployerName" Text="Employer name" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="EmployerName" runat="server" CssClass="form-control " MaxLength="25"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="EmployerName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Employer name of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label ID="Label38" runat="server" AssociatedControlID="EmployerPhone" Text="Employer phone" CssClass="control-label col-md-2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="EmployerPhone" runat="server" CssClass="form-control " MaxLength="8"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="EmployerPhone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid phone number as Employer phone number." ValidationExpression="^[0-9]{8}$"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="EmployerPhone" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Employer phone of Primary holder is required." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label39" runat="server" AssociatedControlID="NatureOfBusiness" Text="Nature of business" CssClass="control-label col-md-2"></asp:Label>
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" ControlToValidate="EmployedFinancialInstitution" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server" ControlToValidate="DirectorOfTradedCompany" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator54" runat="server" ControlToValidate="InvestmentObjective" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator55" runat="server" ControlToValidate="InvestmentKnowledge" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator56" runat="server" ControlToValidate="InvestmentExperience" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please select one of the options."></asp:RequiredFieldValidator>
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
            </div>
        </div>
        <div class="form-group">
            <hr />
        </div>

        <h4>7. Initial Account Deposit</h4>
        <div class="form-group">
            <div class="col-md-6">
                <asp:CheckBoxList ID="InitialDeposit" runat="server" CssClass="table-responsive" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Value="1">A cheque made payable to Hong Kong Electronic Investment LLC is enclosed</asp:ListItem>
                    <asp:ListItem Value="2">A completed Account Transfer Form is attached.</asp:ListItem>
                </asp:CheckBoxList>
                <asp:CustomValidator ID="CustomValidator3" runat="server" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="You must select at least one of the options above." OnServerValidate="CustomValidator3_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
                <asp:CustomValidator ID="CustomValidator4" runat="server" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" EnableTheming="True" ErrorMessage="Error" OnServerValidate="CustomValidator4_ServerValidate"></asp:CustomValidator>
            </div>
        </div>
        <div class="form-group" id="ChequeDiv" runat="server">
            <asp:Label ID="Label50" runat="server" AssociatedControlID="Cheque" Text="Cheque Value:" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-5">
                <asp:TextBox ID="Cheque" runat="server" CssClass="form-control " MaxLength="15" Enabled="True"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="Cheque" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide  a valid Cheque Value with up to 2 decimal places. " ValidationExpression="^\d+(\.\d{1,2})?$"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator60" runat="server" ControlToValidate="Cheque" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide the value of the cheque."></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group" id="TransferDiv" runat="server">
            <asp:Label ID="Label51" runat="server" AssociatedControlID="Transfer" Text="Amount of Transfer:" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-5">
                <asp:TextBox ID="Transfer" runat="server" CssClass="form-control" MaxLength="15" Enabled="True"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="Transfer" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide  a valid Amount of Transfer with up to 2 decimal places. " ValidationExpression="^\d+(\.\d{1,2})?$"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator61" runat="server" ControlToValidate="Transfer" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide the amount of the transfer."></asp:RequiredFieldValidator>
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
</asp:Content>
