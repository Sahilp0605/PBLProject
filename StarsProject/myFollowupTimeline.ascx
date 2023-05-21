<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myFollowupTimeline.ascx.cs" Inherits="StarsProject.myFollowupTimeline" %>
<style type="text/css">
    ul.timeline {
        list-style-type: none;
        position: relative;
    }
    ul.timeline:before {
        content: ' ';
        background: #d4d9df;
        display: inline-block;
        position: absolute;
        left: 29px;
        width: 2px;
        height: 100%;
        z-index: 400;
    }
    ul.timeline > li {
        margin: 15px 0;
        padding-left: 20px;
    }
    ul.timeline > li:before {
        content: ' ';
        background: white;
        display: inline-block;
        position: absolute;
        border-radius: 50%;
        border: 3px solid #22c0e8;
        left: 20px;
        width: 20px;
        height: 20px;
        z-index: 400;
    }
</style>
<asp:HiddenField ID="tlCustomerID" runat="server" ClientIDMode="Static" />
<ul class="timeline">
<asp:Repeater ID="rptTimeline" runat="server" ClientIDMode="Static"  OnItemDataBound="rptTimeline_ItemDataBound">
    <ItemTemplate>
        <asp:HiddenField ID="hdnInquiryNo" runat="server" ClientIDMode="Static" Value='<%# Eval("InquiryNo") %>' />
		<li>
            <div style="width:90%; margin-left: 35px !important; border-radius: 6px !important; background-color: #ffefd5; border: 2px inset silver;padding:10px;">
                <div class="row col m12 mt-0 ml-0 mr-0">
                    <span class="handle">
                        <span class="text" style="color:black; padding-left:5px; font-weight:600;  text-transform:capitalize;">
                            <a title="Show Followup Info." href="javascript:openDashboardBox('followup','view','<%# Eval("pkID") %>');"><%# Eval("CustomerName") %></a>
                        </span>
                        <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i>  <%# Eval("InquiryStatus") %> </small>
                    </span>
                </div>
                <div class="row mt-0 ml-0 mr-0">
                    <div class="col m12">
                        <span class="text" style="color:navy; font-size:12px; font-weight:400;"><%# Eval("MeetingNotes") %></span>
                    </div>
                </div>
                <div class="row mt-0 ml-0 mr-0" style="border-top:1px solid silver; border-bottom:1px solid silver;">
                    <div class="col m3 p-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Followup Date</span></span>
                    </div>
                    <div class="col m3 p-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Next Followup</span></span>
                    </div>
                    <div class="col m3 p-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Lead #</span></span>
                    </div>
                    <div class="col m3 p-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Quot.#</span></span>
                    </div>
                </div>
                <div class="row mt-1 ml-0 mr-0">
                    <div class="col m3 p-0">
                        <small class="badge-primary center"><%# Eval("FollowupDate", "{0:dd-MM-yyyy}") %></small>
                    </div>
                    <div class="col m3 p-0">
                        <small class="badge-primary center">
                            <span ID="Span1" runat="server" clientidmode="Static"><%# Eval("NextFollowupDate", "{0:dd-MM-yyyy}") %>&nbsp;<b><%# Eval("PreferredTime") %></b></span>    
                        </small>
                    </div>
                    <div class="col m3 p-0">
                        <small class="badge-primary center">
                            <a title="Click to Show Inquiry" href="javascript:openDashboardBox('inquirybyno','view','<%# Eval("InquiryNo") %>');"><%# Eval("InquiryNo") %></a> 
                        </small>

                    </div>
                    <div class="col m3 p-0">
                        <small class="badge-secondary center">
                            <a title="Click to Show Quotation" href="javascript:openDashboardBox('quotationbyno','view','<%# Eval("QuotationNo") %>');"><%# Eval("QuotationNo") %></a> 
                        </small>
                    </div>
                </div>
<%--                <div class="row mt-1 ml-0 mr-0 gradient-45deg-light-blue-indigo" style="font-size:12px;">
                    <div class="col m3">
                        <small><span style="font-weight:800;">Contact #1 </span></small>
                    </div>
                    <div class="col m3">
                        <small><span style="color:navy;"><%# Eval("ContactNo1") %></span></small>
                    </div>
                    <div class="col m3">
                        <small><span style="font-weight:800;">Contact #2</span></small>
                    </div>
                    <div class="col m3">
                        <smaill><span style="color:navy;"><%# Eval("ContactNo2") %></span></small>
                    </div>
                </div>
                <div class="row mt-1 ml-0 mr-0 gradient-45deg-yellow-teal" style="font-size:14px;">
                    <div class="col m3">
                        <small><span style="font-weight:800;">Contact Person : </span></small>
                    </div>
                    <div class="col m3">
                        <small><span style="color:navy;"><%# Eval("ContactPerson") %></span></small>
                    </div>
                    <div class="col m3">
                        <small><span style="font-weight:800;">Contact #</span></small>
                    </div>
                    <div class="col m3">
                        <small><span style="color:navy;"><%# Eval("ContactPersonPhone") %></span></small>
                    </div>
                </div>--%>
                <div class="row mt-1 ml-0 mr-0" style="background-color:navy; font-size:14px;">
                    <div class="col m12">
                        <small>
                            <span style="color:white; font-weight:800;">FollowUp Initiated By :</span>
                            <span style="color:white;"><b>&nbsp;<%# Eval("EmployeeName") %></b></span>
                        </small>
                    </div>
                </div>
            </div>
		</li>
    </ItemTemplate>
</asp:Repeater>
</ul>