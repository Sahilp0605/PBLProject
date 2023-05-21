<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myToDOCustomer.ascx.cs" Inherits="StarsProject.myToDOCustomer" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnTaskStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />
<style type="text/css">
    .todoEditIcon {
        font-size: 16px;
        padding: 3px;
        color: navy;
        border: 2px solid navy;
        border-radius: 4px;
        background-color: #90ee90;
        position: relative;
        left: -25px;
        top: -15px;
    }
</style>
<table id="tblInqProductGroup" class="table table-striped table-bordered" style="font-size:12px;" cellpadding="0" cellspacing="0" border="0" width="100%">
<asp:Repeater ID="rptCustomerTODO" runat="server" OnItemDataBound="rptCustomerTODO_ItemDataBound">
    <HeaderTemplate>
        <thead class="thead-dark" style="font-size:12px;">
        <tr>
            <th class="center-align">Initiated By</th>
            <th class="center-align">Assigned To</th>
            <th class="center-align">Priority</th>
            <th class="center-align">Category</th>
            <th class="left-align" style="width:450px;">Task Description</th>
            <th class="center-align">Task Date</th>
            <th class="center-align">Completion Date</th>
            <th class="center-align">Status</th>
        </tr>
        </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:HiddenField ID="hdnPriority" runat="server" ClientIDMode="Static" Value='<%# Eval("Priority") %>' />
        <asp:HiddenField ID="hdnDueDate" runat="server" ClientIDMode="Static" Value='<%# Eval("DueDate") %>' />
        <asp:HiddenField ID="hdnCompletionDate" runat="server" ClientIDMode="Static" Value='<%# Eval("CompletionDate") %>' />
        
        <tr>
            <td class="center-align vertical-align-top">
                <span class="text" style="padding-left:3px; font-size:14px;">
                    <a href="javascript:openDashboardBox('todo','view',<%# Eval("pkID")%>);"><%# Eval("FromEmployeeName") %></a>
                </span>
            </td>
            <td class="center-align vertical-align-top">
                <span class="text" style="padding-left:3px; font-size:14px;">
                    <a href="javascript:openDashboardBox('todo','view',<%# Eval("pkID")%>);"><%# Eval("EmployeeName") %></a>
                </span>
            </td>
            <td class="center-align vertical-align-top">
                <small class="badge badge-secondary mr-1 black-text"><%# Eval("Priority") %></small>
            </td>
            <td class="center-align vertical-align-top">
                <small class="badge badge-secondary mr-1 black-text"><%# Eval("TaskCategory") %></small>
            </td>
            <td class="left-align vertical-align-top">
                <b style="color:orangered; font-size:14px;"><%# Eval("CustomerName") %></b><br />
                <%# Eval("TaskDescription") %>
            </td>
            <td class="center-align vertical-align-top">
                <small class="badge badge-primary bold black-text">
                    <%# Eval("StartDate", "{0:dd-MM-yyyy HH:mm}") %><br /><%# Eval("DueDate", "{0:dd-MM-yyyy HH:mm}") %> 
                </small>
            </td>
            <td class="center-align vertical-align-top">
                <small class="badge badge-success">
                    <span class="bold red-text" ID="ltrCompletion" runat="server" clientidmode="Static"><%# Eval("CompletionDate", "{0:dd-MM-yyyy HH:mm}") %></span>
                </small>
            </td>
            <td class="center-align vertical-align-top">
                <span class="badge gradient-45deg-yellow-green black-text" style="line-height: 20px; border-radius: 8px;">
                    <%# Eval("TaskStatus") %><br />
                    <%# Eval("Duration") %> Days 
                </span>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
