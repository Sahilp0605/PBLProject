<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesOrderLog.aspx.cs" Inherits="StarsProject.SalesOrderLog" %>

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

    <script type="text/javascript">
        function ShowPDFfile(repFilename) {
            repFilename = "OrderDocs/" + repFilename;
            yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }
    </script>
</head>
<body>
    <form id="frmSalesOrderLog" runat="server">
        <asp:HiddenField ID="hdnHeaderID" runat="server" ClientIDMode="Static" />
        <div class="row">
            <div class="col m12">
                <table id="tblSalesOrderLog" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                    <asp:Repeater ID="rptSalesOrderLog" runat="server" ClientIDMode="Static" OnItemDataBound="rptSalesOrderLog_ItemDataBound">
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
                            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                            <asp:HiddenField ID="hdnClosingRemarks" runat="server" ClientIDMode="Static" Value='<%#Eval("ClosingRemarks") %>' />
                            <tr>
                                <td class="center-align " style="color:black;"><%#Eval("RowNum") %></td>
                                <td class="left-align " style="color:black;"><%#Eval("ActionTaken") %></td>
                                <td class="left-align " style="color:maroon;"><%#Eval("TaskDescription") %></td>
                                <td class="center-align" style="color:black;"><%#Eval("FromEmployeeName") %></td>
                                <td class="center-align" style="color:black;"><%#Eval("CreatedDate", "{0: dd/MM/yyyy HH:mm:ss}") %></td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Repeater ID="rptOrderDocs" runat="server" ClientIDMode="Static">
                                    <ItemTemplate>
                                        <div class="col xl4 l6 m4 s6">
                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                <div class="card-content" style="padding: 10px;border-radius: 4px;border: 1px solid silver; background-color: beige;">
                                                <div class="app-file-content-logo grey lighten-4">
                                                    <div class="fonticon p-2" style="background-color: beige;">
                                                        <div style="text-align:left; width:100%;">
                                                            <asp:LinkButton CssClass="float-left" ID="ImgBtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'><i class="material-icons delete">delete_outline</i></asp:LinkButton>
                                                            <a class="float-right cursor-pointer" id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("AttachmentFile").ToString() %>');"><i class="material-icons">find_in_page</i>
                                                            </a>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="app-file-details">
                                                    <div class="app-file-name font-weight-700">
                                                        <p style="display: block;width: 250px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("AttachmentFile").ToString().Substring(6) %></p>
                                                    </div>
                                                </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
