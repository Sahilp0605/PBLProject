<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myWorkOrderApproval.ascx.cs" Inherits="StarsProject.myWorkOrderApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%--<link href="css/Registration.css" rel="stylesheet" type="text/css" />
<link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />--%>
<style type="text/css">
    .Popup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;          
            padding-left: 10px;
            width: 96%;
            height: 96%;
        }

        .btnTopRightCorner {
            display: block;
            box-sizing: border-box;
            width: 30px;
            height: 30px;
            border-width: 3px;
            border-style: solid;
            border-color: red;
            border-radius: 100%;
            background: -webkit-linear-gradient(-45deg, transparent 0%, transparent 46%, white 46%, white 56%,transparent 56%, transparent 100%), -webkit-linear-gradient(45deg, transparent 0%, transparent 46%, white 46%, white 56%,transparent 56%, transparent 100%);
            background-color: red;
            box-shadow: 0px 0px 5px 2px rgba(0,0,0,0.5);
            transition: all 0.3s ease;
            position: relative;
            top: 12px;
            right: 15px;
        }
</style>
<script type="text/javascript">
    function viewWorkOrder(id) {
        var pageUrl = "WorkOrderComm.aspx?mode=view&id=" + id;
        $('#ifrModuleSales').attr('src', pageUrl);
        $find("mpe").show();
    }
</script>   
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnClientUrl" runat="server" ClientIDMode="Static" EnableViewState="true" />


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
                    <th class="center-align">Order #</th>
                    <th class="center-align">Order Date</th>
                    <th class="left-align">Vendor Name</th>
                    <th class="center-align">Approved By</th>
                    <th class="center-align">Approval Status</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                <asp:HiddenField ID="hdnOrderNo" runat="server" ClientIDMode="Static" Value='<%#Eval("OrderNo") %>' />
                <asp:HiddenField ID="hdnApprovalStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("ApprovalStatus") %>' />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                <asp:HiddenField ID="hdnCreatedBy" runat="server" ClientIDMode="Static" Value='<%#Eval("CreatedBy") %>' />
                <asp:HiddenField ID="hdnCustomerName" runat ="server" ClientIDMode="Static" Value='<%# Eval("CustomerName") %>' />
                <td class="center-align">
                    <a href="javascript:viewWorkOrder(<%# Eval("pkID")%>);"><%# Eval("OrderNo") %></a>
                </td>
                <td class="center-align"><%# Eval("OrderDate", "{0:dd/MM/yyyy}") %></td>
                <td class="left-align"><%# Eval("CustomerName") %></td>
                <td class="left-align"><%# Eval("EmployeeName") %></td>
                <%--<td class="center-align"><%# Eval("IssuedBy") %></td>--%>
                <td class="center-align">
                    <% if (!hdnClientUrl.Value.Contains("dashboard"))
                       { %>
                        <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="1" Style="height: 1.70rem; margin: 0px; padding: 0px 0px 0px 10px;">
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="On Hold" Value="On Hold" />
                            <asp:ListItem Text="Approved" Value="Approved" />
                            <asp:ListItem Text="Ready To Dispatch" Value="ReadyToDispatch" />
                            <asp:ListItem Text="Dispatched" Value="Dispatched" />
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

<%-- PopUp Modal Box to Open Window --%>
<asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
<cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
    <div id="myModal123">
        <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
            <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                <i class="material-icons prefix">ac_unit</i>
                <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
            </h5>
        </div>
    </div>
    <iframe id="ifrModuleSales" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
    <br />
</asp:Panel>
