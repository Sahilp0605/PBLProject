<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InquiryShare.aspx.cs" Inherits="StarsProject.InquiryShare" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>


    <style type="text/css">
        /*.clDiv70 {
            height: 50px;
            vertical-align: middle;
        }

        .imgChart {
            vertical-align: middle;
            Height: 40px;
            width: 40px;
        }

        # SelectDepartment {
            margin: 7px 0px;
            font-family: Verdana,Arial;
            font-size: 12px;
            vertical-align: middle;
        }

        .clTrans {
            background-color: transparent;
        }*/

        [type='checkbox']:not(:checked), [type='checkbox']:checked { position: none; opacity: 1; margin-right: 10px; margin-top: 2px; pointer-events:all;}
        .mychklist label { margin-left: 15px;}
    </style>

    <script type="text/javascript">
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            var cnt = jQuery("#tblInqProductGroup tr").length;

            // ----------------------------------------
            if (jQuery("#txtInquiryDate").val() == "" || jQuery("#txtCustomerName").val() == "" || jQuery("#hdnCustomerID").val() == "" || jQuery("#hdnCustomerID").val() == "0" || jQuery("#drpInquiryStatus").val() == "" || jQuery("#drpInquirySource").val() == "" || cnt <= 3) {

                if (jQuery("#txtInquiryDate").val() == "")
                    errmsg += "Inquiry Date is required</br>";

                if (jQuery("#txtCustomerName").val() == "" || jQuery("#hdnCustomerID").val() == "" || jQuery("#hdnCustomerID").val() == "0")
                    errmsg += "Customer selection is required</br>";

                if (jQuery("#drpInquiryStatus").val() == "")
                    errmsg += "Inquiry Status selection is required</br>";

                if (jQuery("#drpInquirySource").val() == "")
                    errmsg += "Inquiry Source selection is required</br>";

                if (cnt <= 3)
                    errmsg += "Atleast One Item is required to save Inquiry Information</br>";



                validflag = false;
            }
            // ----------------------------------------

            if (jQuery("#txtInquiryDate").val() != "") {
                var dateOne = new Date(jQuery("#txtInquiryDate").val()); //Year, Month, Date  
                var dateTwo = new Date(); //Year, Month, Date  
                if (dateOne > dateTwo) {
                    errmsg += "Future Inquiry Date Not Allowed..!!!";
                    validflag = false;
                }
            }
            //---------------------------------------
            jQuery("#divErrorMessage").html(errmsg).show();
            return validflag;
        }
        function CompareDate() {
            //Note: 00 is month i.e. January 
            if (jQuery("#txtInquiryDate").val() != "") {
                var dateOne = new Date(jQuery("#txtInquiryDate").val()); //Year, Month, Date  
                var dateTwo = new Date(); //Year, Month, Date  
                if (dateOne > dateTwo) {
                    alert("Future Inquiry Date Not Allowed..!!!");
                }
            }
        }
        function validateFormItems() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtInquiryDate").val() == "" || jQuery("#drpCustomer").val() == "" || jQuery("#drpInquiryStatus").val() == "" || jQuery("#drpInquirySource").val() == "") {


                if (jQuery("#txtInquiryDate").val() == "")
                    errmsg += "Inquiry Date is required</br>";

                if (jQuery("#drpCustomer").val() == "")
                    errmsg += "Customer selection is required</br>";

                if (jQuery("#drpInquiryStatus").val() == "")
                    errmsg += "Inquiry Status selection is required</br>";

                if (jQuery("#drpInquirySource").val() == "")
                    errmsg += "Inquiry Source selection is required</br>";

                //if (jQuery("#txtFollowupDate").val() == "")
                //    errmsg += "Follow-Up Date selection is required</br>";

                jQuery("#divErrorMessage").html(errmsg).show();

                validflag = false;
            }
            // ----------------------------------------
            return validflag;
        }

        function openCustomerPopup(postbackRedirect, postbackKeyID) {
            //alert("Opening Customer Popup")
            //alert(postbackKeyID)
            //parent.jQuery.colorbox.close();
            //jQuery.colorbox({
            //    width: "50%", height: "50%", iframe: true, href: "customers.aspx?id=1",
            //    onClosed: function () {
            //        //location.reload(); 
            //    }
            //});
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <%-- Bootstrap INQUIRY INFO Modal Popup --%>
                        <div class="clearall"></div>
                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnUserID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />

                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <%--<div class="row">
                                        <div class="input-field col m2">
                                            <label class="active" for="txtInquiryNo">Inquiry #</label>
                                            <asp:TextBox ID="txtInquiryNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtInquiryDate">Inquiry Date</label>
                                            <asp:TextBox ID="txtInquiryDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" TextMode="Date" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="txtCustomerName">Customer Name  <small class="text-muted font-italic">(Minimum 3 chars to activate search)</small></label>
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" placeholder="" TabIndex="3" MaxLength="50" />
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="drpInquiryStatus">Inquiry Status</label>
                                            <asp:DropDownList ID="drpInquiryStatus" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <label class="active" for="drpInquirySource">Source Of Inquiry</label>
                                            <asp:DropDownList ID="drpInquirySource" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="5">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="input-field col m6">
                                            <label class="active" for="txtReferenceName">Reference Name</label>
                                            <asp:TextBox ID="txtReferenceName" runat="server" class="form-control" ClientIDMode="Static" placeholder="" TabIndex="6" MaxLength="50" />
                                        </div>
                                    </div>--%>
                                        
                                    <div class="row">
                                        <!--Change Design-->
                                        <div class="input-field col m6">
                                            <div class="row">
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtInquiryNo">Inquiry #</label>
                                                    <asp:TextBox ID="txtInquiryNo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly />
                                                </div>
                                                <div class="input-field col m3">
                                                    <label class="active" for="txtInquiryDate">Inquiry Date</label>
                                                    <asp:TextBox ID="txtInquiryDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" TextMode="Date" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtCustomerName">Customer Name  <small class="text-muted font-italic">(Minimum 3 chars to activate search)</small></label>
                                                    <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLTo('#txtCustomerName')" placeholder="" TabIndex="3" MaxLength="50" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <label class="active" for="drpInquiryStatus">Inquiry Status</label>
                                                    <asp:DropDownList ID="drpInquiryStatus" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <label class="active" for="drpInquirySource">Source Of Inquiry</label>
                                                    <asp:DropDownList ID="drpInquirySource" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="5">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <label class="active" for="txtReferenceName">Reference Name</label>
                                                    <asp:TextBox ID="txtReferenceName" runat="server" class="form-control" ClientIDMode="Static" placeholder="" TabIndex="6" MaxLength="50" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="input-field col m6">
                                            <!--Change Design-->
                                            <div id="divAssignTo" runat="server" clientidmode="Static" class="input-field mychklist col m6">
                                                <div class="card">
                                                    <div class="card-header">
                                                        <h5 class="card-title mb-0 left-align width-100 gradient-45deg-light-blue-indigo">Shared With</h5>
                                                    </div>
                                                    <div class="card-body">
                                                        <asp:CheckBoxList ID="chklstEmployee" runat="server" TabIndex="6">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
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
                                    <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" TabIndex="16" />
                                    <%--<asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="18" />--%>
                                </div>
                            </div>
                        </div>
                    

                    <script type="text/javascript">
                        $("input").keypress(function (event) {
                            if (event.which == 13) {
                                event.preventDefault();
                                //$("form").submit();
                            }
                        });

                        $("#txtProductName").focusin(function () {
                            window.scrollBy(0, 150);
                        });

                        function bindDDLTo(selector) {
                            if ($(selector).val().length >= 3) {
                                jQuery.ajax({
                                    type: "POST",
                                    url: "InquiryInfo.aspx/FilterCustomer",
                                    data: '{pCustName:\'' + $(selector).val() + '\'}',
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
                                                return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                            },
                                            onSelect: function (e, term, item) {
                                                console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                                $(selector).val(item.html());
                                                $("#hdnCustomerID").val(item.data('langname'));
                                                $("#hdnBlockCustomer").val(item.data('BlockCustomer'));
                                                $("#drpInquiryStatus").focus();
                                            }
                                        });

                                    },
                                    error: function (r) { alert('Error : ' + r.responseText); },
                                    failure: function (r) { alert('failure'); }
                                });
                                return false;
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
                                                $("#txtReferenceName").focus();
                                            }
                                        });

                                    },
                                    error: function (r) { alert('Error : ' + r.responseText); },
                                    failure: function (r) { alert('failure'); }
                                });
                                return false;
                            }
                        }

                        function clearProductField() {
                            $("#hdnProductID").val('');
                            $("#txtProductName").val('');
                            $("#txtUnitPrice").val('');
                            $("#txtQuantity").val('');
                            $("#lblAmount").val('');
                            $("#txtProductName").focus();
                        }
                    </script>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
