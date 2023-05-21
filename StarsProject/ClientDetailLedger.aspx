<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientDetailLedger.aspx.cs" Inherits="StarsProject.ClientDetailLedger" %>

<%@ Register Src="~/myClientDetailLedger.ascx" TagPrefix="uc1" TagName="myClientDetailLedger" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <script type="text/javascript">
        function viewLedgerDocument(id, cat) {
            if (cat == 'SalesOrder') {
                $.colorbox({ width: "98%", height: "95%", iframe: true, href: "SalesOrder.aspx?mode=view&id=" + id, onClosed: function () { } });
            }
            else {
                $.colorbox({ width: "98%", height: "95%", iframe: true, href: "FinancialTrans.aspx?mode=view&category=" + cat + "&id=" + id, onClosed: function () { } });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
        <div id="myModal" style="display: block; width: 100%;">
            <div class="modal-content">
                <div class="modal-header" style="padding: 10px 5px 0px 5px; font-size:12px; background-color: #e8e8c0;">
                    <div class="row mb-0" style="font-size:24px;">
                        <div class="col m6">
                            Customer Name :&nbsp;<label id="lblCustomerName" runat="server" class="active" style="font-size:24px; color: Red !important;"></label>
                        </div>
                        <div class="col m6 right-align">
                            GST No :&nbsp;<label id="lblGSTNO" runat="server" class="active" style="font-size:24px; color: Navy !important;"></label>
                        </div>
                    </div>
                </div>
                <div class="modal-body" style="min-height: 500px; padding: 0px 5px;">
                    <uc1:myClientDetailLedger runat="server" ID="myClientDetailLedger" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
