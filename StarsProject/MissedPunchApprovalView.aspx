<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="MissedPunchApprovalView.aspx.cs" Inherits="StarsProject.MissedPunchApproval" %>
<%@ Register Src="~/myMissedPunch.ascx" TagPrefix="uc1" TagName="myMissedPunch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .select-wrapper input.select-dropdown {
            height: 2rem !important; 
            width: 200px; 
            border: 2px solid navy !important; 
            background-color: antiquewhite !important;
            margin:10px 40px 0px 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server" ClientIDMode="Static" EnablePageMethods="true"></asp:ScriptManager>
    <div class="modal-body float-right" id="divApprovalStatus" runat="server" clientidmode="Static">
        <div class="row">
            <asp:DropDownList ID="drpApprovalStatusMain" runat="server" ClientIDMode="Static" Class="form-control" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="drpApprovalStatusMain_SelectedIndexChanged">
                <asp:ListItem Text="-- All --" Value="" />
                <asp:ListItem Text="Pending" Value="Pending" />
                <asp:ListItem Text="Approved" Value="Approved" />
                <asp:ListItem Text="Rejected" Value="Rejected" />
            </asp:DropDownList>
        </div>
    </div>
    <div class="card bg-info-gradient" style="background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
        <div class="card-header no-border" style="padding: 5px 10px;">
            <h5>
                <i class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Missed Punch Approval</span>
            </h5>
        </div>
    </div>
    <uc1:myMissedPunch runat="server" ID="myMissedPunch" style="width:100%;" />
    <br /><br /><br /><br />
</asp:Content>


