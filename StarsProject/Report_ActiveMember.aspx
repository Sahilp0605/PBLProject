<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="Report_ActiveMember.aspx.cs" Inherits="StarsProject.Reports.Report_ActiveMember" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="/css/dhtmlxcalendar.css" rel="stylesheet" type="text/css" />
    <link href="/css/dhtmlxcalendar_dhx_skyblue.css" rel="stylesheet" type="text/css" />
    <script src="/js/dhtmlxcalendar.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <div class="pageheader">
        <h1 class="pagetitle contenttitle2">Report: <asp:Label ID="lblHead" runat="server" /></h1>
    </div>
    <div id="contentwrapper" class="contentwrapper">
            <div id="basicform" class="subcontent">
                <div class="clearall" />
                <div class="leftdiv50">
                    <p class="bordeelftnn">
                        <label>Start Date</label>
                        <span class="field rghtbrdr" id="marwdthlow">
                            <asp:TextBox ID="strdate" runat="server" class="shortinptfld" MaxLength="100" autocomplete="off"
                                TabIndex="0" ReadOnly="true"></asp:TextBox>
                            <img id="startDate" border="0" alt="" tabindex="0" src="~/images/default/calendar.gif" />
                        </span>
                    </p>
                </div>
                <div class="rightdiv50">
                    <p style="border-left: none;">
                        <label>End Date</label>
                        <span class="field rghtbrdr" id="marwdthlow">
                            <asp:TextBox ID="eddate" runat="server" class="shortinptfld" MaxLength="100" autocomplete="off"
                                TabIndex="1" ReadOnly="true"></asp:TextBox>
                            <img id="enddate" border="0" alt="" tabindex="1" src="~/images/default/calendar.gif" />
                        </span>
                    </p>
                </div>
                <div class="clearall" />
                <div>
                    <p class="stdformbutton bordrtopnone" align="center" style="border: none;">
                       <asp:Button ID="btnSearch" runat="server" Text="Search" class="submit radius2" 
                        ToolTip="Save" onclick="btnSearch_Click"  OnClientClick="checkval();" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" class="reset radius2"  onclick="ResetSerch"
                        ToolTip="Reset"/>
                    </p>
                </div>
            </div>
            <div id="rptviewer" style="padding: 20px 0px 0px 20px" runat="server">
                <rsweb:ReportViewer ID="myViewer" runat="server" Font-Names="Verdana" Font-Size="8pt"
                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%"
                    ShowRefreshButton="true" SizeToReportContent="true" ShowPrintButton="true" ShowPageNavigationControls="true" ShowReportBody="true" ShowZoomControl="true" ZoomMode="Percent">
                    <LocalReport>
                        <DataSources>
                            <rsweb:ReportDataSource />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
            </div>
        <asp:HiddenField ID="hdnReportName" runat="server" Value="" />
        <asp:HiddenField ID="hdnStartDate" runat="server" Value="" />        
        <asp:HiddenField ID="hdnEndDate" runat="server" Value="" />
        
    </div>
    <script type="text/javascript">
        var myCalendar;
        var myCalendar1;
        var startdat = document.getElementById('<%= strdate.ClientID %>').id;
        var enddat = document.getElementById('<%= eddate.ClientID %>').id;

        function doOnLoad() {
            myCalendar = new dhtmlXCalendarObject({ input: startdat, button: "startDate" });
            myCalendar.setDateFormat("%d/%m/%Y");
            myCalendar.showTime();

            myCalendar1 = new dhtmlXCalendarObject({ input: enddat, button: "enddate" });
            myCalendar1.setDateFormat("%d/%m/%Y");
            myCalendar1.showTime();
        }

        jQuery(document).ready(function () {
            doOnLoad();
        });

        function checkval() {
            var hdnstart = document.getElementById('<%= hdnStartDate.ClientID %>').id;
            var hdnend = document.getElementById('<%= hdnEndDate.ClientID %>').id;
            //alert(jQuery('#' + enddat).val());
            jQuery('#' + hdnstart).val(jQuery('#' + startdat).val());
            jQuery('#' + hdnend).val(jQuery('#' + enddat).val());
            //  alert($('#' + hdnend).val());
        }

        function ResetSerch() {
            jQuery('#' + startdat).val('');
            jQuery('#' + enddat).val('');
        }
    </script>
</asp:Content>
