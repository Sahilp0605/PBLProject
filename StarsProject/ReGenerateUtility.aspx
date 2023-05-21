<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ReGenerateUtility.aspx.cs" Inherits="StarsProject.ReGenerateUtility" %>

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
        .card-title {
            color: #1e90ff;
        }

        .mydropdowncontent {
            width: 230px !important;
            height: auto !important;
        }

        .mydropdowncontent li {
            min-height: auto;
            padding: 0px;
        }

        .mydropdowncontent li > a {
            min-height: auto;
            padding: 2px;
            font-size: 12px;
            color: navy;
            padding: 3px 5px;
            background-color: #f3ebeb;
        }

        .dataTables_filter, .dataTables_info, .dataTables_length {
            display: none;
        }

        table.dataTable thead th, table.dataTable thead td, {
            padding: 5px 5px;
            font-size: 12px;
        }

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 5px 5px;
            font-size: 11px;
        }

        .myDropDownClass {
            background-color: #fff6e1;
            color: navy;
            font-size: medium;
            width: 150px;
            height: 30px !important;
            border: 1px solid black;
            margin-top: 1px;
            padding: 1px 5px;
            border: none;
            border-radius: 5px;
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

        .titleHead {
            font-weight: 800;
            margin-left: 10px;
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
            background: linear-gradient(45deg, #303f9f, #7b1fa2) !important;
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
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
            //$('.timepicker').timepicker();
            hideDivLocation();
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
        function showFileExtError(xMode) {
            if (xMode == 'image')
                alert("Only BMP, JPEG, GIF and PNG file is valid for Attachment !");
        }

        function hideDivLocation() {
            if ($("#drpExpenseType").val() == 1)
                $("#divLocation").show();
            else
                $("#divLocation").hide();
        };

    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div class="row">
            <!-- Re-Generate Ledger -->
            <div class="col m6" style="padding-right: 5px;">
                <asp:UpdatePanel ID="updLedger" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="Add New ToDO" href="#" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">ac_unit</i></a>
                                <h5 class="h5Round">
                                    <span id="spnPageHeader" runat="server" clientidmode="Static" class="titleHead text-top">Re-Generate : Trial Balance
                                    &nbsp;<span id="spnTrialCount" runat="server" clientidmode="Static" class="badge gradient-45deg-yellow-green black-text" style="line-height: 25px; float: none; line-height: 25px; vertical-align: text-top; padding: 5px 10px; border-radius: 8px;"></span>
                                    </span>
                                    <button ID="btnSaveTrial" type="button" runat="server" clientidmode="Static" text="Save" class="btn blue right mr-1" onserverclick="btnSaveTrial_Click" tabindex="8" style="width:150px;">ReGenerate</button>
                                </h5>
                                <div style="width: 100%; min-height: 590px; max-height: 590px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <table id="tblTrial" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <asp:Repeater ID="rptTrialBalance" runat="server" ClientIDMode="Static">
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th class="left-align" style="width: 40%; color: white;">Customer Name</th>
                                                        <th class="right-align" style="width: 10%; color: white;">Opening</th>
                                                        <th class="right-align" style="width: 10%; color: white;">Debit</th>
                                                        <th class="right-align" style="width: 10%; color: white;">Credit</th>
                                                        <th class="right-align" style="width: 10%; color: white;">Closing</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%#Eval("CustomerID") %>' />
                                                    <td class="text-center">
                                                        <asp:Label ID="lblEmployeeName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="101" MaxLength="250" Text='<%#Eval("CustomerName") %>' />
                                                    </td>
                                                    <td class="right-align">
                                                        <asp:Label ID="txtTaskDescription" runat="server" class="form-control" ClientIDMode="Static" TabIndex="102"  MaxLength="250" Text='<%#Eval("OpeningAmount") %>' />
                                                    </td>
                                                    <td class="right-align">
                                                        <asp:Label ID="lblTaskCategory" runat="server" class="form-control" ClientIDMode="Static" TabIndex="103" Width="80px" Text='<%#Eval("DebitAmount") %>' />
                                                    </td>
                                                    <td class="right-align">
                                                        <asp:Label ID="txtTaskDuration" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='<%#Eval("CreditAmount") %>' />
                                                    </td>
                                                    <td class="right-align">
                                                        <asp:Label ID="Label1" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='<%#Eval("ClosingAmount") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td><h5>Trial Balance Difference :</h5></td>
                                            <td class="padding-1 right-align">
                                                <asp:Label ID="lblTrialOpening" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='' />
                                            </td>
                                            <td class="padding-1 right-align">
                                                <asp:Label ID="lblTrialDebit" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='' />
                                            </td>
                                            <td class="padding-1 right-align">
                                                <asp:Label ID="lblTrialCredit" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='' />
                                            </td>
                                            <td class="padding-1 right-align">
                                                <asp:Label ID="lblTrialClosing" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='' />
                                            </td>
                                        </tr>
                                    </table>                                    
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Re-Generate Stock Ledger -->
            <div class="col m6" style="padding-left: 5px;">
                <asp:UpdatePanel ID="updDailyFollowup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <a id="lndAddFollowUp" class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New FollowUp" href="#" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">ac_unit</i></a>
                                <h5 class="h5Round">
                                    <span id="Span1" runat="server" clientidmode="Static" class="titleHead text-top">Re-Generate : Stock
                                    &nbsp;<span id="spnStockCount" runat="server" clientidmode="Static" class="badge gradient-45deg-yellow-green black-text" style="line-height: 25px; float: none; line-height: 25px; vertical-align: text-top; padding: 5px 10px; border-radius: 8px;"></span>
                                    </span>
                                    <button ID="btnSaveStock" type="button" runat="server" clientidmode="Static" text="Save" class="btn blue right mr-1" onserverclick="btnSaveStock_Click" tabindex="8" style="width:150px;">ReGenerate</button>
                                </h5>
                                <div style="width: 100%; min-height: 590px; max-height: 590px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <table id="tblStock" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <asp:Repeater ID="rptStock" runat="server" ClientIDMode="Static">
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th class="text-left">Product Name</th>
                                                        <th class="text-cetner" style="width: 10%;">Opening</th>
                                                        <th class="text-cetner" style="width: 10%;">Inward</th>
                                                        <th class="text-cetner" style="width: 10%;">Outward</th>
                                                        <th class="text-center" style="width: 10%;">Closing</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                    <td class="text-center">
                                                        <asp:Label ID="lblEmployeeName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="101" MaxLength="250" Text='<%#Eval("ProductName") %>' />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:Label ID="txtTaskDescription" runat="server" class="form-control" ClientIDMode="Static" TabIndex="102"  MaxLength="250" Text='<%#Eval("OpeningSTK") %>' />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:Label ID="lblTaskCategory" runat="server" class="form-control" ClientIDMode="Static" TabIndex="103" Width="80px" Text='<%#Eval("InwardSTK") %>' />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:Label ID="txtTaskDuration" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='<%#Eval("OutwardSTK") %>' />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:Label ID="Label1" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='<%#Eval("ClosingSTK") %>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>                                    
                                    
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>



