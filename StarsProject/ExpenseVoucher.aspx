<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpenseVoucher.aspx.cs" Inherits="StarsProject.ExpenseVoucher" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %> 

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
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    

    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

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
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
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

        function ShowPDFfile() {
            repFilename = jQuery('#hdnFileName').val();            
            //repFilename = "EmployeeDocs\\Exp-11CrystalReportViewer1 (8).pdf";            
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
            <div class="clearall"></div>

            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnDBCustomerID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCRCustomerID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnFileName" runat="server" ClientIDMode="Static" />

            <div id="myModal" style="display: block; width: 100%;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="input-field col m2">
                                <label class="active" for="txtVoucherDate">Voucher Date <span class="materialize-red-text font-weight-800">*</span></label>
                                <asp:TextBox ID="txtVoucherDate" runat="server" class="form-control" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" TabIndex="1" placeholder="" TextMode="Date" />
                            </div>
                            <div class="input-field col m2">
                                <label class="active" for="txtVoucherNo">Voucher No</label>
                                <asp:TextBox ID="txtVoucherNo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="2" ReadOnly="true" placeholder="" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col m6" style="background-color: #efe6e6; padding: 15px; border: 2px solid silver; border-radius: 5px;">
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtCRCustomerName">Voucher A/c<small class="text-muted font-italic">&nbsp;(Min 3 chars to search)</small> <span class="materialize-red-text font-weight-800">*</span>

                                            <a href="javascript:openCustomerInfo('view');">
                                                <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" Tabindex="3" />
                                            </a>
                                            <a href="javascript:openCustomerInfo('add');">
                                                <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                            </a>

                                        </label>
                                        <asp:TextBox ID="txtCRCustomerName" runat="server" ClientIDMode="Static" class="form-control" placeholder="" AutoPostBack="true" onKeyup="bindDDLToCredit('#txtCRCustomerName')" TabIndex="6" />
                                    </div>
                                    <div class="input-field col m6">
                                        <label class="active" for="txtDBCustomerName">Expn. A/c <small class="text-muted font-italic">&nbsp;(Min 3 chars to search)</small> <span class="materialize-red-text font-weight-800">*</span>

                                            <a href="javascript:openCustomerInfo('view');">
                                                <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" Tabindex="3" />
                                            </a>
                                            <a href="javascript:openCustomerInfo('add');">
                                                <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                            </a>

                                        </label>
                                        <asp:TextBox ID="txtDBCustomerName" runat="server" ClientIDMode="Static" class="form-control" placeholder="" AutoPostBack="true" onKeyup="bindDDLToDebit('#txtDBCustomerName')" TabIndex="7" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtVoucherAmount">Voucher Amount</label>
                                        <asp:TextBox ID="txtVoucherAmount" runat="server" class="form-control" MaxLength="15" ClientIDMode="Static" TabIndex="8" placeholder="" />
                                    </div>
                                </div>
                            </div>
                            <div class="col m6" style="background-color: #efe6e6; padding: 15px; border: 2px solid silver; border-radius: 5px;">
                                <div class="row mt-1 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                    <div class="input-field col m12">
                                        <label class="active" for="txtRemarks">Transaction Notes <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtRemarks" runat="server" class="textarea" ClientIDMode="Static" TabIndex="19" MaxLength="250" TextMode="MultiLine" Rows="3" placeholder="" />
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <div class="row">
                            <div id="empdocs" class="row" runat="server" clientidmode="Static" enableviewstate="true">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <div class="section app-file-manager-wrapper">
                                <div class="app-file-overlay"></div>
                                <div class="row">
                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                        <div class="app-file-header">
                                            <div class="row">
                                                <div class="input-field col m12 add-new-file mt-0">
                                                    <div class="col m12">
                                                        <label class="active" for="uploadDocument">Upload Document Here  </label>
                                                    </div>
                                                    <div class="col m12">
                                                        <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                        <asp:Button ID="btnUploadDoc" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="app-file-content">
                                            <div class="app-file-sidebar-content row">
                                                <div class="col m12 app-file-files">
                                                    <%--<asp:Repeater ID="rptEmpDocs" runat="server" ClientIDMode="Static" OnItemCommand="rptEmpDocs_ItemCommand">
                                                    <ItemTemplate>--%>
                                                        <div class="col xl4 l6 m4 s6">
                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                    <div class="row fonticon p2">
                                                                        <%--<div class="col m6" style="text-align:left;">
                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                        </div>--%>
                                                                        <div class="col m6" style="text-align:left;">
                                                                            <a id="lnkViewFile" onclick="ShowPDFfile();">
                                                                            <i class="material-icons">find_in_page</i>
                                                                            </a>
                                                                        </div>
                                                                    </div>
                                                                    <asp:Image ID="imgEmployee" class="recent-file" runat="server" ClientIDMode="Static" Width="30" Height="30" ImageUrl="app-assets/images/icon/pdf.png" style="display: block; margin: 5px auto;" />
                                                                    <%--<img id="Document" class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">--%>
                                                                </div>
                                                                <div class="app-file-details">
                                                                    <div class="app-file-name font-weight-700">
                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(6) %></p>
                                                                    </div>
                                                                </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    <%--</ItemTemplate>
                                                    </asp:Repeater>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                </Triggers>
                                </asp:UpdatePanel>
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
            function bindDDLToDebit(selector) {
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
                                    $("#hdnDBCustomerID").val(item.data('langname'));
                                    $("#txtDBCustomerName").focus();
                                }
                            });

                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                    return false;
                }
            }
            function bindDDLToCredit(selector) {
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
                                    $("#hdnCRCustomerID").val(item.data('langname'));
                                    $("#txtCRCustomerName").focus();
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