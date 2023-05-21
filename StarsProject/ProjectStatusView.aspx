<%@ Page Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="ProjectStatusView.aspx.cs" Inherits="StarsProject.ProjectStatusView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  
<%@ Register Src="~/myProjectStatus.ascx" TagPrefix="uc1" TagName="myProjectStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
        function viewSalesOrderLog(id) {
            //Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Followup";
            var pageUrl = "SalesOrderLog.aspx?id=" + id;
            $('#ifrModuleLog').attr('src', pageUrl);
            $find("mpeLog").show();
        }

        function openLogAttachment(id, ordno, ordernoid) {
            var pageUrl = "SalesOrderLogAttachment.aspx?id=" + id + "&orderno=" + ordno + "&ordernoid=" + ordernoid;
            $('#ifrModuleLog').attr('src', pageUrl);
            $find("mpeLog").show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server" ClientIDMode="Static" EnablePageMethods="true"></asp:ScriptManager>
    <div class="card bg-info-gradient" style="background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
        <div class="card-header no-border" style="padding: 5px 10px;">
            <h5>
                <i class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Project Status Approval</span>
            </h5>
        </div>
    </div>
    <div class="row">
        <div class="input-field col m3">
            <label class="active" for="drpApprovalStatus">
                <% if (hdnSerialKey.Value == "SOL4-PB94-KY45-TY15")
                    { %>
                        Project Status                                                    
                <% } %>
                <% else 
                    { %>
                        Order Status                                                    
                <% } %>
            </label>
            <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="1" Style="height: inherit; width: 250px;" AutoPostBack="true" OnSelectedIndexChanged="drpApprovalStatus_SelectedIndexChanged" />
        </div>
        <div class="input-field col m3">
            <label class="active" for="drpApprovalStatus">Govt.Project Status</label>
            <asp:DropDownList ID="drpProjectStage" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="1" Style="height: inherit; width:250px;" AutoPostBack="true" OnSelectedIndexChanged="drpProjectStage_SelectedIndexChanged" />
        </div>
    </div>
    <uc1:myProjectStatus runat="server" ID="ucProjectStatus" style="width:100%;" />
    <br /><br />
    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpeLog" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="Panl1" runat="server" ClientIDMode="Static" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
        <div id="myModal">
            <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                    <i class="material-icons prefix">ac_unit</i>
                    <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module.</span>
                    <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" ClientIDMode="Static" Text="" />
                </h5>
            </div>
        </div>
        <iframe id="ifrModuleLog" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 85%; padding: 5px;"></iframe>
        <br />
    </asp:Panel>
    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
</asp:Content>
