<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Expense.aspx.cs" Inherits="StarsProject.Expense" %>

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
                        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m2">
                                            <label class="active required" for="txtExpenseDate">Expense Date</label>
                                            <asp:TextBox ID="txtExpenseDate" runat="server" class="form-control form-control" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" TabIndex="1" placeholder="" TextMode="Date"/>
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active required" for="drpExpenseType">Expense Type</label>
                                            <asp:DropDownList ID="drpExpenseType" runat="server" class="select2-theme browser-default" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="drpExpenseType_SelectedIndexChanged" />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active required" for="txtChargeName">Amount </label>
                                            <asp:TextBox ID="txtAmount" runat="server" ClientIDMode="Static" class="form-control" TabIndex="3" MaxLength="15" placeholder="" />
                                        </div>
                                        <% if (hdnSerialKey.Value == "SO5H-DH90-E34L-SIOF")
                                            {%>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtCustomer">Customer </label>
                                            <asp:TextBox ID="txtCustomer" runat="server" ClientIDMode="Static" class="form-control" TabIndex="4" MaxLength="15" placeholder="" />
                                        </div>
                                        <%}%>
                                    </div>
                                    <div class="row" runat="server" id="divLocation" visible="false">
                                        <div class="input-field col m5">
                                            <label class="active required" for="txtFromLocation">From Location</label>
                                            <asp:TextBox ID="txtFromLocation" runat="server" ClientIDMode="Static" MaxLength="100" class="form-control" TabIndex="5" placeholder="" />
                                        </div>
                                        <div class="input-field col m5">
                                            <label class="active required" for="txtToLocation">To Location</label>
                                            <asp:TextBox ID="txtToLocation" runat="server" ClientIDMode="Static" MaxLength="100" class="form-control" TabIndex="6" placeholder="" />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtDistanceCovered">Distance Covered(in km)</label>
                                            <asp:TextBox ID="txtDistanceCovered" runat="server" ClientIDMode="Static" MaxLength="15" class="form-control" TabIndex="7" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <label class="active required" for="txtGSTPer">Expense Note</label>
                                            <asp:TextBox ID="txtExpenseNote" runat="server" ClientIDMode="Static" MaxLength="250" class="form-control materialize-textarea" TabIndex="8" TextMode="MultiLine" Rows="3" placeholder="" style="min-height:80px;" />
                                        </div>
                                    </div>
                                    <div class="row">
                                    <div class="col m12">
                                    <asp:UpdatePanel ID="updproddocs" runat="server" UpdateMode="Conditional" style="border:double; padding:15px !important; background-color: aliceblue;">
                                        <ContentTemplate>
                                            <div class="container" style="width:100%;">
                                                <h5 class="h5RoundHead">
                                                    <span id="Span1" runat="server" clientidmode="Static" class="text-top" style="color: white;">Upload Expense Voucher Image</span>
                                                </h5>
                                                <div class="row" style="padding:10px;">
                                                    <div class="col m5" style="height:300px; padding: 10px; box-shadow: black -6px 8px 6px -6px; background-color: white;">
                                                        <div class="row" style="width:100%;">
                                                            <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" TabIndex="9" />
                                                        </div>
                                                        <div class="row" style="height:230px; width:100%; overflow-y: auto;">
                                                            <ul class="padding-1">
                                                                <asp:Repeater ID="rptFileListCtrl" runat="server" ClientIDMode="Static" OnItemCommand="rptFileListCtrl_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <li class="col m12 mb-2 pl-0 pr-0">
                                                                            <div class="flip-card" style="width: 100%;">
                                                                                <div class="flip-card-inner" style="width: 100%;">
                                                                                    <div class="flip-card-front" style="width: 70%;">
                                                                                        <img src="images/pdf_document.png" alt="Avatar" style="width: 60px; height: 65px;" />
                                                                                        <br />
                                                                                        <small><%# Eval("Name")%></small>
                                                                                    </div>
                                                                                    <div style="float: right;">
                                                                                        <ul>
                                                                                            <li class="m-1 mt-3">
                                                                                                <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="images/buttons/btnDelete.png" Width="120" Height="30" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' />
                                                                                            </li>
                                                                                            <li class="m-1">
                                                                                                <asp:ImageButton ID="ImgBtnPerview" runat="server" ImageUrl="images/buttons/btnPreview.png" Width="120" Height="30" CommandName="Preview" CommandArgument='<%# Eval("pkID") %>' />
                                                                                            </li>
                                                                                        </ul>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </li>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                    <div class="col m7" style="height:300px; padding: 10px; box-shadow: black -6px 8px 6px -6px; background-color: white;">
                                                        <iframe id="frameDoc" runat="server" clientidmode="Static" src="images/buttons/Preview.jpg" frameborder="0" style="width:100%; height:100%; overflow:hidden;"></iframe>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                                    </div>
                                </div>
                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                    <button ID="btnReset" type="button" runat="server"  clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" TabIndex="11"><i class="material-icons left">backspace</i>Clear & Add New</button>
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


