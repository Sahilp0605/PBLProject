<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrackEmployee.aspx.cs" Inherits="StarsProject.TrackEmployee" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/map-icons.min.css" rel="stylesheet" />
    <script src="js/map-icons.min.js"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBWqvtEGzYip415ZSKe5kfeCiuHyr1kbQQ"></script>
    <link  href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    
    <script type="text/javascript">
        var map;
        var mapOptions;
        var togl = 1;
        var markersRes = [];
        var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        var labelIndex = 0;
        // ***************************************************************************************
<%--        function initMap() {
            //var markers = JSON.parse('<%= ConvertDataTabletoString() %>');
            var varMapType = "roadmap";

            mapOptions = {
                center: new google.maps.LatLng(markers[0].Latitude, markers[0].Longitude),
                zoom: 13,
                mapTypeId: "roadmap",
                panControl: true,
                mapTypeControl: false,
                panControlOptions: { position: google.maps.ControlPosition.RIGHT_CENTER },
                zoomControl: true,
                zoomControlOptions: { style: google.maps.ZoomControlStyle.LARGE, position: google.maps.ControlPosition.RIGHT_CENTER },
                scaleControl: false,
                streetViewControl: false,
                streetViewControlOptions: { position: google.maps.ControlPosition.RIGHT_CENTER }
            };

            var infoWindow = new google.maps.InfoWindow();
            map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
            var imageVar;

            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var myLatlng = new google.maps.LatLng(data.Latitude, data.Longitude);

                if (data.LogStatus == 'Initiated') imageVar = markerInitiated;
                if (data.LogStatus == 'Dispatched') imageVar = markerDispatched;
                if (data.LogStatus == 'Accepted') imageVar = markerAccepted;
                if (data.LogStatus == 'Rejected') imageVar = markerRejected;
                if (data.LogStatus == 'Closed') imageVar = markerClosed;

                var marker = new google.maps.Marker({ position: myLatlng, map: map, title: data.title, icon: imageVar });
                //var markerInit = new StyledMarker({ styleIcon: new StyledIcon(StyledIconTypes.MARKER, { color: "Red", text: "I" }), position: myLatLng, map: map, title: data.title });
                //var markerDisp = new StyledMarker({ styleIcon: new StyledIcon(StyledIconTypes.MARKER, { color: "#63B8FF", text: "D" }), position: myLatLng, map: map, title: data.title });
                //var markerClose = new StyledMarker({ styleIcon: new StyledIcon(StyledIconTypes.MARKER, { color: "#808080", text: "C" }), position: myLatLng, map: map, title: data.title });
                //var markerAccept = new StyledMarker({ styleIcon: new StyledIcon(StyledIconTypes.MARKER, { color: "#008000", text: "A" }), position: myLatLng, map: map, title: data.title });
                //var markerReject = new StyledMarker({ styleIcon: new StyledIcon(StyledIconTypes.MARKER, { color: "#FFA500", text: "R" }), position: myLatLng, map: map, title: data.title });
                // ==================================================================================================
                // Add the circle for this city to the map.
                // ==================================================================================================
                //var defCircle = { strokeColor: '#FF0000', strokeOpacity: 0.8, strokeWeight: 2, fillColor: '#FF0000', fillOpacity: 0.35, map: map,
                //    center: myLatlng, radius: 50
                //};

                //cityCircle = new google.maps.Circle(defCircle);
                // ---------------------------------------------------
                (function (marker, data) {
                    var tmpDescription = '<div class="accordion"><font color="navy">HelpLog Ticket # : </font>' + data.HelpLogID +
                                         '&nbsp;&nbsp;&nbsp;<font color="navy">Date/Time : </font>' + data.LogDateTime + '&nbsp;&nbsp;</div>' +
                                         '<table class="stdtable" cellpadding="0" cellspacing="0" border="0">' +
                                         '<tr><td>Member Name</td><td>' + data.MemberName + '</td></tr>' +
                                         '<tr><td>Driver Name</td><td>' + data.DriverName + '</td></tr>' +
                                         '<tr><td>Latitude</td><td>' + data.Latitude + '</td></tr>' +
                                         '<tr><td>Longitude</td><td>' + data.Longitude + '</td></tr>' +
                                         '</table>';

                    google.maps.event.addListener(marker, "click", function (e) { infoWindow.setContent(tmpDescription); infoWindow.open(map, marker); });
                })(marker, data);
            }
        }--%>


        // ------------------------------------------------------------------------
        // Functionality : On Timer Click  ... Display All Response Team Location
        // ------------------------------------------------------------------------
        function initTackMap() {
            labelIndex = 0;
            // ----------------------------------------
            mapOptions = {
                center: new google.maps.LatLng(23.013006, 72.522269),
                zoom: 13,
                mapTypeId: "roadmap",
                panControl: true,
                mapTypeControl: false,
                panControlOptions: { position: google.maps.ControlPosition.RIGHT_CENTER },
                zoomControl: true,
                zoomControlOptions: { style: google.maps.ZoomControlStyle.LARGE, position: google.maps.ControlPosition.RIGHT_CENTER },
                scaleControl: false,
                streetViewControl: false,
                streetViewControlOptions: { position: google.maps.ControlPosition.RIGHT_CENTER }
            };
            map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
            // ---------------------------------------------
            //ClearResponseTeamMarker();
            // ---------------------------------------------
            var empid = jQuery("#drpEmployee").val();
            var fdt = jQuery("#txtFollowupDate").val();
            var tdt = jQuery("#txtFollowupDate").val();
            //alert(empid)
            //alert(fdt)
            //alert(tdt)
            jQuery.ajax({
                url: "/Services/NagrikService.asmx/getResponseTeamLocation",
                data: '{pEmployeeID: ' + empid + ', pDate1: \'' + fdt + '\', pDate2:\'' + tdt + '\'}',
                type: "post",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    var markersRes1 = JSON.parse(data.d);
                    var infoWindow = new google.maps.InfoWindow();
                    // ----------------------------------------------
                    for (i = 0; i < markersRes1.length; i++) {
                        var data = markersRes1[i];
                        var myLatlng1 = new google.maps.LatLng(data.Latitude, data.Longitude);
                        var marker1 = new google.maps.Marker({ position: myLatlng1, map: map, title: data.title, label: labels[labelIndex++ % labels.length], });
                        
                        (function (marker1, data) {
                            var date1 = new Date(parseInt(data.FollowUpDate.substr(6, data.FollowUpDate.length - 8)));
                            var d1 = (date1.getDate() + "/" + (date1.getMonth() + 1) + "/" + date1.getFullYear());
                            
                            var tmpDescription = '<div class="accordion">' +
                                                 '<table class="stdtable" cellpadding="5" cellspacing="5" border="0" style="background-color:Navy; font-size:18px;">' +
                                                 '<tr><td style="width:150px; color:White; text-align:right;">Customer Name</td><td style="text-align:left;width:250px; color:limegreen;">' + data.CustomerName + '</td></tr>' +
                                                 '<tr><td style="width:150px; color:White; text-align:right;">Employee Name </td><td style="text-align:left;width:250px; color:limegreen;">' + data.EmployeeName + '</td></tr>' +
                                                 '<tr><td style="width:150px; color:White; text-align:right;">Inquiry #</td><td style="text-align:left;width:250px; color:limegreen;">' + data.InquiryNo + '</td></tr>' +
                                                 '<tr><td style="width:150px; color:White; text-align:right;">FollowUp.Date</td><td style="text-align:left;width:250px; color:limegreen;">' + d1 + '</td></tr>' +
                                                 '</table></div>';
                            
                            google.maps.event.addListener(marker1, "click", function (e) { infoWindow.setContent(tmpDescription); infoWindow.open(map, marker1); });
                        })(marker1, data);
                        markersRes.push(marker1);
                    }
                },
                error: function (r) {
                    alert('Error : ' + r.responseText);
                },
                failure: function (r) {
                    alert('failure');
                }
            });

        }

        function ClearResponseTeamMarker() {
            var k = 0;
            for (k = 0; k < markersRes.length; k++) {
                if (markersRes[k] != null) {
                    markersRes[k].setMap(null);
                }
            }
            markersRes = [];
        };
        // ------------------------------------------------------
        // google.maps.event.addDomListener(window, "load", initTackMap);
    </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div id="myModal" style="display:block; width:100%;">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="row" style="width:100%; background-color:navy; color:white; padding:10px;">
                        <div class="input-field col m5 mb-0">
                            <div class="input-group">
                                <label for="drpEmployee" class="input-group-addon pt-2">Select Employee</label>&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="drpEmployee" runat="server"  ClientIDMode="Static" class="form-control" style="max-width:250px;" TabIndex="7" />
                            </div>
                        </div>
                        <div class="input-field col m5 mb-0">
                            <div class="input-group">
                                <label for="drpEmployee" class="input-group-addon pt-2">Followup Date: </label>&nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="txtFollowupDate" runat="server" ClientIDMode="Static" class="form-control"  style="max-width:250px;" TextMode="Date" TabIndex="9"/>
                            </div>
                        </div>
                        <div class="input-field col m2 mb-0">
                            <asp:Button class="btn btn-info float-right" ID="btnLoad" runat="server" ClientIDMode="Static" Text="Load Activity" OnClick="btnLoad_Click" OnClientClick="javascript:initTackMap();" />
                        </div>
                    </div>
                    
                </div>
                <div class="modal-body" style="min-height:500px;">
                    <div class="row">
                        <div class="input-field col m8">
                            <div id="divMapInfo">
                                <div id="map_canvas" runat="server" enableviewstate="true" clientidmode="Static" style="border:5px solid silver; text-align: center; min-height:400px;" ></div>
                            </div>
                        </div>
                        <div class="input-field col m4" style="border:5px solid silver;height:400px; overflow-y:auto;">
                            <asp:Repeater ID="rptFollowup" runat="server" OnItemDataBound="rptFollowup_ItemDataBound">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnNextFollowup" runat="server" ClientIDMode="Static" Value='<%# Eval("NextFollowupDate") %>' />
                                    <asp:HiddenField ID="hdnLatitude" runat="server" ClientIDMode="Static" Value='<%# Eval("Latitude") %>' />
                                    <ul class="todo-list" style="list-style:none; padding:5px !important;">
                                        <li id="liFollowupCard" runat="server" style="background-color:#f4f4f4; border-left: 2px solid #d6c7c7;padding: 5px 0px 5px 10px;border-bottom: 4px ridge silver;border-right: 2px solid #d6c7c7;border-top: 4px ridge silver;">
                                            <span class="handle">
                                                <i class="fa fa-ellipsis-v"></i>
                                                <i class="fa fa-ellipsis-v"></i>
                                                <span class="text" style="color:black; padding-left:5px; font-size:12px; font-weight:600;  text-transform:capitalize;">
                                                    <a href="javascript:openTimeline('followup','<%# Eval("CustomerID") %>','<%# Eval("CustomerName") %>');"><%# Eval("CustomerName") %></a>
                                                </span>
                                                <small class="float-right badge badge-danger mr-1"><i class="fa fa-clock-o"></i>  <%# Eval("InquiryStatus") %> </small>
                                            </span><br />
                                            <span class="text" style="color:navy; font-size:12px; font-weight:400;"><%# Eval("MeetingNotes") %></span>
                                            
                                            <div class="row mt-2">
                                                <div class="col m5" style="margin-right:5px !important;">
                                                    <small class="badge badge-primary"><i class="fa fa-calendar-check-o"></i><span> Follo.Date: </span><%# Eval("FollowupDate", "{0:dd-MM-yy}") %> </small>
                                                </div>
                                                <div class="col m" style="margin-right:5px !important; text-align:right;">
                                                    <small class="badge badge-secondary"><i class="fa fa-calendar-check-o"></i><span> Next Foll.Dt: </span>
                                                        <span ID="ltrNextFollowup" runat="server" clientidmode="Static"><%# Eval("NextFollowupDate", "{0:dd-MM-yy}") %></span>
                                                    </small>
                                                </div>
                                            </div>
                                            <div class="row" style="padding-left: 15px; padding-right: 20px;">
                                                <div class="badge badge-info col m12">
                                                    <small><%# Eval("EmployeeName") %> - <%# Eval("CreatedDate", "{0:dd-MM-yy hh:mi:ss}") %></small>
                                                </div>
                                            </div>
                                        </li>
                                    </ul>
                                </ItemTemplate>
                            </asp:Repeater>     
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
