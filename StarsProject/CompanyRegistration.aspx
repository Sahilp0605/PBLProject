<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="CompanyRegistration.aspx.cs" Inherits="StarsProject.CompanyRegistration" %>

<%@ Register Src="~/PaggingUserControl.ascx" TagName="page" TagPrefix="uc1" %>

<!DOCTYPE html>

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
        .jqte_editor, .jqte_source {
            min-height: 300px !important;
        }
    </style>


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

    <script type="text/javascript">
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtEmployeeName").val() == "" || jQuery("#drpOrganizationEmp").val() == "" || jQuery("#drpDesignationEmp").val() == "") {

                if (jQuery("#txtEmployeeName").val() == "")
                    errmsg += "Employee Name is required</br>";

                if (jQuery("#drpOrganizationEmp").val() == "")
                    errmsg += "Organization is required</br>";

                if (jQuery("#drpDesignationEmp").val() == "")
                    errmsg += "Designation is required</br>";

                jQuery("#divErrorMessage").html(errmsg).show();

                validflag = false;
            }
            // ----------------------------------------
            if (validflag == true) {
                btnCloseDialog();
            }
            return validflag;
        }

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnPkIDEmp").val('');
        }

        function openCompanyRegistration(pID) {
            $.colorbox({ width: "90%", height: "90%", iframe: true, href: "CompanyRegistrationMgmt.aspx?id=" + pID });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

        <div id="contentwrapper" class="contentwrapper">

            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <div class="clearall"></div>
                        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />


                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="card bg-info-gradient" style="background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
                                <div class="card-header no-border" style="padding: 5px 10px;">
                                    <h5>
                                        <i class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Manage Company Registration</span>

                                        <input type="button" class="btn btn-danger float-right" style="padding: 3px 25px !important;" value="Add New" onclick="javascript: openCompanyRegistration(0);" />
                                        <%--<button ID="btnAddNew" runat="server" ClientIDMode="Static" CssClass="btn btn-danger float-right ml-1 mr-1" style="padding: 3px 25px !important;" Text="Add New" onclick="javascript:openCompanyRegistration();"/>--%>
                                    </h5>

                                </div>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="input-field col m12">
                                        <div class="widgetbox">
                                            <div class="widgetcontent padding0 statement">
                                                <table class="stdtable" cellpadding="0" cellspacing="0" border="0">
                                                    <asp:Repeater ID="rptRegistration" runat="server" OnItemCommand="rptRegistration_ItemCommand" OnItemDataBound="rptRegistration_ItemDataBound">
                                                        <HeaderTemplate>
                                                            <thead>
                                                                <tr>
                                                                    <th>Regno.#</th>
                                                                    <th>Company Name</th>
                                                                    <th>Serial Key</th>
                                                                    <th>Installation Date</th>
                                                                    <th>Expiry Date</th>
                                                                    <th>Max. Users</th>
                                                                    <th>DBIP</th>

                                                                    <th style="text-align: center; width: 4%;">Edit</th>
                                                                </tr>
                                                            </thead>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <%--<asp:HiddenField ID="hdnActiveFlagDesc" runat="server" Value='<%# Eval("ActiveFlagDesc") %>' />--%>

                                                                <td style="width: 5%; text-align: center;"><%# Eval("Regno") %></td>
                                                                <td style="width: 15%;"><%# Eval("CompanyName") %></td>
                                                                <td><%# Eval("SerialKey") %></td>
                                                                <td><%# Eval("InstallationDate","{0:dd/MM/yyyy}") %></td>
                                                                <td><%# Eval("ExpiryDate","{0:dd/MM/yyyy}") %></td>
                                                                <td style="width: 5%;"><%# Eval("NoOfUsers") %></td>
                                                                <td><%# Eval("DBIP") %></td>

                                                                <td style="text-align: center;">
                                                                    <asp:ImageButton ID="ImgbtnEdit" runat="server" ImageUrl="~/images/edit.png" Width="10" Height="16" CommandName="Edit" CommandArgument='<%# Eval("pkId") %>' />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <uc1:page ID="pageGrid" runat="server" Visible="true" OnPagerChanged="Pager_Changed" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
