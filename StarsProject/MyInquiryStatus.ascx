<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyInquiryStatus.ascx.cs" Inherits="StarsProject.MyInquiryStatus" %>

<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnInquiryStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />

<%--<link href="css/Registration.css" rel="stylesheet" type="text/css" />
<link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />--%>

<script type="text/javascript">

    function viewInquiry(id) {
        var pageUrl = "InquiryInfo.aspx?mode=view&id=" + id;
        $.colorbox({
            width: "90%", height: "90%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }

    function openQuotationEntry(inqNo, CustomerId) {
        var pageUrl = "Quotation.aspx?mode=add&id=0&InquiryNo=" + inqNo + "&CustomerId=" + CustomerId;
        $.colorbox({
            width: "90%", height: "90%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }

    function openSalesOrderEntry(inqNo, CustomerId) {
        var pageUrl = "SalesOrder.aspx?mode=add&id=0&InquiryNo=" + inqNo + "&CustomerId=" + CustomerId;
        $.colorbox({
            width: "90%", height: "90%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }

    function openSalesBillEntry(inqNo, CustomerId) {
        var pageUrl = "SalesBill.aspx?mode=add&id=0&InquiryNo=" + inqNo + "&CustomerId=" + CustomerId;
        $.colorbox({
            width: "90%", height: "90%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }
    

</script>
    <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptInquiry" runat="server" OnItemDataBound="rptInquiry_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="text-center">Inquiry #</th>
                    <th class="text-center">Inquiry Date</th>
                    <th style="text-align:left !important;">Customer Name</th>
                    <th style="text-align:left !important;">Sales Person</th>
                     <% if (strInquiryStatus == "pending") {%>  
                        <th class="text-right">Generate Quotation</th>
                        <th class="text-right">Generate Order</th>
                        <th class="text-right">Generate Bill</th>
                    <%} %>
                    <% if (strInquiryStatus == "quotationgenerated"){%>        
                        <th class="text-right">Quotation No</th>
                    <%} %>
                    <% if (strInquiryStatus == "salesordergenerated"){%>   
                        <th class="text-right">Sales Order No</th>
                    <%} %>
                    <% if (strInquiryStatus == "salesbillgenerated"){%>   
                        <th class="text-right">Sales Bill No</th>
                    <%} %>

                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />                
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%#Eval("CustomerID") %>' />                
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                <asp:HiddenField ID="hdnCreatedBy" runat="server" ClientIDMode="Static" Value='<%#Eval("CreatedBy") %>' />
                <td class="text-center">
                    <a href="javascript:viewInquiry(<%# Eval("pkID")%>);"><%# Eval("InquiryNo") %></a>
                </td>
                <td class="text-center"><%# Eval("InquiryDate", "{0:dd/MM/yyyy}") %></td>
                <td style="text-align:left !important;"><%# Eval("CustomerName") %></td>
                <td style="text-align:left !important;"><%# Eval("EmployeeName") %></td>
                <% if (strInquiryStatus == "pending") {%>                  
                    <td class="text-center">
                        <a id="lnkQuotation" href="javascript:openQuotationEntry('<%# Eval("InquiryNo") %>','<%# Eval("CustomerID") %>');" tabindex="19"><medium>Generate Quotation</medium></a>
                    </td>
                    <td class="text-center" >
                        <a id="lnkSalesOrder" href="javascript:openSalesOrderEntry('<%# Eval("InquiryNo") %>','<%# Eval("CustomerID") %>');" tabindex="19"><medium>Generate Sales Order</medium></a>
                    </td>
                    <td class="text-center" >
                        <a id="lnkSalesBill" href="javascript:openSalesBillEntry('<%# Eval("InquiryNo") %>','<%# Eval("CustomerID") %>');" tabindex="19"><medium>Generate Sales Bill</medium></a>
                    </td>
                <%} %>
                <% if (strInquiryStatus == "quotationgenerated"){%>               
                    <td class="text-center"><%# Eval("QuotationNo") %></td>
                <%} %>
                <% if (strInquiryStatus == "salesordergenerated"){%>  
                    <td class="text-center"><%# Eval("OrderNo") %></td>             
                <%} %>
                <% if (strInquiryStatus == "salesbillgenerated"){%>   
                    <td class="text-center"><%# Eval("BillNo") %></td>
                <%} %>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>
<%--    <div class="modal-footer">
        <asp:Button ID="btnApproveReject" runat="server" ClientIDMode="Static" class="btn btn-primary" Text="Submit Order Status" OnClick="btnApproveReject_Click" />
    </div>--%>

