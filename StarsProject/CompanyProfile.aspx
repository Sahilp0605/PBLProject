<%@ Page Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true"  CodeBehind="CompanyProfile.aspx.cs" Inherits="StarsProject.CompanyProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
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
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>--%>

    <style type="text/css">
        h5 { line-height:10px !important; }
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

        /* ---------------------------------------------------------- */
        .checkbox-container {
            display: inline-block;
            position: relative;
        }

        .checkbox-container label {
            background-color: lightgray;
            border: 1px solid #fff;
            border-radius: 20px;
            display: inline-block;
            position: relative;
            transition: all 0.3s ease-out;
            width: 45px;
            height: 25px;
            z-index: 2;
        }

        .checkbox-container label::after {
            content: ' ';
            background-color: red;
            border-radius: 50%;
            position: absolute;
            top: 1.5px;
            left: 1px;
            transform: translateX(0);
            transition: transform 0.3s linear;
            width: 20px;
            height: 20px;
            z-index: 3;
        }

        .checkbox-container input {
            visibility: hidden;
            position: absolute;
            z-index: 2;
        }

        .checkbox-container input:checked + label::after {
            transform: translateX(calc(100% + 0.5px));
            background-color: forestgreen;
        }
        /*---------------------------------------------------------------*/
    </style>

    <script type="text/javascript">
        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnPkIDEmp").val('');
        }

        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }

        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper">

            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>
                        <asp:HiddenField ID="hdnCompanyID" runat="server" />
                        <asp:HiddenField ID="hdnBankID" runat="server" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0">
                                    <h5 class="z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 60px; text-align: left; padding-left: 15px;">
                                        <i class="material-icons prefix">ac_unit</i>
                                        <span id="spnCompanyHeader">Manage Company Profile</span>
                                    </h5>
                                </div>
                                <br />
                                <div class="modal-body">
                                    <div class="row badge-light" style="border-radius: 5px;width:100%">
                                        <div class="input-field col m4">
                                            <label for="chkCustomer" class="active">Select Company : </label>
                                            <asp:DropDownList ID="drpCompany" CssClass="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="float: left; padding: 0px; width: 150px; float: left; font-size: 16px; min-width: 400px; margin: 3px 0px 0px -3px;" AutoPostBack="true" OnSelectedIndexChanged="drpCompany_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtCompanyName">Company Name</label>
                                            <asp:TextBox ID="txtCompanyName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" placeholder="" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="drpParentCompany">Branch Of</label>
                                            <asp:DropDownList ID="drpParentCompany" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="2" />
                                        </div>
                                    </div>
                                    <div class="row badge-light" style="border-radius: 5px;width:100%">
                                        <div class="input-field col m4">
                                             <label class="active" for="txtGSTNo">GST #</label>
                                             <asp:TextBox ID="txtGSTNo" runat="server" ClientIDMode="Static" class="form-control" TabIndex="22" placeholder="" />
                                         </div>
                                         <div class="input-field col m4">
                                              <label class="active" for="txtPANNo">PAN #</label>
                                              <asp:TextBox ID="txtPANNo" runat="server" ClientIDMode="Static" class="form-control" TabIndex="23" placeholder="" />
                                         </div>
                                        <div class="input-field col m4">
                                              <label class="active" for="txtCINo">CI No. #</label>
                                              <asp:TextBox ID="txtCINo" runat="server" ClientIDMode="Static" class="form-control" TabIndex="23" placeholder="" />
                                         </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <div class="card">
                                                <div class="card-title blue" style="padding: 10px;">
                                                    <h5 class="card-title mb-0" style="color: white">Registered Address</h5>
                                                </div>
                                                <div class="card-body">
                                                    <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtAddress">Address</label>
                                                            <asp:TextBox ID="txtAddress" runat="server" ClientIDMode="Static" class="form-control" TabIndex="3" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m3">
                                                            <label class="active" for="txtArea">Area</label>
                                                            <asp:TextBox ID="txtArea" runat="server" ClientIDMode="Static" TabIndex="4" class="form-control" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m3">
                                                            <label class="active" for="drpState">State</label>
                                                            <asp:DropDownList ID="drpState" runat="server" ClientIDMode="Static" TabIndex="5" class="select2-theme browser-default" Style="height: inherit;" AutoPostBack="true" OnSelectedIndexChanged="drpState_SelectedIndexChanged" />
                                                        </div>
                                                        <div class="input-field col m3">
                                                            <label class="active" for="drpCity">City</label>
                                                            <asp:DropDownList ID="drpCity" runat="server" ClientIDMode="Static" TabIndex="6" class="select2-theme browser-default" Style="height: inherit;" AutoPostBack="true" />
                                                        </div>
                                                        <div class="input-field col m3">
                                                            <label class="active" for="txtPincode">Pincode</label>
                                                            <asp:TextBox ID="txtPincode" runat="server" TabIndex="7" class="form-control" ClientIDMode="Static" placeholder="" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <div class="card" style="min-height: 370px;">
                                                <div class="card-title blue" style="padding: 10px;">
                                                    <h5 class="card-title mb-0" style="color: white;">Email Notification</h5>
                                                </div>
                                                <div class="card-body" style="margin: -30px 0px 0px 0px;">
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <div class="input-field row mb-3">
                                                                <label class="col-sm-8 col-form-label" for="chkCustomer">
                                                                    Email On New Customer
                                                                <div class="checkbox-container">
                                                                    <asp:CheckBox ID="chkCustomer" runat="server" ClientIDMode="Static" TabIndex="8" />
                                                                    <label for="chkCustomer"></label>
                                                                </div>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <div class="input-field row mb-3">
                                                                <label for="chkInquiry" class="col-sm-8 col-form-label">
                                                                    Email On New Inquiry 
                                                                <div class="checkbox-container">
                                                                    <asp:CheckBox ID="chkInquiry" runat="server" ClientIDMode="Static" TabIndex="9" />
                                                                    <label for="chkInquiry"></label>
                                                                </div>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <div class="input-field row mb-3">
                                                                <label for="chkQuotation" class="col-sm-8 col-form-label">
                                                                    Email On New Quotation
                                                                <div class="checkbox-container">
                                                                    <asp:CheckBox ID="chkQuotation" runat="server" ClientIDMode="Static" TabIndex="10" />
                                                                    <label for="chkQuotation"></label>
                                                                </div>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <div class="input-field row mb-3">
                                                                <label for="chkLeaveRequest" class="col-sm-8 col-form-label">
                                                                    Email On New Leave Request
                                                                <div class="checkbox-container">
                                                                    <asp:CheckBox ID="chkLeaveRequest" runat="server" ClientIDMode="Static" TabIndex="11" />
                                                                    <label for="chkLeaveRequest"></label>
                                                                </div>
                                                                </label>
                                                            </div>
                                                        </div>
                                                         <div class="input-field col m12">
                                                            <div class="input-field row mb-3">
                                                                <label for="chkFeedback" class="col-sm-8 col-form-label">
                                                                    Feedback Email On Sales Order 
                                                                <div class="checkbox-container">
                                                                    <asp:CheckBox ID="chkFeedback" runat="server" ClientIDMode="Static" TabIndex="12" />
                                                                    <label for="chkFeedback"></label>
                                                                </div>
                                                                </label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-field col m4">
                                            <div class="card" style="min-height: 370px;">
                                                <div class="card-title blue" style="padding: 10px;">
                                                    <h5 class="card-title mb-0" style="color: white;">Email Setup Configuration </h5>
                                                </div>
                                                <div class="card-body" style="padding: 20px 20px 0px 10px;">
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtHost">Host Server</label>
                                                            <asp:TextBox ID="txtHost" runat="server" ClientIDMode="Static" class="form-control" TabIndex="13" placeholder="" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtUserName">Sender Email Address</label>
                                                            <asp:TextBox ID="txtUserName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="14" placeholder="" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtPassword">Password</label>
                                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" ClientIDMode="Static" class="form-control" TabIndex="15" placeholder="" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="input-field col m7">
                                                            <label class="active" for="txtPortNumber">Port Number</label>
                                                            <asp:TextBox ID="txtPortNumber" runat="server" ClientIDMode="Static" class="form-control" TabIndex="16" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m5">
                                                            <label class="active" for="chkActive">Enable SSL</label>
                                                            <div class="form-check p-0" style="margin: 5px 0px 0px 0px;">
                                                                <div class="checkbox-container">
                                                                    <asp:CheckBox ID="chkEnableSSL" runat="server" ClientIDMode="Static" TabIndex="17" />
                                                                    <label for="chkEnableSSL"></label>
                                                                    <div class="active-circle"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-field col m4">
                                            <div class="card" style="min-height: 370px;">
                                                <div class="card-title blue" style="padding: 10px;">
                                                    <h5 class="card-title mb-0" style="color: white;">Company Signature</h5>
                                                </div>
                                                <div class="card-body" style="padding: 20px 20px 0px 10px;"">
                                                    <div class="row">
                                                        <div class="app-file-area col m6" style="height:200px; overflow-y:auto;">
                                                            <div class="app-file-header">
                                                                <div class="row">
                                                                    <div class="input-field col m12 add-new-file mt-2">
                                                                        <div class="col m12">
                                                                            <label class="active" for="imgSignatureUpload">Upload Authorized Sign ...</label>
                                                                        </div>
                                                                        <div class="col m12">
                                                                            <asp:FileUpload ID="imgSignatureUpload" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only JPEG/PNG/BMP" /> 
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="app-file-content">
                                                                <div id="divImgEmployee" class="col m12">
                                                                    <div class="col m3 center-align" style="padding: 10px;border-radius: 4px;border: 1px solid silver;">
                                                                        <asp:Image ID="imgSignature" runat="server" ClientIDMode="Static" Width="100" Height="100" ImageUrl="images/noimage.png" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="input-field col m4 hide">
                                            <div class="card" style="min-height: 370px;">
                                                <div class="card-title blue" style="padding: 10px;">
                                                    <h5 class="card-title mb-0" style="color: white;">Bank Information</h5>
                                                </div>
                                                <div class="card-body" style="padding: 20px 20px 0px 10px;"">
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtBankName">Bank Name</label>
                                                            <asp:TextBox ID="txtBankName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="18" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtBranchName">Branch Name</label>
                                                            <asp:TextBox ID="txtBranchName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="19" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtBankAccountNo">Bank Account #</label>
                                                            <asp:TextBox ID="txtBankAccountNo" runat="server" ClientIDMode="Static" class="form-control" TabIndex="20" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtBankIFSC">Bank IFSC Code</label>
                                                            <asp:TextBox ID="txtBankIFSC" runat="server" ClientIDMode="Static" class="form-control" TabIndex="21" placeholder="" />
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <label class="active" for="txtBankSWIFT">Bank SWIFT Code</label>
                                                            <asp:TextBox ID="txtBankSWIFT" runat="server" ClientIDMode="Static" class="form-control" TabIndex="19" placeholder="" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                     <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="width: 98%; margin-bottom: 13px;">
                                        <button id="btnReset" type="button" runat="server" clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="24"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                        <button id="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="25"><i class="material-icons left">save</i>Save</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
</asp:Content>
