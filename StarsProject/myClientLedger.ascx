<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myClientLedger.ascx.cs" Inherits="StarsProject.ClientLedger" %>


<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnDebit" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
<asp:HiddenField ID="hdnCredit" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />

<table id="tblClientLedger" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
<asp:Repeater ID="rptClientLedger" runat="server" OnItemDataBound="rptClientLedger_ItemDataBound">
    <HeaderTemplate>
        <thead>
            <tr>
                <th style="text-align:left !important;">Customer Name</th>
                <th style="text-align:right !important;">Debit Amount</th>
                <th style="text-align:right !important;">Credit Amount</th>
                <th style="text-align:right !important;">Closing Amount</th>
            </tr>
        </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="blueShed">
            <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%#Eval("CustomerID") %>' />
            <asp:HiddenField ID="hdnItemDebit" runat="server" ClientIDMode="Static" Value='<%#Eval("DebitAmount") %>' />
            <asp:HiddenField ID="hdnItemCredit" runat="server" ClientIDMode="Static" Value='<%#Eval("CreditAmount") %>' />
            <asp:HiddenField ID="hdnClosingAmount" runat="server" ClientIDMode="Static" Value='<%#Eval("ClosingAmount") %>' />
            <td style="text-align:left !important;"><%# Eval("CustomerName") %>&nbsp;&nbsp;
                <small><a class="tooltipped" data-position="right" data-tooltip="Show Detailed Ledger" href="javascript:viewLedgerDetail(<%# Eval("CustomerID")%>,'<%# Eval("CustomerName") %>');">(Detailed Ledger)</a></small>
            </td>
            <td style="text-align:right !important;"><%# Eval("DebitAmount") %></td>
            <td style="text-align:right !important;"><%# Eval("CreditAmount") %></td>
            <td id="tdClosing" style="text-align:right !important;"><%# Eval("ClosingAmount") %></td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>

<div id="mySampleDiv">
</div>