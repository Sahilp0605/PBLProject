<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Followup.aspx.cs" Inherits="StarsProject.Followup" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myAudioControl.ascx" TagPrefix="uc1" TagName="myAudioControl" %>
<%@ Register Src="~/myNotificationPanel.ascx" TagPrefix="uc1" TagName="myNotificationPanel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- Section Start : Below Section need to copy in all page--%>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="app-assets/js/plugins.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="app-assets/js/scripts/advance-ui-modals.js"></script>

    <%--    <script type="text/javascript">
            alert(1);
            console.log(Notification.permission);
            if (Notification.permission === "granted") {
                alert("we have permission");
                showNotification();
            } else if (Notification.permission !== "denied") {
                Notification.requestPermission().then(permission => {
                    console.log(permission);
                });
            }

            function showNotification() {
                const notification = new Notification("New message incoming", {
                    body: "Hi there. How are you doing?",
                    icon: "yourimageurl.png"
                })
            }

        </script>--%>

    <%-- Section End --%>

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
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.timepicker').timepicker();

            $('#drpInquiryStatus').on('change', function () {
                //resetClosure();
            });
        });

        function resetClosure() {
            //    var drp = document.getElementById("drpClosureReason");
            //    if (drp != null) {
            //        var optionsText = drp.options[drp.selectedIndex].text;
            //        alert(optionsText)
            //        if (optionsText == 'Close - Lost') {
            //            $("#drpClosureReason").css('display', 'block');
            //            $("#lblReason").css('display', 'block');
            //        }
            //        else {
            //            $("#drpClosureReason").css('display', 'none');
            //            $("#lblReason").css('display', 'none');
            //        }
            //    }
        }

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
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Customer";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
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

        function showInquiryInfo(inqno) {
            jQuery.ajax({
                type: "POST",
                url: 'InquiryInfo.aspx/GetInquiryNoPrimaryID',
                data: '{pInqNo:\'' + inqno + '\'}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var resultdata = JSON.parse(data.d);
                    var returnID = parseInt(data.d);
                    if (returnID > 0) {
                        spnModuleHeader.innerText = "Manage Lead";
                        $('#ifrModule').attr('src', "InquiryInfo.aspx?mode=view&id=" + returnID);
                        $find("mpe").show();
                    }
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });
        }
        function showQuotationInfo(quono) {
            jQuery.ajax({
                type: "POST",
                url: 'Quotation.aspx/GetQuotationNoPrimaryID',
                data: '{pQuotationNo:\'' + quono + '\'}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var resultdata = JSON.parse(data.d);
                    var returnID = parseInt(data.d);
                    if (returnID > 0) {
                        spnModuleHeader.innerText = "Manage Quotation";
                        $('#ifrModule').attr('src', "Quotation.aspx?mode=view&id=" + returnID);
                        $find("mpe").show();
                    }
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });
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
                        <%-- Bootstrap INQUIRY INFO Modal Popup --%>
                        <div class="clearall"></div>
                        <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnOrgCodeEmp" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnRating" runat="server" ClientIDMode="Static" Value="1" />
                        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnInquiryNo" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnAllowBackDatedFollowup" runat="server" ClientIDMode="Static" Value="Yes" />
                        <asp:HiddenField ID="hdnEntryMode" runat="server" ClientIDMode="Static" />
                        
                        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col m7 padding-1" style="min-height:450px; border-right:3px ridge silver;">
                                            <div class="row">
                                                <div class="input-field col m4">
                                                    <label class="active" for="drpFollowupType">Followup Type <span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:DropDownList ID="drpFollowupType" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="1"></asp:DropDownList>
                                                </div>
                                                <div class="input-field col m4">
                                                    <label class="active" for="txtFollowupDate">FollowUp Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:TextBox ID="txtFollowupDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" TextMode="Date" />
                                                </div>
                                                <div class="input-field col m4">
                                                    <label class="active" for="drpPriority">Priority <span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:DropDownList ID="drpPriority" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="3">
                                                        <asp:ListItem Text="High" Value="1" Selected="True" />
                                                        <asp:ListItem Text="Medium" Value="2" />
                                                        <asp:ListItem Text="Low" Value="3" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtCustomerName">
                                                        Customer Name <span class="materialize-red-text font-weight-800">*</span>
                                                        <a href="javascript:openCustomerInfo('view');">
                                                            <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" Tabindex="3" />
                                                        </a>
                                                        <a href="javascript:openCustomerInfo('add');">
                                                            <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                        </a>
                                                    </label>
                                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="3" placeholder="Min.3 Chars To Search"/>
                                                </div>
                                                <div class="input-field col m4">
                                                    <label class="active" for="drpInquiry">Inquiry #</label>
                                                    <asp:DropDownList ID="drpInquiry" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="drpInquiry_SelectedIndexChanged" />
                                                </div>
                                            </div>
                                            <div class="row mt-2 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                                <div class="input-field col m12">
                                                    <label class="active" for="txtMeetingNotes">Meeting Notes <span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:TextBox ID="txtMeetingNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="5" MaxLength="500" TextMode="MultiLine" Rows="3" placeholder="" />
                                                </div>
                                            </div>
                                            <div class="row mt-2 badge-secondary" style="border-radius: 5px; padding: 5px;">

                                                <div class="input-field col m3">
                                                    <label class="active" for="txtNextFollowupDate">Next Followup <span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:TextBox ID="txtNextFollowupDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" placeholder="" TextMode="Date" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtNextFollowupDate">Preferred Time</label>
                                                    <asp:TextBox ID="txtPreferredTime" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="7" onkeypress="return false;" onpaste="return false" Style="width: 100px; display: inline-block;" ReadOnly="false" placeholder="" />
                                                </div>
                                                <div class="input-field col m2">
                                                    <label class="active" for="chkNoFollowup">No FollowUp</label>
                                                    <div class="switch">
                                                        <label>
                                                            <asp:CheckBox ID="chkNoFollowup" runat="server" ClientIDMode="Static" TabIndex="8" OnCheckedChanged="chkNoFollowup_CheckedChanged"/>
                                                            <span class="lever" style="margin: 10px 20px;"></span>
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row mt-2" id="divStatus" runat="server" clientidmode="Static" style="border-radius: 5px;">
                                                <div class="input-field col m6">
                                                    <label class="active" for="drpInquiryStatus">Inquiry Status</label>
                                                    <asp:DropDownList ID="drpInquiryStatus" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="drpInquiryStatus_SelectedIndexChanged" />
                                                </div>
                                                <div class="input-field col m6">
                                                    <label runat="server" clientidmode="Static" class="active" for="drpClosureReason">Closure Reason</label>
                                                    <asp:DropDownList ID="drpClosureReason" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="10"/>
                                                </div> 
                                            </div>
                                            <div class="row mt-2" id="div1" runat="server" clientidmode="Static" style="border-radius: 5px;">
                                                <div class="input-field col m12">
                                                    <label class="active" for="uploadAudioGallery">Upload Audio</label>
                                                    <asp:FileUpload ID="uploadAudioGallery" multiple="multiple" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only Audio Files" /> 
                                                </div>
                                                <div class="input-field col m12">
                                                    <uc1:myAudioControl runat="server" ID="myAudioControl" />
                                                </div>
                                            </div>
                                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                                <button ID="btnReset" type="button" runat="server"  clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="11"><i class="material-icons left">backspace</i>Clear & Add New</button>
<%--                                                <div style="position:inherit;" class="fixed-action-btn right direction-top pt-0">
                                                    <a class="btn-small gradient-45deg-light-blue-cyan gradient-shadow"><i class="material-icons">email</i></a>
                                                    <ul>
                                                        <li><a href="css-helpers.html" class="btn-floating blue"><i class="material-icons">help_outline</i></a></li>
                                                        <li><a href="cards-extended.html" class="btn-floating green"><i class="material-icons">widgets</i></a></li>
                                                        <li><a href="app-calendar.html" class="btn-floating amber"><i class="material-icons">today</i></a></li>
                                                        <li>
                                                            <button id="btnFSaveEmail" type="button" runat="server" clientidmode="Static" class="btn-floating cyan mr-1" onserverclick="btnFSaveEmail_Click" tabindex="105">
                                                                <i class="material-icons left">email</i>
                                                            </button>
                                                        </li>
                                                    </ul>
                                                </div>--%>
                                                <a id="btnNotification"  runat="server" class="waves-effect waves-light gradient-45deg-deep-purple-blue btn modal-trigger z-depth-4 right ml-1 mr-1" href="#myNotificationModal"><i class="material-icons left">email</i>Save & Send Notfication</a>
                                                <%--<button ID="btnNotification" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1"><i class="material-icons left">email</i>Notification</button>--%>
                                                <button ID="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="12"><i class="material-icons left">save</i>Save</button>
                                                
                                            </div>
                                        </div>
                                        <div class="col m5">
                                            <div class="flex-lg-wrap">
                                            <a id="lnkPriorFollowup" runat="server" clientidmode="Static" class="hide accordian btn btn-primary w-100 text-left" data-toggle="collapse" href="#multiCollapseExample1" role="button" aria-expanded="false" aria-controls="multiCollapseExample1" visible="false">Click To Show Prior Followups</a>
                                            <div class="row">
                                                <div class="col m12 p-0">
                                                    <h4 class="card-title padding-2 m-0 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Follow-Up Log</h4>
                                                    <div class="collapse multi-collapse padding-1" id="multiCollapseExample1" style="height:410px; overflow-x:hidden; overflow-y:auto;">
                                                        <div class="card card-body">
                                                            <ul class="todo-list" style="list-style:none;">
                                                            <asp:Repeater ID="rptFollowupTrail" runat="server" ClientIDMode="Static" OnItemDataBound="rptFollowupTrail_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnItemID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                    <asp:HiddenField ID="hdnNextFollowup" runat="server" ClientIDMode="Static" Value='<%# Eval("NextFollowupDate") %>' />
                                                                    <asp:HiddenField ID="hdnFollInqNo" runat="server" ClientIDMode="Static" Value='<%# Eval("InquiryNo") %>' />
                                                                    <asp:HiddenField ID="hdnFollSource" runat="server" ClientIDMode="Static" Value='<%# Eval("InquirySource") %>' />
                                                                        <li id='liitem<%# Eval("pkID") %>' class="gradient-45deg-yellow-green mb-1" style="border-left: 2px solid #d6c7c7; padding:5px;">
                                                                            <span class="handle">
                                                                                <span class="text" style="color:black; font-weight:600; pointer-events: none;cursor: default !important;  text-transform:capitalize;">
                                                                                    <a title="Show Followup Info." href="javascript:void(0)" onclick="return false;"><%# Eval("CustomerName") %></a>
                                                                                </span>
                                                                                <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i>  <%# Eval("InquiryStatus") %> </small>
                                                                            </span><br />
                                                                            <span class="text" style="color:navy; font-size:12px; font-weight:400;"><%# Eval("MeetingNotes") %></span>
                                                                            <div class="row mt-2">
                                                                                <div class="col m6 float-left">
                                                                                    <small class="badge badge-primary w-100"><i class="fa fa-calendar-check-o"></i><span><b>Followup Date: </b></span><%# Eval("FollowupDate", "{0:dd-MM-yyyy}") %> </small>
                                                                                </div>
                                                                                <div class="col m6 float-right">
                                                                                    <small class="badge badge-secondary w-100 float-right"><i class="fa fa-calendar-check-o"></i><span><b>Inquiry #: </b></span>
                                                                                    <a title="Click to Show Inquiry" href="javascript:showInquiryInfo('<%# Eval("InquiryNo") %>');" style="color:navy;"><%# Eval("InquiryNo") %></a> 
                                                                                    </small>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row mt-2">
                                                                                <div class="col m6 float-left w-100">
                                                                                    <small class="badge badge-primary w-100"><i class="fa fa-calendar-check-o"></i>
                                                                                        <span><b>Next Followup :</b></span>
                                                                                        <span ID="ltrNextFollowup" runat="server" clientidmode="Static"><%# Eval("NextFollowupDate", "{0:dd-MM-yyyy}") %>&nbsp;<b><%# Eval("PreferredTime") %></b></span>    
                                                                                    </small>
                                                                                </div>
                                                                                <div class="col m6 float-right w-100">
                                                                                    <small class="badge badge-secondary w-100 float-right"><i class="fa fa-calendar-check-o"></i>
                                                                                        <span><b>Quotation :</b></span>
                                                                                          <a title="Click to Show Quotation" href="javascript:showQuotationInfo('<%# Eval("QuotationNo") %>');" style="color:navy;"><%# Eval("QuotationNo") %></a> 
                                                                                    </small>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row mt-1 ml-0 mr-0" style="background-color:navy;">
                                                                                <div class="col m12">
                                                                                    <small>
                                                                                        <span style="color:white; font-weight:800;">Initiated By :</span>
                                                                                        <span style="color:white;"><b>&nbsp;<%# Eval("EmployeeName") %> - <%# Eval("CreatedDate", "{0:dd-MM-yyyy hh:mm tt}") %></b></span>
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

                                </div>
                            </div>
                        </div>
                    </div>
                <%--**************************** Customer Popup ***********************************--%>
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
                
                <script type="text/javascript">
                    function sendWhatsAppUrl() {
                        alert('test')
                        var site = "https://wa.me/919898621973&text=Hello20%a20%Brother" + "&output=embed";;
                        $('#ifrModule').attr('src', site);
                        //document.getElementsByName('ifrModule')[0].src = site;
                    }
                    function bindDDLTo(selector) {
                        if ($(selector).val().length >= 1) {
                            jQuery.ajax({
                                type: "POST",
                                url: "InquiryInfo.aspx/FilterCustomer",
                                data: '{pCustName:\'' + $(selector).val() + '\'}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    var sample = JSON.parse(data.d);
                                    $(selector).autoComplete({
                                        minChars: 1,
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
                        else {
                            if ($(selector).val().length == 0) {
                                $("#hdnCustomerID").val('');
                            }
                        }
                    }
                </script>
                <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
        <div id="myNotificationModal" class="modal">
            <uc1:myNotificationPanel runat="server" id="myNotificationPanel" />
        </div>        
        <script type="text/javascript">
            $("#ucNotificationClose").click(function () {
                const elem = document.getElementById('myNotificationModal');
                const instance = M.Modal.init(elem, { dismissible: false });
                instance.open();
            });

        </script>
    </form>
</body>
</html>

