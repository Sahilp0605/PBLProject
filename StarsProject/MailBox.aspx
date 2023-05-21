<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailBox.aspx.cs" Inherits="StarsProject.MailBox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png" />
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/quill/quill.snow.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-sidebar.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-email.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <style type="text/css">
        /*::-webkit-scrollbar {
            width: 0px;
            background: transparent; /* make scrollbar transparent */
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnMailCount" runat="server" ClientIDMode="Static" Value="0" />
        <!-- BEGIN: Page Main-->
        <div id="main" class="main-full">
            <div class="row">
                <div class="content-wrapper-before"></div>
                <div class="col s12">
                    <div class="container">
                        <!-- Sidebar Area Starts -->
                        <div class="email-overlay"></div>
                        <div class="sidebar-left sidebar-fixed">
                            <div class="sidebar">
                                <div class="sidebar-content">
                                    <div class="sidebar-header mt-2" style="top: 5px; height: 65px;">
                                        <div class="sidebar-details">
                                            <div class="row valign-wrapper mt-2 pt-2 animate fadeLeft">
                                                <div class="col s3 media-image">
                                                    <img id="imgEmployee" runat="server" src="images/customer.png" alt="" class="circle z-depth-2 responsive-img" />
                                                </div>
                                                <div class="col s9">
                                                    <p id="spnEmployeeName" runat="server" class="m-0 subtitle font-weight-700"></p>
                                                    <p id="spnEmailAddress" runat="server" class="m-0 text-muted"></p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="sidebar-list" class="sidebar-menu list-group position-relative animate fadeLeft" style="height: 400px; background-color: aliceblue; border: 1px solid silver; border-radius: 8px; margin-top: 10px;">
                                        <div class="sidebar-list-padding app-sidebar sidenav" id="email-sidenav">
                                            <ul class="email-list display-grid">
                                                <li class="sidebar-title">Folders</li>
                                                <li class="active"><a href="#!" class="text-sub"><i class="material-icons mr-1">mail_outline </i>Inbox</a>
                                                </li>
                                                <li><a href="#!" class="text-sub"><i class="material-icons mr-1">send </i>Sent</a></li>
                                                <li><a href="#!" class="text-sub"><i class="material-icons mr-1">description </i>Draft</a></li>
                                                <li><a href="#!" class="text-sub"><i class="material-icons mr-1">info_outline </i>Span</a></li>
                                                <li><a href="#!" class="text-sub"><i class="material-icons mr-1">delete </i>Trash</a></li>
                                            </ul>
                                        </div>
                                    </div>
                                    <a href="#" data-target="email-sidenav" class="sidenav-trigger hide-on-large-only"><i class="material-icons">menu</i></a>
                                </div>
                            </div>
                        </div>
                        <!-- Sidebar Area Ends -->

                        <!-- Content Area Starts -->
                        <div class="app-email">
                            <div class="content-area content-right mt-2">
                                <div class="app-wrapper">
                                    <div class="app-search">
                                        <i class="material-icons mr-2 search-icon">search</i>
                                        <input type="text" placeholder="Search Mail" class="app-filter" id="email_filter" />
                                    </div>
                                    <div class="card card card-default scrollspy border-radius-6 fixed-width">
                                        <div class="card-content p-0 pb-2" style="height:398px;">
                                            <div class="email-header">
                                                <div class="left-icons">
                                                    <span class="header-checkbox">
                                                        <label>
                                                            <input type="checkbox" onclick="toggle(this)" />
                                                            <span></span>
                                                        </label>
                                                    </span>
                                                    <span class="action-icons">
                                                        <i class="material-icons">refresh</i>
                                                        <i class="material-icons">mail_outline</i>
                                                        <i class="material-icons">label_outline</i>
                                                        <i class="material-icons">folder_open</i>
                                                        <i class="material-icons">info_outline</i>
                                                        <i class="material-icons delete-mails">delete</i>
                                                    </span>
                                                </div>
                                                <div class="list-content"></div>
                                                <div class="email-action">
                                                    <span class="email-options"><i class="material-icons grey-text">more_vert</i></span>
                                                </div>
                                            </div>
                                            <div class="collection email-collection" style="min-height:320px !important; height: 320px !important; max-height: 320px !important">
                                                <asp:Repeater ID="rptMailList" runat="server" ClientIDMode="Static">
                                                <ItemTemplate>
                                                    <div class="email-brief-info collection-item animate fadeUp delay-1">
                                                        <div class="list-left">
                                                            <label><input type="checkbox" name="foo" /><span></span></label>
                                                            <div class="favorite"><i class="material-icons">star_border</i></div>
                                                            <div class="email-label"><i class="material-icons">label_outline</i></div>
                                                        </div>
                                                        <a class="list-content" href="app-email-content.html">
                                                            <div class="list-title-area">
                                                                <div class="user-media">
                                                                    <div class="title"><%#Eval("MailSubject") %></div>
                                                                </div>
                                                                <div class="title-right">
                                                                    <span class="attach-file">
                                                                        <i class="material-icons">attach_file</i>
                                                                    </span>
                                                                    <span class="badge grey lighten-3"><i class="purple-text material-icons small-icons mr-2">fiber_manual_record </i>Note</span>
                                                                </div>
                                                            </div>
                                                            <div class="list-desc"><%#Eval("MailFrom") %></div>
                                                            <div class="row">
                                                                <div class="col m4"><%#Eval("MailDate") %></div>
                                                            </div>
                                                        </a>
                                                    </div>
                                                </ItemTemplate>
                                                </asp:Repeater> 
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Content Area Ends -->

                        <!-- Add new email popup -->
                        <div style="bottom: 54px; right: 19px;" class="fixed-action-btn direction-top">
                            <a class="btn-floating btn-large primary-text gradient-shadow compose-email-trigger" href="#">
                                <i class="material-icons">add</i>
                            </a>
                        </div>
                        <!-- Add new email popup Ends-->

                        <!-- email compose sidebar -->
                        <div class="email-compose-sidebar" style="width:500px !important;">
                            <div class="card quill-wrapper">
                                <div class="card-content" style="padding:10px;">
                                    <div class="card-header display-flex pb-2">
                                        <h3 class="card-title">NEW MESSAGE</h3>
                                        <div class="close close-icon">
                                            <i class="material-icons">close</i>
                                        </div>
                                    </div>
                                    <div class="divider"></div>
                                    <!-- form start -->

                                    <div class="input-field col m12">
                                        <input type="email" class="edit-email-item-title validate" id="edit-item-from" value="user@example.com" ReadOnly="true" placeholder="" />
                                        <label for="edit-item-from active">From</label>
                                    </div>
                                    <div class="input-field col m12">
                                        <input type="email" class="edit-email-item-date" id="edit-item-to" placeholder="" />
                                        <label for="edit-item-to active">To</label>
                                    </div>
                                    <div class="input-field col m12">
                                        <input type="text" class="edit-email-item-date" id="edit-item-subject" placeholder="" />
                                        <label for="edit-item-subject active">Subject</label>
                                    </div>
                                    <div class="input-field col m12">
                                        <input type="email" class="edit-email-item-date" id="edit-item-CC" placeholder=""/>
                                        <label for="edit-item-CC active">CC</label>
                                    </div>
                                    <div class="input-field col m12">
                                        <input type="email" class="edit-email-item-date" id="edit-item-BCC" placeholder=""/>
                                        <label for="edit-item-BCC active">BCC</label>
                                    </div>
                                    <!-- Compose mail Quill editor -->
                                    <div class="input-field col m12">
                                        <div class="snow-container mt-2">
                                            <div class="compose-editor"></div>
                                            <div class="compose-quill-toolbar">
                                                <span class="ql-formats mr-0">
                                                    <button class="ql-bold"></button>
                                                    <button class="ql-italic"></button>
                                                    <button class="ql-underline"></button>
                                                    <button class="ql-link"></button>
                                                    <button class="ql-image"></button>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="file-field input-field col m12">
                                        <div class="btn btn-file">
                                            <span>Attach File</span>
                                            <input type="file"/>
                                        </div>
                                        <div class="file-path-wrapper">
                                            <input class="file-path validate" type="text" style="display:none;"/>
                                        </div>
                                    </div>

                                    <div class="card-action pl-0 pr-0 right-align">
                                        <button type="reset" class="btn-small waves-effect waves-light cancel-email-item mr-1">
                                            <i class="material-icons left">close</i>
                                            <span>Cancel</span>
                                        </button>
                                        <button class="btn-small waves-effect waves-light send-email-item">
                                            <i class="material-icons left">send</i>
                                            <span>Send</span>
                                        </button>
                                    </div>
                                    <!-- form start end-->
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="content-overlay"></div>
                </div>
            </div>
        </div>
        <!-- END: Page Main-->

        <script src="app-assets/js/vendors.min.js"></script>
        <script src="app-assets/vendors/sortable/jquery-sortable-min.js"></script>
        <script src="app-assets/vendors/quill/quill.min.js"></script>
        <script src="app-assets/js/plugins.js"></script>
        <script src="app-assets/js/search.js"></script>
        <script src="app-assets/js/custom/custom-script.js"></script>
        <script src="app-assets/js/scripts/customizer.js"></script>
        <script src="app-assets/js/scripts/app-email.js"></script>

    </form>
</body>
</html>
