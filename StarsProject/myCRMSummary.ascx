<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myCRMSummary.ascx.cs" Inherits="StarsProject.myCRMSummary" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />

<div id="card-stats" class="pt-0">
    <div class="row">
        <div class="col s12 m6 l3">
            <div class="card animate fadeLeft">
                <div class="card-content cyan white-text">
                    <a class="width-100" href="javascript:viewCRMSummaryList('inquiry', document.getElementById('hdnLoginUserID').value);" style="display: inline-block;">
                        <div class="row">
                            <div class="col m4">
                                <i class="material-icons background-round mt-5">person_outline</i>
                                <p class="white-text">Leads</p>
                            </div>
                            <div class="col m8  right-align pr-10 pt-2">
                                <p class="card-stats-title white-text hide">Leads</p>
                                <h4 id="spnLeads" runat="server" class="card-stats-number white-text">566</h4>
                                <p class="card-stats-compare" style="font-size:21px; border-top: 1px solid black;">
                                    <span class="cyan text text-lighten-5">Todays Summary</span>
                                </p>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>
        <div class="col s12 m6 l3">
            <div class="card animate fadeLeft">
                <div class="card-content red accent-2 white-text">
                    <a class="width-100" href="javascript:viewCRMSummaryList('quotation', document.getElementById('hdnLoginUserID').value);" style="display: inline-block;">
                        <div class="row">
                            <div class="col m4">
                                <i class="material-icons background-round mt-5">content_copy</i>
                                <p class="white-text">Quotation</p>
                            </div>
                            <div class="col m8  right-align pr-10 pt-2">
                                <h4 id="spnQuotation" runat="server" class="card-stats-number white-text">$8990.63</h4>
                                <p class="card-stats-compare" style="font-size:24px; border-top: 1px solid black;">
                                    &#8377;&nbsp;&nbsp;<span id="spnQuotationAmt" runat="server" class="red-text text-lighten-5">last month</span>
                                </p>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>
        <div class="col s12 m6 l3">
            <div class="card animate fadeRight">
                <div class="card-content orange lighten-1 white-text">
                    <a class="width-100" href="javascript:viewCRMSummaryList('salesorder', document.getElementById('hdnLoginUserID').value);" style="display: inline-block;">
                        <div class="row">
                            <div class="col m4">
                                <i class="material-icons background-round mt-5">trending_up</i>
                                <p class="white-text">Sales Orders</p>
                            </div>
                            <div class="col m8  right-align pr-10 pt-2">
                                <h4 id="spnSalesOrder" runat="server" class="card-stats-number white-text">$806.52</h4>
                                <p class="card-stats-compare" style="font-size:24px; border-top: 1px solid black;">
                                    &#8377;&nbsp;&nbsp;<span id="spnSalesOrderAmt" runat="server" class="orange-text text-lighten-5">from yesterday</span>
                                </p>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>
        <div class="col s12 m6 l3">
            <div class="card animate fadeRight">
                <div class="card-content green lighten-1 white-text">
                    <a class="width-100" href="javascript:viewCRMSummaryList('salesbill', document.getElementById('hdnLoginUserID').value);" style="display: inline-block;">
                        <div class="row">
                            <div class="col m4">
                                <i class="material-icons background-round mt-5">attach_money</i>
                                <p class="white-text">Sales Invoice</p>
                            </div>
                            <div class="col m8  right-align pr-10 pt-2">
                                <h4 id="spnSalesBill" runat="server" class="card-stats-number white-text">1806</h4>
                                <p class="card-stats-compare" style="font-size:24px; border-top: 1px solid black;">
                                    &#8377;&nbsp;&nbsp;<span id="spnSalesBillAmt" runat="server" class="green-text text-lighten-5">from last month</span>
                                </p>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>
    </div>
</div>
