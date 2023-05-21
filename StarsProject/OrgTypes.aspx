<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="OrgTypes.aspx.cs" Inherits="StarsProject.OrgTypes" %>
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
            if (jQuery("#txtOrgType").val() == "") {

                if (jQuery("#txtOrgType").val() == "")
                    errmsg += "<li>OrgType is required</li>";

                validflag = false;
            }
            // ----------------------------------------
            if (validflag == true) {
                showcaseError(errmsg);
            }
            return validflag;
        }

    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper" style="padding:20px 15px;">
    
        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />

            <%-- Bootstrap Modal Popup --%>
            <div class="clearall"></div>
            <div id="myModal" style="display:block; width:100%;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="input-field col m8">
                                <asp:TextBox ID="txtOrgType" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " MaxLength="50" TabIndex="1"></asp:TextBox>
                                <label class="active" for="txtOrgType">Organization Type <span class="materialize-red-text font-weight-800">*</span></label>
                            </div>
                            <div class="input-field col m4">
                                <label class="active" for="chkBlock">Active Status</label>
                                <div class = "switch">
                                    <label>Inactive
                                    <asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" Checked="true" TabIndex="2"/>
                                    <span class="lever"></span>Active</label>
                                </div> 
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="36"><i class="material-icons left">backspace</i>Clear & Add New</button>
                <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="38"><i class="material-icons left">save</i>Save</button>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>