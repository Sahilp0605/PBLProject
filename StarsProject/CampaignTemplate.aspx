<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignTemplate.aspx.cs" Inherits="StarsProject.CampaignTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //$('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            //$('.timepicker').timepicker();
        });
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }

        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function showErrorMessage(strMess) {
            jQuery.confirm({ title: 'Data Validation', content: 'Are you sure, You want to delete record !', type: 'red', typeAnimated: true });
        }

        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtCampaignSubject").val().trim() == "" || jQuery("#txtCampaignHeader").val().trim() == "") {

                if (jQuery("#txtCampaignSubject").val().trim() == "")
                    errmsg += "<li>Subject is required</li>";

                if (jQuery("#txtCampaignHeader").val().trim() == "")
                    errmsg += "<li>Content is required</li>";

                validflag = false;
            }
            // ----------------------------------------
            if (validflag == false) {
                showcaseError(errmsg);
            }
            return validflag;
        }

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnPkIDEmp").val('');
        }

        function showFileExtError(xMode) {
            if (xMode == 'image')
                alert("Only BMP, JPEG, GIF and PNG file is valid for Attachment !");
            if (xMode == 'pdf')
                alert("Only PDF file attachment is valid for Upload !");
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper" style="padding:20px 15px;">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <asp:HiddenField ID="hdnCampaignID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnCategory" runat="server" ClientIDMode="Static" Value="Email"/>
                        <asp:HiddenField ID="hdnFileName" runat="server" ClientIDMode="Static" Value="Email"/>
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <label id="lblSubject" runat="server" clientidmode="Static" class="active" for="txtCampaignSubject">Mail Subject</label>
                                            <asp:TextBox ID="txtCampaignSubject" runat="server" ClientIDMode="Static" CssClass="form-control" TabIndex="1" MaxLength="50" placeholder=" " />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <label id="lblContent" runat="server" clientidmode="Static" class="active" for="txtCampaignHeader">Mail Content</label>
                                            <asp:TextBox ID="txtCampaignHeader" runat="server" ClientIDMode="Static" CssClass="textarea" TabIndex="1" MaxLength="50" TextMode="MultiLine" Height="150" placeholder=" " />
                                        </div>
                                    </div>
                                    <div id="divSignOff" runat="server" clientidmode="Static" class="row">
                                        <div class="input-field col m12">
                                            <label class="active" for="txtCampaignFooter">Mail SignOff</label>
                                            <asp:TextBox ID="txtCampaignFooter" runat="server" ClientIDMode="Static" CssClass="textarea" TabIndex="1" MaxLength="50" TextMode="MultiLine" Height="150" placeholder=" " />
                                        </div>
                                    </div>
                                </div>
                                <div id="divImage" runat="server" clientidmode="Static" class="row">
                                    <div class="input-field col m12">
                                        <div class="col m12 well" style="border: 1px solid gray; padding: 10px; border-radius: 4px; height: 100%;">
                                            <div id="divImageControl1 col m12">
                                                <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" TabIndex="4" />
                                            </div>
                                            <div id="divImageControl2 col m12">
                                                <asp:Image ID="imgProduct" runat="server" ClientIDMode="Static" Width="100" Height="100" ImageUrl="images/noimage.png" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:40px;">
                        <button ID="btnReset" type="button" runat="server"  clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" TabIndex="36"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="38"><i class="material-icons left">save</i>Save</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>



