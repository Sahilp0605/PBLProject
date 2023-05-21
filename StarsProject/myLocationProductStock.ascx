<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myLocationProductStock.ascx.cs" Inherits="StarsProject.myLocationProductStock" %>

<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLocation" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnProduct" runat="server" ClientIDMode="Static" EnableViewState="true" />
<style type="text/css">
    #tblMaterialStatus tr td{ font-size:12px; color:black; }
</style>

<table id="tblLocationSummary" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptLocationSummary" runat="server">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="left-align" style="width:250px;">Product Name</th>
                    <th class="right-align">Opening</th>
                    <th class="right-align">Inward</th>
                    <th class="right-align">Outward</th>
                    <th class="right-align">Closing</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <td class="left-align" style="width:250px;"><%# Eval("ProductName") %></td>
                <td class="right-align"><%# Eval("OpeningSTK") %></td>
                <td class="right-align"><%# Eval("InwardSTK") %></td>
                <td class="right-align"><%# Eval("OutwardSTK") %></td>
                <td class="right-align"><%# Eval("ClosingSTK") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptLocationDetail" runat="server">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="center-align">Module</th>
                    <th class="center-align">Date</th>
                    <th class="center-align">Doc.#</th>
                    <th class="left-align" style="width:250px;">Product Name</th>
                    <th class="right-align">Quantity</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <td class="center-align" style="width:100px;"><%# Eval("Module") %></td>
                <td class="center-align" style="width:100px;"><%# Eval("InvoiceDate", "{0:dd/MM/yyyy}") %></td>
                <td class="center-align"><%# Eval("InvoiceNo") %></td>
                <td class="left-align"><%# Eval("ProductName") %></td>
                <td class="right-align"><%# Eval("Quantity") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>

</table>

