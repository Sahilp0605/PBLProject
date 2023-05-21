<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myProjectStatus.ascx.cs" Inherits="StarsProject.myProjectStatus" %>

<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnClientUrl" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdn_ApprovalStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdn_ProjectStage" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" EnableViewState="true"/>

<table id="tblInqProductGroup" class="table striped" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptApproval" runat="server" OnItemDataBound="rptApproval_ItemDataBound" OnItemCommand="rptApproval_ItemCommand">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th class="center-align">Order #<br />Date</th>
                    <th class="left-align">Customer Name<br />Sales Executive</th>
                    <% if (hdnSerialKey.Value == "SOL4-PB94-KY45-TY15")
                    { %>
                        <th class="center-align" style="width:300px;">Project Status</th>
                <% } %>
                <% else 
                    { %>
                        <th class="center-align" style="width:300px;">Order Status</th>
                <% } %>
                    
                    <th class="center-align" style="width:400px;">Government Project Stages</th>
                    <th class="center-align" style="width:125px;">Submit Log</th>
                    <th class="center-align" style="width:190px;">Log & Attach</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="blueShed">
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                <asp:HiddenField ID="hdnOrderNo" runat="server" ClientIDMode="Static" Value='<%#Eval("OrderNo") %>' />
                <asp:HiddenField ID="hdnApprovalStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("ApprovalStatus") %>' />
                <asp:HiddenField ID="hdnProjectStage" runat="server" ClientIDMode="Static" Value='<%#Eval("ProjectStage") %>' />
                <asp:HiddenField ID="hdnStatusRemarks" runat="server" ClientIDMode="Static" Value='<%#Eval("StatusRemarks") %>' />
                <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                <asp:HiddenField ID="hdnCreatedBy" runat="server" ClientIDMode="Static" Value='<%#Eval("CreatedBy") %>' />
                <asp:HiddenField ID="hdnIssuedBy" runat="server" ClientIDMode="Static" Value='<%#Eval("IssuedBy") %>' />
                <td class="center-align">
                    <a href="javascript:viewSalesOrderLog('<%# Eval("OrderNo")%>');"><%# Eval("OrderNo") %></a>
                </td>
                <td class="left-align"><%# Eval("CustomerName") %></td>
                <td class="center-align">
                    <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="1" Style="height: 1.70rem; margin: 0px; padding: 0px 0px 0px 10px;" />
                </td>
                <td class="center-align">
                    <asp:DropDownList ID="drpProjectStage" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="1" Style="height: 1.70rem; margin: 0px; padding: 0px 0px 0px 10px;" />
                </td>
                 <td>
                    <asp:Button ID="btnSubmitLog" runat="server" ClientIDMode="Static" class="btn btn-primary float-right" Text="Submit" CommandName="SaveLog" CommandArgument='<%# Eval("pkID") %>'/>
                </td>
                 <td>
                    <a class="btn btn-primary float-right" href="javascript:openLogAttachment(0, '<%# Eval("OrderNo")%>', '<%# Eval("pkID")%>');">Log & Atttachment</a>
                </td>
            </tr>
            <tr style="border-bottom:4px solid lightgray;">
                <td class="center-align" style="vertical-align: text-top;"><%# Eval("OrderDate", "{0:dd/MM/yyyy}") %></td>
                <td class="left-align" style="vertical-align: text-top;"><%# Eval("EmployeeName") %></td>
                <td colspan="2">
                    <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" TextMode="MultiLine" Style="min-height: 50px !important; height: 50px !important;" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>


