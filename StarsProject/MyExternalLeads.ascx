<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyExternalLeads.ascx.cs" Inherits="StarsProject.MyExternalLeads" %>

<%--<link href="css/Registration.css" rel="stylesheet" type="text/css" />
<link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />--%>

<script type="text/javascript">
    function viewExternalLeads(id) {        
        var pageUrl = "ExternalLeads.aspx?mode=view&id=" + id;
        $.colorbox({
            width: "90%", height: "90%", iframe: true, href: pageUrl, onClosed: function () { }
        });
    }
</script>
    <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">

    <asp:Repeater ID="rptExternalLeads" runat="server" OnItemDataBound="rptExternalLeads_ItemDataBound">
        <HeaderTemplate>
            <thead>
                <tr>
                    <%--<th class="text-center">Select</th>--%>
                    <th class="text-center">Lead No.</th>
                    <th class="text-center">Lead Date</th>
                    <th style="text-align:left !important;">Sender Name</th>
                    <th style="text-align:left !important;">Company Name</th>
                    <th style="text-align:left !important;">For Product</th>
                    <th style="text-align:left !important;">Detail</th>
                    <th style="text-align:center !important;">Lead Source</th>
                    <th style="text-align:center !important;">Inquiry #</th>
                    <%--<th class="text-left">Assign To</th>--%>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">                
                
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                <asp:HiddenField ID="hdnAssignTo" runat="server" ClientIDMode="Static" Value='<%# Eval("EmployeeId") %>' />
                <td class="text-center">
                    <a href="javascript:viewExternalLeads(<%# Eval("pkID")%>);"><%# Eval("pkID") %></a>                    
                </td>
                <td class="text-center"><%# Eval("QueryDatetime", "{0:dd/MM/yyyy}") %></td>
                <td style="text-align:left !important;"><%# Eval("SenderName") %></td>
                <td style="text-align:left !important;"><%# Eval("CompanyName") %></td>
                <td style="text-align:left !important;"><%# Eval("ForProduct") %></td>
                <td style="text-align:left !important;"><%# Eval("Message") %></td>
                <td style="text-align:center !important;"><%# Eval("LeadSource") %></td>
                <td style="text-align:center !important;"><%# Eval("InquiryNo") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    </table>
    <div class="modal-footer">
        <asp:Button ID="btnGenerateInquiry" runat="server" ClientIDMode="Static" class="btn btn-primary" Text="Generate Inquiry" OnClick="btnGenerateInquiry_Click" />
    </div>

