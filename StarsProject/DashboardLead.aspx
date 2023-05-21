<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="DashboardLead.aspx.cs" Inherits="StarsProject.DashboardLead" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="plugins/morris/morris.css"/>
    <script type="text/javascript" src="js/raphael-min.js"></script>
    <script type="text/javascript" src="plugins/morris/morris.min.js"></script>

    <style type="text/css">
        svg { width: 100%; }

        .myDropDownClass {
            background-color: #fff6e1;
            color: navy; 
            font-size: medium;
            width: 150px; height: 30px !important; 
            border: 1px solid darkgray; 
            margin-top:1px; padding:1px 5px;
            border-radius: 5px;
        }

        .myDropDownClass250 {
            background-color: #fff6e1;
            color: navy; 
            font-size: medium;
            width: 250px; height: 30px !important; 
            border: 1px solid darkgray; 
            margin-top:1px; padding:1px 5px;
            border-radius: 5px;
        }
        .card-title { color:#1e90ff; }
        .myDrpContent { width:230px !important; height:auto !important; } 
        .myDrpContent li { min-height:auto; padding:0px; }
        .myDrpContent li > a { min-height:auto; padding:2px; font-size:12px; color: navy; padding: 3px 5px; background-color: #f3ebeb; }
        
        .dataTables_filter, .dataTables_info, .dataTables_length { display: none; }
        table.dataTable thead th, table.dataTable thead td {
            padding: 5px 5px;
            font-size: 12px;
        }
        table.dataTable tbody th, table.dataTable tbody td {
            padding: 5px 5px;
            font-size: 12px;
            color:black;
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
            margin-left: 15px;
        }
        .cardRound { box-shadow: black -6px 8px 6px -6px !important; border-radius: 8px 8px 0px 0px !important; }
        .h5Round { border-radius: 8px 8px 0px 0px; margin-bottom:5px; padding:5px; line-height:35px; background: linear-gradient(45deg, #303f9f, #7b1fa2) !important; color:white !important; }
        .h5RoundHead { 
            border-radius: 0px 0px 0px 0px; 
            margin-bottom:5px; padding:5px; 
            line-height:35px; 
            /*background: linear-gradient(45deg, silver, #7f88ec6e);*/ 
            background: linear-gradient(45deg, #303f9f, #7b1fa2) !important; color:white !important;
        }
        #tblAnalysis.dataTable thead tr th, #tblAnalysis.dataTable thead td {
            font-size:18px !important;
            background-color:black !important;
            color:white !important;
        } 
        #tblAnalysis.dataTable tbody td {
            font-size:18px !important;
        }  
        .myLeftAlign { text-align:left !important;}
        .myCenterAlign { text-align:center !important;}
        .myRightAlign { text-align:right !important;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true" ScriptMode="Release" AsyncPostBackTimeout="100"></asp:ScriptManager>

    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientURL" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnWelcomeGreet" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="updDashboardLead" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <!-- BEGIN: Lead Section -->
        <div id="divLeads" style="background: linear-gradient(45deg, silver, #7f88ec6e); margin-bottom: 10px; padding-left: 10px; padding-right: 10px;">
            <div class="row">
                <div class="col m12">
                    <div class="card user-statistics-card mb-0">
                        <div class="card-content p-0">
                            <h5 class="h5RoundHead">
                                <span id="Span1" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Leads Data Filter : </span>
                                <asp:DropDownList ID="drpLeadMonth" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpLeadMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList ID="drpLeadYear" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpLeadMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList ID="drpLeadEmployee" runat="server" class="myDropDownClass select2-theme browser-default mr-2 float-right" style="width:250px;" ClientIDMode="Static" OnSelectedIndexChanged="drpLeadMonthYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </h5>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col m8">
                    <div id="cardLeadSummary" class="pt-0">
                        <div class="row">
                            <div class="col m4">
                                <div class="card gradient-45deg-light-blue-cyan gradient-shadow min-height-100 white-text animate fadeLeft">
                                    <div class="padding-4">
                                        <a href="javascript:openLeadView('inquiry','Opportunities');" title="Click To View">
                                            <div class="row">
                                                <div class="col m7">
                                                    <span class="avatar-status avatar-online avatar-50">
                                                        <img src="images/ChatBox.jpg" alt="avatar" width="30" height="30" />
                                                    </span>
                                                    <p>Opportunities</p>
                                                </div>
                                                <div class="col m5 right-align">
                                                    <h5 class="mb-0 white-text"><span id="spnUnknown" runat="server" clientidmode="static">0</span></h5>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col m4">
                                <div class="card gradient-45deg-red-pink gradient-shadow min-height-100 white-text animate fadeLeft">
                                    <div class="padding-4">
                                        <a href="javascript:openLeadView('inquiry','Close - Lost');" title="Click To View">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online avatar-50">
                                                    <img src="images/ThumbsDown.png" alt="avatar" width="30" height="30" />
                                                </span>
                                                <p>Lead Lost</p>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnLost" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col m4">
                                <div class="card gradient-45deg-green-teal gradient-shadow min-height-100 white-text animate fadeRight">
                                    <div class="padding-4">
                                        <a href="javascript:openLeadView('inquiry','Close - Success');" title="Click To View">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online avatar-50">
                                                    <img src="images/ThumbsUp.png" alt="avatar" width="30" height="30" />
                                                </span>
                                                <p>Lead Won</p>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnSuccess" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--card stats end-->
                    <!-- Lead Graph -->
                    <div class="row">
                        <div class="col m12">
                            <!-- User Statistics -->
                            <div class="card user-statistics-card animate fadeLeft mt-0">
                                <div class="card-content padding-2">
                                    <div class="select-action">
                                        <h4 class="card-title mb-0">Lead Analysis</a></h4>
                                    </div>
                                    <div class="user-statistics-container" style="width:auto;">
                                        <div id="linechartinq" class="user-statistics-shadow ct-golden-section" style="width:auto; min-height:240px;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col m4 pl-0">
                    <div class="card user-statistics-card animate fadeLeft">
                        <div class="card-content padding-2">
                            <div class="select-action">
                                <h4 class="card-title mb-0">Analysis - Lead Source</h4>
                            </div>
                            <div class="user-statistics-container" style="width:auto;">
                                <div id="donutLeadSoure" class="user-statistics-shadow ct-golden-section" style="width:auto; min-height:150px;"></div>
                            </div>
                        </div>
                    </div>

                    <div class="card user-statistics-card animate fadeLeft">
                        <div class="card-content padding-2">
                            <div class="select-action">
                                <h4 class="card-title mb-0">Analysis - DisQualify</h4>
                            </div>
                            <div class="user-statistics-container" style="width:auto;">
                                <div id="donutLeadDisqualify" class="user-statistics-shadow ct-golden-section" style="width:auto; min-height:175px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Lead Analysis Table -->
            <div class="row">
                <div class="col m12">
                    <!-- Recent Buyers -->
                    <div class="card recent-buyers-card">
                        <div class="card-content padding-2">
                            <div class="select-action">
                                <h4 class="card-title mb-0">Lead Analysis&nbsp;&nbsp;&nbsp;
                                    <a class='dropdown-trigger grey-text float-right' href='#' data-target='drpInqCat'><i class="material-icons">more_vert</i></a>
                                    <asp:DropDownList ID="drpAnalysis" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass250" AutoPostBack="true" OnSelectedIndexChanged="drpAnalysis_SelectedIndexChanged">
                                        <asp:ListItem Text="By Month" Value="bymonth" Selected="True" />
                                        <asp:ListItem Text="By Lead Stages" Value="bystatus" />
                                        <asp:ListItem Text="By Lead Source" Value="bysource" />
                                        <asp:ListItem Text="By Tele-Caller" Value="bycaller" />
                                        <asp:ListItem Text="By Sales Exec." Value="byexecutive" />
                                    </asp:DropDownList>
                                </h4>
                            </div>
                            <table id="tblAnalysis" class="display width-100 nowrap"></table>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Lead Status Table -->
            <div class="row">
                <div class="col m12">
                    <!-- Recent Buyers -->
                    <div class="card recent-buyers-card">
                        <div class="card-content padding-2">
                            <div class="select-action">
                                <h4 class="card-title mb-0">Lead Progress&nbsp;&nbsp;&nbsp;<span id="spnProgressCount" runat="server" clientidmode="Static" class="badge gradient-45deg-yellow-green black-text" style="line-height:25px; float:none;line-height: 25px; vertical-align: text-top; padding: 5px 10px; border-radius: 8px;"></span>
                                    <a class='dropdown-trigger grey-text float-right' href='#' data-target='drpInqCat'><i class="material-icons">more_vert</i></a>
                                    <asp:DropDownList ID="drpInqCat" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass250" AutoPostBack="true" OnSelectedIndexChanged="drpInqCat_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:ImageButton ID="btnExportToExcel" runat="server" ClientIDMode="Static" CssClass="btn-floating float-right ml-1 mr-1 tooltipped" AlternateText="Export" data-tooltip="Export To Excel" ImageAlign="left" ImageUrl="images/buttons/export2excel.png" OnClick="btnExportToExcel_Click" Height="35px" Width="35px" Style="z-index: 0;" />
                                </h4>
                            </div>
                            <table id="tblInquiryStatus" class="display width-100 nowrap"></table>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row hide">
                <div class="col m4">
                    <div class="card animate fadeRight">
                        <div class="card-content padding-2">
                            <h4 class="card-title mb-0">Conversion Ratio</h4>
                            <div class="conversion-ration-container mt-8">
                                <div id="conversion-ration-bar-chart" class="conversion-ration-shadow"></div>
                            </div>
                            <p class="medium-small center-align">This month conversion ratio</p>
                            <h5 class="center-align mb-0 mt-0">62%</h5>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!-- END: Lead Section -->

        <%--*************************************************************************************************--%>
        <!-- BEGIN: External Section -->
        <div id="divExternalLead" style="background: linear-gradient(45deg, #fafdff, #26c6da) !important; margin-bottom: 10px; padding-left: 10px; padding-right: 10px;">
            <div class="row">
                <div class="col m12">
                    <div class="card user-statistics-card mb-0">
                        <div class="card-content p-0">
                            <h5 class="h5RoundHead">
                                <span id="Span3" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">External Portal Opportunities : </span>
                                <asp:DropDownList ID="drpExtMonth" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpExtMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList ID="drpExtYear" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpExtMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList ID="drpExtEmployee" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" style="width:250px;" AutoPostBack="true" OnSelectedIndexChanged="drpExtMonthYear_SelectedIndexChanged"></asp:DropDownList>
                            </h5>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col m12">
                    <div id="cardExternalSummary" class="mt-0 pt-0">
                        <div class="row">
                            <div class="col m3">
                                <div class="card gradient-45deg-orange-amber gradient-shadow min-height-100 white-text animate fadeLeft">
                                    <div class="padding-4">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online avatar-50">
                                                    <img src="images/ChatBox.jpg" alt="avatar" width="30" height="30" />
                                                </span>
                                                <p>New Opportunities</p>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnExtUntouch" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="card gradient-45deg-light-blue-cyan gradient-shadow min-height-100 white-text animate fadeLeft">
                                    <div class="padding-4">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online avatar-50">
                                                    <img src="images/ChatBox.jpg" alt="avatar" width="30" height="30" />
                                                </span>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnExtProcess" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        <div class="row">
                                            <div class="col m12"><p>In Process</p></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="card gradient-45deg-red-pink gradient-shadow min-height-100 white-text animate fadeLeft">
                                    <div class="padding-4">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online avatar-50">
                                                    <img src="images/ThumbsDown.png" alt="avatar" width="30" height="30" />
                                                </span>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnExtDisqualify" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        <div class="row">
                                            <div class="col m12"><p>Opportunity - DisQualified</p></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="card gradient-45deg-green-teal gradient-shadow min-height-100 white-text animate fadeRight">
                                    <div class="padding-4">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online avatar-50">
                                                    <img src="images/ThumbsUp.png" alt="avatar" width="30" height="30" />
                                                </span>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnExtQualify" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        <div class="row">
                                            <div class="col m12"><p>Opportunity - Qualified</p></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col m12">
                    <!-- User Statistics -->
                    <div class="card user-statistics-card animate fadeLeft mt-0">
                        <div class="card-content padding-2" style="min-height:275px;">
                            <h5 class="card-title mb-0">External Lead Analysis <i class="material-icons float-right">more_vert</i></h5>
                            <div class="user-statistics-container">
                                <div class="chart" id="external-chart" style="position: relative; width:100%; height:200px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END: External Section -->

        <%--*************************************************************************************************--%>
        <!-- BEGIN: Tele Caller Section -->
        <div id="divTeleCaller" style="background: linear-gradient(45deg, silver, #2b1faa) !important; margin-bottom: 10px; padding-left: 10px; padding-right: 10px;">
            <!-- Tele Caller Statistics -->
            <div class="row">
                <div class="col m12">
                    <div class="card user-statistics-card mb-0">
                        <div class="card-content p-0">
                            <h5 class="h5RoundHead">
                                <span id="Span2" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Tele Caller Opportunities  : </span>
                                <asp:DropDownList ID="drpTeleMonth" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpTeleMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList ID="drpTeleYear" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpTeleMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList ID="drpTeleEmployee" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" style="width:250px;" AutoPostBack="true" OnSelectedIndexChanged="drpTeleMonthYear_SelectedIndexChanged"></asp:DropDownList>
                            </h5>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col m8">
                    <div id="cardTeleCallerSummary" class="mt-0 pt-0">
                        <div class="row">
                            <div class="col m3">
                                <div class="card gradient-45deg-deep-orange-orange gradient-shadow min-height-100 white-text animate fadeLeft">
                                    <div class="padding-4">
                                        <a href="javascript:openExternalLeadView('telecaller','new');" title="Click To View">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online">
                                                    <img src="images/ChatBox.jpg" alt="avatar" width="30" height="30" />
                                                </span>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnTeleUntouch" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        <div class="row">
                                            <div class="col m12"><p>New Opportunities</p></div>
                                        </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="card gradient-45deg-light-blue-cyan gradient-shadow min-height-100 white-text animate fadeLeft">
                                    <div class="padding-4">
                                        <a href="javascript:openExternalLeadView('telecaller','inprocess');" title="Click To View">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online">
                                                    <img src="images/ChatBox.jpg" alt="avatar" width="30" height="30" />
                                                </span>
                                                    
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnTeleProcess" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        <div class="row">
                                            <div class="col m12"><p>In Process</p></div>
                                        </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="card gradient-45deg-red-pink gradient-shadow min-height-100 white-text animate fadeLeft">
                                    <div class="padding-4">
                                        <a href="javascript:openExternalLeadView('telecaller','disqualified');" title="Click To View">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online">
                                                    <img src="images/ThumbsDown.png" alt="avatar" width="30" height="30" />
                                                </span>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnTeleDisqualify" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        <div class="row">
                                            <div class="col m12"><p>DisQualified</p></div>
                                        </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="card gradient-45deg-green-teal gradient-shadow min-height-100 white-text animate fadeRight">
                                    <div class="padding-4">
                                        <a href="javascript:openExternalLeadView('telecaller','qualified');" title="Click To View">
                                        <div class="row">
                                            <div class="col m7">
                                                <span class="avatar-status avatar-online">
                                                    <img src="images/ThumbsUp.png" alt="avatar" width="30" height="30" />
                                                </span>
                                            </div>
                                            <div class="col m5 right-align"><h5 class="mb-0 white-text"><span id="spnTeleQualify" runat="server" clientidmode="static">0</span></h5></div>
                                        </div>
                                        <div class="row">
                                            <div class="col m12"><p>Qualified</p></div>
                                        </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col m12">
                            <!-- Tele Caller Analysis -->
                            <div class="card user-statistics-card">
                                <div class="card-content padding-2" style="min-height:275px;">
                                    <div class="select-action">
                                        <h5 class="card-title mb-0">Tele Caller Analysis
                                            <%--<a class='dropdown-trigger grey-text float-right' href='#' data-target='drpTeleCallerGraph'><i class="material-icons">more_vert</i></a>--%>
                                        </h5>
                                    </div>
                                    <div class="user-statistics-container">
                                        <div id="linecharttele" class="user-statistics-shadow ct-golden-section" style="min-height:200px;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col m4 pl-0">
                    <div class="card user-statistics-card animate fadeLeft">
                        <div class="card-content padding-2">
                            <div class="select-action">
                                <h4 class="card-title mb-0">Analysis - Conversion</h4>
                            </div>
                            <div class="user-statistics-container" style="width:auto;">
                                <div id="donutTeleConversion" class="user-statistics-shadow ct-golden-section" style="width:auto; min-height:150px;"></div>
                            </div>
                        </div>
                    </div>

                    <div class="card user-statistics-card animate fadeLeft">
                        <div class="card-content padding-2">
                            <div class="select-action">
                                <h4 class="card-title mb-0">Analysis - DisQualify</h4>
                            </div>
                            <div class="user-statistics-container" style="width:auto;">
                                <div id="donutTeleDisqualify" class="user-statistics-shadow ct-golden-section" style="width:auto; min-height:175px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col m6">
                    <!-- User Statistics -->
                    <div class="card user-statistics-card animate fadeLeft mt-0">
                        <div class="card-content padding-2" style="min-height:275px;">
                            <h5 class="card-title mb-0">Analysis - Daily Call Log<i class="material-icons float-right">more_vert</i></h5>
                            <div class="user-statistics-container">
                                <div class="chart" id="teleDailyEntry" style="position: relative; width:100%; height:200px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col m6">
                    <!-- User Statistics -->
                    <div class="card user-statistics-card animate fadeLeft mt-0">
                        <div class="card-content padding-2" style="min-height:275px;">
                            <h5 class="card-title mb-0">Analysis - Monthly Call Log<i class="material-icons float-right">more_vert</i></h5>
                            <div class="user-statistics-container">
                                <div class="chart" id="teleMonthlyEntry" style="position: relative; width:100%; height:200px;"></div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <!-- END: Tele Caller Section -->
        <br /><br /><br />
        <%--*************************************************************************************************--%>
        <asp:Button ID="Button1Lead" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="panelLead" TargetControlID="Button1Lead" CancelControlID="Button2Lead" BackgroundCssClass="Background">  
        </cc1:ModalPopupExtender>  
        <asp:Panel ID="panelLead" runat="server" ClientIDMode="Static" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
            <div id="myModal">
                <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom:10px !important;">
                    <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                        <i class="material-icons prefix">ac_unit</i>
                        <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                        <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2Lead" runat="server" Text=""/>
                    </h5>
                </div>
            </div>
            <iframe id="ifrModuleLead" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border:1px solid silver; background-color:transparent; width:100%; height:90%; padding:5px;"></iframe>  
            <br/>  
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExportToExcel" />
    </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        window.onload = function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "Lead Management - Analytics & Summary";
            loadInquiryStatusList('Open');
            initInqGraphLayout('Inquiry');
            initInqDonutGraphLayout('InquirySource');
            initInqDonutGraphLayout('DisQuali');
            // ----------------------------------------------
            initExternalGraph('IndiaMart');
            // ----------------------------------------------
            initInqGraphLayout('TeleCallStatus');
            initTeleDonutGraphLayout('TeleDisQuali');
            initTeleDonutGraphLayout('TeleConversion');
            initTeleEntryGraphLayout('telecaller', 'monthly');
            initTeleEntryGraphLayout('telecaller', 'daily');
            // ----------------------------------------------
            loadAnalysisReport('bymonth');
        }

        function capitalizeString(str) {
            return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }

        function openDashboardBox(view, mode, id) {
            if (view == 'product') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Products";
                var pageUrl = "Products.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();
            }
            else if (view == 'customer') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Customer";
                var pageUrl = "Customers.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();
            }
            else if (view == 'todo') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Task Activity";
                var pageUrl = "ToDo.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();
            }
            else if (view == 'followup') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Followup";
                var pageUrl = "Followup.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();
            }
            else if (view == 'leave') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leave Request";
                var pageUrl = "LeaveRequest.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();
            }
            else if (view == 'user') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Users";
                var pageUrl = "UserMgmt.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();
            }
            else if (view == 'complaint') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Complaints";
                var pageUrl = "Complaint.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();

            }
            else if (view == 'inquiry') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leads";
                var pageUrl = "InquiryInfo.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();
            }
            else if (view == 'inquirybyno') {
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
                            $('#ifrModule').attr('src', pageUrl);
                            $find("mpe").show();
                        }
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });

            }
            else if (view == 'quotation') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Quotation";
                var pageUrl = "Quotation.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleLead').attr('src', pageUrl);
                $find("mpe").show();
            }
            else if (view == 'quotationbyno') {
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
                            $('#ifrModule').attr('src', pageUrl);
                            $find("mpe").show();
                        }
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
        }
        // ------------------------------------------------------------------------
        // Load Inquiry Status Wise GRID TABLE
        // ------------------------------------------------------------------------
        var table;
        function loadInquiryStatusList(t11) {
            var dataurl;
            var para1;
            var para2;
            var para3;
            var viewmode = 'inquiry';   // jQuery("#hdnView").val();
            var groupColumn, valColspan;
            var groupColumnTitle = "";
            var serialKey = "";
            // --------------------------------------------------------
            //alert(viewmode)
            if (viewmode == 'inquiry')
            {
                valColspan = 11;

                para1 = jQuery.trim(t11);
                para2 = jQuery("#drpLeadMonth").val();
                para3 = jQuery("#drpLeadYear").val();
                uid = jQuery("#drpLeadEmployee").val();
                serialKey = jQuery.trim(jQuery("#hdnSerialKey").val());
                // ---------------------------------------------------------
                groupColumn = 7;
                groupColumnTitle = "Sales Executive :";
                // ---------------------------------------------------------
                //jQuery("#lblCustomHead").text('Inquiry List - [ Status : ' + para1 + ' ]')
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webInquiryStatusList?pLoginUserID=' + uid + '&pStatus=' + t11 + '&pMonth=' + para2 + '&pYear=' + para3;
                if (serialKey == 'ECO3-2G21-TECH-3MRT') {
                    var colstructure = [
                                {
                                    title: "Lead #", data: "InquiryNo", width: "110", className: "text-center",
                                    render: function (data, type, row, meta) {
                                        data = '<a href="javascript:openDashboardBox(\'inquiry\',\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                        return data;
                                    }
                                },
                                { title: "Initiate Date", data: "InquiryDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MM-YYYY') : ''; } },
                                { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                { title: "Contact #", data: "ContactNo1", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                { title: "Source", data: "InquirySource", className: "text-center", width: "150" },
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
                                { title: "Status", data: "InquiryStatus", width: "200" },
                                { title: "Sales Executive", data: "EmployeeName", width: "300" }
                    ];
                }
                else
                {
                    var colstructure = [
                                {
                                    title: "Lead #", data: "InquiryNo", width: "110", className: "text-center",
                                    render: function (data, type, row, meta) {
                                        data = '<a href="javascript:openDashboardBox(\'inquiry\',\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                                        return data;
                                    }
                                },
                                { title: "Initiate Date", data: "InquiryDate", type: "date", width: "150", className: "text-center", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                                { title: "Customer Name", data: "CustomerName", width: "250", render: function (data, type, full) { return capitalizeString(data); } },
                                { title: "Source", data: "InquirySource", className: "text-center", width: "150" },
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
                                { title: "Status", data: "InquiryStatus", width: "200" },
                                { title: "Sales Executive", data: "EmployeeName", width: "300" }
                    ];
                }

            }


            // -----------------------------------------------
            if ($.fn.dataTable.isDataTable('#tblInquiryStatus')) {
                $('#tblInquiryStatus').DataTable().destroy();
                $('#tblInquiryStatus thead').empty();
                $('#tblInquiryStatus tbody').empty();
            }
            
            jQuery.ajax({
                url: dataurl,
                type: "get",
                success: function (data) {
                    if (data != null) {
                        var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                        // -----------------------------------------------
                        document.getElementById("spnProgressCount").innerText = resultdata.length;
                        // ----------------------------------------------

                        table = $('#tblInquiryStatus').DataTable({
                            "columnDefs": [{ "orderable": false, "visible": false, "targets": groupColumn }],
                            "order": [[groupColumn, 'asc']],
                            data: resultdata,
                            columns: colstructure,
                            deferRender: true,
                            "ordering": false,
                            "bSort": false,
                            "bFilter": false,
                            "bInfo": false,
                            "bPaginate": false,
                            "scrollY": 350,
                            "scrollX": true,
                            "bScrollCollapse": false,
                            "displayLength": 25,
                            "drawCallback": function (settings) {
                                var api = this.api();
                                var rows = api.rows({ page: 'current' }).nodes();
                                var last = null;

                                api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                                    if (last !== group) {
                                        if (viewmode == 'dailyactivitybyuser') {
                                            $(rows).eq(i).before('<tr class="group"><td colspan="' + valColspan + '"  style="background-color:#483d8b !important; color:white !important; font-size:14px !important; font-weight:bold !important; text-transform:capitalize;"><b>' + groupColumnTitle + '</b>' + moment(group).format('DD-MMM-YYYY') + '</td></tr>');
                                        }
                                        else
                                            $(rows).eq(i).before('<tr class="group"><td colspan="' + valColspan + '"  style="background-color:#483d8b !important; color:white !important; font-size:14px !important; font-weight:bold !important; text-transform:capitalize;"><b>' + groupColumnTitle + ' </b>' + group + '</td></tr>');

                                        last = group;
                                    }
                                });
                            }
                        });
                    }   // data != null
                },
                error: function (r) {
                    alert('Error : ' + r.responseText);
                },
                failure: function (r) {
                    alert('failure');
                }
            });
        }

        // ------------------------------------------------------------------------
        // Load CRM Analysis
        // ------------------------------------------------------------------------
        var table;
        function loadAnalysisReport(t11) {
            var dataurl;
            var para1;
            var para2;
            var para3;
            var viewmode = t11;   // jQuery("#hdnView").val();
            var groupColumn;
            var groupColumnTitle = "";
            var serialKey = "";

            // --------------------------------------------------------
            if (viewmode == 'bymonth' || viewmode == 'bystatus' || viewmode == 'bysource' || viewmode == 'bycaller' || viewmode == 'byexecutive') {
                valColspan = 10;

                para2 = jQuery("#drpLeadMonth").val();
                para3 = jQuery("#drpLeadYear").val();
                uid = jQuery("#drpLeadEmployee").val();
                serialKey = jQuery.trim(jQuery("#hdnSerialKey").val());
                // ---------------------------------------------------------
                dataurl = '/Services/NagrikService.asmx/webCrmAnalysis?pType=' + viewmode + '&pMonth=' + para2 + '&pYear=' + para3;
                var colstructure = [
                    { title: "Head Description", data: "ByHead", width: "200", className: "myLeftAlign" },
                    { title: "Total Deals", data: "Deals", width: "90", className: "myCenterAlign" },
                    { title: "Won Deal", data: "WonDeal", width: "90", className: "myCenterAlign" },
                    { title: "Lost Deal", data: "LostDeal", width: "90", className: "myCenterAlign" },
                    { title: "Convesion(%)", data: "Conversion", width: "50", className: "myCenterAlign" },
                    { title: "Contribution(%)", data: "Contribution", width: "50", className: "myCenterAlign" },
                    { title: "Total Revenue", data: "TotalRevenue", width: "110", className: "myRightAlign" },
                    { title: "Won Revenue", data: "WonRevenue", width: "110", className: "myRightAlign" },
                    { title: "Lost Revenue", data: "LostRevenue", width: "110", className: "myRightAlign" }
                ];                

                // -----------------------------------------------
                if ($.fn.dataTable.isDataTable('#tblAnalysis')) {
                    $('#tblAnalysis').DataTable().destroy();
                    $('#tblAnalysis thead').empty();
                    $('#tblAnalysis tbody').empty();
                }

                jQuery.ajax({
                    url: dataurl,
                    type: "get",
                    success: function (data) {
                        if (data != null) {
                            var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                            // ----------------------------------------------
                            table = $('#tblAnalysis').DataTable({
                                "columnDefs": [{ "orderable": false, "visible": false }],
                                data: resultdata,
                                columns: colstructure,
                                deferRender: true,
                                "ordering": false,
                                "bSort": false,
                                "bFilter": false,
                                "bInfo": false,
                                "bPaginate": false,
                                "scrollY": 350,
                                "scrollX": true,
                                "bScrollCollapse": false,
                                "displayLength": 25,
                                "drawCallback": function (settings) {
                                    var api = this.api();
                                    var rows = api.rows({ page: 'current' }).nodes();
                                    var last = null;

                                    //api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                                    //    if (last !== group) {
                                    //        if (viewmode == 'dailyactivitybyuser') {
                                    //            $(rows).eq(i).before('<tr class="group"><td colspan="' + valColspan + '"  style="background-color:#483d8b !important; color:white !important; font-size:14px !important; font-weight:bold !important; text-transform:capitalize;"><b>' + groupColumnTitle + '</b>' + moment(group).format('DD-MMM-YYYY') + '</td></tr>');
                                    //        }
                                    //        else
                                    //            $(rows).eq(i).before('<tr class="group"><td colspan="' + valColspan + '"  style="background-color:#483d8b !important; color:white !important; font-size:14px !important; font-weight:bold !important; text-transform:capitalize;"><b>' + groupColumnTitle + ' </b>' + group + '</td></tr>');

                                    //        last = group;
                                    //    }
                                    //});
                                }
                            });
                        }   
                    },
                    error: function (r) {
                        alert('Error : ' + r.responseText);
                    },
                    failure: function (r) {
                        alert('failure');
                    }
                });
            }
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Analysis - Lead Management
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function initInqGraphLayout(cat) {
            var uid = "";
            var chartid = "";
            var dataParameter = "";
            var grLegend = '';
            var m1 = 0, y1 = 0;
            
            // ----------------------------------------------------------
            var srcUrl = "";
            
            if (cat == 'Inquiry') {
                m1 = jQuery("#drpLeadMonth").val();
                y1 = jQuery("#drpLeadYear").val();
                uid = jQuery("#drpLeadEmployee").val();
                chartid = "linechartinq";
                jQuery("#" + chartid).empty();
                dataParameter = '{ pLoginUserID:\'' + uid + '\', pMon: ' + '0' + ', pYear: ' + y1 + ', pCategory:\'YEARLY\'}';
                srcUrl = 'Services/NagrikService.asmx/GetDashboard' + cat + 'Summary';
                grLegend = 'Count';
                jQuery("#divLeads").show("slide");
            }
            if (cat == 'TeleCallStatus') {
                y1 = jQuery("#drpTeleYear").val();
                uid = jQuery("#drpTeleEmployee").val();
                chartid = "linecharttele";
                jQuery("#" + chartid).empty();
                dataParameter = '{pLoginUserID:\'' + uid + '\', pYear: ' + y1 + '}';
                srcUrl = 'Services/NagrikService.asmx/GetDashboardInquiry' + cat + 'Summary';
                grLegend = 'Count';
                jQuery("#divTeleCaller").show("slide");
            }


            // -----------------------------------------------------------
            jQuery("#linechartinq").empty();
            if (srcUrl != '' && srcUrl != null) {
                // ---------------------------------------
                // Loading Line Graph
                // ---------------------------------------
                jQuery.ajax({
                    type: "POST", url: srcUrl,
                    data: dataParameter,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        // ---------------------------------------
                        if (resultdata.length == 0 || data.d == undefined) {
                            jQuery("#" + chartid).parent().attr("class", "hide");
                            jQuery("#label").hide();
                            var resultdata = [{ label: "No Data Availble", value: 0 }];
                        }
                        else {
                            jQuery("#" + chartid).parent().attr("class", "show");
                        }
                        // ---------------------------------------
                        var linechartinq;

                        if (cat == 'Inquiry') {
                            linechartinq = new Morris.Bar({
                                element: chartid, resize: true, redraw : true, 
                                data: resultdata,
                                xkey: 'label',
                                ykeys: ['value1', 'value2', 'value3', 'value4', 'value5', 'value6'],
                                labels: ['Open', 'WIP', 'Hold', 'Lost', 'Success', 'Other'],
                                barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                gridTextFamily: 'Open Sans', gridTextSize: 12
                            }).on('click', function (i, row) {});
                        }
                        else if (cat == 'InquirySource' || cat == 'DisQuali' || cat == 'TeleCallStatus' || cat == 'TeleDisQuali') {
                            var configa = {
                                element: chartid, resize: true, redraw: true,
                                data: resultdata,
                                xkey: 'label',
                                ykeys: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                                barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                gridTextFamily: 'Open Sans', gridTextSize: 12
                            };
                            configa.stacked = false;
                            linechartinq = new Morris.Bar(configa).on('click', function (i, row) { });
                        }
                        else if (cat == 'TeleConversion') {
                            var configa = {
                                element: chartid, resize: true, redraw: true,
                                data: resultdata,
                                xkey: 'label',
                                ykeys: ['value'],
                                labels: ['Conversion Ratio'],
                                barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                gridTextFamily: 'Open Sans', gridTextSize: 12
                            };
                            configa.stacked = false;
                            linechartinq = new Morris.Bar(configa).on('click', function (i, row) { });
                        }
                        // ---------------------------------------
                        if (resultdata.length > 0)
                            linechartinq.draw();
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
        }

        function initInqDonutGraphLayout(cat) {
            var uid = "";
            var chartid = "";
            var dataParameter = "";
            var grLegend = '';
            var m1 = 0, y1 = 0;
            var srcUrl = "";

            // ----------------------------------------------------------
            if (cat == 'InquirySource' || cat == 'DisQuali') {
                m1 = jQuery("#drpLeadMonth").val();
                y1 = jQuery("#drpLeadYear").val();
                uid = jQuery("#drpLeadEmployee").val();
                if (cat == 'InquirySource') {

                    chartid = 'donutLeadSoure';
                    srcUrl = 'Services/NagrikService.asmx/GetDashboard' + cat + 'Summary';
                }
                else if (cat == 'DisQuali') {
                    jQuery("#donutLeadSoure").empty();
                    chartid = 'donutLeadDisqualify';
                    srcUrl = 'Services/NagrikService.asmx/GetDashboardInquiry' + cat + 'Summary';
                }
                dataParameter = '{pLoginUserID:\'' + uid + '\', pMonth:' + m1 + ', pYear: ' + y1 + '}';
                grLegend = 'Count';
            }

            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            // Loading DONUT Graph
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            jQuery('#' + chartid).empty();

            if (srcUrl != '' && srcUrl != null) {
                jQuery.ajax({
                    type: "POST", url: srcUrl,
                    data: dataParameter,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata1 = JSON.parse(data.d);
                        if (resultdata1.length > 0) {
                            var configa = {
                                element: chartid, resize: true, redraw: true,
                                data: resultdata1,
                                xkey: 'label',
                                ykeys: 'value',
                                labels: 'Count',
                                barColors: ['#4a148c','#4caf50','#ff5722','#ffc107','#e91e63','#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                gridTextFamily: 'Open Sans', gridTextSize: 12
                            };
                            var donutchart1inq = new Morris.Donut(configa).on('click', function (i, row) { });
                            donutchart1inq.redraw();
                        }
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Analysis - External Lead Management
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function initExternalGraph(cat) {
            var grLegend = '';
            jQuery("#external-chart").empty();
            m1 = jQuery("#drpExtMonth").val();
            y1 = jQuery("#drpExtYear").val();
            // ----------------------------------------------------------
            var srcUrl = "";
            srcUrl = 'Services/NagrikService.asmx/GetDashboardExternalSummary';
            grLegend = 'Count';
            jQuery("#divExternalLead").show("slide");
            // -----------------------------------------------------------
            if (srcUrl != '' && srcUrl != null) {
                // ---------------------------------------
                // Loading Line Graph
                // ---------------------------------------
                // Checkbox with Image : ttps://stackoverflow.com/questions/30663562/use-images-like-checkboxes
                jQuery.ajax({
                    type: "POST", url: srcUrl,
                    data: '{pMon: ' + m1 + ', pYear: ' + y1 + ', pCategory:\'external\'}',
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
                            element: 'external-chart', resize: true, redraw: true, data: resultdata,
                            xkey: 'label', ykeys: ['value1', 'value2', 'value3', 'value4'], count: [1, 2, 1, 2, 1, 5],
                            labels: ['Qualified', 'DisQualified', 'In-Process', 'Untouch'],
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

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Analysis - Tele Caller Management
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function initTeleDonutGraphLayout(cat) {
            var uid = "";
            var chartid = "";
            var dataParameter = "";
            var grLegend = '';
            var m1 = 0, y1 = 0;
            var srcUrl = "";
            m1 = jQuery("#drpTeleMonth").val();
            y1 = jQuery("#drpTeleYear").val();
            uid = jQuery("#drpTeleEmployee").val();
            grLegend = 'Count';
            // ----------------------------------------------------------
            if (cat == 'TeleConversion') {
                chartid = 'donutTeleConversion';
                srcUrl = 'Services/NagrikService.asmx/GetDashboardInquiry' + cat + 'Summary';
            }
            else if (cat == 'TeleDisQuali') {
                chartid = 'donutTeleDisqualify';
                srcUrl = 'Services/NagrikService.asmx/GetDashboardInquiry' + cat + 'Summary';
            }
            dataParameter = '{pLoginUserID:\'' + uid + '\', pMonth:' + m1 + ', pYear: ' + y1 + '}';

            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            // Loading DONUT Graph
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            jQuery('#' + chartid).empty();

            if (srcUrl != '' && srcUrl != null) {
                jQuery.ajax({
                    type: "POST", url: srcUrl,
                    data: dataParameter,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata1 = JSON.parse(data.d);
                        if (resultdata1.length > 0) {
                            var configa = {
                                element: chartid, resize: true, redraw: true,
                                data: resultdata1,
                                xkey: 'label',
                                ykeys: 'value',
                                labels: 'Count',
                                barColors: ['#4a148c', '#4caf50', '#ff5722', '#ffc107', '#e91e63', '#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                gridTextFamily: 'Open Sans', gridTextSize: 12
                            };
                            var donutchart1inq = new Morris.Donut(configa).on('click', function (i, row) { });
                            donutchart1inq.redraw();
                        }
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
        }

        function initTeleEntryGraphLayout(source, cat) {
            var uid = "";
            var chartid = "";
            var dataParameter = "";
            var grLegend = '';
            var m1 = 0, y1 = 0;
            // ----------------------------------------------------------
            m1 = jQuery("#drpTeleMonth").val();
            y1 = jQuery("#drpTeleYear").val();
            uid = jQuery("#drpTeleEmployee").val();
            // ----------------------------------------------------------
            var srcUrl = 'Services/NagrikService.asmx/GetDashboardTeleEntrySummary';
            dataParameter = '{ pLoginUserID:\'' + uid + '\', pLeadSource:\'' + source + '\', pCategory:\'' + cat + '\', pMonth: ' + m1 + ', pYear: ' + y1 + '}';
            // -----------------------------------------------------------

            if (srcUrl != '' && srcUrl != null) {
                // ---------------------------------------
                // Loading Line Graph
                // ---------------------------------------
                if (cat == 'daily') {
                    chartid = "teleDailyEntry";
                }
                else if (cat == 'monthly') {
                    chartid = "teleMonthlyEntry";
                }
                // ---------------------------------------
                jQuery.ajax({
                    type: "POST", url: srcUrl,
                    data: dataParameter,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        // ---------------------------------------
                        if (resultdata.length == 0 || data.d == undefined) {
                            jQuery("#" + chartid).parent().attr("class", "hide");
                            jQuery("#label").hide();
                            var resultdata = [{ label: "No Data Availble", value: 0 }];
                        }
                        else {
                            var linechartinq;

                            if (cat == 'daily') {
                                jQuery("#" + chartid).parent().attr("class", "show");
                                jQuery("#" + chartid).empty();

                                linechartinq = new Morris.Bar({
                                    element: chartid, resize: true, redraw: true,
                                    data: resultdata,
                                    xkey: 'label',
                                    ykeys: ['d1', 'd2', 'd3', 'd4', 'd5', 'd6', 'd7', 'd8', 'd9', 'd10', 'd11', 'd12', 'd13', 'd14', 'd15', 'd16', 'd17', 'd18', 'd19', 'd20', 'd21', 'd22', 'd23', 'd24', 'd25', 'd26', 'd27', 'd28', 'd29', 'd30', 'd31'],
                                    labels: ['d1', 'd2', 'd3', 'd4', 'd5', 'd6', 'd7', 'd8', 'd9', 'd10', 'd11', 'd12', 'd13', 'd14', 'd15', 'd16', 'd17', 'd18', 'd19', 'd20', 'd21', 'd22', 'd23', 'd24', 'd25', 'd26', 'd27', 'd28', 'd29', 'd30', 'd31'],
                                    barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                    lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                    gridTextFamily: 'Open Sans', gridTextSize: 12
                                }).on('click', function (i, row) { });
                            }
                            else if (cat == 'monthly') {
                                jQuery("#" + chartid).parent().attr("class", "show");
                                jQuery("#" + chartid).empty();
                                var configa = {
                                    element: chartid, resize: true, redraw: true,
                                    data: resultdata,
                                    xkey: 'label',
                                    ykeys: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                                    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                                    barColors: ['#007bff', '#17a2b8', '#6c757d', '#c82333', '#28a745', '#ffc107'],
                                    lineWidth: 2, hideHover: 'auto', gridTextColor: 'black', gridStrokeWidth: 0.4, pointSize: 4,
                                    gridTextFamily: 'Open Sans', gridTextSize: 12
                                };
                                configa.stacked = false;
                                linechartinq = new Morris.Bar(configa).on('click', function (i, row) { });
                            }
                            // ---------------------------------------
                            if (resultdata.length > 0)
                                linechartinq.draw();
                        }

                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
        }

        // ------------------------------------------------------------------------
        // Functionality : Lead View
        // ------------------------------------------------------------------------
        function openLeadView(pCat, t11) {
            var t12, t13, t14;
            t12 = jQuery("#drpLeadMonth").val();
            t13 = jQuery("#drpLeadYear").val();
            if (jQuery("#drpLeadEmployee").val() == '' || jQuery("#drpLeadEmployee").val() == 'all')
                t14 = 'admin';
            else
                t14 = jQuery("#drpLeadEmployee").val();
            // ------------------------------------------------------------------
            var tHead = "";
            if (t11 == 'new' || t11 == 'Opportunities')
                tHead = "New Opportunities";
            else if (t11 == 'Close - Lost')
                tHead = "Lead Lost";
            else if (t11 == 'Close - Success')
                tHead = "Lead Won";
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leads - [ " + tHead + " ] ";
            // ------------------------------------------------------------------
            var pageUrl = "myCustomView.aspx?view=" + pCat + "&para1=" + t11 + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + t14;
            $('#ifrModuleLead').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openExternalLeadView(pCat, t11) {
            // string userid, string source, string acid, string cat
            var t12, t13, t14, t15;

            if (pCat == 'telecaller') {
                t12 = jQuery("#drpTeleMonth").val();
                t13 = jQuery("#drpTeleYear").val();

                t15 = jQuery("#drpTeleEmployee").val();

            }
            else {
                t12 = jQuery("#drpExtMonth").val();
                t13 = jQuery("#drpExtYear").val();

                t15 = jQuery("#drpExtEmployee").val();
            }
            // ------------------------------------------------------------------
            var tHead = "";
            if (t11 == 'new' || t11 == 'opportunities')
                tHead = "New Opportunities";
            else if (t11 == 'inprocess')
                tHead = "In Process";
            else if (t11 == 'qualified')
                tHead = "Qualified";
            else if (t11 == 'disqualified')
                tHead = "Dis-Qualified";
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "External Leads - [ " + tHead + " ] ";
            // ------------------------------------------------------------------
            var pageUrl = "myCustomView.aspx?view=externalstatus&para1=" + t11 + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + pCat + "&para5=" + t15;
            $('#ifrModuleLead').attr('src', pageUrl);
            $find("mpe").show();
        }

        // ------------------------------------------------------------
        // Footer : Quick Link
        // ------------------------------------------------------------
        function openDashboardDaily() {
            $('#ifrModuleLead').attr('src', "DashboardDaily.aspx");
            $find("mpe").show();
        }
        function openDashboardLead() {
            $('#ifrModuleLead').attr('src', "DashboardLead.aspx");
            $find("mpe").show();
        }
        function openDashboardSales() {
            $('#ifrModuleLead').attr('src', "DashboardAccount.aspx");
            $find("mpe").show();
        }
        function openDashboardSupport() {
            $('#ifrModuleLead').attr('src', "DashboardSupport.aspx");
            $find("mpe").show();
        }
        function openCalender() {
            $('#ifrModuleLead').attr('src', "myEventCalender.aspx");
            $find("mpe").show();
        }
        function openTrackEmployee() {
            spnModuleHeader.innerText = "Sales Executive - Track Visit";
            $('#ifrModuleLead').attr('src', "TrackEmployee.aspx");
            $find("mpe").show();

        }
        function openCustomerSearch() {
            spnModuleHeader.innerText = "Customer Activity Information";
            $('#ifrModuleLead').attr('src', "SearchCustomer.aspx");
            $find("mpe").show();
        }
        function openEmployeeSearch() {
            spnModuleHeader.innerText = "Employee Activity Information";
            $('#ifrModuleLead').attr('src', "SearchEmployee.aspx");
            $find("mpe").show();
        }
        function openOrganizationChart() {
            spnModuleHeader.innerText = "Organization Mapping";
            $('#ifrModuleLead').attr('src', "OrganizationChart.aspx");
            $find("mpe").show();
        }
        function openChatBox() {
            spnModuleHeader.innerText = "Chat Box";
            $('#ifrModuleLead').attr('src', "ChatBox.aspx");
            $find("mpe").show();
        }
        function openMailBox() {
            spnModuleHeader.innerText = "Mail Box";
            $('#ifrModuleLead').attr('src', "MailBox.aspx");
            $find("mpe").show();
        }
        function openLibrary() {
            spnModuleHeader.innerText = "Document Gallery";
            $('#ifrModuleLead').attr('src', "DocumentGallery.aspx");
            $find("mpe").show();
        }
    </script>
</asp:Content>
