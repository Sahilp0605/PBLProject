<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="DashboardUser.aspx.cs" Inherits="StarsProject.DashboardUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/myOrderApproval.ascx" TagPrefix="uc1" TagName="myOrderApproval" %>
<%@ Register Src="~/myLeaveRequest.ascx" TagPrefix="uc1" TagName="myLeaveRequest" %>
<%@ Register Src="~/myUserActivity.ascx" TagPrefix="uc1" TagName="myUserActivity" %>
<%@ Register Src="~/mySalesTarget.ascx" TagPrefix="uc1" TagName="mySalesTarget" %>
<%@ Register Src="~/myClientLedger.ascx" TagPrefix="uc1" TagName="myClientLedger" %>
<%@ Register Src="~/myExternalLeads.ascx" TagPrefix="uc1" TagName="myExternalLeads" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="css/style.default.css" type="text/css" />
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>    
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-ui-1.8.16.custom.min.js") %>'></script>
    <script type="text/javascript" src="js/plugins/jquery.cookie.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />

    <link type="text/css" href="css/font-awesome.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="plugins/iCheck/flat/blue.css" />
    <link type="text/css" rel="stylesheet" href="plugins/morris/morris.css" />
    <script type="text/javascript" src="js/raphael-min.js"></script>
    <script type="text/javascript" src="plugins/morris/morris.min.js"></script>
    
     <%--*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*--%>
    <script type="text/javascript">
        var map;
        var mapOptions;
        var togl = 1;

        function setMyDashboard(pgurl) {
            alert(pgurl)
        }

        function initGraphLayout(cat) {
            var grLegend = '';
            var m1, y1;
            m1 = jQuery("#drpSummaryMonth").val();
            y1 = jQuery("#drpSummaryYear").val();
            // ----------------------------------------------------------
            var srcUrl = "";
            srcUrl = 'Services/NagrikService.asmx/GetDashboard' + cat + 'Summary';
            if (cat == 'Inquiry') {
                grLegend = 'Count';
                jQuery("#dash_InqStatus").show("slide");
            }
            else if (cat == 'Sales') {
                grLegend = 'Gross Amount';
                jQuery("#dash_InqStatus").fadeOut("slow");
            }
            // -----------------------------------------------------------
            if (srcUrl != '' && srcUrl != null) {
                jQuery("#line-chart").empty();
                jQuery("#donut-chart1").empty();
                jQuery("#donut-chart2").empty();
                // ---------------------------------------
                // Loading Line Graph
                // ---------------------------------------
                jQuery.ajax({
                    type: "POST", url: srcUrl,
                    data: '{pMon: ' + m1 + ', pYear: ' + y1 + ', pCategory:\'YEARLY\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        // ---------------------------------------
                        if (resultdata.length == 0 || data.d == undefined) {
                            jQuery("#line-chart").parent().attr("class", "hide");
                            jQuery("#label").hide();
                            var resultdata = [{ label: "No Data Availble", value: 0 }];
                        }
                        else {
                            jQuery("#line-chart").parent().attr("class", "show");
                        }
                        // ---------------------------------------
                        var linechart;
                        if (cat == 'Inquiry') {
                            linechart = new Morris.Bar({
                                element: 'line-chart', resize: true, data: resultdata,
                                xkey: 'label', ykeys: ['value1', 'value2', 'value3', 'value4', 'value5', 'value6'],
                                count: [1, 2, 1, 2, 1, 5],
                                labels: ['Open', 'WIP', 'Hold', 'Lost', 'Success', 'Other'],
                                barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                gridTextFamily: 'Open Sans', gridTextSize: 12
                            });
                        }
                        else {
                            linechart = new Morris.Bar({
                                element: 'line-chart', resize: true, data: resultdata,
                                xkey: 'label', ykeys: ['value'], labels: [grLegend],
                                barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'], lineWidth: 2, hideHover: 'auto',
                                gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                gridTextFamily: 'Open Sans', gridTextSize: 12
                            });
                        }
                        // ---------------------------------------
                        linechart.draw();
                        // ---------------------------------------
                        // Loading Donut-1
                        // ---------------------------------------
                        jQuery.ajax({
                            type: "POST", url: srcUrl,
                            data: '{pMon: ' + m1 + ', pYear: ' + y1 + ', pCategory:\'BRAND\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                var resultdata1 = JSON.parse(data.d);
                                var donutchart1 = new Morris.Donut({
                                    element: 'donut-chart1', resize: true, colors: ['#007bff', '#dc3545', '#28a745', '#17a2b8', '#ffc107', '#000080', '#6c757d', '#ff6a52', '#8afbe6'],
                                    data: resultdata1, hideHover: 'auto'
                                })
                                // ---------------------------------------
                                donutchart1.redraw();
                                // ---------------------------------------
                                // Loading Donut-2
                                // ---------------------------------------
                                jQuery.ajax({
                                    type: "POST", url: srcUrl,
                                    data: '{pMon: ' + m1 + ', pYear: ' + y1 + ', pCategory:\'PRGROUP\'}',
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        var resultdata2 = JSON.parse(data.d);
                                        var donutchart2 = new Morris.Donut({
                                            element: 'donut-chart2', resize: true, colors: ['#007bff', '#dc3545', '#28a745', '#17a2b8', '#ffc107', '#000080', '#6c757d', '#ff6a52', '#8afbe6'],
                                            data: resultdata2, hideHover: 'auto'
                                        })
                                        // ---------------------------------------
                                        donutchart2.redraw();
                                    },
                                    error: function (r) { alert('Error : ' + r.responseText); },
                                    failure: function (r) { alert('failure'); }
                                });
                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });



            }
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Loading External Leads Graphical Analysis
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function initExternalGraph(cat) {
            var grLegend = '';
            var m1, y1;
            m1 = jQuery("#drpSummaryMonth").val();
            y1 = jQuery("#drpSummaryYear").val();
            // ----------------------------------------------------------
            var srcUrl = "";
            srcUrl = 'Services/NagrikService.asmx/GetDashboardExternalSummary';
            grLegend = 'Count';
            jQuery("#dash_External").show("slide");
            // -----------------------------------------------------------
            if (srcUrl != '' && srcUrl != null) {
                jQuery("#external-chart").empty();
                // ---------------------------------------
                // Loading Line Graph
                // ---------------------------------------
                // Checkbox with Image : ttps://stackoverflow.com/questions/30663562/use-images-like-checkboxes
                jQuery.ajax({
                    type: "POST", url: srcUrl,
                    data: '{pMon: ' + m1 + ', pYear: ' + y1 + ', pCategory:\'IndiaMart\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        // ---------------------------------------
                        if (resultdata.length == 0 || data.d == undefined) {
                            jQuery("#external-chart").parent().attr("class", "hide");
                            jQuery("#label").hide();
                            var resultdata = [{ label: "No Data Availble", value: 0 }];
                        }
                        else {
                            jQuery("#external-chart").parent().attr("class", "show");
                        }
                        // ---------------------------------------

                        var extchart;

                        extchart = new Morris.Bar({
                            element: 'external-chart', resize: true, data: resultdata,
                            xkey: 'label', ykeys: ['value1', 'value2'], count: [1, 2, 1, 2, 1, 5],
                            labels: ['Qualified', 'DisQualified'],
                            barColors: ['#28a745', '#c82333', '#007bff', '#17a2b8', '#6c757d', '#ffc107'],
                            lineWidth: 2, hideHover: 'auto',
                            gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                            gridTextFamily: 'Open Sans', gridTextSize: 12
                        });
                        // ---------------------------------------
                        extchart.draw();
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
        }

        // ------------------------------------------------------------------------
        function pageLoad(sender, args) {
            initExternalGraph('IndiaMart');
            reloadLayout();

        }

        // ------------------------------------------------------------------------
        // Functionality : Complaint Visit
        // ------------------------------------------------------------------------
        function showComplaintVisit(t11, t12, t13) {
            $.colorbox({
                fixed: true, width: "98%", height: "98%", rel: 'cbComplaintVisit', inline: true, iframe: true, href: "myCustomView.aspx?view=complaintvisit&para1=" + t11 + '&para2=' + t12 + '&para3=' + t13,
                onClosed: function () { }
            });
        }
        // ------------------------------------------------------------------------
        // Functionality : Customer View for INQUIRY
        // ------------------------------------------------------------------------
        function openCustomerView(t11) {
            var t12, t13;
            t12 = jQuery("#drpSummaryMonth").val();
            t13 = jQuery("#drpSummaryYear").val();

            $.colorbox({
                width: "98%", height: "98%", rel: 'cbInq', inline: true, iframe: true, href: "myCustomView.aspx?view=inquiry&para1=" + t11 + '&para2=' + t12 + '&para3=' + t13,
                onClosed: function () { }
            });
        }
        // ------------------------------------------------------------------------
        // Functionality : Customer FollowUp Timeline
        // ------------------------------------------------------------------------
        function openTimeline(xView, xCustID, xCustName) {
            $.colorbox({ fixed: true, width: "50%", height: "90%", rel: 'cbTimeline', iframe: true, href: "FollowupTimeline.aspx?view=" + xView + "&id=" + xCustID + "&name=" + xCustName });
        }
        // ------------------------------------------------------------------------
        // Functionality : Registered Complaint View
        // ------------------------------------------------------------------------
        function openComplaint(pkid) {
            $.colorbox({ fixed: true, width: "85%", height: "95%", rel: 'cbComplaint', iframe: true, href: "Complaint.aspx?mode=view&id=" + pkid });
        }
        // ------------------------------------------------------------------------
        // Functionality : Registered Complaint Visit View
        // ------------------------------------------------------------------------
        function openComplaintVisit(pkid, complaintno) {
            $.colorbox({ fixed: true, width: "85%", height: "95%", rel: 'cbComplaint', iframe: true, href: "ComplaintVisit.aspx?mode=view&id=" + pkid + "&complaintno=" + complaintno });
        }

        function openDashboardBox(view, pID) {
            if (view == 'todo') {
                $.colorbox({ width: "95%", height: "85%", iframe: true, href: "ToDo.aspx?id=0", onClosed: function () { location.reload(); } });
            }
            if (view == 'todoview') {
                $.colorbox({ width: "95%", height: "85%", iframe: true, href: "ToDo.aspx?mode=view&id=" + pID , onClosed: function () { } });
            }
            if (view == 'followup') {
                $.colorbox({
                    fixed: true, width: "85%", height: "75%", iframe: true, href: "Followup.aspx?mode=add&id=0",
                    onClosed: function () {
                        location.reload();
                    }
                });
            }
            if (view == 'leave') {
                $.colorbox({
                    fixed: true, width: "75%", height: "95%", iframe: true, href: "LeaveRequest.aspx?mode=add&id=0",
                    onClosed: function () {
                        location.reload();
                    }
                });
            }
            if (view == 'user') {
                $.colorbox({
                    fixed: true, width: "85%", height: "75%", iframe: true, href: "UserMgmt.aspx?mode=add&id=0",
                    onClosed: function () {
                        location.reload();
                    }
                });
            }
            if (view == 'complaint') {
                $.colorbox({
                    fixed: true, width: "85%", height: "95%", iframe: true, href: "Complaint.aspx?mode=add&id=0",
                    onClosed: function () {
                        location.reload();
                    }
                });
            }
        }

        function openLoader() {
            document.getElementById("loader").style.display = "block";
        }

        function closeLoader() {
            document.getElementById("loader").style.display = "none";
        }

        function reloadLayout() {
            var uid = $("#hdnLoginUserID").val();
            $.ajax({
                type: "POST", url: 'Services/NagrikService.asmx/GetWidgetList',
                data: '{LoginUserID: \'' + uid + '\'}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var markers = JSON.parse(response.d);
                    for (i = 0; i < markers.length; i++) {
                        var data = markers[i]
                        var ctrlDiv = data.label;
                        //alert(ctrlDiv)
                        //alert(data.flag)
                        if (data.flag == 0) {
                            $('#' + ctrlDiv).attr('style', 'display:none');
                        }
                        else {
                            if (ctrlDiv == 'dash_Graphical') {
                                $('#' + ctrlDiv).css('display', 'flex');
                            }
                            else if (ctrlDiv == 'dash_InqStatus') {
                                var cat = jQuery("#drpSummaryMode").val();
                                if (cat == 'Inquiry') {
                                    $('#' + ctrlDiv).css('display', 'block');
                                }
                                else if (cat == 'Sales') {
                                    $('#' + ctrlDiv).css('display', 'none');
                                }
                            }
                            else if (ctrlDiv == 'dash_External') {
                                $('#' + ctrlDiv).css('display', 'block');
                            }
                            else {
                                $('#' + ctrlDiv).css('display', 'inline-block');
                            }
                        }
                    }
                }
            });
            // --------------------------------------------------------------
            //var divList = $("#divWidget .card").filter(':visible');
            var divList = $("#divWidget .card");
            divList.sort(function (a, b) { return $(a).data("listing-price") - $(b).data("listing-price") });
            $("#divWidget").html(divList);
        }

        $("#divToDOMultiple").click(function () {
            alert('testeeeee');
        });
    </script>

    <style type="text/css">
        .rightfixed {
            position: fixed;
            top: 28%;
            right: 0;
            height: 335px;
            z-index: 9999;
            width: 38px;
        }

            .rightfixed .frilst {
                width: 38px;
                height: 125px;
                background: url(images/SaveData.png) no-repeat 0 0;
                display: inline-block;
                cursor: pointer;
            }

            .rightfixed .cscare {
                width: 38px;
                height: 153px;
                background: url(images/SaveData.png) no-repeat 0 0;
                display: inline-block;
                cursor: pointer;
            }

            .rightfixed .jdscl {
                width: 38px;
                height: 108px;
                background: url(images/SaveData.png) no-repeat 0 0;
                display: inline-block;
                cursor: pointer;
            }

        /*---- //*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /*//*/ /* ----*/
        .ImgbtnSelect {
            Width: 40px;
            Height: 40px;
            float: left;
        }

        .divInfo {
            float: left;
            margin: 3px;
            padding: 2px;
            border: 0px;
            width: 100% !important;
        }

        .divHead {
            height: 30px;
            float: left;
            padding: 2px;
            border: 2px solid silver;
            background-color: #ddd;
            font-family: verdana,Arial,Helvetica,sans-serif;
            font-size: 14px;
            font-weight: bold;
            min-width: 99%;
            vertical-align: middle;
            text-align: center;
        }

        .stdtable {
            height: 168px;
            font-size: 14px;
            color: Navy;
            font-weight: 400;
        }

            .stdtable th {
                text-align: center !important;
            }

            .stdtable td {
                text-align: center !important;
            }

        #map_canvas {
            text-align: center;
            float: center;
            min-width: 48% !important;
            height: 600px !important;
        }

        .stdtable tbody tr td:first-child {
            border-left: 0px !important;
            border-right: 0px !important;
        }

        .stdtable tbody tr td:last-child {
            border-left: 0px !important;
            border-right: 0px !important;
        }

        .boxClose {
            display: none;
            height: 0px;
        }

        .boxOpen {
            display: block;
        }

        .limited {
            white-space: nowrap;
            max-width: 100px;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .progmodal {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .progcenter {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .progcenter img {
                height: 128px;
                width: 128px;
            }

        .morris-hover {
            position: absolute;
            z-index: 1000;
        }

        .morris-hover-point {
            color: greenyellow !important;
        }
        /* --------------------------------------------------------------- */
        #loader {
            position: absolute;
            left: 50%;
            top: 50%;
            z-index: 1;
            width: 150px;
            height: 150px;
            margin: -75px 0 0 -75px;
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        /* Add animation to "page content" */
        .animate-bottom {
            position: relative;
            -webkit-animation-name: animatebottom;
            -webkit-animation-duration: 1s;
            animation-name: animatebottom;
            animation-duration: 1s;
        }

        @-webkit-keyframes animatebottom {
            from {
                bottom: -100px;
                opacity: 0;
            }

            to {
                bottom: 0px;
                opacity: 1;
            }
        }

        @keyframes animatebottom {
            from {
                bottom: -100px;
                opacity: 0;
            }

            to {
                bottom: 0;
                opacity: 1;
            }
        }

        #myDiv {
            display: none;
            text-align: center;
        }

        .popover {
            left: 5px !important;
            top: 6px;
            min-width: 600px;
        }

            .popover .arrow {
                left: 6px !important;
            }

        .card-footer {
            border-top: 2px solid navy !important;
        }

        .SummaryCount {
            display: inline-block;
            font-weight: 400;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            user-select: none;
            border: 1px solid transparent;
            font-size: 1rem;
            line-height: 1.15;
            border-radius: .25rem;
        }

        .textureWodden {
            background: url(images/texture_wooden.jpg);
            background-size: cover;
        }

        .textureBubble {
            background: url(images/texture_bubble.jpg);
            background-size: cover;
            color: lightyellow;
        }

        .textureCardboard1 {
            background: url(images/texture_cardboard.jpg);
            background-size: cover;
        }

        .textureCardboard2 {
            background: url(images/texture_cardboard1.jpg);
            background-size: cover;
        }

        .textureGradient {
            background: url(images/texture_gradient1.jpg);
            background-size: cover;
        }

        .textureLight1 {
            background: url(images/texture_light1.jpg);
            background-size: cover;
        }

        .textureLight2 {
            background: url(images/texture_light2.jpg);
            background-size: cover;
        }

        .textureLight3 {
            background: url(images/texture_light3.jpg);
            background-size: cover;
        }

        .texturePaper1 {
            background: url(images/texture_paper.jpg);
            background-size: cover;
        }

        .texturePaper2 {
            background: url(images/texture_paper1.jpg);
            background-size: cover;
        }

        .texturePaper3 {
            background: url(images/texture_paper2.jpg);
            background-size: cover;
        }

        .lightDropdown {
            background-color: lightyellow;
            color: navy;
            font-size: 16px;
            font-weight: 400;
            border-radius: 4px;
            margin-top: 0px;
            padding: 0px 10px;
        }
    </style>

</head>
<body>
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" CompositeScript-ScriptMode="Inherit"></asp:ScriptManager>
        <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTODOVersion" runat="server" ClientIDMode="Static" />
        <div class="pageheader texturePaper2" style="padding-bottom: 0px !important; font-family: 'Montserrat',sans-serif;">
            <h1 class="pagetitle contenttitle2" style="display: block !important;">
                <asp:DropDownList ID="drpDashboard" CssClass="form-control float-left" runat="server" ClientIDMode="Static" Style="border-radius: 6px; border: 1px solid navy; padding: 0px; width: 180px; font-size: 18px; margin-right: 15px;" AutoPostBack="true" OnSelectedIndexChanged="drpDashboard_SelectedIndexChanged">
                    <asp:ListItem Text="Dashboard - User" Value="DashboardUser" />
                    <asp:ListItem Text="Dashboard - Bussiness" Value="DashboardBussiness" />
                </asp:DropDownList>

                <asp:DropDownList ID="drpSummaryMonth" CssClass="form-control" runat="server" ClientIDMode="Static" Style="border-radius: 6px; border: 1px solid navy; padding: 0px; width: 150px; float: right; font-size: 18px;" AutoPostBack="true" OnSelectedIndexChanged="drpSummaryMonthYear_SelectedIndexChanged"></asp:DropDownList>
                <asp:DropDownList ID="drpSummaryYear" CssClass="form-control" runat="server" ClientIDMode="Static" Style="border-radius: 6px; border: 1px solid navy; padding: 0px; width: 150px; float: right; font-size: 18px; margin-right: 10px;" AutoPostBack="true" OnSelectedIndexChanged="drpSummaryMonthYear_SelectedIndexChanged"></asp:DropDownList>
                <asp:DropDownList ID="drpSummaryMode" CssClass="form-control" runat="server" ClientIDMode="Static" Style="border-radius: 6px; border: 1px solid navy; padding: 0px; width: 150px; float: right; font-size: 18px; margin-right: 15px;" AutoPostBack="true" OnSelectedIndexChanged="drpSummaryMode_SelectedIndexChanged">
                    <asp:ListItem Text="Inquiry" Value="Inquiry" />
                    <asp:ListItem Text="Sales" Value="Sales" />
                </asp:DropDownList>

            </h1>
        </div>

        <div id="contentwrapper" class="contentwrapper" style="width: 99%; padding: 5px 10px !important;">
            <div class="container-fluid">
                <div id="dash_InqStatus" class="row-fluid mb-1" runat="server" clientidmode="Static">
                    <%--This section Define "Inquiry Count Summary" --%>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
                        <ContentTemplate>
                            <button type="button" class="col m2 btn btn-primary" style="max-width: 16.17%;" onclick="javascript:openCustomerView('Open');">
                                <asp:Label ID="aOpen" runat="server" ClientIDMode="Static"></asp:Label>
                            </button>
                            <button type="button" class="col m2 btn btn-info" style="min-width: 16.75%;" onclick="javascript:openCustomerView('Work In Progress');">
                                <asp:Label ID="aWorkProg" runat="server" ClientIDMode="Static"></asp:Label>
                            </button>
                            <button type="button" class="col m2 btn btn-secondary" style="max-width: 16.17%;" onclick="javascript:openCustomerView('On Hold');">
                                <asp:Label ID="aHold" runat="server" ClientIDMode="Static"></asp:Label>
                            </button>
                            <button type="button" class="col m2 btn btn-danger" style="max-width: 16.17%;" onclick="javascript:openCustomerView('Close - Lost');">
                                <asp:Label ID="aLost" runat="server" ClientIDMode="Static"></asp:Label>
                            </button>
                            <button type="button" class="col m2 btn btn-success" style="max-width: 16.17%;" onclick="javascript:openCustomerView('Close - Success');">
                                <asp:Label ID="aSuccess" runat="server" ClientIDMode="Static"></asp:Label>
                            </button>
                            <button type="button" class="col m2 btn btn-light" style="background-color: #ffc107; max-width: 16.17%;" onclick="javascript:openCustomerView('Other');">
                                <asp:Label ID="aUnknown" runat="server" ClientIDMode="Static"></asp:Label>
                            </button>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="drpSummaryMonth" />
                            <asp:AsyncPostBackTrigger ControlID="drpSummaryYear" />
                            <asp:AsyncPostBackTrigger ControlID="drpSummaryMode" />
                            <%--                    <asp:AsyncPostBackTrigger ControlID="drpToDo" />
                    <asp:AsyncPostBackTrigger ControlID="drpFollowup" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <%--This section Define "Graphical Analysis" --%>
                <div id="dash_Graphical" class="row mb-1">
                    <div class="col m6 pr-0">
                        <!-- solid sales graph -->
                        <div class="card bg-info-gradient" style="background: #ced3d4; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#ced3d4),color-stop(1,#89d8e4));">
                            <div class="card-header no-border" style="padding: 5px 10px;">
                                <h5 class="card-title"><i class="fa fa-area-chart ml-2 mr-2"></i><span id="spAnalysis1">Annual Statistics</span></h5>
                            </div>
                            <div class="card-body" style="padding: 0px !important;">
                                <div class="chart" id="line-chart" style="position: relative; height: 200px;"></div>
                            </div>
                            <!-- /.card-body -->
                        </div>
                    </div>

                    <div class="col m6">
                        <!-- Donut : Product Brand -->
                        <div class="card bg-info-gradient" style="width: 49%; display: inline-table; background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
                            <div class="card-header no-border" style="padding: 5px 10px;">
                                <h5><i class="fa fa-pie-chart ml-2 mr-2"></i><span id="spAnalysis2">Product Brand</span></h5>
                            </div>
                            <div class="card-body" style="padding: 0px !important;">
                                <div class="chart tab-pane" id="donut-chart1" style="position: relative; height: 205px;"></div>
                            </div>
                            <!-- /.card-body -->
                        </div>
                        <!-- Donut : Product Group -->
                        <div class="card bg-info-gradient" style="width: 49%; display: inline-flex; background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
                            <div class="card-header no-border" style="padding: 5px 10px;">
                                <h5><i class="fa fa-pie-chart ml-2 mr-2"></i><span id="spAnalysis3">Product Group</span></h5>
                            </div>
                            <div class="card-body" style="padding: 0px !important;">
                                <div class="chart tab-pane" id="donut-chart2" style="position: relative; height: 205px;"></div>
                            </div>
                            <!-- /.card-body -->
                        </div>
                    </div>
                </div>

                <%--This section Define "External Leads - Graphical Analysis" --%>
                <div id="dash_External" class="row mb-1 mr-1" style="display: none;">
                    <div class="col m12 pr-0">
                        <!-- solid sales graph -->
                        <div class="card bg-info-gradient" style="background: #a9c1db; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#a9c1db),color-stop(1,#89d8e4));">
                            <div class="card-header no-border" style="padding: 5px 10px;">
                                <h5 class="card-title"><i class="fa fa-area-chart ml-2 mr-2"></i><span id="extAnalysis">External Leads Summary</span></h5>
                            </div>
                            <div class="card-body" style="padding: 0px !important;">
                                <div class="chart" id="external-chart" style="position: relative; width: 100%; height: 200px;"></div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel ID="updWidgetList" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div id="divWidget">
                            <!-- TO DO List -->
                            <div id="dash_TODO" class="card m-1" data-listing-price="1" style="width: 49%; box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header p-2 textureBubble">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-tasks ml-2 mr-2"></i>To Do List
                            <asp:DropDownList ID="drpToDo" runat="server" ClientIDMode="Static" CssClass="lightDropdown float-right" AutoPostBack="true" OnSelectedIndexChanged="drpToDO_SelectedIndexChanged">
                            </asp:DropDownList>
                                    </h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body" style="min-height: 250px; max-height: 250px; overflow-y: auto;">
                                    <ul class="todo-list" style="list-style: none;">
                                        <asp:Repeater ID="rptTODO" runat="server" OnItemDataBound="rptTODO_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnPriority" runat="server" ClientIDMode="Static" Value='<%# Eval("Priority") %>' />
                                                <asp:HiddenField ID="hdnCompletionDate" runat="server" ClientIDMode="Static" Value='<%# Eval("CompletionDate") %>' />
                                                <li style="background-color: #f4f4f4; border-left: 2px solid #d6c7c7; padding: 5px 0px 5px 10px; margin-bottom: 8px;">
                                                    <span class="handle">
                                                        <i class="fa fa-ellipsis-v"></i>
                                                        <i class="fa fa-ellipsis-v"></i>
                                                        <span class="text" style="padding-left: 3px; font-size: 14px; text-transform: capitalize;">
                                                            <a href="javascript:openDashboardBox('todoview',<%# Eval("pkID")%>);"><%# Eval("FromEmployeeName") %></a>
                                                        </span>
                                                        <small class="float-right badge badge-secondary mr-1"><i class="fa fa-clock-o"></i><%# Eval("Priority") %> </small>
                                                        <% if (hdnTODOVersion.Value != "2")
                                                           { %>
                                                        <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i><%# Eval("Duration") %> Days </small>
                                                        <% } %>
                                                    </span>
                                                    <br />
                                                    <div style="padding: 5px; background-color: white; border-radius: 8px; margin: 5px;">
                                                        <span class="text" style="color: navy; font-size: 12px; font-weight: 400; display: inline-block; overflow: hidden; width: 350px; white-space: nowrap; text-overflow: ellipsis;">
                                                            <%# Eval("TaskDescription") %><br />
                                                        </span>
                                                    </div>
                                                    <div class="row mt-2">
                                                        <div class="col m3" style="margin-right: 15px !important;">
                                                            <small class="badge badge-primary"><i class="fa fa-calendar-check-o"></i><span>Start : </span><%# Eval("StartDate", "{0:dd-MM-yyyy}") %> </small>
                                                        </div>
                                                        <% if (hdnTODOVersion.Value != "2")
                                                           { %>
                                                        <div class="col m3" style="margin-right: 15px !important;">
                                                            <small class="badge badge-secondary"><i class="fa fa-calendar-check-o"></i><span>Due : </span><%# Eval("DueDate", "{0:dd-MM-yyyy}") %> </small>
                                                        </div>
                                                        <div class="col m" id="divCompletionDate" runat="server" clientidmode="Static">
                                                            <small class="badge badge-success"><i class="fa fa-calendar-check-o"></i><span>Completed : </span>
                                                                <span id="ltrCompletion" runat="server" clientidmode="Static"><%# Eval("CompletionDate", "{0:dd-MM-yyyy}") %></span>
                                                            </small>
                                                        </div>
                                                        <% } %>
                                                        <% if (hdnTODOVersion.Value == "2")
                                                           { %>
                                                        <div id="divToDOMultiple" class="col m float-right" onclick="javascript:showToDODetail(<%# Eval("pkID")%>);">
                                                            <small class="float-right badge mr-1" style="background-color: #0242b7; color: white; font-size: 13px !important;"><i class="fa fa-check-square-o"></i>&nbsp;&nbsp;<%# Eval("SubTaskCompleted") %> / <%# Eval("TotalSubTask") %> </small>
                                                        </div>
                                                        <% } %>
                                                    </div>

                                                    <div class="tools" style="display: none;">
                                                        <i class="fa fa-edit"></i>
                                                        <i class="fa fa-trash-o"></i>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer clearfix">
                                    <asp:Label CssClass="badge badge-danger p-2 SummaryCount" ID="lblToDOCount" runat="server" ClientIDMode="Static" />
                                    <button type="button" class="btn btn-info float-right" onclick="javascript:openDashboardBox('todo',0);"><i class="fa fa-plus"></i>Add Task</button>
                                </div>
                            </div>

                            <!-- FollowUp List -->
                            <div id="dash_FollowUp" data-listing-price="1" class="card m-1" style="width: 49%; box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header p-2 textureBubble">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-check-square-o ml-2 mr-2"></i>Followup List
                            <asp:DropDownList ID="drpFollowup" runat="server" ClientIDMode="Static" CssClass="lightDropdown float-right" AutoPostBack="true" OnSelectedIndexChanged="drpFollowup_SelectedIndexChanged">
                                <asp:ListItem Text="Todays" Value="Todays" />
                                <asp:ListItem Text="Missed" Value="Missed" />
                            </asp:DropDownList>
                                    </h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body" style="min-height: 250px; max-height: 250px; overflow-y: auto;">
                                    <asp:Repeater ID="rptFollowup" runat="server" OnItemDataBound="rptFollowup_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnNextFollowup" runat="server" ClientIDMode="Static" Value='<%# Eval("NextFollowupDate") %>' />
                                            <asp:HiddenField ID="hdnFollInqNo" runat="server" ClientIDMode="Static" Value='<%# Eval("InquiryNo") %>' />
                                            <asp:HiddenField ID="hdnFollSource" runat="server" ClientIDMode="Static" Value='<%# Eval("InquirySource") %>' />
                                            <ul class="todo-list" style="list-style: none;">
                                                <li style="background-color: #f4f4f4; border-left: 2px solid #d6c7c7; padding: 5px 0px 5px 10px;">
                                                    <span class="handle">
                                                        <i class="fa fa-ellipsis-v"></i>
                                                        <i class="fa fa-ellipsis-v"></i>
                                                        <span class="text" style="color: black; padding-left: 5px; font-weight: 600; text-transform: capitalize;">
                                                            <a href="javascript:openTimeline('followup','<%# Eval("CustomerID") %>','<%# Eval("CustomerName") %>');"><%# Eval("CustomerName") %></a>
                                                        </span>
                                                        <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i><%# Eval("InquiryStatus") %> </small>
                                                    </span>
                                                    <br />
                                                    <span class="text" style="color: navy; font-size: 12px; font-weight: 400;"><%# Eval("MeetingNotes") %></span>
                                                    <div class="row mt-2">
                                                        <div class="col m float-left w-100" style="margin-right: 5px !important;">
                                                            <small class="badge badge-primary w-100"><i class="fa fa-calendar-check-o"></i><span>Followup Date: </span><%# Eval("FollowupDate", "{0:dd-MM-yyyy}") %> </small>
                                                        </div>
                                                        <div class="col m6 float-right w-100" style="margin-right: 5px !important;">
                                                            <small class="badge badge-secondary float-right"><i class="fa fa-calendar-check-o"></i><span>Next Followup: </span>
                                                                <span id="ltrNextFollowup" runat="server" clientidmode="Static"><%# Eval("NextFollowupDate", "{0:dd-MM-yyyy}") %>&nbsp;<b><%# Eval("PreferredTime") %></b></span>
                                                            </small>
                                                        </div>
                                                    </div>
                                                    <div class="row mt-2">
                                                        <div class="col m float-left w-100" style="margin-right: 5px !important;">
                                                            <small class="badge badge-primary w-100"><i class="fa fa-calendar-check-o"></i><span>Inquiry # : </span><%# Eval("InquiryNo") %> </small>
                                                        </div>
                                                        <div class="col m6 float-right w-100" style="padding-left: 0px; margin-right: 5px !important;">
                                                            <small class="badge badge-secondary w-100 float-right"><i class="fa fa-calendar-check-o"></i><span>Source : </span><%# Eval("InquirySource") %></small>
                                                        </div>
                                                    </div>
                                                    <div class="tools" style="display: none;">
                                                        <i class="fa fa-edit"></i>
                                                        <i class="fa fa-trash-o"></i>
                                                    </div>
                                                </li>
                                            </ul>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer clearfix">
                                    <asp:Label CssClass="badge badge-danger p-2 SummaryCount" ID="lblFollowUpCount" runat="server" ClientIDMode="Static" />
                                    <button type="button" class="btn btn-info float-right" onclick="javascript:openDashboardBox('followup',0);"><i class="fa fa-plus"></i>Add FollowUp</button>
                                </div>
                            </div>

                            <!-- Complaint List -->
                            <div id="dash_Complaint" data-listing-price="1" class="card m-1" style="width: 100%; box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header p-2 textureBubble">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-check-square-o ml-2 mr-2"></i>Registered Complaint List
                            <asp:DropDownList ID="drpComplaintStatus" runat="server" ClientIDMode="Static" CssClass="lightDropdown float-right" AutoPostBack="true" OnSelectedIndexChanged="drpComplaintStatus_SelectedIndexChanged">
                                <asp:ListItem Text="Pending" Value="Open" />
                                <asp:ListItem Text="Completed" Value="Close" />
                            </asp:DropDownList>
                                    </h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body" style="min-height: 250px; max-height: 250px; overflow-y: auto;">
                                    <asp:Repeater ID="rptComplaint" runat="server" OnItemDataBound="rptComplaint_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnComplaintID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                            <ul class="todo-list" style="list-style: none;">
                                                <li style="background-color: #9db3da; border-left: 10px solid #dc7b7b; padding: 5px 5px 5px 10px;">
                                                    <div class="row m-2">
                                                        <div class="col m6">
                                                            <span class="handle">
                                                                <span class="text" style="color: white; font-weight: 600; text-transform: capitalize;">
                                                                    <a style="color: navy;" href="javascript:openComplaint('<%# Eval("pkID") %>');"><%# Eval("CustomerName") %></a>
                                                                </span>
                                                            </span>
                                                            <br />
                                                            <div class="text" style="min-height: 60px; background-color: lightyellow; padding: 3px; border-radius: 6px;">
                                                                <span class="text" style="color: navy; font-size: 14px; font-weight: 400;"><%# Eval("ComplaintNotes") %></span>
                                                            </div>
                                                        </div>
                                                        <div class="col m4" style="background-color: white; padding: 8px; border-radius: 8px;">
                                                            <small class="badge badge-primary w-100 mb-1 p-2">
                                                                <i class="fa fa-calendar-check-o"></i><span>Complaint Date : </span><%# Eval("ComplaintDate", "{0:dd-MM-yyyy}") %> 
                                                            </small>
                                                            <small class="badge badge-secondary w-100 mb-1 p-2">
                                                                <i class="fa fa-calendar-check-o"></i><span>Assigned To : </span>
                                                                <span id="ltrAssignedTo" runat="server" clientidmode="Static"><%# Eval("EmployeeName") %></span>
                                                            </small>
                                                        </div>
                                                        <div class="col m text-center" style="border-radius: 8px; background-color: white; margin-left: 8px;">
                                                            <%--<div class="w-100 badge-primary"><span class="p-1">Visit</span></div>--%>
                                                            <div class="w-100 badge-light mt-2">
                                                                <button type="button" class="btn btn-danger" onclick="javascript:openComplaintVisit(0, '<%# Eval("pkID") %>');" style="width: 120px;"><i class="fa fa-plus"></i>Add Visit</button>
                                                            </div>
                                                            <div class="w-100 badge-light mt-1">
                                                                <button type="button" class="btn btn-danger" onclick="javascript:showComplaintVisit(0, '<%# Eval("pkID") %>', 0);" style="width: 120px;"><i class="fa fa-plus"></i>Show Visit</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </li>
                                            </ul>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer clearfix">
                                    <asp:Label CssClass="badge badge-danger p-2 SummaryCount" ID="lblComplaintCount" runat="server" ClientIDMode="Static" />
                                    <button type="button" class="btn btn-info float-right" onclick="javascript:openComplaint(0);"><i class="fa fa-plus"></i>New Complaint</button>

                                </div>
                            </div>

                            <!-- Sales Order List -->
                            <div id="dash_SalesOrder" data-listing-price="2" class="card m-1" style="width: 100%; box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header p-2 textureBubble">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-certificate ml-2 mr-2"></i>Sales Order Approval List
                            <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" CssClass="lightDropdown float-right" AutoPostBack="true" OnSelectedIndexChanged="drpApprovalStatus_SelectedIndexChanged">
                                <asp:ListItem Text="Pending" Value="Pending" />
                                <asp:ListItem Text="On Hold" Value="On Hold" />
                                <asp:ListItem Text="Approved" Value="Approved" />
                                <asp:ListItem Text="Rejected" Value="Rejected" />
                            </asp:DropDownList>
                                    </h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body w-100" style="min-height: 250px; max-height: 250px; overflow-y: auto;">
                                    <uc1:myOrderApproval runat="server" ID="myOrderApproval" />
                                </div>
                            </div>

                            <!-- Leave Request -->
                            <div id="dash_LeaveRequest" data-listing-price="1" class="card m-1" style="width: 49%; box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header p-2 textureBubble">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-paper-plane ml-2 mr-2"></i>Leave Request 
                            <asp:DropDownList ID="drpLeaveStatus" runat="server" ClientIDMode="Static" CssClass="lightDropdown float-right" AutoPostBack="true" OnSelectedIndexChanged="drpLeaveStatus_SelectedIndexChanged">
                                <asp:ListItem Text="Pending" Value="Pending" />
                                <asp:ListItem Text="Approved" Value="Approved" />
                                <asp:ListItem Text="Rejected" Value="Rejected" />
                            </asp:DropDownList>
                                    </h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body w-100" style="height: 270px; overflow-y: auto;">
                                    <uc1:myLeaveRequest runat="server" ID="myLeaveRequest" />
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer clearfix">
                                    <asp:Label CssClass="badge badge-danger p-2 SummaryCount" ID="lblLeaveCount" runat="server" ClientIDMode="Static"></asp:Label>
                                    <button type="button" class="btn btn-info float-right" onclick="javascript:openDashboardBox('leave',0);"><i class="fa fa-plus"></i>Apply Leave</button>
                                </div>
                            </div>

                            <!-- User Activity List -->
                            <div id="dash_UserActivity" data-listing-price="1" class="card m-1" style="width: 49%; box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header p-2 textureBubble">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-user-circle-o ml-2 mr-2"></i>User Activity 
                                    </h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body w-100" style="height: 270px; overflow-y: auto;">
                                    <uc1:myUserActivity runat="server" ID="myUserActivity" />
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer clearfix">
                                    <asp:Label CssClass="badge badge-danger p-2 SummaryCount" ID="lblUserCount" runat="server" ClientIDMode="Static"></asp:Label>
                                    <button type="button" class="btn btn-info float-right" onclick="javascript:openDashboardBox('user',0);"><i class="fa fa-plus"></i>Add User</button>
                                </div>
                            </div>

                            <!-- Sales Target List -->
                            <div id="dash_SalesTarget" data-listing-price="2" class="card m-1" style="width: 100%; box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header p-2 textureBubble">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-bullseye ml-2 mr-2"></i>Sales Target List
                            <asp:DropDownList ID="drpTargetType" runat="server" ClientIDMode="Static" CssClass="lightDropdown float-right" AutoPostBack="true" OnSelectedIndexChanged="drpTargetType_SelectedIndexChanged">
                                <asp:ListItem Text="Amount" Value="A" />
                                <asp:ListItem Text="Quantity" Value="Q" />
                            </asp:DropDownList>
                                    </h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body" style="min-height: 250px; max-height: 250px; overflow-y: auto;">
                                    <uc1:mySalesTarget runat="server" ID="mySalesTarget" />
                                </div>
                            </div>

                            <!-- Customer Ledger List -->
                            <div id="dash_ClientLedger" data-listing-price="1" class="card m-1" style="width: 100%; box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header p-2 textureBubble">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-user-circle-o ml-2 mr-2"></i>Customer Ledger</h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body w-100" style="min-height: 250px; max-height: 250px; overflow-y: auto;">
                                    <uc1:myClientLedger runat="server" ID="myClientLedger" />
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer clearfix">
                                    <asp:Label CssClass="badge badge-danger p-2" ID="lblDebitAmount" runat="server" ClientIDMode="Static" Text="11111" />
                                    <asp:Label CssClass="badge badge-success p-2 float-right" ID="lblCreditAmount" runat="server" ClientIDMode="Static" Text="22222" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpApprovalStatus" />
                        <asp:AsyncPostBackTrigger ControlID="drpLeaveStatus" />
                        <asp:AsyncPostBackTrigger ControlID="drpSummaryMonth" />
                        <asp:AsyncPostBackTrigger ControlID="drpSummaryYear" />
                        <asp:AsyncPostBackTrigger ControlID="drpTargetType" />
                        <asp:AsyncPostBackTrigger ControlID="drpSummaryMode" />
                        <asp:AsyncPostBackTrigger ControlID="drpToDo" />
                        <asp:AsyncPostBackTrigger ControlID="drpFollowup" />
                    </Triggers>
                </asp:UpdatePanel>

                <div id="loader" style="display: none;"></div>
            </div>
        </div>
        <br />
        <br />
        <script type="text/javascript">
            function showToDODetail(iddet) {
                jQuery.ajax({
                    type: "POST",
                    url: "InquiryInfo.aspx/FilterCustomer",
                    data: '{pCustName:\'' + jQuery(selector).val() + '\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var sample = JSON.parse(data.d);

                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
            // ---------------------------------------------------------------------
            // 
            // ---------------------------------------------------------------------
            function speakMyWords(myMsg) {
                try {
                    if ('speechSynthesis' in window) {
                        var msg = new SpeechSynthesisUtterance();
                        msg.text = myMsg;
                        msg.onend = function (e) { console.log('Finished in ' + event.elapsedTime + ' seconds.'); };
                        speechSynthesis.speak(msg);
                    }
                }
                catch (err) { }
            };

        </script>
        <script type="text/javascript" src="js/materialize.min.js"></script>

    </form>
</body>
</html>
