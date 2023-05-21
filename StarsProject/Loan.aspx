<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loan.aspx.cs" Inherits="StarsProject.Loan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  

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

        .star-fg .glyphicon-star {
            color: red !important;
        }

        #hearts {
            color: #FF0000;
        }

        #hearts-existing {
            color: #87bad7;
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

        #chkReminder { position: absolute; pointer-events:all; opacity: 100; width:20px; height:20px;}
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
            $('.timepicker').timepicker();
            checkedEventCall();
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
        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpEmployee').val() : "0";
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

        function checkedEventCall() {
            $('#chkReminder').bind('change', function () {
                if ($(this).is(":checked")) {
                    $("#textDueDate").hide();
                    $("#divReminder").show();
                }
                else {
                    $("#textDueDate").show();
                    $("#divReminder").hide();
                }
            }).trigger('change');
        }
        
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">

    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnModuleView" runat="server" ClientIDMode="Static" />

    <div id="contentwrapper" class="contentwrapper">

        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="widgetbox">
                    <div class="clearall"></div>
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCategory" runat="server" ClientIDMode="Static" />

                    <div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row mt-1 mb-1" style="border-bottom:4px solid;">
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="drpEmployee">Select Employee <span class="materialize-red-text font-weight-800">*</span>
                                                <a class="hide" href="javascript:openEmployeeInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="17" />
                                                </a>
                                                <a class="hide" href="javascript:openEmployeeInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                </a>
                                            </label>
                                            <asp:DropDownList ID="drpEmployee" runat="server" CssClass="select2-theme browser-default" ClientIDMode="Static" TabIndex="2"></asp:DropDownList>
                                        </div>
                                        <div id="divLoanType" runat="server" class="input-field col m2 pr-0">
                                            <label class="active" for="drpLoanType">Loan Type<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpLoanType" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="3" placeholder="">
                                                <asp:ListItem Text="Personal Loan" Value="personal" />
                                                <asp:ListItem Text="Medical Loan" Value="medical" />
                                                <asp:ListItem Text="Home Loan" Value="home" />
                                                <asp:ListItem Text="Vehicle Loan" Value="vehical" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="input-field col m2 pr-0">
                                            <label class="active" for="txtLoanAmount">Loan Amount <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtLoanAmount" runat="server" class="form-control width-70" ClientIDMode="Static" TabIndex="4" placeholder="" AutoPostBack="true" OnTextChanged="txtLoanAmount_TextChanged"/>
                                        </div>
                                        <div id="divApprovalStatus" runat="server" class="input-field col m5 pr-0 float-right">
                                            <label class="active" for="lblApprovalStatus">Approval Status</label>
                                            <asp:Label ID="lblApprovalStatus" runat="server" class="form-control width-70 red-text" ClientIDMode="Static" TabIndex="4" placeholder=""/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m3 pr-0">
                                            <label class="active" for="txtStartDate">Start Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpFromMonth" runat="server" CssClass="select2-theme browser-default width-40 mr-2 float-left" ClientIDMode="Static" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="txtLoanAmount_TextChanged"></asp:DropDownList>
                                            <asp:DropDownList ID="drpFromYear" runat="server" CssClass="select2-theme browser-default width-40 mr-2 float-left" ClientIDMode="Static" TabIndex="6" AutoPostBack="true" OnSelectedIndexChanged="txtLoanAmount_TextChanged"></asp:DropDownList>
                                        </div>
                                        <div id="divEndDate" runat="server" class="input-field col m3 pr-0">
                                            <label class="active" for="drpToMonth">Due Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpToMonth" runat="server" CssClass="select2-theme browser-default width-35 mr-2 float-left" ClientIDMode="Static" TabIndex="7" AutoPostBack="true" OnSelectedIndexChanged="txtLoanAmount_TextChanged"></asp:DropDownList>
                                            <asp:DropDownList ID="drpToYear" runat="server" CssClass="select2-theme browser-default width-35 mr-2 float-left" ClientIDMode="Static" TabIndex="8" AutoPostBack="true" OnSelectedIndexChanged="txtLoanAmount_TextChanged"></asp:DropDownList>
                                        </div>
                                        <div id="divInstallment1" runat="server" class="input-field col m2 pr-0">
                                            <label class="active" for="txtNoOfInstallments">Total Month <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtNoOfInstallments" Enabled="false" runat="server" class="form-control" ClientIDMode="Static" TabIndex="9" placeholder="" style="width:100px;"/>
                                        </div>
                                        <div id="divInstallment2" runat="server" class="input-field col m2 pr-0">
                                            <label class="active" for="txtInstallmentAmount">Loan Installment<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtInstallmentAmount" Enabled="false" runat="server" class="form-control" ClientIDMode="Static" TabIndex="10" placeholder="" style="width:150px;"/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <label class="active" for="txtRemarks">Remarks<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="11" placeholder="" TextMode="MultiLine" Rows="3" />
                                        </div>
                                    </div>
                                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                        <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="13"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                        <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="14"><i class="material-icons left">save</i>Save</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--**************************** Employee Popup ***********************************--%>
    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
    </cc1:ModalPopupExtender>  
    <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
        <div id="myModal1215">
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
                
    </form>
</body>
</html>

