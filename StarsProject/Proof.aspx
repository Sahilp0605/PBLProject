<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="Proof.aspx.cs" Inherits="StarsProject.Proof" %>
<%@ Register Src="~/PaggingUserControl.ascx" TagName="page" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdnOldUserID" runat="server" />
    <asp:ScriptManager ID="srcUser" runat="server">
    </asp:ScriptManager>
    <div id="contentwrapper" class="contentwrapper">
        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="widgetbox">
                    <div class="widgetcontent padding0 statement">
                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0">
                            <asp:Repeater ID="rptUser" runat="server" OnItemCommand="rptUser_ItemCommand">
                                <HeaderTemplate>
                                    <thead>
                                        <tr>
                                            <th>Proof ID</th>
                                            <th>Proof Description</th>
                                            <th>Valid For Address Proof</th>
                                            <th>Valid For Identity Proof</th>
                                            <th>Active Status</th>
                                            <th style="text-align: center; width: 4%;">Edit</th>
                                            <th style="text-align: center; width: 4%;">Delete</th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ProofID") %></td>
                                        <td><%# Eval("ProofName") %></td>
                                        <td><%# Eval("IsAddressProof") %></td>
                                        <td><%# Eval("IsIdentityProof")%></td>
                                        <td><%# Eval("ActiveFlag") %></td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="ImgbtnEdit" runat="server" ImageUrl="~/images/edit.png" Width="16"
                                                Height="16" CommandName="Edit" CommandArgument='<%# Eval("ProofID") %>' />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png"
                                                CommandName="Delete" CommandArgument='<%# Eval("ProofID") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <uc1:page ID="pageGrid" runat="server" Visible="true" OnPagerChanged="Pager_Changed" />
                    </div>
                </div>
                <div id="basicform" class="subcontent">
                    <div class="clearall"></div>
                    <div class="leftdiv50">
                        <asp:HiddenField ID="hdnproofid" runat="server"/>
                        <p class="bordeelftnn">
                            <label>Proof Name</label>
                            <span class="field rghtbrdr" id="marwdthlow">
                                <asp:TextBox ID="txtProofName" runat="server" class="shortinptfld" MaxLength="200" TabIndex="0"></asp:TextBox>
                            </span>
                        </p>
                        <p class="bordeelftnn">
                            <label>Address Proof</label>
                            <span class="field rghtbrdr" id="marwdthlow">
                                <asp:CheckBox ID="chkAddress" runat="server" style="margin:10px 0px;"  TabIndex="1"/>
                            </span>
                        </p>
                    </div>
                    <div class="rightdiv50">
                        <p style="border-left: none; height:53px;">
                            <label style="height:43px;">Active Status</label>
                            <span class="field rghtbrdr" id="marwdthlow" style="height:43px;">
                                <asp:CheckBox ID="chkActive" runat="server" style="margin:10px 0px;"  TabIndex="2"/>
                            </span>
                        </p>
                        <p style="border-left: none; height:53px;">
                            <label style="height:43px;">Identity Proof</label>
                            <span class="field rghtbrdr" id="Span1" style="height:43px;">
                                <asp:CheckBox ID="chkIdentity" runat="server" style="margin:10px 0px;"  TabIndex="3"/>
                            </span>
                        </p>
                    </div>

                      <div class="row">
                            <div class="input-field col m12">
                                <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                            </div>
                        </div>

                    <div class="clearall"></div>
                    <div class="usermsg"></div>
                    <div class="clearall"></div>
                    <div>
                        <p class="stdformbutton bordrtopnone" align="center">
                            <asp:Button ID="btnSave" runat="server" Text="Save" class="submit radius2" ToolTip="Save"
                                ValidationGroup="UserMgmt" OnClick="btnSave_Click" TabIndex="4" />
                            <asp:Button ID="btnReset" runat="server" Text="Reset" class="reset radius2" ToolTip="Reset"
                                OnClick="btnReset_Click" TabIndex="5" />
                        </p>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
