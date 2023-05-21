<%@ Control Language="C#" CodeBehind="myDealerOrderApproval.ascx.cs" Inherits="StarsProject.myDealerOrderApproval" %>

<script type="text/javascript">
    function viewSalesOrder(id) {
        var pageUrl = "SalesOrderDealer.aspx?mode=view&id=" + id;
        $('#ifrModuleSales').attr('src', pageUrl);
        $find("mpe").show();
    }
</script>

<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />

<div class="modal-body float-right" id="divApprovalStatus" runat="server" clientidmode="Static">
    <div class="form-row">
        <asp:DropDownList ID="drpApprovalStatus1" runat="server" ClientIDMode="Static" Class="form-control" TabIndex="1" Style="height: inherit; width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="drpApprovalStatus1_SelectedIndexChanged">
            <asp:ListItem Text="Pending" Value="Pending" />
            <asp:ListItem Text="On Hold" Value="On Hold" />
            <asp:ListItem Text="Approved" Value="Approved" />
            <asp:ListItem Text="Ready To Dispatch" Value="ReadyToDispatch" />
            <asp:ListItem Text="Dispatched" Value="Dispatched" />
            <asp:ListItem Text="Rejected" Value="Rejected" />
        </asp:DropDownList>
    </div>
</div>

<table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptApproval" runat="server" OnItemDataBound="rptApproval_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="text-center">Order #</th>
                    <th class="text-center">Order Date</th>
                    <th style="text-align: left !important;">Customer Name</th>
                    <th class="text-left">Approval Status</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                <asp:HiddenField ID="hdnApprovalStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("ApprovalStatus") %>' />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                <asp:HiddenField ID="hdnCreatedBy" runat="server" ClientIDMode="Static" Value='<%#Eval("CreatedBy") %>' />
                <td class="text-center">
                    <a href="javascript:viewSalesOrder(<%# Eval("pkID")%>);"><%# Eval("OrderNo") %></a>
                </td>
                <td class="text-center"><%# Eval("OrderDate", "{0:dd/MM/yyyy}") %></td>
                <td style="text-align: left !important;"><%# Eval("CustomerName") %></td>
                <td class="text-left">
                    <%# Eval("ApprovalStatus") %>
                    <%--                    <% if (hdnView.Value != "dashboard" && (hdnRole.Value == "admin" || hdnRole.Value == "bradmin")) { %>--%>
                    <%--<asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="1" Style="height: 1.70rem; margin: 0px; padding: 0px 0px 0px 10px;">
                        <asp:ListItem Text="Pending" Value="Pending" />
                        <asp:ListItem Text="On Hold" Value="On Hold" />
                        <asp:ListItem Text="Approved" Value="Approved" />
                        <asp:ListItem Text="Ready To Dispatch" Value="ReadyToDispatch" />
                        <asp:ListItem Text="Dispatched" Value="Dispatched" />
                        <asp:ListItem Text="Rejected" Value="Rejected" />
                    </asp:DropDownList>--%>
                    <%--                    <% } else { %>
                        <%# Eval("ApprovalStatus") %>
                    <% } %>--%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<%--<div class="modal-footer">
    <asp:Button ID="btnApproveReject" runat="server" ClientIDMode="Static" class="btn btn-primary" Text="Submit Order Status" OnClick="btnApproveReject_Click" />
</div>--%>
