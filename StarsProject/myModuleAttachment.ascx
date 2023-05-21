<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myModuleAttachment.ascx.cs" Inherits="StarsProject.myModuleAttachment" %>

<asp:HiddenField ID="hdnModuleName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnKeyValue" runat="server" ClientIDMode="Static" />
<script type="text/javascript">
    function previewAttachment(repFilename) {
        repFilename = "ModuleDocs/" + repFilename;
        yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
        yhooWin.focus();
    }
    alert(IhaveReached)
</script>
<% if (hdnModuleName.Value == "ComplaintAcu-Panel")
    { %>
    <h5 class="card-title m-0 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo" style="line-height: 140%;">&nbsp;Attach Actual Images</h5>
<% } else if(hdnModuleName.Value == "ComplaintAcu-Defect"){%>
    <h5 class="card-title m-0 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo" style="line-height: 140%;">&nbsp;Attach Defective Images</h5>
<% } else if(hdnModuleName.Value == "VisitAcu-Before"){%>
    <h5 class="card-title m-0 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo" style="line-height: 140%;">&nbsp;Attach Panel Photo Images</h5>
<% } else if(hdnModuleName.Value == "VisitAcu-After"){%>
    <h5 class="card-title m-0 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo" style="line-height: 140%;">&nbsp;Attach Photos Taken After Action</h5>
<% } else if(hdnModuleName.Value == "ProductDoc-"){%>
    <h5 class="card-title m-0 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo" style="line-height: 140%;">&nbsp;Upload Documents here..</h5>
<% } else {%>
    <h5 class="card-title m-0 mb-1 float-left left-align width-100 gradient-45deg-light-blue-indigo" style="line-height: 140%;">&nbsp;Attachments List</h5>
<% } %>
<div id="rcbook" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:block; float:left; width:100%;">
    <asp:UpdatePanel ID="pnlAttachment" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="section app-file-manager-wrapper">
            <div class="app-file-overlay"></div>
            <div class="app-file-area" style="height:200px; overflow-y:auto;">
                <div class="app-file-header">
                    <div class="row">
                        <div class="input-field col m4 add-new-file mt-0">
                            <div class="col m12">
                                <label class="active" for="uploadRCBook">Upload Attachment</label>
                            </div>
                            <div class="col m12">
                                <asp:FileUpload ID="uploadAttachment" multiple="multiple" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select PDF And Image File" /> 
                                <asp:Button ID="btnAttachment" CssClass="hide" runat="server" ClientIDMode="Static" Text="Upload"  onclick="btnAttachment_Click" />
                            </div>
                        </div>
                        <div class="col m8 app-file-files">
                            <table id="tblRCBook" class="table table-striped table-bordered" style="font-size:12px;" cellpadding="0" cellspacing="0" border="0" width="100%">
                            <asp:Repeater ID="rptAttachment" runat="server" ClientIDMode="Static" OnItemCommand="rptAttachment_ItemCommand">
                                <HeaderTemplate>
                                    <thead>
                                        <tr>
                                            <th class="left-align" style="width:60%;">File Name</th>
                                            <th class="center-align" style="width:15%;">Uploaded By</th>
                                            <th class="center-align" style="width:15%;">Uploaded Date</th>
                                            <th class="center-align" style="width:5%;">View</th>
                                            <th class="center-align" style="width:5%;">Delete</th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tbody>
                                        <tr>
                                            <td class="left-align" style="width:60%;"><a href="javascript:previewDocument('<%# Eval("DocName").ToString() %>');"><%# Eval("DocName").ToString() %></a></td>
                                            <td class="center-align" style="width:15%;"><%# Eval("CreatedBy").ToString() %></td>
                                            <td class="center-align" style="width:15%;"><%# Eval("CreatedDate", "{0:dd/MM/yyyy}").ToString() %></td>
                                            <td class="center-align" style="width:5%;">
                                                <a id="lnkViewFile"href="javascript:previewAttachment('<%# Eval("DocName").ToString() %>');");">
                                                    <i class="material-icons" style="padding: 5px 0px 0px 20px;">find_in_page</i>
                                                </a>
                                            </td>
                                            <td class="center-align" style="width:5%;">
                                                <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("DocName") %>'/>
                                            </td>
                                        </tr>
                                    </tbody>
                                </ItemTemplate>
                            </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnAttachment" />
    </Triggers>
    </asp:UpdatePanel>
</div>