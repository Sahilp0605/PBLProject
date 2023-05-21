<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneralTemplate.aspx.cs" Inherits="StarsProject.GeneralTemplate" %>
<%@ Register Src="~/PaggingUserControl.ascx" TagName="page" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <link href="css/font-awesome-Editor.min.css" rel="stylesheet" />

    <link href="css/richtext.min.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.richtext.min.js"></script>

    <style type="text/css">
        .jqte { margin-top: 5px;}
        .jqte_editor, .jqte_source {
            min-height: 300px !important;
        }
        .required:after {
            content: " *";
            color: #f00;
        } 
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            
            $('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            $('.timepicker').timepicker();

            $(".content").richText();
        });

        /*---------------------------------------------------------*/
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

        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnOldUserID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnEditor" runat="server" ClientIDMode="Static" />
        
        <div id="contentwrapper" class="contentwrapper">
            <div id="basicform" class="subcontent">
                <div class="row">
                   
                    <div class="col m12">
                        <div class="card card-body padding-2">
                            <div class="row">
                                <div class="input-field col m12">
                                    <label for="txtSubject" class="active required">Subject</label>
                                    <asp:TextBox ID="txtSubject" runat="server" class="form-control" MaxLength="1500" TabIndex="1" placeholder=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m12">
                                    <label for="txtEditor" class="active required">Email Content</label>
<%--                                    <asp:TextBox ID="txtEditor" TextMode="MultiLine" runat="server" CssClass="jqte-test" onblur="Test()" TabIndex="6"></asp:TextBox>                           --%>
                                    <asp:TextBox ID="txtEditor" runat="server" TabIndex="2" class="form-control" TextMode="MultiLine" placeholder="" EnableViewState="true" ClientIDMode="Static"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                    <button ID="btnReset" type="button" runat="server"  clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="4"><i class="material-icons left">backspace</i>Clear & Add New</button>
                    <button ID="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="3"><i class="material-icons left">save</i>Save</button>
                </div>
            </div>
        </div>
<%--        <script type="text/javascript">
            jQuery(".jqte-test").jqte({
                blur: function () {
                document.getElementById('<%=hdnEditor.ClientID %>').value = document.getElementById('<%= txtEditor.ClientID %>').value;
            }
            });
            // settings of status
            var jqteStatus = true;
        </script>--%>

    </form>
</body>
</html>

