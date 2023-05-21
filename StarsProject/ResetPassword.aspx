<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="StarsProject.resetpassword" %>

<!DOCTYPE html>

<html class="loading" lang="en" data-textdirection="ltr">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui">
    <meta name="description" content="Materialize is a Material Design Admin Template,It's modern, responsive and based on Material Design by Google.">
    <meta name="keywords" content="materialize, admin template, dashboard template, flat admin template, responsive admin template, eCommerce dashboard, analytic dashboard">
    <meta name="author" content="ThemeSelect">
    <title>User-Reset Password</title>
    <script type="text/javascript" src="js/plugins/jquery-1.7.min.js"></script>
    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png">
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <!-- BEGIN: VENDOR CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css">
    <!-- END: VENDOR CSS-->
    <!-- BEGIN: Page Level CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css">
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css">
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/register.css">
    <!-- END: Page Level CSS-->
    <!-- BEGIN: Custom CSS-->
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css">
    <!-- END: Custom CSS-->
<%--    <link rel="stylesheet" type="text/css" href="app-assets/vendors/sweetalert/sweetalert.css">--%>
<%--    <script src="app-assets/vendors/sweetalert/sweetalert.min.js"></script>--%>
    <link href="css/jquery-confirm.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-confirm.js"></script>
    <script type="text/javascript">
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
        .swal-icon {
          width: 100px !important;
          height: 100px !important;
          border-width: .25em !important;
        }
    </style>
</head>
<!-- END: Head-->
<body class="vertical-layout vertical-menu-collapsible page-header-dark vertical-modern-menu preload-transitions 1-column register-bg   blank-page blank-page" data-open="click" data-menu="vertical-modern-menu" data-col="1-column">
    <div class="row">
        <div class="col s12">
            <div class="container">
                <div id="register-page" class="row">
                    <div class="col s12 m6 l4 z-depth-4 card-panel border-radius-6 register-card bg-opacity-8" style="background-color:aliceblue;">
                        <form class="login-form" runat="server">
                            <div class="row">
                                <div class="input-field col s12">
                                    <h5 class="ml-4">Reset Password</h5>
                                </div>
                            </div>
                            <div class="row margin">
                                <div class="input-field col s12">
                                    <i class="material-icons prefix pt-2">person_outline</i>
                                    <asp:TextBox ID="username" runat="server" MaxLength="30"></asp:TextBox>
                                    <label for="username" class="center-align">User ID</label>
                                </div>
                            </div>
                            <div class="row margin">
                                <div class="input-field col s12">
                                    <i class="material-icons prefix pt-2">lock_outline</i>
                                    <asp:TextBox ID="oldpassword" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
                                    <label for="oldpassword">Old Password</label>
                                </div>
                            </div>
                            <div class="row margin">
                                <div class="input-field col s12">
                                    <i class="material-icons prefix pt-2">lock_outline</i>
                                    <asp:TextBox ID="newpassword1" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
                                    <label for="newpassword1">New Password</label>
                                </div>
                            </div>
                            <div class="row margin">
                                <div class="input-field col s12">
                                    <i class="material-icons prefix pt-2">lock_outline</i>
                                    <asp:TextBox ID="newpassword2" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
                                    <label for="newpassword2">Confirm Password</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s12">
                                    <asp:Button ID="btnSubmit" runat="server" class="btn waves-effect waves-light border-round gradient-45deg-purple-deep-orange col s12" Text="Submit Password" ToolTip="Sign In" onclick="btnSubmit_Click"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s12">
                                  <p class="margin medium-small"><a href="default.aspx">Already have an account? Login</a></p>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
            <div class="content-overlay"></div>
        </div>
    </div>
    <script src="app-assets/js/vendors.min.js"></script>
    <script src="app-assets/js/plugins.js"></script>
    <script src="app-assets/js/search.js"></script>
    <script src="app-assets/js/custom/custom-script.js"></script>
</body>
</html>
