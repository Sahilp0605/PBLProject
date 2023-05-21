<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="DailyAttendance.aspx.cs" Inherits="StarsProject.DailyAttendance" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        .dayHeader {
            font-size:14px;
            font-weight:bold;
            background-color:navy;
            color:white !important;
            text-align:center;
        }
        .dateHeader {
            font-size:14px;
            font-weight:bold;
            background-color:white;
            color:navy !important;
            text-align:center;
            vertical-align:top;        
        }
        .headWEEK { background-color:White; color:navy; font-size:16px; font-weight:bold; }
        .headOUT { background-color:White; color:maroon; font-size:14px; }

        .timeInputDisable {
            width:120px !important;
            text-align:center;
            disabled:true;
            background-color: #5284de;
            border-radius: 7px;
            color: white;
            font-size:12px;
            font-weight:normal;
        }

        .timeInput {
            width:120px !important;
            text-align:center;
            disabled:false;
            background-color: white;
            border-radius: 7px;
            color: black;
            font-size:12px;
            font-weight:normal;
        }

        .timeLeaveRequest {
            width:120px !important;
            text-align:center;
            disabled:true;
            border-radius: 7px;
            background: repeating-linear-gradient(90deg, antiquewhite, antiquewhite 1px, silver 10px, silver 1px);
        }

        .timeHoliday {
            border-radius: 7px;
            text-align:center;
            disabled:true;
            font-size:14px !important;
            font-weight:bold !important;
            background-color:#cd5c5c !important;
            color:white !important;
            /*background: repeating-linear-gradient(90deg, antiquewhite, antiquewhite 1px, silver 10px, silver 1px);*/
        }
        .timeHoliday:disabled {
            /*width:120px !important;*/
            border-radius: 7px;
            text-align:center;
            disabled:true;
            font-size:14px !important;
            font-weight:bold !important;
            background-color:#cd5c5c !important;
            color:white !important;
            /*background: repeating-linear-gradient(90deg, antiquewhite, antiquewhite 1px, silver 10px, silver 1px);*/
        }
        .bottomRow {
            border-bottom: 5px solid blue;
        }

        .diagonalStrips { 
            background: repeating-linear-gradient(45deg, #606dbc, #606dbc 5px, #465298 10px, #465298 5px); 
            text-align:center;
            color: #ffff8d !important;
        }

        .stdtable tr:hover { background-color:none; }

        /*<%--Preview-Add EmployeeList on Employee Name Dropdown by Vikram Rajput 13-07-2020--%>*/
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
            height: 85%;
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
        .timepicker { text-align:center; width:120px !important; }

        input:not([type]), input[type=text]:not(.browser-default), input[type=password]:not(.browser-default), input[type=email]:not(.browser-default), input[type=url]:not(.browser-default), input[type=time]:not(.browser-default), input[type=date]:not(.browser-default), input[type=datetime]:not(.browser-default), input[type=datetime-local]:not(.browser-default), input[type=tel]:not(.browser-default), input[type=number]:not(.browser-default), input[type=search]:not(.browser-default), textarea.materialize-textarea {
            margin: 0px !important;
        }

        .kanban-comment { 
            cursor: pointer;
            display: inline-block;
            float: right; 
        }
    </style>
    
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "FillUp Your Attendance";
            var Role = $(hdnRole).val();
            $('.timepicker').timepicker();
        });

        function openAttendance() {
            document.getElementById('spnModuleHeader').innerText = "Attendance Log Information";
            var m1, y1;
            var empid = "";
            empid = $('#drpEmployee').val();
            m1 = jQuery("#drpSummaryMonth").val();
            y1 = jQuery("#drpSummaryYear").val();
            var pageUrl = "AttendanceStatus.aspx?empid=" + empid + "&month=" + m1 + "&year=" + y1;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }

        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function showErrorMessage(strMess) {
            jQuery.confirm({ title: 'Data Validation', content: 'Are you sure, You want to delete record !', type: 'red', typeAnimated: true });
        }

        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        <%--Preview-Add EmployeeList on Employee Name Dropdown by Vikram Rajput 13-07-2020--%>
        function openEmployeeInfo(cat) {
            document.getElementById('spnModuleHeader').innerText = "Employee Infomration";
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpEmployee').val() : "0";
            var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

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
        function openUploadDocument() {
            document.getElementById('spnModuleHeader').innerText = "Upload Attendance";
            $('#ifrModule').attr('src', "UploadMaster.aspx?view=" + "Attendance");
            $find("mpe").show();
        }

        function clickMe(ctrl) {
            var i;
            for (i = 1; i <= 42; i++) {
                if ($('#Label' + i.toString()).hasClass("hide") == false)
                    $('#Label' + i.toString()).addClass("hide");
            }
            // -----------------------------------------------------------------
            var textBoxId = $(ctrl).next().next('input').toggleClass("hide");
        }

        function displayTime(MyContrl) {
            var Role = $(hdnRole).val();
            if (Role.toLowerCase() != 'admin' && Role.toLowerCase() != 'hradmin' && Role.toLowerCase() != 'hr')
            {
                var currDate = new Date();
                var hours = currDate.getHours();
                var minutes = currDate.getMinutes();
                var ampm = hours >= 12 ? 'pm' : 'am';
                hours = hours % 12;
                hours = hours ? hours : 12; // the hour '0' should be '12'
                minutes = minutes < 10 ? '0' + minutes : minutes;
                var strTime = hours + ':' + minutes + ' ' + ampm;
                $(MyContrl).val(strTime);
            }
                //var localTime = new Date();
                //var hours = localTime.getHours();
                //var minutes = localTime.getMinutes();
                //var timezoneoffset = localTime.getTimezoneOffset();
            //$(MyContrl).val(localTime.toLocaleTimeString(navigator.language, {
            //    hour: '2-digit',
            //    minute: '2-digit'
            //}));
        }

        //function displayTime() {
        //    var localTime = new Date();
        //    var year = localTime.getYear();
        //    var month = localTime.getMonth() + 1;
        //    var date = localTime.getDate();
        //    var hours = localTime.getHours();
        //    var minutes = localTime.getMinutes();
        //    var seconds = localTime.getSeconds();
        //    var div = document.getElementById("div1");
        //    div.innerText = year + "-" + month + "-" + date + " " + hours + ":" + minutes + ":" + seconds;
        //}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnBiometricAttendance" runat="server" ClientIDMode="Static" />
    
    <asp:HiddenField ID="hdnMenuID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAddFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEditFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDelFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnWeekOff" runat="server" ClientIDMode="Static" />
    <div id="contentwrapper" class="contentwrapper">
        <div id="myModal" style="display:block; width:100%;">
            <div class="row">
                <div class="col m3">
                    <div class="row">
                        <div class="input-field col m6">
                            <label class="active" for="drpSummaryMonth">Select Month<span class="materialize-green-text font-weight-800">*</span></label>
                            <asp:DropDownList ID="drpSummaryMonth" class="select2-theme browser-default mr-2 float-left" runat="server" ClientIDMode="Static" style="background-color:#a5fddf; padding:0px; width:150px; float:right; font-size:18px; margin-top:1px; height: 35px !important; border: 1px solid black; padding:1px;" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpSummaryMonthYear_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="input-field col m6">
                            <label class="active" for="drpSummaryYear">Select Year<span class="materialize-green-text font-weight-800">*</span></label>
                            <asp:DropDownList ID="drpSummaryYear" class="select2-theme browser-default mr-2 float-left" runat="server" ClientIDMode="Static" style="background-color:#a5fddf; padding:0px; width:150px; float:right; font-size:18px; margin-right:10px; margin-top:1px; height: 35px !important; border: 1px solid black; padding:1px;" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpSummaryMonthYear_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" id="divEmployee" runat="server" clientidmode="static">
                        <div class="input-field col m12">
                            <label class="active" for="drpEmployee">Select Employee<span class="materialize-green-text font-weight-800">*</span>
                                    <a href="javascript:openEmployeeInfo('view');">
                                        <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="3" />
                                    </a>
                                    <a href="javascript:openEmployeeInfo('add');">
                                        <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                    </a>
                            </label>
                            <asp:DropDownList ID="drpEmployee" class="select2-theme browser-default mr-2 float-left" runat="server" ClientIDMode="Static" style="background-color: #9bc2e4; width: 325px; float: right; font-size: 18px; margin-top: 1px; height: 35px !important; border: 1px solid black; padding: 1px;" AutoPostBack="true" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged" ></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" id="divUpload1" runat="server" clientidmode="static">
                        <div class="input-field col m6 float-left" style="background-color:aliceblue; text-align: center; padding: 5px; border-top:2px solid; border-bottom:2px solid;">
                            <a id="btnupload"  class="btn btn-success" data-position="center" data-tooltip="Upload Attendance Excel File" href="javascript:openUploadDocument();" style="border-radius: 5px; min-width: 200px; font-size:14px;">Upload Attendance</a>
                        </div>
                        <div class="input-field col m6" style="background-color:aliceblue; text-align: center; padding: 5px; border-top:2px solid; border-bottom:2px solid;">
                            <asp:Button CssClass="btn btn-success" ID="ImgbtnAttendence" runat="server" ClientIDMode="Static" Text="Upload BioMetric" OnClick="ImgbtnAttendence_Click" style="border-radius: 5px; min-width: 200px; font-size:14px;"></asp:Button>
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col m12">
                            <div class="card user-statistics-card cardRound mb-1">
                                <div class="card-content p-0">
                                    <h5 class="h5Round">
                                        <span id="spnPageHeader" runat="server" clientidmode="Static" class="titleHead text-top">Attendance Summary</span>
                                    </h5>
                                    <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                        <div class="row">
                                            <div class="col m12 pt-1 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                <div class="col m8">
                                                    <h5 class="description-header mb-0"><span class="description-text">Presence</span></h5>
                                                </div>
                                                <div class="col m4">
                                                    <h5 class="mb-0"><asp:Label CssClass="blue-text" ID="lblPres" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                                </div>
                                            </div>
                                            <div class="col m12 pt-1 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                <div class="col m8">
                                                    <h5 class="description-header mb-0"><span class="description-text">Full Day</span></h5>
                                                </div>
                                                <div class="col m4">
                                                    <h5 class="mb-0"><asp:Label CssClass="blue-text" ID="lblFull" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                                </div>
                                            </div>
                                            <div class="col m12 pt-1 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                <div class="col m8">
                                                    <h5 class="description-header mb-0"><span class="description-text">Half Day</span></h5>
                                                </div>
                                                <div class="col m4">
                                                    <h5 class="mb-0"><asp:Label CssClass="blue-text" ID="lblHalf" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row mt-3">
                                            <div class="col m12 pt-1 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                <div class="col m8">
                                                    <h5 class="description-header mb-0"><span class="description-text">Shift Total Hrs</span></h5>
                                                </div>
                                                <div class="col m4">
                                                    <h5 class="mb-0"><asp:Label CssClass="blue-text" ID="lblTotalShiftHrs" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                                </div>
                                            </div>
                                            <div class="col m12 pt-1 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                <div class="col m8">
                                                    <h5 class="description-header mb-0"><span class="description-text">Total Hrs (Worked)</span></h5>
                                                </div>
                                                <div class="col m4">
                                                    <h5 class="mb-0"><asp:Label CssClass="blue-text" ID="lblTotalWKHrs" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                                </div>
                                            </div>
                                            <div class="col m12 pt-1 card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                <div class="col m8">
                                                    <h5 class="description-header mb-0"><span class="description-text">Overtime Hrs</span></h5>
                                                </div>
                                                <div class="col m4">
                                                    <h5 class="mb-0"><asp:Label CssClass="blue-text" ID="lblTotalOTHrs" runat="server" ClientIDMode="Static" Text="0" /></h5>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col m9">
                    <table id="tblAttendance" runat="server" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="dayHeader" style="width:8%;"><span>Monday</span></td>
                            <td class="dayHeader" style="width:8%;"><span>Tuesday</span></td>
                            <td class="dayHeader" style="width:8%;"><span>Wednesday</span></td>
                            <td class="dayHeader" style="width:8%;"><span>Thursday</span></td>
                            <td class="dayHeader" style="width:8%;"><span>Friday</span></td>
                            <td class="dayHeader" style="width:8%;"><span>Saturday</span></td>
                            <td class="dayHeader" style="width:8%;"><span>Sunday</span></td>
                        </tr>
                        <tr>
                            <td class="dateHeader">
                                <div id="divFlag1" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label ID="lblDate1" runat="server" ClientIDMode="Static" />
                                <div id="kanban1" runat="server" ClientIDMode="Static" class="kanban-comment" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label1" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" />
                            </td>
                            <td class="dateHeader">
                                <div id="divFlag2" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label id="lblDate2" runat="server" ClientIDMode="Static" />
                                <div id="kanban2" runat="server" ClientIDMode="Static" class="kanban-comment" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label2" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" />
                            </td>
                            <td class="dateHeader">
                                <div id="divFlag3" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label ID="lblDate3" runat="server" ClientIDMode="Static" />
                                <div id="kanban3" runat="server" ClientIDMode="Static" class="kanban-comment" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label3" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" />
                            </td>
                            <td class="dateHeader">
                                <div id="divFlag4" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label ID="lblDate4" runat="server" ClientIDMode="Static" />
                                <div id="kanban4" runat="server" ClientIDMode="Static" class="kanban-comment" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label4" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" />
                            </td>
                            <td class="dateHeader">
                                <div id="divFlag5" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label ID="lblDate5" runat="server" ClientIDMode="Static" />
                                <div id="kanban5" runat="server" ClientIDMode="Static" class="kanban-comment" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label5" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" />
                            </td>
                            <td class="dateHeader">
                                <div id="divFlag6" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label ID="lblDate6" runat="server" ClientIDMode="Static" />
                                <div id="kanban6" runat="server" ClientIDMode="Static" class="kanban-comment" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label6" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" />
                            </td>                        
                            <td class="dateHeader">
                                <div id="divFlag7" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label ID="lblDate7" runat="server" ClientIDMode="Static" />
                                <div id="kanban7" runat="server" ClientIDMode="Static" class="kanban-comment" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label7" runat="server" ClientIDMode="Static" />
                            </td>
                        </tr>

                        <tr>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtIDate1" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate1')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" id="txtIDate2" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate2')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtIDate3" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate3')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtIDate4" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate4')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtIDate5" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate5')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtIDate6" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate6')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtIDate7" runat="server" ClientIDMode="Static"  ReadOnly="false" onFocus="javascript:displayTime('#txtIDate7')" onkeypress="return false" onpaste="return false" /></td>
                        </tr>
                        <tr style="border-bottom:2px solid navy !important;">

                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtODate1" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate1')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" id="txtODate2" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate2')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtODate3" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate3')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtODate4" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate4')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtODate5" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate5')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtODate6" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate6')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox class="timepicker" ID="txtODate7" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate7')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>

                        <tr>
                            <td class="dateHeader">
                                <div id="divFlag8" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label ID="lblDate8" runat="server" ClientIDMode="Static" />
                                <div id="kanban8" runat="server" ClientIDMode="Static" class="kanban-comment display-block" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label8" runat="server" ClientIDMode="Static" />
                            </td>
                            <td class="dateHeader">
                                <div id="divFlag9" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div>
                                <asp:Label ID="lblDate9" runat="server" ClientIDMode="Static" />
                                <div id="kanban9" runat="server" ClientIDMode="Static" class="kanban-comment display-block" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br />
                                <asp:TextBox CssClass="hide" ID="Label9" runat="server" ClientIDMode="Static" />
                            </td>
                            <td class="dateHeader"><div id="divFlag10" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate10" runat="server" ClientIDMode="Static" /><div id="kanban10" runat="server" ClientIDMode="Static" class="kanban-comment display-block" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label10" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag11" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate11" runat="server" ClientIDMode="Static" /><div id="kanban11" runat="server" ClientIDMode="Static" class="kanban-comment display-block" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label11" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag12" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate12" runat="server" ClientIDMode="Static" /><div id="kanban12" runat="server" ClientIDMode="Static" class="kanban-comment display-block" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label12" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag13" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate13" runat="server" ClientIDMode="Static" /><div id="kanban13" runat="server" ClientIDMode="Static" class="kanban-comment display-block" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label13" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>                        
                            <td class="dateHeader"><div id="divFlag14" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate14" runat="server" ClientIDMode="Static"  /><div id="kanban14" runat="server" ClientIDMode="Static" class="kanban-comment display-block" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label14" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtIDate8" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate8')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtIDate9" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate9')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtIDate10" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate10')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtIDate11" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate11')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtIDate12" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate12')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtIDate13" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate13')" onkeypress="return false" onpaste="return false" /></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtIDate14" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate14')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr style="border-bottom:2px solid navy !important;">
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtODate8" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate8')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" id="txtODate9" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate9')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtODate10" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate10')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtODate11" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate11')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtODate12" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate12')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtODate13" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate13')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timepicker" ID="txtODate14" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate14')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><div id="divFlag15" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate15" runat="server" ClientIDMode="Static" /><div id="kanban15" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label15" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag16" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate16" runat="server" ClientIDMode="Static" /><div id="kanban16" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label16" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag17" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate17" runat="server" ClientIDMode="Static" /><div id="kanban17" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label17" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag18" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate18" runat="server" ClientIDMode="Static" /><div id="kanban18" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label18" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag19" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate19" runat="server" ClientIDMode="Static" /><div id="kanban19" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label19" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag20" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate20" runat="server" ClientIDMode="Static" /><div id="kanban20" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label20" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>                        
                            <td class="dateHeader"><div id="divFlag21" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate21" runat="server" ClientIDMode="Static" /><div id="kanban21" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label21" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate15" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate15')" onkeypress="return false" onpaste="return false" /></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate16" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate16')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate17" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate17')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate18" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate18')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate19" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate19')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate20" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate20')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate21" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate21')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr style="border-bottom:2px solid navy !important;">
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate15" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate15')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" id="txtODate16" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate16')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate17" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate17')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate18" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate18')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate19" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate19')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate20" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate20')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate21" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate21')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><div id="divFlag22" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate22" runat="server" ClientIDMode="Static" /><div id="kanban22" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label22" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag23" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate23" runat="server" ClientIDMode="Static" /><div id="kanban23" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label23" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag24" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate24" runat="server" ClientIDMode="Static" /><div id="kanban24" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label24" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag25" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate25" runat="server" ClientIDMode="Static" /><div id="kanban25" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label25" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag26" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate26" runat="server" ClientIDMode="Static" /><div id="kanban26" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label26" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag27" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate27" runat="server" ClientIDMode="Static" /><div id="kanban27" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label27" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>                        
                            <td class="dateHeader"><div id="divFlag28" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate28" runat="server" ClientIDMode="Static" /><div id="kanban28" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label28" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate22" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate22')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate23" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate23')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate24" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate24')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate25" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate25')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate26" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate26')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate27" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate27')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate28" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate28')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr style="border-bottom:2px solid navy !important;">
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate22" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate22')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" id="txtODate23" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate23')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate24" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate24')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate25" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate25')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate26" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate26')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate27" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate27')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate28" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate28')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><div id="divFlag29" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate29" runat="server" ClientIDMode="Static" /><div id="kanban29" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label29" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag30" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate30" runat="server" ClientIDMode="Static" /><div id="kanban30" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label30" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag31" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate31" runat="server" ClientIDMode="Static" /><div id="kanban31" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label31" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag32" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate32" runat="server" ClientIDMode="Static" /><div id="kanban32" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label32" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag33" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate33" runat="server" ClientIDMode="Static" /><div id="kanban33" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label33" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag34" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate34" runat="server" ClientIDMode="Static" /><div id="kanban34" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label34" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>                        
                            <td class="dateHeader"><div id="divFlag35" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate35" runat="server" ClientIDMode="Static" /><div id="kanban35" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label35" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate29" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate29')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" id="txtIDate30" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate30')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate31" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate31')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate32" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate32')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate33" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate33')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate34" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate34')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate35" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate35')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr style="border-bottom:2px solid navy !important;">
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate29" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate29')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" id="txtODate30" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate30')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate31" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate31')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate32" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate32')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate33" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate33')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate34" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate34')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate35" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate35')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><div id="divFlag36" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate36" runat="server" ClientIDMode="Static" /><div id="kanban36" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label36" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag37" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate37" runat="server" ClientIDMode="Static" /><div id="kanban37" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label37" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag38" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate38" runat="server" ClientIDMode="Static" /><div id="kanban38" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label38" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag39" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate39" runat="server" ClientIDMode="Static" /><div id="kanban39" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label39" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag40" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate40" runat="server" ClientIDMode="Static" /><div id="kanban40" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label40" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                            <td class="dateHeader"><div id="divFlag41" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate41" runat="server" ClientIDMode="Static" /><div id="kanban41" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label41" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>                        
                            <td class="dateHeader"><div id="divFlag42" runat="server" ClientIDMode="Static" class="float-left" onclick="javascript:openAttendance();"><i class="material-icons font-size-small green-text">brightness_5</i></div><asp:Label ID="lblDate42" runat="server" ClientIDMode="Static" /><div id="kanban42" runat="server" ClientIDMode="Static" class="kanban-comment display-flex" onclick="javascript:clickMe(this);"><i class="material-icons font-size-small green-text">chat_bubble_outline</i></div><br /><asp:TextBox CssClass="hide" ID="Label42" runat="server" ClientIDMode="Static" style="background-color:#ffffba;" /></td>
                        </tr>
                        <tr>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate36" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate36')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" id="txtIDate37" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate37')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate38" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate38')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate39" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate39')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate40" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate40')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate41" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate41')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtIDate42" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtIDate42')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                        <tr style="border-bottom:2px solid navy !important;">
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate36" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate36')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" id="txtODate37" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate37')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate38" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate38')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate39" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate39')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate40" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate40')" onkeypress="return false" onpaste="return false"/></td>
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate41" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate41')" onkeypress="return false" onpaste="return false"/></td>                        
                            <td class="dateHeader"><asp:TextBox CssClass="timeInput" ID="txtODate42" runat="server" ClientIDMode="Static" ReadOnly="false" onFocus="javascript:displayTime('#txtODate42')" onkeypress="return false" onpaste="return false"/></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div style="background-color:aliceblue; text-align: center; padding: 10px; border-top:2px solid; border-bottom:2px solid;">
        <asp:Button CssClass="btn btn-success" ID="btnSubmit" runat="server" ClientIDMode="Static" Text=" Submit Attendace " OnClick="btnSubmit_Click"></asp:Button>
    </div>
    <br /><br /><br />
    <%-- *=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=* --%>

    <div id="divSetTime" class="modal divSetTime" style="display:none; min-height:500px !important;">
      <div class="modal-dialog" role="document" style="width:250px;">
        <div class="modal-content" style="border:1px groove black; box-shadow: black -6px 8px 6px -6px;">
          <div class="modal-header" style="padding:5px 10px;">
            <h5 class="modal-title">Set Time</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body" style="padding:8px">
                <div style="border-radius:5px; width:100%;">
                    <fieldset style="text-align:left;">
                        <table cellpadding="1" cellspacing="0">
                            <tr style="padding:8px 15px;">
                                <td>
                                    <asp:DropDownList ID="ddlHrs" runat="server" ClientIDMode="Static">
                                        <asp:ListItem Value="01" Text="01" />
                                        <asp:ListItem Value="02" Text="02" />
                                        <asp:ListItem Value="03" Text="03" />
                                        <asp:ListItem Value="04" Text="04" />
                                        <asp:ListItem Value="05" Text="05" />
                                        <asp:ListItem Value="06" Text="06" />
                                        <asp:ListItem Value="07" Text="07" />
                                        <asp:ListItem Value="08" Text="08" />
                                        <asp:ListItem Value="09" Text="09" />
                                        <asp:ListItem Value="10" Text="10" />
                                        <asp:ListItem Value="11" Text="11" />
                                        <asp:ListItem Value="12" Text="12" />
                                    </asp:DropDownList>
                                    &nbsp;:
                                    <asp:DropDownList ID="ddlMins" runat="server" ClientIDMode="Static">
                                        <asp:ListItem Value="00" Text="00" />
                                        <asp:ListItem Value="15" Text="15" />
                                        <asp:ListItem Value="30" Text="30" />
                                        <asp:ListItem Value="45" Text="45" />
                                        <asp:ListItem Value="--" Text="--" />
                                        <asp:ListItem Value="00" Text="00" />
                                        <asp:ListItem Value="00" Text="00" />
                                        <asp:ListItem Value="01" Text="01" />
                                        <asp:ListItem Value="02" Text="02" />
                                        <asp:ListItem Value="03" Text="03" />
                                        <asp:ListItem Value="04" Text="04" />
                                        <asp:ListItem Value="05" Text="05" />
                                        <asp:ListItem Value="06" Text="06" />
                                        <asp:ListItem Value="07" Text="07" />
                                        <asp:ListItem Value="08" Text="08" />
                                        <asp:ListItem Value="09" Text="09" />
                                        <asp:ListItem Value="10" Text="10" />
                                        <asp:ListItem Value="11" Text="11" />
                                        <asp:ListItem Value="12" Text="12" />
                                        <asp:ListItem Value="13" Text="13" />
                                        <asp:ListItem Value="14" Text="14" />
                                        <asp:ListItem Value="15" Text="15" />
                                        <asp:ListItem Value="16" Text="16" />
                                        <asp:ListItem Value="17" Text="17" />
                                        <asp:ListItem Value="18" Text="18" />
                                        <asp:ListItem Value="19" Text="19" />
                                        <asp:ListItem Value="20" Text="20" />
                                        <asp:ListItem Value="21" Text="21" />
                                        <asp:ListItem Value="22" Text="22" />
                                        <asp:ListItem Value="23" Text="23" />
                                        <asp:ListItem Value="24" Text="24" />
                                        <asp:ListItem Value="25" Text="25" />
                                        <asp:ListItem Value="26" Text="26" />
                                        <asp:ListItem Value="27" Text="27" />
                                        <asp:ListItem Value="28" Text="28" />
                                        <asp:ListItem Value="29" Text="29" />
                                        <asp:ListItem Value="30" Text="30" />
                                        <asp:ListItem Value="31" Text="31" />
                                        <asp:ListItem Value="32" Text="32" />
                                        <asp:ListItem Value="33" Text="33" />
                                        <asp:ListItem Value="34" Text="34" />
                                        <asp:ListItem Value="35" Text="35" />
                                        <asp:ListItem Value="36" Text="36" />
                                        <asp:ListItem Value="37" Text="37" />
                                        <asp:ListItem Value="38" Text="38" />
                                        <asp:ListItem Value="39" Text="39" />
                                        <asp:ListItem Value="40" Text="40" />
                                        <asp:ListItem Value="41" Text="41" />
                                        <asp:ListItem Value="42" Text="42" />
                                        <asp:ListItem Value="43" Text="43" />
                                        <asp:ListItem Value="44" Text="44" />
                                        <asp:ListItem Value="45" Text="45" />
                                        <asp:ListItem Value="46" Text="46" />
                                        <asp:ListItem Value="47" Text="47" />
                                        <asp:ListItem Value="48" Text="48" />
                                        <asp:ListItem Value="49" Text="49" />
                                        <asp:ListItem Value="50" Text="50" />
                                        <asp:ListItem Value="51" Text="51" />
                                        <asp:ListItem Value="52" Text="52" />
                                        <asp:ListItem Value="53" Text="53" />
                                        <asp:ListItem Value="54" Text="54" />
                                        <asp:ListItem Value="55" Text="55" />
                                        <asp:ListItem Value="56" Text="56" />
                                        <asp:ListItem Value="57" Text="57" />
                                        <asp:ListItem Value="58" Text="58" />
                                        <asp:ListItem Value="59" Text="59" />
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <input type="radio" name="radioGrp" value="AM" checked="checked"/>AM&nbsp;&nbsp;
                                    <input type="radio" name="radioGrp" value="PM"/>PM&nbsp;&nbsp;&nbsp;&nbsp;
                                    
                                </td>
                            </tr>
                            <tr style="text-align:center;">
                                <td style="text-align: center; padding:8px;">
                                    <input class="center btn-primary" type="button" id="btnSetTime" name="btnSetTime" value="Set Time" style="padding: 2px 8px;border-radius: 5px;" />&nbsp;&nbsp;&nbsp;
                                    <input class="center btn-dark" type="button" id="btnClearTime" name="btnClearTime" value="Clear Time" style="padding: 2px 8px;border-radius: 5px;" />&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
          </div>
        </div>
      </div>
    </div>
    <%--Preview-Add EmployeeList on Employee Name Dropdown by Vikram Rajput 13-07-2020--%>

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

</asp:Content>

