<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myMinStockLevel.ascx.cs" Inherits="StarsProject.myMinStockLevel" %>

<table id="tblClientLedger" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
<asp:Repeater ID="rptMinStock" runat="server">
    <HeaderTemplate>
        <thead>
            <tr>
                <th class="left-align">Product Name</th>
                <th class="left-align">Product Alias</th>
                <th class="center-align">Min.Qty Level</th>
                <th class="center-align">Closing Stock</th>
            </tr>
        </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="blueShed">
            <td class="left-align"><%# Eval("ProductName") %></td>
            <td class="left-align"><%# Eval("ProductAlias") %></td>
            <td class="center-align"><%# Eval("MinQuantity") %></td>
            <td class="center-align"><%# Eval("ClosingSTK") %></td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
