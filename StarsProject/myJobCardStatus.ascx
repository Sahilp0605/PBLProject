<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myJobCardStatus.ascx.cs" Inherits="StarsProject.myJobCardStatus" %>

<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnViewType" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />

<style type="text/css">
    #tblJobCardStatus tr td{ font-size:12px; color:black; }
</style>

<table id="tblJobCardStatus" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
<asp:Repeater ID="rptJobCardStatus" runat="server" OnItemDataBound="rptJobCardStatus_ItemDataBound">
    <HeaderTemplate>
        <thead>
            <tr>
                <th id="headOrd1" runat="server" class="center-align">Outward #</th>
                <th id="headOrd2" runat="server" class="center-align">Outward Date</th>
                <th id="headOrd3" runat="server" class="left-align" style="width:300px;">Customer Name</th>
                <th class="left-align" style="width:300px;">Product Name</th>
                <th class="center-align">Outward Qty</th>
                <th id="headINOUT" runat="server" class="center-align">Inward Qty</th>
                <th class="center-align">Status</th>
            </tr>
        </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="blueShed">
            <asp:HiddenField ID="hdnStatus" runat="server" ClientIDMode="Static" EnableViewState="true" Value='<%# Eval("Status") %>' />
            <td id="itemOrd1" runat="server" class="center-align">
                <a id="lnkSearch1" data-position="center" href="javascript:showOrderForm('jobcardoutward' ,'<%# Eval("pkID") %>');"><%# Eval("OutwardNo") %></a>
            </td>
            <td id="itemOrd2" runat="server" class="center-align"><%# Eval("OutwardDate", "{0:dd/MM/yyyy}") %></td>
            <td id="itemOrd3" runat="server" class="left-align" style="width:300px;"><%# Eval("CustomerName") %></td>
            <td class="left-align" style="width:300px;"><%# Eval("ProductNameLong") %></td>
            <td class="center-align"><%# Eval("OutwardQty") %></td>
            <td class="center-align"><%# Eval("InwardQty") %></td>
            <td class="center-align">
                <asp:Label ID="tdStatus" runat="server" Text= <%# Eval("Status") %> style="padding: 4px 4px 4px 8px !important;border-radius: 5px;">  </asp:Label>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
