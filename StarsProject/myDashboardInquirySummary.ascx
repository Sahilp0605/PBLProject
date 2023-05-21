<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myDashboardInquirySummary.ascx.cs" Inherits="StarsProject.myDashboardInquirySummary" %>
<link type="text/css" href="css/font-awesome.css" rel="stylesheet" />
<link type="text/css" rel="stylesheet" href="plugins/iCheck/flat/blue.css"/>
<link type="text/css" rel="stylesheet" href="plugins/morris/morris.css"/>
<script type="text/javascript" src="js/raphael-min.js"></script>
<script type="text/javascript" src="plugins/morris/morris.min.js"></script>

<script type="text/javascript">
    function initInqGraphLayout(cat) {
        debugger;
                var grLegend = '';
                var m1, y1;
                m1 = jQuery("#hdnMonth").val();
                y1 = jQuery("#hdnYear").val();
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
                    jQuery("#linechartinq").empty();
                    jQuery("#donutchart1inq").empty();
                    jQuery("#donutchart2inq").empty();
                   
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
                                jQuery("#linechartinq").parent().attr("class", "hide");
                                jQuery("#label").hide();
                                var resultdata = [{ label: "No Data Availble", value: 0 }];
                            }
                            else {
                                jQuery("#linechartinq").parent().attr("class", "show");
                            }
                            // ---------------------------------------
                            var linechartinq;
                            if (cat == 'Inquiry') {
                                linechartinq = new Morris.Bar({
                                    element: 'linechartinq', resize: true, data: resultdata,
                                    xkey: 'label', ykeys: ['value1', 'value2', 'value3', 'value4', 'value5', 'value6'],
                                    count:[1,2,1,2,1,5],
                                    labels: ['Open','WIP','Hold','Lost','Success','Other'],
                                    barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                    lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                    gridTextFamily: 'Open Sans', gridTextSize: 12
                                }).on('click', function (i, row) {
                                    //alert('clicked bar')
                                });
                            }
                            else {
                                linechartinq = new Morris.Bar({
                                    element: 'linechartinq', resize: true, data: resultdata,
                                    xkey: 'label', ykeys: ['value'], labels: [grLegend],
                                    barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'], lineWidth: 2, hideHover: 'auto',
                                    gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                    gridTextFamily: 'Open Sans', gridTextSize: 12
                                }).on('click', function (i, row) {
                                    //alert('clicked bar')
                                });
                            }
                            // ---------------------------------------
                            linechartinq.draw();
                            // ---------------------------------------
                            // Loading Donut-1
                            // ---------------------------------------
                            jQuery.ajax({
                                type: "POST", url: srcUrl,
                                data: '{pMon: ' + m1 + ', pYear: ' + y1 + ', pCategory:\'BRAND\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    var resultdata1 = JSON.parse(data.d);
                                    var donutchart1inq = new Morris.Donut({
                                        element: 'donutchart1inq', resize: true, colors: ['#007bff', '#dc3545', '#28a745', '#17a2b8', '#ffc107', '#000080', '#6c757d', '#ff6a52', '#8afbe6'],
                                        data: resultdata1, hideHover: 'auto'
                                    }).on('click', function (i, row) {
                                        alert(row.label + ": " + row.value)
                                    });
                                    // ---------------------------------------
                                    donutchart1inq.redraw();
                                    // ---------------------------------------
                                    // Loading Donut-2
                                    // ---------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: srcUrl,
                                        data: '{pMon: ' + m1 + ', pYear: ' + y1 + ', pCategory:\'PRGROUP\'}',
                                        contentType: "application/json; charset=utf-8",
                                        success: function (data) {
                                            var resultdata2 = JSON.parse(data.d);
                                            var donutchart2inq = new Morris.Donut({
                                                element: 'donutchart2inq', resize: true, colors: ['#007bff', '#dc3545', '#28a745', '#17a2b8', '#ffc107', '#000080', '#6c757d', '#ff6a52', '#8afbe6'],
                                                data: resultdata2, hideHover: 'auto'
                                            }).on('click', function (i, row) {
                                                alert(row.label + ": " + row.value)
                                            });
                                            // ---------------------------------------
                                            donutchart2inq.redraw();
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

        /*---- //*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//*//* ----*/
        .ImgbtnSelect { Width: 40px; Height: 40px; float: left;}
        .divInfo { float: left; margin: 3px; padding: 2px; border: 0px; width: 100% !important;}
        .divHead { height: 30px; float: left; padding:2px; border: 2px solid silver; background-color: #ddd; font-family: verdana,Arial,Helvetica,sans-serif; font-size: 14px; font-weight: bold; min-width: 99%; vertical-align: middle; text-align: center; }
        .stdtable { height: 168px; font-size:14px; color: Navy; font-weight: 400;}
        .stdtable th { text-align: center !important; }
        .stdtable td { text-align: center !important; }

        #map_canvas { text-align: center; float: center; min-width: 48% !important; height: 600px !important; }
        
        .stdtable tbody tr td:first-child { border-left: 0px !important; border-right: 0px !important; }
        .stdtable tbody tr td:last-child { border-left: 0px !important; border-right: 0px !important; }
      
        .boxClose { display:none; height:0px; }
        .boxOpen  { display:block;}

        .limited{
            white-space: nowrap;
            max-width:100px;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .progmodal
        {
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
        .progcenter
        {
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
        .progcenter img
        {
            height: 128px;
            width: 128px;
        }
        .morris-hover{position:absolute;z-index:1000;}
        .morris-hover-point { color: greenyellow !important; }
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
          0% { -webkit-transform: rotate(0deg); }
          100% { -webkit-transform: rotate(360deg); }
        }

        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }

        /* Add animation to "page content" */
        .animate-bottom {
          position: relative;
          -webkit-animation-name: animatebottom;
          -webkit-animation-duration: 1s;
          animation-name: animatebottom;
          animation-duration: 1s
        }

        @-webkit-keyframes animatebottom {
          from { bottom:-100px; opacity:0 } 
          to { bottom:0px; opacity:1 }
        }

        @keyframes animatebottom { 
          from{ bottom:-100px; opacity:0 } 
          to{ bottom:0; opacity:1 }
        }

        #myDiv {
          display: none;
          text-align: center;
        }
        
        .popover { left:5px !important; top:6px; min-width: 600px; }
        .popover .arrow { left:6px !important; }
        .card-footer { border-top:2px solid navy !important; }
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
        .textureWodden { background:url(images/texture_wooden.jpg); background-size: cover; }
        .textureBubble { background:url(images/texture_bubble.jpg); background-size: cover; color:lightyellow; }
        .textureCardboard1 { background:url(images/texture_cardboard.jpg); background-size: cover; }
        .textureCardboard2 { background:url(images/texture_cardboard1.jpg); background-size: cover; }
        .textureGradient { background:url(images/texture_gradient1.jpg); background-size: cover; }
        .textureLight1 { background:url(images/texture_light1.jpg); background-size: cover; }
        .textureLight2 { background:url(images/texture_light2.jpg); background-size: cover; }
        .textureLight3 { background:url(images/texture_light3.jpg); background-size: cover; }
        .texturePaper1 { background:url(images/texture_paper.jpg); background-size: cover; }
        .texturePaper2 { background:url(images/texture_paper1.jpg); background-size: cover; }
        .texturePaper3 { background:url(images/texture_paper2.jpg); background-size: cover; }
        .lightDropdown { background-color:lightyellow; color:navy; font-size: 16px; font-weight:400; border-radius: 4px; margin-top: 0px; padding:0px 10px; }
    </style>

<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" /> 

    <div class="col-md-6 pr-0">
        <!-- solid sales graph -->
        <div class="card bg-info-gradient" style="background: #ced3d4;background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#ced3d4),color-stop(1,#89d8e4));">
            <div class="card-header no-border" style="padding: 5px 10px;">
                <h5 class="card-title"><i class="fa fa-area-chart ml-2 mr-2"></i><span id="spAnalysis1">Annual Statistics</span></h5>
            </div>
            <div class="card-body" style="padding:0px !important;">
            <div class="chart" id="linechartinq" runat="server" clientidmode="static" style="position: relative; height: 200px;"></div>
            </div>
            <!-- /.card-body -->
        </div>
    </div>

    <div class="col-md-6">
        <!-- Donut : Product Brand -->
        <div class="card bg-info-gradient" style="width:49%; display:inline-table; background: #17a2b8;background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
            <div class="card-header no-border" style="padding: 5px 10px;">
                <h5><i class="fa fa-pie-chart ml-2 mr-2"></i><span id="spAnalysis2">Product Brand</span></h5>
            </div>
            <div class="card-body" style="padding:0px !important;">
                <div class="chart tab-pane" id="donutchart1inq" runat="server" clientidmode="static" style="position: relative; height: 205px;"></div>
            </div>
            <!-- /.card-body -->
        </div>
        <!-- Donut : Product Group -->
        <div class="card bg-info-gradient" style="width:49%; display:inline-flex; background: #17a2b8;background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
            <div class="card-header no-border" style="padding: 5px 10px;">
            <h5><i class="fa fa-pie-chart ml-2 mr-2"></i><span id="spAnalysis3">Product Group</span></h5>
            </div>
            <div class="card-body" style="padding:0px !important;">
                <div class="chart tab-pane" id="donutchart2inq" runat="server" clientidmode="static" style="position: relative; height: 205px;"></div>
            </div>
            <!-- /.card-body -->
        </div>
    </div>
