<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myComplaint.ascx.cs" Inherits="StarsProject.myComplaint" %>
<style type="text/css">
    small {
        font-size:14px;
        color:black;
    }
</style>
<asp:HiddenField ID="hdnOpenTicket" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnCloseTicket" runat="server" ClientIDMode="Static" />
<asp:Repeater ID="rptComplaint" runat="server" OnItemDataBound="rptComplaint_ItemDataBound">
    <ItemTemplate>
        <asp:HiddenField ID="hdnComplaintID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
        <asp:HiddenField ID="hdnComplaintDays" runat="server" ClientIDMode="Static" Value='<%# Eval("ComplaintDays") %>' />
        <ul class="todo-list" style="list-style:none;">
            <li id="liBase" runat="server" style="background-color:beige; border: 2px solid #d6c7c7; border-radius:8px; padding:5px;">
                <div class="row col m12 mt-2 ml-0 mr-0">
                    <span class="handle">
                        <a title="Add Visit" href="javascript:openComplaintVisit(0, '<%# Eval("pkID") %>');">
                            <img id="Img1" class="float-left" runat="server" clientidmode="static" src="images/expand.png" style="width:20px; height:20px;" />
                        </a>
                        <span class="text" style="color:black; padding-left:5px; font-weight:600;  text-transform:capitalize;">
                            <a id="lnkShowFollowUp" title="Show Complaint" href="javascript:viewComplaint('view','<%# Eval("pkID") %>');"><%# Eval("CustomerName") %></a>
                        </span>
                        <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i>  <%# Eval("ComplaintStatus") %> </small>
                    </span>
                </div>
                <div class="row mt-2 ml-0 mr-0">
                    <div class="col m12">
                        <span class="text" style="color:navy; font-size:12px; font-weight:400;"><%# Eval("ComplaintNotes") %></span>
                    </div>
                </div>
                <div class="row mt-2 ml-0 mr-0" style="border-top:1px solid silver; border-bottom:1px solid silver;">
                    <div class="col m2 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Complaint #</span></span>
                    </div>
                    <div class="col m3 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Complaint Date</span></span>
                    </div>
                    <div class="col m4 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Schedule Date & Time</span></span>
                    </div>
                    <div class="col m3 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Complaint Days</span></span>
                    </div>
                </div>
                <div class="row mt-1 ml-0 mr-0">
                    <div class="col m2 pr-0">
                        <small class="badge-primary center"><%# Eval("ComplaintNo") %></small>
                    </div>
                    <div class="col m3 pr-0">
                        <small class="badge-primary center"><%# Eval("ComplaintDate", "{0:dd-MM-yyyy}") %></small>
                    </div>
                    <div class="col m4 pr-0">
                        <small class="badge-primary center"><%# Eval("PreferredDate", "{0:dd-MM-yyyy}") %>&nbsp;<b><%# Eval("TimeFrom") %>&nbsp; - &nbsp;<%# Eval("TimeTo") %></b></small>
                    </div>
                    <div class="col m3 pr-0">
                        <small id="lblComplaintDays" runat="server" class="badge-primary center"><%# Eval("ComplaintDays") %></small>
                    </div>
                </div>
                <div class="row mt-1 ml-0 mr-0 gradient-45deg-light-blue-indigo" style="font-size:14px;">
                    <div class="col m3">
                        <small><span style="font-weight:800;">Assigned To</span></small>
                    </div>
                    <div class="col m9">
                        <small><span style="color:navy;"><%# Eval("EmployeeName") %></span></small>
                    </div>
                </div>
                <div class="row mt-1 ml-0 mr-0" style="background-color:navy; font-size:14px;">
                    <div class="col m12">
                        <small>
                            <span style="color:white; font-weight:800;">Initiated By :</span>
                            <span style="color:white;"><b>&nbsp;<%# Eval("CreatedByEmployee") %></b></span>
                            <span style="color:white; font-weight:800; margin-left:20px;">Initiated On :</span>
                            <span style="color:white;"><b>&nbsp;<%# Eval("CreatedDate", "{0:dd-MM-yyyy hh:mm tt}") %></b></span>
                        </small>
                    </div>
                </div>
            </li>
        </ul>
<%--        <ul class="todo-list" style="list-style: none;">
            <li style="background-color: #9db3da; border-left: 10px solid #ff4500; padding: 5px 5px 5px 10px;">
                <div class="row m-0">
                    <div class="col m12 pl-0">
                        <div class="handle" style="width:100%;">
                            <span class="text" style="color: Red; font-weight: 600; text-transform: capitalize;">
                                <h5 class="m-0"><a style="color: navy;" href="javascript:viewComplaint('view','<%# Eval("pkID") %>');"><%# Eval("CustomerName") %></a></h5>
                            </span>
                        </div>
                        <div class="text" style="width:100%; min-height: 70px; background-color: white; padding: 3px; border-radius: 6px;">
                            <span class="text" style="color: navy; font-size: 14px; font-weight: 400;"><%# Eval("ComplaintNotes") %></span>
                        </div>
                    </div>
                    <div class="col m12" style="min-height:100px; background-color: white; border-radius: 8px;">
                        <div class="row m-0 mb-1">
                            <div class="col m6 badge badge-primary left-align">
                                <i class="fa fa-calendar-check-o"></i><span>Complaint Date : </span><%# Eval("ComplaintDate", "{0:dd-MM-yyyy}") %> 
                            </div>
                            <div class="col m6 badge badge-secondary right-align">
                                <i class="fa fa-calendar-check-o"></i><span>Assigned To : </span>
                                <span id="ltrAssignedTo" runat="server" clientidmode="Static"><%# Eval("EmployeeName") %></span>
                            </div>
                        </div>
                        <div class="row m-0 mb-1 mt-2">
                            <div class="col m12" style="border-radius: 8px; background-color: white; margin-left: 8px;">
                                <button type="button" class="btn btn-green left" onclick="javascript:openComplaintVisit(0, '<%# Eval("pkID") %>');" style="width: 180px;"><i class="fa fa-plus"></i>Show Visit</button>
                                <button type="button" class="btn btn-danger right" onclick="javascript:showComplaintVisit(0, '<%# Eval("pkID") %>');" style="width: 180px;"><i class="fa fa-plus"></i>Add Visit</button>
                            </div>
                        </div>
                    </div>
                </div>
            </li>
        </ul>--%>
    </ItemTemplate>
</asp:Repeater>
