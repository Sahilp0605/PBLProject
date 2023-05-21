<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShiftMaster.aspx.cs" Inherits="StarsProject.ShiftMaster" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link  rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>
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
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
            $('.timepicker').timepicker();
        });
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', displayLength:4000 });
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
        //    if (jQuery("#txtMessage").val() == "" || jQuery("#txtFromDate").val() == "" || jQuery("#txtToDate").val() == "") {

        //        if (jQuery("#txtMessage").val() == "")
        //            errmsg += "<li>Message is required</li>";

        //        if (jQuery("#txtFromDate").val() == "")
        //            errmsg += "<li>From Date is required</li>";

        //        if (jQuery("#txtToDate").val() == "")
        //            errmsg += "<li>To Date is required</li>";

        //        if (Date.parse($("#txtFromDate").val()) > Date.parse($("#txtToDate").val()))
        //            errmsg += "<li>From Date is Always Less then To Date</li>";

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    if (validflag == false)
        //        showcaseError(errmsg);
        //    // ----------------------------------------
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
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <%--<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>

                    <div class="widgetbox">
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnShiftCode" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnShiftName" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnStartTime" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnEndTime" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnGraceMins" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnShiftHours" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnMinHrsHalfDay" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnMinHrsFullDay" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtMessage">Shift Name<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtShiftName" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" TabIndex="1" TextMode="SingleLine" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtMessage">Start Time<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtStartTime" runat="server" class="timepicker" PlaceHolder="StartTime" ClientIDMode="Static" Width="150px" TabIndex="2" onkeypress="return false;" onpaste="return false" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtMessage">End Time<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtEndTime" runat="server" class="timepicker" PlaceHolder="EndTime" ClientIDMode="Static" Width="150px" TabIndex="3" onkeypress="return false;" onpaste="return false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtMessage">Grace Minites<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtGraceMins" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" TabIndex="4" TextMode="SingleLine" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtMessage">Minimum Hours Of Half Day<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtMinHrsHalfDay" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" TabIndex="5" TextMode="SingleLine" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtMessage">Minimum Hours Of Full Day<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtMinHrsFullDay" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" TabIndex="6" TextMode="SingleLine" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtMessage">Lunch From<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtLunchFrom" runat="server" class="timepicker" PlaceHolder="LunchFrom" ClientIDMode="Static" TabIndex="7" onkeypress="return false;" onpaste="return false" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtMessage">Lunch To<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtLunchTo" runat="server" class="timepicker" PlaceHolder="LunchTo" ClientIDMode="Static" TabIndex="8" onkeypress="return false;" onpaste="return false" />
                                        </div>
                                    </div>

                                    <%--<div class="row">
                                        <div class="input-field col m6">
                                            <asp:TextBox ID="txtFromDate" runat="server" class="form-control" PlaceHolder="From Date" ClientIDMode="Static" Width="250px" TabIndex="2" onkeypress="return false;" onpaste="return false" TextMode="Date" />
                                            <asp:TextBox ID="txtFromTime" runat="server" class="timepicker" PlaceHolder="From Time" ClientIDMode="Static" Width="150px" TabIndex="2" onkeypress="return false;" onpaste="return false" />
                                            <label class="active" for="txtFromDate">From Date <span class="materialize-red-text font-weight-800">*</span></label>
                                        </div>
                                        <div class="input-field col m6">
                                            <asp:TextBox ID="txtToDate" runat="server" class="form-control" PlaceHolder="To Date" ClientIDMode="Static" Width="250px" TabIndex="3" onkeypress="return false;" onpaste="return false"  TextMode="Date"/>
                                            <asp:TextBox ID="txtToTime" runat="server" class="timepicker" PlaceHolder="To Time" ClientIDMode="Static" Width="150px" TabIndex="3" onkeypress="return false;" onpaste="return false" />
                                            <label class="active" for="txtToDate">To Date <span class="materialize-red-text font-weight-800">*</span></label>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
                        <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                            <button ID="btnReset" type="button" runat="server" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="10"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <button ID="btnSave" type="button" runat="server" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="9"><i class="material-icons left">save</i>Save</button>
                        </div>
                    </div>
                <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </form>
</body>
</html>


