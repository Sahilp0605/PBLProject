<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="SendQuickEmail.aspx.cs" Inherits="StarsProject.SendQuickEmail" %>

<!DOCTYPE html>

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

    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <link href="css/font-awesome-Editor.min.css" rel="stylesheet" />

    <link href="css/richtext.min.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.richtext.min.js"></script>

    <style type="text/css">
        .jqte { margin-top: 5px;}
        .jqte_editor, .jqte_source {
            min-height: 300px !important;
        }
        .required:after {
            content: " *";
            color: #f00;
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
        $(document).ready(function () {
            
            $('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            $('.timepicker').timepicker();

            $(".content").richText();
        });
        function setEditor() {
            $(".content").richText();
        }
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
        function CheckAll() {
            jQuery("#spnSelected").text($("#contentwrapper .chkToCompare").find('input:checked').length);
        }

    </script>

</head>
<body>
    <form id="frmQuickEmail" runat="server" class="edit-email-item mt-2 mb-10">
        <asp:HiddenField ID="hdnTemplateID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPass" runat="server" ClientIDMode="Static" />
        <div class="row">
            <div class="input-field col m6">
                <div class="row">
                    <div class="input-field col m2">
                        <label class="active" for="drpTemplate">Communication By</label>
                        <asp:DropDownList ID="drpCommType" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpCommType_SelectedIndexChanged">
                            <asp:ListItem Text="Send Email" Value="Email" />
                            <asp:ListItem Text="Send SMS" Value="Sms" />
                        </asp:DropDownList>
                    </div>
                    <div class="input-field col m2">
                        <label class="active" for="drpTemplate">Select Template</label>
                        <asp:DropDownList ID="drpType" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpType_SelectedIndexChanged">
                            <asp:ListItem Text="Employee" Value="Employee" />
                            <asp:ListItem Text="Customer" Value="Customer" />
                        </asp:DropDownList>
                    </div>
                    <div class="input-field col m6">
                        <label class="active" for="drpTemplate">Select Template</label>
                        <asp:DropDownList ID="drpTemplate" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpTemplate_SelectedIndexChanged" />
                    </div>
                </div>
                <div class="row">
                    <div class="input-field col m12" style="max-height:500px; height:500px; overflow-y:auto;">
                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                            <asp:Repeater ID="rptEmployee" runat="server" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <thead>
                                        <tr>
                                            <th class="SelectAll center-align" style="font-weight: 600; width: 4%; color: navy;">Select<br /></th>
                                            <th class="left-align">Employee Name</th>
                                            <th class="left-align">Email Address</th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="blueShed border-none pt-1" style="background-color:beige;">
                                        <td class="center-align" style="padding-top:8px;">
                                            <asp:CheckBox CssClass="chkToCompare" ID="chkSelect" runat="server" ClientIDMode="Static" onclick="CheckAll()" />
                                        </td>
                                        <td id="tdName" runat="server" class="left-align bold"><%# Eval("EmployeeName") %></td>
                                        <td id="tdEmail" runat="server" class="left-align bold"><%# Eval("EmailAddress") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="rptCustomer" runat="server" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <thead>
                                        <tr>
                                            <th class="SelectAll center-align" style="font-weight: 600; width: 4%; color: navy;">Select<br /></th>
                                            <th class="left-align">Customer Name</th>
                                            <th class="left-align">Email Address</th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="blueShed border-none pt-1" style="background-color:beige;">
                                        <td class="center-align" style="padding-top:8px;">
                                            <asp:CheckBox CssClass="chkToCompare" ID="chkSelect" runat="server" ClientIDMode="Static" onclick="CheckAll()" />
                                        </td>
                                        <td id="tdName" runat="server" class="left-align bold"><%# Eval("CustomerName") %></td>
                                        <td id="tdEmail" runat="server" class="left-align bold"><%# Eval("EmailAddress") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
            <div class="input-field col m6">
                <div id="divFrom" runat="server" class="row">
                    <div class="input-field col m12">
                        <label class="active" for="txtFrom">From</label>
                        <asp:TextBox ID="txtFrom" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" ReadOnly="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="input-field col m12">
                        <label class="active" for="txtTo">To</label>
                        <asp:TextBox ID="txtTo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                    </div>
                </div>
                <div id="divCC" runat="server" class="row">
                    <div class="input-field col m12">
                        <label class="active" for="txtCC">CC</label>
                        <asp:TextBox ID="txtCC" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                    </div>
                </div>
                <div id="divBCC" runat="server" class="row hide">
                    <div class="input-field col m12">
                        <label class="active" for="txtBCC">BCC</label>
                        <asp:TextBox ID="txtBCC" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                    </div>
                </div>
                <div id="divSubject" runat="server" class="row">
                    <div class="input-field col m12">
                        <label class="active" for="txtSubject">Subject</label>
                        <asp:TextBox ID="txtSubject" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                    </div>
                </div>
                <div class="row">
                    <div class="input-field col m12">
                        <label for="txtEditor" class="active required">Email Content</label>
                        <asp:TextBox ID="txtEditor" runat="server" TabIndex="6" class="content" TextMode="MultiLine" placeholder="" EnableViewState="true" ClientIDMode="Static"/>
                    </div>
                </div>
                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                    <button id="btnSaveEmail" type="button" runat="server" clientidmode="Static" Text="Send Email" class="btn cyan right mr-1" onserverclick="btnSaveEmail_Click" TabIndex="9"><i class="material-icons left">email</i>Save - Send Email</button>
                </div>

            </div>
        </div>

    </form>
</body>
</html>
