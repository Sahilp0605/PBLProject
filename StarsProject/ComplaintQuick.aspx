<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComplaintQuick.aspx.cs" Inherits="StarsProject.ComplaintQuick" %>
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
<%--    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>--%>

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
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('.timepicker').timepicker();
            $('#spnModuleHeader').val("Complaint List");
        });

        //$(document).ready(function ShowHideClosingRemarks() {
        //    $('#drpComplaintType').on('change', function ShowHideClosingRemarks() {
        //        if (this.value == 'Online')
        //            //.....................^.......
        //        {
        //            $("#divClosingRemarks").show();
        //        }
        //        else {
        //            $("#divClosingRemarks").hide();
        //        }
        //    });
        //});

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

        function openCustomerInfo(cat) {
            $("#pnlComplaint").width("60%");
            spnComplaintModuleHeader.innerText = "Customer Information";
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customerquick.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        <%--Preview-Add EmployeeList on Employee Name Dropdown by Vikram Rajput 13-07-2020--%>
        function openEmployeeInfo(cat) {
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
                    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />

                    <div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col m6">
                                        <div class="row">
                                            <div class="input-field col m4">
                                                <label class="active" for="lblComplaintNo">Complaint #</label>
                                                <asp:TextBox ID="lblComplaintNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder="" />
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="txtComplaintDate">Complaint Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtComplaintDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" onpaste="return false" placeholder=""   TextMode="Date"/>
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="drpStatus">Status</label>
                                                <asp:DropDownList ID="drpStatus" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="10">
                                                    <asp:ListItem Text="Open" Value="Open" Selected="True" />
                                                    <asp:ListItem Text="Close" Value="Close" />
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="input-field col m12">
                                                <label class="active" for="txtCustomerName">Customer Name <span class="materialize-red-text font-weight-800">*</span>
                                                    <a href="javascript:openCustomerInfo('view');">
                                                                    <img src="images/registration.png" style="padding: 6px 0px 0px 0px;" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" Tabindex="3" />
                                                                </a>
                                                            <a href="javascript:openCustomerInfo('add');">
                                                                <img src="images/addCustomer.png" style="padding: 6px 0px 0px 0px;" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                            </a>
                                                </label>
                                                <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="3" placeholder="Min. 3 Chars To Search" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m4">
                                                <asp:TextBox ID="txtContactNo1" class="form-control" runat="server" ClientIDMode="Static" Placeholder="" MaxLength="15" TabIndex="6" />
                                                <label class="active" for="txtContactNo1">Contact #1 <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="txtScheduleDate">Schedule Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtScheduleDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" onpaste="return false" placeholder=""   TextMode="Date"/>
                                            </div>
                                            <div class="input-field col m4">
                                                <asp:DropDownList ID="drpComplaintType" runat="server" class="select2-theme browser-default" AutoPostBack="true" ClientIDMode="Static" TabIndex="10" OnSelectedIndexChanged="drpComplaintType_SelectedIndexChanged">
                                                    <asp:ListItem Text="Online" Value="Online" />
                                                    <asp:ListItem Text="Offline" Value="Offline" Selected="True"  />
                                                </asp:DropDownList>
                                                <label class="active" for="drpComplaintType">Complaint Type <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                        </div>
                                        <div id="divClosingRemarks" runat="server" class="row" >
                                            <div class="input-field col m12">
                                                <label class="active" for="txtClosingRemarks">Closing Remarks</label>
                                                <asp:TextBox ID="txtClosingRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="5" MaxLength="500" TextMode="MultiLine" Rows="4" style="min-height:120px;" placeholder=""/>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col m12 pr-0">
                                                <div class="card small" style="height:350px;">
                                                    <div class="card-title blue white-text" style="padding:10px;">
                                                        <p class="no-margin">Registered Office Address</p>
                                                    </div>
                                                    <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                                        <div class="row" style="width:100%">
                                                            <div class="input-field col m12">
                                                                <asp:TextBox ID="txtAddress" runat="server" class="form-control" Placeholder="" TabIndex="15" MaxLength="100"  ClientIDMode="Static" />
                                                                <label class="active" for="txtAddress">Address</label>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="width:100%">
                                                            <div class="input-field col m4">
                                                                <asp:TextBox ID="txtArea" runat="server" TabIndex="16" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                                <label class="active" for="txtArea">Area</label>
                                                            </div>
                                                            <div class="input-field col m4" id="DivCountry" runat="server" clientidmode="Static">
                                                                <asp:DropDownList ID="drpCountry" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="17" AutoPostBack="true"  OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"/>
                                                                <label class="active" for="drpCountry">Country <span class="materialize-red-text font-weight-800">*</span></label>
                                                            </div>
                                                            <div class="input-field col m4">
                                                                <asp:DropDownList ID="drpState" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="17" AutoPostBack="true"  OnSelectedIndexChanged="drpState_SelectedIndexChanged"/>
                                                                <label class="active" for="drpState">State <span class="materialize-red-text font-weight-800">*</span></label>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="width:100%">
                                                            <div class="input-field col m6">
                                                                <asp:DropDownList ID="drpCity" class="select2-theme browser-default" runat="server"  ClientIDMode="Static" TabIndex="18" />
                                                                <label class="active" for="drpCity">City </label>
                                                            </div>
                                                            <div class="input-field col m6">
                                                                <asp:TextBox ID="txtPincode" runat="server" TabIndex="19" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="18"/>
                                                                <label class="active" for="txtPincode">Pincode</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m12">
                                                <label class="active" for="txtComplaintNotes">Complaint Description </label>
                                                <asp:TextBox ID="txtComplaintNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="5" MaxLength="500" TextMode="MultiLine" Rows="4" style="min-height:120px;" placeholder=""/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m12">
                                                <label class="active" for="drpEmployee">Assigned To <span class="materialize-red-text font-weight-800">*</span>
                                                    <a href="javascript:openEmployeeInfo('view');">
                                                        <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="3" />
                                                    </a>
                                                    <a href="javascript:openEmployeeInfo('add');">
                                                        <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                    </a>
                                                </label>
                                                <asp:DropDownList ID="drpEmployee" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="9"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m6">
                                            <div class="flex-lg-wrap">
                                                <a id="lnkPriorFollowup" runat="server" clientidmode="Static" class="hide accordian btn btn-primary w-100 text-left" data-toggle="collapse" href="#multiCollapseExample1" role="button" aria-expanded="false" aria-controls="multiCollapseExample1" visible="false">Click To Show Prior Followups</a>
                                                <div class="row">
                                                    <div class="col m12 p-0">
                                                        <h4 class="card-title padding-2 m-0 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Customer History</h4>
                                                        <div class="collapse multi-collapse padding-1" id="multiCollapseExample1" style="height:300px; overflow-x:hidden; overflow-y:auto;">
                                                            <div class="card card-body">
                                                                <ul class="todo-list" style="list-style:none;">
                                                                <asp:Repeater ID="rptFollowupTrail" runat="server" ClientIDMode="Static">
                                                                    <ItemTemplate>
                                                                        <li id='liitem<%# Eval("pkID") %>' class="gradient-45deg-yellow-green mb-1" style="border-left: 2px solid #d6c7c7; padding:5px;">
                                                                            <span class="handle">
                                                                                <span class="text" style="color:black; font-weight:600; pointer-events: none;cursor: default !important;  text-transform:capitalize;">
                                                                                    <a title="Show Followup Info." href="javascript:void(0)" onclick="return false;" style="font-size:14px;"><%# Eval("CustomerName") %></a>
                                                                                </span>
                                                                                <small class="float-right badge badge-danger mr-1 blue-text" style="font-size:16px;"><i class="fa fa-clock-o"></i>  <%# Eval("ComplaintDate") %> </small>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                <small class="float-right badge badge-danger mr-1 brown-text" style="font-size:16px;"><i class="fa fa-clock-o"></i>  <%# Eval("ComplaintNo") %> </small>
                                                                            </span><br />
                                                                            <span class="text" style="color:navy; font-size:12px; font-weight:400;"><%# Eval("ComplaintNotes") %></span>
                                                                            <div class="row mt-1 ml-0 mr-0" style="background-color:navy;">
                                                                                <div class="col m12">
                                                                                    <small>
                                                                                        <span style="color:white; font-size:16px; font-weight:800;">Initiated By :</span>
                                                                                        <span style="color:white; font-size:16px;"><b>&nbsp;<%# Eval("EmployeeName") %></b></span>
                                                                                    </small>
                                                                                </div>
                                                                            </div>
                                                                        </li>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                                </div>
                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                    <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="11"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                    <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="12"><i class="material-icons left">save</i>Save</button>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <%--**************************** Customer Popup ***********************************--%>
                <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
                <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="pnlComplaint" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
                </cc1:ModalPopupExtender>  
                <asp:Panel ID="pnlComplaint" runat="server" clientidmode="Static" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
                    <div id="myModal111">
                        <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom:10px !important;">
                            <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                                <i class="material-icons prefix">ac_unit</i>
                                <span id="spnComplaintModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                                <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text=""/>
                            </h5>
                        </div>
                    </div>
                    <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border:1px solid silver; background-color:transparent; width:100%; height:90%; padding:5px;"></iframe>  
                   <br/>  
                </asp:Panel> 

                <script type="text/javascript">
                    function bindDDLTo(selector) {
                        if ($(selector).val().length >= 3) {
                            jQuery.ajax({
                                type: "POST",
                                url: "InquiryInfo.aspx/FilterCustomer",
                                data: '{pCustName:\'' + $(selector).val() + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    var sample = JSON.parse(data.d);
                                    $(selector).autoComplete({
                                        minLength: 1,
                                        source: function (term, suggest) {
                                            term = term.toLowerCase();
                                            var choices = sample;
                                            suggest(choices);
                                        },
                                        renderItem: function (item, search) {
                                            $(".autocomplete-suggestion").remove();
                                            return '<div class="autocomplete-suggestion" style="position:relative !important;" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                        },
                                        onSelect: function (e, term, item) {
                                            console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                            $(selector).val(item.html());
                                            $("#hdnCustomerID").val(item.data('langname'));
                                            $("#drpInquiry").focus();
                                        }
                                    });

                                },
                                error: function (r) { alert('Error : ' + r.responseText); },
                                failure: function (r) { alert('failure'); }
                            });
                            return false;
                        }
                    }
                </script>
   <%--     </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    </form>
</body>
</html>