<%@ Page Title="" Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UserMgmt.aspx.cs" Inherits="StarsProject.UserMgmt" %>

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
    <script type="text/javascript" src="js/dataValidation.js"></script>
    
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <style type="text/css">
        .required:after {
            content: " *";
            color: #f00;
        }

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
       /* ---------------------------------------------------------- */
        [type='checkbox']:not(:checked), [type='checkbox']:checked {
            position: inherit !important; 
            pointer-events: auto !important; 
            opacity:10 !important;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            //$('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            //$('.timepicker').timepicker();
            //$('.tabs').tabs();

            //jQuery(".jqte-test").jqte({
            //    blur: function () { }
            //});
            //// settings of status
            //var jqteStatus = true;

            //$(function () {
            //    $("#drpRole").change(function () {
            //        var xRole = this.value;
            //        if (xRole == 'dealer') {
            //            $("#dvCustomer").show();
            //            $("#dvEmployee").hide();
            //        }
            //        else {
            //            $("#dvCustomer").hide();
            //            $("#dvEmployee").show();
            //        }
            //    });

            //});
        });
        /*---------------------------------------------------------*/
        function pageLoad(sender, args) {
            //retainTabPosition();
        }
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
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function openFormContainerNew() {
            jQuery("#txtScreenFullName").val("");
            jQuery("#txtUserID").val("");
            jQuery("#txtPassword").val("");
            jQuery("#txtUserID").val("");
            jQuery("#txtUserID").val("");


            jQuery(".usermsg").empty();
            jQuery('#myModal').modal({ "show": true });
        }

        function openFormContainer() {
            jQuery(".usermsg").empty();
            var options = { "show": true }
            jQuery('#myModal').modal(options);
            //e.preventDefault();
        }
        function dealerShowHide()
        {
            var xRole = $("#drpRole").val();
            if (xRole == 'dealer')
            {
                $("#dvCustomer").show();
                $("#dvEmployee").hide();
            }
            else 
            {
                $("#dvCustomer").hide();
                $("#dvEmployee").show();
            }
        }
        /*---------------------------------------------------------*/
        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpEmployee').val() : "0";
            var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*---------------------------------------------------------*/
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*---------------------------------------------------------*/
        function showErrorPopup(xTitle, xMsg) {
            if (xTitle == '')
                xTitle = 'Message';
            // -------------------------------------
            if (xMsg != '') {
                jQuery.confirm({
                    title: xTitle, content: xMsg, type: 'red', typeAnimated: true, width: 'auto',
                    buttons: { close: { text: 'Close', btnClass: 'btn-blue ConfirmClose', action: function () { } } }
                });
            }
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />

        <div id="divLicenseUser" runat="server" visible="false" clientidmode="Static" style="height: 450px; border: 5px solid navy; margin: 2px; vertical-align: middle; padding: 45px;">
            <div style="margin: 10px; min-height: 300px; width: 100%; background-color: red; border-radius: 8px; padding: 45px;">
                <asp:Label ClientIDMode="Static" runat="server" ID="lblWarning" Style="font-weight: 800; font-size: 24px; color: white;"></asp:Label>
            </div>
            <div style="margin: 10px; min-height: 50px; width: 100%;">
                <span class="badge badge-primary" style="width: 100%; font-size: 16px; padding: 10px;">Please Contact Sharvaya Infotech At support@SharvayaInfotech.com
                </span>
            </div>
        </div>
        <div id="divUserMgmt" class="contentwrapper" runat="server" clientidmode="Static" visible="true">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hdnUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" EnableViewState="true" />
                    <div id="myModal" style="display: block; width: 100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div id="divAccount" runat="server" class="col m6 padding-2" style="min-height:400px; box-shadow: navy -6px 8px 6px -6px;">
                                        <div class="row">
                                            <div class="input-field col m6">
                                                <asp:TextBox ID="txtUserID" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " MaxLength="20" TabIndex="1" AutoCompleteType="Disabled" autocomplete="off" />
                                                <label class="active" for="txtUserID">User ID <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                            <div class="input-field col m6">
                                                <asp:TextBox ID="txtPassword" runat="server" ClientIDMode="Static" TabIndex="2" class="form-control" PlaceHolder=" " TextMode="Password" MaxLength="20" AutoCompleteType="Disabled" autocomplete="off"  />
                                                <label class="active" for="txtPassword">Password <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m6">
                                                <asp:TextBox ID="txtScreenFullName" runat="server" ClientIDMode="Static" PlaceHolder=" " TabIndex="3" class="form-control" MaxLength="20"></asp:TextBox>
                                                <label class="active" for="txtScreenFullName">Display Name <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                            <div class="input-field col m6">
                                                <asp:DropDownList ID="drpRole" runat="server" ClientIDMode="Static" TabIndex="4" class="select2-theme browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpRole_SelectedIndexChanged" />
                                                <label class="active" for="drpRole">Role <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m6">
                                                <asp:DropDownList ID="drpCompany" runat="server" ClientIDMode="Static" TabIndex="5" class="select2-theme browser-default" />
                                                <label class="active" for="drpCompany">Company/Branch <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                            <div class="input-field col m6">
                                                <asp:DropDownList ID="drpOrganization" runat="server" ClientIDMode="Static" PlaceHolder=" " TabIndex="6" class="select2-theme browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpOrganization_SelectedIndexChanged" />
                                                <label class="active" for="drpOrganization">Department/Division <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                        </div>
                                        <div id="dvEmployee" runat="server" clientidmode="Static" class="row" enableviewstate="true" visible="true">
                                            <div class="input-field col m8">
                                                <asp:DropDownList ID="drpEmployee" runat="server" ClientIDMode="Static" TabIndex="7" Width="200" class="select2-theme browser-default"/>
                                                <label class="active" for="drpEmployee">Employee <span class="materialize-red-text font-weight-800">*</span>
                                                    <a href="javascript:openEmployeeInfo('view');">
                                                        <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="17" />
                                                    </a>
                                                    <a href="javascript:openEmployeeInfo('add');">
                                                        <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                    </a>
                                                </label>
                                            </div>
                                        </div>
                                        <div class="row" id="dvCustomer" runat="server" clientidmode="Static" enableviewstate="true" visible="false">
                                            <div class="input-field col m8">
                                                <label class="active" for="txtCustomerName">
                                                    Dealer Name<small class="text-muted font-italic">&nbsp;(Min.3 Chars To Search)</small><span class="materialize-red-text font-weight-800">*</span>
                                                    <a href="javascript:openCustomerInfo('view');">
                                                        <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" Tabindex="3" />
                                                    </a>
                                                    <a href="javascript:openCustomerInfo('add');">
                                                        <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                    </a>
                                                </label>
                                                <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="8" placeholder=""/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m6">
                                                <label class="active" for="chkBlock">Active Status</label>
                                                <div class = "switch">
                                                    <label>Inactive
                                                    <asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" Checked="true" TabIndex="9"/>
                                                    <span class="lever"></span>Active</label>
                                                </div> 
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divOther" runat="server" class="col m6 padding-2" style="min-height:400px; box-shadow: navy -6px 8px 6px -6px;">
                                        <table id="tblAddEditDel" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <asp:Repeater ID="rptAddEditDel" runat="server">
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th class="left-align">Module Name</th>
                                                        <th class="center-align">Add</th>
                                                        <th class="center-align">Edit</th>
                                                        <th class="center-align">Delete</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnMenuID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                <asp:HiddenField ID="hdnMenuURL" runat="server" ClientIDMode="Static" Value='<%# Eval("MenuURL") %>' />
                                                <tr class="blueShed">
                                                    <td class="left-align"><%# Eval("MenuText") %></td>
                                                    <td class="center-align">
                                                        <asp:CheckBox id="chkAddFlag" runat="server" ClientIDMode="Static" Checked='<%# Eval("AddFlag") %>' style="position: inherit !important;pointer-events: auto !important;" />
                                                    </td>
                                                    <td class="center-align">
                                                        <asp:CheckBox id="chkEditFlag" runat="server" ClientIDMode="Static" Checked='<%# Eval("EditFlag") %>' style="position: inherit !important;pointer-events: auto !important;" />
                                                    </td>
                                                    <td class="center-align">
                                                        <asp:CheckBox id="chkDelFlag" runat="server" ClientIDMode="Static" Checked='<%# Eval("DelFlag") %>' style="position: inherit !important;pointer-events: auto !important;" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        </table>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                        <button ID="btnReset" type="button" runat="server" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="9"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSave" type="button" runat="server" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="10"><i class="material-icons left">save</i>Save</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--**************************** Employee Popup ***********************************--%>
            <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
            <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
                <div id="myModal11111">
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
    </form>
</body>
</html>
