<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myAllLeads.ascx.cs" Inherits="StarsProject.myAllLeads" %>

<link rel="stylesheet" type="text/css" href="app-assets/vendors/ionRangeSlider/css/ion.rangeSlider.css">
<link rel="stylesheet" type="text/css" href="app-assets/vendors/ionRangeSlider/css/ion.rangeSlider.skinFlat.css">

<script src="app-assets/vendors/ionRangeSlider/js/ion.rangeSlider.js"></script>
<script src="app-assets/js/scripts/extra-components-range-slider.js"></script>

<asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnSelUserID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnSelEmpID" runat="server" ClientIDMode="Static" EnableViewState="true" />
<asp:HiddenField ID="hdnLeadStatus" runat="server" ClientIDMode="Static" EnableViewState="true" />

<asp:HiddenField ID="hdnFromDays" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
<asp:HiddenField ID="hdnToDays" runat="server" ClientIDMode="Static" EnableViewState="true" Value="365" />
<style type="text/css">
    td, th { color:black; font-size:14px; padding-top:5px; }

    .irs--flat .irs-from, .irs--flat .irs-to, .irs--flat .irs-single { background-color:dodgerblue !important; }

    .irs-bar { background-color:darkseagreen !important; }
</style>

<div class="mb-2" style="width:100%; padding: 5px 30px; border: 2px ridge silver; border-radius: 8px; background-color: beige;">
    <div class="hide" id="example_id"></div>
</div>
<table id="tblAllLead" class="table table-striped table-bordered" style="font-size:12px;" cellpadding="0" cellspacing="0" border="0" width="100%">
    <asp:Repeater ID="rptAllLead" runat="server" OnItemDataBound="rptAllLead_ItemDataBound">
        <HeaderTemplate>
            <thead class="thead-dark" style="font-size:12px;">
                <tr>
                    <th class="center-align">Activity Days</th>
                    <th class="center-align">Source</th>
                    <th class="center-align">Status</th>
                    <th class="center-align">Lead #</th>
                    <th class="center-align">Lead Date</th>
                    <th class="left-align">Customer Name</th>
                    <th class="left-align">Assigned To</th>
                    <th class="center-align">Last FollowUp</th>
                    <th class="center-align">Next FollowUp</th>
                    <th class="center-align">Init. By</th>
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%# Eval("CustomerID") %>' />
                <td class="center-align"><%# Eval("ActivityDays") %><small class="materialize-red-text text-muted font-italic">&nbsp;Days</small></td>
                <td class="center-align"><%# Eval("TypeOfData") %></td>
                <td class="center-align"><%# Eval("InquiryStatus") %></td>
                <td class="center-align"><%# Eval("InquiryNo") %></td>
                <td class="center-align"><%# Eval("InquiryDate", "{0:dd/MM/yyyy}") %></td>
                <td class="left-align"><%# Eval("CustomerName") %></td>
                <td class="left-align"><%# Eval("EmployeeName") %></td>
                <td class="center-align"><%# Eval("LastFollowupDate", "{0:dd/MM/yyyy}") %></td>
                <td class="center-align"><%# Eval("LastNextFollowupDate", "{0:dd/MM/yyyy}") %></td>
                <td class="center-align"><%# Eval("CreatedByEmployee") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>

<button ID="btnTemp" type="button" runat="server" clientidmode="Static" Text="Save" class="btn hide" onserverclick="btnTemp_Click" TabIndex="21">Save</button>
<script type="text/javascript">
    // ----------------------------------------------------
    // Section : Inintialize Bar
    // ----------------------------------------------------
    initLeadBar();
    // ----------------------------------------------------
    function initLeadBar()
    {
        var x1 = $("#hdnFromDays").val();
        var y1 = $("#hdnToDays").val();

        $("#example_id").ionRangeSlider({
            skin: "modern",
            type: "double",
            min: 0,
            max: 365,
            from: x1,
            to: y1,
            grid: true,
            prefix: " Days ",
            onStart: function (data) {
                // fired then range slider is ready
            },
            onChange: function (data) {
                // fired on every range slider update
            },
            onFinish: function (data) {
                $("#hdnFromDays").val(data.from);
                $("#hdnToDays").val(data.to);
                $("#btnTemp").click();
            },
            onUpdate: function (data) {
                // fired on changing slider with Update method
            }
        });
    }
    function setBarRange(x1, y1) {
        $("#example_id").ionRangeSlider({
            from: x1,
            to: y1,
        });
    }
</script>