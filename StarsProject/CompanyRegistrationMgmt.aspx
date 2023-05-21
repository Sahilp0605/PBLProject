<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyRegistrationMgmt.aspx.cs" Inherits="StarsProject.CompanyRegistrationMgmt" %>

<!DOCTYPE html>

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
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtMessage").val() == "" || jQuery("#txtFromDate").val() == "" || jQuery("#txtToDate").val() == "") {

                if (jQuery("#txtMessage").val() == "")
                    errmsg += "Message is required</br>";

                if (jQuery("#txtFromDate").val() == "")
                    errmsg += "From Date is required</br>";

                if (jQuery("#txtToDate").val() == "")
                    errmsg += "To Date is required</br>";

                if (Date.parse($("#txtFromDate").val()) > Date.parse($("#txtToDate").val()))
                    errmsg += "From Date is Always Less then To Date</br>";


                jQuery("#divErrorMessage").html(errmsg).show();

                validflag = false;
            }
            // ----------------------------------------
            if (validflag == true) {
                btnCloseDialog();
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
<body>
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Manage Company Registration</h5>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtInstallationDate">Installation Date</label>
                                            <asp:TextBox ID="txtInstallationDate" runat="server" CssClass="form-control" ClientIDMode="Static" TabIndex="2" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m">
                                            <label class="active" for="txtCompanyName">Company Name</label>
                                            <asp:TextBox ID="txtCompanyName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" TextMode="SingleLine" MaxLength="50" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtNoOfUsers">No Of Users</label>
                                            <asp:TextBox ID="txtNoOfUsers" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtExpiryDate">Expiry Date</label>
                                            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" ClientIDMode="Static" TabIndex="2" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m">
                                            <label class="active" for="txtSerialKey">Serial Key</label>
                                            <asp:TextBox ID="txtSerialKey" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active" for="txtDBIP">Database IP</label>
                                            <asp:TextBox ID="txtDBIP" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" />
                                        </div>
                                        <div class="input-field col m">
                                            <label class="active" for="txtDBName">Database Name</label>
                                            <asp:TextBox ID="txtDBName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="5" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active" for="txtDBUserName">Database UserName</label>
                                            <asp:TextBox ID="txtDBUserName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" />
                                        </div>
                                        <div class="input-field col m">
                                            <label class="active" for="txtDBPassword">Database Password</label>
                                            <asp:TextBox ID="txtDBPassword" runat="server" class="form-control" ClientIDMode="Static" TabIndex="7" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active" for="txtDBUserName">Root Path</label>
                                            <asp:TextBox ID="txtRootPath" runat="server" class="form-control" ClientIDMode="Static" TabIndex="8" />
                                        </div>
                                        <div class="input-field col m">
                                            <label class="active" for="txtDBPassword">Site URL</label>
                                            <asp:TextBox ID="txtSiteURL" runat="server" class="form-control" ClientIDMode="Static" TabIndex="9" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <div class="card">
                                                <div class="card-header">
                                                    <h5 class="card-title mb-0">Primary Account Detail</h5>
                                                </div>
                                                <div class="card-body">
                                                    <div class="row">
                                                        <label class="active" for="txtIndiaMartKey">IndiaMart Key</label>
                                                        <asp:TextBox ID="txtIndiaMartKey" runat="server" class="form-control" ClientIDMode="Static" TabIndex="10" />
                                                    </div>
                                                    <div class="row">
                                                        <label class="active" for="txtIndiaMartMobile">IndiaMart Mobile</label>
                                                        <asp:TextBox ID="txtIndiaMartMobile" runat="server" class="form-control" ClientIDMode="Static" TabIndex="11" />
                                                    </div>
                                                    <div class="row">
                                                        <label class="active" for="txtIndiaMartAcAlias">IndiaMart Account Alias</label>
                                                        <asp:TextBox ID="txtIndiaMartAcAlias" runat="server" class="form-control" ClientIDMode="Static" TabIndex="11" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="input-field col m6">
                                            <div class="card">
                                                <div class="card-header">
                                                    <h5 class="card-title mb-0">Secondary Account Detail</h5>
                                                </div>
                                                <div class="card-body">
                                                    <div class="row">
                                                        <label class="active" for="txtIndiaMartKey2">IndiaMart Key</label>
                                                        <asp:TextBox ID="txtIndiaMartKey2" runat="server" class="form-control" ClientIDMode="Static" TabIndex="10" />
                                                    </div>
                                                    <div class="row">
                                                        <label class="active" for="txtIndiaMartMobile2">IndiaMart Mobile</label>
                                                        <asp:TextBox ID="txtIndiaMartMobile2" runat="server" class="form-control" ClientIDMode="Static" TabIndex="11" />
                                                    </div>
                                                    <div class="row">
                                                        <label class="active" for="txtIndiaMartAcAlias2">IndiaMart Account Alias</label>
                                                        <asp:TextBox ID="txtIndiaMartAcAlias2" runat="server" class="form-control" ClientIDMode="Static" TabIndex="11" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" OnClientClick="return validateForm();" OnClick="btnSave_Click" TabIndex="12" />
                                    <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="13" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
