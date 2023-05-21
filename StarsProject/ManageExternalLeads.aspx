<%@ Page Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="ManageExternalLeads.aspx.cs" Inherits="StarsProject.ManageExternalLeads" %>
<%@ Register Src="~/MyExternalLeads.ascx" TagPrefix="uc1" TagName="MyExternalLeads" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card bg-info-gradient" style="background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
        <div class="card-header no-border" style="padding: 5px 10px;">
            <h5>
                <i class="fa fa-th mr-1"></i><span id="Span1" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Manage External Leads</span>
            </h5>
        </div>
    </div>
    <uc1:MyExternalLeads runat="server" ID="MyExternalLeads" style="width:100%;" />
</asp:Content>

