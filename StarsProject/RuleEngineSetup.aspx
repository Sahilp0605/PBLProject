<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="RuleEngineSetup.aspx.cs" Inherits="StarsProject.RuleEngineSetup" %>
<%@ Register Src="~/PaggingUserControl.ascx" TagName="page" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <link href="css/Registration.css" rel="stylesheet" type="text/css" />
   <link rel="stylesheet" type="text/css" href="css/PageReSetup.css" />

    <style type="text/css">
        .grid-cntNav {height: auto; left: 5px; margin: 10px 0px; padding: 0; top: 0; width: auto;}
        .flRight {float: right;}
        .pagging {color: #949494; float: right; height: 20px; line-height: 19px; padding: 0 10px;}
        .pagging a {background: none repeat scroll 0 0 #DDE0E8; border: 1px solid #D5D5D5; color: #949494; float: left; height: 20px; margin-left: 5px; padding: 0 8px; text-decoration: none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div style="width:100%; text-align:center;"><img src="images/Under_Construction.jpg" alt="Page Under Construction" /></div>--%>
    <asp:ValidationSummary ID="valUserMgmt" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="UserMgmt" />
    <div class="pageheader">
        <h1 class="pagetitle contenttitle2">Rule Engine Setup</h1>
    </div>
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <div id="contentwrapper" class="contentwrapper">
    <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hdnID" runat="server" EnableViewState="true" />
        <div class="leftdiv50">
            <p class="bordeelftnn">
                <label>Exam</label>
                <span class="field rghtbrdr" id="marwdthlow">
                    <asp:DropDownList ID="drpExamMaster" runat="server" Width="200" 
                    style="margin:7px 0px;">
                    </asp:DropDownList>
                </span>
            </p>
            <p class="bordeelftnn">
                <label>District of Domicile</label>
                <span class="field rghtbrdr" id="marwdthlow">
                    <asp:DropDownList ID="drpSrcDomicile" runat="server" Width="200" style="margin:7px 0px;"></asp:DropDownList>
                </span>
            </p>
        </div>
        <div class="rightdiv50">
            <p class="bordeelftnn" style="border-left:none;">
                <label>Category</label>
                <span class="field rghtbrdr" id="marwdthlow">
                    <asp:DropDownList ID="drpCategoryMaster" runat="server" Width="200" 
                    style="margin:7px 0px;">
                    </asp:DropDownList>
                </span>
            </p>
            <p class="bordeelftnn" style="border-left:none;">
                <label>Gender</label>
                <span class="field rghtbrdr" id="marwdthlow">
                    <asp:DropDownList ID="drpSrcGender" runat="server" Width="200" style="margin:7px 0px;">
                        <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                        <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                    </asp:DropDownList>
                </span>
            </p>
        </div>
        <div class="clearall"></div>
        <div>
            <p class="bordrtopnone" style="border-right:none;">
                <label>Event</label>
                <span class="field rghtbrdr" id="marwdthlow">
                    <asp:DropDownList ID="drpsrcEvent" runat="server" Width="200" style="margin:7px 0px;">
                    </asp:DropDownList>
                </span>
            </p>
        </div>
        <div class="clearall"></div>
        <div>
            <p class="stdformbutton bordrtopnone" align="center" style="border:none;">
                <asp:Button ID="btnSearch" runat="server" Text="Search" class="submit radius2" 
                    ToolTip="Save" onclick="btnSearch_Click" />
                <asp:Button ID="bthSearchReset" runat="server" Text="Reset" class="reset radius2" 
                    ToolTip="Reset" onclick="bthSearchReset_Click" />
            </p>
        </div>
        <div class="widgetbox">
            <div class="widgetcontent padding0 statement">
                <table cellpadding="0" cellspacing="0" border="0" class="stdtable">
                    <asp:Repeater ID="rptUser" runat="server" onitemcommand="rptUser_ItemCommand">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Gender</th>
                                    <th>Exam Name</th>
                                    <th>Event Name</th>
                                    <th>District Of Domicile</th>
                                    <th>Category</th>
                                    <th>Eligibility</th>
                                    <th style="text-align:center; width:4%;">Edit</th>
                                    <th style="text-align:center; width:4%;">Delete</th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ID") %></td>
                                <td><%# Eval("Gender") %></td>
                                <td><%# Eval("ExamDesc") %></td>
                                <td><%# Eval("EventDesc") %></td>
                                <td><%# Eval("Domicial")%></td>
                                <td><%# Eval("CategoryDesc") %></td>
                                <td><%# Eval("EligibilityDesc") %></td>
                                <td style="text-align:center;"><asp:ImageButton ID="ImgbtnEdit" runat="server" ImageUrl="~/images/edit.png" Width="16" Height="16" CommandName="Edit" CommandArgument='<%# Eval("ID") %>' /></td>
                                <td style="text-align:center;"><asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' /></td>
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
                    <label>Gender</label>
                    <span class="field rghtbrdr" id="marwdthlow">
                        <asp:DropDownList ID="drpGender" runat="server" Width="200" style="margin:7px 0px;">
                            <asp:ListItem Text="Male" Value="M" />
                            <asp:ListItem Text="Female" Value="F" />
                        </asp:DropDownList>
                    </span>
                </p>
                <p class="bordeelftnn">
                    <label>Category</label>
                    <span class="field rghtbrdr" id="marwdthlow">                        
                            <asp:DropDownList ID="drpCategory" runat="server" Width="200" style="margin:7px 0px;">
                            </asp:DropDownList>
                    </span>
                </p>
                <p class="bordeelftnn">
                    <label>Eligibility</label>
                    <span class="field rghtbrdr" id="marwdthlow">
                        <asp:TextBox ID="txtEligibility" runat="server" class="shortinptfld" MaxLength="20" autocomplete="off"></asp:TextBox>
                    </span>
                </p>
                <p class="bordeelftnn">
                    <label>District of Domicile</label>
                    <span class="field rghtbrdr" id="marwdthlow">
                        <asp:DropDownList ID="drpDomicile" runat="server" Width="200" style="margin:7px 0px;"></asp:DropDownList>
                    </span>
                </p>
            </div>
            <div class="rightdiv50">
                <p style="border-left: none;">
                    <label>Exam</label>
                    <span class="field rghtbrdr" id="marwdthlow">
                        <asp:DropDownList ID="drpExam" runat="server" Width="200" style="margin:7px 0px;">
                        </asp:DropDownList>
                    </span>
                </p>
                <p style="border-left: none;">
                    <label>Event</label>
                    <span class="field rghtbrdr" id="marwdthlow">
                        <asp:DropDownList ID="drpEvent" runat="server" Width="200" style="margin:7px 0px;">
                        </asp:DropDownList>
                    </span>
                </p>
                <p style="border-left: none;">
                    <label>Eligibility To</label>
                    <span class="field rghtbrdr" id="marwdthlow">
                        <asp:TextBox ID="txtEligibility_To" runat="server" class="shortinptfld" MaxLength="20" autocomplete="off"></asp:TextBox>
                    </span>
                </p>
                <p style="border-left: none; height:53px;">&nbsp;</p>
            </div>
            <div class="clearall"></div>
            <div class="usermsg">Rule Added Successfully!!!</div>
            <div class="clearall"></div>
            <div>
                <p class="stdformbutton bordrtopnone" align="center" style="border:none;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" class="submit radius2" 
                        ToolTip="Save" ValidationGroup="UserMgmt" onclick="btnSave_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" class="reset radius2" 
                        ToolTip="Reset" onclick="btnReset_Click" />
                    <%--<a href="javascript:void(0);" title="Reset" class="reset radius2">Reset</a>--%>
                </p>
            </div>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <script type="text/javascript" language="javascript">
        function showmessage(msg) {
            jQuery('.usermsg').html(msg);
            jQuery(".usermsg").fadeIn(3000);
            jQuery(".usermsg").delay(5000).fadeOut(3000);
        }
        function changecolor() {
            jQuery('.stdtable tbody tr').hover(function () {
                jQuery(this).attr('style', 'background:#7EB8CE;');
            }, function () {
                jQuery(this).attr('style', 'background:#FFFFFF;');
            });
        }
        jQuery(document).ready(function () {
            changecolor();
        });
    </script>

</asp:Content>
