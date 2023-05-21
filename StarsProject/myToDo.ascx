<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myToDo.ascx.cs" Inherits="StarsProject.myToDo" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnTaskStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />
<style type="text/css">
    .todoEditIcon {
        font-size: 16px;
        padding: 3px;
        color: navy;
        border: 2px solid navy;
        border-radius: 4px;
        background-color: #90ee90;
        position: relative;
        left: -25px;
        top: -15px;
    }
</style>

  <ul class="todo-list" style="list-style:none;">
    <asp:Repeater ID="rptTODO" runat="server" OnItemDataBound="rptTODO_ItemDataBound">
        <ItemTemplate>
                <asp:HiddenField ID="hdnPriority" runat="server" ClientIDMode="Static" Value='<%# Eval("Priority") %>' />
                <asp:HiddenField ID="hdnDueDate" runat="server" ClientIDMode="Static" Value='<%# Eval("DueDate") %>' />
                <asp:HiddenField ID="hdnCompletionDate" runat="server" ClientIDMode="Static" Value='<%# Eval("CompletionDate") %>' />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%# Eval("CustomerID") %>' />
                <li style="background-color:beige; border:2px solid #d6c7c7; border-radius:8px; padding:5px; margin-bottom:5px;">
                    <div class="row m-0 p-0">
                        <div class="col m12 pr-0">
                            <a class="left" title="Edit Task" href="javascript:openToDO('edit','<%# Eval("pkID") %>');">
                                <i class="material-icons todoEditIcon">border_color</i>
                            </a>
                            <span class="float-right badge gradient-45deg-yellow-green black-text" style="line-height: 10px; vertical-align: text-top; padding: 5px; border-radius: 8px;">
                                <%# Eval("TaskStatus") %>
                            </span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col m4">
                            <span class="text" style="color:maroon; text-align:left; font-size:14px;">
                                Assigned To
                            </span>
                        </div>
                        <div class="col m4">
                            <span class="text" style="color:maroon; text-align:left; font-size:14px;">Initiated By</span>
                        </div>
                        <div class="col m4">
                            <span class="text" style="color:maroon; float:right !important; font-size:14px;"">Status</span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col m4">
                            <span class="handle">
                                <span class="text" style="padding-left:3px; font-size:14px;">
                                    <a href="javascript:openDashboardBox('todo','view',<%# Eval("pkID")%>);"><%# Eval("EmployeeName") %></a>
                                </span>
                            </span>
                        </div>
                        <div class="col m4">
                            <span class="text" style="padding-left:3px; font-size:14px;">
                                <a href="javascript:openDashboardBox('todo','view',<%# Eval("pkID")%>);"><%# Eval("FromEmployeeName") %></a>
                            </span>
                        </div>
                        <div class="col m4">
                            <span class="handle">
                                <small class="float-right badge badge-secondary mr-1"><i class="fa fa-clock-o"></i>  <%# Eval("Priority") %> </small>
                                <small class="float-right badge badge-danger mr-5"><i class="fa fa-clock-o"></i>  <%# Eval("Duration") %> Days </small>
                            </span>
                        </div>
                    </div>
                    <div class="row mt-1">
                        <div class="col m12">
                            <span class="text pl-0" style="color:maroon; font-size:12px; font-weight:bold; display:inline-block; overflow:hidden; width:350px;white-space: nowrap; text-overflow: ellipsis;">
                                <%# Eval("CustomerName") %>
                            </span>
                        </div>
                    </div>
                    <div class="row limited mt-1 mb-1">
                        <div class="col m12" style="width:350px;">
                            <%# Eval("TaskDescription") %>
                        </div>
                    </div>
                    <div class="row mt-1 ml-0 mr-0 gradient-45deg-light-blue-indigo">
                        <div class="col m4">
                            <small class="badge badge-primary bold red-text">
                                <span class="bold blue-text"> Start : </span><%# Eval("StartDate", "{0:dd-MM-yyyy HH:mm}") %> 
                            </small>
                        </div>
                        <div class="col m4">
                            <small class="badge badge-secondary bold red-text">
                                <span class="bold blue-text"> Due : </span><%# Eval("DueDate", "{0:dd-MM-yyyy HH:mm}") %> 
                            </small>
                        </div>
                        <div class="col m4" id="divCompletionDate" runat="server" clientidmode="Static">
                            <small class="badge badge-success">
                                <span class="bold blue-text"> Completed : </span>
                                <span class="bold red-text" ID="ltrCompletion" runat="server" clientidmode="Static"><%# Eval("CompletionDate", "{0:dd-MM-yyyy HH:mm}") %></span>
                            </small>
                        </div>
                    </div>
                </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
