<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Complaint.aspx.cs" Inherits="StarsProject.Complaint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myModuleAttachment.ascx" TagPrefix="uc1" TagName="myModuleAttachment" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
            document.getElementById('spnModuleHeader').innerText = "Manage Customer";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpEmployee').val() : "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Organization Employees";
            var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openProductInfo(cat) {
            var keyid = "0";
            document.getElementById('spnModuleHeader').innerText = "Manage Product";
            var pageUrl = "Products.aspx?mode=view&id=0";
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
                    <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />

                    <div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="input-field col m2">
                                        <label class="active" for="lblComplaintNo">Complaint #</label>
                                        <asp:TextBox ID="lblComplaintNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" placeholder="" />
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="txtComplaintDate">Complaint Date <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtComplaintDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" onpaste="return false" placeholder=""   TextMode="Date"/>
                                    </div>
                                    <div class="input-field col m5">
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
                                    <div class="input-field col m2">
                                        <label class="active" for="txtReferenceNo">Reference #</label>
                                        <asp:TextBox ID="txtReferenceNo" runat="server" class="form-control" ClientIDMode="Static" MaxLength="20" TabIndex="4" placeholder="" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m3">
                                        <label class="active" for="txtSrNo">Sr No.</label>
                                        <asp:TextBox ID="txtSrNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" placeholder="" />
                                    </div>
                                    <div class="input-field col m8">
                                        <label class="active" for="txtProductName">Product Name
                                             <a href="javascript:openProductInfo('add');">
                                                <img src="images/expand.png" width="30" height="20" style="padding: 5px 5px 0px 10px;" alt="Add New Product" title="Add New Product" />
                                            </a>
                                        </label>
                                        <asp:TextBox ID="txtProductName" runat="server" class="form-control" ClientIDMode="Static" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="4" placeholder="Search Product" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m6">
                                        <div class="row mt-1 badge-secondary" style="border-radius: 5px; padding: 5px; margin-left:-22px">
                                            <div class="input-field col m12">
                                                <label class="active" for="txtComplaintNotes">Complaint Description <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtComplaintNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="5" MaxLength="500" TextMode="MultiLine" Rows="4" style="min-height:120px;" placeholder=""/>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m6">
                                        <div class="row mt-1 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                            <div class="input-field col m6 mb-0">
                                                <label class="active" for="txtPreferredDate">Schedule Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtPreferredDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" TextMode="Date" placeholder="" />
                                            </div>
                                            <div class="input-field col m6 mb-0">
                                                <label class="active" for="txtTimeFrom">Preferred Time </label>
                                                <asp:TextBox ID="txtTimeFrom" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="7" onkeypress="return false;" onpaste="return false" style="width:100px; display:inline-block;" placeholder="" />&nbsp;&nbsp;
                                                <asp:TextBox ID="txtTimeTo" runat="server" class="timepicker" ClientIDMode="Static" TabIndex="8" onkeypress="return false;" onpaste="return false" style="width:100px; display:inline-block;" placeholder="" />
                                            </div>
                                        </div>
                                        <div class="row mt-1 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                            <div class="input-field col m4">
                                                <label class="active" for="drpEmployee">Assigned To<span class="materialize-red-text font-weight-800">*</span>
                                                    <a href="javascript:openEmployeeInfo('view');">
                                                        <img src="images/registration.png" width="20" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="3" />
                                                    </a>
                                                    <a href="javascript:openEmployeeInfo('add');">
                                                        <img src="images/addCustomer.png" width="20" height="20" alt="Add New Employee" title="Add New Employee" />
                                                    </a>
                                                </label>
                                                <asp:DropDownList ID="drpEmployee" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="9"></asp:DropDownList>
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="drpStatus">Status</label>
                                                <asp:DropDownList ID="drpStatus" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="10">
                                                    <asp:ListItem Text="Open" Value="Open" Selected="True" />
                                                    <asp:ListItem Text="Close" Value="Close" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="drpType">Type</label>
                                                <asp:DropDownList ID="drpType" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="11">
                                                    <asp:ListItem Text="---Select Type---" Value="" Selected="True" />
                                                    <asp:ListItem Text="Online" Value="Online" />
                                                    <asp:ListItem Text="Offline" Value="Offline" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m12">
                                        <uc1:myModuleAttachment runat="server" ID="myModuleAttachment" />
                                    </div>
                                </div>
                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                    <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="12"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                    <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="13"><i class="material-icons left">save</i>Save</button>
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

                    function bindDDLProductTo(selector) {
                        if ($(selector).val().length >= 3) {
                            jQuery.ajax({
                                type: "POST",
                                url: "InquiryInfo.aspx/FilterProduct",
                                data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchAll\'}',
                                //data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\'}',
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
                                            console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                            $(selector).val(item.html());
                                            $("#hdnProductID").val(item.data('langname'));
                                           
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
   <%--     </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    </form>
</body>
</html>