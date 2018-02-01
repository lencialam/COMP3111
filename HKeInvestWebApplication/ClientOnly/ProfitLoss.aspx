<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfitLoss.aspx.cs" Inherits="HKeInvestWebApplication.ProfitLoss" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Profit/Loss Tracking</h2>
    <hr />
    <div class="form-horizontal">
        <div class="form-group" id="msgDiv" runat="server">
            <asp:Label ID="errormsg" runat="server" Visible="false" CssClass="text-danger"></asp:Label>
            <asp:Label ID="successmsg" runat="server" Visible="false" CssClass="bg-success col-md-11"></asp:Label>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Profit/Loss Tracking Type:" CssClass="control-label col-md-3" AssociatedControlID="TrackingType"></asp:Label>
            <div class="col-md-4">
                <asp:DropDownList ID="TrackingType" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="TrackingType_SelectedIndexChanged" CausesValidation="True">
                    <asp:ListItem>Please select</asp:ListItem>
                    <asp:ListItem>For all assets</asp:ListItem>
                    <asp:ListItem>For one type of security</asp:ListItem>
                    <asp:ListItem>For an individual security</asp:ListItem>
                </asp:DropDownList>
                <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" OnServerValidate="CustomValidator2_ServerValidate"></asp:CustomValidator>
            </div>
        </div>
        <div class="form-group" id="SecurityTypeDiv" runat="server">
            <asp:Label ID="Label2" runat="server" Text="Security Type:" AssociatedControlID="SecurityType" CssClass="control-label col-md-3"></asp:Label>
            <div class="col-md-4">
                <asp:DropDownList ID="SecurityType" runat="server" CssClass="form-control" OnSelectedIndexChanged="SecurityType_SelectedIndexChanged" AutoPostBack="True" CausesValidation="True">
                    <asp:ListItem>Please select</asp:ListItem>
                    <asp:ListItem Value="stock">Stock</asp:ListItem>
                    <asp:ListItem Value="bond">Bond</asp:ListItem>
                    <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                </asp:DropDownList>
                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
            </div>
        </div>
        <div class="form-group" id="ddlCurrencyDiv" runat="server">
            <asp:Label ID="Label6" runat="server" Text="Convert to Currency:" CssClass="control-label col-md-3" AssociatedControlID="ddlCurrency"></asp:Label>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                    <asp:ListItem Value="0">Currency</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group" id="IndividualSecurityPLDiv" runat="server">
            <asp:DetailsView ID="dvIndividualPL" runat="server" Height="50px" Width="650px" AutoGenerateRows="False">
                <Fields>
                    <asp:BoundField DataField="type" HeaderText="Type" ReadOnly="True" SortExpression="type" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="shares" HeaderText="Shares held" ReadOnly="True" SortExpression="share" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="buy" HeaderText="Total dollar amount from buying(HKD)" ReadOnly="True" SortExpression="buy" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField HeaderText="Total dollar amount from buying" ReadOnly="True" SortExpression="buyforeign" Visible="False" DataField="buyforeign" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="sell" HeaderText="Total dollar amount from selling(HKD)" ReadOnly="True" SortExpression="sell" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="sellforeign" HeaderText="Total dollar amount from selling" ReadOnly="True" SortExpression="sellforeign" Visible="False" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="fee" HeaderText="Total fees paid(HKD)" ReadOnly="True" SortExpression="fee" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="feeforeign" HeaderText="Total fees paid" ReadOnly="True" SortExpression="feeforeign" Visible="False" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="PL" HeaderText="Profit/Loss(HKD)" ReadOnly="True" SortExpression="PL" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="PLforeign" HeaderText="Profit/Loss" ReadOnly="True" SortExpression="PLforeign" Visible="False" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="PLP" HeaderText="Profit/Loss percentage" ReadOnly="True" SortExpression="PLP" HeaderStyle-Font-Bold="true" />
                </Fields>
            </asp:DetailsView>
        </div>
        <div class="form-group" id="SecurityListDiv" runat="server">
            <asp:GridView ID="gvSecurityList" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvSecurityList_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="type" HeaderText="Type" ReadOnly="True" SortExpression="type" />
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:ButtonField ButtonType="Button" CommandName="Select" Text="View Profit/Loss" />
                </Columns>
            </asp:GridView>
        </div>

        <div class="form-group" id="AllSecurityPLDiv" runat="server">
            <asp:GridView ID="gvAllPL" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="buy" HeaderText="Total dollar amount from buying(HKD)" ReadOnly="True" SortExpression="buy" />
                    <asp:BoundField DataField="buyforeign" HeaderText="Total dollar amount from buying" ReadOnly="True" SortExpression="buyforeign" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="sell" HeaderText="Total amount from selling(HKD)" ReadOnly="True" SortExpression="sell" />
                    <asp:BoundField DataField="sellforeign" HeaderText="Total amount from selling" ReadOnly="True" SortExpression="sellforeign" Visible="false" />
                    <asp:BoundField DataField="fee" HeaderText="Total fees paid(HKD)" ReadOnly="True" SortExpression="fee" />
                    <asp:BoundField DataField="feeforeign" HeaderText="Total fees paid" ReadOnly="True" SortExpression="feeforeign" Visible="False" />
                    <asp:BoundField DataField="PL" HeaderText="Total profit/loss(HKD)" ReadOnly="True" SortExpression="PL" />
                    <asp:BoundField DataField="PLforeign" HeaderText="Total profit/loss" ReadOnly="True" SortExpression="PLforeign" Visible="False" />
                    <asp:BoundField DataField="PLP" HeaderText="Total profit/loss percentage" ReadOnly="True" SortExpression="PLP" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="form-group" id="OneTypeSecurityPLDiv" runat="server">
            <asp:GridView ID="gvOneTypePL" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="type" HeaderText="Type" ReadOnly="True" SortExpression="type" />
                    <asp:BoundField DataField="buy" HeaderText="Total amount from buying(HKD)" ReadOnly="True" SortExpression="buy" />
                    <asp:BoundField DataField="buyforeign" HeaderText="Total amount from buying" ReadOnly="True" SortExpression="buyforeign" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="sell" HeaderText="Total amount from selling(HKD)" ReadOnly="True" SortExpression="sell" />
                    <asp:BoundField DataField="sellforeign" HeaderText="Total amount from selling" ReadOnly="True" SortExpression="sellforeign" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="fee" HeaderText="Total fees paid(HKD)" ReadOnly="True" SortExpression="fee" />
                    <asp:BoundField DataField="feeforeign" HeaderText="Total fees paid" ReadOnly="True" SortExpression="feeforeign" Visible="False" />
                    <asp:BoundField DataField="PL" HeaderText="Total profit/loss(HKD)" ReadOnly="True" SortExpression="PL" />
                    <asp:BoundField DataField="PLforeign" HeaderText="Total profit/loss" ReadOnly="True" SortExpression="PLforeign" Visible="False" />
                    <asp:BoundField DataField="PLP" HeaderText="Total profit/loss percentage" ReadOnly="True" SortExpression="PLP" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
