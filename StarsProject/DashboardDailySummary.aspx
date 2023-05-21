<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardDailySummary.aspx.cs" Inherits="StarsProject.DashboardDailySummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/animate-css/animate.css" />

    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/quill/quill.snow.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-sidebar.css" />

    <link rel="stylesheet" type="text/css" href="app-assets/vendors/data-tables/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/data-tables/extensions/responsive/css/responsive.dataTables.min.css" />
    
    <script type="text/javascript" src="js/jquery-3.3.1.js"></script>
    <script src="plugins/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="app-assets/vendors/sortable/jquery-sortable-min.js"></script>
    <script type="text/javascript" src="app-assets/vendors/quill/quill.min.js"></script>
    <script type="text/javascript" src="app-assets/js/plugins.js"></script>

    <script src="app-assets/vendors/data-tables/js/jquery.dataTables.min.js"></script>
    <script src="app-assets/vendors/data-tables/extensions/responsive/js/dataTables.responsive.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>


    <style type="text/css">
        .contact-list li span { margin-right:20px; } 
        .summLeftTitle {
            margin-left: -1.8rem;
            padding-left: 1.8rem;
            border-top-right-radius: 2rem;
            border-bottom-right-radius: 2rem;
            background-color: #3f51b5;
            box-shadow: 0 0 8px 0 #3f51b5;
            color: white;
            line-height:35px;
        }

        .selActive {
            margin-left: -1.8rem;
            padding-left: 1.8rem;
            border-bottom:2px ridge silver;
            height:60px; 
            padding-top:3px;
        }
 
        .dataTables_filter input[type=search]:not(.browser-default) {
            width: 180px !important;
            height: 1.5rem;
            margin: 0 0 15px 5px;
            border: 1px solid silver;
            border-radius: 10px;
            padding: 0px 3px 0px 10px;
        }

        .dataTables_filter label {
            color: red;
            font-weight: bold;
            text-align: left !important;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button.current, .dataTables_wrapper .dataTables_paginate .paginate_button.current:hover {
            color: #fff !important;
            border: 1px solid #3f51b5;
            border-radius: 6px;
            background: #3f51b5;
            box-shadow: 0 0 8px 0 #3f51b5;
        }

        .dataTables_wrapper .dataTables_paginate .paginate_button, #main .section-data-tables .dataTables_wrapper #page-length-option_paginate .paginate_button {
            margin-top: .25rem;
            padding: .25em .65em;
        }

        .select-wrapper input.select-dropdown {
            height: 1.5rem !important;
            margin: 0 0px 0px 5px !important;
            border: 1px solid silver;
            border-radius: 8px;
            padding: 0px 0px 0px 5px;
            width: 80px;
        }

        #tblView_length label {
            display: inline-flex !important;
            text-transform: capitalize;
        }

        #tblView_length .select-wrapper {
            margin-right: 7px !important;
        }

        #contact-sidenav li a { color:black;}
        #contact-sidenav li small { color:blue;}
    </style>

    <script type="text/javascript">
        function capitalizeString(str) {
            return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }

        function GetDailySummary(viewmode, title) {
            $('#hdnViewMode').val(viewmode);

            document.getElementById('spnHeader').innerText = title;
            // -----------------------------------------------------------
            var dataurl, para1, para2, para3, para4, para5, para0;
            var groupColumn, valColspan;
            var groupColumnTitle = "";
            // -----------------------------------------------------------
            dataurl = '/Services/NagrikService.asmx/webDashboardDailySummary?xType=' + viewmode + '&xMonth=0&xYear=0&xLoginUserID=admin';

            colstructure = [
                {
                    title: "Document #", data: "DocNo", width: "110", className: "text-center",
                    render: function (data, type, row, meta) {
                        data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                        return data;
                    }
                },
                { title: "Document Date", data: "DocDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MM-YYYY') : ''; } },
                { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
            ];
            // -----------------------------------------------------------------------------
            jQuery.ajax({
                url: dataurl, type: "get",
                success: function (data) {
                    var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                    // ----------------------------------------------
                    var table;
                    if ($.fn.dataTable.isDataTable('#tblView')) {
                        table = $('#tblView').DataTable();
                        table.clear();
                        table.rows.add(resultdata).draw();
                        document.getElementById('spnHeaderCount').innerText = resultdata.length;
                    }
                    else {
                        table = jQuery('#tblView').DataTable({ data: resultdata, columns: colstructure }).on('draw.dt', function () { });
                        document.getElementById('spnHeaderCount').innerText = resultdata.length;
                    }

                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });

        }
    </script>
</head>
<body>
    <form id="frmDashboardDailySummary" runat="server">
        <asp:HiddenField ID="hdnViewMode" runat="server" ClientIDMode="Static" />
        <div class="container">
            <!-- Add new contact popup -->
            <div class="contact-overlay"></div>
<%--            <div style="bottom: 54px; right: 19px;" class="fixed-action-btn direction-top">
                <a class="btn-floating btn-large primary-text gradient-shadow contact-sidebar-trigger" href="#modal1">
                    <i class="material-icons">person_add</i>
                </a>
            </div>--%>
            <!-- Add new contact popup Ends-->

            <!-- Sidebar Area Starts -->
            <div class="row">
                <div class="col m3" style="width:auto !important; height:600px; overflow-x:hidden; overflow-y:scroll !important;">
<%--                    <div class="sidebar-left">
                        <div class="sidebar">
                            <div class="sidebar-content">
                                <div id="sidebar-list" class="sidebar-menu list-group position-relative animate fadeLeft delay-1 ps ps--active-y">
                                    <div class="sidebar-list-padding app-sidebar" id="contact-sidenav">--%>
                                        <ul class="contact-list display-grid">
                                            <li class="summLeftTitle">Today's Due</li>
                                            <%--*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*---%>
                                            <li class="selActive">
                                                <i class="purple-text material-icons">star_border</i>
                                                <a href="javascript:GetDailySummary('DueSalesOrder', 'Todays Due - Material Dispatch');" class="text-sub">Dispatch</a>
                                                <span id="spnDueSalesOrder" runat="server" class="badge badge pill purple">4</span>
                                                &nbsp;&nbsp;<small class="text-muted" style="display: inline-block;width: 100%;margin-left: 35px;">(By Deli.Date)</small>
                                            </li>
                                            <li class="selActive">
                                                <i class="amber-text material-icons">star_border</i>
                                                <a href="javascript:GetDailySummary('DuePurchaseOurder', 'Todays Due - Material Purchase');" class="text-sub">Purchase</a>
                                                <span id="spnDuePurchaseOrder" runat="server" class="badge badge pill amber">4</span>
                                                &nbsp;&nbsp;<small class="text-muted" style="display: inline-block;width: 100%;margin-left: 35px;">(By Deli.Date)</small>
                                            </li>
                                            <li class="selActive">
                                                <i class="light-green-text material-icons">star_border</i>
                                                <a href="javascript:GetDailySummary('DuePurchasePaySch', 'Todays Due - Purchase Order');" class="text-sub">Purchase Order</a>
                                                <span id="spnPurchasePaySch" runat="server" class="badge badge pill light-green">4</span>
                                                &nbsp;&nbsp;<small class="text-muted" style="display: inline-block;width: 100%;margin-left: 35px;">(By Pay.Schedule)</small>
                                            </li>
                                            <li class="selActive">
                                                <i class="light-green-text material-icons">star_border</i>
                                                <a href="javascript:GetDailySummary('DuePayable', 'Todays Due - Purchase Bills');" class="text-sub">Purchase Bill</a>
                                                <span id="spnDuePayable" runat="server" class="badge badge pill light-green">4</span>
                                                &nbsp;&nbsp;<small class="text-muted" style="display: inline-block;width: 100%;margin-left: 35px;">(By Cr.Days)</small>
                                            </li>
                                            <li class="selActive">
                                                <i class="blue-text material-icons">star_border</i>
                                                <a href="javascript:GetDailySummary('DueSalesPaySch', 'Todays Due - Sales Order');" class="text-sub">Sales Order</a>
                                                <span id="spnSalesPaySch" runat="server" class="badge badge pill blue">4</span>
                                                &nbsp;&nbsp;<small class="text-muted" style="display: inline-block;width: 100%;margin-left: 35px;">(By Pay.Schedule)</small>
                                            </li>
                                            <li class="selActive">
                                                <i class="blue-text material-icons">star_border</i>
                                                <a href="javascript:GetDailySummary('DueReceivable', 'Todays Due - Sales Bills');" class="text-sub">Sales Bill</a>
                                                <span id="spnDueReceivable" runat="server" class="badge badge pill blue">4</span>
                                                &nbsp;&nbsp;<small class="text-muted" style="display: inline-block;width: 100%;margin-left: 35px;">(By Cr.Days)</small>
                                            </li>
                                            <%--*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*---%>
                                            <li class="summLeftTitle">Pending Approval</li>
                                            <%--*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*---%>
                                            <li class="hide">
                                                <i class="purple-text material-icons">history</i>
                                                <a href="javascript:GetDailySummary('AppPurchase', 'Pending Approval - Purchase');" class="text-sub">Purchase Quotation</a>
                                                <span id="spnAppPurchase" runat="server" class="badge badge pill purple">4</span>
                                            </li>
                                            <li class="selActive">
                                                <i class="amber-text material-icons">history</i>
                                                <a href="javascript:GetDailySummary('AppSalesOrder', 'Pending Approval - Sales Order');" class="text-sub">Sales Order</a>
                                                <span id="spnAppSalesOrder" runat="server" class="badge badge pill amber">4</span>
                                            </li>
                                            <li class="selActive">
                                                <i class="light-green-text material-icons">history</i>
                                                <a href="javascript:GetDailySummary('AppPurchaseOrder', 'Pending Approval - Purchase Order');" class="text-sub">Purchase Order</a>
                                                <span id="spnAppPurchaseOrder" runat="server" class="badge badge pill light-green">4</span>
                                            </li>
                                            <%--*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*---%>
                                            <li class="summLeftTitle">Pending Future Process</li>
                                            <%--*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*---%>
                                            <li class="selActive">
                                                <i class="purple-text material-icons small-icons">label_outline</i>
                                                <a href="javascript:GetDailySummary('DocInquiry', 'Pending Future Process - Inquiry/Leads');" class="text-sub">Lead</a>
                                                <span id="spnDocInquiry" runat="server" class="badge badge pill purple">1234</span>
                                            </li>
                                            <li class="selActive">
                                                <i class="amber-text material-icons small-icons">label_outline</i>
                                                <a href="javascript:GetDailySummary('DocQuotation', 'Pending Future Process - Quotation');" class="text-sub">Quotation</a>
                                                <span id="spnDocQuotation" runat="server" class="badge badge pill amber">1234</span>
                                            </li>
                                            <li class="selActive">
                                                <i class="light-green-text material-icons small-icons">label_outline</i>
                                                <a href="javascript:GetDailySummary('DocSalesOrder', 'Pending Future Process - Sales Order');" class="text-sub">Sales Order</a>
                                                <span id="spnDocSalesOrder" runat="server" class="badge badge pill light-green">4</span>
                                            </li>
                                        </ul>
 <%--                                   </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                    <!-- Sidebar Area Ends -->
                </div>
                <div class="col m9">
                    <!-- Content Area Starts -->
                    <div class="content-area content-right">
                        <div class="row mt-1">
                            <div class="col m12 summLeftTitle">
                                <div class="row m-0 p-0">
                                    <div class="col m9  m-0 p-0">
                                        <h5 class="white-text mt-1" id="spnHeader">Today's Due</h5>
                                    </div>
                                    <div class="col m3  m-0 p-0">
                                        <span class="float-right badge pill blue mt-3" id="spnHeaderCount" runat="server" style="font-size:16px;">&nbsp;</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-2">
                            <div class="col m12">
                                <table id="tblView" class="stdtable" style="width: 100%;"></table>
                            </div>
                        </div>
                    </div>
                    <!-- Content Area Ends -->
                </div>
            </div>
            <br /><br /><br />
        </div>
    </form>
</body>
</html>
