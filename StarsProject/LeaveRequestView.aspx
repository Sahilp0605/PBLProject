<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="LeaveRequestView.aspx.cs" Inherits="StarsProject.LeaveRequestView" %>
<%@ Register Src="~/myLeaveRequest.ascx" TagPrefix="uc1" TagName="myLeaveRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card bg-info-gradient" style="background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
        <div class="card-header no-border" style="padding: 5px 10px;">
            <h5>
                <i class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Leave Request Approval</span>
            </h5>
        </div>
    </div>
    <uc1:myLeaveRequest runat="server" ID="myLeaveRequest" style="width:100%;" />
</asp:Content>
