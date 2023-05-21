<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myNotifications.ascx.cs" Inherits="StarsProject.myNotifications" %>

<script type="text/javascript" src='<%=ResolveUrl("~/js/plugins/jquery.colorbox-min.js") %>'></script>
<link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
<div class="crmNotiContent">
    <div id="crmDropdown" runat="server" clientidmode="Static" class="crmNotiData">
        <div class="card" style="box-shadow: -6px 8px 6px -6px black; padding: 10px 10px;">
            <div class="card-header" style="padding: 3px;">
                <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-tasks ml-2 mr-2"></i>Notification List                              
                </h3>
            </div>
            <!-- /.card-header -->
            <div class="card-body" style="min-height: 400px;  overflow-y: auto;">
                <ul class="todo-list" style="list-style: none;">
                    <asp:Repeater ID="rptNotification" runat="server">
                        <ItemTemplate>
                            <li style="background-color: #f4f4f4; border-left: 2px solid #d6c7c7; padding: 5px 0px 5px 5px; margin-bottom: 5px;">
                                <span class="handle">
                                    <i class="fa fa-ellipsis-v"></i>
                                    <i class="fa fa-ellipsis-v"></i>
                                    <span class="text" style="padding-left: 5px; font-weight: 600;"><%# Eval("ModuleName") %></span>
                                    <small class="badge badge-success" style="float: right; margin-right: 4px; margin-top: 3px;"><i class="fa fa-calendar-check-o"></i><span>Date : </span><%# Eval("CreatedDate", "{0:dd-MM-yyyy}") %> </small>
                                
                                </span>
                                <br />
                                <div style="padding: 5px; background-color: white; border-radius: 2px; margin: 5px;">
                                    <span class="text" style="color: navy; font-size: 14px; font-weight: 400;"><%# Eval("Description") %></span>
                                </div>

                                <div class="tools" style="display: none;">
                                    <i class="fa fa-edit"></i>
                                    <i class="fa fa-trash-o"></i>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <!-- /.card-body -->

        </div>
    </div>
</div>
