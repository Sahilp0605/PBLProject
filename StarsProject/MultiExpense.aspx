<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiExpense.aspx.cs" Inherits="StarsProject.MultiExpense" %>

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
        .cardRound {
            box-shadow: black -6px 8px 6px -6px !important;
            border-radius: 8px 8px 0px 0px !important;
        }

        .h5Round {
            border-radius: 8px 8px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, silver, #7b2fa2);
        }

        .h5RoundHead {
            border-radius: 0px 0px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, silver, #7f88ec6e);
        }

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

        .Cntrl1 {
            background-color: #cddcea;
            color: Black;
            border: 1px ridge silver;
            font: Verdana 10px;
            padding: 3px 4px;
            font-family: Palatino Linotype, Arial, Helvetica, sans-serif;
            width: -webkit-fill-available;
        }
        /* =================================================================================== */
        .row.heading h2 {
            color: #fff;
            font-size: 52.52px;
            line-height: 95px;
            font-weight: 400;
            text-align: center;
            margin: 0 0 40px;
            padding-bottom: 20px;
            text-transform: uppercase;
        }

        ul {
            margin: 0;
            padding: 0;
            list-style: none;
        }

        .heading.heading-icon {
            display: block;
        }

        .padding-lg {
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
        }

        .practice-area.padding-lg {
            padding-bottom: 55px;
            padding-top: 55px;
        }

        .practice-area .inner {
            border: 1px solid #999999;
            text-align: center;
            margin-bottom: 28px;
            padding: 40px 25px;
        }

        .our-webcoderskull .cnt-block:hover {
            box-shadow: 0px 0px 10px rgba(0,0,0,0.3);
            border: 0;
        }

        .practice-area .inner h3 {
            color: #3c3c3c;
            font-size: 24px;
            font-weight: 500;
            font-family: 'Poppins', sans-serif;
            padding: 10px 0;
        }

        .practice-area .inner p {
            font-size: 14px;
            line-height: 22px;
            font-weight: 400;
        }

        .practice-area .inner img {
            display: inline-block;
        }

        .our-webcoderskull {
            /*background: url("http://www.webcoderskull.com/img/right-sider-banner.png") no-repeat center top / cover;*/
            background-color: #fbf5f5;
        }

            .our-webcoderskull .cnt-block {
                float: left;
                width: 100%;
                background: #fff;
                padding: 30px 20px;
                text-align: center;
                border: 2px solid #d5d5d5;
                margin: 0 0 28px;
            }

                .our-webcoderskull .cnt-block figure {
                    width: 80px;
                    height: 80px;
                    border-radius: 100%;
                    display: inline-block;
                    margin-bottom: 15px;
                }

                .our-webcoderskull .cnt-block img {
                    width: 80px;
                    height: 80px;
                    border-radius: 100%;
                }

                .our-webcoderskull .cnt-block h3 {
                    color: #2a2a2a;
                    font-size: 20px;
                    font-weight: 500;
                    padding: 6px 0;
                    text-transform: uppercase;
                }

                    .our-webcoderskull .cnt-block h3 a {
                        text-decoration: none;
                        color: #2a2a2a;
                    }

                        .our-webcoderskull .cnt-block h3 a:hover {
                            color: #337ab7;
                        }

                .our-webcoderskull .cnt-block p {
                    color: #2a2a2a;
                    font-size: 13px;
                    line-height: 20px;
                    font-weight: 400;
                }

                .our-webcoderskull .cnt-block .follow-us {
                    margin: 20px 0 0;
                }

                    .our-webcoderskull .cnt-block .follow-us li {
                        display: inline-block;
                        width: auto;
                        margin: 0 5px;
                    }

                        .our-webcoderskull .cnt-block .follow-us li .fa {
                            font-size: 24px;
                            color: #767676;
                        }

                            .our-webcoderskull .cnt-block .follow-us li .fa:hover {
                                color: #025a8e;
                            }

        .nav-tabs .nav-item.show .nav-link, .nav-tabs .nav-link.active {
            background-color: navy;
            color: White;
        }

        #myTabContent {
            border-left: 1px solid navy;
            border-right: 1px solid navy;
            border-bottom: 1px solid navy;
        }

        .flip-card {
            background-color: transparent;
            width: 450px;
            height: 100px;
            margin-bottom: 5px;
            perspective: 1000px;
        }

        .flip-card-inner {
            position: relative;
            width: 100%;
            height: 100%;
            text-align: center;
            transition: transform 0.6s;
            transform-style: preserve-3d;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
        }

        /*.flip-card:hover .flip-card-inner {
          transform: rotateY(180deg);
        }*/

        .flip-card-front, .flip-card-back {
            position: absolute;
            width: 100%;
            height: 100%;
            backface-visibility: hidden;
        }

        .flip-card-front {
            background-color: white;
            color: black;
            padding-top: 10px;
            border-radius: 4px;
        }

        .flip-card-back {
            background-color: #2980b9;
            color: white;
            transform: rotateY(180deg);
            border-radius: 4px;
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

                /*.checkbox-container input:checked + label + .active-circle {
	        transform: translate(-50%, -50%) scale(15);
        }*/

                .checkbox-container input:checked + label::after {
                    transform: translateX(calc(100% + 0.5px));
                    background-color: forestgreen;
                }

        .active-circle {
            border-radius: 50%;
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(calc(-50% - 10px), calc(-50% - 2px)) scale(0);
            transition: transform 0.6s ease-out;
            width: 30px;
            height: 30px;
            z-index: 1;
        }

        /* -----https://codepen.io/FlorinPop17/pen/MxPXOL */

        .required:after {
            content: " *";
            color: #f00;
        }
        /* ------------------------------------------------------- */
        /*::-webkit-scrollbar {  width: 10px; }
        ::-webkit-scrollbar-track { background: #f1f1f1;  }
        ::-webkit-scrollbar-thumb { background: #888;  }
        ::-webkit-scrollbar-thumb:hover { background: #555; }*/

        ::-webkit-scrollbar {
            width: 12px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
            -webkit-border-radius: 10px;
            border-radius: 10px;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            -webkit-border-radius: 10px;
            border-radius: 10px;
            background-color: silver; /*rgba(255,0,0,0.8); */
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.5);
        }

            ::-webkit-scrollbar-thumb:window-inactive {
                background: rgba(255,0,0,0.4);
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
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        //function validateForm() {
        //    var errmsg = "";
        //    var validflag = true;
        //    // ----------------------------------------
        //    if (jQuery("#txtChargeName").val().trim() == "") {

        //        if (jQuery("#txtChargeName").val().trim() == "")
        //            errmsg += "Charge Name is required</br>";


        //        jQuery("#divErrorMessage").html(errmsg).show();

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    if (validflag == true) {
        //        btnCloseDialog();
        //    }
        //    return validflag;
        //}

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
        function clearFormData() {
            //$(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnpkID").val('');
            jQuery("#FileUpload1").val("");
            jQuery("#imgProduct").attr("src", "~/images/no-figure.png");
            jQuery("#divImageControl").hide();

        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
<%--            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
                    <div class="widgetbox">
                        <asp:HiddenField ID="hdnExpenseID" runat="server" ClientIDMode="Static" />
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <label class="active required" for="txtExpenseDate">Expense Date</label>
                                            <asp:TextBox ID="txtExpenseDate" runat="server" class="form-control form-control" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" TabIndex="1" placeholder="" TextMode="Date"/>
                                        </div>
                                        <div class="input-field col m8">
                                            <label class="active" for="txtGSTPer">Expense Note</label>
                                            <asp:TextBox ID="txtExpenseNote" runat="server" ClientIDMode="Static" MaxLength="250" class="form-control materialize-textarea" TabIndex="7" TextMode="MultiLine" Rows="3" placeholder="" style="min-height:80px;" />
                                        </div>
                                    </div>
                                </div>
                                <div id="pnl_Expenses" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true">
                                    <asp:Panel runat="server" ID="pnlDetail">
                                    <table id="tblExpenses" class="table responsive-table striped " cellpadding="0" cellspacing="0" border="0">
                                        <asp:Repeater ID="rptExpenseDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptExpenseDetail_ItemCommand" OnItemDataBound="rptExpenseDetail_ItemDataBound">
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th class="Center-align" style="width: 20%;">Expenses <span class="materialize-red-text font-weight-800">*</span></th>
                                                        <th class="center-align" style="width: 10%; padding: 0px 8px 0px 0px;">Expense Date</th>
                                                        <th class="center-align" style="width: 10%; padding: 0px 8px 0px 0px;">Amount</th>
                                                        <th class="center-align" style="width: 20%; padding: 0px 8px 0px 0px;">Remarks</th>
                                                        <th class="center-align" style="width: 10%; padding: 0px 8px 0px 0px;">Voucher</th>
                                                        <th class="center-align" style="width: 30%; padding: 0px 8px 0px 0px;">Action</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnFromLocation" runat="server" ClientIDMode="Static" Value='<%# Eval("FromLocation") %>' />
                                                <asp:HiddenField ID="hdnVoucher" runat="server" ClientIDMode="Static" Value='<%# Eval("Voucher") %>' />
                                                <tr>
                                                    <td class="right-align" style="width: 20%;">
                                                        <asp:TextBox ID="edExpense" Enabled ="false" MaxLength="100" runat="server" ClientIDMode="Static" Text='<%# Eval("ExpenseTypeName") %>' AutoPostBack="true" Style="width: 200px; text-align: left;" TabIndex="12" />
                                                    </td>
                                                     <td class="right-align" style="width: 20%;">
                                                        <asp:TextBox ID="edExpenseDateDetail" runat="server" class="form-control form-control" ClientIDMode="Static" Text='<%# Eval("ExpenseDateDetail", "{0:yyyy-MM-dd}") %>'  onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date"/>
                                                    </td>
                                                    <td class="right-align" style="width: 10%;">
                                                        <asp:TextBox ID="edAmount" Enabled ="false" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' AutoPostBack="true" Style="width: 70px; text-align: right;" TabIndex="13" />
                                                    </td>
                                                    <td class="right-align" style="width: 20%;">
                                                        <asp:TextBox ID="edRemarks" Enabled ="false" MaxLength="500" runat="server" ClientIDMode="Static" Text='<%# Eval("Remarks") %>' AutoPostBack="true" Style="width: 500px; text-align: left;" TabIndex="14" />
                                                    </td>
                                                    <td>
                                                        <img id="imgVoucher" alt="" runat="server" clientidmode="Static" src='' width="20" height="50" />
                                                    </td> 
                                                    <td class="center-align" style="width: 10%;">
                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" OnClientClick="return delClientClick()" TabIndex="20" />
                                                    </td>                                                         
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <tr style="background-color: #dde8f3;">
                                                    <td class="right-align" style="width: 8%;">
                                                        <asp:DropDownList ID="drpExpense" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="5" />
                                                    </td>
                                                    <td class="right-align" style="width: 20%;">
                                                        <asp:TextBox ID="txtExpenseDateDetail" runat="server" class="form-control form-control" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date"/>
                                                    </td>
                                                    <td class="center-align" style="width: 1%;">
                                                        <asp:TextBox CssClass="right-align" MaxLength="15" ID="txtAmount" runat="server" ClientIDMode="Static" Width="70" TabIndex="26" />
                                                    </td>
                                                    <td class="left-align" style="width: 10%;">
                                                        <asp:TextBox CssClass="Left-align" MaxLength="500" ID="txtRemark" runat="server" ClientIDMode="Static" Width="500" TabIndex="27" />
                                                    </td>
                                                    <td class="center-align" style="width: 30%;">
                                                        <div class="app-file-content">
                                                            <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="" /> 
                                                            <asp:Button ID="Button2" CssClass="hide" runat="server" Text="Upload" TabIndex="48" />
                                                        </div>
                                                    </td>
                                                    <td class="center-align" colspan="2" style="width: 20%;">
                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="110" Height="30" TabIndex="35" />
                                                    </td>
                                                </tr>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </asp:Panel>
                                </div>
                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                    <button ID="btnReset" type="button" runat="server"  clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" TabIndex="9"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                    <button ID="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="10"><i class="material-icons left">save</i>Save</button>
                                </div>
                            </div>
                        </div>
                    </div>
<%--                </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </form>
</body>
</html>



