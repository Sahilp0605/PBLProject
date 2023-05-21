<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberIDUpload.aspx.cs" Inherits="StarsProject.MemberIDUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Photo ID Upload</title>
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />

    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="js/plugins/jquery.colorbox-min.js" type="text/javascript"></script>
    
    <script src="js/ajax_script.js" type="text/javascript"></script>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="srcRegistration" runat="server"></asp:ScriptManager>
    <div class="pageheader">
        <h1 class="pagetitle contenttitle2">Upload Photo ID</h1>
    </div>
    <asp:FileUpload id="FileUploadControl" runat="server" />
    <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" />
    <br />
    <br />
    <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
    <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
    <asp:HiddenField ID="hdnDriverID" runat="server" />
    <asp:HiddenField ID="hdnImageFrom" runat="server" />
</form>
</body>
</html>
