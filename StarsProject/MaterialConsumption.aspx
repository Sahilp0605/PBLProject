<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialConsumption.aspx.cs" Inherits="StarsProject.MaterialConsuption" %>
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
    <script type="text/javascript" src="app-assets/js/plugins.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    <style type="text/css">
        table.responsive-table th {
            background-color: #6868d6 !important;
            color: white !important;
        }

        table.responsive-table th, table.responsive-table td {
            border-bottom: 1px solid silver !important;
        }

        #toast-container {
            background-color: aliceblue;
            min-width: 10%;
            top: 50%;
            right: 50%;
            transform: translateX(50%) translateY(50%);
            color: yellow;
            border: 1px solid blue !important;
            border-radius: 8px !important;
        }

        input[type=text], input[type=password], input[type=email], input[type=url], input[type=number], textarea :focus {
            border-color: #3C90BE;
        }

        #ulToast li::before {
            content: "\2022"; /* Add content: \2022 is the CSS Code/unicode for a bullet */
            color: white; /* Change the color */
            font-weight: bold; /* If you want it to be bold */
            display: inline-block; /* Needed to add space between the bullet and the text */
            width: 1em; /* Also needed for space (tweak if needed) */
            margin-left: -1em; /* Also needed for space (tweak if needed) */
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
            $('.timepicker').timepicker();
            $('.datepicker').datepicker();
            $(function () {
                $(".tabs a").click(function () {
                    $("#hdnCurrTab").val($(this).attr("href").replace("#", ""));
                });
            });
            retainTabPosition();
            getAMCType();
        }

        function getAMCType() {
            $('#drpContractType, #txtStartDate').bind('change', function () {

                var date = new Date($('#txtStartDate').val());

                if ($('#drpContractType').val().toLowerCase() == '6 months') {
                    date.setMonth(date.getMonth() + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '1 year') {
                    date.setMonth(date.getMonth() + 11 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '2 years') {
                    date.setMonth(date.getMonth() + 22 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '3 years') {
                    date.setMonth(date.getMonth() + 33 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '4 years') {
                    date.setMonth(date.getMonth() + 44 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '5 years') {
                    date.setMonth(date.getMonth() + 55 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '10 years') {
                    date.setMonth(date.getMonth() + 110 + parseInt($('#drpContractType').val()));
                }

                $("#txtEndDate").val(date.toISOString().slice(0, 10));

            }).trigger('change');
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

        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }


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

        function retainTabPosition() {

            var currTab = $("#hdnCurrTab").val();

            $("#lnkProducts").removeClass("active");
            $("#lnkTNC").removeClass("active");

            $("#pnl_Products").css("display", "none");
            $("#pnl_TNC").css("display", "none");

            if (currTab == 'pnl_Products') {
                $("#lnkProducts").addClass("active");
                $("#pnl_Products").css("display", "block");
            }
            //else if (currTab == 'pnl_TNC') {
            //    $("#lnkTNC").addClass("active");
            //    $("#pnl_TNC").css("display", "block");
            //}
        }
    </script>

</head>
<body class="loginpage" style="background-color: transparent; overflow-x: hidden;">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <%-- Bootstrap CONTRACT INFO Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnOrgCodeEmp" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustEmailAddress" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnDesignation" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnBlockCustomer" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCurrTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Products" />
                <div id="myModal" style="display: block; width: 100%;">
                    <div class="modal-content border-0">
                        <div class="modal-body col m12">
                            <div class="row">
                                <div class="col m5" style="border-right: 3px ridge silver;">
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtConsCode">Movement Code #</label>
                                            <asp:TextBox ID="txtConsCode" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtCDate">Consuption Date <span class="materialize-red-text font-weight-800">*</span> </label>
                                            <asp:TextBox ID="txtCDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" onkeypress="return false;" placeholder="" TextMode="Date" />
                                        </div>
                                        
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <asp:HiddenField ID="hdnCustStateID" runat="server" ClientIDMode="Static" />
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" placeholder="" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="3" />
                                            <label class="active" for="txtCustomerName">
                                                Customer Name&nbsp;<small class="text-muted font-italic">(Minimum 3 chars to activate search)</small> <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" tabindex="3" />
                                                </a>
                                                <a href="javascript:openCustomerInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                </a>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="drpOrderNo">Order No.<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpOrderNo" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="4" />
                                        </div>
                                       <%-- <div class="input-field col m6">
                                            <label class="active" for="drpContact">Contact #</label>
                                            <asp:DropDownList ID="drpContact" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="5" />
                                        </div>--%>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" />
                                            <asp:DropDownList ID="drpEmployee" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="4" />                                            <label class="active" for="txtEmployeeName">
                                                Employee Name&nbsp;<span class="materialize-red-text font-weight-800">*</span>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col m7">
                                    <div class="row">
                                        <div class="col s12">
                                            <ul id="myTab" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color: antiquewhite;">
                                                <li class="tab col m12 p-0"><a id="lnkProducts" class="left active" href="#pnl_Products">Product Detail</a></li>
                                               
                                            </ul>
                                        </div>
                                        <div id="pnl_Products" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                            <asp:UpdatePanel ID="upRepeater" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" ID="pnlDetail">
                                                        
                                                        <table id="tblInqProductGroup" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <asp:Repeater ID="rptContractInfoProductGroup" runat="server"  OnItemDataBound="rptContractInfoProductGroup_ItemDataBound" OnItemCommand="rptContractInfoProductGroup_ItemCommand">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-left">Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                                <a href="javascript:openProductInfo('add');">
                                                                                    <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" tabindex="7" />
                                                                                </a>
                                                                            </th>
                                                                            <th class="text-right">Quantity <span class="materialize-red-text font-weight-800">*</span></th>
                                                                            <th class="text-center">Action</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    
                                                                        <asp:HiddenField ID="hdnProductID_Grid" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <td class="text-left"><%# Eval("ProductName") %></td>
                                                                        <td id="tdQuantity" class="text-right"><%# Eval("Quantity") %></td>
                                                                        <td class="text-center">
                                                                            <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <tr style="padding: 5px;">
                                                                        <td>
                                                                            <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                            <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="8" placeholder="Search Product" />
                                                                        </td>
                                                                        
                                                                        <td class="text-center">
                                                                            <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" Width="80" TabIndex="10" MaxLength="15" />
                                                                        </td>
                                                                        
                                                                        <td class="text-center">
                                                                            <asp:ImageButton ID="imgBtnSave" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="20" Height="20" TabIndex="12" />
                                                                             
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
                                        <%--<div id="pnl_TNC" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <label class="active" for="drpTNC">Select Terms & Conditions </label>
                                                    <asp:DropDownList ID="drpTNC" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="14" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="drpTNC_SelectedIndexChanged" />
                                                </div>
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtContractFooter">Terms & Condition</label>
                                                    <asp:TextBox ID="txtContractFooter" MaxLength="500" runat="server" class="form-control" ClientIDMode="Static" TabIndex="15" TextMode="MultiLine" Rows="14" Height="100px" placeholder="" />
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                <div class="col m12">
                    <button id="btnReset" type="button" runat="server" clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" tabindex="19"><i class="material-icons left">backspace</i>Clear & Add New</button>
                    <button id="btnSaveEmail" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSaveEmail_Click" tabindex="20"><i class="material-icons left">email</i>Save - Send Email</button>
                    <button id="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="21"><i class="material-icons left">save</i>Save</button>
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
                                        $("#hdnCustomerID").val(item.data('langname'));
                                        $("#hdnBlockCustomer").val(item.data('BlockCustomer'));
                                        $("#drpInquiryStatus").focus();
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

                function bindDDLProductTo(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterProduct",
                            data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\'}',
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
        </div>
    </form>
</body>
</html>
