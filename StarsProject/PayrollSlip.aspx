<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="PayrollSlip.aspx.cs" Inherits="StarsProject.PayrollSlip" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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

        .overlay
        {
            position: fixed;
            z-index: 999;
            height: 50%;
            width: 50%;
            top: 200px;
            left:500px;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.8;
            -moz-opacity: 0.8;
        }

        [type='checkbox']:not(:checked), [type='checkbox']:checked { 
            position:relative !important; 
            opacity:100;
            pointer-events:all;
            height: 20px;
            width: 20px;
        }
        td, th {
            padding:0px;
            color:black;
        } 
    </style>
    
    <script type="text/javascript">
        var updateProgress = null;

        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
        $(document).ready(function () {
            $('.btnTopRightCorner').on("click", function () {
                $("#btnBindGrd").trigger("click");
            });
        });

        function openAttendance(empid) {
            var m1, y1;
            m1 = jQuery("#drpSummaryMonth").val();
            y1 = jQuery("#drpSummaryYear").val();
            var pageUrl = "AttendanceStatus.aspx?empid=" + empid + "&month=" + m1 + "&year=" + y1;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        function CheckAll() {
            jQuery("#spnSelected").text($("#contentwrapper .chkToCompare").find('input:checked').length);
        }
        function ShowPDFfile(repFilename) {
            var today = new Date();
            var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            var dateTime = date + ' ' + time;

            yhooWin = window.open(repFilename + "?id=" + dateTime, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }

        function openPayslip(id) {
            var m1, y1;
            m1 = jQuery("#drpSummaryMonth").val();
            y1 = jQuery("#drpSummaryYear").val();
            var pageUrl = (id == 0) ? "PayrollSlipView.aspx?mode=add&month=" + m1 + "&year=" + y1 + "&id=0" : "PayrollSlipView.aspx?mode=edit&id=" + id;
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        function generatePaySlipPDF(id)
        {
            jQuery.ajax({
                type: "POST",
                url: 'PayrollSlip.aspx/GeneratePayslip',
                data: '{pkID:\'' + id + '\'}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var tmpfile = data.d.toString();
                    ShowPDFfile(tmpfile);
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />

    <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
            <div id="contentwrapper" class="contentwrapper">
                <div class="row">
                    <div class="col m10 m-0 p-0">
                        <div class="row m-0 card-title mb-0 left-align width-100 gradient-45deg-light-blue-indigo" style="line-height:50px; padding:10px 5px 5px 5px;">
                            <div class="col m4">
                                <h5>Pay Slip Information</h5>
                            </div>
                            <div class="col m6 float-right">
                                <button id="btnFilterData" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnFilterData_Click" tabindex="17"><i class="material-icons left">save</i>Filter Data</button>
                                <asp:TextBox ID="txtSearchBoxHeader" runat="server" ClientIDMode="Static" placeholder="Type Content To Search .... " class="form-control mb-0 white black-text float-right mr-1" Style="height:33px; width:300px; margin-bottom: 20px;" TabIndex="1" />
                            </div>
                        </div>
                        <div class="row m-0 p-0">
                            <div class="input-field col m12 center-align gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="padding:10px 5px 5px 10px;">
                                <button id="btnSelectAll" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan center mr-1 float-left" onserverclick="btnSelectAll_ServerClick" tabindex="17">Select All</button>
                                <asp:Label ID="lblSelectCount" runat="server" ClientIDMode="Static" CssClass=" float-left" />
                                <button id="btnSelEmailAll" type="button" runat="server" clientidmode="Static" class="btn red float-right ml-2 hide" onserverclick="btnSelEmailAll_ServerClick" TabIndex="8">Email Selected</button>
                                <button id="btnSelDelAll" type="button" runat="server" clientidmode="Static" class="btn red float-right ml-2" onserverclick="btnSelDelAll_ServerClick" TabIndex="8">Delete Selected</button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col m12">
                                <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <asp:Repeater ID="rptPayroll" runat="server" ClientIDMode="Static" OnItemCommand="rptPayroll_ItemCommand">
                                        <HeaderTemplate>
                                            <thead>
                                                <tr>
                                                    <th class="SelectAll center-align" style="font-weight: 600; width: 4%; color: navy;">Select<br /></th>
                                                    <th class="center-align">Employee ID</th>
                                                    <th class="left-align">Employee Name</th>
                                                    <th class="left-align">Gender</th>
                                                    <th class="left-align">Department<br />Designation</th>
                                                    <th class="center-align">Work.Days</th>
                                                    <th class="center-align">Pres.Days</th>
                                                    <th class="center-align">Paid Days</th>
                                                    <th class="center-align">Leaves</th>
                                                    <th class="center-align">OT Hrs</th>
                                                    <th class="right-align">Income</th>
                                                    <th class="right-align">Deduction</th>
                                                    <th class="right-align">Net Pay</th>
                                                    <th class="center-align" colspan="3">Action</th>
                                                </tr>
                                            </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                            <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" Value='<%# Eval("EmployeeID") %>' />
                                            <tr class="blueShed border-none pt-1" style="background-color:beige;">
                                                <td class="center-align" style="padding-top:8px;">
                                                    <asp:CheckBox CssClass="chkToCompare" ID="chkSelect" runat="server" ClientIDMode="Static" onclick="CheckAll()" />
                                                </td>
                                                <td class="center-align"><%# Eval("EmployeeID") %></td>
                                                <td class="left-align bold"><%# Eval("EmployeeName") %></td>
                                                <td class="left-align"><%# Eval("Gender") %></td>
                                                <td class="left-align"><%# Eval("OrgName") %></td>
                                                <td class="center-align navy-text"><%# Eval("WDays") %></td>
                                                <td class="center-align green-text">
                                                    <a href='<%# "javascript:openAttendance(" + Eval("EmployeeID") + ");" %>'><%# Eval("PDays") %></a>
                                                </td>
                                                <td class="center-align"><%# Eval("HDays") %></td>
                                                <td class="center-align red-text"><%# Eval("LDays") %></td>
                                                <td class="center-align"><%# Eval("ODays") %></td>
                                                <td class="right-align blue-text"><%# Eval("Total_Income") %></td>
                                                <td class="right-align red-text"><%# Eval("Total_Deduct") %></td>
                                                <td class="right-align navy-text"><%# Eval("NetSalary") %></td>
                                                <td class="center-align">
                                                    <asp:ImageButton ID="ImgbtnRegen" runat="server" ClientIDMode="Static" ImageUrl="~/images/pdf_document.png" Width="16" Height="16" ToolTip="Re-Generate" CommandName="printpayslip" TabIndex="1" CommandArgument='<%# Eval("pkID") %>' OnClientClick='<%# "generatePaySlipPDF(" + Eval("pkID") + " );" %>' />
                                                </td>
                                                <td class="text-center">
                                                    <asp:ImageButton ID="ImgbtnEdit" runat="server" ClientIDMode="Static" ImageUrl="~/images/edit.png" Width="16" Height="16" ToolTip="Edit Record" CommandName="Edit" TabIndex="2" OnClientClick='<%# "openPayslip(" + Eval("pkID") + " );" %>' />
                                                </td>
                                                <td class="text-center">
                                                    <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete Record" CommandName="Delete" TabIndex="3" CommandArgument='<%# Eval("pkID") %>' />
                                                </td>
                                            </tr>
                                            <tr class="blueShed" style="border-color:silver; background-color:beige;">
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td class="left-align badge navy-text"><%# Eval("EmailAddress") %></td>
                                                <td>&nbsp;</td>
                                                <td class="left-align pill navy-text"><%# Eval("Designation") %></td>
                                                <td colspan="11"></td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                            <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" Value='<%# Eval("EmployeeID") %>' />
                                            <tr class="blueShed border-none pt-1">
                                                <td class="center-align" style="padding-top:8px;">
                                                    <asp:CheckBox CssClass="chkToCompare" ID="chkSelect" runat="server" ClientIDMode="Static" onclick="CheckAll()" />
                                                </td>
                                                <td class="center-align"><%# Eval("EmployeeID") %></td>
                                                <td class="left-align bold"><%# Eval("EmployeeName") %></td>
                                                <td class="left-align"><%# Eval("Gender") %></td>
                                                <td class="left-align"><%# Eval("OrgName") %></td>
                                                <td class="center-align navy-text"><%# Eval("WDays") %></td>
                                                <td class="center-align green-text">
                                                    <a href='<%# "javascript:openAttendance(" + Eval("EmployeeID") + ");" %>'><%# Eval("PDays") %></a>
                                                </td>
                                                <td class="center-align"><%# Eval("HDays") %></td>
                                                <td class="center-align red-text"><%# Eval("LDays") %></td>
                                                <td class="center-align"><%# Eval("ODays") %></td>
                                                <td class="right-align blue-text"><%# Eval("Total_Income") %></td>
                                                <td class="right-align red-text"><%# Eval("Total_Deduct") %></td>
                                                <td class="right-align navy-text"><%# Eval("NetSalary") %></td>
                                                <td class="center-align">
                                                    <asp:ImageButton ID="ImgbtnRegen" runat="server" ClientIDMode="Static" ImageUrl="~/images/pdf_document.png" Width="16" Height="16" ToolTip="Re-Generate" CommandName="printpayslip" TabIndex="1" CommandArgument='<%# Eval("pkID") %>' OnClientClick='<%# "generatePaySlipPDF(" + Eval("pkID") + " );" %>' />
                                                </td>
                                                <td class="text-center">
                                                    <asp:ImageButton ID="ImgbtnEdit" runat="server" ClientIDMode="Static" ImageUrl="~/images/edit.png" Width="16" Height="16" ToolTip="Edit Record" CommandName="Edit" TabIndex="2" OnClientClick='<%# "openPayslip(" + Eval("pkID") + " );" %>' />
                                                </td>
                                                <td class="text-center">
                                                    <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete Record" CommandName="Delete" TabIndex="3" CommandArgument='<%# Eval("pkID") %>' />
                                                </td>
                                            </tr>
                                            <tr class="blueShed" style="border-color:silver;">
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td class="left-align badge navy-text"><%# Eval("EmailAddress") %></td>
                                                <td>&nbsp;</td>
                                                <td class="left-align pill navy-text"><%# Eval("Designation") %></td>
                                                <td colspan="11"></td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="col m2">
                        <div class="row mb-2">
                            <div class="col m12">
                                <h5 class="card-title mb-0 left-align width-100 gradient-45deg-light-blue-indigo">Filter Pay Slip Information</h5>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m12">
                                <label class="active" for="txtTaskDescription1">Select Month</label>
                                <asp:DropDownList ID="drpSummaryMonth" CssClass="select2-theme browser-default" runat="server" ClientIDMode="Static" AutoPostBack="true" TabIndex="4" OnSelectedIndexChanged="drpSummaryMonthYear_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="input-field col m12">
                                <label class="active" for="txtTaskDescription1">Select Year</label>
                                <asp:DropDownList ID="drpSummaryYear" CssClass="select2-theme browser-default" runat="server" ClientIDMode="Static" AutoPostBack="true" TabIndex="5" OnSelectedIndexChanged="drpSummaryMonthYear_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="input-field col m12 center-align padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom">
                                <button id="btnAddNew" type="button" runat="server"  clientidmode="Static" class="btn orange center" onclick ="javascript:openPayslip(0);" TabIndex="6">Add New Payslip</button>
                                <button id="btnGenerate" type="button" runat="server" clientidmode="Static" class="btn cyan center mr-1 hide" onserverclick="btnGenerate_Click" TabIndex="7">Re-Generate</button>
                                <button id="btnBindGrd" type="button" runat="server" clientidmode="Static" class="btn cyan center mr-1 hide" onserverclick="btnBindGrd_ServerClick" TabIndex="8">Bind Grid</button>
                            </div>
                            <div class="input-field col m12 center-align padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom">
                                <button id="btnAutoGenerate" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan center mr-1" onserverclick="btnAutoGenerate_Click" tabindex="17"><i class="material-icons left">save</i>Auto Generate</button>
                            </div>
                            <div class="input-field col m12 center-align padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom">
                                <button id="btnAutoReCalculate" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan center mr-1" onserverclick="btnAutoReCalculate_Click" tabindex="17"><i class="material-icons left">save</i>Re-Calculate</button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="upUserPanel">
    <ProgressTemplate>
        <div style=" z-index: 1000; margin-left: 350px; margin-top:200px;position: fixed; top: 150px; left:350px;">
            <img alt="" src="images/loaders/ajax-loader.gif" height="200" width="200"/>
        </div>
    </ProgressTemplate>
    </asp:UpdateProgress>
    <%--**************************** Payslip View Popup ***********************************--%>
    <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
    <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
        <div id="myModal123">
            <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                    <i class="material-icons prefix">ac_unit</i>
                    <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Payslip View</span>
                    <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                </h5>
            </div>
        </div>
        <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
        <br />
    </asp:Panel>

</asp:Content>

