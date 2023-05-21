<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrders.aspx.cs" Inherits="StarsProject.PurchaseOrders" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myModuleAttachment.ascx" TagPrefix="uc1" TagName="myModuleAttachment" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />

    <script src="app-assets/vendors/select2/select2.full.min.js"></script>
    <script src="app-assets/js/scripts/form-select2.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <style type="text/css">
        table.responsive-table.striped.width-100 th {
            background-color: #6868d6 !important;
            color: white !important;
        }

        .clDiv70 {
            height: 50px;
            vertical-align: middle;
        }

        .imgChart {
            vertical-align: middle;
            Height: 40px;
            width: 40px;
        }

        #drpSelectDepartment {
            margin: 7px 0px;
            font-family: Verdana,Arial;
            font-size: 12px;
            vertical-align: middle;
        }

        .clTrans {
            background-color: transparent;
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
        /* ==================================== */
        /***** 19. STANDARD TABLES STYLES *****/
        /* ==================================== */
        .table input:not([type]), input[type=text]:not(.browser-default), input[type=password]:not(.browser-default),
        input[type=email]:not(.browser-default), input[type=url]:not(.browser-default), input[type=time]:not(.browser-default),
        input[type=date]:not(.browser-default), input[type=datetime]:not(.browser-default), input[type=datetime-local]:not(.browser-default),
        input[type=tel]:not(.browser-default), input[type=number]:not(.browser-default), input[type=search]:not(.browser-default), textarea.materialize-textarea {
            margin: 0px !important;
            height: 1.5rem !important;
            border-radius: 3px !important;
            font-size: 13px !important;
        }

        #tblQuotationDetail > tbody > tr > td {
            padding-top: 0px !important;
            padding-bottom: 0px !important;
        }

        #tblCharge > tbody > tr > td {
            padding-top: 0px !important;
            padding-bottom: 0px !important;
        }

        #tblSummary > tbody > tr > td {
            padding-top: 0px !important;
            padding-bottom: 0px !important;
        }

        .limited {
            white-space: nowrap;
            max-width: 300px;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        td, th {
            padding: 3px !important;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            
        });

        function pageLoad(sender, args) {
            $('tabs').tabs();
            $('.datepicker').datepicker({
                defaultDate: new Date((new Date()).getFullYear(), (new Date()).getMonth(), (new Date()).getDay()),
                format: "dd-mm-yyyy"
            });
            $('.timepicker').timepicker();
            $(".inputOther input:text").focus(function () { $(this).select(); });
            $(".inputOther input:text").click(function () { $(this).select(); });
            $('#menuDiscovery').click(function () { $('.tap-target').tapTarget('open'); });
            $('#txtOrderNo').focus();
            // --------------------------------------------------------
            $(function () {
                $(".tabs a").click(function () {
                    $("#hdnCurrTab").val($(this).attr("href").replace("#", ""));
                    retainTabPosition();
                });
            });
            // --------------------------------------------------------
            retainTabPosition();
            // --------------------------------------------------------
            setHiddenControls();
        }
        function retainTabPosition() {

            var currTab = $("#hdnCurrTab").val();

            $("#lnkProducts").removeClass("active");
            $("#lnkTNC").removeClass("active");
            $("#lnkEmail").removeClass("active");
            $("#lnkPaySch").removeClass("active");
            $("#lnkFollowUp").removeClass("active");
            $("#lnkDelivery").removeClass("active");
            $("#lnkAttachment").removeClass("active");
            $("#lnkCheckList").removeClass("active");

            $("#pnl_Products").css("display", "none");
            $("#pnl_TNC").css("display", "none");
            $("#pnl_Email").css("display", "none");
            $("#pnl_PaySch").css("display", "none");
            $("#pnl_FollowUp").css("display", "none");
            $("#pnl_Delivery").css("display", "none");
            $("#pnl_Attachment").css("display", "none");
            $("#pnl_CheckList").css("display", "none");

            if (currTab == 'pnl_Products') {
                $("#lnkProducts").addClass("active");
                $("#pnl_Products").css("display", "block");
            }
            else if (currTab == 'pnl_TNC') {
                $("#lnkTNC").addClass("active");
                $("#pnl_TNC").css("display", "block");
            }
            else if (currTab == 'pnl_Email') {
                $("#lnkEmail").addClass("active");
                $("#pnl_Email").css("display", "block");
            }
            else if (currTab == 'pnl_PaySch') {
                $("#lnkPaySch").addClass("active");
                $("#pnl_PaySch").css("display", "block");
            }
            else if (currTab == 'pnl_FollowUp') {
                $("#lnkFollowUp").addClass("active");
                $("#pnl_FollowUp").css("display", "block");
            }
            else if (currTab == 'pnl_Delivery') {
                $("#lnkDelivery").addClass("active");
                $("#pnl_Delivery").css("display", "block");
            }
            else if (currTab == 'pnl_Attachment') {
                $("#lnkAttachment").addClass("active");
                $("#pnl_Attachment").css("display", "block");
            }
            else if (currTab == 'pnl_CheckList') {
                $("#lnkCheckList").addClass("active");
                $("#pnl_CheckList").css("display", "block");
            }
        }
        function gridAction(mode, id) {
            document.getElementById('spnModuleHeader').innerText = "General Template Information";
            var pageUrl = ''
            var id = jQuery("#drpQuotationCategory").val();
            pageTitle = "Manage General Template";
            pageUrl = "GeneralTemplate.aspx?mode=" + mode + "&id=" + (id != '' && id != null ? id : 0);
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function showErrorMessage(strMess) {
            jQuery.confirm({ title: 'Data Validation', content: 'Are you sure, You want to delete record !', type: 'red', typeAnimated: true });
        }
        /*---------------------------------------------------------*/
        function calcQuotation() {
            var q = parseFloat(jQuery("#txtQuantity").val() != '' ? jQuery("#txtQuantity").val() : 0);
            var ur = parseFloat(jQuery("#txtUnitRate").val() != '' ? jQuery("#txtUnitRate").val() : 0);
            var dp = parseFloat(jQuery("#txtDiscountPercent").val() != '' ? jQuery("#txtDiscountPercent").val() : 0);

            var nr = (ur - ((ur * dp) / 100)).toFixed(2);
            jQuery("#txtNetRate").val(nr);

            var a = (q * nr).toFixed(2);
            jQuery("#txtAmount").val(a);

            var tr = parseFloat(jQuery("#txtTaxRate").val() != '' ? jQuery("#txtTaxRate").val() : 0);
            ta = ((a * tr) / 100).toFixed(2);
            jQuery("#txtTaxAmount").val(ta);

            na = (parseFloat(a) + parseFloat(ta)).toFixed(2);
            jQuery("#txtNetAmount").val(na);
        }

        function openCustomerInfo(cat) {
            document.getElementById('spnModuleHeader').innerText = "Customer Information";
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        /*Add Product view on Product Name Vikram Rajput 15-07-2020*/
        function openProductInfo(cat) {
            document.getElementById('spnModuleHeader').innerText = "Product Information";
            var keyid = "0";
            var pageUrl = "Products.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        function openSpecification(pProdID) {
            document.getElementById('spnModuleHeader').innerText = "Product Specification";
            var t11;
            t11 = jQuery("#txtOrderNo").val();
            var pageUrl = "ProductSpecification.aspx?Module=PurchaseOrder&DocNo=" + t11 + "&FinishProductID=" + pProdID;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        $('.select2').select2({
            dropdownAutoWidth: true, width: '100%', containerCssClass: 'select-sm'
        });

        function getSelectedIndents() {
            var test = $(".select2-selection__rendered").find('li.select2-selection__choice').text();
            $("#hdnSelectedReference").val(test.substring(1).replace("×", ","));
            // --------------------------------------------------
        }
        function setHiddenControls() {
            var userData = JSON.stringify({ "pPageName": 'purchaseorder' });
            $.ajax({
                type: "POST",
                url: "/Services/NagrikService.asmx/GetPageHiddenControls",
                data: userData,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    $("#hdnHiddenControl").val(data.d);
                    // ---------------------------------------------------------------
                    $('#frmPurchaseOrder *').filter(':input').each(function () {
                        var idName = $(this).attr('id') + ',';
                        if ($("#hdnHiddenControl").val().indexOf(idName) >= 0) {
                            $(this).parent().hide();
                        }
                    });
                },
                failure: function (errMsg) { alert(errMsg); }
            });
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmPurchaseOrder" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <%-- Bootstrap Quotation Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnQuotationSpecification" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCurrTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Products" />
                <asp:HiddenField ID="hdnPurchaseOrderVersion" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnPurchaseOrderCurrency" runat="server" ClientIDMode="Static" Value="No" />
                <asp:HiddenField ID="hdnSelectedReference" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnHiddenControl" runat="server" ClientIDMode="Static" Value=""/>
                <div id="myModal" style="display: block; width: 98%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="col m12">
                                    <div class="row">
                                        <div class="input-field col m2">
                                            <label class="active" for="txtOrderNo">Order #</label>
                                            <asp:TextBox ID="txtOrderNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder="" MaxLength="20" />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtOrderDate">Order Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtOrderDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m4">
                                            <asp:HiddenField ID="hdnCustStateID" runat="server" ClientIDMode="Static" />
                                            <label class="active" for="txtCustomerName">
                                                Supplier Name  
                                                <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block; padding: 6px 0px 0px 0px;" />
                                                </a>
                                                <a href="javascript:openCustomerInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" style="display: inline-block; padding: 6px 0px 0px 0px;" />
                                                </a>
                                            </label>
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" AutoPostBack="true" OnTextChanged="txtCustomerName_TextChanged" TabIndex="3" placeholder="Minimum 3 chars to activate search" MaxLength="100" />
                                        </div>
                                        <% if (hdnPurchaseOrderCurrency.Value.ToLower() == "yes" || hdnPurchaseOrderCurrency.Value.ToLower() == "y")
                                            { %>
                                        <div class="input-field col m2">
                                            <label class="active" for="drpCurrency">Select Currency</label>
                                            <asp:DropDownList ID="drpCurrency" runat="server" placeholder="" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="7" />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtExchangeRate">Exchange Rate</label>
                                            <asp:TextBox ID="txtExchangeRate" runat="server" MaxLength="12" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="8" />
                                        </div>
                                        <% } %>
                                       <%-- <% if (!hdnSerialKey.Value.Contains("ECO3-2G21-TECH-3MRT"))  //ECOTECH
                                            { %>--%>
                                        <div class="input-field col m3 hide">
                                            <label class="active" for="drpApprovalStatus" style="margin-left: 15px;">Approval Status</label>
                                            <asp:DropDownList ID="drpApprovalStatus" runat="server" class="select2-theme browser-default hide" EnableViewState="true" ClientIDMode="Static" TabIndex="4" disabled="true">
                                                <%--<asp:ListItem Text="---Select---" Value=" " />--%>
                                                <asp:ListItem Text="Pending" Value="Pending" />
                                                <asp:ListItem Text="On Hold" Value="On Hold" />
                                                <asp:ListItem Text="Approved" Value="Approved" />
                                                <asp:ListItem Text="Dispatched" Value="Dispatched" />
                                                <asp:ListItem Text="Rejected" Value="Rejected" />
                                            </asp:DropDownList>
                                        </div>
                                       <%-- <% }  %>--%>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m2">
                                            <label class="active" runat="server" clientidmode="Static" id="lblReferenceType" for="txtReferenceNo">Reference #</label>
                                            <asp:TextBox ID="txtReferenceNo" runat="server" ClientIDMode="Static" class="form-control" TabIndex="5" placeholder="" MaxLength="50"/>   
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" runat="server" clientidmode="Static" id="lblBuyerRef" for="txtBuyerRef">Buyer's Reference #</label>
                                            <asp:TextBox ID="txtBuyerRef" runat="server" ClientIDMode="Static" class="form-control" TabIndex="6" placeholder="" MaxLength="50"/>
                                        </div>
                                        <div id="divEmployee" runat="server" clientidmode="static" class="input-field col m2">
                                            <label class="active" for="drpSalesPerson">Sales Executive</label>
                                            <asp:DropDownList ID="drpSalesPerson" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="7" placeholder="" />
                                        </div>

                                        <div class="input-field col m2 ">
                                            <label class="active " for="drpProjects">Projects </label>
                                            <asp:DropDownList ID="drpProjects" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="8" placeholder="" />
                                        </div>
                                       <%-- <% if (!hdnSerialKey.Value.Contains("ECO3-2G21-TECH-3MRT"))  //ECOTECH
                                            { %>--%>
                                            <div id="dvLoadItems" runat="server" class="row" visible="false">
                                                <div class="input-field col m3">
                                                    <label class="active" for="drpReferenceNo">Select Indent #</label>
                                                    <asp:DropDownList ID="drpReferenceNo" runat="server" ClientIDMode="Static" EnableViewState="true" TabIndex="9" CssClass="select2-size-sm browser-default" multiple="multiple" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <button id="btnLoadItems" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onclick="javascript:getSelectedIndents();" onserverclick="btnLoadItems_Click" tabindex="10"><i class="material-icons left">save</i>Load Products</button>
                                                </div>
                                            </div>
                                       <%-- <% }  %>--%>                                       
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="uPnlDetails" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col s12">
                                            <ul id="Ul1" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color: antiquewhite;">
                                                <li class="tab col p-0"><a id="lnkProducts" class="left active" href="#pnl_Products" tabindex="11">Product Detail</a></li>
                                                <li class="tab col p-0"><a id="lnkTNC" class="left" href="#pnl_TNC" tabindex="47">Terms & Coditions</a></li>
                                                <li class="tab col p-0"><a id="lnkEmail" class="left" href="#pnl_Email" tabindex="51">Email Content</a></li>
                                                <li class="tab col p-0"><a id="lnkPaySch" class="left" href="#pnl_PaySch" tabindex="56">Payment Schedule</a></li>
                                                <li class="tab col p-0"><a id="lnkFollowUp" class="left" href="#pnl_FollowUp" tabindex="63">Follow Up</a></li>
                                                <li class="tab col p-0"><a id="lnkDelivery" class="left" href="#pnl_Delivery" tabindex="67">Supplier Details</a></li>
                                                <li class="tab col p-0"><a id="lnkAttachment" class="left" href="#pnl_Attachment" tabindex="83">Attachment</a></li>
                                                <li class="tab col p-0"><a id="lnkCheckList" class="left" href="#pnl_CheckList" tabindex="83">CheckList</a></li>
                                            </ul>
                                        </div>
                                        <div id="pnl_Products" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                            <asp:Panel runat="server" ID="pnlDetail">
                                                <div class="row">
                                                    <table id="tblQuotationDetail" class="table responsive-table striped width-100">
                                                        <asp:Repeater ID="rptOrderDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptOrderDetail_ItemCommand" OnItemDataBound="rptOrderDetail_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr>
                                                                        <th class="left-align" style="width: 25%;">Product Name <span class="materialize-red-text font-weight-800">*</span> 
                                                                            <a href="javascript:openProductInfo('add');">
                                                                                <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product"/>
                                                                            </a>
                                                                        </th>
                                                                        <th class="center-align width-6" >Quantity <span class="materialize-red-text font-weight-800">*</span></th>
                                                                        <th class="center-align width-5" >Unit</th>
                                                                        <th class="center-align width-7" >Unit Rate<span class="materialize-red-text font-weight-800">*</span></th>
                                                                        <th class="center-align width-4" >Disc. %</th>
                                                                        <th class="center-align hide width-5" >Disc.Amt</th>
                                                                        <th class="center-align width-7">Net Rate</th>
                                                                        <th class="center-align width-7 hide">Total</th>
                                                                        <th class="center-align width-5">Header Disc</th>
                                                                        <th class="center-align width-7">Amount</th>
                                                                        <th class="center-align width-4">Tax %</th>
                                                                        <th class="center-align width-7">Tax Amount</th>
                                                                        <th class="center-align width-7">Net Amount</th>
                                                                        <th class="center-align width-7">Indent #</th>
                                                                        <th class="left-align width-7">Delivery Date</th>
                                                                        <th class="center-align width-5">Specs</th>
                                                                        <th class="center-align width-5">Delete</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="left-align" style="width: 25%;">
                                                                        <%# Eval("ProductNameLong") %>
                                                                        <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                                        <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                                        <asp:HiddenField ID="edTaxType" runat="server" ClientIDMode="Static" Value='<%# Eval("TaxType") %>' />
                                                                    </td>
                                                                    <td class="center-align width-6">
                                                                        <asp:TextBox ID="edQuantity" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' AutoPostBack="true" Width="60" OnTextChanged="editItem_TextChanged" Style="text-align: right;" TabIndex="13" />
                                                                    </td>
                                                                    <%--<td class="text-center hide"><%# Eval("Unit") %>></td>--%>
                                                                    <td class="center-align width-5">
                                                                        <asp:TextBox ID="edUnit" runat="server" ClientIDMode="Static" Text='<%# Eval("Unit") %>' AutoPostBack="true" Width="50" OnTextChanged="editItem_TextChanged" Style="text-align: right;" TabIndex="11" />
                                                                    </td>
                                                                    <td class="center-align width-7">
                                                                        <asp:TextBox ID="edUnitRate" runat="server" ClientIDMode="Static" Text='<%# Eval("UnitRate") %>' AutoPostBack="true" Width="70" OnTextChanged="editItem_TextChanged" Style="text-align: right;" TabIndex="12" />
                                                                    </td>
                                                                    <td class="center-align width-4">
                                                                        <asp:TextBox ID="edDiscountPercent" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountPercent") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 50px; text-align: right;" TabIndex="13" />
                                                                    </td>
                                                                    <td class="center-align hide width-7" >
                                                                        <asp:TextBox ID="edDiscountAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountAmt") %>' OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" Enabled="false" TabIndex="14" />
                                                                    </td>
                                                                    <td class="center-align width-7">
                                                                        <asp:TextBox ID="edNetRate" runat="server" ClientIDMode="Static" Text='<%# Eval("NetRate") %>' Style="width: 80px; text-align: right;" Enabled="false" TabIndex="15" />
                                                                    </td>
                                                                    <td class="center-align width-7 hide">
                                                                        <%# String.Format("{0:0.00}", Convert.ToDecimal(Eval("Quantity")) * Convert.ToDecimal(Eval("NetRate"))) %>
                                                                    </td>
                                                                    <td class="center-align width-5">
                                                                        <asp:TextBox ID="edHeaderDiscAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("HeaderDiscAmt") %>' Width="70" Style="text-align: right;" Enabled="false" TabIndex="16" CssClass="form-control" />
                                                                    </td>
                                                                    <td class="center-align width-7">
                                                                        <asp:TextBox ID="edAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' Style="width: 90px; text-align: right;" Enabled="false" TabIndex="17" />
                                                                    </td>
                                                                    <td class="center-align width-4" >
                                                                        <asp:HiddenField ID="edhdnCGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("CGSTPer") %>' />
                                                                        <asp:HiddenField ID="edhdnSGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("SGSTPer") %>' />
                                                                        <asp:HiddenField ID="edhdnIGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("IGSTPer") %>' />
                                                                        <asp:TextBox ID="edTaxRate" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 50px; text-align: right;" TabIndex="18" />
                                                                    </td>
                                                                    <td class="center-align width-7" >
                                                                        <asp:HiddenField ID="edhdnCGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("CGSTAmt") %>' />
                                                                        <asp:HiddenField ID="edhdnSGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("SGSTAmt") %>' />
                                                                        <asp:HiddenField ID="edhdnIGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("IGSTAmt") %>' />
                                                                        <asp:TextBox ID="edTaxAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxAmount") %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="19" />
                                                                    </td>
                                                                    <td class="center-align width-7" >
                                                                        <asp:TextBox ID="edNetAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("NetAmount") %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="20" />
                                                                    </td>
                                                                    <td class="center-align width-7">
                                                                        <asp:TextBox ID="edForOrderNo" runat="server" ClientIDMode="Static" Text='<%# Eval("IndentNo") %>' AutoPostBack="true" Style="width: 100px; text-align: center; font-size: 12px;" Enabled="false" TabIndex="15" />
                                                                    </td>
                                                                    <td class="center-align width-7" >
                                                                        <asp:TextBox ID="edDelDate" runat="server" ClientIDMode="Static" Text='<%# Eval("DeliveryDate","{0:yyyy-MM-dd}") %>' AutoPostBack="true" Width="130" Style="text-align: right;" TabIndex="21" TextMode="Date" OnTextChanged="editItem_TextChanged" />
                                                                    </td>
                                                                    <td class="center-align width-5">
                                                                        <a id="lnkSpecs" href="javascript:openSpecification('<%# Eval("ProductID") %>');" tabindex="19"><small>Specs</small></a>
                                                                    </td>
                                                                    <td class="center-align width-5" >
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="22" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="background-color: #dde8f3; border-bottom: 5px solid navy;">
                                                                    <td class="left-align" style="width: 25%;">
                                                                        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnTaxType" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" Width="100%" TabIndex="13" />
                                                                    </td>
                                                                    <td class="center-align width-6" >
                                                                        <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" Width="60" TabIndex="14" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged" MaxLength="12" />
                                                                    </td>
                                                                    <td class="center-align width-5">
                                                                        <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="50" TabIndex="15" Enabled="false" OnTextChanged="txtUnit_TextChanged" MaxLength="12" />
                                                                    </td>
                                                                    <td class="center-align width-7" >
                                                                        <asp:TextBox ID="txtUnitRate" runat="server" ClientIDMode="Static" Width="70" TabIndex="16" AutoPostBack="true" OnTextChanged="txtUnitRate_TextChanged" MaxLength="12"/>
                                                                    </td>
                                                                    <td class="center-align width-4">
                                                                        <asp:TextBox ID="txtDiscountPercent" runat="server" ClientIDMode="Static" Width="50" TabIndex="17" AutoPostBack="true" OnTextChanged="txtDiscountPercent_TextChanged" MaxLength="12"/>
                                                                    </td>
                                                                    <td class="center-align hide width-7">
                                                                        <asp:TextBox ID="txtDiscountAmt" runat="server" ClientIDMode="Static" Width="80" TabIndex="18" Enabled="false" MaxLength="12" />
                                                                    </td>
                                                                    <td class="center-align width-7">
                                                                        <asp:TextBox ID="txtNetRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="19" Enabled="false" MaxLength="12"/>
                                                                    </td>
                                                                    <td class="center-align text-right width-7" >
                                                                        <asp:TextBox ID="txtHeaderDiscAmt" runat="server" ClientIDMode="Static" Width="70" TabIndex="20" Enabled="false" CssClass="form-control" MaxLength="12"/>
                                                                    </td>
                                                                    <td class="center-align right-align width-7" >
                                                                        <asp:TextBox ID="txtAmount" runat="server" ClientIDMode="Static" Width="90" TabIndex="21" Enabled="false" MaxLength="12"/>
                                                                    </td>
                                                                    <td class="center-align width-4" >
                                                                        <asp:HiddenField ID="hdnCGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnSGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnIGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtTaxRate" runat="server" ClientIDMode="Static" Width="50" TabIndex="22" AutoPostBack="true" OnTextChanged="txtTaxRate_TextChanged" MaxLength="12"/>
                                                                    </td>
                                                                    <td class="center-align width-7" >
                                                                        <asp:HiddenField ID="hdnCGSTAmt" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnSGSTAmt" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnIGSTAmt" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtTaxAmount" runat="server" ClientIDMode="Static" Width="90" TabIndex="23" Enabled="false" MaxLength="12" />
                                                                    </td>
                                                                    <td class="center-align width-7" >
                                                                        <asp:TextBox ID="txtNetAmount" runat="server" ClientIDMode="Static" Width="90" TabIndex="24" Enabled="false" MaxLength="12"/>
                                                                    </td>
                                                                    <td class="center-align width-7" style="vertical-align: top;">
                                                                        <asp:DropDownList ID="drpForOrderNo" runat="server" ClientIDMode="Static" TabIndex="25" data-placeholder="Select ..." CssClass="select2-theme browser-default" />
                                                                    </td>
                                                                    <td class="center-align width-7">
                                                                        <asp:TextBox ID="txtDelDate" runat="server" ClientIDMode="Static" Width="130" TabIndex="26" Enabled="true" TextMode="Date" />
                                                                    </td>
                                                                    <td class="center-align width-5" colspan="2" >
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="110" Height="30" TabIndex="27" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="hide" style="background-color: navy; min-height: 50px;">
                                                                    <td class="text-right" colspan="5" style="color: White; font-size: 16px;">Grand Total: </td>
                                                                    <td class="text-right" style="display: none;">
                                                                        <asp:Label ID="lblTotalDiscAmt" runat="server" ClientIDMode="Static" Width="100" TabIndex="38" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td class="text-right" style="color: White; font-size: 16px;"></td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotalAmt" runat="server" ClientIDMode="Static" Width="100" TabIndex="39" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td class="text-right" style="color: White; font-size: 16px;"></td>

                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotalTaxAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="40" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotalNetAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="41" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td colspan="2"></td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                            <div class="row mt-1">
                                                <div class="col m3">
                                                    <asp:HiddenField ID="hdnOthChrgGST1" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgBasic1" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgGST2" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgBasic2" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgGST3" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgBasic3" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgGST4" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgBasic4" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgGST5" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnOthChrgBasic5" runat="server" ClientIDMode="Static" />
                                                    <table id="tblCharge" class="table responsive-table striped width-100" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tbody>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg1" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="28" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg1_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt1" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="29" AutoPostBack="true" OnTextChanged="txtOthChrgAmt1_TextChanged" Style="text-align: right;" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg2" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="30" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg2_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt2" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="31" AutoPostBack="true" OnTextChanged="txtOthChrgAmt2_TextChanged" Style="text-align: right;" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg3" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="32" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg3_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt3" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="33" AutoPostBack="true" OnTextChanged="txtOthChrgAmt3_TextChanged" Style="text-align: right;" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg4" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="34" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg4_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt4" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="35" AutoPostBack="true" OnTextChanged="txtOthChrgAmt4_TextChanged" Style="text-align: right;" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg5" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="36" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg5_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt5" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="37" AutoPostBack="true" OnTextChanged="txtOthChrgAmt5_TextChanged" Style="text-align: right;" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="col m9">
                                                    <table id=" " class="table responsive-table striped" border="0">
                                                        <tbody>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Discount Amount</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtHeadDiscount" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="38" AutoPostBack="true" OnTextChanged="txtHeadDiscount_TextChanged" MaxLength="12" />
                                                                </td>
                                                            </tr>

                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Basic Amount</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotBasicAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="39" ReadOnly="true" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Other Charge <small style="color: navy; font-weight: 600;">(With Tax)</small></td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotOthChrgBeforeGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="40" ReadOnly="true" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <asp:HiddenField ID="hdnTotCGSTAmt" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnTotSGSTAmt" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnTotIGSTAmt" runat="server" ClientIDMode="Static" />
                                                                <td class="width-80 right-align">Total GST</td>
                                                                <td class="width-20">
                                                                    <asp:HiddenField ID="hdnTotItemGST" runat="server" ClientIDMode="Static" />
                                                                    <asp:TextBox ID="txtTotGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="41" ReadOnly="true" MaxLength="12"/>
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Other Charges <small style="color: Navy; font-weight: 600;">(Excluding Tax)</small></td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotOthChrgAfterGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="42" ReadOnly="true" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Round Off</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtRoff" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="43" ReadOnly="true" MaxLength="12" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Net Amount</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotNetAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="44" ReadOnly="true" Style="font-weight: bold;" MaxLength="12"/>
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-65 right-align">Adv.%</td>
                                                                <td class="width-35">
                                                                    <table class="table responsive-table striped">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAdvPer" class="form-control" runat="server" ClientIDMode="Static" TabIndex="45" Style="font-weight: bold; width: 50px;" ToolTip="Advance Percentage" placeholder="%" OnTextChanged="txtAdvPer_TextChanged" /></td>
                                                                                <td><span>Amt </span></td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAdvAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="46" Style="font-weight: bold; width: 80px;" ToolTip="Advance Amount" /></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>

                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="pnl_TNC" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtExchangeRate">Select Terms & Conditions</label>
                                                    <asp:DropDownList ID="drpTNC" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="48" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="drpTNC_SelectedIndexChanged" />
                                                </div>
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtTermsCondition">Terms & Condition</label>
                                                    <asp:TextBox ID="txtTermsCondition" runat="server" class="form-control" ClientIDMode="Static" TabIndex="49" TextMode="MultiLine" Rows="63" Height="200px" placeholder="" MaxLength="500" />
                                                </div>
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtDeliveryNote">Delivery Note</label>
                                                    <asp:TextBox ID="txtDeliveryNote" runat="server" class="form-control" ClientIDMode="Static" TabIndex="50" placeholder="500" MaxLength="500"  TextMode="MultiLine"/>
                                                </div>

                                            </div>
                                        </div>

                                        <div id="pnl_Email" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                            <div class="row">
                                                <div class="input-field col m2">
                                                    <label class="active" for="drpQuotationCategory">Select Subject</label>
                                                    <asp:DropDownList ID="drpQuotationCategory" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="52" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="drpQuotationCategory_SelectedIndexChanged" />
                                                </div>
                                                <div class="input-field col m10">
                                                    <%--Section : Add New Entry --%>
                                                    <a class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-Left tooltipped ml-5 mr-2" data-position="center" data-tooltip="Add New Entry" href="javascript:gridAction('add','<%# drpQuotationCategory.SelectedValue %>');" style="padding: 5px;" tabindex="53"><i class="material-icons">add</i></a>
                                                </div>
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtQuotationSubject">Subject</label>
                                                    <asp:TextBox ID="txtQuotationSubject" runat="server" placeholder="" class="form-control" ClientIDMode="Static" MaxLength="1500" TabIndex="54" />
                                                </div>
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtQuotationHeader">Email Introduction</label>
                                                    <asp:TextBox ID="txtQuotationHeader" MaxLength="500" runat="server" class="form-control" ClientIDMode="Static" TabIndex="55" TextMode="MultiLine" Rows="13" placeholder="" />
                                                </div>
                                            </div>
                                        </div>

                                        <div id="pnl_PaySch" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                            <table id="tblPaySchedule" class="table responsive-table striped">
                                                <asp:Repeater ID="rptPaySchedule" runat="server" ClientIDMode="Static" OnItemCommand="rptPaySchedule_ItemCommand">
                                                    <HeaderTemplate>
                                                        <thead>
                                                            <tr>
                                                                <th class="right-align width-30">Amount</th>
                                                                <th class="center-align width-30">Due Date</th>
                                                                <th class="center-align width-40">Action</th>
                                                            </tr>
                                                        </thead>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="right-align width-30">
                                                                <asp:TextBox ID="edPayAmount" runat="server" ClientIDMode="Static" CssClass="form-control text-right" Text='<%# Eval("PayAmount") %>' AutoPostBack="true" TabIndex="57" MaxLength="12" />
                                                            </td>
                                                            <td class="center-align width-30">
                                                                <asp:TextBox ID="edDueDate" runat="server" ClientIDMode="Static" CssClass="datepicker" Text='<%# Eval("DueDate", "{0:yyyy-MM-dd}") %>' AutoPostBack="true" Width="180px" TabIndex="58" />
                                                            </td>
                                                            <td class="center-align width-40">
                                                                <asp:ImageButton ID="ImgbtnEdit11" runat="server" ClientIDMode="Static" ImageUrl="~/images/savedata.png" ToolTip="Update" CommandName="updpayment" CommandArgument='<%# Eval("pkID") %>' Width="40" Height="20" TabIndex="68" Visible="false" />
                                                                <asp:ImageButton ID="ImgbtnDelete11" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="delpayment" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="59" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr>
                                                            <td class="right-align width-30">
                                                                <asp:TextBox ID="ftPayAmount" runat="server" ClientIDMode="Static" CssClass="form-control text-right" Text='<%# Eval("PayAmount") %>' AutoPostBack="true" TabIndex="60" MaxLength="12" />
                                                            </td>
                                                            <td class="center-align width-30">
                                                                <asp:TextBox ID="ftDueDate" runat="server" ClientIDMode="Static" Class="form-control" Text='<%# Eval("DueDate", "{0:yyyy-MM-dd}") %>' AutoPostBack="true" Width="180px" TabIndex="61" TextMode="Date" />
                                                            </td>
                                                            <td class="center-align width-40">
                                                                <asp:ImageButton ID="ImgbtnAddSch" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Schedule" CommandName="addpayment" CommandArgument='<%# Eval("pkID") %>' Width="30" Height="30" TabIndex="62" />
                                                            </td>
                                                        </tr>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>

                                        <div id="pnl_FollowUp" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <div id="divFollowUp" runat="server" clientidmode="Static">
                                                        <div class="row">
                                                            <div class="input-field col m6">
                                                                <label class="active" for="txtNextFollowupDate">Next Followup</label>
                                                                <asp:TextBox ID="txtNextFollowupDate" MaxLength="500" runat="server" class="form-control" ClientIDMode="Static" TabIndex="64" placeholder="" TextMode="Date"/>
                                                            </div>
                                                            <div class="input-field col m6">
                                                                <label class="active" for="drpFollowupType">Followup Type</label>
                                                                <asp:DropDownList ID="drpFollowupType" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="65"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m12">
                                                                <label class="active" for="txtMeetingNotes">Meeting Notes</label>
                                                                <asp:TextBox ID="txtMeetingNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="66" MaxLength="500" TextMode="MultiLine" Rows="4" placeholder="" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="pnl_Delivery" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                            <div class="row">
                                                <div class="input-field col m3">
                                                    <label class="active" for="drpTankerNo">Tanker Number</label>
                                                    <asp:DropDownList ID="drpTankerNo" runat="server" ClientIDMode="Static" class="select2-theme browser-default" OnSelectedIndexChanged="drpTankerNo_SelectedIndexChanged" EnableViewState="true" TabIndex="68" AutoPostBack="true" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtGrossWeight">Gross Weight (R.L.W)</label>
                                                    <asp:TextBox ID="txtGrossWeight" runat="server" class="form-control" ClientIDMode="Static" TabIndex="69" placeholder="" MaxLength="12" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtTareWeight">Tare Weight (U.L.W)</label>
                                                    <asp:TextBox ID="txtTareWeight" runat="server" class="form-control" ClientIDMode="Static" TabIndex="70" placeholder="" MaxLength="12" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtNetWeight">Net Weight (Capacity)</label>
                                                    <asp:TextBox ID="txtNetWeight" runat="server" class="form-control" ClientIDMode="Static" TabIndex="71" placeholder="" MaxLength="12" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtLicenseNo">Explosive License Number</label>
                                                    <asp:TextBox ID="txtLicenseNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="72" placeholder="" MaxLength="100" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m3">
                                                    <label class="active" for="drpDriverName">Driver Name</label>
                                                    <asp:DropDownList ID="drpDriverName" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="73" AutoPostBack="true" OnSelectedIndexChanged="drpDriverName_SelectedIndexChanged" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtDriverNumber">Driver Number</label>
                                                    <asp:TextBox ID="txtDriverNumber" runat="server" class="form-control" ClientIDMode="Static" TabIndex="74" placeholder="" MaxLength="50" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtDrivingLicenseNo">Driving License Number</label>
                                                    <asp:TextBox ID="txtDrivingLicenseNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="75" placeholder="" MaxLength="100" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtDriverDetails">Driver Details</label>
                                                    <asp:TextBox ID="txtDriverDetails" runat="server" class="form-control" ClientIDMode="Static" TabIndex="76" placeholder="Max 100 Char" MaxLength="100" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtConductorName">Conductor Name</label>
                                                    <asp:TextBox ID="txtConductorName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="77" placeholder="" MaxLength="50" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtModeOfPayment">Mode Of Payment</label>
                                                    <asp:TextBox ID="txtModeOfPayment" runat="server" class="form-control" ClientIDMode="Static" TabIndex="78" placeholder="" MaxLength="500" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtTransporterName">Transporter Name</label>
                                                    <asp:TextBox ID="txtTransporterName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="79" placeholder="" MaxLength="50" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtTripDistance">Distance (One Way)</label>
                                                    <asp:TextBox ID="txtTripDistance" runat="server" class="form-control" ClientIDMode="Static" TabIndex="80" placeholder="" MaxLength="50" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtConsigneeName">Consignee Name</label>
                                                    <asp:TextBox ID="txtConsigneeName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="81" placeholder="" MaxLength="50" />
                                                </div>
                                                <div class="input-field col m9">
                                                    <label class="active" for="txtConsigneeAddress">Consignee Address</label>
                                                    <asp:TextBox ID="txtConsigneeAddress" runat="server" class="form-control" ClientIDMode="Static" TabIndex="82" placeholder="Max 250 Char" MaxLength="250" />
                                                </div>
                                            </div>
                                        </div>
                                        <div id="pnl_Attachment" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;" tabindex="84">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                    <uc1:myModuleAttachment runat="server" id="myModuleAttachment" />
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div id="pnl_CheckList" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                            <table id="tblChecklist" class="table responsive-table striped">
                                                <asp:Repeater ID="rptCkeckList" runat="server" ClientIDMode="Static" OnItemCommand="rptCkeckList_ItemCommand">
                                                    <HeaderTemplate>
                                                        <thead>
                                                            <tr>
                                                                <th class="center-align width-50">Check List Detail</th>
                                                                <th class="center-align width-25">Comment</th>
                                                                <th class="center-align width-15">Due Date</th>
                                                                <th class="center-align width-10">Action</th>
                                                            </tr>
                                                        </thead>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <asp:HiddenField ID ="hdnCpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>'/>
                                                            <td class="right-align width-30">
                                                                <asp:TextBox ID="edDetails" runat="server" ClientIDMode="Static" CssClass="form-control text-right" Text='<%# Eval("CheckPointDetail") %>' OnTextChanged="editItemCheckListChanged" AutoPostBack="true" TabIndex="57" MaxLength="12" />
                                                            </td>
                                                            <td class="right-align width-30">
                                                                <asp:TextBox ID="edComment" runat="server" ClientIDMode="Static" CssClass="form-control text-right" Text='<%# Eval("Comment") %>' OnTextChanged="editItemCheckListChanged" AutoPostBack="true" TabIndex="57" MaxLength="12" />
                                                            </td>
                                                            <td class="center-align width-30">
                                                                <asp:TextBox ID="edDate" runat="server" ClientIDMode="Static" CssClass="datepicker" Text='<%# Eval("Date", "{0:yyyy-MM-dd}") %>' OnTextChanged="editItemCheckListChanged" AutoPostBack="true" Width="180px" TabIndex="58" />
                                                            </td>
                                                            <td class="center-align width-40">
                                                                <asp:ImageButton ID="ImgbtnDelete11" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="delcheck" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="59" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr>
                                                            <td class="right-align width-30">
                                                                <asp:TextBox ID="txtDetails" runat="server" ClientIDMode="Static" CssClass="form-control text-right" TabIndex="60" MaxLength="12" />
                                                            </td>
                                                            <td class="center-align width-30">
                                                                <asp:TextBox ID="txtComment" runat="server" ClientIDMode="Static" Class="form-control" TabIndex="61"  />
                                                            </td>
                                                            <td class="center-align width-30">
                                                                <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" CssClass="form-control text-right" TabIndex="60" Width="150px" MaxLength="12" TextMode="Date" />
                                                            </td>
                                                            <td class="center-align width-10">
                                                                <asp:ImageButton ID="ImgbtnAddSch" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Schedule" CommandName="addCheckList" CommandArgument='<%# Eval("pkID") %>' Width="30" Height="30" TabIndex="62" />
                                                            </td>
                                                        </tr>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                            <div class="col m12">
                                <button type="button" id="btnReset" runat="server" clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" tabindex="87"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button type="button" id="btnSaveEmail" runat="server" clientidmode="Static" class="btn cyan  right mr-1" onserverclick="btnSaveEmail_Click" tabindex="85"><i class="material-icons left">email</i>Save - Send Email</button>
                                <button type="button" id="btnSave" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="86"><i class="material-icons left">save</i>Save</button>
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
                $("input").keypress(function (event) {
                    if (event.which == 13) {
                        event.preventDefault();
                        //$("form").submit();
                    }
                });

                function bindDDLTo(selector) {
                    if ($(selector).val().length >= 1) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterCustomerByModule",
                            data: '{pCustName:\'' + $(selector).val() + '\', pSearchModule:\'CustomerSearchPurchase\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                console.log(data);
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
                                        $("#drpQuotation").focus();
                                    }
                                });
                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                    }
                    else {
                        if ($(selector).val().length == 0) {
                            $("#hdnCustomerID").val('');
                        }
                    }
                }

                function bindDDLProductTo(selector) {
                    if ($(selector).val().length >= 1) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterProduct",
                            data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypePurchaseOrder\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
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
                                        return '<div class="autocomplete-suggestion" data-langname="' + item.pkID + '" data-lang="' + item.ProductNameLong + '" data-val="' + search + '">' + item.ProductNameLong + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        //console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnProductID").val(item.data('langname'));
                                        $("#txtReferenceName").focus();
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
                            $("#hdnProductID").val('');
                        }
                    }
                }
                function clearProductField() {
                    $("#hdnProductID").val('');
                    $("#txtProductName").val('');
                    $("#txtQuantity").val('');
                    $("#txtUnit").val('');
                    $("#txtUnitRate").val('');
                    $("#txtDiscountPercent").val('');
                    $("#txtNetRate").val('');
                    $("#txtAmount").val('');
                    $("#txtTaxRate").val('');
                    $("#txtTaxAmount").val('');
                    $("#txtNetAmount").val('');
                    $("#txtProductName").focus();
                }
                $('#btnReset').focusout(function () {
                    $('#txtOrderNo').focus();
                });

            </script>
        </div>
    </form>
</body>
</html>
