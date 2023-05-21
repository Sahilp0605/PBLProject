<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardTicketList.aspx.cs" Inherits="StarsProject.DashboardTicketList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Src="~/PaggingUserControl.ascx" TagName="page" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="css/style.default.css" type="text/css" />
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .widgetbox1  
        {
            border:2px solid navy; 
            width: 95%;
            margin: 40px 0 0 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="stdform2 stdform">
    
        <center><h4 class="subh4title" style="Color:Navy;">Help Log Ticket</h4></center>
        <div id="dvViewTicket" class="widgetbox1">
            <table class="stdtable" cellpadding="0" cellspacing="0" border="0">
                <asp:Repeater ID="rptHelpLog" runat="server" OnItemDataBound="rptHelpLog_ItemDataBound" ClientIDMode="Static" Visible="false">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th style="text-align:center;">Ticket #</th>
                                <th style="text-align:center;">Member Name</th>
                                <th style="text-align:center;">SOS Date</th>
                                <th style="text-align:center;">SOS Time</th>
                                <th style="text-align:center;">Category</th>
                                <th style="text-align:center;">Status</th>
                                <% if (!String.IsNullOrEmpty(Request.QueryString["LogStatus"]) && Request.QueryString["LogStatus"].ToString() == "C") { %>
                                <th style="text-align:center;">Type Of Crime</th>
                                <th style="text-align:center;">FIR #</th>
                                <th style="text-align:center;">Attended On</th>
                                <th style="text-align:center;">Attended By</th>
                                <% } %>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="blueShed">
                            <asp:HiddenField ID="hdnPkID" runat="server" Value='<%#Eval("pkID") %>' />
                            <asp:HiddenField ID="hdnRegNo" runat="server" Value='<%#Eval("RegistrationNo") %>' />
                            <td style="text-align:center;"><%# Eval("pkID") %></td>
                            <td class="tdLink"  style="text-align:center;"><a id="ahideShow" href="javascript:openColorBoxForm('<%# Eval("RegistrationNo") %>');"><%# Eval("MemberName") %></a></td>
                            <td style="text-align:center;"><%# Eval("LogDateTime", "{0:dd/MM/yyyy}")%></td>
                            <td style="text-align:center;"><%# Eval("LogDateTime", "{0:hh:mm:ss tt}")%></td>
                            <td style="text-align:center;"><%# Eval("TypeName") %></td>
                            <td id="tdLogStatus" runat="server" style="text-align:center;"><asp:Label ID="lblLogStatus" runat="server" Text='<%# Eval("LogStatus") %>' style="text-align:center !important;"/></td>
                            <% if (!String.IsNullOrEmpty(Request.QueryString["LogStatus"]) && Request.QueryString["LogStatus"].ToString() == "C") { %>
                            <td style="text-align:center;"><asp:Label ID="Label1" runat="server" Text='<%# Eval("CrimeTypeDesc")%>'  style="color: Red; font-weight: bold;"/></td>
                            <td style="text-align:center;"><%# Eval("FIRNo") %></td>
                            <td style="text-align:center;"><%# Eval("AttendOn", "{0:dd/MM/yyyy hh:mm:ss tt}")%></td>
                            <td style="text-align:center;"><%# Eval("AttendOrgName")%></td>
                            <% } %>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Repeater ID="rptHelpLogCrime" runat="server" ClientIDMode="Static" Visible="false">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th style="text-align:center;">Ticket #</th>
                                <th style="text-align:center;">Member Name</th>
                                <th style="text-align:center;">SOS Date</th>
                                <th style="text-align:center;">SOS Time</th>
                                <th style="text-align:center;">Category</th>
                                <th style="text-align:center;">Status</th>
                                <th style="text-align:center;">Type Of Crime</th>
                                <th style="text-align:center;">FIR #</th>
                                <th style="text-align:center;">Attended On</th>
                                <th style="text-align:center;">Attended By</th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="blueShed">
                            <asp:HiddenField ID="hdnPkID" runat="server" Value='<%#Eval("pkID") %>' />
                            <asp:HiddenField ID="hdnRegNo" runat="server" Value='<%#Eval("RegistrationNo") %>' />
                            <td style="text-align:center;"><%# Eval("pkID") %></td>
                            <td class="tdLink" style="text-align:center;"><a id="ahideShow" href="javascript:openColorBoxForm('<%# Eval("RegistrationNo") %>');"><%# Eval("MemberName") %></a></td>
                            <td style="text-align:center;"><%# Eval("LogDateTime", "{0:dd/MM/yyyy}")%></td>
                            <td style="text-align:center;"><%# Eval("LogDateTime", "{0:hh:mm:ss tt}")%></td>
                            <td style="text-align:center;"><%# Eval("TypeName") %></td>
                            <td id="tdLogStatus" runat="server"  style="text-align:center;"><asp:Label ID="lblLogStatus" runat="server" Text='<%# Eval("LogStatus") %>'  style="text-align:center;"/></td>
                            <td style="text-align:center;"><asp:Label ID="Label1" runat="server" Text='<%# Eval("CrimeTypeDesc")%>'  style="color: Red; font-weight: bold; text-align:center;"/></td>
                            <td style="text-align:center;"><%# Eval("FIRNo") %></td>
                            <td style="text-align:center;"><%# Eval("AttendOn", "{0:dd/MM/yyyy hh:mm:ss tt}")%></td>
                            <td style="text-align:center;"><%# Eval("AttendOrgName")%></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="10">
                        <uc1:page ID="pageGrid" runat="server" Visible="true" OnPagerChanged="Pager_Changed"/>
                    </td>
                </tr>
            </table>

        </div>
        <asp:HiddenField ID="hdnMode" runat="server" />
        <div class="clearall"></div>
        <div class="clearall"></div>

    </form>
</body>
</html>
