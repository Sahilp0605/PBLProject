<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutwardAssembly.aspx.cs" Inherits="StarsProject.OutwardAssembly" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <script type="text/javascript">
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }
    </script>
</head>
<body>
    <form id="frmOutwardAssembly" runat="server" autocomplete="off">

        <asp:HiddenField ID="hdnOutwardNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />

        <div id="myModal" style="display: block; width: 100%;">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <div class="col m12">
                            <table id="tblOutwardDetail" class="stdtable" cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                <asp:Repeater ID="rptOutwardDetailAssembly" runat="server" ClientIDMode="Static" OnItemCommand="rptOutwardDetailAssembly_ItemCommand">
                                    <HeaderTemplate>
                                        <thead>
                                            <tr>
                                                <th>Product Name <span class="materialize-red-text font-weight-800">*</span></th>
                                                <th class="text-right">Quantity <span class="materialize-red-text font-weight-800">*</span></th>
                                                <th class="center-align">Unit<span class="materialize-red-text font-weight-800">*</span></th>
                                                <th class="center-align">Weight</th>
                                                <th class="center-align">Serial #</th>
                                                <th class="center-align">Box #</th>
                                                <th class="center-align">Action</th>
                                            </tr>
                                        </thead>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <asp:HiddenField ID="edpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                            <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                            <asp:HiddenField ID="edAssemblyID" runat="server" ClientIDMode="Static" Value='<%# Eval("AssemblyID") %>' />
                                            <td class="text-left">
                                                <%# Eval("AssemblyName") %>
                                                <asp:HiddenField ID="edAssemblyName" runat="server" ClientIDMode="Static" Value='<%# Eval("AssemblyName") %>' />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="edQuantity" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' Style="width: 80px; text-align: right;" />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="edUnit" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("Unit") %>' Style="width: 80px; text-align: right;" />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="edQuantityWeight" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("QuantityWeight") %>' Style="width: 120px; text-align: right;" />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="edSerialNo" MaxLength="30" runat="server" ClientIDMode="Static" Text='<%# Eval("SerialNo") %>' Style="width: 200px; text-align: right;"  />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="edBoxNo" MaxLength="10" runat="server" ClientIDMode="Static" Text='<%# Eval("BoxNo") %>' Style="width: 200px; text-align: right;"  />
                                            </td>
                                            <td class="center-align">
                                                <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
<%--                                    <FooterTemplate>
                                        <tr style="background-color: #dde8f3;">
                                            <td>
                                                <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="21" Width="100%" />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" CssClass="form-control" Width="80" TabIndex="52" />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="80" TabIndex="50"/>
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="txtQuantityWeight" runat="server" ClientIDMode="Static" Width="120" TabIndex="54" />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="txtSerialNo" runat="server" ClientIDMode="Static" Width="200" TabIndex="55" />
                                            </td>
                                            <td class="center-align">
                                                <asp:TextBox ID="txtBoxNo" runat="server" ClientIDMode="Static" Width="200" TabIndex="56" />
                                            </td>
                                            <td class="center-align">
                                                <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/Buttons/bt-Add.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="150" Height="40" TabIndex="61" />
                                            </td>
                                        </tr>
                                    </FooterTemplate>--%>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                        <button id="btnSaveAssembly" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSaveAssembly_Click" tabindex="63"><i class="material-icons left">save</i>Save</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
