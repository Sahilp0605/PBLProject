<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="NagrikConfiguration.aspx.cs" Inherits="StarsProject.NagrikConfiguration" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    
    <link  href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="js/jquery-3.3.1.js"></script>
    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/datatables/jquery.dataTables.min.js"></script>

<%--    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />--%>

    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/plugins/jquery.ui.css" rel="stylesheet" />
    <script type="text/javascript" src="js/plugins/jquery-ui-1.8.16.custom.min.js"></script>

    <style type="text/css">
        .clDiv70 {
            height: 50px;
            vertical-align: middle;
        }
    </style>

    <script type="text/javascript">
    </script>

</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">

    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">

        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="widgetbox">
                    <div class="clearall"></div>
                    <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />

                    <div id="myModal" style="display:block; width:100%;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">FollowUp Information</h5>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="input-field col m">
                                        <label class="form-check-label" for="chkActive">Customer</label>
                                        <div class="form-check">
                                            <asp:CheckBox class="form-check-input" ClientIDMode="Static" ID="chkActive" runat="server" TabIndex="5" />
                                        </div>
                                    </div>
                                    <div class="input-field col m">
                                        <label class="form-check-label" for="chkActive">Active Status</label>
                                        <div class="form-check">
                                            <asp:CheckBox class="form-check-input" ClientIDMode="Static" ID="CheckBox1" runat="server" TabIndex="5" />
                                        </div>
                                    </div>
                                    <div class="input-field col m">
                                        <label class="form-check-label" for="chkActive">Active Status</label>
                                        <div class="form-check">
                                            <asp:CheckBox class="form-check-input" ClientIDMode="Static" ID="CheckBox2" runat="server" TabIndex="5" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row mt-2 badge-secondary" style="border-radius: 5px; padding: 5px;">
                                    <div class="input-field col m12">
                                        <label class="active" for="txtMeetingNotes" style="color:white !important;">Meeting Notes</label>
                                        <asp:TextBox ID="txtMeetingNotes" runat="server" class="form-control" ClientIDMode="Static" TabIndex="4" MaxLength="500" TextMode="MultiLine" Rows="3" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m12">
                                        <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static">
                                            <ul id="wowList" runat="server" class="text-left" clientidmode="Static"></ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" TabIndex="10" OnClick="btnSave_Click" />
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