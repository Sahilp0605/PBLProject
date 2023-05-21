<%@ Page Title="" Language="C#" MasterPageFile="~/Dealer.Master" AutoEventWireup="true" CodeBehind="DashboardDealer.aspx.cs" Inherits="StarsProject.DashboardDealer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myDealerOrderApproval.ascx" TagPrefix="uc1" TagName="myDealerOrderApproval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .card-title {
            color: #1E90FF;
        }
        .mydropdowncontent {
            width: 230px !important;
            height: auto !important;
        }
            .mydropdowncontent li {
                min-height: auto;
                padding: 0px;
            }
                .mydropdowncontent li > a {
                    min-height: auto;
                    padding: 2px;
                    font-size: 12px;
                    color: navy;
                    padding: 3px 5px;
                    background-color: #F3EBEB;
                }
        .dataTables_filter, .dataTables_info, .dataTables_length {
            display: none;
        }
        table.dataTable thead th, table.dataTable thead td, {
            padding: 5px 5px;
            font-size: 12px;
        }
        table.dataTable tbody th, table.dataTable tbody td {
            padding: 5px 5px;
            font-size: 11px;
        }
        .myDropDownClass {
            background-color: #F9E3B1;
            color: navy;
            font-size: medium;
            width: 150px;
            height: 30px !important;
            border: 1px solid black;
            margin-top: 1px;
            padding: 1px 5px;
            border: none;
        }
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
        .cardRound {
            box-shadow: black -6px 8px 6px -6px !important;
            border-radius: 8px 8px 0px 0px !important;
        }
        .h5Round {
            border-radius: 8px 8px 0px 0px;
            margin-bottom: 5px;
            padding: 5px;
            line-height: 35px;
            background: linear-gradient(45deg, #303F9F, #7B1FA2) !important;
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
        var pageUrl = '';
        function viewSalesOrder(id) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Dealer Sales Order";
            var pageUrl = "SalesOrderDealer.aspx?mode=view&id=" + id;
            $('#ifrModuleSales').attr('src', pageUrl);
            $find("mpe").show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" CompositeScript-ScriptMode="Inherit"></asp:ScriptManager>
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientURL" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnWelcomeGreet" runat="server" ClientIDMode="Static" />
    <div id="divLedger" style="background: linear-gradient(45deg, silver, #F9E3B1); margin-bottom: 10px; padding-left: 10px; padding-right: 10px;">
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <div class="content-wrapper-before gradient-45deg-indigo-purple"></div>
          <div class="row">
            <div class="col m12">
                <div class="card user-statistics-card mb-0">
                    <div class="card-content p-0">
                        <h5 class="h5RoundHead">
                            <span id="Span4" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Filter Data : </span>
                            <asp:DropDownList ID="drpDailyMonth" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpDailyMonthYear_SelectedIndexChanged"></asp:DropDownList>
                            <asp:DropDownList ID="drpDailyYear" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpDailyMonthYear_SelectedIndexChanged"></asp:DropDownList>
                        </h5>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <!-- Dealer Sales Order Approval -->
            <div class="col m12">
                <asp:UpdatePanel ID="updDealerSalesOrder" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-0">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New Dealer Sales Order" href="javascript:openDashboardBox('salesorderdealer', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="Span2" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Dealer Sales Order</span>
                                    <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" class="myDropDownClass select2-theme browser-default mr-2 float-right" AutoPostBack="true" OnSelectedIndexChanged="drpApprovalStatus_SelectedIndexChanged">
                                        <asp:ListItem Text="-- All --" Value="" />
                                        <asp:ListItem Text="Pending" Value="Pending" />
                                        <asp:ListItem Text="On Hold" Value="On Hold" />
                                        <asp:ListItem Text="Approved" Value="Approved" />
                                        <asp:ListItem Text="Dispatched" Value="Dispatched" />
                                        <asp:ListItem Text="Rejected" Value="Rejected" />
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 250px; max-height: 250px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myDealerOrderApproval runat="server" ID="myDealerOrderApproval" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpApprovalStatus" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
        <div id="myModal">
            <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                    <i class="material-icons prefix">ac_unit</i>
                    <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Dealer Sales Order</span>
                    <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                </h5>
            </div>
        </div>
        <iframe id="ifrModuleSales" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
        <br />
    </asp:Panel>
    <script type="text/javascript">
        window.onload = function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "Accounts - Analytics & Summary";
        }
        $(document).ready(function () {
            $('.btnTopRightCorner').on("click", function () {
                var pgSource = $('#ifrModuleSales').attr('src');
                var strPos1 = pgSource.indexOf('mode=add');
                var strPos2 = pgSource.indexOf('mode=edit');
                if (strPos1 >= 0 || strPos2 >= 0) {
                    __doPostBack('', '');
                }
            });
        });
        function capitalizeString(str) {
            return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }
        function openDashboardBox(view, mode, id) {
            if (view == 'salesorderdealer') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Dealer Sales Order";
                var pageUrl = "SalesOrderDealer.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleSales').attr('src', pageUrl);
                $find("mpe").show();
            }
        }
    </script>
</asp:Content>