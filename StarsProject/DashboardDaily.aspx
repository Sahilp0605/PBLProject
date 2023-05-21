<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="DashboardDaily.aspx.cs" Inherits="StarsProject.DashboardDaily" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myToDo.ascx" TagPrefix="uc1" TagName="myToDo" %>
<%@ Register Src="~/myFollowup.ascx" TagPrefix="uc1" TagName="myFollowup" %>
<%@ Register Src="~/myUserActivity.ascx" TagPrefix="uc1" TagName="myUserActivity" %>
<%@ Register Src="~/myAllLeads.ascx" TagPrefix="uc1" TagName="myAllLeads" %>
<%@ Register Src="~/myCRMSummary.ascx" TagPrefix="uc1" TagName="myCRMSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="app-assets/js/scripts/advance-ui-carousel.js"></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/plugins/jquery.colorbox-min.js"></script>

    <link rel="stylesheet" type="text/css" href="app-assets/vendors/ionRangeSlider/css/ion.rangeSlider.css">
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/ionRangeSlider/css/ion.rangeSlider.skinFlat.css">

<%--    <script type="text/javascript" src="js/jquery-3.3.1.js"></script>
    <script type="text/javascript" src="plugins/jquery/jquery.min.js"></script>--%>
    <script type="text/javascript" src="app-assets/vendors/ionRangeSlider/js/ion.rangeSlider.js"></script>

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
        $(document).ready(function () {
            //var $range = $(".js-range-slider");
            //var min = 100;
            //var max = 1000;
            //var marks = [700, 900];

            //$range.ionRangeSlider({
            //    skin: "big",
            //    type: "single",
            //    min: min,
            //    max: max,
            //    from: 300,
            //    onStart: function (data) {
            //        addMarks(data.slider);
            //    }
            //});

            //function convertToPercent(num) {
            //    var percent = (num - min) / (max - min) * 100;

            //    return percent;
            //}

            //function addMarks($slider) {
            //    var html = '';
            //    var left = 0;
            //    var i;

            //    for (i = 0; i < marks.length; i++) {
            //        left = convertToPercent(marks[i]);
            //        html += '<span class="mark" style="left: ' + left + '%">' + marks[i] + '</span>';
            //    }

            //    $slider.append(html);
            //}
            //$("#range_03").ionRangeSlider({ skin: "modern", min: 0, max: 1000, from: 200, to: 800 });

            //mySlider();
        });
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
        function viewUserContactList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Contacts";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=contactbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserFollowUpList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of FollowUp";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=followupbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserInquiryList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Leads";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            var clienturl = $("#hdnClientURL").val();
            pageUrl = "myCustomView.aspx?view=inquirybyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserQuotationList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Quotation";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=quotbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserSaleOrderList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Sales Order";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=salesorderbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function viewUserToDOList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Task";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=todobyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserDailyActivity(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Daily Activity";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=dailyactivitybyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserLoginLogoutList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Login/Logout ";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=userloginlogout&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserLatePunch(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List of Late Punch";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=latepunchbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewUserLeaveRequestList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Leave Request";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=leaverequestbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function viewUserSaleBillList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Sales Bill";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=salesbillbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        //Created by Vikram Rajput
        function viewUserPurchaseBillList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Purchase Bill";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=purchasebillbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        //Created by Vikram Rajput
        function viewUserInwardList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Inward";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=inwardbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
            $('#ifrModuleDaily').attr('src', pageUrl);
            $find("mpe").show();
        }
        //Created by Vikram Rajput
        function viewUserOutwardList(userid, empname, d1, d2) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "List Of Outward";
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            pageUrl = "myCustomView.aspx?view=outwardbyuser&para1=" + userid + "&para2=" + d1 + "&para3=" + d2 + "&para4=" + empname;
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
    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
<%--    <audio controls>
        <source src="AudioFiles/YashTarwadi-20210809104229.aac" type="audio/AAC">
    </audio>--%>
    <div id="divLeads" style="background: linear-gradient(45deg, silver, #f9e3b1); margin-bottom: 10px; padding-left: 10px; padding-right: 10px;">
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <div class="content-wrapper-before gradient-45deg-indigo-purple"></div>
        <div class="row">
            <div class="col m12">
                <div class="card user-statistics-card mb-0 mt-1">
                    <div class="card-content p-0 h5RoundHead" style="min-height: 45px; margin-bottom: 15px; padding: 6px 0px 6px 0px !important;">
                        <asp:Label CssClass="float-left white-text ml-1 mr-2" ID="lblDailyTitle" runat="server" ClientIDMode="Static" style="font-size:24px;">Filter Dashboard : </asp:Label>
                        <asp:DropDownList ID="drpDailyMonth" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-left myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpDailyMonthYear_SelectedIndexChanged"></asp:DropDownList>
                        <asp:DropDownList ID="drpDailyYear" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-left myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpDailyMonthYear_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- CRM Summary Count -->
            <div class="col m12" style="padding-right: 5px;">
                <uc1:myCRMSummary runat="server" id="myCRMSummary" />
            </div>
        </div>
        <div class="row">
            <!-- ToDO Tasks -->
            <div class="col m6" style="padding-right: 5px;">
                <asp:UpdatePanel ID="updDailyToDO" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="Add New ToDO" href="javascript:openDashboardBox('todo', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="spnPageHeader" runat="server" clientidmode="Static" class="titleHead text-top">To-DO Tasks
                                    &nbsp;<span id="spnToDOCount" runat="server" clientidmode="Static" class="badge gradient-45deg-yellow-green black-text" style="line-height: 25px; float: none; line-height: 25px; vertical-align: text-top; padding: 5px 10px; border-radius: 8px;"></span>
                                    </span>
                                    <asp:DropDownList ID="drpToDO" runat="server" class="myDropDownClass select2-theme browser-default mr-2 float-right" ClientIDMode="Static" OnSelectedIndexChanged="drpToDO_SelectedIndexChanged" TabIndex="5" AutoPostBack="true"  style="width:120px;" >
                                        <asp:ListItem Text="Todays" Value="Todays" />
                                        <asp:ListItem Text="Pending" Value="Pending" />
                                        <asp:ListItem Text="Completed" Value="Completed" />
                                        <asp:ListItem Text="Pending-OverDue" Value="Pending-OverDue" />
                                        <asp:ListItem Text="Completed-OverDue" Value="Completed-OverDue" />
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="drpEmployeeToDO" runat="server" class="myDropDownClass select2-theme browser-default mr-2 float-right" ClientIDMode="Static" OnSelectedIndexChanged="drpEmployeeToDO_SelectedIndexChanged" TabIndex="5" AutoPostBack="true" style="width:150px;" />
                                    
                                </h5>
                                <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myToDo runat="server" ID="myToDo" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpToDO" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <!-- Follow Up -->
            <div class="col m6" style="padding-left: 5px;">
                <asp:UpdatePanel ID="updDailyFollowup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <a id="lndAddFollowUp" class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New FollowUp" href="javascript:openDashboardBox('followup', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="Span1" runat="server" clientidmode="Static" class="titleHead text-top">Follow Up
                                    &nbsp;<span id="spnFollowCount" runat="server" clientidmode="Static" class="badge gradient-45deg-yellow-green black-text" style="line-height: 25px; float: none; line-height: 25px; vertical-align: text-top; padding: 5px 10px; border-radius: 8px;"></span>
                                    </span>
                                    <asp:DropDownList ID="drpFollowup" runat="server" class="myDropDownClass select2-theme browser-default mr-2 float-right" ClientIDMode="Static" OnSelectedIndexChanged="drpFollowup_SelectedIndexChanged" TabIndex="5" AutoPostBack="true">
                                        <asp:ListItem Text="Todays" Value="Todays" />
                                        <asp:ListItem Text="Missed" Value="Missed" />
                                        <asp:ListItem Text="Future" Value="Future" />
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="drpEmployee" runat="server" class="myDropDownClass select2-theme browser-default mr-2 float-right" ClientIDMode="Static" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged" TabIndex="5" AutoPostBack="true">
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myFollowup runat="server" ID="myFollowup" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpEmployee" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="drpFollowup" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <!-- User Activity -->
            <div class="col m12">

                <asp:UpdatePanel ID="updDailyActivity" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <h5 class="h5Round">
                                    <span id="Span3" runat="server" clientidmode="Static" class="titleHead text-top">User's CRM Activity</span>
                                    <asp:DropDownList ID="drpUser" runat="server" ClientIDMode="Static" CssClass="myDropDownClass select2-theme browser-default mr-2 float-right" OnSelectedIndexChanged="drpUser_SelectedIndexChanged" TabIndex="5" AutoPostBack="true">
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 230px;padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myUserActivity runat="server" ID="myUserActivity" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpUser" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <!-- Leads -->
            <div class="col m12">
<%--                <asp:UpdatePanel ID="updDailyLead" runat="server" UpdateMode="Conditional" EnableViewState="true">
                    <ContentTemplate>--%>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0" data-position="center" data-tooltip="Your Assigned Leads" href="#" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">ac_unit</i></a>
                                <h5 class="h5Round">
                                    <span id="Span2" runat="server" clientidmode="Static" class="titleHead text-top">Manage Your Leads</span>
                                     &nbsp;<span id="spnAllLeadCount" runat="server" clientidmode="Static" class="badge gradient-45deg-yellow-green black-text" style="line-height: 25px; float: none; line-height: 25px; vertical-align: text-top; padding: 5px 10px; border-radius: 8px;"></span>
                                    <asp:DropDownList ID="drpLeadEmployee" runat="server" ClientIDMode="Static" CssClass="myDropDownClass select2-theme browser-default mr-2 float-right" OnSelectedIndexChanged="drpLeadEmployee_SelectedIndexChanged" TabIndex="5" AutoPostBack="true">
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 250px; max-height: 250px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myAllLeads runat="server" id="myAllLeads" />                                    
                                </div>

                            </div>
                        </div>
<%--                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpLeadEmployee" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="myAllLeads" />
                    </Triggers>
                </asp:UpdatePanel>--%>

            </div>
        </div>
    </div>
    <br />
    <br />
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
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
        <iframe id="ifrModuleDaily" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 85%; padding: 5px;"></iframe>
        <br />
    </asp:Panel>

    <script type="text/javascript">
        window.onload = function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "Daily Activity - Analytics & Summary";

        }
       

        $(document).ready(function () {
            setUserActivityInterface();

            $('.btnTopRightCorner').on("click", function () {
                var pgSource = $('#ifrModuleDaily').attr('src');
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

        function openDashboardFollowUp(view, mode, id, custid) {
            if (custid == '0' || custid == '') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Followup";
                var pageUrl = "FollowupExternal.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            else {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Followup";
                var pageUrl = "Followup.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
        }

        function openDashboardBox(view, mode, id) {
            if (view == 'product') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Products";
                var pageUrl = "Products.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            if (view == 'customer') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Customer";
                var pageUrl = "Customers.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            if (view == 'todo') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Task Activity";
                var pageUrl = "ToDo.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            if (view == 'followup') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Followup";
                var pageUrl = "Followup.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            if (view == 'leave') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leave Request";
                var pageUrl = "LeaveRequest.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            if (view == 'user') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Users";
                var pageUrl = "UserMgmt.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            if (view == 'complaint') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Complaints";
                var pageUrl = "Complaint.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();

            }
            if (view == 'inquiry') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leads";
                var pageUrl = "InquiryInfo.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            if (view == 'inquirybyno') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leads";
                jQuery.ajax({
                    type: "POST",
                    url: 'InquiryInfo.aspx/GetInquiryNoPrimaryID',
                    data: '{pInqNo:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        var returnID = parseInt(data.d);
                        if (returnID > 0) {
                            var pageUrl = "InquiryInfo.aspx?mode=view&id=" + returnID;
                            $('#ifrModuleDaily').attr('src', pageUrl);
                            $find("mpe").show();
                        }
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });

            }
            if (view == 'quotation') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Quotation";
                var pageUrl = "Quotation.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleDaily').attr('src', pageUrl);
                $find("mpe").show();
            }
            if (view == 'quotationbyno') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Quotation";
                jQuery.ajax({
                    type: "POST",
                    url: 'Quotation.aspx/GetQuotationNoPrimaryID',
                    data: '{pQuotationNo:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var id;
                        var resultdata = JSON.parse(data.d);
                        var returnID = parseInt(data.d);
                        if (returnID > 0) {
                            var pageUrl = "Quotation.aspx?mode=view&id=" + returnID;
                            $('#ifrModuleDaily').attr('src', pageUrl);
                            $find("mpe").show();
                        }
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
        }
    </script>
</asp:Content>
