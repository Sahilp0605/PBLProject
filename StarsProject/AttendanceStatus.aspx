<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceStatus.aspx.cs" Inherits="StarsProject.AttendanceStatus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"> 
    <title></title>
<%--    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/datatables/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://code.jquery.com/ui/1.11.2/jquery-ui.min.js" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    <style type="text/css">
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
        /* ==================================== */
        /*****         Modal Popup        *****/
        /* ==================================== */
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

        .text-input {
            position: relative;
            margin-top: 50px;
            input [type="text"];
        }

        input[type="text"] + label {
            position: absolute;
            top: 0;
            left: 0;
            bottom: 0;
            height: 40px;
            line-height: 40px;
            color: white;
            border-radius: 3px 0 0 3px;
            padding: 0 20px;
            background: #E03616;
            transform: translateZ(0) translateX(0);
            transition: all 0.3s ease-in;
            transition-delay: 0.2s;
        }

        input[type="text"]:focus + label {
            transform: translateY(-120%) translateX(0%);
            border-radius: 3px;
            transition: all 0.1s ease-out;
        }

        input[type="text"]:focus {
            padding: 10px;
            transition: all 0.3s ease-out;
            transition-delay: 0.2s;
        }

        /* =================================================================================== */
        .row.heading h2 {
            color: #fff;
            font-size: 52.52px;
            line-height: 95px;
            font-weight: 400;
            text-align: center;
            margin: 0 0 40px;
            padding-bottom: 20px;
            text-transform: uppercase;
        }

        ul {
            margin: 0;
            padding: 0;
            list-style: none;
        }

        .heading.heading-icon {
            display: block;
        }

        .padding-lg {
            display: block;
            padding-top: 20px;
            padding-bottom: 20px;
        }

        .practice-area.padding-lg {
            padding-bottom: 55px;
            padding-top: 55px;
        }

        .practice-area .inner {
            border: 1px solid #999999;
            text-align: center;
            margin-bottom: 28px;
            padding: 40px 25px;
        }

        .our-webcoderskull .cnt-block:hover {
            box-shadow: 0px 0px 10px rgba(0,0,0,0.3);
            border: 0;
        }

        .practice-area .inner h3 {
            color: #3c3c3c;
            font-size: 24px;
            font-weight: 500;
            font-family: 'Poppins', sans-serif;
            padding: 10px 0;
        }

        .practice-area .inner p {
            font-size: 14px;
            line-height: 22px;
            font-weight: 400;
        }

        .practice-area .inner img {
            display: inline-block;
        }


        .our-webcoderskull {
            /*background: url("http://www.webcoderskull.com/img/right-sider-banner.png") no-repeat center top / cover;*/
            background-color: #e8e8e8;
        }

        .our-webcoderskull .cnt-block {
            float: left;
            width: 100%;
            background: #fff;
            padding: 30px 20px;
            text-align: center;
            border: 2px solid #d5d5d5;
            margin: 0 0 28px;
        }

        .our-webcoderskull .cnt-block figure {
            width: 80px;
            height: 80px;
            border-radius: 100%;
            display: inline-block;
            margin-bottom: 15px;
        }

        .our-webcoderskull .cnt-block img {
            width: 80px;
            height: 80px;
            border-radius: 100%;
        }

        .our-webcoderskull .cnt-block h3 {
            color: #2a2a2a;
            font-size: 20px;
            font-weight: 500;
            padding: 6px 0;
            text-transform: uppercase;
        }

        .our-webcoderskull .cnt-block h3 a {
            text-decoration: none;
            color: #2a2a2a;
        }

        .our-webcoderskull .cnt-block h3 a:hover {
            color: #337ab7;
        }

        .our-webcoderskull .cnt-block p {
            color: #2a2a2a;
            font-size: 13px;
            line-height: 20px;
            font-weight: 400;
        }

        .our-webcoderskull .cnt-block .follow-us {
            margin: 20px 0 0;
        }

        .our-webcoderskull .cnt-block .follow-us li {
            display: inline-block;
            width: auto;
            margin: 0 5px;
        }

        .our-webcoderskull .cnt-block .follow-us li .fa {
            font-size: 24px;
            color: #767676;
        }

        .our-webcoderskull .cnt-block .follow-us li .fa:hover {
            color: #025a8e;
        }

        .flip-card {
            background-color: transparent;
            width: 300px;
            height: 180px;
            margin-bottom: 15px;
            perspective: 1000px;
        }

        .flip-card-inner {
            position: relative;
            width: 100%;
            height: 100%;
            text-align: center;
            transition: transform 0.6s;
            transform-style: preserve-3d;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
        }

        .flip-card:hover .flip-card-inner {
            transform: rotateY(180deg);
        }

        .flip-card-front, .flip-card-back {
            position: absolute;
            width: 100%;
            height: 100%;
            backface-visibility: hidden;
        }

        .flip-card-front {
            background-color: white;
            color: black;
            padding-top: 10px;
            border-radius: 4px;
        }

        .flip-card-back {
            background-color: #2980b9;
            color: white;
            transform: rotateY(180deg);
            border-radius: 4px;
        }

        .nav-tabs .nav-item.show .nav-link, .nav-tabs .nav-link.active {
            color: white;
            background-color: navy;
            border-color: #dee2e6 #dee2e6 #fff;
        }

        th {
            color: #e6e6f1 !important;
            background-color: #4a69d8 !important;
        }
        .nav-tabs {
            border-bottom: 1px solid #234b73;
        }
        .nav-link {
            padding: .4rem .5rem;
        }
    </style>

    <script type="text/javascript">
        function viewInquiry(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "InquiryInfo.aspx?mode=view&id=" + id, onClosed: function () { } });
        }

        function viewSalesOrder(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "SalesOrder.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewOutward(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "Outward.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewSalesBill(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "SalesBill.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewPurchaseOrder(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "PurchaseOrders.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewInward(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "Inward.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewPurchaseBill(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "PurchaseBill.aspx?mode=view&id=" + id, onClosed: function () { } });
            //var pageUrl = "PurchaseBill.aspx?mode=view&id=" + id;
            //$('#ifrSearchCust').attr('src', pageUrl);
            //$find("mpe").show();
        }

        function viewQuotation(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "Quotation.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewFollowup(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "Followup.aspx?mode=view&id=" + id, onClosed: function () { } });
        }

        function viewLedgerDocument(id, cat) {
            if (cat == 'SalesOrder') {
                $.colorbox({ width: "98%", height: "95%", iframe: true, href: "SalesOrder.aspx?mode=view&id=" + id, onClosed: function () { } });
            }
            else {
                $.colorbox({ width: "98%", height: "95%", iframe: true, href: "FinancialTrans.aspx?mode=view&category=" + cat + "&id=" + id, onClosed: function () { } });
            }
        }
        function openContFollowup(mode, pkid) {
            var pageUrl = "Followup.aspx?mode=" + mode + "&id=" + pkid
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: pageUrl, onClosed: function () { } });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" />

        <div class="row mb-1 mt-1 w-100 pl-0 pr-0">
            <div class="col m12 pl-0 pr-0">
                <h5 style="min-height: 35px; padding-top: 5px;" class="card-title mb-0 left-align width-100 gradient-45deg-light-blue-indigo">
                    &nbsp;&nbsp;Employee Shift/Attendance Information
                </h5>
            </div>
        </div>
        <div class="row m-0">
            <div class="input-field col m3">
                <label class="active text-dark" for="lblEmployeeName" style="font-size: 24px;">Employee Name</label>
                <h5 style="margin-top:15px; margin-left:3px; color:blue;">
                    <asp:Label ID="lblEmployeeName" runat="server" ClientIDMode="Static" class="form-control mb-0" TabIndex="3" Style="width: 500px;" />
                </h5>
            </div>
            <div class="input-field col m2">
                <label class="active text-dark" for="lblShift" style="font-size: 24px;">Shift</label>
                <h5 style="margin-top:15px; margin-left:3px; color:blue;">
                    <asp:Label ID="lblShift" runat="server" ClientIDMode="Static" class="form-control mb-0" TabIndex="3"/>
                </h5>
            </div>
            <div class="input-field col m2">
                <label class="active text-dark" for="lblShiftMinHrs" style="font-size: 24px;">Shift Min.Hrs</label>
                <h5 style="margin-top:15px; margin-left:3px; color:blue;">
                    <asp:Label ID="lblShiftMinHrs" runat="server" ClientIDMode="Static" class="form-control mb-0" TabIndex="3"/>
                </h5>
            </div>
            <div class="input-field col m2">
                <label class="active text-dark" for="lblShiftMaxHrs" style="font-size: 24px;">Shift Max.Hrs</label>
                <h5 style="margin-top:15px; margin-left:3px; color:blue;">
                    <asp:Label ID="lblShiftMaxHrs" runat="server" ClientIDMode="Static" class="form-control mb-0" TabIndex="3"/>
                </h5>
            </div>
            <div class="input-field col m2">
                <label class="active text-dark" for="lblGraceMins" style="font-size: 24px;">Lunch Time(Mins)</label>
                <h5 style="margin-top:15px; margin-left:3px; color:blue;">
                    <asp:Label ID="lblGraceMins" runat="server" ClientIDMode="Static" class="form-control mb-0" TabIndex="3"/>
                </h5>
            </div>
        </div>
        <div class="row">
            <div class="col m3">
                <div class="card user-statistics-card cardRound mb-1">
                    <div class="card-content p-0">
                        <h5 class="h5Round">
                            <span id="spnPageHeader" runat="server" clientidmode="Static" class="titleHead text-top">Attendance Summary</span>
                        </h5>
                        <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                            <div class="row">
                                <div class="col m12 mt-3 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                    <div class="col m6">
                                        <h5 class="description-header"><span class="description-text">Presence</span></h5>
                                    </div>
                                    <div class="col m6">
                                        <h5><asp:Label CssClass="blue-text" ID="lblPres" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                    </div>
                                </div>
                                <div class="col m12 mt-3 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                    <div class="col m6">
                                        <h5 class="description-header"><span class="description-text">Full Day</span></h5>
                                    </div>
                                    <div class="col m6">
                                        <h5><asp:Label CssClass="blue-text" ID="lblFull" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                    </div>
                                </div>
                                <div class="col m12 mt-3 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                    <div class="col m6">
                                        <h5 class="description-header"><span class="description-text">Half Day</span></h5>
                                    </div>
                                    <div class="col m6">
                                        <h5><asp:Label CssClass="blue-text" ID="lblHalf" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-1">
                                <div class="col m12 mt-3 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                    <div class="col m6">
                                        <h5 class="description-header"><span class="description-text">Shift Total Hrs</span></h5>
                                    </div>
                                    <div class="col m6">
                                        <h5><asp:Label CssClass="blue-text" ID="lblTotalShiftHrs" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                    </div>
                                </div>
                                <div class="col m12 mt-3 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                    <div class="col m6">
                                        <h5 class="description-header"><span class="description-text">Total Hrs (Worked)</span></h5>
                                    </div>
                                    <div class="col m6">
                                        <h5><asp:Label CssClass="blue-text" ID="lblTotalWKHrs" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                    </div>
                                </div>
                                <div class="col m12 mt-3 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                    <div class="col m6">
                                        <h5 class="description-header"><span class="description-text">Overtime Hrs</span></h5>
                                    </div>
                                    <div class="col m6">
                                        <h5><asp:Label CssClass="blue-text" ID="lblTotalOTHrs" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col m9">
                <div class="card user-statistics-card cardRound mb-1">
                    <div class="card-content p-0">
                        <h5 class="h5Round">
                            <span id="Span1" runat="server" clientidmode="Static" class="titleHead text-top">Attendance Log</span>
                        </h5>
                        <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                            <div class="row">
                                <div class="input-field col m12">
                                    <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="98%">
                                        <asp:Repeater ID="rptAttendaceStatus" runat="server" ClientIDMode="Static" OnItemDataBound="rptAttendaceStatus_ItemDataBound">
                                            <HeaderTemplate>
                                                <tr>
                                                    <th class="center-align">Attendance Date</th>
                                                    <th class="center-align">Time IN</th>
                                                    <th class="center-align">Time OUT</th>
                                                    <th class="center-align">Working Hrs</th>
                                                    <th class="center-align">OT Hrs</th>
                                                    <th class="center-align">Working Status</th>
                                                    <th class="center-align">Day Status</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" Value='<%# Eval("EmployeeID") %>' />
                                                <asp:HiddenField ID="hdnWorkingTotalHrs" runat="server" ClientIDMode="Static" Value='<%# Eval("WorkingTotalHrs") %>' />
                                                <asp:HiddenField ID="hdnWorkingHrsFlag" runat="server" ClientIDMode="Static" Value='<%# Eval("WorkingHrsFlag") %>' />
                                                <asp:HiddenField ID="hdnDayStatus" runat="server" ClientIDMode="Static" Value='<%# Eval("DayStatus") %>' />
                                                <tr id="trItem" runat="server" class="blueShed">
                                                    <td class="center-align"><%# Eval("PresenceDate", "{0:dd/MM/yyyy}") %></td>
                                                    <td class="center-align"><%# Eval("TimeIn") %></td>
                                                    <td class="center-align"><%# Eval("TimeOut") %></td>
                                                    <td class="center-align"><%# Eval("WorkingTotalHrs") %></td>
                                                    <td class="center-align"><%# Eval("OTHrs") %></td>
                                                    <td id="tdWorkingHrsFlag" runat="server" class="center-align"><%# Eval("WorkingHrsFlag") %></td>
                                                    <td id="tdDayStatus" runat="server" class="center-align"><%# Eval("DayStatus") %></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <br />
        <%-- PopUp Modal Box to Open Window --%>
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="pnlSearchCust" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlSearchCust" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
            <div id="myModal123">
                <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                    <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                        <i class="material-icons prefix">ac_unit</i>
                        <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                        <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                    </h5>
                </div>
            </div>
            <iframe id="ifrSearchCust" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
            <br />
        </asp:Panel>

        <script type="text/javascript">
            $("input").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    //$("form").submit();
                }
            });
            // -------------------------------------------------------------
            function ShowPDFfile(repFilename) {
                yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
                yhooWin.focus();
            }
        </script>
    </form>
</body>
</html>
