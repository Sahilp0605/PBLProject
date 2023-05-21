<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesTarget.aspx.cs" Inherits="StarsProject.SalesTarget" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

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

        /*<%--Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020--%>*/
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

        <%--Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020--%>
        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpEmployee').val() : "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Organization Employees";
            var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        /*Add Product view on Product Name Vikram Rajput 15-07-2020*/
        function openProductInfo(cat) {
            var keyid = "0";
            var pageUrl = "Products.aspx?mode=view&id=0";
            document.getElementById('spnModuleHeader').innerText = "Manage Product";
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
<body class="loginpage">
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
        <%--<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <div class="widgetbox">
                    <%-- Bootstrap Modal Popup --%>
                    <div class="clearall"></div>
                    
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                    <div id="myModal" class="mt-2">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="input-field col m3">
                                        <label class="active" for="txtFromDate">From Date <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtFromDate" runat="server" class="form-control" TextMode="Date" onkeypress="return false;" onpaste="return false" placeholder="" ClientIDMode="Static" TabIndex="1" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtToDate">To Date <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtToDate" runat="server" class="form-control" TextMode="Date" onkeypress="return false;" onpaste="return false" placeholder="" ClientIDMode="Static" TabIndex="2" />
                                    </div>
                                    <div class="input-field col m6">
                                        <label class="active" for="drpEmployee">Employee Name <span class="materialize-red-text font-weight-800">*</span>
                                            <%--Preview-Add EmployeeList on Assign Name Dropdown by Vikram Rajput 13-07-2020--%>
                                                            <a href="javascript:openEmployeeInfo('view');">
                                                                <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="3" />
                                                            </a>
                                                            <a href="javascript:openEmployeeInfo('add');">
                                                                <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                            </a>
                                        </label>
                                        <asp:DropDownList ID="drpEmployee" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="3"></asp:DropDownList>
                                    </div>
                                </div>
                                 <div class="row">
                                    <div class="input-field col m4">
                                        <label class="active" for="drpBrand">Product Brand <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:DropDownList ID="drpBrand" runat="server" class="select2-theme browser-default" TabIndex="4" />
                                    </div>
                                    <div class="input-field col m4">
                                        <label class="active" for="drpProductGroup">Product Group <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:DropDownList ID="drpProductGroup" runat="server" class="select2-theme browser-default" TabIndex="5" />
                                    </div>  
                                     <div class="input-field col m4">
                                        <label class="active" for="txtProductName">Product Name  <span class="materialize-red-text font-weight-800">*</span>
                                            <%--/*Add Product view on Product Name Vikram Rajput 15-07-2020*/--%>
                                            <a href="javascript:openProductInfo('add');">
                                                <img src="images/expand.png" width="30" height="20" style="padding: 0px 0px 0px 10px;" alt="Add New Product" title="Add New Product" tabindex="6" />
                                            </a>
                                        </label>
                                         <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                        <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" AutoPostBack="true"  class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" TabIndex="6" placeholder="Min. 3 Chars To Search"/>
                                     </div>  
                                </div> 
                                <div class="row hide">
                                     <div class="input-field col m6">
                                          <label class="active" for="rdblSalesTargetType">Select Sales Target Type</label>
                                            <asp:RadioButtonList class="form-control" ID="rdblSalesTargetType" runat="server" RepeatDirection="Horizontal" onselectedindexchanged="rdblSalesTargetType_SelectedIndexChanged" AutoPostBack="True" TabIndex="7">
                                                  <asp:ListItem Text="Amount" runat="server" Selected="True" Value="1" style="margin-right:70px"></asp:ListItem>
                                                  <asp:ListItem Text="Quantity" runat="server" Value="2" ></asp:ListItem>
                                            </asp:RadioButtonList>
                                     </div>
                                </div> 
                                <div class="row">
                                    <div class="input-field col m3">
                                        <label class="active" for="txtTargetAmount">Sales Target Amount <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtTargetAmount" runat="server" ClientIDMode="Static" MaxLength="15" TabIndex="8" placeholder="" />      
                                    </div> 
                                </div>
                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                    <button id="btnReset" type="button" runat="server"  clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" tabindex="36"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                    <button id="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="38"><i class="material-icons left">save</i>Save</button>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
        </div>

                                        <%--Preview-Add EmployeeList on Employee Name Dropdown by Vikram Rajput 13-07-2020--%>
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
        <script type="text/javascript">
            $("input").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    //$("form").submit();
                }
            });

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
                                    console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                    $(selector).val(item.html());
                                    $("#hdnProductID").val(item.data('langname'));
                                    $("#rdblSalesTargetType").focus();
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
    </form>
</body>
</html>
