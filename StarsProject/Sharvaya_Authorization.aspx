<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sharvaya_Authorization.aspx.cs" Inherits="StarsProject.Sharvaya_Authorization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>E-Office-Desk :: Sharvaya</title>

    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/login.css" />

    <script type="text/javascript">
        function showcaseError(xMsg, xClass) {
            //alert(1);
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
    </script>

    <style type="text/css">
        input[type='submit'] {
            height: 45px;
            width: 100%;
        }

        .btn {
            padding: 0px !important;
        }

        .input-field .prefix {
            top: -0.5rem !important;
        }
    </style>
</head>

<body class="vertical-layout vertical-menu-collapsible page-header-dark vertical-modern-menu preload-transitions 1-column blank-page blank-page" data-open="click" data-menu="vertical-modern-menu" data-col="1-column">
    <div class="row">
        <div class="col s12">
            <div class="container">
                <div id="login-page" class="row">
                    <div class="col s12 m6 l4 z-depth-4 card-panel border-radius-6 login-card bg-opacity-8" style="background-color: aliceblue;">
                        <form id="login" class="login-form" runat="server">
                            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                            <div id="divLogin" runat="server" clientidmode="Static" visible="true">
                                <div class="row">
                                    <div class="input-field col s12">
                                        <h5 class="ml-4">Sign in</h5>
                                    </div>
                                </div>
                                <div class="row margin">
                                    <div class="input-field col s12">
                                        <i class="material-icons prefix pt-2">lock_outline</i>
                                        <asp:TextBox ID="txtpassword" runat="server" MaxLength="20" TextMode="Password" placeholder=""></asp:TextBox>
                                        <label for="password">Password</label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col s12">
                                        <asp:Button ID="btnSubmit" runat="server" class="btn waves-effect waves-light border-round gradient-45deg-purple-deep-orange vertical-text-middle col s12" Text="Submit" OnClick="btnSubmit_Click" Style="padding: 12px;"></asp:Button>
                                    </div>
                                </div>
                            </div>
                            <div id="divAuthorization" runat="server" clientidmode="Static" visible="false">
                                <div class="row">
                                    <div class="input-field col s12">
                                        <asp:Button ID="btnEncrypt" runat="server" class="btn waves-effect waves-light border-round gradient-45deg-purple-deep-orange vertical-text-middle col s12" Text="Encrypt Connection String" OnClick="btnEncrypt_Click" Style="padding: 12px;"></asp:Button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col s12">
                                        <asp:Button ID="btnDecrypt" runat="server" class="btn waves-effect waves-light border-round gradient-45deg-purple-deep-orange vertical-text-middle col s12" Text="Decrypt Connection String" OnClick="btnDecrypt_Click" Style="padding: 12px;"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
            <div class="content-overlay"></div>
        </div>
    </div>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
</body>
</html>
