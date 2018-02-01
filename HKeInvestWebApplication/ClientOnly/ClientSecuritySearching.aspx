<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientSecuritySearching.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.ClientSecuritySearching" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Searching</h2>
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label ID="SearchError" runat="server" CssClass="text-danger col-md-11" Visible="False"></asp:Label>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Search Criteria" AssociatedControlID="SearchCriteria" CssClass="control-label col-md-3"></asp:Label>
            <div class="col-md-4">
                <asp:DropDownList ID="SearchCriteria" runat="server" OnSelectedIndexChanged="SearchCriteria_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control">
                    <asp:ListItem Value="type">Security Type only</asp:ListItem>
                    <asp:ListItem Value="typename">Security Type+Security Name</asp:ListItem>
                    <asp:ListItem Value="typecode">Security Type+Security Code</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Security Type" CssClass="control-label col-md-3" AssociatedControlID="SecurityType"></asp:Label>
            <div class="col-md-4">
                <asp:DropDownList ID="SecurityType" runat="server" CssClass="form-control">
                    <asp:ListItem Value="select">Select</asp:ListItem>
                    <asp:ListItem Value="stock">Stock</asp:ListItem>
                    <asp:ListItem Value="bond">Bond</asp:ListItem>
                    <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                </asp:DropDownList>
                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
            </div>
        </div>
        <div id="SecurityNameDiv" class="form-group" runat="server">
            <asp:Label ID="Label4" runat="server" Text="Security Name(could be partial):" AssociatedControlID="SecurityName" CssClass="control-label col-md-3"></asp:Label>
            <div class="col-md-9">
                <asp:TextBox ID="SecurityName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SecurityName" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide the name or partial name of the security."></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group" id="SecurityCodeDiv" runat="server">
            <asp:Label ID="Label5" runat="server" Text="Security Code:" AssociatedControlID="SecurityCode" CssClass="control-label col-md-3"></asp:Label>
            <div class="col-md-9">
                <asp:TextBox ID="SecurityCode" runat="server" MaxLength="4" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Please provide the code of the security." ControlToValidate="SecurityCode"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="SecurityCode" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide a valid code for the security." ValidationExpression="^[0-9]{1,4}$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-3 col-md-10">
                <asp:Button ID="Search" runat="server" Text="Search" CssClass="btn button-default" OnClick="Search_Click" />
            </div>
        </div>
        <div id="IndividualSecurityDetailDiv" runat="server">
            <hr />
            <h4>Security Details</h4>
            <div class="form-group">
                <asp:Label ID="Label6" runat="server" Text="Convert to Currency:" CssClass="control-label col-md-3" AssociatedControlID="ddlCurrencyForIndividual"></asp:Label>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCurrencyForIndividual" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlCurrencyForIndividual_SelectedIndexChanged">
                        <asp:ListItem Value="0">Currency</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:DetailsView ID="StockDetailsView" runat="server" Height="50px" Width="700px" AutoGenerateRows="False" Visible="False">
                    <Fields>
                        <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="close" HeaderText="Most recent closing price per share (HKD)" ReadOnly="True" SortExpression="close" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="closeForeign" HeaderText="Most recent closing price per share" ReadOnly="True" SortExpression="closeForeign" Visible="False" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="changeDollar" HeaderText="The last trading day change in price (HKD)" ReadOnly="True" SortExpression="changeDollar" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="changeDollarForeign" HeaderText="The last trading day change in price " ReadOnly="True" SortExpression="changeDollarForeign" Visible="False" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="changePercent" HeaderText="The last trading day percentage change in price" ReadOnly="True" SortExpression="changePercent" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="volume" HeaderText="The last trading day volume of shares traded" ReadOnly="True" SortExpression="volume" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="high" HeaderText="The high price during the preceding 52 weeks (HKD)" ReadOnly="True" SortExpression="high" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="highForeign" HeaderText="The high price during the preceding 52 weeks" ReadOnly="True" SortExpression="highForeign" Visible="False" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="low" HeaderText="The low price during the preceding 52 weeks (HKD)" ReadOnly="True" SortExpression="low" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="lowForeign" HeaderText="The low price during the preceding 52 weeks" ReadOnly="True" SortExpression="lowForeign" Visible="False" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="peRatio" HeaderText="The price earning (P/E) ratio" ReadOnly="True" SortExpression="peRatio" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="yield" HeaderText="Yield" ReadOnly="True" SortExpression="yield" HeaderStyle-Font-Bold="true" />
                    </Fields>
                </asp:DetailsView>
                <asp:DetailsView ID="BondDetailView" runat="server" Height="50px" Width="700px" AutoGenerateRows="False" Visible="False">
                    <Fields>
                        <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="launchDate" HeaderText="Launch date" ReadOnly="True" SortExpression="launchDate" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="base" HeaderText="Base currency" ReadOnly="True" SortExpression="base" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="size" HeaderText="Total monetary value in base currency" ReadOnly="True" SortExpression="size" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sizeForeign" HeaderText="Total monetary value" ReadOnly="True" SortExpression="sizeForeign" Visible="False" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="price" HeaderText="Price per share in base currency" ReadOnly="True" SortExpression="price" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="priceForeign" HeaderText="Price per share" ReadOnly="True" SortExpression="priceForeign" Visible="False" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="sixMonths" HeaderText="Compound annual growth percentage over the last six months" ReadOnly="True" SortExpression="sixMonths" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="oneYear" HeaderText="Compound annual growth percentage over the last one year" ReadOnly="True" SortExpression="oneYear" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="threeYears" HeaderText="Compound annual growth percentage over the last three years" ReadOnly="True" SortExpression="threeYears" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sinceLaunch" HeaderText="Compound annual growth percentage since launch date" ReadOnly="True" SortExpression="sinceLaunch" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                    </Fields>
                </asp:DetailsView>
                <asp:DetailsView ID="UnitTrustDetailView" runat="server" Height="50px" Width="700px" AutoGenerateRows="False" Visible="False">
                    <Fields>
                        <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="launchDate" HeaderText="Launch date" ReadOnly="True" SortExpression="launchDate" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="base" HeaderText="Base currency" ReadOnly="True" SortExpression="base" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="size" HeaderText="Total monetary value in base currency" ReadOnly="True" SortExpression="size" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sizeForeign" HeaderText="Total monetary value" ReadOnly="True" SortExpression="sizeForeign" Visible="False" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="price" HeaderText="Price per share in base currency" ReadOnly="True" SortExpression="price" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="priceForeign" HeaderText="Price per share" ReadOnly="True" SortExpression="priceForeign" Visible="False" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="riskReturn" HeaderText="Risk/return rating" ReadOnly="True" SortExpression="riskReturn" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sixMonths" HeaderText="Compound annual growth percentage over the last six months" ReadOnly="True" SortExpression="sixMonths" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="oneYear" HeaderText="Compound annual growth percentage over the last one year" SortExpression="oneYear" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="threeYears" HeaderText="Compound annual growth percentage over the last three years" ReadOnly="True" SortExpression="threeYears" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sinceLaunch" HeaderText="Compound annual growth percentage since launch date" ReadOnly="True" SortExpression="sinceLaunch" HeaderStyle-Font-Bold="true">
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:BoundField>
                    </Fields>
                </asp:DetailsView>
            </div>
            <div class="form-group">
                <div class="col-md-offset-3 col-md-10">
                    <asp:Button ID="AddToWachlist" runat="server" Text="Add to Watchlist" CssClass="btn button-default" OnClick="AddToWachlist_Click" />
                </div>
            </div>
            <div class="form-group" runat="server" id="addwatchlistmsgDiv">
                <asp:Label ID="addError" runat="server" Text="You have added this security to your watchlist before." Visible="false" CssClass="text-danger"></asp:Label>
                <asp:Label ID="addSuccess" runat="server" Text="Successfully added to your watchlist." Visible="false" CssClass="bg-success col-md-11"></asp:Label>
            </div>
        </div>
        <div id="SecurityDetailDiv" runat="server">
            <hr />
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Convert to Currency:" CssClass="control-label col-md-3" AssociatedControlID="ddlCurrencyForList"></asp:Label>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCurrencyForList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrencyForList_SelectedIndexChanged" CssClass="form-control">
                        <asp:ListItem Value="0">Currency</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:GridView ID="gvSecurityDetails" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvSecurityDetails_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                        <asp:BoundField DataField="base" HeaderText="Base currency" ReadOnly="True" SortExpression="base" Visible="False" />
                        <asp:BoundField DataField="price" HeaderText="Price" ReadOnly="True" SortExpression="price" />
                        <asp:BoundField DataField="convertValue" HeaderText="Value in" ReadOnly="True" SortExpression="convertValue" Visible="False" />
                        <asp:ButtonField ButtonType="Button" CommandName="Select" HeaderText="View Security details" ShowHeader="True" Text="View details" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
