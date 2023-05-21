<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myOvertimeApproval.ascx.cs" Inherits="StarsProject.myOvertimeApproval" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnClientUrl" runat="server" ClientIDMode="Static" EnableViewState="true" />

<div class="modal-body float-right" id="divApprovalStatus" runat="server" clientidmode="Static">
    <div class="form-row">
        <asp:DropDownList ID="drpApprovalStatusMain" runat="server" ClientIDMode="Static" Class="form-control" TabIndex="1" Style="height: inherit; width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="drpApprovalStatusMain_SelectedIndexChanged">
            <asp:ListItem Text="-- All --" Value="" />
            <asp:ListItem Text="Pending" Value="Pending" />
            <asp:ListItem Text="Approved" Value="Approved" />
            <asp:ListItem Text="Rejected" Value="Rejected" />
        </asp:DropDownList>
    </div>
</div>

<table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptApproval" runat="server" OnItemDataBound="rptApproval_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="center-align">Sr.#</th>
                    <th class="left-align">Employee Name</th>
                    <th class="center-align">From Date</th>
                    <th class="center-align">From Time</th>
                    <th class="center-align">To Date</th>
                    <th class="center-align">To Time</th>
                    <th class="center-align">Approval Status</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeID") %>' />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                <asp:HiddenField ID="hdnApprovalStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("ApprovalStatus") %>' />
                <asp:HiddenField ID="hdnCreatedBy" runat="server" ClientIDMode="Static" Value='<%#Eval("CreatedBy") %>' />
                <td class="center-align">
                    <a href="javascript:viewOvertime(<%# Eval("pkID")%>);"><%# Eval("pkID") %></a>
                </td>
                <td class="left-align"><%# Eval("EmployeeName") %></td>
                <td class="center-align"><%# Eval("FromDate", "{0:dd/MM/yyyy}") %></td>
                <td class="center-align"><%# Eval("FromDate", "{0:HH:mm}") %></td>
                <td class="center-align"><%# Eval("ToDate", "{0:dd/MM/yyyy}") %></td>
                <td class="center-align"><%# Eval("ToDate", "{0:HH:mm}") %></td>
                <td class="center-align">
                    <% if (!hdnClientUrl.Value.Contains("dashboard.aspx") && !hdnClientUrl.Value.Contains("dashboarddaily.aspx") && !hdnClientUrl.Value.Contains("dashboardhr.aspx"))
                       { %>
                        <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="1" Style="height: 1.70rem; margin: 0px; padding: 0px 0px 0px 10px;">
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="Approved" Value="Approved" />
                            <asp:ListItem Text="Rejected" Value="Rejected" />
                        </asp:DropDownList>
                    <% } else { %>
                            <%# Eval("ApprovalStatus") %>
                    <% } %>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<div class="modal-footer mt-2">
    <asp:Button ID="btnApproveReject" runat="server" ClientIDMode="Static" class="btn btn-primary float-right" Text="Submit Order Status" OnClick="btnApproveReject_Click" />
</div>
<%--*************************************************************************************************--%>
