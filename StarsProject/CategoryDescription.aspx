<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryDescription.aspx.cs" Inherits="StarsProject.CategoryDescription" %>

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
        //function validateForm() {
        //    var errmsg = "";
        //    var validflag = true;
        //    // ----------------------------------------
        //    if (jQuery("#txtInquiryStatus").val() == "") {

        //        if (jQuery("#txtInquiryStatus").val() == "")
        //            errmsg += "<li>Inquiry Status is required</li>";

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    if (validflag == false) {
        //        showcaseError(errmsg);
        //    }
        //    return validflag;
        //}

    </script>
    <style type="text/css">
        .required:after {
            content: " *";
            color: #f00;
        }
    </style>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper" style="padding:20px 15px;">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:HiddenField ID="hdnpkid" runat="server" />
                    <div id="myModal" style="display: block; width: 100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="input-field col m6">
                                        <asp:DropDownList ID="drpCategory" runat="server" class="select2-theme browser-default" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="drpCategory_SelectedIndexChanged">
                                            <asp:ListItem Text="---Select---" Value="" />
                                            <asp:ListItem Text="Equipment Location" Value="EquipmentLocation" />
                                            <asp:ListItem Text="System Availablity" Value="SystemAvailablity" />
                                            <asp:ListItem Text="Required Engineering" Value="RequiredEngineering" />
                                        </asp:DropDownList>
                                        <label class="active" for="txtCategory">Category</label>
                                    </div>
                                    <div class="input-field col m6">
                                        <asp:TextBox ID="txtDescription" runat="server" class="form-control" TabIndex="2" MaxLength="100" placeholder=""></asp:TextBox>
                                        <label type="label" ID="lblDescription" runat="server" clientidmode="Static" class="active" for="txtDescription">Description of Category <span class="materialize-red-text font-weight-800">*</span></label>
                                    </div>
                            </div>
                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:40px;">
                        <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="36"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="38"><i class="material-icons left">save</i>Save</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>