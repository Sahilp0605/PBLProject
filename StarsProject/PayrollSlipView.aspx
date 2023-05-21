<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayrollSlipView.aspx.cs" Inherits="StarsProject.PayrollSlipView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
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

        .card-title {
            background: linear-gradient(45deg, #303f9f, #7b1fa2) !important;
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
        jQuery(document).ready(function () {
        });

        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">

            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <%-- Bootstrap Quotation Modal Popup --%>
                        <div class="clearall"></div>
                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />\

                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="card-title" style="padding: 10px;">
                                    <h5 class="card-title mb-0" style="color: white">Pay Slip
                                        <asp:Label CssClass="float-right" ID="lblBasicPer" runat="server" ClientIDMode="Static" />
                                    </h5>
                                </div>
                                <br />
                                <div class="modal-body">
                                    <div class="row badge-light" style="border-radius: 5px;">
                                        <div class="input-field col m3">
                                            <label class="active" for="drpEmployee">Employee Name</label>
                                            <asp:DropDownList ID="drpEmployee" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged" />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtPayDate">Payroll For</label>
                                            <asp:TextBox ID="txtPayDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" TextMode="Date"  placeholder=" " AutoPostBack="true" OnTextChanged="txtPayDate_TextChanged" />
                                        </div>
                                        <div class="input-field col m1">
                                            <label class="active" for="txtWDays">Month Days</label>
                                            <asp:TextBox ID="txtWDays" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" ReadOnly="true" />
                                        </div>
                                        <div class="input-field col m1">
                                            <label id="lblPDays" runat="server" class="active" for="txtPDays">Pres.Days</label>
                                            <asp:TextBox ID="txtPDays" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" placeholder="" AutoPostBack="true" OnTextChanged="txtPDays_TextChanged" />
                                        </div>
                                        <div class="input-field col m1">
                                            <label class="active" for="txtHDays">Paid Days</label>
                                            <asp:TextBox ID="txtHDays" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" placeholder="" AutoPostBack="true"  OnTextChanged="txtPDays_TextChanged" />
                                        </div>
                                        <div class="input-field col m1">
                                            <label class="active" for="txtLDays">Leave.Days</label>
                                            <asp:TextBox ID="txtLDays" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" AutoPostBack="true" OnTextChanged="txtPDays_TextChanged" />
                                        </div>
                                        <div class="input-field col m1">
                                            <label id="lblTotDays" runat="server" class="active" for="txtTotDays">Net Days</label>
                                            <asp:TextBox ID="txtTotDays" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" ReadOnly="true" />
                                        </div>
                                        <div class="input-field col m1">
                                            <label class="active" for="txtHDays">O.T Hrs</label>
                                            <asp:TextBox ID="txtODays" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" placeholder="" AutoPostBack="true"  OnTextChanged="txtPDays_TextChanged" />
                                        </div>
                                        <div class="input-field col m1">
                                            <label class="active" for="txtFixedSalary">Fixed Salary</label>
                                            <asp:TextBox ID="txtFixedSalary" runat="server" class="form-control" ClientIDMode="Static" TabIndex="5" placeholder="" ReadOnly="true"/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <div class="card">
                                                <div class="card-title" style="padding: 10px;">
                                                    <h5 class="card-title mb-0 left-align" style="color: white">Income
                                                        <asp:TextBox ID="txtTotal_Income" runat="server" Style="width: 20%; float: right; margin: -4px -50px 0px 0px; color: white; border: none;" class="form-control" ClientIDMode="Static" ReadOnly="true" placeholder=" " />
                                                    </h5>

                                                </div>
                                                <div class="card-body">
                                                    <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtBasic">Basic</label>
                                                            <asp:TextBox ID="txtBasic" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" placeholder=" " AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtHRA">House Rent Allow</label>
                                                            <asp:TextBox ID="txtHRA" runat="server" class="form-control" ClientIDMode="Static" TabIndex="7" placeholder=" "  AutoPostBack="true" OnTextChanged="txtHRA_TextChanged" />
                                                        </div>
                                                    </div>
                                                    <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtConveyance">Conveyance</label>
                                                            <asp:TextBox ID="txtConveyance" runat="server" class="form-control" ClientIDMode="Static" TabIndex="8" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtMedical">Medical Allow.</label>
                                                            <asp:TextBox ID="txtMedical" runat="server" class="form-control" ClientIDMode="Static" TabIndex="9" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                    </div>
                                                    <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtDA">Dearness Allow.</label>
                                                            <asp:TextBox ID="txtDA" runat="server" class="form-control" ClientIDMode="Static" TabIndex="10" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtSpecial">Special Allow.</label>
                                                            <asp:TextBox ID="txtSpecial" runat="server" class="form-control" ClientIDMode="Static" TabIndex="11" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                    </div>
                                                    <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtOverTime">O.T Allow.</label>
                                                            <asp:TextBox ID="txtOverTime" runat="server" class="form-control" ClientIDMode="Static" TabIndex="10" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="input-field col m6">
                                            <div class="card">
                                                <div class="card-title" style="padding: 10px;">
                                                    <h5 class="card-title mb-0" style="color: white">Deduction
                                                        <asp:TextBox ID="txtTotal_Deduct" runat="server" class="form-control" Style="width: 20%; float: right; margin: -4px -50px 0px 0px; color: white; border: none;" ClientIDMode="Static" ReadOnly="true" placeholder=" " />
                                                    </h5>
                                                </div>
                                                <div class="card-body">
                                                    <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtPT">P.F</label>
                                                            <asp:TextBox ID="txtPF" runat="server" class="form-control" ClientIDMode="Static" TabIndex="12" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtTDS">E.S.I.C</label>
                                                            <asp:TextBox ID="txtESI" runat="server" class="form-control" ClientIDMode="Static" TabIndex="13" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                    </div>
                                                    <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtPT">Professional Tax</label>
                                                            <asp:TextBox ID="txtPT" runat="server" class="form-control" ClientIDMode="Static" TabIndex="12" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtTDS">Tax Deduction Scheme</label>
                                                            <asp:TextBox ID="txtTDS" runat="server" class="form-control" ClientIDMode="Static" TabIndex="13" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                    </div>
                                                    <div class="row" style="padding: 15px 0px 0px 0px; width: 100%;">
                                                        <div class="input-field col m6">
                                                            <label class="active" for="txtPT">Loan Installment/Upad</label>
                                                            <asp:TextBox ID="txtLoan" runat="server" class="form-control" ClientIDMode="Static" TabIndex="12" placeholder=" "  AutoPostBack="true" OnTextChanged="EarningDeduction_TextChanged" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="card">
                                    <div class="card-title" style="padding: 10px;">
                                        <h5 class="card-title right-align mb-0" style="color: white;">
                                            Net Payable : <asp:TextBox ID="txtNetSalary" runat="server" class="form-control left-align white-text border-none" Style="width: 20%; margin: -4px -50px 0px 0px;" ClientIDMode="Static" ReadOnly="true" placeholder=" " />
                                        </h5>
                                    </div>
                                </div>
                            </div>
                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom">
                                <button id="btnReset" type="button" runat="server" clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="16"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button id="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="17"><i class="material-icons left">save</i>Save</button>
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

