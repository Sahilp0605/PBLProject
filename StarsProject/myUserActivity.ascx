<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myUserActivity.ascx.cs" Inherits="StarsProject.myUserActivity" %>

<style type="text/css">
    .widget-user .widget-user-header {
        padding: 5px 0px 5px 5px;
        height: 45px;
        border-top-left-radius: .25rem;
        border-top-right-radius: .25rem;
    }

    .widget-user .widget-user-image {
        position: relative;
        top: -65px;
        left: 96%;
        margin-left: -50px;
    }

        .widget-user .widget-user-image > img {
            width: 60px;
            height: auto;
            border: 3px solid #fff;
        }

    .widget-user-username {
        text-transform: capitalize;
        color: navy;
        float: left;
        padding-left: 3px;
    }

    .widget-user-desc {
        text-transform: capitalize;
        color: maroon;
        float: right;
        padding-right: 5px;
    }

    .card-widget {
        margin-right: 3px;
        min-height: 30px;
        min-width: 90px;
        text-align: center;
        border-radius: 6px;
        padding: 0px;
        box-shadow: 0 3px 6px rgba(0,0,0,.16), 0 3px 6px rgba(0,0,0,.23);
        background-color: #f0f8ff;
    }

    .description-text {
        display: block;
        color: navy;
        font-size: 13px;
    }

    .card-widget .row .col-sm-3 {
        text-align: center;
        padding: 3px;
        box-shadow: 0 3px 6px rgba(0,0,0,.16), 0 3px 6px rgba(0,0,0,.23);
        background-color: #f0f8ff;
    }

    .card-widget .row .col-sm-6 {
        text-align: center;
        padding: 3px;
        box-shadow: 0 3px 6px rgba(0,0,0,.16), 0 3px 6px rgba(0,0,0,.23);
        background-color: #f0f8ff;
    }

    .elevation-2 {
        box-shadow: 0 3px 6px rgba(0,0,0,.16), 0 3px 6px rgba(0,0,0,.23);
    }

    .img-circle {
        border-radius: 0%;
    }

    .badgeCircle {
        border-radius: 50px !important;
        padding: 10px !important;
    }

    h2 {
        margin-top: 10px;
        margin-bottom: 10px;
    }
</style>

<script type="text/javascript">
    function ShowPDFfile(repFilename) {
        var today = new Date();
        var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
        var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        var dateTime = date + ' ' + time;

        yhooWin = window.open(repFilename + "?id=" + dateTime, "ywin", "width=1050,height=750");
        yhooWin.focus();
    }

    function GenerateDailyReport() {
        var d1 = $("#txtDailyStartDate").val();
        var d2 = $("#txtDailyEndDate").val();
        //var uid = "admin";
        var uid = $("#hdnUserID").val();
        jQuery.ajax({
            type: "POST",
            url: 'Services/DashboardService.asmx/webGenerateDailyReport',
            data: '{startdt:\'' + d1 + '\',enddt:\'' + d2 + '\',loginuserid:\'' + uid + '\'}',
            contentType: "application/json; charset=utf-8",
            success: function (data11) {
                debugger;
                var retval = (data11.d).replace("/", "-");
                if (retval != "")
                    ShowPDFfile('PDF/' + retval)
            }
        });
    }
</script>
<asp:HiddenField ID="hdnView1" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth1" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear1" runat="server" ClientIDMode="Static" EnableViewState="true" />

<asp:HiddenField ID="hdnFromDate" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnToDate" runat="server" ClientIDMode="Static" EnableViewState="true" />

<asp:HiddenField ID="hdnUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnDailyStartDate" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnDailyEndDate" runat="server" ClientIDMode="Static" EnableViewState="true" />
<div class="card-widget widget-user mb-1" style="padding-top: 10px; background: linear-gradient(45deg, #c5e1a5, #fff8e1) !important; background-color: none; box-shadow: -6px 8px 6px -6px #cbcef5; min-height: 225px;">
    <div class="chat-user animate fadeUp delay-1">
        <div class="user-section">
            <div class="row">
                <div class="col m4">
                    <div class="row">
                        <div class="col m3">
                            <img src="images/noimage.png" alt="" class="circle z-depth-5 responsive-img p-1" width="80" height="80" />
                        </div>
                        <div class="col m9 mt-1 left-align">
                            <p class="m-0 blue-grey-text text-darken-4 font-weight-700">
                                <span id="spnUserEmployeeName" runat="server" clientidmode="static" />
                            </p>
                            <p class="m-0 info-text">
                                <span id="spnUserDesignation" runat="server" clientidmode="static" />
                            </p>
                        </div>
                    </div>
                    <div id="dvDailyReport" runat="server" clientidmode="static">
                        <div class="row" style="margin: 15px 10px 0px;">
                            <div class="input-field col m6">
                                <label class="active" for="txtDailyStartDate">Start Date <span class="materialize-red-text font-weight-800">*</span></label>
                                <asp:TextBox ID="txtDailyStartDate" runat="server" class="form-control" ClientIDMode="Static" placeholder="" TabIndex="2" TextMode="Date" />
                            </div>
                            <div class="input-field col m6">
                                <label class="active" for="txtDailyEndDate">End Date <span class="materialize-red-text font-weight-800">*</span></label>
                                <asp:TextBox ID="txtDailyEndDate" runat="server" class="form-control" ClientIDMode="Static" placeholder="" TabIndex="2" TextMode="Date" />
                            </div>
                        </div>
                        <div class="row col m6">
                            <div class="gradient-45deg-blue-grey-blue-grey gradient-shadow center white-text" style="min-height: 40px; border-radius: 5px; padding-top: 7px;">
                                <a class="white-text" href="javascript:GenerateDailyReport();">Generate Daily Report</a>
                            </div>
                        </div>
                        <div class="row col m6">
                            <div style="min-height: 40px; border-radius: 5px;">
                                <button ID="btnLoadUserActivity" type="button" runat="server" clientidmode="Static" Text="Save" class="btn gradient-45deg-blue-grey-blue-grey gradient-shadow center white-text" onserverclick="btnLoadUserActivity_Click" TabIndex="37" style="width: 220px; height:40px; margin-left:10px; background-color: transparent !important;"><i class="material-icons left">save</i>Load Activity</button>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="col m8">
                    <table id="tblUserAct" class="table table-striped table-bordered" style="font-size: 12px;" cellpadding="0" cellspacing="0" border="0" width="100%">
                        <asp:Repeater ID="rptUserActivity" runat="server" ClientIDMode="Static">
                            <ItemTemplate>
                                <div class="card-widget widget-user mb-1" style="padding-top: 10px; background: linear-gradient(45deg, #c5e1a5, #fff8e1) !important; background-color: none; box-shadow: -6px 8px 6px -6px #cbcef5; min-height: 225px;">
                                    <div class="chat-user animate fadeUp delay-1">
                                        <div class="user-section">
                                            <div class="row">
                                                <div class="col m12">
                                                    <div class="carousel carousel-slider center mb-1 gradient-45deg-yellow-teal" data-indicators="true" style="height: 225px !important;">
                                                        <div class="carousel-item gradient-45deg-indigo-light-blue white-text" href="#">
                                                            <div class="row col m12">
                                                                <h2 class="white-text">Lead Management</h2>
                                                            </div>
                                                            <div class="row col m12 center" style="padding: 20px 10px;">
                                                                <a href="javascript:viewUserContactList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrContacts" runat="server" ClientIDMode="Static" Text='<%#Eval("Contacts") %>' />
                                                                            <span class="description-text">Contacts</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a class="img-circle" href="javascript:viewUserFollowUpList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrFollowUp" runat="server" ClientIDMode="Static" Text='<%#Eval("Followup") %>' />
                                                                            <span class="description-text">Follow Up</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserInquiryList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrInquiry" runat="server" ClientIDMode="Static" Text='<%#Eval("Inquiry") %>' />
                                                                            <span class="description-text">Inquiry</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserQuotationList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrQuotation" runat="server" ClientIDMode="Static" Text='<%#Eval("Quotation") %>' />
                                                                            <span class="description-text">Quotation</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserSaleOrderList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrSaleOrder" runat="server" ClientIDMode="Static" Text='<%#Eval("SalesOrder") %>' />
                                                                            <span class="description-text">Sales Order</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                        </div>
                                                        <div class="carousel-item gradient-45deg-indigo-light-blue white-text" href="#two!">
                                                            <div class="row col m12">
                                                                <h2 class="white-text">Office Management</h2>
                                                            </div>
                                                            <div class="row col m12 center" style="padding: 20px 10px;">
                                                                <a href="javascript:viewUserToDOList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrToDO" runat="server" ClientIDMode="Static" Text='<%#Eval("ToDO") %>' />
                                                                            <span class="description-text">TO-DO</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserDailyActivity('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrDailyActivity" runat="server" ClientIDMode="Static" Text='<%#Eval("DailyActivity") %>' />
                                                                            <span class="description-text">Work Log</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserLoginLogoutList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrLogIn" runat="server" ClientIDMode="Static" Text='<%#Eval("login_logout") %>' />
                                                                            <span class="description-text">Login</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserLatePunch('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrLatePunch" runat="server" ClientIDMode="Static" Text='<%#Eval("LatePunch") %>' />
                                                                            <span class="description-text">Late Punch</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserLeaveRequestList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="ltrLeaveReq" runat="server" ClientIDMode="Static" Text='<%#Eval("Leave") %>' />
                                                                            <span class="description-text">Leaves</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                        </div>
                                                        <div class="carousel-item gradient-45deg-indigo-light-blue white-text" href="#three!">
                                                            <div class="row col m12">
                                                                <h2 class="white-text">Accounts & Inventory Management</h2>
                                                            </div>
                                                            <div class="row col m12 center" style="padding: 20px 10px;">
                                                                <a href="javascript:viewUserSaleBillList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="Literal1" runat="server" ClientIDMode="Static" Text='<%#Eval("SalesInvoice") %>' />
                                                                            <span class="description-text">Sales</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserPurchaseBillList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="Literal2" runat="server" ClientIDMode="Static" Text='<%#Eval("PurchaseInvoice") %>' />
                                                                            <span class="description-text">Purchase</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserInwardList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="Literal3" runat="server" ClientIDMode="Static" Text='<%#Eval("Inward") %>' />
                                                                            <span class="description-text">Inward</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                                <a href="javascript:viewUserOutwardList('<%# Eval("UserID")%>', '<%# Eval("EmployeeName")%>',document.getElementById('txtDailyStartDate').value,document.getElementById('txtDailyEndDate').value);" style="display: inline-block;">
                                                                    <div class="card-widget description-block left gradient-45deg-light-green-amber gradient-shadow">
                                                                        <h5 class="description-header">
                                                                            <asp:Literal ID="Literal4" runat="server" ClientIDMode="Static" Text='<%#Eval("Outward") %>' />
                                                                            <span class="description-text">Outward</span>
                                                                        </h5>
                                                                    </div>
                                                                </a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>


