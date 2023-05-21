<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckList.aspx.cs" Inherits="StarsProject.CheckList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png"/>
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png"/>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css"/>
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
            //$('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            ////$('.datepicker').on('mousedown', function (event) { event.preventDefault(); })
            //$('.timepicker').timepicker();
        });
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
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

        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        //function validateForm() {
        //    var errmsg = "";
        //    var validflag = true;
        //    // ----------------------------------------
        //    if (jQuery("#txtChargeName").val().trim() == "") {

        //        if (jQuery("#txtChargeName").val().trim() == "")
        //            errmsg += "<li>Charge Name is required</li>";

        //        validflag = false;
        //    }
        //    // ----------------------------------------
        //    if (validflag == false) {
        //        showcaseError(errmsg);
        //    }
        //    return validflag;
        //}

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnPkIDEmp").val('');
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper" style="padding:20px 15px;">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row"> 
                                        <div class="input-field col m6">
                                            <label class="active" for="drpCheckType">CheckList Type <span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:DropDownList ID="drpCheckType" runat="server" class="select2-theme browser-default" OnSelectedIndexChanged="drpCheckType_SelectedIndexChanged" AutoPostBack="true" TabIndex="1" placeholder="">
                                            </asp:DropDownList>
                                        </div>
                                    
                                        <div class="input-field col m3">
                                            <label class="active" for="txtCheckHead">Check Head<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtCheckHead" runat="server" ClientIDMode="Static" MaxLength="15" class="form-control" TabIndex="2" placeholder="" />
                                        </div>
                                        <div class="input-field col m3">
                                            <label class="active" for="txtCheckDesc">Check Description<span class="materialize-red-text font-weight-800">*</span></label>
                                            <asp:TextBox ID="txtCheckDesc" runat="server" ClientIDMode="Static" MaxLength="50" class="form-control" TabIndex="3" placeholder="" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="input-field col m6">
                                      
                                            <asp:UpdatePanel ID="upRepeater" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" ID="pnlDetail">
                                                        <table id="tblCheckList" class="table responsive-table striped" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <asp:Repeater ID="rptCheckListDetail" runat="server" ClientIDMode="Static">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-left">Check Head <span class="materialize-red-text font-weight-800"></span></th>
                                                                            <th class="text-right">Check Description <span class="materialize-red-text font-weight-800"></span></th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                                                        <td id="tdChkHead" class="text-left"><%# Eval("CheckHead") %></td>
                                                                        <td id="tdChkDescription" class="text-right"><%# Eval("CheckDesc") %></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div class="input-field col m6">
                                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:40px;">
                                                    <button ID="btnReset" type="button" runat="server" clientidmode="Static" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="4"><i class="material-icons left">backspace</i>Clear & Add New</button>
                                                    <button ID="btnSave" type="button" runat="server" clientidmode="Static" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="5"><i class="material-icons left">save</i>Save</button>
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
        </div>
    </form>
</body>
</html>
