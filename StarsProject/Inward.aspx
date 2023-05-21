<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inward.aspx.cs" Inherits="StarsProject.Inward" %>
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

<%--    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>--%>
    <%--http://www.codingfusion.com/Download/146267254--%>
    <link type="text/css" href="css/sumoselect.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.sumoselect.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $(<%=lstOrderProduct.ClientID%>).SumoSelect();
        });
    </script>

    <style type="text/css">
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
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.timepicker').timepicker();
            // --------------------------------------------------------
            $(function () {
                $(".tabs a").click(function () {
                    $("#hdnCurrTab").val($(this).attr("href").replace("#", ""));
                    retainTabPosition();
                });
            });
            //// --------------------------------------------------------
            //$('#drpReferenceNo').multiselect();
        });
        /*---------------------------------------------------------*/
        function retainTabPosition() {

            var currTab = $("#hdnCurrTab").val();

            $("#lnkProducts").removeClass("active");
            $("#lnkTransport").removeClass("active");

            $("#pnl_Products").css("display", "none");
            $("#pnl_Transport").css("display", "none");

            if (currTab == 'pnl_Products') {
                $("#lnkProducts").addClass("active");
                $("#pnl_Products").css("display", "block");
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

        function openProductInfo(cat) {
            var keyid = "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Product";
            var pageUrl = "Products.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*---------------------------------------------------------*/
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

        $('.select2').select2({
            dropdownAutoWidth: true, width: '100%', containerCssClass: 'select-sm'
        });

        //function getSelectedOrders() {
        //    var test = $(".select2-selection__rendered").find('li.select2-selection__choice').text();
        //    $("#hdnSelectedReference").val(test.substring(1).replace("×", ","));
        //    $("#hdnSelectedReference").val($("#hdnSelectedReference").val().replace("×", ","));
        //    // --------------------------------------------------
        //}

        function getSelectedOrders() {
            var test = $(".select2-selection__rendered").find('li.select2-selection__choice').text();
            var myArr = test.substring(1).split("×");
            $("#hdnSelectedReference").val(myArr.join());
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper">

<%--            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
                    <div class="widgetbox">
                        <%-- Bootstrap Quotation Modal Popup --%>
                        <div class="clearall"></div>
                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnLocationStock" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnCurrTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Products" />
                        <asp:HiddenField ID="hdnSelectedReference" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m2">
                                            <label class="active" for="txtInwardNo">Inward #</label>
                                            <asp:TextBox ID="txtInwardNo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder=""/>
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtInwardDate">Inward Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtInwardDate" runat="server" class="form-control" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" placeholder="" TabIndex="2" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m4">
                                            <asp:HiddenField ID="hdnCustStateID" runat="server" ClientIDMode="Static" />
                                            <label class="active" for="txtCustomerName">Supplier Name  <small class="text-muted font-italic">(Min. 3 chars to search)</small> <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                            <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block; padding: 6px 0px 0px 0px;" Tabindex="3" />
                                                        </a>
                                                    <a href="javascript:openCustomerInfo('add');">
                                                        <img src="images/addCustomer.png" width="30" height="20" style="padding: 6px 0px 0px 0px;" alt="Add New Customer" title="Add New Customer" />
                                                    </a>
                                            </label>
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" TabIndex="2" AutoPostBack="true" placeholder="Min. 3 Chars to Search" OnTextChanged="txtCustomerName_TextChanged" />
                                        </div>
                                        <% if (hdnLocationStock.Value == "yes")
                                           {%>
                                            <div class="input-field col m2">
                                                <label class="active" for="drpLocation">Location</label>
                                                <asp:DropDownList ID="drpLocation" runat="server" ClientIDMode="Static" data-placeholder="Select ..." class="select2 browser-default" TabIndex="3" />
                                            </div>
                                        <%}%>
                                        <% if (hdnSerialKey.Value == "VAR2-DH0A-MAN9-8SIO")  /*"VAR2-DH0A-MAN9-8SIO"*/
                                        {%>
                                            <div class="input-field col m2">
                                                <label class="active" for="txtManualInwardNo">Manual Inward #</label>
                                                <asp:TextBox ID="txtManualInwardNo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="4"  placeholder=""/>
                                            </div>
                                        <%}%>
                                    </div>
                                    <div id="dvLoadItems" runat="server" class="row" visible="false">
                                        <div class="input-field col m3">
                                            <label class="active" for="drpReferenceNo">Select P.O #</label>
<%--                                            <asp:DropDownList ID="drpReferenceNo" runat="server" ClientIDMode="Static" EnableViewState="true" TabIndex="4" CssClass="select2-size-sm browser-default" multiple="multiple"/>--%>
                                                <%--<asp:DropDownList ID="drpReferenceNo" runat="server" ClientIDMode="Static" EnableViewState="true" TabIndex="4" CssClass="select2-size-sm browser-default" multiple="multiple"/>--%>
                                                <asp:listbox runat="server" id="drpReferenceNo" selectionmode="Multiple" style="width: 200px;" ClientIDMode="Static" EnableViewState="true" TabIndex="4" CssClass="select2-size-sm browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpReferenceNo_SelectedIndexChanged" onchange="YourChangeEventJS(this)"></asp:listbox>
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="lstOrderProduct">Select P.O Product</label>
                                            <asp:listbox runat="server" id="lstOrderProduct" selectionmode="Multiple" style="width: 200px;"></asp:listbox>                                        
                                        </div>
                                        <div class="input-field col m2">
<%--                                            <button id="Button3" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onclick="javascript:getSelectedOrders();" onserverclick="btnLoadItems_Click" TabIndex="5"><i class="material-icons left">save</i>Load Products</button>--%>

                                            <button id="btnLoadItems" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnLoadItems_Click" TabIndex="5"><i class="material-icons left">save</i>Load Products</button>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col m12">
                                            <ul id="Ul1" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color:antiquewhite; color:navy;">
                                                <li class="tab col m3 p-0"><a id="lnkProducts" class="left active" href="#pnl_Products">Product Detail</a></li>
                                                <li class="tab col m3 p-0"><a id="lnkTransport" class="left" href="#pnl_Transport">Transport Detail</a></li>
                                            </ul>
                                        </div>
                                        <div id="pnl_Products" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                            <div class="row">
                                                <div class="col m12">
                                                    <table id="tblInwardDetail" class="stdtable" cellpadding="0" cellspacing="0" border="0" style="width:100%;">
                                                        <asp:Repeater ID="rptInwardDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptInwardDetail_ItemCommand" OnItemDataBound="rptInwardDetail_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr>
                                                                        <th>Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                            <a href="javascript:openProductInfo('add');">
                                                                                <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" tabindex="51" />
                                                                            </a>
                                                                        </th>
                                                                        <th class="right-align">Quantity <span class="materialize-red-text font-weight-800">*</span></th>
                                                                        <th class="center-align">Unit</th>
                                                                        <th class="right-align">Unit Rate <span class="materialize-red-text font-weight-800">*</span></th>
                                                                        <th class="right-align">Disc. %</th>
                                                                        <th class="center-align hide">Dis.Amt</th>
                                                                        <th class="right-align">Net Rate</th>
                                                                        <th class="right-align">Amount</th>
                                                                        <th class="right-align">Tax Rate</th>
                                                                        <th class="right-align">Tax Amount</th>
                                                                        <th class="right-align">Net Amount</th>
                                                                        <th class="center-align width-10">Order #</th>
                                                                        <th class="center-align">Action</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                                    <asp:HiddenField ID="edTaxType" runat="server" ClientIDMode="Static" Value='<%# Eval("TaxType") %>' />
                                                                    <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                                    <td class="left-align"><%# Eval("ProductNameLong") %></td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edQuantity" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;"  TabIndex="6"/>
                                                                    </td>
                                                                    <td class="center-align width-7"><%# Eval("Unit") %></td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edUnitRate" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("UnitRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;"  TabIndex="7"/>
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edDiscountPercent" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountPercent") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;"  TabIndex="8"/>
                                                                    </td>
                                                                    <td class="right-align hide">
                                                                        <asp:TextBox ID="edDiscountAmt" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountAmt") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Enabled="false" Style="width: 80px; text-align: right;" TabIndex="9"/>
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edNetRate" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("NetRate") %>' AutoPostBack="true" Style="width: 80px; text-align: right;" Enabled="false" TabIndex="10"/>
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edAmount" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false"  TabIndex="11"/>
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:HiddenField ID="edhdnCGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("CGSTPer") %>' />
                                                                        <asp:HiddenField ID="edhdnSGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("SGSTPer") %>' />
                                                                        <asp:HiddenField ID="edhdnIGSTPer" runat="server" ClientIDMode="Static" Value='<%# Eval("IGSTPer") %>' />
                                                                        <asp:TextBox ID="edTaxRate" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;"  TabIndex="12"/>
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:HiddenField ID="edhdnCGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("CGSTAmt") %>' />
                                                                        <asp:HiddenField ID="edhdnSGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("SGSTAmt") %>' />
                                                                        <asp:HiddenField ID="edhdnIGSTAmt" runat="server" ClientIDMode="Static" Value='<%# Eval("IGSTAmt") %>' />
                                                                        <asp:TextBox ID="edTaxAmount" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxAmount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" TabIndex="13"/>
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edNetAmount" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("NetAmount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" TabIndex="14"/>
                                                                    </td>
                                                                    <td class="center-align width-10">
                                                                         <asp:TextBox ID="edForOrderNo" runat="server" ClientIDMode="Static" Text='<%# Eval("OrderNo") %>' Style="width:100px; text-align: center; font-size:12px;" Enabled="false" TabIndex="15" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="16" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="background-color: #dde8f3;">
                                                                    <td>
                                                                        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnTaxType" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="17" Width="100%" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtQuantity" MaxLength="15" runat="server" ClientIDMode="Static" CssClass="form-control" Width="80" TabIndex="18" OnTextChanged="txtQuantity_TextChanged" onblur="javascript:calcQuotation();" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox MaxLength="15" ID="txtUnit" runat="server" ClientIDMode="Static" Width="80" TabIndex="19" Enabled="false" />
                                                                     </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtUnitRate" MaxLength="15" runat="server" ClientIDMode="Static" Width="80" TabIndex="20"  OnTextChanged="txtUnitRate_TextChanged" onblur="javascript:calcQuotation();" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtDiscountPercent" MaxLength="15" runat="server" ClientIDMode="Static" Width="80" TabIndex="21" OnTextChanged="txtDiscountPercent_TextChanged" onblur="javascript:calcQuotation();" />
                                                                    </td>
                                                                    <td class="text-right hide">
                                                                        <asp:TextBox ID="txtDiscountAmt" runat="server" ClientIDMode="Static" Width="80" TabIndex="22" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtNetRate" MaxLength="15" runat="server" ClientIDMode="Static" Width="80" TabIndex="23" Enabled="false" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtAmount" MaxLength="15" runat="server" ClientIDMode="Static" Width="100" TabIndex="24" Enabled="false" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:HiddenField ID="hdnCGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnSGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnIGSTPer" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtTaxRate" MaxLength="15" runat="server" ClientIDMode="Static" Width="80" TabIndex="25" OnTextChanged="txtTaxRate_TextChanged" onblur="javascript:calcQuotation();" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:HiddenField ID="hdnCGSTAmt" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnSGSTAmt" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnIGSTAmt" runat="server" ClientIDMode="Static" />

                                                                        <asp:TextBox ID="txtTaxAmount" MaxLength="15" runat="server" ClientIDMode="Static" Width="100" TabIndex="26" Enabled="false" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtNetAmount" MaxLength="15" runat="server" ClientIDMode="Static" Width="100" TabIndex="27" Enabled="false" />
                                                                    </td>
                                                                    <td class="right-align width-10" style="vertical-align: top;">
                                                                        <asp:DropDownList ID="drpForOrderNo" runat="server" ClientIDMode="Static" TabIndex="28" data-placeholder="Select ..." CssClass="select2-theme browser-default" />
                                                                    </td>                                                        
                                                                    <td class="text-center" style="vertical-align: top;">
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="90" Height="30" TabIndex="29" />
                                                                    </td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col m12">
                                                    <table id="tblSummary" class="table responsive-table striped float-right" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tbody>
                                                            <tr class="border-none">
                                                                <td class="width-80 right-align">Basic Amount</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotBasicAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="45" ReadOnly="true" />
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
                                                            <tr class="border-none hide">
                                                                <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="HiddenField2" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="HiddenField3" runat="server" ClientIDMode="Static" />
                                                                <td class="width-80 right-align">Total Aadditional Amount</td>
                                                                <td class="width-20">
                                                                    <asp:TextBox ID="txtTotAddTaxAmt" class="form-control inputCalc" runat="server" ClientIDMode="Static" TabIndex="54" ReadOnly="true" />
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
                                        <div id="pnl_Transport" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                            <div class="container" style="max-width: none; width: 98%; margin: 5px !important;">
                                                <div class="row">
                                                    <div class="input-field col m3">
                                                        <label class="active" for="drpModeOfTransport">Mode Of Transport #</label>
                                                        <asp:DropDownList ID="drpModeOfTransport" runat="server" class="select2-theme browser-default" TabIndex="41">
                                                            <asp:ListItem Text="Road" Value="Road" />
                                                            <asp:ListItem Text="Rail" Value="Rail" />
                                                            <asp:ListItem Text="Air" Value="Air" />
                                                            <asp:ListItem Text="Ship" Value="Ship" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtTransporterName">Transporter Name</label>
                                                        <asp:TextBox ID="txtTransporterName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="42" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtVehicleNo">Vehicle No.</label>
                                                        <asp:TextBox ID="txtVehicleNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="43" placeholder="" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtLRNo">LR No./DC No.</label>
                                                        <asp:TextBox ID="txtLRNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="44" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m3">
                                                        <label class="active" for="txtLRDate">LR Date/DC Date</label>
                                                        <asp:TextBox ID="txtLRDate" runat="server" class="form-control" TextMode="Date" ClientIDMode="Static" TabIndex="45" placeholder="" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="input-field col m6">
                                                        <label class="active" for="txtTransportRemark">Remark</label>
                                                        <asp:TextBox ID="txtTransportRemark" runat="server" class="form-control" ClientIDMode="Static" TabIndex="46" placeholder="" TextMode="MultiLine" MaxLength="1000" Rows="4" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                            <button id="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="47"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button id="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="48"><i class="material-icons left">save</i>Save</button>
                        </div>
                    </div>

            <%--**************************** Customer Popup ***********************************--%>
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
                        $("#txtProductName").focusin(function () {
                            window.scrollBy(0, 150);
                        });

                        function bindDDLTo(selector) {
                            if ($(selector).val().length >= 3) {
                                jQuery.ajax({
                                    type: "POST",
                                    url: "InquiryInfo.aspx/FilterCustomer",
                                    data: '{pCustName:\'' + $(selector).val() + '\'}',
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
                                                return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                            },
                                            onSelect: function (e, term, item) {
                                                console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                                $(selector).val(item.html());
                                                $("#hdnCustomerID").val(item.data('langname'));
                                                $("#txtInwardDate").focus();
                                            }
                                        });
                                    },
                                    error: function (r) { alert('Error : ' + r.responseText); },
                                    failure: function (r) { alert('failure'); }
                                });
                                return false;
                            }
                        }

                        function bindDDLProductTo(selector) {
                            if ($(selector).val().length >= 3) {
                                jQuery.ajax({
                                    type: "POST",
                                    url: "InquiryInfo.aspx/FilterProduct",
                                    data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeMaterialInward\'}',
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
                        }


                        function clearProductField() {
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

                        function YourChangeEventJS(ddl) {
                            var test = $(".select2-selection__rendered").find('li.select2-selection__choice').text();
                            $("#hdnSelectedReference").val(test.substring(1).replace("×", ","));
                            $("#hdnSelectedReference").val($("#hdnSelectedReference").val().replace("×", ","));
                        }
                    </script>
<%--                </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </form>
</body>
</html>
