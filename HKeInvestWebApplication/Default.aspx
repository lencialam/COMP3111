<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HKeInvestWebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Team 115</h1>
        <p class="lead">Team 115 is a cool team for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
    </div>

    <div class="form-horizontal">
        <div class="form-group" runat="server" id="AnyoneSearchDiv">
            <a class="btn btn-default" href="SecuritySearching.aspx">Search Securities &raquo;</a>
        </div>
        <div class="form-group" runat="server" id="ClientSearchDiv">
            <a class="btn btn-default" href="ClientOnly/ClientSecuritySearching.aspx">Search Securities &raquo;</a>
        </div>
        <div class="form-group">
            <a class="btn btn-default" href="AccountApplicationForm.pdf" target="_blank">Download Account Application Form &raquo;</a>
        </div>
        <div class="form-group">
            <a class="btn btn-default" href="fee.png" target="_blank">Download Table for Service Fee &raquo;</a>
        </div>
    </div>

    <div class="row">
        <div runat="server" class="col-md-6" id="ClientUI">
            <h2>Client Only</h2>
            <p>
                Already have an account? Log in your client account to check updates of your securities!
            </p>
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/ClientManageClientInfo.aspx" runat="server">Manage Client Information</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/SecurityBuyAndSell.aspx" runat="server">Buy and Sell Securities</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/SetAlert.aspx" runat="server">Set Alert for Security Holdings</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/ProfitLoss.aspx" runat="server">Track Profit and Loss</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/Watchlist.aspx" runat="server">My Watchlist</asp:HyperLink>
                </div>
                <div class="form-group">
                    <h3>Report Generation</h3>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/ClientSummarySecurityHolding.aspx" runat="server">Security Holding Summary</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/ClientSecurityHoldingsDetails.aspx" runat="server">Security Holding Details</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/ClientShowActiveOrders.aspx" runat="server">Active Orders Summary</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="ClientOnly/ClientOrderHistory.aspx" runat="server">Historic Orders Summary</asp:HyperLink>
                </div>
            </div>
        </div>


        <div runat="server" class="col-md-6" id="EmployeeUI">
            <h2>Employee Only</h2>
            <p>
                Employees can login here to generate reports!
            </p>
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:HyperLink href="EmployeeOnly/ManageClientInfo.aspx" runat="server">Manage Client Information</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="EmployeeOnly/AccountApplicationForm.aspx" runat="server">Account Application</asp:HyperLink>
                </div>
                <div class="form-group">
                    <h3>Report Generation</h3>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="EmployeeOnly/SummarySecurityHolding.aspx" runat="server">Security Holding Summary</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="EmployeeOnly/SecurityHoldingDetails.aspx" runat="server">Security Holding Details</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="EmployeeOnly/ShowActiveOrders.aspx" runat="server">Active Orders Summary</asp:HyperLink>
                </div>
                <div class="form-group">
                    <asp:HyperLink href="EmployeeOnly/OrderHistory.aspx" runat="server">Historic Orders Summary</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
