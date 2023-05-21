<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DailyActivity.aspx.cs" Inherits="StarsProject.DailyActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "Daily Work Log";
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
            //$('.timepicker').timepicker();
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

        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        // -----------------------------------------------------
        function openPayslip(id) {
            var m1, y1;
            m1 = jQuery("#drpSummaryMonth").val();
            y1 = jQuery("#drpSummaryYear").val();
            // -------------------------------------------
            if (id == 0)
                jQuery.colorbox({
                    width: "90%", height: "90%", iframe: true, href: "DailyActivityView.aspx?mode=add&month=" + m1 + "&year=" + y1 + "&id=" + id, onClosed: function () {
                        location.reload();

                    }
                });
            else
                jQuery.colorbox({
                    width: "90%", height: "90%", iframe: true, href: "DailyActivityView.aspx?mode=edit&id=" + id, onClosed: function () {
                        location.reload();
                    }
                });
        }
    </script>
    <style type="text/css">
        .stdtable thead th {
            font-size: 12px !important;
        }

        .stdtable tr td {
            font-size: 12px !important;
        }

        .form-control {
            font-size: 12px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:HiddenField ID="hdnRole" runat="server" ClientIDMode="Static" />    
    <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnMenuID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAddFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEditFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDelFlag" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEmailTemplate" runat="server" ClientIDMode="Static" />

    <div id="contentwrapper" class="contentwrapper">
    <div class="row">
        <div class="col m7">
            <div class="row mb-1">
                <div class="col m12">
                    <h5 class="card-title mb-0 left-align width-100 gradient-45deg-light-blue-indigo">Work Log Information</h5>
                </div>
            </div>
            <div class="row">
                <div class="col m12">
                <table id="tblInqProductGroup" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                    <asp:Repeater ID="rptDailyActivity" runat="server" ClientIDMode="Static" OnItemCommand="rptDailyActivity_ItemCommand" OnItemDataBound="rptDailyActivity_ItemDataBound">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th class="text-left" style="width: 30%; color: white;">Employee Name</th>
                                    <th class="text-cetner" style="width: 40%; color: white;">Work Notes</th>
                                    <th class="text-cetner" style="width: 10%; color: white;">Work Category</th>
                                    <th class="text-cetner" style="width: 10%; color: white;">Work Hrs.</th>
                                    <th class="text-center" style="width: 10%; color: white;" colspan="2">Action</th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                <asp:HiddenField ID="hdnCatID" runat="server" ClientIDMode="Static" Value='<%#Eval("TaskCategoryID") %>' />
                                <td class="text-center">
                                    <asp:Label ID="lblEmployeeName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="101" MaxLength="250" Text='<%#Eval("CreatedEmployeeName") %>' />
                                </td>
                                <td class="text-center">
                                    <asp:Label ID="txtTaskDescription" runat="server" class="form-control" ClientIDMode="Static" TabIndex="102"  MaxLength="250" Text='<%#Eval("TaskDescription") %>' />
                                </td>
                                <td class="text-center">
                                    <asp:Label ID="lblTaskCategory" runat="server" class="form-control" ClientIDMode="Static" TabIndex="103" Width="80px" Text='<%#Eval("TaskCategoryName") %>' />
                                </td>
                                <td class="text-center">
                                    <asp:Label ID="txtTaskDuration" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='<%#Eval("TaskDuration") %>' />
                                </td>
                                <td class="text-center">
                                    <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="~/images/edit.png" ToolTip="Save" Width="20" Height="20" TabIndex="105" CommandName="Save" CommandArgument='<%# Eval("pkID") %>' />
                                </td>
                                <td class="text-center">
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" Width="20" Height="20" TabIndex="106" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:HiddenField ID="hdnTotTaskDesc" runat="server" ClientIDMode="Static"  />
                            <tr>
                                <td class="text-center">
                                    <asp:Label ID="lblEmpName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="101" MaxLength="250"  />
                                </td>
                                <td class="text-center">
                                    <asp:Label ID="txtTaskDesc" runat="server" class="form-control" ClientIDMode="Static" TabIndex="102"  MaxLength="150"  />
                                </td>
                                <td class="text-center">
                                    <asp:Label ID="lblTaskCat" runat="server" class="form-control" ClientIDMode="Static" TabIndex="103" Width="200px" />
                                </td>
                                <td class="text-center">
                                    <asp:Label ID="txtTotTaskDuration" runat="server" class="form-control" ClientIDMode="Static" TabIndex="104" Width="80px" Text='<%#Eval("hdnTotTaskDesc") %>' />
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
                </div>
            </div>
            <%--<div class="row">
                <asp:Label ID="txtTotTaskDuration" runat="server" class="form-control right-align" ClientIDMode="Static" TabIndex="104" Width="800px" />
            </div>--%>
        </div>
        
        <div class="col m5">
            <div class="row mb-2">
                <div class="col m12">
                    <h5 class="card-title mb-0 left-align width-100 gradient-45deg-light-blue-indigo">Add New Work Log</h5>
                </div>
            </div>
            <div class="row">
                <div class="col m12">
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                    <div class="row">
                        <div class="input-field col m12">
                            <label class="active" for="drpEmployee">Select Employee<span class="materialize-green-text font-weight-800">*</span>
                                <a href="javascript:openEmployeeInfo('view');">
                                    <img src="images/registration.png" width="30" height="20" alt="Preview Employee Info" title="Preview Employee Info" style="display: inline-block;"/>
                                </a>
                            </label>
                            <asp:DropDownList ID="drpEmployee" class="select2-theme browser-default mr-2 float-left" runat="server" ClientIDMode="Static" style="background-color: #9bc2e4; width: 325px; float: right; font-size: 18px; margin-top: 1px; height: 35px !important; border: 1px solid black; padding: 1px;" AutoPostBack="true" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged" TabIndex="1" ></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="input-field col m5">
                            <label class="active" for="txtActivityDate">Select Date <span class="materialize-red-text font-weight-800">*</span></label>
                            <asp:TextBox ID="txtActivityDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" onpaste="return false" AutoPostBack="True" placeholder="" OnTextChanged="txtActivityDate_TextChanged"  TextMode="Date"/>
                        </div>
                        <div class="input-field col m5">
                            <label class="active" for="drpTaskCategory1">Type Of Work <span class="materialize-red-text font-weight-800">*</span></label>
                            <asp:DropDownList ID="drpTaskCategory1" runat="server" class="select2-theme browser-default" EnableViewState="true" ClientIDMode="Static" TabIndex="3" placeholder="" />
                        </div>
                        <div class="input-field col m2">
                            <label class="active" for="txtTaskDuration1">Worked Hrs <span class="materialize-red-text font-weight-800">*</span></label>
                            <asp:TextBox ID="txtTaskDuration1" runat="server" class="form-control" ClientIDMode="Static" MaxLength="15" TabIndex="4" Width="80px" Text='<%#Eval("TaskDuration") %>' placeholder="" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="input-field col m12">
                            <label class="active" for="txtTaskDescription1">Work Notes <span class="materialize-red-text font-weight-800">*</span> </label>
                            <asp:TextBox ID="txtTaskDescription1" runat="server" class="form-control" ClientIDMode="Static" TabIndex="5"  TextMode="MultiLine" Rows="3" Text='<%#Eval("TaskDescription") %>' placeholder="" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="input-field col m12">
                            <label class="active" for="drpTODO">Select Task</label>
                            <asp:DropDownList ID="drpTODO" runat="server" CssClass="select2-theme browser-default" ClientIDMode="Static" TabIndex="6"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="input-field col m12">
                            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                <button id="btnReset" type="button" runat="server"  clientidmode="Static" class="btn orange left" onserverclick="btnReset_Click" TabIndex="9"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                <button id="btnSaveEmail" type="button" runat="server" clientidmode="Static" class="btn cyan  right mr-1" onserverclick="btnSaveEmail_Click" tabindex="7"><i class="material-icons left">email</i>Only Send Email</button>
                                <button id="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="8"><i class="material-icons left">save</i>Save</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
