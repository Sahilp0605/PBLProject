<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myTaxSummary.ascx.cs" Inherits="StarsProject.myTaxSummary" %>
<asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnDocumentNo" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnModule" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnKeyID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnHSNFlag" runat="server" ClientIDMode="Static" Value="0" />
<style type="text/css">
    th { background-color: inherit; color:white; font-size:14px; }
    td { font-size:14px; }
</style>

<table id="tblLeaveBalances" class="table table-striped table-bordered mt-2 mb-2" style="font-size:12px;" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptBalances" runat="server">
        <HeaderTemplate>
            <thead class="thead-dark" style="font-size:12px;">
                <tr style="background: linear-gradient(45deg, #3949ab, #4fc3f7) !important;">
                    <% if (hdnHSNFlag.Value == "1" || hdnHSNFlag.Value == "true")
                        { %>
                    <th class="center-align white-text navy">HSN Code</th>
                    <% } %>
                    <th class="center-align white-text navy">TAX %</th>
                    <th class="center-align white-text navy">CGST</th>
                    <th class="center-align white-text navy">SGST</th>
                    <th class="center-align white-text navy">IGST</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <% if (hdnHSNFlag.Value == "1" || hdnHSNFlag.Value == "true")
                    {%>
                <td class="center-align"><%# Eval("HSNCode") %></td>
                <% } %>
                <td class="center-align"><%# Eval("TaxPer") %></td>
                <td class="center-align"><%# Eval("CGSTAmt") %></td>
                <td class="center-align"><%# Eval("SGSTAmt") %></td>
                <td class="center-align"><%# Eval("IGSTAmt") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>