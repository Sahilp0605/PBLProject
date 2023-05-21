<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myAssemblyStockProductWise.ascx.cs" Inherits="StarsProject.myAssemblyStockProductWise" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnQuantity" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnFinishProductID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<style type="text/css">
    .table tr td { font-size:14px; font-weight:bold; color:black; vertical-align:text-top; }
    .table tr { border-bottom:2px solid gray; }
    .paginate_button { background-color: deepskyblue !important; color:navajowhite !important; }
    .current { background-color:navy !important; color:white !important; }
</style>

<table id="tblView1" class="table table-striped table-bordered" style="width: 100%;"></table>

