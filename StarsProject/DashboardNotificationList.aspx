<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardNotificationList.aspx.cs" Inherits="StarsProject.DashboardNotificationList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/PaggingUserControl.ascx" TagName="page" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notificaiton List</title>
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />

    <script src="js/plugins/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
            <div id="divNotification" class="widgetbox" style="margin: 0 5px 0 5px;">
                <div class="widgetcontent padding0 statement">   
                    <br clear="all" />
                    <div class="pageheader">
                        <h1 class="pagetitle">Notification List</h1>
                    </div>
                    <br clear="all" />        
                    <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                    <asp:Repeater ID="rptNotification" runat="server" ClientIDMode="Static" EnableViewState="true" OnItemCommand="rptNotification_ItemCommand" >
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th style="background: none; background-color: Navy !important; color: White;">Registration #</th>
                                    <th style="background: none; background-color: Navy !important; color: White;">Police / Relative</th>
                                    <th style="background: none; background-color: Navy !important; color: White;">Email Address</th>
                                    <th style="background: none; background-color: Navy !important; color: White;">Mobile #</th>
                                    <th style="background: none; background-color: Navy !important; color: White;">Landline #</th>
                                    <th style="background: none; background-color: Navy !important; color: White;">Designation/Relation</th>
                                    <th style="background: none; background-color: Navy !important; color: White;">Department</th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="background:#b5cfd2 url('/images/cell-blue.jpg'); font-weight: bold;">
                                <asp:HiddenField ID="hdn_HelpLogID" runat="server" Value='<%#Eval("HelpLogID") %>' />
                                <asp:HiddenField ID="hdn_KeyCode" runat="server" Value='<%#Eval("KeyCode") %>' />
                                <asp:HiddenField ID="hdn_OrgCode" runat="server" Value='<%#Eval("OrgCode") %>' />
                                <asp:HiddenField ID="hdn_OrgName" runat="server" Value='<%#Eval("OrgName") %>' />
                                <asp:HiddenField ID="hdn_OrgDepartmentCode" runat="server" Value='<%#Eval("OrgDepartmentCode") %>' />
                                <asp:HiddenField ID="hdn_OrgDepartmentName" runat="server" Value='<%#Eval("OrgDepartmentName") %>' />
                                <td style="color: Black;"><asp:Label ID="lbl_RegistrationNo" runat="server" Text='<%# Eval("RegistrationNo") %>' /></td>
                                <td style="color: Black;"><asp:Label ID="lbl_MemberName" runat="server" Text='<%# Eval("MemberName") %>' /></td>
                                <td style="color: Black;"><asp:Label ID="lbl_EmailAddress" runat="server" Text='<%# Eval("EmailAddress") %>' /></td>
                                <td style="color: Black;"><asp:Label ID="lbl_MobileNo" runat="server" Text='<%# Eval("MobileNo") %>' /></td>
                                <td style="color: Black;"><asp:Label ID="lbl_Landline" runat="server" Text='<%# Eval("Landline") %>' /></td>
                                <td style="color: Black;"><asp:Label ID="lbl_RelationType" runat="server" Text='<%# Eval("RelationType") %>' /></td>
                                <td style="color: Black;"><asp:Label ID="lbl_DispatchCategory" runat="server" Text='<%# Eval("DispatchCategory") %>' /></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr style="font-weight: bold; text-align: center; color: Black; height:40px;">
                                <td colspan="7"><asp:Button ID="btnSendNotification" runat="server" CommandName="SendNotification" ClientIDMode="Static" Text="Send Notification"/></td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    </table>
                </div>
            </div>
            <asp:HiddenField ID="hdnHelpPkID" runat="server" />
            <asp:HiddenField ID="hdnHelpRegistrationNo" runat="server" />
            <asp:HiddenField ID="hdnHelpMemberName" runat="server" />
            <asp:HiddenField ID="hdnHelpLatitude" runat="server" />
            <asp:HiddenField ID="hdnHelpLongitude" runat="server"/> 
            <asp:HiddenField ID="hdnHelpOrgCode" runat="server"/>  
            <div class="clearall"></div>
            <div ID="divMessage" class="usermsg" runat="server"></div>
            <div class="clearall"></div>
    </form>
</body>
</html>
