<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myLeaveBalances.ascx.cs" Inherits="StarsProject.myLeaveBalances" %>
<asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLeaveTypeID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
<style type="text/css">
    th { background-color: inherit; color:white; font-size:14px; }
    td { font-size:14px; }
</style>
<table id="tblLeaveBalances" class="table table-striped table-bordered mt-2 mb-2" style="font-size:12px;" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptBalances" runat="server" OnItemDataBound="rptBalances_ItemDataBound">
        <HeaderTemplate>
            <thead class="thead-dark" style="font-size:12px;">
                <tr style="background: linear-gradient(45deg, #3949ab, #4fc3f7) !important;">
                    <th class="center-align white-text navy">Leave Type</th>
                    <th class="center-align white-text navy">Leave Code</th>
                    <th class="center-align white-text navy">Opening Balance</th>
                    <th class="center-align white-text navy">Leave Earned</th>
                    <th class="center-align white-text navy">Leave Used</th>
                    <th class="center-align white-text navy">Closing Balance</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnEmpID" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeID") %>' />        
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                <asp:HiddenField ID="hdnOpeningBal" runat="server" ClientIDMode="Static" Value='<%#Eval("OpeningBal") %>' />        
                <asp:HiddenField ID="hdnEarned" runat="server" ClientIDMode="Static" Value='<%#Eval("Earned") %>' />        
                <asp:HiddenField ID="hdnUsed" runat="server" ClientIDMode="Static" Value='<%#Eval("Used") %>' />        
                <asp:HiddenField ID="hdnClosingBal" runat="server" ClientIDMode="Static" Value='<%#Eval("ClosingBal") %>' />        

                <td class="center-align"><%# Eval("LeaveType") %></td>
                <td class="center-align"><%# Eval("LeaveCode") %></td>
                <td class="center-align"><%# Eval("OpeningBal") %></td>
                <td class="center-align"><%# Eval("Earned") %></td>
                <td class="center-align"><%# Eval("Used") %></td>
                <td class="center-align"><%# Eval("ClosingBal") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
