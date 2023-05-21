<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Designations.aspx.cs" Inherits="StarsProject.Designations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
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


    <script type="text/javascript">
        $(document).ready(function () {
            //$('.datepicker').datepicker({ format: "dd/mm/yyyy" });
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
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtDesigCode").val() == "" || jQuery("#txtDesignation").val() == "") {

                if (jQuery("#txtDesigCode").val() == "")
                    errmsg += "<li>Designation alias is required</li>";

                if (jQuery("#txtDesignation").val() == "")
                    errmsg += "<li>Designation name is required</li>";

                validflag = false;
            }
            // ----------------------------------------
            if (validflag == false) {
                showcaseError(errmsg);
            }
            return validflag;
        }

    </script>

    <style type="text/css">
        .required:after {
            content: " *";
            color: #f00;
        }
    </style>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper" style="padding:20px 15px;">

            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:HiddenField ID="hdnDesigCode" runat="server" ClientIDMode="Static" />

                    <%-- Bootstrap Modal Popup --%>
                    <div class="clearall"></div>
                    <div id="myModal" style="display: block; width: 100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="input-field col m8">
                                        <asp:TextBox ID="txtDesignation" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" MaxLength="50" TabIndex="1"></asp:TextBox>
                                        <label class="active" for="txtDesignation">Designation <span class="materialize-red-text font-weight-800">*</span></label>
                                    </div>
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtDesigCode" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" MaxLength="20" TabIndex="2"></asp:TextBox>
                                        <label class="active" for="txtDesigCode">Designation Alias <span class="materialize-red-text font-weight-800">*</span></label>
                                    </div>
                                </div>
                                <div class="row mt-2">
                                    <div class="input-field col m6">
                                        <label class="active" for="chkBlock">Active Status</label>
                                        <div class = "switch" style="margin-bottom:15px;">
                                            <label>Inactive
                                            <asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" Checked="true" TabIndex="3"/>
                                            <span class="lever"></span>Active</label>
                                        </div> 
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:15px;"">
                        <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="4"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="5"><i class="material-icons left">save</i>Save</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
