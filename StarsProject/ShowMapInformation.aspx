<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowMapInformation.aspx.cs" Inherits="StarsProject.ShowMapInformation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Geo Location Information</title>
    <link rel="stylesheet" href="css/style.default.css" type="text/css" />
    <link href="css/Registration.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>

    <script type="text/javascript">
        function lightBox(t2) {
            jQuery.colorbox({ width: "80%", height: "80%", open: true, href: "ShowMapInformation.aspx?PkID=" + t2 });
        };

        function InitializeMap() {
            var latlng = new google.maps.LatLng(-34.397, 150.644);
            var myOptions = {
                zoom: 8,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
        }
        window.onload = InitializeMap;
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pageheader">
        <h1 class="pagetitle contenttitle2">Member Location</h1>
    </div>
    <div id="contentwrapper" class="contentwrapper">
    <div id="basicform" class="subcontent">
        <div style="width:100%; height:80px; padding: 5px 5px 5px 5px;">
            <p class="rollpnum" style="border-top: 1px solid #ddd;">
                <label>Registration No.</label>
                <span class="field spaninptregisr">
                    <asp:TextBox ID="txtRegistrationNo" runat="server" Columns="10" MaxLength="25" TabIndex="1" autocomplete="off" ></asp:TextBox>
                </span>
            </p>
            <span class="frigtimg"><asp:Image ID="imgProfile" runat="server" onClick="lightbox()" /></span>
        </div>

        <div style="width:50%; float:left; position:relative; padding: 0px 5px 0px 0px;">
            <h4 class="subh4title" style="Color:Navy;">&nbsp;&nbsp;Map Information</h4>

            <asp:Panel ID="Panel1" runat="server">
                
                <%--Place holder to fill with javascript by server side code--%>
                <asp:Literal ID="js" runat="server"></asp:Literal>
                
                <%--Place for google to show your MAP--%>
                <div id="map_canvas" style="width: 100%; height: 500px; margin-bottom: 2px; border: 2px solid red;"></div>
            </asp:Panel>       
                  
        </div>

    </div>
</div>  

    <input id="hdnPkID" type="hidden" runat="server" />
    <input id="hdnRegistrationNo" type="hidden" runat="server" />
    <input id="hdnMacID" type="hidden" runat="server" />
    <input id="hdnSimID" type="hidden" runat="server" />
    <input id="hdnLatitude" type="hidden" runat="server" />
    <input id="hdnLatitudeDirection" type="hidden" runat="server" />
    <input id="hdnLongitude" type="hidden" runat="server" />
    <input id="hdnLongitudeDirection" type="hidden" runat="server" />
    
    </form>
</body>
</html>
