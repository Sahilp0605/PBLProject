<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLockScreen.aspx.cs" Inherits="StarsProject.UserLockScreen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>E-Office-Desk :: Login</title>

    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x1521.png" />
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x321.png" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/login.css" />


    <script type="text/javascript">
        window.history.forward(-1);
        // ----------------------------------------------
        function showMyMessage(pTitle, str) {
            if (pTitle == 'Warning' || pTitle == 'Error')
                $.confirm({ title: pTitle, content: str, type: 'red', typeAnimated: true, useBootstrap: false, boxWidth: '25%' });
            if (pTitle == 'Success')
                $.confirm({ title: pTitle, content: str, type: 'green', typeAnimated: true, useBootstrap: false, boxWidth: '25%' });
            if (pTitle == 'Information' || pTitle == 'Message')
                $.confirm({ title: pTitle, content: str, type: 'blue', typeAnimated: true, useBootstrap: false, boxWidth: '25%' });
        }
    </script>

    <style type="text/css">
        input[type='submit'] { height:45px; width:100%; }
        .btn { padding:0px !important; }
        .input-field .prefix { top:-0.5rem !important; }
    </style>
</head>
   
<body class="vertical-layout vertical-menu-collapsible page-header-dark vertical-modern-menu preload-transitions 1-column login-bg   blank-page blank-page" data-open="click" data-menu="vertical-modern-menu" data-col="1-column">
    <div class="row">
        <div class="col s12">
            <div class="container">
                <div id="login-page" class="row">
                    <div class="col s12 m6 l4 z-depth-4 card-panel border-radius-6 login-card bg-opacity-8" style="background-color:aliceblue;">
                        <form id="login" class="login-form" runat="server">
                            <div class="row">
                                <div class="input-field col s12">
                                    <h5 class="ml-4">You Are On Rest !</h5>
                                </div>
                            </div>
                            <div class="row margin">
                                <div class="input-field col s12">
                                    <i class="material-icons prefix pt-2">lock_outline</i>
                                    <asp:TextBox ID="password" runat="server" MaxLength="20" TextMode="Password" placeholder=""></asp:TextBox>
                                    <label for="password">Password</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s12">
                                    <asp:Button ID="btnSubmit" runat="server" class="btn waves-effect waves-light border-round gradient-45deg-purple-deep-orange vertical-text-middle col s12" Text="Re-Login" onclick="btnSubmit_Click" Style="padding:12px;"></asp:Button>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s6 m6 l6">
                                    <p class="margin medium-small"><a href="forgotpassword.aspx">Forgot password ?</a></p>
                                </div>
                                <div class="input-field col s6 m6 l6">
                                    <p class="margin right-align medium-small">
                                        <a href="resetpassword.aspx">Reset Password</a>
                                    </p>
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

