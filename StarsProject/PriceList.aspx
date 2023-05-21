<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PriceList.aspx.cs" Inherits="StarsProject.PriceList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>


    <style type="text/css">
        .required:after {
            content: " *";
            color: #f00;
        }
    </style>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            $('.timepicker').timepicker();
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
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtCategoryName").val() == "") {

                if (jQuery("#txtCategoryName").val() == "")
                    errmsg += "<li>Category Name required</li>";


                validflag = false;
            }
            // ----------------------------------------
            if (validflag == false) {
                showcaseError(errmsg);
            }
            return validflag;
        }
        function bindDDLProductTo(selector) {
            if ($(selector).val().length >= 1) {
                jQuery.ajax({
                    type: "POST",
                    url: "InquiryInfo.aspx/FilterProduct",
                    data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypePriceList\'}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
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
                                return '<div class="autocomplete-suggestion" data-langname="' + item.pkID + '" data-lang="' + item.ProductNameLong + '" data-val="' + search + '">' + item.ProductNameLong + '</div>';
                            },
                            onSelect: function (e, term, item) {
                                //console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                $(selector).val(item.html());
                                $("#hdnProductID").val(item.data('langname'));
                                $("#txtReferenceName").focus();
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
                    $("#hdnProductID").val('');
                }
            }
        }

    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">

        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper" style="padding:20px 15px;">

            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />

                    <%-- Bootstrap Modal Popup --%>
                    <div class="clearall"></div>
                    <div id="myModal" style="display: block; width: 100%;">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="input-field col m3">
                                        <asp:TextBox ID="txtPriceListName" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " MaxLength="50" TabIndex="1"></asp:TextBox>
                                        <label class="active" for="txtPriceListName">Price List Name <span class="materialize-red-text font-weight-800">*</span></label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div id="divPriceList" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">  
                                        <table id="tblCustPriceList" class="table responsive-table striped width-100">        
                                            <asp:Repeater ID="rptCustPriceList" runat="server" ClientIDMode="Static" OnItemCommand="rptCustPriceList_ItemCommand">
                                                <HeaderTemplate>
                                                    <thead>
                                                        <tr>
                                                            <th class="center-align width-50">Product Name</th>
                                                            <th class="right-align width-20">Rate</th>
                                                            <th class="right-align width-20">Discount</th>
                                                            <th class="center-align width-10">Action</th>
                                                        </tr>
                                                    </thead>
                                                    </HeaderTemplate>
                                                <ItemTemplate>    
                                                    <tr>
                                                        <td class="left-align width-50">
                                                            <%# Eval("ProductName") %>
                                                            <asp:HiddenField ID="edProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                            <asp:Label ID="edProductName" runat="server" ClientIDMode="Static" CssClass="form-control text-right" Text='<%# Eval("ProductName") %>' TabIndex="7" />
                                                        </td>
                                                        <td class="right-align width-20">
                                                            <asp:Label ID="edUnitRate" runat="server" ClientIDMode="Static" CssClass="form-control text-right" Text='<%# Eval("UnitPrice") %>' TabIndex="8" />
                                                        </td>
                                                        <td class="right-align width-20">
                                                            <asp:Label ID="edDiscount" runat="server" ClientIDMode="Static" CssClass="form-control text-right" Text='<%# Eval("Discount") %>' TabIndex="9" />
                                                        </td>
                                                        <td class="center-align width-10">
                                                            <asp:ImageButton ID="ImgbtnEdit11" runat="server" ClientIDMode="Static" ImageUrl="~/images/savedata.png" ToolTip="Update" CommandName="updprice" CommandArgument='<%# Eval("pkID") %>' Width="40" Height="20" TabIndex="6" Visible="false" />
                                                            <asp:ImageButton ID="ImgbtnDelete11" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="delprice" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <tr>
                                                        <td class="left-align" style="width: 25%;">
                                                            <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                            <asp:TextBox ID="txtProductName"  runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" Width="100%" TabIndex="2" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged"/>
                                                        </td>
                                                        <td class="right-align" style="width: 8%;">
                                                            <asp:TextBox ID="txtUnitRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="3" />
                                                        </td>
                                                        <td class="right-align">
                                                            <asp:TextBox ID="txtDiscount" runat="server" ClientIDMode="Static" Width="50" TabIndex="4"/>
                                                        </td>
                                                        <td class="center-align width-40">
                                                            <asp:ImageButton ID="ImgbtnAddPrice" runat="server" ClientIDMode="Static" ImageUrl="~/images/expand.png" ToolTip="Add Price" CommandName="addprice" CommandArgument='<%# Eval("pkID") %>' Width="30" Height="30" TabIndex="5"/>
                                                        </td>
                                                    </tr>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                        <button ID="btnReset" type="button" runat="server" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="36"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSave" type="button" runat="server" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="38"><i class="material-icons left">save</i>Save</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
