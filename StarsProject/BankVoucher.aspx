<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankVoucher.aspx.cs" Inherits="StarsProject.BankVoucher" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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

        .ui-widget-header, .ui-state-default, ui-button {
            background: #b9cd6d;
            border: 1px solid #b9cd6d;
            color: #FFFFFF;
            font-weight: bold;
        }

        input[type=radio] {
            opacity: 1;
            pointer-events: all;
        }

        .checkbox-container input {
            visibility: hidden;
            position: absolute;
            z-index: 2;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
            //$('.timepicker').timepicker();
        });
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
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        //function validateForm() {
        //    var errmsg = "";
        //    var validflag = true;
        //    // ----------------------------------------
        //    if (jQuery("#txtCustomerName").val() == "") {

        //        if (jQuery("#txtCustomerName").val() == "")
        //            errmsg += "CustomerName is required</br>";


        //        jQuery("#divErrorMessage").html(errmsg).show();

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    return validflag;
        //}
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <div class="clearall"></div>

                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnAccountID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnTDSAccountID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustStateID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnPayRec" runat="server" ClientIDMode="Static" />

                <div id="myModal" style="display: block; width: 100%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="input-field col m2">
                                    <label class="active" for="txtVoucherDate">Voucher Date <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:TextBox ID="txtVoucherDate" runat="server" class="form-control" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" TabIndex="1" placeholder=""  TextMode="Date"/>
                                </div>
                                <div class="input-field col m2">
                                    <label class="active" for="txtVoucherNo">Voucher No</label>
                                    <asp:TextBox ID="txtVoucherNo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="2" ReadOnly="true" placeholder="" />
                                </div>
                                <div class="input-field col m2" runat="server" visible="false">
                                    <label class="active" for="drpInquiry">Voucher Type</label>
                                    <asp:DropDownList ID="drpVoucherType" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="3">
                                        <asp:ListItem Text="Bank" Value="Bank" Selected="True"/>
                                    </asp:DropDownList>
                                </div>
                                <div class="input-field col m3">
                                    <label class="active" for="drpInquiry">Rec./Pay.</label>
                                    <asp:DropDownList ID="drpRecPay" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="drpRecPay_SelectedIndexChanged">
                                        <asp:ListItem Text="Receivable" Value="Receivable" />
                                        <asp:ListItem Text="Payable" Value="Payable" />
                                    </asp:DropDownList>
                                </div>
                                <div class="input-field col m3">
                                    <label class="active" for="drpEmployee">Select Employee</label>
                                    <asp:DropDownList ID="drpEmployee" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="drpRecPay_SelectedIndexChanged" />
                                </div>
                                <div class="input-field col m5 hide">
                                    <label class="active" for="group101">Select Transaction Type</label>
                                    <label style="left:0px;">
                                        <input class="with-gap" name="group101" type="radio" checked="checked" style="position:relative !important;"/>
                                        <span>On A/c</span>

                                        <input class="with-gap" name="group101" type="radio" style="position:relative !important;"/>
                                        <span>Advance</span>
                                    </label>
                                    <asp:RadioButtonList class="form-control hide" ID="rdblTransType" runat="server" RepeatDirection="Horizontal" TabIndex="5">
                                        <asp:ListItem Text="On A/C." runat="server" Selected="True" Value="acc"></asp:ListItem>
                                        <asp:ListItem Text="Advance" runat="server" Value="adv" style="margin-right: 70px"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col m6" style="background-color: #efe6e6; padding: 15px; border: 2px solid silver; border-radius: 5px;">
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" runat="server" clientidmode="Static" id="lblAccountName" for="txtAccountName">Debit A/c Name
                                                <small class="text-muted font-italic">&nbsp;(Min 3 chars to search)</small><span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" Tabindex="3" />
                                                </a>
                                                <a href="javascript:openCustomerInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                </a>
                                            </label>
                                            <asp:TextBox ID="txtAccountName" runat="server" ClientIDMode="Static" class="form-control" placeholder="" AutoPostBack="true" onKeyup="bindDDLToAccount('#txtAccountName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="6" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" runat="server" clientidmode="Static" id="lblCustomerName" for="txtCustomerName">Credit A/c Name
                                                <small class="text-muted font-italic">&nbsp;(Min 3 chars to search)</small> <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" Tabindex="3" />
                                                </a>
                                                <a href="javascript:openCustomerInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                </a>
                                            </label>
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" placeholder="" AutoPostBack="true" onKeyup="bindDDLToCustomer('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="7" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtVoucherAmount">Voucher Amount</label>
                                            <asp:TextBox ID="txtVoucherAmount" runat="server" class="form-control" MaxLength="15" ClientIDMode="Static" TabIndex="8" OnTextChanged="txtVoucherAmount_TextChanged" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtTDSAccountID">TDS A/c</label>
                                            <asp:TextBox ID="txtTDSAccountID" runat="server" ClientIDMode="Static" class="form-control" placeholder="" AutoPostBack="true" onKeyup="bindDDLToTDS('#txtTDSAccountID')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="7" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtTDSAmount">TDS Amount</label>
                                            <asp:TextBox ID="txtTDSAmount" runat="server" class="form-control" MaxLength="15" ClientIDMode="Static" TabIndex="8" OnTextChanged="txtVoucherAmount_TextChanged" placeholder="" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col m6" style="background-color: #efe6e6; padding: 15px; border: 2px solid silver; border-radius: 5px;">
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="drpTransMode">Transaction Mode <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpTransMode" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="9" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtBankName">Bank/Portal/Payment App. Name</label>
                                            <asp:TextBox ID="txtBankName" runat="server" MaxLength="100" class="form-control" ClientIDMode="Static" TabIndex="10" onKeyup="bindDDLBankNameTo('#txtBankName')" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtTransID">Transaction ID/Cheque No</label>
                                            <asp:TextBox ID="txtTransID" runat="server" MaxLength="50" class="form-control" ClientIDMode="Static" TabIndex="11" placeholder="" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtTransDate">Transaction Date/Cheque Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtTransDate" runat="server" class="form-control" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" TabIndex="12" placeholder=""  TextMode="Date"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                            </div>
                            <asp:Panel CssClass="row mt-1" ID="pnlPayment" runat="server" Visible="false" style="background-color: #eff1f3; padding: 15px; border: 2px solid silver; border-radius: 5px;">
                                <div class="row">
                                    <div class="input-field col m4">
                                        <label class="active" for="drpTerminationOfDelivery">Termination Of Delivery <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:DropDownList ID="drpTerminationOfDelivery" runat="server" ClientIDMode="Static" TabIndex="13" class="select2-theme browser-default" Style="height: inherit;" AutoPostBack="true" OnTextChanged="drpTerminationOfDelivery_SelectedIndexChanged" />
                                    </div>
                                    <div class="input-field col m4">
                                        <label class="active" for="rdblRDURD">RD/URD</label>
                                        <asp:RadioButtonList class="form-control" ID="rdblRDURD" runat="server" RepeatDirection="Horizontal" TabIndex="14" OnSelectedIndexChanged="rdblRDURD_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="RD" runat="server" Selected="True" Value="rd" style="margin-right: 70px"></asp:ListItem>
                                            <asp:ListItem Text="URD" runat="server" Value="urd"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="input-field col m4">
                                        <label class="active" for="drpTaxPer">Tax %</label>
                                        <asp:DropDownList ID="drpTaxPer" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="15" AutoPostBack="true" OnSelectedIndexChanged="drpTaxPer_SelectedIndexChanged">
                                            <asp:ListItem Text="3 %" Value="3.00" />
                                            <asp:ListItem Text="5 %" Value="5.00" />
                                            <asp:ListItem Text="12 %" Value="12.00" />
                                            <asp:ListItem Text="18 %" Value="18.00" />
                                            <asp:ListItem Text="28 %" Value="28.00" />
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnCGSTPer" runat="server" ClientIDMode="Static" />
                                        <asp:HiddenField ID="hdnSGSTPer" runat="server" ClientIDMode="Static" />
                                        <asp:HiddenField ID="hdnIGSTPer" runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m4">
                                        <label class="active" for="txtTotBasicAmt">Basic Amount</label>
                                        <asp:TextBox ID="txtTotBasicAmt" class="form-control inputCalc" MaxLength="15" runat="server" ClientIDMode="Static" TabIndex="16" ReadOnly="true" placeholder="" />
                                    </div>
                                    <div class="input-field col m4">
                                        <label class="active" for="txtTotGST">GST Amount</label>
                                        <asp:TextBox ID="txtTotGST" class="form-control inputCalc" runat="server" MaxLength="15" ClientIDMode="Static" TabIndex="17" ReadOnly="true" placeholder="" />
                                        <asp:HiddenField ID="hdnCGSTAmt" runat="server" ClientIDMode="Static" />
                                        <asp:HiddenField ID="hdnSGSTAmt" runat="server" ClientIDMode="Static" />
                                        <asp:HiddenField ID="hdnIGSTAmt" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div class="input-field col m4">
                                        <label class="active" for="txtTotNetAmt">Net Amount</label>
                                        <asp:TextBox ID="txtTotNetAmt" class="form-control inputCalc" MaxLength="15" runat="server" ClientIDMode="Static" TabIndex="18" ReadOnly="true" placeholder="" Style="font-weight: bold;" />
                                    </div>
                                </div>
                            </asp:Panel>

                            <div class="row mt-1 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                <div class="input-field col m6">
                                    <label class="active" for="txtTransRemark">Transaction Notes <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:TextBox ID="txtTransRemark" runat="server" class="textarea" ClientIDMode="Static" TabIndex="19" MaxLength="250" TextMode="MultiLine" Rows="3" placeholder="" />
                                </div>
                                <div class="input-field col m6">
                                    <label class="active" for="rptPartialTrans">Allocate Bill Wise Payment</label>
                                    <table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <asp:Repeater ID="rptPartialTrans" runat="server" ClientIDMode="Static" OnItemCommand="rptPartialTrans_ItemCommand" OnItemDataBound="rptPartialTrans_ItemDataBound" >
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th class="center-align" style="width: 40%; color: white;">Invoice No</th>
                                                        <th class="center-align" style="width: 40%; color: white;">Amount</th>
                                                        <th class="center-align" style="width: 20%; color: white;">Action</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                    <asp:HiddenField ID="hdnParentID" runat="server" ClientIDMode="Static" Value='<%#Eval("ParentID") %>' />
                                                    <td class="center-align">
                                                        <asp:Label ID="lblInvoiceNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="101" MaxLength="250" Text='<%#Eval("InvoiceNo") %>' />
                                                    </td>
                                                    <td class="center-align">
                                                        <asp:Label ID="lblAmount" runat="server" class="form-control" ClientIDMode="Static" TabIndex="102"  MaxLength="250" Text='<%#Eval("Amount") %>' />
                                                    </td>
                                                    <td class="center-align">
                                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" Width="20" Height="20" TabIndex="106" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <tr>
                                                    <td class="center-align">
                                                        <asp:DropDownList ID="drpInvoiceNo" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="101" MaxLength="250"  AutoPostBack="true" OnSelectedIndexChanged="drpInvoiceNo_SelectedIndexChanged" />
                                                    </td>
                                                    <td class="center-align">
                                                        <asp:TextBox ID="txtAmount" runat="server" class="form-control" ClientIDMode="Static" TabIndex="102"  MaxLength="250" />
                                                    </td>
                                                    <td class="center-align">
                                                        <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/buttons/bt-add.png" data-tooltip="Add Contacts" Width="110" Height="30" TabIndex="106" CommandName="Save"/>
                                                    </td>
                                                </tr>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                            <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="20"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="21"><i class="material-icons left">save</i>Save</button>
                        </div>
                    </div>
                </div>
            </div>

            <script type="text/javascript">
                function bindDDLToCustomer(selector) {
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
                                        return '<div class="autocomplete-suggestion" style="position:relative !important;" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        //if ($("#hdnPayRec").val() == 'Payable')
                                        //    $("#hdnAccountID").val(item.data('langname'));
                                        //else
                                        $("#hdnCustomerID").val(item.data('langname'));
                                        $("#txtCustomerName").focus();
                                    }
                                });

                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                        return false;
                    }
                }

                function bindDDLToAccount(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterCustomerByModule",
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
                                        return '<div class="autocomplete-suggestion" style="position:relative !important;" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        //if ($("#hdnPayRec").val() == 'Payable')
                                        //    $("#hdnCustomerID").val(item.data('langname'));
                                        //else
                                        $("#hdnAccountID").val(item.data('langname'));
                                        $("#txtAccountName").focus();
                                    }
                                });

                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                        return false;
                    }
                }

                function bindDDLBankNameTo(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "FinancialTrans.aspx/FilterBank",
                            data: '{pBankName:\'' + $(selector).val() + '\'}',
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
                                        return '<div class="autocomplete-suggestion" data-lang="' + item.BankName + '" data-val="' + search + '">' + item.BankName + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#txtBankName").focus();
                                    }
                                });

                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                        return false;
                    }
                }

                function bindDDLToTDS(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterCustomerByModule",
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
                                        return '<div class="autocomplete-suggestion" style="position:relative !important;" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnTDSAccountID").val(item.data('langname'));
                                        $("#txtTDSAccountID").focus();
                                    }
                                });

                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                        return false;
                    }
                }

            </script>
        </div>
    </form>
</body>
</html>
