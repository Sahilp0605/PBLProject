<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ProjectSheet.aspx.cs" Inherits="StarsProject.ProjectSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

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
        
        table.responsive-table th {
            background-color: #6868d6 !important;
            color: white !important;
        }

        table.responsive-table th, table.responsive-table td {
            border-bottom: 1px solid silver !important;
        }

        input[type=text], input[type=password], input[type=email], input[type=url], input[type=number], textarea :focus {
            border-color: #3C90BE;
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
            $('tabs').tabs();
            $('.timepicker').timepicker();
            $('textarea#txtMeetingNotes').characterCounter();
            // --------------------------------------------------------
            $('.btnTopRightCorner').on("click", function () {
                __doPostBack('txtCustomerName', '');
            });
            // --------------------------------------------------------
            $(function () {
                $(".tabs a").click(function () {
                    $("#hdnCurrTab").val($(this).attr("href").replace("#", ""));
                    retainTabPosition();
                });
            });

        });
        // --------------------------------------------------------
        function retainTabPosition() {
            var currTab = $("#hdnCurrTab").val();

            $("#lnkProducts").removeClass("active");
            $("#lnkAddress").removeClass("active");
            $("#lnkFollowUp").removeClass("active");

            $("#pnl_Products").css("display", "none");
            $("#pnl_Address").css("display", "none");
            $("#pnl_FollowUp").css("display", "none");

            if (currTab == 'pnl_Products') {
                $("#lnkProducts").addClass("active");
                $("#pnl_Products").css("display", "block");
            }
            else if (currTab == 'pnl_Address') {
                $("#lnkAddress").addClass("active");
                $("#pnl_Address").css("display", "block");
            }
            else if (currTab == 'pnl_FollowUp') {
                $("#lnkFollowUp").addClass("active");
                $("#pnl_FollowUp").css("display", "block");
            }
        }
        /*---------------------------------------------------------*/
        $("#drpInquiryStatus").on("focus", function () { $("#drpInquiryStatus").click(); });
        $("#drpInquirySource").on("focus", function () { $("#drpInquirySource").click(); });
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
        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        /*---------------------------------------------------------*/
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openDealerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnDealerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        /*Add Product view on Product Name Vikram Rajput 15-07-2020*/
        function openProductInfo(cat) {
            var keyid = "0";
            var pageUrl = "Products.aspx?mode=view&id=0";
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

</head>
<body class="loginpage" style="background-color: transparent;">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <%-- Bootstrap INQUIRY INFO Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnOrgCodeEmp" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustEmailAddress" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnDesignation" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnDealerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnBlockCustomer" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCurrTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Products" />
                <asp:HiddenField ID="hdnStatusID" runat="server" ClientIDMode="Static" />

                <div class="row">
                    <div class="col m6" style="border-right: 3px ridge silver;">
                        <div class="row">
                            <div class="input-field col" style="width:180px">
                                <label class="active font " for="txtProjectNo">Project #</label>
                                <asp:TextBox ID="txtProjectNo" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" />
                            </div>
                            <div class="input-field col m2.5" style="width:180px">
                                <label class="active" for="txtProjectDate">Project Date <span class="materialize-red-text font-weight-800">*</span></label>
                                <asp:TextBox ID="txtProjectDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" placeholder="" TextMode="Date"/>
                            </div>
                            <div class="input-field col" style="width:280px">
                                <label class="active" for="txtCustomerName">
                                    Customer Name&nbsp; <span class="materialize-red-text font-weight-800">*</span>
                                    <a href="javascript:openCustomerInfo('view');">
                                        <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;"/>
                                    </a>
                                    <a href="javascript:openCustomerInfo('add');">
                                        <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                    </a>
                                </label>
                                <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" placeholder="Minimum 3 chars to activate search" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" TabIndex="3" MaxLength="50" AutoPostBack="true" OnTextChanged="txtCustomerName_TextChanged" />
                            </div>
                            <div class="input-field col" style="width:150px">
                                <label class="active font " for="txtSiteNo">Site No #</label>
                                <asp:TextBox ID="txtSiteNo" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1" />
                            </div>
                        </div>
                        <div class="col m12 pr-0">
                            <div class="card small" style="height:auto;">
                                <div class="card-title blue white-text" style="padding:10px;">
                                    <p class="no-margin">Site Address</p>
                                </div>
                                <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                    <div class="row" style="width:100%">
                                        <div class="input-field col m12">
                                            <asp:TextBox ID="txtAddress" runat="server" class="form-control" Placeholder="" TabIndex="25" MaxLength="100"  ClientIDMode="Static" TextMode="MultiLine"/>
                                            <label class="active" for="txtAddress1">Address </label>
                                        </div>
                                    </div>
                                    <div class="row" style="width:100%">
                                        <div class="input-field col m4">
                                            <asp:TextBox ID="txtArea" runat="server" TabIndex="26" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"  />
                                            <label class="active" for="txtArea">Area</label>
                                        </div>
                                        <div class="input-field col m4">
                                            <asp:DropDownList ID="drpCountry" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="27" AutoPostBack="true"  OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"/>
                                            <label class="active" for="drpCountry1">Country 
                                                <a href="javascript:openRegion('add');">
                                                    <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add Country" title="Add Country" />
                                                </a>
                                            </label>
                                        </div>
                                        <div class="input-field col m4">
                                            <asp:DropDownList ID="drpState" class="select2-theme browser-default"  runat="server"  ClientIDMode="Static" TabIndex="28" AutoPostBack="true"  OnSelectedIndexChanged="drpState_SelectedIndexChanged"/>
                                            <label class="active" for="drpState1">State 
                                                <a href="javascript:openRegion('add');">
                                                    <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add State" title="Add State" />
                                                </a>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="row" style="width:100%">
                                        <div class="input-field col m6">
                                            <asp:DropDownList ID="drpCity" class="select2-theme browser-default" runat="server"  ClientIDMode="Static" TabIndex="29" />
                                            <label class="active" for="drpCity1">City 
                                                <a href="javascript:openRegion('add');">
                                                    <img src="images/RegionAdd.jpg" width="20" height="20" alt="Add City" title="Add City" />
                                                </a>
                                            </label>
                                        </div>
                                        <div class="input-field col m6">
                                            <asp:TextBox ID="txtPincode" runat="server" TabIndex="30" class="form-control"  ClientIDMode="Static"  Placeholder="" MaxLength="30"/>
                                            <label class="active" for="txtPincode">Pincode</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m12">
                                <label class="active" for="txtRemarks">Remarks</label>
                                <asp:TextBox ID="txtRemarks" runat="server" placeholder="" class="materialize-textarea" ClientIDMode="Static" TabIndex="8" TextMode="MultiLine" data-length="500" />
                            </div>
                        </div>
                        

                    </div>
                    <div class="row">
                        <div id="divSelectAssembly" runat="server" clientidmode="Static" class="row">
                            <div class="col m6" style="border-right: 3px ridge silver; border-top: 3px ridge silver;">
                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <asp:Panel runat="server" ID="Panel1">
                                            <h5 class="card-title padding-1 m-0 ml-1 mt-2 left-align gradient-45deg-light-blue-indigo p-0" style="line-height:30px;">&nbsp;Assembly List</h5>
                                            <div class="input-field col m12" style="min-height:100px; overflow-y:auto;">
                                                <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <asp:Repeater ID="rptAssembly" runat="server" ClientIDMode="Static" OnItemCommand="rptAssembly_ItemCommand" OnItemDataBound="rptAssembly_ItemDataBound">
                                                        <HeaderTemplate>
                                                            <thead>
                                                                <tr class="accent-1">
                                                                    <th class="text-left">Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                        <a href="javascript:openProductInfo('add');">
                                                                            <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" tabindex="13" />
                                                                        </a>
                                                                    </th>
                                                                    <th class="text-center center-align width-10">Quantity <span class="materialize-red-text font-weight-800">*</span></th>
                                                                    <th class="text-center center-align width-20">Unit <span class="materialize-red-text font-weight-800">*</span></th>
                                                                    <th class="text-center center-align width-20">Product Make <span class="materialize-red-text font-weight-800">*</span></th>
                                                                    <th class="text-center center-align width-10">Action</th>
                                                                </tr>
                                                            </thead>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnBrandID" runat="server" ClientIDMode="Static" Value='<%# Eval("BrandID")%>' />
                                                            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID")%>' />
                                                            <tr class="blueShed">
                                                                <td class="text-left"><%# Eval("ProductName") %></td>
                                                                <td class="center"><%# Eval("Quantity")%></td>
                                                                <td class="center"><%# Eval("Unit")%></td>
                                                                <td id="drpBrand" class="center">
                                                                    <asp:DropDownList CssClass="select2-theme browser-default" ID="drpBrandName" runat="server" ClientIDMode="Static" >  </asp:DropDownList>
                                                                </td>
                                                                <td class="center">
                                                                    <asp:ImageButton ID="imgBtnDelDetail" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    <FooterTemplate>
                                                    <tr class="blueShed">
                                                        <td>
                                                            <asp:HiddenField ID="hdnProductID2" runat="server" ClientIDMode="Static" />
                                                            <asp:TextBox ID="txtProductName2" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName2')" AutoPostBack="true" TabIndex="9" placeholder="Search Product" />
                                                        </td>
                                                        <td class="text-center center-align width-10">
                                                            <asp:TextBox ID="txtQuantity2" runat="server" ClientIDMode="Static" Width="80" TabIndex="10" />
                                                        </td>
                                                        <td class="text-center center-align width-20">
                                                            <asp:TextBox ID="txtUnit2" runat="server" ClientIDMode="Static" Width="120" TabIndex="12" />
                                                        </td>
                                                        <td class="center-align">
                                                            <asp:DropDownList CssClass="select2-theme browser-default" ID="drpBrandName" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                        <td class="text-center center-align width-10">
                                                            <asp:ImageButton ID="imgBtnSave" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="20" Height="20" TabIndex="13" />
                                                        </td>
                                                    </tr>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                
                                    <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col m6" style="border-right: 3px ridge silver; border-top: 3px ridge silver;">
                        <asp:UpdatePanel ID="upRepeater" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="pnlDetail">
                                    <div class="row" style="margin-top:10px">
                                        <h5 class="card-title padding-1 m-0 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Product Detail</h5>
                                        <table id="tblInqProductGroup" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="99%">
                                            <asp:Repeater ID="rptProject" runat="server" ClientIDMode="Static" OnItemCommand="rptProject_ItemCommand" OnItemDataBound="rptProject_ItemDataBound">
                                                <HeaderTemplate>
                                                    <thead>
                                                        <tr>
                                                            <th class="text-left">Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                <a href="javascript:openProductInfo('add');">
                                                                    <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" tabindex="13" />
                                                                </a>
                                                            </th>
                                                            <th class="text-center center-align width-10">Unit <span class="materialize-red-text font-weight-800">*</span></th>
                                                            <th class="text-center center-align width-20">System Capacity <span class="materialize-red-text font-weight-800">*</span></th>
                                                            <th class="text-center center-align width-20">Panal Wattage <span class="materialize-red-text font-weight-800">*</span></th>
                                                            <th class="text-center center-align width-10">Action</th>
                                                        </tr>
                                                    </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr class="blueShed">
                                                        <asp:HiddenField ID="hdnProductID_Grid" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                        <td class="text-left"><%# Eval("ProductNameLong") %></td>
                                                        <td class="text-center center-align width-10"> 
                                                            <asp:TextBox id="tdUnit" runat="server" ClientIDMode="Static" Text = '<%# Eval("Unit") %>' Style="width: 70px;"></asp:TextBox>
                                                        </td>
                                                        <td class="text-center center-align width-20">
                                                            <asp:TextBox id="tdSysCapacity" runat="server" ClientIDMode="Static" Text = '<%# Eval("SysCapacity") %>' width="120"></asp:TextBox>
                                                        </td>
                                                        <td class="text-center center-align width-20">
                                                            <asp:TextBox id="tdPanalWattage" runat="server" ClientIDMode="Static" Text = '<%# Eval("PanalWattage") %>' width="120"></asp:TextBox>
                                                        </td>
                                                        <td class="text-center center-align width-10">
                                                            <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <tr style="padding: 5px;">
                                                        <td>
                                                            <asp:HiddenField ID="hdnProductIDNew" runat="server" ClientIDMode="Static" />
                                                            <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductToNew('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="9" placeholder="Search Product" />
                                                        </td>
                                                        <td class="text-center center-align width-10">
                                                            <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="80" TabIndex="10" />
                                                        </td>
                                                        <td class="text-center center-align width-20">
                                                            <asp:TextBox ID="txtSysCapacity" runat="server" ClientIDMode="Static" Width="120" TabIndex="15" />
                                                        </td>
                                                        <td class="text-center center-align width-20">
                                                            <asp:TextBox ID="txtPanalWattage" runat="server" ClientIDMode="Static" Width="120" TabIndex="12" />
                                                        </td>
                                                        <td class="text-center center-align width-10">
                                                            <asp:ImageButton ID="imgBtnSave" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="20" Height="20" TabIndex="13" />
                                                        </td>
                                                    </tr>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    <//div>
                </div>
            </div>
        </div>
            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                <div class="col m12">
                    <button id="btnReset" type="button" runat="server" clientidmode="Static" class="btn orange left" onserverclick="btnReset_ServerClick" tabindex="19"><i class="material-icons left">backspaceave</button>
                    <button id="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_ServerClick" TabIndex="48"><i class="material-icons left">save</i>Save</button>
                </div>
            </div>

            <%--**************************** Customer Popup ***********************************--%>
            <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
            <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
                <div id="myModal">
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

            <%-- *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-* --%>
            <script type="text/javascript">
                $("input").keypress(function (event) {
                    if (event.which == 13) {
                        event.preventDefault();
                        //$("form").submit();
                    }
                });

                $("#txtProductName").focusin(function () {
                    window.scrollBy(0, 150);
                });

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
                                        return '<div class="autocomplete-suggestion" data-langname1="' + item.CustomerSourceName + '" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnCustomerID").val(item.data('langname'));
                                    }
                                });

                            },
                            error: function (r) { alert('Error. : ' + r.responseText); },
                            failure: function (r) { alert('failure.'); }
                        });
                        return false;
                    }
                    else {
                        if ($(selector).val().length == 0) {
                            $("#hdnCustomerID").val('');
                        }
                    }
                }


                function bindDDLToDealer(selector) {
                    if ($(selector).val().length >= 1) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterCustomerByType",
                            data: '{pCustName:\'' + $(selector).val() + '\', pCustomerType:\'Dealer\'}',
                            //url: "InquiryInfo.aspx/FilterCustomerByModule",
                            //data: '{pCustName:\'' + $(selector).val() + '\', pSearchModule:\'CustomerSearchDealer\'}',
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
                                        $("#hdnDealerID").val(item.data('langname'));
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
                            $("#hdnDealerID").val('');
                        }
                    }
                }


                function bindDDLProductTo(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterProduct",
                            data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\'}',
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
                                        return '<div class="autocomplete-suggestion" data-langname="' + item.pkID + '" data-lang="' + item.ProductNameLong + '" data-val="' + search + '">' + item.ProductNameLong + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        //console.log('Item "' + item.data('langname') + ' (' + item.data('langstk') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnProductID2").val(item.data('langname'));
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
                            $("#hdnProductID2").val('');
                        }
                    }
                }

                function bindDDLProductToNew(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterProduct",
                            data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\'}',
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
                                        return '<div class="autocomplete-suggestion" data-langname="' + item.pkID + '" data-lang="' + item.ProductNameLong + '" data-val="' + search + '">' + item.ProductNameLong + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        $(selector).val(item.html());
                                        $("#hdnProductIDNew").val(item.data('langname'));
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
                            $("#hdnProductIDNew").val('');
                        }
                    }
                }

                function clearProductField() {
                    $("#hdnProductID").val('');
                    $("#txtProductName").val('');
                    $("#txtUnitPrice").val('');
                    $("#txtQuantity").val('');
                    $("#lblAmount").val('');
                    $("#txtProductName").focus();
                }

                $('#btnReset').focusout(function () {
                    $('#txtInquiryNo').focus();
                    //alert('reset button focusout invoke');
                });

                $('#btnCloseModel').focusout(function () {
                    closeSetTime();
                });
            </script>
        </div>
    </form>

</body>
</html>
