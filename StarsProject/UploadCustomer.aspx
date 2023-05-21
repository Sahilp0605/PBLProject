<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="UploadCustomer.aspx.cs" Inherits="StarsProject.UploadCustomer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style= "background: white;">
<head runat="server">
    <title></title>
    <script src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="plugins/jquery/jquery.min.js"></script>
    <style>
        .uploadcustomer {
            height: 200px;
            background-color: ivory;
        }

        .uploadcustomerHead {
            border-radius: 3px 4px 0px 0px;
            background-color: #337AB7;
            padding: 10px;
            vertical-align: central;
            font-size: 20px;
            color: white;
            font-weight: bold;
        }

        .uploadcustomerBody {
            padding-left: 40px;
            padding-top: 15px;
            color: black;
        }
    </style>
</head>
<body>

<%--    <form id="form1" runat="server" class="uploadcustomer">       
                <div class="form-row uploadcustomerHead">
                    <div class="container-fluid">
                        <div class="col m12">
                            <div class="form-row">
                                <asp:Label runat="server" Text="Upload Customer"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-row uploadcustomerBody">
                    <div class="col m12">
                        <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                        <div id="dvFileUpload" class="form-group">
                            <div class="row">
                                <asp:Label runat="server" Text="Upload Customer File "></asp:Label><hr />
                                <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" onchange="this.form.submit()" CssClass="form-control-file" />
                            </div>
                        </div>
                    </div>
                </div>
                <div>
        </div>
    </form>--%>

        <form id="frmEntry" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">
    
        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="widgetbox">
            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
            <div id="myModal" style="display:block; width:100%;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="form-row uploadcustomerHead">
                            <div class="container-fluid">
                                <div class="col m12">
                                    <div class="form-row">
                                        <asp:Label runat="server" Text="Upload Customer"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-row uploadcustomerBody">
                            <div class="col m12">
                                <asp:HiddenField ID="hdnMode" runat="server" ClientIDMode="Static" />
                                <div id="dvFileUpload" class="form-group">
                                    <div class="row">
                                        <asp:Label runat="server" Text="Upload Customer File "></asp:Label><hr />
                                        <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" onchange="this.form.submit()" CssClass="form-control-file" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div></div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" OnClientClick="return validateForm();" OnClick="btnSave_Click" TabIndex="3" />
                        <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="4" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>

</body>
</html>
