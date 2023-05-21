<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="StarsProject.Feedback" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png" />
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
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

    <style type="text/css">
        .stdtable th, .stdtable td {
            line-height: 18px;
            vertical-align: middle;
            color: #333;
        }
        /*.stdtable th {background:#b5cfd2 url('/images/cell-blue.jpg'); Color: Navy;}*/
        .stdtable thead th, .stdtable thead td {
            padding: 5px 5px;
            border: 1px solid silver;
            border-bottom: 1px solid gray;
            text-align: left;
        }

        .stdtable thead th {
            background: #b5cfd2 url('/images/cell-blue.jpg');
            Color: Navy;
        }

        .stdtable thead td {
            font-weight: bold;
        }

        .stdtable tbody tr td {
            padding: 3px 5px;
            border-right: 1px solid #eee;
            border-bottom: 1px solid #eee;
            color: #666;
        }

        .stdtable {
            border-left: 1px solid Navy;
            border-top: 1px solid Navy;
            border-right: 1px solid Gray;
            border-bottom: 1px solid Gray;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="widgetbox">
                        <%-- Bootstrap Modal Popup --%>
                        <div class="clearall"></div>

                        <%--<asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />--%>
                        <div id="myModal" style="display: block; width: 100%;">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Manage FeedBack</h5>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <label class="active" for="txtCustomerName">Customer Name</label>
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" MaxLength="1000" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active" for="txtAddress">Address</label>
                                            <asp:TextBox ID="txtAddress" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" TextMode="MultiLine" MaxLength="1000" Rows="3" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col m12" style="margin-bottom: 7px; padding-left: 0px;">
                                            <!-- User Activity List -->
                                            <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                                            <div class="card" style="box-shadow: -6px 8px 6px -6px black;">
                                                <div class="card-header" style="padding: 3px;">
                                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-check-square-o ml-2 mr-2"></i>Over All Review
                                                    </h3>
                                                </div>
                                                <!-- /.card-header -->
                                                <div class="card-body" style="min-height: 150px; max-height: 250px; overflow-y: auto;">
                                                    <table id="tblFeedback" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%" runat="server">
                                                        <%-- <asp:Repeater ID="rptFeedback" runat="server" >
                                                                <HeaderTemplate>--%>
                                                        <thead>
                                                            <tr>
                                                                <th class="text-center">Questions</th>
                                                                <th class="text-center">Excellent</th>
                                                                <th class="text-center">Good</th>
                                                                <th class="text-center">Average</th>
                                                                <th class="text-center">Poor</th>
                                                            </tr>
                                                        </thead>
                                                        <%-- </HeaderTemplate>
                                                                <ItemTemplate>--%>
                                                        <tbody>
                                                            <tr class="blueShed">
                                                                <td class="text-center">Product Satisfaction</td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade1" runat="Server" GroupName="Grade1" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade2" runat="Server" GroupName="Grade1" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade3" runat="Server" GroupName="Grade1" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade4" runat="Server" GroupName="Grade1" /></td>
                                                            </tr>

                                                            <tr class="blueShed">
                                                                <td class="text-center">Sales Executive Presentation</td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade11" runat="Server" GroupName="Grade2" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade22" runat="Server" GroupName="Grade2" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade33" runat="Server" GroupName="Grade2" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade44" runat="Server" GroupName="Grade2" /></td>
                                                            </tr>

                                                            <tr class="blueShed">
                                                                <td class="text-center">Product Features</td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade111" runat="Server" GroupName="Grade3" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade222" runat="Server" GroupName="Grade3" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade333" runat="Server" GroupName="Grade3" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade444" runat="Server" GroupName="Grade3" /></td>
                                                            </tr>

                                                            <tr class="blueShed">
                                                                <td class="text-center">Product Presentation</td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade1111" runat="Server" GroupName="Grade4" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade2222" runat="Server" GroupName="Grade4" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade3333" runat="Server" GroupName="Grade4" /></td>
                                                                <td class="text-center">
                                                                    <asp:RadioButton ID="rdbGrade4444" runat="Server" GroupName="Grade4" /></td>
                                                            </tr>
                                                            <%--</ItemTemplate>
                                                            </asp:Repeater>--%>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="card-footer clearfix">
                                                    <%--<button type="button" class="btn btn-info float-right" onclick="javascript:openDashboardBox('followup',0);"><i class="fa fa-plus"></i> Add FollowUp</button>--%>
                                                    <%--                                <table id="Table1" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="50%" runat="server">
                                                             <tr>
                                                                 <td>--%>
                                                    <label class="active" for="rdblRecommend">Can You Recommend This Product To Others...?</label>
                                                    <%--</td>
                                                                 <td>--%>
                                                    <asp:RadioButtonList class="form-control" ID="rdblRecommend" runat="server" RepeatDirection="Horizontal" TabIndex="6">
                                                        <asp:ListItem Text="Yes" runat="server" Selected="True" Value="1" style="margin-right: 70px"></asp:ListItem>
                                                        <asp:ListItem Text="No" runat="server" Value="2"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <%--</td>
                                                             </tr>
                                                         </table>--%>
                                                </div>
                                            </div>
                                            <!-- /.card -->
                                            <%-- </ContentTemplate>

                                        </asp:UpdatePanel>--%>
                                        </div>
                                    </div>
                                    <%--<div class="row">
                                    <div class="input-field col m">
                                        <table id="Table1" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="50%" runat="server">
                                            <tr>
                                                <td>
                                                    <label class="active" for="rdblRecommend">Can You Recommend This Product To Others...?</label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList class="form-control" ID="rdblRecommend" runat="server" RepeatDirection="Horizontal" TabIndex="6">
                                                        <asp:ListItem Text="Yes" runat="server" Selected="True" Value="1" style="margin-right: 70px"></asp:ListItem>
                                                        <asp:ListItem Text="No" runat="server" Value="2"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>                                   
                                </div>--%>
                                    <div class="row">
                                        <div class="input-field col m">
                                            <label class="active" for="txtComment">Comment</label>
                                            <asp:TextBox ID="txtComment" runat="server" ClientIDMode="Static" class="form-control" TabIndex="1" TextMode="MultiLine" MaxLength="1000" Rows="3" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="input-field col m12">
                                            <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Submit" class="btn btn-primary" OnClientClick="return validateForm();" OnClick="btnSave_Click" TabIndex="4" />
                                    <%--<asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Cancel" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="5" />--%>
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
