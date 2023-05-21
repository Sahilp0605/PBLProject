<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myEmployeeExpnLedger.ascx.cs" Inherits="StarsProject.myEmployeeExpnLedger" %>
<asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnClosing" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
<table id="tblExpenseLedger" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
<asp:Repeater ID="rptExpnLedgerDetail" runat="server" OnItemDataBound="rptExpnLedgerDetail_ItemDataBound">
    <HeaderTemplate>
        <thead>
            <tr style="font-size:12px !important;">
                <th class="center-align">Date.</th>
                <th class="center-align">Description</th>
                <th class="center-align">Category</th>
                <th class="center-align">Expn./Trnas.</th>
                <th class="right-align">&nbsp;</th>
                <th class="right-align">Debit</th>
                <th class="right-align">Credit</th>
                <th class="right-align">Closing</th>
            </tr>
            <tr style="background-color:#e4e4e4 !important; color:black !important;">
                <th class="right-align" colspan="4" style="background-color:#e4e4e4 !important; color:black !important;">Opening Balance : </th>
                <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;">&nbsp;</th>
                <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdHeaderDebit" runat="server">&nbsp;</th>
                <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdHeaderCredit" runat="server">&nbsp;</th>
                <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdHeaderClosing" runat="server">&nbsp;</th>
            </tr>
        </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:HiddenField ID="hdnTransCategory" runat="server" ClientIDMode="Static" Value='<%#Eval("TransCategory") %>' />
        <asp:HiddenField ID="hdnDbAmount" runat="server" ClientIDMode="Static" Value='<%#Eval("DebitAmount") %>' />
        <asp:HiddenField ID="hdnCrAmount" runat="server" ClientIDMode="Static" Value='<%#Eval("CreditAmount") %>' />
        <tr class="white black-text">
            <td class="center-align"><%# Eval("TransDate", "{0:dd-MMM-yyyy}") %></td>
            <td class="center-align"><%# Eval("Description") %></td>
            <td class="center-align"><%# Eval("TransCategory") %></td>
            <td class="center-align"><%# Eval("TransType") %></td>
            <td class="right-align">&nbsp;</td>
            <td class="right-align"><%# Eval("DebitAmount") %></td>
            <td class="right-align"><%# Eval("CreditAmount") %></td>
            <td class="right-align" id="tdClosing" runat="server"></td>
         </tr>
    </ItemTemplate>
    <FooterTemplate>
        <tr>
            <th class="right-align" colspan="4" style="background-color:#e4e4e4 !important; color:black !important;">Closing Balance : </th>
            <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;">&nbsp;</th>
            <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdFooterDebit" runat="server">&nbsp;</th>
            <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdFooterCredit" runat="server">&nbsp;</th>
            <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdFooterClosing" runat="server">&nbsp;</th>
        </tr>
    </FooterTemplate>
</asp:Repeater>
</table>