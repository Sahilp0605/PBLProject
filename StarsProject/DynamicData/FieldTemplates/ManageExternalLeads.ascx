<%@ Control Language="C#" CodeBehind="ManageExternalLeads.ascx.cs" Inherits="StarsProject.DynamicData.FieldTemplates.ManageExternalLeads1" %>

<link href="css/Registration.css" rel="stylesheet" type="text/css" />
<link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function viewExternalLeads(id) {
        var pageUrl = "ManageExternalLeads.aspx?mode=view&id=" + id;
        $.colorbox({
            width: "90%", height: "90%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }
</script>
    <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptApproval" runat="server" OnItemDataBound="rptApproval_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="text-center">Order #</th>
                    <th class="text-center">Order Date</th>
                    <th style="text-align:left !important;">Customer Name</th>
                    <th style="text-align:left !important;">Sales Person</th>
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
                <td style="text-align:left !important;"><%# Eval("CustomerName") %></td>
                <td style="text-align:left !important;"><%# Eval("EmployeeName") %></td>
                <td class="text-left">
<%--                    <% if (hdnView.Value != "dashboard" && (hdnRole.Value == "admin" || hdnRole.Value == "bradmin")) { %>--%>
                    <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static"  class="form-control" TabIndex="1" style="height:inherit;">
                        <asp:ListItem Text="Pending" Value="Pending" />
                        <asp:ListItem Text="On Hold" Value="On Hold" />
                        <asp:ListItem Text="Approved" Value="Approved" />
                        <asp:ListItem Text="Rejected" Value="Rejected" />
                    </asp:DropDownList>
<%--                    <% } else { %>
                        <%# Eval("ApprovalStatus") %>
                    <% } %>--%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>
    <div class="modal-footer">
        <asp:Button ID="btnApproveReject" runat="server" ClientIDMode="Static" class="btn btn-primary" Text="Submit Order Status" OnClick="btnApproveReject_Click" />
    </div>
