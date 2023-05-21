<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherCharge.aspx.cs" Inherits="StarsProject.OtherCharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png"/>
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png"/>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //$('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            ////$('.datepicker').on('mousedown', function (event) { event.preventDefault(); })
            //$('.timepicker').timepicker();
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

        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }

        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        //function validateForm() {
        //    var errmsg = "";
        //    var validflag = true;
        //    // ----------------------------------------
        //    if (jQuery("#txtChargeName").val().trim() == "") {

        //        if (jQuery("#txtChargeName").val().trim() == "")
        //            errmsg += "<li>Charge Name is required</li>";

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    if (validflag == false) {
        //        showcaseError(errmsg);
        //    }
        //    return validflag;
        //}

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnPkIDEmp").val('');
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper" style="padding:20px 15px;">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <asp:HiddenField ID="hdnChargeID" runat="server" ClientIDMode="Static" />
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtChargeName">Charge Name <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtChargeName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" MaxLength="100" placeholder="" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="drpTaxType">Tax Type <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpTaxType" runat="server" class="select2-theme browser-default" TabIndex="2" placeholder="">
                                                <asp:ListItem Text="Inclusive" Value="0" />
                                                <asp:ListItem Text="Exclusive" Value="1" />
                                                <asp:ListItem Text="None" Value="2" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <label class="active" for="txtGSTPer">GST % <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtGSTPer" runat="server" ClientIDMode="Static" MaxLength="15" class="form-control" TabIndex="3" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtHSNCode">HSN Code</label>
                                            <asp:TextBox ID="txtHSNCode" runat="server" ClientIDMode="Static" MaxLength="50" class="form-control" TabIndex="4" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="chkBeforeGST">Calculate Charge</label>
                                            <div class = "switch" style="margin-top:15px;">
                                                <label>After GST
                                                <asp:CheckBox ID="chkBeforeGST" runat="server" ClientIDMode="Static" Checked="true" TabIndex="5"/>
                                                <span class="lever"></span>Before GST</label>
                                            </div> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:40px;">
                            <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="36"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="38"><i class="material-icons left">save</i>Save</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>

