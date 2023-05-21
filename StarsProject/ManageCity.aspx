<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCity.aspx.cs" Inherits="StarsProject.ManageCity" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png" />
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
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
    <style type="text/css">
        .required:after {
            content: " *";
            color: #f00;
        }
    </style>
    <script type="text/javascript">

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
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtCityName").val().trim() == "" || jQuery("#drpState").val().trim() == "") {

                if (jQuery("#txtCityName").val().trim() == "")
                    errmsg += "City Name is required</br>";

                if (jQuery("#drpState").val().trim() == "")
                    errmsg += "State is required</br>";

                validflag = false;
            }
            // ----------------------------------------
            if (validflag == false) {
                showcaseError(errmsg);
            }
            return validflag;
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
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper" style="padding:20px 15px;">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <asp:HiddenField ID="hdnCityID" runat="server" ClientIDMode="Static" />
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <div class="row mb-1">
                                                <div class="col m12">
                                                    <h5 class="card-title padding-1 mb-2 left-align width-100 gradient-45deg-light-blue-indigo">Add City</h5>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <asp:DropDownList ID="drpCountry" runat="server" ClientIDMode="Static" TabIndex="1" class="select2-theme browser-default" Style="height: inherit;" AutoPostBack="true" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" />
                                                    <label class="active" for="drpCountry">Country</label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <asp:DropDownList ID="drpState" runat="server" ClientIDMode="Static" TabIndex="2" class="select2-theme browser-default" Style="height: inherit;" />
                                                    <label class="active" for="drpState">State <span class="materialize-red-text font-weight-800">*</span></label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtCityName" runat="server" ClientIDMode="Static" class="form-control" Placeholder="" TabIndex="3" MaxLength="1000" />
                                                    <label class="active" for="txtCityName">City Name <span class="materialize-red-text font-weight-800">*</span></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="input-field col m4">
                                            <div id="myModal11" style="display: block; width: 100%;">
                                                <div class="modal-content">
                                                    <div class="modal-body">
                                                        <div class="row mb-1">
                                                            <div class="col m12">
                                                                <h5 class="card-title padding-1 mb-2 left-align width-100 gradient-45deg-light-blue-indigo">Add Country</h5>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="input-field col m12">
                                                                <asp:TextBox ID="txtNewCountryCode" runat="server" ClientIDMode="Static" class="form-control" Placeholder="" TabIndex="5" MaxLength="20" />
                                                                <label class="active" for="txtNewCountryCode">Country Code<span class="materialize-red-text font-weight-800">*</span></label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m12">
                                                                <asp:TextBox ID="txtNewCountryISO" runat="server" ClientIDMode="Static" class="form-control" Placeholder="" TabIndex="6" MaxLength="10" />
                                                                <label class="active" for="txtNewCountryISO">Country ISO<span class="materialize-red-text font-weight-800">*</span></label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m12">
                                                                <asp:TextBox ID="txtNewCountry" runat="server" ClientIDMode="Static" class="form-control" Placeholder="" TabIndex="7" MaxLength="50" />
                                                                <label class="active" for="txtNewCountry">Country<span class="materialize-red-text font-weight-800">*</span></label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="input-field col m4">
                                            <div id="myModal12" style="display: block; width: 100%;">
                                                <div class="modal-content">
                                                    <div class="modal-body">
                                                        <div class="row mb-1">
                                                            <div class="col m12">
                                                                <h5 class="card-title padding-1 mb-2 left-align width-100 gradient-45deg-light-blue-indigo">Add State</h5>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m12">
                                                                <asp:DropDownList ID="drpStateCountry" runat="server" ClientIDMode="Static" TabIndex="9" class="select2-theme browser-default" Style="height: inherit;" />
                                                                <label class="active" for="drpStateCountry">Select Country<span class="materialize-red-text font-weight-800">*</span></label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m12">
                                                                <asp:TextBox ID="txtNewState" runat="server" ClientIDMode="Static" class="form-control" Placeholder="" TabIndex="10" MaxLength="50"  />
                                                                <label class="active" for="txtNewState">State Name <span class="materialize-red-text font-weight-800">*</span></label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m6">
                                                                <asp:TextBox ID="txtStateCode" runat="server" ClientIDMode="Static" class="form-control" Placeholder="" TabIndex="10" MaxLength="50" AutoPostBack="true" OnTextChanged="txtNewState_TextChanged"/>
                                                                <label class="active" for="txtStateCode" >State Code <span class="materialize-red-text font-weight-800">*</span></label>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom">
                                        <div class="col m4">
                                            <button ID="btnSave" type="button" runat="server" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="4"><i class="material-icons left">save</i>Add City</button>
                                        </div>
                                        <div class="col m4">
                                            <button ID="btnAddCountry" type="button" runat="server" Text="Save Country" class="btn cyan right mr-1" onserverclick="btnAddCountry_Click" TabIndex="8"><i class="material-icons left">save</i>Add Country</button>
                                        </div>
                                        <div class="col m4">
                                            <button ID="btnAddState" type="button" runat="server" Text="Save State" class="btn cyan right mr-1" onserverclick="btnAddState_Click" TabIndex="11"><i class="material-icons left">save</i>Add State</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                            <button ID="btnReset" type="button" runat="server" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="12"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
