<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrationPage.aspx.cs" Inherits="HKeInvestWebApplication.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Create a new login account</h4>
    <div class="form-horizontal">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" EnableClientScript="False" />
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="FirstName" Text="First Name" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="FirstName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FirstName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="First Name is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label ID="Label2" runat="server" AssociatedControlID="LastName" Text="Last Name" CssClass="control-label col-md-2 "></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="LastName" runat="server" CssClass="form-control " MaxLength="35"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="LastName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Last Name is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" AssociatedControlID="AccountNumber" Text="Account#" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="AccountNumber" runat="server" CssClass="form-control " MaxLength="10"></asp:TextBox>
                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="AccountNumber" CssClass="text-danger" EnableClientScript="False" OnServerValidate="CustomValidator1_ServerValidate" ValidateEmptyText="True" Display="Dynamic">*</asp:CustomValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="AccountNumber" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Account # is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label ID="Label4" runat="server" AssociatedControlID="HKID" Text="HKID/Passport#" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="HKID" runat="server" CssClass="form-control " MaxLength="8"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="HKID" CssClass="text-danger" EnableClientScript="False" ErrorMessage="A HKID or Passport is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label5" runat="server" AssociatedControlID="DateOfBirth" Text="Date of Birth" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="DateOfBirth" runat="server" CssClass="form-control "></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="DateOfBirth" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Date of Birth is not valid." ValidationExpression="^((0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/(19\d\d|200[0-9]|201[0-6]))$" Display="Dynamic">*</asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DateOfBirth" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Date of Birth is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label ID="Label6" runat="server" AssociatedControlID="Email" Text="Email" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Email" runat="server" TextMode="Email" CssClass="form-control " MaxLength="30"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Email" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Email address is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label7" runat="server" AssociatedControlID="UserName" Text="User Name" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="UserName" runat="server" CssClass="form-control " MaxLength="10"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="UserName" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="User Name must contain at least 6 characters" ValidationExpression="^.{6,10}$">*</asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="UserName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="User Name must contain only letters and digits." ValidationExpression="(?i:^([a-zA-Z0-9]{6,10}))" Display="Dynamic">*</asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="UserName" CssClass="text-danger" EnableClientScript="False" ErrorMessage="User Name is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label8" runat="server" AssociatedControlID="Password" Text="Password" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control "></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="Password" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Password must contain at least 8 characters." ValidationExpression="^.{8,15}$">*</asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="Password" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Password must contain at least 2 nonalphanumeric characters." ValidationExpression="^(?=.*[^a-zA-Z0-9].*[^a-zA-Z0-9])(.{8,15})$" Display="Dynamic">*</asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="Password" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Password is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label9" runat="server" AssociatedControlID="ConfirmPassword" Text="Confirm Password" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" CssClass="form-control col-md-4"></asp:TextBox>
                <br />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" EnableClientScript="False" ErrorMessage="Password and Confirm Password do not match." CssClass="text-danger" Display="Dynamic">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ConfirmPassword" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Confirm Password is required." Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10"> 
                <asp:Button ID="Register" runat="server" Text="Register" CssClass="btn button-default" />
            </div>
        </div>
    </div>
</asp:Content>
