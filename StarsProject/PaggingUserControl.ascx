<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaggingUserControl.ascx.cs" Inherits="StarsProject.PaggingUserControl" %>
<style type="text/css">
    .grid-cntNav {height: auto; left: 5px; margin: 10px 0px; padding: 0; top: 0; width: 100%;}
    .flRight {float: right;}
    .leftp {float: left; margin-right:20px;}
    .leftp b{font-size:15px;}
    .leftp span{font-size:14px; margin:0px 5px;}
    .pagging {color: #949494; float: right; height: 20px; line-height: 19px; padding: 0 10px; width:98%;}
    .pagging a {background: none repeat scroll 0 0 #DDE0E8; border: 1px solid #D5D5D5; color: #949494; float: left; height: 20px; margin-left: 5px; padding: 0 8px; text-decoration: none;}
    .currpage{background: none repeat scroll 0 0 #FB9337 !important; color: #000000 !important; font-weight: bold;}
</style>
<div class="flRight grid-cntNav">
    <div class="pagging">
        <div class="leftp">
        <span> Record </span><b style="color:#34445E;"><asp:Literal ID="lblstrat" runat="server" Text=""></asp:Literal></b><span> To </span><b style="color:#34445E;"><asp:Literal ID="lblend" runat="server" Text=""></asp:Literal></b><span>of</span> <b style="color:#34445E;"><asp:Literal ID="lblcount" runat="server" Text=""></asp:Literal></b>
        </div>
        <div class="flRight">
            <asp:LinkButton ID="lnkPrevious" runat="server" Text="Previous" OnClick="lnkPrevious_Click"></asp:LinkButton>
            <asp:Repeater ID="rptPaging" runat="server" OnItemCommand="rptPaging_ItemCommand"
                OnItemDataBound="rptPaging_ItemDataBound">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPageNo" runat="server" Text='<%# Container.DataItem %>' CommandName="ChangePage"
                        CommandArgument='<%# Container.DataItem %>'>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:Repeater>
            <asp:LinkButton ID="lnkNext" runat="server" Text="Next" OnClick="lnkNext_Click"
                CssClass="pagebtn"></asp:LinkButton>
        </div>
    </div>
</div>
<div class="cboth">
</div>
