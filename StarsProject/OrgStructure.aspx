<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="OrgStructure.aspx.cs" Inherits="StarsProject.OrgStructure" %>
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
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>

    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {

        });
        /*---------------------------------------------------------*/
        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("hdnpkID").val('');
            jQuery("#hdnOrgCodeEmp").val('');
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
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnOrgType" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnOrgCodeEmp" runat="server" ClientIDMode="Static" />

                <div id="myModal" style="display:block; width:100%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row padding-2">
                                <div class="col m6" style="min-height:400px; box-shadow: navy -6px 8px 6px -6px;">
                                    <h5 class="card-title padding-2 mb-2 float-left center-align width-100 gradient-45deg-light-blue-indigo">Define Organization / Department / Division</h5>
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <asp:TextBox ID="txtOrgCode" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " MaxLength="20" TabIndex="1" />
                                            <label class="active" for="txtOrgCode">Organization Code <span class="materialize-red-text font-weight-800">*</span></label>
                                        </div>
                                        <div class="input-field col m8">
                                            <asp:TextBox ID="txtOrgName" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder=" " MaxLength="50" TabIndex="2" />
                                            <label class="active" for="txtOrgName">Organization Name <span class="materialize-red-text font-weight-800">*</span></label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m4">
                                            <asp:DropDownList ID="drpOrgType" runat="server" ClientIDMode="Static" class="select2-theme browser-default"  TabIndex="3" AutoPostBack ="true"/>
                                            <label class="active" for="drpOrgType">Type <span class="materialize-red-text font-weight-800">*</span></label>
                                        </div>
                                        <div class="input-field col m8">
                                            <asp:DropDownList ID="drpReportTo" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="4"  />
                                            <label class="active" for="drpReportTo">Report To <span class="materialize-red-text font-weight-800">*</span></label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <asp:TextBox ID="txtLandline1" runat="server" ClientIDMode="Static" class="form-control"  placeholder=" " MaxLength="15" TabIndex="9"></asp:TextBox>
                                            <label class="active" for="txtLandline1">Landline</label>
                                        </div>
                                        <div class="input-field col m6">
                                            <asp:TextBox ID="txtFax1" runat="server" ClientIDMode="Static" class="form-control"  placeholder=" " TabIndex="10" MaxLength="100"></asp:TextBox>
                                            <label class="active" for="txtFax1">Fax</label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m8">
                                            <asp:TextBox ID="txtEmailAddress" runat="server" ClientIDMode="Static" class="form-control" placeholder=" " MaxLength="50" TabIndex="11"></asp:TextBox>
                                            <label class="active" for="txtEmailAddress">Email Address</label>
                                        </div>
                                        <div class="input-field col m4">
                                            <label class="active" for="drpOrgHead">Organisation Head</label>
                                            <asp:DropDownList ID="drpOrgHead" runat="server" class="select2-theme browser-default" ClientIDMode="Static" TabIndex="7"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="raw" >
                                        <div class="col m6">
                                            <h5 class="card-title padding-2 mb-2 float-left center-align width-100 gradient-45deg-light-blue-indigo">Define Location Information</h5>
                                            <div class="row">
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtAddress" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" MaxLength="200" TabIndex="6" TextMode="MultiLine" height="100px"></asp:TextBox>
                                                    <label class="active" for="txtAddress">Address</label>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="input-field col m6">
                                                    <asp:DropDownList ID="drpCity" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="7"></asp:DropDownList>
                                                    <label class="active" for="drpCity">City</label>
                                                </div>
                                                <div class="input-field col m6">
                                                    <asp:TextBox ID="txtPincode" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" MaxLength="15" TabIndex="8"></asp:TextBox>
                                                    <label class="active" for="txtPincode">Pincode</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m6">
                                            <% if (hdnOrgType.Value == "1")
                                            {%> 
                                            <div class="col m12" id="gstDetails">
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtGSTNo" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" MaxLength="15" TabIndex="8" AutoPostBack="true" OnTextChanged="txtGSTNo_TextChanged"></asp:TextBox>
                                                    <label class="active" for="txtGSTNo">GST No</label>
                                                </div>
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtPANNo" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" MaxLength="15" TabIndex="8"></asp:TextBox>
                                                    <label class="active" for="txtPANNo">PAN No</label>
                                                </div>
                                                <div class="input-field col m12">
                                                    <asp:TextBox ID="txtCINNo" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" TabIndex="8"></asp:TextBox>
                                                    <label class="active" for="txtCINNo">CIN No</label>
                                                </div>
                                            </div>
                                            <%} %>
                                            <div class="input-field col m12 pt-1">
                                                <label class="active" for="chkBlock">Active Status</label>
                                                <div class = "switch">
                                                    <label>Inactive<asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" Checked="true" TabIndex="5"/>
                                                    <span class="lever"></span>Active</label>
                                                </div> 
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col m6" style="min-height:400px; box-shadow: navy -6px 8px 6px -6px;">
                                    <table id="tblOrgEmployee" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <asp:Repeater ID="rptOrgEmployee" runat="server">
                                        <HeaderTemplate>
                                            <thead>
                                                <tr>
                                                    <th class="center-align">Employee Name</th>
                                                    <th class="center-align">Mobile #</th>
                                                    <th class="center-align">Email Address</th>
                                                    <th class="center-align">Report To</th>
                                                </tr>
                                            </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="blueShed">
                                                <td class="center-align"><%# Eval("EmployeeName") %></td>
                                                <td class="center-align"><%# Eval("MobileNo") %></td>
                                                <td class="center-align"><%# Eval("EmailAddress") %></td>
                                                <td class="center-align"><%# Eval("ReportToEmployeeName") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    </table>
                                </div>
                               <%-- <div class="col m6 hide" style="min-height:400px; box-shadow: navy -6px 8px 6px -6px;">
                                    <h5 class="card-title padding-2 mb-2 float-left center-align width-100 gradient-45deg-light-blue-indigo">Define Location Information</h5>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <asp:TextBox ID="txtAddress" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" MaxLength="200" TabIndex="6" TextMode="MultiLine" height="100px"></asp:TextBox>
                                            <label class="active" for="txtAddress">Address</label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m6">
                                            <asp:DropDownList ID="drpCity" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="7"></asp:DropDownList>
                                            <label class="active" for="drpCity">City</label>
                                        </div>
                                        <div class="input-field col m6">
                                            <asp:TextBox ID="txtPincode" runat="server" ClientIDMode="Static" class="form-control" PlaceHolder="" MaxLength="15" TabIndex="8"></asp:TextBox>
                                            <label class="active" for="txtPincode">Pincode</label>
                                        </div>
                                    </div>

                                </div>--%>
                            </div>
                            
                        </div>
                    </div>
                </div>
                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                    <button ID="btnReset" type="button" runat="server" Text="Reset" class="btn orange left" onserverclick="btnReset_Click" TabIndex="12"><i class="material-icons left">backspace</i>Clear & Add New</button>
                    <button ID="btnSave" type="button" runat="server" Text="Save" class="btn cyan right mr-1" onserverclick="btnSave_Click" TabIndex="13"><i class="material-icons left">save</i>Save</button>
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
