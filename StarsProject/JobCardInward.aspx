<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobCardInward.aspx.cs" Inherits="StarsProject.JobCardInward" %>
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

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

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
        $(document).ready(function () {
            $('.timepicker').timepicker();
        });
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
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        //function validateForm() {
        //    var errmsg = "";
        //    var validflag = true;
        //    var cnt = jQuery("#tblInwardDetail tr").length;
        //    // ----------------------------------------
        //    if (jQuery("#txtInwardNo").val() == "" || jQuery("#txtInwardDate").val() == "" || (jQuery("#txtCustomerName").val() == "" || jQuery("#hdnCustomerID").val() == "" || jQuery("#hdnCustomerID").val() == "0") || cnt <= 1) {

        //        if (jQuery("#txtInwardNo").val() == "")
        //            errmsg += "Inward No is required</br>";

        //        if (jQuery("#txtInwardDate").val() == "")
        //            errmsg += "Inward Date is required</br>";

        //        if (jQuery("#txtCustomerName").val() == "" || jQuery("#hdnCustomerID").val() == "" || jQuery("#hdnCustomerID").val() == "0")
        //            errmsg += "Customer selection is required</br>";

        //        if (cnt <= 1)
        //            errmsg += "Atleast One Item is required to save Inward</br>";

        //        jQuery("#divErrorMessage").html(errmsg).show();

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    return validflag;
        //}

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

                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtInwardNo">Job Card #</label>
                                            <asp:TextBox ID="txtInwardNo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder=""/>
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtInwardDate">Job Card Inward Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtInwardDate" runat="server" class="form-control" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" placeholder="" TabIndex="2" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m5">
                                            <label class="active" for="txtCustomerName">Vendor Name  <small class="text-muted font-italic">(Min. 3 chars to search)</small> <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                            <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Vendor Info" style="display: inline-block; padding: 6px 0px 0px 0px;" Tabindex="3" />
                                                        </a>
                                                    <a href="javascript:openCustomerInfo('add');">
                                                        <img src="images/addCustomer.png" width="30" height="20" style="padding: 6px 0px 0px 0px;" alt="Add New Vendor" title="Add New Vendor" />
                                                    </a>
                                            </label>
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" TabIndex="3" placeholder="" />
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
                                                            <th class="center-align">Quantity <span class="materialize-red-text font-weight-800">*</span></th>
<%--                                                            <th class="text-right">Unit Rate <span class="materialize-red-text font-weight-800">*</span></th>
                                                            <th class="text-right">Disc. %</th>
                                                            <th class="text-right">Net Rate</th>
                                                            <th class="text-right">Amount</th>
                                                            <th class="text-right">Tax Rate</th>
                                                            <th class="text-right">Tax Amount</th>
                                                            <th class="text-right">Net Amount</th>--%>
                                                            <th class="left-align">Outward No</th>
                                                            <th class="text-center center-align">Action</th>
                                                        </tr>
                                                    </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                        <td class="text-left">
                                                            <%# Eval("ProductName") %>
                                                            <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                        </td>
                                                        <td class="center-align">
                                                            <asp:TextBox ID="edQuantity" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' AutoPostBack="true" OnTextChanged="edQuantity_TextChanged" Style="width: 150px; text-align: right;" />
                                                        </td>
<%--                                                        <td class="text-right">
                                                            <asp:TextBox ID="edUnitRate" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("UnitRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="edDiscountPercent" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountPercent") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="edNetRate" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("NetRate") %>' AutoPostBack="true" Style="width: 80px; text-align: right;" Enabled="false" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="edAmount" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="edTaxRate" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="edTaxAmount" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxAmount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="edNetAmount" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("NetAmount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" />
                                                        </td>--%>
                                                        <td class="left-align">
                                                                 <asp:TextBox ID="edOutwardNo" runat="server" ClientIDMode="Static" Text='<%# Eval("OutwardNo") %>' Style="width: 200px; text-align: right;" Enabled="false" TabIndex="15" />
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
                                                        <td class="text-center center-align">
                                                            <asp:TextBox ID="txtQuantity" MaxLength="15" runat="server" ClientIDMode="Static" CssClass="form-control" Width="150px" TabIndex="52" OnTextChanged="txtQuantity_TextChanged" onblur="javascript:calcQuotation();" />
                                                        </td>
<%--                                                        <td class="text-right hide">
                                                            <asp:TextBox MaxLength="15" ID="txtUnit" runat="server" ClientIDMode="Static" Width="80" TabIndex="50" Enabled="false" />
                                                         </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="txtUnitRate" MaxLength="15" runat="server" ClientIDMode="Static" Width="80" TabIndex="54"  OnTextChanged="txtUnitRate_TextChanged" onblur="javascript:calcQuotation();" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="txtDiscountPercent" MaxLength="15" runat="server" ClientIDMode="Static" Width="80" TabIndex="55" OnTextChanged="txtDiscountPercent_TextChanged" onblur="javascript:calcQuotation();" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="txtNetRate" MaxLength="15" runat="server" ClientIDMode="Static" Width="80" TabIndex="56" Enabled="false" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="txtAmount" MaxLength="15" runat="server" ClientIDMode="Static" Width="100" TabIndex="57" Enabled="false" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="txtTaxRate" MaxLength="15" runat="server" ClientIDMode="Static" Width="80" TabIndex="58" OnTextChanged="txtTaxRate_TextChanged" onblur="javascript:calcQuotation();" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="txtTaxAmount" MaxLength="15" runat="server" ClientIDMode="Static" Width="100" TabIndex="59" Enabled="false" />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:TextBox ID="txtNetAmount" MaxLength="15" runat="server" ClientIDMode="Static" Width="100" TabIndex="60" Enabled="false" />
                                                        </td>
    --%>                                                <td class="left-align">
                                                            <asp:DropDownList ID="drpOutwardNo" runat="server" ClientIDMode="Static" TabIndex="17" CssClass="select2-theme browser-default" Width="200px" style="text-align:center;" />
                                                        </td>
                                                        <td class="text-center center-align">
                                                            <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/Buttons/bt-Add.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="100" Height="40" TabIndex="61" />
                                                        </td>
                                                    </tr>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                        </div>
                                    </div>
                                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                        <button id="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_ServerClick" TabIndex="62"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                        <button id="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_ServerClick" TabIndex="63"><i class="material-icons left">save</i>Save</button>
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
                                    data: '{pCustName:\'' + $(selector).val() + '\', pSearchModule:\'CustomerSearchAll\'}',
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
                    </script>
<%--                </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </form>
</body>
</html>
