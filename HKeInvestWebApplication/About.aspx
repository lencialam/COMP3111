<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="HKeInvestWebApplication.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>About the Addtional Feature: Watchlist</h3>
    <div class="form-horizontal">
        <div class="form-group">
            <h4>Description</h4>
            <p>In the world of business, time is money. Wasting time on checking the prices of securities by searching them one by one is totally unnecessary and it could cost the Client many precious opprotunities to profit.</p>
            <p>This feature enables the Client to add the securities that he/she is interested into the Watchlist so that the Client could easily check these securities instead of searching them one by one. Moreover, the Client could simply choose one of the securities in the list to buy it directly. Watchlist leverages the user experience for the Clients by allowing them to check the prices of several securities in a easier and quicker way and possibly could help the Clients seize more opportunities to profit by making better buying decisions.</p>
            <p>This feature requires the system to allow the Client to add a specific security into the Watchlist after searching it. The Watchlist should display the basic information of the securities including its type, code, name and price. It should also allow the Client to delete a security from the Watchlist. </p>
        </div>
        <div class="form-group">
            <h4>How to use the additional feature</h4>
            <p>1. Add security to the Watchlist</p>
            <p>After login, the Client could search the security and view the detail of it. And in the detail view for the security, there is a button for the Client to add the security to the Watchlist</p>
            <p>2. Manage the Watchlist</p>
            <p>After login, the Client could see a hyperlink named ‘My Watchlist’ in the main page (or ‘Default’ page). And the Client is able to view and delete any Security in the Watchlist.</p>
        </div>
    </div>
</asp:Content>
