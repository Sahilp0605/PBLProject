<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadMaster.aspx.cs" Inherits="StarsProject.UploadMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="app-assets/js/plugins.js"></script>
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <script type="text/javascript">
        
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function showErrorMessage(strMess) {
            jQuery.confirm({ title: 'Data Validation', content: 'Are you sure, You want to delete record !', type: 'red', typeAnimated: true });
        }
        /*---------------------------------------------------------*/
    </script>
</head>
<body>
    <form id="form1" runat="server" style="color:navy;">
        <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="widgetbox">
                    <div id="myModal" style="display: block; width: 100%; overflow-x: hidden;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row gradient-45deg-indigo-purple white-text">
                                    <div class="col m12">
                                        <div id="dvFileUpload" style="margin:20px 100px;">
                                            <asp:FileUpload ID="FileUpload1" runat="server" accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" ClientIDMode="Static" onchange="this.form.submit()" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m12">
                                        <div style="margin-top:10px; padding:10px; background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">Preview Format</div>
                                        <div class="width-100" style="overflow-x:auto;">
                                            <asp:Image ID="imgFormat" runat="server" ClientIDMode="Static" Height="50" ImageUrl="images/no-figure2.png" /> 
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m12">
                                        <div class="row" style="margin-top:10px; padding:10px; background: linear-gradient(45deg, #c5e1a5, #fff8e1) !important;">
                                            <div class="col m3">
                                                <asp:Label ID="Label1" runat="server" ClientIDMode="Static" Text="Upload Statistics" />
                                            </div>
                                            <div class="col m3">
                                                <asp:Label ID="lblSuccessCount" runat="server" ClientIDMode="Static" Text="Success Count : " />
                                            </div>
                                            <div class="col m3">
                                                <asp:Label ID="lblFailedCount" runat="server" ClientIDMode="Static" Text="Failed Count : " />
                                            </div>
                                            <div class="col m3">
                                                <asp:Label ID="lbldataIssueCount" runat="server" ClientIDMode="Static" Text="Invalid Data Count : " />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
