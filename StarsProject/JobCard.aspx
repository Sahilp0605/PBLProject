<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobCard.aspx.cs" Inherits="StarsProject.JobCard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png" />
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <style type="text/css">
        /*Preview-Add EmployeeList on Employee Name Dropdown by Vikram Rajput 14-07-2020*/

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
            $('.tabs').tabs();

            jQuery(".jqte-test").jqte({
                blur: function () { }
            });
            // settings of status
            var jqteStatus = true;
        });
        /*---------------------------------------------------------*/
        function pageLoad(sender, args) {
            retainTabPosition();
            //IdentityType();
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

        $(function () {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "profile";
            $("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "profile";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab").val(currTab);
            });
        });

        function retainTabPosition() {

            $('#lnkprofile').removeClass('active');
            $('#lnkcompany').removeClass('active');
            $('#lnkdocuments').removeClass('active');

            $('#profile').css("display", "none");
            $('#company').css("display", "none");
            $('#documents').css("display", "none");

            if ($("#hdnCurrentTab").val() == 'profile') {
                $('#lnkprofile').addClass('active');
                $('#profile').css("display", "block");
            }

            if ($("#hdnCurrentTab").val() == 'company') {
                $('#lnkcompany').addClass('active');
                $('#company').css("display", "block");
            }

            else if ($("#hdnCurrentTab").val() == 'documents') {
                $('#lnkdocuments').addClass('active');
                $('#documents').css("display", "block");
            }
        }
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpReportTo').val() : "0";
            if (keyid != "0" && keyid != null)
            {
                var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
                $('#ifrModule').attr('src', pageUrl);
                $find("mpe").show();
            }
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

        function showFileExtError(xMode) {
            if (xMode == 'image')
                alert("Only BMP, JPEG, GIF and PNG file is valid for Attachment !");
            if (xMode == 'pdf')
                alert("Only PDF file attachment is valid for Upload !");
        }

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
</head>
<body class="loginpage" style="overflow-x: hidden;">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="profile" />
            <div class="widgetbox">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <div id="myModal" style="display: block; width: 98%; overflow-x: hidden;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                               <div class="input-field col m2">
                                  <label class="active" for="txtJobCardNo">Job Card No<span class="materialize-red-text font-weight-800">*</span></label>
                                   <asp:TextBox ID="txtJobCardNo" Enabled="false" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1"   />
                               </div>
                               <div class="input-field col m2">
                                  <label class="active" for="txtDate">Date <span class="materialize-red-text font-weight-800">*</span></label>
                                   <asp:TextBox ID="txtDate" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1" TextMode="Date" Width="150px"  />
                               </div>
                               <div class="input-field col m3">
                                    <label class="active" for="txtCustomerName">
                                        Customer Name <small class="text-muted font-italic" style="font-size:.8rem"></small> <span class="materialize-red-text font-weight-800">*</span>
                                        <a href="javascript:openCustomerInfo('view');">
                                            <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block; padding: 6px 0px 0px 0px;" tabindex="3" />
                                        </a>
                                        <a href="javascript:openCustomerInfo('add');">
                                            <img src="images/addCustomer.png" width="30" height="20" style="padding: 6px 0px 0px 0px;" alt="Add New Customer" title="Add New Customer" />
                                        </a>
                                    </label>
                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" AutoPostBack="true" OnTextChanged="txtCustomerName_TextChanged" TabIndex="3" placeholder="Min.3 Char.To Search" />
                                </div>
                               <div class="input-field col m2">
                                  <label class="active" for="txtDateIn">Date In <span class="materialize-red-text font-weight-800">*</span></label>
                                   <asp:TextBox ID="txtDateIn" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1" TextMode="Date" Width="150px"  />
                               </div>
                                 <div class="input-field col m2">
                                     <label class="active" for="txtDateReturn">Date Return <span class="materialize-red-text font-weight-800">*</span></label>
                                      <asp:TextBox ID="txtDateReturn" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1" TextMode="Date" Width="150px"  />
                                 </div>
                                    
                                </div>
                                    <div class="row" style="max-width: 100%; margin-top:1rem">
                                        <div class="input-field col m2">
                                            <label class="active" for="drpLocation">Location</label>
                                            <asp:DropDownList ID="drpLocation" runat="server" placeholder="" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="2" Width="150px" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtCollectedFrom">Collected From </label>
                                            <asp:TextBox ID="txtCollectedFrom" runat="server" ClientIDMode="Static" MaxLength="200" class="form-control" placeholder=" " TabIndex="1" />
                                        </div>
                              
                                        <div class="input-field col m3">
                                            <label class="active" for="txtPaintCode">Paint Code </label>
                                            <asp:TextBox ID="txtPaintCode" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="6" />
                                        </div>
                                         
                                        <div class="input-field col m2">
                                            <label class="active" for="drpDiamondCut">Type of Job <span class="materialize-red-text font-weight-800"></span></label>
                                            <%--<asp:TextBox ID="txtDiamondCut" runat="server" ClientIDMode="Static" MaxLength="50" class="form-control" placeholder=" " TabIndex="4" />--%>
                                            <asp:DropDownList ID="drpDiamondCut" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="14" >
                                                <asp:ListItem Text="--Select--" Value="" Selected="True" />    
                                                <asp:ListItem Text="Diamond-Cut" Value="Diamond-Cut" />
                                                <asp:ListItem Text="Painted" Value="Painted" />
                                                <asp:ListItem Text="Straightening" Value="Straightening" />
                                                <asp:ListItem Text="Tyre Service" Value="Tyre Service" />
                                                <asp:ListItem Text="Tyre Supply" Value="Tyre Supply" />
                                                <asp:ListItem Text="Wheel Balancing" Value="Wheel Balancing" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtWheelMake">Wheel Make <span class="materialize-red-text font-weight-800"></span></label>
                                            <asp:TextBox ID="txtWheelMake" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="15" TabIndex="5" />
                                        </div>
                                    </div>     
                                 <div class="row" style="max-width: 100%;">
                                        <div class="input-field col m2">
                                            <label class="active" for="txtWheelNo">Wheel No <span class="materialize-red-text font-weight-800"></span></label>
                                            <asp:TextBox ID="txtWheelNo" runat="server" ClientIDMode="Static" MaxLength="50" class="form-control" placeholder=" " TabIndex="4" onkeypress="return isNumber(event);"/>
                                        </div>
                                        <div id="divLoad1" runat="server" clientidmode="Static" class="input-field col m3 hide">
                                            <label class="active" for="drpInvoiceNo">Invoice No</label>
                                            <asp:DropDownList ID="drpInvoiceNo" runat="server" placeholder="" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="2" />
                                        </div>
                                        
                                       <%-- <div class="input-field col m3">
                                            <label class="active" for="txtInvoiceNo">Invoice No <span class="materialize-red-text font-weight-800"></span></label>
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="15" TabIndex="5" />
                                        </div>--%>
                                     <div class="input-field col m2">
                                            <label class="active" for="txtDeliveryNoteNo">Delivery Note No </label>
                                            <asp:TextBox ID="txtDeliveryNoteNo" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="6" />
                                        </div>
                                        <%--<div class="input-field col m3">
                                            <label class="active" for="txtTyre">Tyre</label>
                                            <asp:TextBox ID="txtTyre" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="6" />
                                        </div>--%>
                                         <div class="input-field col m2">
                                            <label class="active" for="drpTyre">Tyre </label>
                                            <asp:DropDownList ID="drpTyre" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="14" >
                                                <asp:ListItem Text="--Select--" Value="" Selected="True" />    
                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                <asp:ListItem Text="No" Value="No" />
                                            </asp:DropDownList>
                                        </div>
                                        
                                        <div class="input-field col m2">
                                            <label class="active" for="drpCap">Cap </label>
                                            <asp:DropDownList ID="drpCap" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="14" >
                                                <asp:ListItem Text="--Select--" Value="" Selected="True" />    
                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                <asp:ListItem Text="No" Value="No" />
                                            </asp:DropDownList>
                                        </div>
                                        
                                        <div class="input-field col m2">
                                            <label class="active" for="drpSensor">Sensor </label>
                                            <asp:DropDownList ID="drpSensor" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="14" >
                                                <asp:ListItem Text="--Select--" Value="" Selected="True" />    
                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                <asp:ListItem Text="No" Value="No" />
                                            </asp:DropDownList>
                                        </div>
                                        
                                        <div class="input-field col m2">
                                            <label class="active" for="drpSensorValue">Sensor Value </label>
                                            <asp:DropDownList ID="drpSensorValue" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="14" >
                                                <asp:ListItem Text="--Select--" Value="" Selected="True" />    
                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                <asp:ListItem Text="No" Value="No" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row" style="max-width: 100%;">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtPartNumber">Part Number <span class="materialize-red-text font-weight-800"></span></label>
                                            <asp:TextBox ID="txtPartNumber" runat="server" ClientIDMode="Static" MaxLength="50" class="form-control" placeholder=" " TabIndex="4" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtStraightenedMeasurement">Straightened Measurement </label>
                                            <asp:TextBox ID="txtStraightenedMeasurement" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="6" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtClaimNo">Claim No <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtClaimNo" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="6" />
                                        </div> 
                                         <div class="input-field col m3">
                                            <label class="active" for="txtRegNo">Reg No <span class="materialize-red-text font-weight-800"></span></label>
                                            <asp:TextBox ID="txtRegNo" runat="server" ClientIDMode="Static" MaxLength="50" class="form-control" placeholder=" " TabIndex="4" />
                                        </div>
                                    </div>

                                    <div class="row" style="max-width: 100%;">
                                        <div class="input-field col m6">
                                            <label class="active" for="txtRemarks">Remarks </label>
                                            <asp:TextBox ID="txtRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" MaxLength="50" TextMode="MultiLine" placeholder="" />
                                        </div> 
                                        <div class="input-field col m6">
                                              <label class="active" for="txtComment">Comment </label>
                                            <asp:TextBox ID="txtComment" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" MaxLength="50" TextMode="MultiLine" placeholder="" />
                                        </div>
                                    </div>
                                    <div class="row" style="max-width: 100%;">
                                        <div class="input-field col m2">
                                            <label class="active" for="txtBuyerRef">Buyer's Ref</label>
                                            <asp:TextBox ID="txtBuyerRef" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="500" TabIndex="6" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtChassisNo">Chassis No </label>
                                            <asp:TextBox ID="txtChassisNo" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="500" TabIndex="5" />
                                        </div>  
                                        <div class="input-field col m2">
                                            <label class="active" for="txtStartDate">Start Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" class="form-control" Width="150" TextMode="Date" placeholder=" " TabIndex="2" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtQualityCheck">Quality Check</label>
                                            <asp:TextBox ID="txtQualityCheck" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="6" />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtDeliveryDate">Delivery Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtDeliveryDate" runat="server" ClientIDMode="Static" onkeypress="return false;" onpaste="return false" class="form-control" TextMode="Date" placeholder=" " TabIndex="2" Width="150" />
                                        </div>                     
                                    </div>
                                    <div class="row" style="max-width: 100%;">
                                        <div class="input-field col m3 hide">
                                            <label class="active" for="txtEstimatePrice">Estimate Price</label>
                                            <asp:TextBox ID="txtEstimatePrice" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="6" />
                                        </div>
                                     </div>
                            <div class="row">
                                <div class="col s12">
                                    <ul id="Ul1" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color: antiquewhite;">
                                        <li class="tab col m2 p-0"><a id="lnkProducts" class="left active" href="#pnl_Products">Product Detail</a></li>
                                    </ul>
                                </div>
                                <div id="pnl_Products" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                    <asp:Panel runat="server" ID="pnlDetail">
                                        <div class="row">
                                            <table id="tblJobCardDetail" class="table responsive-table striped width-100">
                                                <asp:Repeater ID="rptJobCardDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptJobCardDetail_ItemCommand" OnItemDataBound="rptJobCardDetail_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <thead>
                                                            <tr>
                                                                <th class="left-align" style="width: 25%;">Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                    <a href="javascript:openProductInfo('add');">
                                                                        <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" tabindex="13" />
                                                                    </a>
                                                                </th>
                                                                <th class="center-align" style="width: 15%;">Unit</th>
                                                                <th class="right-align" style="width: 15%;">Quantity <span class="materialize-red-text font-weight-800">*</span></th>
                                                                <th class="right-align" style="width: 15%;">Unit Rate<span class="materialize-red-text font-weight-800">*</span></th>
                                                                <th class="right-align" style="width: 15%;">Amount</th>
                                                                <th class="center-align" style="width: 15%;">Delete</th>
                                                            </tr>
                                                        </thead>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="left-align" style="width: 25%;">
                                                                <%# Eval("ProductName") %>
                                                                <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                                <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                            </td>
                                                            <td class="text-center" style="width: 15%;">
                                                                <asp:TextBox ID="edUnit" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Unit") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 100px; text-align: left;" TabIndex="12" />
                                                            </td>
                                                            <td class="right-align" style="width: 15%;">
                                                                <asp:TextBox ID="edQuantity" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 150px; text-align: right;" TabIndex="15" />
                                                            </td>
                                                            <td class="right-align" style="width: 15%;">
                                                                <asp:TextBox ID="edUnitRate" runat="server" ClientIDMode="Static" Text='<%# Eval("UnitRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 150px; text-align: right;" TabIndex="16" />
                                                            </td>
                                                            <td class="right-align" style="width: 15%;">
                                                                <asp:TextBox ID="edAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' Style="width: 150px; text-align: right;" Enabled="false" TabIndex="21" />
                                                            </td>
                                                            <td class="center-align" style="width: 15%;">
                                                                <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="27" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr style="background-color: #dde8f3; border-bottom: 5px solid navy;">
                                                            <td class="left-align" style="width: 25%;">
                                                                <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnTaxType" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnOrgUnitRate" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnBox_SQFT" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnBox_SQMT" runat="server" ClientIDMode="Static" />
                                                                <asp:HiddenField ID="hdnUnitQuantity" runat="server" ClientIDMode="Static" />
                                                                <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" Width="100%" TabIndex="28" />
                                                            </td>
                                                            <td class="center-align" style="width: 15%;">
                                                                <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="100" TabIndex="29" AutoPostBack="true" OnTextChanged="txtUnit_TextChanged" />
                                                            </td>
                                                            <td class="right-align" style="width: 15%;">
                                                                <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" Width="150" TabIndex="31" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged" />
                                                            </td>
                                                            <td class="right-align" style="width: 15%;">
                                                                <asp:TextBox ID="txtUnitRate" runat="server" ClientIDMode="Static" Width="150" TabIndex="32" AutoPostBack="true" OnTextChanged="txtUnitRate_TextChanged" />
                                                            </td>
                                                            <td class="right-align" style="width: 15%;">
                                                                <asp:TextBox ID="txtAmount" runat="server" ClientIDMode="Static" Width="150" TabIndex="37" Enabled="false" />
                                                            </td>
                                                            <td class="center-align" colspan="2" style="width: 15%;">
                                                                <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="110" Height="30" TabIndex="42" />
                                                            </td>
                                                        </tr>
                                                        
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                                </div>
                           </div>
                        </div>
                    </div>
                     
                        <div class="row">
                            <div class="input-field col m12">
                                <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static" style="color:"></div>
                            </div>
                        <%--<div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 5px;">
                            <button id="Button3" type="button" runat="server" clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="12"><i class="material-icons left">backspace</i>Clear & Add New</button>
                            <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn cyan right mr-1" OnClientClick="return validateForm();" OnClick="btnSave_Click" TabIndex="11" />
                            <asp:Button ID="btnReset" Visible="false" runat="server" ClientIDMode="Static" Text="Reset" class="btn cyan right mr-1" OnClick="btnReset_Click" TabIndex="12" />
                        </div>--%>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                        <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="36"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="37"><i class="material-icons left">save</i>Save</button>
                    </div>
                </div>
            </div>
            <%--</div>--%>

        <%--**************************** Employee Popup ***********************************--%>
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
            <div id="myModal123">
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
    </form>

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
                                return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                            },
                            onSelect: function (e, term, item) {
                                console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                $(selector).val(item.html());
                                $("#hdnCustomerID").val(item.data('langname'));
                                $("#txtStartDate").focus();
                            }
                        });
                    },
                    error: function (r) { alert('Error : ' + r.responseText); },
                    failure: function (r) { alert('failure'); }
                });
                return false;
            }
        }

        function bindDDLProductTo(selector) {
            //var X = $("#hdnCustWisePro").val();
            //var Y = 0;
            //if (X.toLowerCase() == 'yes' && $("#hdnCustomerID").val() != '')
            //    Y = $("#hdnCustomerID").val();
            if ($(selector).val().length >= 3) {
                jQuery.ajax({
                    type: "POST",
                    url: "InquiryInfo.aspx/FilterProduct",
                    //data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\', CustomerID:' + Y + '}',
                    data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeSalesOrder\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
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
                                return '<div class="autocomplete-suggestion" data-langname="' + item.pkID + '" data-lang="' + item.ProductNameLong + '" data-val="' + search + '">' + item.ProductNameLongStk + '</div>';
                            },
                            onSelect: function (e, term, item) {
                                //console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                $(selector).val(item.data('lang'));
                                $("#hdnProductID").val(item.data('langname'));
                                $("#txtUnit").focus();
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
                    $("#hdnProductID").val('');
                }
            }
        }
    </script>
</body>
</html>
