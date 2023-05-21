<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitorInfo.aspx.cs" Inherits="StarsProject.VisitorInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <style type="text/css">
        /*Preview-Add EmployeeList on Employee Name Dropdown by Vikram Rajput 14-07-2020*/

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
            $('.tabs').tabs();

            jQuery(".jqte-test").jqte({
                blur: function () { }
            });
            // settings of status
            var jqteStatus = true;
        });
        /*---------------------------------------------------------*/
        function pageLoad(sender, args) {
            retainTabPosition();
            //IdentityType();
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

        $(function () {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "profile";
            $("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "profile";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab").val(currTab);
            });
        });

        function retainTabPosition() {

            $('#lnkprofile').removeClass('active');
            $('#lnkcompany').removeClass('active');
            $('#lnkdocuments').removeClass('active');

            $('#profile').css("display", "none");
            $('#company').css("display", "none");
            $('#documents').css("display", "none");

            if ($("#hdnCurrentTab").val() == 'profile') {
                $('#lnkprofile').addClass('active');
                $('#profile').css("display", "block");
            }

            if ($("#hdnCurrentTab").val() == 'company') {
                $('#lnkcompany').addClass('active');
                $('#company').css("display", "block");
            }

            else if ($("#hdnCurrentTab").val() == 'documents') {
                $('#lnkdocuments').addClass('active');
                $('#documents').css("display", "block");
            }
        }

        function openEmployeeInfo(cat) {
            var keyid = "";
            document.getElementById('spnModuleHeader').innerText = "Manage Organization Employees";
            keyid = (cat == 'view') ? $('#drpReportTo').val() : "0";
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

        function showFileExtError(xMode) {
            if (xMode == 'image')
                alert("Only BMP, JPEG, GIF and PNG file is valid for Attachment !");
            if (xMode == 'pdf')
                alert("Only PDF file attachment is valid for Upload !");
        }

    </script>
</head>
<body class="loginpage" style="overflow-x: hidden;">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="profile" />
            <div class="widgetbox">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <div id="myModal" style="display: block; width: 98%; overflow-x: hidden;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div id="divPersonal" runat="server" class="col m7 padding-1" style="min-height: 450px; box-shadow: navy -6px 8px 6px -6px;">
                                    <h5 class="card-title padding-1 mb-1 float-left center-align width-100 gradient-45deg-light-blue-indigo">Visitor Personal Information</h5>
                                    <div class="row" style="max-width: 100%;">
                                        <div class="input-field col m4">
                                            <label class="active" for="txtInquiryNo">Visitor # </label>
                                            <asp:TextBox ID="txtInquiryNo" runat="server" ClientIDMode="Static" MaxLength="20" class="form-control" placeholder=" " ReadOnly="true" TabIndex="1" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtVisitDate">Visit Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtVisitDate" runat="server" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" class="form-control" TextMode="Date" placeholder=" " TabIndex="2" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtVisitTime">Visit Time <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtVisitTime" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="3" onkeypress="return false;" onpaste="return false" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row" style="max-width: 100%;">
                                        <div class="input-field col m4">
                                            <label class="active" for="txtVisitorName">Visitor Name <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtVisitorName" runat="server" ClientIDMode="Static" MaxLength="50" class="form-control" placeholder=" " TabIndex="4" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtVisitorContact">Contact # <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtVisitorContact" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="15" TabIndex="5" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtVisitorEmail">Email </label>
                                            <asp:TextBox ID="txtVisitorEmail" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="6" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <label class="active" for="txtPurposeOfVisit">Visit Notes <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtPurposeOfVisit" runat="server" class="form-control" ClientIDMode="Static" TabIndex="7" MaxLength="500" TextMode="MultiLine" Rows="3" placeholder="" />
                                        </div>
                                    </div>
                                    <h5 class="card-title padding-1 mb-1 float-left center-align width-100 gradient-45deg-light-blue-indigo">Meeting To Person Information</h5>
                                    <div class="row" style="max-width: 100%;">
                                        <div class="input-field col m6">
                                            <label class="active" for="drpOrganizationEmp">Department <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpDepartment" runat="server" ClientIDMode="Static" class="select2-theme browser-default" AutoPostBack="true" OnSelectedIndexChanged="drpDepartment_SelectedIndexChanged" TabIndex="8" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="drpReportTo">
                                                Meeting To <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openEmployeeInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" />
                                                </a>
                                                <a href="javascript:openEmployeeInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                </a>
                                            </label>
                                            <asp:DropDownList ID="drpMeetingTo" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="9" />
                                        </div>
                                    </div>
                                </div>
                                <div id="divOther" runat="server" class="col m5 padding-1" style="min-height: 450px; box-shadow: navy -6px 8px 6px -6px;">
                                    <div class="row">
                                        <div class="card user-card-negative-margin z-depth-0" id="feed" style="margin: 0px;">
                                            <div class="card-content card-border-gray" style="padding-top: 0px;">
                                                <div class="row">
                                                    <ul id="myTab" class="tabs card-border-gray" runat="server" clientidmode="Static" enableviewstate="true">
                                                        <li class="tab col m4 p-0"><a id="lnkprofile" class="left active" href="#profile">Visitor Image</a></li>
                                                        <li class="tab col m4 p-0"><a id="lnkcompany" class="left active" href="#company" tabindex="11">Company Details</a></li>
                                                        <li class="tab col m4 p-0"><a id="lnkdocuments" class="left active" href="#documents" tabindex="19">Documents</a></li>
                                                    </ul>
                                                </div>
                                                <div id="profile" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                                    <div class="row">
                                                        <div class="app-file-area" style="height: 300px;">
                                                            <div class="app-file-header">
                                                                <div class="row">
                                                                    <div class="input-field col m12 add-new-file mt-0">
                                                                        <div class="col m12">
                                                                            <label class="active" for="uploadImage">Upload Visitor Image ...</label>
                                                                        </div>
                                                                        <br />
                                                                        <div class="col m12">
                                                                            <asp:FileUpload ID="uploadImage" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only BMP, JPEG, GIF and PNG File" TabIndex="10" />
                                                                            <asp:Button ID="Button3" CssClass="hide" runat="server" Text="Upload" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="app-file-content">
                                                                <div id="divImageControl2 col m12">
                                                                    <div class="col m3 center-align" style="padding: 10px; margin: 0px 0px 0px 20px; border-radius: 4px; border: 1px solid silver;">
                                                                        <asp:Image ID="imgVisitor" runat="server" ClientIDMode="Static" Width="100" Height="100" ImageUrl="images/noimage.png" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="company" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                                    <div class="input-field col m12">
                                                        <div class="row">
                                                            <div class="input-field col m7">
                                                                <label class="active" for="txtCompanyName">Company Name</label>
                                                                <asp:TextBox ID="txtCompanyName" runat="server" ClientIDMode="Static" MaxLength="150" class="form-control" TabIndex="12" placeholder="" />
                                                            </div>
                                                            <div class="input-field col m5">
                                                                <label class="active" for="txtCompanyContact">Contact #</label>
                                                                <asp:TextBox ID="txtCompanyContact" runat="server" ClientIDMode="Static" MaxLength="15" class="form-control" TabIndex="13" placeholder="" />
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="input-field col m12">
                                                                <label class="active" for="txtCompanyAddress">Address <span class="materialize-red-text font-weight-800">*</span></label>
                                                                <asp:TextBox ID="txtCompanyAddress" runat="server" ClientIDMode="Static" MaxLength="100" class="form-control" TabIndex="14" placeholder="" />
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="input-field col m6">
                                                                <label class="active" for="drpCountry">Country <span class="materialize-red-text font-weight-800">*</span></label>
                                                                <asp:DropDownList ID="drpCountry" runat="server" ClientIDMode="Static" TabIndex="15" class="select2-theme browser-default" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" AutoPostBack="true" />
                                                            </div>
                                                            <div class="input-field col m6">
                                                                <label class="active" for="drpState">State <span class="materialize-red-text font-weight-800">*</span></label>
                                                                <asp:DropDownList ID="drpState" runat="server" ClientIDMode="Static" TabIndex="16" class="select2-theme browser-default" OnSelectedIndexChanged="drpState_SelectedIndexChanged" AutoPostBack="true" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="input-field col m6">
                                                                <label class="active" for="drpCity">City <span class="materialize-red-text font-weight-800">*</span></label>
                                                                <asp:DropDownList ID="drpCity" runat="server" ClientIDMode="Static" TabIndex="17" class="select2-theme browser-default" placeholder="" />
                                                            </div>
                                                            <div class="input-field col m6">
                                                                <label class="active" for="txtPinCode">Pincode </label>
                                                                <asp:TextBox ID="txtPinCode" runat="server" MaxLength="50" ClientIDMode="Static" class="form-control" TabIndex="18" placeholder="" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="documents" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display: none;">
                                                    <div class="row">
                                                        <div class="app-file-area" style="height: 300px;">
                                                            <div class="app-file-header">
                                                                <div class="row">
                                                                    <div class="input-field col m12 add-new-file mt-0">
                                                                        <div class="col m12">
                                                                            <label class="active" for="uploadDocument">Upload Document ...</label>
                                                                        </div>
                                                                        <br />
                                                                        <div class="col m12">
                                                                            <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only BMP, JPEG, GIF and PNG File" TabIndex="20" />
                                                                            <asp:Button ID="Button4" CssClass="hide" runat="server" Text="Upload" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="app-file-content">
                                                                <div id="divDocumentControl2 col m12">
                                                                    <div class="col m3 center-align" style="padding: 10px; height: 300px; width: 450px; margin: 0px 0px 0px 20px; border-radius: 4px; border: 1px solid silver;">
                                                                        <asp:Image ID="imgDocument" runat="server" ClientIDMode="Static" Width="100%" Height="100%" ImageUrl="images/attach_document.png" />
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
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 5px;">
                            <button id="btnReset" type="button" runat="server" clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="22"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button id="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="21"><i class="material-icons left">save</i>Save</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--**************************** Employee Popup ***********************************--%>
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
            <div id="myModal123">
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
    </form>
</body>
</html>
