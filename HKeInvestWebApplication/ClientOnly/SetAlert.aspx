<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="SetAlert.aspx.cs" Inherits="HKeInvestWebApplication.SetAlert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="form-horizontal">
        <h2>Set Alert for Security Holdings</h2>
        <h4>Please notice the pop-up window which may be hidden under the current window.</h4>
        <div class="form-group">
            <hr />
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="SecurityType" runat="server" Text="Security Type" CssClass="control-label col-md-2"></asp:Label>
            <asp:DropDownList ID="SecurityType" runat="server" CssClass="col-md-4" OnSelectedIndexChanged="SecurityType_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
            <asp:Label AssociatedControlID="SecurityCode" runat="server" Text="Security Code" CssClass="control-label col-md-2"></asp:Label>
            <asp:TextBox ID="SecurityCode" runat="server" CssClass="col-md-4" MaxLength="4"></asp:TextBox>
            <asp:RegularExpressionValidator runat="server" ControlToValidate="SecurityCode" CssClass="text-danger" Display="Dynamic" ErrorMessage="Security Code should contain only numbers." ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="SecurityCode" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Security Code is required."></asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="AlertType" runat="server" Text="Alert Type" CssClass="control-label col-md-2"></asp:Label>
            <asp:RadioButtonList ID="AlertType" runat="server" CssClass="col-md-4 table-responsive" RepeatDirection="Horizontal" OnSelectedIndexChanged="AlertType_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem>Low Value Alert</asp:ListItem>
                <asp:ListItem>High Value Alert</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="AlertType" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Alert Type is required."></asp:RequiredFieldValidator>
        </div>

        <div class="form-group" id="LowValueDiv" runat="server" visible="false">
            <asp:Label AssociatedControlID="LowValue" runat="server" Text="Low Value" CssClass="control-label col-md-2"></asp:Label>
            <asp:TextBox ID="LowValue" runat="server" CssClass="col-md-4" MaxLength="7"></asp:TextBox>
            <asp:RegularExpressionValidator ID="revLowValue" runat="server" ErrorMessage="Low Value must be a decimal number with at most 3 digits before decimal point and a precision of at most two." ValidationExpression="^([0-9]{0,3})(\.[0-9]{1,2})?$" ControlToValidate="LowValue" CssClass="text-danger"></asp:RegularExpressionValidator>
        </div>

        <div class="form-group" id="HighValueDiv" runat="server" visible="false">
            <asp:Label AssociatedControlID="HighValue" runat="server" Text="High Value" CssClass="control-label col-md-2"></asp:Label>
            <asp:TextBox ID="HighValue" runat="server" CssClass="col-md-4" MaxLength="7"></asp:TextBox>
            <asp:RegularExpressionValidator ID="revHighValue" runat="server" ErrorMessage="High Value must be a decimal number with at most 3 digits before decimal point and a precision of at most two." ValidationExpression="^([0-9]{0,3})(\.[0-9]{1,2})?$" ControlToValidate="HighValue" CssClass="text-danger"></asp:RegularExpressionValidator>
        </div>

        <div class="form-group">
            <div class="col-md-2"></div>
            <asp:Button ID="SetAlertBT" runat="server" Text="Set Alert" CssClass="btn button-default col-md-2" OnClick="SetAlert_Click" />
            <div class="col-md-2"></div>
            <asp:Button ID="DeleteAlertBT" runat="server" Text="Delete Alert" CssClass="btn button-default col-md-2" OnClick="DeleteAlert_Click" />
        </div>

        <div class="form-group">
            <hr />
        </div>

        <div>
            <asp:GridView ID="gvManageAlert" runat="server" AutoGenerateColumns="False" DataSourceID="ManageAlertSqlDataSource">
                <Columns>
                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type" />
                    <asp:BoundField DataField="code" HeaderText="Code" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" />
                    <asp:BoundField DataField="base" HeaderText="Base" SortExpression="base" />
                    <asp:BoundField DataField="HighValue" HeaderText="High Value" SortExpression="HighValue" />
                    <asp:BoundField DataField="LowValue" HeaderText="Low Value" SortExpression="LowValue" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="ManageAlertSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:HKeInvestConnectionString %>" SelectCommand="SELECT [type], [code], [name], [base], [HighValue], [LowValue] FROM [SecurityHolding] WHERE ([accountNumber] = @accountNumber AND ([HighValue] IS NOT NULL OR [LowValue] IS NOT NULL))">
                <SelectParameters>
                    <asp:SessionParameter Name="accountNumber" SessionField="AccountNumber" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

        <div class="form-group">
            <hr />
        </div>
    </div>
</asp:Content>
