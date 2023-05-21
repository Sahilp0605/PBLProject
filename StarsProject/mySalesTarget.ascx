<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mySalesTarget.ascx.cs" Inherits="StarsProject.mySalesTarget" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
<asp:Repeater ID="rptApproval" runat="server" OnItemDataBound="rptApproval_ItemDataBound">
    <HeaderTemplate>
        <thead>
            <tr>
                <th class="left-align">Employee Name</th>
                <th class="center-align">Period From</th>
                <th class="center-align">Period To</th>
                <th class="right-align">Target Amount</th>
                <th class="right-align">Amount Achieved</th>
                <th class="center-align">Achieved %</th>
                <th class="center-align">Target Type</th>
            </tr>
        </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="blueShed">
            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
            <asp:HiddenField ID="hdnTargetType" runat="server" ClientIDMode="Static" Value='<%#Eval("TargetType") %>' />
            <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
            <td class="text-center">
                <a href="javascript:viewSalesTarget(<%# Eval("pkID")%>);"><%# Eval("EmployeeName") %></a>
            </td>
            <td class="center-align"><%# Eval("FromDate", "{0:dd/MM/yyyy}") %></td>
            <td class="center-align"><%# Eval("ToDate", "{0:dd/MM/yyyy}") %></td>                
            <td class="right-align"><%# Eval("TargetAmount") %></td>
            <td class="right-align"><%# Eval("AchievedAmount") %></td>
            <td class="center-align"><%# Eval("Percentage") %> %</td>
            <td class="center-align"><%# Eval("TargetType") %></td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
<div class="modal-footer">
    <asp:Button ID="btnApproveReject" runat="server" ClientIDMode="Static" class="btn btn-primary" Text="Submit Order Status" OnClick="btnApproveReject_Click" />
</div>

