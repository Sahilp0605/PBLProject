<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myProductLedger.ascx.cs" Inherits="StarsProject.myProductLedger" %>
<asp:HiddenField ID="hdn_ProjectID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdn_LocationID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdn_LocationFlag" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdn_ProductID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdn_Opening" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
<asp:HiddenField ID="hdn_Inward" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
<asp:HiddenField ID="hdn_Outward" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
<asp:HiddenField ID="hdn_Closing" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
<table id="tblInwardOutwardLedger" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
    <asp:Repeater ID="rptProductLedger" runat="server" OnItemDataBound="rptProductLedger_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr style="font-size: 12px !important;">
                    <th class="center-align">Date</th>
                    <th class="center-align">Voucher #</th>
                    <th class="left-align">Customer/Supplier Name</th>
                    <th class="center-align">Module</th>
                    <th class="right-align">&nbsp;</th>
                    <th class="right-align">Inward</th>
                    <th class="right-align">Outward</th>
                    <th class="right-align">Closing</th>
                </tr>
                <tr style="background-color:#e4e4e4 !important; color:black !important; font-size:14px;">
                    <th class="right-align" colspan="4" style="background-color:#e4e4e4 !important; color:black !important;">Opening Stock : </th>
                    <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;">&nbsp;</th>
                    <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdHeaderInward" runat="server">&nbsp;</th>
                    <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdHeaderOutward" runat="server">&nbsp;</th>
                    <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important;" id="tdHeaderClosing" runat="server">&nbsp;</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed" style="background-color: white; font-size: 12px !important;">
                <asp:HiddenField ID="hdnTransType" runat="server" ClientIDMode="Static" Value='<%#Eval("TransType") %>' />
                <asp:HiddenField ID="hdnQuantity" runat="server" ClientIDMode="Static" Value='<%#Eval("Quantity") %>' />
                <td class="center-align"><%# Eval("InvoiceDate", "{0:dd-MMM-yyyy}") %></td>
                <td class="center-align"><%# Eval("InvoiceNo") %></td>
                <td class="left-align"><%# Eval("CustomerName") %></td>
                <td class="center-align"><%# Eval("Module") %></td>
                <td class="right-align">&nbsp;</td>
                <td id="tdInwardQty" runat="server" class="right-align" style="font-size:14px;"></td>
                <td id="tdOutwardQty" runat="server" class="right-align" style="font-size:14px;"></td>
                <td id="tdClosing" runat="server" class="right-align" style="font-weight: bold; font-size:14px;"></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr style="font-size:14px;">
                <th class="right-align" colspan="4" style="background-color:#e4e4e4 !important; color:black !important;">Closing Stock : </th>
                <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important; font-size:14px;">&nbsp;</th>
                <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important; font-size:14px;" id="tdFooterInward" runat="server">&nbsp;</th>
                <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important; font-size:14px;" id="tdFooterOutward" runat="server">&nbsp;</th>
                <th class="right-align" style="background-color:#e4e4e4 !important; color:black !important; font-size:14px;" id="tdFooterClosing" runat="server">&nbsp;</th>
            </tr>
        </FooterTemplate>
    </asp:Repeater>
</table>

