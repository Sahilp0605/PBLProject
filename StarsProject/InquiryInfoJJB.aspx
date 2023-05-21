<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="InquiryInfoJJB.aspx.cs" Inherits="StarsProject.InquiryInfoJJB" %>

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
            $('.timepicker').timepicker();
            $('textarea#txtMeetingNotes').characterCounter();
        });

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

        /*Add Product view on Product Name Vikram Rajput 15-07-2020*/
        function openProductInfo(cat) {
            var keyid = "0";
            var pageUrl = "Products.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        function checkedEventCall() {
            debugger;
            //var index = document.getElementById('#drpInquiryStatus');
            //var item = index.options[index.selectedIndex].innerHTML;
            //var item = $("#drpInquiryStatus").text();
            //var item = document.getElementById("drpInquiryStatus").options[document.getElementById('drpInquiryStatus').selectedIndex].text;
            var item = $("#drpInquiryStatus :selected").text();
                if (item == 'Assign To Partner') {
                    $("#divAssignTo").show();
                }
                else {
                    $("#divAssignTo").hide();
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
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnOrgCodeEmp" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustEmailAddress" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnDesignation" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnBlockCustomer" runat="server" ClientIDMode="Static" />
                <div id="myModal11" style="display: block; width: 99%;">
                    <div class="modal-content border-0">
                        <div class="modal-body col m12">
                            <div class="row">
                                <div class="col m12" style="border-right: 3px ridge silver;">
                                    <div class="row">
                                        <div class="input-field col m2">
                                            <label class="active" for="txtInquiryNo">Lead #</label>
                                            <asp:TextBox ID="txtInquiryNo" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtInquiryDate">Lead Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtInquiryDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" placeholder="" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtCustomerName">
                                                Customer Name&nbsp; <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" />
                                                </a>
                                                <a href="javascript:openCustomerInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                </a>
                                            </label>
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" placeholder="Minimum 3 chars to activate search" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" TabIndex="3" MaxLength="50" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="drpPriority">Lead Priority</label>
                                            <asp:DropDownList ID="drpPriority" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="6">
                                                <asp:ListItem Text="Low" Value="Low" />
                                                <asp:ListItem Text="Medium" Value="Medium" />
                                                <asp:ListItem Text="High" Value="High" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="drpInquiryStatus">Lead Status</label>
                                            <asp:DropDownList ID="drpInquiryStatus" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="drpInquiryStatus_SelectedIndexChanged" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="drpInquirySource">Lead Source <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpInquirySource" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="5" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtReferenceName">Reference Name</label>
                                            <asp:TextBox ID="txtReferenceName" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="7" MaxLength="50" />
                                        </div>
                                        <div class="input-field col m3 hide">
                                            <label class="active" for="txtRefNo">Reference No</label>
                                            <asp:TextBox ID="txtRefNo" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="7" MaxLength="50" />
                                        </div>
                                        <div id="divAssignTo" visible="false" class="input-field col m3" ClientIDMode="Static" runat="server" >
                                            <label id="lblAssignTo" class="active" for="drpAssignTo">Assign To <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpAssignTo" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="5" >
                                                <asp:ListItem Text="--Select--" Value=" " />
                                                <asp:ListItem Text="TBS Ahmedbad" Value="TBS Ahmedbad" />
                                                <asp:ListItem Text="TBS Pune" Value="TBS Pune" />
                                                <asp:ListItem Text="TBS Ludhiana" Value="TBS Ludhiana" />
                                                <asp:ListItem Text="Team REB" Value="Team REB" />
                                                <asp:ListItem Text="TBS Nashik" Value="TBS Nashik" />
                                                <asp:ListItem Text="TBS Rajkot" Value="TBS Rajkot" />
                                                <asp:ListItem Text="TBS Hyderabad" Value="TBS Hyderabad" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    
                                   
                                    <div id="divNextFollowup" runat="server" clientidmode="Static" style="background-color: aliceblue; border: 1px solid silver; border-radius: 8px; padding: 10px 0px 0px 0px;">
                                        <div class="row">
                                            <div class="input-field col m6" id="divFollowupNotes" runat="server" clientidmode="Static">
                                                <label class="active" for="txtFollowupNotes">Followup Notes</label>
                                                <asp:TextBox ID="txtFollowupNotes" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="21" TextMode="MultiLine" Rows="3" MaxLength="500" />
                                            </div>
                                            <div class="input-field col m6">
                                                <div class="row">
                                                    <label class="active" for="txtMeetingNotes">Description<span class="materialize-red-text font-weight-800">*</span></label>
                                                    <asp:TextBox ID="txtMeetingNotes" runat="server" placeholder="" class="materialize-textarea" ClientIDMode="Static" TabIndex="8" TextMode="MultiLine" Rows="3" data-length="500"/>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="input-field col m3">
                                                <asp:TextBox ID="txtFollowupDate" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="22" onkeypress="return false;" TextMode="Date" />
                                                <label class="active" for="txtFollowupDate">Next FollowUp <span class="materialize-red-text font-weight-800">*</span></label>
                                            </div>
                                            <div class="input-field col m3">
                                                <asp:TextBox ID="txtPreferredTime" runat="server" placeholder="" class="timepicker" ClientIDMode="Static" TabIndex="23" Style="width: 150px;" ReadOnly="false" onkeypress="return false;" onfocus="popupSetTime(this);" />&nbsp;&nbsp;
                                                    <label class="active" for="txtPreferredTime">Preferred Time</label>
                                            </div>
                                        </div>
                                    
                                        
                                    </div>
                                    <div class="row mt-2" id="divClosureReason" runat="server" clientidmode="static" visible="false">
                                        <div class="input-field col m6">
                                            <asp:DropDownList ID="drpClosureReason" runat="server" ClientIDMode="Static" class="select2-theme browser-default" EnableViewState="true" TabIndex="24" />
                                            <label class="active" for="drpClosureReason">Closure Reason <span class="materialize-red-text font-weight-800">*</span></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col m12 padding-2"  >
                                    <asp:UpdatePanel ID="upRepeater" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnlDetail">
                                                <div class="row">
                                                    <h5 class="card-title padding-1 m-0 float-left left-align width-100 gradient-45deg-light-blue-indigo">&nbsp;Product Detail</h5>
                                                    <table id="tblInqProductGroup" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="99%">
                                                        <asp:Repeater ID="rptInquiryProductGroup" runat="server" ClientIDMode="Static" OnItemCommand="rptInquiryProductGroup_ItemCommand" OnItemDataBound="rptInquiryProductGroup_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr class="padding-1">
                                                                        <th class="text-left" width="400">Product Name <span class="materialize-red-text font-weight-800">*</span>
                                                                            <a href="javascript:openProductInfo('add');">
                                                                                <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" tabindex="13" />
                                                                            </a>
                                                                        </th>
                                                                        <th class="text-right" width="130">Unit</th>
                                                                        <th class="text-right hide">Thickness</th>
                                                                        <th class="text-right" width="80">Area</th>
                                                                        <th class="text-right" width="80">Factor</th>
                                                                        <th class="text-right" width="80">Quantity<span class="materialize-red-text font-weight-800">*</span></th>
                                                                        <th class="text-right" width="300">Remarks</th>
                                                                        <th class="text-center" width="20">Action</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="blueShed">
                                                                    <asp:HiddenField ID="hdnProductID_Grid" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                    <td class="text-left"width="400"><%# Eval("ProductNameLong") %></td>
                                                                    <td class="text-right hide"> 
                                                                        <asp:TextBox id="tdUnitPrice" runat="server" ClientIDMode="Static" Text = '<%# Eval("UnitPrice") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="text-align: right;"></asp:TextBox>
                                                                    </td>
                                                                    <td class="text-center" width="130"><%# Eval("Unit") %></td>
                                                                    <td class="text-center hide"><%# Eval("Thickness") %></td>
                                                                    <td class="text-right" width="100"><%# Eval("Area") %></td>
                                                                    <td class="text-right" width="100"><%# Eval("Factor") %></td>
                                                                    <%--<td class="text-right ">
                                                                        <asp:TextBox id="tdFactor" runat="server" ClientIDMode="Static" Text = '<%# Eval("Factor") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" width="80" Style="text-align: right;"></asp:TextBox>
                                                                    </td>
                                                                    <td class="text-right ">
                                                                        <asp:TextBox id="tdArea" runat="server" ClientIDMode="Static" Text = '<%# Eval("Area") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" width="80" Style="text-align: right;"></asp:TextBox>
                                                                    </td>--%>
                                                                    <td class="text-right ">
                                                                        <asp:TextBox id="tdQuantity" runat="server" ClientIDMode="Static" Text = '<%# Eval("Quantity") %> ' AutoPostBack="true" OnTextChanged="editItem_TextChanged" width="80" Style="text-align: right;"></asp:TextBox>
                                                                    </td>
                                                                    <td class="text-left ">
                                                                        <asp:TextBox id="tdRemarks" runat="server" ClientIDMode="Static" Text = '<%# Eval("Remarks") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" width="80" Style=" text-align: left;"></asp:TextBox>
                                                                    </td>
                                                                    <td class="text-right hide"><%#  String.Format("{0:0.00}", Convert.ToDecimal(Eval("UnitPrice")) *  Convert.ToDecimal(Eval("Quantity"))) %></td>
                                                                    <td class="text-center">
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="padding: 5px;">
                                                                    <td>
                                                                        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="9" width="400" placeholder="Search Product" />
                                                                    </td>
                                                                    <td class="text-right hide">
                                                                        <asp:TextBox ID="txtUnitPrice" runat="server" ClientIDMode="Static" Width="100" TabIndex="10" />
                                                                    </td>
                                                                    <td >
                                                                        <asp:DropDownList ID="drpUnit" runat="server" Width="130" ClientIDMode="Static" CssClass="select2-theme browser-default" TabIndex="10" >
                                                                            <asp:ListItem Text="--Select--" Value=" " />
                                                                            <asp:ListItem Text="Sq.Ft" Value="Sq.Ft" />
                                                                            <asp:ListItem Text="Sq.Mt" Value="Sq.Mt" />
                                                                            <asp:ListItem Text="Cu.Mt" Value="Cu.Mt" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="hide">
                                                                        <asp:DropDownList ID="drpThickness" runat="server" ClientIDMode="Static" CssClass="select2-theme browser-default" TabIndex="10" >
                                                                            <asp:ListItem Text="--Select--" Value=" " />
                                                                            <asp:ListItem Text="4''" Value="4 inch" />
                                                                            <asp:ListItem Text="9''" Value="9 inch" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtArea" runat="server" ClientIDMode="Static" Width="80" TabIndex="15" AutoPostBack="true" OnTextChanged="CalQuantity" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtFactor" runat="server" ClientIDMode="Static" Width="80" TabIndex="15" AutoPostBack="true" OnTextChanged="CalQuantity" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" Width="80" TabIndex="15" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" Width="300" TabIndex="15" TextMode="SingleLine"  />
                                                                    </td>
                                                                    <td class="text-right hide">
                                                                        <asp:Label ID="lblAmount" runat="server" ClientIDMode="Static" Width="80" TabIndex="12" Enabled="false" />
                                                                    </td>
                                                                    <td class="text-center">
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="20" Height="20" TabIndex="13" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none; background-color: navy; min-height: 50px;">
                                                                    <td class="text-right" colspan="3" style="color: White; font-size: 16px;">Estimated Amount: </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotalAmount" runat="server" ClientIDMode="Static" Width="80" TabIndex="14" Enabled="false" Style="color: White; font-size: 16px;" />
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                <div class="col m12">
                    <button id="btnReset" type="button" runat="server" clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" tabindex="19"><i class="material-icons left">backspace</i>Clear & Add New</button>
                    <button id="btnSaveEmail" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSaveEmail_Click" tabindex="21"><i class="material-icons left">email</i>Save - Send Email</button>
                    <button id="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="20"><i class="material-icons left">save</i>Save</button>
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
                                        $("#hdnBlockCustomer").val(item.data('BlockCustomer'));
                                        $("#drpInquirySource").val(item.data('langname1'));
                                        $("#drpInquiryStatus").focus();
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
                                        $("#hdnProductID").val(item.data('langname'));
                                        $("#txtReferenceName").focus();
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
