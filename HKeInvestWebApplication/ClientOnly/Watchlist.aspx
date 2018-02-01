<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Watchlist.aspx.cs" Inherits="HKeInvestWebApplication.Watchlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>My Watchlist</h2>
    <hr />
    <div class="form-horizontal">
        <div class="form-group" runat="server" id="msgDiv">
            <asp:Label ID="errormsg" runat="server" Visible="false" CssClass="text-danger"></asp:Label>
            <asp:Label ID="successmsg" runat="server" Visible="false" CssClass="bg-success"></asp:Label>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Security Type:" CssClass="control-label col-md-2" AssociatedControlID="SecurityType"></asp:Label>
            <div class="col-md-4">
                <asp:DropDownList ID="SecurityType" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="SecurityType_SelectedIndexChanged">
                    <asp:ListItem Value="all">All</asp:ListItem>
                    <asp:ListItem Value="stock">Stock</asp:ListItem>
                    <asp:ListItem Value="bond">Bond</asp:ListItem>
                    <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label6" runat="server" Text="Convert to Currency:" CssClass="control-label col-md-2" AssociatedControlID="ddlCurrency"></asp:Label>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                    <asp:ListItem Value="0">Currency</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <asp:GridView ID="gvSecurityDetails" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvSecurityDetails_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="type" HeaderText="Type" ReadOnly="True" SortExpression="type" />
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="base" HeaderText="Base currency" ReadOnly="True" SortExpression="base" />
                    <asp:BoundField DataField="price" HeaderText="Price in base currency" ReadOnly="True" SortExpression="price" />
                    <asp:BoundField DataField="convertedValue" HeaderText="Value in" ReadOnly="True" SortExpression="convertedValue" Visible="False" />
                    <asp:ButtonField ButtonType="Button" Text="Remove" HeaderText="Remove the security from Watchlist" CommandName="Select" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
