<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentGallery.aspx.cs" Inherits="StarsProject.DocumentGallery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- BEGIN: VENDOR CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <!-- END: VENDOR CSS-->
    <!-- BEGIN: Page Level CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/widget-timeline.css" />
    <!-- END: Page Level CSS-->
    <!-- BEGIN: Custom CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <!-- END: Custom CSS-->
    
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>

    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>
    <script type="text/javascript" src="app-assets/js/scripts/app-file-manager.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
         
        });
        /*---------------------------------------------------------*/
        function pageLoad(sender, args) {
           
        }
        function ShowPDFfile(repFilename) {
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
            //ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
        }
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }

        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function ShowPDFfile(repFilename) {
            repFilename = "DocumentGallery/" + repFilename;
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }
    </script>
    <style type="text/css">
        ::-webkit-scrollbar {
            width: 0px;
            background: transparent; /* make scrollbar transparent */
        }
    </style>
</head>
<body>
    <form id="frmFileManager" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server" EnablePartialRendering="true" EnablePageMethods="true" EnableViewState="true"></asp:ScriptManager>

        <!-- BEGIN: Page Main-->
        <div id="main" class="main-full">
            <div class="row">
                <div class="col m12">
                    <div class="container">
                        <div id="proddocs" class="row" runat="server" clientidmode="Static" enableviewstate="true">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <div class="section app-file-manager-wrapper">
<%--                                <div class="app-file-overlay"></div>--%>
                                <div class="row">
                                    <div class="app-file-area" style="min-height:465px; overflow-x: hidden;">
                                        <div class="app-file-header">
                                            <div class="row">
                                                <div class="input-field col m12 add-new-file mt-0">
                                                    <div class="col m12">
                                                        <label class="active" for="uploadDocument">Upload Product Specification Document Here  </label>
                                                    </div>
                                                    <div class="col m12">
                                                        <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                        <asp:Button ID="btnUpload1" CssClass="hide" runat="server" Text="Upload"  onclick="btnUpload1_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="app-file-content">
                                            <div class="app-file-sidebar-content row">
                                                <div class="col m12 app-file-files">
                                                    <asp:Repeater ID="rptDocGallery" runat="server" ClientIDMode="Static" OnItemCommand="rptDocGallery_ItemCommand">
                                                    <ItemTemplate>
                                                        <div class="col m3">
                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                <div class="card-content border-radius-4" style="padding: 8px; border:2px solid grey;">
                                                                    <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                        <div class="row fonticon p2">
                                                                            <div class="col m2 left-align">
                                                                                <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'>
                                                                                    <i class="material-icons delete" style="padding: 5px 0px 0px 0px;">delete_outline</i>
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                            <div class="col m10 right-align">
                                                                                <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                    <i class="material-icons" style="padding: 5px 0px 0px 20px;">find_in_page</i>
                                                                                </a>
                                                                            </div>
                                                                        </div>
                                                                        <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                    </div>
                                                                    <div class="app-file-details border-bottom-1">
                                                                        <div class="app-file-name font-weight-700">
                                                                            <label class="blue-text" style="display: block;width: 300px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(6) %></label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col m6 left-align"><label>Init.By <font class="text-capitalize" color="red"><%# Eval("CreatedBy") %></font></label></div>
                                                                        <div class="col m6 right-align"><label><font color="navy"><%# Eval("CreatedDate", "{0:dd/MM/yyyy}") %></font></label></div>
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
                                <asp:PostBackTrigger ControlID="btnUpload1" />
                            </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="content-overlay"></div>
                </div>
            </div>
        </div>
        <!-- END: Page Main-->

    </form>
</body>
</html>
