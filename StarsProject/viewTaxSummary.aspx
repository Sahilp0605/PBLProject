<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewTaxSummary.aspx.cs" Inherits="StarsProject.viewTaxSummary" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myTaxSummary.ascx" TagPrefix="uc1" TagName="myTaxSummary" %>

<!DOCTYPE html>

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
    
    <script src="app-assets/vendors/select2/select2.full.min.js"></script>
    <script src="app-assets/js/scripts/form-select2.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
</head>
<body>
    <form id="frmViewTaxSummary" runat="server">
        <asp:HiddenField ID="hdnMainModule" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnMainKeyID" runat="server" ClientIDMode="Static" />
        <div class="row">
            <div class="col m4">
                <label class="active" for="drpState">HSN Wise Summary</label>
                <asp:DropDownList ID="drpHSNFlag" runat="server" ClientIDMode="Static" TabIndex="5" class="select2-theme browser-default" Style="height: inherit;" AutoPostBack="true" OnSelectedIndexChanged="drpHSNFlag_SelectedIndexChanged">
                    <asp:ListItem Text="No" Value="0" />
                    <asp:ListItem Text="Yes" Value="1" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col m4">
                <uc1:myTaxSummary runat="server" id="myTaxSummary" />
            </div>
        </div>
    </form>
</body>
</html>
