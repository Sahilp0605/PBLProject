<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebComplaintNew.aspx.cs" Inherits="StarsProject.WebComplaintNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myModuleAttachment.ascx" TagPrefix="uc1" TagName="myModuleAttachment" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css"/>
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
        .cardRound {
            box-shadow: black -6px 8px 6px -6px !important;
            border-radius: 8px 8px 0px 0px !important;
        }
        .h5Round {
            border-radius: 8px 8px 8px 8px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, #303F9F, #7B1FA2) !important;
            color: white !important;
        }
        .h5RoundHead {
            border-radius: 0px 0px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, silver, #7f88ec6e);
        }
        .webcenter {
        
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('.timepicker').timepicker();
            $('#spnModuleHeader').val("Complaint List");
        });

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
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

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

    <div id="contentwrapper" class="contentwrapper p-5">
        <div class="widgetbox">
            <div class="clearall"></div>
            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCustEmailAddress" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />

            <div id="myModal" style="display:block; width:100%;">
                <div class="modal-content">
                    <div class="modal-body" style="padding:50px;">
                        <div class="row">
                            <div class="col m12">
                                <img src="images/CompanyLogo/CompanyLogo.png" width="500" height="150" />
                            </div>
                            <div class="col m12" style="text-align:center">
                                <h3 class="h5Round">
                                    <span id="Span2" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Register Complaint</span>
                                </h3>
                            </div>
                            <%--<div class="col m12 webcenter">
                                <a id="lnkCustInfo" class="left" style="width:400px;" href="#custInfo"><h5 class="h5Round" style="text-align:center"><span id="Span3" runat="server" clientidmode="Static" class="text-top" style="color: white; text-align:center;">Customer Informations</span></h5></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                                <a id="lnkCompDetails" class="left" style="width:400px;margin-left:5em" href="#CompDetails"><h5 class="h5Round" style="text-align:center"><span id="Span4" runat="server" clientidmode="Static" class="text-top" style="color: white; text-align:center;">Complaint Details</span></h5></a>
                                <a id="lnkAttachment" class="left" style="width:400px;margin-left:5em" href="#attachment"><h5 class="h5Round" style="text-align:center"><span id="Span1" runat="server" clientidmode="Static" class="text-top" style="color: white; text-align:center;">Attachment</span></h5></a>
                            </div>--%>
                        </div>
                        <div class="row">
                            <div class="input-field col m12">
                                <div class="row">
                                    <div class="input-field col m2">
                                        <label class="active" for="lblComplaintNo">Complaint #</label>
                                        <asp:TextBox ID="lblComplaintNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder="" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtMobileNo">Mobile No.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtMobileNo" runat="server" class="form-control" ClientIDMode="Static" MaxLength="18"  TabIndex="2" placeholder="" />
                                    </div>
                                    <div class="input-field col m7">
                                        <label class="active" for="txtNameOfCustomer">Customer Name <span class="materialize-red-text font-weight-800">*</span>
                                            <a href="javascript:openCustomerInfo('view');">
                                                <img src="images/registration.png" style="padding: 6px 0px 0px 0px;" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" />
                                            </a>
                                        </label>
                                        <asp:TextBox ID="txtNameOfCustomer" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" placeholder="" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m4">
                                        <label class="active" for="txtDesignation">Designation<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtDesignation" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" placeholder="" />
                                    </div>
                                    <div class="input-field col m4">
                                        <label class="active" for="txtCompanyEmailID">Email ID<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtCompanyEmailID" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" placeholder="" />
                                    </div>
                                    <div class="input-field col m4">
                                        <label class="active" for="txtName">Name<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtName" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="5" placeholder="" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtWorkOdrNo">Work Order No.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtWorkOdrNo" runat="server" class="form-control"  MaxLength="20" ClientIDMode="Static" TabIndex="6" placeholder="" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtdtPurchase">Date Of Purchase<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtdtPurchase" runat="server" class="form-control" ClientIDMode="Static" TabIndex="7" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date" />
                                    </div>
                                </div>
                                   <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtPanelSRNo">Panel Serial No.</label>
                                        <asp:TextBox ID="txtPanelSRNo" runat="server" class="form-control"  MaxLength="20"  ClientIDMode="Static" TabIndex="8" placeholder="" />
                                    </div>
                                    <div class="input-field col m6">
                                        <label class="active" for="txtProductSRNO">Product Serial No.</label>
                                        <asp:TextBox ID="txtProductSRNO" runat="server" ClientIDMode="Static" MaxLength="20" class="form-control" TabIndex="9" placeholder="" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtSiteAdd">Site Address<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtSiteAdd" runat="server" class="form-control" ClientIDMode="Static" MaxLength="150" TabIndex="10" placeholder="" TextMode="MultiLine"/>
                                    </div>
                                    <div class="input-field col m6">
                                        <div class="row">
                                            <div class="input-field col m4">
                                                <label class="active" for="drpCountry">Country <span class="materialize-red-text font-weight-800">*</span>
                                                    <a href="javascript:openRegion('add');">
                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add Country" title="Add Country" />
                                                    </a>
                                                </label>
                                                <asp:DropDownList ID="drpCountry" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="11" AutoPostBack="true"  OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"/>
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="drpState">State<span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:DropDownList id="drpState" runat="server" CssClass="browser-default" ClientIDMode="Static" TabIndex="12" AutoPostBack="true" OnSelectedIndexChanged="drpState_SelectedIndexChanged"/>
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="drpCity">City<span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:DropDownList id="drpCity" runat="server" CssClass="browser-default" ClientIDMode="Static"  TabIndex="13" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m6">
                                                <label class="active" for="txtPinCode">Pin Code<span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtPinCode" runat="server" class="form-control" ClientIDMode="Static" TabIndex="14" placeholder="" />
                                            </div>
                                        </div>
                                   </div>

                                </div>
                                <div class="row">
                                    <div class="input-field col m3">
                                        <label class="active" for="txtSiteCoordinatorName">Site Coordinator Name<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtSiteCoordinatorName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="15" placeholder="" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtsiteMobileNo">Contact No.<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtsiteMobileNo" runat="server" MaxLength="18" class="form-control" ClientIDMode="Static" TabIndex="16" placeholder=""/>
                                    </div>                                        
                                    <div class="input-field col m3">
                                        <label class="active" for="txtConvinientDate">Convinient Date </label>
                                        <asp:TextBox ID="txtConvinientDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="17" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtConvinientTimeSlot">Convinient Time Slot</label>
                                        <asp:TextBox ID="txtConvinientTimeSlot" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="18" onkeypress="return false;" onpaste="return false" Style="width: 350px; display: inline-block;" ReadOnly="false" placeholder="" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtComplaintNotes">Complaint Description<span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtComplaintNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="19" MaxLength="500" TextMode="MultiLine" placeholder="" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="drpStatus">Status</label>
                                        <asp:DropDownList ID="drpStatus" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="20">
                                            <asp:ListItem Text="Open" Value="Open" Selected="True" />
                                            <asp:ListItem Text="Close" Value="Close" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="attachment" runat="server" style="display:block"> 
                                    <div class="row">
                                        <div class="col m12">
                                            <uc1:myModuleAttachment runat="server" id="myAttachPanel" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col m12">
                                            <uc1:myModuleAttachment runat="server" id="myAttachDefect" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                <button id="btnReset" type="button" runat="server"  clientidmode="Static" class="btn orange left" onserverclick="btnReset_ServerClick" TabIndex="23"><i class="material-icons left">backspace</i>Clear & Add New</button>
                <button id="btnSaveEmail" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSaveEmail_ServerClick" TabIndex="22"><i class="material-icons left">email</i>Save - Send Email</button>
                <button id="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_ServerClick" TabIndex="21"><i class="material-icons left">save</i>Save</button>
            </div>  
        </div>
        <%--**************************** Customer Popup ***********************************--%>
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
        </cc1:ModalPopupExtender>  
        <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
            <div id="myModal111">
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
    </div>
    <script type="text/javascript">
        function bindDDLTo(selector) {
            if ($(selector).val().length >= 3) {
                jQuery.ajax({
                    type: "POST",
                    url: "InquiryInfo.aspx/FilterCustomerByModule",
                    data: '{pCustName:\'' + $(selector).val() + '\', pSearchModule:\'CustomerSearchAll\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        console.log(data);
                        //var resultdata = JSON.parse(data.d);
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
                                return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                            },
                            onSelect: function (e, term, item) {
                                $(selector).val(item.html());
                                $("#hdnCustomerID").val(item.data('langname'));
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
    </form>
</body>
</html>

