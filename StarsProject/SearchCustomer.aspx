<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchCustomer.aspx.cs" Inherits="StarsProject.SearchCustomer" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/myFollowupTimeline.ascx" TagPrefix="uc1" TagName="myFollowupTimeline" %>
<%@ Register Src="~/myClientDetailLedger.ascx" TagPrefix="uc1" TagName="myClientDetailLedger" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/datatables/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://code.jquery.com/ui/1.11.2/jquery-ui.min.js" />

<%--    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>


    <style type="text/css">
/* ==================================== */
        /*****         Modal Popup        *****/
        /* ==================================== */
        .Popup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 96%;
            height: 96%;
        }

        .btnTopRightCorner {
            display: block;
            box-sizing: border-box;
            width: 30px;
            height: 30px;
            border-width: 3px;
            border-style: solid;
            border-color: red;
            border-radius: 100%;
            background: -webkit-linear-gradient(-45deg, transparent 0%, transparent 46%, white 46%, white 56%,transparent 56%, transparent 100%), -webkit-linear-gradient(45deg, transparent 0%, transparent 46%, white 46%, white 56%,transparent 56%, transparent 100%);
            background-color: red;
            box-shadow: 0px 0px 5px 2px rgba(0,0,0,0.5);
            transition: all 0.3s ease;
            position: relative;
            top: 12px;
            right: 15px;
        }

        .text-input {
            position: relative;
            margin-top: 50px;
            input [type="text"];
        }

        input[type="text"] + label {
            position: absolute;
            top: 0;
            left: 0;
            bottom: 0;
            height: 40px;
            line-height: 40px;
            color: white;
            border-radius: 3px 0 0 3px;
            padding: 0 20px;
            background: #E03616;
            transform: translateZ(0) translateX(0);
            transition: all 0.3s ease-in;
            transition-delay: 0.2s;
        }

        input[type="text"]:focus + label {
            transform: translateY(-120%) translateX(0%);
            border-radius: 3px;
            transition: all 0.1s ease-out;
        }

        input[type="text"]:focus {
            padding: 10px;
            transition: all 0.3s ease-out;
            transition-delay: 0.2s;
        }

        }

        /* =================================================================================== */
        .row.heading h2 {
            color: #fff;
            font-size: 52.52px;
            line-height: 95px;
            font-weight: 400;
            text-align: center;
            margin: 0 0 40px;
            padding-bottom: 20px;
            text-transform: uppercase;
        }

        ul {
            margin: 0;
            padding: 0;
            list-style: none;
        }

        .heading.heading-icon {
            display: block;
        }

        .padding-lg {
            display: block;
            padding-top: 20px;
            padding-bottom: 20px;
        }

        .practice-area.padding-lg {
            padding-bottom: 55px;
            padding-top: 55px;
        }

        .practice-area .inner {
            border: 1px solid #999999;
            text-align: center;
            margin-bottom: 28px;
            padding: 40px 25px;
        }

        .our-webcoderskull .cnt-block:hover {
            box-shadow: 0px 0px 10px rgba(0,0,0,0.3);
            border: 0;
        }

        .practice-area .inner h3 {
            color: #3c3c3c;
            font-size: 24px;
            font-weight: 500;
            font-family: 'Poppins', sans-serif;
            padding: 10px 0;
        }

        .practice-area .inner p {
            font-size: 14px;
            line-height: 22px;
            font-weight: 400;
        }

        .practice-area .inner img {
            display: inline-block;
        }


        .our-webcoderskull {
            /*background: url("http://www.webcoderskull.com/img/right-sider-banner.png") no-repeat center top / cover;*/
            background-color: #e8e8e8;
        }

            .our-webcoderskull .cnt-block {
                float: left;
                width: 100%;
                background: #fff;
                padding: 30px 20px;
                text-align: center;
                border: 2px solid #d5d5d5;
                margin: 0 0 28px;
            }

                .our-webcoderskull .cnt-block figure {
                    width: 80px;
                    height: 80px;
                    border-radius: 100%;
                    display: inline-block;
                    margin-bottom: 15px;
                }

                .our-webcoderskull .cnt-block img {
                    width: 80px;
                    height: 80px;
                    border-radius: 100%;
                }

                .our-webcoderskull .cnt-block h3 {
                    color: #2a2a2a;
                    font-size: 20px;
                    font-weight: 500;
                    padding: 6px 0;
                    text-transform: uppercase;
                }

                    .our-webcoderskull .cnt-block h3 a {
                        text-decoration: none;
                        color: #2a2a2a;
                    }

                        .our-webcoderskull .cnt-block h3 a:hover {
                            color: #337ab7;
                        }

                .our-webcoderskull .cnt-block p {
                    color: #2a2a2a;
                    font-size: 13px;
                    line-height: 20px;
                    font-weight: 400;
                }

                .our-webcoderskull .cnt-block .follow-us {
                    margin: 20px 0 0;
                }

                    .our-webcoderskull .cnt-block .follow-us li {
                        display: inline-block;
                        width: auto;
                        margin: 0 5px;
                    }

                        .our-webcoderskull .cnt-block .follow-us li .fa {
                            font-size: 24px;
                            color: #767676;
                        }

                            .our-webcoderskull .cnt-block .follow-us li .fa:hover {
                                color: #025a8e;
                            }

        .flip-card {
            background-color: transparent;
            width: 300px;
            height: 180px;
            margin-bottom: 15px;
            perspective: 1000px;
        }

        .flip-card-inner {
            position: relative;
            width: 100%;
            height: 100%;
            text-align: center;
            transition: transform 0.6s;
            transform-style: preserve-3d;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
        }

        .flip-card:hover .flip-card-inner {
            transform: rotateY(180deg);
        }

        .flip-card-front, .flip-card-back {
            position: absolute;
            width: 100%;
            height: 100%;
            backface-visibility: hidden;
        }

        .flip-card-front {
            background-color: white;
            color: black;
            padding-top: 10px;
            border-radius: 4px;
        }

        .flip-card-back {
            background-color: #2980b9;
            color: white;
            transform: rotateY(180deg);
            border-radius: 4px;
        }

        .nav-tabs .nav-item.show .nav-link, .nav-tabs .nav-link.active {
            color: white;
            background-color: navy;
            border-color: #dee2e6 #dee2e6 #fff;
        }

        th {
            color: #e6e6f1 !important;
            background-color: #4a69d8 !important;
        }
        .nav-tabs {
            border-bottom: 1px solid #234b73;
        }
        .nav-link {
            padding: .4rem .5rem;
        }
    </style>

    <script type="text/javascript">
        function viewInquiry(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "InquiryInfo.aspx?mode=view&id=" + id, onClosed: function () { } });
        }

        function viewSalesOrder(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "SalesOrder.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewOutward(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "Outward.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewSalesBill(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "SalesBill.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewPurchaseOrder(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "PurchaseOrders.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewInward(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "Inward.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewPurchaseBill(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "PurchaseBill.aspx?mode=view&id=" + id, onClosed: function () { } });
            //var pageUrl = "PurchaseBill.aspx?mode=view&id=" + id;
            //$('#ifrSearchCust').attr('src', pageUrl);
            //$find("mpe").show();
        }

        function viewQuotation(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "Quotation.aspx?mode=view&id=" + id, onClosed: function () { } });
        }
        function viewFollowup(id) {
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: "Followup.aspx?mode=view&id=" + id, onClosed: function () { } });
        }

        function viewLedgerDocument(id, cat) {
            if (cat == 'SalesOrder') {
                //$.colorbox({ width: "98%", height: "95%", iframe: true, href: "SalesOrder.aspx?mode=view&id=" + id, onClosed: function () { } });
                viewSalesOrder(id);
            }
            else if (cat == 'Sales') {
                viewSalesBill(id);
            }
            else if (cat == 'Purchase' || cat == 'Purc') {
                viewPurchaseBill(id);
            }
            else if (cat == 'Bank' || cat == 'Cash') {
                $.colorbox({ width: "98%", height: "95%", iframe: true, href: "FinancialTrans.aspx?mode=view&category=" + cat + "&id=" + id, onClosed: function () { } });
            }
        }

        function openContFollowup(mode, pkid) {
            var pageUrl = "Followup.aspx?mode=" + mode + "&id=" + pkid
            $.colorbox({ width: "98%", height: "95%", iframe: true, href: pageUrl, onClosed: function () { } });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />

        <div id="myModal" style="display: block; width: 100%;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: white; border-bottom: 4px solid darkgrey;">
                    <div class="row m-0">
                        <div class="col m8">
                            <label class="active text-dark" for="txtCustomerName" style="font-size: 18px;">Customer Name<small class="text-muted text-white font-italic pl-2">(Minimum 3 chars to activate search)</small></label>
                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control mb-0" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="3" Style="width: 500px;" />
                        </div>
                        <div class="col m4">
                            <button ID="btnSaveTrial" type="button" runat="server" clientidmode="Static" text="Save" class="btn blue right mr-1" onserverclick="btnSaveTrial_Click" tabindex="8" style="width:150px;">ReGenerate</button>
                        </div>
                    </div>
                </div>
                <div class="customize-devider"></div>
                <div class="modal-body" style="min-height: 500px; padding: 0px 5px;">
                    <div class="row">
                        <div id="showContacts" class="col m12 mt-2 p-1" style="color: navy; border-top-left-radius: 0.5rem; border-bottom-left-radius: 0.5rem; border-top-right-radius: 0.5rem; border-bottom-right-radius: 0.5rem;">
                            <div id="Contacts-content">
                                <div class="emp-profile" style="padding: 10px;">
                                    <div class="row">
                                        <div class="col m12">
                                            <div class="profile-head">
                                                <ul class="nav nav-tabs" id="myTab" role="tablist">
                                                    <li class="nav-item">
                                                        <a class="nav-link active" id="primary-tab" data-toggle="tab" href="#primary" role="tab" aria-controls="home" aria-selected="true">Primary Information</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="home-tab" data-toggle="tab" href="#home" role="tab" aria-controls="home" aria-selected="false">Primary Contacts</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="inquiry-tab" data-toggle="tab" href="#inquiry" role="tab" aria-controls="inquiry" aria-selected="false">Inquiry & Followup</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="quotation-tab" data-toggle="tab" href="#quotation" role="tab" aria-controls="inquiry" aria-selected="false">Quotation</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="salesorder-tab" data-toggle="tab" href="#salesorder" role="tab" aria-controls="salesorder" aria-selected="false">Sales Order</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="outward-tab" data-toggle="tab" href="#outward" role="tab" aria-controls="outward" aria-selected="false">Outward [Pack.List]</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="salesbill-tab" data-toggle="tab" href="#salesbill" role="tab" aria-controls="salesbill" aria-selected="false">Sales Invoice</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="purcorder-tab" data-toggle="tab" href="#purcorder" role="tab" aria-controls="purcorder" aria-selected="false">Purchase Order</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="inward-tab" data-toggle="tab" href="#inward" role="tab" aria-controls="inward" aria-selected="false">Inward [G.R.N]</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="purcbill-tab" data-toggle="tab" href="#purcbill" role="tab" aria-controls="purcbill" aria-selected="false">Purchase Invoice</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="clientledger-tab" data-toggle="tab" href="#clientledger" role="tab" aria-controls="clientledger" aria-selected="false">Client Ledger</a>
                                                    </li>
                                                </ul>
                                            </div>
                                            <div class="tab-content profile-tab" id="myTabContent">
                                                <div class="tab-pane fade show active" id="primary" role="tabpanel" aria-labelledby="primary-tab">
                                                    <section>
                                                        <ul class="row">
                                                            <li class="col m3">
                                                                <div class="flip-card" style="margin: 20px 0px 5px 0px">
                                                                    <p>
                                                                        <h5>Primary Address</h5>
                                                                    </p>
                                                                    <p class="m-0 p-0" style="text-transform: uppercase;">
                                                                        <asp:Label ID="lblAddress11" runat="server" ClientIDMode="Static" />
                                                                    </p>
                                                                    <p class="m-0 p-0" style="text-transform: uppercase;">
                                                                        <asp:Label ID="lblAddress12" runat="server" ClientIDMode="Static" />
                                                                    </p>
                                                                    <p class="m-0 p-0" style="text-transform: uppercase;">
                                                                        <asp:Label ID="lblAddress13" runat="server" ClientIDMode="Static" />
                                                                    </p>
                                                                    <hr />
                                                                    <p>
                                                                        <h5>Alternate Address</h5>
                                                                    </p>
                                                                    <p class="m-0 p-0" style="text-transform: uppercase;">
                                                                        <asp:Label ID="lblAddress21" runat="server" ClientIDMode="Static" />
                                                                    </p>
                                                                    <p class="m-0 p-0" style="text-transform: uppercase;">
                                                                        <asp:Label ID="lblAddress22" runat="server" ClientIDMode="Static" />
                                                                    </p>
                                                                    <p class="m-0 p-0" style="text-transform: uppercase;">
                                                                        <asp:Label ID="lblAddress23" runat="server" ClientIDMode="Static" />
                                                                    </p>
                                                                </div>
                                                                <div class="flip-card" style="margin: -185px 0px 0px 440px;">
                                                                    <h5>Contact Us</h5>
                                                                    <table style="width: 100%;">
                                                                        <tr style="margin-bottom: 3px;">
                                                                            <td style="float: left; width: 40%;">Contact # :</td>
                                                                            <td style="float: left; width: 60%;">
                                                                                <asp:Label ID="lblContact" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                                                        </tr>
                                                                        <tr style="margin-bottom: 3px;">
                                                                            <td style="float: left; width: 40%;">Email ID  :</td>
                                                                            <td style="float: left; width: 60%;">
                                                                                <asp:Label ID="lblEmail" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                                                        </tr>
                                                                        <tr style="margin-bottom: 3px;">
                                                                            <td style="float: left; width: 40%;">Website   :</td>
                                                                            <td style="float: left; width: 60%;">
                                                                                <asp:Label ID="lblWebsite" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="flip-card" style="margin: -181px 0px 0px 860px;;">
                                                                    <h5>Other Details</h5>
                                                                    <table style="width: 100%;">
                                                                        <tr style="margin-bottom: 3px;">
                                                                            <td style="float: left; width: 40%;">GST # :</td>
                                                                            <td style="float: left; width: 60%;">
                                                                                <asp:Label ID="lblGST" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                                                        </tr>
                                                                        <tr style="margin-bottom: 3px;">
                                                                            <td style="float: left; width: 40%;">PAN # :</td>
                                                                            <td style="float: left; width: 60%;">
                                                                                <asp:Label ID="lblPAN" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                                                        </tr>
                                                                        <tr style="margin-bottom: 3px;">
                                                                            <td style="float: left; width: 40%;">CIN # :</td>
                                                                            <td style="float: left; width: 60%;">
                                                                                <asp:Label ID="lblCIN" runat="server" ClientIDMode="Static" Font-Size="12px" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Birth Date :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblBirthDate" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Anniversary Date :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblAnniversaryDate" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </li>
                                                        </ul>
                                                    </section>
                                                </div>
                                                <div class="tab-pane fade" id="home" role="tabpanel" aria-labelledby="home-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; overflow-y: auto;">
                                                        <div class="container m-0" style="width: 100%;">
                                                            <ul class="row">
                                                                <asp:Repeater ID="rptContacts" runat="server">
                                                                    <ItemTemplate>
                                                                        <li class="col m3 float-left">
                                                                            <div class="flip-card">
                                                                                <div class="flip-card-inner">
                                                                                    <div class="flip-card-front">
                                                                                        <img src="images/customer.png" alt="Avatar" style="width: 80px; height: 80px;" />
                                                                                        <h5 class="mt-1"><%# Eval("ContactPerson1") %></h5>
                                                                                    </div>
                                                                                    <div class="flip-card-back mt-1">
                                                                                        <h5 style="margin-top: 0.5rem;"><%# Eval("ContactPerson1") %></h5>
                                                                                        <p><%# Eval("ContactDesigCode1") %></p>
                                                                                        <p><%# Eval("ContactNumber1") %></p>
                                                                                        <div style="overflow-wrap: break-word; overflow: auto; text-overflow: ellipsis; white-space: nowrap;"><small><%# Eval("ContactEmail1") %></small></div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </li>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </ul>
                                                        </div>
                                                    </section>
                                                </div>
                                                <div class="tab-pane fade" id="quotation" role="tabpanel" aria-labelledby="quotation-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; overflow-y: auto;">
                                                        <table id="tblQuotation" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <asp:Repeater ID="rptQuotation" runat="server">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-center">Quotation #</th>
                                                                            <th class="text-center">Quotation Date</th>
                                                                            <th class="text-center">Inquiry No</th>
                                                                            <th class="text-center">InquiryStatus</th>
                                                                            <th class="text-right">Amount</th>
                                                                            <th class="text-center">Sales Person</th>
                                                                            <th class="text-center">Preview</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <%--<asp:HiddenField ID="hdnApprovalStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("ApprovalStatus") %>' />--%>
                                                                        <%--<asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />--%>
                                                                        <td class="text-center">
                                                                            <a href="javascript:viewQuotation(<%# Eval("pkID")%>);"><%# Eval("QuotationNo") %></a>
                                                                        </td>
                                                                        <td class="text-center"><%# Eval("QuotationDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td class="text-center"><%# Eval("InquiryNo") %></td>
                                                                        <td class="text-center"><%# Eval("InquiryStatus") %></td>
                                                                        <td class="text-right"><%# Eval("QuotationAmount") %></td>
                                                                        <td class="text-center"><%# Eval("CreatedEmployeeName") %></td>
                                                                        <td class="text-center">
                                                                            <img src="images/pdf_document.png" alt="" style="width: 15px; height: 15px;" onclick='ShowQuotationPreview(<%# Eval("pkID") %>)' />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>

                                                    </section>
                                                </div>
                                                <div class="tab-pane fade" id="inquiry" role="tabpanel" aria-labelledby="inquiry-tab">
                                                    <section class="our-webcoderskull" style="height: 405px; padding: 10px 10px 10px 15px; width: inherit;">
                                                        <div class="row" style="padding: 10px !important;">
                                                            <div style="height: 370px; background-color: white; width: 65%;">
                                                                <table id="tblInquiry" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                                    <asp:Repeater ID="rptInquiry" runat="server">
                                                                        <HeaderTemplate>
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="text-center">Inquiry #</th>
                                                                                    <th class="text-center">Inquiry Date</th>
                                                                                    <th class="text-center">Source</th>
                                                                                    <th class="text-right">Amount</th>
                                                                                    <th class="text-center">Status</th>
                                                                                    <th class="text-left">Sales Person</th>
                                                                                </tr>
                                                                            </thead>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr class="blueShed">
                                                                                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                                <asp:HiddenField ID="hdnInquiryStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("InquiryStatus") %>' />
                                                                                <td class="text-center"><a href="javascript:viewInquiry(<%# Eval("pkID")%>);"><%# Eval("InquiryNo") %></a></td>
                                                                                <td class="text-center"><%# Eval("InquiryDate", "{0:dd/MM/yyyy}") %></td>
                                                                                <td class="text-left"><%# Eval("InquirySource") %></td>
                                                                                <td class="text-right"><%# Eval("TotalAmount") %></td>
                                                                                <td class="text-center"><%# Eval("InquiryStatus") %></td>
                                                                                <td class="text-left"><%# Eval("EmployeeName") %></td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </table>
                                                            </div>
                                                            <div style="height: 370px; background-color: white; margin-left: 10px; width: 33%; overflow-y: auto;">
                                                                <ul class="todo-list" style="list-style: none;">
                                                                    <asp:Repeater ID="rptFollowup" runat="server" ClientIDMode="Static">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnItemID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                            <asp:HiddenField ID="hdnNextFollowup" runat="server" ClientIDMode="Static" Value='<%# Eval("NextFollowupDate") %>' />
                                                                            <asp:HiddenField ID="hdnFollInqNo" runat="server" ClientIDMode="Static" Value='<%# Eval("InquiryNo") %>' />
                                                                            <asp:HiddenField ID="hdnFollSource" runat="server" ClientIDMode="Static" Value='<%# Eval("InquirySource") %>' />
                                                                            <li id='liitem<%# Eval("pkID") %>' class="gradient-45deg-yellow-green mb-1" style="border-left: 2px solid #d6c7c7; padding: 5px;">
                                                                                <span class="handle">
                                                                                    <span class="text" style="color: black; font-weight: 600; text-transform: capitalize;">
                                                                                        <a title="Show Followup Info." href="javascript:viewFollowup('<%# Eval("pkID") %>');"><%# Eval("CustomerName") %></a>
                                                                                    </span>
                                                                                    <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i><%# Eval("InquiryStatus") %> </small>
                                                                                </span>
                                                                                <br />
                                                                                <span class="text" style="color: navy; font-size: 12px; font-weight: 400;"><%# Eval("MeetingNotes") %></span>
                                                                                <div class="row mt-2">
                                                                                    <div class="col m-6 float-left w-100" style="margin-right: 5px !important;">
                                                                                        <small class="badge badge-primary w-100"><i class="fa fa-calendar-check-o"></i><span>Followup Date: </span><%# Eval("FollowupDate", "{0:dd-MM-yyyy}") %> </small>
                                                                                    </div>
                                                                                    <div class="col m float-right w-100" style="margin-right: 5px !important;">
                                                                                        <small class="badge badge-secondary w-100 float-right"><i class="fa fa-calendar-check-o"></i><span>Inquiry #: </span>
                                                                                            <a title="Click to Show Inquiry" href="javascript:viewInquiry('<%# Eval("pkID")%>');" style="color: navy;"><%# Eval("InquiryNo") %></a>
                                                                                        </small>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row mt-2">
                                                                                    <div class="col m-6 float-left w-100" style="margin-right: 5px !important;">
                                                                                        <small class="badge badge-primary w-100"><i class="fa fa-calendar-check-o"></i>
                                                                                            <span>Next Followup : </span>
                                                                                            <span id="ltrNextFollowup" runat="server" clientidmode="Static"><%# Eval("NextFollowupDate", "{0:dd-MM-yyyy}") %>&nbsp;<b><%# Eval("PreferredTime") %></b></span>
                                                                                        </small>
                                                                                    </div>
                                                                                    <div class="col m float-right w-100" style="margin-right: 5px !important;">
                                                                                        <small class="badge badge-secondary w-100 float-right"><i class="fa fa-calendar-check-o"></i>
                                                                                            <span>Quotation : </span>
                                                                                            <a title="Click to Show Quotation" href="javascript:viewQuotation('<%# Eval("pkID") %>');" style="color: navy;"><%# Eval("QuotationNo") %></a>
                                                                                        </small>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="row mt-1 ml-0 mr-0" style="background-color: navy;">
                                                                                    <div class="col m12">
                                                                                        <small>
                                                                                            <span style="color: white; font-weight: 800;">Initiated By :</span>
                                                                                            <span style="color: white;"><b>&nbsp;<%# Eval("EmployeeName") %></b></span>
                                                                                        </small>
                                                                                    </div>
                                                                                </div>

                                                                            </li>

                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </section>
                                                </div>

                                                <div class="tab-pane fade" id="salesorder" role="tabpanel" aria-labelledby="salesorder-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                        <table id="tblInqProductGroup" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <asp:Repeater ID="rptApproval" runat="server">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-center">Order #</th>
                                                                            <th class="text-center">Order Date</th>
                                                                            <th style="text-align: left !important;">Sales Person</th>
                                                                            <th class="text-left">Approval Status</th>
                                                                            <th class="text-center">Preview</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <asp:HiddenField ID="hdnApprovalStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("ApprovalStatus") %>' />
                                                                        <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                                                                        <td class="text-center">
                                                                            <a href="javascript:viewSalesOrder(<%# Eval("pkID")%>);"><%# Eval("OrderNo") %></a>
                                                                        </td>
                                                                        <td class="text-center"><%# Eval("OrderDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td style="text-align: left !important;"><%# Eval("EmployeeName") %></td>
                                                                        <td class="text-left"><%# Eval("ApprovalStatus") %></td>
                                                                        <td class="text-center">
                                                                            <img src="images/pdf_document.png" alt="" style="width: 15px; height: 15px;" onclick='gridAction(<%# Eval("pkID") %>)' />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </section>
                                                </div>

                                                <div class="tab-pane fade" id="outward" role="tabpanel" aria-labelledby="outward-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                        <table id="tbloutwardGroup" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <asp:Repeater ID="rptOutward" runat="server">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-center">Outward #</th>
                                                                            <th class="text-center">Outward Date</th>
                                                                            <th class="text-left">Customer Name</th>
                                                                            <th class="text-center">Executive Name</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                                                                        <td class="text-center">
                                                                            <a href="javascript:viewOutward(<%# Eval("pkID")%>);"><%# Eval("OutwardNo") %></a>
                                                                        </td>
                                                                        <td class="text-center"><%# Eval("OutwardDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td class="text-left"><%# Eval("CustomerName") %></td>
                                                                        <td class="text-center"><%# Eval("EmployeeName") %></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </section>
                                                </div>

                                                <div class="tab-pane fade" id="salesbill" role="tabpanel" aria-labelledby="salesbill-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                        <table id="tblsalesbill" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <asp:Repeater ID="rptSalesBill" runat="server">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-center">Invoice #</th>
                                                                            <th class="text-center">Invoice Date</th>
                                                                            <th style="text-align: left !important;">Customer Name</th>
                                                                            <th style="text-align: left !important;">Order Amount</th>
                                                                            <th style="text-align: left !important;">Net Amount</th>
                                                                            <th style="text-align: left !important;">Sales Person</th>
                                                                            <th class="text-center">Preview</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                                                                        <td class="text-center">
                                                                            <a href="javascript:viewSalesBill(<%# Eval("pkID")%>);"><%# Eval("InvoiceNo") %></a>
                                                                        </td>
                                                                        <td class="text-center"><%# Eval("InvoiceDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td style="text-align: left !important;"><%# Eval("CustomerName") %></td>
                                                                        <td style="text-align: left !important;"><%# Eval("BasicAmt") %></td>
                                                                        <td style="text-align: left !important;"><%# Eval("NetAmt") %></td>
                                                                        <td style="text-align: left !important;"><%# Eval("EmployeeName") %></td>
                                                                        <td class="text-center">
                                                                            <img src="images/pdf_document.png" alt="" style="width: 15px; height: 15px;" onclick='ShowSalesBillPreview(<%# Eval("pkID") %>)' />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </section>
                                                </div>

                                                <div class="tab-pane fade" id="purcorder" role="tabpanel" aria-labelledby="purcorder-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                        <table id="tblpurcorderGroup" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <asp:Repeater ID="rptPurcOrder" runat="server">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-center">Order #</th>
                                                                            <th class="text-center">Order Date</th>
                                                                            <th class="text-left">Supplier Name</th>
                                                                            <th class="text-right">Basic Amt</th>
                                                                            <th class="text-right">Net Amt</th>
                                                                            <th class="text-center">Executive Name</th>
                                                                            <th class="text-center">Approval Status</th>
                                                                            <th class="text-center">Preview</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <asp:HiddenField ID="hdnApprovalStatus" runat="server" ClientIDMode="Static" Value='<%#Eval("ApprovalStatus") %>' />
                                                                        <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                                                                        <td class="text-center">
                                                                            <a href="javascript:viewPurchaseOrder(<%# Eval("pkID")%>);"><%# Eval("OrderNo") %></a>
                                                                        </td>
                                                                        <td class="text-center"><%# Eval("OrderDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td class="text-left"><%# Eval("CustomerName") %></td>
                                                                        <td class="text-right"><%# Eval("BasicAmt") %></td>
                                                                        <td class="text-right"><%# Eval("NetAmt") %></td>
                                                                        <td class="text-center"><%# Eval("EmployeeName") %></td>
                                                                        <td class="text-center"><%# Eval("ApprovalStatus") %></td>
                                                                        <td class="text-center">
                                                                            <img src="images/pdf_document.png" alt="" style="width: 15px; height: 15px;" onclick='ShowPurcOrderPreview(<%# Eval("pkID") %>)' />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </section>
                                                </div>

                                                <div class="tab-pane fade" id="inward" role="tabpanel" aria-labelledby="inward-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                        <table id="tblinwardGroup" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <asp:Repeater ID="rptInward" runat="server">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-center">Inward #</th>
                                                                            <th class="text-center">Inward Date</th>
                                                                            <th class="text-left">Customer Name</th>
                                                                            <th class="text-center">Executive Name</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                                                                        <td class="text-center">
                                                                            <a href="javascript:viewInward(<%# Eval("pkID")%>);"><%# Eval("InwardNo") %></a>
                                                                        </td>
                                                                        <td class="text-center"><%# Eval("InwardDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td class="text-left"><%# Eval("CustomerName") %></td>
                                                                        <td class="text-center"><%# Eval("EmployeeName") %></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </section>
                                                </div>

                                                <div class="tab-pane fade" id="purcbill" role="tabpanel" aria-labelledby="purcbill-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                        <table id="tblpurcbillbill" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <asp:Repeater ID="rptPurcBill" runat="server">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-center">Bill #</th>
                                                                            <th class="text-center">Bill Date</th>
                                                                            <th class="text-left">Supplier Name</th>
                                                                            <th class="text-right">Order Amount</th>
                                                                            <th class="text-right">Net Amount</th>
                                                                            <th class="text-center">Executive Name</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%#Eval("EmployeeName") %>' />
                                                                        <td class="text-center">
                                                                            <a href="javascript:viewPurchaseBill(<%# Eval("pkID")%>);"><%# Eval("InvoiceNo") %></a>
                                                                        </td>
                                                                        <td class="text-center"><%# Eval("InvoiceDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td class="text-left"><%# Eval("CustomerName") %></td>
                                                                        <td class="text-right"><%# Eval("BasicAmt") %></td>
                                                                        <td class="text-right"><%# Eval("NetAmt") %></td>
                                                                        <td class="text-center"><%# Eval("CreatedEmployeeName") %></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                    </section>
                                                </div>

                                                <div class="tab-pane fade" id="clientledger" role="tabpanel" aria-labelledby="clientledger-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                        <table id="tblCustomerLedger" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <uc1:myClientDetailLedger runat="server" ID="myClientDetailLedger" />
                                                        </table>
                                                    </section>
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
        </div>
        <%-- PopUp Modal Box to Open Window --%>
        <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
        <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="pnlSearchCust" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlSearchCust" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
            <div id="myModal123">
                <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                    <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                        <i class="material-icons prefix">ac_unit</i>
                        <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                        <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                    </h5>
                </div>
            </div>
            <iframe id="ifrSearchCust" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
            <br />
        </asp:Panel>

        <script type="text/javascript">
            $("input").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    //$("form").submit();
                }
            });
            // -------------------------------------------------------------
            function bindDDLTo(selector) {
                if ($(selector).val().length >= 3) {
                    jQuery.ajax({
                        type: "POST",
                        url: "InquiryInfo.aspx/FilterCustomer",
                        data: '{pCustName:\'' + $(selector).val() + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            console.log(data);
                            //var resultdata = JSON.parse(data.d);
                            var sample = JSON.parse(data.d);
                            $(selector).autoComplete({
                                minChars: 1,
                                source: function (term, suggest) {
                                    term = term.toLowerCase();
                                    var choices = sample;
                                    suggest(choices);
                                },
                                renderItem: function (item, search) {
                                    $(".autocomplete-suggestion").remove();
                                    //search = search.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
                                    //var re = new RegExp("(" + search.split(' ').join('|') + ")", "gi");
                                    return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                },
                                onSelect: function (e, term, item) {
                                    console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                    $(selector).val(item.html());
                                    $("#hdnCustomerID").val(item.data('langname'));
                                    $("#drpInquiry").focus();
                                }
                            });

                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                    return false;
                }
                else {
                    if ($(selector).val().length == 0) {
                        $("#hdnCustomerID").val('');
                    }
                }
            }

            // -------------------------------------------------------------
            function ShowPDFfile(repFilename) {
                yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
                yhooWin.focus();
            }

            function ShowQuotationPreview(id) {
                jQuery.ajax({
                    type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8", success: function (data11) {
                        QuotationNoForPDF = (data11.d).replace("/", "-");
                        if (QuotationNoForPDF != "") {
                            ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                        }
                        else {
                            alert('Quotation PDF Not Found !')
                        }
                    }
                });
            }

            function ShowSalesOrderPreview(id) {
                jQuery.ajax({
                    type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8", success: function (data11) {
                        OrderNoForPDF = (data11.d).replace("/", "-");
                        if (OrderNoForPDF != "")
                            ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                        else
                            alert('Sales Order PDF Not Found !')
                    }
                });
            }

            function ShowSalesBillPreview(id) {
                jQuery.ajax({
                    type: "POST", url: 'SalesBill.aspx/GetSalesBillNoForPDF', data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8", success: function (data11) {
                        OrderNoForPDF = (data11.d).replace("/", "-");
                        if (OrderNoForPDF != "")
                            ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                        else
                            alert('Sales Order PDF Not Found !')
                    }
                });
            }

            function ShowPurcOrderPreview(id) {
                jQuery.ajax({
                    type: "POST", url: 'PurchaseOrders.aspx/GetPurchaseOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8", success: function (data11) {
                        OrderNoForPDF = (data11.d).replace("/", "-");
                        if (OrderNoForPDF != "")
                            ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                        else
                            alert('Sales Order PDF Not Found !')
                    }
                });
            }

            function gridAction(id) {
                
                    var SOPageUrl = '';
                    //var DSOPageUrl = '';
                    var SalesOrderVersion = jQuery.trim(jQuery("#hdnSalesOrderVersion").val());
                    // =========================================================================
                    if (jQuery.trim(jQuery("#hdnpopupPrintHeader").val()) == 'yes') {
                        swal({
                            title: "Print SO/PI Header ?", text: "Are you sure? You want to print Header !", icon: 'warning', dangerMode: true,
                            buttons: { cancel: 'Dont Print', delete: 'Yes, Print' }
                        }).then(function (willDelete) {
                            if (willDelete) {
                                var x = PageMethods.setPrintHeader('yes');
                                SOPageUrl = 'SalesOrder.aspx/GenerateSalesOrder';
                                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                                jQuery.ajax({
                                    type: "POST",
                                    url: SOPageUrl,
                                    data: '{pkID:\'' + id + '\'}',
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        // -----------------------------------------------------------
                                        jQuery.ajax({
                                            type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                            contentType: "application/json; charset=utf-8", success: function (data11) {
                                                OrderNoForPDF = (data11.d).replace("/", "-");
                                                if (OrderNoForPDF != "")
                                                    ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                                else
                                                    alert('Sales Order PDF Not Found !')
                                            }
                                        });
                                    },
                                    error: function (r) { alert('Error : ' + r.responseText); },
                                    failure: function (r) { alert('failure'); }
                                });
                            }
                            else {
                                var x = PageMethods.setPrintHeader('no');
                                SOPageUrl = 'SalesOrder.aspx/GenerateSalesOrder';
                                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                                jQuery.ajax({
                                    type: "POST",
                                    url: SOPageUrl,
                                    data: '{pkID:\'' + id + '\'}',
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        // -----------------------------------------------------------
                                        jQuery.ajax({
                                            type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                            contentType: "application/json; charset=utf-8", success: function (data11) {
                                                OrderNoForPDF = (data11.d).replace("/", "-");
                                                if (OrderNoForPDF != "")
                                                    ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                                else
                                                    alert('Sales Order PDF Not Found !')
                                            }
                                        });
                                    },
                                    error: function (r) { alert('Error : ' + r.responseText); },
                                    failure: function (r) { alert('failure'); }
                                });
                            }
                        });
                    }
                    else {
                        SOPageUrl = 'SalesOrder.aspx/GenerateSalesOrder';
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        jQuery.ajax({
                            type: "POST",
                            url: SOPageUrl,
                            data: '{pkID:\'' + id + '\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                // -----------------------------------------------------------
                                jQuery.ajax({
                                    type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                                    contentType: "application/json; charset=utf-8", success: function (data11) {
                                        OrderNoForPDF = (data11.d).replace("/", "-");
                                        if (OrderNoForPDF != "")
                                            ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                                        else
                                            alert('Sales Order PDF Not Found !')
                                    }
                                });
                            },
                            error: function (r) { alert('Error : ' + r.responseText); },
                            failure: function (r) { alert('failure'); }
                        });
                    }
                }
            

            //function ShowPurcBillPreview(id) {
            //    jQuery.ajax({
            //        type: "POST", url: 'SalesOrder.aspx/GetPurchaseBillNoForPDF', data: '{pkID:\'' + id + '\'}',
            //        contentType: "application/json; charset=utf-8", success: function (data11) {
            //            OrderNoForPDF = (data11.d).replace("/", "-");
            //            if (OrderNoForPDF != "")
            //                ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
            //            else
            //                alert('Sales Order PDF Not Found !')
            //        }
            //    });
            //}

        </script>
    </form>
</body>
</html>
