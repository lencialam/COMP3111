<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientSecurityHoldingsDetails.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.ClientSecurityHoldingsDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Holding Details</h2>
    <div>
        <div class="form-horizontal">
            <asp:Label runat="server" Text="Account Number:"></asp:Label>

            <asp:Label ID="ClientAccountNum" runat="server"></asp:Label>
            <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged">
                <asp:ListItem Value="0">Security type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit trust</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" Visible="False">
                <asp:ListItem Value="0">Currency</asp:ListItem>
            </asp:DropDownList>
        </div>
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
