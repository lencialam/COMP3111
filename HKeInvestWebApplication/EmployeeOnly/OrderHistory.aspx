<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderHistory.aspx.cs" Inherits="HKeInvestWebApplication.EmployeeOnly.OrderHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Order History</h2>
    <div>
        <div class="form-horizontal">
            <asp:Label runat="server" Text="Account Number:" AssociatedControlID="txtAccountNumber"></asp:Label>
            <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="10"></asp:TextBox>
            <asp:RegularExpressionValidator ID="ANvalidator" runat="server" ControlToValidate="txtAccountNumber" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide valid Account Number" ValidationExpression="^[A-Z]{2}[0-9]{8}$"></asp:RegularExpressionValidator>
        </div>
        <div class="form-horizontal">

            <asp:Label runat="server" Text="Date: From" AssociatedControlID="DateFrom"></asp:Label>
            <asp:TextBox ID="DateFrom" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="DateFromValidator" runat="server" ControlToValidate="DateFrom" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Date From is not valid."
                ValidationExpression="^(((0[1-9]|1\d|2[0-8])\/(0[1-9]|1[012])|(29|30)\/(0[13456789]|1[012])|31\/(0[13578]|1[02]))\/\d{4}|29\/02\/((\d{2})(0[48]|[2468][048]|[13579][26])|(([02468][048]|[13579][26])00)))$" Display="Dynamic"></asp:RegularExpressionValidator>
            <asp:Label runat="server" Text="To" AssociatedControlID="DateTo"></asp:Label>
            <asp:TextBox ID="DateTo" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="DateToValidator" runat="server" ControlToValidate="DateTo" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Date To is not valid."
                ValidationExpression="^(((0[1-9]|1\d|2[0-8])\/(0[1-9]|1[012])|(29|30)\/(0[13456789]|1[012])|31\/(0[13578]|1[02]))\/\d{4}|29\/02\/((\d{2})(0[48]|[2468][048]|[13579][26])|(([02468][048]|[13579][26])00)))$" Display="Dynamic"></asp:RegularExpressionValidator>
        </div>
        <asp:Label ID="lblDateError" runat="server" Visible="False"></asp:Label>
        <div runat="server" id="Filtering" visible="False">
            <div class="form-horizontal">
                <asp:Label runat="server" Text="You may filter the results with following criteria"></asp:Label>
            </div>
            <div class="col-md-4">
                <asp:RadioButtonList ID="BuySell" runat="server" CssClass="table-responsive">
                    <asp:ListItem Value="all" Selected="True">All (Buy and Sell)</asp:ListItem>
                    <asp:ListItem Value="buy">Buy type orders</asp:ListItem>
                    <asp:ListItem Value="sell">Sell type orders</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="col-md-4">
                <asp:CheckBoxList ID="OrderStatus" runat="server">
                    <asp:ListItem Value="completed">Completed</asp:ListItem>
                    <asp:ListItem Value="partial">Partial</asp:ListItem>
                    <asp:ListItem Value="pending">Pending</asp:ListItem>
                    <asp:ListItem Value="cancelled">Cancelled</asp:ListItem>
                </asp:CheckBoxList>
            </div>
            <div class="col-md-8">
                <asp:CheckBox ID="FilterByType" Text="Filter the results by security type and code." runat="server" />
            </div>
            <div class="col-md-8">
                <asp:DropDownList ID="ddlSecurityType" runat="server">
                    <asp:ListItem Value="0">Security type</asp:ListItem>
                    <asp:ListItem Value="bond">Bond</asp:ListItem>
                    <asp:ListItem Value="stock">Stock</asp:ListItem>
                    <asp:ListItem Value="unit trust">Unit trust</asp:ListItem>
                </asp:DropDownList>
                <asp:Label runat="server" Text="Security Code:" AssociatedControlID="txtSecurityCode"></asp:Label>
                <asp:TextBox ID="txtSecurityCode" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-12">
            <asp:Label ID="lblResultMessage" runat="server" Visible="False" ForeColor="Red"></asp:Label>
        </div>
        <div class="col-md-offset-2 col-md-10">
            <asp:Button ID="Check" runat="server" Text="Check" CssClass="btn button-default" OnClick="Check_Click" />
        </div>


        <div class="col-md-12">
            <asp:Label ID="lblAccountMessage" runat="server" Visible="False"></asp:Label>
        </div>

        <div>
            <asp:GridView ID="gvOrder" runat="server" AutoGenerateColumns="False" OnSorting="gvOrder_Sorting" Visible="False" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="OrderNumber" HeaderText="Reference No." ReadOnly="True" />
                    <asp:BoundField DataField="buysell" HeaderText="Buy/Sell" ReadOnly="True" />
                    <asp:BoundField DataField="SecurityType" HeaderText="Type" ReadOnly="True" SortExpression="securitytype" />
                    <asp:BoundField DataField="SecurityCode" HeaderText="Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="DateOfSubmission" HeaderText="Date of Submission" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="True" SortExpression="dateofsubmission" />
                    <asp:BoundField DataField="CurrentStatus" HeaderText="Current Status" ReadOnly="True" SortExpression="currentstatus" />
                    <asp:BoundField DataField="TotalShares" HeaderText="Total No. of Shares" ReadOnly="True" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Executed Dollar Amount (in HKD)" DataFormatString="{0:n2}" ReadOnly="True" />
                    <asp:BoundField DataField="Fee" HeaderText="Total Fee Charged (in HKD)" ReadOnly="True" />
                </Columns>
            </asp:GridView>
        </div>
        <div runat="server" id="trans" visible="False">
            <h4>Transaction Details For each Order</h4>
            <div>
                <asp:GridView ID="gvTransaction" runat="server" AutoGenerateColumns="False" Visible="False">
                    <Columns>
                        <asp:BoundField DataField="OrderNumber" HeaderText="Order Reference No." ReadOnly="True" />
                        <asp:BoundField DataField="TransactionNumber" HeaderText="Transaction Number" ReadOnly="True" />
                        <asp:BoundField DataField="ExecuteDate" HeaderText="Executed Date" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="True" />
                        <asp:BoundField DataField="ExecuteShares" HeaderText="Quantity of Shares Executed" ReadOnly="True" />
                        <asp:BoundField DataField="ExecutePrice" HeaderText="Price Paid per Share" ReadOnly="True" />
                        <asp:BoundField DataField="BaseCurrency" HeaderText="Base Currency" ReadOnly="True" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
