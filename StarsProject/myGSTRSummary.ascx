<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myGSTRSummary.ascx.cs" Inherits="StarsProject.myGSTRSummary" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<table id="tblGSTR" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
<asp:Repeater ID="rptGSTR" runat="server" OnItemCommand="rptGSTR_ItemCommand" OnItemDataBound="rptGSTR_ItemDataBound">
    <HeaderTemplate>
        <thead>
            <tr>
                <th class="left-align">Particulars</th>
                <th class="center-align">Voucher Count</th>
                <th class="right-align">Taxable Amount</th>
                <th class="right-align">Integrated Tax Amount</th>
                <th class="right-align">Central Tax Amount</th>
                <th class="right-align">State Tax Amount</th>
                <th class="right-align">Total Tax Amount</th>
                <th class="right-align">Invoice Amount</th>
            </tr>
        </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:HiddenField ID="hdnModule" runat="server" ClientIDMode="Static" Value='<%# Eval("Module") %>' />
        <tr id='trDataRow' runat="server" class="blueShed">
            <td class="left-align" style="color:blue;"><%# Eval("Description") %></td>
            <td class="center-align">
                <asp:Button CssClass="background-round gradient-45deg-red-pink white-text p-0 tooltipped" ID="btnDelete" runat="server" Width="20" Height="20" TabIndex="106" data-position="left" data-tooltip="Click To Show/Hide Detail !" CommandName="gstrDetail" CommandArgument='<%# Eval("Module") %>' Text='<%# Eval("NOE") %>' style="height:30px; width:30px; border:none;" />
            </td>
            <td class="right-align" style="color:navy;"><%# Eval("BasicAmount") %></td>
            <td class="right-align"><%# Eval("IGSTAmt") %></td>
            <td class="right-align"><%# Eval("CGSTAmt") %></td>
            <td class="right-align"><%# Eval("SGSTAmt") %></td>
            <td class="right-align" style="color:maroon;"><span id="spnTax" runat="server" clientidmode="static" /><%# Eval("TaxAmt") %></td>
            <td class="right-align" style="color:navy;"><span id="spnInvoice" runat="server" clientidmode="static" /><%# Eval("InvoiceAmt") %></td>
        </tr>
        <tr id="trDetail" runat="server" clientidmode="static" visible="false">
            <td colspan="8">
                <table id="tblGSTR" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%" style="height:50px; overflow-y:auto; border:3px solid; background-color:beige;">
                    <asp:Repeater ID="rptGSTRDetail" runat="server" ClientIDMode="Static">
                        <HeaderTemplate>
                            <thead>
                                <tr style="font-size:12px;">
                                    <th class="center-align" style="color: white !important;background-color: black !important;">Date</th>
                                    <th class="left-align" style="color: white !important;background-color: black !important;">Particulars</th>
                                    <th class="center-align" style="color: white !important;background-color: black !important;">Invoice #</th>
                                    <th class="right-align" style="color: white !important;background-color: black !important;">Taxable Amount</th>
                                    <th class="right-align" style="color: white !important;background-color: black !important;">Integrated Tax Amount</th>
                                    <th class="right-align" style="color: white !important;background-color: black !important;">Central Tax Amount</th>
                                    <th class="right-align" style="color: white !important;background-color: black !important;">State Tax Amount</th>
                                    <th class="right-align" style="color: white !important;background-color: black !important;">Total Tax Amount</th>
                                    <th class="right-align" style="color: white !important;background-color: black !important;">Invoice Amount</th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="blueShed" style="font-size:12px;">
                                <td class="center-align" style="color:blue;"><%# Eval("InvoiceDate", "{0:dd-MM-yyyy}") %></td>
                                <td class="left-align" style="color:blue;"><%# Eval("Description") %></td>
                                <td class="center-align" style="color:navy;"><%# Eval("InvoiceNo") %></td>
                                <td class="right-align" style="color:navy;"><%# Eval("BasicAmount") %></td>
                                <td class="right-align"><%# Eval("IGSTAmt") %></td>
                                <td class="right-align"><%# Eval("CGSTAmt") %></td>
                                <td class="right-align"><%# Eval("SGSTAmt") %></td>
                                <td class="right-align" style="color:maroon;"><span id="spnTax" runat="server" clientidmode="static" /><%# Eval("TaxAmt") %></td>
                                <td class="right-align" style="color:navy;"><span id="spnInvoice" runat="server" clientidmode="static" /><%# Eval("InvoiceAmt") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
