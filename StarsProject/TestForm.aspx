<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="TestForm.aspx.cs" Inherits="StarsProject.TestForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function GetMobileQuotation()
        {
            jQuery.ajax({
                type: "POST",
                url: 'Quotation.aspx/GenerateQuotation',
                data: '{pkID:\'' + id + '\'}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var resultdata = JSON.parse(data.d);
                    returnID = resultdata[0].ReturnCode;
                    if (returnID > 0) {
                        var pageUrl = 'Quotation.aspx?id=' + returnID
                        $('#ifrModule').attr('src', pageUrl);
                        $find("mpe").show();
                    }
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
    </form>
</body>
</html>




