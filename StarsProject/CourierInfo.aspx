<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourierInfo.aspx.cs" Inherits="StarsProject.CourierInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--- TESTIING --->
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <style type="text/css">
        /* ==================================== */
        /*****         Modal Popup        *****/
        /* ==================================== */

        .card-title {
            background: linear-gradient(45deg, #303f9f, #7b1fa2) !important;
        }

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
            modeOfSelection();
        });

        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }

        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

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

        function modeOfSelection() {
            $('#drpModeOfSelection').change(function () {
                if ($('#drpModeOfSelection').val().toLowerCase() == 'recipient') {
                    $('#txtSender').insertAfter($('#txtRecipient'));
                }
                else {
                    $('#txtRecipient').insertAfter($('#txtSender'));
                }
            }).trigger('change');
        }
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Customer";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=" + cat + "&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        function ShowPDFfile(repFilename) {
            repFilename = "otherimages/" + repFilename;
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnPostedFile" runat="server" ClientIDMode="Static" />

                <div id="myModal" style="display: block; width: 98%; overflow-x: hidden;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="card-title" style="padding: 10px;">
                                <h5 class="card-title mb-0" style="color: white">Courier Management</h5>
                            </div>
                            <br />
                            <div class="modal-body">
                                <div class="row badge-light" style="border-radius: 5px;">
                                    <div class="input-field col m2">
                                        <label class="active" for="txtSerialNo">Serial No. #</label>
                                        <asp:TextBox ID="txtSerialNo" runat="server" class="form-control" MaxLength="25" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder=" " />
                                    </div>
                                    <div class="input-field col m2">
                                        <label class="active" for="txtActivityDate">Activity Date <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtActivityDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" onpaste="return false" placeholder=" " TextMode="Date" />
                                    </div>
                                    <div class="input-field col m2">
                                        <label class="active" for="drpModeOfSelection">Mode Of Selection <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:DropDownList ID="drpModeOfSelection" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="3">
                                            <asp:ListItem Text="-- Select --" Value="-- Select --" />
                                            <asp:ListItem Text="Dispatch" Value="Dispatch" />
                                            <asp:ListItem Text="Receive" Value="Receive" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="input-field col m5" id="txtSender">
                                        <label class="active" for="txtCustomerName">
                                            Sender/Receipient Name<span class="materialize-red-text font-weight-800">*</span>
                                            <a href="javascript:openCustomerInfo('view');">
                                                <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" />
                                            </a>
                                            <a href="javascript:openCustomerInfo('add');">
                                                <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" style="display:inline-block;" />
                                            </a>
                                        </label>
                                        <asp:TextBox ID="txtCustomerName" runat="server" class="form-control" ClientIDMode="Static" MaxLength="100" TabIndex="4" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" placeholder="Min.3 Chars To Search..." />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m4">
                                        <div class="card" style="height: 325px;">
                                            <div class="card-title" style="padding: 10px;">
                                                <h5 class="card-title mb-0" style="color: white">Transporter Information</h5>
                                            </div>
                                            <div class="card-body">
                                                <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                    <div class="input-field col m6">
                                                        <label class="active" for="txtDocketNo">Docket / Tracking No <span class="materialize-red-text font-weight-800">*</span></label>
                                                        <asp:TextBox ID="txtDocketNo" runat="server" class="form-control" MaxLength="50" ClientIDMode="Static" TabIndex="5" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m6">
                                                        <label class="active" for="txtDocumentType">Type Of Parcel <span class="materialize-red-text font-weight-800">*</span></label>
                                                        <asp:TextBox ID="txtDocumentType" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="6" placeholder=" " />
                                                    </div>
                                                </div>
                                                <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                    <div class="input-field col m8">
                                                        <label class="active" for="txtCourierName">Courier Name</label>
                                                        <asp:TextBox ID="txtCourierName" runat="server" class="form-control" ClientIDMode="Static" MaxLength="100" TabIndex="7" placeholder=" " />
                                                    </div>
                                                </div>
                                                <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                    <div class="input-field col m6">
                                                        <label class="active" for="txtCourierContact">Courier Contact</label>
                                                        <asp:TextBox ID="txtCourierContact" runat="server" class="form-control" ClientIDMode="Static" MaxLength="15" TabIndex="8" placeholder=" " />
                                                    </div>
                                                    <div class="input-field col m6">
                                                        <label class="active" for="txtCourierEmail">Courier Email</label>
                                                        <asp:TextBox ID="txtCourierEmail" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="9" placeholder=" " />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="input-field col m8">
                                        <div class="card">
                                            <div class="card-title" style="padding: 10px;">
                                                <h5 class="card-title mb-0" style="color: white">Address Information</h5>
                                            </div>
                                            <div class="card-body">
                                                <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                    <div class="input-field col m12">
                                                        <label class="active" for="txtAddress">Address <span class="materialize-red-text font-weight-800">*</span></label>
                                                        <asp:TextBox ID="txtAddress" runat="server" Style="width: 100%;" class="form-control" ClientIDMode="Static" MaxLength="100" TextMode="MultiLine" TabIndex="10" placeholder=" " />
                                                    </div>
                                                </div>
                                                <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                    <div class="input-field col m4">
                                                        <label class="active" for="drpCountry">Country <span class="materialize-red-text font-weight-800">*</span></label>
                                                        <asp:DropDownList ID="drpCountry" runat="server" ClientIDMode="Static" TabIndex="11" class="select2-theme browser-default" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" AutoPostBack="true" />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="drpState">State <span class="materialize-red-text font-weight-800">*</span></label>
                                                        <asp:DropDownList ID="drpState" runat="server" ClientIDMode="Static" TabIndex="12" class="select2-theme browser-default" OnSelectedIndexChanged="drpState_SelectedIndexChanged" AutoPostBack="true" />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="drpCity">City <span class="materialize-red-text font-weight-800">*</span></label>
                                                        <asp:DropDownList ID="drpCity" runat="server" ClientIDMode="Static" TabIndex="13" class="select2-theme browser-default" placeholder="" />
                                                    </div>
                                                </div>
                                                <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtPinCode">Pincode </label>
                                                        <asp:TextBox ID="txtPinCode" runat="server" MaxLength="50" ClientIDMode="Static" class="form-control" TabIndex="14" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtContact">Contact No </label>
                                                        <asp:TextBox ID="txtContact" runat="server" MaxLength="20" ClientIDMode="Static" class="form-control" TabIndex="15" placeholder="" />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtEmail">Email </label>
                                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" ClientIDMode="Static" class="form-control" TabIndex="16" placeholder="" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m12">
                                        <div class="card" style="height: 325px;">
                                            <div class="card-title" style="padding: 10px;">
                                                <h5 class="card-title mb-0" style="color: white">Upload Images</h5>
                                            </div>
                                            <div class="card-body">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="col m5">
                                                                    <div class="input-field col m12 add-new-file mt-0">
                                                                        <div class="col m12">
                                                                            <label class="active" for="uploadDocument">Upload Product Image ...</label>
                                                                        </div>
                                                                        <div class="col m12">
                                                                            <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" TabIndex="17" /> 
                                                                            <asp:Button ID="Button3" CssClass="hide" runat="server" Text="Upload"  onclick="btnUpload1_Click" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptCourierImage" runat="server" ClientIDMode="Static" OnItemCommand="rptCourierImage_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6" style="text-align:left;">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6" style="text-align:right;">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("Name").ToString() %>');">
                                                                                            <i class="material-icons">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("Name").ToString().Substring(0) %></p>
                                                                                    </div>
                                                                                </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtRemarks">Remarks <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtRemarks" Style="width: 205%;" runat="server" class="form-control" ClientIDMode="Static" TextMode="MultiLine" MaxLength="250" TabIndex="18" placeholder=" " />
                                    </div>
                                </div>
                            </div>
                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 5px;">
                                <button id="btnReset" type="button" runat="server" clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="20"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button id="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="19"><i class="material-icons left">save</i>Save</button>
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
        </div>
        <script type="text/javascript">
            function bindDDLTo(selector) {
                if ($(selector).val().length >= 1) {
                    jQuery.ajax({
                        type: "POST",
                        url: "InquiryInfo.aspx/FilterCustomer",
                        data: '{pCustName:\'' + $(selector).val() + '\'}',
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
                                    return '<div class="autocomplete-suggestion" style="position:relative !important;" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                },
                                onSelect: function (e, term, item) {
                                    $(selector).val(item.html());
                                    $("#hdnCustomerID").val(item.data('langname'));
                                    //$("#drpInquiry").focus();
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
        </script>
    </form>
</body>
</html>
