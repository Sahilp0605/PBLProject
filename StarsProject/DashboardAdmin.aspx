<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="DashboardAdmin.aspx.cs" Inherits="StarsProject.DashboardAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/datatables/jquery.dataTables.min.js"></script>
    <link type="text/css" href="css/adminlte.min.css" rel="stylesheet" />
<%--    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />--%>

    <link type="text/css" rel="stylesheet" href="plugins/morris/morris.css" />
    <script type="text/javascript" src="js/raphael-min.js"></script>
    <script type="text/javascript" src="plugins/morris/morris.min.js"></script>
    
    <style type="text/css">
        .rightfixed {
            position: fixed;
            top: 28%;
            right: 0;
            height: 335px;
            /*z-index: 9999;*/
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

        .info-box-content {
            padding: 5px 5px !important;
        }

        .info-box-icon {
            width: 50px !important;
            height: 60px !important;
            margin-right: 5px !important;
        }
    </style>

    <script type="text/javascript">
        function initGraphLayout(cat) {
            //alert('Initiate Graph')
            var grLegend = '';
            var date = new Date();
            //var d1 = new Date('2019-01-01');
            //var d2 = new Date('2019-12-31');
            var d1 = '2019-01-01';
            var d2 = '2019-12-31';
            // ----------------------------------------------------------
            var srcUrl = "";
            srcUrl = 'Services/NagrikService.asmx/GetDashboard2Summary';
            var paraData = '{pCategory: \'' + cat + '\', date1: \'' + d1 + '\', date2: \'' + d2 + '\'}';
            grLegend = 'Amount';
            //jQuery("#dash_External").show("slide");
            // -----------------------------------------------------------
            if (srcUrl != '' && srcUrl != null) {
                jQuery("#" + cat).empty();
                // -------------------------------------------------------
                jQuery.ajax({
                    type: "POST", url: srcUrl, dataType: 'json',
                    data: paraData,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        //alert(resultdata.length)
                        // ---------------------------------------
                        if (resultdata.length == 0 || data.d == undefined) {
                            jQuery("#" + cat).parent().attr("class", "hide");
                            jQuery("#label").hide();
                            var resultdata = [{ label: "No Data Availble", value: 0 }];
                        }
                        else {
                            jQuery("#" + cat).parent().attr("class", "show");
                        }
                        // ---------------------------------------
                        // Sales / Purchase 
                        // ---------------------------------------
                        var spChart;

                        spChart = new Morris.Bar({
                            element: cat, resize: true, data: resultdata, hideHover: 'auto',
                            xkey: 'label', ykeys: ['value1', 'value2'], labels: ['Sales', 'Purchase'],
                            lineColors: ['#28a745', '#c82333', '#007bff', '#17a2b8', '#6c757d', '#ffc107']
                        });
                        spChart.draw();
                        // ---------------------------------------
                        // Calculate Total of SALES & PURCHASE
                        // ---------------------------------------
                        var totalSale = 0, totalPurc = 0;
                        var totalReceipt = 0, totalPayment = 0;
                        if (cat.toLowerCase() == 'salespurchasechart')
                        {
                            $.each(resultdata, function (index) {
                                totalSale += parseFloat(resultdata[index].value1);
                                totalPurc += parseFloat(resultdata[index].value2);
                            });
                            $("#spnTotalPurc").text(totalPurc);
                            $("#spnTotalSale").text(totalSale);
                        }
                        if (cat.toLowerCase() == 'collectionchart') {
                            $.each(resultdata, function (index) {
                                totalReceipt += parseFloat(resultdata[index].value1);
                                totalPayment += parseFloat(resultdata[index].value2);
                            });
                            $("#spnTotalPayIn").text(totalReceipt);
                            $("#spnTotalPayOut").text(totalPayment);
                        }

                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
        }

        initGraphLayout('salespurchaseChart');
        initGraphLayout('collectionChart');
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pageheader texturePaper2" style="padding-bottom: 0px !important; font-family: 'Montserrat',sans-serif;">
        <h1 class="pagetitle contenttitle2" style="display: block !important;">&nbsp;&nbsp;
            <asp:DropDownList ID="drpDashboard" CssClass="form-control dropdown float-left" runat="server" ClientIDMode="Static" Style="width: 250px; background-color: navy; color: white; margin-left: 10px;" AutoPostBack="true" OnSelectedIndexChanged="drpDashboard_SelectedIndexChanged">
                <asp:ListItem CssClass="dropdown-item" Text="Dashboard" Value="Dashboard" />
                <asp:ListItem CssClass="dropdown-item" Text="Dashboard-Lead" Value="Dashboard-Lead" />
                <asp:ListItem CssClass="dropdown-item" Text="Dashboard-Inventory" Value="Dashboard-Inventory" />
            </asp:DropDownList>

            <asp:DropDownList ID="drpSummaryMonth" CssClass="form-control" runat="server" ClientIDMode="Static" Style="border-radius: 6px; border: 1px solid navy; padding: 0px; width: 150px; float: right; font-size: 18px;" AutoPostBack="true" OnSelectedIndexChanged="drpSummaryMonthYear_SelectedIndexChanged"></asp:DropDownList>
            <asp:DropDownList ID="drpSummaryYear" CssClass="form-control" runat="server" ClientIDMode="Static" Style="border-radius: 6px; border: 1px solid navy; padding: 0px; width: 150px; float: right; font-size: 18px; margin-right: 10px;" AutoPostBack="true" OnSelectedIndexChanged="drpSummaryMonthYear_SelectedIndexChanged"></asp:DropDownList>
        </h1>
    </div>
    <div id="contentwrapper" class="contentwrapper" style="width: 99%; padding: 5px 10px !important;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12 col-sm-6 col m2">
                    <div class="info-box shadow">
                        <span class="info-box-icon bg-danger elevation-1"><i class="fa fa-shopping-cart"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Purchase</span>
                            <span id="spnPurchaseCount" class="info-box-number">10</span>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->
                <div class="col-12 col-sm-6 col m2">
                    <div class="info-box shadow">
                        <span class="info-box-icon bg-danger elevation-1"><i class="fa fa-plus-circle"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Inward</span>
                            <span id="spnInwardCount" class="info-box-number">41,410</span>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->

                <!-- fix for small devices only -->
                <div class="clearfix hidden-md-up"></div>

                <div class="col-12 col-sm-6 col m2">
                    <div class="info-box shadow">
                        <span class="info-box-icon bg-success elevation-1"><i class="fa fa-shopping-cart"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Sales</span>
                            <span id="spnSalesCount" class="info-box-number">760</span>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->

                <div class="col-12 col-sm-6 col m2">
                    <div class="info-box shadow">
                        <span class="info-box-icon bg-success elevation-1"><i class="fa fa-minus-circle"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Outward</span>
                            <span id="spnOutwardCount" class="info-box-number">2000</span>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->

                <!-- fix for small devices only -->
                <div class="clearfix hidden-md-up"></div>

                <div class="col-12 col-sm-6 col m2">
                    <div class="info-box shadow">
                        <span class="info-box-icon bg-primary elevation-1"><i class="fa fa-minus-circle"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Payable</span>
                            <span id="spnPayableCount" class="info-box-number">2000</span>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->

                <div class="col-12 col-sm-6 col m2">
                    <div class="info-box shadow">
                        <span class="info-box-icon bg-primary elevation-1"><i class="fa fa-plus-circle"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Receivable</span>
                            <span id="spnReceivableCount" class="info-box-number">760</span>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->

            </div>
            <!-- /.row -->

            <%-- *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-* --%>
            <div class="row">
                <div class="col m12">
                    <div class="card">
                        <div class="card-header bg-primary-gradient">
                            <h5 class="card-title">Purchase/Sales Analysis</h5>
                            <div class="card-tools">
                                <button type="button" class="btn btn-tool" data-toggle="collapse" data-target="#collapseExample">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-widget="remove">
                                    <i class="fa fa-times"></i>
                                </button>
                            </div>
                        </div>
                        <!-- /.card-header -->
                        <div class="card-body collapse show" id="collapseExample">
                            <div class="row">
                                <div class="col m8">
                                    <p class="text-center"><strong>Sales: 1 Jan, 2014 - 30 Jul, 2014</strong></p>
                                    <div class="chart">
                                        <!-- Sales Chart Canvas -->
                                        <div id="salespurchaseChart" style="height:180px;"></div>
                                    </div>
                                    <!-- /.chart-responsive -->
                                </div>
                                <!-- /.col -->
                                <div class="col m4">
                                    <p class="text-center"><strong>Sales Analysis</strong></p>

                                    <div class="progress-group">Sales Target
                                        <span class="float-right">&nbsp;/&nbsp;100%</span>
                                        <span class="float-right font-weight-bold">90%</span>
                                        <div class="progress progress-sm">
                                            <div class="progress-bar bg-primary" style="width: 80%"></div>
                                        </div>
                                    </div>
                                    <div class="progress-group">Sales Order Approval
                                        <span class="float-right">&nbsp;/&nbsp;100%</span>
                                        <span class="float-right font-weight-bold">90%</span>
                                        <div class="progress progress-sm">
                                            <div class="progress-bar bg-danger" style="width: 75%"></div>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.col -->
                            </div>
                            <!-- /.row -->
                        </div>
                        <!-- ./card-body -->
                        <div class="card-footer">
                            <div class="row">
                                <div class="col-sm-3 col-3">
                                    <div class="description-block border-right">
                                        <%--<span class="description-percentage text-success"><i class="fa fa-caret-up"></i>17%</span>--%>
                                        <h5 class="description-header"><span id="spnTotalPurc">1111</span></h5>
                                        <span class="description-text">TOTAL PURCHASE</span>
                                    </div>
                                    <!-- /.description-block -->
                                </div>
                                <!-- /.col -->
                                <div class="col-sm-3 col-3">
                                    <div class="description-block border-right">
                                        <%--<span class="description-percentage text-warning"><i class="fa fa-caret-left"></i>0%</span>--%>
                                        <h5 class="description-header"><span id="spnTotalSale"></span></h5>
                                        <span class="description-text">TOTAL SALE</span>
                                    </div>
                                    <!-- /.description-block -->
                                </div>
                                <!-- /.col -->
                                <div class="col-sm-3 col-3">
                                    <div class="description-block border-right">
                                        <%--<span class="description-percentage text-success"><i class="fa fa-caret-up"></i>20%</span>--%>
                                        <h5 class="description-header"><span id="spnTotalProfit">0</span></h5>
                                        <span class="description-text">TOTAL PROFIT</span>
                                    </div>
                                    <!-- /.description-block -->
                                </div>
                                <!-- /.col -->
                                <div class="col-sm-3 col-3 hide">
                                    <div class="description-block">
                                        <%--<span class="description-percentage text-danger"><i class="fa fa-caret-down"></i>18%</span>--%>
                                        <h5 class="description-header">1200</h5>
                                        <span class="description-text">GOAL COMPLETIONS</span>
                                    </div>
                                    <!-- /.description-block -->
                                </div>
                            </div>
                            <!-- /.row -->
                        </div>
                        <!-- /.card-footer -->
                    </div>
                    <!-- /.card -->
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->

            <%-- *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-* --%>
            <div class="row">
                <div class="col m12">
                    <div class="card">
                        <div class="card-header bg-danger-gradient">
                            <h5 class="card-title">Receivable/Payable Analysis</h5>
                            <div class="card-tools">
                                <button type="button" class="btn btn-tool" data-toggle="collapse" data-target="#collapseCollection">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-widget="remove">
                                    <i class="fa fa-times"></i>
                                </button>
                            </div>
                        </div>
                        <!-- /.card-header -->
                        <div class="card-body collapse show" id="collapseCollection">
                            <div class="row">
                                <div class="col m8">
                                    <p class="text-center">
                                        <strong>Sales: 1 Jan, 2014 - 30 Jul, 2014</strong>
                                    </p>

                                    <div class="chart">
                                        <!-- Sales Chart Canvas -->
                                        <div id="collectionChart" style="height:180px;"></div>
                                    </div>
                                    <!-- /.chart-responsive -->
                                </div>
                                <!-- /.col -->
                                <div class="col m4">
                                    <p class="text-center">
                                        <strong>Goal Completion</strong>
                                    </p>

                                    <div class="progress-group">Add Products to Cart
                                        <span class="float-right"><b>160</b>/200</span>
                                        <div class="progress progress-sm">
                                            <div class="progress-bar bg-primary" style="width: 80%"></div>
                                        </div>
                                    </div>
                                    <!-- /.progress-group -->

                                    <div class="progress-group">Complete Purchase
                                        <span class="float-right"><b>310</b>/400</span>
                                        <div class="progress progress-sm">
                                            <div class="progress-bar bg-danger" style="width: 75%"></div>
                                        </div>
                                    </div>

                                    <!-- /.progress-group -->
                                    <div class="progress-group">
                                        <span class="progress-text">Visit Premium Page</span>
                                        <span class="float-right"><b>480</b>/800</span>
                                        <div class="progress progress-sm">
                                            <div class="progress-bar bg-success" style="width: 60%"></div>
                                        </div>
                                    </div>

                                    <!-- /.progress-group -->
                                    <div class="progress-group">
                                        Send Inquiries
                          <span class="float-right"><b>250</b>/500</span>
                                        <div class="progress progress-sm">
                                            <div class="progress-bar bg-warning" style="width: 50%"></div>
                                        </div>
                                    </div>
                                    <!-- /.progress-group -->
                                </div>
                                <!-- /.col -->
                            </div>
                            <!-- /.row -->
                        </div>
                        <!-- ./card-body -->
                        <div class="card-footer">
                            <div class="row">
                                <div class="col-sm-3 col-6">
                                    <div class="description-block border-right">
                                        <%--<span class="description-percentage text-success"><i class="fa fa-caret-up"></i>17%</span>--%>
                                        <h5 class="description-header"><span id="spnTotalPayIn">0</span></h5>
                                        <span class="description-text">TOTAL COLLECTION</span>
                                    </div>
                                    <!-- /.description-block -->
                                </div>
                                <!-- /.col -->
                                <div class="col-sm-3 col-6">
                                    <div class="description-block border-right">
                                        <%--<span class="description-percentage text-warning"><i class="fa fa-caret-left"></i>0%</span>--%>
                                        <h5 class="description-header"><span id="spnTotalPayOut">0</span></h5>
                                        <span class="description-text">TOTAL PAYMET</span>
                                    </div>
                                    <!-- /.description-block -->
                                </div>
                                <!-- /.col -->
                                <div class="col-sm-3 col-6 hide">
                                    <div class="description-block border-right">
                                        <%--<span class="description-percentage text-success"><i class="fa fa-caret-up"></i>20%</span>--%>
                                        <h5 class="description-header"><span id="spnTotalProfit">0</span></h5>
                                        <span class="description-text">TOTAL PROFIT</span>
                                    </div>
                                    <!-- /.description-block -->
                                </div>
                                <!-- /.col -->
                                <div class="col-sm-3 col-6 hide">
                                    <div class="description-block">
                                        <%--<span class="description-percentage text-danger"><i class="fa fa-caret-down"></i>18%</span>--%>
                                        <h5 class="description-header">1200</h5>
                                        <span class="description-text">GOAL COMPLETIONS</span>
                                    </div>
                                    <!-- /.description-block -->
                                </div>
                            </div>
                            <!-- /.row -->
                        </div>
                        <!-- /.card-footer -->
                    </div>
                    <!-- /.card -->
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->

        </div>
    </div>
    <div class="clearall"></div>
</asp:Content>
