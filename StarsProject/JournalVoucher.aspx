<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JournalVoucher.aspx.cs" Inherits="StarsProject.JournalVoucher" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <style type="text/css">
        table.responsive-table th {
            background-color: #6868d6 !important;
            color: white !important;
        }

        table.responsive-table th, table.responsive-table td {
            border-bottom: 1px solid silver !important;
        }

        input[type=text], input[type=password], input[type=email], input[type=url], input[type=number], textarea :focus {
            border-color: #3C90BE;
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

        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        //function validateForm() {
        //    var errmsg = "";
        //    var validflag = true;
        //    var cnt = jQuery("#tblInqProductGroup tr").length;

        //    // ----------------------------------------
        //    if (jQuery("#txtInquiryDate").val() == "" || jQuery("#txtCustomerName").val() == "" || jQuery("#hdnCustomerID").val() == "" || jQuery("#hdnCustomerID").val() == "0" || jQuery("#drpInquiryStatus").val() == "" || jQuery("#drpInquirySource").val() == "" || cnt <= 3) {

        //        if (jQuery("#txtInquiryDate").val() == "")
        //            errmsg += "<li>Inquiry Date is required</li>";

        //        if (jQuery("#txtCustomerName").val() == "" || jQuery("#hdnCustomerID").val() == "" || jQuery("#hdnCustomerID").val() == "0")
        //            errmsg += "<li>Customer selection is required</li>";

        //        if (jQuery("#drpInquiryStatus").val() == "")
        //            errmsg += "<li>Inquiry Status selection is required</li>";

        //        if (jQuery("#drpInquirySource").val() == "")
        //            errmsg += "<li>Inquiry Source selection is required</li>";

        //        if (cnt<=3)
        //            errmsg += "<li>Atleast One Item is required to save Inquiry Information</li>";



        //        validflag = false;
        //    }
        //    // ----------------------------------------

        //    if (jQuery("#txtInquiryDate").val() != "") {
        //        var dateOne = new Date(jQuery("#txtInquiryDate").val()); //Year, Month, Date  
        //        var dateTwo = new Date(); //Year, Month, Date  
        //        if (dateOne > dateTwo) {
        //            errmsg += "<li>Future Inquiry Date Not Allowed..!!!</li>";
        //            validflag = false;
        //        }
        //    }
        //    //---------------------------------------
        //    //jQuery("#divErrorMessage").html(errmsg).show();
        //    //alert('validate form')
        //    showErrorPopup(errmsg);
        //    return validflag;
        //}

        //function CompareDate() {
        //    //Note: 00 is month i.e. January 
        //    if (jQuery("#txtInquiryDate").val() != ""){
        //        var dateOne = new Date(jQuery("#txtInquiryDate").val()); //Year, Month, Date  
        //        var dateTwo = new Date(); //Year, Month, Date  
        //        if (dateOne > dateTwo) {
        //            //alert("Future Inquiry Date Not Allowed..!!!");
        //            showErrorPopup('<li>Future Inquiry Date Not Allowed..!!!</li>');
        //        } 
        //    }
        //}

        //function validateFormItems() {
        //    var errmsg = "";
        //    var validflag = true;
        //    // ----------------------------------------
        //    if (jQuery("#txtInquiryDate").val() == "" || jQuery("#drpCustomer").val() == "" || jQuery("#drpInquiryStatus").val() == "" || jQuery("#drpInquirySource").val() == "") {


        //        if (jQuery("#txtInquiryDate").val() == "")
        //            errmsg += "<li>Inquiry Date is required</li>";

        //        if (jQuery("#drpCustomer").val() == "")
        //            errmsg += "<li>Customer selection is required</li>";

        //        if (jQuery("#drpInquiryStatus").val() == "")
        //            errmsg += "<li>Inquiry Status selection is required</li>";

        //        if (jQuery("#drpInquirySource").val() == "")
        //            errmsg += "<li>Inquiry Source selection is required</li>";

        //        //if (jQuery("#txtFollowupDate").val() == "")
        //        //    errmsg += "Follow-Up Date selection is required</br>";
        //        // ----------------------------------------------------------
        //        //jQuery("#divErrorMessage").html(errmsg).show();
        //        alert('validate form item')
        //        showErrorPopup(errmsg);

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    return validflag;
        //}

        //function openCustomerInfo(cat) {
        //    if (cat == 'view') {
        //        var keyid = jQuery('#hdnCustomerID').val();
        //        if (keyid != '' && keyid != undefined) {
        //            jQuery.colorbox({ width: "100%", height: "100%", iframe: true, href: "customers.aspx?mode=view&id=" + keyid });
        //        }
        //        else {
        //            showErrorPopup('Please Search & Select Customer !');
        //        }
        //    }
        //    else if (cat == 'add') {
        //        jQuery.colorbox({ width: "100%", height: "100%", iframe: true, href: "customers.aspx?mode=view&id=0" });
        //    }
        //}

        /*---------------------------------------------------------*/
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Customer";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        /*Add Product view on Product Name Vikram Rajput 15-07-2020*/
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
    </script>

</head>
<body class="loginpage" style="background-color: transparent;">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <div id="myModal123" style="display: block; width: 100%;">
                    <div class="modal-content border-0">
                        <div class="modal-body col m12">
                            <div class="row">
                                <div class="col m5" style="border-right: 3px ridge silver;">
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtVoucherDate">Voucher Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtVoucherDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" onkeypress="return false;" placeholder="" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtVoucherNo">Voucher #</label>
                                            <asp:TextBox ID="txtVoucherNo" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="2" ReadOnly="true" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <label class="active" for="txtVoucherAmount">Voucher Amount</label>
                                            <asp:TextBox ID="txtVoucherAmount" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="3" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <label class="active" for="txtRemarks">Remarks<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtRemarks" runat="server" placeholder="" class="materialize-textarea" ClientIDMode="Static" TabIndex="4" TextMode="MultiLine" Rows="3" MaxLength="500" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col m7">
                                    <asp:UpdatePanel ID="upRepeater" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnlDetail">
                                                <div class="row">
                                                    <h5 class="card-title padding-1 m-0 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Product Detail</h5>
                                                    <table id="tblInqProductGroup" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <asp:Repeater ID="rptJournal" runat="server" ClientIDMode="Static" OnItemCommand="rptJournal_ItemCommand">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr>
                                                                        <th class="text-left">DB/CR</th>
                                                                        <th class="text-left">A/c Name
                                                                            <a href="javascript:openCustomerInfo('add');">
                                                                                 <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                                             </a></th>
                                                                        <th class="text-right">Amount</th>
                                                                        <th class="text-center">Action</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="blueShed">
                                                                    <asp:HiddenField ID="hdnpkIDDetail" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%#Eval("CustomerID") %>' />
                                                                    <asp:HiddenField ID="hdnTransType" runat="server" ClientIDMode="Static" Value='<%#Eval("TransType") %>' />
                                                                    <td class="text-left"><%# Eval("TransType") %></td>
                                                                    <td class="text-left"><%# Eval("CustomerName") %></td>
                                                                    <td class="text-right"><%# String.Format("{0:0.00}", Convert.ToDecimal(Eval("VoucherAmount"))) %></td>
                                                                    <td class="text-center">
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="padding: 5px;">
                                                                    <td class="text-right">
                                                                        <asp:DropDownList ID="drpTransType" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="6">
                                                                            <asp:ListItem Text="DB" Value="DB" />
                                                                            <asp:ListItem Text="CR" Value="CR" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hdnCustomerIDNew" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtCustomerNameNew" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerNameNew')" AutoPostBack="true" TabIndex="13" placeholder="Search A/C Name" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtVoucherAmount" runat="server" ClientIDMode="Static" Width="80" TabIndex="15" MaxLength="15" />
                                                                    </td>
                                                                    <td class="text-center">
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="20" Height="20" TabIndex="17" />
                                                                    </td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                <div class="col m12">
                    <button id="btnReset" type="button" runat="server" clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" tabindex="19"><i class="material-icons left">backspace</i>Clear & Add New</button>
                    <button id="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="20"><i class="material-icons left">save</i>Save</button>
                </div>
            </div>

            <%--**************************** Customer Popup ***********************************--%>
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
                <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
                <br />
            </asp:Panel>

            <%-- *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-* --%>
            <script type="text/javascript">
                $("input").keypress(function (event) {
                    if (event.which == 13) {
                        event.preventDefault();
                        //$("form").submit();
                    }
                });

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
                                        $("#hdnCustomerIDNew").val(item.data('langname'));
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
                            $("#hdnCustomerID").val('');
                        }
                    }
                }

                function clearProductField() {
                    $("#hdnProductID").val('');
                    $("#txtProductName").val('');
                    $("#txtUnitPrice").val('');
                    $("#txtQuantity").val('');
                    $("#lblAmount").val('');
                    $("#txtProductName").focus();
                }

                $('#btnReset').focusout(function () {
                    $('#txtInquiryNo').focus();
                    //alert('reset button focusout invoke');
                });

                $('#btnCloseModel').focusout(function () {
                    closeSetTime();
                });
            </script>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
        </div>
    </form>

</body>
</html>
