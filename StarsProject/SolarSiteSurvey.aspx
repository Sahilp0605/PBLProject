<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="SolarSiteSurvey.aspx.cs" Inherits="StarsProject.SolarSiteSurvey" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <style type="text/css">
        /* ==================================== */
        /*****         Modal Popup        *****/
        /* ==================================== */

        .card-title {
            background: linear-gradient(45deg, #303f9f, #7b1fa2) !important;
        }
        .jqte { margin:0px; }
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
        .ui-autocomplete-input
        { 
            width: 600px;
        }

        .autocomplete-suggestions
        {
             min-width: 600px !important;
        }
        a:hover { cursor:pointer; }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            modeOfSelection();

            $('.tabs').tabs();

            jQuery(".jqte-test").jqte({
                blur: function () { }
            });
            // settings of status
            var jqteStatus = true;
        });

        function pageLoad(sender, args) {
            retainTabPosition();
            retainTabPosition2();
            retainTabPosition3();
        }

        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }

        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function showErrorPopup(xTitle, xMsg) {
            if (xTitle == '')
                xTitle = 'Message';
            // -------------------------------------
            if (xMsg != '') {
                jQuery.confirm({
                    title: xTitle, content: xMsg, type: 'red', typeAnimated: true, width: 'auto',
                    buttons: { close: { text: 'Close', btnClass: 'btn-blue ConfirmClose', action: function () { } } }
                });
            }
        }

        function modeOfSelection() {
            $('#drpModeOfSelection').change(function () {
                if ($('#drpModeOfSelection').val().toLowerCase() == 'recipient') {
                    $('#txtSender').insertAfter($('#txtRecipient'));
                }
                else {
                    $('#txtRecipient').insertAfter($('#txtSender'));
                }
            }).trigger('change');
        }
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=" + cat + "&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        $(function () {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "sitephoto";
            $("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "sitephoto";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab").val(currTab);
            });
        });

        function retainTabPosition() {
            $('#lnksitephoto').removeClass('active');
            $('#lnksitevideo').removeClass('active');
            $('#lnktransnameplate').removeClass('active');
            $('#lnksitesketchwithobject').removeClass('active');
            $('#lnkomrequirements').removeClass('active');
            $('#lnkmodulecleaningrequirements').removeClass('active');
            $('#lnkroofplan').removeClass('active');
            $('#lnkloaddetails').removeClass('active');
            $('#lnkelectricitybill').removeClass('active');
            $('#lnkearthresistive').removeClass('active');
            $('#lnkearthpil').removeClass('active');
            $('#lnkdistancefromelectricroom').removeClass('active');
            

            $('#sitephoto').css("display", "none");
            $('#sitevideo').css("display", "none");
            $('#transnameplate').css("display", "none");
            $('#sitesketchwithobject').css("display", "none");
            $('#omrequirements').css("display", "none");
            $('#modulecleaningrequirements').css("display", "none");
            $('#roofplan').css("display", "none");
            $('#loaddetails').css("display", "none");
            $('#electricitybill').css("display", "none");
            $('#earthresistive').css("display", "none");
            $('#earthpit').css("display", "none");
            $('#distancefromelectricroom').css("display", "none");
            // --------------------------------------------------- 
            if ($("#hdnCurrentTab").val() == 'sitephoto') {
                $('#lnksitephoto').addClass('active');
                $('#sitephoto').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'sitevideo') {
                $('#lnksitevideo').addClass('active');
                $('#sitevideo').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'transnameplate') {
                $('#lnktransnameplate').addClass('active');
                $('#transnameplate').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'sitesketchwithobject') {
                $('#lnksitesketchwithobject').addClass('active');
                $('#sitesketchwithobject').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'omrequirements') {
                $('#lnkomrequirements').addClass('active');
                $('#omrequirements').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'modulecleaningrequirements') {
                $('#lnkmodulecleaningrequirements').addClass('active');
                $('#modulecleaningrequirements').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'roofplan') {
                $('#lnkroofplan').addClass('active');
                $('#roofplan').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'loaddetails') {
                $('#lnkloaddetails').addClass('active');
                $('#loaddetails').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'electricitybill') {
                $('#lnkelectricitybill').addClass('active');
                $('#electricitybill').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'earthresistive') {
                $('#lnkearthresistive').addClass('active');
                $('#earthresistive').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'earthpit') {
                $('#lnkearthpit').addClass('active');
                $('#earthpit').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'distancefromelectricroom') {
                $('#lnkdistancefromelectricroom').addClass('active');
                $('#distancefromelectricroom').css("display", "block");
            }
        }
        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        $(function () {
            var tabName = $("[id*=hdnCurrentTab2]").val() != "" ? $("[id*=hdnCurrentTab2]").val() : "sheettype";
            $("#myTab2 a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab2]").val() != "" ? $("[id*=hdnCurrentTab2]").val() : "sheettype";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab2").val(currTab);
            });
        });

        function retainTabPosition2() {
            $('#lnksheettype').removeClass('active');
            $('#lnkpurlindistance').removeClass('active');
            $('#lnkroofsheet').removeClass('active');
            $('#lnkstructurestability').removeClass('active');
            $('#lnskylight').removeClass('active');
            $('#lnkladdertoroof').removeClass('active');

            $('#sheettype').css("display", "none");
            $('#purlindistance').css("display", "none");
            $('#roofsheet').css("display", "none");
            $('#structurestability').css("display", "none");
            $('#skylight').css("display", "none");
            $('#laddertoroof').css("display", "none");
           
            // --------------------------------------------------- 
            if ($("#hdnCurrentTab2").val() == 'sheettype') {
                $('#lnksheettype').addClass('active');
                $('#sheettype').css("display", "block");
            }
            if ($("#hdnCurrentTab2").val() == 'purlindistance') {
                $('#lnkpurlindistance').addClass('active');
                $('#purlindistance').css("display", "block");
            }
            if ($("#hdnCurrentTab2").val() == 'roofsheet') {
                $('#lnkroofsheet').addClass('active');
                $('#roofsheet').css("display", "block");
            }
            if ($("#hdnCurrentTab2").val() == 'structurestability') {
                $('#lnkstructurestability').addClass('active');
                $('#structurestability').css("display", "block");
            }
            if ($("#hdnCurrentTab2").val() == 'skylight') {
                $('#lnskylight').addClass('active');
                $('#skylight').css("display", "block");
            }
            if ($("#hdnCurrentTab2").val() == 'laddertoroof') {
                $('#lnkladdertoroof').addClass('active');
                $('#laddertoroof').css("display", "block");
            }
        }
        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        $(function () {
            var tabName = $("[id*=hdnCurrentTab3]").val() != "" ? $("[id*=hdnCurrentTab3]").val() : "soiltest";
            $("#myTab3 a").click(function () {
                debugger;
                var prevTab = $("[id*=hdnCurrentTab3]").val() != "" ? $("[id*=hdnCurrentTab3]").val() : "soiltest";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab3").val(currTab);
            });
        });

        function retainTabPosition3() {
            $('#lnksoiltest').removeClass('active');
            $('#lnkcontoursurvey').removeClass('active');
            $('#lnktilt').removeClass('active');
            $('#lnkinverter').removeClass('active');
            

            $('#soiltest').css("display", "none");
            $('#contoursurvey').css("display", "none");
            $('#tilt').css("display", "none");
            $('#inverter').css("display", "none");
           
            // --------------------------------------------------- 
            if ($("#hdnCurrentTab3").val() == 'soiltest') {
                $('#lnksoiltest').addClass('active');
                $('#soiltest').css("display", "block");
            }
            if ($("#hdnCurrentTab3").val() == 'contoursurvey') {
                $('#lnkcontoursurvey').addClass('active');
                $('#contoursurvey').css("display", "block");
            }
            if ($("#hdnCurrentTab3").val() == 'tilt') {
                $('#lnktilt').addClass('active');
                $('#tilt').css("display", "block");
            }
            if ($("#hdnCurrentTab3").val() == 'inverter') {
                $('#lnkinverter').addClass('active');
                $('#inverter').css("display", "block");
            }
        }
        function ShowPDFfile(repFilename) {
            repFilename =  "SiteDocs/" + repFilename;
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
                <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" Value="sitephoto" EnableViewState="true" />
                <asp:HiddenField ID="hdnCurrentTab2" runat="server" ClientIDMode="Static" Value="sheettype" EnableViewState="true" />
                <asp:HiddenField ID="hdnCurrentTab3" runat="server" ClientIDMode="Static" Value="soiltest" EnableViewState="true" />
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnPostedFile" runat="server" ClientIDMode="Static" />

                <div id="myModal" style="display: block; width: 98%; overflow-x: hidden;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row badge-light" style="border-radius: 5px;">
                                <div class="input-field col m2">
                                    <label class="active" for="txtDocNo">Document No. <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:TextBox ID="txtDocNo" runat="server" class="form-control" MaxLength="25" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder=" " />
                                </div>
                                <div class="input-field col m2">
                                    <label class="active" for="txtSheetNo">Sheet No. <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:TextBox ID="txtSheetNo" runat="server" class="form-control" MaxLength="25" ClientIDMode="Static" TabIndex="2" placeholder=" " />
                                </div>
                                <div class="input-field col m3">
                                    <label class="active" for="txtSurveyDate">Survey Date <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:TextBox ID="txtSurveyDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" onkeypress="return false;" onpaste="return false" placeholder=" " TextMode="Date" />
                                </div>
                                <div class="input-field col m5">
                                    <label class="active" for="txtCustomerName">Name of the Customer <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" placeholder="Minimum 3 chars to activate search" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="4" style="margin-top:3px"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m12">
                                    <div class="card" style="height: 380px;">
                                        <div class="card-title" style="padding: 10px;">
                                            <h5 class="card-title mb-0" style="color: white">Site Information & Customer Detail</h5>
                                        </div>
                                        <div class="card-body">
                                            <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtContPerson1">Name of Contact Person (1) </label>
                                                    <asp:TextBox ID="txtContPerson1" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="5" placeholder=" " />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtContNo1">Contact Number (1) </label>
                                                    <asp:TextBox ID="txtContNo1" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="6" placeholder=" " />
                                                </div>
                                                <div class="input-field col m4">
                                                    <label class="active" for="txtContAddress1">Contact Address </label>
                                                    <asp:TextBox ID="txtContAddress1" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" " />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtContEmail1">Email Address </label>
                                                    <asp:TextBox ID="txtContEmail1" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="8" placeholder=" " />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtContDesignation11">Designation </label>
                                                    <asp:TextBox ID="txtContDesignation1" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="9" placeholder=" " />
                                                </div>
                                            </div>
                                            <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtContPerson2">Name of Contact Person (2) </label>
                                                    <asp:TextBox ID="txtContPerson2" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="10" placeholder=" " />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtContNo2">Contact Number (2) </label>
                                                    <asp:TextBox ID="txtContNo2" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="11" placeholder=" " />
                                                </div>
                                                <div class="input-field col m4">
                                                    <label class="active" for="txtContAddress2">Contact Address </label>
                                                    <asp:TextBox ID="txtContAddress2" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="12" placeholder=" " />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtContEmail2">Email Address </label>
                                                    <asp:TextBox ID="txtContEmail2" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="13" placeholder=" " />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtContDesignation12">Designation </label>
                                                    <asp:TextBox ID="txtContDesignation2" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="14" placeholder=" " />
                                                </div>
                                            </div>
                                            <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtSiteAddress">Full Site Address </label>
                                                    <asp:TextBox ID="txtSiteAddress" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="15" placeholder=" "  TextMode="MultiLine"/>
                                                </div>
                                                <div class="input-field col m6">
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtLatitude">Latitude </label>
                                                        <asp:TextBox ID="txtLatitude" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="16" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtLongitude">Longitude </label>
                                                        <asp:TextBox ID="txtLongitude" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="17" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtAltitude">Altitude </label>
                                                        <asp:TextBox ID="txtAltitude" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="18" placeholder=" " />
                                                    </div>
                                                </div>
                                                <div class="input-field col m6">
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtNearByRailwayStation">Nearest Railway Station </label>
                                                        <asp:TextBox ID="txtNearByRailwayStation" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="19" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtNearByAirport">Nearest Airport </label>
                                                        <asp:TextBox ID="txtNearByAirport" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="20" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtWaterElectricity">Avail. Of Water & Electricity </label>
                                                        <asp:TextBox ID="txtWaterElectricity" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="21" placeholder=" " />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m12">
                                    <div class="card" style="height: 320px;">
                                        <div class="card-title" style="padding: 10px;">
                                            <h5 class="card-title mb-0" style="color: white">Civil / Mechanical Detail</h5>
                                        </div>
                                        <div class="card-body">
                                            <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="col m12">
                                                            <table id="tblCivilMech" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <thead>
                                                                    <th class="center-align">Mounting Type</th>
                                                                    <th class="center-align">Name of Location</th>
                                                                    <th class="center-align">Tilt Angle</th>
                                                                    <th class="center-align">Available Area</th>
                                                                    <th class="center-align"> </th>
                                                                    <th class="center-align">Orientation</th>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td class="width-25">
                                                                            <label class="active">Rooftop-RCC </label></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtRoofTopRCCLocation" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="22" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <asp:TextBox ID="txtRoofTopRCCTildAngle" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="23" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <asp:TextBox ID="txtRoofTopRCCArea" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="24" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <label class="active">Sq M</label></td>
                                                                        <td class="width-20">
                                                                            <asp:TextBox ID="txtRoofTopRCCOrientation" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="25" Style="text-align: left;" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="width-25">
                                                                            <label class="active">Rooftop-Metal Sheet </label></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtRoofTopMetalSheetLocation" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="26" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <asp:TextBox ID="txtRoofTopMetalSheetTildAngle" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="27" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <asp:TextBox ID="txtRoofTopMetalSheetArea" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="28" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <label class="active">Sq M</label></td>
                                                                        <td class="width-20">
                                                                            <asp:TextBox ID="txtRoofTopMetalSheetOrientation" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="29" Style="text-align: left ;" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="width-25">
                                                                            <label class="active">Ground Mount </label></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtGroundMountLocation" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="30" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <asp:TextBox ID="txtGroundMountTildAngle" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="31" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <asp:TextBox ID="txtGroundMountArea" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="32" Style="text-align: left;" /></td>
                                                                        <td class="width-10">
                                                                            <label class="active">Sq M</label></td>
                                                                        <td class="width-20">
                                                                            <asp:TextBox ID="txtGroundMountOrientation" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="33" Style="text-align: left;" /></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>  
                                            <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtStructureType">Structure Type </label>
                                                    <asp:TextBox ID="txtStructureType" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="34" placeholder=" " />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtPenetrationAllowed">Penetration Allowed </label>
                                                    <asp:TextBox ID="txtPenetrationAllowed" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="35" placeholder=" " />
                                                </div>
                                            </div> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m12">
                                    <div class="card" style="height: 430px;">
                                        <div class="card-title" style="padding: 10px;">
                                            <h5 class="card-title mb-0" style="color: white">Elecrical Detail</h5>
                                        </div>
                                        <div class="card-body">
                                            <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="col m12">
                                                            <table id="tblElectrical" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <thead>
                                                                    <th class="center-align">Plant Type</th>
                                                                    <th class="center-align">DG Rating & Numbers</th>
                                                                    <th class="center-align">Cotract Demand</th>
                                                                    <th class="center-align">Installation Capacity (KW)</th>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td class="width-25">
                                                                            <label class="active">On-Grid </label></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtOnGridDGRating" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="36" Style="text-align: left;" /></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtOnGridContractDemand" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="37" Style="text-align: left;" /></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtOnGridInstallationCapacity" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="38" Style="text-align: left;" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="width-25">
                                                                            <label class="active">Off-Grid </label></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtOffGridDGRating" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="39" Style="text-align: left;" /></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtOffGridContractDemand" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="40" Style="text-align: left;" /></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtOffGridInstallationCapacity" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="41" Style="text-align: left;" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="width-25">
                                                                            <label class="active">Hybrid </label></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtHybridDGRating" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="42" Style="text-align: left;" /></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtHybridContractDemand" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="43" Style="text-align: left;" /></td>
                                                                        <td class="width-25">
                                                                            <asp:TextBox ID="txtHybridInstallationCapacity" MaxLength="100" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="44" Style="text-align: left;" /></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>  
                                            <div class="input-field col m6">
                                                <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtInstallationType">Installation Type </label>
                                                        <asp:TextBox ID="txtInstallationType" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="45" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtDGSynchronisation">DG Synchronisation </label>
                                                        <asp:TextBox ID="txtDGSynchronisation" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="46" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtDGOperationMode">DG Operation Mode </label>
                                                        <asp:TextBox ID="txtDGOperationMode" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="47" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtDataMonitoring">Data Monitoring </label>
                                                        <asp:TextBox ID="txtDataMonitoring" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="48" placeholder=" " />
                                                    </div>
                                                </div>
                                                <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtWeatherMonitoringSystem">Weather Monitoring System </label>
                                                        <asp:TextBox ID="txtWeatherMonitoringSystem" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="49" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtBreaker">Avail. Breaker to Feed Solar </label>
                                                        <asp:TextBox ID="txtBreaker" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="50" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtBusBarTypeSize">Bus bar type & size </label>
                                                        <asp:TextBox ID="txtBusBarTypeSize" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="51" placeholder=" " />
                                                    </div>
                                                </div> 
                                            </div>
                                            <div class="input-field col m6">
                                                <div class="card" style="height: 170px;">
                                                    <div class="card-title" style="padding: 10px;">
                                                        <h5 class="card-title mb-0" style="color: white">Transformer Detail</h5>
                                                    </div>
                                                    <div class="row" style="padding: 5px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m4">
                                                            <label class="active" for="txtKVARating">KVA Rating </label>
                                                            <asp:TextBox ID="txtKVARating" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="52" placeholder=" " />
                                                        </div>
                                                        <div class="input-field col m4">
                                                            <label class="active" for="txtPrimaryVoltage">Primary Voltage </label>
                                                            <asp:TextBox ID="txtPrimaryVoltage" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="53" placeholder=" " />
                                                        </div>
                                                        <div class="input-field col m4">
                                                            <label class="active" for="txtSecondaryVoltage">Secondary Voltage </label>
                                                            <asp:TextBox ID="txtSecondaryVoltage" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="54" placeholder=" " />
                                                        </div>
                                                    </div>
                                                    <div class="row" style="padding: 0px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtImpedance">%Z (Impedance) </label>
                                                            <asp:TextBox ID="txtImpedance" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="55" placeholder=" " />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtVectorGrp">Vector Group </label>
                                                            <asp:TextBox ID="txtVectorGrp" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="56" placeholder=" " />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divOther" runat="server" class="col m12 padding-1" style="min-height:482px; box-shadow: navy -6px 8px 6px -6px;">
                                <div class="row">
                                    <div class="card user-card-negative-margin z-depth-0" id="feed" style="margin:0px;">
                                        <div class="card-content card-border-gray" style="padding-top: 0px;">
                                            <div class="card-title" style="padding: 10px;">
                                                <h5 class="card-title mb-0" style="color: white">Common Required Inputs</h5>
                                            </div>
                                            <div class="row">
                                                <ul id="myTab" class="tabs card-border-gray" runat="server" clientidmode="Static" enableviewstate="true">
                                                    <li class="tab col p-0"><a id="lnksitephoto" class="left active" style="padding:0 15px;" href="#sitephoto">Site Photographs </a></li>
                                                    <li class="tab col p-0"><a id="lnksitevideo" class="left" style="padding:0 15px;" href="#sitevideo">Site Video </a></li>
                                                    <li class="tab col p-0"><a id="lnktransnameplate" class="left" style="padding:0 15px;" href="#transnameplate">Transformer's NamePlate </a></li>
                                                    <li class="tab col p-0"><a id="lnksitesketchwithobject" class="left" style="padding:0 15px;" href="#sitesketchwithobject">Site Sketch with Object </a></li>
                                                    <li class="tab col p-0"><a id="lnkomrequirements" class="left" style="padding:0 15px;" href="#omrequirements">O&M Requirements </a></li>
                                                    <li class="tab col p-0"><a id="lnkmodulecleaningrequirements" class="left" style="padding:0 15px;" href="#modulecleaningrequirements">Module Clearing Requirements </a></li>
                                                    <li class="tab col p-0"><a id="lnkroofplan" class="left" style="padding:0 15px;" href="#roofplan">Roof Plan,Elevation,SLD </a></li>
                                                    <li class="tab col p-0"><a id="lnkloaddetails" class="left" style="padding:0 15px;" href="#loaddetails">Load Details </a></li>
                                                    <li class="tab col p-0"><a id="lnkelectricitybill" class="left" style="padding:0 15px;" href="#electricitybill">Electricity Bill </a></li>
                                                    <li class="tab col p-0"><a id="lnkearthresistive" class="left" style="padding:0 15px;" href="#earthresistive">Earth Resistivity Test </a></li>
                                                    <li class="tab col p-0"><a id="lnkearthpit" class="left" style="padding:0 15px;" href="#earthpit">Earth Pit,Inverter,ACCB Location </a></li>
                                                    <li class="tab col p-0"><a id="lnkdistancefromelectricroom" class="left" style="padding:0 15px;" href="#distancefromelectricroom">Distance From Electric Room </a></li>
                                                </ul>
                                            </div>
                                            <div id="sitephoto" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                                <asp:UpdatePanel ID="pnlSitePhoto" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="uploadDocument">Upload Site Photos Here  </label>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="btnUploadDoc" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptSitePhoto" runat="server" ClientIDMode="Static" OnItemCommand="rptSitePhoto_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="sitevideo" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlSiteVideo" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="uploadVideo">Upload Site Video Here  </label>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="uploadVideo" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only MP4 File" /> 
                                                                        <asp:Button ID="Button3" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptSiteVideo" runat="server" ClientIDMode="Static" OnItemCommand="rptSiteVideo_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowMP4file('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <%--<img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">--%>
                                                                                    <video class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;" type="video/mp4">
                                                                                    <%--<video width="130" height="130" controls>  
                                                                                        <source src='<%#Eval("FileName")%>' type="video/mp4">  
                                                                                    </video>--%>
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                                <%--<label>Choose Video:</label>  
                                                <div class="input-group">  
                                                    <div class="custom-file">  
                                                        <asp:FileUpload ID="uploadVideo" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only mp4 File" />   
                                                        <label class="custom-file-label"></label>  
                                                    </div>  
                                                    <div class="input-group-append">  
                                                        <asp:Button ID="btnUpload" CssClass="btn btn-outline-secondary" runat="server" Text="Upload" OnClick="btnUploadDoc_Click" />  
                                                    </div>  
                                                </div>
                                                <asp:Label ID="lblMessage" runat="server"></asp:Label>  
                                                <asp:GridView ID="GridView1" ShowHeaderWhenEmpty="true" HeaderStyle-CssClass="bg-primary text-white" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">  
                                                    <EmptyDataTemplate>  
                                                        <div class="text-center">No Data Found <strong>Upload New Video</strong></div>  
                                                    </EmptyDataTemplate>  
                                                    <Columns>  
                                                        <asp:BoundField HeaderText="ID" DataField="ID" />  
                                                        <asp:BoundField HeaderText="Name" DataField="Name" />  
                                                        <asp:TemplateField HeaderText="Videos">  
                                                            <ItemTemplate>  
                                                                <video width="130" height="130" controls>  
                                                                    <source src='<%#Eval("FileName")%>' type="video/mp4">  
                                                                </video>  
                                                            </ItemTemplate>  
                                                        </asp:TemplateField>  
                                                    </Columns>  
                                                </asp:GridView>   --%>
                                            </div>
                                            <div id="transnameplate" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlTransNamePlate" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="UploadTransNamePlate">Upload Transformer's Name Plate Here  </label>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadTransNamePlate" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button4" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptTransNamePlate" runat="server" ClientIDMode="Static" OnItemCommand="rptTransNamePlate_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="sitesketchwithobject" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlSiteSketchWithObject" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="UploadSiteSketchWithObject">Upload Site Sketch With Object Here  </label>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadSiteSketchWithObject" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button5" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptSiteSketchWithObject" runat="server" ClientIDMode="Static" OnItemCommand="rptSiteSketchWithObject_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="omrequirements" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlOMRequirements" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtOMRequirements">O & M Requirements </label>
                                                                        <asp:TextBox ID="txtOMRequirements" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="modulecleaningrequirements" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlModuleCleaningRequirements" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtOMRequirements">Module Cleaning Requirements </label>
                                                                        <asp:TextBox ID="txtModuleCleaningRequirements" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="roofplan" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlRoofPlan" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtRoofPlan">Roof Plan, Elevation Drawing, SLD </label>
                                                                        <asp:TextBox ID="txtRoofPlan" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadRoofPlan" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button6" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptRoofPlan" runat="server" ClientIDMode="Static" OnItemCommand="rptRoofPlan_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="loaddetails" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlLoadDetails" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtLoadDetails">Load Details </label>
                                                                        <asp:TextBox ID="txtLoadDetails" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="electricitybill" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlElectricityBill" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="UploadElectricityBill">Upload Electricity Bill Here  </label>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadElectricityBill" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button7" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptElectricityBill" runat="server" ClientIDMode="Static" OnItemCommand="rptElectricityBill_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="earthresistive" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlEarthResistive" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtEarthResistive">Earth Resistivety Test </label>
                                                                        <asp:TextBox ID="txtEarthResistive" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadEarthResistivity" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button8" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptEarthResistivity" runat="server" ClientIDMode="Static" OnItemCommand="rptEarthResistivity_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="earthpit" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlEarthPil" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtEarthPil">Earth Pil, Inverter, ACCB Location </label>
                                                                        <asp:TextBox ID="txtEarthPil" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadEarthPil" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button9" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptEarthPil" runat="server" ClientIDMode="Static" OnItemCommand="rptEarthPil_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="distancefromelectricroom" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlDistanceFromElectricRomm" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtDistanceFromElectricRoom">Distance From Electric Room </label>
                                                                        <asp:TextBox ID="txtDistanceFromElectricRoom" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divOther2" runat="server" class="col m12 padding-1" style="min-height:482px; box-shadow: navy -6px 8px 6px -6px;">
                                <div class="row">
                                    <div class="card user-card-negative-margin z-depth-0" id="feed2" style="margin:0px;">
                                        <div class="card-content card-border-gray" style="padding-top: 0px;">
                                            <div class="card-title" style="padding: 10px;">
                                                <h5 class="card-title mb-0" style="color: white">Required Inputs For Metal Sheet Roof</h5>
                                            </div> 
                                            <div class="row">
                                                <ul id="myTab2" class="tabs card-border-gray" runat="server" clientidmode="Static" enableviewstate="true">
                                                    <li class="tab col p-0"><a id="lnksheettype" class="left active" style="padding:0 15px;" href="#sheettype">Sheet Type & Structure </a></li>
                                                    <li class="tab col p-0"><a id="lnkpurlindistance" class="left" style="padding:0 15px;" href="#purlindistance">Distance Between Purlins </a></li>
                                                    <li class="tab col p-0"><a id="lnkroofsheet" class="left" style="padding:0 15px;" href="#roofsheet">Roof Sheet </a></li>
                                                    <li class="tab col p-0"><a id="lnkstructurestability" class="left" style="padding:0 15px;" href="#structurestability">Structure Stability </a></li>
                                                    <li class="tab col p-0"><a id="lnskylight" class="left" style="padding:0 15px;" href="#skylight">Skylight & Ventilator </a></li>
                                                    <li class="tab col p-0"><a id="lnkladdertoroof" class="left" style="padding:0 15px;" href="#laddertoroof">Access/Ladder To Roof </a></li>
                                                </ul>
                                            </div>
                                            <div id="sheettype" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                                <asp:UpdatePanel ID="pnlSheetType" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtSheetType">Sheet Type </label>
                                                                        <asp:TextBox ID="txtSheetType" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="purlindistance" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlPurlinDistance" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtPurlinDistance">Purlin to Purlin Distance </label>
                                                                        <asp:TextBox ID="txtPurlinDistance" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadPurlinDistance" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button14" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptPurlinDistance" runat="server" ClientIDMode="Static" OnItemCommand="rptPurlinDistance_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="roofsheet" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlRoofSheet" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtRoofSheet">Roof Sheet </label>
                                                                        <asp:TextBox ID="txtRoofSheet" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="structurestability" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlStructureStability" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtStructureStability">Structure Stability </label>
                                                                        <asp:TextBox ID="txtStructureStability" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadStructureStability" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button18" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptStructureStability" runat="server" ClientIDMode="Static" OnItemCommand="rptStructureStability_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="skylight" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlSkylight" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtSkylight">Skylight & Ventilators </label>
                                                                        <asp:TextBox ID="txtSkylight" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadSkylights" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button19" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptSkylights" runat="server" ClientIDMode="Static" OnItemCommand="rptSkylights_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="laddertoroof" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlLadderRoof" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtLadderRoof">Sheet Type </label>
                                                                        <asp:TextBox ID="txtLadderRoof" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divOther3" runat="server" class="col m12 padding-1" style="min-height:482px; box-shadow: navy -6px 8px 6px -6px;">
                                <div class="row">
                                    <div class="card user-card-negative-margin z-depth-0" id="feed3" style="margin:0px;">
                                        <div class="card-content card-border-gray" style="padding-top: 0px;">
                                            <div class="card-title" style="padding: 10px;">
                                                <h5 class="card-title mb-0" style="color: white">Required Inputs For Ground Mounts</h5>
                                            </div>
                                            <div class="row">
                                                <ul id="myTab3" class="tabs card-border-gray" runat="server" clientidmode="Static" enableviewstate="true">
                                                    <li class="tab col p-0"><a id="lnksoiltest" class="left active" style="padding:0 15px;" href="#soiltest">Soil Test Report </a></li>
                                                    <li class="tab col p-0"><a id="lnkcontoursurvey" class="left" style="padding:0 15px;" href="#contoursurvey">Contour Survey </a></li>
                                                    <li class="tab col p-0"><a id="lnktilt" class="left" style="padding:0 15px;" href="#tilt">Fix / Seasonal Tilt </a></li>
                                                    <li class="tab col p-0"><a id="lnkinverter" class="left" style="padding:0 15px;" href="#inverter">String / Central Inverter </a></li>
                                                </ul>
                                            </div>
                                            <div id="soiltest" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                                <asp:UpdatePanel ID="pnlSoilTest" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtSoilTest">Soil Test </label>
                                                                        <asp:TextBox ID="txtSoilTest" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadSoilTest" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button10" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptSoilTest" runat="server" ClientIDMode="Static" OnItemCommand="rptSoilTest_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="contoursurvey" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlContourSurvey" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtContourSurvey">Contour Survey </label>
                                                                        <asp:TextBox ID="txtContourSurvey" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="UploadContourSurvey" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                                                        <asp:Button ID="Button13" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptContourSurvey" runat="server" ClientIDMode="Static" OnItemCommand="rptContourSurvey_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="tilt" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlTilt" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtTilt">Fix / Seasonal Tilt </label>
                                                                        <asp:TextBox ID="txtTilt" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="inverter" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="pnlInverter" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="txtInverter">String / Central Inverter </label>
                                                                        <asp:TextBox ID="txtInverter" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                                
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 5px;">
                                <button id="btnReset" type="button" runat="server" clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="17"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button id="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="18"><i class="material-icons left">save</i>Save</button>
                            </div>
                        </div>
                    </div>
                </div>
        </div>

        <%-- PopUp Modal Box to Open Window --%>
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
            <div id="myModal123">
                <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                    <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                        <i class="material-icons prefix">ac_unit</i>
                        <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                        <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                    </h5>
                </div>
            </div>
            <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
            <br />
        </asp:Panel>

        <script type="text/javascript">
            function bindDDLTo(selector) {
                if ($(selector).val().length >= 3) {
                    jQuery.ajax({
                        type: "POST",
                        url: "InquiryInfo.aspx/FilterCustomerByModule",
                        data: '{pCustName:\'' + $(selector).val() + '\', pSearchModule:\'CustomerSearchAll\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            debugger;
                            console.log(data);
                            //var resultdata = JSON.parse(data.d);
                            var sample = JSON.parse(data.d);
                            $(selector).autoComplete({
                                minChars: 1,
                                source: function (term, suggest) {
                                    term = term.toLowerCase();
                                    var choices = sample;
                                    suggest(choices);
                                },
                                renderItem: function (item, search) {
                                    $(".autocomplete-suggestion").remove();
                                    //search = search.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
                                    //var re = new RegExp("(" + search.split(' ').join('|') + ")", "gi");
                                    return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                },
                                onSelect: function (e, term, item) {
                                    //console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                    $(selector).val(item.html());
                                    $("#hdnCustomerID").val(item.data('langname'));
                                    $("#drpInquiry").focus();
                                }
                            });

                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                    return false;
                }
                else {
                    if ($(selector).val().length == 0) {
                        $("#hdnCustomerID").val('');
                    }
                }
            }
        </script>
    </form>
</body>
</html>

