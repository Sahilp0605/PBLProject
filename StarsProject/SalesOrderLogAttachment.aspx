<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesOrderLogAttachment.aspx.cs" Inherits="StarsProject.SalesOrderLogAttachment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
</head>
<body>
    <form id="frmLogAttachment" runat="server">
         <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnLogID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnOrderpkID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnOrderNo" runat="server" ClientIDMode="Static" />
        <div class="row">
            <div class="input-field col m3">
                <label class="active" for="drpInquiry">Order Status</label>
                <asp:DropDownList ID="drpApprovalStatus" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="1" Style="height: 1.70rem; margin: 0px; padding: 0px 0px 0px 10px;" />
            </div>
            <div class="input-field col m3">
                <label class="active" for="drpInquiry">Government Project Stages</label>
                <asp:DropDownList ID="drpProjectStage" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="2" Style="height: 1.70rem; margin: 0px; padding: 0px 0px 0px 10px;" />
            </div>
        </div>
        <div class="row">
            <div class="input-field col m4">
                <label class="active" for="txtRemarks">Remark</label>
                <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" TabIndex="3" TextMode="MultiLine" placeholder="" Style="min-height: 50px !important; height: 50px !important;" />
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <div class="section app-file-manager-wrapper">
                <div class="app-file-overlay"></div>
                <div class="row">
                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                        <div class="app-file-header">
                            <div class="row">
                                <div class="input-field col m12 add-new-file mt-0">
                                    <div class="col m12">
                                        <label class="active" for="uploadDocument">Upload Product Specification Document Here  </label>
                                    </div>
                                    <div class="col m12">
                                        <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                        <asp:Button ID="btnUploadDoc" CssClass="hide" runat="server" Text="Upload"  onclick="btnUploadDoc_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="app-file-content">
                            <div class="app-file-sidebar-content row">
                                <div class="col m12 app-file-files">
                                    <asp:Repeater ID="rptOrderDocs" runat="server" ClientIDMode="Static" OnItemCommand="rptOrderDocs_ItemCommand">
                                    <ItemTemplate>
                                        <div class="col xl4 l6 m4 s6">
                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                    <div class="row fonticon p2">
                                                        <div class="col m6" style="text-align:left;">
                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                        </div>
                                                        <div class="col m6" style="text-align:right;">
                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("AttachmentFile").ToString() %>');">
                                                            <i class="material-icons">find_in_page</i>
                                                            </a>
                                                        </div>
                                                    </div>
                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                </div>
                                                <div class="app-file-details">
                                                    <div class="app-file-name font-weight-700">
                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("AttachmentFile").ToString().Substring(6) %></p>
                                                    </div>
                                                </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUploadDoc" />
                </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row mt-2 padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom">
            <button id="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="21"><i class="material-icons left">save</i>Save</button>
        </div>

    </form>
</body>
</html>
