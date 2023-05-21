<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DebitCreditNoteNew.aspx.cs" Inherits="StarsProject.DebitCreditNoteNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
        th, td { display: inline-block !important; }
        #tblQuotationDetail > tbody > tr > td { padding-top:0px !important; padding-bottom:0px !important; }
        #tblCharge > tbody > tr > td { padding-top:0px !important; padding-bottom:0px !important; }
        #tblSummary > tbody > tr > td { padding-top:0px !important; padding-bottom:0px !important; }
        .limited { white-space: nowrap; max-width: 300px; overflow: hidden; text-overflow: ellipsis; }
    </style>

    <script type="text/javascript">
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
        }

        function retainTabPosition() {

            var currTab = $("#hdnCurrTab").val();

            $("#lnkProducts").removeClass("active");
            $("#lnkTNC").removeClass("active");
            $("#lnkTransport").removeClass("active");

            $("#pnl_Products").css("display", "none");
            $("#pnl_TNC").css("display", "none");
            $("#pnl_Transport").css("display", "none");

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

        function showErrorMessage(strMess) {
            jQuery.confirm({ title: 'Data Validation', content: 'Are you sure, You want to delete record !', type: 'red', typeAnimated: true });
        }
        /*---------------------------------------------------------*/
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Customer";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

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
        function openSpecification(pProdID) {
            var t11;
            if ($("#hdnInvBasedOn").val() == "quotation")
            {
                t11 = jQuery("#drpReferenceNo").val();
                var pageUrl = "ProductSpecification.aspx?Module=quotation&DocNo=" + t11 + "&FinishProductID=" + pProdID;
            }
            else if ($("#hdnInvBasedOn").val() == "salesorder")
            {
                t11 = jQuery("#drpReferenceNo").val();
                var pageUrl = "ProductSpecification.aspx?Module=salesorder&DocNo=" + t11 + "&FinishProductID=" + pProdID;
            }
            else
            {
                t11 = jQuery("#txtInvoiceNo").val();
                var pageUrl = "ProductSpecification.aspx?Module=salesbill&DocNo=" + t11 + "&FinishProductID=" + pProdID;
            }
            //t11 = jQuery("#txtInvoiceNo").val();
            //var pageUrl = "ProductSpecification.aspx?Module=salesbill&DocNo=" + t11 + "&FinishProductID=" + pProdID;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <%-- Bootstrap Quotation Modal Popup --%>
                        <div class="clearall"></div>
                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnProductUnitQty" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnBlockCustomer" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnStockOutward" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnLocationStock" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnInvBasedOn" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnCurrTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Products" />
                        <asp:HiddenField ID="hdnSelectedReference" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnDBC" runat="server" ClientIDMode="Static" />

                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row col m12 hide">
                                        <asp:Label CssClass="float-right" ID="lblApprovalStatus" runat="server" ClientIDMode="Static" Style="font-size: 14px; color: white;" />
                                    </div>
                                    <div class="row">
                                        <div class="col m12">
                                            <div class="row">
                                                <%--<div class="input-field col m2">
                                                    <label class="active" for="txtInvoiceNo">Invoice #</label>
                                                    <asp:TextBox ID="txtInvoiceNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" placeholder="" ReadOnly="true" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtInvoiceDate">Invoice Date</label>
                                                    <asp:TextBox ID="txtInvoiceDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" TextMode="Date" />
                                                </div>--%>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtVoucherDate">Voucher Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:TextBox ID="txtVoucherDate" runat="server" class="form-control" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" TabIndex="1" placeholder="" TextMode="Date" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtVoucherNo">Voucher No</label>
                                                    <asp:TextBox ID="txtVoucherNo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="2" ReadOnly="true" placeholder="" />
                                                </div>
                                                <div class="input-field col m4">
                                                    <asp:HiddenField ID="hdnCustStateID" runat="server" ClientIDMode="Static" />
                                                    <label class="active" for="txtCustomerName">Customer<small class="text-muted font-italic">(Minimum 3 chars to activate search)</small> <span class="materialize-red-text font-weight-800">*</span>

                                                    <a href="javascript:openCustomerInfo('view');">
                                                        <img src="images/registration.png" width="25" height="15" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" Tabindex="3" />
                                                    </a>
                                                    <a href="javascript:openCustomerInfo('add');">
                                                        <img src="images/addCustomer.png" width="25" height="15" alt="Add New Customer" title="Add New Customer" />
                                                    </a>

                                                    </label>
                                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" AutoPostBack="true" placeholder="Min. 3 Chars to Search" TabIndex="3" OnTextChanged="txtCustomerName_TextChanged" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="drpBillNo">Bill No<span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:DropDownList ID="drpBillNo" runat="server" ClientIDMode="Static" TabIndex="4" class="select2-theme browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpBillNo_SelectedIndexChanged" Style="height: inherit;" />
                                                </div>
                                                <%--<div class="input-field col m2">
                                                    <label class="active" for="drpBank">Select Bank<span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:DropDownList ID="drpBank" runat="server" ClientIDMode="Static" TabIndex="4" class="select2-theme browser-default" Style="height: inherit;" />
                                                </div>--%>
                                            </div>
                                           
                                            <%--<div class="row" id="divSupplierRef1" runat="server" clientidmode="Static">
                                                <div class="input-field col m2">
                                                    <label class="active" for="rdblOption">Select Option</label>
                                                    <asp:DropDownList ID="rdblOption" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="rdblOption_SelectedIndexChanged" TabIndex="4">
                                                        <asp:ListItem Text="Inquiry" Value="Inquiry" />
                                                        <asp:ListItem Text="Quotation" Value="Quotation" />
                                                        <asp:ListItem Text="SalesOrder" Value="SalesOrder" />
                                                        <asp:ListItem Text="Complaint" Value="Complaint" />
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="input-field col m3">
                                                    <label id="lblReferenceType" class="active" for="drpReferenceNo" runat="server" clientidmode="Static" >Order No</label>
                                                    <asp:DropDownList ID="drpReferenceNo" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="drpReferenceNo_SelectedIndexChanged" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtSupplierRef">Supplier's Ref. #</label>
                                                    <asp:TextBox ID="txtSupplierRef" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6"  placeholder="" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="txtSupplierRef">Supplier's Ref. Date</label>
                                                    <asp:TextBox ID="txtSupplierRefDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6"  placeholder="" TextMode="Date"/>
                                                </div>
                                                <% if (hdnLocationStock.Value == "yes")
                                                     {%>
                                                        <div class="input-field col m2">
                                                            <label class="active" for="drpLocation">Location</label>
                                                            <asp:DropDownList ID="drpLocation" runat="server" ClientIDMode="Static" TabIndex="6" class="select2-theme browser-default"/>
                                                        </div>
                                                    <%}%>
                                            </div>
                                            <div class="row">
                                                 <div class="input-field col m3">
                                                    <label class="active" for="drpTerminationOfDelivery">Termination Of Delivery - State</label>
                                                    <asp:DropDownList ID="drpTerminationOfDelivery" runat="server" ClientIDMode="Static" TabIndex="8" class="select2-theme browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpTerminationOfDelivery_SelectedIndexChanged" />
                                                </div>
                                                 <div class="input-field col m3">
                                                    <label class="active" for="drpTerminationOfDeliveryCity">Termination Of Delivery - City</label>
                                                    <asp:DropDownList ID="drpTerminationOfDeliveryCity" runat="server" ClientIDMode="Static" TabIndex="8" class="select2-theme browser-default" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtOtherRef">Other's Reference</label>
                                                    <asp:TextBox ID="txtOtherRef" runat="server" class="form-control" ClientIDMode="Static" TabIndex="7"  placeholder="" />
                                                </div>
                                                 <div class="input-field col m3">
                                                    <label class="active" for="txtDisDocNo">Dispatch Document No.</label>
                                                    <asp:TextBox ID="txtDisDocNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="7"  placeholder="" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <%--<div class="row">
                                        <div class="col s12">
                                            <ul id="Ul1" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color:antiquewhite;">
                                                <li class="tab col m3 p-0"><a id="lnkProducts" class="left active" href="#pnl_Products">Product Detail</a></li>
                                                <li class="tab col m3 p-0"><a id="lnkTNC" class="left" href="#pnl_TNC">Terms & Coditions</a></li>
                                                <li class="tab col m3 p-0"><a id="lnkTransport" class="left" href="#pnl_Transport">Transport Detail</a></li>
                                            </ul>
                                        </div>--%>
                                        <div id="pnl_Products" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                            <asp:Panel runat="server" ID="pnlDetail">
                                                <div class="row">
                                                    <table id="tblQuotationDetail" class="table responsive-table striped width-100" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <asp:Repeater ID="rptOrderDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptOrderDetail_ItemCommand" OnItemDataBound="rptOrderDetail_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr>
                                                                        <th class="left-align width-20">Product Name <span class="materialize-red-text font-weight-800">*</span></th>
                                                                        <% if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")
                                                                            { %>
                                                                        <th class="right-align width-6">Unit Qty </th>
                                                                        <% } %>
                                                                        <th class="center-align width-6">Quantity <span class="materialize-red-text font-weight-800">*</span></th>
                                                                        <th class="center-align width-5">Unit</th>
                                                                        <th class="center-align width-7">Unit Rate <span class="materialize-red-text font-weight-800">*</span></th>
                                                                        <th class="center-align width-5">Disc. %</th>
                                                                        <th class="center-align width-6">Disc.Amt</th>
                                                                        <th class="center-align width-6">Net Rate</th>
                                                                        <th class="center-align width-7">Header Disc</th>
                                                                        <th class="center-align width-6">Amount</th>
                                                                        <th class="center-align width-5">Tax %</th>
                                                                        <th class="center-align width-7">Tax Amount</th>
                                                                        <th class="center-align hide">Add.Tax %</th>
                                                                        <th class="center-align hide">Add.Tax Amt</th>
                                                                        <th class="center-align width-7">Net Amount</th>
                                                                        <%--<th id="tdOutwardNo" runat="server" class="center-align width-9">S.O #</th>--%>
                                                                        <th class="center-align width-4">Specs</th>
                                                                        <th class="center-align width-4">Delete</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="left-align limited width-20">
                                                                        <%# Eval("ProductNameLong") %>
                                                                        <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                                        <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                                        <% if (hdnDBC.Value == "CRNT")     
                                                                        { %> 
                                                                        <asp:HiddenField ID="edUnitQuantity" runat="server" ClientIDMode="Static" Value='<%# Eval("UnitQuantity") %>' />
                                                                        <% } %>
                                                                        <asp:HiddenField ID="edTaxType" runat="server" ClientIDMode="Static" Value='<%# Eval("TaxType") %>' />
                                                                    </td>
                                                                    <% if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")
                                                                        { %>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox ID="edUnitQty" runat="server" ClientIDMode="Static" Text='<%# Eval("UnitQty") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 70px; text-align: right;" TabIndex="6" />
                                                                    </td>
                                                                    <% } %>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox ID="edQuantity" runat="server" ClientIDMode="Static" Text='<%# Eval("Qty") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 70px; text-align: right;" TabIndex="7" />
                                                                    </td>
                                                                    <td class="center-align width-5"><%# Eval("Unit") %></td>
                                                                    <td class="right-align width-7">
                                                                        <asp:TextBox ID="edUnitRate" runat="server" ClientIDMode="Static" Text='<%# Eval("Rate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" TabIndex="8" />
                                                                    </td>
                                                                    <td class="right-align width-5">
                                                                        <asp:TextBox ID="edDiscountPercent" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountPer") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 50px; text-align: right;" TabIndex="9" />
                                                                    </td>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox ID="edDiscountAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountAmt") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Enabled="false" Style="width: 80px; text-align: right;" TabIndex="10" />
                                                                    </td>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox ID="edNetRate" runat="server" ClientIDMode="Static" Text='<%# Eval("NetRate") %>' Style="width: 80px; text-align: right;" Enabled="false" TabIndex="11" />
                                                                    </td>
                                                                    <td class="right-align width-7">
                                                                        <asp:TextBox ID="edHeaderDiscAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("HeaderDiscAmt") %>' Style="width: 80px; text-align: right;" Enabled="false" TabIndex="12" />
                                                                    </td>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox ID="edAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="13" />
                                                                    </td>
                                                                    <td class="right-align width-5">
                                                                        <asp:HiddenField ID="edhdnCGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("CGSTPer") %>' />
                                                                        <asp:HiddenField ID="edhdnSGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("SGSTPer") %>' />
                                                                        <asp:HiddenField ID="edhdnIGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("IGSTPer") %>' />
                                                                        <asp:TextBox ID="edTaxRate" runat="server" ClientIDMode="Static" Text='<%# (Eval("TaxRate") ) %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 50px; text-align: right;" TabIndex="14" />
                                                                    </td>
                                                                    <td class="right-align width-7">
                                                                        <asp:HiddenField ID="edhdnCGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("CGSTAmt") %>' />
                                                                        <asp:HiddenField ID="edhdnSGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("SGSTAmt") %>' />
                                                                        <asp:HiddenField ID="edhdnIGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("IGSTAmt") %>' />
                                                                        <asp:TextBox ID="edTaxAmount" runat="server" ClientIDMode="Static" Text='<%# (Eval("TaxAmount") ) %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="15" />
                                                                    </td>
                                                                    <td class="right-align hide">
                                                                        <asp:TextBox ID="edAddTaxPer" runat="server" ClientIDMode="Static" Text='<%# Eval("AddTaxPer") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 50px; text-align: right;" TabIndex="16" />
                                                                    </td>
                                                                    <td class="right-align hide">
                                                                        <asp:TextBox ID="edAddTaxAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("AddTaxAmt") %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="17" />
                                                                    </td>
                                                                    <td class="right-align width-7">
                                                                        <asp:TextBox ID="edNetAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("NetAmt") %>' AutoPostBack="true" Style="width: 90px; text-align: right;" Enabled="false" TabIndex="18" />
                                                                    </td>
                                                                    <%--<td class="right-align width-10">
                                                                         <asp:TextBox ID="edForOrderNo" runat="server" ClientIDMode="Static" Text='<%# Eval("ForOrderNo") %>' Style="width:100px; text-align: right;" Enabled="false" TabIndex="19" />
                                                                    </td>--%>
                                                                    <td class="center-align width-4">
                                                                        <a id="lnkSpecs" href="javascript:openSpecification('<%# Eval("ProductID") %>');" tabindex="16"><small>Specs</small></a>
                                                                    </td>
                                                                    <td class="center-align width-3">
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="20" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="background-color: #dde8f3;">
                                                                    <td class="left-align width-20">
                                                                        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnTaxType" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnUnitQuantity" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" Width="100%" TabIndex="21" />
                                                                    </td>
                                                                    <% if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")
                                                                        { %>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox CssClass="OnlyNumeric" ID="txtUnitQty" runat="server" ClientIDMode="Static" Width="70" AutoPostBack="true" OnTextChanged="txtUnitQty_TextChanged" TabIndex="22" />
                                                                    </td>
                                                                    <% } %>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox CssClass="OnlyNumeric" ID="txtQuantity" runat="server" ClientIDMode="Static" Width="70" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged" TabIndex="23" />
                                                                    </td>
                                                                    <td class="center-align width-5">
                                                                        <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="50" TabIndex="24" Enabled="false" />
                                                                    </td>
                                                                    <td class="right-align width-7">
                                                                        <asp:TextBox CssClass="OnlyDecimal" ID="txtUnitRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="25" AutoPostBack="true" OnTextChanged="txtUnitRate_TextChanged" />
                                                                    </td>
                                                                    <td class="right-align width-5">
                                                                        <asp:TextBox ID="txtDiscountPercent" runat="server" ClientIDMode="Static" Width="50" TabIndex="26" AutoPostBack="true" OnTextChanged="txtDiscountPercent_TextChanged" />
                                                                    </td>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox ID="txtDiscountAmt" runat="server" ClientIDMode="Static" Width="80" TabIndex="27" AutoPostBack="true" OnTextChanged="txtDiscountAmt_TextChanged" />
                                                                    </td>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox ID="txtNetRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="28" Enabled="false" />
                                                                    </td>
                                                                    <td class="right-align width-7">
                                                                        <asp:TextBox ID="txtHeaderDiscAmt" runat="server" ClientIDMode="Static" Width="80" TabIndex="29" Enabled="false" />
                                                                    </td>
                                                                    <td class="right-align width-6">
                                                                        <asp:TextBox ID="txtAmount" runat="server" ClientIDMode="Static" Width="90" TabIndex="30" Enabled="false" />
                                                                    </td>
                                                                    <td class="right-align width-5">
                                                                        <asp:HiddenField ID="hdnCGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnSGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnIGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtTaxRate" runat="server" ClientIDMode="Static" Width="50" TabIndex="31" AutoPostBack="true" OnTextChanged="txtTaxRate_TextChanged" />
                                                                    </td>
                                                                    <td class="right-align width-7">
                                                                        <asp:HiddenField ID="hdnCGSTAmt" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnSGSTAmt" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnIGSTAmt" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtTaxAmount" runat="server" ClientIDMode="Static" Width="90" TabIndex="32" Enabled="false" />
                                                                    </td>
                                                                    <td class="right-align hide">
                                                                        <asp:TextBox ID="txtAddTaxPer" runat="server" ClientIDMode="Static" Width="50" TabIndex="33" AutoPostBack="true" OnTextChanged="txtAddTaxPer_TextChanged" />
                                                                    </td>
                                                                    <td class="right-align hide">
                                                                        <asp:TextBox ID="txtAddTaxAmt" runat="server" ClientIDMode="Static" Width="90" TabIndex="34" Enabled="false" />

                                                                    </td>
                                                                    <td class="right-align width-7">
                                                                        <asp:TextBox ID="txtNetAmount" runat="server" ClientIDMode="Static" Width="90" TabIndex="35" Enabled="false" />
                                                                    </td>
                                                                    <%--<td class="right-align width-10">
                                                                        <asp:DropDownList ID="drpForOrderNo" runat="server" ClientIDMode="Static" TabIndex="17" CssClass="select2-theme browser-default" />
                                                                    </td>--%>
                                                                    <td class="center-align width-4">
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="80" Height="30" TabIndex="33" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="hide" style="background-color: navy; min-height: 50px;">
                                                                    <td class="text-right" colspan="5" style="color: White; font-size: 16px;">Grand Total: </td>
                                                                    <td class="text-right" style="display: none;">
                                                                        <asp:Label ID="lblTotalDiscAmt" runat="server" ClientIDMode="Static" Width="100" TabIndex="35" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td class="text-right" style="color: White; font-size: 16px;"></td>
                                                                    <td class="text-right" style="color: White; font-size: 16px;"></td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotalAmt" runat="server" ClientIDMode="Static" Width="100" TabIndex="36" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td class="text-right" style="color: White; font-size: 16px;"></td>

                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotalTaxAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="37" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td class="text-right" style="color: White; font-size: 16px;"></td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblAddTaxAmt" runat="server" ClientIDMode="Static" Width="100" TabIndex="38" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotalNetAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="39" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
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
                                                                    <asp:DropDownList ID="drpOthChrg1" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="34" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg1_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt1" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="35" AutoPostBack="true" OnTextChanged="txtOthChrgAmt1_TextChanged" Style="text-align: right;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg2" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="37" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg2_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt2" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="38" AutoPostBack="true" OnTextChanged="txtOthChrgAmt2_TextChanged" Style="text-align: right;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg3" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="40" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg3_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt3" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="41" AutoPostBack="true" OnTextChanged="txtOthChrgAmt3_TextChanged" Style="text-align: right;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg4" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="43" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg4_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt4" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="44" AutoPostBack="true" OnTextChanged="txtOthChrgAmt4_TextChanged" Style="text-align: right;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="width-70">
                                                                    <asp:DropDownList ID="drpOthChrg5" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="46" AutoPostBack="true" OnSelectedIndexChanged="drpOthChrg5_SelectedIndexChanged" />
                                                                </td>
                                                                <td class="width-30">
                                                                    <asp:TextBox ID="txtOthChrgAmt5" class="form-control inputOther" runat="server" ClientIDMode="Static" TabIndex="47" AutoPostBack="true" OnTextChanged="txtOthChrgAmt5_TextChanged" Style="text-align: right;" />
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
                                                                    <asp:TextBox ID="txtHeadDiscount" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="42" AutoPostBack="true" OnTextChanged="txtHeadDiscount_TextChanged" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Basic Amount</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotBasicAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="45" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align"><label class="active" for="txTotOthChrg">Other Charge <small style="color: navy; font-weight: 600;">(With Tax)</small> </label></td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotOthChrgBeforeGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="48" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <asp:HiddenField ID="hdnTotCGSTAmt" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnTotSGSTAmt" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnTotIGSTAmt" runat="server" ClientIDMode="Static" />
                                                                <td class="width-80 right-align">Total GST</td>
                                                                <td class="width-20">
                                                                    <asp:HiddenField ID="hdnTotItemGST" runat="server" ClientIDMode="Static" />
                                                                    <asp:TextBox ID="txtTotGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="51" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="HiddenField2" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="HiddenField3" runat="server" ClientIDMode="Static" />
                                                                <td class="width-80 right-align">Total Aadditional Amount</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotAddTaxAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="54" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">
                                                                    <label class="active" for="txTotOthChrg">Other Charges <small style="color: Navy; font-weight: 600;">(Excluding Tax)</small></label>
                                                                </td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotOthChrgAfterGST" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="55" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Round Off</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtRoff" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="56" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Net Amount</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotNetAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="57" ReadOnly="true" Style="font-weight: bold;" />
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                     <%--   <div id="pnl_TNC" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtExchangeRate">Select Terms & Conditions</label>
                                                    <asp:DropDownList ID="drpTNC" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="81" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="drpTNC_SelectedIndexChanged" />
                                                </div>
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtTermsCondition">Terms & Condition</label>
                                                    <asp:TextBox ID="txtTermsCondition" runat="server" class="form-control" ClientIDMode="Static" TabIndex="82" TextMode="MultiLine" Rows="20" Height="200px" placeholder="" />
                                                </div>
                                            </div> 
                                        </div>
                                        <div id="pnl_Transport" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                            <div class="container" style="max-width: none; width: 98%; margin: 5px !important;">
                                                <div class="row">
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtInvoiceNo">Mode Of Transport #</label>
                                                        <asp:DropDownList ID="drpModeOfTransport" runat="server" class="select2-theme browser-default" TabIndex="4">
                                                            <asp:ListItem Text="Road" Value="Road" />
                                                            <asp:ListItem Text="Rail" Value="Rail" />
                                                            <asp:ListItem Text="Air" Value="Air" />
                                                            <asp:ListItem Text="Ship" Value="Ship" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtTransporterName">Transporter Name</label>
                                                        <asp:TextBox ID="txtTransporterName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="58" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtVehicleNo">Vehicle No.</label>
                                                        <asp:TextBox ID="txtVehicleNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="59" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtVehicleNo">Delivery Note</label>
                                                        <asp:TextBox ID="txtDelMote" runat="server" class="form-control" ClientIDMode="Static" TabIndex="59" placeholder="" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtLRNo">LR No./DC No.</label>
                                                        <asp:TextBox ID="txtLRNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="60" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtLRDate">LR Date/DC Date</label>
                                                        <asp:TextBox ID="txtLRDate" runat="server" class="form-control" TextMode="Date" ClientIDMode="Static" TabIndex="61" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txteWay">e-Way Bill No.</label>
                                                        <asp:TextBox ID="txteWay" runat="server" class="form-control"  ClientIDMode="Static" TabIndex="61" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtModePay">Mode Of Payment</label>
                                                        <asp:TextBox ID="txtModePay" runat="server" class="form-control"  ClientIDMode="Static" TabIndex="61" placeholder="" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="input-field col m6">
                                                        <label class="active" for="txtTransportRemark">Remark</label>
                                                        <asp:TextBox ID="txtTransportRemark" runat="server" class="form-control" ClientIDMode="Static" TabIndex="62" placeholder="" TextMode="MultiLine" MaxLength="1000" Rows="4" />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtDeliverTo">Deliver To</label>
                                                        <asp:TextBox ID="txtDeliverTo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="63" placeholder=""/>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                            <div class="col m12">
                                <button type="button" id="btnReset" runat="server" clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" tabindex="28"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button type="button" id="btnSaveEmail" runat="server" clientidmode="Static" class="btn cyan  right mr-1" onserverclick="btnSaveEmail_Click" tabindex="27"><i class="material-icons left">email</i>Save - Send Email</button>
                                <button type="button" id="btnSave" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="26"><i class="material-icons left">save</i>Save</button>
                            </div>
                        </div>
                    </div>
                    <%-- PopUp Modal Box to Open Window --%>
                    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
                    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="Panl1" runat="server" CssClass="Popup" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
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
                                    data: '{pCustName:\'' + $(selector).val() + '\', pSearchModule:\'CustomerSearchSales\'}',
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
                                                $("#hdnBlockCustomer").val(item.data('BlockCustomer'));
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
                                    data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeSalesBill\'}',
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
                    </script>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
