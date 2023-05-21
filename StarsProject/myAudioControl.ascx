<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myAudioControl.ascx.cs" Inherits="StarsProject.myAudioControl" %>
<asp:HiddenField ID="hdnServerPath" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnFilePrefix" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnModule" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnModuleType" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnKeyID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<table id="tblAudioList" class="stdtable width-100" cellpadding="0" cellspacing="0" border="0">
<asp:Repeater ID="rptAudioList" runat="server" ClientIDMode="Static" OnItemCommand="rptAudioList_ItemCommand">
    <HeaderTemplate>
        <tr>
            <th class="center-align">Action</th>
            <th class="center-align">File Name</th>
            <th class="center-align">Upload By</th>
            <th class="center-align">Upload Date</th>
            <th class="center-align">Delete</th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
        <tr>
            <td class="center-align">
                <audio id="tmpFileName" controls><source src='<%# "AudioFiles/" + Eval("FileName") %>' type="audio/aac"></audio>
            </td>
            <td class="center-align"><%# Eval("FileName") %></td>
            <td class="center-align"><%# Eval("CreatedBy") %></td>
            <td class="center-align"><%# Eval("CreatedDate", "{0:dd-MM-yyyy}") %></td>
            <td class="center-align">
                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" Width="20" Height="20" TabIndex="106" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' />
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
</ContentTemplate>
</asp:UpdatePanel>