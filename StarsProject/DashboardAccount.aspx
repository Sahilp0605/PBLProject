<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="DashboardAccount.aspx.cs" Inherits="StarsProject.DashboardAccount" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myClientLedger.ascx" TagPrefix="uc1" TagName="myClientLedger" %>
<%@ Register Src="~/myOrderApproval.ascx" TagPrefix="uc1" TagName="myOrderApproval" %>
<%@ Register Src="~/mySalesTarget.ascx" TagPrefix="uc1" TagName="mySalesTarget" %>
<%@ Register Src="~/MyOrderBillingStatus.ascx" TagPrefix="uc1" TagName="MyOrderBillingStatus" %>
<%@ Register Src="~/myGSTRSummary.ascx" TagPrefix="uc1" TagName="myGSTRSummary" %>
<%@ Register Src="~/myOutstandingBills.ascx" TagPrefix="uc1" TagName="myOutstandingBills" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .card-title {
            color: #1e90ff;
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
                    background-color: #f3ebeb;
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
            background-color: #f9e3b1;
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
        var pageUrl = '';
        function viewUserInquiryList(userid, empname) {
            var t12, t13;
            t12 = (jQuery("#hdnMonth1").val() != '') ? jQuery("#hdnMonth1").val() : 0;
            t13 = (jQuery("#hdnYear1").val() != '') ? jQuery("#hdnYear1").val() : 2020;
            var clienturl = $("#hdnClientURL").val();
            pageUrl = "myCustomView.aspx?view=inquirybyuser&para1=" + userid + "&para2=" + t12 + "&para3=" + t13 + "&para4=" + empname;
            $('#ifrModuleSales').attr('src', pageUrl);
            $find("mpe").show();
        }

        function viewLedgerDetail(id, idname) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Client Ledger";
            var pageUrl = "ClientDetailLedger.aspx?custname=" + idname + "&id=" + id;
            $('#ifrModuleSales').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewSalesOrder(id) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Sales Order";
            var pageUrl = "SalesOrder.aspx?mode=view&id=" + id;
            $('#ifrModuleSales').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewSalesTarget(id) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Sales Target";
            var pageUrl = "SalesTarget.aspx?mode=view&id=" + id;
            $('#ifrModuleSales').attr('src', pageUrl);
            $find("mpe").show();
        }
        function viewSalesOrder(id) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Sales Order";
            var pageUrl = "SalesOrder.aspx?mode=view&id=" + id;
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


    <div id="divLedger" style="background: linear-gradient(45deg, silver, #f9e3b1); margin-bottom: 10px; padding-left: 10px; padding-right: 10px;">
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <div class="content-wrapper-before gradient-45deg-indigo-purple"></div>

        <div class="row">
            <div class="col m12">
                <div class="card user-statistics-card mb-0 mt-1">
                    <div class="card-content p-0">
                        <h5 class="h5RoundHead" style="min-height: 42px;">
                            <span id="Span4" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px; float:left;">Filter : </span>
                            <asp:DropDownList ID="drpDailyMonth" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" style="float:left;" AutoPostBack="true" OnSelectedIndexChanged="drpDailyMonthYear_SelectedIndexChanged"></asp:DropDownList>
                            <asp:DropDownList ID="drpDailyYear" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" style="float:left;" AutoPostBack="true" OnSelectedIndexChanged="drpDailyMonthYear_SelectedIndexChanged"></asp:DropDownList>
                        </h5>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col m12" style="margin-right: 5px;">
                <!-- User Statistics -->
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-1">
                            <div class="card-content p-0">
                                <h5 class="h5Round">
                                    <span id="Span5" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">GSTR Detail
                                    <asp:DropDownList ID="drpGSTR" runat="server" ClientIDMode="Static" class="myDropDownClass select2-theme browser-default mr-2 float-right" AutoPostBack="true" OnSelectedIndexChanged="drpGSTR_SelectedIndexChanged">
                                        <asp:ListItem Text="GSTR1" Value="GSTR1" />
                                        <asp:ListItem Text="GSTR2" Value="GSTR2" />
                                        <asp:ListItem Text="GSTR3B" Value="GSTR3B" />
                                    </asp:DropDownList>
                                    </span>
<%--                                    <div class="invoice-action-btn">
                                        <a href="#" class="btn-block btn btn-light-indigo waves-effect waves-light invoice-print">
                                          <span>Print</span>
                                        </a>
                                    </div>--%>
                                </h5>
                                <div class="invoice-print" style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myGSTRSummary runat="server" ID="myGSTRSummary" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col m6" style="padding-right: 5px;">
                <!-- User Statistics -->
                <asp:UpdatePanel ID="updmyLedgerRec" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-2">
                            <div class="card-content p-0">
                                <h5 class="h5Round">
                                    <span id="spnLedgerRec" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Account Ledger - Debtors</span>
                                </h5>
                                <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myClientLedger runat="server" ID="myLedgerRec" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col m6" style="padding-left: 5px;">
                <!-- User Statistics -->
                <asp:UpdatePanel ID="updsmyLedgerPay" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-2">
                            <div class="card-content p-0">
                                <h5 class="h5Round">
                                    <span id="spnLedgerPay" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Account Ledger - Creditors</span>
                                </h5>
                                <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myClientLedger runat="server" ID="myLedgerPay" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row">
            <!-- Sales Order Approval -->
            <div class="col m6" style="padding-right: 5px;">
                <asp:UpdatePanel ID="updSalesOrder" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-2">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New Sales Order" href="javascript:openDashboardBox('salesorder', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="Span2" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Sales Order</span>
                                    <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" class="myDropDownClass select2-theme browser-default mr-2 float-right" AutoPostBack="true" OnSelectedIndexChanged="drpApprovalStatus_SelectedIndexChanged">
                                        <asp:ListItem Text="Pending" Value="Pending" />
                                        <asp:ListItem Text="On Hold" Value="On Hold" />
                                        <asp:ListItem Text="Approved" Value="Approved" />
                                        <asp:ListItem Text="Dispatched" Value="Dispatched" />
                                        <asp:ListItem Text="Rejected" Value="Rejected" />
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myOrderApproval runat="server" ID="myOrderApproval" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpApprovalStatus" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <!-- Billing Status -->
            <div class="col m6" style="padding-left: 5px;">
                <asp:UpdatePanel ID="updBillingStatus" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-2">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New Billing Status" href="javascript:openDashboardBox('salesorder', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="Span3" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Billing Status</span>
                                    <asp:DropDownList ID="drpBillStatus" runat="server" ClientIDMode="Static" class="myDropDownClass select2-theme browser-default mr-2 float-right" AutoPostBack="true" OnSelectedIndexChanged="drpBillStatus_SelectedIndexChanged">
                                        <asp:ListItem Text="Pending" Value="Pending" />
                                        <asp:ListItem Text="Bill Generated" Value="BillGenerated" />
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:MyOrderBillingStatus runat="server" ID="MyOrderBillingStatus" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpBillStatus" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row">
            <div class="col m12" style="padding-left: 5px;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-2">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New Billing Status" href="javascript:openDashboardBox('salesorder', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="Span6" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Outstanding Purchase / Sales Billing  Status</span>
                                    <asp:DropDownList ID="drpPendingStatus" runat="server" ClientIDMode="Static" class="myDropDownClass select2-theme browser-default mr-2 float-right" AutoPostBack="true" OnSelectedIndexChanged="drpPendingStatus_SelectedIndexChanged">
                                        <asp:ListItem Text="Pending" Value="Pending" />
                                        <asp:ListItem Text="Completed" Value="Completed" />
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="drpBillingCategory" runat="server" ClientIDMode="Static" class="myDropDownClass select2-theme browser-default mr-2 float-right" AutoPostBack="true" OnSelectedIndexChanged="drpBillingCategory_SelectedIndexChanged">
                                        <asp:ListItem Text="Purchase" Value="Purchase" />
                                        <asp:ListItem Text="Sales" Value="Sales" />
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="drpAging" runat="server" ClientIDMode="Static" class="myDropDownClass select2-theme browser-default mr-2 float-right" AutoPostBack="true" OnSelectedIndexChanged="drpAging_SelectedIndexChanged">
                                        <asp:ListItem Text="-- No Ageing --" Value="" />
                                        <asp:ListItem Text="By Bill Date" Value="bill" />
                                        <asp:ListItem Text="By Due Date" Value="due" />
                                    </asp:DropDownList>
                                </h5>
                                <div id="divAging" runat="server" style="margin:8px 10px; border:4px solid navy;" visible="false">
                                    <div class="row" style="padding:10px 10px 5px 10px;">
                                        <div class="col m2">
                                            <%--<label class="active" for="lblSlab1">SLAB-I</label>--%>
                                            <asp:TextBox ID="txtAge1" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" MaxLength="150" placeholder="" Text="0" AutoPostBack="true" OnTextChanged="txtAge_TextChanged" />
                                        </div>
                                        <div class="col m2">
                                            <asp:TextBox ID="txtAge2" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" MaxLength="150" placeholder="" Text="30" AutoPostBack="true" OnTextChanged="txtAge_TextChanged" />
                                        </div>
                                        <div class="col m2">
                                            <asp:TextBox ID="txtAge3" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" MaxLength="150" placeholder="" Text="30" AutoPostBack="true" OnTextChanged="txtAge_TextChanged" />
                                        </div>
                                        <div class="col m2">
                                            <asp:TextBox ID="txtAge4" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" MaxLength="150" placeholder="" Text="60" AutoPostBack="true" OnTextChanged="txtAge_TextChanged" />
                                        </div>
                                        <div class="col m2">
                                            <asp:TextBox ID="txtAge5" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" MaxLength="150" placeholder="" Text="60" AutoPostBack="true" OnTextChanged="txtAge_TextChanged" />
                                        </div>
                                        <div class="col m2">
                                            <asp:TextBox ID="txtAge6" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" MaxLength="150" placeholder="" Text="90" AutoPostBack="true" OnTextChanged="txtAge_TextChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myOutstandingBills runat="server" id="myOutstandingBills" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpPendingStatus" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="drpBillingCategory" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="drpAging" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAge1" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAge2" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAge3" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAge4" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAge5" EventName="TextChanged" />
                        <asp:AsyncPostBackTrigger ControlID="txtAge6" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <!-- Sales Target -->
            <div class="col m12">
                <asp:UpdatePanel ID="updSalesTarget" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card user-statistics-card cardRound mb-2">
                            <div class="card-content p-0">
                                <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New Sales Target" href="javascript:openDashboardBox('salestarget', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                <h5 class="h5Round">
                                    <span id="Span1" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Sales Target</span>
                                    <asp:DropDownList ID="drpTargetType" runat="server" ClientIDMode="Static" class="myDropDownClass select2-theme browser-default mr-2 float-right" AutoPostBack="true" OnSelectedIndexChanged="drpTargetType_SelectedIndexChanged">
                                        <asp:ListItem Text="Amount" Value="A" />
                                        <asp:ListItem Text="Quantity" Value="Q" />
                                    </asp:DropDownList>
                                </h5>
                                <div style="width: 100%; min-height: 180px; max-height: 250px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:mySalesTarget runat="server" ID="mySalesTarget" />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drpTargetType" EventName="SelectedIndexChanged" />
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
                    <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
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

            //$(".invoice-print").on("click", function () {
            //    window.print();
            //})
        });

        function capitalizeString(str) {
            return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }
        function openDashboardBox(view, mode, id) {
            if (view == 'salesorder') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Sales Order";
                var pageUrl = "SalesOrder.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleSales').attr('src', pageUrl);
                $find("mpe").show();

            }
            if (view == 'salestarget') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Sales Target";
                var pageUrl = "SalesTarget.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModuleSales').attr('src', pageUrl);
                $find("mpe").show();

            }
        }
        // ------------------------------------------------------------
        // Footer : Quick Link
        // ------------------------------------------------------------
        function openDashboardDaily() {
            $('#ifrModuleSales').attr('src', "DashboardDaily.aspx");
            $find("mpe").show();
        }
        function openDashboardLead() {
            $('#ifrModuleSales').attr('src', "DashboardLead.aspx");
            $find("mpe").show();
        }
        function openDashboardSales() {
            $('#ifrModuleSales').attr('src', "DashboardAccount.aspx");
            $find("mpe").show();
        }
        function openDashboardSupport() {
            $('#ifrModuleSales').attr('src', "DashboardSupport.aspx");
            $find("mpe").show();
        }
        function openCalender() {
            $('#ifrModuleSales').attr('src', "myEventCalender.aspx");
            $find("mpe").show();
        }
        function openTrackEmployee() {
            spnModuleHeader.innerText = "Sales Executive - Track Visit";
            $('#ifrModuleSales').attr('src', "TrackEmployee.aspx");
            $find("mpe").show();

        }
        function openCustomerSearch() {
            spnModuleHeader.innerText = "Customer Activity Information";
            $('#ifrModuleSales').attr('src', "SearchCustomer.aspx");
            $find("mpe").show();
        }
        function openEmployeeSearch() {
            spnModuleHeader.innerText = "Employee Activity Information";
            $('#ifrModuleSales').attr('src', "SearchEmployee.aspx");
            $find("mpe").show();
        }
        function openOrganizationChart() {
            spnModuleHeader.innerText = "Organization Mapping";
            $('#ifrModuleSales').attr('src', "OrganizationChart.aspx");
            $find("mpe").show();
        }

        function openChatBox() {
            spnModuleHeader.innerText = "Chat Box";
            $('#ifrModuleSales').attr('src', "ChatBox.aspx");
            $find("mpe").show();
        }
        function openMailBox() {
            spnModuleHeader.innerText = "Mail Box";
            $('#ifrModuleSales').attr('src', "MailBox.aspx");
            $find("mpe").show();
        }
        function openLibrary() {
            spnModuleHeader.innerText = "Document Gallery";
            $('#ifrModuleSales').attr('src', "DocumentGallery.aspx");
            $find("mpe").show();
        }
    </script>
</asp:Content>

