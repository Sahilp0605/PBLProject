<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="DashboardSupport.aspx.cs" Inherits="StarsProject.DashboardSupport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myComplaint.ascx" TagPrefix="uc1" TagName="myComplaint" %>
<%@ Register Src="~/myAMCContract.ascx" TagPrefix="uc1" TagName="myAMCContract" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .myDropDownClass {
            background-color: #fff6e1;
            color: navy;
            font-size: medium;
            width: 150px;
            height: 30px !important;
            border: 1px solid darkgray;
            margin-top: 1px;
            padding: 1px 5px;
            border-radius: 5px;
        }

        .card-title {
            color: #1e90ff;
        }

        .myDrpContent {
            width: 230px !important;
            height: auto !important;
        }

            .myDrpContent li {
                min-height: auto;
                padding: 0px;
            }

                .myDrpContent li > a {
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
            font-size: 12px;
            color: black;
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
        function viewComplaint(mode, id) {
            Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Complaints";
            var pageUrl = "Complaint.aspx?mode=" + mode + "&id=" + id;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientURL" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnWelcomeGreet" runat="server" ClientIDMode="Static" />
    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
    <div class="content-wrapper-before gradient-45deg-indigo-purple"></div>
    <!-- BEGIN: Complaint Section -->
    <asp:UpdatePanel ID="upLeadSection" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divComplaint" style="background: linear-gradient(45deg, silver, #f9e3b1); margin-bottom: 10px; padding-left: 10px; padding-right: 10px;">
                <div class="row">
                    <div class="col m12">
                        <div class="card user-statistics-card mb-1 mt-1">
                            <div class="card-content p-0">
                                <h5 class="h5RoundHead">
                                    <span id="Span1" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Support Filter Data : </span>
                                    <asp:DropDownList ID="drpSuppMonth" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpSuppMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:DropDownList ID="drpSuppYear" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpSuppMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                </h5>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="cardLeadSummary" class="pt-0">
                    <div class="row">
                        <div class="col m6">
                            <div class="card gradient-45deg-red-pink gradient-shadow min-height-100 white-text animate fadeLeft">
                                <div class="padding-4">
                                    <div class="row">
                                        <div class="col m7">
                                            <span class="avatar-status avatar-online avatar-50">
                                                <img src="images/ThumbsDown.png" alt="avatar" width="30" height="30" />
                                            </span>
                                            <p>Open Tickets</p>
                                        </div>
                                        <div class="col m5 right-align">
                                            <h5 class="mb-0 white-text"><span id="spnOpen" runat="server" clientidmode="static">0</span></h5>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col m6">
                            <div class="card gradient-45deg-green-teal gradient-shadow min-height-100 white-text animate fadeRight">
                                <div class="padding-4">
                                    <div class="row">
                                        <div class="col m7">
                                            <span class="avatar-status avatar-online avatar-50">
                                                <img src="images/ThumbsUp.png" alt="avatar" width="30" height="30" />
                                            </span>
                                            <p>Close Tickets</p>
                                        </div>
                                        <div class="col m5 right-align">
                                            <h5 class="mb-0 white-text"><span id="spnClose" runat="server" clientidmode="static">0</span></h5>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Complaint List With Status -->
                <div class="row">
                    <div class="col m6">
                        <!-- User Statistics -->
                        <div class="card user-statistics-card cardRound mb-0">
                            <div class="card-content padding-2" style="min-height: 275px;">
                                <%--<div class="select-action">--%>
                                <div class="card-content p-0">
                                    <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New Sales Target" href="javascript:openDashboardBox('complaint', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                    <h5 class="h5Round">
                                        <span id="Span2" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Complaint Ticket List</span>
                                        <asp:DropDownList ID="drpSuppCategory" runat="server" ClientIDMode="Static" class="select2-theme browser-default mr-2 float-right myDropDownClass" AutoPostBack="true" OnSelectedIndexChanged="drpSuppCategory_SelectedIndexChanged" />
                                    </h5>
                                </div>
                                <div class="user-statistics-container" style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myComplaint runat="server" ID="myComplaint" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col m6">
                        <!-- User Statistics -->
                        <div class="card user-statistics-card cardRound mb-0">
                            <div class="card-content padding-2" style="min-height: 275px;">
                                <div class="card-content p-0">
                                    <a class="material-icons background-round gradient-45deg-red-pink white-text float-left p-0 adddialog" data-position="center" data-tooltip="New Sales Target" href="javascript:openDashboardBox('complaint', 'add', 0);" style="margin: 8px 0 8px 8px;"><i class="material-icons" style="font-size: 20px; padding: 5px;">add</i></a>
                                    <h5 class="h5Round">
                                        <span id="Span3" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">AMC Contract List</span>
                                        <asp:DropDownList ID="drpContractType" runat="server" class="select2-theme browser-default mr-2 float-right myDropDownClass" EnableViewState="true" ClientIDMode="Static" TabIndex="2"  AutoPostBack="true" OnSelectedIndexChanged="drpContractType_SelectedIndexChanged" >
                                            <asp:ListItem Text="-- All --" Value="" />
                                            <asp:ListItem Text="3 Months" Value="3 Months" />
                                            <asp:ListItem Text="6 Months" Value="6 Months" />
                                            <asp:ListItem Text="9 Months" Value="9 Months" />
                                            <asp:ListItem Selected="True" Text="1 Year" Value="1 Year" />
                                            <asp:ListItem Text="2 Years" Value="2 Years" />
                                            <asp:ListItem Text="3 Years" Value="3 Years" />
                                            <asp:ListItem Text="4 Years" Value="4 Years" />
                                            <asp:ListItem Text="5 Years" Value="5 Years" />
                                            <asp:ListItem Text="10 Years" Value="10 Years" />
                                        </asp:DropDownList>
                                    </h5>
                                </div>
                                <div class="user-statistics-container" style="width: 100%; min-height: 350px; max-height: 350px; padding: 2px 10px 0px 10px; overflow-x: hidden; overflow-y: auto;">
                                    <uc1:myAMCContract runat="server" id="myAMCContract" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END: Complaint Section -->
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
        <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 85%; padding: 5px;"></iframe>
        <br />
    </asp:Panel>

    <br />
    <br />
    <br />
    <script type="text/javascript">

        window.onload = function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "Support Management - Analytics & Summary";
        }

        $(document).ready(function () {
            $('.btnTopRightCorner').on("click", function () {
                __doPostBack('', '');
            });
        });

        function capitalizeString(str) {
            return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
        }
        // ------------------------------------------------------------------------
        // Functionality : Registered Complaint Visit View
        // ------------------------------------------------------------------------
        function openComplaintVisit(pkid, complaintno) {
            var pageUrl = "ComplaintVisit.aspx?mode=view&id=" + pkid + "&complaintno=" + complaintno
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        // ------------------------------------------------------------------------
        // Functionality : Complaint Visit
        // ------------------------------------------------------------------------
        function showComplaintVisit(t11, t12, t13) {
            var pageUrl = "ComplaintVisit.aspx?view=complaintvisit&para1=" + t11 + '&para2=' + t12 + '&para3=' + t13;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        function openDashboardBox(view, mode, id) {
            if (view == 'complaint') {
                Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Complaints";
                var pageUrl = "Complaint.aspx?mode=" + mode + "&id=" + id;
                $('#ifrModule').attr('src', pageUrl);
                $find("mpe").show();

            }
        }
    </script>

</asp:Content>
