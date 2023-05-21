<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialTrans_old.aspx.cs" Inherits="StarsProject.FinancialTrans_old" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>

    <link  href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />    
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src='<%=ResolveUrl("js/jquery.auto-complete.min.js") %>'></script>

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

        .ui-widget-header,.ui-state-default, ui-button {
            background:#b9cd6d;
            border: 1px solid #b9cd6d;
            color: #FFFFFF;
            font-weight: bold;
        }

    </style>

    <script type="text/javascript">
        jQuery(document).ready(function () { });
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtCustomerName").val() == "") {

                if (jQuery("#txtCustomerName").val() == "")
                    errmsg += "CustomerName is required</br>";


                jQuery("#divErrorMessage").html(errmsg).show();

                validflag = false;
            }
            // ----------------------------------------
            return validflag;
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
                    <asp:HiddenField ID="hdnTransCategory" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />

                    <div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 id="lblPageHead" runat="server" clientidmode="Static" class="modal-title">Manage Financial Transaction</h5>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="input-field col m3">
                                        <label class="active" for="txtTransDate">Trans.Date</label>
                                        <asp:TextBox ID="txtTransDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" TextMode="Date" />
                                    </div>
                                    <div class="input-field col m">
                                        <label class="active" for="txtCustomerName">Customer Name<small class="text-muted font-italic">(Minimum 3 chars to activate search)</small></label>
                                        <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="2" />
                                    </div>
                                    <div class="input-field col m2">
                                        <label class="active" for="drpInquiry">Transaction Type</label>
                                        <asp:DropDownList ID="drpTransType" runat="server" class="form-control" ClientIDMode="Static" TabIndex="3"  AutoPostBack="true" OnSelectedIndexChanged="drpTransType_SelectedIndexChanged">
                                            <asp:ListItem Text="Bank" Value="Bank" />
                                            <asp:ListItem Text="Cash" Value="Cash" />
                                            <asp:ListItem Text="Paytm" Value="Paytm" />
                                            <asp:ListItem Text="BhimUPI" Value="BhimUPI" />
                                            <asp:ListItem Text="Phone-pe" Value="Phone-pe" />
                                            <asp:ListItem Text="Other" Value="Other" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row mt-2 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                    <div class="input-field col m12">
                                        <label class="active" for="txtTransNotes" style="color:white !important;">Remarks</label>
                                        <asp:TextBox ID="txtTransNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" MaxLength="500" TextMode="MultiLine" Rows="3" />
                                    </div>
                                </div>
                                <div class="row mt-2 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                    <div class="input-field col m">
                                        <label class="active" for="txtTransAmount" style="color:white !important;">Sales Order</label>
                                        <asp:DropDownList ID="drpOrder" runat="server" class="form-control" ClientIDMode="Static" TabIndex="5"  AutoPostBack="true" OnSelectedIndexChanged="drpTransType_SelectedIndexChanged" />
                                    </div>
                                    <div class="input-field col m">
                                        <label class="active" for="txtTransAmount" style="color:white !important;">Transaction Amount</label>
                                        <asp:TextBox ID="txtTransAmount" runat="server" class="form-control" ClientIDMode="Static" TabIndex="6" />
                                    </div>
                                    <div class="input-field col m">
                                        <label class="active" for="txtTransFrom" style="color:white !important;">Bank/Portal/Payment App. Name</label>
                                        <asp:TextBox ID="txtTransFrom" runat="server" class="form-control" ClientIDMode="Static" TabIndex="7" />
                                    </div>
                                    <div class="input-field col m">
                                        <label class="active" for="txtTransID" style="color:white !important;">Transaction ID</label>
                                        <asp:TextBox ID="txtTransID" runat="server" class="form-control" ClientIDMode="Static" TabIndex="8" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m12">
                                        <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static">
                                            <ul id="wowList" runat="server" class="text-center" clientidmode="Static"></ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" TabIndex="9" />
                                <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="10" />
                            </div>
                        </div>
                    </div>
                </div>
                <script type="text/javascript">
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
                                            return '<div class="autocomplete-suggestion" style="position:relative !important;" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
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
                    }
                </script>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>