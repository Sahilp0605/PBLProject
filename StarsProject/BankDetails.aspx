<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankDetails.aspx.cs" Inherits="StarsProject.BankDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link  rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>
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
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', displayLength:4000 });
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
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        //function validateForm() {
        //    var errmsg = "";
        //    var validflag = true;
        //    // ----------------------------------------
        //    if (jQuery("#txtMessage").val() == "" || jQuery("#txtFromDate").val() == "" || jQuery("#txtToDate").val() == "") {

        //        if (jQuery("#txtMessage").val() == "")
        //            errmsg += "<li>Message is required</li>";

        //        if (jQuery("#txtFromDate").val() == "")
        //            errmsg += "<li>From Date is required</li>";

        //        if (jQuery("#txtToDate").val() == "")
        //            errmsg += "<li>To Date is required</li>";

        //        if (Date.parse($("#txtFromDate").val()) > Date.parse($("#txtToDate").val()))
        //            errmsg += "<li>From Date is Always Less then To Date</li>";

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    if (validflag == false)
        //        showcaseError(errmsg);
        //    // ----------------------------------------
        //    return validflag;
        //}

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
        <div id="contentwrapper" class="contentwrapper">
            <%--<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
                    <div class="widgetbox">
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnCompanyID" runat="server" ClientIDMode="Static" />
                        <div class="input-field col m4">
                                            <div class="card" style="min-height: 370px;">
                                                <div class="card-title blue" style="padding: 10px;">
                                                    <h5 class="card-title mb-0" style="color: white;">Bank Information</h5>
                                                </div>
                                                <div class="card-body" style="padding: 20px 20px 0px 10px;"">
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtBankName">Bank Name</label>
                                                            <asp:TextBox ID="txtBankName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" placeholder="" />
                                                        </div>
                                                         <div class="input-field col m12">
                                                            <label class="active" for="txtAccountName">Bank Account Name</label>
                                                            <asp:TextBox ID="txtAccountName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtBranchName">Branch Name</label>
                                                            <asp:TextBox ID="txtBranchName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="2" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtBankAccountNo">Bank Account #</label>
                                                            <asp:TextBox ID="txtBankAccountNo" runat="server" ClientIDMode="Static" class="form-control" TabIndex="3" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtBankIFSC">Bank IFSC Code</label>
                                                            <asp:TextBox ID="txtBankIFSC" runat="server" ClientIDMode="Static" class="form-control" TabIndex="4" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtBankSWIFT">Bank SWIFT Code</label>
                                                            <asp:TextBox ID="txtBankSWIFT" runat="server" ClientIDMode="Static" class="form-control" TabIndex="5" placeholder="" />
                                                        </div>
                                                        
                                                        

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                            <button ID="btnReset" type="button" runat="server" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="6"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button ID="btnSave" type="button" runat="server" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="7"><i class="material-icons left">save</i>Save</button>
                        </div>
                    </div>
                <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </form>
</body>
</html>
