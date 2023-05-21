<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="StarsProject.UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#myTab a").click(function () {
                var empid = $("#hdnEmployeeID").val();
                var currTab = $(this).attr("href").replace("#", "");
                if (currTab == 'personal') {
                    document.getElementById("ifrPersonal").src = "OrgEmployee.aspx?mode=Edit&type=profile1&id=" + empid;
                }
                else if (currTab == 'humanres') {
                    document.getElementById("ifrHuman").src = "OrgEmployee.aspx?mode=Edit&type=profile2&id=" + empid;
                }
                else if (currTab == 'useracc') {
                    var userid = $("#hdnLoginUserID").val();
                    document.getElementById("ifrUser").src = "UserMgmt.aspx?mode=Edit&type=profile1&id=" + userid;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" CompositeScript-ScriptMode="Inherit"></asp:ScriptManager>
    <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <!-- BEGIN: Page Main-->
    <div id="main" style="padding-left:0px;">
            <div class="content-wrapper-before gradient-45deg-indigo-purple"></div>
            <!-- Search for small screen-->
            <div class="container">
                <div class="row">
                    <div class="col s3 center" style="width:27%;">
                        <h5 class="breadcrumbs-title mt-0 mb-0"><span class="white-text">User Profile</span></h5>
                    </div>
                </div>
            </div>
            <div class="col s12">
                <div class="container">
                    <div class="section">
                        <div class="section" id="user-profile">
                            <div class="row">
                                <!-- User Profile Feed -->
                                <div class="col s12 m4 l3 user-section-negative-margin">
                                    <div class="row">
                                        <div class="col s12 center-align">
                                            <img id="imgUserProfile" runat="server" class="responsive-img circle z-depth-5" width="120" src="images/customer.png" alt="" />
                                            <br>
                                        </div>
                                    </div>

                                    <div class="row mt-5">
                                        <div class="col s12">
                                            <hr><h5 class="m-0">Followers</h5><hr>
                                            <h5 class="m-0"><a id="lnkFollower" runat="server" clientidmode="static" href="#">540</a></h5>
                                        </div>
                                    </div>
                                    
                                    <div class="row">
                                        <div class="col s12">
                                            <hr><h5 class="m-0">Work Accountability</h5><hr>
                                            <p class="m-0">Leads        :&nbsp;<a id="lnkInquiry" runat="server" clientidmode="static" href="#">42</a></p>
                                            <p class="m-0">Followups    :&nbsp;<a id="lnkFollowup" runat="server" clientidmode="static" href="#">42</a></p>
                                            <p class="m-0">Quotation    :&nbsp;<a id="lnkQuotation" runat="server" clientidmode="static" href="#">42</a></p>
                                            <p class="m-0">Sales Order  :&nbsp;<a id="lnkSalesOrder" runat="server" clientidmode="static" href="#">42</a></p>
                                            <p class="m-0">Sales Bill   :&nbsp;<a id="lnkSalesBill" runat="server" clientidmode="static" href="#">42</a></p>
                                            <p class="m-0">Purchase Bill:&nbsp;<a id="lnkPurcBill" runat="server" clientidmode="static" href="#">42</a></p>
                                            <p class="m-0">Customers    :&nbsp;<a id="lnkCustomers" runat="server" clientidmode="static" href="#">42</a></p>
                                        </div>
                                    </div>
                                </div>
                                <!-- User Post Feed -->
                                <div class="col s24 m20 l9">
                                    <div class="row">
                                        <div class="card user-card-negative-margin z-depth-0" id="feed" style="margin:0px;">
                                            <div class="card-content card-border-gray">
                                                <div class="row">
                                                    <div class="col s12">
                                                        <h5 class="no-margin"><asp:Label ID="lblUserName" runat="server" ClientIDMode="Static" Text="Mrunal Yoddha"/></h5>
                                                        <p><asp:Label ID="lblDesignation" runat="server" ClientIDMode="Static" Text="Technical Head"/></p>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col s12">
                                                        <ul id="myTab" class="tabs card-border-gray mt-2" runat="server" clientidmode="Static" enableviewstate="true">
                                                            <li class="tab col m4 s8 p-0"><a class="left active" href="#personal">Personal Info</a></li>
                                                            <li class="tab col m4 s8 p-0"><a class="left" href="#humanres">Human Resource</a></li>
                                                            <li class="tab col m4 s8 p-0"><a class="left" href="#useracc">User Account</a></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                                <div id="personal" class="row mt-1">
                                                    <div class="col s12">
                                                        <iframe id="ifrPersonal" runat="server" clientidmode="static" src="" style="width:100%; min-height:500px;"></iframe>
                                                    </div>
                                                </div>
                                                <div id="humanres" class="row mt-1">
                                                    <div class="col s12">
                                                        <iframe id="ifrHuman" runat="server" clientidmode="static" src="" style="width:100%; min-height:500px;"></iframe>
                                                    </div>
                                                </div>
                                                <div id="useracc" class="row mt-1">
                                                    <div class="col s12">
                                                        <iframe id="ifrUser" runat="server" clientidmode="static" src="" style="width:100%; min-height:500px;"></iframe>
                                                    </div>
                                                </div>
                                            </div>
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
    <!-- END: Page Main-->
</asp:Content>
