<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FollowupTimeline.aspx.cs" Inherits="StarsProject.FollowupTimeline" %>
<%@ Register Src="~/myFollowupTimeline.ascx" TagPrefix="uc1" TagName="myFollowupTimeline" %>
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

    <style type="text/css">
        .h5Round {
            border-radius: 8px 8px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, #303f9f, #7b1fa2) !important;
            color: white !important;
        }

        .h5RoundHead {
            border-radius: 0px 0px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, silver, #7f88ec6e);
        }
    </style>
    <script type="text/javascript">
        function openFollowup(mode, id) {
<%--            $.colorbox({
                fixed: true, width: "98%", height: "95%", iframe: true,
                href: "Followup.aspx?mode=Edit&id=0&CustomerID=<%= hdnCustomerID.Value %>&CustomerName=<%= hdnCustomerName.Value %>",
                onClosed: function () {
                    location.reload();
                }
            });--%>
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustomerName" runat="server" ClientIDMode="Static" />

        <div id="myModal" style="display: block; width: 100%;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: navy; color: white; font-size: 18px; font-weight: 400;">
                    <div class="navbar-main navbar-color nav-collapsible nav-expanded navbar-dark gradient-45deg-indigo-purple no-shadow">
                        <h5 class="h5RoundHead pl-1" style="line-height:40px;">
                            <asp:Label ID="lblCustomerName" runat="server" ClientIDMode="Static" Style="color: lightgoldenrodyellow;" /></b>
                        </h5>
                    </div>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col m5">
                            <div class="row">
                                <div class="col m12">
                                    <div class="w-100 mt-5">
                                        <h5 class="h5RoundHead">Primary Address</h5>
                                        <p class="m-0 p-0" style="text-transform: uppercase;">
                                            <asp:Label ID="lblAddress11" runat="server" ClientIDMode="Static" />
                                        </p>
                                        <p class="m-0 p-0" style="text-transform: uppercase;">
                                            <asp:Label ID="lblAddress12" runat="server" ClientIDMode="Static" />
                                        </p>
                                        <p class="m-0 p-0" style="text-transform: uppercase;">
                                            <asp:Label ID="lblAddress13" runat="server" ClientIDMode="Static" />
                                        </p>
                                    </div>
                                    <div class="w-100 mt-5">
                                        <h5 class="h5RoundHead">Contact Us</h5>
                                        <table>
                                            <tr class="mb-1">
                                                <td style="float: left; width: 40%;">Contact # :</td>
                                                <td style="float: left; width: 60%;">
                                                    <asp:Label ID="lblContact" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                            </tr>
                                            <tr class="mb-1">
                                                <td style="float: left; width: 40%;">Email ID  :</td>
                                                <td style="float: left; width: 60%;">
                                                    <asp:Label ID="lblEmail" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                            </tr>
                                            <tr class="mb-1">
                                                <td style="float: left; width: 40%;">Website   :</td>
                                                <td style="float: left; width: 60%;">
                                                    <asp:Label ID="lblWebsite" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col m7" style="overflow-y:auto; height: 430px; border: 2px solid silver;">
                            <uc1:myFollowupTimeline runat="server" ID="myFollowupTimeline" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
