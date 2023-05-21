<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myPendingMedicalLead.ascx.cs" Inherits="StarsProject.PendingMedicalLead" %>

<%--<link href="css/Registration.css" rel="stylesheet" type="text/css" />
<link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />--%>

<script type="text/javascript">
    function viewPatientLead(id) {
        var pageUrl = "InquiryInfoClinic.aspx?mode=view&id=" + id;
        $.colorbox({
            width: "98%", height: "98%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }

    function showSalesOrderInfo(OrderNo) {
        jQuery.ajax({
            type: "POST",
            url: 'SalesOrderClinic.aspx/GetOrderNoPrimaryID',
            data: '{pOrderNo:\'' + OrderNo + '\'}',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var id;
                var resultdata = JSON.parse(data.d);
                var returnID = parseInt(data.d);
                if (returnID > 0) {
                    $.colorbox({
                        fixed: true, width: "98%", height: "98%", rel: 'showInq', inline: true, iframe: true,
                        href: "SalesOrderClinic.aspx?mode=view&id=" + returnID,
                        onClosed: function () { }
                    });
                }
            },
            error: function (r) { alert('Error : ' + r.responseText); },
            failure: function (r) { alert('failure'); }
        });
    }

    function showSalesBillInfo(BillNo) {
        jQuery.ajax({
            type: "POST",
            url: 'SalesBillClinic.aspx/GetInvoiceNoPrimaryID',
            data: '{pInvoiceNo:\'' + BillNo + '\'}',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var id;
                var resultdata = JSON.parse(data.d);
                var returnID = parseInt(data.d);
                if (returnID > 0) {
                    $.colorbox({
                        fixed: true, width: "98%", height: "98%", rel: 'showInq', inline: true, iframe: true,
                        href: "SalesBillClinic.aspx?mode=view&id=" + returnID,
                        onClosed: function () { }
                    });
                }
            },
            error: function (r) { alert('Error : ' + r.responseText); },
            failure: function (r) { alert('failure'); }
        });
    }

    function viewSalesOrder(id) {
        var pageUrl = "SalesOrder.aspx?mode=view&id=" + id;
        $.colorbox({
            width: "98%", height: "98%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }
    
    function viewSalesInvoice(id) {
        var pageUrl = "SalesBill.aspx?mode=view&id=" + id;
        $.colorbox({
            width: "98%", height: "98%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }

</script>
<style type="text/css">
    table tr th {
        padding: 5px 10px;
        border-bottom: 1px solid #111;
        line-height: 18px;
        font-size: 12px !important;
        font-weight:bold;
    }
    table tr td {
        padding: 5px 10px;
        border-bottom: 1px solid #111;
        line-height: 18px;
        font-size: 12px !important;
    }

</style>
    <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnFilter" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptPatient" runat="server" OnItemDataBound="rptPatient_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="text-center" style="width:100px;">Lead #</th>
                    <th class="text-center">Lead Date</th>
                    <th style="text-align:left !important;width:200px;">Patient Name</th>
                    <th style="text-align:left !important;">Patient Type</th>
                    <th style="text-align:left !important;width:180px;">Hospital/Clinic Name</th>
                    <th style="text-align:left !important;width:150px;">Doctor Name</th>
                    <th style="text-align:right !important;">Final Amount</th>
                    <th style="text-align:right !important;">Patient Payment</th>
                    <th style="text-align:right !important; width:90px;">Proforma Invoice #</th>
                    <th style="text-align:right !important; width:90px;">Invoice #</th>
                    <%--<th style="text-align:right !important;">Billed Amount</th>--%>
                    <%--<th style="text-align:right !important;">Received Payment</th>--%>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnInquiryNo" runat="server" ClientIDMode="Static" Value='<%#Eval("InquiryNo") %>' />
                <asp:HiddenField ID="hdnInquiryDate" runat="server" ClientIDMode="Static" Value='<%#Eval("InquiryDate") %>' />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%#Eval("CustomerID") %>' />
                <asp:HiddenField ID="hdnDoctorID" runat="server" ClientIDMode="Static" Value='<%#Eval("DoctorID") %>' />
                <td class="text-center">
                    <a href="javascript:viewPatientLead(<%# Eval("InquirypkID")%>);"><%# Eval("InquiryNo") %></a>
                </td>
                <td class="text-center"><%# Eval("InquiryDate", "{0:dd/MM/yyyy}") %></td>
                <td style="text-align:left !important;"><%# Eval("CustomerName") %></td>
                <td style="text-align:left !important;"><%# Eval("TreatmentType") %></td>
                <td style="text-align:left !important;"><%# Eval("HospitalName") %></td>
                <td style="text-align:left !important;"><%# Eval("DoctorName") %></td>
                <td style="text-align:right !important;"><%# String.Format("{0:0.00}", Convert.ToDecimal(Eval("FinalAmount"))) %></td>
                <td style="text-align:right !important;"><%# String.Format("{0:0.00}", Convert.ToDecimal(Eval("PatientPaid"))) %></td>
                <td class="text-center">
                    <a tooltip="Click to Show Proforma" href="javascript:showSalesOrderInfo('<%# Eval("OrderNo") %>');" ><%# Eval("OrderNo") %></a> 
<%--                    <a href="javascript:viewSalesOrder(<%# Eval("OrderNo")%>);"><%# Eval("OrderNo") %></a>--%>
                </td>
                <td class="text-center">
                    <a tooltip="Click to Show Sales Invoice" href="javascript:showSalesBillInfo('<%# Eval("BillNo") %>');"><%# Eval("BillNo") %></a> 
                    <%--<a href="javascript:viewSalesInvoice(<%# Eval("BillNo")%>);"><%# Eval("BillNo") %></a>--%>
                </td>
<%--                <td style="text-align:right !important;"><%# String.Format("{0:0.00}", Convert.ToDecimal(Eval("BilledAmount"))) %></td>
                <td style="text-align:right !important;"><%# String.Format("{0:0.00}", Convert.ToDecimal(Eval("ReceivedAmount")))%></td>--%>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>

