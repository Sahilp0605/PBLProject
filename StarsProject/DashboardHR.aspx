<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="DashboardHR.aspx.cs" Inherits="StarsProject.DashboardHR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myLeaveRequest.ascx" TagPrefix="uc1" TagName="myLeaveRequest" %>
<%@ Register Src="~/myOvertimeApproval.ascx" TagPrefix="uc1" TagName="myOvertimeApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .card-title {
            color: #1e90ff;
        }

        .mydropdowncontent {
            width: 230px !important;
            height: auto !important;
        }

        .mydropdowncontent li {
            min-height: auto;
            padding: 0px;
        }

        .mydropdowncontent li > a {
            min-height: auto;
            padding: 2px;
            font-size: 12px;
            color: navy;
            padding: 3px 5px;
            background-color: #f3ebeb;
        }

        .dataTables_filter, .dataTables_info, .dataTables_length {
            display: none;
        }

        table.dataTable thead th, table.dataTable thead td, {
            padding: 5px 5px;
            font-size: 12px;
        }

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 5px 5px;
            font-size: 11px;
        }

        .myDropDownClass {
            background-color: #fff6e1;
            color: navy;
            font-size: medium;
            width: 150px;
            height: 30px !important;
            border: 1px solid black;
            margin-top: 1px;
            padding: 1px 5px;
            border: none;
            border-radius: 5px;
        }

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

        .titleHead {
            font-weight: 800;
            margin-left: 10px;
        }

        .cardRound {
            box-shadow: black -6px 8px 6px -6px !important;
            border-radius: 8px 8px 0px 0px !important;
        }

        .h5Round {
            border-radius: 8px 8px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, #303f9f, #7b1fa2) !important;
            color: white !important;
        }

        .h5RoundHead {
            border-radius: 0px 0px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, silver, #7f88ec6e);
        }
    </style>

    <script type="text/javascript">
        var pageUrl = '';

        function openMixTimeline(xExtID, xCustID, xCustName) {
            if (xCustID != '0')
                openTimeline('followup', xCustID, xCustName);
            else
                openTimelineExt('followupext', xExtID, xCustName);
        }

        function openTimeline(xView, xCustID, xCustName) {
            var pageUrl = "FollowupTimeline.aspx?view=" + xView + "&id=" + xCustID + "&name=" + xCustName;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openTimelineExt(xView, xCustID, xCustName) {
            var pageUrl = "FollowupTimeline.aspx?view=" + xView + "&id=" + xCustID + "&name=" + xCustName;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openToDO(mode, pkid) {
            var pageUrl = "ToDO.aspx?mode=" + mode + "&id=" + pkid
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openMixFollowup(mode, pkid, custid, extpkid)
        {
            if (extpkid=='0' || extpkid=='')
                openContFollowup(mode, pkid);
            else
                openContFollowFromTeleCaller('add', extpkid);
        }
        function openContFollowup(mode, pkid) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage FollowUp";
            var pageUrl = "Followup.aspx?mode=" + mode + "&id=" + pkid
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openContFollowFromTeleCaller(mode, ExtpkID) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage External FollowUp";
            var pageUrl = "FollowupExternal.aspx?mode=" + mode + "&id=0" + "&ExtpkID=" + ExtpkID;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // User Activity
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function viewUserInquiryList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Leads";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            var clienturl = $("#hdnClientURL").val();
            pageUrl = "myCustomView.aspx?view=inquirybyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserQuotationList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Quotation";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=quotbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserFollowUpList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of FollowUp";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=followupbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserSaleOrderList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Sales Order";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=salesorderbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserContactList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Contacts";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=contactbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        //Created by Vikram Rajput
        function viewUserSaleBillList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Sales Bill";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=salesbillbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        //Created by Vikram Rajput
        function viewUserPurchaseBillList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Purchase Bill";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=purchasebillbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        //Created by Vikram Rajput
        function viewUserInwardList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Inward";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=inwardbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        //Created by Vikram Rajput
        function viewUserOutwardList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Outward";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=outwardbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserToDOList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Task";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=todobyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserLeaveRequestList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Leave Request";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=leaverequestbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserLoginLogoutList(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Login/Logout ";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=userloginlogout&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserLatePunch(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List of Late Punch";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=latepunchbyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserDailyActivity(userid, empname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Daily Activity";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=dailyactivitybyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" CompositeScript-ScriptMode="Inherit"></asp:ScriptManager>
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTODOVersion" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientURL" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnWelcomeGreet" runat="server" ClientIDMode="Static" />
    <%--************************************************************************************ --%>
    <%-- Leave Request --%>
    <%--************************************************************************************ --%>
    <div id="divDashHR" style="background: linear-gradient(45deg, silver, #f9e3b1); margin-bottom: 10px; padding-left: 10px; padding-right: 10px;">
        <asp:Button ID="Button3" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <div class="content-wrapper-before gradient-45deg-indigo-purple"></div>
        <div class="row">
            <div class="col m12">
                <div class="card user-statistics-card mb-0 mt-1">
                    <div class="card-content p-0">
                        <h5 class="h5RoundHead">
                            <span id="Span4" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Filter Dashboard : </span>
                            <asp:DropDownList ID="drpDailyMonth" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpDailyMonthYear_SelectedIndexChanged"></asp:DropDownList>
                            <asp:DropDownList ID="drpDailyYear" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpDailyMonthYear_SelectedIndexChanged"></asp:DropDownList>
                        </h5>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                <asp:UpdatePanel ID="updDailyLeave" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0" data-position="center" data-tooltip="New Leave Request" href="javascript:openDashboardBox('leave', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="Span2" runat="server" clientidmode="Static" class="titleHead text-top">Leave Request</span>
                                    <asp:DropDownList ID="drpLeaveStatus" runat="server" ClientIDMode="Static" CssClass="myDropDownClass select2-theme browser-default mr-2 float-right" OnSelectedIndexChanged="drpLeaveStatus_SelectedIndexChanged" TabIndex="5" AutoPostBack="true">
                                        <asp:ListItem Text="Pending" Value="Pending" />
                                        <asp:ListItem Text="Approved" Value="Approved" />
                                        <asp:ListItem Text="Rejected" Value="Rejected" />
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 250px; max-height: 250px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myLeaveRequest runat="server" ID="myLeaveRequest" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpLeaveStatus" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0" data-position="center" data-tooltip="New OverTime Request" href="javascript:openDashboardBox('overtime', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="Span1" runat="server" clientidmode="Static" class="titleHead text-top">OverTime Request</span>
                                    <asp:DropDownList ID="drpOTStatus" runat="server" ClientIDMode="Static" CssClass="myDropDownClass select2-theme browser-default mr-2 float-right" OnSelectedIndexChanged="drpOTStatus_SelectedIndexChanged" TabIndex="5" AutoPostBack="true">
                                        <asp:ListItem Text="Pending" Value="Pending" />
                                        <asp:ListItem Text="Approved" Value="Approved" />
                                        <asp:ListItem Text="Rejected" Value="Rejected" />
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 250px; max-height: 250px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myOvertimeApproval runat="server" ID="myOvertimeApproval" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpOTStatus" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>
    <br />
    <br />
    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpeHR" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="Panl1" runat="server" ClientIDMode="Static" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
        <div id="myModal">
            <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                    <i class="material-icons prefix">ac_unit</i>
                    <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module.</span>
                    <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" ClientIDMode="Static" Text="" />
                </h5>
            </div>
        </div>
        <iframe id="ifrModuleHR" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 85%; padding: 5px;"></iframe>
        <br />
    </asp:Panel>

    <script type="text/javascript">
        window.onload = function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "Daily Activity - Analytics & Summary";
        }

        $(document).ready(function () {
            setUserActivityInterface();

            $('.btnTopRightCorner').on("click", function () {
                var pgSource = $('#ifrModuleHR').attr('src');
                var strPos1 = pgSource.indexOf('mode=add');
                var strPos2 = pgSource.indexOf('mode=edit');
                var strPos3 = pgSource.indexOf('mode=continue');
                if (strPos1 >= 0 || strPos2 >= 0 || strPos3 >= 0) {
                    __doPostBack('', '');
                }
            });
        });

        function setUserActivityInterface() {
            $('.carousel.carousel-slider').carousel({ fullWidth: true, indicators: true });
        }

        function capitalizeString(str) {
            return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }

        function openDashboardBox(view, mode, id) {
            if (view == 'leave') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leave Request";
                var pageUrl = "LeaveRequest.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleHR').attr('src', pageUrl);
                $find("mpeHR").show();
            }
            else if (view == 'overtime') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage OverTime Request";
                var pageUrl = "OverTime.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleHR').attr('src', pageUrl);
                $find("mpeHR").show();
            }
        }
    </script>
</asp:Content>
