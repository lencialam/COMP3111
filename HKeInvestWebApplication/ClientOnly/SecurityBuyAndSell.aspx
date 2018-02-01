<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurityBuyAndSell.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.SecurityBuyAndSell" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Buy & Sell</h2>
    <h4>Please notice the pop-up window which may be hidden under the current window.</h4>
    <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="text-danger" EnableClientScript="False" />
    <div>
        <asp:Label ID="lblClientName" runat="server"></asp:Label>
        <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblOrderType" runat="server" AssociatedControlID="OrderType" Text="Order Type:"></asp:Label>
        <asp:DropDownList ID="OrderType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="OrderType_SelectedIndexChanged" ForeColor="Black">
            <asp:ListItem Value="0">-- Select order type --</asp:ListItem>
            <asp:ListItem Value="buy">Buy</asp:ListItem>
            <asp:ListItem Value="sell">Sell</asp:ListItem>
        </asp:DropDownList>

        <asp:CustomValidator ID="cvOrderType" runat="server" ControlToValidate="OrderType" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="OrderType is required." OnServerValidate="cvOrderType_ServerValidate">*</asp:CustomValidator>

        <asp:Label ID="lblSecurityType" runat="server" AssociatedControlID="SecurityType" Text="Security Type:" Width="102px"></asp:Label>
        <asp:DropDownList ID="SecurityType" runat="server" AutoPostBack="True" ForeColor="Black" OnSelectedIndexChanged="SecurityType_SelectedIndexChanged">
            <asp:ListItem Value="0">-- Select security type --</asp:ListItem>
            <asp:ListItem Value="bond">Bond</asp:ListItem>
            <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            <asp:ListItem Value="stock">Stock</asp:ListItem>
        </asp:DropDownList>
        <asp:CustomValidator ID="cvSecurityType" runat="server" ControlToValidate="SecurityType" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Security type is required." OnServerValidate="cvSecurityType_ServerValidate">*</asp:CustomValidator>
    </div>
    <div>
        <asp:Label ID="lblSecurityCode" runat="server" Text="Security Code:" AssociatedControlID="SecurityCode"></asp:Label>
        <asp:TextBox runat="server" ID="SecurityCode"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="SecurityCode" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Security code is required.">*</asp:RequiredFieldValidator>
    </div>
    <div id="BUT" style="margin-top: 0px">
        <asp:Label ID="lblTotal" runat="server" AssociatedControlID="Total"></asp:Label>
        <asp:TextBox runat="server" ID="Total" Width="128px"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="Total" CssClass="text-danger" Display="Dynamic" ErrorMessage="Total amount is required." EnableClientScript="False" ID="rfvTotal">*</asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="rvTotal" runat="server" ControlToValidate="Total" CssClass="text-danger" Display="Dynamic" EnableClientScript="False">*</asp:RegularExpressionValidator>
    </div>
    <div id="STOCK">
        <div>
            <asp:Label ID="lblStockOrderType" runat="server" AssociatedControlID="StockOrderType" Text="Stock order type:"></asp:Label>
            <asp:DropDownList ID="StockOrderType" runat="server" AutoPostBack="True" ForeColor="Black" OnSelectedIndexChanged="StockOrderType_SelectedIndexChanged">
                <asp:ListItem Value="0">-- Select Stock order  --</asp:ListItem>
                <asp:ListItem Value="market">Market</asp:ListItem>
                <asp:ListItem Value="limit">Limit</asp:ListItem>
                <asp:ListItem Value="stop">Stop</asp:ListItem>
                <asp:ListItem Value="stop limit">Stop Limit</asp:ListItem>
            </asp:DropDownList>

            <asp:Label ID="lblStopPrice" runat="server" Text="Stop Price:" AssociatedControlID="StopPrice"></asp:Label>
            <asp:TextBox runat="server" ID="StopPrice"></asp:TextBox>
            <asp:RegularExpressionValidator ID="rvStopPrice" runat="server" ControlToValidate="StopPrice" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" Enabled="False" ErrorMessage="Stop price should be an positive integer with at most two significant digits." ValidationExpression="&quot;\d+(?:\.\d{1,2})?&quot;">*</asp:RegularExpressionValidator>
            <asp:Label ID="lblLimitPrice" runat="server" Text="Limit Price:" AssociatedControlID="LimitPrice"></asp:Label>
            <asp:TextBox runat="server" ID="LimitPrice"></asp:TextBox>
            <asp:CustomValidator ID="cvStock" runat="server" ControlToValidate="SecurityType" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Security type is required." OnServerValidate="cvStock_ServerValidate">*</asp:CustomValidator>
            <asp:RegularExpressionValidator ID="rvLimitPrice" runat="server" ControlToValidate="LimitPrice" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" Enabled="False" ErrorMessage="Limit price should be an positive integer with at most two significant digits." ValidationExpression="&quot;\d+(?:\.\d{1,2})?&quot;">*</asp:RegularExpressionValidator>
        </div>
        <div>
            <asp:Label ID="lblExpiryDate" runat="server" Text="Expiry date:" AssociatedControlID="ExpiryDate"></asp:Label>
            <asp:DropDownList ID="ExpiryDate" runat="server" ForeColor="Black" AutoPostBack="True">
                <asp:ListItem>-- Select extended date length --</asp:ListItem>
                <asp:ListItem Value="1">1 Day</asp:ListItem>
                <asp:ListItem Value="2">2 Days</asp:ListItem>
                <asp:ListItem Value="3">3 Days</asp:ListItem>
                <asp:ListItem Value="4">4 Days</asp:ListItem>
                <asp:ListItem Value="5">5 Days</asp:ListItem>
                <asp:ListItem Value="6">6 Days</asp:ListItem>
                <asp:ListItem Value="7">7 Days</asp:ListItem>
            </asp:DropDownList>
            <asp:CustomValidator ID="cvExpiryDate" runat="server" ControlToValidate="ExpiryDate" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Security type is required." OnServerValidate="cvExpiryDate_ServerValidate">*</asp:CustomValidator>
            <asp:Label ID="lblAllOrNone" runat="server" Text="All or None:" AssociatedControlID="AllOrNone" Width="81px"></asp:Label>
            <asp:RadioButtonList ID="AllOrNone" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="N">No</asp:ListItem>
                <asp:ListItem Value="Y">Yes</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvAllOrNone" runat="server" ControlToValidate="AllOrNone" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Information of All Or None is required.">*</asp:RequiredFieldValidator>
        </div>
    </div>

    <div>
    </div>

    <div>
        <asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_OnClick" />
    </div>
</asp:Content>
