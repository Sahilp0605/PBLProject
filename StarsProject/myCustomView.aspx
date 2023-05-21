<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myCustomView.aspx.cs" Inherits="StarsProject.myCustomView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="js/jquery-3.3.1.js"></script>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/data-tables/css/jquery.dataTables.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/data-tables/extensions/responsive/css/responsive.dataTables.min.css"/>
    <%--<link rel="stylesheet" type="text/css" href="app-assets/vendors/data-tables/css/select.dataTables.min.css"/>--%>

    <script src="app-assets/vendors/data-tables/js/jquery.dataTables.min.js"></script>
    <script src="app-assets/vendors/data-tables/extensions/responsive/js/dataTables.responsive.min.js"></script>
<%--    <script src="app-assets/vendors/data-tables/js/dataTables.select.min.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>

    <style type="text/css">
        .loader {
            border: 16px solid #f3f3f3; /* Light grey */
            border-top: 16px solid #3498db; /* Blue */
            border-radius: 50%;
            width: 120px;
            height: 120px;
            animation: spin 2s linear infinite;
            position: absolute;
            left: 45%;
            top: 45%;
            z-index: 1000;
        }

        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }
        .hide-loader {
            display:none;
        }

        table.dataTable thead th, table.dataTable thead td {
            padding: 5px 10px;
            border-bottom: 1px solid #111;
            line-height: 18px;
            font-size: 12px !important;
        }

        table.dataTable tbody td {
            padding: 0px 3px !important;
            vertical-align: top;
            border: 1px groove steelblue;
            font-size: 12px !important;
            font-weight: 600;
            color: navy;
            text-transform: initial;
        }

        table.dataTable thead {
            /*background: #b5cfd2 url(/images/cell-blue.jpg);*/
            background-color: lightgray;
            background-blend-mode: soft-light;
            Color: black;
        }

            table.dataTable thead th {
                font-size: 14px !important;
            }

        /*.dataTables_filter input {box-shadow: 0 0 15px 5px #b0e0ee;padding: 2px 2px 2px 5px; background-color:aliceblue;}*/
        .dataTables_length {
            padding: 5px 10px 0px 10px;
            border: 1px solid navy;
            border-radius: 8px;
            margin-bottom: 8px;
            background-color: #f0f8ff;
        }

        input[type=search]::-webkit-search-cancel-button {
            -webkit-appearance: searchfield-cancel-button;
        }

        .myDropDownClass {
            background-color: #fff6e1;
            color: navy; 
            font-size: medium;
            width: 150px; height: 30px !important; 
            border: 1px solid black; 
            margin-top:1px; padding:1px 5px;
            border:none;
            border-radius: 5px;
        }

        .Popup  
        {  
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
            display:block;
            box-sizing:border-box;
            width:30px;
            height:30px;
            border-width:3px;
            border-style: solid;
            border-color:red;
            border-radius:100%;
            background: -webkit-linear-gradient(-45deg, transparent 0%, transparent 46%, white 46%,  white 56%,transparent 56%, transparent 100%), -webkit-linear-gradient(45deg, transparent 0%, transparent 46%, white 46%,  white 56%,transparent 56%, transparent 100%);
            background-color:red;
            box-shadow:0px 0px 5px 2px rgba(0,0,0,0.5);
            transition: all 0.3s ease;
            position: relative;
            top: 12px;
            right: 15px;
        }
        .titleHead {
            font-weight:800;
            margin-left: 10px;
        }
        .cardRound { box-shadow: black -6px 8px 6px -6px !important; border-radius: 8px 8px 0px 0px !important; }
        .h5Round { border-radius: 8px 8px 0px 0px; margin-bottom:5px; padding:5px; line-height:35px; background: linear-gradient(45deg, #303f9f, #7b1fa2) !important; color:white !important; }
        .h5RoundHead { border-radius: 0px 0px 0px 0px; margin-bottom:5px; padding:5px; line-height:35px; background: linear-gradient(45deg, silver, #7f88ec6e); }

    </style>

    <script type="text/javascript">


        function show_loader() {
            jQuery("#myLoader").removeClass("hide-loader");
            jQuery("#myLoader").addClass("loader");
        }

        function hide_loader() {
            jQuery("#myLoader").removeClass("loader");
            jQuery("#myLoader").addClass("hide-loader");
        }

        function capitalizeString(str) {
            return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }

        jQuery(document).ready(function () {
            $(".btnTopRightCorner").click(function () {
                var pgSource = $('#ifrModule22').attr('src');
                var strPos = pgSource.indexOf('mode=edit');
                var strPos1 = pgSource.indexOf('mode=add');
                if (strPos >= 0 || strPos1 >= 0) {
                    gridReloadData('reload');
                }

            })

            show_loader();
            //-------------------------------------------------------------
            var dataurl, para1, para2, para3, para4, para5, para0;
            var viewmode = jQuery("#hdnView").val();
            var groupColumn, valColspan;
            var groupColumnTitle = "";
            var serialKey = "";
            // --------------------------------------------------------
            if (viewmode == 'inquiry') {
                
                //para0 = jQuery.trim(jQuery("#hdnLoginUserID").val());
                para0 = jQuery.trim(jQuery("#hdnPara4").val());
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                serialKey = jQuery.trim(jQuery("#hdnSerialKey").val());
                // ---------------------------------------------------------
                if (para1 == 'Other') {
                    groupColumn = (para1 != "Close - Lost") ? 6 : 7;
                    groupColumnTitle = "Inquiry Status :";
                }
                else {
                    groupColumn = (para1 != "Close - Lost") ? 9 : 10;
                    groupColumnTitle = "Sales Executive :";
                }
                // ---------------------------------------------------------
                jQuery("#spnModuleHeader").text('Inquiry List - [ Status : ' + para1 + ' ]')
                // ---------------------------------------------------------
                
                dataurl = '/Services/NagrikService.asmx/webInquiryStatusList?pLoginUserID=' + para0 + '&pStatus=' + para1 + '&pMonth=' + para2 + '&pYear=' + para3;
                if (para1 != "Close - Lost")
                {
                    valColspan = 11;

                    if (serialKey == 'ECO3-2G21-TECH-3MRT')
                    {
                        colstructure = [
                                {
                                    title: "Inquiry #", data: "InquiryNo", width: "110", className: "text-center",
                                    render: function (data, type, row, meta) {
                                        data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                        return data;
                                    }
                                },
                                { title: "Inquiry Date", data: "InquiryDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MM-YYYY') : ''; } },
                                { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                { title: "Contact #", data: "Contact1", className: "text-center", width: "110" },
                                { title: "City", data: "CityName", className: "text-center", width: "110" },
                                { title: "State", data: "StateName", className: "text-center", width: "110" },
                                { title: "Source", data: "InquirySource", className: "text-center", width: "150" },
                                { title: "Status", data: "InquiryStatus", className: "text-center", width: "150" },
                                {
                                    title: "Last Followup", data: "LastFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                        return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MM-YYYY') : '') : '';
                                    }
                                },
                                {
                                    title: "Next Followup", data: "LastNextFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                        return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MM-YYYY') : '') : '';
                                    }
                                },
                                { title: " Sales Executive", data: "EmployeeName", width: "300" }
                        ];
                    }
                    else
                    {
                        colstructure = [
                                {
                                    title: "Inquiry #", data: "InquiryNo", width: "110", className: "text-center",
                                    render: function (data, type, row, meta) {
                                        data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                        return data;
                                    }
                                },
                                { title: "Inquiry Date", data: "InquiryDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                                { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                { title: "City", data: "CityName", className: "text-center", width: "110" },
                                { title: "State", data: "StateName", className: "text-center", width: "110" },
                                { title: "Source", data: "InquirySource", className: "text-center", width: "150" },
                                { title: "Status", data: "InquiryStatus", className: "text-center", width: "150" },
                                {
                                    title: "Last Followup", data: "LastFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                        return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MMM-YYYY') : '') : '';
                                    }
                                },
                                {
                                    title: "Next Followup", data: "LastNextFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                        return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MMM-YYYY') : '') : '';
                                    }
                                },
                                { title: " Sales Executive", data: "EmployeeName", width: "300" }
                        ];
                    }
                    
                }
                else {
                    valColspan = 12;
                    if (serialKey == 'ECO3-2G21-TECH-3MRT') {
                        colstructure = [
                                    {
                                        title: "Inquiry #", data: "InquiryNo", width: "110", className: "text-center",
                                        render: function (data, type, row, meta) {
                                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                            return data;
                                        }
                                    },
                                    { title: "Inquiry Date", data: "InquiryDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MM-YYYY') : ''; } },
                                    { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                    { title: "Contact #", data: "ContactNo1", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                    { title: "City", data: "CityName", className: "text-center", width: "110" },
                                    { title: "State", data: "StateName", className: "text-center", width: "110" },
                                    { title: "Source", data: "InquirySource", className: "text-center", width: "150" },
                                    { title: "Status", data: "InquiryStatus", className: "text-center", width: "150" },
                                    { title: "Reason", data: "ClosureReasonName", className: "text-center", width: "150" },
                                    {
                                        title: "Last Followup", data: "LastFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                            return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MM-YYYY') : '') : '';
                                        }
                                    },
                                    {
                                        title: "Next Followup", data: "LastNextFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                            return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MM-YYYY') : '') : '';
                                        }
                                    },
                                    { title: " Sales Executive", data: "EmployeeName", width: "300" }
                        ];
                    }
                    else
                    {
                        colstructure = [
                                    {
                                        title: "Inquiry #", data: "InquiryNo", width: "110", className: "text-center",
                                        render: function (data, type, row, meta) {
                                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                            return data;
                                        }
                                    },
                                    { title: "Inquiry Date", data: "InquiryDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                                    { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                    { title: "City", data: "CityName", className: "text-center", width: "110" },
                                    { title: "State", data: "StateName", className: "text-center", width: "110" },
                                    { title: "Source", data: "InquirySource", className: "text-center", width: "150" },
                                    { title: "Status", data: "InquiryStatus", className: "text-center", width: "150" },
                                    { title: "Reason", data: "ClosureReasonName", className: "text-center", width: "150" },
                                    {
                                        title: "Last Followup", data: "LastFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                            return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MMM-YYYY') : '') : '';
                                        }
                                    },
                                    {
                                        title: "Next Followup", data: "LastNextFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                            return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MMM-YYYY') : '') : '';
                                        }
                                    },
                                    { title: " Sales Executive", data: "EmployeeName", width: "300" }
                        ];
                    }

                }

                var InquiryShare = jQuery.trim(jQuery("#hdnInquiryShare").val());
                if (InquiryShare == "Yes") {
                    valColspan = (para1 != "Close - Lost") ? 11 : 12;
                    colstructure.push({
                        title: "Share", className: "text-center", width: "60", mRender: function (data, type, row) {
                            return '<img src="images/multi-user-icon.jpg" alt="" style="width:20px; height:20px;" onclick="openInquirySharing(\'' + row.pkID + '\',\'' + jQuery.trim(jQuery("#hdnLoginUserID").val()) + '\');"/>';
                        }
                    });
                }
            }
            else if (viewmode == 'inquirybyuser') {

                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                serialKey = jQuery.trim(jQuery("#hdnSerialKey").val());

                // ---------------------------------------------------------
                jQuery("#lblCustomHead").text('Inquiry List - [ Executive Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webInquiryByUserPeriod?pLoginUserID=' + para1 + "&pFromDate=" + para2 + "&pToDate=" + para3;

                if (serialKey == 'ECO3-2G21-TECH-3MRT') {
                    groupColumn = 10;
                    valColspan = 11;
                    colstructure = [
                                {
                                    title: "Inquiry #", data: "InquiryNo", width: "110", className: "text-center",
                                    render: function (data, type, row, meta) {
                                        data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                        return data;
                                    }
                                },
                                { title: "Inquiry Date", data: "InquiryDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MM-YYYY') : ''; } },
                                { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                { title: "Contact #", data: "ContactNo1", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                { title: "City", data: "CityName", className: "text-center", width: "110" },
                                { title: "State", data: "StateName", className: "text-center", width: "110" },
                                { title: "Source", data: "InquirySource", className: "text-center", width: "150" },
                                { title: "Status", data: "InquiryStatus", className: "text-center", width: "150" },
                                {
                                    title: "Last Followup", data: "LastFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                        return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MM-YYYY') : '') : '';
                                    }
                                },
                                {
                                    title: "Next Followup", data: "LastNextFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                        return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MM-YYYY') : '') : '';
                                    }
                                },
                                { title: " Sales Executive", data: "EmployeeName", width: "300" }
                    ];
                }
                else
                {
                    groupColumn = 9;
                    valColspan = 10;
                    colstructure = [
                                {
                                    title: "Inquiry #", data: "InquiryNo", width: "110", className: "text-center",
                                    render: function (data, type, row, meta) {
                                        data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                        return data;
                                    }
                                },
                                { title: "Inquiry Date", data: "InquiryDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                                { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                { title: "City", data: "CityName", className: "text-center", width: "110" },
                                { title: "State", data: "StateName", className: "text-center", width: "110" },
                                { title: "Source", data: "InquirySource", className: "text-center", width: "150" },
                                { title: "Status", data: "InquiryStatus", className: "text-center", width: "150" },
                                {
                                    title: "Last Followup", data: "LastFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                        return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MMM-YYYY') : '') : '';
                                    }
                                },
                                {
                                    title: "Next Followup", data: "LastNextFollowupDate", className: "text-center", type: "date", width: "150", render: function (data, type, full) {
                                        return (data) ? (moment(data).format('YYYY') >= 2000 ? moment(data).format('DD-MMM-YYYY') : '') : '';
                                    }
                                },
                                { title: " Sales Executive", data: "EmployeeName", width: "300" }
                    ];
                }

                var InquiryShare = jQuery.trim(jQuery("#hdnInquiryShare").val());
                if (InquiryShare == "Yes") {
                    valColspan = valColspan + 1;
                    colstructure.push({
                        title: "Share", className: "text-center", width: "60", mRender: function (data, type, row) {
                            return '<img src="images/multi-user-icon.jpg" alt="" style="width:20px; height:20px;" onclick="openInquirySharing(\'' + row.pkID + '\',\'' + jQuery.trim(jQuery("#hdnLoginUserID").val()) + '\');"/>';
                        }
                    });
                }
            }
            else if (viewmode == 'externalstatus') {
                groupColumn = 9;
                valColspan = 9;
                para1 = jQuery.trim(jQuery("#hdnPara1").val()); // Lead Status
                para2 = jQuery.trim(jQuery("#hdnPara2").val()); // Month
                para3 = jQuery.trim(jQuery("#hdnPara3").val()); // Year
                para4 = jQuery.trim(jQuery("#hdnPara4").val()); // Source
                para5 = jQuery.trim(jQuery("#hdnPara5").val()); // UserID
                // ---------------------------------------------------------
                $("#spnModuleHeader").val('External Lead List');
                // ---------------------------------------------------------
                //webExternalLeadView(string status, string source, Int64 month, Int64 year, string userid)
                dataurl = '/Services/NagrikService.asmx/webExternalLeadView?status=' + para1 + '&userid=' + para5 + "&source=" + para4 + "&month=" + para2 + "&year=" + para3;
                colstructure = [
                        {
                            title: "Duration", data: "QueryDatetime", className: "text-center", width: "110",
                            render: function (data, type, row, meta) {
                                data = '<p class="mb-0" style="color:red;">' + getStatusUpdate('days', moment(data).format('YYYY-MM-DD'), row.pkID) + '</p><p class="badge badge-info mb-0" style="font-size:12px; text-transform:capitalize;">' + row.CreatedBy + '</p>';
                                return data;
                            }
                        },
                        {
                            title: "ID.", data: "pkID", className: "text-center", width: "90",
                            render: function (data, type, row, meta) {
                                data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                return data;
                            }
                        },
                        { title: "Date", data: "QueryDatetime", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                        { title: "State", data: "State", className: "text-center", width: "100" },
                        {
                            title: "Sender Name", data: "SenderName", className: "text-left", width: "150",
                            render: function (data, type, row, meta) {
                                data = '<p class="mb-0" style="color:navy;font-weight:bold;">' + row.SenderName + '</p><p class="badge badge-primary" style="font-size:12px;">' + row.PrimaryMobileNo + '</p>';
                                return data;
                            }

                        },
                        {
                            title: "Company Name<br />Email ID", data: "CompanyName", className: "text-left", width: "200",
                            render: function (data, type, row, meta) {
                                if (row.CompanyName != '')
                                    data = '<p class="mb-0" style="color:darkred;font-weight:bold;">' + row.CompanyName + '</p><p class="mb-0" style="color:blue;">' + row.SenderMail + '</p>';
                                else
                                    data = '<p class="mb-0" style="color:blue;">' + row.SenderMail + '</p>';
                                return data;
                            }
                        },
                        { title: "For Product", data: "ForProduct", className: "text-left", width: "200" },
                        {
                            title: "Status", data: "LeadSource", className: "text-center",
                            render: function (data, type, row, meta) {
                                if (row.LeadStatus == 'Qualified')
                                    data = '<p class="mb-0 pill green white-text">&nbsp;' + row.InquiryStatus + '&nbsp;</p>';
                                else if (row.LeadStatus == 'Disqualified')
                                    data = '<p class="mb-0 pill red white-text">&nbsp;' + row.ExLeadCloserReason + '&nbsp;</p>';
                                else
                                    data = '<p class="mb-0 pill blue white-text ">&nbsp;' + ' New ' + '&nbsp;</p>';
                                return data;

                            }
                        },
                        { title: "Lead #", data: "InquiryNo", className: "text-center", width: "130" },
                ];

                var InquiryShare = jQuery.trim(jQuery("#hdnInquiryShare").val());
                if (InquiryShare == "Yes") {
                    valColspan = 10;
                    colstructure.push({
                        title: "Share", className: "text-center", width: "60", mRender: function (data, type, row) {
                            return '<img src="images/multi-user-icon.jpg" alt="" style="width:20px; height:20px;" onclick="openInquirySharing(\'' + row.pkID + '\',\'' + jQuery.trim(jQuery("#hdnLoginUserID").val()) + '\');"/>';
                        }
                    });
                }
            }
            else if (viewmode == 'quotation') {
                groupColumn = 4;
                valColspan = 5;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                //jQuery("#lblCustomHead").text('Quotation List')
                dataurl = '/Services/NagrikService.asmx/webQuotationByUser?pLoginUserID=' + para1 + "&pMonth=" + para2 + "&pYear=" + para3;
                colstructure = [
                            {
                                title: "Quotation #", data: "QuotationNo", className: "text-center",
                                render: function (data, type, row, meta) {
                                    data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                    return data;
                                }
                            },
                            { title: "Quotation Date", data: "QuotationDate", type: "date", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                            {
                                title: "Inquiry #", data: "InquiryNo", width: "110", className: "text-center",
                                render: function (data, type, row, meta) {
                                    data = '<a href="javascript:gridActionInquiry(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                    return data;
                                }
                            },
                            { title: "Customer Name", data: "CustomerName", render: function (data, type, full) { return capitalizeString(data); } },
                            { title: "Sales Executive", data: "CreatedEmployeeName", className: "text-center", width: "150" },
                ];
            }
            else if (viewmode == 'quotbyuser') {
                groupColumn = 4;
                valColspan = 5;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                //jQuery("#lblCustomHead").text('Quotation List')
                dataurl = '/Services/NagrikService.asmx/webQuotationByUserPeriod?pLoginUserID=' + para1 + "&pFromDate=" + para2 + "&pToDate=" + para3;
                colstructure = [
                            {
                                title: "Quotation #", data: "QuotationNo", className: "text-center",
                                render: function (data, type, row, meta) {
                                    data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                    return data;
                                }
                            },
                            { title: "Quotation Date", data: "QuotationDate", type: "date", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                            {
                                title: "Inquiry #", data: "InquiryNo", width: "110", className: "text-center",
                                render: function (data, type, row, meta) {
                                    data = '<a href="javascript:gridActionInquiry(\'view\',\'' + row.InquirypkID + '\');">' + data + '</a>';
                                    return data;
                                }
                            },
                            { title: "Customer Name", data: "CustomerName", render: function (data, type, full) { return capitalizeString(data); } },
                            { title: "Sales Executive", data: "CreatedEmployeeName", className: "text-center", width: "150", render: function (data, type, full) { return capitalizeString(data); } },
                ];
            }
            else if (viewmode == 'followupbyuser') {
                groupColumn = 3;
                valColspan = 7;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                //jQuery("#lblCustomHead").text('FollowUp List')
                dataurl = '/Services/NagrikService.asmx/webFollowUpByUserPeriod?pLoginUserID=' + para1 + "&pFromDate=" + para2 + "&pToDate=" + para3;
                colstructure = [
                            { title: "Follow-Up Date", data: "FollowupDate", type: "date", className: "text-center", width: "100", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                            { title: "Next FollowUp", data: "NextFollowupDate", type: "date", className: "text-center", width: "100", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                            {
                                title: "Customer Name", data: "CustomerName", width: "200", className: "text-left",
                                render: function (data, type, row, meta) {
                                    data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + capitalizeString(data) + '</a>';
                                    return data;
                                }
                            },
                            { title: "Status", data: "InquiryStatus", width: "80", className: "text-center" },
                            { title: "Meeting Notes", data: "MeetingNotes", width: "300", className: "text-left" },
                            { title: "Rating", data: "Rating", width: "50", className: "text-center" },
                            {
                                title: "No-FollowUp", data: "NoFollowup", width: "80", className: "text-center",
                                render: function (data, type, full) {
                                    if (data == true)
                                        return ("No FollowUp");
                                    else
                                        return ("");
                                }
                            },
                            { title: "Sales Executive", data: "EmployeeName", className: "text-center", width: "150", render: function (data, type, full) { return capitalizeString(data); } }
                ];
            }
            else if (viewmode == 'salesorderbyuser') {
                groupColumn = 6;
                valColspan = 7;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('Sales Order List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webSalesOrderByUserPeriod?pLoginUserID=' + para1 + "&pFromDate=" + para2 + "&pToDate=" + para3;

                colstructure = [
                   { title: "Order Date", data: "OrderDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                   {
                       title: "Order #", data: "OrderNo", className: "text-center", width: "150",
                       render: function (data, type, row, meta) {
                           data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                           return data;
                       }
                   },
                   { title: "Quotation #", data: "QuotationNo", className: "text-center", width: "150" },
                   { title: "Customer Name", data: "CustomerName", width: "350", render: function (data, type, full) { return capitalizeString(data); } },
                   { title: "Status", data: "ApprovalStatus", className: "text-center", width: "150" },
                   { title: "Order Amount", data: "OrderAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Sales Executive", data: "EmployeeName", className: "text-center", width: "150", render: function (data, type, full) { return capitalizeString(data); } }
                ];

            }

                //Created by Vikram Rajput
            else if (viewmode == 'salesbillbyuser') {
                groupColumn = 6;
                valColspan = 7;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                jQuery("#lblCustomHead").text('Sales Bill List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webSalesBillByUserPeriod?pLoginUserID=' + para1 + "&FromDate=" + para2 + "&ToDate=" + para3;

                colstructure = [
                   { title: "Invoice Date", data: "InvoiceDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                   {
                       title: "Invoice #", data: "InvoiceNo", className: "text-center", width: "150",
                       render: function (data, type, row, meta) {
                           data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                           return data;
                       }
                   },
                   { title: "Customer Name", data: "CustomerName", width: "350", render: function (data, type, full) { return capitalizeString(data); } },
                   { title: "Bill Amount", data: "BillAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Tax Amount", data: "TaxAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Net Amount", data: "NetAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Sales Executive", data: "EmployeeName", className: "text-center", width: "150", render: function (data, type, full) { return capitalizeString(data); } }
                ];

            }

                //Created by Vikram Rajput
            else if (viewmode == 'purchasebillbyuser') {
                groupColumn = 6;
                valColspan = 6;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                jQuery("#lblCustomHead").text('Purchase Bill List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webPurchaseBillByUserPeriod?pLoginUserID=' + para1 + "&FromDate=" + para2 + "&ToDate=" + para3;

                colstructure = [
                   { title: "Invoice Date", data: "InvoiceDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                   {
                       title: "Invoice #", data: "InvoiceNo", className: "text-center", width: "150",
                       render: function (data, type, row, meta) {
                           data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                           return data;
                       }
                   },
                   { title: "Customer Name", data: "CustomerName", width: "275", render: function (data, type, full) { return capitalizeString(data); } },
                   { title: "Bill Amount", data: "BasicAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Tax Amount", data: "TaxAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Net Amount", data: "NetAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Sales Executive", data: "EmployeeName", className: "text-center", width: "150", render: function (data, type, full) { return capitalizeString(data); } }
                ];

            }

                //Created by Vikram Rajput
            else if (viewmode == 'inwardbyuser') {
                groupColumn = 6;
                valColspan = 6;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                jQuery("#lblCustomHead").text('Inward List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webInwardByUserPeriod?pLoginUserID=' + para1 + "&FromDate=" + para2 + "&ToDate=" + para3;

                colstructure = [
                   { title: "Inward Date", data: "InwardDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                   {
                       title: "Inward #", data: "InwardNo", className: "text-center", width: "150",
                       render: function (data, type, row, meta) {
                           data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                           return data;
                       }
                   },
                   { title: "Customer Name", data: "CustomerName", width: "350", render: function (data, type, full) { return capitalizeString(data); } },
                   { title: "Basic Amount", data: "BasicAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Tax Amount", data: "TaxAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Net Amount", data: "InwardAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Sales Executive", data: "EmployeeName", className: "text-center", width: "150", render: function (data, type, full) { return capitalizeString(data); } }
                ];

            }

                //Created by Vikram Rajput
            else if (viewmode == 'outwardbyuser') {
                groupColumn = 6;
                valColspan = 6;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                jQuery("#lblCustomHead").text('Outward List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webOutwardByUserPeriod?pLoginUserID=' + para1 + "&FromDate=" + para2 + "&ToDate=" + para3;

                colstructure = [
                   { title: "Outward Date", data: "OutwardDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                   {
                       title: "Outward #", data: "OutwardNo", className: "text-center", width: "150",
                       render: function (data, type, row, meta) {
                           data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                           return data;
                       }
                   },
                   { title: "Customer Name", data: "CustomerName", width: "350", render: function (data, type, full) { return capitalizeString(data); } },
                   { title: "Basic Amount", data: "BasicAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Tax Amount", data: "TaxAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Net Amount", data: "OutwardAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                   { title: "Sales Executive", data: "EmployeeName", className: "text-center", width: "150", render: function (data, type, full) { return capitalizeString(data); } }
                ];

            }

            else if (viewmode == 'contactbyuser') {
                groupColumn = 5;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('Contact List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webContactByUserPeriod?pLoginUserID=' + para1 + "&pFromDate=" + para2 + "&pToDate=" + para3;

                colstructure = [
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Category", data: "CustomerType", className: "text-center", width: "150" },
                    { title: "Contact #1", data: "ContactNo1" },
                    { title: "Contact #2", data: "ContactNo2" },
                    { title: "Email Address", data: "EmailAddress" },
                    { title: " Sales Executive", data: "CreatedEmployee", width: "300" }
                ];
            }
            else if (viewmode == 'todobyuser') {
                groupColumn = 8;
                valColspan = 9;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('To-DO Task List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webToDOListByUserPeriod?pLoginUserID=' + para1 + "&pFromDate=" + para2 + "&pToDate=" + para3;
                colstructure = [
                    {
                        title: "Task Description", data: "TaskDescription", className: "text-left", width: "350",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Start Date", data: "StartDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Due Date", data: "DueDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Completion Date", data: "CompletionDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Priority", data: "Priority", className: "text-center", width: "80" },
                    { title: "Overdue", data: "Duration", className: "text-center", width: "80" },
                    { title: "Task Status", data: "TaskStatus", className: "text-center", width: "80" },
                    { title: "Initiated By", data: "FromEmployeeName", className: "text-center", width: "120" },
                    { title: "Assigned To", data: "EmployeeName", className: "text-center", width: "120" }
                ];
            }
            else if (viewmode == 'leaverequestbyuser') {
                groupColumn = 0;
                valColspan = 6;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('Login-Logout List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webLeaveRequestListByUserPeriod?pLoginUserID=' + para1 + "&FromDate=" + para2 + "&ToDate=" + para3;

                colstructure = [
                    {
                        title: "Employee Name", data: "EmployeeName", className: "text-left", width: "200",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "From Date", data: "FromDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "To Date", data: "ToDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Reason For Leave", data: "ReasonForLeave", className: "text-center", width: "300" },
                    { title: "Approval Status", data: "ApprovalStatus", className: "text-center", width: "100" },
                    { title: "Approved By", data: "ApprovedBy", className: "text-center", width: "200" }
                ];
            }
            else if (viewmode == 'userloginlogout') {
                groupColumn = 0;
                valColspan = 4;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('Login-Logout List - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webLoginLogoutByUserPeriod?pLoginUserID=' + para1 + "&FromDate=" + para2 + "&ToDate=" + para3;

                colstructure = [
                    { title: "UserId #", data: "UserID", className: "text-center", width: "150" },
                    { title: "Login DateTime #", data: "LoginDateTime", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY hh:mm:ss') : ''; } },
                    { title: "Logout DateTime #", data: "LogoutDateTime", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY hh:mm:ss') : ''; } },
                    { title: "MacId #", data: "MacID", className: "text-center", width: "150" }
                ];
            }
            else if (viewmode == 'latepunchbyuser') {
                groupColumn = 0;
                valColspan = 4;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('Late Punch - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webLatePunchByUserPeriod?pLoginUserID=' + para1 + "&FromDate=" + para2 + "&ToDate=" + para3;

                colstructure = [
                    { title: "Employee Name", data: "EmployeeName", className: "text-left", width: "200" },
                    { title: "Presence Date", data: "PresenceDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Time-IN", data: "TimeIn", className: "text-center", width: "100" },
                    { title: "Time-OUT", data: "TimeOut", className: "text-center", width: "100" }
                ];
            }
            else if (viewmode == 'dailyactivitybyuser') {
                groupColumn = 2;
                valColspan = 4;
                groupColumnTitle = "Activity Date : ";
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('Daily Activity - [ Employee Name : ' + para4 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webDailyActivityByUserPeriod?pLoginUserID=' + para1 + "&pActivityDate=" + "&pFromDate=" + para2 + "&pToDate=" + para3;

                colstructure = [
                    { title: "Task Description", data: "TaskDescription", className: "text-left", width: "300" },
                    { title: "Task Category", data: "TaskCategoryName", className: "text-center", width: "100" },
                    { title: "Activity Date", data: "ActivityDate", type: "date", className: "text-center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Duration", data: "TaskDuration", className: "text-center", width: "100" }
                ];
            }
            else if (viewmode == 'complaintvisit') {
                groupColumn = 0;
                valColspan = 4;
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                para2 = jQuery.trim(jQuery("#hdnPara2").val());
                para3 = jQuery.trim(jQuery("#hdnPara3").val());
                para4 = jQuery.trim(jQuery("#hdnPara4").val());
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('Complaint Visit List')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webComplaintVisitList?pkID=' + para1 + "&ComplaintNo=" + para2 + "&CustID=" + para3;

                colstructure = [
                    { title: "Customer Name", data: "CustomerName", className: "text-center", width: "250" },
                    { title: "Complaint #", data: "ComplaintNo", className: "text-center", width: "90" },
                    { title: "Complaint Date", data: "ComplaintDate", className: "text-center", width: "100", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Visit Date", data: "VisitDate", className: "text-center", width: "100", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Visit Type", data: "VisitType", className: "text-center", width: "100" },
                    { title: "Charge Type", data: "VisitChargeType", className: "text-center", width: "100" },
                    { title: "Visit Charge", data: "VisitCharge", className: "text-right", width: "100", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Attended By", data: "EmployeeName", className: "text-center", width: "150" },
                ];
            }
            // -------------------------------------------------------------------------
            // Configure Edit/Delete Button Display 
            // -------------------------------------------------------------------------
            if (jQuery("#hdnActionFlag").val() == 'true') {
                colstructure.push({
                    title: "Edit", className: "text-center", mRender: function (data, type, row) {
                        return '<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(' + row.pkID + ');"/>';
                    }
                });

                colstructure.push({
                    title: "Delete", className: "text-center", mRender: function (data, type, row) {
                        return '<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(' + row.pkID + ');"/>';
                    }
                });
            }
            // -------------------------------------------------------------------------
            // Design and Load Table as per above configuration 
            // -------------------------------------------------------------------------
            if (viewmode == 'inquiry' || viewmode == 'inquirybyuser' ||
                viewmode == 'quotbyuser' || viewmode == 'followupbyuser' || viewmode == 'salesorderbyuser' ||
                viewmode == 'contactbyuser' || viewmode == 'salesbillbyuser' || viewmode == 'purchasebillbyuser' || viewmode == 'inwardbyuser' || viewmode == 'outwardbyuser' ||
                viewmode == 'userloginlogout' || viewmode == 'dailyactivitybyuser') {
                //alert(dataurl)
                jQuery.ajax({
                    url: dataurl,
                    type: "get",
                    success: function (data) {
                        //alert('before load table')
                        var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                        // ----------------------------------------------
                        jQuery('#tblView').DataTable({
                            "columnDefs": [{ "visible": false, "targets": groupColumn }],
                            "order": [[groupColumn, 'asc']],
                            data: resultdata,
                            columns: colstructure,
                            "displayLength": 25,
                            "drawCallback": function (settings) {
                                var api = this.api();
                                var rows = api.rows({ page: 'current' }).nodes();
                                var last = null;

                                api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                                    if (last !== group) {
                                        if (viewmode == 'dailyactivitybyuser') {
                                            $(rows).eq(i).before('<tr class="group"><td colspan="' + valColspan + '"  style="background-color:#483d8b !important; color:white !important; font-size:18px !important; font-weight:bold !important; text-transform:capitalize;"><b>' + groupColumnTitle + '</b>' + moment(group).format('DD-MMM-YYYY') + '</td></tr>');
                                        }
                                        else
                                            $(rows).eq(i).before('<tr class="group"><td colspan="' + valColspan + '"  style="background-color:#483d8b !important; color:white !important; font-size:18px !important; font-weight:bold !important; text-transform:capitalize;"><b>' + groupColumnTitle + ' </b>' + group + '</td></tr>');

                                        last = group;
                                    }
                                });
                            }
                        })

                    },
                    error: function (r) {
                        alert('Error : ' + r.responseText);
                    },
                    failure: function (r) {
                        alert('failure');
                    }
                });
            }
            else {
                jQuery.ajax({
                    url: dataurl, type: "get",
                    success: function (data) {
                        var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                        // ----------------------------------------------
                        jQuery('#tblView').DataTable({ data: resultdata, columns: colstructure }).on('draw.dt', function () { window.setTimeout(hide_loader, 500); });
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }

            $('#tblView').on('draw.dt', function () { window.setTimeout(hide_loader, 500); });
        });

        // -------------------------------------------------------------------------
        // Function : This will invoke by Edit/Delete button
        // -------------------------------------------------------------------------
        function gridActionInquiry(mode, id) {
            var pageUrl = "InquiryInfo.aspx?mode=view&id=" + id;
            $('#ifrModule22').attr('src', pageUrl);
            $find("mpe22").show();
        }

        // -------------------------------------------------------------
        // Section : Call page for Share Inquiry 
        // -------------------------------------------------------------
        function openInquirySharing(id, userid) {
            var pageUrl = "InquiryShare.aspx?id=" + id + "&userid=" + userid;
            $('#ifrModule22').attr('src', pageUrl);
            $find("mpe22").show();
        }

        function gridAction(mode, id) {

            var varView = jQuery("#hdnView").val();

            if (varView == 'inquirybyuser' || varView == 'inquiry' || varView == 'inquiryclinic' || varView == 'inquiryclinicbyuser') {
                spnModuleHeader.innerText = "Manage Lead";
                var pageUrl = "InquiryInfo.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'quotbyuser' || varView == 'quotation') {
                spnModuleHeader.innerText = "Manage Quotation";
                var pageUrl = "Quotation.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'followupbyuser') {
                spnModuleHeader.innerText = "Manage FollowUp";
                var pageUrl = "FollowUp.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'salesorderbyuser') {
                spnModuleHeader.innerText = "Manage Sales Order";
                var pageUrl = "SalesOrder.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'salesbillbyuser') {
                spnModuleHeader.innerText = "Manage Sales Bill";
                var pageUrl = "SalesBill.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
                //Created by Vikram Rajput
            else if (varView == 'purchasebillbyuser') {
                spnModuleHeader.innerText = "Manage Purchase Bill";
                var pageUrl = "Purchasebill.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'inwardbyuser') {
                spnModuleHeader.innerText = "Manage Inward"
                var pageUrl = "Inward.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'outwardbyuser') {
                spnModuleHeader.innerText = "Manage Outward"
                var pageUrl = "Outward.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'contactbyuser') {
                spnModuleHeader.innerText = "Manage Customer"
                var pageUrl = "Customers.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'todobyuser') {
                spnModuleHeader.innerText = "Manage Task"
                var pageUrl = "ToDo.aspx?id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
            else if (varView == 'leaverequestbyuser') {
                spnModuleHeader.innerText = "Manage Leave Request"
                var pageUrl = "LeaveRequest.aspx?mode=view&id=" + id;
                $('#ifrModule22').attr('src', pageUrl);
                $find("mpe22").show();
            }
        }

        // -------------------------------------------------------------------------
        // Calculate Dates 
        // -------------------------------------------------------------------------
        function monthDiff(d1, d2) {
            var months;
            months = (d2.getFullYear() - d1.getFullYear()) * 12;
            months -= d1.getMonth() + 1;
            months += d2.getMonth();
            return months <= 0 ? 0 : months;
        }

        function getStatusUpdate(interval, date1, id) {
            var second = 1000, minute = second * 60, hour = minute * 60, day = hour * 24, week = day * 7;
            date1 = new Date(moment(date1).format('DD-MMM-YYYY'));
            date2 = new Date(moment(new Date()).format('DD-MMM-YYYY hh:mm:ss'));

            //console.log('---------------------------------------');
            var timediff = date2 - date1;
            if (isNaN(timediff)) return NaN;
            var tmpVal = 0;
            var returnVal = "";
            // -------------------------------------------------------------
            //var y = date2.diff(date1, 'years');
            //var m = (date2.getFullYear() * 12 + date2.getMonth()) -(date1.getFullYear() * 12 + date1.getMonth());
            var m = monthDiff(date1, date2);
            var w = Math.floor(timediff / week);
            var d = Math.floor(timediff / day);;
            var h = Math.floor(timediff / hour);
            var mn = Math.floor(timediff / minute);
            // -------------------------------------------------------------           
            if (returnVal == "" && m > 0)
                returnVal = m + " Months";
            else {
                if (returnVal == "" && m <= 0 && d > 0)
                    returnVal = d + " Days";
                else {
                    if (returnVal == "" && m <= 0 && d <= 0 && h > 0)
                        returnVal = h + " Hours";
                    else {
                        if (returnVal == "" && m <= 0 && d <= 0 && h <= 0 && mn > 0)
                            returnVal = mn + " Mins";
                    }
                }
            }
            return returnVal;
        }

    </script>
</head>
<body>
    <form id="frmCustomView" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" CompositeScript-ScriptMode="Inherit"></asp:ScriptManager>
        <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnInquiryShare" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnActionFlag" runat="server" ClientIDMode="Static" Value="false" />
        <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPara1" runat="server" ClientIDMode="Static" />      <%-- UserID   --%>
        <asp:HiddenField ID="hdnPara2" runat="server" ClientIDMode="Static" />      <%-- FromDate --%>
        <asp:HiddenField ID="hdnPara3" runat="server" ClientIDMode="Static" />      <%-- ToDate   --%>
        <asp:HiddenField ID="hdnPara4" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPara5" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />

        <div id="myLoader" class="hide-loader"></div>
        <asp:Button ID="Button1" runat="server" Text="Fill Form in Popup" Style="display:none;" /> 
        
        <div class="card bg-info-gradient" style="display: none;">
            <div class="card-header no-border" style="padding: 5px 10px; background-color: beige;">
                <h5><i class="fa fa-th mr-1"></i>
                    <asp:Label ID="lblCustomHead" runat="server" ClientIDMode="Static"></asp:Label></h5>
            </div>
        </div>
        <div style="margin: 10px 20px 10px 20px;">
            <table id="tblView" class="stdtable" style="width: 100%;"></table>
        </div>
        <br /><br />
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe22" runat="server" ClientIDMode="Static" PopupControlID="Panl22" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
        </cc1:ModalPopupExtender>  
        <asp:Panel ID="Panl22" runat="server" ClientIDMode="Static" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
            <div id="myModal">
                <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom:10px !important;">
                    <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                        <i class="material-icons prefix">ac_unit</i>
                        <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module.</span>
                        <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" ClientIDMode="Static" Text="" />
                    </h5>
                </div>
            </div>
            <iframe id="ifrModule22" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border:1px solid silver; background-color:transparent; width:100%; height:85%; padding:5px;"></iframe>  
            <br/>  
        </asp:Panel> 
    </form>
</body>
</html>
