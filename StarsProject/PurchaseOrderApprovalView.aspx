<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/StarsSite.Master" CodeBehind="PurchaseOrderApprovalView.aspx.cs" Inherits="StarsProject.PurchaseOrderApprovalView" %>

<%@ Register Src="~/myPurchaseApproval.ascx" TagPrefix="uc1" TagName="myPurchaseApproval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server" ClientIDMode="Static" EnablePageMethods="true"></asp:ScriptManager>
    <div class="card bg-info-gradient" style="background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
        <div class="card-header no-border" style="padding: 5px 10px;">
            <h5>
                <i class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Purchase Order Approval</span>
            </h5>
        </div>
    </div>
    <uc1:myPurchaseApproval runat="server" ID="myPurchaseApproval" style="width:100%;" />
    <br /><br /><br /><br />
</asp:Content>
