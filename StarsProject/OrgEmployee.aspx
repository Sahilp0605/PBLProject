<%@ Page Title="" Language="C#" AutoEventWireup="true" ValidateRequest="false"  CodeBehind="OrgEmployee.aspx.cs" Inherits="StarsProject.OrgEmployee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ Register Src="~/myModuleAttachment.ascx" TagPrefix="uc1" TagName="myModuleAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>
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
        .jqte {
            margin: 0px;
        }

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

        a:hover {
            cursor: pointer;
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
        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnPkIDEmp").val('');
        }
        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        $(function () {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "empimg";
            $("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "empimg";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab").val(currTab);
            });
        });

        function retainTabPosition() {
            $('#lnkempimg').removeClass('active');
            $('#lnkemphr').removeClass('active');
            $('#lnkempbank').removeClass('active');
            $('#lnkempemail').removeClass('active');
            $('#lnkpwdvault').removeClass('active');
            $('#lnkempdocs').removeClass('active');
            $('#lnkempAddress').removeClass('active');

            $('#empimg').css("display", "none");
            $('#emphr').css("display", "none");
            $('#empbank').css("display", "none");
            $('#empemail').css("display", "none");
            $('#pwdvault').css("display", "none");
            $('#empdocs').css("display", "none");
            $('#empAddress').css("display", "none");
            // --------------------------------------------------- 
            if ($("#hdnCurrentTab").val() == 'empimg') {
                $('#lnkempimg').addClass('active');
                $('#empimg').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'emphr') {
                $('#lnkemphr').addClass('active');
                $('#emphr').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'empbank') {
                $('#lnkempbank').addClass('active');
                $('#empbank').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'empemail') {
                $('#lnkempemail').addClass('active');
                $('#empemail').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'pwdvault') {
                $('#lnkpwdvault').addClass('active');
                $('#pwdvault').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'empdocs') {
                $('#lnkempdocs').addClass('active');
                $('#empdocs').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'empAddress') {
                $('#lnkempAddress').addClass('active');
                $('#empAddress').css("display", "block");
            }
        }

        /*Preview-Add EmployeeList on Employee Name Dropdown by Vikram Rajput 14-07-2020*/
        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpReportTo').val() : "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Organization Employees";
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

        function ShowPDFfile(repFilename) {
            repFilename = "EmployeeDocs/" + repFilename;
            //alert(repFilename);
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }
    </script>

</head>
<body class="loginpage">
<form id="frmEmployee" runat="server" autocomplete="off">
        
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">
        <%--<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" Value="empimg" EnableViewState="true" />
                <div class="widgetbox">
                <%-- Bootstrap Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnPkIDEmp" runat="server" />
                <asp:HiddenField ID="hdnEditor" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value="0" />
                <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />

                <div id="myModal" style="display:block; width:100%; padding:10px 5px 5px 5px;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div id="divPersonal" runat="server" class="col m5 padding-1" style="min-height:520px; box-shadow: navy -6px 8px 6px -6px;">
                                    <h5 class="card-title padding-1 mb-1 float-left center-align width-100 gradient-45deg-light-blue-indigo">Employee Personal Information</h5>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtEmployeeName">Employee Name <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtEmployeeName" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="100" TabIndex="1" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="drpOrganizationEmp">Department/Division <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpOrganizationEmp" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="2" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <label class="active" for="txtMobileNo">Mobile # <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtMobileNo" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="3" MaxLength="15" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtLandline"><% if (hdnSerialKey.Value == "ACSI-C803-CUP0-SHEL") { %>Emergency  No <% } else {%> Landline <%} %> #</label>
                                            <asp:TextBox ID="txtLandline" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="4" MaxLength="15" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtBirthDate">Birth Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtBirthDate" runat="server" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" class="form-control" TextMode="Date" placeholder=" " TabIndex="5" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="drpDesignationEmp">Designation <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpDesignationEmp" runat="server"  ClientIDMode="Static" class="select2-theme browser-default" TabIndex="6" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="drpReportTo">Report To <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openEmployeeInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" />
                                                </a>
                                                <a href="javascript:openEmployeeInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                </a>
                                            </label>
                                            <asp:DropDownList ID="drpReportTo" runat="server"  ClientIDMode="Static" class="select2-theme browser-default" TabIndex="7"/>
                                        </div>

                                    </div>
                                    <div class="row mt-1">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtDrivingLicenseNo">Driving Lic.No</label>
                                            <asp:TextBox ID="txtDrivingLicenseNo" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="8" MaxLength="15" />
                                        </div>
                                        <div class="input-field col m6">
                                       <label class="active" for="txtPassportNo"><% if (hdnSerialKey.Value == "ACSI-C803-CUP0-SHEL") { %>Insurance No <% } else {%> Passport No <%} %></label>
                                            <asp:TextBox ID="txtPassportNo" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="9" MaxLength="15" />
                                        </div>                                    
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtAadharCardNo">Aadhar Card #</label>
                                            <asp:TextBox ID="txtAadharCardNo" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="10" MaxLength="15" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtPANCardNo">PAN Card #</label>
                                            <asp:TextBox ID="txtPANCardNo" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="11" MaxLength="15" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtEmpCode">Employee Code</label>
                                            <asp:TextBox ID="txtEmpCode" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="12" MaxLength="15" />
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtBloodGroup">Blood Group </label>
                                            <asp:TextBox ID="txtBloodGroup" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="13" MaxLength="50" />
                                        </div>
                                    </div>

                                </div>
                                <div id="divOther" runat="server" class="col m7 padding-1" style="min-height:482px; box-shadow: navy -6px 8px 6px -6px;">
                                    <div class="row">
                                        <div class="card user-card-negative-margin z-depth-0" id="feed" style="margin:0px;">
                                            <div class="card-content card-border-gray" style="padding-top: 0px;">
                                                <div class="row">
                                                    <ul id="myTab" class="tabs card-border-gray" runat="server" clientidmode="Static" enableviewstate="true">
                                                        <li class="tab col p-0"><a id="lnkempimg" class="left active" style="padding:0 15px;" href="#empimg">Profile Image</a></li>
                                                        <li class="tab col p-0"><a id="lnkemphr" class="left" style="padding:0 15px;" href="#emphr">HR Info</a></li>
                                                        <li class="tab col p-0"><a id="lnkempAddress" class="left" style="padding:0 15px;" href="#empAddress">Address</a></li>
                                                        <li class="tab col p-0"><a id="lnkempbank" class="left" style="padding:0 15px;" href="#empbank">Bank Info.</a></li>
                                                        <li class="tab col p-0"><a id="lnkempemail" class="left" style="padding:0 15px;" href="#empemail">Email Config.</a></li>
                                                        <li class="tab col p-0"><a id="lnkpwdvault" class="left" style="padding:0 15px;" href="#pwdvault">Passwords</a></li>
                                                        <li class="tab col p-0"><a id="lnkempdocs" class="left" style="padding:0 15px;" href="#empdocs">Documents</a></li>
                                                    </ul>
                                                </div>
                                                <div id="empimg" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>

                                                    <div class="section app-file-manager-wrapper">
                                                    <div class="app-file-overlay"></div>
                                                    <div class="row">
                                                        <div class="app-file-area col m6" style="height:200px; overflow-y:auto;">
                                                            <div class="app-file-header">
                                                                <div class="row">
                                                                    <div class="input-field col m12 add-new-file mt-2">
                                                                        <div class="col m12">
                                                                            <label class="active" for="uploadDocument">Upload Employee Image ...</label>
                                                                        </div>
                                                                        <div class="col m12">
                                                                            <asp:FileUpload ID="imgEmpFileUpload" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                                            <asp:Button ID="btnEmpImage" CssClass="hide" runat="server" Text="Upload"/>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="app-file-content">
                                                                <div id="divImgEmployee" class="col m12">
                                                                    <div class="col m3 center-align" style="padding: 10px;border-radius: 4px;border: 1px solid silver;">
                                                                        <asp:Image ID="imgEmployee" runat="server" ClientIDMode="Static" Width="100" Height="100" ImageUrl="images/noimage.png" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-area col m6" style="height:200px; overflow-y:auto;">
                                                            <div class="app-file-header">
                                                                <div class="row">
                                                                    <div class="input-field col m12 add-new-file mt-2">
                                                                        <div class="col m12">
                                                                            <label class="active" for="imgSignatureUpload">Upload eSignature...</label>
                                                                        </div>
                                                                        <div class="col m12">
                                                                            <asp:FileUpload ID="imgSignatureUpload" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                                            <asp:Button ID="btnSignImage" CssClass="hide" runat="server" Text="Upload"/>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="app-file-content">
                                                                <div id="divImgSignature" class="col m12">
                                                                    <div class="col m3 center-align" style="padding: 10px;border-radius: 4px;border: 1px solid silver;">
                                                                        <asp:Image ID="imgSignature" runat="server" ClientIDMode="Static" Width="100" Height="100" ImageUrl="images/noimage.png" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtEditor">Authorized Signatory</label>
                                                            <asp:TextBox ID="txtEditor" runat="server" ClientIDMode="Static" placeholder=" " CssClass="jqte-test form-control mt-0" MaxLength="1000" TextMode="MultiLine" TabIndex="14" style="height:7rem !important;"></asp:TextBox>                
                                                        </div>
                                                    </div>
                                                    </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnEmpImage" />
                                                    </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div id="emphr" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                    <div class="input-field col m12">
                                                        <div id="container1" runat="server" clientidmode="Static" class="container p-0" style="min-height:250px;">
                                                            <h5 class="card-title padding-1 mb-1 float-left width-100 gradient-45deg-light-blue-indigo font-weight-800">Joining Information</h5>
                                                            <div class="row">
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtJoiningDate">Joining Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                                    <asp:TextBox ID="txtJoiningDate" runat="server" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" class="form-control" TextMode="Date" placeholder=" " TabIndex="15" />
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtConfirmationDate">Confirmation Date</label>
                                                                    <asp:TextBox ID="txtConfirmationDate" runat="server" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" class="form-control" TextMode="Date" placeholder=" " TabIndex="16" />
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtReleaseDate">Release Date</label>
                                                                    <asp:TextBox ID="txtReleaseDate" runat="server" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" class="form-control" TextMode="Date" placeholder=" " TabIndex="17" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtFixedSalary">Biometric #</label>
                                                                    <asp:TextBox ID="txtCardNo" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" placeholder=" " TabIndex="18" />
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtConfirmationDate">Select Shift</label>
                                                                    <asp:DropDownList ID="drpShift" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="19" />
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtConfirmationDate">Basic Per</label>
                                                                    <asp:DropDownList ID="drpBasicPer" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="20">
                                                                        <asp:ListItem Text="Monthly" Value="Monthly" /> 
                                                                        <asp:ListItem Text="Daily" Value="Daily" />
                                                                        <asp:ListItem Text="Hourly" Value="Hourly" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        
                                                            <h5 class="card-title padding-1 mb-1 float-left width-100 gradient-45deg-light-blue-indigo font-weight-800">Salary Fixed Parameters</h5>
                                                            <div class="row">
                                                                <div class="input-field col m4">
                                                                    <label class="active navy-text" for="txtFixedSalary">Fixed Salary <span class="materialize-red-text font-weight-800">*</span></label>
                                                                    <asp:TextBox ID="txtFixedSalary" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" placeholder=" " TabIndex="21" />
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active navy-text" for="txtFixedBasic">Fixed Basic</label>
                                                                    <asp:TextBox ID="txtFixedBasic" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" placeholder=" " TabIndex="22" />
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active navy-text" for="txtFixedHRA">Fixed H.R.A</label>
                                                                    <asp:TextBox ID="txtFixedHRA" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" placeholder=" " TabIndex="23" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="input-field col m4">
                                                                    <label class="active navy-text" for="txtFixedConv">Fixed Conveyance</label>
                                                                    <asp:TextBox ID="txtFixedConv" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" placeholder=" " TabIndex="24" />
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active navy-text" for="txtFixedDA">Fixed D.A</label>
                                                                    <asp:TextBox ID="txtFixedDA" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" placeholder=" " TabIndex="25" />
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active navy-text" for="txtFixedSpecial">Fixed Special Allow</label>
                                                                    <asp:TextBox ID="txtFixedSpecial" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" placeholder=" " TabIndex="26" />
                                                                </div>

                                                            </div>
                                                            <div class="row">
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="drpGender">Gender</label>
                                                                    <asp:DropDownList ID="drpGender" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="27">
                                                                        <asp:ListItem Text="---Select---" Value=" " /> 
                                                                        <asp:ListItem Text="Male" Value="Male" /> 
                                                                        <asp:ListItem Text="Female" Value="Female" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="drpMaritalStatus">Marital Status </label>
                                                                    <asp:DropDownList ID="drpMaritalStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="28">
                                                                        <asp:ListItem Text="---Select---" Value=" " /> 
                                                                        <asp:ListItem Text="Married" Value="Married" /> 
                                                                        <asp:ListItem Text="Unmarried" Value="Unmarried" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtWorkingHours">Working Hours</label>
                                                                    <asp:TextBox ID="txtWorkingHours" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" placeholder=" " TabIndex="29" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="drpPFCalc">PF Calculation</label>
                                                                    <asp:DropDownList ID="drpPFCalc" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="30">
                                                                        <asp:ListItem Text="Yes" Value="Yes" /> 
                                                                        <asp:ListItem Text="No" Value="No" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="drpPTCalc">PT Calculation</label>
                                                                    <asp:DropDownList ID="drpPTCalc" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="31">
                                                                        <asp:ListItem Text="Yes" Value="Yes" /> 
                                                                        <asp:ListItem Text="No" Value="No" />
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtPFAccountNo">PF Account No.</label>
                                                                    <asp:TextBox ID="txtPFAccountNo" runat="server" ClientIDMode="Static" class="form-control" MaxLength="20" placeholder=" " TabIndex="32" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="input-field col m4">
                                                                    <label class="active" for="txtESICNo">ESIC No.</label>
                                                                    <asp:TextBox ID="txtESICNo" runat="server" ClientIDMode="Static" class="form-control" MaxLength="20" placeholder=" " TabIndex="33" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="empAddress" class="row col m12" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                    <div class="row mt-2">
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtAddress">Address</label>
                                                            <asp:TextBox ID="txtAddress" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="34" TextMode="MultiLine" />
                                                        </div>
                                                         <div class="input-field col m12">
                                                            <div class="input-field col m4">
                                                                <asp:TextBox ID="txtArea" runat="server" TabIndex="35" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                                <label class="active" for="txtArea">Area</label>
                                                            </div>
                                                            <div class="input-field col m4">
                                                                <asp:DropDownList ID="drpCountry" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="36" AutoPostBack="true"  OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"
                                                                    />
                                                                <label class="active" for="dwrpCountry">Country <span class="materialize-red-text font-weight-800">*</span>
                                                                    <a href="javascript:openRegion('add');">
                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add Country" title="Add Country" />
                                                                    </a>
                                                                </label>
                                                            </div>
                                                            <div class="input-field col m4">
                                                                <asp:DropDownList ID="drpState" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="37" AutoPostBack="true"  OnSelectedIndexChanged="drpState_SelectedIndexChanged"/>
                                                                <label class="active" for="drpState">State <span class="materialize-red-text font-weight-800">*</span>
                                                                    <a href="javascript:openRegion('add');">
                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add State" title="Add State" />
                                                                    </a>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <div class="input-field col m4">
                                                                <asp:DropDownList ID="drpCity" class="select2-theme browser-default" runat="server"  ClientIDMode="Static" TabIndex="38" />
                                                                <label class="active" for="drpCity">City <span class="materialize-red-text font-weight-800">*</span>
                                                                    <a href="javascript:openRegion('add');">
                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add City" title="Add City" />
                                                                    </a>
                                                                </label>
                                                            </div>
                                                            <div class="input-field col m4">
                                                                <asp:TextBox ID="txtPincode" runat="server" TabIndex="39" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="24"/>
                                                                <label class="active" for="txtPincode">Pincode</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="empbank" class="row col m12" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                    <div class="row mt-2">
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtBankName">Bank Name</label>
                                                            <asp:TextBox ID="txtBankName" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="40" />
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtBankBranch">Branch Name</label>
                                                            <asp:TextBox ID="txtBankBranch" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="41" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtBankAccountNo">Account #</label>
                                                            <asp:TextBox ID="txtBankAccountNo" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="42" Width="200px" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtBankIFSC">IFSC Code</label>
                                                            <asp:TextBox ID="txtBankIFSC" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " TabIndex="43" Width="200px" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="empemail" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                    <div class="input-field col m12">
                                                        <div id="container2" runat="server" clientidmode="Static" class="container p-0" style="min-height:250px;">
                                                            <div class="input-field col m8">
                                                                <label class="active" for="txtEmailAddress">Email ID </label>
                                                                <asp:TextBox ID="txtEmailAddress" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="44" />
                                                            </div>
                                                            <div class="input-field col m4">
                                                                <label class="active" for="txtEmailPassword">Email Password </label>
                                                                <asp:TextBox ID="txtEmailPassword" runat="server"  ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="20" TabIndex="45" TextMode="Password" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="pwdvault" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                    <div class="input-field col m12">
                                                        <div class="card small" style="height:350px;">
                                                             <div class="card-title blue white-text" style="padding:10px; line-height:25px;">
                                                                <p class="no-margin">Add Personal Portal/Detail Credentials</p>
                                                                 <a id="lnkFilter" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped btn-move-up accent-2 z-depth-4 right" data-position="center" data-tooltip="Add New Contact"  style="padding:5px; position:relative; margin-right: 15px !important;">
                                                                     <i class="material-icons activator" style="line-height:23px;">add</i>
                                                                 </a>
                                                            </div>
                                                            <div class="card-content row" style="height: 280px; width: 100%; padding: 0px; margin: 0px;overflow-y:auto;">
                                                                <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <asp:Repeater ID="rptCredentials" runat="server" ClientIDMode="Static" OnItemCommand="rptCredentials_ItemCommand">
                                                                    <HeaderTemplate>
                                                                        <thead>
                                                                            <tr>
                                                                                <th>Description</th>
                                                                                <th>User ID</th>
                                                                                <th>Password</th>
                                                                                <th class="text-center" colspan="2">Action</th>
                                                                            </tr>
                                                                        </thead>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnpkIDPass" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                        <tr class="blueShed">
                                                                            <td>
                                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("Description") %>' TabIndex="46" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUserID" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("UserID") %>' TabIndex="47" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUserPassword" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("UserPassword") %>' TabIndex="48" />
                                                                            </td>
                                                                            <td class="text-center">
                                                                                <asp:ImageButton ID="ImgbtnUpdate" runat="server" ImageUrl="~/images/SaveData.png" CommandName="Update" CommandArgument='<%# Eval("pkID") %>' Width="35" Height="20" TabIndex="49" />
                                                                            </td>
                                                                            <td class="text-center">
                                                                                <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="50" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>

                                                                </asp:Repeater>
                                                                </table>
                                                            </div>
                                                            <div class="card-reveal gradient-45deg-light-blue-teal gradient-shadow black-text">
                                                                <span class="card-title grey-text text-darken-4">Create New Contact<i class="material-icons right">close</i></span>
                                                                <div id="profile-card1" class="card animate fadeRight mb-0">
                                                                    <div class="card-content p-0 col m12">
                                                                        <div class="row">
                                                                            <div class="input-field col m8">
                                                                                <asp:TextBox ID="txtDescriptionNew" runat="server" class="form-control" ClientIDMode="Static" placeholder=" " TabIndex="51" MaxLength="50" />
                                                                                <label class="active" for="txtDescriptionNew">Description</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="input-field col m4">
                                                                                <asp:TextBox ID="txtUserIDNew" runat="server" class="form-control" ClientIDMode="Static" placeholder=" " TabIndex="52" MaxLength="15" />
                                                                                <label class="active" for="txtUserIDNew">User ID</label>
                                                                            </div>
                                                                            <div class="input-field col m8">
                                                                                <asp:TextBox ID="txtUserPasswordNew" runat="server" class="form-control" ClientIDMode="Static" placeholder=" " TabIndex="53" MaxLength="100" />
                                                                                <label class="active" for="txtUserPasswordNew">Password</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="input-field col m12">
                                                                                <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add.png" data-tooltip="Add Contacts" Width="110" Height="30" TabIndex="54" OnClick="imgBtnSave_Click" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="empdocs" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                    <div class="section app-file-manager-wrapper">
                                                    <div class="app-file-overlay"></div>
                                                    <div class="row">
                                                        <div class="col m12">
                                                            <uc1:myModuleAttachment runat="server" id="myProductDocs" TabIndex="55"/>
                                                        </div>
                                                    </div>
                                                    <%--<div class="row">
                                                        <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                            <div class="app-file-header">
                                                                <div class="row">
                                                                    <div class="input-field col m12 add-new-file mt-0">
                                                                        <div class="col m12">
                                                                            <label class="active" for="uploadDocument">Upload Product Specification Document Here  </label>
                                                                        </div>
                                                                        <div class="col m12">
                                                                            <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                                            <asp:Button ID="btnUploadDoc" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="app-file-content">
                                                                <div class="app-file-sidebar-content row">
                                                                    <div class="col m12 app-file-files">
                                                                        <asp:Repeater ID="rptEmpDocs" runat="server" ClientIDMode="Static" OnItemCommand="rptEmpDocs_ItemCommand">
                                                                        <ItemTemplate>
                                                                            <div class="col xl4 l6 m4 s6">
                                                                                <div class="card box-shadow-none mb-1 app-file-info">
                                                                                    <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                    <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                        <div class="row fonticon p2">
                                                                                            <div class="col m6" style="text-align:left;">
                                                                                                <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                                <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                            </div>
                                                                                            <div class="col m6" style="text-align:right;">
                                                                                                <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                                <i class="material-icons">find_in_page</i>
                                                                                                </a>
                                                                                            </div>
                                                                                        </div>
                                                                                        <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                    </div>
                                                                                    <div class="app-file-details">
                                                                                        <div class="app-file-name font-weight-700">
                                                                                            <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(6) %></p>
                                                                                        </div>
                                                                                    </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>--%>
                                                    </div>
                                                    </ContentTemplate>
                                                    <%--<Triggers>
                                                        <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                    </Triggers>--%>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                            <button ID="btnReset" type="button" runat="server" clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="57"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="56"><i class="material-icons left">save</i>Save</button>
                        </div>
                    </div>
                </div>

                </div>

          <%--  </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    <%--**************************** Employee Popup ***********************************--%>
    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
        <div id="myModal1">
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