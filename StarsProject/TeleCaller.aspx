<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeleCaller.aspx.cs" Inherits="StarsProject.TeleCaller" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />

    <script type="text/javascript" src='js/plugins/jquery-1.7.min.js'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />

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
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.timepicker').timepicker();
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

        /*---------------------------------------------------------*/
        function openRegion(cat) {
            var pageUrl = "ManageCity.aspx?mode=add&id=0";
            spnPageModuleHeader.innerText = "Manage Region";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=add&id=0";
            spnPageModuleHeader.innerText = "Customer Information";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*Add Product view on Product Name Vikram Rajput 15-07-2020*/
        function openProductInfo(cat) {
            var keyid = "0";
            var pageUrl = "Products.aspx?mode=view&id=0";
            spnPageModuleHeader.innerText = "Product Information";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020*/
        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpAssignTo').val() : "0";
            spnPageModuleHeader.innerText = "Employee Information";
            var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
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
    <form id="frmViewTeleCaller" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="updDailyToDO" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="widgetbox">
                <asp:HiddenField ID="hdnCountryFlagURL" runat="server" ClientIDMode="Static" />
                <%-- Bootstrap Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnSource" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnQueryDateTime" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCountryISO" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCountryCode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnInquiryNopkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                <%--hdnCityCode and hdnStateCode is used to implement SearchBox in State and City--%>
                <asp:HiddenField ID="hdnCityCode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnStateCode" runat="server" ClientIDMode="Static" />
                
                <div id="myModal" style="display: block; width: 100%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="col m12">
                                    <div class="card user-statistics-card float-left mt-0 padding-1" style="width: 58%;">
                                        <div class="card-content p-0" style="min-height: 200px;">
                                            <h5 class="card-title padding-1 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Manage Opportunities<i class="material-icons float-left">ac_unit</i></h5>
                                            <div id="panel1" runat="server" class="user-statistics-container">
                                                <div class="row">
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtLeadID">Lead # </label>
                                                        <asp:TextBox ID="txtLeadID" runat="server" ClientIDMode="Static" MaxLength="20" class="form-control" placeholder="" ReadOnly="true" TabIndex="1" />
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <label class="active" for="txtQueryDate">    Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                        <asp:TextBox ID="txtQueryDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" TextMode="Date" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="input-field col m12">
                                                        <label class="active" for="txtProductName" style="color: black !important;margin: -4px 0px 0px 0px;">
                                                            Product&nbsp;&nbsp;<small class="text-muted font-italic" style="color: maroon !important;"></small> <span class="materialize-red-text font-weight-800">*</span>
                                                            <a href="javascript:openProductInfo('add');">
                                                                <img src="images/expand.png" width="30" height="20" style="padding: 0px 0px 0px 10px; margin: 5px 0px 0px 0px;" alt="Add New Product" title="Add New Product" />
                                                            </a>
                                                        </label>
                                                        <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" EnableViewState="true" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" TabIndex="3" placeholder="Type to Find Product" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="input-field col m12">
                                                        <label class="active" for="txtMessage">Detail <span class="materialize-red-text font-weight-800">*</span></label>
                                                        <asp:TextBox ID="txtMessage" runat="server" ClientIDMode="Static" EnableViewState="true" MaxLength="280" TextMode="MultiLine" class="form-control materialize-textarea" TabIndex="4" Rows="3" placeholder="" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col m12">
                                                        <div class="row">
                                                            <div class="input-field col m5">
                                                                <label class="active required" for="txtSenderName" style="color: black !important;">Contact Person</label>
                                                                <asp:TextBox ID="txtSenderName" runat="server" MaxLength="100" ClientIDMode="Static" EnableViewState="true" class="form-control" TabIndex="5" placeholder="" />
                                                            </div>
                                                            <div class="input-field col m7">
                                                                <label class="active" for="txtCompanyName" style="color: black !important;">Company Name <span class="materialize-red-text font-weight-800">*</span>
                                                                    <a href="javascript:openCustomerInfo('view');">                         
                                                                        <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;"/>
                                                                    </a>
                                                                </label>
                                                                <asp:TextBox ID="txtCompanyName" runat="server" ClientIDMode="Static" EnableViewState="true" class="form-control" TabIndex="6" placeholder="Type to Find Customer" AutoPostBack="true" onKeyup="bindDDLTo('#txtCompanyName')" OnTextChanged="txtCompanyName_TextChanged" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m5">
                                                                <label class="active" for="txtEmail" style="color: black !important;">Email</label>
                                                                <asp:TextBox ID="txtEmail" runat="server" class="form-control" MaxLength="50" ClientIDMode="Static" TabIndex="7" placeholder="" />
                                                            </div>
                                                            <div class="input-field col m3">
                                                                <label class="active required" for="txtContact" style="color: black !important;">Primary Contact #</label>
                                                                <asp:TextBox ID="txtContact" runat="server" class="form-control" MaxLength="15" ClientIDMode="Static" TabIndex="8" placeholder="" />
                                                            </div>
                                                            <div class="input-field col m3">
                                                                <label class="active" for="txtContact1" style="color: black !important;">Alternate Contact #</label>
                                                                <asp:TextBox ID="txtContact1" runat="server" class="form-control" MaxLength="15" ClientIDMode="Static" TabIndex="9" placeholder="" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m12">
                                                                <label class="active" for="txtAddress" style="color: black !important;">Address </label>
                                                                <asp:TextBox ID="txtAddress" runat="server" ClientIDMode="Static" MaxLength="100" class="form-control" TabIndex="10" placeholder="" AutoCompleteType="None" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m6">
                                                               <% if (hdnSerialKey.Value != "BLUE-CHEM-56JK-BC88")
                                                                    { %>
                                                                <label class="active required" for="drpCountry" style="color: black !important;">Country&nbsp;
                                                                    <a href="javascript:openRegion('add');">
                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add New Country" title="Add Country" />
                                                                    </a>
                                                                </label>
                                                                <asp:DropDownList ID="drpCountry" runat="server" ClientIDMode="Static" TabIndex="11" class="select2-theme browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" />
                                                                <% } %>
                                                            </div>
                                                            
                                                            <div class="input-field col m6">
                                                               <% if (hdnSerialKey.Value != "BLUE-CHEM-56JK-BC88")
                                                                    { %>
                                                                 <label class="active required" for="drpState" style="color: black !important;">State&nbsp;
                                                                    <a href="javascript:openRegion('add');">
                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add State" title="Add State" />
                                                                    </a>
                                                                </label>
                                                                <asp:DropDownList ID="drpState" runat="server" ClientIDMode="Static" TabIndex="12" class="select2-theme browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpState_SelectedIndexChanged" />
                                                            <% } %>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m6">
                                                                 <% if (hdnSerialKey.Value != "BLUE-CHEM-56JK-BC88")
                                                                    { %>
                                                                <label class="active required" for="drpCity" style="color: black !important;">City&nbsp;
                                                                    <a href="javascript:openRegion('add');">
                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add City" title="Add City" />
                                                                    </a>
                                                                </label>
                                                                <asp:DropDownList ID="drpCity" runat="server" ClientIDMode="Static" TabIndex="13" class="select2-theme browser-default" placeholder="" AutoPostBack="true" OnSelectedIndexChanged="drpCity_SelectedIndexChanged" />
                                                            <% } %>
                                                            </div>
                                                            <div class="input-field col m6">
                                                                <% if (hdnSerialKey.Value != "BLUE-CHEM-56JK-BC88")
                                                                    { %>
                                                                <label class="active" for="txtPinCode" style="color: black !important;">Pincode</label>
                                                                <asp:TextBox ID="txtPinCode" runat="server" MaxLength="50" ClientIDMode="Static" class="form-control" TabIndex="14" placeholder="" AutoCompleteType="None" />
                                                            <% } %>  
                                                            </div>
                                                        </div>
                                                    </div>                                      
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card user-statistics-card float-right mt-0 padding-1" style="width: 40%;">
                                        <div class="card-content p-0" style="min-height: 200px;">
                                            <h5 class="card-title padding-1 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Lead Status<i class="material-icons float-left">ac_unit</i></h5>
                                            <div id="panel2" runat="server" class="user-statistics-container">
                                                <div class="row">
                                                    <div class="input-field col m6">
                                                        <label class="active" for="drpInquiryStatus">Lead Status </label>
                                                        <asp:DropDownList ID="drpInquiryStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="15" Style="height: inherit;" AutoPostBack="true" OnSelectedIndexChanged="drpInquiryStatus_SelectedIndexChanged" placeholder="">
                                                            <asp:ListItem Text="---Select Status---" Selected="True" />
                                                            <asp:ListItem Text="Disqualified" Value="Disqualified" />
                                                            <asp:ListItem Text="In-Process" Value="InProcess" />
                                                            <asp:ListItem Text="Qualified" Value="Qualified" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="input-field col m6">
                                                        <label class="active" for="spnInquiryNo">Lead/Inquiry No</label>
                                                        <asp:TextBox ID="spnInquiryNo" runat="server" ClientIDMode="Static" MaxLength="20" class="form-control"  ReadOnly="true" placeholder="" />
                                                    </div>
                                                </div>
                                                <div class="row" id="divAssignTo" runat="server" clientidmode="static" visible="false">
                                                    <div class="input-field col m12">
                                                        <label class="active" for="drpAssignTo" style="color: black !important;">Assign To  <span class="materialize-red-text font-weight-800">*</span>
                                                             <%--Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020--%>
                                                            <a href="javascript:openEmployeeInfo('view');">
                                                                <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;"/>
                                                            </a>
                                                            <a href="javascript:openEmployeeInfo('add');">
                                                                <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee"/>
                                                            </a>
                                                        </label>
                                                        <asp:DropDownList ID="drpAssignTo" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="18" Style="height: inherit;"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col m12">
                                                        <div id="divQualified" runat="server" clientidmode="static" visible="false">
                                                            <h5 class="card-title padding-1 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Add Followup Reminder<i class="material-icons float-left">ac_unit</i></h5>
                                                            <div id="divFollowup" runat="server" clientidmode="static" class="p-md">
                                                                <div class="row mt-2 badge-secondary">
                                                                    <div id="divFollowupNotes" runat="server" clientidmode="Static" class="input-field col m">
                                                                        <label class="active required" for="txtFollowupNotes" style="color: black !important;">FollowUp Notes</label>
                                                                        <asp:TextBox ID="txtFollowupNotes" runat="server" Style="width: 248%;" class="form-control" ClientIDMode="Static" TabIndex="19" TextMode="MultiLine" Rows="3" MaxLength="500" placeholder="" />
                                                                    </div>
                                                                </div>
                                                                <div id="divNextFollowup" runat="server" clientidmode="Static" class="row mt-2 badge-secondary">
                                                                    <div class="input-field col m6">
                                                                        <label class="active required" for="txtFollowupDate" style="color: black !important;">Next FollowUp</label>
                                                                        <asp:TextBox ID="txtFollowupDate" runat="server" class="form-control" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" TabIndex="20" placeholder="" TextMode="Date" />
                                                                    </div>
                                                                    <div class="input-field col m6">
                                                                        <label class="active" for="txtPreferredTime" style="color: black !important;">Preferred Time</label>
                                                                        <asp:TextBox ID="txtPreferredTime" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="21" Style="width: 150px;" onkeypress="return false;" onpaste="return false" ReadOnly="false" onfocus="popupSetTime(this);" placeholder="00:00" />&nbsp;&nbsp;
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row hide">
                                                                <div class="input-field col m12">
                                                                    <label class="active" for="txtCustomerName" style="color: black !important;">
                                                                        Search Customer&nbsp;&nbsp;<span class="materialize-red-text font-weight-800">*</span>
                                                                        <a href="javascript:openCustomerInfo('view');">
                                                                            <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block; padding: 6px 0px 0px 0px" tabindex="19" />
                                                                        </a>
                                                                        <a href="javascript:openCustomerInfo('add');">
                                                                            <img src="images/addCustomer.png" width="30" style="padding: 6px 0px 0px 0px" height="20" alt="Add New Customer" title="Add New Customer" />
                                                                        </a>
                                                                    </label>
                                                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" TabIndex="21" placeholder="" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="input-field col m12">
                                                        <div id="divDisQualified" runat="server" clientidmode="static" class="badge-primary p-md-6">
                                                            <div class="card badge-primary">
                                                                <div class="card-header">
                                                                    <h5 class="card-title mb-0">&nbsp;Disqualified Lead Detail</h5>
                                                                </div>
                                                                <div class="card-body">
                                                                    <div class="row" style="margin: 10px 0px 0px -10px;">
                                                                        <div class="input-field col m12">
                                                                            <label class="active required" for="txtPinCode" style="color: black !important;">DisQualified Reason</label>
                                                                            <asp:DropDownList ID="drpDisQualifiedReason" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="24" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div id="divDisQualifiedRemarks" runat="server" clientidmode="static" class="badge-primary p-md-6">
                                                            <div class="card badge-primary">
                                                                <div class="card-header">
                                                                    <h5 class="card-title mb-0">&nbsp;Remarks</h5>
                                                                </div>
                                                                <div class="card-body">
                                                                    <div class="row" style="margin: 10px 0px 0px -10px;">
                                                                        <div class="input-field col m12">
                                                                            <label class="active" for="txtDisQualifiedRemarks" style="color: black !important;">Disqualified Remarks</label>
                                                                            <asp:TextBox ID="txtDisQualifiedRemarks" runat="server" MaxLength="50" TextMode="MultiLine" ClientIDMode="Static" class="form-control" TabIndex="25" placeholder="" AutoCompleteType="None" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>
                        <div class="modal-footer">
                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                <button id="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="27"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button id="btnGenerateInquiry" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnGenerateInquiry_Click" TabIndex="26"><i class="material-icons left">save</i>Save & Generate Inquiry</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
            <%--**************************** Customer Popup ***********************************--%>
            <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
            <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
                <div id="myModal111">
                    <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                        <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                            <i class="material-icons prefix">ac_unit</i>
                            <span id="spnPageModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                            <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                        </h5>
                    </div>
                </div>
                <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
                <br />
            </asp:Panel>

            <script type="text/javascript">
                $("input").keypress(function (event) {
                    if (event.which == 13) {
                        event.preventDefault();
                        //$("form").submit();
                    }
                });

                function bindDDLTo(selector) {
                    if ($(selector).val().length >= 3 && ($("#hdnpkID").val() == '' || $("#hdnpkID").val() == '0'))
                    {
                        //  && ($("#hdnpkID").val() == '' || $("#hdnpkID").val()=='0')
                        //alert($("#hdnpkID").val())
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterCustomer",
                            data: '{pCustName:\'' + $(selector).val() + '\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                console.log(data);
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
                                        return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnCustomerID").val(item.data('langname'));

                                    }
                                });
                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                    }
                    else {
                        if ($(selector).val().length == 0) {
                            $("#hdnCustomerID").val('');
                        }
                    }
                }

                function bindDDLProductTo(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterProduct",
                            data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                var sample = JSON.parse(data.d);
                                $(selector).autoComplete({
                                    minLength: 1,
                                    source: function (term, suggest) {
                                        term = term.toLowerCase();
                                        var choices = sample;
                                        suggest(choices);
                                    },
                                    renderItem: function (item, search) {
                                        $(".autocomplete-suggestion").remove();
                                        return '<div class="autocomplete-suggestion" data-langname="' + item.pkID + '" data-lang="' + item.ProductNameLong + '" data-val="' + search + '">' + item.ProductNameLong + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnProductID").val(item.data('langname'));
                                        $("#txtReferenceName").focus();
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
                            $("#hdnProductID").val('');
                        }
                    }
                }
            </script>
        </div>
    </form>
</body>

</html>
