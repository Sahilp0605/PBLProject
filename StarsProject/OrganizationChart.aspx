<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="OrganizationChart.aspx.cs" Inherits="StarsProject.OrganizationChart" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.orgchart.css" media="all" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.11.1.min.js"></script>
    <script src="js/jquery.orgchart.js" type="text/javascript"></script>

    <script type="text/javascript">
        // -----------------------------------------------------------------------
        var itsPostBack = <%= Page.IsPostBack ? "true" : "false" %>;
        // ------------------------------------------------------------------------
        var togl = 1;
        jQuery(document).ready(function () {
            var dataPoints = JSON.parse('<%= ConvertOrgChart() %>');
                org_chart = jQuery('#orgChart').orgChart({ data: dataPoints, showControls: false, allowEdit: false });
                org_chart.draw();
            });
    </script>
    <style type="text/css">
        #orgChart {
            width: auto;
            height: auto;
        }

        #orgChartContainer {
            width: 100%;
            height: 600px;
            overflow: auto;
            background: white;
        }
        div.orgChart div.node { background:none !important; background-color:navy !important; color:white !important; height:auto !important; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="contentwrapper" class="contentwrapper">
            <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
            <div class="widgetbox">
                <div class="widgetcontent padding0 statement">
                    <div id="orgChartContainer">
                        <div id="orgChart"></div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnSelectedDeptCode" runat="server" />
        <asp:HiddenField ID="hdnSelectedChartType" runat="server" />
        <div class="clearall"></div>
        <div class="clearall"></div>
        <%--</asp:Content>--%>
    </form>
</body>
</html>
