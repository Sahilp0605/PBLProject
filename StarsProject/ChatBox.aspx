<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChatBox.aspx.cs" Inherits="StarsProject.ChatBox" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png" />
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-chat.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="app-assets/vendors/perfect-scrollbar/perfect-scrollbar.css" rel="stylesheet" />
    <script src="app-assets/vendors/perfect-scrollbar/perfect-scrollbar.min.js"></script>
    <script type="text/javascript">

        function pageLoad(sender, args) {
            getUpdatedChat();
        }
        function getUpdatedChat() {
            var loginUser = $("#hdnLoginUserID").val();
            var currentUser = $("#hdnCurrentUserID").val();
            var userData = JSON.stringify({ "pFrom": loginUser, "pTo": currentUser });
            $.ajax({
                type: "POST",
                url: "/Services/NagrikService.asmx/webChatBoxList",
                data: userData,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) { },
                failure: function (errMsg) { alert(errMsg); }
            });
        }
        setInterval(getUpdatedChat, 5000);

        function setChatFocus() {
            //alert($(".chat-area > .chats").height())
            $(".chat-area").scrollTop(5000);
        }

        function insertChat() {
            //var message = $(".message").val();
            //if (message != "") {
            //    var html = '<div class="chat-text">' + "<p>" + message + "</p>" + "</div>";
            //    $(".chat:last-child .chat-body").append(html);
            //    $(".message").val("");
            //    $(".chat-area").scrollTop($(".chat-area > .chats").height());
            //}
        }

        function setCurrentUser(currUser) {
            alert(currUser)
        }
    </script>

    <style type="text/css">
        ::-webkit-scrollbar {
            width: 0px;
            background: transparent;
        }
    </style>
</head>
<body>
    <form id="frmChatBox" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCurrentUserID" runat="server" ClientIDMode="Static" />

<%--        <asp:UpdatePanel ID="updChatBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
                <!-- BEGIN: Page Main-->
                <div id="main" class="main-full">
                    <div class="row">
                        <div class="content-wrapper-before gradient-45deg-indigo-purple"></div>
                        <div class="col s12">
                            <div class="container">
                                <div class="chat-application">
                                    <div class="app-chat">
                                        <div class="content-area content-right">
                                            <div class="app-wrapper">
                                                <!-- Sidebar menu for small screen -->
                                                <a href="#" data-target="chat-sidenav" class="sidenav-trigger hide-on-large-only">
                                                    <i class="material-icons">menu</i>
                                                </a>
                                                <!--/ Sidebar menu for small screen -->

                                                <div class="card card card-default scrollspy border-radius-6 fixed-width">
                                                    <div class="card-content chat-content p-0">
                                                        <!-- Sidebar Area -->
                                                        <div class="sidebar-left sidebar-fixed">
                                                            <div class="sidebar">
                                                                <div class="sidebar-content">
                                                                    <div id="sidebar-list" class="sidebar-menu chat-sidebar list-group position-relative">
                                                                        <div class="sidebar-list-padding app-sidebar sidenav" id="chat-sidenav">
                                                                            <!-- Sidebar Header -->
                                                                            <div class="sidebar-header" style="background-color: navy;">
                                                                                <div class="row valign-wrapper">
                                                                                    <div class="col s2 media-image pr-0">
                                                                                        <img id="imgLoginUser" runat="server" src="images/customer.png" alt=""
                                                                                            class="circle z-depth-2 responsive-img" />
                                                                                    </div>
                                                                                    <div class="col s10">
                                                                                        <p class="m-0 white-text text-darken-4 font-weight-700">
                                                                                            <span id="spnLoginUserName" runat="server"></span>
                                                                                        </p>
                                                                                        <p class="m-0 white-text"><span id="spnLoginDesignation" runat="server"></span></p>
                                                                                    </div>
                                                                                </div>
                                                                                <span class="option-icon">
                                                                                    <i class="material-icons white-text">more_vert</i>
                                                                                </span>
                                                                            </div>
                                                                            <!--/ Sidebar Header -->

                                                                            <!-- Sidebar Content List -->
                                                                            <div class="sidebar-content sidebar-chat">
                                                                                <div class="chat-list">
                                                                                    <asp:UpdatePanel ID="updUserList" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                            <asp:Repeater ID="rptUserList" runat="server" ClientIDMode="Static" OnItemDataBound="rptUserList_ItemDataBound" OnItemCommand="rptUserList_ItemCommand">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdnUserID" runat="server" ClientIDMode="Static" Value='<%#Eval("UserID")%>' />
                                                                                                    <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" CommandArgument='<%# Eval("UserID") %>' OnClientClick='<%# "setCurrentUser(" +Eval("UserID") + " );" %>'>
                                                                                                    <%--<a id="lnkSelect" href='<%#Eval("UserID","javascript:setCurrentUser(\"{0}\")")%>'>--%>
                                                                                                        <div id="divUser" name='<%# string.Format("div{0}", Eval("UserID")) %>' runat="server" clientidmode="static" class="chat-user">
                                                                                                            <div class="user-section">
                                                                                                                <div class="row valign-wrapper">
                                                                                                                    <div class="col s2 media-image online pr-0">
                                                                                                                        <img id="imgUserList" src='<%# Eval("EmployeeImage") %>' alt="" class="circle z-depth-2 responsive-img" />
                                                                                                                    </div>
                                                                                                                    <div class="col s10">
                                                                                                                        <p class="m-0 blue-grey-text text-darken-4 font-weight-700 text-capitalize"><%#Eval("EmployeeName") %></p>
                                                                                                                        <p class="m-0 info-text text-capitalize "><%#Eval("RoleCode") %></p>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="info-section">
                                                                                                                <div class="star-timing">
                                                                                                                    <div class="favorite">
                                                                                                                        <i class="material-icons amber-text">star</i>
                                                                                                                    </div>
                                                                                                                    <div class="time">
                                                                                                                        <span><%#Eval("LastTimestamp", "{0:dd/MM HH:mm}") %></span>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <span id="spnUnread" runat="server" class="badge pill red"><%#Eval("UnreadMessageCount") %></span>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    <%--</a>--%>
                                                                                                    </asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:Repeater>
                                                                                    </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                                <div class="no-data-found">
                                                                                    <h6 class="center">No Results Found</h6>
                                                                                </div>
                                                                            </div>
                                                                            <!--/ Sidebar Content List -->
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!--/ Sidebar Area -->

                                                        <!-- Content Area -->
                                                        <div class="chat-content-area">
                                                            <!-- Chat header -->
                                                            <div class="chat-header">
                                                                <div class="row valign-wrapper">
                                                                    <div class="col media-image online pr-0">
                                                                        <img id="imgCurrUser" runat="server" src="images/customer.png" alt="" class="circle z-depth-2 responsive-img" />
                                                                    </div>
                                                                    <div class="col">
                                                                        <p class="m-0 blue-grey-text text-darken-4 font-weight-700"><span id="spnCurrentUser" runat="server"></span></p>
                                                                        <p class="m-0 chat-text truncate"><span id="spnCurrentDesig" runat="server">Last Seen On : </span></p>
                                                                    </div>
                                                                </div>
                                                                <span class="option-icon">
                                                                    <span class="favorite">
                                                                        <i class="material-icons">star_outline</i>
                                                                    </span>
                                                                    <i class="material-icons">delete</i>
                                                                    <i class="material-icons">more_vert</i>
                                                                </span>
                                                            </div>
                                                            <!--/ Chat header -->
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                    <!-- Chat content area -->
                                                                    <div class="chat-area">
                                                                        <div class="chats" style="height: 250px; overflow-y: scroll !important;">
                                                                            <div class="chats">
                                                                                <asp:Repeater ID="rptChatBoxList" runat="server" ClientIDMode="Static" OnItemDataBound="rptChatBoxList_ItemDataBound">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnFlag" runat="server" ClientIDMode="Static" Value='<%#Eval("Flag") %>' />
                                                                                        <div id="dvItem" runat="server" clientidmode="static" class="chat chat-right">
                                                                                            <div class="chat-avatar">
                                                                                                <a class="avatar">
                                                                                                    <img id="imgChatUser" runat="server" src="images/customer.png" class="circle" alt="avatar" />
                                                                                                    <span style="width: 100%; display: inline-block; color: maroon; font-weight: bold; font-size: 8px;"><%#Eval("CreatedDate", "{0:dd/MM/yyyy}") %></span>
                                                                                                </a>
                                                                                            </div>
                                                                                            <div class="chat-body">
                                                                                                <div class="chat-text">
                                                                                                    <p><%#Eval("Message") %></p>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>

                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                                <asp:Button ID="btnFocus" runat="server" ClientIDMode="Static" Style="width: 1px !important; height: 1px !important; border: none;" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <!--/ Chat content area -->

                                                                    <!-- Chat footer <-->
                                                                    <div class="chat-footer">
                                                                        <asp:TextBox ID="txtMessage" runat="server" ClientIDMode="Static" placeholder="Type message here.." class="message mb-0" Style="width: 80%;" />
                                                                        <button type="button" id="btnSave" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="12"><i class="material-icons left">send</i>Send</button>
                                                                    </div>
                                                            </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <!--/ Chat footer -->
                                                        </div>
                                                        <!--/ Content Area -->
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="content-overlay"></div>
                        </div>
                    </div>
                </div>
                <!-- END: Page Main-->
<%--        </ContentTemplate>
        </asp:UpdatePanel>--%>

        <script src="app-assets/js/vendors.min.js"></script>
        <script src="app-assets/js/plugins.js"></script>
        <script src="app-assets/js/search.js"></script>
        <script src="app-assets/js/custom/custom-script.js"></script>
        <script src="app-assets/js/scripts/customizer.js"></script>
        <script src="app-assets/js/scripts/app-chat.js"></script>
    </form>
</body>
</html>
