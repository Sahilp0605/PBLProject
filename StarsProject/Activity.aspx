<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="Activity.aspx.cs" Inherits="StarsProject.Activity" %>
<%@ Register Src="~/PaggingUserControl.ascx" TagName="page" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .grid-cntNav {height: auto; left: 5px; margin: 10px 0px; padding: 0; top: 0; width: auto;}
        .flRight {float: right;}
        .pagging {color: #949494; float: right; height: 20px; line-height: 19px; padding: 0 10px;}
        .pagging a {background: none repeat scroll 0 0 #DDE0E8; border: 1px solid #D5D5D5; color: #949494; float: left; height: 20px; margin-left: 5px; padding: 0 8px; text-decoration: none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pageheader">
        <h1 class="pagetitle contenttitle2">Manage Activity</h1>
    </div>
    <asp:HiddenField ID="hdnOldUserID" runat="server" />
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">
         
        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="widgetbox">
            <div class="widgetcontent padding0 statement">
                <table class="stdtable" cellpadding="0" cellspacing="0" border="0">
                    <asp:Repeater ID="rptUser" runat="server" onitemcommand="rptUser_ItemCommand">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th>Activity Code</th>
                                    <th>Activity Name</th>
                                    <th>Active Status</th>
                                    <th style="text-align:center; width:4%;">Edit</th>
                                    <th style="text-align:center; width:4%;">Delete</th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ActivityCode") %></td>
                                <td><%# Eval("ActivityName") %></td>
                                <td><%# Eval("ActiveFlag") %></td>
                                <td style="text-align:center;"><asp:ImageButton ID="ImgbtnEdit" runat="server" ImageUrl="~/images/edit.png" Width="16" Height="16" CommandName="Edit" CommandArgument='<%# Eval("ActivityCode") %>' /></td>
                                <td style="text-align:center;"><asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" CommandName="Delete" CommandArgument='<%# Eval("ActivityCode") %>' /></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <uc1:page ID="pageGrid" runat="server" Visible="true" OnPagerChanged="Pager_Changed"/>
            </div>
        </div>

            <div id="basicform" class="subcontent">
            <div class="clearall"></div>
            <div class="leftdiv50">
                <p class="bordeelftnn">
                    <label>Activity Code</label>
                    <span class="field rghtbrdr" id="marwdthlow">
                        <asp:TextBox ID="txtActivityCode" runat="server" class="shortinptfld" MaxLength="20" TabIndex="1"></asp:TextBox>
                    </span>
                </p>
                <p class="bordeelftnn">
                    <label>Short Desc.</label>
                    <span class="field rghtbrdr" id="marwdthlow">
                        <asp:TextBox ID="txtActivityName" runat="server" class="shortinptfld" MaxLength="50" TabIndex="2"></asp:TextBox>
                    </span>
                </p>
            </div>
            <div class="rightdiv50">
                <p style="border-left: none; height:53px;">
                    <label style="height:43px;">Active</label>
                    <span class="field rghtbrdr" id="marwdthlow" style="height:43px;">
                        <asp:CheckBox ID="chkActive" runat="server" style="margin:10px 0px;"  TabIndex="3"/>
                    </span>
                </p>
            </div>
            <div class="clearall"></div>
            <div>
                <p class="stdformbutton bordrtopnone" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" class="submit radius2" ToolTip="Save" ValidationGroup="UserMgmt" onclick="btnSave_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" class="reset radius2" ToolTip="Reset" onclick="btnReset_Click" />
                </p>
            </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
