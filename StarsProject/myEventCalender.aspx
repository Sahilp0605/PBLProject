<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myEventCalender.aspx.cs" Inherits="StarsProject.myEventCalender" %>
<%@ Register Src="~/myCalenderView.ascx" TagPrefix="uc1" TagName="myCalenderView" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .fc td, .fc th { background-color:White; }
        .fc th { height:30px  !important; vertical-align:inherit !important; }
        .fc-view table { border-radius: 4px; }
        .fc td { font-size:18px; font-weight:400; } 
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="div-container" style="margin:10px;">
        <uc1:myCalenderView runat="server" ID="myCalenderView" />    
    </div>
    </form>
</body>
</html>
