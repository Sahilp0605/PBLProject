<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="GeneralMaster.aspx.cs" Inherits="StarsProject.GeneralMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            $('.timepicker').timepicker();
            $('.tabs').tabs();
            /* ======================================================== */
            jQuery(document).click(function (event) {
                var ud = jQuery('.userinfodrop');
                if (!jQuery(event.target).is('.userinfodrop') && !jQuery(event.target).is('.userdata') && ud.is(':visible') && $("#hdnView").val() != "") {
                    ud.hide();
                    jQuery('.userinfodrop').removeClass('active');
                }
            });
            /* ======================================================== */
        });
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

        /*---------------------------------------------------------*/
        function pageLoad(sender, args) {
            // ------------------------------------------------------------
            sidenavMain.removeClass("nav-lock");
            $(".nav-collapsible .navbar-toggler i").text("radio_button_unchecked");
            navCollapsible.removeClass("sideNav-lock");
            var ctrlMain = $("#main");
            if (!ctrlMain.hasClass("main-full"))
                ctrlMain.toggleClass("main-full");
            navigationCollapse();
            //defaultMenuCollapse();
            // Toggle navigation expan and collapse on radio click
            //toogleMenuCollapse();
            //// Set navigation lock / unlock with radio icon
            //if (
            //   $(this)
            //      .children()
            //      .text() == "radio_button_unchecked"
            //) {
            //    $(this)
            //       .children()
            //       .text("radio_button_checked");
            //    sidenavMain.addClass("nav-lock");
            //    navCollapsible.addClass("sideNav-lock");
            //} else {
            //    $(this)
            //       .children()
            //       .text("radio_button_unchecked");
            //    sidenavMain.removeClass("nav-lock");
            //    navCollapsible.removeClass("sideNav-lock");
            //}
            // ------------------------------------------------------------
            retainTabPosition();
            // ------------------------------------------------------------
            buildGeneralMenu();
        }
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }

        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function ShowPDFfile(repFilename) {
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }

        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        function openWideMenu() {

            if (!jQuery('.userinfodrop').hasClass('active')) {
                jQuery('.userinfodrop').show();
                jQuery('.userinfodrop').addClass('active');
            }
            else {
                jQuery('.userinfodrop').hide();
                jQuery('.userinfodrop').removeClass('active');
            }
            return false;
        }
        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        $(function () {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "city";
            $("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "city";
                var currTab = $(this).attr("href").replace("#", "");

                $("#hdnCurrentTab").val(currTab);
                $("#hdnView").val(currTab);
                $("#hdnAction").val('true');

                $("#hdnCurrPageNo").val("1");
                $("#spnPageNo").text("Page # : 1" + currPgNo);
                /* --------------------------------------------------------- */
                $('.card-reveal').css('transform', 'translate(0%)');
                $('.card-reveal').css('display', 'none');
                /* --------------------------------------------------------- */
                loadPageLayout();
            });
            /* ------------------------------------------------------------- */
            $(".userinfodrop a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "city";
                var currTab = $(this).attr("href").replace("#", "");

                $("#hdnCurrentTab").val(currTab);
                $("#hdnView").val(currTab);
                $("#hdnAction").val('true');

                $("#hdnCurrPageNo").val("1");
                /* --------------------------------------------------------- */
                $('.card-reveal').css('transform', 'translate(0%)');
                $('.card-reveal').css('display', 'none');
                /* --------------------------------------------------------- */
                loadPageLayout();
            });
        });

        function loadFromDashboard()
        {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "city";
            //$("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "city";
                var currTab = $(this).attr("href").replace("#", "");

                $("#hdnCurrentTab").val(currTab);
                $("#hdnView").val(currTab);
                $("#hdnAction").val('true');

                $("#hdnCurrPageNo").val("1");
                $("#spnPageNo").text("Page # : 1" + currPgNo);
                /* --------------------------------------------------------- */
                $('.card-reveal').css('transform', 'translate(0%)');
                $('.card-reveal').css('display', 'none');
                /* --------------------------------------------------------- */
                loadPageLayout();
            //});
            ///* ------------------------------------------------------------- */
            //$(".userinfodrop a").click(function () {
            //    var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "city";
            //    var currTab = $(this).attr("href").replace("#", "");

            //    $("#hdnCurrentTab").val(currTab);
            //    $("#hdnView").val(currTab);
            //    $("#hdnAction").val('true');

            //    $("#hdnCurrPageNo").val("1");
            //    /* --------------------------------------------------------- */
            //    $('.card-reveal').css('transform', 'translate(0%)');
            //    $('.card-reveal').css('display', 'none');
            //    /* --------------------------------------------------------- */
            //    loadPageLayout();
            //});
        }

        function retainTabPosition() {
            $('lnkRegion').removeClass('active');
            $('lnkCustCat').removeClass('active');
            $('lnkSpecs').removeClass('active');
            $('lnkTNC').removeClass('active');
            $('lnkOrgType').removeClass('active');
            $('lnkDesig').removeClass('active');
            $('lnkHoliday').removeClass('active');
            $('lnkStatus').removeClass('active');
            $('lnkOffer').removeClass('active');
            $('lnkCharge').removeClass('active');
            $('lnkTemplate').removeClass('active');
            $('lnkGTemplate').removeClass('active');
            $('lnkPriceList').removeClass('active');
            $('lnkCheck1').removeClass('active');
            $('dvRegion').css("display", "block");
            $('lnkDocument').removeClass('active');
            $('lnkTODOCat1').removeClass('active');
            $('lnkbankdetails').removeClass('active');
            $('lnklocation').removeClass('active');
            $('lnkbatch').removeClass('active');
            $('lnkcolor').removeClass('active');
            $('lnkgrade').removeClass('active');
            $('lnksize').removeClass('active');
            $('lnkshade').removeClass('active');
            $('lnkspunch').removeClass('active');
            $('lnknatureofcall').removeClass('active');
            $('lnkcatdesc').removeClass('active');
            var obj1, obj2;
            obj1 = '#lnk' + $("#hdnCurrentTab").val();
            //obj2 = '#dv' + $("#hdnCurrentTab").val();

            $(obj1).addClass('active');
            //$(obj2).css("display", "block");
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Section : Show/Hide General Menu
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        function buildGeneralMenu() {
            var newData = localStorage.getItem('userGeneralMenuList');
            if (newData == null) {
                jQuery.ajax({
                    url: '/Services/NagrikService.asmx/webGetGeneralMenuList',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        for (i = 0; i < resultdata.length; i++) {
                            tmpStr = "";
                            if (resultdata[i].MenuName != null) {
                                tmpStr = '#' + resultdata[i].MenuName;
                                //alert(tmpStr)
                                jQuery(tmpStr).removeClass("canceldisabled");
                            }
                        }
                    }
                });
            }
        }

    </script>

    <style type="text/css">
        .limited {
            white-space: nowrap;
            max-width: 300px;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .tabs .tab {
            line-height: 30px;
            height: 30px;
        }

        .tabs .tab a {
            font-size: 12px;
        }

        table tr td {
            padding: 2px 5px !important;
            vertical-align: text-top !important;
        }
        table tr th {
            padding: 5px 0px 5px 5px !important;
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

        /*.modalPopup {
            border-radius: 10px;
            padding-right: 10px !important;
            left: 71px !important;
            top: 30px !important;
            width: 1690px;
            height: 690px;
        }*/

        .userinfodrop {
            display: block;
            padding: 10px;
            min-width: 1305px;
            min-height: 468px;
            position: absolute;
            top: 125px;
            left: 25px;
            z-index: 1;
            -moz-border-radius: 2px;
            -webkit-border-radius: 2px;
            border-radius: 2px;
            background: #f8f8ff;
            border: 5px ridge silver;
            -moz-box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
            -webkit-box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
        }

        .userinfodrop table tr {
            border-bottom: 0px !important;
        }

        .canceldisabled {
          pointer-events: none;
          cursor: default;
          color:lightgray;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server" ClientIDMode="Static" EnablePageMethods="true"></asp:ScriptManager>
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLoginUserRole" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnActionFlag" runat="server" ClientIDMode="Static" Value="true" />
    <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnKeyID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTitle" runat="server" ClientIDMode="Static"  Value=""/>
    <asp:HiddenField ID="hdnPara1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPara2" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPara3" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnQuotationVersion" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnQuotationNoForPDF" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSalesOrderVersion" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientERPIntegration" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnInquiryShare" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnpopupPrintHeader" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDataUrl" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDataUrlSearch" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCurrPageNo" runat="server" ClientIDMode="Static" Value="1" />
    <asp:HiddenField ID="hdnCurrPageSize" runat="server" ClientIDMode="Static" EnableViewState="true" Value="10" />
    <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" Value="" EnableViewState="true" />
    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />

    <asp:HiddenField ID="hdnMenuID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAddFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEditFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDelFlag" runat="server" ClientIDMode="Static" />
    <div class="row">
        <div class="col s12">
            <div class="card m-0" style="background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
                <h5 style="padding:5px; line-height:35px; margin: 0.42rem 0 .556rem 0;">
                    <a class="userinfo" id="lnkWideMenu" href="javascript:openWideMenu();"><i class="material-icons" style="vertical-align: text-bottom;">apps</i></a>
                    <span class="text-top mr-2" style="color: white; margin-left: 10px;">Application Constants</span>
                    <span id="spnModuleHeader" runat="server" clientidmode="static" class="text-top chip" style="margin-left: 20px; min-width:620px; color: navy; background-color: white;text-align: center;font-size: 18px;">Layout</span>
                    <%--Section : Download/Upload --%>
                    <asp:ImageButton ID="btnExportToExcel" runat="server" ClientIDMode="Static" CssClass="btn-floating float-right ml-1 mr-1 tooltipped" AlternateText="Export" data-tooltip="Export To Excel" ImageAlign="left" ImageUrl="images/buttons/export2excel.png" OnClick="btnExportToExcel_Click" Height="35px" Width="35px" Style="z-index: 0;" />

                    <%--Section : Add New Entry --%>
                    <a id="lnkAddNew" runat="server" clientidmode="static" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped ml-1 mr-2" data-position="center" data-tooltip="Add New Entry" href="javascript:gridAction('add', 0);" style="padding:5px;"><i class="material-icons">add</i></a>
                    <a id="btnupload"  class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped ml-5 mr-1" data-position="center" data-tooltip="Upload Excel File" href="javascript:openUploadDocument();" style="padding:5px; visibility:hidden;"><i class="material-icons">backup</i></a>
                </h5>
            </div>
            <div id="generalMenuDrop" class="userinfodrop">
                <div class="row">
                    <div class="col m3">
                        <table>
                            <tr><td><h5>Lead Management</h5></td></tr>
                            <tr><td><a id="lnkTNC1" class="left canceldisabled" href="#contents">Terms & Conditions</a></td></tr>
                            <tr><td><a id="lnkStatus1" class="left canceldisabled" href="#inquirystatus">Lead Status</a></td></tr>
                            <tr><td><a id="lnkcatdesc" class="left canceldisabled" href="#catdesc">Category Description SSR</a></td></tr>
                            <tr><td><a id="lnkOffer1" class="left canceldisabled" href="#bundle">Define Quotation Offer</a></td></tr>
                            <tr><td><a id="lnkCharge1" class="left canceldisabled" href="#othercharge">Additional Charges</a></td></tr>
                            <tr><td><a id="lnknatureofcall" class="left canceldisabled" href="#natureofcall">Nature of Call</a></td></tr>

                            <tr><td><h5>Customers</h5></td></tr>
                            <tr><td><a id="lnkCustomer1" class="left canceldisabled" href="#customer">Manage Customer</a></td></tr>
                            <tr><td><a id="lnkCustCat1" class="left canceldisabled" href="#customercategory">Customer Type</a></td></tr>
                            <tr><td><a id="lnkProjects1" class="left canceldisabled" href="#projects">Projects</a></td></tr>
                            <tr><td></td></tr>
                        </table>
                    </div>
                    <div class="col m3">
                        <table>
                            <tr><td><h5>Product Details</h5></td></tr>
                            <tr><td><a id="lnklocation" class="left canceldisabled" href="#location">Location</a></td></tr>
                            <tr><td><a id="lnkbatch" class="left canceldisabled" href="#batch">Batch</a></td></tr>
                            <tr><td><a id="lnkcolor" class="left canceldisabled" href="#color">Color</a></td></tr>
                            <tr><td><a id="lnkgrade" class="left canceldisabled" href="#grade">Grade</a></td></tr>
                             <tr><td><a id="lnksize" class="left canceldisabled" href="#size">Size</a></td></tr>
                            <tr><td><a id="lnkshade" class="left canceldisabled" href="#shade">Shade</a></td></tr>
                            <tr><td><a id="lnkpunch" class="left canceldisabled" href="#punch">Punch</a></td></tr>
                            <tr><td><h5>Products</h5></td></tr>
                            <tr><td><a id="lnkProudct1" class="left canceldisabled" href="#products">Manage Product</a></td></tr>
                            <tr><td><a id="lnkSpecs1" class="left canceldisabled" href="#productspecification">Specification</a></td></tr>
                            <tr><td><a id="lnkProdGroup1" class="left canceldisabled" href="#productgroup">Product Group</a></td></tr>
                            <tr><td><a id="lnkProdBrand1" class="left canceldisabled" href="#productbrand">Product Brand</a></td></tr>
                            <tr><td><a id="lnkPriceList" class="left canceldisabled" href="#pricelist">Price List</a></td></tr>
                        </table>
                    </div>
                    <div class="col m3">
                        <table border="0">
                            <tr><td><h5>Templates</h5></td></tr>
                            <tr><td><a id="lnkEmailTemplate" class="left canceldisabled" href="#emailtemplate">Email Template</a></td></tr>
                            <tr><td><a id="lnkGenTemplate" class="left canceldisabled" href="#gentemplate">Generate Template</a></td></tr>
                            <tr><td><a id="lnkTemplateEmail1" class="left canceldisabled" href="#campaigntemplateemail">Campaign Email</a></td></tr>
                            <tr><td><a id="lnkTemplateSMS1" class="left canceldisabled" href="#campaigntemplatesms">Campaign SMS</a></td></tr>

                            <tr><td></td></tr>
                            <tr><td><h5>Organization</h5></td></tr>
                            <tr><td><a id="lnkOrgType1" class="left canceldisabled" href="#orgtype">Organizaiton Types</a></td></tr>
                            <tr><td><a id="lnkDesig1" class="left canceldisabled" href="#designation">Manage Designations</a></td></tr>
                            <tr><td><a id="lnkHoliday1" class="left canceldisabled" href="#holiday">Annual Holidays</a></td></tr>
                            <tr><td><a id="lnkDocument1" class="left canceldisabled" href="#document">Document Type</a></td></tr>
                            <tr><td><a id="lnkTODOCat1" class="left canceldisabled" href="#todocategory">TODO Category</a></td></tr>
                            <tr><td><a id="lnkbankdetails" class="left canceldisabled" href="#bankdetails">Organization Bank Details</a></td></tr>
                        </table>
                    </div>
                    <div class="col m3">
                        <table>
                            <tr><td><h5>General</h5></td></tr>
                            <tr><td><a id="lnkRegion1" class="left canceldisabled" href="#city">Manage Region</a></td></tr>
                            <tr><td><a id="lnkExpType" class="left canceldisabled" href="#expensetype">Expense Types</a></td></tr>
                            <tr><td><a id="lnkCheck1" class="left canceldisabled" href="#checklist">Check List</a></td></tr>
                            <tr><td></td></tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container" style="padding:0 0.9rem !important;">
    <section class="tabs-vertical section">
        <div class="row">
            <div class="col m12">
                <div id="dvRegion" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                    <div class="row">
                        <div class="col m12">
                            <div id="dvDataList" class="row" runat="server" clientidmode="Static" enableviewstate="true">
                                <div id="profilecard" class="card animate fadeRight mb-0" style="margin:5px !important;">
                                    <div class="col m12 p-0" style="margin-bottom:10px;">
                                        <h5 class="card-title mb-0 left-align width-100 gradient-45deg-indigo-purple" style="min-height:45px;">
                                            <span id="spnPageNo" class="ml-1 float-left white-text" style="padding:3px; border-radius:5px;">Page # : </span>
                                            <span class="ml-2 float-left white-text" style="padding:3px; border-radius:5px;">Page size :</span> 
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
                                            <span id="spnRecordCount" class="mr-1 float-right white-text" style="padding:3px; border-radius:5px;">Total Count : </span>
                                            <div class="float-right mr-15" style="width:500px;">
                                                <asp:TextBox ID="txtSearchBoxHeader" runat="server" ClientIDMode="Static" placeholder="Type Content To Search .... " class="form-control white black-text mr-1" Style="width:300px; margin-bottom: 20px;" TabIndex="1" />
                                                <a id="lnkSearchHeader" class="btn waves-effect waves-light gradient-45deg-purple-deep-orange" data-position="center" href="javascript:gridSearchData('top');" style="background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">Filter Data</a>
                                            </div>
                                        </h5>
                                    </div>
                                    <div class="card-content p-0">
                                        <table id="tblView" class="display width-100 nowrap" style="margin-left:0px;"></table>
                                        <div style="text-align:center; width:100%; margin:auto;margin-top:15px;">
                                            <a id="lnkResetPage" class="material-icons background-round gradient-45deg-red-pink gradient-shadow white-text float-left tooltipped ml-1 mr-1" data-tooltip="Refresh Data" href="javascript:gridReloadData('reset');" style="position:relative; padding:10px; top:-25px;"><i class="material-icons">autorenew</i></a>
                                            <a id="lnkPrevPage" class="material-icons background-round gradient-45deg-purple-deep-orange gradient-shadow white-text tooltipped ml-1 mr-1 hide" data-tooltip="Previous" href="javascript:gridReloadData('prev');" style="position:relative; padding:10px; top:-25px;"><i class="material-icons">chevron_left</i></a>
                                            <a id="lnkNextPage" class="material-icons background-round gradient-45deg-purple-deep-orange gradient-shadow white-text tooltipped ml-1 mr-1" data-tooltip="Next" href="javascript:gridReloadData('next');" style="position:relative; padding:10px; top:-25px;"><i class="material-icons">chevron_right</i></a>
                                            <a id="lnkFilter" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped btn-move-up accent-2 z-depth-4 right" data-tooltip="Filter Data" style="padding:10px; top:-25px; right:-5px;"><i class="material-icons activator">filter_list</i></a>
                                        </div>
                                    </div>
                                    <div class="card-reveal gradient-45deg-light-blue-teal gradient-shadow black-text">
                                        <span class="card-title grey-text text-darken-4">Filter Data<i class="material-icons right">close</i></span>
                                        <div id="profile-card1" class="card animate fadeRight mb-0">
                                            <div class="card-content p-0 col m6">
                                                <asp:TextBox ID="txtSearchBox" runat="server" ClientIDMode="Static" placeholder="Type Content To Search .... " class="form-control black-text" style="margin-bottom:20px;" TabIndex="1"  />
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
    <//section>
    </div>

    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
    </cc1:ModalPopupExtender>  
    <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" style = "display:none;">  
        <div id="myModal">
            <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0">
                <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                    <i class="material-icons prefix">ac_unit</i>
                    <span id="spnGeneralHeader" runat="server" clientidmode="static">Manage Module</span>
                    <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text=""/>
                </h5>
            </div>
        </div>
        <iframe id="ifrModulePopup" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border:3px solid silver; width:100%; height:86%;"></iframe>  
        <br/>  
    </asp:Panel>

    <script type="text/javascript">
        // ------------------------------------------------------------
        // Footer : Quick Link
        // ------------------------------------------------------------
        function openDashboardDaily() {
            $('#ifrModulePopup').attr('src', "DashboardDaily.aspx");
            $find("mpe").show();
        }
        function openDashboardLead() {
            $('#ifrModulePopup').attr('src', "DashboardLead.aspx");
            $find("mpe").show();
        }
        function openDashboardSales() {
            $('#ifrModulePopup').attr('src', "DashboardAccount.aspx");
            $find("mpe").show();
        }
        function openDashboardSupport() {
            $('#ifrModulePopup').attr('src', "DashboardSupport.aspx");
            $find("mpe").show();
        }
        function openCalender() {
            $('#ifrModulePopup').attr('src', "myEventCalender.aspx");
            $find("mpe").show();
        }
        function openTrackEmployee() {
            spnModuleHeader.innerText = "Sales Executive - Track Visit";
            $('#ifrModulePopup').attr('src', "TrackEmployee.aspx");
            $find("mpe").show();
            
        }
        function openCustomerSearch() {
            spnModuleHeader.innerText = "Customer Activity Information";
            $('#ifrModulePopup').attr('src', "SearchCustomer.aspx");
            $find("mpe").show();
        }
        function openEmployeeSearch() {
            spnModuleHeader.innerText = "Employee Activity Information";
            $('#ifrModulePopup').attr('src', "SearchEmployee.aspx");
            $find("mpe").show();
        }
        function openOrganizationChart() {
            spnModuleHeader.innerText = "Organization Mapping";
            $('#ifrModulePopup').attr('src', "OrganizationChart.aspx");
            $find("mpe").show();
        }

        function openChatBox() {
            spnModuleHeader.innerText = "Chat Box";
            $('#ifrModulePopup').attr('src', "ChatBox.aspx");
            $find("mpe").show();
        }
        function openMailBox() {
            spnModuleHeader.innerText = "Mail Box";
            $('#ifrModulePopup').attr('src', "MailBox.aspx");
            $find("mpe").show();
        }
        function openLibrary() {
            spnModuleHeader.innerText = "Document Gallery";
            $('#ifrModulePopup').attr('src', "DocumentGallery.aspx");
            $find("mpe").show();
        }
        // ---------------------------------------------------------------------------
        $("#lnkFilter").click(function () {
            $("#txtSearchBox").focus();
        })
        // -------------------------------------------------------------------------
        function gridReloadData(action) {
            var currPgNo;
            var oldPgNo = $("#hdnCurrPageNo").val();
            // -----------------------------------------------------------
            jQuery.ajax({
                url: '/Services/NagrikService.asmx/setGridPageSize?pSize=' + $("#drpPageSize").val(),
                type: "get",
                success: function (data) { }
            });
            // -----------------------------------------------------------
            //alert(action)
            //alert(currPgNo)
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
                //$("#lnkResetPage").removeClass("hide");

                //$("#lnkResetPage").addClass("hide");
                $("#lnkPrevPage").addClass("hide");
            }
            else if (action == 'refresh') {
                currPgNo = $("#hdnCurrPageNo").val();
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
            //alert('gridReLoad-Data')
            // ---------------------------------------------------
            var gridTable;
            jQuery.ajax({
                url: dataurl,
                type: "get",
                success: function (data) {
                    var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                    if (resultdata != '') {
                        //jQuery.ajax({
                        //    url: "/Services/NagrikService.asmx/getSessionObject",
                        //    type: "post",
                        //    contentType: "application/json; charset=utf-8",
                        //    dataType: "json",
                        //    success: function (data1) {
                        //        var myTotalRecord1 = data1.d.toString();
                        //        $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                        //    }
                        //});

                        jQuery.ajax({
                            url: "Services/NagrikService.asmx/getSessionObject",
                            type: "GET",
                            dataType: "json",
                            success: function (datapageno) {
                                var resultdata11 = JSON.parse(datapageno.d);
                                var myTotalRecord1 = resultdata11[0].value;
                                if (myTotalRecord1 != null && myTotalRecord1 != '') {
                                    $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                                }
                            }
                        });
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
                //$("#lnkResetPage").removeClass("hide");

                $("#lnkPrevPage").addClass("hide");
                $("#lnkNextPage").addClass("hide");
                //$("#lnkResetPage").removeClass("hide");
                // ---------------------------------------------------
                dataurlsearchNew = dataurlsearch;
                dataurlsearchNew = dataurlsearchNew.replace('?currPageNo=1', '');
                dataurlsearchNew = dataurlsearchNew.replace("?currPageNo=" + $("#hdnCurrPageNo").val(), '');
                dataurlsearchNew = dataurlsearchNew + "?ModuleName=product&SearchKey=" + tmpVal;
                // ---------------------------------------------------
                var gridTable;
                jQuery.ajax({
                    url: dataurlsearchNew,
                    type: "get",
                    success: function (data) {
                        var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                        if (resultdata != '') {
                            // ------------------------------------------------------------
                            //jQuery.ajax({
                            //    url: "/Services/NagrikService.asmx/getSessionObject",
                            //    type: "post",
                            //    contentType: "application/json; charset=utf-8",
                            //    dataType: "json",
                            //    success: function (data1) {
                            //        var myTotalRecord1 = data1.d.toString();
                            //        $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                            //        $("#spnPageNo").text("Filtered Data");
                            //    }
                            //});
                            jQuery.ajax({
                                url: "Services/NagrikService.asmx/getSessionObject",
                                type: "GET",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (datapageno) {
                                    var resultdata11 = JSON.parse(datapageno.d);
                                    var myTotalRecord1 = resultdata11[0].value;
                                    if (myTotalRecord1 != null && myTotalRecord1 != '') {
                                        $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                                        $("#spnPageNo").text("Filtered Data");
                                    }
                                }
                            });
                            // ------------------------------------------------------------
                            var table;
                            if ($.fn.dataTable.isDataTable('#tblView')) {
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
        // -------------------------------------------------------------------------
        // Open Customer View
        // -------------------------------------------------------------------------
        function openCustomerInfo(cat, keyid) {

            if ($("#hdnSerialKey").val() == 'HEXA-09PM-56JK-KG88')          //For Hexagon
            {
                if (keyid != '' && keyid != undefined) {
                    jQuery.colorbox({ width: "100%", height: "100%", iframe: true, href: "customerhexagon.aspx?mode=" + cat + "&id=" + keyid });
                }
            }
            else {
                if (keyid != '' && keyid != undefined) {
                    jQuery.colorbox({ width: "100%", height: "100%", iframe: true, href: "customers.aspx?mode=" + cat + "&id=" + keyid });
                }
            }
        }
        
        function openUploadDocument() {
            if (viewmode != null && viewmode != '') {
                var string = viewmode;
                spnGeneralHeader.innerText = string[0].toUpperCase() + string.slice(1) + " Upload";
                spnModuleHeader.innerText = string[0].toUpperCase() + string.slice(1) + " Upload";
                $('#ifrModulePopup').attr('src', "UploadMaster.aspx?view=" + viewmode);
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
            var table;
            if ($.fn.dataTable.isDataTable('#tblView')) {
                table = $('#tblView').DataTable();
                table.clear();
            }

            document.getElementById("btnupload").style.visibility = "hidden";
            viewmode = jQuery("#hdnView").val();
            if (viewmode != '' && viewmode != null) {
                var ud = jQuery('.userinfodrop');
                ud.hide();
            }
            // --------------------------------------------------------
            setAddEditDel(viewmode);



            if ($("#hdnAddFlag").val() == 'true' || $("#hdnAddFlag").val() == '' || $("#hdnAddFlag").val() == 'True')
                $("#lnkAddNew").show();
            else
                $("#lnkAddNew").hide();
            // --------------------------------------------------------
            if (viewmode == 'customer') {
                //alert($("#hdnSerialKey").val())
                document.getElementById("btnupload").style.visibility = "visible";
                spnGeneralHeader.innerText = "Customer Information";
                spnModuleHeader.innerText = "Customer Information";
                dataurl = '/Services/NagrikService.asmx/webCustomerDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCustomerDetailSearch';

                if ($("#hdnSerialKey").val() == "BLUE-CHEM-56JK-BC88")      // For Bluechem 
                {
                    colstructure = [
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'edit\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "GST #", data: "GSTNo", className: "center-center-align", width: "100" },
                    {
                        title: "Category/Source", data: "CustomerType", className: "center-center-align", width: "130",
                        render: function (data, type, row, meta) {
                            data = '<span style="color:navy;">' + row.CustomerType + '</span><br /><span style="color:maroon;">' + row.CustomerSourceName + '</span>';
                            return data;
                        }
                    },
                    { title: "CreatedBy", data: "CreatedEmployeeName", className: "center-align", width: "110" },
                    { title: "City", data: "CityName", className: "center-align", width: "110" },
                    { title: "State", data: "StateName", className: "center-align", width: "110" },
                    {
                        title: "Contact #1 / Email", data: "ContactNo1", className: "center-align", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<span style="color:navy;">' + row.ContactNo1 + '</span><br /><span style="color:maroon;">' + row.EmailAddress + '</span>';
                            return data;
                        }
                    },
                    { title: "Opening", data: "OpeningAmount", className: "right-align" },
                    { title: "Debit", data: "DebitAmount", className: "right-align" },
                    { title: "Credit", data: "CreditAmount", className: "right-align" },
                    { title: "Closing", data: "ClosingAmount", className: "right-align" }];
                }
                else if ($("#hdnSerialKey").val() == "HEXA-09PM-56JK-KG88")      // For Hexagon
                {
                    colstructure = [
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'edit\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "GST #", data: "GSTNo", className: "center-center-align", width: "100" },
                    {
                        title: "Category/Source", data: "CustomerType", className: "center-center-align", width: "130",
                        render: function (data, type, row, meta) {
                            data = '<span style="color:navy;">' + row.CustomerType + '</span><br /><span style="color:maroon;">' + row.CustomerSourceName + '</span>';
                            return data;
                        }
                    },
                    { title: "CreatedBy", data: "CreatedEmployeeName", className: "center-align", width: "110" },
                    { title: "City", data: "CityName", className: "center-align", width: "110" },
                    { title: "State", data: "StateName", className: "center-align", width: "110" },
                    {
                        title: "Contact #1 / Email", data: "ContactNo1", className: "center-align", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<span style="color:navy;">' + row.ContactNo1 + '</span><br /><span style="color:maroon;">' + row.EmailAddress + '</span>';
                            return data;
                        }
                    },
                    ];
                }
                else {
                    colstructure = [
                    {
                        title: "Customer Name", data: "CustomerName", className: "text-left limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'edit\',\'' + row.CustomerID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "GST #", data: "GSTNo", className: "center-center-align", width: "100" },
                    {
                        title: "Category/Source", data: "CustomerType", className: "center-center-align", width: "130",
                        render: function (data, type, row, meta) {
                            data = '<span style="color:navy;">' + row.CustomerType + '</span><br /><span style="color:maroon;">' + row.CustomerSourceName + '</span>';
                            return data;
                        }
                    },
                    { title: "City", data: "CityName", className: "center-align", width: "110" },
                    { title: "State", data: "StateName", className: "center-align", width: "110" },
                    {
                        title: "Contact #1 / Email", data: "ContactNo1", className: "center-align", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<span style="color:navy;">' + row.ContactNo1 + '</span><br /><span style="color:maroon;">' + row.EmailAddress + '</span>';
                            return data;
                        }
                    },
                    { title: "Opening", data: "OpeningAmount", className: "right-align" },
                    { title: "Debit", data: "DebitAmount", className: "right-align" },
                    { title: "Credit", data: "CreditAmount", className: "right-align" },
                    { title: "Closing", data: "ClosingAmount", className: "right-align" }];
                }
            }

            else if (viewmode == 'products') {
                document.getElementById("btnupload").style.visibility = "visible";
                spnGeneralHeader.innerText = "Manage Products";
                spnModuleHeader.innerText = "Manage Products";
                dataurl = '/Services/NagrikService.asmx/webProductsDetail?currPageNo='+$("#hdnCurrPageNo").val();
                dataurlsearch = '/Services/NagrikService.asmx/webProductsDetailSearch';
                // ----------------------------------------------------------------------
                //alert($("hdnSerialKey").val())
                if ($("#hdnSerialKey").val() == "AIRI-G3Y5-2T9E-YN9W")      // For Ambani
                {
                    colstructure = [
                        {
                            title: "Product Name", data: "ProductName", className: "left-align limited", width: "250",
                            render: function (data, type, row, meta) {
                                data = '<a href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                                return data;
                            }
                        },
                        { title: "Product Alias", data: "ProductAlias" },
                        { title: "Grade", data: "UnitGrade", className: "right-align" },
                        { title: "UnitPrice", data: "UnitPrice", className: "right-align" },
                        { title: "TaxRate", data: "TaxRate", className: "center-align" },
                        { title: "Product Group", data: "ProductGroupName", className: "center-align" },
                        { title: "Product Brand", data: "BrandName", className: "center-align" },
                        {
                            title: "Image", data: "ProductImage", className: "center-align",
                            render: function (data, type, row, meta) {
                                data = '<img src="' + row.ProductImage + '" width="60px" height="50px" />';
                                return data;
                            }
                        },
                        { title: "Status", data: "ActiveFlagDesc", className: "center-align" }
                    ];

                }
                else if ($("#hdnSerialKey").val() == "HEXA-09PM-56JK-KG88")         //For Hexagon
                {
                    colstructure = [
                        {
                            title: "Product Name", data: "ProductName", className: "left-align limited", width: "500",
                            render: function (data, type, row, meta) {
                                data = '<a href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                                return data;
                            }
                        },
                        { title: "Product Alias", data: "ProductAlias", className: "center-align"},
                        { title: "Product Brand", data: "BrandName", className: "center-align" },
                        { title: "Status", data: "ActiveFlagDesc", className: "center-align" }
                    ];
                } else {
                    colstructure = [
                        {
                            title: "Product Name", data: "ProductName", className: "left-align limited", width: "250",
                            render: function (data, type, row, meta) {
                                data = '<a href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                                return data;
                            }
                        },
                        { title: "Product Alias", data: "ProductAlias" },
                        { title: "UnitPrice", data: "UnitPrice", className: "right-align" },
                        { title: "TaxRate", data: "TaxRate", className: "center-align" },
                        { title: "Product Group", data: "ProductGroupName", className: "center-align" },
                        { title: "Product Brand", data: "BrandName", className: "center-align" },
                        {
                            title: "Image", data: "ProductImage", className: "center-align",
                            render: function (data, type, row, meta) {
                                data = '<img src="' + row.ProductImage + '" width="60px" height="50px" />';
                                return data;
                            }
                        },
                        { title: "Status", data: "ActiveFlagDesc", className: "center-align" }
                    ];
                }
            }

            else if (viewmode == 'broadcast') {
                spnGeneralHeader.innerText = "Manage Announcements";
                spnModuleHeader.innerText = "Manage Announcements";
                dataurl = '/Services/NagrikService.asmx/webBroadcastMessage?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webBroadcastMessageSearch';
                colstructure = [
                    { title: "From Date", data: "FromDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "To Date", data: "ToDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    {
                        title: "Sr. #", data: "pkID", className: "center", width: "150",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Message", data: "Message", width: "350" }];
            }
            else if (viewmode == 'projects') {
                spnGeneralHeader.innerText = "Define Organization Projects";
                spnModuleHeader.innerText = "Define Organization Projects";
                dataurl = '/Services/NagrikService.asmx/webProjectsDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webProjectsDetailSearch';
                colstructure = [
                    {
                        title: "Project Name", data: "ProjectName", className: "text-left", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Project Description", data: "ProjectDescription", className: "text-left", width: "250" },
                    { title: "Start Date", data: "StartDate", type: "date", className: "center", width: "130", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Due Date", data: "DueDate", type: "date", className: "center", width: "130", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Completion Date", data: "CompletionDate", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } }];
            }
            else if (viewmode == 'productgroup') {
                spnGeneralHeader.innerText = "Manage Product Group";
                spnModuleHeader.innerText = "Manage Product Group";
                dataurl = '/Services/NagrikService.asmx/webProductGroupDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webProductGroupDetailSearch';
                colstructure = [
                    {
                        title: "Product Group", data: "ProductGroupName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Status", data: "ActiveFlagDesc" }];
            }
            else if (viewmode == 'productbrand') {
                spnGeneralHeader.innerText = "Manage Product Brand";
                spnModuleHeader.innerText = "Manage Product Brand";
                dataurl = '/Services/NagrikService.asmx/webProductBrandDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webProductBrandDetailSearch';
                colstructure = [
                    {
                        title: "Brand Name", data: "BrandName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Brand Alias", data: "BrandAlias" },
                    { title: "Status", data: "ActiveFlagDesc" }];
            }
            else if (viewmode == 'pricelist') {
                document.getElementById("btnupload").style.visibility = "visible";
                spnGeneralHeader.innerText = "Manage Price List";
                spnModuleHeader.innerText = "Manage Price List";
                dataurl = '/Services/NagrikService.asmx/webPriceList?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webPriceListSearch';
                colstructure = [
                    {
                        title: "Price List Name", data: "PriceListName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
            }
            else if (viewmode == 'customercategory') {
                spnGeneralHeader.innerText = "Customer Category";
                spnModuleHeader.innerText = "Customer Category";
                dataurl = '/Services/NagrikService.asmx/webCustomerCategory?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCustomerCategorySearch';
                colstructure = [
                    {
                        title: "Category Name", data: "CategoryName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
            }
            else if (viewmode == 'inquirystatus') {
                spnGeneralHeader.innerText = "Define Status";
                spnModuleHeader.innerText = "Define Status";
                dataurl = '/Services/NagrikService.asmx/webInquiryStatusDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webInquiryStatusDetailSearch';
                colstructure = [
                    {
                        title: "Status", data: "InquiryStatusName",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Status Category", data: "StatusCategory",
                        render: function (data, type, row, meta) {
                            if (data == 'InquiryStatus')
                                data = '<span class="chip">' + 'Lead Status' + '</span>';
                            else if (data == 'InquirySource')
                                data = '<span class="chip">' + 'Lead Source' + '</span>';
                            else if (data == 'FollowupType')
                                data = '<span class="chip">' + 'Followup Type' + '</span>';
                            else if (data == 'DisQualifiedReason')
                                data = '<span class="chip">' + 'Dis-Quali.Reason' + '</span>';
                            else
                                data = '<span class="chip">' + data + '</span>';
                            return data;
                        }
                    }];
            }
            else if (viewmode == 'catdesc') {
                spnGeneralHeader.innerText = "Define Category And Description";
                spnModuleHeader.innerText = "Define Category And Description";
                dataurl = '/Services/NagrikService.asmx/webCategoryDescriptionDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCategoryDescriptionDetailSearch';
                colstructure = [
                    {
                        title: "Description", data: "Description",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Category", data: "Category" },
                    ];
            }
            else if (viewmode == 'contents') {

                spnGeneralHeader.innerText = "Manage - Terms & Conditions";
                spnModuleHeader.innerText = "Manage - Terms & Conditions";
                dataurl = '/Services/NagrikService.asmx/webContentsDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webContentsDetailSearch';
                colstructure = [
                    {
                        title: "Header", data: "TNC_Header",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Category", data: "Category" },
                    { title: "Contents", data: "TNC_Content" }];
            }
            else if (viewmode == 'followupstatus') {
                spnGeneralHeader.innerText = "Manage FollowUp Status";
                spnModuleHeader.innerText = "Manage FollowUp Status";
                dataurl = '/Services/NagrikService.asmx/webFollowupStatusDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webFollowupStatusDetailSearch';
                colstructure = [
                    {
                        title: "Status", data: "InquiryStatus",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Status Category", data: "StatusCategory" }];
            }
            else if (viewmode == 'designation') {
                spnGeneralHeader.innerText = "Employee Profile - Designation";
                spnModuleHeader.innerText = "Employee Profile - Designation";
                dataurl = '/Services/NagrikService.asmx/webDesignationDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webDesignationDetailSearch';
                colstructure = [
                    {
                        title: "Designation", data: "Designation",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.DesigCode + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Designation Alias", data: "DesigCode" },
                    { title: "Status", data: "ActiveFlagDesc" }];
            }
            else if (viewmode == 'orgtype') {
                spnGeneralHeader.innerText = "Organization Type";
                spnModuleHeader.innerText = "Organization Type";
                dataurl = '/Services/NagrikService.asmx/webOrgTypeDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webOrgTypeDetailSearch';
                colstructure = [
                    {
                        title: "Organization Type", data: "OrgType",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Status", data: "ActiveFlagDesc" }];
            }
            else if (viewmode == 'document') {
                spnGeneralHeader.innerText = "Document Type Information";
                spnModuleHeader.innerText = "Document Type Information";
                dataurl = '/Services/NagrikService.asmx/webDocumentType?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webDocumentTypeSearch';
                colstructure = [

                    { title: "Document Type", data: "DocumentType" },
                ];
            }
            else if (viewmode == 'proof') {
                spnGeneralHeader.innerText = "Documents Proof Types";
                spnModuleHeader.innerText = "Documents Proof Types";
                dataurl = '/Services/NagrikService.asmx/webProofDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webProofDetailSearch';
                colstructure = [
                    { title: "Proof Name", data: "ProofName", className: "text-left", width: "300" },
                    { title: "Valid For Address Proof", data: "IsAddressProof", className: "center", width: "200" },
                    { title: "Valid For Identity Proof", data: "IsIdentityProof", className: "center", width: "200" },
                    { title: "Status", data: "ActiveFlagDesc", className: "center", width: "100" }];
            }
            else if (viewmode == 'city') {
                spnGeneralHeader.innerText = "Define Geographic Location - City";
                spnModuleHeader.innerText = "Define Geographic Location - City";
                dataurl = '/Services/NagrikService.asmx/webCityDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCityDetailSearch';
                colstructure = [
                    {
                        title: "City", data: "CityName", className: "activator",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.CityCode + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "State", data: "StateName" },
                    { title: "Country", data: "CountryName" }];
            }
            else if (viewmode == 'state') {
                spnGeneralHeader.innerText = "Define Geographic Location - State";
                spnModuleHeader.innerText = "Define Geographic Location - State";
                dataurl = '/Services/NagrikService.asmx/webStateDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webStateDetailSearch';
                colstructure = [
                    {
                        title: "State Name", data: "StateName",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.StateCode + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Country", data: "CountryCode" }];
            }
            else if (viewmode == 'country') {
                spnGeneralHeader.innerText = "Define Geographic Location - Country";
                spnModuleHeader.innerText = "Define Geographic Location - Country";
                dataurl = '/Services/NagrikService.asmx/webCountryDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCountryDetailSearch';
                colstructure = [
                    {
                        title: "Country Name", data: "CountryName",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.CountryCode + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Country", data: "CountryCode" },
                    { title: "Currency Name", data: "CurrencyName" },
                    { title: "Currency Symbol", data: "CurrencySymbol" }
                ];
            }
            else if (viewmode == 'productspecification') {
                spnGeneralHeader.innerText = "Material Specification";
                spnModuleHeader.innerText = "Material Specification";
                dataurl = '/Services/NagrikService.asmx/webproductspecificationDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webproductspecificationDetailSearch';
                colstructure = [
                    { title: "Group Name", data: "GroupHead" },
                    { title: "Order #", data: "ItemOrder" },
                    { title: "Material Head", data: "MaterialHead" },
                    {
                        title: "Material Specification", data: "MaterialSpec", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Remarks", data: "MaterialRemarks" },
                ];
            }
            else if (viewmode == 'checklist') {
                spnGeneralHeader.innerText = "Define Taxation Information";
                spnModuleHeader.innerText = "Define Taxation Information";
                dataurl = '/Services/NagrikService.asmx/webCheckList?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCheckListSearch';
                colstructure = [
                    { title: "Check Head", data: "CheckHead" },
                    { title: "Check Description", data: "CheckDesc" }
                ];
            }
            else if (viewmode == 'othercharge') {
                spnGeneralHeader.innerText = "Define Taxation Information";
                spnModuleHeader.innerText = "Define Taxation Information";
                dataurl = '/Services/NagrikService.asmx/webOtherChargeDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webOtherChargeDetailSearch';
                colstructure = [
                    {
                        title: "Charge Name", data: "ChargeName",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "GST %", data: "GST_Per" },
                    { title: "HSN Code", data: "HSNCODE" },
                    { title: "Tax Type", data: "TaxTypeName" },
                    { title: "BeforeGST Y/N", data: "BeforeGST" }];
            }
            else if (viewmode == 'campaigntemplateemail' || viewmode == 'campaigntemplatesms') {
                spnGeneralHeader.innerText = "Email Campaign Template";
                spnModuleHeader.innerText = "Email Campaign Template";
                if (viewmode == 'campaigntemplateemail')
                    dataurl = '/Services/NagrikService.asmx/webCampaignTemplateDetail?Category=Email&currPageNo=1';
                else
                    dataurl = '/Services/NagrikService.asmx/webCampaignTemplateDetail?Category=SMS&currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webCampaignTemplateDetailSearch';
                colstructure = [
                    {
                        title: "Campaign Subject", data: "CampaignSubject",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.CampaignID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Campaign Category", data: "CampaignCategory" },
                    { title: "Campaign Header", data: "CampaignHeader" },
                    { title: "Campaign Footer", data: "CampaignFooter" }
                ];
            }
            else if (viewmode == 'bankdetails') {
                spnGeneralHeader.innerText = "Organization Bank Details";
                spnModuleHeader.innerText = "Organization Bank Details";
                dataurl = '/Services/NagrikService.asmx/webOrganizationBankDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webOrganizationBankDetailSearch';
                colstructure = [


                    { title: "Bank Name", data: "BankName" },
                    { title: "Bank Account Name", data: "BankAccountName" },
                    { title: "Branch Name", data: "BranchName", className: "center" },
                    { title: "Account Number", data: "BankAccountNo" }
                ];
            }

                //-----------------Location-------------------

            else if (viewmode == 'location') {
                spnGeneralHeader.innerText = "Locations";
                spnModuleHeader.innerText = "Locations";
                dataurl = '/Services/NagrikService.asmx/webLocation?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webLocationSearch';
                colstructure = [


                    { title: "Location Name", data: "LocationName",  className: "center"},
                   
                ];
            }
                //-----------------Punch-------------------

            else if (viewmode == 'punch') {
                spnGeneralHeader.innerText = "Punches";
                spnModuleHeader.innerText = "Punches";
                dataurl = '/Services/NagrikService.asmx/webPunch?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webPunchSearch';
                colstructure = [


                    { title: "Punch Name", data: "PunchName", className: "center" },

                ];
            }
                //-----------------Nature Of call-------------------

            else if (viewmode == 'natureofcall') {
                spnGeneralHeader.innerText = "NatureOfCall";
                spnModuleHeader.innerText = "NatureOfCall";
                dataurl = '/Services/NagrikService.asmx/webNatureOfCall?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webNatureOfCallSearch';
                colstructure = [


                    { title: "NatureOf Call", data: "NatureOfCall", className: "center" },

                ];
            }

                //-----------------Color-------------------

            else if (viewmode == 'color') {
                spnGeneralHeader.innerText = "Colors";
                spnModuleHeader.innerText = "Colors";
                dataurl = '/Services/NagrikService.asmx/webColor?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webColorSearch';
                colstructure = [


                    { title: "Color Name", data: "ColorName", className: "center" },

                ];
            }

                //-----------------Size-------------------

            else if (viewmode == 'size') {
                spnGeneralHeader.innerText = "Sizes";
                spnModuleHeader.innerText = "Sizes";
                dataurl = '/Services/NagrikService.asmx/webSize?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webSizeSearch';
                colstructure = [


                    { title: "Size Name", data: "SizeName", className: "center" },

                ];
            }

            //-----------------Batch-------------------

        else if (viewmode == 'batch') {
                spnGeneralHeader.innerText = "Batches";
                spnModuleHeader.innerText = "Batches";
                dataurl = '/Services/NagrikService.asmx/webBatch?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webBatchSearch';
                colstructure = [


                    { title: "Batch Name", data: "BatchName", className: "center" },
                   
                ];
        }
            //-----------------Grade-------------------

        else if (viewmode == 'grade') {
            spnGeneralHeader.innerText = "Grades";
            spnModuleHeader.innerText = "Grades";
            dataurl = '/Services/NagrikService.asmx/webGrade?currPageNo=1';
            dataurlsearch = '/Services/NagrikService.asmx/webGradeSearch';
            colstructure = [


                { title: "Grade Name", data: "GradeName", className: "center" },

            ];
        }

            //-----------------Shade-------------------

        else if (viewmode == 'shade') {
            spnGeneralHeader.innerText = "Shades";
            spnModuleHeader.innerText = "Shades";
            dataurl = '/Services/NagrikService.asmx/webShade?currPageNo=1';
            dataurlsearch = '/Services/NagrikService.asmx/webShadeSearch';
            colstructure = [


                { title: "Shade Name", data: "ShadeName", className: "center" },

            ];
        }

            else if (viewmode == 'emailtemplate') {
                spnGeneralHeader.innerText = "Manage Email Template";
                spnModuleHeader.innerText = "Manage Email Template";
                dataurl = '/Services/NagrikService.asmx/webEmailTemplate?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webEmailTemplateSearch';
                colstructure = [
                    { title: "Category", data: "Category", className: "center" },
                    {
                        title: "Template ID", data: "TemplateID",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.TemplateID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    {
                        title: "Description", data: "Description"
                    },
                    { title: "Subject", data: "Subject" }
                ];
            }
            else if (viewmode == 'gentemplate') {
                spnGeneralHeader.innerText = "Manage General Template";
                spnModuleHeader.innerText = "Manage General Template";
                dataurl = '/Services/NagrikService.asmx/webGenTemplate?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webGenTemplateSearch';
                colstructure = [
                    { title: "Subject", data: "Subject" },
                    {
                        title: "Description", data: "ContentData"
                    }
                    
                ];
            }
            else if (viewmode == 'holiday') {
                spnGeneralHeader.innerText = "Annual Holidays List";
                spnModuleHeader.innerText = "Annual Holidays List";
                dataurl = '/Services/NagrikService.asmx/webHolidayDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webHolidayDetailSearch';
                colstructure = [
                    {
                        title: "Holiday Name", data: "Holiday_Name",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Year", data: "Holiday_Year" },
                    { title: "Date", data: "Holiday_Date", type: "date", className: "center", width: "150", render: function (data, type, full) { return (data) ? moment(data).format('DD-MMM-YYYY') : ''; } },
                    { title: "Type", data: "Holiday_Type" },
                    { title: "Description", data: "Holiday_Description" }];
            }
            else if (viewmode == 'todocategory') {
                spnGeneralHeader.innerText = "TODO Category";
                spnModuleHeader.innerText = "TODO Category";
                dataurl = '/Services/NagrikService.asmx/webTODOCategoryDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webTODOCategoryDetailSearch';
                colstructure = [
                    {
                        title: "Category Name", data: "TaskCategoryName", className: "center", width: "300",
                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
            }
            else if (viewmode == 'wallet') {
                spnGeneralHeader.innerText = "Define - Digital Wallet";
                spnModuleHeader.innerText = "Define - Digital Wallet";
                dataurl = '/Services/NagrikService.asmx/webWalletDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webWalletDetailSearch';
                colstructure = [
                    {
                        title: "Wallet Name", data: "WalletName",

                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    }];
            }
            else if (viewmode == 'expensetype') {
                spnGeneralHeader.innerText = "Define - Types Of Expense";
                spnModuleHeader.innerText = "Define - Types Of Expense";
                dataurl = '/Services/NagrikService.asmx/webExpenseTypeDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webExpenseTypeDetailSearch';
                colstructure = [
                    {
                        title: "ExpenseType Name", data: "ExpenseTypeName",

                        render: function (data, type, row, meta) {
                            data = '<a class="activator" href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                { title: "Location Required", data: "IsLocationRequired" }];
            }
            else if (viewmode == 'productopening') {
                spnGeneralHeader.innerText = "Manage Product Opening Stock";
                spnModuleHeader.innerText = "Manage Product Opening Stock";
                dataurl = '/Services/NagrikService.asmx/webProductOpeningDetail?currPageNo=1';
                dataurlsearch = '/Services/NagrikService.asmx/webProductOpeningDetailSearch';
                // ----------------------------------------------------------------------                                
                colstructure = [
                    {
                        title: "Product Name", data: "ProductName", className: "left-align limited", width: "250",
                        render: function (data, type, row, meta) {
                            data = '<a href="javascript:gridAction(\'edit\',\'' + row.pkID + '\');">' + data + '</a>';
                            return data;
                        }
                    },
                    { title: "Product Alias", data: "ProductAlias" },
                    { title: "Product Group", data: "ProductGroupName", className: "center-align" },
                    { title: "Unit", data: "Unit", className: "center-align" },
                    { title: "Opening Stock Quantity", data: "Quantity", className: "right-align" },
                    { title: "Opening Average Rate", data: "Quantity", className: "right-align" },
                    { title: "Opening Stock Value", data: "Quantity", className: "right-align" }

                ];
            }

            // -------------------------------------------------------------------------
            // Storing DataUrl for Export Purpose
            // -------------------------------------------------------------------------

            jQuery("#hdnDataUrl").val(dataurl);
            jQuery("#hdnDataUrlSearch").val(dataurlsearch);

            // -------------------------------------------------------------------------
            // Configure Edit/Delete Button Display 
            // -------------------------------------------------------------------------

            if (jQuery("#hdnActionFlag").val() == 'true') {
                if (viewmode == 'customer' || viewmode == 'customerhexagon') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.CustomerID + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.CustomerID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'products' || viewmode == 'producthexagon') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'orgtype') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'designation') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.DesigCode + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.DesigCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'proof') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.ProofID + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.ProofID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'orgstructure') {
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
                else if (viewmode == 'quotation' || viewmode == 'salesorder' || viewmode == 'salesbill' || viewmode == 'purchasebill' || viewmode == 'salesorderclinic' || viewmode == 'salesbillclinic') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';

                            if (viewmode == 'salesorder' || viewmode == 'salesorderclinic') {
                                // print S.O
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/>';
                                // print Performa
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'printpi\',\'' + row.pkID + '\');"/>';
                            }
                            else {
                                // Print Normal Module Document (PDF)
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'print\',\'' + row.pkID + '\');"/>';
                            }
                            // ---------------------------------------------------
                            if (viewmode == 'quotation') {
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/recaptcha.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'regenerate\',\'' + row.pkID + '\');"/>';
                            }
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'inquiry' || viewmode == 'inquiryclinic') {
                    var InquiryShare = jQuery.trim(jQuery("#hdnInquiryShare").val());
                    var userRole = jQuery.trim(jQuery("#hdnLoginUserRole").val());
                    // ---------------------------------------------------------------
                    if (InquiryShare == "Yes" && (userRole == 'admin' || userRole == 'bradmin')) {

                        colstructure.splice(0, 0, {
                            title: "Action", className: "center", width: "60", mRender: function (data, type, row) {
                                var rtnval = "";
                                rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/multi-user-icon.jpg" alt="" style="width:20px; height:20px;" onclick="gridAction(\'share\',\'' + row.pkID + '\');"/>';
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                                return rtnval;
                                //return '<img src="images/multi-user-icon.jpg" alt="" style="width:20px; height:20px;" onclick="gridAction(\'share\',\'' + row.pkID + '\');"/>';
                            }
                        });
                    }
                }
                else if (viewmode == 'externalleads') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'city') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.CityCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.CityCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'state') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.StateCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.StateCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'country') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.CountryCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.CountryCode + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }

                else if (viewmode == 'bundle') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.BundleId + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.BundleId + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'campaigntemplateemail' || viewmode == 'campaigntemplatesms') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.CampaignID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.CampaignID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'bankdetails') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }

                    //----------------------Location----------------------
                else if (viewmode == 'location') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                    //----------------------color----------------------
                else if (viewmode == 'color') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                    //----------------------Batch----------------------
                else if (viewmode == 'batch') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                    //----------------------Size----------------------
                else if (viewmode == 'size') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }

                //----------------------Punch----------------------
            else if (viewmode == 'punch') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }

                //----------------------NatureOfCall----------------------
            else if (viewmode == 'natureofcall') {
                colstructure.splice(0, 0, {
                    title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                        var rtnval = "";
                        rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                        rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                        rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                        rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                        return rtnval;
                    }
                });
            }

                    //----------------------Grade----------------------
                else if (viewmode == 'grade') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }

                    //----------------------Shade----------------------
                else if (viewmode == 'shade') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }

                else if (viewmode == 'emailtemplate') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            if ($("#hdnEditFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.TemplateID + '\');"/>';
                            if ($("#hdnDelFlag").val() == 'true')
                                rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.TemplateID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'gentemplate') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
                else if (viewmode == 'productopening') {
                    colstructure.splice(0, 0, {
                        title: "Action", className: "center", width: "30", mRender: function (data, type, row) {
                            var rtnval = "";
                            rtnval = '<div style="padding:2px; border-radius:8px; background-color:silver;">';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.CampaignID + '\');"/>';
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
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',\'' + row.pkID + '\');"/>';
                            rtnval += '&nbsp;&nbsp;&nbsp;&nbsp;</div>';
                            return rtnval;
                        }
                    });
                }
            }
            // -------------------------------------------------------------------------
            // Design and Load Table as per above configuration 
            // -------------------------------------------------------------------------
            //jQuery.ajax({ type: "post", url: "/Services/NagrikService.asmx/cleanSessionData?varSession='ReturnTotalRecord'", data: '', contentType: "application/json; charset=utf-8", dataType: "json", success: function (response) { } });
            // ---------------------------------------------------
            $('#dvRegion').css("display", "block");
            var gridTable;
            jQuery.ajax({
                url: dataurl,
                type: "get",
                success: function (data) {
                    var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                    
                    //alert('loadpagelayout' + myTotalRecord);
                    
                    $("#spnPageNo").text("Page # : " + $("#hdnCurrPageNo").val());
                    //jQuery.ajax({
                    //    url: "/Services/NagrikService.asmx/getSessionObject",
                    //    type: "post",
                    //    contentType: "application/json; charset=utf-8",
                    //    dataType: "json",
                    //    success: function (data) {
                    //        var myTotalRecord = data.d.toString();
                    //        $("#spnRecordCount").text("Total Count : " + myTotalRecord);
                    //    }
                    //});
                    jQuery.ajax({
                        url: "Services/NagrikService.asmx/getSessionObject",
                        type: "GET",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (datapageno) {
                            var resultdata11 = JSON.parse(datapageno.d);
                            var myTotalRecord1 = resultdata11[0].value;
                            if (myTotalRecord1 != null && myTotalRecord1 != '') {
                                $("#spnRecordCount").text("Total Count : " + myTotalRecord1);
                            }
                        }
                    });
                    // ------------------------------------------------------------
                    //var sortingColNo = 0;
                    //var sortingOrder = "asc";
                    sortingDef = [[0, "asc"]];
                    // ----------------------------------------------
                    if (viewmode == 'inquiry' || viewmode == 'quotation' || viewmode == 'followup' || viewmode == 'leaverequest') {
                        //sortingColNo = 0;
                        //sortingOrder = "desc";
                        sortingDef = [[0, "desc"]];
                    }
                    if (viewmode == 'salesorder') {
                        sortingDef = [[0, "desc"], [1, "desc"]];
                    }
                    if (viewmode == 'salesorderclinic') {
                        sortingDef = [[0, "desc"], [1, "desc"]];
                    }
                    if (viewmode == 'inquirystatus') {
                        //sortingColNo = 1;
                        //sortingOrder = "asc";
                        sortingDef = [[1, "asc"]];
                    }
                    if (viewmode == 'externalleads') {
                        //sortingColNo = 1;
                        //sortingOrder = "desc";
                        sortingDef = [[2, "desc"]];
                    }
                    // --------------------------------------------------
                    // Loading and Drawing Datatable .... From Database
                    // --------------------------------------------------
                    if ($.fn.dataTable.isDataTable('#tblView')) {
                        $('#tblView').DataTable().destroy();
                        $('#tblView thead').empty();
                        $('#tblView tbody').empty();
                        //$('#tblView').clear();
                    }
                    // ------------------------------------------
                    //alert('before table load')
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
                    //alert('after table load')
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
            // ------------------------------------------
            //$.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().fixedColumns().relayout();
        }

        function setTableRowFormat(viewmode, row, data, dataIndex) {
            if (viewmode == 'externalleads') {
                //jQuery("td:eq(0)", row).css("font-weight", "bold");
                //jQuery("td:eq(0)", row).css("color", "Red");
            }
            if (viewmode == 'inquiry') {
                //jQuery("td:eq(4),td:eq(2)", row).css("font-weight", "bold");
                if (jQuery.trim(data.InquiryStatus) == 'Open') {
                    jQuery("td:eq(4)", row).css("color", "Blue");
                }
                else if (jQuery.trim(data.InquiryStatus) == 'On Hold' || data.InquiryStatus == 'Work In Progress') {
                    jQuery("td:eq(4)", row).css("color", "Gray");
                }
                else if (jQuery.trim(data.InquiryStatus) == 'Close - Lost') {
                    jQuery("td", row).css("color", "Red");
                }
                else if (jQuery.trim(data.InquiryStatus) == 'Close - Success') {
                    jQuery("td:eq(4)", row).css("color", "Green");
                }
            }
            if (viewmode == 'followup') {
                jQuery("td:eq(1)", row).css("font-weight", "bold");
                if (jQuery.trim(data.NoFollowup) != '') {
                    jQuery("td:eq(3)", row).css("color", "Red");
                    jQuery("td:eq(4)", row).css("color", "Maroon");
                }
            }
            if (viewmode == 'quotation') {
                jQuery("td:eq(3),td:eq(4)", row).css("font-weight", "bold");
                jQuery("td:eq(4)", row).css("color", "navy");
            }
            if (viewmode == 'projects' || viewmode == 'customer') {
                jQuery("td:eq(0)", row).css("color", "DarkBlue");
                jQuery("td:eq(0)", row).css("font-weight", "bold");
            }
            if (viewmode == 'products' || viewmode == 'producthexagon') {
                if (jQuery.trim(data.ActiveFlagDesc) == 'Active')
                    jQuery("td", row).css("color", "black");
                else
                    jQuery("td", row).css("color", "red");
            }
            if (viewmode == 'todo') {
                if (data.Priority == 'Low') {
                    //jQuery(row).css("background-color", "Orange");
                    jQuery("td:eq(1)", row).css("background-color", "Silver");
                    jQuery("td:eq(1)", row).css("color", "White");
                }
                if (data.Priority == 'Medium') {
                    jQuery("td:eq(1)", row).css("background-color", "Orange");
                    jQuery("td:eq(1)", row).css("color", "White");
                }
                if (data.Priority == 'High') {
                    jQuery("td:eq(1)", row).css("background-color", "Red");
                    jQuery("td:eq(1)", row).css("color", "White");
                }
                // ---------------------------
                if (data.TaskStatus == 'Overdue') {
                    //jQuery("td:eq(0),td:eq(2),td:eq(3),td:eq(4),td:eq(5)", row).css("color", "Red");
                    jQuery("td:eq(5)", row).css("background-color", "Red");
                    jQuery("td:eq(5)", row).css("color", "White");
                }
            }
        }

        function setAddEditDel(menuid)
        {
            if (menuid != '')
            {
                $("#hdnMenuID").val(viewmode);
                // -----------------------------------------------------------
                $.ajax({
                    type: "POST",
                    url: 'Services/NagrikService.asmx/GeneralAddEditDel',
                    data: '{menuid:\'' + menuid + '\'}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (data) {
                        var resultdata = JSON.parse(data.d);
                        var resp = resultdata[0];
                        $("#hdnAddFlag").val(resp.AddFlag);
                        $("#hdnEditFlag").val(resp.EditFlag);
                        $("#hdnDelFlag").val(resp.DelFlag);
                    },
                    failure: function (errMsg) { }
                });
            }
        }
        // -------------------------------------------------------------------------
        // Function : This will invoke by Edit/Delete button
        // -------------------------------------------------------------------------
        function gridAction(mode, id) {
            var cbWidth = "90%";
            var cbHeight = "90%";
            var varView = jQuery("#hdnView").val();
            //alert('Module : ' + mode)
            var pageUrl = '', deleteUrl = '', deletePara = '';
            // -------------------------------------------------------------
            if (varView == 'customer')
            {
                if ($("#hdnSerialKey").val() == 'HEXA-09PM-56JK-KG88')  //Hexagon Customer
                {
                    pageUrl = "CustomerHexagon.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'CustomerHexagon.aspx/DeleteCustomer';
                }
                else
                {
                    pageUrl = "Customers.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'Customers.aspx/DeleteCustomer';
                }
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'broadcast') {
                cbWidth = "60%";
                cbHeight = "80%";
                pageUrl = "BroadcastMessage.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'BroadcastMessage.aspx/DeleteBroadcastMessage';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'products')
            {
                if ($("#hdnSerialKey").val() == 'HEXA-09PM-56JK-KG88')  //Hexagon Product
                {
                    pageUrl = "ProductHexagon.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'ProductHexagon.aspx/DeleteProduct';
                }
                else
                {
                    pageUrl = "Products.aspx?mode=" + mode + "&id=" + id;
                    deleteUrl = 'Products.aspx/DeleteProduct';
                }
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'productgroup') {
                cbWidth = "40%";
                cbHeight = "50%";
                pageUrl = "ProductGroup.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ProductGroup.aspx/DeleteProductGroup';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'productbrand') {
                cbWidth = "40%";
                cbHeight = "65%";
                pageUrl = "ProductBrand.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ProductBrand.aspx/DeleteProductBrand';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'pricelist') {
                cbWidth = "40%";
                cbHeight = "65%";
                pageUrl = "PriceList.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'PriceList.aspx/DeletePriceList';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'customercategory') {
                cbWidth = "40%";
                cbHeight = "65%";
                pageUrl = "CustomerCategory.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'CustomerCategory.aspx/DeleteCustomerCategory';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'inquirystatus') {
                cbWidth = "30%";
                cbHeight = "75%";
                pageUrl = "InquiryStatus.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'InquiryStatus.aspx/DeleteInquiryStatus';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'catdesc') {
                cbWidth = "30%";
                cbHeight = "75%";
                pageUrl = "CategoryDescription.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'CategoryDescription.aspx/DeleteCategoryDescription';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'contents') {
                cbWidth = "60%";
                cbHeight = "75%";
                pageUrl = "Contents.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Contents.aspx/DeleteContents';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'orgtype') {
                cbWidth = "30%";
                cbHeight = "75%";
                pageUrl = "OrgTypes.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'OrgTypes.aspx/DeleteOrgType';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'designation') {
                cbWidth = "40%";
                cbHeight = "75%";
                pageUrl = "Designations.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Designations.aspx/DeleteDesignation';
                deletePara = '{DesigCode:\'' + id + '\'}';
            }
            else if (varView == 'todocategory') {
                cbWidth = "40%";
                cbHeight = "65%";
                pageUrl = "TODOCategory.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'TODOCategory.aspx/DeleteTODOCategory';
                deletePara = '{pkID:' + id + '}';
            }
            else if (varView == 'document') {
                var cbWidth = "70%";
                var cbHeight = "75%";
                pageUrl = "Document_Type.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Document_Type.aspx/DeleteDocument_Type';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'proof') {
                cbWidth = "60%";
                cbHeight = "75%";
                pageUrl = "Proof.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Proof.aspx/DeleteProof';
                deletePara = '{ProofID:\'' + id + '\'}';
            }
            else if (varView == 'projects') {
                var cbWidth = "75%";
                var cbHeight = "85%";
                pageUrl = "Projects.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Projects.aspx/DeleteProjects';
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
            else if (varView == 'city') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "ManageCity.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ManageCity.aspx/DeleteCity';
                deletePara = '{CityCode:\'' + id + '\'}';
            }
            else if (varView == 'state') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "ManageState.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ManageState.aspx/DeleteState';
                deletePara = '{StateCode:\'' + id + '\'}';
            }
            else if (varView == 'country') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "ManageCountry.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ManageCountry.aspx/DeleteCountry';
                deletePara = '{CountryCode:\'' + id + '\'}';
            }
            else if (varView == 'productspecification') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Product_Specification.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Product_Specification.aspx/DeleteProductSpecification';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'checklist') {
                var cbWidth = "70%";
                var cbHeight = "75%";
                pageUrl = "CheckList.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'CheckList.aspx/DeleteCheckList';
                deletePara = '{pkID:\'' + id + '\'}';
                debugger;
            }
            else if (varView == 'othercharge') {
                var cbWidth = "70%";
                var cbHeight = "75%";
                pageUrl = "OtherCharge.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'OtherCharge.aspx/DeleteOtherCharge';
                deletePara = '{ChargeID:\'' + id + '\'}';
            }
            else if (varView == 'bundle') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Bundle.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Bundle.aspx/DeleteBundle';
                deletePara = '{BundleID:\'' + id + '\'}';
            }
            else if (varView == 'campaigntemplateemail') {
                var cbWidth = "60%";
                var cbHeight = "90%";
                pageUrl = "CampaignTemplate.aspx?mode=" + mode + "&category=Email&id=" + id;
                deleteUrl = 'CampaignTemplate.aspx/DeleteCampaignTemplate';
                deletePara = '{CampaignID:\'' + id + '\'}';
            }
            else if (varView == 'bankdetails') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "BankDetails.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'BankDetails.aspx/DeleteBankDetails';
                deletePara = '{pkID:\'' + id + '\'}';
            }
                //--------------------Location-----------------
            else if (varView == 'location') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Location.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Location.aspx/DeleteLocation';
                deletePara = '{pkID:\'' + id + '\'}';
            }
                //--------------------grade-----------------
            else if (varView == 'grade') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Grade.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Grade.aspx/DeleteGrade';
                deletePara = '{pkID:\'' + id + '\'}';
            }
                //--------------------color-----------------
            else if (varView == 'color') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Color.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Color.aspx/DeleteColor';
                deletePara = '{pkID:\'' + id + '\'}';
            }
                //--------------------Batch-----------------
            else if (varView == 'batch') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Batch.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Batch.aspx/DeleteBatch';
                deletePara = '{pkID:\'' + id + '\'}';
            }
                //--------------------Size-----------------
            else if (varView == 'size') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Size.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Size.aspx/DeleteSize';
                deletePara = '{pkID:\'' + id + '\'}';
            }
                //--------------------Shade-----------------
            else if (varView == 'shade') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Shade.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Shade.aspx/DeleteShade';
                deletePara = '{pkID:\'' + id + '\'}';
            }
                //--------------------Punch-----------------
            else if (varView == 'punch') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Punch.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Punch.aspx/DeletePunch';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'campaigntemplatesms') {
                var cbWidth = "60%";
                var cbHeight = "90%";
                pageUrl = "CampaignTemplate.aspx?mode=" + mode + "&category=SMS&id=" + id;
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
            else if (varView == 'emailtemplate') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "EmailTemplate.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'EmailTemplate.aspx/DeleteEmailTemplate';
                deletePara = '{pTemplateID:\'' + id + '\'}';
            }
            else if (varView == 'gentemplate') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "GeneralTemplate.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'GeneralTemplate.aspx/DeleteGenTemplate';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'holiday') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "ManageHoliday.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ManageHoliday.aspx/DeleteHoliday';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'complaint') {
                var cbWidth = "80%";
                var cbHeight = "95%";
                pageUrl = "Complaint.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Complaint.aspx/DeleteComplaint';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'natureofcall') {
                var cbWidth = "80%";
                var cbHeight = "95%";
                pageUrl = "NatureOfCall.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'NatureOfCall.aspx/DeleteNatureCall';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'externalleads') {
                var cbWidth = "90%";
                var cbHeight = "90%";
                pageUrl = "ExternalLeads.aspx?mode=" + mode + "&id=" + id
                deleteUrl = 'ExternalLeads.aspx/DeleteExternalLeads';
                deletePara = '{pkID:\'' + id + '\'}';
            }
            else if (varView == 'wallet') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "Wallet.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'Wallet.aspx/DeleteWallet';
                deletePara = '{WalletID:\'' + id + '\'}';
            }
            else if (varView == 'expensetype') {
                var cbWidth = "60%";
                var cbHeight = "75%";
                pageUrl = "ExpenseType.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ExpenseType.aspx/DeleteExpenseType';
                deletePara = '{ExpenseTypeID:\'' + id + '\'}';
            }
            else if (varView == 'productopening') {
                pageUrl = "ProductOpeningStock.aspx?mode=" + mode + "&id=" + id;
                deleteUrl = 'ProductOpeningStock.aspx/DeleteProduct';
                deletePara = '{pkID:' + id + '}';
            }

            // -------------------------------------------------------------
            // Section : Call page for ADD / EDIT
            // -------------------------------------------------------------
            if (mode == 'add' || mode == 'edit' || mode == 'view') {
                if (pageUrl != null && pageUrl != '')
                {
                    pageUrl = pageUrl + "&passFlag=" + $("#hdnAddFlag").val();
                    $('#ifrModulePopup').attr('src', pageUrl);
                    $find("mpe").show();
                }
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
                                __doPostBack('deleterec', '');
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
                            var QuotationVersion = jQuery.trim(jQuery("#hdnQuotationVersion").val());
                            if (QuotationVersion == "1" || QuotationVersion == "2") {
                                jQuery.colorbox({ width: cbWidth, height: cbHeight, iframe: true, href: 'Quotation_old.aspx?id=' + returnID, onClosed: function () { location.reload(); } });
                            }
                            else {
                                jQuery.colorbox({ width: cbWidth, height: cbHeight, iframe: true, href: 'Quotation.aspx?id=' + returnID, onClosed: function () { location.reload(); } });
                            }
                        }
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
            }
            // -------------------------------------------------------------
            // Section : QUOTATION PRINTING 
            // -------------------------------------------------------------
            if (mode == 'print' && (varView == 'quotation')) {
                var QuotPageUrl = '', QuotationVersion = '', QuotationNoForPDF = '';
                QuotationVersion = jQuery.trim(jQuery("#hdnQuotationVersion").val());
                QuotationNoForPDF = '';
                // ----------------------------------------------------------------
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    jQuery.confirm({
                        title: 'Print - With Header', content: 'Are you sure, You want to print Header !', type: 'red', typeAnimated: true,
                        buttons: {
                            tryAgain: {
                                text: 'Yes', btnClass: 'btn-red', action: function () {
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
                                }
                            },
                            close: {
                                text: 'No', btnClass: 'btn-blue', action: function () {
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
                            }
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
            // ----------------------------------------------------------------------
            if ((mode == 'print' || mode == 'printpi') && varView == 'salesorder') {
                var SOPageUrl = '';
                var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                // =========================================================================
                if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                    jQuery.confirm({
                        title: 'Print - With Header',
                        content: 'Are you sure, You want to print Header !',
                        type: 'red',
                        typeAnimated: true,
                        buttons: {
                            tryAgain: {
                                text: 'Yes',
                                btnClass: 'btn-red',
                                action: function () {
                                    var x = PageMethods.setPrintHeader('yes');
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
                            },
                            close: {
                                text: 'No',
                                btnClass: 'btn-blue',
                                action: function () {
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
                            }
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
            // -------------------------------------------------------------
            // Section : Call page for Share Inquiry 
            // -------------------------------------------------------------
            if (mode == 'share') {
                $.colorbox({ width: "90%", height: "90%", iframe: true, href: "InquiryShare.aspx?id=" + id + "&userid=" + jQuery.trim(jQuery("#hdnLoginUserID").val()) });
            }
        }

        function ShowPDFfile(repFilename) {
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
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
                        $.colorbox({
                            fixed: true, width: "98%", height: "98%", rel: 'showInq', inline: true, iframe: true,
                            href: "InquiryInfo.aspx?mode=view&id=" + returnID,
                            onClosed: function () { }
                        });
                    }
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });
        }

        $(".btnTopRightCorner").click(function () {
            if ($("#spnPageNo").text() != 'Filtered Data') {
                var pgSource = $('#ifrModulePopup').attr('src');
                var strPos = pgSource.indexOf('mode=edit');
                var strPos1 = pgSource.indexOf('mode=add');
                var pageno = $("#hdnCurrPageNo").val();
                //alert(pageno);
                if (strPos >= 0 || strPos1 >= 0) {
                    jQuery.ajax({
                        url: '/Services/NagrikService.asmx/setGridPageSize?pSize=' + $("#drpPageSize").val(), type: "get",
                        success: function (data) {
                            //alert('success-123')
                            $("#hdnCurrPageNo").val(pageno)
                            gridReloadData('refresh');
                        }
                    });

                }
                //if (strPos >= 0 || strPos1 >= 0) {
                //    gridReloadData('refresh');
                //}
            }
        })

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Below Section : SetUp & Load DataTable
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        $(document).ready(function () {
            loadPageLayout();           
        });

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
    </script>    
</asp:Content>
