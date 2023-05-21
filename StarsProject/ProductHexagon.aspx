<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductHexagon.aspx.cs" Inherits="StarsProject.ProductHexagon" %>

<!DOCTYPE html>

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
            if (jQuery("#txtProductName").val() == "" || jQuery("#txtProductAlias").val() == "" || jQuery("#txtBrandName").val() == "") {
                validflag = false;

                if (jQuery("#txtProductName").val() == "")
                    errmsg += "<li>Product Name is required</li>";

                if (jQuery("#txtProductAlias").val() == "")
                    errmsg += "<li>Product Alias is required</li>";

                if (jQuery("#txtBrandName").val() == "")
                    errmsg += "<li>Brand Name is required</li>";
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

        //function retainTabPosition() {
        //    //alert($("#hdnCurrentTab").val());
        //    $('#lnkprodspecs').removeClass('active');
        //    $('#lnkshortdesc').removeClass('active');
        //    $('#lnkproddetail').removeClass('active');
        //    $('#lnkproddocs').removeClass('active');
        //    $('#lnkprodimg').removeClass('active');
        //    $('#lnkprodacc').removeClass('active');

        //    $('#prodspecs').css("display", "none");
        //    $('#shortdesc').css("display", "none");
        //    $('#proddetail').css("display", "none");
        //    $('#proddocs').css("display", "none");
        //    $('#prodimg').css("display", "none");
        //    $('#prodacc').css("display", "none");
        //    // --------------------------------------------------- 
        //    if ($("#hdnCurrentTab").val() == 'prodspecs') {
        //        //alert('1')
        //        $('#lnkprodspecs').addClass('active');
        //        $('#prodspecs').css("display", "block");
        //    }
        //    else if ($("#hdnCurrentTab").val() == 'shortdesc') {
        //        //alert('2')
        //        $('#lnkshortdesc').addClass('active');
        //        $('#shortdesc').css("display", "block");
        //    }
        //    else if ($("#hdnCurrentTab").val() == 'proddetail') {
        //        //alert('3')
        //        $('#lnkproddetail').addClass('active');
        //        $('#proddetail').css("display", "block");
        //    }
        //    else if ($("#hdnCurrentTab").val() == 'proddocs') {
        //        //alert('4')
        //        $('#lnkproddocs').addClass('active');
        //        $('#proddocs').css("display", "block");
        //    }
        //    else if ($("#hdnCurrentTab").val() == 'prodimg') {
        //        //alert('5')
        //        $('#lnkprodimg').addClass('active');
        //        $('#prodimg').css("display", "block");
        //    }
        //    else if ($("#hdnCurrentTab").val() == 'prodacc') {
        //        //alert('5')
        //        $('#lnkprodacc').addClass('active');
        //        $('#prodacc').css("display", "block");
        //    }

        //}
        
        //function setHiddenControls()
        //{
        //    $('#frmProduct *').filter(':input').each(function () {
        //        var idName = $(this).attr('id') + ',';
        //        if ($("#hdnHiddenControl").val().indexOf(idName)>=0) {
        //            $(this).parent().hide();
        //        }
        //    });
        //}
    </script>
</head>
<body class="loginpage">
    <form id="frmProduct" runat="server">
         <asp:ScriptManager ID="srcUser" runat="server" EnablePartialRendering="true" EnablePageMethods="true" EnableViewState="true"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">
        <div class="widgetbox">
            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
            <div id="myModal" style="display:block; width:100%; padding:10px;">
                <div class="modal-content">
                    <div class="modal-body" style="min-height:300px;">
                        <div class="row">
                            <div class="input-field col m12">
                                <div class="row">
                                    <div class="input-field col m12">
                                        <label for="txtProductName" class="active">Product Name <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtProductName" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="1" />
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
                                    <div class="input-field col m6">
                                        <label class="active" for="txtProductAlias">Product Alias  <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtProductAlias" runat="server" ClientIDMode="Static" class="form-control" Placeholder="" MaxLength="50" TabIndex="3"/>
                                    </div>
                                    <div class="input-field col m6">
                                        <label class="active" for="txtBrandName">Brand Name <span class="materialize-red-text font-weight-800">*</span></label>
                                        <asp:TextBox ID="txtBrandName" runat="server" class="form-control" ClientIDMode="Static" Placeholder="" TabIndex="4"/>
                                    </div>                            
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="btnPanel" runat="server" clientidmode="Static" class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;">
                        <button id="btnReset" type="button" runat="server" text="Reset" class="btn orange left" onserverclick="btnReset_Click" tabindex="4"><i class="material-icons left">backspace</i>Clear & Add New</button>
                        <button id="btnSave" type="button" runat="server" text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" tabindex="5"><i class="material-icons left">save</i>Save</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
