<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myFollowup.ascx.cs" Inherits="StarsProject.myFollowup" %>
<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnFollowupStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />
<script type="text/javascript">


</script>
<asp:Repeater ID="rptFollowup" runat="server" OnItemDataBound="rptFollowup_ItemDataBound">
    <ItemTemplate>
        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
        <asp:HiddenField ID="hdnExtpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("ExtpkID") %>' />
        <asp:HiddenField ID="hdnNextFollowup" runat="server" ClientIDMode="Static" Value='<%# Eval("NextFollowupDate") %>' />
        <asp:HiddenField ID="hdnFollInqNo" runat="server" ClientIDMode="Static" Value='<%# Eval("InquiryNo") %>' />
        <asp:HiddenField ID="hdnFollSource" runat="server" ClientIDMode="Static" Value='<%# Eval("InquirySource") %>' />
        <asp:HiddenField ID="hdnCustID" runat="server" ClientIDMode="Static" Value='<%# Eval("CustomerID") %>' />
        <asp:HiddenField ID="hdnCustomerName" runat="server" ClientIDMode="Static" Value='<%# Eval("CustomerName") %>' />
        <ul class="todo-list" style="list-style:none;">
            <li id="liBase" runat="server" style="background-color:beige; border: 2px solid #d6c7c7; border-radius:8px; padding:5px;">
                <div class="row col m12 mt-2 ml-0 mr-0">
                    <span class="handle">
                        <a title="Add Folloup in Continuation" href="javascript:openMixFollowup('continue','<%# Eval("pkID") %>', '<%# Eval("CustomerID") %>', '<%# Eval("ExtpkID") %>');">
                            <img id="Img1" class="float-left" runat="server" clientidmode="static" src="images/expand.png" style="width:20px; height:20px;" />
                        </a>
                        <span class="text" style="color:black; padding-left:5px; font-weight:600;  text-transform:capitalize;">
                            <a id="lnkShowFollowUp" title="Show Followup Info." href="javascript:openDashboardFollowUp('followup','view','<%# Eval("pkID") %>','<%# Eval("CustomerID") %>');"><%# Eval("CustomerName") %></a>
                        </span>
                        <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i>  <%# Eval("InquiryStatus") %> </small>
                        <a title="Show Followup Timeline" href="javascript:openMixTimeline('<%# Eval("ExtpkID") %>','<%# Eval("CustomerID") %>','<%# Eval("CustomerName") %>');">
                        <img id="follTimeline" class="float-right mr-2" src="images/timeline3.png" style="width:20px; height:20px;" />
                        </a>
                    </span>
                </div>
                <div class="row mt-2 ml-0 mr-0">
                    <div class="col m12">
                        <span class="text" style="color:navy; font-size:12px; font-weight:400;"><%# Eval("MeetingNotes") %></span>
                    </div>
                </div>
                <div class="row mt-2 ml-0 mr-0" style="border-top:1px solid silver; border-bottom:1px solid silver;">
                    <div class="col m3 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Followup Date</span></span>
                    </div>
                    <div class="col m3 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Next Followup</span></span>
                    </div>
                    <div class="col m3 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Lead #</span></span>
                    </div>
                    <div class="col m3 pr-0" style="background-color:navy; color:white;">
                        <span class="badge-primary center"><i class="fa fa-calendar-check-o"></i><span>Quot.#</span></span>
                    </div>
                </div>
                <div class="row mt-1 ml-0 mr-0">
                    <div class="col m3 pr-0">
                        <small class="badge-primary center"><%# Eval("FollowupDate", "{0:dd-MM-yyyy}") %></small>
                    </div>
                    <div class="col m3 pr-0">
                        <small class="badge-primary center">
                            <span ID="Span1" runat="server" clientidmode="Static"><%# Eval("NextFollowupDate", "{0:dd-MM-yyyy}") %>&nbsp;<b><%# Eval("PreferredTime") %></b></span>    
                        </small>
                    </div>
                    <div class="col m3 pr-0">
                        <small class="badge-primary center">
                            <a title="Click to Show Inquiry" href="javascript:openDashboardBox('inquirybyno','view','<%# Eval("InquiryNo") %>');"><%# Eval("InquiryNo") %></a> 
                        </small>
                    </div>
                    <div class="col m3 pr-0">
                        <small class="badge-secondary center">
                            <a title="Click to Show Quotation" href="javascript:openDashboardBox('quotationbyno','view','<%# Eval("QuotationNo") %>');"><%# Eval("QuotationNo") %></a> 
                        </small>
                    </div>
                </div>
                <div class="row mt-1 ml-0 mr-0 gradient-45deg-light-blue-indigo" style="font-size:14px;">
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
                </div>
                <div class="row mt-1 ml-0 mr-0" style="background-color:navy; font-size:14px;">
                    <div class="col m12">
                        <small>
                            <span style="color:white; font-weight:800;">FollowUp Initiated By :</span>
                            <span style="color:white;"><b>&nbsp;<%# Eval("EmployeeName") %></b></span>
                            <span style="color:white; font-weight:800; margin-left:20px;">Initiated On :</span>
                            <span style="color:white;"><b>&nbsp;<%# Eval("CreatedDate", "{0:dd-MM-yyyy hh:mm tt}") %></b></span>
                            <span class="float-right" style="color:white; font-weight:800;">Inquiry Source :
                                <span style="color:white; text-transform:capitalize;"><b>&nbsp;<%# Eval("FollowUpSource") %></b></span>
                            </span>
                            
                        </small>
                    </div>
                </div>
            </li>
        </ul>
    </ItemTemplate>
</asp:Repeater>