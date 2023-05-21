<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadSalesOrder.aspx.cs" Inherits="StarsProject.UploadSalesOrder" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="app-assets/js/plugins.js"></script>
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <script type="text/javascript">
        
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
    </script>
</head>
<body>
    <form id="form1" runat="server" style="color:navy;">
        <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="widgetbox">
                    <div id="myModal" style="display: block; width: 100%; overflow-x: hidden;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="col m12">
                                    <div class="row">
                                        <div class="input-field col m3">
                                            <label class="active" for="txtOrderDate">Order Date <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtOrderDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" placeholder="" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m4">
                                            <asp:HiddenField ID="hdnCustStateID" runat="server" ClientIDMode="Static" />
                                            <label class="active" for="txtCustomerName">
                                                Customer Name 
                                        <%--<small class="text-muted font-italic">(Minimum 3 chars to activate search)</small>--%> 
                                                <span class="materialize-red-text font-weight-800">*</span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                    <img src="images/registration.png" width="20" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block; padding: 6px 0px 0px 0px;" />
                                                </a>
                                                <a href="javascript:openCustomerInfo('add');">
                                                    <img src="images/addCustomer.png" width="20" height="20" alt="Add New Customer" title="Add New Customer" style="display: inline-block; padding: 6px 0px 0px 0px;"/>
                                                </a>
                                            </label>
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" AutoPostBack="true" OnTextChanged="txtCustomerName_TextChanged" TabIndex="2" placeholder="Minimum 3 chars to activate search" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row gradient-45deg-indigo-purple white-text">
                                    <div class="col m12">
                                        <div id="dvFileUpload" style="margin:20px 100px;">
                                            <asp:FileUpload ID="FileUpload1" runat="server" accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" ClientIDMode="Static" onchange="this.form.submit()" />
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <p><asp:Label id="lblConfirm" runat="server"></asp:Label></p>
                                </div>
                                
                            </div>
                        </div>
                    </div>
                </div>

                <script type="text/javascript">
                       
                    function bindDDLTo(selector) {
                            if ($(selector).val().length >= 1) {
                                jQuery.ajax({
                                    type: "POST",
                                    url: "InquiryInfo.aspx/FilterCustomerByModule",
                                    data: '{pCustName:\'' + $(selector).val() + '\', pSearchModule:\'CustomerSearchSales\'}',
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        console.log(data);
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
                                                //search = search.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
                                                //var re = new RegExp("(" + search.split(' ').join('|') + ")", "gi");
                                                return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                            },
                                            onSelect: function (e, term, item) {
                                                //console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                                $(selector).val(item.html());
                                                $("#hdnCustomerID").val(item.data('langname'));
                                                $("#txtReference").focus();
                                            }
                                        });
                                    },
                                    error: function (r) { alert('Error : ' + r.responseText); },
                                    failure: function (r) { alert('failure'); }
                                });
                            }
                            else {
                                if ($(selector).val().length == 0) {
                                    $("#hdnCustomerID").val('');
                                }
                            }
                        }
                    </Script>

            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>

