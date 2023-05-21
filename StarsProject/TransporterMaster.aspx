<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransporterMaster.aspx.cs" Inherits="StarsProject.TransporterMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
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
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
            $('.timepicker').timepicker();
        });
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', displayLength: 4000 });
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
        function btnCloseDialog() {
            jQuery('.modal-backdrop').remove();
            jQuery('#myModal').modal('hide'); //or  $('#IDModal').modal('hide');
            jQuery('.modal-backdrop').remove();
        }

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnPkIDEmp").val('');
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmTransporterMaster" runat="server" autocomplete="off">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <div id="myModal" style="display: block; width: 100%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="input-field col m4">
                                    <asp:TextBox ID="txtRegistrationNo" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " TabIndex="1" MaxLength="50" />
                                    <label class="active" for="txtRegistrationNo">Registration Number <span class="materialize-red-text font-weight-800">*</span></label>
                                </div>
                                <div class="input-field col m4">
                                    <asp:TextBox ID="txtOwnerName" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " TabIndex="2" MaxLength="50" />
                                    <label class="active" for="txtOwnerName">Owner Name <span class="materialize-red-text font-weight-800">*</span></label>
                                </div>
                                <div class="input-field col m2">
                                    <asp:TextBox ID="txtRatePerKM" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " TabIndex="3" MaxLength="50" />
                                    <label class="active" for="txtRatePerKM">Rate Per Kms</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m5">
                                    <div class="card small" style="height:380px;">
                                        <div class="card-title blue white-text" style="padding:10px;">
                                            <p class="no-margin">Vehicle Detail</p>
                                        </div>
                                        <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <asp:TextBox ID="txtChasisNo" runat="server" class="form-control" Placeholder="" TabIndex="4" MaxLength="500"  ClientIDMode="Static" />
                                                    <label class="active" for="txtChasisNo">Chasis #</label>
                                                </div>
                                                <div class="input-field col m6">
                                                    <asp:TextBox ID="txtMfg" runat="server" TabIndex="5" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                    <label class="active" for="txtMfg">Mfg.Company</label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <asp:TextBox ID="txtModel" runat="server" class="form-control" PlaceHolder="" ClientIDMode="Static" TabIndex="6" />
                                                    <label class="active" for="txtModel">Model</label>
                                                </div>
                                                <div class="input-field col m6">
                                                    <asp:TextBox ID="txtColor" runat="server" class="form-control" Placeholder="" TabIndex="7" MaxLength="500"  ClientIDMode="Static" />
                                                    <label class="active" for="txtColor">Color</label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m4">
                                                    <asp:TextBox ID="txtVehicleType" runat="server" TabIndex="8" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                    <label class="active" for="txtVehicleType">Vehile Type</label>
                                                </div>
                                                <div class="input-field col m4">
                                                    <asp:TextBox ID="txtMfgYear" runat="server" class="form-control" PlaceHolder="" ClientIDMode="Static" TabIndex="9" />
                                                    <label class="active" for="txtMfgYear">Mfg.Year</label>
                                                </div>
                                                <div class="input-field col m4">
                                                    <asp:TextBox ID="txtEngineCC" runat="server" class="form-control" PlaceHolder="" ClientIDMode="Static" TabIndex="10" />
                                                    <label class="active" for="txtEngineCC">Engine CC</label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtGrossWeight">Gross Weight (R.L.W)</label>
                                                    <asp:TextBox ID="txtGrossWeight" runat="server" class="form-control" ClientIDMode="Static" TabIndex="60" placeholder="" MaxLength="15" />
                                                </div>
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtTareWeight">Tare Weight (U.L.W)</label>
                                                    <asp:TextBox ID="txtTareWeight" runat="server" class="form-control" ClientIDMode="Static" TabIndex="61" placeholder="" MaxLength="15" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtNetWeight">Net Weight (Capacity)</label>
                                                    <asp:TextBox ID="txtNetWeight" runat="server" class="form-control" ClientIDMode="Static" TabIndex="62" placeholder="" MaxLength="15" />
                                                </div>
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtLicenseNo">Explosive License Number</label>
                                                    <asp:TextBox ID="txtLicenseNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="63" placeholder="" MaxLength="100" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="input-field col m4">
                                    <div class="card small" style="height:380px;">
                                        <div class="card-title blue white-text" style="padding:10px;">
                                            <p class="no-margin">Registered Address</p>
                                        </div>
                                        <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtOwnerAddress" runat="server" class="form-control" Placeholder="" TabIndex="11" MaxLength="500"  ClientIDMode="Static" />
                                                    <label class="active" for="txtOwnerAddress">Address</label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <asp:TextBox ID="txtOwnerMobile" runat="server" TabIndex="12" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                    <label class="active" for="txtOwnerMobile">Mobile No</label>
                                                </div>
                                                <div class="input-field col m6">
                                                    <asp:TextBox ID="txtOwnerLandline" runat="server" TabIndex="13" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                    <label class="active" for="txtOwnerLandline">Landline</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="input-field col m3">
                                    <div class="card small" style="height:380px;">
                                        <div class="card-title blue white-text" style="padding:10px;">
                                            <p class="no-margin">Insurance Detail</p>
                                        </div>
                                        <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtInsuranceCompany" runat="server" class="form-control" Placeholder="" TabIndex="14" MaxLength="500"  ClientIDMode="Static" />
                                                    <label class="active" for="txtInsuranceCompany">Company Name</label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtInsurancePolicyNo" runat="server" TabIndex="15" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                    <label class="active" for="txtInsurancePolicyNo">Policy #</label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtInsuranceExpiry" runat="server" class="form-control" PlaceHolder="From Date" ClientIDMode="Static" TabIndex="16" onkeypress="return false;" onpaste="return false" TextMode="Date" />
                                                    <label class="active" for="txtInsuranceExpiry">Insurance Expiry Date </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="input-field col m12">
                                    <div class="card small" style="height:210px;">
                                        <div class="card-title blue white-text" style="padding:10px;">
                                            <p class="no-margin">Trip Management</p>
                                        </div>
                                        <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                            <div class="row">
                                                <div class="input-field col m3">
                                                    <asp:TextBox ID="txtTripDate" runat="server" class="form-control" PlaceHolder="From Date" ClientIDMode="Static" Width="250px" TabIndex="1" onkeypress="return false;" onpaste="return false" TextMode="Date" />
                                                    <label class="active" for="txtTripDate">Trip Date </label>
                                                </div>
                                                <%--<div class="input-field col m3">
                                                    <asp:DropDownList ID="drpVehicle" runat="server" ClientIDMode="Static" TabIndex="11" class="select2-theme browser-default"/>
                                                    <label class="active" for="drpVehicle">Vehicle <span class="materialize-red-text font-weight-800">*</span></label>
                                                </div>--%>
                                                <div class="input-field col m3">
                                                    <asp:TextBox ID="txtDriverName" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " TabIndex="3" MaxLength="50" />
                                                    <label class="active" for="txtDriverName">Driver Name </label>
                                                </div>
                                            <%--<div class="input-field col m3">
                                                <asp:DropDownList ID="drpDriverName" runat="server" ClientIDMode="Static" class="select2-theme browser-default" PlaceHolder=" " TabIndex="3" />
                                                <label class="active" for="txtDriverName">Driver Name <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>--%>
                                        </div>
                                    </div>
                                    <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                        <div class="row">
                                            <div class="input-field col m3">
                                                <asp:TextBox ID="txtFromStation" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " TabIndex="4" MaxLength="50" />
                                                <label class="active" for="txtFromStation">From Location </label>
                                            </div>
                                            <div class="input-field col m3">
                                                <asp:TextBox ID="txtToStation" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " TabIndex="5" MaxLength="50" />
                                                <label class="active" for="txtToStation">To Location </label>
                                            </div>
                                            <div class="input-field col m3">
                                                <asp:TextBox ID="txtKilometers" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " TabIndex="8"  />
                                                <label class="active" for="txtKilometers">Kilometers </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                    <button ID="btnReset" type="button" runat="server" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="17"><i class="material-icons left">backspace</i>Clear & Add New</button>
                    <button ID="btnSave" type="button" runat="server" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="18"><i class="material-icons left">save</i>Save</button>
                </div>
            </div>
        </div>


        <%--**************************** Customer Popup ***********************************--%>

        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
            <div id="myModal1215">
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
                                    $("#drpInquiry").focus();
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

        <div id="myNotificationModal" class="modal" style="width: 25% !important;">
            <div class="modal-content">
                <h4>Send Notification</h4>
                <hr />
                <div class="row mt-3">
                    <div class="col m12 p-0">
                        <span>Send Email Notfication</span>
                        <div class="switch left">
                            <label>
                                <input id="chkSendEmail" type="checkbox" />
                                <span class="lever"></span>
                            </label>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col m12 p-0">
                        <span>Send SMS Notification</span>
                        <div class="switch left">
                            <label>
                                <input id="chkSendSMS" type="checkbox" />
                                <span class="lever"></span>
                            </label>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <%--<div class="modal-footer">--%>
                <%--<a href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">Close</a>--%>
                <%--<button id="btnFSaveEmail" type="button" runat="server" clientidmode="Static" text="Save" class="modal-action modal-close waves-effect waves-green btn-flat right" onserverclick="btnFSaveEmail_Click" tabindex="12"><i class="material-icons left">email</i>Save & Send Notification</button>--%>
           <%-- </div>--%>
        </div>

        <script type="text/javascript">
            $("#btnNotification").click(function () {
                const elem = document.getElementById('myNotificationModal');
                const instance = M.Modal.init(elem, { dismissible: false });
                instance.open();
            });
            $("#chkSendEmail").change(function () {
                $("#hdnSendEmail").val($("#chkSendEmail").is(':checked'));
            });
            $("#chkSendSMS").change(function () {
                $("#hdnSendSMS").val($("#chkSendSMS").is(':checked'));
            });
        </script>
    </form>
</body>
</html>
