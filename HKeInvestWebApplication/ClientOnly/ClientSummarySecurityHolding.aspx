<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientSummarySecurityHolding.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.ClientSummarySecurityHolding" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Summary of Security Holding</h2>
    <div>
        <div class="form-horizontal">
            <asp:Label runat="server" Text="Account Number:"></asp:Label>
            <asp:Label ID="ClientAccountNum" runat="server"></asp:Label>
            <asp:Button ID="Check" runat="server" Text="Check" CssClass="btn button-default" OnClick="Check_Click" />
        </div>
        <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" Visible="False" CausesValidation="True">
                <asp:ListItem Value="0">Currency</asp:ListItem>
        </asp:DropDownList>
        <div>
            <div class="form-horizontal">
                <asp:Label ID="lblAccountNumber" runat="server" Visible="False"></asp:Label>
            </div>
            <div class="form-horizontal">
                <asp:Label ID="lblClientName" runat="server" Visible="False"></asp:Label>
            </div>
            <div class="form-horizontal">
                <asp:Label ID="lblTotalValue" runat="server" Visible="False"></asp:Label>
            </div>
            <div class="form-horizontal">
                <asp:Label ID="lblTotalConvertedValue" runat="server" Visible="False"></asp:Label>
            </div>
            <div class="form-horizontal">
                <asp:Label ID="lblFreeBalance" runat="server" Visible="False"></asp:Label>
            </div>
            <div class="form-horizontal">
                <asp:Label ID="lblConvertedFreeBalance" runat="server" Visible="False"></asp:Label>
            </div>
        </div>
        <div>
            <asp:GridView ID="gvSummary" runat="server" Visible="False" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="type" HeaderText="Security Type" ReadOnly="True" />
                    <asp:BoundField DataField="value" DataFormatString="{0:n2}" HeaderText="Total Monetary Value (in HKD)" ReadOnly="True" />
                    <asp:BoundField DataField="convertedvalue" DataFormatString="{0:n2}" HeaderText="Total in " ReadOnly="True" Visible="False" />
                    <asp:BoundField DataField="date" HeaderText="Submission date of last executed order" ReadOnly="True" />
                    <asp:BoundField DataField="value_of_last" HeaderText="Total Monetary Value of last executed order (in HKD)" ReadOnly="True" />
                    <asp:BoundField DataField="convertedvalue_of_last" HeaderText="Total of last executed order in " ReadOnly="True" Visible="False" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="form-horizontal" runat="server" id="links" visible="false">
            <asp:Button ID="stock" runat="server" Text="Check Stock details" CssClass="btn button-default" OnClick="Stock_Click" />
            <asp:Button ID="bond" runat="server" Text="Check Bond details" CssClass="btn button-default" OnClick="Bond_Click" />
            <asp:Button ID="ut" runat="server" Text="Check Unit Trust details" CssClass="btn button-default" OnClick="UT_Click" />
        </div>
    </div>
</asp:Content>
