  <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebComplaint.aspx.cs" Inherits="StarsProject.WebComplaint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myModuleAttachment.ascx" TagPrefix="uc1" TagName="myModuleAttachment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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

        .ui-widget-header, .ui-state-default, ui-button {
            background: #b9cd6d;
            border: 1px solid #b9cd6d;
            color: #FFFFFF;
            font-weight: bold;
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
        .cardRound {
            box-shadow: black -6px 8px 6px -6px !important;
            border-radius: 8px 8px 0px 0px !important;
        }
        .h5Round {
            border-radius: 8px 8px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, #303F9F, #7B1FA2) !important;
            color: white !important;
        }
        .h5RoundHead {
            border-radius: 0px 0px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, silver, #7f88ec6e);
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('.timepicker').timepicker();
            $('#spnModuleHeader').val("Complaint List");
        });

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

        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpEmployee').val() : "0";
            var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

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

    <div id="contentwrapper" class="contentwrapper p-5">
        <div class="widgetbox">
            <div class="clearall"></div>
            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCustEmailAddress" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />

            <div id="myModal" style="display:block; width:100%;">
                <div class="modal-content">
                    <div class="modal-body" style="padding:50px;">
                        <div class="row">
                            <div class="col m12">
                            <h5 class="h5Round">
                                <span id="Span2" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Register Complaint</span>
                            </h5>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m12">
                                    <div class="row">
                                    <div class="input-field col m3">
                                        <label class="active" for="txtName">Name<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtName" runat="server" ClientIDMode="Static" class="form-control" ReadOnly="true"  TabIndex="4" onkeypress="return false;" onpaste="return false" placeholder=""/>
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtMobileNo">Mobile No.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtMobileNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" MaxLength="10" AutoPostBack="true" />
                                    </div>
                                        <div class="input-field col m3">
                                        <label class="active" for="txtWorkOdrNo">Work Order No.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="TextBox1" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtdtPurchase">Date Of Purchase<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="TextBox2" runat="server" ClientIDMode="Static" class="form-control" ReadOnly="true"  TabIndex="4" placeholder="" TextMode="Date" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtCompanyName">Company Name</label>
                                        <asp:TextBox ID="txtCompanyName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" onkeypress="return false;" onpaste="return false" placeholder=""/>
                                    </div>
                                    <div class="input-field col m6">
                                        <label class="active" for="txtCompanyEmailID">Email ID<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtCompanyEmailID" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" onpaste="return false" ReadOnly="true" placeholder="" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtWorkOdrNo">Work Order No.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtWorkOdrNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" />
                                    </div>
                                    <div class="input-field col m6">
                                        <label class="active" for="txtdtPurchase">Date Of Purchase<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtdtPurchase" runat="server" ClientIDMode="Static" class="form-control" ReadOnly="true"  TabIndex="4" placeholder="" TextMode="Date" />
                                    </div>
                                </div>
                                   <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtPanelSRNo">Panel Serial No.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtPanelSRNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" />
                                    </div>
                                    <div class="input-field col m6">
                                        <label class="active" for="txtProductSRNO">Product Serial No.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtProductSRNO" runat="server" ClientIDMode="Static" class="form-control" ReadOnly="true"  TabIndex="4" placeholder="" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtSiteAdd">Site Address</label>
                                        <asp:TextBox ID="txtSiteAdd" runat="server" class="form-control" ClientIDMode="Static" MaxLength="20" TabIndex="5" placeholder="" TextMode="MultiLine"/>
                                    </div>
                                    <div class="input-field col m6">
                                        <div class="row">
                                            <div class="input-field col m6">
                                                <label class="active" for="drpCity">City<span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:DropDownList id="drpCity" runat="server" CssClass="browser-default" ClientIDMode="Static"/>
                                            </div>
                                            <div class="input-field col m6">
                                                <label class="active" for="drpState">State<span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:DropDownList id="drpState" runat="server" CssClass="browser-default" ClientIDMode="Static"/>
                                            </div>
                                        </div>
                                        <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtPinCode">Pin Code<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtPinCode" runat="server" ClientIDMode="Static" class="form-control" ReadOnly="true"  TabIndex="4" placeholder="" />
                                        </div>
                                            <div class="input-field col m6">
                                            </div>
                                        </div>
                                   </div>

                                </div>
                                <div class="row">
                                    <div class="input-field col m3">
                                        <label class="active" for="txtName">SITE COORDINATOR NAME <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="TextBox3" runat="server" ClientIDMode="Static" class="form-control" ReadOnly="true"  TabIndex="4" onkeypress="return false;" onpaste="return false" placeholder=""/>
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtMobileNo">CONTACT NO.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="TextBox4" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" MaxLength="10" AutoPostBack="true" />
                                    </div>
                                        <div class="input-field col m3">
                                        <label class="active" for="txtWorkOdrNo">CONVINIENT TIME SLOT<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="TextBox5" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtdtPurchase">CONVINIENT DATE<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="TextBox6" runat="server" ClientIDMode="Static" class="form-control" ReadOnly="true"  TabIndex="4" placeholder="" TextMode="Date" />
                                    </div>
                                </div>
                                <%--<div class="col m6">
                                    <div class="row mt-1 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                        <div class="input-field col m6 mb-0 hide">
                                            <label class="active" for="txtPreferredDate">Schedule Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtPreferredDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" TextMode="Date" placeholder="" />
                                        </div>
                                        <div class="input-field col m6 mb-0 hide">
                                            <label class="active" for="txtTimeFrom">Preferred Time </label>
                                            <asp:TextBox ID="txtTimeFrom" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="7" onkeypress="return false;" onpaste="return false" style="width:100px; display:inline-block;" placeholder="" />&nbsp;&nbsp;
                                            <asp:TextBox ID="txtTimeTo" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="8" onkeypress="return false;" onpaste="return false" style="width:100px; display:inline-block;" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row mt-1 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                        <div class="input-field col m4 hide">
                                            <label class="active" for="drpEmployee">Assigned To<span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openEmployeeInfo('view');">
                                                    <img src="images/registration.png" width="20" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="3" />
                                                </a>
                                                <a href="javascript:openEmployeeInfo('add');">
                                                    <img src="images/addCustomer.png" width="20" height="20" alt="Add New Employee" title="Add New Employee" />
                                                </a>
                                            </label>
                                            <asp:DropDownList ID="drpEmployee" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="9"></asp:DropDownList>
                                        </div>
                                        <div class="input-field col m4 hide">
                                            <label class="active" for="drpStatus">Status</label>
                                            <asp:DropDownList ID="drpStatus" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="10">
                                                <asp:ListItem Text="---Select Status---" Value="" Selected="True" />
                                                <asp:ListItem Text="Open" Value="Open" Selected="True" />
                                                <asp:ListItem Text="Close" Value="Close" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="input-field col m4 hide">
                                            <label class="active" for="drpType">Type</label>
                                            <asp:DropDownList ID="drpType" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="11">
                                                <asp:ListItem Text="---Select Type---" Value="" Selected="True" />
                                                <asp:ListItem Text="Online" Value="Online" />
                                                <asp:ListItem Text="Offline" Value="Offline" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                            <div class="input-field col m12">
                                <div class="row mt-1 badge-secondary" style="border-radius: 5px; padding: 5px; margin-left:-22px">
                                    <div class="input-field col m12">
                                        <label class="active" for="txtComplaintNotes">Complaint Description <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtComplaintNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" MaxLength="500" TextMode="MultiLine" Rows="4" style="min-height:120px;" Enabled="true" placeholder=""/>
                                    </div>
                                </div>
                            </div>

                        </div>
                       <%-- <div class="row hide">
                            <div class="col m6 pr-0 left-align">
                                <div class="card small padding-1 float left-align" style="height:350px;">
                                    <div class="card-title blue white-text" style="padding:10px;">
                                        <p class="no-margin">Address Information </p>
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
                                            <div class="input-field col m4" id="DivCountry" runat="server" clientidmode="Static">
                                                <asp:DropDownList ID="drpCountry" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="17" AutoPostBack="true"  OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"/>
                                                <label class="active" for="drpCountry">Country <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                            <div class="input-field col m4">
                                                <asp:DropDownList ID="drpState" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="17" AutoPostBack="true"  OnSelectedIndexChanged="drpState_SelectedIndexChanged"/>
                                                <label class="active" for="drpState">State <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                        </div>
                                        <div class="row" style="width:100%">
                                            <div class="input-field col m6">
                                                <asp:DropDownList ID="drpCity" class="select2-theme browser-default" runat="server"  ClientIDMode="Static" TabIndex="18" />
                                                <label class="active" for="drpCity">City </label>
                                            </div>
                                          <%--  <div class="input-field col m6">
                                                <asp:TextBox ID="txtPincode" runat="server" TabIndex="19" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="18"/>
                                                <label class="active" for="txtPincode">Pincode</label>
                                            </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>--%>
                        <div class="row">
                            <div class="col m12">
                                <uc1:myModuleAttachment runat="server" id="myModuleAttachment" />
                            </div>
                        </div>
                     <%--   <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                            <button id="btnReset" type="button" runat="server"  clientidmode="Static" class="btn orange left" onserverclick="btnReset_ServerClick" TabIndex="11"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button id="btnSaveEmail" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSaveEmail_ServerClick" TabIndex="12"><i class="material-icons left">email</i>Save - Send Email</button>
                            <button id="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_ServerClick" TabIndex="13"><i class="material-icons left">save</i>Save</button>
                        </div>--%>  
                    </div>
                </div>
            </div>
        </div>
        <%--**************************** Customer Popup ***********************************--%>
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
        </cc1:ModalPopupExtender>  
        <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
            <div id="myModal111">
                <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom:10px !important;">
                    <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                        <i class="material-icons prefix">ac_unit</i>
                        <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                        <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text=""/>
                    </h5>
                </div>
            </div>
            <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border:1px solid silver; background-color:transparent; width:100%; height:90%; padding:5px;"></iframe>  
            <br/>  
        </asp:Panel> 
    </div>
    </form>
</body>
</html>
