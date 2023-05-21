<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerHexagon.aspx.cs" Inherits="StarsProject.CustomerHexagon" %>
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
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    <style type="text/css">
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
            $('.datepicker').datepicker({
                defaultDate: new Date((new Date()).getFullYear(), (new Date()).getMonth(), (new Date()).getDay()),
                format: "dd-mm-yyyy",
                minDate: new Date(1990, 12, 31)
            });
            $('.timepicker').timepicker();

            $('.tabs').tabs();
            /*---------------------------------------------------------*/
            setHiddenControls();
        });
        /*---------------------------------------------------------*/
        function setHiddenControls() {
            $('#frmProduct *').filter(':input').each(function () {
                var idName = $(this).attr('id') + ',';
                if ($("#hdnHiddenControl").val().indexOf(idName) >= 0) {
                    $(this).parent().hide();
                }
            });
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
        function ShowPDFfile(repFilename) {
            repFilename = "CustomerDocs/" + repFilename;
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }
        // -------------------------------------------------------
        function openFormContainerNew() {
            clearFormData();
            // ------------------------------------------
            jQuery(".usermsg").empty();
            jQuery('#myModal').modal({ "show": true });
        }

        function openFormContainer() {

            jQuery('#myModal').modal({ "show": true });
        }

        function btnCloseDialog() {
            jQuery('.modal-backdrop').remove();
            jQuery('#myModal').modal('hide'); //or  $('#IDModal').modal('hide');
            jQuery('.modal-backdrop').remove();
        }

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');

            if (jQuery("#hdnPageMode").val() == 'supplier')
                jQuery("#drpCustomerType option[value='Supplier']").attr("selected", true);
            else
                jQuery("#drpCustomerType option[value='Customer']").attr("selected", true);
        }

        function openRILLog() {
            var pageUrl = "RILLog.aspx";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }


        function setCustomerContact() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtCustomerName").val() == "" || jQuery("#drpOrgType").val() == "") {

                if (jQuery("#txtCustomerName").val() == "")
                    errmsg += "Customer Name is required</br>";

                if (jQuery("#drpOrgType").val() == "")
                    errmsg += "Organization Type is required</br>";

                jQuery("#divErrorMessage").html(errmsg).show();
                validflag = false;
            }
            // -----------------------------------------------
            if (validflag) {
                jQuery.ajax({
                    url: 'Services/NagrikService.asmx/setCustomerContacts',
                    method: 'post',
                    data: '{cust: ' + JSON.stringify(cust) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (resp) {
                        alert(resp.d);
                    },
                    error: function (err) { }
                });
                // -----------------------------------------------
                var cust = {};

                if (jQuery('#hdnCustomerID').val() == "" || jQuery('#hdnCustomerID').val() == "0")
                    cust.CustomerID = "0";
                else
                    cust.CustomerID = jQuery('#hdnCustomerID').val();

                cust.ContactPerson1 = jQuery('#txtContactPerson1').val();
                cust.ContactDesigCode1 = jQuery('#drpContactDesigCode1').val();
                cust.ContactNumber1 = jQuery('#txtContactNumber1').val();
                cust.ContactEmail1 = jQuery('#txtContactEmail1').val();
                cust.LoginUserID = 'admin';


                jQuery.ajax({
                    url: 'Services/NagrikService.asmx/setCustomerContacts',
                    method: 'post',
                    data: '{cust: ' + JSON.stringify(cust) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (resp) {
                        alert(resp.d);
                    },
                    error: function (err) { }
                });
            }
        }

        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        /*---------------------------------------------------------*/

        function pageLoad(sender, args) {
            retainTabPosition();
        }

        $(function () {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "custaddress";
            $("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "custaddress";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab").val(currTab);
                //alert("Curr.Tab Gen.Func : " + currTab)
            });
        });
        function openProductInfo(cat) {
            var keyid = "0";
            var pageUrl = "ProductHexagon.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        function retainTabPosition() {
            $('#lnkcustaddress').removeClass('active');
            $('#lnkcustdocs').removeClass('active');
            $('#lnkcustcont').removeClass('active');
            $('#lnkprocust').removeClass('active');

            $('#custaddress').css("display", "none");
            $('#custdocs').css("display", "none");
            $('#custcont').css("display", "none");
            $('#procust').css("display", "none");
            // ------------------------------------------------------------
            if ($("#hdnCurrentTab").val() == 'custaddress') {
                $('#lnkcustaddress').addClass('active');
                $('#custaddress').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'custcont') {
                $('#lnkcustcont').addClass('active');
                $('#custcont').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'custdocs') {
                $('#lnkcustdocs').addClass('active');
                $('#custdocs').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'custPro') {
                $('#lnkcustpro').addClass('active');
                $('#custPro').css("display", "block");
            }
            if ($("#hdnCurrentTab").val() == 'procust') {
                $('#lnkprocust').addClass('active');
                $('#procust').css("display", "block");
            }
        }
        function openRegion(cat) {
            var pageUrl = "ManageCity.aspx?mode=add&id=0";
            spnPageModuleHeader.innerText = "Manage Region";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
    <asp:HiddenField ID="hdnPageMode" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientERPIntegration" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnStateCode" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCustWisePro" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnRILPrice" runat="server" ClientIDMode="Static" Value="" />
    <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" Value="custaddress" EnableViewState="true" />
    <asp:HiddenField ID="hdnIsPriceListCustomer" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnHiddenControl" runat="server" ClientIDMode="Static" Value=""/>

    <asp:ScriptManager ID="srcUser" runat="server" EnableViewState="true" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">
        <%--<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <div class="widgetbox">

            <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />

            <div id="myModal" style="display:block; width:100%; padding:10px;">
                <div class="modal-content">
                    <div class="modal-body" style="min-height:300px;">
                        <div class="row">
                            <div class="input-field col m4">
                                <asp:TextBox ID="txtCustomerName" runat="server" class="form-control" ClientIDMode="Static" MaxLength="100" Placeholder="" TabIndex="1" />
                                <label id="lblCustomerName" for="txtCustomerName" class="active">Customer/Company Name <span class="materialize-red-text font-weight-800">*</span></label>
                            </div>
                            <div class="input-field col m2">
                                <asp:DropDownList ID="drpCustomerType" runat="server" ClientIDMode="Static" class="select2-theme browser-default" MaxLength="50" TabIndex="2"></asp:DropDownList>
                                <label class="active" for="drpCustomerType">Category <span class="materialize-red-text font-weight-800">*</span></label>
                            </div>
                            <div class="input-field col m2">
                                <asp:DropDownList ID="drpCustomerSource" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="3"></asp:DropDownList>
                                <label class="active" for="drpCustomerSource">Source</label>
                            </div>
                            <%--/*<% if (hdnSerialKey.Value != "ZE5W-HOME-AG41-SF61") /*"ZEMOTE"*/--%>
                            <% if (hdnIsPriceListCustomer.Value == "yes" || hdnIsPriceListCustomer.Value == "y" || hdnIsPriceListCustomer.Value == "1")  
                            {%>
                            <div class="input-field col m2 hide">
                                <asp:DropDownList ID="drpPriceList" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="4"></asp:DropDownList>
                                <label class="active" for="drpPriceList">Price List </label>
                            </div>
                            <%} else {%>
                            <div class="input-field col m2">
                                
                            </div>
                             <%}%>
                            <div class="input-field col m2 hide">
                                <label class="active" for="chkBlock">Active Status</label>
                                <div class = "switch"><label>Inactive
                                    <asp:CheckBox ID="chkBlock" runat="server" ClientIDMode="Static" Checked="true" TabIndex="4"/>
                                    <span class="lever"></span>Active</label>
                                </div> 
                            </div>
                        </div>

                        <div class="row">
                            <div class="col m9">
                                <asp:UpdatePanel ID="uPnlBasicInfo" runat="server">
                                    <ContentTemplate>
                                        <div class="card">
                                    <div class="card-title white-text gradient-45deg-indigo-purple" style="padding:10px;">
                                        <p class="no-margin">Basic Information</p>
                                    </div>
                                    <div class="card-content">
                                        <div class="row">
                                            <div class="input-field col m2">
                                                <asp:TextBox ID="txtContactNo1" class="form-control" runat="server" ClientIDMode="Static" Placeholder="" MaxLength="15" TabIndex="5" />
                                                <label class="active" for="txtContactNo1">Contact #1 <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                            <div class="input-field col m2">
                                                <asp:TextBox ID="txtContactNo2" runat="server" TabIndex="6" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="15" />
                                                <label class="active" for="txtContactNo2">Contact #2</label>
                                            </div>
                                            <div class="input-field col m5">
                                                <label class="active" for="txtEmailAddress">Email Address</label>
                                                <asp:TextBox ID="txtEmailAddress" class="form-control" runat="server" ClientIDMode="Static"  MaxLength="50" Placeholder="" TabIndex="7"  />
                                            </div>
                                            <div class="input-field col m3">
                                                <label class="active" for="txtWebsite">Website</label>
                                                <asp:TextBox ID="txtWebsite" class="form-control" runat="server" ClientIDMode="Static" MaxLength="50" TabIndex="8"  Placeholder="" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m2">
                                                <asp:TextBox ID="txtGSTNo" class="form-control" runat="server"   ClientIDMode="Static" Placeholder="" MaxLength="50" TabIndex="9"
                                                    AutoPostBack="true" OnTextChanged="txtGSTNo_TextChanged"/>
                                                <label class="active" for="txtGSTNo">GST #</label>
                                            </div>
                                            <div class="input-field col m2">
                                                <asp:TextBox ID="txtPANNo" class="form-control" runat="server" TabIndex="10" MaxLength="50"  ClientIDMode="Static" Placeholder="" />
                                                <label class="active" for="txtPANNo">PAN #</label>
                                            </div>
                                            <div class="input-field col m2 ">
                                                <asp:TextBox ID="txtRemarks" class="form-control" runat="server" TabIndex="11" Width="700px" TextMode="MultiLine" ClientIDMode="Static" Placeholder="" />
                                                <label class="active" for="txtRemarks">Remarks </label>
                                            </div>
                                            <div class="input-field col m2 hide">
                                                <asp:TextBox ID="txtCINNo" class="form-control" runat="server" TabIndex="11" MaxLength="50" ClientIDMode="Static" Placeholder="" />
                                                <label class="active" for="txtCINNo">CIN #</label>
                                            </div>
                                           <% if (hdnSerialKey.Value != "JAYJ-ALAR-AMBR-ICKS")
                                               { %>
                                            <div class="input-field col m3 hide" visible="false">
                                                <label class="active" for="txtBirthDate">BirthDate</label>
                                                <asp:TextBox ID="txtBirthDate" class="form-control" runat="server" ClientIDMode="Static" TabIndex="12"  Placeholder="" TextMode="Date"  />
                                            </div>
                                            <div class="input-field col m3 hide">
                                                <label class="active" for="txtAnniversaryDate">Anniver.Date</label>
                                                <asp:TextBox ID="txtAnniversaryDate" class="form-control" runat="server" ClientIDMode="Static" TabIndex="15"  Placeholder=""  TextMode="Date"/>
                                            </div>
                                            <% } %>
                                        </div>
                                    </div>
                                </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col m3 hide">
                                <div class="card">
                                    <div class="card-title white-text gradient-45deg-indigo-purple" style="padding:10px;">
                                        <p class="no-margin">Ledger Balance</p>
                                    </div>
                                    <div class="card-content padding-3">
                                    <div class="row">
                                        <div class="col m12 pt-1">
                                            Opening
                                            <div class="input-field inline mt-0 float-right">
                                                <asp:TextBox ID="txtOpening" class="form-control m-0 right-align" runat="server" ClientIDMode="Static" TabIndex="20"  Placeholder="" AutoPostBack="true" OnTextChanged="txtOpening_TextChanged"/>
                                            </div>
                                        </div>
                                        <div class="col m12">
                                            Debit
                                            <div class="input-field inline mt-0 float-right">
                                                <asp:TextBox ID="txtDebit" class="form-control m-0 right-align" runat="server" ClientIDMode="Static" ReadOnly="true"   Placeholder=""/>
                                            </div>
                                        </div>
                                        <div class="col m12">
                                            Credit
                                            <div class="input-field inline mt-0 float-right">
                                                <asp:TextBox ID="txtCredit" class="form-control m-0 right-align" runat="server" ClientIDMode="Static" ReadOnly="true" Placeholder=""/>
                                            </div>
                                        </div>
                                        <div class="col m12">
                                            Closing
                                            <div class="input-field inline mt-0 float-right">
                                                <asp:TextBox ID="txtClosing" class="form-control m-0 right-align" runat="server" ClientIDMode="Static" ReadOnly="true"   Placeholder=""/>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col m9 hide">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                    <div class="card">
                                    <div class="card-title white-text gradient-45deg-indigo-purple" style="padding:10px;">
                                        <p class="no-margin">Credit Information</p>
                                    </div>
                                    <div class="card-content">
                                        <div class="row">
                                            <div class="input-field col m2">
                                                <asp:TextBox ID="txtCrDays" class="form-control" runat="server" ClientIDMode="Static" Placeholder="" MaxLength="15" TabIndex="18" />
                                                <label class="active" for="txtContactNo1">CR Days <span class="materialize-red-text font-weight-800"></span></label>
                                            </div>
                                            <div class="input-field col m2">
                                                <asp:TextBox ID="txtCrLimit" runat="server" TabIndex="19" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="15" />
                                                <label class="active" for="txtContactNo2">CR Limit</label>
                                            </div>
                                        </div>
                                    </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divFollowUp" runat="server" clientidmode="Static" class="row">
                                <div class="input-field col m12">
                                    <div class="card small" style="height:100px;">
                                        <div class="card-header"><p class="card-title ml-1">Create Followup</p></div>
                                        <div class="card-content row" style="padding:5px !important;">
                                            <div class="input-field col m3">
                                                <asp:TextBox ID="txtNextFollowupDate" class="form-control" runat="server" ClientIDMode="Static" Placeholder="" TabIndex="21" TextMode="Date"/>
                                                <label class="active" for="txtNextFollowupDate">Next Followup</label>
                                            </div>
                                            <div class="input-field col m3">        
                                                <asp:DropDownList ID="drpFollowupType" class="select2-theme browser-default" runat="server" ClientIDMode="Static" TabIndex="24"></asp:DropDownList>
                                                <label class="active" for="drpFollowupType">Followup Type</label>
                                            </div>
                                            <div class="input-field col m6">
                                                <asp:TextBox ID="txtMeetingNotes" class="materialize-textarea" runat="server" ClientIDMode="Static" TabIndex="25" Placeholder="" MaxLength="500" Rows="4" />
                                                <label class="active" for="txtMeetingNotes">Meeting Notes</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    </div>
                    <div class="row">
                        <div class="input-field col m12 mt-0">
                            <div class="row" style="box-shadow: navy -6px 8px 6px -6px;">
                                <div class="card user-card-negative-margin z-depth-0" id="feed" style="margin:0px;">
                                    <div class="card-content card-border-gray" style="height:500px;padding-top: 5px;">
                                        <div class="row">
                                            <ul id="myTab" class="tabs card-border-gray" runat="server" clientidmode="Static" enableviewstate="true">
                                                <li class="tab col m3 p-0"><a id="lnkcustaddress" class="left active" href="#custaddress">Address</a></li>
                                                <li class="tab col m3 p-0"><a id="lnkcustcont" class="left active" href="#custcont">Primary Contacts</a></li>
                                                <li class="tab col m3 p-0 hide"><a id="lnkcustdocs" class="left" runat="server" clientidmode="Static" href="#custdocs">Documents</a></li>
                                                <% if (hdnCustWisePro.Value == "yes")
                                                    { %>
                                                <li class="tab col m3 p-0 hide"><a id="lnkcustpro" class="left active" href="#custpro">Products</a></li>
                                                <%} %>
                                                <li class="tab col m3 p-0"><a id="lnkprocust" class="left" href="#procust">Products</a></li>
                                            </ul>
                                        </div>

                                        <div id="custaddress" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                            <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                <div id="container1" runat="server" clientidmode="Static" class="container">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col m12 pr-0">
                                                                <div class="card small" style="height:350px;">
                                                                    <div class="card-title blue white-text" style="padding:10px;">
                                                                        <p class="no-margin">Address Details</p>
                                                                    </div>
                                                                    <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m12">
                                                                                <asp:TextBox ID="txtAddress" runat="server" class="form-control" Placeholder="" TabIndex="27" MaxLength="100"  ClientIDMode="Static" TextMode="MultiLine"/>
                                                                               <label class="active" for="txtAddress">Address </label> <%--<span class="materialize-red-text font-weight-800">*</span></label>--%>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m4">
                                                                                <asp:TextBox ID="txtArea" runat="server" TabIndex="28" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                                                <label class="active" for="txtArea">Area</label>
                                                                            </div>
                                                                            <div class="input-field col m4">
                                                                                <asp:DropDownList ID="drpCountry" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="29" AutoPostBack="true"  OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"/>
                                                                                <label class="active" for="dwrpCountry">Country <span class="materialize-red-text font-weight-800">*</span>
                                                                                    <a href="javascript:openRegion('add');">
                                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add Country" title="Add Country" />
                                                                                    </a>
                                                                                </label>
                                                                            </div>
                                                                            <div class="input-field col m4">
                                                                                <asp:DropDownList ID="drpState" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="30" AutoPostBack="true"  OnSelectedIndexChanged="drpState_SelectedIndexChanged"/>
                                                                                <% if (hdnSerialKey.Value.Contains("PRI9-DG8H-G6GF-TP5V"))
                                                                                    { %>
                                                                                        <label class="active" for="drpState">State </label>
                                                                                <% }
                                                                                    else
                                                                                    { %>
                                                                                        <label class="active" for="drpState">State <span class="materialize-red-text font-weight-800">*</span>
                                                                                            <a href="javascript:openRegion('add');">
                                                                                                <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add State" title="Add State" />
                                                                                            </a>
                                                                                        </label>
                                                                                <% }; %>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m6">
                                                                                <asp:DropDownList ID="drpCity" class="select2-theme browser-default" runat="server"  ClientIDMode="Static" TabIndex="31" />
                                                                                <% if (hdnSerialKey.Value.Contains("PRI9-DG8H-G6GF-TP5V"))
                                                                                    { %>
                                                                                        <label class="active" for="drpCity">City </label>
                                                                                <% }
                                                                                    else
                                                                                    { %>
                                                                                        <label class="active" for="drpCity">City <span class="materialize-red-text font-weight-800">*</span>
                                                                                            <a href="javascript:openRegion('add');">
                                                                                                <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add City" title="Add City" />
                                                                                            </a>
                                                                                        </label>
                                                                                <% }; %>
                                                                            </div>
                                                                            <div class="input-field col m6">
                                                                                <asp:TextBox ID="txtPincode" runat="server" TabIndex="32" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="24"/>
                                                                                <label class="active" for="txtPincode">Pincode</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                             <div class="col m6 pr-0 hide">
                                                                <div class="card small" style="height:400px;">
                                                                    <div class="card-title blue white-text" style="padding:10px;">
                                                                        <p class="no-margin">Shipping Address</p>
                                                                    </div>
                                                                    <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m6">
                                                                                <asp:TextBox ID="txtShipToCompName" runat="server" TabIndex="33" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"  />
                                                                                <label class="active" for="txtShipToCompName"><% if (hdnSerialKey.Value == "BLUE-CHEM-56JK-BC88") { %>Transportation <% } else {%> Company Name <%} %> </label>
                                                                            </div>
                                                                            <div class="input-field col m6">
                                                                                <asp:TextBox ID="txtShipToGSTNo" runat="server" TabIndex="34" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100" />
                                                                                <label class="active" for="txtShipToGSTNo"><% if (hdnSerialKey.Value == "BLUE-CHEM-56JK-BC88") { %>Branch <% } else {%> GST No. <%} %> </label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m12">
                                                                                <asp:TextBox ID="txtAddress1" runat="server" class="form-control" Placeholder="" TabIndex="35" MaxLength="100"  ClientIDMode="Static" TextMode="MultiLine"/>
                                                                                <label class="active" for="txtAddress1">Address </label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m4">
                                                                                <asp:TextBox ID="txtArea1" runat="server" TabIndex="36" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"  />
                                                                                <label class="active" for="txtArea1">Area</label>
                                                                            </div>
                                                                            <div class="input-field col m4">
                                                                                <asp:DropDownList ID="drpCountry1" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="37" AutoPostBack="true"  OnSelectedIndexChanged="drpCountry1_SelectedIndexChanged"/>
                                                                                <label class="active" for="drpCountry1">Country 
                                                                                    <a href="javascript:openRegion('add');">
                                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add Country" title="Add Country" />
                                                                                    </a>
                                                                                </label>
                                                                            </div>
                                                                            <div class="input-field col m4">
                                                                                <asp:DropDownList ID="drpState1" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="38" AutoPostBack="true"  OnSelectedIndexChanged="drpState1_SelectedIndexChanged"/>
                                                                                <label class="active" for="drpState1">State 
                                                                                    <a href="javascript:openRegion('add');">
                                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add State" title="Add State" />
                                                                                    </a>
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m6">
                                                                                <asp:DropDownList ID="drpCity1" class="select2-theme browser-default" runat="server"  ClientIDMode="Static" TabIndex="39" />
                                                                                <label class="active" for="drpCity1">City 
                                                                                    <a href="javascript:openRegion('add');">
                                                                                        <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add City" title="Add City" />
                                                                                    </a>
                                                                                </label>
                                                                            </div>
                                                                            <div class="input-field col m6">
                                                                                <asp:TextBox ID="txtPincode1" runat="server" TabIndex="40" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="30"/>
                                                                                <label class="active" for="txtPincode1">Pincode</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="custcont" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                            <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                <div id="Div2" runat="server" clientidmode="Static" class="container">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col m12">
                                                                <div class="card small" style="height:350px;">
                                                                     <div class="card-title blue white-text" style="padding:10px;">
                                                                        <p class="no-margin">Company's Primary Contacts</p>
                                                                         <a id="lnkFilter" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped btn-move-up accent-2 z-depth-4 right" data-position="center" data-tooltip="Add New Contact"  style="padding:5px; top:-30px; right:-5px;"><i class="material-icons activator">add</i></a>
                                                                    </div>
                                                                         <div class="card-content row" style="height: 280px; width: 100%; padding: 0px; margin: 0px;overflow-y:auto;">
                                                                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <asp:Repeater ID="rptContacts" runat="server" ClientIDMode="Static" OnItemDataBound="rptContacts_ItemDataBound" OnItemCommand="rptContacts_ItemCommand">
                                                                            <HeaderTemplate>
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th class="center">Contact Person</th>
                                                                                        <th class="center">Contact #</th>
                                                                                        <th class="center">Email Address</th>
                                                                                        <th class="center">Designation</th>
                                                                                        <th class="center" colspan="2">Action</th>
                                                                                    </tr>
                                                                                </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnpkIDContact" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                                <asp:HiddenField ID="hdnDesigCode" runat="server" ClientIDMode="Static" Value='<%# Eval("ContactDesigCode1") %>' />
                                                                                <tr class="blueShed">
                                                                                    <td class="center">
                                                                                        <asp:TextBox ID="txtContactPerson0" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("ContactPerson1") %>' TabIndex="26" /></td>
                                                                                    <td class="center">
                                                                                        <asp:TextBox ID="txtContactNumber0" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("ContactNumber1") %>' TabIndex="27" /></td>
                                                                                    <td class="center">
                                                                                        <asp:TextBox ID="txtContactEmail0" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("ContactEmail1") %>' TabIndex="28" /></td>
                                                                                    <td class="center">
                                                                                        <asp:DropDownList ID="drpContactDesigCode0" runat="server" ClientIDMode="Static" CssClass="select2-theme browser-default" TabIndex="29" Style="font-size: 1rem;box-sizing: content-box;width: 100%;height: 2.0rem;margin: 0 0 10px 0;padding: 0 0 0 5px;-webkit-transition: box-shadow .3s, border .3s;transition: box-shadow .3s, border .3s;border: 2px solid #9e9e9e;border-radius: 5px;outline: none;background-color: transparent;box-shadow: none;" />
                                                                                    </td>
                                                                                    <td class="center">
                                                                                        <asp:ImageButton ID="ImgbtnUpdate" runat="server" ImageUrl="~/images/SaveData.png" CommandName="Update" CommandArgument='<%# Eval("pkID") %>' Width="35" Height="20" TabIndex="30" />
                                                                                    </td>
                                                                                    <td class="center">
                                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="31" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>

                                                                        </asp:Repeater>
                                                                        </table>
                                                                    </div>
                                                                    <div class="card-reveal gradient-45deg-light-blue-teal gradient-shadow black-text">
                                                                        <span class="card-title grey-text text-darken-4">Create New Contact<i class="material-icons right">close</i></span>
                                                                        <div id="profile-card1" class="card animate fadeRight mb-0">
                                                                            <div class="card-content p-0 col m12">
                                                                                <div class="row">
                                                                                    <div class="input-field col m8">
                                                                                        <asp:TextBox ID="txtContactPerson1" runat="server" class="form-control" ClientIDMode="Static" placeholder=" " TabIndex="31" MaxLength="50" />
                                                                                        <label class="active" for="txtContactPerson1">Contact Person</label>
                                                                                    </div>
                                                                                    <div class="input-field col m4">
                                                                                        <asp:DropDownList ID="drpContactDesigCode1" runat="server" ClientIDMode="Static" class="select2-theme browser-default" placeholder=" "  TabIndex="32" />
                                                                                        <label class="active" for="drpContactDesigCode1">Designation</label>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="input-field col m4">
                                                                                        <asp:TextBox ID="txtContactNumber1" runat="server" class="form-control" ClientIDMode="Static" placeholder=" "  TabIndex="33" MaxLength="15" />
                                                                                        <label class="active" for="txtContactNumber1">Contact #</label>
                                                                                    </div>
                                                                                    <div class="input-field col m8">
                                                                                        <asp:TextBox ID="txtContactEmail1" runat="server" class="form-control" ClientIDMode="Static" placeholder=" "  TabIndex="34" MaxLength="100" />
                                                                                        <label class="active" for="txtContactEmail1">Email ID</label>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row">
                                                                                    <div class="input-field col m12">
                                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add.png" data-tooltip="Add Contacts" Width="110" Height="30" TabIndex="35" OnClick="imgBtnSave_Click" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="custdocs" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                            <div class="input-field col m12" style="height:400px;">
                                                <div id="Div3" runat="server" clientidmode="Static" class="container">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="section app-file-manager-wrapper">
                                                        <div class="app-file-overlay"></div>
                                                        <div class="row">
                                                            <div class="app-file-area" style="height:400px;">
                                                                <div class="app-file-header">
                                                                    <div class="row">
                                                                        <div class="input-field col m12 add-new-file mt-0">
                                                                            <div class="col m12">
                                                                                <label class="active" for="uploadDocument">Upload Customer Document Here  </label>
                                                                            </div>
                                                                            <div class="col m12">
                                                                                <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                                                <asp:Button ID="btnUploadDoc" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="app-file-content">
                                                                    <div class="app-file-sidebar-content row">
                                                                        <div class="col m12 app-file-files">
                                                                            <asp:Repeater ID="rptEmpDocs" runat="server" ClientIDMode="Static" OnItemCommand="rptEmpDocs_ItemCommand">
                                                                            <ItemTemplate>
                                                                                <div class="col xl4 l6 m4 s6">
                                                                                    <div class="card box-shadow-none mb-1 app-file-info">
                                                                                        <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                        <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                            <div class="row fonticon p2">
                                                                                                <div class="col m6" style="text-align:left;">
                                                                                                    <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                                    <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                                                </div>
                                                                                                <div class="col m6" style="text-align:right;">
                                                                                                    <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                                    <i class="material-icons" style="padding: 5px 0px 0px 20px;">find_in_page</i>
                                                                                                    </a>
                                                                                                </div>
                                                                                            </div>
                                                                                            <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                        </div>
                                                                                        <div class="app-file-details">
                                                                                            <div class="app-file-name font-weight-700">
                                                                                                <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(6) %></p>
                                                                                            </div>
                                                                                        </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnUploadDoc" />
                                                    </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <div id="custpro" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                            <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                <div id="Div4" runat="server" clientidmode="Static" class="container">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col m12">
                                                                <div class="card small" style="height:350px;">
                                                                     <div class="card-title blue white-text row" style="padding:10px;">
                                                                        <div class="col m3">
                                                                            <p class="no-margin">Customer's Products</p>
                                                                        </div>
                                                                        <div class="col m4 right-align">
                                                                            <label class="active" for="txtRILPrice" style="color:white;font-size:large";>RIL Price</label>
                                                                            <asp:TextBox ID="txtRILPrice" runat="server" class="form-control" ClientIDMode="Static" placeholder=" "  TabIndex="47" MaxLength="15" Width="100px" BackColor="White"/>
                                                                        </div>
                                                                        <div class="col m3" style="width:20%;">
                                                                                <button ID="btnUpdateRILPrice" type="button" runat="server" clientidmode="Static" Text="Update RIL Price" class="btn cyan right mr-1" onserverclick="btnUpdateRILPrice_ServerClick" TabIndex="48">Update RIL Price</button>
                                                                        </div>
                                                                         <div class="col m3" style="width:20%;">
                                                                                <button ID="btnRilLog" type="button" runat="server" clientidmode="Static" Text="RIl Price Log" class="btn cyan right mr-1" onclick="return openRILLog();" TabIndex="49">RIL Price Log</button>
                                                                        </div>
                                                                        <a id="lnkFilter1" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped btn-move-up accent-2 z-depth-4 right hide" data-position="center" data-tooltip="Add New Product"  style="padding:5px; top:-30px; right:-5px;"><i class="material-icons activator">add</i></a>
                                                                    </div>
                                                                    <div class="card-content row" style="height: 280px; width: 100%; padding: 0px; margin: 0px;overflow-y:auto;">
                                                                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <asp:Repeater ID="rptProducts" runat="server" ClientIDMode="Static" OnItemDataBound="rptProducts_ItemDataBound" OnItemCommand="rptProducts_ItemCommand">
                                                                            <HeaderTemplate>
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th class="left-align" style="width: 20%;">Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                                            <a href="javascript:openProductInfo('add');">
                                                                                                <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" />
                                                                                            </a>
                                                                                        </th>
                                                                                        <th>Size</th>
                                                                                        <th>Weight</th>
                                                                                        <th>Conversion</th>
                                                                                        <th>Rate</th>
                                                                                        <th class="text-center" colspan="2">Action</th>
                                                                                    </tr>
                                                                                </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnpkIDProduct" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                                <%--<asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />--%>
                                                                                <tr class="blueShed">
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtProduct" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("ProductName") %>' TabIndex="48" ReadOnly="true"/></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtSize" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("UnitSize") %>' TabIndex="49" ReadOnly="true" /></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("Box_Weight") %>' TabIndex="50" ReadOnly="true" /></td>
                                                                                     <td>
                                                                                        <asp:TextBox ID="txtConversion" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("ConversionRate") %>' TabIndex="51" AutoPostBack="true" OnTextChanged="txtConversion_TextChanged"/></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtBagRate" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("RatePerBag") %>' TabIndex="52"  ReadOnly="true"/></td>
                                                                                     <td class="text-center">
                                                                                        <asp:ImageButton ID="ImgbtnUpdate" runat="server" ImageUrl="~/images/SaveData.png" CommandName="Update" CommandArgument='<%# Eval("pkID") %>' Width="35" Height="20" TabIndex="53" />
                                                                                   
                                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="54" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:HiddenField ID="hdnpkIDProduct" runat="server" ClientIDMode="Static" />
                                                                                <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                                <tr class="blueShed">
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtProductC" runat="server" CssClass="form-control" ClientIDMode="Static"  TabIndex="55" onKeyup="bindDDLProductTo('#txtProductC')" AutoPostBack="true" OnTextChanged="txtProductC_TextChanged"/></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtSizeC" runat="server" CssClass="form-control" ClientIDMode="Static"  TabIndex="56" /></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtWeightC" runat="server" CssClass="form-control" ClientIDMode="Static"  TabIndex="57" /></td>
                                                                                     <td>
                                                                                        <asp:TextBox ID="txtConversionC" runat="server" CssClass="form-control" ClientIDMode="Static"  TabIndex="58" AutoPostBack="true" OnTextChanged="txtConversionC_TextChanged"/></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtBagRateC" runat="server" CssClass="form-control" ClientIDMode="Static"  TabIndex="59" /></td>

                                                                                    <td class="text-center">
                                                                                        <asp:ImageButton ID="ImgbtnUpdate" runat="server" ImageUrl="~/images/SaveData.png" CommandName="Save" CommandArgument='0' Width="35" Height="20" TabIndex="60" />
                                                                                    </td>
                                                                                   
                                                                                </tr>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                        </table>
                                                                    </div>
                                                                   
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="procust" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                            <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                <div id="Div5" runat="server" clientidmode="Static" class="container">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col m12">
                                                                <div class="card small" style="height:350px;">
                                                                     <div class="card-title blue white-text row" style="padding:10px;">
                                                                        <div class="col m3">
                                                                            <p class="no-margin">Customer's Products</p>
                                                                        </div>
                                                                    </div>
                                                                    <div class="card-content row" style="height: 280px; width: 100%; padding: 0px; margin: 0px;overflow-y:auto;">
                                                                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <asp:Repeater ID="rptPro" runat="server" ClientIDMode="Static" OnItemDataBound="rptPro_ItemDataBound" OnItemCommand="rptPro_ItemCommand">
                                                                            <HeaderTemplate>
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th class="left-align" style="width: 20%;">Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                                            <a href="javascript:openProductInfo('add');">
                                                                                                <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" />
                                                                                            </a>
                                                                                        </th>
                                                                                        <th class="text-center" colspan="2">Action</th>
                                                                                    </tr>
                                                                                </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnpkIDProduct" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                                <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                                                <tr class="blueShed">
                                                                                    <td>
                                                                                        <asp:TextBox ID="edPro" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("ProductName") %>' TabIndex="48" ReadOnly="true"/></td>
                                                                                    <td class="text-center">
                                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="54" />
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:HiddenField ID="hdnpkIDProduct" runat="server" ClientIDMode="Static" />
                                                                                <asp:HiddenField ID="hdnProID" runat="server" ClientIDMode="Static" />
                                                                                <tr class="blueShed">
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtPro" runat="server" CssClass="form-control" ClientIDMode="Static"  TabIndex="55" onKeyup="bindDDLProductTo('#txtPro')" /></td>
                                                                                    <td class="text-center">
                                                                                        <asp:ImageButton ID="ImgbtnUpdate" runat="server" ImageUrl="~/images/SaveData.png" CommandName="Save" CommandArgument='0' Width="35" Height="20" TabIndex="60" />
                                                                                    </td>
                                                                                </tr>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                        </table>
                                                                    </div>
                                                                   
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                   
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom center-align yellow-text" id="divRestriction" runat="server" clientidmode="Static" style="display:none;margin-top:10px;margin-bottom:10px; font-size:20px;" />
                    <div id="btnPanel" runat="server" clientidmode="Static" class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;">
                        <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="43"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSaveEmail" type="button" runat="server" clientidmode="Static" Text="Save & Send Email" class="btn cyan right mr-1" onserverclick="btnSaveEmail_Click" TabIndex="41"><i class="material-icons left">email</i>Save - Send Email</button>
                        <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="42"><i class="material-icons left">save</i>Save</button>
                    </div>
                </div>
                </div>
                <%-- PopUp Modal Box to Open Window --%>
                <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
            <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
                <div id="myModal111">
                    <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                        <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                            <i class="material-icons prefix">ac_unit</i>
                            <span id="spnPageModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                            <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                        </h5>
                    </div>
                </div>
                <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
                <br />
            </asp:Panel>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>

                <script type="text/javascript">
                    function bindDDLProductTo(selector) {
                        if ($(selector).val().length >= 3) {
                            jQuery.ajax({
                                type: "POST",
                                url: "InquiryInfo.aspx/FilterProduct",
                                data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeAll\'}',
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
                                            //$(".autocomplete-suggestion").style.width = 'auto';
                                            return '<div class="autocomplete-suggestion" style"color:red;" data-langname="' + item.pkID + '" data-lang="' + item.ProductNameLong + '" data-val="' + search + '">' + item.ProductNameLong + '</div>';
                                        },
                                        onSelect: function (e, term, item) {
                                            //console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                            $(selector).val(item.html());
                                            $("#hdnProID").val(item.data('langname'));

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
                                $("#hdnProID").val('');
                            }
                        }
                        
                    }

                    //function bindDDlState(selector) {
                    //    var ddl = document.getElementById("drpCountry");
                    //    var StrCountryCode = ddl.options[ddl.selectedIndex].value;

                    //    if($(selector).val.length >= 1){
                    //    jQuery.ajax({
                    //        url: "Customers.aspx/FillState",
                    //        type: "POST",
                    //        datatype: "json",
                    //        data: '{pCountryCode:\'' + StrCountryCode + '\'}',
                    //        contentType: "application/json; charset=utf-8",
                    //        success: function(data){
                    //            //$(data).each(function(index,item){})
                    //            var options = '';
                    //            $("#drpState").empty();
                    //            $("#drpState").prop('disabled', false);
                    //            $("#drpCity").prop('disabled', true);
                    //            $("#drpState").append($('<option/>', { value: 0, text: '-- All State --' }));
                    //            if (data.d.length > 0) {
                    //                for (i = 0; i < data.d.length; i++) {
                    //                    options += '<option value="' + data.d[i].StateCode + '">' + data.d[i].StateName + '</option>';
                    //                }
                    //                $('#drpState').append(options);
                    //            }
                    //        }.bind(this),
                    //        error: function (r) { alert('Error : ' + r.responseText); },
                    //        failure: function (r) { alert('failure'); }
                    //    });
                    //    }
                    //}

                    //function bindDDlCity(selector) {
                    //    var ddl = document.getElementById("drpState");
                    //    var StrStateCode = ddl.options[ddl.selectedIndex].value;
                    //    if ($(selector).val.length >= 1) {
                    //        jQuery.ajax({
                    //            url: "Customers.aspx/FillCity",
                    //            type: "POST",
                    //            datatype: "json",
                    //            data: '{pStateCode:\'' + StrStateCode + '\'}',
                    //            contentType: "application/json; charset=utf-8",
                    //            success: function (data) {
                    //                //$(data).each(function(index,item){})
                    //                var options = '';
                    //                $("#drpCity").empty();
                    //                $("#drpCity").prop('disabled', false);
                    //                $("#drpCity").append($('<option/>', { value: 0, text: '-- All City --' }));
                    //                if (data.d.length > 0) {
                    //                    for (i = 0; i < data.d.length; i++) {
                    //                        options += '<option value="' + data.d[i].CityCode + '">' + data.d[i].CityName + '</option>';
                    //                    }
                    //                    $('#drpCity').append(options);
                    //                }
                    //            }.bind(this),
                    //            error: function (r) { alert('Error : ' + r.responseText); },
                    //            failure: function (r) { alert('failure'); }
                    //        });
                    //    }
                    //}

                    //$("#drpState").change(function () {
                    //    document.getElementById("hdnStateCode").value = $(this).val();
                    //})

                    //$("#drpCity").change(function () {
                    //    document.getElementById("hdnCityCode").value = $(this).val();
                    //    //alert($(this).val());
                    //})

                </script>
            </div>
            </div>
        </div>
    </form>
</body>
</html>
