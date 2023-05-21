 <%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Quotation_old.aspx.cs" Inherits="StarsProject.Quotation" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"> 
    <title></title>
    <link  href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>
    <script type="text/javascript" src="js/jquerysession.js"></script>

    <style type="text/css">
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
    </style>

    <script type="text/javascript">
        //jQuery(document).ready(function()  {
            //jQuery('#drpInquiry').on('change', function () {
            //    var tmpval = jQuery(this).val();
            //    alert(tmpval)
            //    e.preventDefault();
            //    jQuery('#dialog-loadproducts').dialog('open');
            //});

            //jQuery('#dialog-loadproducts').dialog({
            //    resizable: false,
            //    height: 200,
            //    autoOpen: false,
            //    modal: true,
            //    buttons: {
            //        "Yes": function () {
            //            jQuery("#hdnConfirm").val("yes");
            //            __doPostBack("loadproducts", "");
            //            jQuery(this).dialog("close");
            //        },
            //        "No": function () { jQuery(this).dialog("close"); }
            //    }
            //});
        //});
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            debugger;
            var cnt = jQuery("#tblQuotationDetail tr").length;
            // ----------------------------------------
            if (jQuery("#txtQuotationDate").val() == "" || jQuery("#drpCustomer").val() == "") {

                //if (jQuery("#txtQuotationNo").val() == "")
                //    errmsg += "Quotation No is required</br>";

                if (jQuery("#txtQuotationDate").val() == "")
                    errmsg += "Quotation Date is required</br>";

                if (jQuery("#drpCustomer").val() == "")
                    errmsg += "Customer selection is required</br>";

                //if (cnt <= 3)
                //    errmsg += "Atleast One Item is required to save Quotation</br>";

                jQuery("#divErrorMessage").html(errmsg).show();

                validflag = false;
            }
            // ----------------------------------------
            return validflag;
        }

        function calcQuotation() {
            var q = parseFloat(jQuery("#txtQuantity").val() != '' ? jQuery("#txtQuantity").val() : 0);
            var ur = parseFloat(jQuery("#txtUnitRate").val() != '' ? jQuery("#txtUnitRate").val() : 0);
            var dp = parseFloat(jQuery("#txtDiscountPercent").val() != '' ? jQuery("#txtDiscountPercent").val() : 0);

            var nr = (ur - ((ur * dp) / 100)).toFixed(2);
            jQuery("#txtNetRate").val(nr);

            var a = (q * nr).toFixed(2);
            jQuery("#txtAmount").val(a);

            var tr = parseFloat(jQuery("#txtTaxRate").val() != '' ? jQuery("#txtTaxRate").val() : 0);
            ta = ((a * tr) / 100).toFixed(2);
            jQuery("#txtTaxAmount").val(ta);

            na = (parseFloat(a) + parseFloat(ta)).toFixed(2);
            jQuery("#txtNetAmount").val(na);
        }
        // ----------------------------------------------------------
        function openSpecification(pProdID) {
            var t11;
            t11 = jQuery("#txtQuotationNo").val();
            $.colorbox({
                width: "98%", height: "98%", rel: 'cbQtSpec', inline: true, iframe: true, href: "QuotationSpecification.aspx?QuotationNo=" + t11 + "&FinishProductID=" + pProdID,
                onClosed: function () { }
            });
        }

        function openCustomerInfo(cat) {
            if (cat == 'view') {
                var keyid = jQuery('#hdnCustomerID').val();
                if (keyid != '' && keyid != undefined) {
                    jQuery.colorbox({ width: "100%", height: "100%", iframe: true, href: "customers.aspx?mode=view&id=" + keyid });
                }
                else {
                    alert("Please Search & Select Customer !")
                }
            }
            else if (cat == 'add') {
                jQuery.colorbox({ width: "100%", height: "100%", iframe: true, href: "customers.aspx?mode=view&id=0" });
            }
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server"  autocomplete="off">

    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">

        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
            <div class="widgetbox">
                <%-- Bootstrap Quotation Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnQuotationVersion" runat="server" ClientIDMode="Static" />
                <div id="myModal" style="display:block; width:100%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="input-field col m2">
                                    <label class="active" for="txtQuotationNo">Quotation #</label>
                                    <asp:TextBox ID="txtQuotationNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" readonly />
                                </div>
                                <div class="input-field col m3">
                                    <label class="active" for="txtQuotationDate">Quotation Date</label>
                                    <asp:TextBox ID="txtQuotationDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" TextMode="Date" />
                                </div>
                                <div class="input-field col m">
                                    <label class="active" for="txtCustomerName">Customer Name  
                                        <small class="text-muted font-italic">(Minimum 3 chars to activate search)</small>
                                        <a href="javascript:openCustomerInfo('view');">
                                            <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display:inline-block;" />
                                        </a>&nbsp;
                                        <a href="javascript:openCustomerInfo('add');">
                                            <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" style="display:inline-block;" />
                                        </a>
                                    </label>
                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="3"  />
                                </div>
                                <div class="input-field col m2">
                                    <label class="active" for="drpInquiry">Inquiry No</label>
                                    <asp:DropDownList ID="drpInquiry" runat="server" class="form-control" EnableViewState="true" ClientIDMode="Static" TabIndex="4" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m3">
                                    <label class="active" for="drpQuotationKindAttn">Kind Attn.</label>
                                    <asp:DropDownList ID="drpQuotationKindAttn" runat="server" class="form-control" EnableViewState="true" ClientIDMode="Static" TabIndex="5" />
                                </div>
                                <div class="input-field col m">
                                    <label class="active" for="txtQuotationSubject">Subject</label>
                                    <asp:TextBox ID="txtQuotationSubject" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" MaxLength="100" />
                                </div>
                                <div class="input-field col m3">
                                    <label class="active" for="drpProjects">Projects</label>
                                    <asp:DropDownList ID="drpProjects" runat="server" class="form-control" EnableViewState="true" ClientIDMode="Static" TabIndex="7" />
                                </div>
                            </div>
                            <asp:Panel runat="server" ID="pnlDetail">
                                <div class="row">
                                    <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>--%>
                                    <table id="tblQuotationDetail" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <asp:Repeater ID="rptQuotationDetail" runat="server" ClientIDMode="Static" OnItemCommand="rptQuotationDetail_ItemCommand" OnItemDataBound="rptQuotationDetail_ItemDataBound">
                                            <HeaderTemplate>
                                                <thead>
                                                    <tr>
                                                        <th class="text-center">Product Name</th>
                                                        <th class="text-right">Quantity</th>
                                                        <th class="text-center">Unit</th>
                                                        <th class="text-right">Unit Rate</th>
                                                        <th class="text-right">Disc. %</th>
                                                        <th class="text-right">Net Rate</th>
                                                        <th class="text-right">Amount</th>
                                                        <th class="text-right">Tax Rate</th>
                                                        <th class="text-right">Tax Amount</th>
                                                        <th class="text-right">Net Amount</th>
                                                        <% if (hdnQuotationVersion.Value == "2")
                                                           { %>
                                                        <th class="text-right">Bundle</th>
                                                        <% } %>
                                                        <% if (hdnQuotationVersion.Value == "3")
                                                           { %>
                                                        <th class="text-right">Specs</th>
                                                        <% } %>
                                                        <th class="text-center">Delete</th>
                                                    </tr>
                                                </thead>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <asp:HiddenField ID="hdnItemCode" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductID") %>' />
                                                    <td>
                                                        <%# Eval("ProductName") %>
                                                        <asp:HiddenField ID="edProductName" runat="server" ClientIDMode="Static" Value='<%# Eval("ProductName") %>' />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edQuantity" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" TabIndex="9" />
                                                    </td>
                                                    <td class="text-center"><%# Eval("Unit") %></td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edUnitRate" runat="server" ClientIDMode="Static" Text='<%# Eval("UnitRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" TabIndex="10" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edDiscountPercent" runat="server" ClientIDMode="Static" Text='<%# Eval("DiscountPercent") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" TabIndex="11" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edNetRate" runat="server" ClientIDMode="Static" Text='<%# Eval("NetRate") %>' AutoPostBack="true" Style="width: 80px; text-align: right;" Enabled="false" TabIndex="12" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("Amount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" TabIndex="13" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edTaxRate" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxRate") %>' AutoPostBack="true" OnTextChanged="editItem_TextChanged" Style="width: 80px; text-align: right;" TabIndex="14" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edTaxAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("TaxAmount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" TabIndex="15" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="edNetAmount" runat="server" ClientIDMode="Static" Text='<%# Eval("NetAmount") %>' AutoPostBack="true" Style="width: 100px; text-align: right;" Enabled="false" TabIndex="16" />
                                                    </td>
                                                    <% if (hdnQuotationVersion.Value == "2")
                                                       { %>
                                                    <td class="text-right">
                                                        <asp:DropDownList ID="drpBundle0" runat="server" CssClass="form-control" ClientIDMode="Static" TabIndex="6" Style="width: 100px;" AutoPostBack="true" OnSelectedIndexChanged="editItem_TextChanged"></asp:DropDownList>
                                                        <asp:HiddenField ID="hdnBundle" runat="server" ClientIDMode="Static" Value='<%# Eval("BundleId") %>' />
                                                    </td>
                                                    <% } %>
                                                    <% if (hdnQuotationVersion.Value == "3")
                                                       { %>
                                                    <td class="text-right">
                                                        <a id="lnkSpecs" href="javascript:openSpecification('<%# Eval("ProductID") %>');"><small>Specs</small></a>
                                                    </td>
                                                    <% } %>
                                                    <td class="text-center">
                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="17" OnClientClick="return delClientClick()" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <tr style="background-color: #dde8f3;">
                                                    <td>
                                                        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
                                                        <%--<asp:DropDownList ID="drpProduct" runat="server" ClientIDMode="Static"  Width="200" TabIndex="51" AutoPostBack="true" OnSelectedIndexChanged="drpProduct_SelectedIndexChanged"/>--%>
                                                        <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLProductTo('#txtProductName')" AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="19" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" Width="80" TabIndex="20" onblur="javascript:calcQuotation();" />
                                                    </td>
                                                    <td class="text-center">
                                                        <asp:TextBox ID="txtUnit" runat="server" ClientIDMode="Static" Width="80" TabIndex="21" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtUnitRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="22" onblur="javascript:calcQuotation();" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtDiscountPercent" runat="server" ClientIDMode="Static" Width="80" TabIndex="23" onblur="javascript:calcQuotation();" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtNetRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="24" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="25" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtTaxRate" runat="server" ClientIDMode="Static" Width="80" TabIndex="26" onblur="javascript:calcQuotation();" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtTaxAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="27" Enabled="false" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:TextBox ID="txtNetAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="28" Enabled="false" />
                                                    </td>
                                                    <% if (hdnQuotationVersion.Value == "2")
                                                       { %>
                                                    <td class="text-right">
                                                        <asp:DropDownList ID="drpBundle1" runat="server" CssClass="form-control" ClientIDMode="Static" TabIndex="6" Width="100" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <% } %>
                                                    <td class="text-center" colspan="2">
                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/expand.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="20" Height="20" TabIndex="29" />
                                                    </td>
                                                </tr>
                                                <tr style="background-color: navy; min-height: 50px;">
                                                    <td class="text-right" colspan="6" style="color: White; font-size: 16px;">Grand Total: </td>
                                                    <td class="text-right">
                                                        <asp:Label ID="lblTotalGrossAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="30" Enabled="false" Style="color: White; font-size: 16px;" Text="1111" />
                                                    </td>
                                                    <td class="text-right" style="color: White; font-size: 16px;"></td>
                                                    <td class="text-right">
                                                        <asp:Label ID="lblTotalTaxAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="31" Enabled="false" Style="color: White; font-size: 16px;" Text="1111" />
                                                    </td>
                                                    <td class="text-right">
                                                        <asp:Label ID="lblTotalNetAmount" runat="server" ClientIDMode="Static" Width="100" TabIndex="32" Enabled="false" Style="color: White; font-size: 16px;" Text="1111" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <%--</ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rptQuotationDetail" />
                                </Triggers>
                                </asp:UpdatePanel>--%>
                                </div>
                            </asp:Panel>
                            <div class="row mt-2 badge-primary" style="border-radius: 5px; padding: 5px;">
                                <div class="input-field col m6">
                                    <div style="height: 50px; width: 100%; margin-bottom: 10px; padding: 5px; border-bottom: 1px solid white;">Header Description</div>
                                    <asp:TextBox ID="txtQuotationHeader" runat="server" class="form-control" ClientIDMode="Static" TabIndex="33" TextMode="MultiLine" Rows="4" MaxLength="500" />
                                </div>
                                <div class="input-field col m6">
                                    <div style="height: 50px; width: 100%; margin-bottom: 10px; padding: 5px; border-bottom: 1px solid white;">Terms & Condition
                                        <asp:DropDownList ID="drpTNC" runat="server" ClientIDMode="Static" class="form-control float-right" EnableViewState="true" TabIndex="34" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="drpTNC_SelectedIndexChanged" />
                                    </div>
                                    <asp:TextBox ID="txtQuotationFooter" runat="server" class="form-control" ClientIDMode="Static" MaxLength="1000" TabIndex="35" TextMode="MultiLine" Rows="4" />
                                </div>
                            </div>
                            <div id="divFollowUp" runat="server" clientidmode="Static">
                                <div class="row mt-2 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                    <div class="input-field col m4">
                                        <label class="active" for="txtNextFollowupDate" style="color:white !important;">Next Followup</label>
                                        <asp:TextBox ID="txtNextFollowupDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="36" TextMode="Date"/>
                                        <label class="active" for="drpFollowupType" style="color:white !important;">Followup Type</label>
                                        <asp:DropDownList ID="drpFollowupType" runat="server" class="form-control" ClientIDMode="Static" TabIndex="37"></asp:DropDownList>
                                    </div>
                                    <div class="input-field col m8">
                                        <label class="active" for="txtMeetingNotes" style="color:white !important;">Meeting Notes</label>
                                        <asp:TextBox ID="txtMeetingNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="38" MaxLength="500" TextMode="MultiLine" Rows="4" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="input-field col m12">
                                    <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" OnClientClick="return validateForm();" OnClick="btnSave_Click" TabIndex="39" />
                            <asp:Button ID="btnSaveEmail" runat="server" ClientIDMode="Static" Text="Save & Send Email" class="btn btn-primary" OnClick="btnSaveEmail_Click" TabIndex="40" />
                            <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="42" />
                        </div>
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                var isByDelete = false;
                $(document).on("keypress", "input", function (event) {                    
                    if (event.which === 13 && event.target.type!=="image") {
                        event.preventDefault();
                        event.stopPropagation();
                    }                    
                });

                function bindDDLTo(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterCustomer",
                            data: '{pCustName:\'' + $(selector).val() + '\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                console.log(data);
                                //var resultdata = JSON.parse(data.d);
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
                                        //search = search.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
                                        //var re = new RegExp("(" + search.split(' ').join('|') + ")", "gi");
                                        return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnCustomerID").val(item.data('langname'));
                                        $("#drpInquiry").focus();
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
                            $("#hdnCustomerID").val('');
                        }
                    }
                }

                function bindDDLProductTo(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterProduct",
                            data: '{pProductName:\'' + $(selector).val() + '\'}',
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
                                        $("#hdnProductID").val(item.data('langname'));
                                        $("#txtQuantity").focus();
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
               
                function clearProductField() {
                    $("#hdnProductID").val('');
                    $("#txtProductName").val('');
                    $("#txtQuantity").val('');
                    $("#txtUnit").val('');
                    $("#txtUnitRate").val('');
                    $("#txtDiscountPercent").val('');
                    $("#txtNetRate").val('');
                    $("#txtAmount").val('');
                    $("#txtTaxRate").val('');
                    $("#txtTaxAmount").val('');
                    $("#txtNetAmount").val('');
                    $("#txtProductName").focus();
                }
            </script>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
    
</body>
</html>
