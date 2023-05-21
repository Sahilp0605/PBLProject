<%@ Page Title="" Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Products.aspx.cs" Inherits="StarsProject.Products" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link  rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>

    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="app-assets/js/scripts/app-file-manager.js"></script>
    <link href="css/font-awesome-Editor.min.css" rel="stylesheet" />

    <link href="css/richtext.min.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.richtext.min.js"></script>

    <style type="text/css">
        .MsoTableGrid {
            margin-left: 0px !important;
        }
        .tabs .tab a { padding: 0 8px !important; }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            $('.timepicker').timepicker();
            $('.tabs').tabs();
            $('.collapsible').collapsible({ accordion: true });
            $(".content").richText();

            setHiddenControls();
        });
        function reinitializePage() {
            $('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            $('.timepicker').timepicker();
            $('.tabs').tabs();
            $('.collapsible').collapsible({ accordion: true });
            $(".content").richText();
        }
        /*---------------------------------------------------------*/
        function pageLoad(sender, args) {
            retainTabPosition();
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

        function ShowPDFfile(repFilename) {
            yhooWin = window.open('productimages//' + repFilename, "ywin", "width=1050,height=750");
            yhooWin.focus();
        }
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtProductName").val() == "" || jQuery("#txtProductAlias").val() == "") {
                validflag = false;

                if (jQuery("#txtProductName").val() == "")
                    errmsg += "<li>Product Name is required</li>";

                if (jQuery("#txtProductAlias").val() == "")
                    errmsg += "<li>Product Alias is required</li>";
            }
            // ----------------------------------------
            if (validflag == false)
                showcaseError(errmsg);
            // ----------------------------------------
            return validflag;
        }

        function clearFormData() {
            //$(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnpkID").val('');
            jQuery("#FileUpload1").val("");
            jQuery("#imgProduct").attr("src", "~/images/no-figure.png");
            jQuery("#divImageControl").hide();
            
        }

        jQuery(".form-control").focus(function () {
            alert("Handler for .focus() called.");
        });

        function showFileExtError(xMode) {
            if (xMode == 'image')
                alert("Only BMP, JPEG, GIF and PNG file is valid for Attachment !");
            if (xMode == 'pdf')
                alert("Only PDF file attachment is valid for Upload !");
        }

        function showPDFContent() {
            alert('test');
        }

        $("li .nav-item").click(function () {
            alert('testeeeee');
        });

        /*---------------------------------------------------------*/
        /* Below section to Maintain Current TAB                   */
        /*---------------------------------------------------------*/
        $(function () {
            var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "prodspecs";
            $("#myTab a").click(function () {
                var prevTab = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "prodspecs";
                var currTab = $(this).attr("href").replace("#", "");
                $('#' + prevTab + '-tab').removeClass('show');
                $('#' + currTab + '-tab').addClass('show');
                $("#hdnCurrentTab").val(currTab);
                //alert("Curr.Tab Gen.Func : " + currTab)
            });
        });

        $(function () {
            $(".tabs a").click(function () {
                $("#hdnCurrTab").val($(this).attr("href").replace("#", ""));
                retainTabPosition();
            });
        });

        function retainTabPosition() {
            //alert($("#hdnCurrentTab").val());
            $('#lnkprodspecs').removeClass('active');
            $('#lnkshortdesc').removeClass('active');
            $('#lnkproddetail').removeClass('active');
            $('#lnkproddocs').removeClass('active');
            $('#lnkprodimg').removeClass('active');
            $('#lnkprodacc').removeClass('active');

            $('#prodspecs').css("display", "none");
            $('#shortdesc').css("display", "none");
            $('#proddetail').css("display", "none");
            $('#proddocs').css("display", "none");
            $('#prodimg').css("display", "none");
            $('#prodacc').css("display", "none");
            // --------------------------------------------------- 
            if ($("#hdnCurrentTab").val() == 'prodspecs') {
                //alert('1')
                $('#lnkprodspecs').addClass('active');
                $('#prodspecs').css("display", "block");
            }
            else if ($("#hdnCurrentTab").val() == 'shortdesc') {
                //alert('2')
                $('#lnkshortdesc').addClass('active');
                $('#shortdesc').css("display", "block");
            }
            else if ($("#hdnCurrentTab").val() == 'proddetail') {
                //alert('3')
                $('#lnkproddetail').addClass('active');
                $('#proddetail').css("display", "block");
            }
            else if ($("#hdnCurrentTab").val() == 'proddocs') {
                //alert('4')
                $('#lnkproddocs').addClass('active');
                $('#proddocs').css("display", "block");
            }
            else if ($("#hdnCurrentTab").val() == 'prodimg') {
                //alert('5')
                $('#lnkprodimg').addClass('active');
                $('#prodimg').css("display", "block");
            }
            else if ($("#hdnCurrentTab").val() == 'prodacc') {
                //alert('5')
                $('#lnkprodacc').addClass('active');
                $('#prodacc').css("display", "block");
            }

        }
        function setHiddenControls() {
            var userData = JSON.stringify({ "pPageName": 'product' });
            $.ajax({
                type: "POST",
                url: "/Services/NagrikService.asmx/GetPageHiddenControls",
                data: userData,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    $("#hdnHiddenControl").val(data.d);
                    // ---------------------------------------------------------------
                    $('#frmQuotation *').filter(':input').each(function () {
                        var idName = $(this).attr('id') + ',';
                        if ($("#hdnHiddenControl").val().indexOf(idName) >= 0) {
                            $(this).parent().hide();
                        }
                    });
                },
                failure: function (errMsg) { alert(errMsg); }
            });
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmProduct" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server" EnablePartialRendering="true" EnablePageMethods="true" EnableViewState="true"></asp:ScriptManager>

        <asp:HiddenField ID="hdnPageMode" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <asp:HiddenField ID="hdnClientERPIntegration" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" Value="prodspecs" EnableViewState="true" />
        <asp:HiddenField ID="hdnQuatSpecRemark" runat="server" ClientIDMode="Static" EnableViewState="true" />
        <asp:HiddenField ID="hdnQuotSpecFormat" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnApplicationIndustry" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSerialKey" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnHiddenControl" runat="server" ClientIDMode="Static" Value=""/>
    <div id="contentwrapper" class="contentwrapper">
        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="widgetbox">
            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
            <div id="myModal" style="display:block; width:100%;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="input-field col m5">
                                <div class="row">
                                     <% if (hdnClientERPIntegration.Value.ToLower() == "yes")
                                     { %>
                                        <div style="padding: 5px 15px; background-color: white; color: Blue; border-radius: 15px; font-size: 18px;">
                                             Closing Stock :&nbsp;&nbsp;<asp:Label CssClass="float-right" ID="lblClosingStock" runat="server" ClientIDMode="Static" />
                                        </div>
                                     <% } %>
                                </div>
                                <div class="row">
                                    <div class="input-field col m9">
                                        <asp:TextBox ID="txtProductName" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="1" MaxLength="100" />
                                        <label class="active" for="txtProductName">Product Name <span class="materialize-red-text font-weight-800">*</span></label>
                                    </div>
                                    <div class="input-field col m3">
                                        <label class="active" for="chkBlock">Active Status</label>
                                        <div class = "switch">
                                            <label>Inactive
                                            <asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" Checked="true" TabIndex="2"/>
                                            <span class="lever"></span>Active</label>
                                        </div> 
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtProductAlias" runat="server" MaxLength="50" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="3" />                                
                                        <label class="active" for="txtProductAlias">Product Alias <span class="materialize-red-text font-weight-800">*</span></label>
                                    </div>
                                    <div class="input-field col m4">
                                        <asp:DropDownList ID="drpBrand" runat="server" class="select2-theme browser-default" TabIndex="4" />
                                        <label class="active" for="drpBrand">Product Brand</label>
                                    </div>
                                    <div class="input-field col m4">
                                        <asp:DropDownList ID="drpProductType" runat="server" class="select2-theme browser-default" TabIndex="5">
                                            <asp:ListItem Text="General" Value="General" />
                                            <asp:ListItem Text="Finished" Value="Finished" />
                                            <asp:ListItem Text="Semi-Finished" Value="Semi-Finished" />
                                            <asp:ListItem Text="Raw Material" Value="Raw Material" />
                                        </asp:DropDownList>
                                        <label class="active" for="drpProductType">Type Of Product</label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m4">
                                        <asp:DropDownList ID="drpProductGroup" runat="server" class="select2-theme browser-default" TabIndex="6" />
                                        <label class="active" for="drpProductGroup">Product Category</label>
                                    </div>    
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtHSNCode" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="7" MaxLength="50" />
                                        <label class="active" for="txtHSNCode">HSN Code</label>
                                    </div>
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtUnit" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="8"  MaxLength="5"/>
                                        <label class="active" for="txtUnit">Product Unit</label>
                                    </div>
                                    <% if (hdnSerialKey.Value == "STX1-UP06-YU89-JK23")
                                        {%>
                                    <div class="input-field col m4">
                                        <label class="active" for="txtLRDate">Revised Date</label>
                                        <asp:TextBox ID="txtLRDate" runat="server" placeholder="" class="form-control" ClientIDMode="Static"  TextMode="Date" />
                                    </div>
                                    <%} %>
                                </div>
                                <div class="row">
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtUnitQuantity" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="9"  MaxLength="5"/>
                                        <label class="active" for="txtUnitQuantity">Unit Quantity</label>
                                    </div>
                                    <div id="dvCeraSize" runat="server" clientidmode="Static" class="input-field col m4">
                                        <asp:TextBox ID="txtUnitSize" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="10"  MaxLength="30"/>
                                        <label class="active" for="txtUnitSize">Unit Size</label>
                                    </div>
                                    <div id="dvCeraSurface" runat="server" clientidmode="Static" class="input-field col m4">
                                        <asp:TextBox ID="txtUnitSurface" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="11"  MaxLength="30"/>
                                        <label class="active" for="txtUnitSurface">Unit Surface</label>
                                    </div>      
                                </div>
                                <div id="dvCeraBox" runat="server" clientidmode="Static" class="row">
                                    <div class="input-field col m3">
                                        <asp:DropDownList ID="drpUnitGrade" runat="server" class="select2-theme browser-default" TabIndex="12">
											<asp:ListItem Text="PRM" Value="PRM" />
											<asp:ListItem Text="STD" Value="STD" />
											<asp:ListItem Text="COM" Value="COM" />
										</asp:DropDownList>
                                        <label class="active" for="txtUnitQuantity">Unit Grade</label>
                                    </div>
                                    <div class="input-field col m3">
                                        <asp:TextBox ID="txtBox_Weight" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="13"  MaxLength="12"/>
                                        <label class="active" for="txtBox_Weight">Wght Per Box</label>
                                    </div>
                                    <div class="input-field col m3">
                                        <asp:TextBox ID="txtBox_SQFT" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="14"  MaxLength="12"/>
                                        <label class="active" for="txtBox_SQFT">Sq.Ft Per Box</label>
                                    </div>
                                    <div class="input-field col m3">
                                        <asp:TextBox ID="txtBox_SQMT" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="15"  MaxLength="12"/>
                                        <label class="active" for="txtBox_SQMT">Sq.Mtr Per Box</label>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtUnitPrice" runat="server" class="form-control" ClientIDMode="Static" Placeholder=" " TabIndex="12" MaxLength="10"/>
                                        <label class="active" for="txtUnitPrice">Product Price <span class="materialize-red-text font-weight-800">*</span></label>
                                    </div>
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtTaxRate" runat="server" class="form-control" ClientIDMode="Static" Placeholder=" " TabIndex="17" MaxLength="10"/>
                                        <label class="active" for="txtTaxRate">GST Tax %</label>
                                    </div>
                                    <div class="input-field col m4">
                                        <asp:DropDownList ID="drpTaxType" runat="server" class="select2-theme browser-default" TabIndex="18">
                                            <asp:ListItem Text="Exclusive" Value="1" />
                                            <asp:ListItem Text="Inclusive" Value="0" />
                                            <asp:ListItem Text="None" Value="2" />
                                        </asp:DropDownList> 
                                        <label class="active" for="drpTaxType">GST Tax</label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m3">
                                        <asp:TextBox ID="txtMinQuantity" runat="server" class="form-control" ClientIDMode="Static" Placeholder=" " TabIndex="19" MaxLength="12" />
                                        <label class="active" for="txtMinQuantity">Min.Stock Qty</label>
                                    </div>
                                    <div class="input-field col m3">
                                        <asp:TextBox ID="txtMinUnitPrice" runat="server" class="form-control" ClientIDMode="Static" Placeholder=" " TabIndex="20" MaxLength="10"/>
                                        <label class="active" for="txtMinUnitPrice">Lower Price</label>
                                    </div>
                                    <div class="input-field col m3">
                                        <asp:TextBox ID="txtMaxUnitPrice" runat="server" class="form-control" ClientIDMode="Static" Placeholder=" " TabIndex="21" MaxLength="10" />
                                        <label class="active" for="txtMaxUnitPrice">Upper Price</label>
                                    </div>
                                    <div class="input-field col m3">
                                        <asp:TextBox ID="txtProfitRatio" runat="server" class="form-control" ClientIDMode="Static" Placeholder=" " TabIndex="22" MaxLength="10" />
                                        <label class="active" for="txtProfitRatio">Profit Ratio (%)</label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtOpeningValuation" runat="server" class="form-control" ClientIDMode="Static" Placeholder=" " TabIndex="19" MaxLength="12" />
                                        <label class="active" for="txtOpeningValuation">Opening Valuation</label>
                                    </div>
                                    <div class="input-field col m4">
                                        <asp:TextBox ID="txtOpeningWeightRate" runat="server" class="form-control" ClientIDMode="Static" Placeholder=" " TabIndex="20" MaxLength="10"/>
                                        <label class="active" for="txtOpeningWeightRate">Opening Weight Rate</label>
                                    </div>
                                </div>
                                
                            </div>

                            <div class="input-field col m7 mt-0">
                                <div class="row" style="box-shadow: navy -6px 8px 6px -6px;">
                                    <div class="card user-card-negative-margin z-depth-0" id="feed" style="margin:0px;">
                                        <div class="card-content card-border-gray" style="height:500px;padding-top: 5px;">
                                            <div class="row">
                                                <ul id="myTab" class="tabs card-border-gray" runat="server" clientidmode="Static" enableviewstate="true">
                                                    <li class="tab col m2.5 p-0"><a id="lnkprodspecs" class="left active" href="#prodspecs" tabindex="23">Specification</a></li>
                                                    <li class="tab col m2.5 p-0"><a id="lnkshortdesc" class="left" href="#shortdesc" tabindex="35">Short Description</a></li>
                                                    <li class="tab col m2 p-0"><a id="lnkproddetail" class="left" href="#proddetail" tabindex="37">Assembly</a></li>
                                                    <li class="tab col m2 p-0"><a id="lnkprodacc" class="left" href="#prodacc" tabindex="41">Accessories</a></li>
                                                    <li id="liBrochure" runat="server" class="tab col m2.5 p-0"><a id="lnkproddocs" class="left" href="#proddocs" tabindex="44" >Brochure</a></li>
                                                    <li id="liProdImages" runat="server" class="tab col m2.5 p-0"><a id="lnkprodimg" class="left" href="#prodimg" tabindex="46">Image Gallery</a></li>
                                                </ul>
                                            </div>
                                            <div id="prodspecs" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;">
                                                <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                    <div id="container1" runat="server" clientidmode="Static" class="container">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="input-field col m12 p-0 m-0">
                                                                    
                                                                    <div id="divTabRemarks" runat="server" clientidmode="Static" class="input-field col m12 p-0  m-0">
                                                                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                            <asp:Repeater ID="rptProductSpecs" runat="server" ClientIDMode="Static" OnItemDataBound="rptProductSpecs_ItemDataBound" OnItemCommand="rptProductSpecs_ItemCommand">
                                                                                <HeaderTemplate>
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th class="center" style="width:15px">Order.#</th>
                                                                                            <th class="center">Group Description</th>
                                                                                            <th class="center">Head</th>
                                                                                            <th class="center">Specification</th>
                                                                                            <th class="center">Action</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>                                                                    
                                                                                    <tr class="blueShed">
                                                                                        <td class="center-align">
                                                                                            <asp:TextBox class="form-control center" style="width:50px" ID="newOrder" runat="server" ClientIDMode="Static" TabIndex="24" Text='<%# Eval("itemOrder") %>' />
                                                                                        </td>
                                                                                        <td class="center-align">
                                                                                            <asp:TextBox class="form-control center" ID="newGroupHead" runat="server" ClientIDMode="Static" TabIndex="25" Text='<%# Eval("GroupHead") %>' />
                                                                                        </td>
                                                                                        <td class="center-align">
                                                                                            <asp:TextBox class="form-control center" ID="newMaterialHead" runat="server" ClientIDMode="Static" TabIndex="26" Text='<%# Eval("MaterialHead") %>' />
                                                                                        </td>
                                                                                        <td class="center-align">
                                                                                            <asp:TextBox class="form-control center" ID="newMaterialSpec" runat="server" ClientIDMode="Static" TabIndex="27" Text='<%# Eval("MaterialSpec") %>' />
                                                                                        </td>
                                                                                        <td class="center-align">
                                                                                            <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>'/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="blueShed">
                                                                                        <td class="center-align" colspan="3">
                                                                                            <asp:TextBox class="form-control center" ID="newMaterialRemarks" runat="server" ClientIDMode="Static" TabIndex="28" Text='<%# Eval("MaterialRemarks") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                            <tr class="blueShed mt-1" style="border-top:3px solid grey; padding-top:5px;">
                                                                                <td class="center-align">
                                                                                    <asp:TextBox class="form-control text-left" style="width:50px" ID="newOrder1" runat="server" ClientIDMode="Static" TabIndex="29" Text='<%# Eval("itemOrder") %>' />
                                                                                </td>
                                                                                <td class="center-align">
                                                                                    <asp:TextBox class="form-control text-left" ID="newGroupHead1" runat="server" ClientIDMode="Static" TabIndex="30" Text='<%# Eval("GroupHead") %>' />
                                                                                </td>
                                                                                <td class="center-align">
                                                                                    <asp:TextBox class="form-control text-left" ID="newMaterialHead1" runat="server" ClientIDMode="Static" TabIndex="31" Text='<%# Eval("MaterialHead") %>' />
                                                                                </td>
                                                                                <td class="center-align">
                                                                                    <asp:TextBox class="form-control text-left" ID="newMaterialSpec1" runat="server" ClientIDMode="Static" TabIndex="32" Text='<%# Eval("MaterialSpec") %>' />
                                                                                </td>
                                                                                <td class="center-align">
                                                                                    <asp:ImageButton CssClass="text-center" ID="btnSaveSpecAdd" runat="server" ClientIDMode="Static" ImageUrl="~/images/Buttons/bt-add.png" Width="70" Height="30" OnClick="btnSaveSpecAdd_Click"/>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="blueShed mb-1" style="border-bottom:3px solid grey; padding-bottom:5px;">
                                                                                <td class="center-align" colspan="3">
                                                                                    <asp:TextBox class="form-control text-left" ID="newMaterialRemarks1" runat="server" ClientIDMode="Static" TabIndex="33" Text='<%# Eval("MaterialRemarks") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div class="row">
                                                                            <div class="col m12">
                                                                                <div id="divspecmsg" class="usermsg" runat="server" clientidmode="Static"></div>
                                                                            </div>
                                                                            <div class="col m12">
                                                                                <asp:Button class="btn cyan center mr-1" ID="btnSaveSpec" runat="server" ClientIDMode="Static" Text="Save" TabIndex="34" OnClick="btnSaveSpec_Click" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                </div>
                                            </div>

                                            <div id="shortdesc" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="row">
                                                    <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                        <div class="row">
                                                            <div id="divRemarks" runat="server" clientidmode="Static" class="input-field col m12 p-5  m-5">
                                                                <asp:TextBox ID="txtProductSpecification" runat="server" TabIndex="36" class="content" TextMode="MultiLine" placeholder="" EnableViewState="true" ClientIDMode="Static" Style="min-height:350px;" MaxLength="500" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <div id="proddetail" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="row">
                                                    <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <asp:Repeater ID="rptProductDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptProductDetail_ItemCommand">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="left-align" >Product Name</th>
                                                                            <th class="center-align">Unit</th>
                                                                            <th class="center-align">Quantity</th>
                                                                            <th class="center-align">Action</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <td class="left-align" ><%# Eval("ProductName")%></td>
                                                                        <td class="center-align"><%# Eval("Unit")%></td>
                                                                        <td class="center-align"><%# Eval("Quantity")%></td>
                                                                        <td class="center-align">
                                                                            <asp:ImageButton ID="imgBtnDelDetail" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="DeleteDetail" CommandArgument='<%# Eval("ProductID") %>' Width="20" Height="20" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <tr class="blueShed">
                                                                <asp:HiddenField ID="hdnSubProductID" runat="server" ClientIDMode="Static" />
                                                                <td class="left width-100">
                                                                    <asp:TextBox class="form-control" ID="newProductName" runat="server" ClientIDMode="Static" TabIndex="38" AutoPostBack="true" onKeyup="bindDDLProductTo('#newProductName')" OnTextChanged="newProductName_TextChanged" placeholder="Min. 3 Char. To Search Product ..." />
                                                                </td>
                                                                <td class="center-align">
                                                                    <asp:TextBox class="form-control" ID="newUnit" runat="server" ClientIDMode="Static" Width="100px" TabIndex="39" style="display:inline-block;" />
                                                                </td>
                                                                <td class="center-align">
                                                                    <asp:TextBox class="form-control" ID="newQuantity" runat="server" ClientIDMode="Static" Width="100px" TabIndex="40" style="display:inline-block;" />
                                                                </td>
                                                                <td class="center-align">
                                                                    <asp:ImageButton ID="imgBtnSaveDetail" runat="server" ClientIDMode="Static" ImageUrl="~/images/Buttons/bt-add.png" Width="70" Height="30" OnClick="imgBtnSaveDetail_Click"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <div id="prodacc" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="row">
                                                    <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                        <table class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <asp:Repeater ID="rptAccessories" runat="server" ClientIDMode="Static" OnItemCommand="rptAccessories_ItemCommand">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="left-align">Product Name</th>
                                                                            <th class="center-align">Quantity</th>
                                                                            <th class="center-align">Action</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <td class="left-align" ><%# Eval("ProductName")%></td>
                                                                        <td class="center-align"><%# Eval("Quantity")%></td>
                                                                        <td class="center-align">
                                                                            <asp:ImageButton ID="imgBtnDelDetail" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="DeleteDetail" CommandArgument='<%# Eval("ProductID") %>' Width="20" Height="20" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <tr class="blueShed">
                                                                <asp:HiddenField ID="hdnAccProductID" runat="server" ClientIDMode="Static" />
                                                                <td class="left width-100">
                                                                    <asp:TextBox class="form-control" ID="accProductName" runat="server" ClientIDMode="Static" TabIndex="42" AutoPostBack="true" onKeyup="bindDDLProductToAcc('#accProductName')" placeholder="Min. 3 Char. To Search Product ..." />
                                                                </td>
                                                                <td class="center">
                                                                    <asp:TextBox class="form-control" ID="accQuantity" runat="server" ClientIDMode="Static" Width="100px" TabIndex="43" style="display:inline-block;" MaxLength="5"/>
                                                                </td>
                                                                <td class="center">
                                                                    <asp:ImageButton ID="imgBtnSaveDetailAcc" runat="server" ClientIDMode="Static" ImageUrl="~/images/Buttons/bt-add.png" Width="70" Height="30" OnClick="imgBtnSaveDetailAcc_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <div id="proddocs" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="section app-file-manager-wrapper">
                                                <div class="app-file-overlay"></div>
                                                <div class="row">
                                                    <div class="app-file-area" style="height:400px;">
                                                        <div class="app-file-header">
                                                            <div class="row">
                                                                <div class="input-field col m12 add-new-file mt-0">
                                                                    <div class="col m12">
                                                                        <label class="active" for="uploadDocument">Upload Product Specification Document Here  </label>
                                                                    </div>
                                                                    <div class="col m12">
                                                                        <asp:FileUpload ID="uploadDocument" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                                        <asp:Button ID="btnUpload1" CssClass="hide" runat="server" Text="Upload" onclick="btnUpload1_Click"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="app-file-content">
                                                            <div class="app-file-sidebar-content row">
                                                                <div class="col m12 app-file-files">
                                                                    <asp:Repeater ID="rptProdDocs" runat="server" ClientIDMode="Static" OnItemCommand="rptProdDocs_ItemCommand">
                                                                    <ItemTemplate>
                                                                        <div class="col xl4 l6 m4 s6">
                                                                            <div class="card box-shadow-none mb-1 app-file-info">
                                                                                <div class="card-content" style="background-color:beige; padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                    <div class="row fonticon p2">
                                                                                        <div class="col m6">
                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" TabIndex="47" CommandArgument='<%# Eval("pkID") %>'>
                                                                                            <i class="material-icons delete" style="padding: 5px 0px 0px 20px;">delete_outline</i>
                                                                                            </asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="col m6">
                                                                                            <a id="lnkViewFile" onclick="ShowPDFfile('<%# Eval("FileName").ToString() %>');">
                                                                                                <i class="material-icons" style="padding: 5px 0px 0px 20px;">find_in_page</i>
                                                                                            </a>
                                                                                        </div>
                                                                                    </div>
                                                                                    <img class="recent-file" src="app-assets/images/icon/pdf.png" height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">
                                                                                </div>
                                                                                <div class="app-file-details" style="background-color:silver;border-radius: 8px; padding-left: 10px;">
                                                                                    <div class="app-file-name font-weight-700">
                                                                                        <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("FileName").ToString().Substring(6) %></p>
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
                                                    <asp:PostBackTrigger ControlID="btnUpload1" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>

                                            <div id="prodimg" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                <div class="row">
                                                    <div class="col s12">
<%--                                                        <ul class="collapsible collapsible-accordion">
                                                            <li>--%>
<%--                                                                <div class="collapsible-header purple lightrn-1 white-text"><i class="material-icons">settings_backup_restore</i> Primary Image</div>
                                                                <div class="collapsible-body purple lighten-5 padding-1">--%>
                                                                    <div class="section app-file-manager-wrapper no-margin border-none">
                                                                        <div class="app-file-overlay"></div>
                                                                        <div class="app-file-area">
                                                                                <div class="app-file-header">
                                                                                    <div class="row">
                                                                                        <div class="input-field col m12 add-new-file mt-0">
                                                                                            <div class="col m12">
                                                                                                <label class="active" for="uploadDocument">Upload Product Image ...</label>
                                                                                            </div>
                                                                                            <div class="col m12">
                                                                                                <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                                                                <asp:Button ID="Button2" CssClass="hide" runat="server" Text="Upload" TabIndex="48" onclick="btnUpload1_Click" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="app-file-content">
                                                                                    <div id="divImageControl2 col m12">
                                                                                        <div class="col m3 center-align" style="padding: 10px;border-radius: 4px;border: 1px solid silver;">
                                                                                            <asp:Image ID="imgProduct" runat="server" ClientIDMode="Static" Width="100" Height="100" TabIndex="49" ImageUrl="images/noimage.png" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div id="divImageControl3 col m12">
                                                                                    <asp:ImageButton ID="btnDeleteImg" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" OnClick="btnDeleteImg_Click" Width="20" Height="20" TabIndex="39" />
                                                                                </div>

                                                                            </div>
                                                                    </div>
<%--                                                                </div>--%>
<%--                                                            </li>
                                                            <li>--%>
<%--                                                                <div class="collapsible-header cyan white-text"><i class="material-icons">toll</i> Additional Image Gallery</div>
                                                                <div class="collapsible-body cyan lighten-5 lighten-5 padding-1">--%>
                                                                    <div class="section app-file-manager-wrapper no-margin  border-none">
                                                                    <div class="app-file-overlay"></div>
                                                                        <div class="app-file-area" style="height:400px; overflow-y:auto;">
                                                                            <div class="app-file-header">
                                                                                <div class="row">
                                                                                    <div class="input-field col m12 add-new-file mt-0">
                                                                                        <div class="col m12">
                                                                                            <label class="active" for="FileUpload2">Upload Image Gallery</label>
                                                                                        </div>
                                                                                        <div class="col m12">
                                                                                            <asp:FileUpload ID="uploadImgGallery" multiple="multiple" runat="server" ClientIDMode="Static" CssClass="Cntrl1" onchange="this.form.submit()" ToolTip="Select Only PDF File" /> 
                                                                                            <asp:Button ID="btnUpload3" CssClass="hide" runat="server" Text="Upload" TabIndex="45" onclick="btnUpload3_Click" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="app-file-content">
                                                                                <div class="app-file-sidebar-content row">
                                                                                    <div class="col m12 app-file-files">
                                                                                        <asp:Repeater ID="rptProductImages" runat="server" ClientIDMode="Static" OnItemCommand="rptProductImages_ItemCommand">
                                                                                        <ItemTemplate>
                                                                                            <div class="col m4">
                                                                                                <div class="card box-shadow-none mb-1 app-file-info">
                                                                                                    <div class="row col m12 p-0">
                                                                                                        <div class="col m6 p-0 left-align">
                                                                                                            <asp:LinkButton ID="ImgBtnDelete" runat="server" CommandName="Delete" TabIndex="47" CommandArgument='<%# Eval("pkID") %>'>
                                                                                                            <i class="material-icons delete" style="padding: 5px 0px 0px 20px;">delete_outline</i>
                                                                                                            </asp:LinkButton>
                                                                                                        </div>
                                                                                                        <div class="col m6 p-0 right-align">
                                                                                                            <a id="lnkViewFile">
                                                                                                                <i class="material-icons" style="padding: 5px 0px 0px 20px;">find_in_page</i>
                                                                                                            </a>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="card-content" style="background-color:beige; padding: 10px;border-radius: 4px;border: 1px solid silver">
                                                                                                    <div class="app-file-content-logo grey lighten-4" style="height:100px">
                                                                                                            
                                                                                                        <div class="row fonticon p2" style="background:url('<%# "productimages/" + Eval("Name").ToString() %>') no-repeat top center; background-size:cover; width: 150px; height: 98px; margin-left: 10px;">

                                                                                                        </div>
                                                                                                        <%--<img class="recent-file" src='<%# "productimages/" + Eval("Name").ToString() %>' height="38" width="30" alt="Card image cap" style="display: block; margin: 5px auto;">--%>
                                                                                                    </div>
                                                                                                    <div class="app-file-details" style="background-color:silver;border-radius: 8px; padding-left: 10px;">
                                                                                                        <div class="app-file-name font-weight-700">
                                                                                                            <p style="display: block;width: 180px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;"><%# Eval("Name").ToString() %></p>
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
<%--                                                                </div>
                                                            </li>
                                                        </ul>--%>
                                                    </div>
                                                 </div>

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUpload1" />
                                                    <asp:PostBackTrigger ControlID="btnDeleteImg" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row border-radius-5" style="border:2px solid navy;">
                            <h5 class="card-title left-align width-100 white-text gradient-45deg-indigo-purple" style="min-height:35px; padding:8px;">Stock Information</h5>
                            <div class="input-field col m3">
                                <asp:TextBox ID="txtOpeningSTK" runat="server" MaxLength="50" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="44" AutoPostBack="true" OnTextChanged="txtOpeningSTK_TextChanged" /> 
                                <label class="active" for="txtOpeningSTK">Opening Stock</label>
                            </div>
                            <div class="input-field col m3">
                                <asp:TextBox ID="txtInwardSTK" runat="server" MaxLength="50" class="form-control" ClientIDMode="Static" ReadOnly="true" Placeholder=""  />                                
                                <label class="active" for="txtInwardSTK">Inward Stock</label>
                            </div>
                            <div class="input-field col m3">
                                <asp:TextBox ID="txtOutwardSTK" runat="server" MaxLength="50" class="form-control" ClientIDMode="Static" ReadOnly="true" Placeholder="" />                                
                                <label class="active" for="txtOutwardSTK">Outward Stock</label>
                            </div>
                            <div class="input-field col m3">
                                <asp:TextBox ID="txtClosingSTK" runat="server" MaxLength="50" class="form-control" ClientIDMode="Static" ReadOnly="true" Placeholder="" />                                
                                <label class="active" for="txtClosingSTK">Closing Stock</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m12">
                                <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                            </div>
                        </div>
                    </div>
                    <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom center-align yellow-text" id="divRestriction" runat="server" clientidmode="Static" style="display:none;margin-top:10px;margin-bottom:10px; font-size:20px;" />
                    <div id="btnPanel" runat="server" clientidmode="Static" class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;">
                        <button ID="btnReset" type="button" runat="server" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabIndex="46"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button ID="btnSave" type="button" runat="server" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="45"><i class="material-icons left">save</i>Save</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>



        <script type="text/javascript">
            $("input").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                }
            });
            // -------------------------------------------------------------
            function bindDDLProductTo(selector) {
                if ($(selector).val().length >= 3) {
                    jQuery.ajax({
                        type: "POST",
                        url: "InquiryInfo.aspx/FilterProduct",
                        data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var sample = JSON.parse(data.d);
                            $(selector).autoComplete({
                                minLength: 1,
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
                                    console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                    $(selector).val(item.html());
                                    $("#hdnSubProductID").val(item.data('langname'));
                                    $("#newQuantity").focus();
                                }
                            });

                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                    return false;
                }
            }
            // -------------------------------------------------------------
            function bindDDLProductToAcc(selector) {
                if ($(selector).val().length >= 3) {
                    jQuery.ajax({
                        type: "POST",
                        url: "InquiryInfo.aspx/FilterProduct",
                        data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var sample = JSON.parse(data.d);
                            $(selector).autoComplete({
                                minLength: 1,
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
                                    console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                    $(selector).val(item.html());
                                    $("#hdnAccProductID").val(item.data('langname'));
                                    $("#accQuantity").focus();
                                }
                            });

                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                    return false;
                }
            }
            // -------------------------------------------------------------
            //$(function () {
            //    var tabName = $("[id*=hdnCurrentTab]").val() != "" ? $("[id*=hdnCurrentTab]").val() : "prodspecs";
            //    $('#myTab a[href="#' + tabName + '"]').tab('show');
            //    $("#myTab a").click(function () {
            //        $("[id*=hdnCurrentTab]").val($(this).attr("href").replace("#", ""));
            //    });
            //});
        </script>
    </form>
</body>
</html>