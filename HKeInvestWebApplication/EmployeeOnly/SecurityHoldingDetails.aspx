<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurityHoldingDetails.aspx.cs" Inherits="HKeInvestWebApplication.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Holding Details</h2>
    <div>
        <div class="form-horizontal">
            <asp:Label runat="server" Text="Account Number:" AssociatedControlID="txtAccountNumber"></asp:Label>
            <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="10"></asp:TextBox>
            <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged" CausesValidation="True">
                <asp:ListItem Value="0">Security type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit trust</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" Visible="False" CausesValidation="True">
                <asp:ListItem Value="0">Currency</asp:ListItem>
            </asp:DropDownList>
        </div>
        <asp:RegularExpressionValidator ID="ANvalidator" runat="server" ControlToValidate="txtAccountNumber" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide valid Account Number" ValidationExpression="^[A-Z]{2}[0-9]{8}$"></asp:RegularExpressionValidator>
        <div>
            <div class="form-horizontal">
                <asp:Label ID="lblClientName" runat="server" Visible="False"></asp:Label>
            </div>
            <div class="form-horizontal">
                <asp:Label ID="lblResultMessage" runat="server" Visible="False"></asp:Label>
            </div>
        </div>
        <div>
            <asp:GridView ID="gvSecurityHolding" runat="server" AutoGenerateColumns="False" OnSorting="gvSecurityHolding_Sorting" Visible="False" Width="639px" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Shares" ReadOnly="True" SortExpression="shares" />
                    <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="True" />
                    <asp:BoundField DataField="price" DataFormatString="{0:n2}" HeaderText="Price" ReadOnly="True" />
                    <asp:BoundField DataField="value" DataFormatString="{0:n2}" HeaderText="Value" ReadOnly="True" SortExpression="value" />
                    <asp:BoundField DataField="convertedValue" DataFormatString="{0:n2}" HeaderText="Value in" ReadOnly="True" SortExpression="convertedValue" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
