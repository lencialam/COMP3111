﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowActiveOrders.aspx.cs" Inherits="HKeInvestWebApplication.EmployeeOnly.ShowActiveOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Active Orders</h2>
    <div>
        <div class="form-horizontal">
            <asp:Label runat="server" Text="Account Number:" AssociatedControlID="txtAccountNumber"></asp:Label>
            <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="10"></asp:TextBox>
            <asp:Button ID="Check" runat="server" Text="Check" CssClass="btn button-default" OnClick="Check_Click" />
        </div>
        <asp:RegularExpressionValidator ID="ANvalidator" runat="server" ControlToValidate="txtAccountNumber" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please provide valid Account Number" ValidationExpression="^[A-Z]{2}[0-9]{8}$"></asp:RegularExpressionValidator>
        <div class="form-horizontal">
            <asp:Label ID="lblAccountNumber" runat="server" Visible="False"></asp:Label>
        </div>
        <div class="form-horizontal">
            <asp:Label ID="lblResultMessage" runat="server" Visible="False"></asp:Label>
        </div>
        <div>
            <asp:GridView ID="gvActiveOrders" runat="server" AutoGenerateColumns="False" Visible="False" Width="639px">
                <Columns>
                    <asp:BoundField DataField="OrderNumber" HeaderText="Reference No." ReadOnly="True" />
                    <asp:BoundField DataField="buysell" HeaderText="Buy/Sell" ReadOnly="True" />
                    <asp:BoundField DataField="SecurityType" HeaderText="Type" ReadOnly="True" />
                    <asp:BoundField DataField="SecurityCode" HeaderText="Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" />
                    <asp:BoundField DataField="DateOfSubmission" HeaderText="Date of Submission" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="True" />
                    <asp:BoundField DataField="CurrentStatus" HeaderText="Current Status" ReadOnly="True" />

                    <asp:BoundField DataField="QuantityOfShares_Stock" HeaderText="Quantity Of Shares (for stock)" ReadOnly="True" />
                    <asp:BoundField DataField="ExpiryDate" HeaderText="ExpiryDate (for stock)" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="True" />
                    <asp:BoundField DataField="StockOrderType" HeaderText="Stock Order Type" ReadOnly="True" />
                    <asp:BoundField DataField="HBLS_Price" HeaderText="Highest Buying / Lowest Selling price (for stock)" ReadOnly="True" />
                    <asp:BoundField DataField="StopPrice" HeaderText="Stop Price" ReadOnly="True" />
                    <asp:BoundField DataField="DollarAmount" HeaderText="Dollar Amount in HKD (for buying bond/unit trust)" ReadOnly="True" />
                    <asp:BoundField DataField="QuantityOfShares_BUT" HeaderText="Quantity Of Shares (for selling bond/unit trust)" ReadOnly="True" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
