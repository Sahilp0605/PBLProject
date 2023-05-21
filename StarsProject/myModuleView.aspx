<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="myModuleView.aspx.cs" Inherits="StarsProject.myModuleView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myTaxSummary.ascx" TagPrefix="uc1" TagName="myTaxSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/sweetalert/sweetalert.css">

    <style type="text/css">
        div.click-to-top { display:inline-block; }
        div.click-to-top span {display: none; position: absolute; left: 0; right: 0; background: #333; color: #fff; width:100px;}

        div.click-to-top:hover span {display: block; }

        .limited {
            white-space: nowrap;
            max-width: 300px;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .limitedWrap {
            white-space: normal !important;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .DTFC_LeftBodyLiner {
            overflow-y: hidden !important;
        }

        .dataTable thead tr {
            background: linear-gradient(45deg, #303f9f, #7b1fa2);
        }

        table.dataTable thead th {
            font-size: 14px !important;
            padding: 3px 5px;
            color: white;
        }

        table.dataTable tbody td {
            font-size: 13px !important;
            padding: 3px 5px;
            color:black !important;
        }

        table.dataTable tbody td a {
            text-decoration: underline;
            color: blueviolet;
        }

        /* ----------------------------------------------------------------- */
        /* On Table Row Hover */
        /* ----------------------------------------------------------------- */
        table.dataTable.hover tbody tr:hover,table.dataTable.display tbody tr:hover{
            background-color:#f9ffbf;
        }
        .dataTable {
            border: 1px solid silver !important;
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

        .Background {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
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

        ::placeholder { /* Chrome, Firefox, Opera, Safari 10.1+ */
            color: navy;
            opacity: 1; /* Firefox */
        }

        :-ms-input-placeholder { /* Internet Explorer 10-11 */
            color: navy;
        }

        ::-ms-input-placeholder { /* Microsoft Edge */
            color: navy;
        }
    </style>
    <script type="text/javascript"> 
        $(document).ready(function () {
            $('.tooltipped').tooltip();
            loadPageLayout();
        });
        function ShowConverationLog(keyvalue) {
            var modname = $("#hdnView").val();
            document.getElementById('spnModuleHeader').innerText = "Conversation Log";
            $('#ifrModule').attr('src', "ConversationChatBox.aspx?modulename=" + modname + "&keyvalue=" + keyvalue);
            $find("mpe").show();
        }

        function openQuoteFromInquiry(id) {
            document.getElementById('spnModuleHeader').innerText = "Manage Quotation";
            $('#ifrModule').attr('src', "Quotation.aspx?mode=add&id=0&InquiryNo=" + id);
            $find("mpe").show();
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Below Section : For External Lead Cont.Followup 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function openTimeline(xView, xCustID, xCustName) {
            var pageUrl = "FollowupTimeline.aspx?view=" + xView + "&id=" + xCustID + "&name=" + xCustName;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        function openContFollowFromExternalLeads(mode, ExtpkID) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage External FollowUp";
            var pageUrl = "FollowupExternal.aspx?followupsource=external&mode=" + mode + "&id=0" + "&ExtpkID=" + ExtpkID;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Below Section : To Restrict EnterKey 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function EnterKeyFilter() {
            if (window.event.keyCode == 13) {
                event.returnValue = false
                event.cancel = true
            }
        }
        window.addEventListener('keydown', EnterKeyFilter)
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server" ClientIDMode="Static" EnablePageMethods="true"></asp:ScriptManager>
    
    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLoginUserRole" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnActionFlag" runat="server" ClientIDMode="Static" Value="false" />
    <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnKeyID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTitle" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPara1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPara2" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPara3" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnOutwardVersion" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnQuotationVersion" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnQuotationNoForPDF" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSalesOrderVersion" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientERPIntegration" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnInquiryShare" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnInquiryShareRoles" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnInquiryAssign" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnpopupPrintHeader" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDataUrl" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDataUrlSearch" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCurrPageNo" runat="server" ClientIDMode="Static" Value="1" />
    <asp:HiddenField ID="hdnCurrPageSize" runat="server" ClientIDMode="Static" EnableViewState="true" Value="10" />
    <asp:HiddenField ID="hdnConversationLog" runat="server" ClientIDMode="Static" />

    <asp:HiddenField ID="hdnMenuID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAddFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEditFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDelFlag" runat="server" ClientIDMode="Static" />
    <div class="row">
        <div class="col s12">
            <div class="card">
                <div class="card-content" style="padding: 8px;">
                    <%--<h5 style="padding: 5px; line-height: 35px; margin: 0.42rem 0 .556rem 0; background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">--%>
                    <div class="float-left" style="width:150px; padding-top:8px;">
                        <span id="spnPageNo" class="ml-1 float-left white-text" style="font-size: 18px; border-radius: 5px;">Page # : </span>  
                        <span id="spnRecordCount" class="mr-1 float-left white-text" style="font-size: 18px; border-radius: 5px;"></span>
                    </div>
                    <div class="float-left" style="width:150px; padding-top:8px;">
                        <asp:Label runat="server" ClientIDMode="Static" ID="Label2" Style="float: left; margin-right: 10px; color:white; font-size:18px;">Size : </asp:Label>
                        <asp:DropDownList ID="drpPageSize" runat="server" ClientIDMode="Static" class="select2-theme browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpPageSize_SelectedIndexChanged" style="width: 60px; display:inline-block; background-color: beige; margin-left: 5px; height: 1.8rem; padding: 0px; font-size:14px;">
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="20" Value="20" />
                            <asp:ListItem Text="30" Value="30" />
                            <asp:ListItem Text="40" Value="40" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="100" Value="100" />
                            <asp:ListItem Text="150" Value="150" />
                            <asp:ListItem Text="200" Value="200" />
                            <asp:ListItem Text="250" Value="250" />
                            <asp:ListItem Text="300" Value="300" />
                        </asp:DropDownList>
                    </div>     
                    <div class="float-left mr-2" style="width:300px; padding-top:8px;">
                        <asp:Label runat="server" ClientIDMode="Static" ID="lblExternalLeadAc" Style="float: left; margin-right: 10px; color:white; font-size:18px;">Select Account</asp:Label>
                        <asp:DropDownList ID="drpExternalLeadAc" runat="server" class="select2-theme browser-default mr-2 float-left white blue-text" ClientIDMode="Static" OnSelectedIndexChanged="drpExternalLeadAc_SelectedIndexChanged" TabIndex="5" Style="font-size: 18px; font-weight: bold; width: 150px; margin-top: 1px; height: 35px !important; border: 1px solid black; border-radius: 5px; padding: 1px;" AutoPostBack="true">
                            <asp:ListItem Text="IndiaMart" Value="IndiaMart" />
                            <asp:ListItem Text="TradeIndia" Value="TradeIndia" />
                        </asp:DropDownList>
                    </div>
                    <div class="float-left mr-2" style="width:300px; padding-top:8px;">
                        <asp:Label runat="server" ClientIDMode="Static" ID="lblExternalLeadCat" Style="float: left; margin-right: 10px; color:white; font-size:18px;">Filter : </asp:Label>
                        <asp:DropDownList ID="drpExternalLeadCat" runat="server" class="select2-theme browser-default mr-2 float-left white blue-text" ClientIDMode="Static" OnSelectedIndexChanged="drpExternalLeadCat_SelectedIndexChanged" TabIndex="5" Style="font-size: 18px; font-weight: bold; width: 150px; margin-top: 1px; height: 35px !important; border: 1px solid black; border-radius: 5px; padding: 1px;" AutoPostBack="true">
                            <asp:ListItem Text="All Leads" Value="" />
                            <asp:ListItem Text="Qualified" Value="Qualified" />
                            <asp:ListItem Text="In-Process" Value="InProcess" />
                            <asp:ListItem Text="DisQualified" Value="Disqualified" />
                        </asp:DropDownList>
                    </div>
                    <h5 class="gradient-45deg-indigo-purple" style="padding: 5px; line-height: 35px; margin: 0.42rem 0 .556rem 0;">
                        <i id="pageIcon" onclick="javascript:showhideMyMenu();" class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Manage Data</span>

                        <%--Section : Add New Entry --%>
                        <a id="lnkAddNew" runat="server" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped ml-2 mr-2" data-position="center" data-tooltip="Add New Entry" href="javascript:gridAction('add', 0);" style="padding: 5px;"><i class="material-icons">add</i></a>

                        <%--Section : Download/Upload --%>

                        <asp:ImageButton ID="btnExportToExcel" runat="server" ClientIDMode="Static" CssClass="btn-floating float-right ml-1 mr-1 tooltipped" AlternateText="Export" data-tooltip="Export To Excel" ImageAlign="left" ImageUrl="images/buttons/export2excel.png" OnClick="btnExportToExcel_Click" Height="35px" Width="35px" Style="z-index: 0;" />
                        <asp:ImageButton ID="btnExportToPdf" runat="server" ClientIDMode="Static" CssClass="btn-floating float-right ml-1 mr-1 tooltipped" AlternateText="Export" data-tooltip="Export To PDF" ImageAlign="left" ImageUrl="images/pdf_document.png" OnClick="btnExportToPdf_Click" Height="35px" Width="35px" Style="z-index: 0; visibility : hidden;" />
                        <div class="float-right mr-15" style="width:500px;">
                            <asp:TextBox ID="txtSearchBoxHeader" runat="server" ClientIDMode="Static" placeholder="Type Content To Search .... " class="form-control white black-text mr-1" Style="width:300px; margin-bottom: 20px;" TabIndex="1" />
                            <a id="lnkSearchHeader" class="btn waves-effect waves-light gradient-45deg-purple-deep-orange" data-position="center" href="javascript:gridSearchData('top');" style="background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4)); z-index:0;">Filter Data</a>
                        </div>
                        <%--Section : External Lead Filters --%>
                        <asp:Label CssClass="ml-5 mr-1 float-left white-text" runat="server" ClientIDMode="Static" ID="lblComplainStatus">Filter : </asp:Label>
                        <asp:DropDownList ID="drpComplainStatus" runat="server" class="select2-theme browser-default mr-2 float-left white blue-text" ClientIDMode="Static" OnSelectedIndexChanged="drpComplainStatus_SelectedIndexChanged" TabIndex="5" Style="font-size: 18px; font-weight: bold; width: 150px; margin-top: 1px; height: 35px !important; border: 1px solid black; border-radius: 5px; padding: 1px;" AutoPostBack="true">
                            <asp:ListItem Text="All Status" Value="" />
                            <asp:ListItem Text="Open" Value="Open" />
                            <asp:ListItem Text="Close" Value="Close" />
                        </asp:DropDownList>

                    </h5>
                    <div class="row">
                        <div class="col s12">

                            <div id="profilecard" class="card animate fadeRight mb-0">
                                <%--                                <div class="col m12 p-0" style="margin-bottom: 10px;">
                                    <h5 class="left-align mb-0 width-100 gradient-45deg-indigo-purple" style="min-height: 30px;">
                                    </h5>
                                </div>--%>
                                <div class="card-content p-0">
                                    <table id="tblView" class="display width-100 nowrap" style="margin-left: 0px;"></table>
                                    <div style="text-align: center; width: 100%; margin: auto; margin-top:10px;">
                                        <a id="lnkResetPage" class="material-icons background-round gradient-45deg-red-pink gradient-shadow white-text float-left tooltipped ml-1 mr-1" data-tooltip="Refresh Data" href="javascript:gridReloadData('reset');" style="position: relative; padding: 10px; top: -25px;"><i class="material-icons">autorenew</i></a>
                                        <a id="lnkPrevPage" class="material-icons background-round gradient-45deg-purple-deep-orange gradient-shadow white-text tooltipped ml-1 mr-1 hide" data-tooltip="Previous" href="javascript:gridReloadData('prev');" style="position: relative; padding: 10px; top: -25px;"><i class="material-icons">chevron_left</i></a>
                                        <a id="lnkNextPage" class="material-icons background-round gradient-45deg-purple-deep-orange gradient-shadow white-text tooltipped ml-1 mr-1" data-tooltip="Next" href="javascript:gridReloadData('next');" style="position: relative; padding: 10px; top: -25px;"><i class="material-icons">chevron_right</i></a>
                                        <a id="lnkFilter" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped btn-move-up accent-2 z-depth-4 right" data-tooltip="Filter Data" style="padding: 10px; top: -25px; right: -5px;"><i class="material-icons activator">filter_list</i></a>
                                    </div>
                                </div>
                                <div class="card-reveal gradient-45deg-light-blue-teal gradient-shadow black-text">
                                    <div id="divSearchArea" runat="server">
                                        <span class="card-title grey-text text-darken-4">Filter Data<i class="material-icons right">close</i></span>
                                        <div id="profile-card1" class="card animate fadeRight mb-0">
                                            <div class="card-content p-0 col m6">
                                                <asp:TextBox ID="txtSearchBox" runat="server" ClientIDMode="Static" placeholder="Type Content To Search .... " class="form-control black-text" Style="margin-bottom: 20px;" TabIndex="1" />
                                                <a id="lnkSearch" class="btn waves-effect waves-light gradient-45deg-purple-deep-orange" data-position="center" href="javascript:gridSearchData('bottom');" style="background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">Submit</a>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divTaxSummary" runat="server" class="row hide">
                                        <div class="col m12">
                                            <h5 class="card-title badge gradient-45deg-yellow-green black-text gradient-shadow" style="padding: 5px; line-height: 35px; margin: 0.42rem 0 .556rem 0;">
                                                <span class="grey-text text-darken-4">TAX SUMMARY<i class="material-icons right">close</i></span>
                                            </h5>
                                            <uc1:myTaxSummary runat="server" id="myTaxSummary" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="pnlMyModule" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlMyModule" runat="server" ClientIDMode="Static" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
        <div id="myModal">
            <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                    <i class="material-icons prefix">ac_unit</i>
                    <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module..</span>
                    <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                </h5>
            </div>
        </div>
        <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 85%; padding: 5px;"></iframe>
        <br />
    </asp:Panel>
    <%-- ********************************************************************************************** --%>
    <%--    <script src="app-assets/js/vendors.min.js"></script>--%>
    <%--    <script src="app-assets/vendors/data-tables/js/jquery.dataTables.min.js"></script>
    <script src="app-assets/vendors/data-tables/extensions/responsive/js/dataTables.responsive.min.js"></script>
    <script src="app-assets/vendors/data-tables/js/dataTables.select.min.js"></script>--%>

    <script type="text/javascript">
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Below Section : To Restrict EnterKey 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function EnterKeyFilter() {
            if (window.event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                document.getElementById('lnkSearchHeader').focus();
                document.getElementById('lnkSearchHeader').click();
            }
        }
        window.addEventListener('keydown', EnterKeyFilter)
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function show_loader() {
            jQuery("#myLoader").removeClass("hide-loader");
            jQuery("#myLoader").addClass("loader");
            //event.preventDefault();
        }

        function hide_loader() {
            jQuery("#myLoader").removeClass("loader");
            jQuery("#myLoader").addClass("hide-loader");
            //event.preventDefault();
        }

        $("#lnkFilter").click(function () {
            $("#txtSearchBox").focus();
        })

        $(".btnTopRightCorner").click(function () {
            if ($("#spnPageNo").text() != 'Filtered Data') {
                var pgSource = $('#ifrModule').attr('src');
                 
                var strPos = pgSource.indexOf('mode=edit');
                var strPos1 = pgSource.indexOf('mode=add');
                if (strPos >= 0 || strPos1 >= 0) {
                    jQuery.ajax({
                        url: '/Services/NagrikService.asmx/setGridPageSize?pSize=' + $("#drpPageSize").val(), type: "get",
                        success: function (data) {
                            //alert('success-123')
                            $("#hdnCurrPageNo").val("1")
                            gridReloadData('reset');
                        }
                    });
                }
                //if (strPos >= 0 || strPos1 >= 0) {
                //    gridReloadData('reload');
                //}
            }
        })
        // -------------------------------------------------------------------------
        function gridReloadData(action) {
            show_loader();
            var currPgNo;
            var oldPgNo = $("#hdnCurrPageNo").val();
            // -----------------------------------------------------------
            jQuery.ajax({
                url: '/Services/NagrikService.asmx/setGridPageSize?pSize=' + $("#drpPageSize").val(),
                type: "get",
                success: function (data) { }
            });
            // -----------------------------------------------------------
            //alert(action + ' .... ' + oldPgNo)
            if (action == 'next') {
                currPgNo = (parseInt(oldPgNo) + 1);
                $("#lnkPrevPage").removeClass("hide");
            }
            else if (action == 'prev') {
                if (parseInt(oldPgNo) > 1)
                    currPgNo = (parseInt(oldPgNo) - 1);
                else {
                    currPgNo = parseInt(oldPgNo);
                    $("#lnkPrevPage").addClass("hide");
                }
            }
            else if (action == 'reset') {
                currPgNo = 1;
                $("#lnkPrevPage").removeClass("hide");
                $("#lnkNextPage").removeClass("hide");
            }
            else if (action == 'reload') {
                currPgNo = (parseInt(oldPgNo));
            }
            $("#hdnCurrPageNo").val(currPgNo);
            $("#spnPageNo").text("Page # : " + currPgNo);
            // ---------------------------------------------------
            var orgdataurl = dataurl;
            //alert(dataurl)
            if (dataurl.indexOf('?currPageNo') != -1) {
                dataurl = dataurl.substring(0, dataurl.indexOf('?currPageNo'));
                dataurl = dataurl + "?currPageNo=" + $("#hdnCurrPageNo").val();

            }
            else if (dataurl.indexOf('&currPageNo') != -1) {
                dataurl = dataurl.substring(0, dataurl.indexOf('&currPageNo'));
                dataurl = dataurl + "&currPageNo=" + $("#hdnCurrPageNo").val();

            }
            // ---------------------------------------------------
            if ($("#hdnView").val() == 'bankvoucher')
                dataurl = dataurl + "&TrType=bank";
            if ($("#hdnView").val() == 'cashvoucher')
                dataurl = dataurl + "&TrType=cash";
            // ---------------------------------------------------
            var gridTable;
            jQuery.ajax({
                url: dataurl,
                type: "get",
                success: function (data) {
                    var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                    if (resultdata != null && resultdata != '') {
                        try {
                            // GridReloadData
                            //jQuery.ajax({
                            //    url: "Services/NagrikService.asmx/getSessionObject",
                            //    type: "GET",
                            //    dataType: "json",
                            //    success: function (datapageno) {
                            //        debugger; // One
                            //        var resultdata11 = JSON.parse(datapageno.d);
                            //        var myTotalRecord1 = resultdata11[0].value;
                            //        if (myTotalRecord1 != null && myTotalRecord1 != '') {
                            //            $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                            //        }
                            //    }
                            //});
                        }
                        catch (ex) { }
                        // ------------------------------------------------------------
                        var table;
                        if ($.fn.dataTable.isDataTable('#tblView')) {
                            table = $('#tblView').DataTable();
                            table.clear();
                            table.rows.add(resultdata).draw();
                        }
                    }
                    else {
                        if (action == 'prev' && $("#hdnCurrPageNo").val() == 1 && oldPgNo == 2) {
                            swal({ title: "Data Information !", text: "You are on First Page !", icon: 'info' })
                        }
                        else if (action == 'next') {
                            $("#hdnCurrPageNo").val(oldPgNo);
                            swal({ title: "Data Information !", text: "You are on Last Page !", icon: 'info' });
                        }
                        $("#spnPageNo").text("Page # : " + $("#hdnCurrPageNo").val());
                    }
                }
            });
            // ---------------------------------------------------
            if (dataurl.indexOf('?currPageNo') != -1) {
                dataurl = dataurl.substring(0, dataurl.indexOf('?currPageNo'));
                dataurl = dataurl + "?currPageNo=" + $("#hdnCurrPageNo").val();
            }
            else if (dataurl.indexOf('&currPageNo') != -1) {
                dataurl = dataurl.substring(0, dataurl.indexOf('&currPageNo'));
                dataurl = dataurl + "&currPageNo=" + $("#hdnCurrPageNo").val();
            }
            //alert(dataurl)
            window.setTimeout(hide_loader, 500);
        }
        // -------------------------------------------------------------------------
        function gridSearchData(pos) {
            // display: block; transform: translateY(-100%);

            var tmpVal;
            if (pos=='top')
                tmpVal = $("#txtSearchBoxHeader").val();
            else
            {
                $('.card-reveal').css('transform', 'translate(0%)');
                $('.card-reveal').css('display', 'none');
                tmpVal = $("#txtSearchBox").val();
            }

            // --------------------------------------------------
            if (tmpVal != null && tmpVal != "") {
                $("#hdnCurrPageNo").val(1);
                $("#lnkPrevPage").removeClass("hide");
                $("#lnkNextPage").removeClass("hide");
                $("#lnkResetPage").removeClass("hide");

                $("#lnkPrevPage").addClass("hide");
                $("#lnkNextPage").addClass("hide");
                $("#lnkResetPage").removeClass("hide");
                // ---------------------------------------------------
                dataurlsearchNew = dataurlsearch;
                dataurlsearchNew = dataurlsearchNew.replace('?currPageNo=1', '');
                dataurlsearchNew = dataurlsearchNew.replace("?currPageNo=" + $("#hdnCurrPageNo").val(), '');
                if (dataurlsearchNew.indexOf('?') != -1) {
                    dataurlsearchNew = dataurlsearchNew + "&SearchKey=" + tmpVal;
                }
                else {
                    dataurlsearchNew = dataurlsearchNew + "?SearchKey=" + tmpVal;
                }
                // ---------------------------------------------------
                var gridTable;
                jQuery.ajax({
                    url: dataurlsearchNew,
                    type: "get",
                    success: function (data) {
                        var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                        if (resultdata != null && resultdata != '') {
                            // ------------------------------------------------------------
                            //jQuery.ajax({
                            //    url: "Services/NagrikService.asmx/getSessionObject",
                            //    type: "GET",
                            //    contentType: "application/json; charset=utf-8",
                            //    dataType: "json",
                            //    success: function (datapageno) {
                            //        debugger; //two
                            //        var resultdata11 = JSON.parse(datapageno.d);
                            //        var myTotalRecord1 = resultdata11[0].value;
                            //        if (myTotalRecord1 != null && myTotalRecord1 != '') {
                            //            $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                            //            $("#spnPageNo").text("Filtered Data");
                            //        }
                            //    }
                            //});
                            // ------------------------------------------------------------
                            var table;
                            if ($.fn.dataTable.isDataTable('#tblView')) {
                                $("#spnPageNo").text("Filtered Data");
                                table = $('#tblView').DataTable();
                                table.clear();
                                table.rows.add(resultdata).draw();
                            }
                        }
                    }
                });
            }
            else {
                swal({ title: "Data Information !", text: "Empty Search is not allowed !", icon: 'info' });
            }
            // ----------------------------------------
            $("#txtSearchBoxHeader").val('');
            $("#txtSearchBox").val('');
        }
        // -------------------------------------------------------------------------
        function openNewModuleForm() {
            gridAction('add', 0);
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
        // -------------------------------------------------------------------------
        // Open Customer View
        // -------------------------------------------------------------------------
        function openCustomerInfo(cat, keyid) {
            if (keyid != '' && keyid != undefined) {
                document.getElementById('spnModuleHeader').innerText = "Manage Customer";
                $('#ifrModule').attr('src', "customers.aspx?mode=" + cat + "&id=" + keyid);
                $find("mpe").show();
            }
        }
        // -------------------------------------------------------------------------
        // Loading ..... PAGE LAYOUT
        // -------------------------------------------------------------------------
        var colstructure = [];
        var dataurl, dataurlsearch, para1, para2, para3, viewmode;
        //var _addFlag = true, _editFlag = true, _delFlag = true;
        var sortingDef;
        var loginuserid = "";
        var pageTitle = "";

        function loadPageLayout() {

            viewmode = jQuery("#hdnView").val();
            loginuserid = jQuery("#hdnLoginUserID").val();
            // --------------------------------------------------------
            mysection1: {
            if (viewmode == 'inquiry') {
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                dataurl = '/Services/NagrikService.asmx/webInquiryDetailByStatus?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webInquiryDetailByStatusSearch?';
                colstructure = [
                    { title: "Lead Date", data: "InquiryDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Lead #", data: "InquiryNo", className: "center", width: "110",
                        render: function (data, type, row, meta) {
                            if ($("#hdnLoginUserID").val() == row.CreatedBy)
                                data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            else
                                data = '<a style="border-radius:8px; background-color:red; color:white; padding:5px;"  href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';

                            return data;
                        }
                    },
                    //{ title: "Priority", data: "Priority", className: "center", width: "80" },
                    {
                        title: "Customer/Company Name", data: "CustomerName", width: "100",
                        render: function (data, type, row, meta) {
                            data = '<a style="display:inline-block; width:150px; white-space: pre-wrap;" href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');" >' + data + '</a>';
                            //data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');" >' + data + '</a>';
                            return data;
                        }

                    },
                    { title: "ContactNo", data: "ContactNo1", className: "center", width: "100" },
                    { title: "ReferenceName", data: "ReferenceName", className: "center", width: "100" },
                    { title: "City", data: "CityName", className: "center", width: "100" },
                    { title: "State", data: "StateName", className: "center", width: "100" },
                    { title: "Source", data: "InquirySource", className: "center", width: "100" },
                    { title: "Status", data: "InquiryStatus", className: "center", width: "150" },
                    { title: "Next Folloup", data: "LastNextFollowupDate", className: "center", type: "date", width: "200", 
                        render: function (data, type, full) {
                            var newdate;
                            if (moment(data).year() > 2000)
                                newdate = moment(data).format('DD-MMM-YYYY'); 
                            else 
                                newdate = '';
                            return newdate;
                        } 
                    },
                    {
                        title: "No Folloup", data: "NoFollowUp", className: "center", type: "date", width: "200",
                        render: function (data, type, row, meta) {
                            var newdata;
                            if (data == 0)
                            {
                                if (moment(data).year()>2000)
                                    newdata = moment(row.LastNextFollowupDate).format('DD-MMM-YYYY');
                                else 
                                    newdata = ''
                            }
                            else
                                newdata = '<span class="badge gradient-45deg-purple-deep-orange white-text gradient-shadow">No FollowUp</span>';
                            return newdata;
                        }
                    },
                    { title: "Total Amount", data: "TotalAmount", className: "right-align", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: " Sales Executive", data: "EmployeeName", width: "150" }
                ];
                break mysection1;

            }
            else if (viewmode == 'inquiryjjb') {
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                dataurl = '/Services/NagrikService.asmx/webInquiryDetailByStatus?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webInquiryDetailByStatusSearch?';
                colstructure = [
                    { title: "Lead Date", data: "InquiryDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Lead #", data: "InquiryNo", className: "center", width: "110",
                        render: function (data, type, row, meta) {
                            if ($("#hdnLoginUserID").val() == row.CreatedBy)
                                data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            else
                                data = '<a style="border-radius:8px; background-color:red; color:white; padding:5px;"  href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';

                            return data;
                        }
                    },
                    //{ title: "Priority", data: "Priority", className: "center", width: "80" },
                    {
                        title: "Customer/Company Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }

                    },
                    { title: "ContactNo", data: "ContactNo1", className: "center", width: "100" },
                    { title: "ReferenceName", data: "ReferenceName", className: "center", width: "100" },
                    { title: "City", data: "CityName", className: "center", width: "100" },
                    { title: "State", data: "StateName", className: "center", width: "100" },
                    { title: "Source", data: "InquirySource", className: "center", width: "100" },
                    { title: "Status", data: "InquiryStatus", className: "center", width: "150" },
                    { title: "Next Folloup", data: "LastNextFollowupDate", className: "center", type: "date", width: "200", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Total Amount", data: "TotalAmount", className: "right-align", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: " Sales Executive", data: "EmployeeName", width: "150" }
                ];
                break mysection1;

            }
            else if (viewmode == 'externalleads') {
                para1 = jQuery.trim(jQuery("#drpExternalLeadAc").val());
                para2 = jQuery.trim(jQuery("#drpExternalLeadCat").val());
                dataurl = '/Services/NagrikService.asmx/webExternalLeadList?source=' + para1 + '&acid=' + '&cat=' + para2 + "&currPageNo=1";
                dataurlsearch = '/Services/NagrikService.asmx/webExternalLeadListSearch?source=indiamart&acid=' + '&cat=' + para2;
                var xUntouchTitle = "Updated By";
                //var xUntouchTitle = "Untouch Since <br />Updated By";
                //xUntouchTitle = (para2 == 'Qualified' || para2 == 'Disqualified') ? "Last Folloup" : "Untouch Since";
                colstructure = [
                    {
                        title: xUntouchTitle, data: "QueryDatetime", className: "center", width: "110",
                        render: function (data, type, row, meta) {
                            //data = '<p class="mb-0" style="color:red;">' + getStatusUpdate('days', moment(data).format('YYYY-MM-DD'), row.pkID) + '</p><p class="badge badge-info mb-0" style="font-size:12px; text-transform:capitalize;">' + row.CreatedBy + '</p>';
                            data = '<p class="badge badge-info mb-0" style="font-size:12px; text-transform:capitalize;">' + row.UpdatedBy + '</p>';
                            return data;
                        }
                    },
                    { title: "Source", data: "LeadSource", className: "center", width: "80" },
                    {
                        title: "Assign To", data: "EmployeeName", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            if (row.EmployeeName != '--Not Available--')
                                data = '<p class="mb-0" style="color:blue; font-weight:bold;">' + row.EmployeeName + '</p>';
                            else
                                data = '';
                            return data;
                        }
                    },
                    {
                        title: "Lead No.", data: "pkID", className: "center", width: "90",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Date", data: "QueryDatetime", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "State", data: "State", className: "center", width: "100" },
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
                    //{ title: "Email ID", data: "SenderMail", className: "text-left" },
                    { title: "For Product", data: "ForProduct", className: "text-left", width: "200" },
                    {
                        title: "Status", data: "LeadSource", className: "center",
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
                    {
                        title: "Lead #", data: "InquiryNo", className: "center", width: "130",
                        render: function (data, type, row, meta) {
                            if (data != '' && data != '')
                                data = '<a href="javascript:showInquiryInfo(\'' + data + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    //{ title: "Quali./Disquali. By", data: "CreatedBy", className: "center", width: "130" }
                ];

                break mysection1;
            }
            else if (viewmode == 'telecaller') {
                para1 = '';
                para2 = jQuery.trim(jQuery("#drpExternalLeadCat").val());
                dataurl = '/Services/NagrikService.asmx/webExternalLeadList?source=telecaller&acid=' + para1 + '&cat=' + para2 + "&currPageNo=1";
                dataurlsearch = '/Services/NagrikService.asmx/webExternalLeadListSearch?source=telecaller&acid=' + para1 + '&cat=' + para2;
                var xUntouchTitle = "Untouch Since <br />Updated By";
                colstructure = [
                    {
                        title: xUntouchTitle, data: "QueryDatetime", className: "center", width: "110",
                        render: function (data, type, row, meta) {
                            data = '<p class="mb-0" style="color:red;">' + getStatusUpdate('days', moment(data).format('YYYY-MM-DD'), row.pkID) + '</p><p class="badge badge-info mb-0" style="font-size:12px; text-transform:capitalize;">' + row.CreatedBy + '</p>';
                            return data;
                        }
                    },
                    {
                        title: "Lead No.", data: "pkID", className: "center", width: "90",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Date", data: "QueryDatetime", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "State", data: "State", className: "center", width: "100" },
                    {
                        title: "Sender Name", data: "SenderName", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<p class="mb-0" style="color:navy;font-weight:bold;">' + row.SenderName + '</p><p class="badge badge-primary" style="font-size:12px;">' + row.PrimaryMobileNo + '</p>';
                            
                            return data;
                        }

                    },
                    {
                        title: "Company Name<br />Email ID", data: "CompanyName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            if (row.CompanyName != '')
                                data = '<p class="mb-0" style="color:darkred;font-weight:bold;">' + row.CompanyName + '</p><p class="mb-0" style="color:blue;">' + row.SenderMail + '</p>';
                            else
                                data = '<p class="mb-0" style="color:blue;">' + row.SenderMail + '</p>';
                            return data;
                        }
                    },
                    //{ title: "Email ID", data: "SenderMail", className: "text-left" },
                    { title: "For Product", data: "ForProduct", className: "text-left", width: "200" },
                    {
                        title: "Status", data: "LeadSource", className: "center",
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
                    {
                        title: "Lead #", data: "InquiryNo", className: "center", width: "130",
                        render: function (data, type, row, meta) {
                            if (data != '' && data != '')
                                data = '<a href="javascript:showInquiryInfo(\'' + data + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    //{ title: "Quali./Disquali. By", data: "CreatedBy", className: "center", width: "130" }
                ];

                break mysection1;
            }
            else if (viewmode == 'followup') {
                dataurl = '/Services/NagrikService.asmx/webFollowupDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webFollowupDetailSearch';
                colstructure = [
                    {
                        title: "Followup Date", data: "FollowupDate", type: "date", className: "center", width: "160",
                        render: function (data, type, row, meta) {
                            data = (data) ? moment(data).format('DD-MMM-YYYY') : '';
                            return '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                        }
                    },
                    { title: "Followup Type", data: "InquiryStatus", className: "center", width: "80" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },

<%--                    <% if (hdnSerialKey.Value.Contains("ECO3-2G21-TECH-3MRT"))
                       { %>
                            { title: "City", data: "CityName", className: "center", width: "160" },
                        <% }; %>--%>

                    {
                        title: "Next Followup", data: "NextFollowupDate", className: "center", type: "date", width: "160",
                        render: function (data, type, full)
                        {
                            var newdate;
                            if (moment(data).year() > 2000)
                                newdate = moment(data).format('DD-MMM-YYYY');
                            else
                                newdate = '';
                            return newdate;
                            //return (data) ? moment(data).format('DD-MMM-YYYY') : '';
                        }
                    },
                    { title: "Notes", data: "MeetingNotes", className: "limited", width: "200" },
                    {
                        title: "Lead #", data: "InquiryNo", className: "center", width: "100",
                        render: function (data, type, row, meta) {
                            if (data != '' && data != '')
                                data = '<a href="javascript:showInquiryInfo(\'' + data + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "No Followup", data: "NoFollowup", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            if (data == true)
                                data = '<span class="badge gradient-45deg-purple-deep-orange white-text gradient-shadow">No FollowUp</span>';
                            else
                                data = '';

                            return data;
                        }
                    },
                    {
                        title: "Closure Reason", data: "NoFollClosureName", className: "center", width: "140",
                        render: function (data, type, row, meta) {
                            data = (data == '--Not Available--') ? '' : data;
                            return data;
                        }
                    },
                    { title: " Sales Executive", data: "EmployeeName", width: "150" }
                ];

                break mysection1;
            }
            else if (viewmode == 'quotation' || viewmode == 'quotationsolar') {
                dataurl = '/Services/NagrikService.asmx/webQuotationDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webQuotationDetailSearch';
                colstructure = [
                    { title: "Quotation Date", data: "QuotationDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Quotation #", data: "QuotationNo", className: "center", width: "200",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Lead #", data: "InquiryNo", className: "limitedWrap center", width: "150"
                        ,render: function (data, type, row, meta) {
                            if (data != '' && data != '')
                                data = '<a href="javascript:showInquiryInfo(\'' + data + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Status", data: "InquiryStatus", className: "center", width: "180" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Basic Amount", data: "QuotationAmount", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Net Amount", data: "NetAmt", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Sales Executive", data: "CreatedEmployeeName", width: "150" },
                ];

                break mysection1;
            }
            else if (viewmode == 'salesorder' || viewmode == 'salesordernew') {
                dataurl = '/Services/NagrikService.asmx/webSalesOrderDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webSalesOrderDetailSearch';
                colstructure = [
                    { title: "Order Date", data: "OrderDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Order #", data: "OrderNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Lead/Quot.#", data: "DocRefNoList", className: "limitedWrap center", width: "150" },
                    { title: "Reference #", data: "ReferenceNo", className: "center", width: "150" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Sales Exec.", data: "EmployeeName", className: "center", width: "150" },
                     <% if (hdnSerialKey.Value.Contains("SO5H-DH90-E34L-SIOF"))
                       { %>
                            { title: "Gov. Project Stages", data: "ProjectStage", className: "center", width: "200" },
                        <% }; %>
                    { title: "Status", data: "ApprovalStatus", className: "center", width: "150" },
                    { title: "Order Amount", data: "OrderAmount", className: "right-align", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Adv. Amount", data: "AdvAmt", className: "right-align", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') }
                    ];
                break mysection1;
            }
            else if (viewmode == 'salesorderdealer') {
                dataurl = '/Services/NagrikService.asmx/webSalesOrderDealerDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webSalesOrderDealerDetailSearch';
                colstructure = [
                    { title: "Order Date", data: "OrderDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Order #", data: "OrderNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Ref #", data: "RefNo", className: "center", width: "150" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Sales Exec.", data: "EmployeeName", className: "center", width: "150" },
                    { title: "Status", data: "ApprovalStatus", className: "center", width: "150" },
                    { title: "Order Amount", data: "OrderAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') }];
                break mysection1;
            }
            else if (viewmode == 'purchaseorder') {
                dataurl = '/Services/NagrikService.asmx/webPurchaseOrderDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webPurchaseOrderDetailSearch';
                colstructure = [
                    { title: "Order Date", data: "OrderDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "PO Order #", data: "OrderNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Indent #", data: "DocRefNoList", className: "limitedWrap center", width: "90" },
                    { title: "Ref #", data: "RefNo", className: "center", width: "150" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Sales Exec.", data: "EmployeeName", className: "center", width: "150" },
                    { title: "Status", data: "ApprovalStatus", className: "center", width: "150" },
                    { title: "Order Amount", data: "OrderAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') }];
                break mysection1;
            }
            else if (viewmode == 'workordercomm') {
                dataurl = '/Services/NagrikService.asmx/webWorkOrderDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webWorkOrderDetailSearch';
                colstructure = [
                    { title: "Order Date", data: "OrderDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Work Order #", data: "OrderNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Ref #", data: "RefNo", className: "center", width: "150" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Order Amount", data: "OrderAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') }];
                break mysection1;
            }
            else if (viewmode == 'inward') {
                dataurl = '/Services/NagrikService.asmx/webInwardDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webInwardDetailSearch';
                colstructure = [
                    { title: "Inward Date", data: "InwardDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Inward No#", data: "InwardNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Indent/PO #", data: "DocRefNoList", className: "limitedWrap center", width: "90" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    
                    <% if (hdnSerialKey.Value.Contains("VAR2-DH0A-MAN9-8SIO"))
                       { %>
                            { title: "Manual Inward#", data: "ManuaLInwardNo", className: "center", width: "120" },
                        <% }; %>

                    { title: "Basic Amount", data: "BasicAmount", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Tax Amount", data: "TaxAmount", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Net Amount", data: "InwardAmount", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    ];

            }
            else if (viewmode == 'productionagain') {
            dataurl = '/Services/NagrikService.asmx/webProductionDetail?currPageNo=1';
            dataurlsearch = '/Services/NagrikService.asmx/webProductionDetailSearch';
            colstructure = [
                { title: "Production Date", data: "ProductionDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                {
                    title: "Production No#", data: "pkID", className: "center", width: "150",
                    render: function (data, type, row, meta) {
                        data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                        return data;
                    }
                },
                {
                    title: "Product Name", data: "FinishedProductName", className: "text-left limited", width: "250"
                }];

            }

            else if (viewmode == 'productionbyso') {
                dataurl = '/Services/NagrikService.asmx/webProductionBySODetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webProductionBySODetailSearch';
                colstructure = [
                { title: "Production Date", data: "ProductionDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                {
                    title: "Production No#", data: "pkID", className: "center", width: "150",
                    render: function (data, type, row, meta) {
                        data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                        return data;
                    }
                },
                { title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250" },
                { title: "SalesOrder No", data: "SoNo", className: "limitedWrap center", width: "90" },
                ];
            }
            else if (viewmode == 'comaccu') {
                dataurl = '/Services/NagrikService.asmx/webCompalaintDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCompalaintDetailSearch';
                colstructure = [

                    { title: "Complaint No", data: "ComplaintNo", className: "limitedWrap center", width: "90" },

                    { title: "Customer Name", data: "NameOfCustomer", className: "limitedWrap center", width: "90" },

                    { title: "Complaint Date", data: "ComplaintDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Status", data: "ComplaintStatus", className: "center", width: "70",
                        render: function (data, type, row, meta) {
                            if (row.ComplaintStatus == 'Open')
                                data = '<p class="mb-0 pill green white-text ">&nbsp;' + row.ComplaintStatus + '&nbsp;</p>';
                            else if (row.ComplaintStatus == 'Close')
                                data = '<p class="mb-0 pill red white-text ">&nbsp;' + row.ComplaintStatus + '&nbsp;</p>';
                            return data;
                        }
                    },
                    { title: "Created By", data: "CreatedByEmployee", className: "limitedWrap center", width: "90" },
                    ];
            }
            else if (viewmode == 'visitaccu') {
                dataurl = '/Services/NagrikService.asmx/webComplaintVisitList?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webComplaintVisitListSearch';
                colstructure = [

                    { title: "Complaint No", data: "ComplaintNo", className: "limitedWrap center", width: "90" },
                    { title: "Customer Name", data: "NameOfCustomer", className: "limitedWrap center", width: "90" },
                    { title: "Created Date", data: "CreatedDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Created By", data: "CreatedByEmployee", className: "limitedWrap center", width: "90" },
                    { title: "Contact No", data: "ContactNo1", className: "limitedWrap center", width: "90" },
                ];
            }
            else if (viewmode == 'production') {
                dataurl = '/Services/NagrikService.asmx/webMoldingDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webMoldingDetailSearch';
                colstructure = [
                    { title: "Production Date", data: "MoldingDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Production No#", data: "MoldingNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Work Type", data: "WorkType", type: "date", className: "center", width: "150" },
                    { title: "Customer Name", data: "CustomerName", type: "date", className: "center", width: "150" },
                    { title: "Order No", data: "OrderNo", type: "date", className: "center", width: "150" },

                    ];

            }
            else if (viewmode == 'internalworkorder') {
                dataurl = '/Services/NagrikService.asmx/webInternalWorkOrderDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webInternalWorkOrderDetailSearch';
                colstructure = [
                    { title: "Work Order Date", data: "WorkOrderDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Work Order No#", data: "WorkOrderNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },

                    { title: "Sales Order", data: "SalesOrderNo", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Quotation", data: "QuotationNo", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Inquiry", data: "InquiryNo", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                ];

            }
            else if (viewmode == 'jobcard') {
                dataurl = '/Services/NagrikService.asmx/webJobCard?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webJobCardSearch';
                colstructure = [
                    { title: "Job Card No", data: "JobCardNo", className: "center", width: "150" },
                    { title: "Date", data: "Date", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Registration No", data: "RegNo", className: "center", width: "150",
                    //    render: function (data, type, row, meta) {
                    //        data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                    //        return data;
                    //    }
                    },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },

                    { title: "Location Name", data: "LocationName", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Delivery Date", data: "DeliveryDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    //{ title: "Quotation", data: "QuotationNo", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    //{ title: "Inquiry", data: "InquiryNo", className: "text-right", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                ];

            }
            else if (viewmode == 'jobcardinward') {
                dataurl = '/Services/NagrikService.asmx/webJobCardInwardDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webJobCardInwardDetailSearch';
                colstructure = [
                    { title: "JC Inward Date", data: "InwardDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "JC Inward No#", data: "InwardNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Vendor Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Sales Executive", data: "CreatedEmployeeName", width: "150" }];

            }
                //-------------------------------------------Indent-----------------------------------------
            else if (viewmode == 'indent') {
                dataurl = '/Services/NagrikService.asmx/webIndentDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webIndentDetailSearch';
                colstructure = [
                    { title: "Indent Date", data: "IndentDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Indent No", data: "IndentNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Remarks", data: "Remarks", width: "150" },
                    { title: "Approval Status", data: "ApprovalStatus", width: "90" },
                    { title: "Initiated By", data: "CreatedEmployeeName", width: "150" }
                ];

            }
                //-------------------------------------------------------------------------------------
            else if (viewmode == 'outward') {
                dataurl = '/Services/NagrikService.asmx/webOutwardDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webOutwardDetailSearch';
                colstructure = [
                    { title: "Outward Date", data: "OutwardDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Outward No#", data: "OutwardNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Lead/Quot/SO #", data: "DocRefNoList", className: "limitedWrap center", width: "90" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                     <% if (hdnSerialKey.Value.Contains("VAR2-DH0A-MAN9-8SIO"))
                       { %>
                            { title: "Manual Outward#", data: "ManualOutwardNo", className: "center", width: "120" },
                        <% }; %>
                    {
                        title: "Reference #", data: "ReferenceNo", className: "center-align", width: "110",
                        render: function (data, type, row, meta) {
                            data = '<b class="blue-text">' + row.OrderNo + '</b><br /><b class="red-text">' + row.ReferenceNo + '</b>';
                            return data;
                        }
                    },
                    { title: "Basic Amount", data: "BasicAmount", className: "right-align", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Tax Amount", data: "TaxAmount", className: "right-align", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Net Amount", data: "OutwardAmount", className: "right-align", width: "110", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Sales Executive", data: "CreatedEmployeeName", className: "left-align", width: "150" }
                ];
                break mysection1;
            }
            else if (viewmode == 'jobcardoutward') {
                dataurl = '/Services/NagrikService.asmx/webJobCardOutwardDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webJobCardOutwardDetailSearch';
                colstructure = [
                    { title: "JC Outward Date", data: "OutwardDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "JC Outward No#", data: "OutwardNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Vendor Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Sales Executive", data: "CreatedEmployeeName", width: "150" }
                ];
                break mysection1;
            }
            else if (viewmode == 'contractinfo') {
                para1 = jQuery.trim(jQuery("#hdnPara1").val());
                dataurl = '/Services/NagrikService.asmx/webContractInfoDetailByStatus?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webContractInfoDetailByStatusSearch?';
                colstructure = [
                    {
                        title: "AMC Code #", data: "InquiryNo", className: "center", width: "110",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Contract Type", data: "ContractType", className: "center", width: "150" },
                    {
                        title: "Customer Name", data: "CustomerName", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Start Date", data: "StartDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "End Date", data: "EndDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Total Amount", data: "TotalAmount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Sales Executive", data: "EmployeeName", width: "150" }
                ];
                break mysection1;
            }
            else if (viewmode == 'salestarget') {
                dataurl = '/Services/NagrikService.asmx/webSalesTarget?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webSalesTargetSearch';
                colstructure = [
                    {
                        title: "Employee Name", data: "EmployeeName", width: "350",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Sales Target", className: "text-right", data: "TargetAmount", width: "150" },
                    { title: "From Date", data: "FromDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "To Date", data: "ToDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Target Type", className: "center", data: "TargetType", width: "50" }];
                break mysection1;
            }
            else if (viewmode == 'broadcast') {
                dataurl = '/Services/NagrikService.asmx/webBroadcastMessage?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webBroadcastMessageSearch';
                colstructure = [
                    { title: "From Date", data: "FromDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "To Date", data: "ToDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Message", data: "Message", width: "350",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];

                break mysection1;
            }
            else if (viewmode == 'vehiclemaster') {
                dataurl = '/Services/NagrikService.asmx/webVehicleMaster?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webVehicleMasterSearch';
                colstructure = [
                    {
                        title: "Registration Number", data: "RegistrationNo", width: "50",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Owner Name", data: "OwnerName", width: "50" },
                    { title: "Vehicle Type", data: "VehicleType", width: "50" },
                    { title: "Chasis Number", data: "ChasisNo", width: "30" },
                    { title: "Engine CC", data: "EngineCC", width: "30" },
                    { title: "Insurance Company", data: "InsuranceCompany", width: "50" },
                    { title: "Insurance Policy Number", data: "InsurancePolicyNo", width: "50" },
                    { title: "Insurance Expiry Date", data: "InsuranceExpiry", type: "date", className: "center", width: "50", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } }
                ];

                break mysection1;
            }
            else if (viewmode == 'testmodeule') {
                dataurl = '/Services/NagrikService.asmx/testmodule?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/testmoduleSearch';
                closture = [
                    {
                        title: "Registration Number", data: "RegistrationNo", width: "50",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Owner Name", data: "OwnerName", width: "50" },
                    { title: "Vehicle Type", data: "VehicleType", width: "50" },
                    { title: "Chasis Number", data: "ChasisNo", width: "30" },
                    { title: "Engine CC", data: "EngineCC", width: "30" },
                    { title: "Insurance Company", data: "InsuranceCompany", width: "50" },
                    { title: "Insurance Policy Number", data: "InsurancePolicyNo", width: "50" },
                    { title: "Insurance Expiry Date", data: "InsuranceExpiry", type: "date", className: "center", width: "50", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } }
                ];
                break mysection1;
            }
            else if (viewmode == 'transporter') {
                dataurl = '/Services/NagrikService.asmx/webTransporterMaster?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webTransporterMasterSearch';
                colstructure = [
                    {
                        title: "Registration Number", data: "RegistrationNo", width: "50",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Owner Name", data: "OwnerName", width: "50" },
                    { title: "Trip Date", data: "TripDate", type: "date", className: "center", width: "50", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Driver Name", data: "DriverName", width: "70" },
                    { title: "Location From", data: "From_Station", width: "50" },
                    { title: "Location To", data: "To_Station", width: "50" },
                    { title: "Kilometers", data: "Kilometers", width: "50" },
                    
                ];

                break mysection1;
            }
            else if (viewmode == 'vehiclemastertrip') {
                dataurl = '/Services/NagrikService.asmx/webVehicleMasterTrip?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webVehicleMasterTripSearch';
                colstructure = [
                    { title: "Trip Date", data: "TripDate", type: "date", className: "center", width: "50", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Vehicle", data: "RegistrationNo", width: "50",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Driver Name", data: "DriverName", width: "50" },
                    { title: "From Location", data: "From_Station", width: "50" },
                    { title: "To Location", data: "To_Station", width: "50" },
                    { title: "Kilometers", data: "Kilometers", width: "50" },
                    { title: "Trip Cost", data: "TripCost", width: "50" }];

                break mysection1;
            }
            else if (viewmode == 'visitorinfo') {
                dataurl = '/Services/NagrikService.asmx/webVisitorInfoDetailByStatus?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webVisitorInfoDetailByStatusSearch';
                colstructure = [
                    {
                        title: "Visitor #", data: "InquiryNo", width: "50",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Visit Date", data: "VisitDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Visit Time", data: "VisitTime", width: "150" },
                    { title: "Visitor Name", data: "VisitorName", className: "text-left limited", width: "150" },
                    { title: "Visit Purpose", data: "PurposeOfVisit", width: "150" },
                    { title: "Company Name", data: "CompanyName", width: "150" },
                    { title: "Meeting To", data: "MeetingTo", width: "150" },
                    { title: "Employee Name", data: "EmployeeName", className: "center", width: "150" }];
                break mysection1;
            }
            else if (viewmode == 'courierinfo') {
                dataurl = '/Services/NagrikService.asmx/webCourierInfoDetailByStatus?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCourierInfoDetailByStatusSearch';
                colstructure = [
                    {
                        title: "Serial No. #", data: "SerialNo", width: "100",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Delievery Date", data: "ActivityDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Trnas.Type", data: "AcceptanceType", width: "90" },
                    { title: "Sender/Recipient Name", data: "CustomerName", className: "text-left limited", width: "150" },
                    { title: "Docket/Tracking No", data: "DocketNo", width: "150" },
                    { title: "Courier Name", data: "CourierName", className: "text-left limited", width: "150" },
                    { title: "Document Type", data: "DocumentType", width: "150" },
                    { title: "Employee Name", data: "EmployeeName", className: "center", width: "150" },
                    { title: "Country", data: "CountryName", className: "center", width: "150" },
                    { title: "State", data: "StateName", className: "center", width: "150" },
                    { title: "City", data: "CityName", className: "center", width: "150" }
                ];
                break mysection1;
            }
            else if (viewmode == 'leaverequest') {
                dataurl = '/Services/NagrikService.asmx/webLeaveRequest?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webLeaveRequestSearch';
                colstructure = [
                    { title: "From Date", data: "FromDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "To Date", data: "ToDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Leave Type", data: "LeaveType", className: "center", width: "150" },
                    {
                        title: "Employee Name", data: "EmployeeName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Paid/Unpaid", data: "PaidUnpaid", className: "center", width: "100" },
                    { title: "Reason For Leave", data: "ReasonForLeave", className: "limited", width: "300" },
                    { title: "Approval Status", data: "ApprovalStatus", className: "center", width: "150" }];
                break mysection1;
            }
            else if (viewmode == 'overtime') {
                dataurl = '/Services/NagrikService.asmx/webOverTime?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webOverTimeSearch';
                colstructure = [
                    { title: "From Date", data: "FromDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "To Date", data: "ToDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Employee Name", data: "EmployeeName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Reason For Over Time", data: "ReasonForOT", className: "center", width: "300" }];
                    //{ title: "Approval Status", data: "ApprovalStatus", className: "center", width: "150" }];
                break mysection1;
            }
            else if (viewmode == 'projectsheet') {
                dataurl = '/Services/NagrikService.asmx/webProjectSheet?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webProjectSheetSearch';
                colstructure = [
                    {
                        title: "ProjectSheet No", data: "ProjectSheetNo", width: "100",
                        
                    },
                    { title: "ProjectSheet Date", data: "ProjectSheetDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Site No", data: "SiteNo", className: "center", width: "150" },
                    {
                        title: "Remarks", data: "Remarks", className: "center", width: "300",
                        
                    }];
                break mysection1;
            }
            else if (viewmode == 'materialmovementinward') {
                dataurl = '/Services/NagrikService.asmx/webMaterialMovementInward?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webMaterialMovementInwardSearch';
                colstructure = [
                    {
                        title: "Trans. ID", data: "pkID", width: "100",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Trans.Date", data: "TransDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "S/O #", data: "OrderNo", className: "center", width: "150" },
                    {
                        title: "Employee Name", data: "EmployeeName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.EmployeeID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
                break mysection1;
            }
            else if (viewmode == 'materialmovementoutward') {
                dataurl = '/Services/NagrikService.asmx/webMaterialMovementOutward?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webMaterialMovementOutwardSearch';
                colstructure = [
                    {
                        title: "Trans. ID", data: "pkID", width: "100",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Trans.Date", data: "TransDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "S/O #", data: "OrderNo", className: "center", width: "150" },
                    {
                        title: "Employee Name", data: "EmployeeName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.EmployeeID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
                break mysection1;
            }
            else if (viewmode == 'jobworkmovementinward') {
                dataurl = '/Services/NagrikService.asmx/webJobWorkMovementInward?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webJobWorkMovementInwardSearch';
                colstructure = [
                    {
                        title: "Trans. ID", data: "pkID", width: "100",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Trans.Date", data: "TransDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "S/O #", data: "OrderNo", className: "center", width: "150" },
                    {
                        title: "Employee Name", data: "EmployeeName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.EmployeeID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
                break mysection1;
            }
            else if (viewmode == 'jobworkmovementoutward') {
                dataurl = '/Services/NagrikService.asmx/webJobWorkMovementOutward?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webJobWorkMovementOutwardSearch';
                colstructure = [
                    {
                        title: "Trans. ID", data: "pkID", width: "100",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Trans.Date", data: "TransDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "S/O #", data: "OrderNo", className: "center", width: "150" },
                    {
                        title: "Employee Name", data: "EmployeeName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.EmployeeID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
                break mysection1;
            }
            else if (viewmode == 'materialconsumption') {
                dataurl = '/Services/NagrikService.asmx/webMaterialConsumption?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webMaterialConsumptionSearch';
                colstructure = [
                    {
                        title: "ID", data: "pkID", width: "100",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Date", data: "ConsDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "S/O #", data: "OrderNo", className: "center", width: "150" },
                    {
                        title: "Employee Name", data: "EmployeeName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.EmployeeID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
                break mysection1;
            }
            else if (viewmode == 'inspection') {
                dataurl = '/Services/NagrikService.asmx/webInspectionList?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webInspectionListSearch';
                colstructure = [
                    {
                        title: "Trans. ID", data: "pkID", width: "100",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Trans.Date", data: "InspectionDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Customer Name", data: "CustomerName", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "S/O #", data: "OrderNo", className: "center", width: "150" },
                    {
                        title: "Employee Name", data: "EmployeeName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.EmployeeID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
                break mysection1;
            }
            else if (viewmode == 'shiftmaster') {
                dataurl = '/Services/NagrikService.asmx/webShiftMaster?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webShiftMasterSearch';
                colstructure = [
                    { title: "Shift Name", data: "ShiftName", className: "center", width: "300" },
                    { title: "Start Time", data: "StartTime" },
                    { title: "End Time", data: "EndTime" },
                    { title: "Full Day [Hrs]", data: "MinHrsFullDay" },
                    { title: "Half Day [Hrs]", data: "MinHrsHalfDay" }];

                break mysection1;
            }
            else if (viewmode == 'orgstructure') {
                dataurl = '/Services/NagrikService.asmx/webOrgStructureDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webOrgStructureDetailSearch';
                colstructure = [
                    {
                        title: "Branch/Dept/Division Name", data: "OrgName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.OrgCode + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Org.Code", data: "OrgCode" },
                    { title: "Org.Type", data: "OrgType" },
                    { title: "Report To", data: "ReportTo_OrgName" },
                    { title: "Status", data: "ActiveFlagDesc" }];

                break mysection1;
            }
            else if (viewmode == 'orgemployee') {
                dataurl = '/Services/NagrikService.asmx/webOrgEmployeeDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webOrgEmployeeDetailSearch';
                colstructure = [
                    {
                        title: "Employee Name", data: "EmployeeName", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Designation", data: "Designation", className: "center", width: "100" },
                    { title: "Department", data: "OrgName", className: "center", width: "100" },
                    { title: "Report To", data: "ReportToEmployeeName", className: "text-left", width: "150" },
                    { title: "Biometric ID", data: "CardNo", className: "center", width: "80" },
                    { title: "Shift", data: "ShiftName", className: "center", width: "80" },
                    { title: "Pay Type", data: "BasicPer", className: "center", width: "80" },
                    { title: "Fixed Salary", data: "FixedSalary", className: "center", width: "80" },
                    { title: "Email Address", data: "EmailAddress", className: "text-left", width: "180" }
                ];

                break mysection1;
            }
                //-------------------------Site Survey-------------
            else if (viewmode == 'sitesurvay') {
                dataurl = '/Services/NagrikService.asmx/webSiteSurvay?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webSiteSurvaySearch';
                colstructure = [
                    {
                        title: "Customer Name", data: "Customer", className: "text-left", width: "400",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Document No", data: "DocNo", className: "text-left", width: "180" },
                    { title: "Survay Date", data: "SurvayDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Sheet No", data: "SheetNo", className: "text-left", width: "200" },
                ];
                break mysection1;
            }
            else if (viewmode == 'sitesurvayreport') {
                dataurl = '/Services/NagrikService.asmx/webSiteSurvayReport?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webSiteSurvayReportSearch';
                colstructure = [
                    {
                        title: "Survey #", data: "SurveyID", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Customer Name", data: "CustomerName", width: "350",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Survey ID", data: "SurveyID", className: "text-left", width: "180" },
                    { title: "Visit Date", data: "VisitDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    
                ];
                break mysection1;
            }
            else if (viewmode == 'financialtransaction') {
                dataurl = '/Services/NagrikService.asmx/webFinancialTrans?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webFinancialTransSearch';
                colstructure = [
                    {
                        title: "ID", data: "pkID", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher No", data: "VoucherNo", type: "text", className: "center", width: "80" },
                    { title: "Voucher Date", data: "VoucherDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Voucher Type", data: "VoucherType", type: "text", className: "center", width: "80" },
                    { title: "RecPay", data: "RecPay", type: "text", className: "center", width: "200" },
                    {
                        title: "Voucher Amount", data: "VoucherAmount", type: "text", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<h6>' + data + '</h6>';
                            return data;
                        }
                    },
                    {
                        title: "Customer Name", data: "CustomerName", type: "text", className: "text-left limited", width: "200",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Account Name", data: "AccountName", type: "text", className: "center", width: "200" }];

                break mysection1;
            }
            else if (viewmode == 'cashvoucher') {
                dataurl = '/Services/NagrikService.asmx/webFinancialTrans?currPageNo=1&TrType=cash';
                dataurlsearch = '/Services/NagrikService.asmx/webFinancialTransSearch?TrType=cash';
                colstructure = [
                    {
                        title: "ID", data: "pkID", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher No", data: "VoucherNo", type: "text", className: "center", width: "80" },
                    { title: "Voucher Date", data: "VoucherDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Voucher Type", data: "VoucherType", type: "text", className: "center", width: "80" },
                    { title: "Invoice No", data: "InvoiceNoDetail", type: "text", className: "center", width: "80" },
                    {
                        title: "RecPay", data: "RecPay", type: "text", className: "center", width: "200",
                        render: function (data, type, row, meta) {
                            if (row.RecPay == 'Receivable')
                                data = '<span class="badge gradient-45deg-yellow-green black-text gradient-shadow">' + data + '</span>';
                            else
                                data = '<span class="badge waves-effect waves-light gradient-45deg-purple-deep-orange">' + data + '</span>';
                            return data;
                        }
                    },
                    {
                        title: "Voucher Amount", data: "VoucherAmount", type: "text", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<h6>' + data + '</h6>';
                            return data;
                        }
                    },
                    {
                        title: "Customer Name", data: "CustomerName", type: "text", className: "center", width: "200",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Account Name", data: "AccountName", type: "text", className: "text-left limited", width: "150" },
                    { title: "Employee Name", data: "EmployeeName", type: "text", className: "text-left limited", width: "150"}
                ];
                break mysection1;
            }
            else if (viewmode == 'bankvoucher') {
                dataurl = '/Services/NagrikService.asmx/webFinancialTrans?currPageNo=1&TrType=bank';
                dataurlsearch = '/Services/NagrikService.asmx/webFinancialTransSearch?TrType=bank';
                colstructure = [
                    {
                        title: "ID", data: "pkID", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher No", data: "VoucherNo", type: "text", className: "center", width: "80" },
                    { title: "Voucher Date", data: "VoucherDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Voucher Type", data: "VoucherType", type: "text", className: "center", width: "80" },
                    <%--<% if (hdnSerialKey.Value.Contains("TWS3-RT90-E22O-K88P"))  //TWS
                    { %>--%>
                        { title: "Invoice No", data: "InvoiceNoDetail", type: "text", className: "center", width: "80" },
                    <%--<% }  %>--%>
                    
                    {
                        title: "RecPay", data: "RecPay", type: "text", className: "center", width: "200",
                        render: function (data, type, row, meta) {
                            if (row.RecPay == 'Receivable')
                                data = '<span class="badge gradient-45deg-yellow-green black-text gradient-shadow">' + data + '</span>';
                            else
                                data = '<span class="badge waves-effect waves-light gradient-45deg-purple-deep-orange">' + data + '</span>';
                            return data;
                        }
                    },
                    {
                        title: "Voucher Amount", data: "VoucherAmount", type: "text", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<h6>' + data + '</h6>';
                            return data;
                        }
                    },
                    {
                        title: "Customer Name", data: "CustomerName", type: "text", className: "center", width: "200",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Account Name", data: "AccountName", type: "text", className: "center", width: "200" },
                    { title: "Employee Name", data: "EmployeeName", type: "text", className: "center", width: "200" }
                ];

                break mysection1;
            }
            else if (viewmode == 'pettycash') {
                dataurl = '/Services/NagrikService.asmx/webPettyCash?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webPettyCashSearch';
                colstructure = [
                    {
                        title: "ID", data: "pkID", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher No", data: "VoucherNo", type: "text", className: "center", width: "80" },
                    { title: "Voucher Date", data: "VoucherDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Debit A/c", data: "DBCustomerName", type: "text", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.DBCustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Credit A/c", data: "CRCustomerName", type: "text", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CRCustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher Amount", data: "VoucherAmount", type: "text", className: "center", width: "150" }];

                break mysection1;
            }
            else if (viewmode == 'expensevoucher') {
                dataurl = '/Services/NagrikService.asmx/webExpenseVoucher?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webExpenseVoucherSearch';
                colstructure = [
                    {
                        title: "ID", data: "pkID", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher No", data: "VoucherNo", type: "text", className: "center", width: "80" },
                    { title: "Voucher Date", data: "VoucherDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Debit A/c", data: "DBCustomerName", type: "text", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.DBCustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Credit A/c", data: "CRCustomerName", type: "text", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CRCustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher Amount", data: "VoucherAmount", type: "text", className: "center", width: "150" }];

                break mysection1;
            }
            else if (viewmode == 'debitnote' || viewmode == 'debitnotenew') {
                dataurl = '/Services/NagrikService.asmx/webDBCRNote?currPageNo=1&DBC=DBNT';
                dataurlsearch = '/Services/NagrikService.asmx/webDBCRNoteSearch?DBC=DBNT';
                colstructure = [
                    {
                        title: "ID", data: "pkID", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher No", data: "VoucherNo", type: "text", className: "center", width: "80" },
                    { title: "Voucher Date", data: "VoucherDate", type: "date", className: "text-left limited", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Debit A/c", data: "DBCustomerName", type: "text", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.DBCustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Credit A/c", data: "CRCustomerName", type: "text", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CRCustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher Amount", data: "VoucherAmount", type: "text", className: "center", width: "150" }];

                break mysection1;
            }
            else if (viewmode == 'creditnote' || viewmode == 'creditnotenew') {
                dataurl = '/Services/NagrikService.asmx/webDBCRNote?currPageNo=1&DBC=CRNT';
                dataurlsearch = '/Services/NagrikService.asmx/webDBCRNoteSearch?DBC=CRNT';
                colstructure = [
                    {
                        title: "ID", data: "pkID", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher No", data: "VoucherNo", type: "text", className: "center", width: "80" },
                    { title: "Voucher Date", data: "VoucherDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Debit A/c", data: "DBCustomerName", type: "text", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.DBCustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Credit A/c", data: "CRCustomerName", type: "text", className: "text-left limited", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CRCustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher Amount", data: "VoucherAmount", type: "text", className: "center", width: "150" }];

                break mysection1;
            }
            else if (viewmode == 'journalvoucher') {
                dataurl = '/Services/NagrikService.asmx/webJournalVoucher?currPageNo=1&DBC=CRNT';
                dataurlsearch = '/Services/NagrikService.asmx/webJournalVoucherSearch?DBC=CRNT';
                colstructure = [
                    {
                        title: "ID", data: "pkID", className: "center", width: "80",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Voucher No", data: "VoucherNo", type: "text", className: "center", width: "80" },
                    { title: "Voucher Date", data: "VoucherDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Voucher Amount", data: "VoucherAmount", type: "text", className: "center", width: "150" },
                    { title: "Remarks", data: "Remarks", className: "limited", width: "200" }
                ];

                break mysection1;
            }
            else if (viewmode == 'todo') {
                dataurl = '/Services/NagrikService.asmx/webToDoDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webToDoDetailSearch';
                colstructure = [
                    {
                        title: "Task Description", data: "TaskDescription", className: "limited", width: "200",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Reminder", data: "Reminder", className: "center", width: "10",
                        render: function (data, type, row, meta) {
                            if (row.Reminder == 1)
                                data = "<div class='material-icons background-round gradient-45deg-purple-deep-orange gradient-shadow white-text tooltipped ml-1 mr-1' style='position:relative; padding:0px;'><i class='material-icons'>check</i></div>";
                            else
                                data = "";
                            return data;
                        }
                    },
                    { title: "Priority", data: "Priority", className: "center", width: "60" },
                    { title: "Start Date", data: "StartDate", type: "date", className: "center", width: "90", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Due Date", data: "DueDate", type: "date", className: "center", width: "90", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Completion Date", data: "CompletionDate", type: "date", className: "center", width: "100",
                        render: function (data, type, full) {
                            var year = moment(data).format("YYYY");
                            if (year < 1900) {
                                return 'In Progress';
                            }
                            else {
                                return moment(data).format('DD-MMM-YYYY');
                            }
                        }
                    },
                    { title: "Status", data: "TaskStatus", className: "center", width: "120" },
                    { title: "Assigned To", data: "EmployeeName", className: "center", width: "150" },
                    { title: "Assigned From", data: "FromEmployeeName", className: "center", width: "150" }
                ];

                break mysection1;
            }
            else if (viewmode == 'missedpunch') {
                dataurl = '/Services/NagrikService.asmx/webMissedPunch?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webMissedPunchSearch';
                colstructure = [

                    { title: "EmployeeName", data: "EmployeeName", className: "center", width: "100" },
                    { title: "Missed Date", data: "PresenceDate", type: "date", className: "center", width: "90", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Time IN", data: "TimeIn", className: "center", width: "90" },
                    { title: "Time Out", data: "TimeOut", className: "center", width: "120" },
                    { title: "Notes", data: "Notes", className: "limited", width: "200" },
                    { title: "Initiated By", data: "CreatedBy", className: "center-align", width: "100" },
                    { title: "Initiated On", data: "CreatedDate", className: "center-align", width: "100", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                ];
                break mysection1;
            }
            else if (viewmode == 'loan') {
                dataurl = '/Services/NagrikService.asmx/webLoanInstallment?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webLoanInstallmentSearch';
                colstructure = [

                    { title: "EmployeeName", data: "EmployeeName", className: "center", width: "100" },
                    { title: "Start Date", data: "StartDate", type: "date", className: "center", width: "90", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Due Date", data: "EndDate", type: "date", className: "center", width: "90", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Loan Amount", data: "LoanAmount", className: "center", width: "120" },
                    { title: "No.Of Install.", data: "NoOfInstallments", className: "center", width: "150" },
                    { title: "Install.Amount", data: "InstallmentAmount", className: "center", width: "150" },
                    { title: "Remarks", data: "Remarks", className: "limited", width: "200" },
                    { title: "Initiated By", data: "CreatedBy", className: "center-align", width: "100" },
                    { title: "Initiated On", data: "CreatedDate", className: "center-align", width: "100", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                ];
                break mysection1;
            }
            else if (viewmode == 'advance') {
                dataurl = '/Services/NagrikService.asmx/webAdvanceSalary?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webAdvanceSalarySearch';
                colstructure = [
                    { title: "EmployeeName", data: "EmployeeName", className: "center", width: "100" },
                    { title: "Advance Amount", data: "LoanAmount", className: "center", width: "120" },
                    { title: "Pay Month", data: "StartDate", type: "date", className: "center", width: "90", render: function (data, type, full) { return (data) ? moment(data).format('MMM-YYYY') : ''; } },
                    { title: "Remarks", data: "Remarks", className: "limited", width: "200" },
                    { title: "Initiated By", data: "CreatedBy", className: "center-align", width: "100" },
                    { title: "Initiated On", data: "CreatedDate", className: "center-align", width: "100", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                ];
                break mysection1;
            }
            else if (viewmode == 'users') {
                pageTitle = "Manage - Application Users";
                dataurl = '/Services/NagrikService.asmx/webUsersDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webUsersDetailSearch';
                colstructure = [
                    { title: "Screen Name", data: "ScreenFullName", className: "text-left", width: "210" },
                    { title: "User ID", data: "UserID", className: "center", width: "100" },
                    { title: "Organization", data: "CompanyName", className: "text-left", width: "220" },
                    { title: "Department/Division", data: "OrgName", className: "text-left", width: "180" },
                    { title: "Role", data: "RoleName", className: "center", width: "200" },
                    { title: "Employee", data: "EmployeeName", className: "text-left", width: "200" },
                    { title: "Status", data: "ActiveFlagDesc", className: "center", width: "80" }];

                break mysection1;
            }
            else if (viewmode == 'roles') {
                dataurl = '/Services/NagrikService.asmx/webRolesDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webRolesDetailSearch';
                colstructure = [
                    { title: "User Role", data: "Description", className: "text-left", width: "210" },
                    { title: "Role ID", data: "RoleCode", className: "center", width: "100" },
                    { title: "Description", data: "Comments", className: "limited center", width: "300" }];

                break mysection1;
            }
            else if (viewmode == 'cluster') {
                dataurl = '/Services/NagrikService.asmx/webClusterDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webClusterDetailSearch';
                colstructure = [
                    {
                        title: "Cluster Name", data: "ClusterName",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.StateCode + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Country", data: "CountryName" },
                    { title: "State", data: "StateName" },
                    { title: "City", data: "CityName" }
                ];

                break mysection1;
            }
            else if (viewmode == 'salesbill' || viewmode == 'salesbillnew') {
                dataurl = '/Services/NagrikService.asmx/webSalesBillDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webSalesBillDetailSearch';
                colstructure = [
                    { title: "Invoice Date", data: "InvoiceDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Invoice #", data: "InvoiceNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    <% if (hdnSerialKey.Value.Contains("TWS3-RT90-E22O-K88P"))  //TWS
                    { %>
                    //{
                        //title: "Job Card #", data: "DocRefNoList", className: "limitedWrap center", width: "250"
                    //},
                    <% }  %>
                    <% else if (hdnSerialKey.Value.Contains("STX1-UP06-YU89-JK23"))  //Stainex
                    { %>
                    {
                        title: "Lead/Quot/SO #", data: "ReferenceNo", className: "limitedWrap center", width: "250"
                    },
                    <% }  %>
                    <% else
                    { %>
                    {
                        title: "Lead/Quot/SO #", data: "DocRefNoList", className: "limitedWrap center", width: "250"
                    },
                    <% } %>
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Supplier Ref", data: "SupplierRef", className: "center", width: "90" },
                    <% if (hdnSerialKey.Value.Contains("TWS3-RT90-E22O-K88P"))  //TWS
                       { %>
                    { title: "Vehicle Reg No", data: "VehicleNo", className: "center", width: "90" },
                    { title: "Location", data: "LocationName", className: "center", width: "90" },
                    <% }; %>
                    { title: "GST #", data: "GSTNO", className: "center limited", width: "100" },
                    { title: "BasicAmt", data: "BasicAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "DiscountAmt", data: "DiscountAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "TaxAmt", data: "TaxAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "ROffAmt", data: "ROffAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "NetAmt", data: "NetAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') }
                ];

                break mysection1;
            }
            else if (viewmode == 'SalesChallan') {
                dataurl = '/Services/NagrikService.asmx/webSalesChallanDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webSalesChallanDetailSearch';
                colstructure = [
                    { title: "Challan Date", data: "ChallanDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Challan #", data: "ChallanNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Ref.#", data: "RefNo", className: "center", width: "90" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Supplier Ref", data: "SupplierRef", className: "center", width: "90" },
                    { title: "GST #", data: "GSTNO", className: "center", width: "90" },
                    { title: "BasicAmt", data: "BasicAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "DiscountAmt", data: "DiscountAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "TaxAmt", data: "TaxAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "ROffAmt", data: "ROffAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "NetAmt", data: "NetAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') }
                ];

                break mysection1;
            }
            else if (viewmode == 'purchasebill' || viewmode == 'purchasebillnew') {
                dataurl = '/Services/NagrikService.asmx/webPurchaseBillDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webPurchaseBillDetailSearch';
                colstructure = [
                    { title: "Invoice Date", data: "InvoiceDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Invoice #", data: "InvoiceNo", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Indent/PO/Inward #", data: "DocRefNoList", className: "limitedWrap center", width: "90" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "GST #", data: "GSTNO", className: "center", width: "90" },
                    { title: "BillNo", data: "BillNo", className: "text-right", width: "150" },
                    { title: "BasicAmt", data: "BasicAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "DiscountAmt", data: "DiscountAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "TaxAmt", data: "TaxAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "ROffAmt", data: "ROffAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "NetAmt", data: "NetAmt", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') }
                ];

                break mysection1;
            }
            else if (viewmode == 'Expense') {
                dataurl = '/Services/NagrikService.asmx/webExpenseDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webExpenseDetailSearch';
                colstructure = [
                    {
                        title: "Voucher #", data: "pkID", width: "100", className: "center",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Voucher Date", data: "ExpenseDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; }
                    },

                    { title: "Expense Type", data: "ExpenseTypeName", width: "200" },
                    { title: "Amount", data: "Amount", className: "text-right", width: "150", render: jQuery.fn.dataTable.render.number(',', '.', 2, '') },
                    { title: "Expense Notes", data: "ExpenseNotes" },
                    { title: "Employee Name", data: "EmployeeName" }
                ];

                break mysection1;
            }
            else if (viewmode == 'multiexpense') {
                dataurl = '/Services/NagrikService.asmx/webMultiExpense?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webMultiExpenseSearch';
                colstructure = [
                    {
                        title: "Voucher #", data: "pkID", width: "100", className: "center",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Voucher Date", data: "ExpenseDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; }
                    },
                ];

                break mysection1;
            }
            else if (viewmode == 'complaint') {
                para1 = jQuery.trim(jQuery("#drpComplainStatus").val());
                dataurl = '/Services/NagrikService.asmx/webComplaintList?ComplaintStatus=' + para1  + '&currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webComplaintListSearch?ComplaintStatus=' + para1;
                colstructure = [
                    {
                        title: "Complaint #", data: "ComplaintNo", className: "center", width: "90",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Date", data: "ComplaintDate", type: "date", className: "center", width: "130", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Complaint Notes", data: "ComplaintNotes", className: "limited", width: "250" },
                    { title: "Ref. #", data: "ReferenceNo", className: "center", width: "100" },
                    { title: "Assigned From", data: "CreatedByEmployee", className: "center", width: "150" },
                    { title: "Assigned To", data: "EmployeeName", className: "center", width: "150" },
                    {
                        title: "Status", data: "ComplaintStatus", className: "center", width: "100",
                        render: function (data, type, row, meta){
                            if (row.ComplaintStatus == 'Open')
                                data = '<p class="mb-0 pill green white-text ">&nbsp;' + row.ComplaintStatus + '&nbsp;</p>';
                            else if (row.ComplaintStatus == 'Close')
                                data = '<p class="mb-0 pill red white-text ">&nbsp;' + row.ComplaintStatus + '&nbsp;</p>';
                            return data;
                        }
                    },
                    <% if (hdnSerialKey.Value.Contains("ZE5W-HOME-AG41-SF61"))
                       { %>
                            { title: "Sch.Date", data: "ScheduleDate", type: "date", className: "center", width: "130", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    <% } %>
                    <% else { %>
                            { title: "Sch.Date", data: "PreferredDate", type: "date", className: "center", width: "130", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    <% } %>

                    {
                        title: "Sch.Time", data: "TimeFrom", className: "center", width: "130",
                        render: function (data, type, row, meta) {
                            return (row.TimeFrom + " - " + row.TimeTo);
                        }
                    },
                ];

                break mysection1;
            }
            else if (viewmode == 'complaintvisit') {
                dataurl = '/Services/NagrikService.asmx/webComplaintVisitList?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webComplaintVisitListSearch';
                colstructure = [
                    {
                        title: "Visit ID", data: "pkID", className: "center", width: "90",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Date", data: "ComplaintDate", type: "date", className: "center", width: "130", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Complaint #", data: "ComplaintNoString", className: "center", width: "100" },
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:openCustomerInfo(\'View\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Visit Type", data: "VisitType", className: "center", width: "150" },
                    { title: "Charge Type", data: "VisitChargeType", className: "center", width: "150" },
                    { title: "Visit Charge", data: "VisitCharge", className: "center", width: "180" },
                    { title: "Assigned From", data: "CreatedByEmployee", className: "center", width: "150" },
                    { title: "Assigned To", data: "EmployeeName", className: "center", width: "150" },
                    { title: "Sch.Date", data: "PreferredDate", type: "date", className: "center", width: "130", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Sch.Time", data: "TimeFrom", className: "center", width: "130",
                        render: function (data, type, row, meta) {
                            return (row.TimeFrom + " - " + row.TimeTo);
                        }
                    },
                ];

                break mysection1;
            }
            else if (viewmode == 'wallet') {
                dataurl = '/Services/NagrikService.asmx/webWalletDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webWalletDetailSearch';
                colstructure = [
                    {
                        title: "Wallet Name", data: "WalletName",

                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];

                break mysection1;
            }
            else if (viewmode == 'expensetype') {
                dataurl = '/Services/NagrikService.asmx/webExpenseTypeDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webExpenseTypeDetailSearch';
                colstructure = [
                    {
                        title: "ExpenseType Name", data: "ExpenseTypeName",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'view\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                { title: "Location Required", data: "IsLocationRequired" }];

                break mysection1;
            }

            }
            // -------------------------------------------------------------------------
            // Storing DataUrl for Export Purpose
            // -------------------------------------------------------------------------
            jQuery("#hdnDataUrl").val(dataurl);
            jQuery("#hdnDataUrlSearch").val(dataurlsearch);
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = jQuery("#hdnTitle").val()
            pageTitle = jQuery("#hdnTitle").val();
            spnPageHeader.innerHTML = "";

            // -------------------------------------------------------------------------
            // Configure Edit/Delete Button Display 
            // -------------------------------------------------------------------------
            if (jQuery("#hdnActionFlag").val() == 'true') {
                if (viewmode == 'orgstructure') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.OrgCode + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.OrgCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'users') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.UserID + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.UserID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'roles') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.RoleCode + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.RoleCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'cluster') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.RoleCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.RoleCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'projectsheet') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.RoleCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.RoleCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Project Sheet</span></div>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'jobcard') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';

                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';

                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';

                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Job Card</span></div>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'quotation' || viewmode == 'quotationsolar' || viewmode == 'complaintvisit' || viewmode == 'visitaccu' || viewmode == 'salesorder'
                    || viewmode == 'salesordernew' || viewmode == 'salesorderdealer' || viewmode == 'purchaseorder' || viewmode == 'workordercomm' || viewmode == 'salesbill'
                    || viewmode == 'SalesChallan' || viewmode == 'purchasebill' || viewmode == 'purchasebillnew' || viewmode == 'materialmovementoutward' || viewmode == 'outward'
                    || viewmode == 'jobcardoutward' || viewmode == 'indent' || viewmode == 'cashvoucher' || viewmode == 'bankvoucher' || viewmode == 'inward' || viewmode == 'orgemployee') {
                    
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/><span>Edit Record</span></div>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/><span>Delete Record</span></div>';

                            if (viewmode == 'salesorder' || viewmode == 'salesordernew') {
                                if ($("#hdnSerialKey").val() == 'HONP-MEDF-9RTS-FG10') {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printextra\',\'' + row.pkID + '\');"/><span>Sales Order</span></div>';
                                }
                                // print S.O
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Sales Order</span></div>';
                                // print Performa
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printpi\',\'' + row.pkID + '\');"/><span>Proforma Invoice</span></div>';
                                if ($("#hdnSerialKey").val() == 'JAYJ-ALAR-AMBR-ICKS') {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/expand.png" alt="" style="width:15px; height:15px;" onclick="openContFollowFromQuatation(\'add\',\'' + row.pkID + '\');"/>';
                                }
                            }
                            else if (viewmode == 'orgemployee') //Accu Panel
                            {
                                if ($("#hdnSerialKey").val() == 'ACSI-C803-CUP0-SHEL')
                                   {
                                    // print Org Employee
                                        rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printorgemployee\',\'' + row.pkID + '\');"/><span>Org Employee</span></div>';
                                   }
                            }
                            else if (viewmode == 'salesorderdealer') {
                                // print D.S.O
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Sales Order</span></div>';
                                // print Performa
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printpi\',\'' + row.pkID + '\');"/><span>Proforma Invoice</span></div>';
                            }
                            else if (viewmode == 'purchaseorder' || viewmode == 'workordercomm') {
                                // print P.O
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Purchase Order</span></div>';
                                // print D.O (Delivery Order)
                                if ($("#hdnSerialKey").val() == 'H0PX-EMRW-23IJ-C1TD')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printdo\',\'' + row.pkID + '\');"/><span>Delivery Order</span></div>';
                            }
                            else if (viewmode == 'salesbill' || viewmode == 'salesbillnew') {
                                if ($("#hdnSerialKey").val() == '8YWQ-DDRO-V98V-LDN2')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printchallan\',\'' + row.pkID + '\');"/><span>Delivery Challan</span></div>';
                                if ($("#hdnSerialKey").val() == 'TJ7S-06Q2-8R2U-KJWG' || $("#hdnSerialKey").val() == 'J63H-F8LX-B4B2-GYVZ' || $("#hdnSerialKey").val() == 'EUSI-LI85-4SL5-88GT')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printchallan\',\'' + row.pkID + '\');"/><span>Delivery Challan</span></div>';
                                if ($("#hdnSerialKey").val() == 'D33J-H872-3545-71A1' || $("#hdnSerialKey").val() == '6GZP-BW7W-78DF-HG88' || $("#hdnSerialKey").val() == 'PI01-YU02-RUBB-03ER')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printchallan\',\'' + row.pkID + '\');"/><span>Delivery Challan</span></div>';
                                if ($("#hdnSerialKey").val() == 'JAYJ-ALAR-AMBR-ICKS')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printchallan\',\'' + row.pkID + '\');"/><span>Packing Order</span></div>';
                                if ($("#hdnSerialKey").val() == 'PRI9-DG8H-G6GF-TP5V') {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'pridomestic\',\'' + row.pkID + '\');"/><span>Domestic</span></div>';
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printinvoice\',\'' + row.pkID + '\');"/><span>Invoice</span></div>';
                                }
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Sales Invoice</span></div>';

                                if ($("#hdnSerialKey").val() == 'ARAN-WR5K-U7D4-LN9F')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printchallan\',\'' + row.pkID + '\');"/><span>Delivery Challan</span></div>';
                                if ($("#hdnSerialKey").val() == 'ACSI-C803-CUP0-SHEL')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printchallan\',\'' + row.pkID + '\');"/><span>Delivery Challan</span></div>';


                            }
                            else if (viewmode == 'SalesChallan') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Sales Challan</span></div>';
                            }
                            else if (viewmode == 'cashvoucher') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Cash Voucher</span></div>';
                            }
                            else if (viewmode == 'bankvoucher') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Bank Voucher</span></div>';
                            }
                            else if (viewmode == 'materialmovementoutward') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printmovement\',\'' + row.pkID + '\');"/><span>Material Movement</span></div>';
                            }
                            else if (viewmode == 'purchasebill' || viewmode == 'purchasebillnew') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printpurchase\',\'' + row.pkID + '\');"/><span>Purchase Bill</span></div>';
                            }
                            else if (viewmode == 'outward') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printoutward\',\'' + row.pkID + '\');"/><span>Outward Details</span></div>';

                                if ($("#hdnSerialKey").val() == 'HONP-MEDF-9RTS-FG10')
                                {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printhnmed\',\'' + row.pkID + '\');"/><span>Customer DT Sticker</span></div>';
                                }
                            }
                            else if (viewmode == 'inward') {
                                if ($("#hdnSerialKey").val() == 'HONP-MEDF-9RTS-FG10') {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printhnmed\',\'' + row.pkID + '\');"/><span>Customer DT Sticker</span></div>';
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/><span>Customer DT Sticker</span></div>';
                                }
                            }
                            else if (viewmode == 'jobcardoutward') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printjobcardoutward\',\'' + row.pkID + '\');"/><span>JobCard Outward Details</span></div>';
                            }
                            else if (viewmode == 'indent') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'indent\',\'' + row.pkID + '\');"/><span>Indent Details</span></div>';
                            }
                            else if (viewmode == 'visitaccu') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printvisitacupanel\',\'' + row.pkID + '\');"/><span>Visit Complaint</span></div>';
                            }
                            else if (viewmode == 'complaintvisit')
                            {       // Print for Parth Kitchen (PDF)
                                if ($("#hdnSerialKey").val() == 'PART-KIT9-SIHC-CHEN')
                                    {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img class="tooltipped" src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printcomplaintvisitParthKitchen\',\'' + row.pkID + '\');"/><span>Complaint Visit</span></div>';
                                    }
                                }
                            else {
                                // Print Normal Module Document (PDF)
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/>';
                            }
                            // ---------------------------------------------------

                            if (viewmode == 'quotation' || viewmode == 'quotationsolar') {
                                if ($("#hdnSerialKey").val() == '4JM1-E874-JBK0-5HAN') {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printremarks\',\'' + row.pkID + '\');"/><span>Quatation</span></div>';
                                }
                                if ($("#hdnSerialKey").val() == 'SBR2-SI89-GH89-KI90') {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printremarks\',\'' + row.pkID + '\');"/><span>Quatation</span></div>';
                                }
                                if ($("#hdnSerialKey").val() == 'HONP-MEDF-9RTS-FG10') {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printHonmad\',\'' + row.pkID + '\');"/><span>Quatation</span></div>';
                                }


                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/recaptcha.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'regenerate\',\'' + row.pkID + '\');"/><span>Quatation</span></div>';
                                if ($("#hdnSerialKey").val() == 'JAYJ-ALAR-AMBR-ICKS') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/expand.png" alt="" style="width:15px; height:15px;" onclick="openContFollowFromQuatation(\'add\',\'' + row.pkID + '\');"/>';
                                }
                            }
                            // ----------------------------------------------------------
                            if ($("#hdnConversationLog").val().toLowerCase() == 'yes' && viewmode != 'complaintvisit' && viewmode != 'orgemployee')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/ChatBox.png" alt="" style="width:15px; height:15px;" onclick="ShowConverationLog(\'' + row.QuotationNo + '\');"/><span>Conversation Log</span></div>';

                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'inquiry' || viewmode == 'inquiryjjb')
                {
                    var InquiryShare = jQuery.trim(jQuery("#hdnInquiryShare").val().toLowerCase());
                    var InquiryShareRoles = jQuery.trim(jQuery("#hdnInquiryShareRoles").val().toLowerCase());
                    var InquiryAssign = jQuery.trim(jQuery("#hdnInquiryAssign").val().toLowerCase());

                    var userRole = jQuery.trim(jQuery("#hdnLoginUserRole").val().toLowerCase());
                    var chatboxlog = jQuery.trim(jQuery("#hdnConversationLog").val().toLowerCase());
                    // ---------------------------------------------------------------
                    if (InquiryShare == "yes")
                    {
                        colstructure.splice(0, 0, {
                            title: "Action", className: "center", width: "60", mRender: function (data, type, row) {
                                var rtnval = "";
                                rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                                if (InquiryShareRoles.indexOf('#' + userRole + '#') >= 0 || (userRole == 'admin')) {
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/multi-user-icon.jpg" alt="" style="width:20px; height:20px;" onclick="gridAction(\'share\',\'' + row.pkID + '\');"/>';
                                }

                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/QuickQuote.png" alt="Generate Quote" style="width:20px; height:20px;" onclick="openQuoteFromInquiry(' + row.pkID + ');"/><span>Generate Quotation</span></div>';

                                if ($("#hdnEditFlag").val() == 'true')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';

                                if ($("#hdnDelFlag").val() == 'true')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';

                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/expand.png" alt="" style="width:15px; height:15px;" onclick="openContFollowFromInquiry(\'add\',\'' + row.pkID + '\');"/>';
                                // ----------------------------------------------------------

                                if ($("#hdnConversationLog").val().toLowerCase() == 'yes')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/ChatBox.png" alt="" style="width:15px; height:15px;" onclick="ShowConverationLog(\'' + row.InquiryNo + '\');"/><span>Conversation Log</span></div>';

                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                                return rtnval;
                            }
                        });
                    }
                    else
                    {
                        colstructure.splice(0, 0, {
                            title: "Action", className: "center", width: "60", mRender: function (data, type, row) {
                                var rtnval = "";
                                rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                                if ($("#hdnEditFlag").val() == 'true')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                                if ($("#hdnDelFlag").val() == 'true')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + '<%# Eval("pkID") %>' + '\');"/>';
                                // ----------------------------------------------------------
                                if (chatboxlog == 'true')
                                    rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/ChatBox.png" alt="" style="width:15px; height:15px;" onclick="ShowConverationLog(\'' + row.InquiryNo + '\');"/><span>Conversation Log</span></div>';

                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                                return rtnval;
                            }
                        });
                    }
                }
                else if (viewmode == 'externalleads' || viewmode == 'telecaller') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            if (viewmode == 'externalleads')
                            {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<a class="tooltipped" data-position="center" data-tooltip="Add Followup" href="javascript:openContFollowFromExternalLeads(\'add\',\'' + row.pkID + '\');"><img src="images/expand.png" alt="" style="width:15px; height:15px;"/></a>';
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<a class="tooltipped" data-position="center" data-tooltip="View Timeline" href="javascript:openTimeline(\'followupext\',\'' + row.pkID + '\',\'Hellow\');"><img src="images/timeline3.png" alt="" style="width:15px; height:15px;"/></a>';
                            }
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'campaigntemplate') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.CampaignID + '\');"/><span>Edit Record</span></div>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.CampaignID + '\');"/><span>Delete Record</span></div>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'shiftmaster') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.ShiftCode + '\');"/><span>Edit Record</span></div>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.ShiftCode + '\');"/><span>Delete Record</span></div>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else {
                    
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/><span>Edit Record</span></div>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<div class="click-to-top"><img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/><span>Delete Record</span></div>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
            }
            // -------------------------------------------------------------------------
            // Design and Load Table as per above configuration 
            // ------------------------------------------------------------------------- 

    var gridTable;
    jQuery.ajax({
        url: dataurl,
        type: "GET",
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
            if (resultdata != null && resultdata != '') {
                $("#spnPageNo").text("Page # : " + $("#hdnCurrPageNo").val());
                //jQuery.ajax({
                //    url: "Services/NagrikService.asmx/getSessionObject",
                //    type: "GET",
                //    dataType: "json",
                //    success: function (datapageno) {
                //        debugger; // three
                //        var resultdata11 = JSON.parse(datapageno.d);
                //        var myTotalRecord1 = resultdata11[0].value;
                //        if (myTotalRecord1 != null && myTotalRecord1 != '') {
                //            $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                //        }
                //    }
                //});
            }
            // ----------------------------------------------
            //var sortingColNo = 0;
            //var sortingOrder = "asc";
            sortingDef = [[0, "asc"]];
            // ----------------------------------------------
            if (viewmode == 'inquiry' || viewmode == 'inquiryjjb' || viewmode == 'contractinfo' || viewmode == 'visitorinfo' || viewmode == 'courierinfo' || viewmode == 'quotation' || viewmode == 'quotationsolar' || viewmode == 'leaverequest') {
                //sortingColNo = 0;
                //sortingOrder = "desc";
                sortingDef = [[2, "asc"]];
            }
            else if (viewmode == 'followup')
            {
                sortingDef = [[1, "asc"]];
            }
            else if (viewmode == 'salesorder' || viewmode == 'salesordernew' || viewmode == 'purchaseorder' || viewmode == 'workordercomm') {
                sortingDef = [[0, "desc"], [1, "desc"]];
            }
            else if (viewmode == 'salesorderclinic') {
                sortingDef = [[0, "desc"], [1, "desc"]];
            }
            else if (viewmode == 'salesorderdealer') {
                sortingDef = [[0, "desc"], [1, "desc"]];
            }
            else if (viewmode == 'salesorderdealerclinic') {
                sortingDef = [[0, "desc"], [1, "desc"]];
            }
            else if (viewmode == 'inquirystatus') {
                //sortingColNo = 1;
                //sortingOrder = "asc";
                sortingDef = [[1, "asc"]];
            }
            if (viewmode == 'externalleads' || viewmode == 'telecaller') {
                //sortingColNo = 1;
                //sortingOrder = "desc";
                sortingDef = [[2, "desc"]];
            }
            // --------------------------------------------------
            // Loading and Drawing Datatable .... From Database
            // --------------------------------------------------
            //alert(sortingDef)
            $('#tblView').DataTable({
                data: resultdata,
                columns: colstructure,
                deferRender: true,
                "ordering": false,
                "bFilter": false,
                "bInfo": false,
                "bPaginate": false,
                order: sortingDef,
                //"scrollY": 350,
                "scrollX": true,
                "bScrollCollapse": false,
                createdRow: function (row, data, dataIndex) { setTableRowFormat(viewmode, row, data, dataIndex) }
            });

            // ----------------------------------------------
            //var currPage = $.session.get('tblCurrPage');
            //if (typeof currPage === 'undefined')        // Set default to First Page
            //    jQuery('#tblView').dataTable().fnPageChange(0, true);
            //else
            //    jQuery('#tblView').dataTable().fnPageChange(currPage - 1, true);
        },
        error: function (r) {
            alert('Error : ' + r.responseText);
        },
        failure: function (r) {
            alert('failure');
        }
    });
            // ----------------------------------------
            // Hiding AddNew button ...
            // ----------------------------------------
    if (viewmode == 'externalleads') {
        jQuery("#myAddNew").hide();
    }
            // ------------------------------------------
            //$.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().fixedColumns().relayout();
}

function setTableRowFormat(viewmode, row, data, dataIndex) {
    if (viewmode == 'externalleads') {
        //jQuery("td:eq(0)", row).css("font-weight", "bold");
        //jQuery("td:eq(0)", row).css("color", "Red");
    }
    if (viewmode == 'inquiry' || viewmode == 'inquiryjjb') {
        //jQuery("td:eq(4),td:eq(2)", row).css("font-weight", "bold");
        if (jQuery.trim(data.InquiryStatus) == 'Open' || jQuery.trim(data.InquiryStatus) == 'On Hold' || data.InquiryStatus == 'Work In Progress') {
            jQuery("td", row).css("color", "Blue");
        }
        else if (jQuery.trim(data.InquiryStatus) == 'Close - Lost') {
            jQuery("td", row).css("color", "Red");
        }
        else if (jQuery.trim(data.InquiryStatus) == 'Close - Success') {
            jQuery("td", row).css("color", "Green");
        }
    }
    if (viewmode == 'courierinfo') {
        if (jQuery.trim(data.AcceptanceType) == 'Sender') {
            jQuery("td:eq(4)", row).css("background-color", "red");
            jQuery("td:eq(4)", row).css("color", "white");
        }
        else {
            jQuery("td:eq(4)", row).css("background-color", "light grey");
            jQuery("td:eq(4)", row).css("color", "white");
        }
    }
    if (viewmode == 'followup') {
        jQuery("td:eq(1)", row).css("font-weight", "bold");
        if (jQuery.trim(data.NoFollowup) != '') {
            jQuery("td:eq(3)", row).css("color", "Red");
            jQuery("td:eq(4)", row).css("color", "Maroon");
        }
    }
    if (viewmode == 'quotation' || viewmode == 'quotationsolar') {
        jQuery("td:eq(3),td:eq(4)", row).css("font-weight", "bold");
        jQuery("td:eq(4)", row).css("color", "navy");
    }
    if (viewmode == 'projects' || viewmode == 'customer') {
        jQuery("td:eq(0)", row).css("color", "DarkBlue");
        jQuery("td:eq(0)", row).css("font-weight", "bold");
    }
    if (viewmode == 'products') {
        if (jQuery.trim(data.ActiveFlagDesc) == 'Active')
            jQuery("td", row).css("color", "black");
        else
            jQuery("td", row).css("color", "red");
    }
    if (viewmode == 'leaverequest') {
        jQuery("td:eq(3),td:eq(4)", row).css("font-weight", "bold");
        jQuery("td:eq(4)", row).css("color", "red");
    }
    if (viewmode == 'overtime') {
        jQuery("td:eq(3),td:eq(4)", row).css("font-weight", "bold");
        jQuery("td:eq(4)", row).css("color", "red");
    }
    if (viewmode == 'todo') {
        if (data.Priority == 'Low') {
            //jQuery(row).css("background-color", "Orange");
            jQuery("td:eq(3)", row).css("background-color", "#ffc107");
            jQuery("td:eq(3)", row).css("color", "White");
        }
        if (data.Priority == 'Medium') {
            jQuery("td:eq(3)", row).css("background-color", "#9c27b0");
            jQuery("td:eq(3)", row).css("color", "White");
        }
        if (data.Priority == 'High') {
            jQuery("td:eq(3)", row).css("background-color", "#f44336");
            jQuery("td:eq(3)", row).css("color", "White");
        }
        // ---------------------------
        if (data.TaskStatus == 'Overdue') {
            //jQuery("td:eq(0),td:eq(2),td:eq(3),td:eq(4),td:eq(5)", row).css("color", "Red");
            jQuery("td:eq(5)", row).css("background-color", "Red");
            jQuery("td:eq(5)", row).css("color", "White");
        }
    }
}

//------------------------------------------------------------------------------
function openContFollowFromInquiry(mode, InqID) {
    Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage FollowUp";
    var pageUrl = "Followup.aspx?mode=" + mode + "&id=0" + "&InqID=" + InqID;
    $('#ifrModule').attr('src', pageUrl);
    $find("mpe").show();
}

//------------------------------------------------------------------------------
function openContFollowFromQuatation(mode, QuotID) {
    Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage FollowUp";
    var pageUrl = "QuatationFollowUp.aspx?mode=" + mode + "&id=0" + "&QuotID=" + QuotID;
    $('#ifrModule').attr('src', pageUrl);
    $find("mpe").show();
}

        //------------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Function : This will invoke by Edit/Delete button
        // -------------------------------------------------------------------------

        function gridAction(mode, id)
        {
            
            var cbWidth = "90%";
            var cbHeight = "90%";
            var varView = jQuery("#hdnView").val();
            //alert('Module : ' + mode)
            var pageUrl = '', deleteUrl = '', deletePara = '';
            // -------------------------------------------------------------
            if (varView == 'customer') {
                pageUrl = "Customers.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Customers.aspx/DeleteCustomer';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'inquiry') {
                pageUrl = "InquiryInfo.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'InquiryInfo.aspx/DeleteInquiry';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'inquiryjjb') {
                pageUrl = "InquiryInfoJJB.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'InquiryInfoJJB.aspx/DeleteInquiry';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'followup') {
                var cbWidth = "80%";
                pageUrl = "Followup.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Followup.aspx/DeleteFollowup';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'shiftmaster') {
                var cbWidth = "80%";
                pageUrl = "ShiftMaster.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ShiftMaster.aspx/DeleteShiftMaster';
                deletePara = '{ShiftCode:' + id + '}';
            }
            else if (varView == 'todo') {
                var cbWidth = "60%";
                var cbHeight = "85%";
                pageUrl = "ToDo.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ToDo.aspx/DeleteToDo';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'missedpunch') {
                var cbWidth = "60%";
                var cbHeight = "85%";
                pageUrl = "MissedPunch.aspx?view=" + varView + "&mode=" + mode + "&id=" + id;
                deleteUrl = 'MissedPunch.aspx/DeleteMissedPunch';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'loan') {
                var cbWidth = "60%";
                var cbHeight = "85%";
                pageUrl = "Loan.aspx?view=" + varView + "&mode=" + mode + "&id=" + id;
                deleteUrl = 'Loan.aspx/DeleteLoan';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'advance') {
                var cbWidth = "60%";
                var cbHeight = "85%";
                pageUrl = "Loan.aspx?view=" + varView + "&mode=" + mode + "&id=" + id;
                deleteUrl = 'Loan.aspx/DeleteLoan';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'quotation') {
                var QuotationVersion = jQuery.trim(jQuery("#hdnQuotationVersion").val());
                if (QuotationVersion == "1" || QuotationVersion == "2") {
                    var cbWidth = "100%";
                    var cbHeight = "100%";
                    pageUrl = "Quotation_old.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'Quotation_old.aspx/DeleteQuotation';
                    deletePara = '{pkID:' + id + '}';
                }
                else {
                    if ($("#hdnSerialKey").val() == 'COL1-AKL9-TEC9-SJ99') // Coldtech
                    {
                        pageUrl = "Quotation_Coldtech.aspx?mode=" + mode + "&id=" + id;
                        deleteUrl = 'Quotation_Coldtech.aspx/DeleteQuotation';
                        deletePara = '{pkID:' + id + '}';
                    }
                    else {
                        pageUrl = "Quotation.aspx?mode=" + mode + "&id=" + id;
                        deleteUrl = 'Quotation.aspx/DeleteQuotation';
                        deletePara = '{pkID:' + id + '}';
                    }
                }
            }
            else if (varView == 'quotationsolar') {
                pageUrl = "Quotation_Solar.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Quotation_Solar.aspx/DeleteQuotation';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'salesorder') {
                var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                if (SalesOrderVersion == "1") {
                    var cbWidth = "100%";
                    var cbHeight = "100%";
                    if ($("#hdnSerialKey").val() == 'TWS3-RT90-E22O-K88P')  //TWS
                    {
                        pageUrl = "SalesOrder_TWS.aspx?mode=" + mode + "&id=" + id;
                        deleteUrl = 'SalesOrder_TWS.aspx/DeleteSalesOrder';
                    }
                    else {
                        pageUrl = "SalesOrder_Old.aspx?mode=" + mode + "&id=" + id;
                        deleteUrl = 'SalesOrder_Old.aspx/DeleteSalesOrder';
                    }
                        deletePara = '{pkID:' + id + '}';
                }
                else {
                    if ($("#hdnSerialKey").val() == 'TWS3-RT90-E22O-K88P')  //TWS
                    {
                        pageUrl = "SalesOrder_TWS.aspx?mode=" + mode + "&id=" + id;
                        deleteUrl = 'SalesOrder_TWS.aspx/DeleteSalesOrder';
                    }
                    else {
                        pageUrl = "SalesOrder.aspx?mode=" + mode + "&id=" + id;
                        deleteUrl = 'SalesOrder.aspx/DeleteSalesOrder';
                    }
                    deletePara = '{pkID:' + id + '}';
                }
            }
            else if (varView == 'salesordernew') {
                var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                if (SalesOrderVersion == "1") {
                    var cbWidth = "100%";
                    var cbHeight = "100%";
                    pageUrl = "SalesOrder_Old.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'SalesOrder_Old.aspx/DeleteSalesOrder';
                    deletePara = '{pkID:' + id + '}';
                }
                else {
                    pageUrl = "SalesOrderNew.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'SalesOrderNew.aspx/DeleteSalesOrder';
                    deletePara = '{pkID:' + id + '}';
                }
            }
            else if (varView == 'salesorderdealer') {
                var cbWidth = "100%";
                var cbHeight = "100%";
                pageUrl = "SalesOrderDealer.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'SalesOrderDealer.aspx/DeleteSalesOrderDealer';
                deletePara = '{pkID:' + id + '}';
                //}
            }
            else if (varView == 'purchaseorder') {
                var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                pageUrl = "PurchaseOrders.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'PurchaseOrders.aspx/DeletePurchaseOrder';
                deletePara = '{pkID:' + id + '}';

            }
            else if (varView == 'workordercomm') {
                var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                pageUrl = "WorkOrderComm.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'WorkOrderComm.aspx/DeleteWorkOrderComm';
                deletePara = '{pkID:' + id + '}';

            }
            else if (varView == 'contractinfo') {
                pageUrl = "ContractInfo.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ContractInfo.aspx/DeleteContractInfo';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'inward') {
                pageUrl = "inward.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'inward.aspx/Deleteinward';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'comaccu') {
                pageUrl = "ComplaintAcupanel.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ComplaintAcupanel.aspx/DeleteComplaint';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'visitaccu') {
                pageUrl = "VisitAcupanel.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'VisitAcupanel.aspx/DeleteComplaintVisit';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'production') {
                pageUrl = "Molding.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Molding.aspx/DeleteMolding';
                deletePara = '{pkID:' + id + '}';
            }

            else if (varView == 'productionagain') {
                pageUrl = "Production.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Production.aspx/DeleteProduction';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'productionbyso') {
                pageUrl = "ProductionBySo.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ProductionBySo.aspx/DeleteProductionBySo';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'internalworkorder') {
                pageUrl = "InternalWorkOrder.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'InternalWorkOrder.aspx/DeleteInternalWorkOrder';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'jobcard') {
                pageUrl = "JobCard.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'JobCard.aspx/DeleteJobCard';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'jobcardinward') {
                pageUrl = "JobCardInward.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'JobCardInward.aspx/DeleteJobCardinward';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'indent') {
                pageUrl = "MaterialIndent.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'MaterialIndent.aspx/DeleteMaterialIndent';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'outward') {
                var OutwardVersion = jQuery.trim(jQuery("#hdnOutwardVersion").val());
                if (OutwardVersion == "2")
                {
                    pageUrl = "outwardwithassembly.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'outwardwithassembly.aspx/DeleteOutward';
                }
                else {
                    pageUrl = "outward.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'outward.aspx/DeleteOutward';
                }
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'jobcardoutward') {
                pageUrl = "JobCardOutward.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'JobCardoutward.aspx/DeleteJobCardOutward';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'visitorinfo') {
                var cbWidth = "75%";
                var cbHeight = "90%";
                pageUrl = "VisitorInfo.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'VisitorInfo.aspx/DeleteVisitorInfo';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'courierinfo') {
                pageUrl = "CourierInfo.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'CourierInfo.aspx/DeleteCourierInfo';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'salestarget') {
                cbWidth = "70%";
                cbHeight = "90%";
                pageUrl = "salestarget.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'salestarget.aspx/DeleteSalesTarget';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'broadcast') {
                cbWidth = "60%";
                cbHeight = "80%";
                pageUrl = "BroadcastMessage.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'BroadcastMessage.aspx/DeleteBroadcastMessage';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'vehiclemaster') {
                cbWidth = "60%";
                cbHeight = "80%";
                pageUrl = "VehicleMaster.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'VehicleMaster.aspx/DeleteVehicle';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'vehiclemastertrip') {
                cbWidth = "60%";
                cbHeight = "80%";
                pageUrl = "VehicleMasterTrip.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'VehicleMasterTrip.aspx/DeleteVehicleTrip';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'transporter') {
                cbWidth = "60%";
                cbHeight = "80%";
                pageUrl = "TransporterMaster.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'TransporterMaster.aspx/DeleteTransporter';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'leaverequest') {
                cbWidth = "55%";
                cbHeight = "85%";
                pageUrl = "LeaveRequest.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'LeaveRequest.aspx/DeleteLeaveRequest';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'overtime') {
                cbWidth = "55%";
                cbHeight = "85%";
                pageUrl = "OverTime.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'OverTime.aspx/DeleteOverTime';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'products') {
                pageUrl = "Products.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Products.aspx/DeleteProduct';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'orgstructure') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "OrgStructure.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'OrgStructure.aspx/DeleteOrgStructure';
                deletePara = '{OrgCode:\'' + id + '\'}';
            }
            else if (varView == 'orgemployee') {
                var cbWidth = "75%";
                var cbHeight = "90%";
                pageUrl = "OrgEmployee.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'OrgEmployee.aspx/DeleteOrgEmployee';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'users') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "UserMgmt.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'UserMgmt.aspx/DeleteUsers';
                deletePara = '{UserID:\'' + id + '\'}';
            }
            else if (varView == 'roles') {
                var cbWidth = "95%";
                var cbHeight = "95%";
                pageUrl = "ManageRole.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ManageRole.aspx/DeleteRole';
                deletePara = '{RoleCode:\'' + id + '\'}';
            }
            else if (varView == 'financialtransaction') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "FinancialTrans.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'FinancialTrans.aspx/DeleteFinancialTrans';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'sitesurvay') {
                var cbWidth = "75%";
                var cbHeight = "90%";
                pageUrl = "SolarSiteSurvey.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'SolarSiteSurvey.aspx/DeleteSiteSurvay';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'sitesurvayreport') {
                var cbWidth = "75%";
                var cbHeight = "90%";
                pageUrl = "SiteSurveyReport.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'SiteSurveyReport.aspx/DeleteSiteSurvayReport';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'cashvoucher') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "CashVoucher.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'CashVoucher.aspx/DeleteFinancialTrans';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'bankvoucher') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "BankVoucher.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'BankVoucher.aspx/DeleteFinancialTrans';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'pettycash') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "PettyCash.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'PettyCash.aspx/DeletePettyCash';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'expensevoucher') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "ExpenseVoucher.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ExpenseVoucher.aspx/DeleteExpenseVoucher';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'debitnote') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "DebitCreditNoteNew.aspx?DBC=DBNT&mode=" + mode + "&id=" + id;
                deleteUrl = 'DebitCreditNoteNew.aspx/DeleteDBCRNote';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'creditnote') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "DebitCreditNoteNew.aspx?DBC=CRNT&mode=" + mode + "&id=" + id;
                deleteUrl = 'DebitCreditNoteNew.aspx/DeleteDBCRNote';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'debitnotenew') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "DebitCreditNoteNew.aspx?DBC=DBNT&mode=" + mode + "&id=" + id;
                deleteUrl = 'DebitCreditNoteNew.aspx/DeleteDBCRNote';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'creditnotenew') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "DebitCreditNoteNew.aspx?DBC=CRNT&mode=" + mode + "&id=" + id;
                deleteUrl = 'DebitCreditNoteNew.aspx/DeleteDBCRNote';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'journalvoucher') {
                var cbWidth = "80%";
                var cbHeight = "85%";
                pageUrl = "JournalVoucher.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'JournalVoucher.aspx/DeleteJournalVoucher';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'salesbill') {
                var cbWidth = "95%";
                var cbHeight = "85%";
                if ($("#hdnSerialKey").val() == 'TWS3-RT90-E22O-K88P')  //TWS
                {
                    pageUrl = "SalesBill_TWS.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'SalesBill_TWS.aspx/DeleteSalesBill';
                }
                else
                {
                    pageUrl = "SalesBill.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'SalesBill.aspx/DeleteSalesBill';
                }
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'SalesChallan') {
                var cbWidth = "95%";
                var cbHeight = "85%";
                pageUrl = "SalesChallan.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'SalesChallan.aspx/DeleteSalesChallan';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'purchasebill') {
                var cbWidth = "95%";
                var cbHeight = "85%";
                pageUrl = "PurchaseBill.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'PurchaseBill.aspx/DeletePurchaseBill';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'purchasebillnew') {
                var cbWidth = "95%";
                var cbHeight = "85%";
                pageUrl = "PurchaseBillNew.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'PurchaseBillNew.aspx/DeletePurchaseBill';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'productspecification') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Product_Specification.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Product_Specification.aspx/DeleteProductSpecification';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'campaigntemplate') {
                var cbWidth = "60%";
                var cbHeight = "90%";
                pageUrl = "CampaignTemplate.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'CampaignTemplate.aspx/DeleteCampaignTemplate';
                deletePara = '{CampaignID:\'' + id + '\'}';
            }
            else if (varView == 'constant') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "ManageConstant.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ManageConstant.aspx/DeleteConstant';
                deletePara = '{ConstantCode:\'' + id + '\'}';
            }
            else if (varView == 'Expense') {
                //var cbWidth = "65%";
                //var cbHeight = "85%";
                pageUrl = "Expense.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Expense.aspx/DeleteExpense';
                deletePara = '{ExpenseID:\'' + id + '\'}';
            }
            else if (varView == 'multiexpense') {
                //var cbWidth = "65%";
                //var cbHeight = "85%";
                pageUrl = "MultiExpense.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'MultiExpense.aspx/DeleteExpense';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'complaint') {
                $("#pnlMyModule").width("80%");
                var cbWidth = "80%";
                var cbHeight = "95%";
                if (($("#hdnSerialKey").val() == 'ADE9-8SD7-JU89-DF46') || ($("#hdnSerialKey").val() == 'ZE5W-HOME-AG41-SF61'))    // Jaquel - Quick Complaint
                    pageUrl = "ComplaintQuick.aspx?mode=" + mode + "&id=" + id;
                else
                    pageUrl = "Complaint.aspx?mode=" + mode + "&id=" + id;

                deleteUrl = 'Complaint.aspx/DeleteComplaint';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'complaintvisit') {
                var cbWidth = "80%";
                var cbHeight = "95%";
                if ($("#hdnSerialKey").val() == 'ADE9-8SD7-JU89-DF46')     // Jaquel - Quick Complaint
                    pageUrl = "ComplaintVisitQuick.aspx?mode=" + mode + "&id=" + id;
                else
                pageUrl = "ComplaintVisit.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ComplaintVisit.aspx/DeleteComplaintVisit';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'externalleads') {
                var cbWidth = "90%";
                var cbHeight = "90%";
                pageUrl = "ExternalLeads.aspx?source=indiamart&mode=" + mode + "&id=" + id
                deleteUrl = 'ExternalLeads.aspx/DeleteExternalLeads';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'telecaller') {
                var cbWidth = "90%";
                var cbHeight = "90%";
                pageUrl = "TeleCaller.aspx?source=telecaller&mode=" + mode + "&id=" + id
                deleteUrl = 'TeleCaller.aspx/DeleteExternalLeads';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'wallet') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Wallet.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Wallet.aspx/DeleteWallet';
                deletePara = '{WalletID:\'' + id + '\'}';
            }
            else if (varView == 'materialmovementinward') {
                pageUrl = "MaterialMovement.aspx?transtype=in&mode=" + mode + "&id=" + id;
                deleteUrl = 'MaterialMovement.aspx/DeleteTransaction';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'projectsheet') {
                pageUrl = "ProjectSheet.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ProjectSheet.aspx/DeleteProjectSheet';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'materialmovementoutward') {
                pageUrl = "MaterialMovement.aspx?transtype=out&mode=" + mode + "&id=" + id;
                deleteUrl = 'MaterialMovement.aspx/DeleteTransaction';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'jobworkmovementoutward') {
                pageUrl = "JobWork_Movement.aspx?transtype=out&mode=" + mode + "&id=" + id;
                deleteUrl = 'JobWork_Movement.aspx/DeleteTransaction';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'jobworkmovementinward') {
                pageUrl = "JobWork_Movement.aspx?transtype=in&mode=" + mode + "&id=" + id;
                deleteUrl = 'JobWork_Movement.aspx/DeleteTransaction';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'materialconsumption') {
                pageUrl = "MaterialConsumption.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'MaterialConsumption.aspx/DeleteTransaction';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'inspection') {
                pageUrl = "inspection.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'inspection.aspx/DeleteTransaction';
                deletePara = '{pkID:' + id + '}';
            }
            // -------------------------------------------------------------
            // Section : Call page for ADD / EDIT
            // -------------------------------------------------------------
            if (mode == 'add' || mode == 'edit' || mode == 'view') {
                pageUrl = pageUrl + "&passFlag=" + $("#hdnAddFlag").val();
                document.getElementById('spnModuleHeader').innerText = pageTitle;

                $('#ifrModule').attr('src', pageUrl);
                $find("mpe").show();
            }
            // -------------------------------------------------------------
            // Section : Call page for DELETE 
            // -------------------------------------------------------------
            if (mode == 'delete') {
                swal({
                    title: "Delete Record !", text: "Are you sure? You want to delete record !", icon: 'warning', dangerMode: true,
                    buttons: { cancel: 'No, Please!', delete: 'Yes, Delete It' }
                }).then(function (willDelete) {
                    if (willDelete) {
                        jQuery.ajax({
                            type: "POST",
                            url: deleteUrl,
                            data: deletePara,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                var resultdata = JSON.parse(data.d);
                                swal(resultdata[0].ReturnMsg, { title: 'Record Deleted', icon: "success", });
                                // -----------------------------------
                                gridReloadData('reload');
                                //location.reload();
                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                    } else {
                        swal("Your Delete Action Cancelled ! ", { title: 'Cancelled', icon: "error", });
                    }
                });
            }
            // -------------------------------------------------------------
            // Section : Call page for REGENERATE QUOTATION REVISION
            // -------------------------------------------------------------
            if (mode == 'regenerate') {
                var userid = jQuery("#hdnLoginUserID").val();
                var returnID = 0;
                jQuery.ajax({
                    type: "POST",
                    url: 'Quotation.aspx/GenerateQuotationRevision',
                    data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        returnID = resultdata[0].ReturnCode;
                        if (returnID > 0) {
                            var pageUrl = 'Quotation.aspx?id=' + returnID
                            $('#ifrModule').attr('src', pageUrl);
                            $find("mpe").show();
                        }
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }

            // -------------------------------------------------------------
            // Section : QUOTATION PRINTING 
            // -------------------------------------------------------------
            if ((mode == 'print') && (varView == 'quotation' || varView == 'quotationsolar')) {
                var QuotPageUrl = '', QuotationVersion = '', QuotationNoForPDF = '';
                QuotationVersion = jQuery.trim(jQuery("#hdnQuotationVersion").val());
                QuotationNoForPDF = '';
                // ----------------------------------------------------------------
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print Quotation Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            // -------------------------------------------------------------------
                            if (QuotationVersion == "1" || QuotationVersion == "2") {
                                QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                            }
                            else {
                                QuotPageUrl = 'Quotation.aspx/GenerateQuotation';
                            }
                            // ======================================================
                            // Special Section For Crystal Quotation Printing 
                            // ======================================================
                            jQuery.ajax({
                                type: "POST",
                                url: QuotPageUrl,
                                data: '{pQuotID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            QuotationNoForPDF = (data11.d).replace("/", "-");
                                            if (QuotationNoForPDF != "") {
                                                ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Quotation PDF Not Found !')
                                            }

                                        }
                                    });
                                    // -----------------------------------------------------------
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        } else {
                            var x = PageMethods.setPrintHeader('no');
                            // -------------------------------------------------------------------
                            if (QuotationVersion == "1" || QuotationVersion == "2") {
                                QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                            }
                            else {
                                QuotPageUrl = 'Quotation.aspx/GenerateQuotation';
                            }
                            // ======================================================
                            // Special Section For Crystal Quotation Printing 
                            // ======================================================
                            jQuery.ajax({
                                type: "POST",
                                url: QuotPageUrl,
                                data: '{pQuotID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            QuotationNoForPDF = (data11.d).replace("/", "-");
                                            if (QuotationNoForPDF != "") {
                                                ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Quotation PDF Not Found !')
                                            }

                                        }
                                    });
                                    // -----------------------------------------------------------
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    // -------------------------------------------------------------------
                    if (QuotationVersion == "1" || QuotationVersion == "2") {
                        QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                    }
                    else {
                        QuotPageUrl = 'Quotation.aspx/GenerateQuotation';
                    }
                    // ======================================================
                    // Special Section For Crystal Quotation Printing 
                    // ======================================================
                    jQuery.ajax({
                        type: "POST",
                        url: QuotPageUrl,
                        data: '{pQuotID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    QuotationNoForPDF = (data11.d).replace("/", "-");
                                    if (QuotationNoForPDF != "") {
                                        ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Quotation PDF Not Found !')
                                    }

                                }
                            });
                            // -----------------------------------------------------------
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }

            }
            //-------------------------------------------------------------------------

            if ((mode == 'printremarks') && (varView == 'quotation' || varView == 'quotationsolar')) {
                var QuotPageUrl = '', QuotationVersion = '', QuotationNoForPDF = '';
                QuotationVersion = jQuery.trim(jQuery("#hdnQuotationVersion").val());
                QuotationNoForPDF = '';
                // ----------------------------------------------------------------
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print Quotation Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            // -------------------------------------------------------------------
                            if (QuotationVersion == "1" || QuotationVersion == "2") {
                                QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                            }
                            else {
                                QuotPageUrl = 'Quotation.aspx/GenerateQuotationRemarks_Balaji';
                            }
                            // ======================================================
                            // Special Section For Crystal Quotation Printing 
                            // ======================================================
                            jQuery.ajax({
                                type: "POST",
                                url: QuotPageUrl,
                                data: '{pQuotID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            QuotationNoForPDF = (data11.d).replace("/", "-");
                                            if (QuotationNoForPDF != "") {
                                                ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Quotation PDF Not Found !')
                                            }
                                        }
                                    });
                                    // -----------------------------------------------------------
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        } else {
                            var x = PageMethods.setPrintHeader('no');
                            // -------------------------------------------------------------------
                            if (QuotationVersion == "1" || QuotationVersion == "2") {
                                QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                            }
                            else {
                                QuotPageUrl = 'Quotation.aspx/GenerateQuotationRemarks_Balaji';
                            }
                            // ======================================================
                            // Special Section For Crystal Quotation Printing 
                            // ======================================================
                            jQuery.ajax({
                                type: "POST",
                                url: QuotPageUrl,
                                data: '{pQuotID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            QuotationNoForPDF = (data11.d).replace("/", "-");
                                            if (QuotationNoForPDF != "") {
                                                ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Quotation PDF Not Found !')
                                            }

                                        }
                                    });
                                    // -----------------------------------------------------------
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    // -------------------------------------------------------------------
                    if (QuotationVersion == "1" || QuotationVersion == "2") {
                        QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                    }
                    else {
                        QuotPageUrl = 'Quotation.aspx/GenerateQuotation';
                    }
                    // ======================================================
                    // Special Section For Crystal Quotation Printing 
                    // ======================================================
                    jQuery.ajax({
                        type: "POST",
                        url: QuotPageUrl,
                        data: '{pQuotID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    QuotationNoForPDF = (data11.d).replace("/", "-");
                                    if (QuotationNoForPDF != "") {
                                        ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Quotation PDF Not Found !')
                                    }

                                }
                            });
                            // -----------------------------------------------------------
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }

            }

            if ((mode == 'printHonmad') && (varView == 'quotation' || varView == 'quotationsolar')) {
                var QuotPageUrl = '', QuotationVersion = '', QuotationNoForPDF = '';
                QuotationVersion = jQuery.trim(jQuery("#hdnQuotationVersion").val());
                QuotationNoForPDF = '';
                // ----------------------------------------------------------------
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print Quotation Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            // -------------------------------------------------------------------
                            if (QuotationVersion == "1" || QuotationVersion == "2") {
                                QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                            }
                            else {
                                QuotPageUrl = 'Quotation.aspx/GenerateQuotationRemarks_HONMED';
                            }
                            // ======================================================
                            // Special Section For Crystal Quotation Printing 
                            // ======================================================
                            jQuery.ajax({
                                type: "POST",
                                url: QuotPageUrl,
                                data: '{pQuotID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            QuotationNoForPDF = (data11.d).replace("/", "-");
                                            if (QuotationNoForPDF != "") {
                                                ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Quotation PDF Not Found !')
                                            }
                                        }
                                    });
                                    // -----------------------------------------------------------
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        } else {
                            var x = PageMethods.setPrintHeader('no');
                            // -------------------------------------------------------------------
                            if (QuotationVersion == "1" || QuotationVersion == "2") {
                                QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                            }
                            else {
                                QuotPageUrl = 'Quotation.aspx/GenerateQuotationRemarks_HONMED';
                            }
                            // ======================================================
                            // Special Section For Crystal Quotation Printing 
                            // ======================================================
                            jQuery.ajax({
                                type: "POST",
                                url: QuotPageUrl,
                                data: '{pQuotID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            QuotationNoForPDF = (data11.d).replace("/", "-");
                                            if (QuotationNoForPDF != "") {
                                                ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Quotation PDF Not Found !')
                                            }

                                        }
                                    });
                                    // -----------------------------------------------------------
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    // -------------------------------------------------------------------
                    if (QuotationVersion == "1" || QuotationVersion == "2") {
                        QuotPageUrl = 'Quotation_old.aspx/GenerateQuotation';
                    }
                    else {
                        QuotPageUrl = 'Quotation.aspx/GenerateQuotation';
                    }
                    // ======================================================
                    // Special Section For Crystal Quotation Printing 
                    // ======================================================
                    jQuery.ajax({
                        type: "POST",
                        url: QuotPageUrl,
                        data: '{pQuotID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    QuotationNoForPDF = (data11.d).replace("/", "-");
                                    if (QuotationNoForPDF != "") {
                                        ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Quotation PDF Not Found !')
                                    }

                                }
                            });
                            // -----------------------------------------------------------
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }

            }
            if (mode == 'printextra' && varView == 'salesorder') {
                var SOPageUrl = '';
                //var DSOPageUrl = '';
                var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                // =========================================================================
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            // --------------------------------------------------------------------
                            // Print SalesOrder as SalesOrder
                            // --------------------------------------------------------------------
                            SOPageUrl = 'SalesOrder.aspx/GenerateSalesOrder_HonmedExtra';
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Sales Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            // --------------------------------------------------------------------
                            // Print SalesOrder as SalesOrder
                            // --------------------------------------------------------------------
                            SOPageUrl = 'SalesOrder.aspx/GenerateSalesOrder_HonmedExtra';
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Sales Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    // --------------------------------------------------------------------
                    // Print SalesOrder as SalesOrder
                    // --------------------------------------------------------------------
                    SOPageUrl = 'SalesOrder.aspx/GenerateSalesOrder_HonmedExtra';
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Sales Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            // ----------------------------------------------------------------------
            if ((mode == 'print' || mode == 'printpi') && varView == 'salesorder') {
                var SOPageUrl = '';
                //var DSOPageUrl = '';
                var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                // =========================================================================
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print SO/PI Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            // --------------------------------------------------------------------
                            // Print SalesOrder as SalesOrder
                            // --------------------------------------------------------------------
                            if (mode == 'print') {
                                SOPageUrl = (SalesOrderVersion == "1") ? 'SalesOrder_Old.aspx/GenerateSalesOrder' : 'SalesOrder.aspx/GenerateSalesOrder';
                                //DSOPageUrl = 'SalesOrderDealer.aspx/GenerateSalesOrderDealer';
                            }
                            // --------------------------------------------------------------------
                            // Print SalesOrder as Perfoma Invoice
                            // --------------------------------------------------------------------
                            if (mode == 'printpi') {
                                SOPageUrl = 'SalesOrder.aspx/GenerateSalesPerfoma';
                                //DSOPageUrl = 'SalesOrderDealer.aspx/GenerateSalesOrderDealer';
                            }
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Sales Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            // --------------------------------------------------------------------
                            // Print SalesOrder as SalesOrder
                            // --------------------------------------------------------------------
                            if (mode == 'print') {
                                SOPageUrl = (SalesOrderVersion == "1") ? 'SalesOrder_Old.aspx/GenerateSalesOrder' : 'SalesOrder.aspx/GenerateSalesOrder';
                            }
                            // --------------------------------------------------------------------
                            // Print SalesOrder as Perfoma Invoice
                            // --------------------------------------------------------------------
                            if (mode == 'printpi') {
                                SOPageUrl = 'SalesOrder.aspx/GenerateSalesPerfoma';
                            }
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Sales Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    // --------------------------------------------------------------------
                    // Print SalesOrder as SalesOrder
                    // --------------------------------------------------------------------
                    if (mode == 'print') {
                        SOPageUrl = (SalesOrderVersion == "1") ? 'SalesOrder_Old.aspx/GenerateSalesOrder' : 'SalesOrder.aspx/GenerateSalesOrder';
                    }
                    // --------------------------------------------------------------------
                    // Print SalesOrder as Perfoma Invoice
                    // --------------------------------------------------------------------
                    if (mode == 'printpi') {
                        SOPageUrl = 'SalesOrder.aspx/GenerateSalesPerfoma';
                    }
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Sales Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            // ----------------------------------------------------------------------
            if ((mode == 'print' || mode == 'printpi') && varView == 'salesordernew') {
                var SOPageUrl = '';
                //var DSOPageUrl = '';
                var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                // =========================================================================
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print SO/PI Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            // --------------------------------------------------------------------
                            // Print SalesOrder as SalesOrderNew
                            // --------------------------------------------------------------------
                            if (mode == 'print') {
                                SOPageUrl = (SalesOrderVersion == "1") ? 'SalesOrder_Old.aspx/GenerateSalesOrder' : 'SalesOrderNew.aspx/GenerateSalesOrder';
                                //DSOPageUrl = 'SalesOrderDealer.aspx/GenerateSalesOrderDealer';
                            }
                            // --------------------------------------------------------------------
                            // Print SalesOrder as Perfoma Invoice
                            // --------------------------------------------------------------------
                            if (mode == 'printpi') {
                                SOPageUrl = 'SalesOrderNew.aspx/GenerateSalesPerfoma';
                                //DSOPageUrl = 'SalesOrderDealer.aspx/GenerateSalesOrderDealer';
                            }
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesOrderNew.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "") 
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Sales Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            // --------------------------------------------------------------------
                            // Print SalesOrder as SalesOrderNew
                            // --------------------------------------------------------------------
                            if (mode == 'print') {
                                SOPageUrl = (SalesOrderVersion == "1") ? 'SalesOrder_Old.aspx/GenerateSalesOrder' : 'SalesOrderNew.aspx/GenerateSalesOrder';
                            }
                            // --------------------------------------------------------------------
                            // Print SalesOrder as Perfoma Invoice
                            // --------------------------------------------------------------------
                            if (mode == 'printpi') {
                                SOPageUrl = 'SalesOrderNew.aspx/GenerateSalesPerfoma';
                            }
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesOrderNew.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Sales Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    // --------------------------------------------------------------------
                    // Print SalesOrder as SalesOrder
                    // --------------------------------------------------------------------
                    if (mode == 'print') {
                        SOPageUrl = (SalesOrderVersion == "1") ? 'SalesOrder_Old.aspx/GenerateSalesOrder' : 'SalesOrderNew.aspx/GenerateSalesOrder';
                    }
                    // --------------------------------------------------------------------
                    // Print SalesOrder as Perfoma Invoice
                    // --------------------------------------------------------------------
                    if (mode == 'printpi') {
                        SOPageUrl = 'SalesOrderNew.aspx/GenerateSalesPerfoma';
                    }
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'SalesOrderNew.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Sales Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            // ----------------------------------------------------------------------
            // Section : Purchase Order 
            // ----------------------------------------------------------------------
            if (mode == 'print' && varView == 'purchaseorder') {
                var SOPageUrl = '';
                SOPageUrl = 'PurchaseOrders.aspx/GeneratePurchaseOrder';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Purc.Order Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });

                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');

                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });

                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            if (mode == 'printdo' && varView == 'purchaseorder') {
                var SOPageUrl = '';
                SOPageUrl = 'PurchaseOrders.aspx/GeneratePurchaseDeliveryOrder';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Delivery Order Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Delivery Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }

            }

            // ----------------------------------------------------------------------
            // Section : Visite Complaint Acupanel 
            // ----------------------------------------------------------------------
            //-------------------------------------------------------------------------------------
            if (mode == 'printvisitacupanel' && (varView == 'visitaccu')) {
                debugger;
                var SOPageUrl = '';
                SOPageUrl = 'VisitAcupanel.aspx/GenerateVisitAcupanel';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Tax Invoice Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            debugger;
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        <% if (hdnSerialKey.Value.Contains(" "))
                                           { %>
                                        type: "POST", url: 'VisitAcupanel.aspx/GetVisitAcupanelNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        <% }
                                    else
                                    { %>
                                        type: "POST", url: 'VisitAcupanel.aspx/GetVisitAcupanelNoForPDF', data: '{pkID:\'' + id + '\'}',
                                   <% } %>
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            VisitAcuplanelNoPDF = (data11.d).replace("/", "-");
                                            if (VisitAcuplanelNoPDF != "") {
                                                ShowPDFfile('PDF/' + "VISITCOMP-" + VisitAcuplanelNoPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        <% if (hdnSerialKey.Value.Contains(" "))
                                           { %>
                                        type: "POST", url: 'VisitAcupanel.aspx/GetVisitAcupanelNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        <% }
                                    else
                                    { %>
                                        type: "POST", url: 'VisitAcupanel.aspx/GetVisitAcupanelNoForPDF', data: '{pkID:\'' + id + '\'}',
                                   <% } %>
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            VisitAcuplanelNoPDF = (data11.d).replace("/", "-");
                                            if (VisitAcuplanelNoPDF != "") {
                                                ShowPDFfile('PDF/' + "VISITCOMP-" + VisitAcuplanelNoPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                <% if (hdnSerialKey.Value.Contains(" "))
                                           { %>
                                type: "POST", url: 'VisitAcupanel.aspx/GetVisitAcupanelNoForPDF', data: '{pkID:\'' + id + '\'}',
                                <% }
                                    else
                                    { %>
                                type: "POST", url: 'VisitAcupanel.aspx/GetVisitAcupanelNoForPDF', data: '{pkID:\'' + id + '\'}',
                                   <% } %>
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    VisitAcuplanelNoPDF = (data11.d).replace("/", "-");
                                    if (VisitAcuplanelNoPDF != "") {
                                        ShowPDFfile('PDF/' + "VISITCOMP-" + VisitAcuplanelNoPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Sales Bill PDF Not Found !')
                                    }

                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            // ----------------------------------------------------------------------
            // Section :  Complaint  Visite Parth Kitchen 
            // ----------------------------------------------------------------------
            //-------------------------------------------------------------------------------------
            if (mode == 'printcomplaintvisitParthKitchen' && (varView == 'complaintvisit')) {
                debugger;
                var SOPageUrl = '';
                SOPageUrl = 'ComplaintVisit.aspx/GenerateComplaintVisit';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Complaint Visit Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            debugger;
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        <% if (hdnSerialKey.Value.Contains(" "))
                                           { %>
                                        type: "POST", url: 'ComplaintVisit.aspx/GetComplaintVisitNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        <% }
                                    else
                                    { %>
                                        type: "POST", url: 'ComplaintVisit.aspx/GetComplaintVisitNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        <% } %>
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            ComplaintVisitNoForPDF = (data11.d).replace("/", "-");
                                            if (ComplaintVisitNoForPDF != "") {
                                                ShowPDFfile('PDF/' + "Complaint Visit -" + ComplaintVisitNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Complaint Visit PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        <% if (hdnSerialKey.Value.Contains(" "))
                                           { %>
                                        type: "POST", url: 'ComplaintVisit.aspx/GetComplaintVisitNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        <% }
                                    else
                                    { %>
                                        type: "POST", url: 'ComplaintVisit.aspx/GetComplaintVisitNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        <% } %>
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            ComplaintVisitNoForPDF = (data11.d).replace("/", "-");
                                            if (ComplaintVisitNoForPDF != "") {
                                                ShowPDFfile('PDF/' + "Complaint Visit -" + ComplaintVisitNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Complaint Visit PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                <% if (hdnSerialKey.Value.Contains(" "))
                                           { %>
                                type: "POST", url: 'ComplaintVisit.aspx/GetComplaintVisitNoForPDF', data: '{pkID:\'' + id + '\'}',
                                <% }
                                    else
                                    { %>
                                type: "POST", url: 'ComplaintVisit.aspx/GetComplaintVisitNoForPDF', data: '{pkID:\'' + id + '\'}',
                                <% } %>
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    ComplaintVisitNoForPDF = (data11.d).replace("/", "-");
                                    if (ComplaintVisitNoForPDF != "") {
                                        ShowPDFfile('PDF/' + "Complaint Visit -" + ComplaintVisitNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Complaint Visit PDF Not Found !')
                                    }

                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            // ----------------------------------------------------------------------
            // Section : Purchase Order New
            // ----------------------------------------------------------------------
            if (mode == 'print' && varView == 'workordercomm') {
                var SOPageUrl = '';
                SOPageUrl = 'WorkOrderComm.aspx/GenerateWorkOrder';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Purc.Order Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'WorkOrderComm.aspx/GetWorkOrderCommNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });

                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');

                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'WorkOrderComm.aspx/GetWorkOrderCommNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });

                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'WorkOrderComm.aspx/GetWorkOrderCommNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            if (mode == 'printdo' && varView == 'purchaseordernew') {
                var SOPageUrl = '';
                SOPageUrl = 'PurchaseOrderNew.aspx/GeneratePurchaseDeliveryOrder';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Delivery Order Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseOrderNew.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseOrderNew.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            OrderNoForPDF = (data11.d).replace("/", "-");
                                            if (OrderNoForPDF != "")
                                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'PurchaseOrderNew.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Delivery Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }

            }

            if (mode == 'printpurchase' && varView == 'purchasebill') {
                var SOPageUrl = '';
                SOPageUrl = 'PurchaseBill.aspx/GeneratePurchaseBill';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Delivery Order Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseBill.aspx/GetPurchaseBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseBill.aspx/GetPurchaseBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'PurchaseBill.aspx/GetPurchaseBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Delivery Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            /*------------------------------------------------------------------*/
            if (mode == 'printpurchase' && varView == 'purchasebillnew') {
                var SOPageUrl = '';
                SOPageUrl = 'PurchaseBillNew.aspx/GeneratePurchaseBill';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Delivery Order Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseBillNew.aspx/GetPurchaseBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'PurchaseBillNew.aspx/GetPurchaseBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'PurchaseBillNew.aspx/GetPurchaseBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Delivery Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            // ----------------------------------------------------------------------
            // SALES BILL : for ALL OTHER except FEMICURE
            // ----------------------------------------------------------------------
            if (mode == 'print' && varView == 'salesbill') {
                var SOPageUrl = '';
                if ($("#hdnSerialKey").val() == 'TWS3-RT90-E22O-K88P')
                    SOPageUrl = 'SalesBill_TWS.aspx/GenerateSalesBill';
                else
                    SOPageUrl = 'SalesBill.aspx/GenerateSalesBill';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Tax Invoice Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            debugger;
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        <% if (hdnSerialKey.Value.Contains("TWS3-RT90-E22O-K88P"))
                                           { %>
                                        type: "POST", url: 'SalesBill_TWS.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        <% }
                                    else
                                    { %>
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                   <% } %>
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        <% if (hdnSerialKey.Value.Contains("TWS3-RT90-E22O-K88P"))
                                           { %>
                                        type: "POST", url: 'SalesBill_TWS.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        <% }
                                    else
                                    { %>
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                   <% } %>
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                <% if (hdnSerialKey.Value.Contains("TWS3-RT90-E22O-K88P"))
                                           { %>
                                        type: "POST", url: 'SalesBill_TWS.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                <% }
                                    else
                                    { %>
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                   <% } %>
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    SalesBillNoForPDF = (data11.d).replace("/", "-");
                                    if (SalesBillNoForPDF != "") {
                                        ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Sales Bill PDF Not Found !')
                                    }

                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            if (mode == 'pridomestic' && varView == 'salesbill') {
                var SOPageUrl = '';
                SOPageUrl = 'SalesBill.aspx/GenerateSalesBill_PRIDomestic';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Tax Invoice Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    SalesBillNoForPDF = (data11.d).replace("/", "-");
                                    if (SalesBillNoForPDF != "") {
                                        ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Sales Bill PDF Not Found !')
                                    }

                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            if (mode == 'printchallan' && varView == 'salesbill') {
                var SOPageUrl = '';
                SOPageUrl = 'SalesBill.aspx/GenerateSalesChallan';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Challan Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    SalesBillNoForPDF = (data11.d).replace("/", "-");
                                    if (SalesBillNoForPDF != "") {
                                        ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Sales Bill PDF Not Found !')
                                    }

                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            if (mode == 'printinvoice' && varView == 'salesbill') {
                var SOPageUrl = '';
                SOPageUrl = 'SalesBill.aspx/GenerateSalesBill_Invoice';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Invoice Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    SalesBillNoForPDF = (data11.d).replace("/", "-");
                                    if (SalesBillNoForPDF != "") {
                                        ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Sales Bill PDF Not Found !')
                                    }

                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            // ----------------------------------------------------------------------
            // SALES BILL NEW : for ALL OTHER except FEMICURE
            // ----------------------------------------------------------------------
            if (mode == 'print' && varView == 'SalesChallan') {
                var SOPageUrl = '';
                SOPageUrl = 'SalesChallan.aspx/GenerateSalesChallan';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Challan Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesChallan.aspx/GetSalesChallanNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Bill PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'SalesChallan.aspx/GetSalesChallanNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            SalesBillNoForPDF = (data11.d).replace("/", "-");
                                            if (SalesBillNoForPDF != "") {
                                                ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Sales Challan PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'SalesCHallan.aspx/GetSalesChallanNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    SalesBillNoForPDF = (data11.d).replace("/", "-");
                                    if (SalesBillNoForPDF != "") {
                                        ShowPDFfile('PDF/' + SalesBillNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Sales Challan PDF Not Found !')
                                    }

                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }


            //--------------------------------------------------------------------
            //----Section :  Organization Employee Printing Method For Accu Panel 
            //---------------------------------------------------------------------

            if (mode == 'printorgemployee' && varView == 'orgemployee') {
                var SOPageUrl = '';
                SOPageUrl = 'OrgEmployee.aspx/GenerateOrgEmpAccuPanel';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                jQuery.ajax({
                    type: "POST",
                    url: SOPageUrl,
                    data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        // -----------------------------------------------------------
                        jQuery.ajax({
                            type: "POST", url: 'OrgEmployee.aspx/GetOrgEmpAccuPanelNoForPDF', data: '{pkID:\'' + id + '\'}',
                            contentType: "application/json; charset=utf-8", success: function (data11) {
                                orgemployeeNoForPDF = (data11.d).replace("/", "-");
                                if (orgemployeeNoForPDF != "")
                                    ShowPDFfile('PDF/' + orgemployeeNoForPDF.toString() + '.pdf');
                                else
                                    alert('PDF Not Found !')
                            }
                        });
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }


            //--------------------------------------------------------------------
            //-------------------------------------Outward Hnmed Printing
            //---------------------------------------------------------------------

            if (mode == 'printhnmed' && varView == 'outward') {
                var SOPageUrl = '';
                SOPageUrl = 'Outward.aspx/GenerateOutwardhnmed';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
               
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Outward.aspx/GetOutwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
            }


            if (mode == 'printhnmed' && varView == 'inward') {
                var SOPageUrl = '';
                SOPageUrl = 'Inward.aspx/GenerateInwardhnmed';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                jQuery.ajax({
                    type: "POST",
                    url: SOPageUrl,
                    data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        // -----------------------------------------------------------
                        jQuery.ajax({
                            type: "POST", url: 'Inward.aspx/GetInwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                            contentType: "application/json; charset=utf-8", success: function (data11) {
                                BillNoForPDF = (data11.d).replace("/", "-");
                                if (BillNoForPDF != "")
                                    ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                else
                                    alert('PDF Not Found !')
                            }
                        });
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }


            if (mode == 'print' && varView == 'inward') {
                var SOPageUrl = '';
                SOPageUrl = 'Inward.aspx/GenerateInward';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Need Document Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Inward.aspx/GetInwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Inward.aspx/GetInwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'Inward.aspx/GetInwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            //--------------------------------------------------------------------
            //-------------------------------------------------------
            //---------------------------------------------------------------------

            //----------------------------------Main Outward Printing---------------------
            if (mode == 'printoutward' && varView == 'outward') {
                var SOPageUrl = '';
                SOPageUrl = 'Outward.aspx/GenerateOutward';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Delivery Order Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Outward.aspx/GetOutwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Outward.aspx/GetOutwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Purchase Delivery Order PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'Outward.aspx/GetOutwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Delivery Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            //-----------------------------Indent Print-------------------------------
            if (mode == 'indent' && varView == 'indent') {
                var SOPageUrl = '';
                SOPageUrl = 'MaterialIndent.aspx/GenerateIndent';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Indent Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'MaterialIndent.aspx/GetIndentNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Indent PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'MaterialIndent.aspx/GetIndentNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Indent PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'MaterialIndent.aspx/GetIndentNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Indent PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            //-----------------------------Job Card Indent Print--------------------------
            if (mode == 'printjobcardoutward' && varView == 'jobcardoutward') {
                var SOPageUrl = '';
                SOPageUrl = 'JobCardOutward.aspx/GenerateJobCardOutward';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "JobCard Outward Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'JobCardOutward.aspx/GetJobCardOutwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Job Card Delivery PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'JobCardOutward.aspx/GetJobCardOutwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('Job Card Delivery PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'JobCardOutward.aspx/GetJobCardOutwardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Job Card Delivery PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }
            if (mode == 'print' && varView == 'projectsheet') {
                var SOPageUrl = '';
                SOPageUrl = 'ProjectSheet.aspx/GenerateProjectSheet';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'ProjectSheet.aspx/GenerateProjectSheet', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'ProjectSheet.aspx/GetProjectNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'ProjectSheet.aspx/GetProjectNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            if (mode == 'print' && varView == 'jobcard') {
                var SOPageUrl = '';
                SOPageUrl = 'JobCard.aspx/GenerateJobCard_TWS';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Job Card Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'JobCard.aspx/GetJobCardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            GetJobCardNoForPDF = (data11.d).replace("/", "-");
                                            if (GetJobCardNoForPDF != "") {
                                                ShowPDFfile('PDF/' + GetJobCardNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Job Card PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'JobCard.aspx/GetJobCardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            GetJobCardNoForPDF = (data11.d).replace("/", "-");
                                            if (GetJobCardNoForPDF != "") {
                                                ShowPDFfile('PDF/' + GetJobCardNoForPDF.toString() + '.pdf');
                                            }
                                            else {
                                                alert('Job Card PDF Not Found !')
                                            }

                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            //ShowPDFfile('PDF/SalesOrder_' + id.toString() + '.pdf');
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'JobCard.aspx/GetJobCardNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    GetJobCardNoForPDF = (data11.d).replace("/", "-");
                                    if (GetJobCardNoForPDF != "") {
                                        ShowPDFfile('PDF/' + GetJobCardNoForPDF.toString() + '.pdf');
                                    }
                                    else {
                                        alert('Job Card PDF Not Found !')
                                    }

                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            if (mode == 'printmovement' && varView == 'materialmovementoutward') {
                var SOPageUrl = '';
                SOPageUrl = 'Inspection.aspx/GenerateMarterialMovementReport_Abhishek';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Inspection.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'Inspection.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'Inspection.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }


            if (mode == 'print' && varView == 'bankvoucher') {
                var SOPageUrl = '';
                SOPageUrl = 'BankVoucher.aspx/GenerateCashVoucher';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'BankVoucher.aspx/GetVoucherNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'BankVoucher.aspx/GetVoucherNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'BankVoucher.aspx/GetVoucherNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            if (mode == 'print' && varView == 'cashvoucher') {
                var SOPageUrl = '';
                SOPageUrl = 'CashVoucher.aspx/GenerateCashVoucher';
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    swal({
                        title: "Print Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                        buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                    }).then(function (willDelete) {
                        if (willDelete) {
                            var x = PageMethods.setPrintHeader('yes');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'CashVoucher.aspx/GetVoucherNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                        else {
                            var x = PageMethods.setPrintHeader('no');
                            jQuery.ajax({
                                type: "POST",
                                url: SOPageUrl,
                                data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    // -----------------------------------------------------------
                                    jQuery.ajax({
                                        type: "POST", url: 'CashVoucher.aspx/GetVoucherNoForPDF', data: '{pkID:\'' + id + '\'}',
                                        contentType: "application/json; charset=utf-8", success: function (data11) {
                                            BillNoForPDF = (data11.d).replace("/", "-");
                                            if (BillNoForPDF != "")
                                                ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                            else
                                                alert('PDF Not Found !')
                                        }
                                    });
                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'CashVoucher.aspx/GetVoucherNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    BillNoForPDF = (data11.d).replace("/", "-");
                                    if (BillNoForPDF != "")
                                        ShowPDFfile('PDF/' + BillNoForPDF.toString() + '.pdf');
                                    else
                                        alert('PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                }
            }

            // -------------------------------------------------------------
            // Section : Call page for Share Inquiry 
            // -------------------------------------------------------------
            if (mode == 'share') {
                $('#ifrModule').attr('src', "InquiryShare.aspx?id=" + id + "&userid=" + jQuery.trim(jQuery("#hdnLoginUserID").val()));
                $find("mpe").show();
            }

        }   

        function ShowPDFfile(repFilename) {

            var today = new Date();
            var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            var dateTime = date + ' ' + time;

            yhooWin = window.open(repFilename + "?id=" + dateTime, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }

        function showInquiryInfo(inqno) {
            jQuery.ajax({
                type: "POST",
                url: 'InquiryInfo.aspx/GetInquiryNoPrimaryID',
                data: '{pInqNo:\'' + inqno + '\'}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var id;
                    var resultdata = JSON.parse(data.d);
                    var returnID = parseInt(data.d);
                    if (returnID > 0) {
                        document.getElementById('spnModuleHeader').innerText = "Manage Lead";
                        $('#ifrModule').attr('src', "InquiryInfo.aspx?mode=view&id=" + returnID);
                        $find("mpe").show();
                    }
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });
        }

    </script>

</asp:Content>

