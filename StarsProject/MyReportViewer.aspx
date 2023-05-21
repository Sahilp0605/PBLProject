<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyReportViewer.aspx.cs" Inherits="StarsProject.MyReportViewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
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

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnReportName" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnKeyVal" runat="server" ClientIDMode="Static" />
        <div>
            <div class="row" style="padding-top: 10px">
                <div class="col m12">

                    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" HasToggleGroupTreeButton="False" HasPrintButton="true" HasPageNavigationButtons="True" DisplayToolbar="True" ToolPanelView="None" SeparatePages="True" Width="100%" BestFitPage="False" CssClass="depth" Height="450" HasExportButton="True" HasDrillUpButton="False" HasCrystalLogo="False" PrintMode="Pdf" />

                </div>
            </div>
        </div>
    </form>
</body>
</html>
