<%@ Page Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="ViewTeleCaller.aspx.cs" Inherits="StarsProject.ViewTeleCaller" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .limited {
            white-space: nowrap;
            max-width: 300px;
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
            font-size: 12px !important;
            padding: 3px 5px;
            vertical-align:text-top !important;
        }

        table.dataTable tbody td a {
            text-decoration: underline;
            color: blueviolet;

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

        .pill {
            border-radius: 12px;
            padding: 2px 8px;
            text-transform: capitalize;
        }
    </style>

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
        function openTimeline(xView, xCustID, xCustName) {
            var pageUrl = "FollowupTimeline.aspx?view=" + xView + "&id=" + xCustID + "&name=" + xCustName;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
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
                        spnModuleHeader.innerText = "Manage Lead";
                        $('#ifrModule').attr('src', "InquiryInfo.aspx?mode=view&id=" + returnID);
                        $find("mpe").show();
                    }
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });
        }


        function openContFollowFromTeleCaller(mode, ExtpkID) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage External FollowUp";
            var pageUrl = "FollowupExternal.aspx?followupsource=telecaller&mode=" + mode + "&id=0" + "&ExtpkID=" + ExtpkID;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server" ClientIDMode="Static" EnablePageMethods="true"></asp:ScriptManager>

    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLoginUserRole" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnActionFlag" runat="server" ClientIDMode="Static" Value="false" />
    <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnKeyID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTitle" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPara1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPara2" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPara3" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientERPIntegration" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnpopupPrintHeader" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDataUrl" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDataUrlSearch" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCurrPageNo" runat="server" ClientIDMode="Static" Value="1" />
    <asp:HiddenField ID="hdnCurrPageSize" runat="server" ClientIDMode="Static" EnableViewState="true" Value="10" />

    <asp:HiddenField ID="hdnMenuID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAddFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEditFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDelFlag" runat="server" ClientIDMode="Static" />
    <%--<asp:UpdatePanel ID="upMyModule" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
    <div class="row">
        <div class="col s12">
            <div class="card">
                <div class="card-content" style="padding: 8px;">
                    <%--<h5 style="padding: 5px; line-height: 35px; margin: 0.42rem 0 .556rem 0; background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">--%>
                    <h5 class="gradient-45deg-indigo-purple" style="padding: 5px; line-height: 35px; margin: 0.42rem 0 .556rem 0;">
                        <i id="pageIcon" onclick="javascript:showhideMyMenu();" class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Manage Data</span>
                        <div class="ml-2 mr-2 float-left">
                            <span class="float-left white-text" style="padding:3px; border-radius:5px;">Page size :</span> 
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

                        <%--Section : Add New Entry --%>
                        <a class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped ml-5 mr-2" data-position="center" data-tooltip="Add New Entry" href="javascript:gridAction('add', 0);" style="padding: 5px;"><i class="material-icons">add</i></a>

                        <%--Section : Download/Upload --%>
                        <asp:ImageButton ID="btnExportToExcel" runat="server" ClientIDMode="Static" CssClass="btn-floating float-right ml-1 mr-1 tooltipped" AlternateText="Export" data-tooltip="Export To Excel" ImageAlign="left" ImageUrl="images/buttons/export2excel.png" OnClick="btnExportToExcel_Click" Height="35px" Width="35px" Style="z-index: 0;" />
                        <asp:ImageButton ID="btnExportToPdf" runat="server" ClientIDMode="Static" CssClass="btn-floating float-right ml-1 mr-1 tooltipped" AlternateText="Export" data-tooltip="Export To PDF" ImageAlign="left" ImageUrl="images/pdf_document.png" OnClick="btnExportToPdf_Click" Height="35px" Width="35px" Style="z-index: 0; visibility : hidden;" />
                        <a id="btnupload"  class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped ml-5 mr-1" data-position="center" data-tooltip="Upload Excel File" href="javascript:openUploadDocument();" style="padding:5px;"><i class="material-icons">backup</i></a>

                        <%--Section : Header Search Box --%>
                        <div class="float-right mr-15" style="width:500px;">
                            <asp:TextBox ID="txtSearchBoxHeader" runat="server" ClientIDMode="Static" placeholder="Type Content To Search .... " class="form-control white black-text mr-1" Style="width:300px; margin-bottom: 20px;" TabIndex="1" />
                            <a id="lnkSearchHeader" class="btn waves-effect waves-light gradient-45deg-purple-deep-orange" data-position="center" href="javascript:gridSearchData('top');" style="background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">Filter Data</a>
                        </div>

                        <%--Section : External Lead Filters --%>
                        <asp:Label runat="server" ClientIDMode="Static" ID="lblExternalLeadCat"  Style="float: left; color:yellow; margin-right: 10px;">Filter : </asp:Label>
                        <asp:DropDownList ID="drpExternalLeadCat" runat="server" class="select2-theme browser-default mr-2 float-left white blue-text" ClientIDMode="Static" OnSelectedIndexChanged="drpExternalLeadCat_SelectedIndexChanged" TabIndex="5" Style="font-size: 18px; font-weight: bold; width: 150px; margin-top: 1px; height: 35px !important; border: 1px solid black; border-radius: 5px; padding: 1px;" AutoPostBack="true">
                            <asp:ListItem Text="-- All --" Value="" />
                            <asp:ListItem Text="Qualified" Value="Qualified" />
                            <asp:ListItem Text="In-Process" Value="InProcess" />
                            <asp:ListItem Text="DisQualified" Value="Disqualified" />
                        </asp:DropDownList>
                        
                        <asp:DropDownList ID="drpExternalLeadAc" runat="server" class="select2-theme browser-default mr-2 float-left white blue-text" ClientIDMode="Static" OnSelectedIndexChanged="drpExternalLeadAc_SelectedIndexChanged" TabIndex="5" Style="display: none; font-size: 18px; font-weight: bold; width: 150px; margin-top: 1px; height: 35px !important; border: 1px solid black; border-radius: 5px; padding: 1px;" AutoPostBack="true"></asp:DropDownList>
                        <asp:Label CssClass="hide" runat="server" ClientIDMode="Static" ID="lblExternalLeadAc" Style="float: right; margin-right: 10px;">Select Account</asp:Label>
                    </h5>
                    <div class="row">
                        <div class="col s12">

                            <div id="profilecard" class="card animate fadeRight mb-0">
                                <div class="col m12 p-0" style="margin-bottom: 10px;">
                                    <h5 class="left-align mb-0 width-100 gradient-45deg-indigo-purple" style="min-height: 30px;">
                                        <span id="spnPageNo" class="ml-1 float-left white-text" style="font-size: 18px; border-radius: 5px;">Page # : </span>
                                        <%--<span id="spnRecordCount" class="mr-1 float-right white-text" style="font-size: 18px; border-radius: 5px;">Total Count : </span>--%>
                                    </h5>
                                </div>
                                <div class="card-content p-0">
                                    <table id="tblView" class="display width-100 nowrap" j style="margin-left: 0px;"></table>
                                    <div style="text-align: center; width: 100%; margin: auto;">
                                        <a id="lnkResetPage" class="material-icons background-round gradient-45deg-red-pink gradient-shadow white-text float-left tooltipped ml-1 mr-1" data-tooltip="Refresh Data" href="javascript:gridReloadData('reset');" style="position: relative; padding: 10px; top: -25px;"><i class="material-icons">autorenew</i></a>
                                        <a id="lnkPrevPage" class="material-icons background-round gradient-45deg-purple-deep-orange gradient-shadow white-text tooltipped ml-1 mr-1 hide" data-tooltip="Previous" href="javascript:gridReloadData('prev');" style="position: relative; padding: 10px; top: -25px;"><i class="material-icons">chevron_left</i></a>
                                        <a id="lnkNextPage" class="material-icons background-round gradient-45deg-purple-deep-orange gradient-shadow white-text tooltipped ml-1 mr-1" data-tooltip="Next" href="javascript:gridReloadData('next');" style="position: relative; padding: 10px; top: -25px;"><i class="material-icons">chevron_right</i></a>
                                        <a id="lnkFilter" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped btn-move-up accent-2 z-depth-4 right" data-tooltip="Filter Data" style="padding: 10px; top: -25px; right: -5px;"><i class="material-icons activator">filter_list</i></a>
                                    </div>
                                </div>
                                <div class="card-reveal gradient-45deg-light-blue-teal gradient-shadow black-text">
                                    <span class="card-title grey-text text-darken-4">Filter Data<i class="material-icons right">close</i></span>
                                    <div id="profile-card1" class="card animate fadeRight mb-0">
                                        <div class="card-content p-0 col m6">
                                            <asp:TextBox ID="txtSearchBox" runat="server" ClientIDMode="Static" placeholder="Type Content To Search .... " class="form-control black-text" Style="margin-bottom: 20px;" TabIndex="1" />
                                            <a id="lnkSearch" class="btn waves-effect waves-light gradient-45deg-purple-deep-orange" data-position="center" href="javascript:gridSearchData('bottom');" style="background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">Submit</a>
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
    <%--</ContentTemplate>
         <Triggers>
             <asp:AsyncPostBackTrigger ControlID="drpExternalLeadAc" />
             <asp:AsyncPostBackTrigger ControlID="drpExternalLeadCat"/>
         </Triggers>
    </asp:UpdatePanel>--%>

    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
        <div id="myModal">
            <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                    <i class="material-icons prefix">ac_unit</i>
                    <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                    <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                </h5>
            </div>
        </div>
        <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 85%; padding: 5px;"></iframe>
        <br />
    </asp:Panel>
    <%-- ********************************************************************************************** --%>
<%--    <script src="app-assets/js/vendors.min.js"></script>
    <script src="app-assets/vendors/sweetalert/sweetalert.min.js"></script>--%>
    <script type="text/javascript">
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
                            //    contentType: "application/json; charset=utf-8",
                            //    dataType: "json",
                            //    success: function (datapageno) {
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
            if (pos == 'top')
                tmpVal = $("#txtSearchBoxHeader").val();
            else {
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
                            //        var resultdata11 = JSON.parse(datapageno.d);
                            //        var myTotalRecord1 = resultdata11[0].value;
                            //        if (myTotalRecord1 != null && myTotalRecord1 != '') {
                            //            $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                            //            $("#spnPageNo").text("Filtered Data");
                            //        }
                            //    }
                            //});
                            // ------------------------------------------------------------
                            sortingDef = [[3, "desc"]];
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
            return months <= 0 ? 0 : months+1;
        }

        function getStatusUpdate(interval, date1, id) {
            var second = 1000, minute = second * 60, hour = minute * 60, day = hour * 24, week = day * 7;
            date1 = new Date(moment(date1).format('DD-MMM-YYYY hh:mm:ss'));
            date2 = new Date(moment(new Date()).format('DD-MMM-YYYY hh:mm:ss'));
            // -------------------------------------------------------------
            var timediff = date2 - date1;
            if (isNaN(timediff)) return NaN;
            var tmpVal = 0;
            var returnVal = "";
            // -------------------------------------------------------------
            var m = parseInt(Math.floor(timediff / day)/30);     //monthDiff(date1, date2);
            var w = Math.floor(timediff / week);
            var d = Math.floor(timediff / day);
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
                spnModuleHeader.innerText = "Manage Customer";
                $('#ifrModule').attr('src', "customers.aspx?mode=" + cat + "&id=" + keyid);
                $find("mpe").show();
            }
        }
        function openUploadDocument() {
            if (viewmode != null && viewmode != '') {
                var string = viewmode;
                spnModuleHeader.innerText = string[0].toUpperCase() + string.slice(1) + " Upload";
                $('#ifrModule').attr('src', "UploadMaster.aspx?view=" + viewmode);
                $find("mpe").show();
            }
        }
        // -------------------------------------------------------------------------
        // Loading ..... PAGE LAYOUT
        // -------------------------------------------------------------------------
        var colstructure = [];
        var dataurl, dataurlsearch, para1, para2, para3, viewmode;
        var sortingDef;

        function loadPageLayout() {
            viewmode = jQuery("#hdnView").val();
            // --------------------------------------------------------
            mysection1: {
                if (viewmode == 'telecaller') {
                    spnModuleHeader.innerText = "Tele Caller Leads";
                    para1 = '';
                    para2 = jQuery.trim(jQuery("#drpExternalLeadCat").val());
                    dataurl = '/Services/NagrikService.asmx/webExternalLeadList?source=telecaller&acid=' + para1 + '&cat=' + para2 + "&currPageNo=1";
                    dataurlsearch = '/Services/NagrikService.asmx/webExternalLeadListSearch?source=telecaller&acid=' + para1 + '&cat=' + para2;
                    var xUntouchTitle = "Untouch Since <br />Init.By";
                    colstructure = [
                        {
                            title: xUntouchTitle, data: "QueryDatetime", className: "center", width: "110",
                            render: function (data, type, row, meta) {
                                data = '<p class="mb-0" style="color:red;">' + getStatusUpdate('days', moment(row.UpdatedDate).format('YYYY-MM-DD hh:mm:ss'), row.pkID) + '</p><p class="badge badge-info mb-0" style="font-size:12px; text-transform:capitalize;">' + row.CreatedBy + '</p>';
                                return data;
                            }
                        },
                        {
                            title: "Assign To", data: "EmployeeName", className: "center", width: "150",
                            render: function (data, type, row, meta) {
                                if (row.EmployeeName != '--Not Available--')
                                    data = '<p class="mb-0" style="color:blue; font-weight:bold; width:150px; white-space: pre-wrap; word-wrap: break-word;">' + row.EmployeeName + '</p>';
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
                                data = '<p class="mb-0" style="color:navy;font-weight:bold; width:150px; white-space: pre-wrap; word-wrap: break-word;">' + row.SenderName + '</p><p class="badge badge-primary" style="font-size:12px;  width:150px; white-space: pre-wrap; word-wrap: break-word;">' + row.PrimaryMobileNo + '</p>';
                                return data;
                            }

                        },
                        {
                            title: "Company Name<br />Email ID", data: "CompanyName", className: "text-left", width: "150",
                            render: function (data, type, row, meta) {
                                if (row.CompanyName != '')
                                    data = '<p class="mb-0" style="color:darkred;font-weight:bold; width:150px; white-space: pre-wrap; word-wrap: break-word;">' + row.CompanyName + '</p><p class="mb-0" style="color:blue; width:150px; white-space: pre-wrap; word-wrap: break-word;">' + row.SenderMail + '</p>';
                                else
                                    data = '<p class="mb-0" style="color:blue;">' + row.SenderMail + '</p>';
                                return data;
                            }
                        },
                        //{ title: "Email ID", data: "SenderMail", className: "text-left" },
                        {
                            title: "For Product", data: "ForProduct", className: "text-left", width: "150",
                            render: function (data, type, row, meta) {
                                data = '<p class="mb-0" style="color:gray;font-weight:bold; width:150; white-space: pre-wrap; word-wrap: break-word;">' + row.ForProduct + '</p>';
                                return data;
                            }
                        },
                        {
                            title: "Status", data: "LeadSource", className: "center", width:"150",
                            render: function (data, type, row, meta) {
                                if (row.LeadStatus == 'Qualified')
                                    data = '<p class="mb-0 pill green white-text" style="width:150; white-space: pre-wrap; word-wrap: break-word;">&nbsp;' + row.InquiryStatus + '&nbsp;</p>';
                                else if (row.LeadStatus == 'Disqualified')
                                    data = '<p class="mb-0 pill red white-text" style="width:150; white-space: pre-wrap; word-wrap: break-word;"> &nbsp;' + row.ExLeadCloserReason + '&nbsp;</p>';
                                else
                                    data = '<p class="mb-0 pill blue white-text " style="width:150; white-space: pre-wrap; word-wrap: break-word;">&nbsp;' + ' New ' + '&nbsp;</p>';
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
            }
            // -------------------------------------------------------------------------
            // Storing DataUrl for Export Purpose
            // -------------------------------------------------------------------------
            jQuery("#hdnDataUrl").val(dataurl);
            jQuery("#hdnDataUrlSearch").val(dataurlsearch);
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = jQuery("#hdnTitle").val()
            spnModuleHeader.innerHTML = jQuery("#hdnTitle").val();
            spnPageHeader.innerHTML = "";
            // -------------------------------------------------------------------------
            // Configure Edit/Delete Button Display 
            // -------------------------------------------------------------------------
            if (jQuery("#hdnActionFlag").val() == 'true') {
                if (viewmode == 'telecaller') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';

                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img class="tooltipped" data-position="center" data-tooltip="Edit Record" src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';

                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<a class="tooltipped" data-position="center" data-tooltip="Add Followup" href="javascript:openContFollowFromTeleCaller(\'add\',\'' + row.pkID + '\');"><img src="images/expand.png" alt="" style="width:15px; height:15px;"/></a>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<a class="tooltipped" data-position="center" data-tooltip="View Timeline" href="javascript:openTimeline(\'followupext\',\'' + row.pkID + '\',\'Hellow\');"><img src="images/timeline3.png" alt="" style="width:15px; height:15px;"/></a>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img class="tooltipped" data-position="center" data-tooltip="Delete Record" src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';

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
                type: "get",
                success: function (data) {
                    var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                    if (resultdata != null && resultdata != '') {
                        $("#spnPageNo").text("Page # : " + $("#hdnCurrPageNo").val());
                        //jQuery.ajax({
                        //    url: "Services/NagrikService.asmx/getSessionObject",
                        //    type: "GET",
                        //    contentType: "application/json; charset=utf-8",
                        //    dataType: "json",
                        //    success: function (datapageno) {
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
                    if (viewmode == 'telecaller') {
                        //sortingColNo = 1;
                        //sortingOrder = "desc";
                        sortingDef = [[3, "desc"]];
                    }
                    // --------------------------------------------------
                    // Loading and Drawing Datatable .... From Database
                    // --------------------------------------------------

                    $('#tblView').DataTable({
                        data: resultdata,
                        columns: colstructure,
                        deferRender: true,
                        "ordering": false,
                        "bFilter": false,
                        "bInfo": false,
                        "bPaginate": false,
                        order: sortingDef,
                        "scrollY": 350,
                        "scrollX": true,
                        "bScrollCollapse": false,
                        createdRow: function (row, data, dataIndex) { setTableRowFormat(viewmode, row, data, dataIndex) }
                    });
                },
                error: function (r) {
                    alert('Error : ' + r.responseText);
                },
                failure: function (r) {
                    alert('failure');
                }
            });
        }

        function setTableRowFormat(viewmode, row, data, dataIndex) {
            if (viewmode == 'externalleads') {
                //jQuery("td:eq(0)", row).css("font-weight", "bold");
                //jQuery("td:eq(0)", row).css("color", "Red");
            }
            if (viewmode == 'inquiry') {
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
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        // Function : This will invoke by Edit/Delete button
        // -------------------------------------------------------------------------
        function gridAction(mode, id) {
            var pageTitle = "";
            var cbWidth = "90%";
            var cbHeight = "90%";
            var varView = jQuery("#hdnView").val();
            //alert('Module : ' + mode)
            var pageUrl = '', deleteUrl = '', deletePara = '';
            // -------------------------------------------------------------

            if (varView == 'telecaller') {
                pageTitle = "Manage Tele-Caller Leads";
                var cbWidth = "90%";
                var cbHeight = "90%";
                pageUrl = "TeleCaller.aspx?source=telecaller&mode=" + mode + "&id=" + id
                deleteUrl = 'TeleCaller.aspx/DeleteExternalLeads';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            // -------------------------------------------------------------
            // Section : Call page for ADD / EDIT
            // -------------------------------------------------------------
            if (mode == 'add' || mode == 'edit' || mode == 'view') {
                spnModuleHeader.innerText = pageTitle;
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
                                location.reload();
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
                window.setTimeout(hide_loader, 500);
            // ----------------------------------------------------------------------

        }

        function ShowPDFfile(repFilename) {
            var today = new Date();
            var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            var dateTime = date + ' ' + time;

            yhooWin = window.open(repFilename + "?id=" + dateTime, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Below Section : SetUp & Load DataTable
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        $(document).ready(function () {
            loadPageLayout();
        });
    </script>
</asp:Content>
