<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyOrderBillingStatus.ascx.cs" Inherits="StarsProject.MyOrderBillingStatus" %>

<%--<link href="css/Registration.css" rel="stylesheet" type="text/css" />
<link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />--%>

<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<table id="tblInqProductGroup" class="stdtable" border="0" width="100%">
    <asp:Repeater ID="rptApproval" runat="server" OnItemDataBound="rptApproval_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="text-center">Order #</th>
                    <th class="text-center">Order Date</th>
                    <th style="text-align: left !important;">Customer Name</th>
                    <th style="text-align: left !important;">Sales Person</th>
                    <th class="text-left">Bill #</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                <asp:HiddenField ID="hdnCreatedBy" runat="server" ClientIDMode="Static" Value='<%#Eval("CreatedBy") %>' />
                <td class="text-center">
                    <a href="javascript:viewSalesOrder(<%# Eval("pkID")%>);"><%# Eval("OrderNo") %></a>
                </td>
                <td class="text-center"><%# Eval("OrderDate", "{0:dd/MM/yyyy}") %></td>
                <td style="text-align: left !important;"><%# Eval("CustomerName") %></td>
                <td style="text-align: left !important;"><%# Eval("EmployeeName") %></td>
                <td style="text-align: left !important;"><%# Eval("BillNo") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>

