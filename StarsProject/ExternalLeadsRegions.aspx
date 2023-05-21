<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="ExternalLeadsRegions.aspx.cs" Inherits="StarsProject.ExternalLeadsRegions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
        /* Container holding the image and the text */
        .container {
            position: relative;
            text-align: center;
            color: white;
        }

        /* Bottom left text */
        .bottom-left {
            position: absolute;
            bottom: 5px;
            left: 10px;
        }

        /* Top left text */
        .top-left {
            position: absolute;
            top: 0px;
            left: 10px;
        }

        /* Top right text */
        .top-right {
            position: absolute;
            top: 8px;
            right: 16px;
        }

        /* Bottom right text */
        .bottom-right {
            position: absolute;
            bottom: 8px;
            right: 16px;
        }

        /* Centered text */
        .centered {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
        }

        /*Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020*/

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

        [type='checkbox']:not(:checked), [type='checkbox']:checked { position: none; opacity: 1; margin-right: 10px; margin-top: 5px; pointer-events:all;}

        .mychklist label { margin-left: 15px;}

    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#imgMapIcon').click(function (event) { event.preventDefault(); return false; });
            //$('.container').click(function (event) { event.preventDefault(); return false; });
            $('.accent-1').click(function (event) { event.preventDefault(); return false; });
        });
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
        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
        /*Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020*/

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <div class="pageheader" style="padding-bottom: 0px !important;">
        <div id="contentwrapper" class="contentwrapper">
            <div class="masonry-gallery-wrapper">
                <div class="popup-gallery">
                    <div class="row">
                        <div class="col m8">
                            <h5 class="card-title mb-3 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Allocated State<i class="material-icons float-left">ac_unit</i></h5>
                            <table id="tblCard" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                <asp:Repeater ID="rptExternalLeadRegion" runat="server" ClientIDMode="Static" OnItemCommand="rptExternalLeadRegion_ItemCommand">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                        <asp:HiddenField ID="hdnEmpID" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeID") %>' />
                                        <asp:HiddenField ID="hdnCountryID" runat="server" ClientIDMode="Static" Value='<%# Eval("CountryCode") %>' />
                                        <asp:HiddenField ID="hdnStateID" runat="server" ClientIDMode="Static" Value='<%# Eval("StateCode") %>' />
                                        <asp:HiddenField ID="hdnCityList" runat="server" ClientIDMode="Static" Value='<%# Eval("CityList") %>' />
                                        <div class="col m4 padding-1">
                                            <div id="cont1" class="container" style="width:100%;">
                                                <a class="accent-1" href="images/GoogleMapBackground.png" title="The Cleaner">
                                                    <img id="imgMapIcon" src="images/GoogleMapBackground.png" class="responsive-img" alt="" />
                                                    <div class="top-left" style="font-size:18px; font-weight:bold; color:maroon;"><%# Eval("EmployeeName") %></div>
                                                    <div class="bottom-left" style="font-size:22px; font-weight:bold;"><%# Eval("State") %></div>
                                                </a>
                                            </div>
                                            <div id="cont2" class="container" style="width:100%; padding:5px; background-color: lightsteelblue; height:30px;" onclick="">
                                                <asp:ImageButton CssClass="float-left hide" ID="ImgbtnSave" runat="server" ImageUrl="~/images/expand.png" ToolTip="Save" Width="20" Height="20" TabIndex="5" CommandName="Save" CommandArgument='<%# Eval("EmployeeID") %>' />
                                                <asp:ImageButton CssClass="float-right" ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" Width="20" Height="20" TabIndex="6" CommandName="Delete" CommandArgument='<%# Eval("EmployeeID") %>' />
                                            </div>
                                        </div>

                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                        <div class="col m4">
                            <h5 class="card-title mb-4 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Assign New State<i class="material-icons float-left">ac_unit</i></h5>
                            <div class="row">
                                <div class="input-field col m12">
                                    <label class="active required" for="drpEmployee">Select Employee <span class="materialize-red-text font-weight-800">*</span>
                                        <%--Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020--%>
                                        <a href="javascript:openEmployeeInfo('view');">
                                            <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="1" />
                                         </a>
                                         <a href="javascript:openEmployeeInfo('add');">
                                            <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                         </a>
                                    </label>
                                    <asp:DropDownList ID="drpEmployee" runat="server" CssClass="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="1"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m6">
                                    <label class="active required" for="drpCountryCode">Assign Country <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:DropDownList ID="drpCountryCode" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="drpCountryCode_SelectedIndexChanged" />
                                </div>
                                <div class="input-field col m6">
                                    <label class="active required" for="drpStateCode">Assign State <span class="materialize-red-text font-weight-800">*</span></label>
                                    <asp:DropDownList ID="drpStateCode" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="3" AutoPostBack ="true" OnSelectedIndexChanged ="drpStateCode_SelectedIndexChanged" />
                                </div>

                            </div>
                            <div class="row">
                                <h6 class="card-title padding-1 mb-4 center-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Select & Assign City</h6>
                                <div class="input-field mychklist col m12" style="height:150px; overflow-y:auto;">
                                    <asp:CheckBoxList ID="chkCity" runat="server" TabIndex="4">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                <button id="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_ServerClick" TabIndex="5"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="6"><i class="material-icons left">save</i>Save</button>
                            </div>
                       </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--/*Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020*/--%>
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
