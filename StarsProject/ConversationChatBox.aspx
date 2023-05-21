<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConversationChatBox.aspx.cs" Inherits="StarsProject.ConversationChatBox" %>

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
        function EnterKeyFilter() {
            if (window.event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                document.getElementById('btnSave').focus();
                document.getElementById('btnSave').click();
            }
        }
        window.addEventListener('keydown', EnterKeyFilter)
        //function pageLoad(sender, args) {
        //    getUpdatedChat();
        //}
        //function getUpdatedChat() {
        //    var loginUser = $("#hdnLoginUserID").val();
        //    var currentUser = $("#hdnCurrentUserID").val();
        //    var userData = JSON.stringify({ "pFrom": loginUser, "pTo": currentUser });
        //    $.ajax({
        //        type: "POST",
        //        url: "/Services/NagrikService.asmx/webChatBoxList",
        //        data: userData,
        //        contentType: 'application/json; charset=utf-8',
        //        dataType: 'json',
        //        success: function (data) { },
        //        failure: function (errMsg) { alert(errMsg); }
        //    });
        //}
        //setInterval(getUpdatedChat, 5000);

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
    <form id="frmConversationLog" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnKeyValue" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnModuleName" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCurrentUserID" runat="server" ClientIDMode="Static" />

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
                                                <!-- Content Area -->
                                                <div class="chat-content-area">
                                                    <!-- Chat header -->
                                                    <div class="chat-header badge gradient-45deg-yellow-green black-text gradient-shadow">
                                                        <div class="row valign-wrapper">
                                                            <div class="col m8 media-image online p-1">
                                                                <h5>Project No : <span class="blue-text" id="spnProjectNo" runat="server"></span></h5>
                                                            </div>
                                                            <div class="col m4 float-right">
                                                                <span class="option-icon float-right">
                                                                    <i class="material-icons">more_vert</i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!--/ Chat header -->
                                        <%--            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>--%>
                                                            <!-- Chat content area -->
                                                            <div class="chat-area" style="background: url(images/chat-bg.jpg) repeat scroll 0 0;">
                                                                <div class="chats" style="height: 450px; overflow-y: scroll !important; padding:10px;">
                                                                    <div class="chats">
                                                                        <asp:Repeater ID="rptChatBoxList" runat="server" ClientIDMode="Static" OnItemDataBound="rptChatBoxList_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnFlag" runat="server" ClientIDMode="Static" Value='' />
                                                                                <asp:HiddenField ID="hdnFromUser" runat="server" ClientIDMode="Static" Value='<%#Eval("FromUser") %>' />
                                                                                <asp:HiddenField ID="hdnFromEmployee" runat="server" ClientIDMode="Static" Value='<%#Eval("FromEmployeeName") %>' />
                                                                                <asp:HiddenField ID="hdnToEmployee" runat="server" ClientIDMode="Static" Value='<%#Eval("ToEmployeeName") %>' />
                                                                                <div id="dvItem" runat="server" clientidmode="static" class="chat chat-right">
                                                                                    <div class="chat-avatar">
                                                                                        <a class="avatar">
                                                                                            <img id="imgChatUser" runat="server" src="images/customer.png" class="circle" alt="avatar" width="40" />
                                                                                            <span id="spnEmployee" runat="server" style="width: 100%; display: inline-block; color: navy; font-weight: bold; font-size: 12px; text-transform:capitalize;"><%#Eval("CreatedBy", "{0:dd/MM/yyyy}") %></span>
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
                                        <%--            </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
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
        <script src="app-assets/js/vendors.min.js"></script>
        <script src="app-assets/js/plugins.js"></script>
        <script src="app-assets/js/search.js"></script>
        <script src="app-assets/js/custom/custom-script.js"></script>
        <script src="app-assets/js/scripts/customizer.js"></script>
        <script src="app-assets/js/scripts/app-chat.js"></script>
    </form>
</body>
</html>

