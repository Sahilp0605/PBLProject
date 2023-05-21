<%@ Page Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true"  CodeBehind="SendWaMsg.aspx.cs" Inherits="StarsProject.SendWaMsg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div>
        <asp:TextBox ID="txtMessage" runat="server" ClientIDMode="Static" />
        <asp:Button ID="btnSendMessage" runat="server" ClientIDMode="Static" Text="Send Message" OnClick="btnSendMessage_Click" />
    </div>
    <div>
        Whatsapp Message
        <div>
            <asp:TextBox ID="txtWaMessage" runat="server" ClientIDMode="Static" />
            <asp:Button ID="btnSendWaMessage" runat="server" ClientIDMode="Static" Text="Send Message"/>
        </div>
        <div>
            <asp:Button ID="btnSendWaDoc" runat="server" ClientIDMode="Static" Text="Send Message"/>
        </div>
    </div>
</asp:Content>
