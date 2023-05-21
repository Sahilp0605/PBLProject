<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="QuotationSpecification.aspx.cs" Inherits="StarsProject.QuotationSpecification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="app-assets/js/plugins.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <script type="text/javascript" src="app-assets/vendors/select2/select2.full.min.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <link href="css/richtext.min.css" rel="stylesheet" />
    <script src="js/jquery.richtext.min.js"></script>

    <style type="text/css">
        .jqte_editor {
            height: 250px !important;
        }

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

        .autocomplete-suggestions {
            min-width: 600px !important;
        }

        .lblOther {
            font-size: 14px;
        }

        .drpOther {
            width: 100%;
            font-size: 13px;
        }

        .inputOther {
            width: 100px;
            font-size: 13px;
            text-align: right;
            height: 30px;
        }

        .inputCalc {
            width: 140px;
            text-align: right;
            height: 27px;
        }

        .active {
            font-size: 15px;
        }

        .radio-inline label {
            color: white !important;
            margin-left: 5px !important;
            margin-right: 20px !important;
        }

        /* ------------------------------------------------------- */
        ul {
            margin: 0;
            padding: 0;
            list-style: none;
        }

        .heading.heading-icon {
            display: block;
        }

        .padding-lg {
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
        }

        .practice-area.padding-lg {
            padding-bottom: 55px;
            padding-top: 55px;
        }

        .practice-area .inner {
            border: 1px solid #999999;
            text-align: center;
            margin-bottom: 28px;
            padding: 40px 25px;
        }

        .our-webcoderskull .cnt-block:hover {
            box-shadow: 0px 0px 10px rgba(0,0,0,0.3);
            border: 0;
        }

        .practice-area .inner h3 {
            color: #3c3c3c;
            font-size: 24px;
            font-weight: 500;
            font-family: 'Poppins', sans-serif;
            padding: 10px 0;
        }

        .practice-area .inner p {
            font-size: 14px;
            line-height: 22px;
            font-weight: 400;
        }

        .practice-area .inner img {
            display: inline-block;
        }

        .our-webcoderskull {
            /*background: url("http://www.webcoderskull.com/img/right-sider-banner.png") no-repeat center top / cover;*/
            background-color: #fbf5f5;
        }

            .our-webcoderskull .cnt-block {
                float: left;
                width: 100%;
                background: #fff;
                padding: 30px 20px;
                text-align: center;
                border: 2px solid #d5d5d5;
                margin: 0 0 28px;
            }

                .our-webcoderskull .cnt-block figure {
                    width: 80px;
                    height: 80px;
                    border-radius: 100%;
                    display: inline-block;
                    margin-bottom: 15px;
                }

                .our-webcoderskull .cnt-block img {
                    width: 80px;
                    height: 80px;
                    border-radius: 100%;
                }

                .our-webcoderskull .cnt-block h3 {
                    color: #2a2a2a;
                    font-size: 20px;
                    font-weight: 500;
                    padding: 6px 0;
                    text-transform: uppercase;
                }

                    .our-webcoderskull .cnt-block h3 a {
                        text-decoration: none;
                        color: #2a2a2a;
                    }

                        .our-webcoderskull .cnt-block h3 a:hover {
                            color: #337ab7;
                        }

                .our-webcoderskull .cnt-block p {
                    color: #2a2a2a;
                    font-size: 13px;
                    line-height: 20px;
                    font-weight: 400;
                }

                .our-webcoderskull .cnt-block .follow-us {
                    margin: 20px 0 0;
                }

                    .our-webcoderskull .cnt-block .follow-us li {
                        display: inline-block;
                        width: auto;
                        margin: 0 5px;
                    }

                        .our-webcoderskull .cnt-block .follow-us li .fa {
                            font-size: 24px;
                            color: #767676;
                        }

                            .our-webcoderskull .cnt-block .follow-us li .fa:hover {
                                color: #025a8e;
                            }

        .nav-tabs .nav-item.show .nav-link, .nav-tabs .nav-link.active {
            background-color: #6cd883;
            color: Navy;
        }

        /*#myTabContent {
            border-left: 1px solid navy;
            border-right: 1px solid navy;
            border-bottom: 1px solid navy;
        }*/
    </style>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('tabs').tabs();
            // --------------------------------------------------------
            $(function () {
                $(".tabs a").click(function () {
                    $("#hdnCurrTabSpec").val($(this).attr("href").replace("#", ""));
                    retainTabPosition();
                });
            });
        }
        // --------------------------------------------------------
        function retainTabPosition() {
            var currTab = $("#hdnCurrTabSpec").val();

            $("#lnkRemark").removeClass("active");
            $("#lnkSpec").removeClass("active");
            $("#lnkPart").removeClass("active");

            $("#pnl_Remark").css("display", "none");
            $("#pnl_Spec").css("display", "none");
            $("#pnl_Part").css("display", "none");

            if (currTab == 'pnl_Remark') {
                $("#lnkRemark").addClass("active");
                $("#pnl_Remark").css("display", "block");
            }
            else if (currTab == 'pnl_Spec') {
                $("#lnkSpec").addClass("active");
                $("#pnl_Spec").css("display", "block");
            }
            else if (currTab == 'pnl_Part') {
                $("#lnkPart").addClass("active");
                $("#pnl_Part").css("display", "block");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="srcQuotSpec" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnQuotationNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnFinishProductID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnQuatSpecRemark" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnQuotSpecFormat" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRefNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnQuotationProductParts" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCurrTabSpec" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Remark" />

            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div id="myModal" style="display: block; width: 100%;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row mt-2">
                            <div class="input-field col m12">
                                <div class="row">
                                    <div class="col m12">
                                        <ul id="myTab" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color: antiquewhite;">
                                            <li class="tab col m2 p-0"><a id="lnkRemark" class="left active" href="#pnl_Remark">Product Remarks</a></li>
                                            <li class="tab col m2 p-0"><a id="lnkSpec" class="left" href="#pnl_Spec">Product Specification</a></li>
                                            <% if (hdnQuotationProductParts.Value.ToLower() == "yes")
                                                { %>
                                            <li class="tab col m2 p-0" id="liProductPart" runat="server" clientidmode="static"><a id="lnkPart" class="left" href="#pnl_Part">Product Parts Description</a></li>
                                            <% } %> 
                                        </ul>
                                    </div>
                                </div>
                                <div id="pnl_Remark" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true">
                                    <section class="our-webcoderskull padding-lg" style="height: 320px; overflow-y: auto;">
                                        <div class="container" style="max-width: none; width: 98%; margin: 5px !important;">
                                            <div id="divRemarks" runat="server" clientidmode="Static" class="input-field col m">
                                                <asp:TextBox ID="txtRemarks" runat="server" class="content" ClientIDMode="Static" TabIndex="17" TextMode="MultiLine" Rows="7" MaxLength="500" Style="height: 7rem !important; width:800px;" />
                                            </div>
                                        </div>
                                    </section>
                                </div>
                                <div id="pnl_Spec" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true">
                                    <section class="our-webcoderskull padding-lg" style="height: 320px; overflow-y: auto;" id="prodsec1">
                                        <div class="container" style="max-width: none; width: 98%; margin: 20px !important;">
                                            <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <asp:Repeater ID="rptProductSpecs" runat="server" ClientIDMode="Static" OnItemDataBound="rptProductSpecs_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <thead>
                                                            <tr>
                                                                <th class="text-center" style="width: 15px">Order.#</th>
                                                                <th class="text-center">Group Description</th>
                                                                <th class="text-center">Head</th>
                                                                <th class="text-center">Specification</th>
                                                                <th class="text-center">Material Remarks</th>
                                                            </tr>
                                                        </thead>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnQuotNo" runat="server" ClientIDMode="Static" Value='<%# Eval("QuotationNo") %>' />
                                                        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" Value='<%# Eval("FinishProductID") %>' />
                                                        <tr class="blueShed">
                                                            <td class="text-center">
                                                                <asp:TextBox class="form-control text-left" Style="width: 50px" ID="newOrder" runat="server" ClientIDMode="Static" TabIndex="14" Text='<%# Eval("itemOrder") %>' />
                                                            </td>
                                                            <td class="text-center">
                                                                <asp:TextBox class="form-control text-left" ID="newGroupHead" runat="server" ClientIDMode="Static" TabIndex="7" Text='<%# Eval("GroupHead") %>' />
                                                            </td>
                                                            <td class="text-center">
                                                                <asp:TextBox class="form-control text-left" ID="newMaterialHead" runat="server" ClientIDMode="Static" TabIndex="7" Text='<%# Eval("MaterialHead") %>' />
                                                            </td>
                                                            <td class="text-center">
                                                                <asp:TextBox class="form-control text-left" ID="newMaterialSpec" runat="server" ClientIDMode="Static" TabIndex="7" Text='<%# Eval("MaterialSpec") %>' />
                                                            </td>
                                                            <td class="text-center">
                                                                <asp:TextBox class="form-control text-left" ID="newMaterialRemarks" runat="server" ClientIDMode="Static" TabIndex="7" Text='<%# Eval("MaterialRemarks") %>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </section>
                                </div>
                                
                                <% if (hdnQuotationProductParts.Value.ToLower() == "yes")
                                { %>
                                <div id="pnl_Part" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true">
                                    <section class="our-webcoderskull padding-lg" style="height: 320px; overflow-y: auto;" id="prodpart1">
                                        <div class="container" style="max-width: none; width: 98%; margin: 20px !important;">
                                            <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <asp:Repeater ID="rptProdPart" runat="server" ClientIDMode="Static" OnItemDataBound="rptProdPart_ItemDataBound" OnItemCommand="rptProdPart_ItemCommand">
                                                    <HeaderTemplate>
                                                        <thead>
                                                            <tr>
                                                                <th class="text-center" style="width: 15px">Order.#</th>
                                                                <th class="text-center">Part Description</th>
                                                                <th class="text-center">Brand Name</th>
                                                                <th class="center">Action</th>
                                                            </tr>
                                                        </thead>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnOrder" runat="server" ClientIDMode="Static" Value='<%# Eval("itemOrder") %>' />
                                                        <asp:HiddenField ID="hdnDescription" runat="server" ClientIDMode="Static" Value='<%# Eval("PartDescription") %>'/>
                                                        <asp:HiddenField ID="hdnBrand" runat="server" ClientIDMode="Static" Value='<%# Eval("BrandName") %>' />

                                                        <tr class="blueShed">
                                                            <td class="text-center">
                                                                <asp:TextBox class="form-control text-left" Style="width: 50px" ID="newOrder" runat="server" ClientIDMode="Static" TabIndex="14" Text='<%# Eval("itemOrder") %>' AutoPostBack="true" OnTextChanged="newOrder_TextChanged" />
                                                            </td>
                                                            <td class="text-center">
                                                                <asp:TextBox class="form-control text-left" ID="txtPartDescription" runat="server" ClientIDMode="Static" TabIndex="7" Text='<%# Eval("PartDescription") %>' AutoPostBack="true" OnTextChanged="txtPartDescription_TextChanged" />
                                                            </td>
                                                            <td class="text-center">
                                                                <asp:TextBox class="form-control text-left" ID="txtBrandName" runat="server" ClientIDMode="Static" TabIndex="7" Text='<%# Eval("BrandName") %>' AutoPostBack="true" OnTextChanged="txtBrandName_TextChanged" />
                                                            </td>
                                                            <td class="center">
                                                                <asp:ImageButton ID="imgBtnDelDetail" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="DeleteDetail" CommandArgument='<%# Eval("PartDescription") %>' Width="20" Height="20" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr class="blueShed">
                                                    <td class="text-center">
                                                        <asp:TextBox class="form-control text-left" Style="width: 50px" ID="newOrder1" runat="server" ClientIDMode="Static" TabIndex="14" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:TextBox class="form-control text-left" ID="txtPartDescription1" runat="server" ClientIDMode="Static" TabIndex="7" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:TextBox class="form-control text-left" ID="txtBrandName1" runat="server" ClientIDMode="Static" TabIndex="7" />
                                                    </td>
                                                    <td class="center">
                                                        <asp:ImageButton ID="imgBtnSaveDetail" runat="server" ClientIDMode="Static" ImageUrl="~/images/Buttons/bt-add.png" Width="70" Height="30" OnClick="imgBtnSaveDetail_Click" TabIndex="43" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </section>
                                </div>
                                <% } %>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="input-field col m12">
                            <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="input-field col m12">
                            <center>
                                <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/SaveData.png" ToolTip="Save Specification" Width="100" Height="35" TabIndex="104" OnClick="btnSave_Click" />
                            </center>
                        </div>
                    </div>
                </div>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
    </form>

</body>
</html>
