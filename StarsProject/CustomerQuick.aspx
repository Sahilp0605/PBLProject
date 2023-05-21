<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerQuick.aspx.cs" Inherits="StarsProject.CustomerQuick" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
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
            $('.datepicker').datepicker({
                defaultDate: new Date((new Date()).getFullYear(), (new Date()).getMonth(), (new Date()).getDay()),
                format: "dd-mm-yyyy",
                minDate: new Date(1990,12,31)
            });
            $('.timepicker').timepicker();

            $('.tabs').tabs();
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
        // -------------------------------------------------------
        function openFormContainerNew() {
            clearFormData();
            // ------------------------------------------
            jQuery(".usermsg").empty();
            jQuery('#myModal').modal({ "show": true });
        }

        function openFormContainer() {

            jQuery('#myModal').modal({ "show": true });
        }

        function btnCloseDialog() {
            jQuery('.modal-backdrop').remove();
            jQuery('#myModal').modal('hide'); //or  $('#IDModal').modal('hide');
            jQuery('.modal-backdrop').remove();
        }

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');

            if (jQuery("#hdnPageMode").val() == 'supplier')
                jQuery("#drpCustomerType option[value='Supplier']").attr("selected", true);
            else
                jQuery("#drpCustomerType option[value='Customer']").attr("selected", true);
        }

        function setCustomerContact() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtCustomerName").val() == "" || jQuery("#drpOrgType").val() == "") {

                if (jQuery("#txtCustomerName").val() == "")
                    errmsg += "Customer Name is required</br>";

                if (jQuery("#drpOrgType").val() == "")
                    errmsg += "Organization Type is required</br>";

                jQuery("#divErrorMessage").html(errmsg).show();
                validflag = false;
            }
            // -----------------------------------------------
            if (validflag) {
                jQuery.ajax({
                    url: 'Services/NagrikService.asmx/setCustomerContacts',
                    method: 'post',
                    data: '{cust: ' + JSON.stringify(cust) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (resp) {
                        alert(resp.d);
                    },
                    error: function (err) { }
                });
                // -----------------------------------------------
                var cust = {};

                if (jQuery('#hdnCustomerID').val() == "" || jQuery('#hdnCustomerID').val() == "0")
                    cust.CustomerID = "0";
                else
                    cust.CustomerID = jQuery('#hdnCustomerID').val();

                cust.ContactPerson1 = jQuery('#txtContactPerson1').val();
                cust.ContactDesigCode1 = jQuery('#drpContactDesigCode1').val();
                cust.ContactNumber1 = jQuery('#txtContactNumber1').val();
                cust.ContactEmail1 = jQuery('#txtContactEmail1').val();
                cust.LoginUserID = 'admin';


                jQuery.ajax({
                    url: 'Services/NagrikService.asmx/setCustomerContacts',
                    method: 'post',
                    data: '{cust: ' + JSON.stringify(cust) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (resp) {
                        alert(resp.d);
                    },
                    error: function (err) { }
                });
            }
        }

        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        $(function () {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "custaddress";
            $("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "custaddress";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab").val(currTab);
                //alert("Curr.Tab Gen.Func : " + currTab)
            });
        });

        function retainTabPosition() {
            $('#lnkcustaddress').removeClass('active');
            $('#lnkcustprice').removeClass('active');
            $('#lnkcustcont').removeClass('active');

            $('#custaddress').css("display", "none");
            $('#custprice').css("display", "none");
            $('#lnkcustcont').css("display", "none");
            // ------------------------------------------------------------
            if ($("#hdnCurrentTab").val() == 'custaddress') {
                $('#lnkcustaddress').addClass('active');
                $('#custaddress').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'custprice') {
                $('#lnkcustprice').addClass('active');
                $('#custprice').css("display", "block");
            }
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
    <asp:HiddenField ID="hdnPageMode" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientERPIntegration" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnStateCode" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" Value="0" EnableViewState="true" />
    <asp:ScriptManager ID="srcUser" runat="server" EnableViewState="true" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">
        <%--<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <div class="widgetbox">

            <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />

            <div id="myModal" style="display:block; width:100%; padding:10px;">
                <div class="modal-content">
                    <div class="modal-body" style="min-height:300px;">
                        <div class="row">
                            <div class="input-field col m12">
                                <asp:TextBox ID="txtCustomerName" runat="server" class="form-control" ClientIDMode="Static" MaxLength="100" Placeholder="" TabIndex="1" />
                                <label id="lblCustomerName" for="txtCustomerName" class="active">Customer/Company Name <span class="materialize-red-text font-weight-800">*</span></label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m6">
                                <asp:DropDownList ID="drpCustomerType" runat="server" ClientIDMode="Static" class="select2-theme browser-default" MaxLength="50" TabIndex="2"></asp:DropDownList>
                                <label class="active" for="drpCustomerType">Category</label>
                            </div>
                            <div class="input-field col m6">
                                <asp:DropDownList ID="drpCustomerSource" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="3"></asp:DropDownList>
                                <label class="active" for="drpCustomerSource">Source</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m6">
                                <asp:TextBox ID="txtContactNo1" class="form-control" runat="server" ClientIDMode="Static" Placeholder="" MaxLength="15" TabIndex="6" />
                                <label class="active" for="txtContactNo1">Contact #1 <span class="materialize-red-text font-weight-800">*</span></label>
                            </div>
                            <div class="input-field col m6">
                                <asp:TextBox ID="txtContactNo2" runat="server" TabIndex="7" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="15" />
                                <label class="active" for="txtContactNo2">Contact #2</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m12">
                                <label class="active" for="txtEmailAddress">Email Address</label>
                                <asp:TextBox ID="txtEmailAddress" class="form-control" runat="server" ClientIDMode="Static"  MaxLength="50" Placeholder="" TabIndex="8"  />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col m12 pr-0">
                                <div class="card small" style="height:350px;">
                                    <div class="card-title blue white-text" style="padding:10px;">
                                        <p class="no-margin">Registered Office Address</p>
                                    </div>
                                    <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                        <div class="row" style="width:100%">
                                            <div class="input-field col m12">
                                                <asp:TextBox ID="txtAddress" runat="server" class="form-control" Placeholder="" TabIndex="15" MaxLength="100"  ClientIDMode="Static" />
                                                <label class="active" for="txtAddress">Address</label>
                                            </div>
                                        </div>
                                        <div class="row" style="width:100%">
                                            <div class="input-field col m4">
                                                <asp:TextBox ID="txtArea" runat="server" TabIndex="16" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                <label class="active" for="txtArea">Area</label>
                                            </div>
                                            <div class="input-field col m4">
                                                <asp:DropDownList ID="drpCountry" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="17" AutoPostBack="true"  OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"/>
                                                <label class="active" for="drpCountry">Country</label>
                                            </div>
                                            <div class="input-field col m4">
                                                <asp:DropDownList ID="drpState" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="17" AutoPostBack="true"  OnSelectedIndexChanged="drpState_SelectedIndexChanged"/>
                                                <label class="active" for="drpState">State</label>
                                            </div>
                                        </div>
                                        <div class="row" style="width:100%">
                                            <div class="input-field col m6">
                                                <asp:DropDownList ID="drpCity" class="select2-theme browser-default" runat="server"  ClientIDMode="Static" TabIndex="18" />
                                                <label class="active" for="drpCity">City</label>
                                            </div>
                                            <div class="input-field col m6">
                                                <asp:TextBox ID="txtPincode" runat="server" TabIndex="19" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="18"/>
                                                <label class="active" for="txtPincode">Pincode</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                        <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="34"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSaveEmail" type="button" runat="server" clientidmode="Static" Text="Save & Send Email" class="btn cyan right mr-1" onserverclick="btnSaveEmail_Click" TabIndex="35"><i class="material-icons left">email</i>Save - Send Email</button>
                        <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="36"><i class="material-icons left">save</i>Save</button>
                    </div>
                </div>
            </div>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    </form>
</body>
</html>