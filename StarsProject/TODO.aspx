<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TODO.aspx.cs" Inherits="StarsProject.TODO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>  

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    <style type="text/css">
        .clDiv70 {
            height: 50px;
            vertical-align: middle;
        }

        .imgChart {
            vertical-align: middle;
            Height: 40px;
            width: 40px;
        }

        #drpSelectDepartment {
            margin: 7px 0px;
            font-family: Verdana,Arial;
            font-size: 12px;
            vertical-align: middle;
        }

        .clTrans {
            background-color: transparent;
        }

        .star-fg .glyphicon-star {
            color: red !important;
        }

        #hearts {
            color: #FF0000;
        }

        #hearts-existing {
            color: #87bad7;
        }

        .ui-widget-header, .ui-state-default, ui-button {
            background: #b9cd6d;
            border: 1px solid #b9cd6d;
            color: #FFFFFF;
            font-weight: bold;
        }

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

        #chkReminder { position: absolute; pointer-events:all; opacity: 100; width:20px; height:20px;}
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
            $('.timepicker').timepicker();
            checkedEventCall();
        });

        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }

        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function showErrorMessage(strMess) {
            jQuery.confirm({ title: 'Data Validation', content: 'Are you sure, You want to delete record !', type: 'red', typeAnimated: true });
        }

        /*---------------------------------------------------------*/
        function openEmployeeInfo(cat) {
            var keyid = "";
            keyid = (cat == 'view') ? $('#drpAssignTo').val() : "0";
            var pageUrl = (cat == 'view') ? "OrgEmployee.aspx?mode=view&id=" + keyid : "OrgEmployee.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*---------------------------------------------------------*/
        function showErrorPopup(xTitle, xMsg) {
            if (xTitle == '')
                xTitle = 'Message';
            // -------------------------------------
            if (xMsg != '') {
                jQuery.confirm({
                    title: xTitle, content: xMsg, type: 'red', typeAnimated: true, width: 'auto',
                    buttons: { close: { text: 'Close', btnClass: 'btn-blue ConfirmClose', action: function () { } } }
                });
            }
        }

        function checkedEventCall() {
            $('#chkReminder').bind('change', function () {
                if ($(this).is(":checked")) {
                    $("#textDueDate").hide();
                    $("#divReminder").show();
                }
                else {
                    $("#textDueDate").show();
                    $("#divReminder").hide();
                }
            }).trigger('change');
        }

        function bindDDLTo(selector) {
            if ($(selector).val().length >= 3) {
                jQuery.ajax({
                    type: "POST",
                    url: "InquiryInfo.aspx/FilterCustomerByModule",
                    data: '{pCustName:\'' + $(selector).val() + '\', pSearchModule:\'CustomerSearchAll\'}',
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
                                //console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
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
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">

    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hdnToDOMode" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCreatedBy" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAssignTo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAssignToEmployeeName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCompletionDate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
    <div id="contentwrapper" class="contentwrapper">

        <%--<asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <div class="widgetbox">
                    <div class="clearall"></div>
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                    

                    <%--<div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col m4">
                                        <div class="row">
                                            <div class="input-field col m12">
                                                <label class="active" for="txtTaskDescription">Task Description <span class="materialize-red-text font-weight-800">*</span> <small class="text-muted font-italic"> (Max 350 Char) </small></label>
                                                <asp:TextBox ID="txtTaskDescription" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" MaxLength="150" TextMode="MultiLine" Rows="3" placeholder="" style="min-height:200px;" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m8">
                                        <div class="row pb-2">
                                            <div class="input-field col m3">
                                                <label class="active" for="drpTaskCategory">Category <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:DropDownList ID="drpTaskCategory" runat="server" CssClass="select2-theme browser-default" ClientIDMode="Static" TabIndex="2"></asp:DropDownList>
                                            </div>
                                            <div class="input-field col m3">
                                                <label class="active" for="txtLocation">Location</label>
                                                <asp:TextBox ID="txtLocation" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3" MaxLength="50" placeholder="Max 50 char" />
                                            </div>
                                            <div class="input-field col m2">
                                                <label class="active" for="chkReminder">Reminder Alert</label>
                                                <asp:CheckBox id="chkReminder" class="filled-in" runat="server" clientidmode="Static" TabIndex="4" />
                                            </div>
                                            <div id="divReminder" runat="server" clientidmode="Static" class="input-field col m4" style="display:none;">
                                                <label class="active" for="drpReminderMonth">Reminder Type</label>
                                                <asp:DropDownList ID="drpReminderMonth" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="5">
                                                    <asp:ListItem Text="Just Once" Value="0" />
                                                    <asp:ListItem Text="Every Month" Value="1" />
                                                    <asp:ListItem Text="Quarterly" Value="3" />
                                                    <asp:ListItem Text="Half Yearly" Value="6" />
                                                    <asp:ListItem Text="Yearly" Value="12" />
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                        <div class="row pb-2">
                                            <div class="input-field col m8">
                                                <label class="active" for="drpAssignTo">Assign To <span class="materialize-red-text font-weight-800">*</span>
                                                    
                                                    <a href="javascript:openEmployeeInfo('view');">
                                                        <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;" tabindex="17" />
                                                    </a>
                                                    <a href="javascript:openEmployeeInfo('add');">
                                                        <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                    </a>
                                                </label>
                                                <asp:DropDownList ID="drpAssignTo" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="6"></asp:DropDownList>
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="drpPriority">Priority Level</label>
                                                <asp:DropDownList ID="drpPriority" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="7">
                                                    <asp:ListItem Text="Low" Value="Low" />
                                                    <asp:ListItem Text="Medium" Value="Medium" />
                                                    <asp:ListItem Text="High" Value="High" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row pb-2">
                                            <div class="input-field col m4 pr-0">
                                                <label class="active" for="txtStartDate">Start Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtStartDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="8" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date" Width="150" />
                                                <asp:TextBox ID="txtStartTime" runat="server" class="timepicker" PlaceHolder="From Time" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" Width="80px" TabIndex="9" />
                                            </div>
                                            <div class="input-field col m4 pr-0" id="textDueDate">
                                                <label class="active" for="txtDueDate">Due Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtDueDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="10" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date"  Width="150"/>
                                                <asp:TextBox ID="txtDueTime" runat="server" class="timepicker" PlaceHolder="From Time" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" Width="80px" TabIndex="11" />
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="txtCompletionDate">Actual Completion</label>
                                                <asp:TextBox ID="txtCompletionDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="12" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m12">
                                        <label class="active" for="txtClosingRemarks">Closing Remarks <small class="text-muted font-italic"> (Max 350 Char) </small></label>
                                        <asp:TextBox ID="txtClosingRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="13" MaxLength="150" TextMode="MultiLine" Rows="3" placeholder="" style="min-height:200px;" />
                                    </div>
                                </div>
                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                    <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="13"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                    <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="14"><i class="material-icons left">save</i>Save</button>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                    <div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row mt-1 mb-1" style="border-bottom:4px solid;">
                                    <div class="col m4">
                                        <div class="row">
                                            <div class="input-field col m12">
                                                <label class="active" for="txtTaskDescription">Task Description <span class="materialize-red-text font-weight-800">*</span> <small class="text-muted font-italic"> (Max 350 Char) </small></label>
                                                <asp:TextBox ID="txtTaskDescription" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" MaxLength="150" TextMode="MultiLine" placeholder="" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m8">
                                        <div class="row">
                                            <div class="input-field col m3">
                                                <label class="active" for="drpTaskCategory">Category <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:DropDownList ID="drpTaskCategory" runat="server" CssClass="select2-theme browser-default" ClientIDMode="Static" TabIndex="2"></asp:DropDownList>
                                            </div>
                                            <div class="input-field col m2">
                                                <label class="active" for="drpPriority">Priority Level</label>
                                                <asp:DropDownList ID="drpPriority" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="3">
                                                    <asp:ListItem Text="Low" Value="Low" />
                                                    <asp:ListItem Text="Medium" Value="Medium" />
                                                    <asp:ListItem Text="High" Value="High" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="input-field col m3">
                                                <label class="active" for="txtLocation">Location</label>
                                                <asp:TextBox ID="txtLocation" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" MaxLength="50" placeholder="Max 50 char" />
                                            </div>
                                            <div class="input-field col m4">
                                                <label class="active" for="drpAssignTo">Assign To <span class="materialize-red-text font-weight-800">*</span>
                                                    <a href="javascript:openEmployeeInfo('view');">
                                                        <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;"/>
                                                    </a>
                                                    <a href="javascript:openEmployeeInfo('add');">
                                                        <img src="images/addCustomer.png" width="30" height="20" alt="Add New Employee" title="Add New Employee" />
                                                    </a>
                                                </label>
                                                <asp:DropDownList ID="drpAssignTo" runat="server" CssClass="select2-theme browser-default" ClientIDMode="Static" TabIndex="5"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row" style="min-height: 75px;">
                                            <div class="input-field col m2">
                                                <label class="active" for="chkReminder">Reminder Alert</label>
                                                <asp:CheckBox id="chkReminder" class="filled-in" runat="server" clientidmode="Static" TabIndex="6" />
                                            </div>

                                            <div id="divReminder" runat="server" clientidmode="Static" class="input-field col m2" style="display:none;">
                                                <label class="active" for="drpReminderMonth">Reminder Type</label>
                                                <asp:DropDownList ID="drpReminderMonth" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="7">
                                                    <asp:ListItem Text="Just Once" Value="0" />
                                                    <asp:ListItem Text="Every Month" Value="1" />
                                                    <asp:ListItem Text="Quarterly" Value="3" />
                                                    <asp:ListItem Text="Half Yearly" Value="6" />
                                                    <asp:ListItem Text="Yearly" Value="12" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="input-field col m3 pr-0">
                                                <label class="active" for="txtStartDate">Start Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtStartDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="8" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date" Width="150" />
                                                <asp:TextBox ID="txtStartTime" runat="server" class="timepicker" PlaceHolder="From Time" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" Width="80px" TabIndex="9" />
                                            </div>
                                            <div class="input-field col m3 pr-0" id="textDueDate">
                                                <label class="active" for="txtDueDate">Due Date <span class="materialize-red-text font-weight-800">*</span></label>
                                                <asp:TextBox ID="txtDueDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="10" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date"  Width="150"/>
                                                <asp:TextBox ID="txtDueTime" runat="server" class="timepicker" PlaceHolder="From Time" onkeypress="return false;" onpaste="return false" ClientIDMode="Static" Width="80px" TabIndex="11" />
                                            </div>
                                            <%if (hdnSerialKey.Value == "SI08-SB94-MY45-RY15")
                                                { %>
                                            <div class="input-field col m4">
                                                <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" placeholder="Minimum 3 chars to activate search" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" TabIndex="1" style="margin-top:3px"/>
                                                <label class="active" for="txtCustomerName" style="margin-top:3px">Select Customer</label>
                                            </div>
                                            <% } %>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m4">
                                        <div class="input-field col m12">
                                            <label class="active" for="drpOption">Transfer To</label>
                                            <asp:DropDownList ID="drpOption" runat="server" CssClass="select2-theme browser-default" ClientIDMode="Static" TabIndex="12" AutoPostBack="true" OnSelectedIndexChanged="drpOption_SelectedIndexChanged">
                                                <asp:ListItem Text="Complete Task" Value="complete" Selected="True" />
                                                <asp:ListItem Text="Re-Assign Task" Value="reassign" />
                                                <asp:ListItem Text="Add Activity" Value="activity" />
                                            </asp:DropDownList>
                                        </div>
                                        <div id="divTransfer" runat="server" clientidmode="Static" class="input-field col m12" style="display:none;">
                                            <label class="active" for="drpTransferTo">Transfer To</label>
                                            <asp:DropDownList ID="drpTransferTo" runat="server" CssClass="select2-theme browser-default" ClientIDMode="Static" TabIndex="12" AutoPostBack="true" OnSelectedIndexChanged="drpTransferTo_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div id="divCompDate" runat="server" clientidmode="Static" class="input-field col m12" style="display:block;">
                                            <label class="active" for="txtCompletionDate">Actual Completion</label>
                                            <asp:TextBox ID="txtCompletionDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="13" onkeypress="return false;" onpaste="return false" placeholder="" TextMode="Date" />
                                        </div>

                                        <div class="input-field col m12">
                                            <label class="active" for="txtClosingRemarks">Closing Remarks <small class="text-muted font-italic"> (Max 350 Char) </small></label>
                                            <asp:TextBox ID="txtClosingRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="14" MaxLength="150" TextMode="MultiLine" Rows="3" placeholder="" style="min-height:100px;" />
                                        </div>
                                    </div>
                                    <div class="col m8" style="height:400px; overflow-y:auto;">
                                        <table id="tblToDOLog" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                            <asp:Repeater ID="rptToDOLog" runat="server" ClientIDMode="Static" OnItemDataBound="rptToDOLog_ItemDataBound">
                                                <HeaderTemplate>
                                                    <thead>
                                                        <tr>
                                                            <th class="center-align black-text" style="width: 5%;">Sr.#</th>
                                                            <th class="left-align black-text" style="width: 15%;">Action</th>
                                                            <th class="left-align black-text" style="width: 40%;">Description</th>
                                                            <th class="center-align black-text" style="width: 20%;">Initiated By</th>
                                                            <th class="center-align black-text" style="width: 20%;">Initiated On</th>
                                                        </tr>
                                                    </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnClosingRemarks" runat="server" ClientIDMode="Static" Value='<%#Eval("ClosingRemarks") %>' />
                                                    <tr>
                                                        <td class="center-align " style="color:black;"><%#Eval("RowNum") %></td>
                                                        <td class="left-align " style="color:black;"><%#Eval("ActionTaken") %></td>
                                                        <td class="left-align " style="color:maroon;"><%#Eval("TaskDescription") %></td>
                                                        <td class="center-align" style="color:black;"><%#Eval("FromEmployeeName") %></td>
                                                        <td class="center-align" style="color:black;"><%#Eval("CreatedDate", "{0: dd/MM/yyyy HH:mm:ss}") %></td>
                                                    </tr>
                                                    <tr id="trRemarks" runat="server" clientidmode="Static">
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td class="center-align">
                                                            <asp:TextBox ID="txtClosingRemarks" runat="server" class="form-control" ClientIDMode="Static" Rows="2" TextMode="MultiLine" style="color:navy; border:none;resize:none; height:auto;overflow: hidden; min-height:auto;" Enabled="false" Text='<%#Eval("ClosingRemarks") %>'/>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </div>
                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                    <button ID="btnReset" type="button" runat="server"  clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="16"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                    <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="15"><i class="material-icons left">save</i>Save</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>

                <%--**************************** Employee Popup ***********************************--%>
                <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" /> 
                <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">  
                </cc1:ModalPopupExtender>  
                <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" style = "display:none; border-radius:10px; padding-right: 10px !important;">  
                    <div id="myModal1215">
                        <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom:10px !important;">
                            <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                                <i class="material-icons prefix">ac_unit</i>
                                <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                                <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text=""/>
                            </h5>
                        </div>
                    </div>
                    <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border:1px solid silver; background-color:transparent; width:100%; height:90%; padding:5px;"></iframe>  
                   <br/>  
                </asp:Panel> 
                
    </form>
</body>
</html>
