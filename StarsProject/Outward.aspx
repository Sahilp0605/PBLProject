<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Outward.aspx.cs" Inherits="StarsProject.Outward" %>

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
        function pageLoad(sender, args) {
            $('tabs').tabs();
            $('.datepicker').datepicker({
                defaultDate: new Date((new Date()).getFullYear(), (new Date()).getMonth(), (new Date()).getDay()),
                format: "dd-mm-yyyy"
            });
            $('.timepicker').timepicker();
            // ------------------------------------------------------
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
            $("#lnkTransport").removeClass("active");
            $("#lnkShipment").removeClass("active");
            

            $("#pnl_Products").css("display", "none");
            $("#pnl_Transport").css("display", "none");
            $("#pnl_Shipment").css("display", "none");
            

            if (currTab == 'pnl_Products') {
                $("#lnkProducts").addClass("active");
                $("#pnl_Products").css("display", "block");
            }
            else if (currTab == 'pnl_Transport') {
                $("#lnkTransport").addClass("active");
                $("#pnl_Transport").css("display", "block");
            }
            else if (currTab == 'pnl_Shipment') {
                $("#lnkShipment").addClass("active");
                $("#pnl_Shipment").css("display", "block");
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
        // ----------------------------------------------------------
        function openSpecification(pProdID) {
            var t11;
            t11 = jQuery("#txtOutwardNo").val();
            var pageUrl = "ProductSpecification.aspx?Module=Outward&DocNo=" + t11 + "&FinishProductID=" + pProdID;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <%-- Bootstrap Outward Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnLocationStock" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCurrTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Products" />
                <asp:HiddenField ID="hdnSelectedReference" runat="server" ClientIDMode="Static" />

                <div id="myModal" style="display: block; width: 100%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="input-field col m2">
                                    <label class="active" for="txtOutwardNo">Outward #</label>
                                    <asp:TextBox ID="txtOutwardNo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder="" />
                                </div>
                                <div class="input-field col m2">
                                    <label class="active" for="txtOutwardDate">Outward Date <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:TextBox ID="txtOutwardDate" runat="server" class="form-control" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" placeholder="" TabIndex="2" TextMode="Date" />
                                </div>
                                <div class="input-field col m4">
                                    <label class="active" for="txtCustomerName">
                                        Customer Name  <small class="text-muted font-italic">(Minimum 3 chars to activate search)</small> <span class="materialize-red-text font-weight-800">*</span>
                                        <a href="javascript:openCustomerInfo('view');">
                                            <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block; padding: 6px 0px 0px 0px;" tabindex="3" />
                                        </a>
                                        <a href="javascript:openCustomerInfo('add');">
                                            <img src="images/addCustomer.png" width="30" height="20" style="padding: 6px 0px 0px 0px;" alt="Add New Customer" title="Add New Customer" />
                                        </a>
                                    </label>
                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="3" placeholder="" />
                                </div>
                                <div class="input-field col m2">
                                    <label class="active" for="drpReferenceNo">Sales Order #</label>
                                    <asp:DropDownList ID="drpReferenceNo" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="drpReferenceNo_SelectedIndexChanged" />
                                </div>
                                <% if (hdnLocationStock.Value == "yes")
                                    {%>
                                <div class="input-field col m2">
                                    <label class="active" for="drpLocation">Location</label>
                                    <asp:DropDownList ID="drpLocation" runat="server" ClientIDMode="Static" TabIndex="6" class="select2-theme browser-default" />
                                </div>
                                <%}%>
                            </div>
                            <div class="row" id="divReference" runat="server" clientidmode="Static" visible="false">
                                <div class="input-field col m2">
                                    <label class="active" for="txtExpRef">Exporter's Reference</label>
                                    <asp:TextBox ID="txtExpRef" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                                </div>
                                <div class="input-field col m2">
                                    <label class="active" for="txtBuyRef">Buyer's Order Reference</label>
                                    <asp:TextBox ID="txtBuyRef" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                                </div>
                                <div class="input-field col m2">
                                    <label class="active" for="txtOrderDate">Order Date </label>
                                    <asp:TextBox ID="txtOrderDate" runat="server" class="form-control" ClientIDMode="Static" placeholder="" TabIndex="2" TextMode="Date" />
                                </div>
                                <div class="input-field col m2">
                                    <label class="active" for="txtOtherRef">Other's Reference</label>
                                    <asp:TextBox ID="txtOtherRef" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                                </div>
                            </div>
                            <% if (hdnSerialKey.Value == "VAR2-DH0A-MAN9-8SIO")  
                                {%>
                            <div class="row">
                                <div class="input-field col m2">
                                    <label class="active" for="txtManualOutwardNo">Manual Outward #</label>
                                                <asp:TextBox ID="txtManualOutwardNo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="6"  placeholder=""/>
                                </div>
                            </div>
                            <%}%>
                            <div class="row">
                                <div class="col s12">
                                    <ul id="Ul1" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color: antiquewhite;">
                                        <li class="tab col m3 p-0"><a id="lnkProducts" class="left active" href="#pnl_Products">Product Detail</a></li>
                                        <li class="tab col m3 p-0"><a id="lnkTransport" class="left" href="#pnl_Transport">Transport Detail</a></li>
                                        <li class="tab col m3 p-0"><a id="lnkShipment" class="left" href="#pnl_Shipment" tabindex="79">Shipment Detail</a></li>
                                        
                                    </ul>
                                </div>
                                <div id="pnl_Products" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                    <div class="row">
                                        <div class="col m12">
                                            <table id="tblOutwardDetail" class="stdtable" cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                <asp:Repeater ID="rptOutwardDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptOutwardDetail_ItemCommand" OnItemDataBound="rptOutwardDetail_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <thead>
                                                            <tr>
                                                                <th>Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                    <a href="javascript:openProductInfo('add');">
                                                                        <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" tabindex="51" />
                                                                    </a>
                                                                </th>
                                                                <th class="text-right">Quantity <span class="materialize-red-text font-weight-800">*</span></th>
                                                                <th class="center-align">Unit<span class="materialize-red-text font-weight-800">*</span></th>
                                                                <th id="HTWeight" runat="server" class="center-align">Weight</th>
                                                                <th id="HTSerialNo" runat="server" class="center-align">Serial #</th>
                                                                <th id="HTBoxNo" runat="server" class="center-align">Box #</th>
                                                                <th class="center-align width-10">Order #</th>
                                                                <th class="center-align" style="width: 10%;">Specs</th>
                                                                <th class="center-align">Action</th>
                                                            </tr>
                                                        </thead>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                            <td class="text-left">
                                                                <%# Eval("ProductNameLong") %>
                                                                <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:TextBox ID="edQuantity" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:TextBox ID="edUnit" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Unit") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                            </td>
                                                            <td id="ITWeight" runat="server" class="center-align">
                                                                <asp:TextBox ID="edQuantityWeight" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("QuantityWeight") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 120px; text-align: right;" />
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:TextBox ID="edSerialNo" MaxLength="30" runat="server" ClientIDMode="Static" Text='<%# Eval("SerialNo") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 200px; text-align: right;" />
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:TextBox ID="edBoxNo" MaxLength="10" runat="server" ClientIDMode="Static" Text='<%# Eval("BoxNo") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 200px; text-align: right;" />
                                                            </td>
                                                            <td class="center-align width-10">
                                                                <asp:TextBox ID="edForOrderNo" runat="server" ClientIDMode="Static" Text='<%# Eval("OrderNo") %>' Style="width: 100px; text-align: center; font-size: 12px;" Enabled="false" TabIndex="15" />
                                                            </td>
                                                            <td class="center-align" style="width: 10%;">
                                                                <a id="lnkSpecs" href="javascript:openSpecification('<%# Eval("ProductID") %>');" tabindex="19"><small>Specs</small></a>
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr style="background-color: #dde8f3;">
                                                            <td>
                                                                <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="21" Width="100%" />
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" CssClass="form-control" Width="80" TabIndex="52" />
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="80" TabIndex="50" />
                                                            </td>
                                                            <td id="FTWeight" runat="server" class="center-align">
                                                                <asp:TextBox ID="txtQuantityWeight" runat="server" ClientIDMode="Static" Width="120" TabIndex="54" />
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:TextBox ID="txtSerialNo" runat="server" ClientIDMode="Static" Width="200" TabIndex="55" />
                                                            </td>
                                                            <td class="center-align">
                                                                <asp:TextBox ID="txtBoxNo" runat="server" ClientIDMode="Static" Width="200" TabIndex="56" />
                                                            </td>
                                                            <td class="right-align width-10" style="vertical-align: top;">
                                                                <asp:DropDownList ID="drpForOrderNo" runat="server" ClientIDMode="Static" TabIndex="17" CssClass="select2-theme browser-default" />
                                                            </td>
                                                            <td class="center-align" colspan="2">
                                                                <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/Buttons/bt-Add.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="150" Height="40" TabIndex="61" />
                                                            </td>
                                                        </tr>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div id="pnl_Transport" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
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
                                                <label class="active" for="txtDeliveryNote">Delivery Note</label>
                                                <asp:TextBox ID="txtDeliveryNote" runat="server" class="form-control" ClientIDMode="Static" TabIndex="59" placeholder="" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m2">
                                                <label class="active" for="txtLRNo">LR No.</label>
                                                <asp:TextBox ID="txtLRNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="60" placeholder="" />
                                            </div>
                                            <div class="input-field col m3">
                                                <label class="active" for="txtLRDate">LR Date</label>
                                                <asp:TextBox ID="txtLRDate" runat="server" class="form-control" TextMode="Date" ClientIDMode="Static" TabIndex="61" placeholder="" />
                                            </div>
                                            <div class="input-field col m2">
                                                <label class="active" for="txtDCNo">DC No.</label>
                                                <asp:TextBox ID="txtDCNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="60" placeholder="" />
                                            </div>
                                            <div class="input-field col m3">
                                                <label class="active" for="txtDCDate">DC Date</label>
                                                <asp:TextBox ID="txtDCDate" runat="server" class="form-control" TextMode="Date" ClientIDMode="Static" TabIndex="61" placeholder="" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m6">
                                                <label class="active" for="txtRemarks">Remarks</label>
                                                <asp:TextBox ID="txtRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="60" placeholder="" Rows="4" TextMode="MultiLine" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div id="pnl_Shipment" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <label class="active" for="txtPreCarrBy">Pre Carriage By (Transporter Name)</label>
                                            <asp:TextBox ID="txtPreCarrBy" runat="server" class="form-control" ClientIDMode="Static" TabIndex="80" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtPreCarrRecPlace">Place Of Rec.By Pre Carrier</label>
                                            <asp:TextBox ID="txtPreCarrRecPlace" runat="server" class="form-control" ClientIDMode="Static" TabIndex="81" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtFlightNo">Vessel/Flight No</label>
                                            <asp:TextBox ID="txtFlightNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="82" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <label class="active" for="txtPortOfLoading">Port Of Loading</label>
                                            <asp:TextBox ID="txtPortOfLoading" runat="server" class="form-control" ClientIDMode="Static" TabIndex="83" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtPortOfDispatch">Port Of Dispatch</label>
                                            <asp:TextBox ID="txtPortOfDispatch" runat="server" class="form-control" ClientIDMode="Static" TabIndex="84" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtPortOfDestination">Port Of Destination</label>
                                            <asp:TextBox ID="txtPortOfDestination" runat="server" class="form-control" ClientIDMode="Static" TabIndex="85" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <label class="active" for="txtContainerNo">Container No.</label>
                                            <asp:TextBox ID="txtContainerNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="86" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtPackages">Packages</label>
                                            <asp:TextBox ID="txtPackages" runat="server" class="form-control" ClientIDMode="Static" TabIndex="87" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtPackageType">Type Of Package</label>
                                            <asp:TextBox ID="txtPackageType" runat="server" class="form-control" ClientIDMode="Static" TabIndex="88" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <label class="active" for="txtNetWeight">Net Weight(KGs)</label>
                                            <asp:TextBox ID="txtNetWeight" runat="server" class="form-control" ClientIDMode="Static" TabIndex="89" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtGrossWeight">Gross Weight(KGs)</label>
                                            <asp:TextBox ID="txtGrossWeight" runat="server" class="form-control" ClientIDMode="Static" TabIndex="90" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtFOB">FOB (Free Of Board)</label>
                                            <asp:TextBox ID="txtFOB" runat="server" class="form-control" ClientIDMode="Static" TabIndex="91" placeholder="" />
                                        </div>
                                    </div>
                                </div>
                                
                            </div>

                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                                <button id="btnReset" type="button" runat="server" clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="62"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button id="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="63"><i class="material-icons left">save</i>Save</button>
                            </div>
                        </div>
                    </div>
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
                                        $("#txtOutwardDate").focus();
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
                            data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeMaterialOutward\'}',
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
            </script>
        </div>
    </form>
</body>
</html>
