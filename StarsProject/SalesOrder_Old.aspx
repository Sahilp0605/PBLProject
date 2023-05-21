<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesOrder_Old.aspx.cs" Inherits="StarsProject.SalesOrder_Old" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link  href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

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

        .autocomplete-suggestions {
            min-width: 600px !important;
        }
    </style>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            //jQuery('#drpInquiry').on('change', function () {
            //    var tmpval = jQuery(this).val();
            //    alert(tmpval)
            //    e.preventDefault();
            //    jQuery('#dialog-loadproducts').dialog('open');
            //});

            //jQuery('#dialog-loadproducts').dialog({
            //    resizable: false,
            //    height: 200,
            //    autoOpen: false,
            //    modal: true,
            //    buttons: {
            //        "Yes": function () {
            //            jQuery("#hdnConfirm").val("yes");
            //            __doPostBack("loadproducts", "");
            //            jQuery(this).dialog("close");
            //        },
            //        "No": function () { jQuery(this).dialog("close"); }
            //    }
            //});
        });
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            var cnt = jQuery("#tblQuotationDetail tr").length;
            // ----------------------------------------
            if (jQuery("#txtQuotationDate").val() == "" || jQuery("#txtCustomerName").val() == "" || jQuery("#hdnCustomerID").val() == "" || jQuery("#hdnCustomerID").val() == "0" || cnt <= 3) {

                //if (jQuery("#txtQuotationNo").val() == "")
                //    errmsg += "Quotation No is required</br>";

                if (jQuery("#txtQuotationDate").val() == "")
                    errmsg += "Quotation Date is required</br>";

                if (jQuery("#txtCustomerName").val() == "" || jQuery("#hdnCustomerID").val() == "" || jQuery("#hdnCustomerID").val() == "0")
                    errmsg += "Customer selection is required</br>";

                if (cnt <= 3)
                    errmsg += "Atleast One Item is required to save SalesOrder</br>";

                jQuery("#divErrorMessage").html(errmsg).show();

                validflag = false;
            }
            // ----------------------------------------
            return validflag;
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
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">

    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">

        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <div class="widgetbox">
                <%-- Bootstrap Quotation Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <div id="myModal" style="display:block; width:100%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Manage Sales Order
                                <asp:Label CssClass="float-right" ID="lblApprovalStatus" runat="server" ClientIDMode="Static" style="font-size:14px; color:white;" />
                            </h5>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="input-field col m2">
                                    <label class="active" for="txtQuotationNo">Order #</label>
                                    <asp:TextBox ID="txtOrderNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" readonly />
                                </div>
                                <div class="input-field col m3">
                                    <label class="active" for="txtQuotationDate">Order Date</label>
                                    <asp:TextBox ID="txtOrderDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" TextMode="Date" />
                                </div>
                                <div class="input-field col m">
                                    <label class="active" for="txtCustomerName">Customer Name  
                                        <small class="text-muted font-italic">(Minimum 3 chars to activate search)</small>
                                        <a href="javascript:openCustomerInfo('view');">
                                            <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display:inline-block;" />
                                        </a>&nbsp;
                                        <a href="javascript:openCustomerInfo('add');">
                                            <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" style="display:inline-block;" />
                                        </a>
                                    </label>
                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" AutoPostBack="true" OnTextChanged="txtCustomerName_TextChanged" TabIndex="1" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m3">
                                    <label class="active" for="drpQuotation">Quotation No</label>
                                    <asp:DropDownList ID="drpQuotation" runat="server" class="form-control" EnableViewState="true" ClientIDMode="Static" TabIndex="4" AutoPostBack="true" OnTextChanged="drpQuotation_TextChanged" />
                                </div>
                                <div  id="divEmployee" runat="server" clientidmode="static">
                                    <div class="input-field col m">
                                        <label class="active" for="drpSalesPerson">Sales Executive</label>
                                        <asp:DropDownList ID="drpSalesPerson" runat="server" class="form-control" EnableViewState="true" ClientIDMode="Static" TabIndex="5">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="input-field col m">
                                    <label class="active" for="drpApprovalStatus">Approval Status</label>
                                    <asp:DropDownList ID="drpApprovalStatus" runat="server" class="form-control" EnableViewState="true" ClientIDMode="Static" TabIndex="5" disabled>
                                        <asp:ListItem Text="Pending" Value="Pending" />
                                        <asp:ListItem Text="On Hold" Value="On Hold" />
                                        <asp:ListItem Text="Approved" Value="Approved" />
                                        <asp:ListItem Text="Rejected" Value="Rejected" />
                                    </asp:DropDownList>

                                </div>
                            </div>
                            <asp:Panel runat="server" ID="pnlDetail">
                                <div class="row">
                                    <table id="tblQuotationDetail" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <asp:Repeater ID="rptOrderDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptOrderDetail_ItemCommand" OnItemDataBound="rptOrderDetail_ItemDataBound">
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th>Product Name</th>
                                                        <th class="text-right">Quantity</th>
                                                        <th class="text-center">Unit</th>
                                                        <th class="text-right">Unit Rate</th>
                                                        <th class="text-right">Disc. %</th>
                                                        <th class="text-right">Net Rate</th>
                                                        <th class="text-right">Amount</th>
                                                        <th class="text-right">Tax Rate</th>
                                                        <th class="text-right">Tax Amount</th>
                                                        <th class="text-right">Net Amount</th>
                                                        <th class="text-center">Delete</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%# Eval("ProductName") %>
                                                        <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edQuantity" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                    </td>
                                                    <td class="text-center"><%# Eval("Unit") %></td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edUnitRate" runat="server" ClientIDMode="Static" Text='<%# Eval("UnitRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edDiscountPercent" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountPercent") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edNetRate" runat="server" ClientIDMode="Static" Text='<%# Eval("NetRate") %>' AutoPostBack="true" Style="width: 80px; text-align: right;" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edTaxRate" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edTaxAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxAmount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edNetAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("NetAmount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <tr style="background-color: #dde8f3;">
                                                    <td>
                                                        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                        <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="51" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" Width="80" TabIndex="52" onblur="javascript:calcQuotation();" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="80" TabIndex="53" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtUnitRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="54" onblur="javascript:calcQuotation();" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtDiscountPercent" runat="server" ClientIDMode="Static" Width="80" TabIndex="55" onblur="javascript:calcQuotation();" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtNetRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="56" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="57" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtTaxRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="58" onblur="javascript:calcQuotation();" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtTaxAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="59" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtNetAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="60" Enabled="false" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="20" Height="20" TabIndex="61" />
                                                    </td>
                                                </tr>
                                                <tr style="background-color: navy; min-height: 50px;">
                                                    <td class="text-right" colspan="6" style="color: White; font-size: 16px;">Grand Total: </td>
                                                    <td class="text-right">
                                                        <asp:Label ID="lblTotalGrossAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="12" Enabled="false" Style="color: White; font-size: 16px;" Text="1111" />
                                                    </td>
                                                    <td class="text-right" style="color: White; font-size: 16px;"></td>
                                                    <td class="text-right">
                                                        <asp:Label ID="lblTotalTaxAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="12" Enabled="false" Style="color: White; font-size: 16px;" Text="1111" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:Label ID="lblTotalNetAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="12" Enabled="false" Style="color: White; font-size: 16px;" Text="1111" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <%--                                </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </div>
                            </asp:Panel>
                            <div class="row mt-2 badge-primary" style="border-radius: 5px; padding: 5px;">
                                <div class="input-field col m7">
                                    <div style="height: 50px; width: 100%; margin-bottom: 10px; padding: 5px; border-bottom: 1px solid white;">Terms & Condition
                                        <asp:DropDownList ID="drpTNC" runat="server" ClientIDMode="Static" class="form-control float-right" EnableViewState="true" TabIndex="9" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="drpTNC_SelectedIndexChanged" />
                                    </div>
                                    <asp:TextBox ID="txtTermsCondition" runat="server" class="form-control" ClientIDMode="Static" TabIndex="10" TextMode="MultiLine" Rows="4"/>
                                    <small class="text-muted font-italic" style="color:white !important;"><b>Note: </b>Terms & Condition will carried on selection of Quotation #.</small>
                                </div>
                                <div class="input-field col m5 badge-light">
                                    <table id="tblPaySchedule" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <asp:Repeater ID="rptPaySchedule" runat="server" ClientIDMode="Static" OnItemCommand="rptPaySchedule_ItemCommand">
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th class="text-right">Amount</th>
                                                        <th class="text-center">Due Date</th>
                                                        <th class="text-center">Action</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox id="edPayAmount" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("PayAmount") %>' AutoPostBack="true" style="width:80px;"  Enabled="false" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox id="edDueDate" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("DueDate", "{0:yyyy-MM-dd}") %>' AutoPostBack="true" TextMode="Date" width="180px" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:ImageButton ID="ImgbtnEdit11" runat="server" ClientIDMode="Static" ImageUrl="~/images/savedata.png" ToolTip="Update" CommandName="Update" CommandArgument='<%# Eval("pkID") %>'  Width="40" Height="20" />
                                                        &nbsp;&nbsp;
                                                        <asp:ImageButton ID="ImgbtnDelete11" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'  Width="20" Height="20" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox id="ftPayAmount" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("PayAmount") %>' AutoPostBack="true" style="width:80px;"  Enabled="false" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox id="ftDueDate" runat="server" ClientIDMode="Static" CssClass="form-control" Text='<%# Eval("DueDate", "{0:yyyy-MM-dd}") %>' AutoPostBack="true" TextMode="Date" width="180px" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:ImageButton ID="ImgbtnAddSch" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Schedule" CommandName="addsch" CommandArgument='<%# Eval("pkID") %>'  Width="30" Height="30" />
                                                    </td>
                                                </tr>

                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m12">
                                    <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" OnClientClick="return validateForm();" OnClick="btnSave_Click" TabIndex="11" />
                            <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="12" />
                        </div>
                    </div>
                </div>
            </div>
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
                                url: "InquiryInfo.aspx/FilterCustomer",
                                data: '{pCustName:\'' + $(selector).val() + '\'}',
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
                                            console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
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
                        if ($(selector).val().length >= 3) {
                            jQuery.ajax({
                                type: "POST",
                                url: "InquiryInfo.aspx/FilterProduct",
                                data: '{pProductName:\'' + $(selector).val() + '\'}',
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
                </script>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
