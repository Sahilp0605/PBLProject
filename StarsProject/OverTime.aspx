<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OverTime.aspx.cs" Inherits="StarsProject.OverTime" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
<%--    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />--%>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
<%--    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />--%>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
<%--    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>--%>

    <style type="text/css">
        /* ==================================== */
        /*****         Modal Popup        *****/
        /* ==================================== */
        .Popup  
        {  
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
            display:block;
            box-sizing:border-box;
            width:30px;
            height:30px;
            border-width:3px;
            border-style: solid;
            border-color:red;
            border-radius:100%;
            background: -webkit-linear-gradient(-45deg, transparent 0%, transparent 46%, white 46%,  white 56%,transparent 56%, transparent 100%), -webkit-linear-gradient(45deg, transparent 0%, transparent 46%, white 46%,  white 56%,transparent 56%, transparent 100%);
            background-color:red;
            box-shadow:0px 0px 5px 2px rgba(0,0,0,0.5);
            transition: all 0.3s ease;
            position: relative;
            top: 12px;
            right: 15px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.timepicker').timepicker();
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
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Employees";
            var keyid = "0";
            keyid = $("#drpEmployee").val();
            var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">

    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">

        <%--<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <div class="widgetbox">
                    <div class="clearall"></div>
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnEmpEmailAddress" runat="server" ClientIDMode="Static" />

                    <div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col m5">
                                        <div class="row">
                                            <div class="input-field col m12">
                                                <asp:TextBox ID="txtReasonForOT" runat="server" class="textarea" ClientIDMode="Static" PlaceHolder=" " TabIndex="1" MaxLength="1000" TextMode="MultiLine" Height="100px" />
                                                <label class="active" for="txtReasonForOT">Over Time Reason<span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m7">
                                         <div class="row">
                                            <div class="input-field col m6">
                                                <label class="active" for="drpEmployee">
                                                    Employee Name<small class="text-muted font-italic">&nbsp;(Min.3 Chars To Search)</small>  <span class="materialize-red-text font-weight-800">*</span>
                                                    <a href="javascript:openEmployeeInfo('view');">
                                                        <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" Tabindex="3" />
                                                    </a>
                                                    <a href="javascript:openEmployeeInfo('add');">
                                                        <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                    </a>
                                                </label>
                                                <asp:DropDownList ID="drpEmployee" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="7"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m6">
                                                <asp:TextBox ID="txtFromDate" runat="server" class="form-control" TextMode="Date" PlaceHolder="From Date" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" Width="150px" TabIndex="2" />
                                                <asp:TextBox ID="txtFromTime" runat="server" class="timepicker" PlaceHolder="From Time" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" Width="100px" TabIndex="3" />
                                                <label class="active" for="txtFromDate">From Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                            <div class="input-field col m6">
                                                <asp:TextBox ID="txtToDate" runat="server" class="form-control" TextMode="Date" PlaceHolder="To Date" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" Width="150px" TabIndex="4" />
                                                <asp:TextBox ID="txtToTime" runat="server" class="timepicker" PlaceHolder="To Time" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" Width="100px" TabIndex="5" />
                                                <label class="active" for="txtToDate">To Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                        </div>
                                       
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                    <%--**************************** Customer Popup ***********************************--%>
                    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
                    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
                    </cc1:ModalPopupExtender>  
                    <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
                        <div id="myModal1211">
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
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                        <button id="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="8"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button id="btnSaveEmail" type="button" runat="server" clientidmode="Static" Text="Save & Send Email" class="btn cyan right mr-1" onserverclick="btnSaveEmail_Click" TabIndex="9"><i class="material-icons left">email</i>Save - Send Email</button>
                        <button id="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="10"><i class="material-icons left">save</i>Save</button>
                    </div>
                </div>
       <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    </form>
</body>
</html>
