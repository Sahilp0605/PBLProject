<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseBill.aspx.cs" Inherits="StarsProject.PurchaseBill" %>
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
        /* ==================================== */
        /*****         Modal Popup        *****/
        /* ==================================== */
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
        /* ==================================== */
        /***** 19. STANDARD TABLES STYLES *****/
        /* ==================================== */
        .table input:not([type]), input[type=text]:not(.browser-default), input[type=password]:not(.browser-default),
        input[type=email]:not(.browser-default), input[type=url]:not(.browser-default), input[type=time]:not(.browser-default),
        input[type=date]:not(.browser-default), input[type=datetime]:not(.browser-default), input[type=datetime-local]:not(.browser-default),
        input[type=tel]:not(.browser-default), input[type=number]:not(.browser-default), input[type=search]:not(.browser-default), textarea.materialize-textarea {
            margin:0px !important;
            height:1.5rem !important;
            border-radius:0px !important;
            font-size:13px !important;
        }
        th, td { display: inline-block !important; }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            
        });

        function pageLoad(sender, args) {
            $('tabs').tabs();
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
        function openTaxSummary() {
            document.getElementById('spnModuleHeader').innerText = "Tax Summary";
            var keyid = $('#txtInvoiceNo').val();
            var pageUrl = "viewTaxSummary.aspx?module=purchasebill&keyid=" + keyid;
            $('#ifrModulePurc').attr('src', pageUrl);
            $find("mpePurc").show();
        }
        /*---------------------------------------------------------*/
        function retainTabPosition() {

            var currTab = $("#hdnCurrTab").val();

            $("#lnkProducts").removeClass("active");
            $("#lnkTNC").removeClass("active");
            $("#lnkTransport").removeClass("active");
            $("#lnkPaySch").removeClass("active");
            $("#lnkAttachment").removeClass("active");

            $("#pnl_Products").css("display", "none");
            $("#pnl_TNC").css("display", "none");
            $("#pnl_Transport").css("display", "none");
            $("#pnl_PaySch").css("display", "none");
            $("#pnl_Attachment").css("display", "none");

            if (currTab == 'pnl_Products') {
                $("#lnkProducts").addClass("active");
                $("#pnl_Products").css("display", "block");
            }
            else if (currTab == 'pnl_TNC') {
                $("#lnkTNC").addClass("active");
                $("#pnl_TNC").css("display", "block");
            }
            else if (currTab == 'pnl_Transport') {
                $("#lnkTransport").addClass("active");
                $("#pnl_Transport").css("display", "block");
            }
            else if (currTab == 'pnl_PaySch') {
                $("#lnkPaySch").addClass("active");
                $("#pnl_PaySch").css("display", "block");
            }
            else if (currTab == 'pnl_Attachment') {
                $("#lnkAttachment").addClass("active");
                $("#pnl_Attachment").css("display", "block");
            }
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

        function openCustomerInfo(cat) {
            document.getElementById('spnModuleHeader').innerText = "Customer Information";
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModulePurc').attr('src', pageUrl);
            $find("mpePurc").show();
        }
        // ------------------------------------------------------------------------
        function calcQuotation() {
            //var q = parseFloat(jQuery("#txtQuantity").val() != '' ? jQuery("#txtQuantity").val() : 0);
            //var ur = parseFloat(jQuery("#txtUnitRate").val() != '' ? jQuery("#txtUnitRate").val() : 0);
            //var dp = parseFloat(jQuery("#txtDiscountPercent").val() != '' ? jQuery("#txtDiscountPercent").val() : 0);

            //var nr = (ur - ((ur * dp) / 100)).toFixed(2);
            //jQuery("#txtNetRate").val(nr);

            //var a = (q * nr).toFixed(2);
            //jQuery("#txtAmount").val(a);

            //var tr = parseFloat(jQuery("#txtTaxRate").val() != '' ? jQuery("#txtTaxRate").val() : 0);
            //ta = ((a * tr) / 100).toFixed(2);
            //jQuery("#txtTaxAmount").val(ta);

            //na = (parseFloat(a) + parseFloat(ta)).toFixed(2);
            //jQuery("#txtNetAmount").val(na);
        }
       
        function loadContent() {
            alert('test')
        }

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

        //$('.select2-size-sm').select2({
        //    dropdownAutoWidth: true, width: '100%', containerCssClass: 'select-sm'
        //});

        function getSelectedOrders() {
            var test = $(".select2-selection__rendered").find('li.select2-selection__choice').text().substring(1).replace("×", ",");
            $("#hdnSelectedReference").val(test);
        }
        function calculateDueDate() {
            var nd = moment($("#txtInvoiceDate").val()).add($("#txtCRDays").val(), "days").format("YYYY-MM-DD");
            $("#txtDueDate").val(nd);
        }

        function setHiddenControls() {
            var userData = JSON.stringify({ "pPageName": 'purchasebill' });
            $.ajax({
                type: "POST",
                url: "/Services/NagrikService.asmx/GetPageHiddenControls",
                data: userData,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    $("#hdnHiddenControl").val(data.d);
                    // ---------------------------------------------------------------
                    $('#frmPurcBill *').filter(':input').each(function () {
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
    <form id="frmPurcBill" runat="server" autocomplete="off">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper">
<%--            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>

                <div class="widgetbox">
                    <%-- Bootstrap Quotation Modal Popup --%>
                    <div class="clearall"></div>
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCurrTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Products" />
                    <asp:HiddenField ID="hdnStockInward" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnLocationStock" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSelectedReference" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />

                    <asp:HiddenField ID="hdnHiddenControl" runat="server" ClientIDMode="Static" Value=""/>
                    <div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row col m12 hide">
                                    <asp:Label CssClass="float-right" ID="lblApprovalStatus" runat="server" ClientIDMode="Static" style="font-size:14px; color:white;" />
                                </div>
                                <div class="row">
                                    <div class="input-field col m2">
                                        <label class="active" for="txtInvoiceNo">Invoice #</label>
                                        <asp:TextBox ID="txtInvoiceNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" readonly="true" placeholder="" MaxLength="20" />
                                    </div>
                                    <div class="input-field col m2">
                                        <label class="active" for="txtInvoiceDate">Invoice Date</label>
                                        <asp:TextBox ID="txtInvoiceDate" runat="server" class="form-control" TextMode="Date" ClientIDMode="Static" TabIndex="2" placeholder="" AutoPostBack="true" OnTextChanged="txtInvoiceDate_TextChanged"/>
                                    </div>
                                    <div class="input-field col m4">
                                        <asp:HiddenField ID="hdnCustStateID" runat="server" ClientIDMode="Static" />
                                        <label class="active" for="txtCustomerName">Supplier <small class="text-muted font-italic">(Minimum 3 chars to activate search)</small> <span class="materialize-red-text font-weight-800">*</span>
                                            <a href="javascript:openCustomerInfo('view');">
                                                <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" />
                                            </a>
                                            <a href="javascript:openCustomerInfo('add');">
                                                <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                            </a>
                                            &nbsp;&nbsp;&nbsp;<small id="isGSTAvailable" runat="server" style="padding-left: 5px;padding-right: 5px;border-radius: 8px;"></small>
                                        </label>
                                        <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" AutoPostBack="true"  TabIndex="5" placeholder="" OnTextChanged ="txtCustomerName_TextChanged" MaxLength="100"/>
                                    </div>
                                    <div class="input-field col m2">
                                        <label class="active" for="drpState">Purchase A/c <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:DropDownList ID="drpLedger" runat="server" ClientIDMode="Static" TabIndex="6" data-placeholder="Select ..." class="select2-theme browser-default"/>
                                    </div>
                                    <div class="input-field col m2">
                                        <label class="active" for="drpBank">Select Bank <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:DropDownList ID="drpBank" runat="server" ClientIDMode="Static" TabIndex="7" data-placeholder="Select ..." class="select2-theme browser-default"/>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m2">
                                        <label class="active" for="drpTerminationOfDelivery">Termination Of Delivery</label>
                                        <asp:DropDownList ID="drpTerminationOfDelivery" runat="server" ClientIDMode="Static" TabIndex="8" class="select2-theme browser-default" data-placeholder="Select ..." AutoPostBack="true" OnSelectedIndexChanged="drpTerminationOfDelivery_SelectedIndexChanged" />
                                    </div>
                                    <div class="input-field col m2">
                                        <label class="active" for="txtBillNo">Supp.Bill #</label>
                                        <asp:TextBox ID="txtBillNo" runat="server" ClientIDMode="Static" TabIndex="9" class="form-control" placeholder="" MaxLength="20"/>                                        
                                    </div>
                                    <div class="col m8">
                                        <% if (hdnLocationStock.Value == "yes")
                                         {%>
                                            <div class="input-field col m2">
                                                <label class="active" for="drpLocation">Location</label>
                                                <asp:DropDownList ID="drpLocation" runat="server" ClientIDMode="Static" TabIndex="10" class="select2-theme browser-default" data-placeholder="Select ..." />
                                            </div>
                                        <%}%>
                                       <div id="dvLoadItems" runat="server" class="row" visible="false">
                                            <div class="input-field col m2">
                                                <label class="active" for="rdblOption">Select Option</label>
                                                <asp:DropDownList ID="rdblOption" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" data-placeholder="Select ..." AutoPostBack="true" OnSelectedIndexChanged="rdblOption_SelectedIndexChanged" TabIndex="11">
                                                    <asp:ListItem Text="G.R.N" Value="GRN" />
                                                    <asp:ListItem Text="Purc.Order" Value="PO" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="input-field col m2">
                                                <label class="active" for="drpReferenceNo">Select P.O #</label>
                                                <asp:DropDownList ID="drpReferenceNo" runat="server" ClientIDMode="Static" TabIndex="12" data-placeholder="Select ..." CssClass="select2-size-sm browser-default" multiple="multiple" />
                                            </div>
                                            <div class="input-field col m2">
                                                <button id="btnLoadItems" type="button" runat="server" clientidmode="Static" TabIndex="13" class="btn cyan right mr-1" onclick="javascript:getSelectedOrders();" onserverclick="btnLoadItems_Click">Load Products</button>
                                            </div>

                                        </div>
                                        <div class="input-field col m2">
                                            <asp:TextBox ID="txtCRDays" class="form-control" runat="server" ClientIDMode="Static" Placeholder="" MaxLength="15" TabIndex="14"  onblur="calculateDueDate();" />
                                            <label class="active" for="txtCRDays">CR Days<span class="materialize-red-text font-weight-800"></span></label>
                                        </div>
                                        <div class="input-field col m2">
                                            <asp:TextBox ID="txtDueDate" runat="server" TabIndex="15" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="15" TextMode="Date" ReadOnly="true"/>
                                            <label class="active" for="txtDueDate">Due Date</label>
                                        </div>
                                    </div>
                                </div>
                                <% if (hdnSerialKey.Value == "SO5H-DH90-E34L-SIOF")
                                    {%>
                                <div class="row">
                                    <div class="input-field col m2">
                                        <label class="active" for="txtConsumer">Purchase For</label>
                                        <asp:TextBox ID="txtConsumer" runat="server" ClientIDMode="Static" TabIndex="12" class="form-control" placeholder=""/>                                        
                                    </div>
                                </div>
                                <% } %>
                                <div class="row">
                                    <div class="col m12">
                                        <ul id="Ul1" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color:antiquewhite; color:navy;">
                                            <li class="tab col m3 p-0"><a id="lnkProducts" class="left active" href="#pnl_Products" tabindex="16">Product Detail</a></li>
                                            <li class="tab col m3 p-0"><a id="lnkTNC" class="left" href="#pnl_TNC" tabindex="51">Terms & Coditions</a></li>
                                            <li class="tab col m3 p-0"><a id="lnkTransport" class="left" href="#pnl_Transport" tabindex="54">Transport Detail</a></li>
                                            <li class="tab col m2 p-0"><a id="lnkAttachment" class="left" href="#pnl_Attachment" tabindex="61">Attachment</a></li>
                                        </ul>
                                    </div>
                                    <div id="pnl_Products" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                        <asp:Panel runat="server" ID="pnlDetail">
                                            <div class="row">
                                                <table id="tblQuotationDetail" class="table responsive-table striped width-100" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <asp:Repeater ID="rptOrderDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptOrderDetail_ItemCommand" OnItemDataBound="rptOrderDetail_ItemDataBound">
                                                        <HeaderTemplate>
                                                            <thead>
                                                                <tr>
                                                                    <th class="left-align" style="width:190px;">Product Name <span class="materialize-red-text font-weight-800">*</span></th>
                                                                    <th class="center-align width-7">Qty <span class="materialize-red-text font-weight-800">*</span></th>
                                                                    <th class="center-align width-5">Unit </th>
                                                                    <th class="center-align width-6">Rate <span class="materialize-red-text font-weight-800">*</span></th>
                                                                    <th class="center-align width-5">Dis.%</th>
                                                                    <th class="center-align width-6">Dis.Amt</th>
                                                                    <th class="center-align width-6">Net Rate</th>
                                                                    <th class="center-align width-7">Header Disc</th>
                                                                    <th class="center-align width-6">Amount</th>
                                                                    <th class="left-align width-5">Tax %</th>
                                                                    <th class="left-align width-6">Tax Amt</th>
                                                                    <th class="left-align width-6">Add.Tax %</th>
                                                                    <th class="left-align width-7">Add.Tax Amt</th>
                                                                    <th class="left-align width-6">Net Amt</th>
                                                                    <th id="tdInwardNo" runat="server" class="center-align width-6">P.O #</th>
                                                                    <th class="center-align width-3">Delete</th>
                                                                </tr>
                                                            </thead>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="left-align" style="width:190px;">
                                                                    <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                                    <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                                    <asp:HiddenField ID="edTaxType" runat="server" ClientIDMode="Static" Value='<%# Eval("TaxType") %>' />
                                                                    <asp:TextBox ID="lblProductName" runat="server" ClientIDMode="Static" class="form-control" Text='<%# Eval("ProductNameLong") %>' Enabled="false" TabIndex="61" />
                                                                </td>
                                                                <td class="right-align width-7">
                                                                    <asp:TextBox ID="edQuantity" runat="server" ClientIDMode="Static" Text='<%# Eval("Qty") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 70px; text-align: right;" TabIndex="62" />
                                                                </td>
                                                                <td class="right-align width-5">
                                                                    <asp:TextBox ID="edUnit" runat="server" ClientIDMode="Static" Text='<%# Eval("Unit") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 60px; text-align: right;" TabIndex="63" />
                                                                </td>
                                                                <td class="right-align width-6">
                                                                    <asp:TextBox ID="edUnitRate" runat="server" ClientIDMode="Static" Text='<%# Eval("Rate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 70px; text-align: right;"  TabIndex="64"/>
                                                                </td>
                                                                <td class="text-right width-5">
                                                                    <asp:TextBox ID="edDiscountPercent" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountPer") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 60px; text-align: right;" TabIndex="65" />
                                                                </td>
                                                                <td class="text-right width-6">
                                                                    <asp:TextBox ID="edDiscountAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountAmt") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Enabled="false" Style="width: 80px; text-align: right;" TabIndex="66"/>
                                                                </td>
                                                                <td class="text-right width-6">
                                                                    <asp:TextBox ID="edNetRate" runat="server" ClientIDMode="Static" Text='<%# Eval("NetRate") %>' Style="width: 80px; text-align: right;" Enabled="false" TabIndex="67"/>
                                                                </td>
                                                                <td class="text-right width-5">
                                                                    <asp:TextBox ID="edHeaderDiscAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("HeaderDiscAmt") %>' Style="width: 60px; text-align: right;" Enabled="false"  TabIndex="68"/>
                                                                </td>
                                                                <td class="right-align width-6">
                                                                    <asp:TextBox ID="edAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="69"/>
                                                                </td>
                                                                <td class="right-align width-5">
                                                                    <asp:HiddenField ID="edhdnCGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("CGSTPer") %>' />
                                                                    <asp:HiddenField ID="edhdnSGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("SGSTPer") %>' />
                                                                    <asp:HiddenField ID="edhdnIGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("IGSTPer") %>' />
                                                                    <asp:TextBox ID="edTaxRate" runat="server" ClientIDMode="Static" Text='<%# (Eval("TaxRate") ) %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 50px; text-align: right;" TabIndex="70"/>
                                                                </td>
                                                                <td class="right-align width-7">
                                                                    <asp:HiddenField ID="edhdnCGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("CGSTAmt") %>' />
                                                                    <asp:HiddenField ID="edhdnSGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("SGSTAmt") %>' />
                                                                    <asp:HiddenField ID="edhdnIGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("IGSTAmt") %>' />
                                                                    <asp:TextBox ID="edTaxAmount" runat="server" ClientIDMode="Static" Text='<%# (Eval("TaxAmount") ) %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="71"/>
                                                                </td>
                                                                <td class="right-align width-5">
                                                                    <asp:TextBox ID="edAddTaxPer" runat="server" ClientIDMode="Static" Text='<%# Eval("AddTaxPer") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 50px; text-align: right;" TabIndex="72" />
                                                                </td>
                                                                <td class="right-align width-7">
                                                                    <asp:TextBox ID="edAddTaxAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("AddTaxAmt") %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="73"/>
                                                                </td>
                                                                <td class="right-align width-6">
                                                                    <asp:TextBox ID="edNetAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("NetAmt") %>' AutoPostBack="true" Style="width: 80px; text-align: right;" Enabled="false" TabIndex="74"/>
                                                                </td>
                                                                <td class="right-align width-7">
                                                                     <asp:TextBox ID="edOrderNo" runat="server" ClientIDMode="Static" Text='<%# Eval("OrderNo") %>' Style="width: 100px; text-align: right;" Enabled="false" TabIndex="75" />
                                                                </td>
                                                                <td class="center-align width-4">
                                                                    <asp:ImageButton CssClass="center-align" ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="76" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <tr style="background-color: #dde8f3;">
                                                                <td class="left" style="width:190px;">
                                                                    <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="hdnTaxType" runat="server" ClientIDMode="Static" />
                                                                    <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="17" MaxLength="100"/>
                                                                </td>
                                                                <td class="right-align width-7">
                                                                    <asp:TextBox CssClass="OnlyNumeric" ID="txtQuantity" runat="server" ClientIDMode="Static" Width="70" TabIndex="18" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged" MaxLength="10"/>
                                                                </td>
                                                                <td class="right-align width-5">
                                                                    <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="70" TabIndex="19" AutoPostBack="true" OnTextChanged="txtUnit_TextChanged" MaxLength="5"/>
                                                                </td>
                                                                <td class="right-align width-6">
                                                                    <asp:TextBox CssClass="OnlyDecimal" ID="txtUnitRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="20" AutoPostBack="true" OnTextChanged="txtUnitRate_TextChanged" MaxLength="12" />
                                                                </td>
                                                                <td class="text-right width-5">
                                                                    <asp:TextBox ID="txtDiscountPercent" runat="server" ClientIDMode="Static" Width="60" TabIndex="21" AutoPostBack="true" OnTextChanged="txtDiscountPercent_TextChanged" MaxLength="12"/>
                                                                </td>
                                                                <td class="text-right width-6">
                                                                    <asp:TextBox ID="txtDiscountAmt" runat="server" ClientIDMode="Static" Width="80" TabIndex="22" AutoPostBack="true" OnTextChanged="txtDiscountAmt_TextChanged" MaxLength="12"/>
                                                                </td>
                                                                <td class="text-right width-6">
                                                                    <asp:TextBox ID="txtNetRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="23" Enabled="false" MaxLength="12"/>
                                                                </td>
                                                                <td class="text-right width-5">
                                                                    <asp:TextBox ID="txtHeaderDiscAmt" runat="server" ClientIDMode="Static" Width="60" TabIndex="24" Enabled="false" MaxLength="12" />
                                                                </td>
                                                                <td class="right-align width-6">
                                                                    <asp:TextBox ID="txtAmount" runat="server" ClientIDMode="Static" Width="90" TabIndex="25" Enabled="false" MaxLength="12"/>
                                                                </td>
                                                                <td class="right-align width-5">
                                                                    <asp:HiddenField ID="hdnCGSTPer" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="hdnSGSTPer" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="hdnIGSTPer" runat="server" ClientIDMode="Static" />
                                                                    <asp:TextBox ID="txtTaxRate" runat="server" ClientIDMode="Static" Width="50" TabIndex="26" AutoPostBack="true" OnTextChanged="txtTaxRate_TextChanged" />
                                                                </td>
                                                                <td class="right-align width-7">
                                                                    <asp:HiddenField ID="hdnCGSTAmt" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="hdnSGSTAmt" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="hdnIGSTAmt" runat="server" ClientIDMode="Static" />
                                                                    <asp:TextBox ID="txtTaxAmount" runat="server" ClientIDMode="Static" Width="90" TabIndex="27" Enabled="false" />
                                                                </td>
                                                                <td class="right-align width-5">
                                                                    <asp:TextBox ID="txtAddTaxPer" runat="server" ClientIDMode="Static" Width="50" TabIndex="28" AutoPostBack="true" OnTextChanged="txtAddTaxPer_TextChanged" MaxLength="12" />
                                                                </td>
                                                                <td class="left-align width-7">
                                                                    <asp:TextBox ID="txtAddTaxAmt" runat="server" ClientIDMode="Static" Width="90" TabIndex="29" Enabled="false" MaxLength="12"/>

                                                                </td>
                                                                <td class="right-align width-6">
                                                                    <asp:TextBox ID="txtNetAmount" runat="server" ClientIDMode="Static" Width="80" TabIndex="30" Enabled="false" MaxLength="12" />
                                                                </td>
                                                                <td class="right-align width-7">
                                                                    <asp:DropDownList ID="drpOrderNo" runat="server" ClientIDMode="Static" TabIndex="31" CssClass="select2-theme browser-default" data-placeholder="Select ..." />
                                                                </td>
                                                                <td class="center-align width-4">
                                                                    <asp:ImageButton ID="imgBtnSave" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="20" Height="20" TabIndex="32" />
                                                                </td>
                                                            </tr>
                                                            <tr style="background-color: navy; min-height: 50px;">
                                                                <td class="left-align width-20 white-text">Grand Total:</td>
                                                                <td class="right-align width-8"></td>
                                                                <td class="right-align width-10"></td>
                                                                <td class="right-align hide"><asp:Label ID="lblTotalDiscAmt" runat="server" ClientIDMode="Static" Width="100" Enabled="false" Text="" /></td></td>
                                                                <td class="right-align hide"></td>
                                                                <td class="right-align hide"></td>
                                                                <td class="right-align hide"></td>
                                                                <td class="right-align width-10 white-text"><asp:Label ID="lblTotalAmt" runat="server" ClientIDMode="Static" Width="100" Enabled="false" Text="" /></td>
                                                                <td class="right-align width-8"></td>
                                                                <td class="right-align width-8 white-text"><asp:Label ID="lblTotalTaxAmount" runat="server" ClientIDMode="Static" Width="100" Enabled="false" Text=""  /></td>
                                                                <td class="right-align width-8"></td>
                                                                <td class="right-align width-10 white-text"><asp:Label ID="lblAddTaxAmt" runat="server" ClientIDMode="Static" Width="100"  Enabled="false" Text=""  /></td>
                                                                <td class="right-align width-10 white-text"><asp:Label ID="lblTotalNetAmount" runat="server" ClientIDMode="Static" Width="100"  Enabled="false" Text="" /></td>
                                                                <td class="center-align width-8"></td>
                                                            </tr>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                        <div class="container" style="max-width:none; width:98%; margin:5px !important;">
                                                <div class="row mt-1">
                                                    <div class="col m4">
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
                                                        <table id="tblCharge" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td class="width-70">
                                                                        <asp:DropDownList ID="drpOthChrg1" runat="server" class="select2-theme browser-default" data-placeholder="Select ..." EnableViewState="true" ClientIDMode="Static" TabIndex="33" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg1_SelectedIndexChanged" />
                                                                    </td>
                                                                    <td class="width-30">
                                                                        <asp:TextBox ID="txtOthChrgAmt1" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="34" AutoPostBack="true" OnTextChanged="txtOthChrgAmt1_TextChanged" Style="text-align: right;" MaxLength="12" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="width-70">
                                                                        <asp:DropDownList ID="drpOthChrg2" runat="server" class="select2-theme browser-default" data-placeholder="Select ..." EnableViewState="true" ClientIDMode="Static" TabIndex="35" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg2_SelectedIndexChanged" />
                                                                    </td>
                                                                    <td class="width-30">
                                                                        <asp:TextBox ID="txtOthChrgAmt2" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="36" AutoPostBack="true" OnTextChanged="txtOthChrgAmt2_TextChanged" Style="text-align: right;" MaxLength="12"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="width-70">
                                                                        <asp:DropDownList ID="drpOthChrg3" runat="server" class="select2-theme browser-default" data-placeholder="Select ..." EnableViewState="true" ClientIDMode="Static" TabIndex="37" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg3_SelectedIndexChanged" />
                                                                    </td>
                                                                    <td class="width-30">
                                                                        <asp:TextBox ID="txtOthChrgAmt3" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="38" AutoPostBack="true" OnTextChanged="txtOthChrgAmt3_TextChanged" Style="text-align: right;" MaxLength="12"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="width-70">
                                                                        <asp:DropDownList ID="drpOthChrg4" runat="server" class="select2-theme browser-default" data-placeholder="Select ..." EnableViewState="true" ClientIDMode="Static" TabIndex="39" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg4_SelectedIndexChanged" />
                                                                    </td>
                                                                    <td class="width-30">
                                                                        <asp:TextBox ID="txtOthChrgAmt4" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="40" AutoPostBack="true" OnTextChanged="txtOthChrgAmt4_TextChanged" Style="text-align: right;" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="width-70">
                                                                        <asp:DropDownList ID="drpOthChrg5" runat="server" class="select2-theme browser-default" data-placeholder="Select ..." EnableViewState="true" ClientIDMode="Static" TabIndex="41" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg5_SelectedIndexChanged" />
                                                                    </td>
                                                                    <td class="width-30">
                                                                        <asp:TextBox ID="txtOthChrgAmt5" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="42" AutoPostBack="true" OnTextChanged="txtOthChrgAmt5_TextChanged" Style="text-align: right;" MaxLength="12" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <div class="col m8">
                                                        <table id="tblSummary" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tbody>
                                                                <tr class="border-none">
                                                                    <td class="width-80 right-align">Discount Amount</td>
                                                                    <td class="width-20">
                                                                        <asp:TextBox ID="txtHeadDiscount" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="43" AutoPostBack="true" OnTextChanged="txtHeadDiscount_TextChanged" MaxLength="12" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="border-none">
                                                                    <td class="width-80 right-align">Basic Amount</td>
                                                                    <td class="width-20">
                                                                        <asp:TextBox ID="txtTotBasicAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="44" ReadOnly="true" MaxLength="12" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="border-none">
                                                                    <td class="width-80 right-align"><label class="active" for="txTotOthChrg">Other Charge <small style="color: navy; font-weight: 600;">(With Tax)</small> </label></td>
                                                                    <td class="width-20">
                                                                        <asp:TextBox ID="txtTotOthChrgBeforeGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="45" ReadOnly="true" MaxLength="12" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="border-none">
                                                                    <asp:HiddenField ID="hdnTotCGSTAmt" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="hdnTotSGSTAmt" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="hdnTotIGSTAmt" runat="server" ClientIDMode="Static" />
                                                                    <td class="width-80 right-align">
                                                                        <a id="lnkTaxSummary" href="javascript:openTaxSummary();">
                                                                        <small class="mr-3" id="spnTaxApplied" runat="server" style="padding-left: 5px;padding-right: 5px;border-radius: 8px;"></small>
                                                                        Total GST
                                                                        </a>
                                                                    </td>
                                                                    <td class="width-20">
                                                                        <asp:HiddenField ID="hdnTotItemGST" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtTotGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="46" ReadOnly="true" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="border-none">
                                                                    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="HiddenField2" runat="server" ClientIDMode="Static" />
                                                                    <asp:HiddenField ID="HiddenField3" runat="server" ClientIDMode="Static" />
                                                                    <td class="width-80 right-align">Total Aadditional Amount</td>
                                                                    <td class="width-20">
                                                                        <asp:TextBox ID="txtTotAddTaxAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="47" ReadOnly="true" MaxLength="12" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="border-none">
                                                                    <td class="width-80 right-align">
                                                                        <label class="active" for="txTotOthChrg">Other Charges <small style="color: Navy; font-weight: 600;">(Excluding Tax)</small></label>
                                                                    </td>
                                                                    <td class="width-20">
                                                                        <asp:TextBox ID="txtTotOthChrgAfterGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="48" ReadOnly="true"  />
                                                                    </td>
                                                                </tr>
                                                                <tr class="border-none">
                                                                    <td class="width-80 right-align">Round Off</td>
                                                                    <td class="width-20">
                                                                        <asp:TextBox ID="txtRoff" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="49" ReadOnly="true" MaxLength="12" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="border-none">
                                                                    <td class="width-80 right-align">Net Amount</td>
                                                                    <td class="width-20">
                                                                        <asp:TextBox ID="txtTotNetAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="50" ReadOnly="true" Style="font-weight: bold;" MaxLength="12" />
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>                                                             
                                        </div>
                                    </div>
                                    <div id="pnl_TNC" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                          <div class="row">
                                                    <div class="input-field col m12">
                                                        <label class="active" for="drpTNC">Select Terms & Conditions</label>
                                                        <asp:DropDownList ID="drpTNC" runat="server" ClientIDMode="Static" data-placeholder="Select ..." class="select2-theme browser-default" EnableViewState="true" TabIndex="52" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="drpTNC_SelectedIndexChanged" />
                                                    </div>
                                                    <div class="input-field col m12">
                                                        <label class="active" for="txtTermsCondition">Terms & Condition</label>
                                                        <asp:TextBox ID="txtTermsCondition" runat="server" class="form-control" ClientIDMode="Static" TabIndex="53" TextMode="MultiLine" Rows="20" Height="200px" placeholder="" />
                                                    </div>
                                                </div> 
                                    </div>
                                    <div id="pnl_Transport" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                        <div class="container" style="max-width: none; width: 98%; margin: 5px !important;">
                                            <div class="row">
                                                <div class="input-field col m3">
                                                    <label class="active" for="drpModeOfTransport">Mode Of Transport #</label>
                                                    <asp:DropDownList ID="drpModeOfTransport" runat="server" data-placeholder="Select ..." class="select2-theme browser-default" TabIndex="55">
                                                        <asp:ListItem Text="Road" Value="Road" />
                                                        <asp:ListItem Text="Rail" Value="Rail" />
                                                        <asp:ListItem Text="Air" Value="Air" />
                                                        <asp:ListItem Text="Ship" Value="Ship" />
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtTransporterName">Transporter Name</label>
                                                    <asp:TextBox ID="txtTransporterName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="56" placeholder="" MaxLength="100" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtVehicleNo">Vehicle No.</label>
                                                    <asp:TextBox ID="txtVehicleNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="57" placeholder="" MaxLength="50" />
                                                </div>
                                                <%-- <div class="input-field col m3">
                                                    <label class="active" for="txtVehicleNo">Delivery Note</label>
                                                    <asp:TextBox ID="txtDelMote" runat="server" class="form-control" ClientIDMode="Static" TabIndex="59" placeholder="" />
                                                </div>--%>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtLRNo">LR No./DC No.</label>
                                                    <asp:TextBox ID="txtLRNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="58" placeholder="" MaxLength="50" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtLRDate">LR Date/DC Date</label>
                                                    <asp:TextBox ID="txtLRDate" runat="server" class="form-control" TextMode="Date" ClientIDMode="Static" TabIndex="59" placeholder="" />
                                                </div>
                                                <%--<div class="input-field col m3">
                                                    <label class="active" for="txteWay">e-Way Bill No.</label>
                                                    <asp:TextBox ID="txteWay" runat="server" class="form-control"  ClientIDMode="Static" TabIndex="61" placeholder="" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtModePay">Mode Of Payment</label>
                                                    <asp:TextBox ID="txtModePay" runat="server" class="form-control"  ClientIDMode="Static" TabIndex="61" placeholder="" />
                                                </div>--%>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtTransportRemark">Remark</label>
                                                    <asp:TextBox ID="txtTransportRemark" runat="server" class="form-control" ClientIDMode="Static" TabIndex="60" placeholder="" TextMode="MultiLine" MaxLength="150 " Rows="4" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="pnl_Attachment" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                                <uc1:myModuleAttachment runat="server" id="myModuleAttachment" />
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>    
                            </div>
                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                        <div class="col m12">
                            <button type="button" id="btnReset" runat="server" clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" tabindex="64"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button type="button" id="btnSaveEmail" runat="server" clientidmode="Static" class="btn cyan  right mr-1" onserverclick="btnSaveEmail_Click" tabindex="62"><i class="material-icons left">email</i>Save - Send Email</button>
                            <button type="button" id="btnSave" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="63"><i class="material-icons left">save</i>Save</button>
                        </div>
                    </div>
                </div>
<%--            </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
        <%-- PopUp Modal Box to Open Window --%>
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpePurc" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
        </cc1:ModalPopupExtender>  
        <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
            <div id="myModal123">
                <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom:10px !important;">
                    <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                        <i class="material-icons prefix">ac_unit</i>
                        <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                        <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text=""/>
                    </h5>
                </div>
            </div>
            <iframe id="ifrModulePurc" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border:1px solid silver; background-color:transparent; width:100%; height:90%; padding:5px;"></iframe>  
            <br/>  
        </asp:Panel> 
        <script type="text/javascript">
            $("input").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    //$("form").submit();
                }
            });


            function bindDDLTo(selector) {
                if ($(selector).val().length >= 3) {
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
                                    return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                },
                                onSelect: function (e, term, item) {
                                    //console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                    $(selector).val(item.html());
                                    $("#hdnCustomerID").val(item.data('langname'));
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
                if ($(selector).val().length >= 3) {
                    jQuery.ajax({
                        type: "POST",
                        url: "InquiryInfo.aspx/FilterProduct",
                        data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypePurchaseBill\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var sample = JSON.parse(data.d);
                            $(selector).autoComplete({
                                minLength: 1,
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
                                    console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
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
                $('#txtInvoiceNo').focus();

            });

            function NextFocus(id)
            {
                $(id).focus();
            }
        </script>
    </form>
</body>
</html>