<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myLeaveRequest.ascx.cs" Inherits="StarsProject.myLeaveRequest" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <table id="tblInqProductGroup" class="table table-striped table-bordered" style="font-size:12px;" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptApproval" runat="server" OnItemDataBound="rptApproval_ItemDataBound">
        <HeaderTemplate>
            <thead class="thead-dark" style="font-size:12px;">
                <tr>
                    <th class="center-align">Employee Name</th>
                    <th class="center-align">Period From</th>
                    <th class="center-align">Period To</th>
                    <th class="center-align">Leave Type</th>
                    <th class="center-align">Reason For Leave</th>
                    <th class="center-align">Paid / Unpaid</th>
                    <th class="center-align">Approval Status</th>
                    <%--<% if (hdnView.Value != "dashboarddaily") { %>--%>
                        <th class="center-align">Approved By</th>
                    <%--<% } %>--%>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                <asp:HiddenField ID='hdnApprovalStatus' runat="server" ClientIDMode="Static" Value='<%#Eval("ApprovalStatus") %>' />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeID") %>' />        
                <asp:HiddenField ID='hdnPaidUnpaid' runat="server" ClientIDMode="Static" Value='<%#Eval("PaidUnpaid") %>' />
                        
                <td class="text-center">
                    <a href="javascript:openDashboardBox('leave','view',<%# Eval("pkID")%>);"><%# Eval("EmployeeName") %></a>
                </td>
                <td class="center-align"><%# Eval("FromDate", "{0:dd/MM/yyyy}") %></td>
                <td class="center-align"><%# Eval("ToDate", "{0:dd/MM/yyyy}") %></td>
                <td class="center-align"><%# Eval("LeaveType") %></td>
                <td class="center-align"><%# Eval("ReasonForLeave") %></td>
                <td class="center-align" style="width:150px;">
                    <% if (hdnView.Value != "dashboarddaily" && (hdnRole.Value == "admin" || hdnRole.Value == "bradmin" || hdnRole.Value == "hradmin"))
                       { %>
                        <asp:DropDownList ID="drpPaidUnpaid" runat="server" ClientIDMode="Static"  class="form-control" TabIndex="1" style="height:inherit; font-size:12px;">
                            <asp:ListItem Text="Paid" Value="paid" />
                            <asp:ListItem Text="UnPaid" Value="unpaid" />
                        </asp:DropDownList>
                    <% } else { %> 
                        <span class="center-align"><%# Eval("PaidUnpaid") %></span>
                    <% } %>
                </td>
                <td class="center-align" style="width:150px;">
                    <% if (hdnView.Value != "dashboarddaily" && (hdnRole.Value == "admin" || hdnRole.Value == "bradmin" || hdnRole.Value == "hradmin"))
                       { %>
                        <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static"  class="form-control" TabIndex="1" style="height:inherit; font-size:12px;">
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="Approved" Value="Approved" />
                            <asp:ListItem Text="Rejected" Value="Rejected" />
                        </asp:DropDownList>
                    <% } else { %> 
                        <%--<center><span class="badge badge-info" style="font-size:12px;"><%# Eval("ApprovalStatus") %></span></center>--%>
                        <span class="center-align"><%# Eval("ApprovalStatus") %></span>
                    <% } %>
                </td>
                <td class="center-align"><%# Eval("ApprovedEmployeeName") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>
    <% if (hdnView.Value != "dashboarddaily" && hdnView.Value != "dashboardhr") { %>
    <div class="modal-footer">
        <asp:Button ID="btnApproveReject" runat="server" ClientIDMode="Static" class="btn btn-primary float-right" Text="Submit Approval Status" OnClick="btnApproveReject_Click" />
    </div>
    <% } %>
