<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StarsProject.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>E-Office-Desk :: Login</title>
<%--    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x1521.png" />
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x321.png" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />--%>
    

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
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/login.css" />

    

    <script type="text/javascript">
        localStorage.removeItem('userMenuList');
        window.history.forward(-1);

        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
    </script>

    <style type="text/css">
        input[type='submit'] { height:45px; width:100%; }
        .btn { padding:0px !important; }
        .input-field .prefix { top:-0.5rem !important; }
    </style>
</head>
   
<body class="vertical-layout vertical-menu-collapsible page-header-dark vertical-modern-menu preload-transitions 1-column login-bg blank-page blank-page" data-open="click" data-menu="vertical-modern-menu" data-col="1-column">
    <div class="row">
        <div class="col s12">
            <div class="container">
                <div id="login-page" class="row">
                    <div class="col s12 m6 l4 z-depth-4 card-panel border-radius-6 login-card bg-opacity-8" style="background-color:aliceblue;">
                        <form id="login" class="login-form" runat="server">
                           
                            <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
                            
                             <div class="row">
                                <div class="input-field col s12">
                                    <h5 class="ml-4">Sign in</h5>
                                </div>
                            </div>
                            <div class="row margin">
                                <div class="input-field col s12">
                                    <i class="material-icons prefix pt-2">person_outline</i>
                                    <asp:TextBox ID="username" runat="server" MaxLength="30" placeholder=""></asp:TextBox>
                                    <label for="username" class="center-align">User ID</label>
                                </div>
                            </div>
                            <div class="row margin">
                                <div class="input-field col s12">
                                    <i class="material-icons prefix pt-2">lock_outline</i>
                                    <asp:TextBox ID="password" runat="server" MaxLength="20" TextMode="Password" placeholder=""></asp:TextBox>
                                    <label for="password">Password</label>
                                </div>
                            </div>
                            <div id="divLoginAs" class="row margin" runat="server" clientidmode="Static">
                                <div class="input-field col s12">
                                    <i class="material-icons prefix pt-2">person</i>
                                    <label class="active" for="drpLoginAs">Login As</label>
                                    <asp:DropDownList ID="drpLoginAs" class="select2-theme browser-default" runat="server" ClientIDMode="Static" style="margin-left:45px;width: 91%;">
                                        <asp:ListItem Text="Employee" Value="Employee" />
                                        <asp:ListItem Text="Dealer/Re-Seller" Value="Dealer" />
                                    </asp:DropDownList>
                                    
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col s12">
                                    <asp:Button ID="btnSubmit" runat="server" class="btn waves-effect waves-light border-round gradient-45deg-purple-deep-orange vertical-text-middle col s12" Text="Login" onclick="btnSubmit_Click" Style="padding:12px;"></asp:Button>
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

                            <%--Below section for AccuPanel --%>
<%--                            <div style="margin-top:100px; background-color:navy; width:100%; position:absolute; left:0px; bottom:0px;">
                                <h4 style="text-align:left; font-weight:bold; padding-left:20px;">
                                    <a class="white-text" href="Default.aspx?mode=webcom">Generate Ticket</a>
                                </h4>
                            </div>   --%>
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
