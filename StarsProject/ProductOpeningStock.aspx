<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ProductOpeningStock.aspx.cs" Inherits="StarsProject.ProductOpeningStock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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

    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>

    <style type="text/css">
        .jqte { margin-top: 5px;}
        .jqte_editor, .jqte_source {
            min-height: 300px !important;
        }
        .required:after {
            content: " *";
            color: #f00;
        } 
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            $('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            $('.timepicker').timepicker();

            $(".content").richText();
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
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
        <asp:HiddenField ID="hdnTemplateID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnOldUserID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnEditor" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnImageAttachment1" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnImageAttachment2" runat="server" ClientIDMode="Static" />

        <div id="contentwrapper" class="contentwrapper">
            <div id="basicform" class="subcontent">
                <div class="row">
                    <div class="col m4">
                        <div class="card card-body padding-2">
                            <div class="row">
                                <div class="input-field col m12">
                                    <label for="txtTemplateID" class="active required">Product Name</label>
                                    <asp:TextBox ID="txtProductName" runat="server" class="form-control" MaxLength="20" TabIndex="1" placeholder=""></asp:TextBox>
                                </div>                                    
                                <div class="input-field col m12">
                                    <label class="active required">Product Category</label>
                                    <asp:TextBox ID="txtProductGroup" runat="server" class="form-control" MaxLength="20" TabIndex="1" placeholder=""></asp:TextBox>
                                </div>
                                <div class="input-field col m12">
                                    <label for="txtDescription" class="active required">Product Brand</label>
                                    <asp:TextBox ID="txtBrand" runat="server" class="form-control" MaxLength="50" TabIndex="3" placeholder=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                            <div class="col m8">
                                <div class="card small" style="height:350px;">
                                     <div class="card-title blue white-text" style="padding:10px;">
                                        <p class="no-margin">Opening Stock</p>
                                         <a id="lnkFilter" class="material-icons background-round gradient-45deg-purple-amber gradient-shadow white-text float-right tooltipped btn-move-up accent-2 z-depth-4 right" data-position="center" data-tooltip="Add New Contact"  style="padding:5px; top:-30px; right:-5px;"><i class="material-icons activator">add</i></a>
                                    </div>
                                    <div class="card-content row" style="height: 280px; width: 100%; padding: 0px; margin: 0px;overflow-y:auto;">
                                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <asp:Repeater ID="rptOpeningStk" runat="server" ClientIDMode="Static" OnItemDataBound="rptOpeningStk_ItemDataBound" OnItemCommand="rptOpeningStk_ItemCommand">
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th>Quantity</th>
                                                        <th>Rate</th>
                                                        <th>Value</th>
                                                        <th class="text-center" colspan="2">Action</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnpkIDOpening" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                <asp:HiddenField ID="hdnProductId" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductId") %>' />
                                                <tr class="blueShed">
                                                    <td>
                                                        <asp:TextBox ID="txtQuantity0" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' TabIndex="26" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtRate0" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("Rate") %>' TabIndex="27" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtValue0" runat="server" CssClass="form-control" ClientIDMode="Static" Text='<%# Eval("Value") %>' TabIndex="28" /></td>
                                                    <td class="text-center">
                                                        <asp:ImageButton ID="ImgbtnUpdate" runat="server" ImageUrl="~/images/SaveData.png" CommandName="Update" CommandArgument='<%# Eval("pkID") %>' Width="35" Height="20" TabIndex="30" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ImageUrl="~/images/delete.png" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="31" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>

                                        </asp:Repeater>
                                        </table>
                                    </div>
                                    <div class="card-reveal gradient-45deg-light-blue-teal gradient-shadow black-text">
                                        <span class="card-title grey-text text-darken-4">Product Opening Stock<i class="material-icons right">close</i></span>
                                        <div id="profile-card1" class="card animate fadeRight mb-0">
                                            <div class="card-content p-0 col m12">
                                                <div class="row">
                                                    <div class="input-field col m4">
                                                        <asp:TextBox ID="txtQuantity1" runat="server" class="form-control" ClientIDMode="Static" placeholder=" " TabIndex="26" MaxLength="50" />
                                                        <label class="active" for="txtQuantity1">Quantity</label>
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <asp:TextBox ID="txtRate1" runat="server" class="form-control" ClientIDMode="Static" placeholder=" " TabIndex="26" MaxLength="50" />
                                                        <label class="active" for="txtRate1">Rate</label>
                                                    </div>
                                                    <div class="input-field col m4">
                                                        <asp:TextBox ID="txtContactNumber1" runat="server" class="form-control" ClientIDMode="Static" placeholder=" "  TabIndex="28" MaxLength="15" />
                                                        <label class="active" for="txtContactNumber1">Value</label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="input-field col m12">
                                                        <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="input-field col m12">
                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add.png" data-tooltip="Add Contacts" Width="110" Height="30" TabIndex="30" OnClick="imgBtnSave_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                </div>
                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                    <button ID="btnSave" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="8"><i class="material-icons left">save</i>Save</button>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

