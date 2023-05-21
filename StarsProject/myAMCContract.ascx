<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myAMCContract.ascx.cs" Inherits="StarsProject.myAMCContract" %>
<style type="text/css">
    small {
        font-size:14px;
        color:black;
    }
</style>
<asp:HiddenField ID="hdnOpenTicket" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnCloseTicket" runat="server" ClientIDMode="Static" />
<asp:Repeater ID="rptContract" runat="server" OnItemDataBound="rptContract_ItemDataBound">
    <ItemTemplate>
        <asp:HiddenField ID="hdnComplaintID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
        <asp:HiddenField ID="hdnRenewDays" runat="server" ClientIDMode="Static" Value='<%# Eval("RenewDays") %>' />
        <ul class="todo-list" style="list-style:none;">
            <li id="liBase" runat="server" style="background-color:beige; border: 2px solid #d6c7c7; border-radius:8px; padding:5px;">
                <div class="row col m12 mt-1 mb-1 ml-0 mr-0">
                    <span class="handle">
                        <span class="text" style="color:black; padding-left:5px; font-weight:600;  text-transform:capitalize;">
                            <a id="lnkShowFollowUp" title="Show Complaint" href="javascript:viewContract('view','<%# Eval("pkID") %>');"><%# Eval("CustomerName") %></a>
                        </span>
                        <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i>  <%# Eval("ContractType") %> </small>
                    </span>
                </div>
                <div class="row mt-2 ml-0 mr-0" style="border-bottom:1px solid silver;">
                    <div class="col m3 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Contract #</span></span>
                    </div>
                    <div class="col m6 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Contract Period</span></span>
                    </div>
                    <div class="col m3 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Contract Validity</span></span>
                    </div>                    
                </div>
                <div class="row mt-1 ml-0 mr-0">
                    <div class="col m3 pr-0">
                        <small class="badge-primary center"><%# Eval("InquiryNo") %></small>
                    </div>
                    <div class="col m6 pr-0">
                        <small class="badge-primary center"><b><%# Eval("StartDate", "{0:dd-MM-yyyy}") %>&nbsp; - &nbsp;<%# Eval("EndDate", "{0:dd-MM-yyyy}") %></b></small>
                    </div>
                    <div class="col m3 pr-0">
                        <small id="lblRenewDays" runat="server" class="badge-primary center"><%# Eval("RenewDays") %></small>
                    </div>
                </div>
                <div class="row mt-1 ml-0 mr-0" style="background-color:navy; font-size:14px;">
                    <div class="col m12">
                        <small>
                            <span style="color:white; font-weight:800;">Initiated By :</span>
                            <span style="color:white;"><b>&nbsp;<%# Eval("EmployeeName") %></b></span>
                            <span style="color:white; font-weight:800; margin-left:20px;">Initiated On :</span>
                            <span style="color:white;"><b>&nbsp;<%# Eval("CreatedDate", "{0:dd-MM-yyyy hh:mm tt}") %></b></span>
                        </small>
                    </div>
                </div>
            </li>
        </ul>
    </ItemTemplate>
</asp:Repeater>
