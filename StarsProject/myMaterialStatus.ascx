<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myMaterialStatus.ascx.cs" Inherits="StarsProject.myMaterialStatus" %>

<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnViewType" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" EnableViewState="true" />

<style type="text/css">
    #tblMaterialStatus tr td{ font-size:12px; color:black; }
</style>
<script>
    function ShowPDFfile(repFilename) {

        var today = new Date();
        var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
        var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        var dateTime = date + ' ' + time;

        yhooWin = window.open(repFilename + "?id=" + dateTime, "ywin", "width=1050,height=750");
        yhooWin.focus();
    }
    function OpenPDF(id,OrderNo)
    {
        var SOPageUrl = '';
        SOPageUrl = 'PurchaseOrders.aspx/GenerateDynamicRemainingQuantityPDF';
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
            swal({
                title: "Purc.Order Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
            }).then(function (willDelete) {
                if (willDelete) {
                    var x = PageMethods.setPrintHeader('yes');
                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\',OrderNo:\'' + OrderNo + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });

                }
                else {
                    var x = PageMethods.setPrintHeader('no');

                    jQuery.ajax({
                        type: "POST",
                        url: SOPageUrl,
                        data: '{pkID:\'' + id + '\',OrderNo:\'' + OrderNo + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            // -----------------------------------------------------------
                            jQuery.ajax({
                                type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                contentType: "application/json; charset=utf-8", success: function (data11) {
                                    OrderNoForPDF = (data11.d).replace("/", "-");
                                    if (OrderNoForPDF != "")
                                        ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                    else
                                        alert('Purchase Order PDF Not Found !')
                                }
                            });
                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });

                }
            });
        }
        else {
            jQuery.ajax({
                type: "POST",
                url: SOPageUrl,
                data: '{pkID:\'' + id + '\',OrderNo:\'' + OrderNo + '\'}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    // -----------------------------------------------------------
                    jQuery.ajax({
                        type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                        contentType: "application/json; charset=utf-8", success: function (data11) {
                            OrderNoForPDF = (data11.d).replace("/", "-");
                            if (OrderNoForPDF != "")
                                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                            else
                                alert('Purchase Order PDF Not Found !')
                        }
                    });
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });
        }
    }
</script>
<table id="tblMaterialStatus" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
<asp:Repeater ID="rptMaterialStatus" runat="server" OnItemDataBound="rptMaterialStatus_ItemDataBound">
    <HeaderTemplate>
        <thead>
            <tr>
                <% if (hdnView.Value == "purchase") { %>
                    <th id="headOrd1" runat="server" class="left-align">Order #</th>
                <% if (hdnSerialKey.Value == "DYNA-2GF3-J7G8-FF12") { %>
                    <th id="headfpdf" runat="server" class="left-align">#</th>
                <% } %>
                <% } 
                else { %>
                    <th id="headOrd2" runat="server" class="left-align">Order #</th>
                <% } %>
                <th class="left-align" style="width:250px;">Product Name</th>
                <th class="center-align">Order Qty</th>
                <th id="headINOUT" runat="server" class="center-align">Dispatch Qty</th>
                <th class="center-align">Pending Qty</th>
                <% if (hdnViewType.Value.ToLower() == "detail") { %>
                <th class="center-align">Approval</th>
                <% } %>
                <th class="center-align">Status</th>
            </tr>
        </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="blueShed">
            <asp:HiddenField ID="hdnApprovalStatus" runat="server" ClientIDMode="Static" EnableViewState="true" Value='<%# Eval("ApprovalStatus") %>' />
            <asp:HiddenField ID="hdnRequestStatus" runat="server" ClientIDMode="Static" EnableViewState="true" Value='<%# Eval("RequestStatus") %>' />
            <asp:HiddenField ID="hdnDeliveryStatus" runat="server" ClientIDMode="Static" EnableViewState="true" Value='<%# Eval("DeliveryStatus") %>' />

            <% if (hdnView.Value == "purchase") { %>
                <td id="itemOrd1" runat="server" class="left-align">
                    <a id="lnkSearch1" data-position="center" href="javascript:showOrderForm('purchase' ,'<%# Eval("pkID") %>');"><%# Eval("OrderNo") %></a>
                </td>
                <% if (hdnSerialKey.Value == "DYNA-2GF3-J7G8-FF12") { %>
                <td id="itempdf" runat="server" class="left-align">
                    <img src="images/pdf_document.png" alt="" style="width:15px; height:15px;" onclick="OpenPDF('<%# Eval("pkID") %>','<%# Eval("OrderNo") %>');"/>
                </td>
                <% } %>
            <% } 
            else { %>
                <td id="itemOrd2" runat="server" class="left-align">
                    <a id="lnkSearch2" data-position="center" href="javascript:showOrderForm('sale' ,'<%# Eval("pkID") %>');"><%# Eval("OrderNo") %></a>
                </td>
            <% } %>

            <% if (hdnViewType.Value.ToLower() == "detail") { %>
                <td class="left-align" style="width:250px;"><b class="blue-text"><%# Eval("CustomerName") %></b><br /><%# Eval("ProductNameLong") %></td>
            <% } 
            else { %>
                <td class="left-align" style="width:250px;"><%# Eval("ProductNameLong") %></td>
            <% } %>
            <td class="center-align"><%# Eval("OrderQty") %></td>
            <td class="center-align"><%# Eval("DispatchQty") %></td>
            <td class="center-align"><%# Eval("PendingQty") %></td>   
            <% if (hdnViewType.Value.ToLower() == "detail") { %>
            <td id="tdApproval" runat="server" class="center-align">
                <i class="material-icons" style="font-size: 20px; padding: 5px;">add</i>
            </td>   
            <% } %>      
            <td class="center-align">
                <asp:Label ID="tdStatus" runat="server" style="padding: 4px 4px 4px 8px !important;border-radius: 5px;"><%# Eval("RequestStatus") %></asp:Label>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
