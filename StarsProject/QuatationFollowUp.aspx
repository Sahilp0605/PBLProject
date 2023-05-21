<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="QuatationFollowUp.aspx.cs" Inherits="StarsProject.QuatationFollowUp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <style type="text/css">
        .card-title {
            background: linear-gradient(45deg, #303f9f, #7b1fa2) !important;
        }
        .jqte { margin:0px; }
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
        .ui-autocomplete-input
        { 
            width: 600px;
        }

        .autocomplete-suggestions
        {
             min-width: 600px !important;
        }
        a:hover { cursor:pointer; }
    </style>

    <script type="text/javascript">
        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }

        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

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

        function ShowPDFfile(repFilename) {
            debugger;
            repFilename =  repFilename;
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }
    </script>
</head>
<body>
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <div id="contentwrapper" class="contentwrapper">
        <asp:HiddenField ID="hdnLogID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnQuatationNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnQuatationpkID" runat="server" ClientIDMode="Static" />
        <div id="myModal" style="display: block; width: 98%; overflow-x: hidden;">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <div class="col m6">
                            <div class="row badge-light" style="border-radius: 5px;">
                                <div class="col m3">
                                    <label class="active" for="txtFollowupNo">Followup No </label>
                                    <asp:TextBox Enabled="false" ID="txtFollowupNo" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "/>
                                </div>
                                <div class="col m9">
                                    <label class="active" for="txtRemarks">Remarks </label>
                                    <asp:TextBox ID="txtRemarks" runat="server" class="form-control" ClientIDMode="Static" MaxLength="50" TabIndex="7" placeholder=" "  TextMode="MultiLine"/>
                                </div>
                            </div>
                            <div id="quatationfollowup" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                            <asp:UpdatePanel ID="pnlQuatationFollowup" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <div class="section app-file-manager-wrapper">
                                <div class="app-file-overlay"></div>
                                <div class="row">
                                <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                    <div class="app-file-header">
                                        <div class="row">
                                            <div class="input-field col m12 add-new-file mt-0">
                                                <div class="col m12">
                                                    <label class="active" for="uploadDocument">Upload Documents Here  </label>
                                                </div>
                                                <div class="col m12">
                                                    <asp:FileUpload ID="UploadFile" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select File" /> 
                                                    <asp:Button ID="btnUpload" CssClass="hide" runat="server" Text="Upload"  onclick="btnUpload_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="app-file-content">
                                        <div class="app-file-sidebar-content row">
                                            <div class="col m12 app-file-files">
                                                <asp:Repeater ID="rptQuatationFollowUp" runat="server" ClientIDMode="Static" OnItemCommand="rptQuatationFollowUp_ItemCommand">
                                                    <ItemTemplate>
                                                    <div class="col xl4 l6 m4 s6">
                                                        <div class="card box-shadow-none mb-1 app-file-info">
                                                            <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                    <div class="row fonticon p2">
                                                                        <div class="col m6" style="text-align:left;">
                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("RowNum") %>'>
                                                                            <i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                                        </div>
                                                                        <div class="col m6" style="text-align:right;">
                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                            <i class="material-icons">find_in_page</i>
                                                                            </a>
                                                                        </div>
                                                                    </div>
                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                </div>
                                                                <div class="app-file-details">
                                                                    <div class="app-file-name font-weight-700">
                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(18) %></p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        </div>
                        <div class="col m6">
                        <table id="tblQuatationFollowupLog" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                        <asp:Repeater ID="rptQuatationFollowupLog" runat="server" ClientIDMode="Static">
                            <HeaderTemplate>
                                <thead>
                                    <tr>
                                        <th class="center-align black-text" style="width: 5%;">Sr.#</th>
                                        <%--<th class="left-align black-text" style="width: 15%;">Fol.No</th>--%>
                                        <th class="left-align black-text" style="width: 25%;">Remark</th>
                                        <th class="left-align black-text" style="width: 15%;">FilePath</th>
                                        <th class="center-align black-text" style="width: 20%;">Initiated By</th>
                                        <th class="center-align black-text" style="width: 20%;">Initiated On</th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnClosingRemarks" runat="server" ClientIDMode="Static" Value='<%#Eval("Remark") %>' />
                                <asp:HiddenField ID="hdnQuatationNo" runat="server" ClientIDMode="Static" Value='<%#Eval("QuatationID") %>' />
                                <asp:HiddenField ID="hdnpkid" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                <%--<asp:HiddenField ID="hdnFollowUpNo" runat="server" ClientIDMode="Static" Value='<%#Eval("FollowUpID") %>' />--%>
                                <tr>
                                    <td class="center-align " style="color:black;"><%#Eval("RowNum") %></td>
                                    <%--<td class="left-align " style="color:black;"><%#Eval("FollowUpID") %></td>--%>
                                    <td class="left-align " style="color:maroon;"><%#Eval("Remark") %></td>
                                    <td class="left-align " style="color:maroon;"><%#Eval("FileName") %></td>
                                    <td class="center-align" style="color:black;"><%#Eval("FromEmployeeName") %></td>
                                    <td class="center-align" style="color:black;"><%#Eval("CreatedDate", "{0: dd/MM/yyyy HH:mm:ss}") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        </table>
                    </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom hide" style="margin-top: 5px;">
                        <button id="btnReset" type="button" runat="server" clientidmode="Static" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="17"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button id="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="18"><i class="material-icons left">save</i>Save</button>
                    </div>
                </div>
               
            </div>
        </div>
    </div>
    </form>
</body>
</html>
