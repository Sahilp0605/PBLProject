<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FollowupExternal.aspx.cs" Inherits="StarsProject.FollowupExternal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ Register Src="~/myFollowupTimeline.ascx" TagPrefix="uc1" TagName="myFollowupTimeline" %>
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
                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnExtpkID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnEntryMode" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnFollowUpSource" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnAllowBackDatedFollowup" runat="server" ClientIDMode="Static" Value="Yes" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col m6 padding-1" style="min-height:450px; border-right:3px ridge silver;">
                                            <div class="row">
                                                <div class="input-field col m4">
                                                    <label class="active" for="drpFollowupType">Followup Type <span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:DropDownList ID="drpFollowupType" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="1"></asp:DropDownList>
                                                </div>
                                                <div class="input-field col m4">
                                                    <label class="active" for="txtFollowupDate">FollowUp Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:TextBox ID="txtFollowupDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date" />
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
                                                    <asp:TextBox ID="txtNextFollowupDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date" />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtNextFollowupDate">Preferred Time</label>
                                                    <asp:TextBox ID="txtPreferredTime" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="7" onkeypress="return false;" onpaste="return false" Style="width: 100px; display: inline-block;" ReadOnly="false" placeholder="00:00" />
                                                </div>
                                            </div>
                                            <div class="row mt-2" id="divStatus" runat="server" clientidmode="Static" style="border-radius: 5px;">
                                                <div class="input-field col m4">
                                                    <label class="active" for="drpLeadStatus">Lead Status</label>
                                                    <asp:DropDownList ID="drpLeadStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="15" Style="height: inherit;" placeholder="" AutoPostBack="true" OnSelectedIndexChanged="drpLeadStatus_SelectedIndexChanged">
                                                        <asp:ListItem Text="In-Process" Value="InProcess" Selected="True" />
                                                        <asp:ListItem Text="Disqualified" Value="Disqualified" />
                                                        <asp:ListItem Text="Qualified" Value="Qualified" />
                                                    </asp:DropDownList>
                                                </div>
                                                <div id="divClosureReason" runat="server" class="input-field col m4" visible="false">
                                                    <label runat="server" clientidmode="Static" class="active" for="drpClosureReason">Closure Reason</label>
                                                    <asp:DropDownList ID="drpClosureReason" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="10"/>
                                                </div> 
                                                <div id="divAssignTo" runat="server" class="input-field col m4" visible="false">
                                                    <label runat="server" clientidmode="Static" class="active" for="drpAssignTo">Assign To</label>
                                                    <asp:DropDownList ID="drpAssignTo" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="10"/>
                                                </div> 

                                            </div>
                                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                                <button ID="btnReset" type="button" runat="server" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabIndex="13"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                                <button ID="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="12"><i class="material-icons left">save</i>Save</button>
                                            </div>
                                        </div>
                                        <div class="col m6 padding-1" style="min-height:450px; border-right:3px ridge silver;">
                                            <uc1:myFollowupTimeline runat="server" ID="myFollowupTimeline" />
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </form>
</body>
</html>

