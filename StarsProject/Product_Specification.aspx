<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Product_Specification.aspx.cs" Inherits="StarsProject.Product_Specification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="js/jquery-3.3.1.js"></script>
    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/datatables/jquery.dataTables.min.js"></script>
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />


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
    </style>
    <style type="text/css">
        .required:after {
            content: " *";
            color: #f00;
        }
    </style>
    <script type="text/javascript">
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtGroupHead").val().trim() == "" || jQuery("#txtMaterialHead").val().trim() == "") {

                if (jQuery("#txtGroupHead").val().trim() == "")
                    errmsg += "Group Name is required</br>";

                if (jQuery("#txtMaterialHead").val().trim() == "")
                    errmsg += "Specification Head is required</br>";

                jQuery("#divErrorMessage").html(errmsg).show();

                validflag = false;
            }
            // ----------------------------------------
            if (validflag == true) {
                btnCloseDialog();
            }
            return validflag;
        }

        function btnCloseDialog() {
            jQuery('.modal-backdrop').remove();
            jQuery('#myModal').modal('hide');
            jQuery('.modal-backdrop').remove();
        }

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnPkIDEmp").val('');
        }
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <asp:HiddenField ID="hdnProductSpecificationID" runat="server" ClientIDMode="Static" />
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active required" for="drpGroupHead">Group Description</label>
                                            <%--<asp:DropDownList ID="drpGroupHead" runat="server" ClientIDMode="Static" TabIndex="16" class="form-control"  Style="height: inherit;" />--%>
                                            <asp:TextBox ID="txtGroupHead" runat="server" ClientIDMode="Static" class="form-control" onKeyup="bindDDLGroupHeadTo('#txtGroupHead')" TabIndex="1" MaxLength="50" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active required" for="txtMaterialHead">Specification Head</label>
                                            <asp:TextBox ID="txtMaterialHead" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" MaxLength="50" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active" for="txtMaterialSpec">Specification Description</label>
                                            <asp:TextBox ID="txtMaterialSpec" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" MaxLength="50" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active" for="txtMaterialRemarks">Material Remarks</label>
                                            <asp:TextBox ID="txtMaterialRemarks" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" MaxLength="50"  TextMode="MultiLine" Rows="4" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active" for="txtItemOrder">Sorting Order #</label>
                                            <asp:TextBox ID="txtItemOrder" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" MaxLength="50" Width="50" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" OnClientClick="return validateForm();" OnClick="btnSave_Click" TabIndex="4" />
                                    <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="5" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <script type="text/javascript">
                        $("input").keypress(function (event) {
                            if (event.which == 13) {
                                event.preventDefault();
                            }
                        });

                        function bindDDLGroupHeadTo(selector) {
                            if ($(selector).val().length >= 3) {
                                jQuery.ajax({
                                    type: "POST",
                                    url: "Product_Specification.aspx/FilterGroup",
                                    data: '{pGroupName:\'' + $(selector).val() + '\'}',
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
                                                return '<div class="autocomplete-suggestion" data-lang="' + item.GroupHead + '" data-val="' + search + '">' + item.GroupHead + '</div>';
                                            },
                                            onSelect: function (e, term, item) {
                                                console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                                $(selector).val(item.html());
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
