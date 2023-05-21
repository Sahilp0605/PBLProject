<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myOutstandingBills.ascx.cs" Inherits="StarsProject.myOutstandingBills" %>
<asp:HiddenField ID="hdnAgeing" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnCategory" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />

<asp:HiddenField ID="hdnAge1" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnAge2" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnAge3" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnAge4" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnAge5" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnAge6" runat="server" ClientIDMode="Static" EnableViewState="true" />

<table id="tblOutstanding" class="stdtable" border="0" width="100%">
    <asp:Repeater ID="rptOutstanding" runat="server">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="center-align">Bill #</th>
                    <th class="center-align">Bill Date</th>
                    <th class="left-align">Customer Name</th>
                    <th class="right-align">Bill Amount</th>
                    <%--<th class="right-align">Received</th>
                    <th class="right-align">DB Note</th>
                    <th class="right-align">CR Note</th>--%>
                    <th class="right-align">Balance Amt</th>
                    <th class="center-align">Status</th>
                    <th class="center-align">Due On</th>
                    <% if (hdnAgeing.Value == "") { %>
                    <th class="center-align">Overdue Days</th>
                    <% } else { %>
                    <th class="center-align"> Below <%= hdnAge2.Value %></th>
                    <th class="center-align"><%= hdnAge3.Value %> To <%= hdnAge4.Value %></th>
                    <th class="center-align"><%= hdnAge5.Value %> To <%= hdnAge6.Value %></th>
                    <th class="center-align">Above <%= hdnAge6.Value %></th>
                    <% }  %>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnBillStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("BillStatus") %>' />
                <asp:HiddenField ID="hdnBillAmount" runat="server" ClientIDMode="Static" Value='<%#Eval("BillAmount") %>' />
                <asp:HiddenField ID="hdnReceivedAmount" runat="server" ClientIDMode="Static" Value='<%#Eval("ReceivedAmount") %>' />

                <td class="center-align"><%# Eval("InvoiceNo") %></td>
                <td class="center-align"><%# Eval("InvoiceDate", "{0:dd/MM/yyyy}") %></td>
                <td class="left-align"><%# Eval("CustomerName") %>
                    <small><a class="tooltipped" data-position="right" data-tooltip="Show Detailed Ledger" href="javascript:viewLedgerDetail(<%# Eval("CustomerID")%>,'<%# Eval("CustomerName") %>');">(Detailed Ledger)</a></small>
                </td>
                <td class="right-align"><%# Eval("BillAmount") %></td>
                <%--<td class="right-align"><%# Eval("ReceivedAmount") %></td>
                <td class="right-align"><%# Eval("DBAmount") %></td>
                <td class="right-align"><%# Eval("CRAmount") %></td>--%>
                <td class="right-align"><%# Eval("BalanceAmount") %></td>
                <td class="center-align"><%# Eval("BillStatus") %></td>
                <td class="center-align"><%# Eval("DueDate", "{0:dd/MM/yyyy}") %></td>
                <% if (hdnAgeing.Value == "") { %>
                <th class="center-align"><%# Eval("OverdueDays") %></th>
                <% } else { %>
                <th class="center-align"><%# Eval("Slab1") %></th>
                <th class="center-align"><%# Eval("Slab2") %></th>
                <th class="center-align"><%# Eval("Slab3") %></th>
                <th class="center-align"><%# Eval("Slab4") %></th>
                <% }  %>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
