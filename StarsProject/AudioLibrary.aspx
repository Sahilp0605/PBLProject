<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AudioLibrary.aspx.cs" Inherits="StarsProject.AudioLibrary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link  rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>

    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="app-assets/js/scripts/app-file-manager.js"></script>
</head>
<body>
    <form id="frmAudio" runat="server">
        <asp:HiddenField ID="hdnModuleName" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <asp:HiddenField ID="hdnKeyID" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />

        <div id="prodimg" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
            <asp:UpdatePanel ID="updAudioPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="row">
                <div class="col s12">
                    <ul class="collapsible collapsible-accordion">
                        <li>
                            <div class="collapsible-header cyan white-text"><i class="material-icons">toll</i> Additional Image Gallery
                            </div>
                            <div class="collapsible-body cyan lighten-5 lighten-5 padding-1">
                                <div class="section app-file-manager-wrapper">
                                <div class="app-file-overlay"></div>
                                    <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                        <div class="app-file-header">
                                            <div class="row">
                                                <div class="input-field col m12 add-new-file mt-0">
                                                    <div class="col m12">
                                                        <label class="active" for="uploadFileAudio">Upload Audio</label>
                                                    </div>
                                                    <div class="col m12">
                                                        <asp:FileUpload ID="uploadAudioGallery" multiple="multiple" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only Audio Files" /> 
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="app-file-content">
                                            <div class="app-file-sidebar-content row">
                                                <div class="col m12 app-file-files">
                                                    <asp:Repeater ID="rptAudioList" runat="server" ClientIDMode="Static" OnItemCommand="rptAudioList_ItemCommand">
                                                    <ItemTemplate>
                                                        <div class="col m4">
                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                <div class="row col m12 p-0">
                                                                    <div class="col m6 p-0 left-align">
                                                                        <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" TabIndex="47" CommandArgument='<%# Eval("pkID") %>'>
                                                                        <i class="material-icons delete" style="padding: 5px 0px 0px 20px;">delete_outline</i>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div class="col m6 p-0 right-align">
                                                                        <a id="lnkViewFile">
                                                                            <i class="material-icons" style="padding: 5px 0px 0px 20px;">find_in_page</i>
                                                                        </a>
                                                                    </div>
                                                                </div>
                                                                <div class="card-content" style="background-color:beige; padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                    <div class="row fonticon p2" style="background:url('<%# "AudioFiles/" + Eval("FileName").ToString() %>') no-repeat top center; background-size:cover; width: 150px; height: 98px; margin-left: 10px;"></div>
                                                                </div>
                                                                <div class="app-file-details" style="background-color:silver;border-radius: 8px; padding-left: 10px;">
                                                                    <div class="app-file-name font-weight-700">
                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString() %></p>
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
                        </li>
                    </ul>
                </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload1" />
            </Triggers>
            </asp:UpdatePanel>
        </div>

    </form>
</body>
</html>
