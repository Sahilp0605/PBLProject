<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myMissedPunch.ascx.cs" Inherits="StarsProject.myMissedPunch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">
    function viewMissedPunch(id) {
        var pageUrl = "MissedPunch.aspx?mode=view&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
</script>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnClientUrl" runat="server" ClientIDMode="Static" EnableViewState="true" />

<table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptApproval" runat="server" OnItemDataBound="rptApproval_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="center-align">Sr.#</th>
                    <th class="center-align">Miss.Punch Date</th>
                    <th class="left-align">Employee Name</th>
                    <th class="center-align">Time IN</th>
                    <th class="center-align">Time OUT</th>
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
                    <a href="javascript:viewMissedPunch(<%# Eval("pkID")%>);"><%# Eval("pkID") %></a>
                </td>
                <td class="center-align"><%# Eval("PresenceDate", "{0:dd/MM/yyyy}") %></td>
                <td class="left-align"> <%# Eval("EmployeeName") %></td>
                <td class="center-align"> <%# Eval("TimeIn") %></td>
                <td class="center-align"> <%# Eval("TimeOut") %></td>
                <td class="center-align">
                    <% if (!hdnClientUrl.Value.Contains("dashboarddaily"))
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
    <asp:Button ID="btnApproveReject" runat="server" ClientIDMode="Static" class="btn btn-primary float-right" Text="Submit Status" OnClick="btnApproveReject_Click" />
</div>
<%--*************************************************************************************************--%>
<asp:Button ID="Button1Lead" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
<cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="panelLead" TargetControlID="Button1Lead" CancelControlID="Button2Lead" BackgroundCssClass="Background">  
</cc1:ModalPopupExtender>  
<asp:Panel ID="panelLead" runat="server" ClientIDMode="Static" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
    <div id="myModal">
        <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom:10px !important;">
            <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                <i class="material-icons prefix">ac_unit</i>
                <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2Lead" runat="server" Text=""/>
            </h5>
        </div>
    </div>
    <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border:1px solid silver; background-color:transparent; width:100%; height:90%; padding:5px;"></iframe>  
    <br/>  
</asp:Panel>
