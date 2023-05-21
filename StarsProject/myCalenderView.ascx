<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myCalenderView.ascx.cs" Inherits="StarsProject.myCalenderView" %>

<script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
<script src="js/plugins/jquery-ui-1.8.16.custom.min.js"></script>
<link  href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
<script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
<script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
<link href="plugins/fullcalendar/fullcalendar.min.css" rel="stylesheet" />
<script src="plugins/daterangepicker/moment.min.js"></script>
<script src="plugins/fullcalendar/fullcalendar.min.js"></script>


<style type="text/css">
    .fade {
        opacity: 1;
    }

    .bs-popover-auto[x-placement^=top], .bs-popover-top {
        margin-bottom: .5rem;
    }

    .arrow {
        bottom: -17px;
    }

    .popover .arrow {
        position: absolute;
        display: block;
        width: 1rem;
        height: .5rem;
        margin: 0 .3rem;
    }

    .bs-popover-auto[x-placement^=top] .arrow::before, .bs-popover-top .arrow::before {
        bottom: 0;
        border-top-color: rgba(0,0,0,.25);
    }

    .bs-popover-auto[x-placement^=top] .arrow::after, .bs-popover-auto[x-placement^=top] .arrow::before, .bs-popover-top .arrow::after, .bs-popover-top .arrow::before {
        border-width: .5rem .6rem 0;
    }

    .popover .arrow::after, .popover .arrow::before {
        position: absolute;
        display: block;
        content: "";
        border-color: transparent;
        border-style: solid;
    }

    .bs-popover-auto[x-placement^=top] .arrow::after, .bs-popover-top .arrow::after {
        bottom: 1px;
        border-top-color: #fff;
    }

    .popover-header {
        padding: 0.7rem 0.75rem;
        margin-bottom: 0;
        font-size: 1.6rem;
        color: inherit;
        background-color: #eaeaea;
        border-bottom: 1px solid #ebebeb;
        margin-top: 0px;
    }

    .popover-body {
        padding: .5rem .75rem;
        color: #212529;
    }

    *, ::after, ::before {
        box-sizing: border-box;
    }

    .bs-popover-top {
        margin-bottom: .5rem;
    }

    .popover {
        position: absolute;
        top: 0;
        left: 0;
        z-index: 1060;
        padding: 1px;
        font-size: 14px;
        box-shadow: 0 8px 10px rgba(0,0,0,.2);
        display: block;
        max-width: 276px;
        font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol";
        font-style: normal;
        font-weight: 400;
        line-height: 1.5;
        text-align: left;
        text-align: start;
        text-decoration: none;
        text-shadow: none;
        text-transform: none;
        letter-spacing: normal;
        word-break: normal;
        word-spacing: normal;
        white-space: normal;
        line-break: auto;
        word-wrap: break-word;
        background-color: #fff;
        background-clip: padding-box;
        border: 1px solid rgba(0, 0, 0, 0.15);
        border-radius: .3rem;
    }

    .Holiday {
        opacity: 0.7;
        background-color: transparent;
    }

    .fc-other-month .fc-day-number .fc-today-button .fc-prev-button .fc-next-button {
        display: none !important;
    }

    .fc-toolbar { background-color: navy; color: white !important; padding-top: 5px; border-radius: 6px; }
    .fc-toolbar h2 { color: white; }
    .fc-day-grid-container {
        height: 435px !important;
    }
    h2 { font-size:24px; }
</style>

<div id="contentwrapper" class="contentwrapper">
    <div class="widgetbox">
        <div id="myModal" style="display: block; width: 100%;">
            <div class="modal-content">
                <div class="modal-body p-2">
                    <div class="row" style="padding: 10px 10px 5px 10px; background-color: beige; border: 1px solid gray; border-radius: 5px; margin: 3px 3px 15px 3px;">
                        <div class="col-md-6">
                            <label class="active required" for="drpEmployee" style="font-weight:500; font-size: 18px;">Employee</label>
                            <asp:DropDownList ID="drpEmployee" runat="server" ClientIDMode="Static" class="select2-theme browser-default" TabIndex="19" Style="height: inherit; width: 250px; font-size: 18px;"></asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <label class="active required" for="drpSummaryYear" style="font-weight:500; font-size: 18px;">Year</label>
                            <asp:DropDownList ID="drpSummaryYear" CssClass="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="padding: 0px; width: 120px; font-size: 18px;"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                            <label class="active required" for="drpSummaryMonth" style="font-weight:500; font-size: 18px;">Month</label>
                            <asp:DropDownList ID="drpSummaryMonth" CssClass="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="padding: 0px; width: 120px; font-size: 18px; margin-right: 10px;"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-10">
                            <div id="calendar" class="width-100 height-100vh"></div>
                        </div>
                        <div class="col-md-2">
                            <h5 style="background-color: navy; color: white; min-height: 38px; padding: 5px; text-align: center; border-radius:5px; margin-bottom:10px;">Activity Legend</h5>
                            <label style="width:100%; background-color: #4a148c; color: white; border-radius: 2px;">&nbsp Followup &nbsp </label>
                            <label style="width:100%; background-color: #4caf50; color: white; border-radius: 2px;">&nbsp Payment &nbsp </label>
                            <label style="width:100%; background-color: #ff5722; color: white; border-radius: 2px;">&nbsp Leave &nbsp </label>
                            <label style="width:100%; background-color: #ffc107; color: white; border-radius: 2px;">&nbsp Holiday &nbsp </label>
                            <label style="width:100%; background-color: #e91e63; color: white; border-radius: 2px;">&nbsp Birthday &nbsp </label>
                            <label style="width:100%; background-color: #007bff; color: white; border-radius: 2px;">&nbsp Task Reminder &nbsp </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="eventContent" title="Event Details" style="background-color: #e0caca; border-radius: 8px; color: navy; display: none;">
    <span id="startTime1"></span><span id="startTime2"></span>
    <br>
    <span id="endTime1"></span><span id="endTime2"></span>
    <br>
    <p id="eventInfo"></p>
</div>
<div>
</div>
<script type="text/javascript">
    var Cal;
    var m11 = parseInt($("#drpSummaryMonth").val());
    var y11 = parseInt($("#drpSummaryYear").val());
    var eid = parseInt($("#drpEmployee").val());
    // --------------------------------------------------------------------------
    loadCalendar(m11, y11, eid);
    // --------------------------------------------------------------------------
    function loadCalendar(m1, y1, eid1) {
        jQuery(function () {
            var eventList;
            $("#calendar").fullCalendar('removeEvents');
            jQuery.ajax({
                type: "POST",
                url: 'Services/NagrikService.asmx/GetCalenderListByEmployee',
                data: '{pMonth: ' + (m1 + 1) + ', pYear: ' + y1 + ',pEmployeeID:' + eid1 + '}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    eventList = JSON.parse(data.d);
                    Cal = jQuery('#calendar').fullCalendar({
                        height: 500,
                        width: 900,
                        header: { left: '', center: 'title', right: '' },
                        events: jQuery.map(eventList, function (item, i) {
                            //alert(item.StartDate)
                            var event = new Object();
                            event.id = item.pkID;
                            event.start = item.StartDate;
                            event.end = item.EndDate;
                            if (item.className == 'Followup') {
                                event.title = item.Title;
                            }
                            else if (item.className == 'Leave') {
                                event.title = item.EmployeeName;
                            }
                            else if (item.className == 'Payment') {
                                event.title = item.Title;
                            }
                            else if (item.className == 'Birthday') {
                                event.title = item.EmployeeName;
                            }
                            else {
                                event.title = item.Title;
                                event.url = item.ImageUrl;
                            }
                            event.allDay = ((item.className == 'Leave' && item.StartDate == item.EndDate) || item.className == 'Followup') ? true : false;
                            event.className = item.className;

                            $('#calendar').fullCalendar('renderEvent', event, true);
                            return event;
                        }),
                        eventRender: function (event, eventElement) {
                            if (event.className == 'Followup') {
                                eventElement.css('background-color', '#4a148c');
                                eventElement.css('color', 'White');
                                eventElement.css('font-family', 'Arial, Helvetica Neue, Helvetica, sans-serif');
                                eventElement.css('font-size', '.85em');
                            }
                            else if (event.className == 'Leave') {
                                eventElement.css('background-color', '#ff5722');
                                eventElement.css('color', 'White');
                                eventElement.css('font-family', 'Arial, Helvetica Neue, Helvetica, sans-serif');
                                eventElement.css('font-size', '.85em');
                            }
                            else if (event.className == 'Holiday') {
                                eventElement.css('background-color', '#ffc107');
                                eventElement.css('color', 'Navy');
                                eventElement.css('font-family', 'Arial, Helvetica Neue, Helvetica, sans-serif');
                                eventElement.css('font-size', '.85em');
                                eventElement.css("height", "200px");
                                eventElement.css('background-image', "URL(" + event.url + ")");
                                eventElement.css('background-size', '150px 75px');
                                eventElement.css('background-position', '50% 60%');
                                eventElement.css("margin-top", "-25px");
                            }
                            else if (event.className == 'Birthday') {
                                eventElement.css('background-color', '#e91e63');
                                eventElement.css('color', 'White');
                                eventElement.css('font-family', 'Arial, Helvetica Neue, Helvetica, sans-serif');
                                eventElement.css('font-size', '.85em');
                            }
                            else if (event.className == 'Payment') {
                                eventElement.css('background-color', '#4caf50');
                                eventElement.css('color', 'White');
                                eventElement.css('font-family', 'Arial, Helvetica Neue, Helvetica, sans-serif');
                                eventElement.css('font-size', '.85em');
                            }
                            else if (event.className == 'Reminder') {
                                eventElement.css('background-color', '#007bff');
                                eventElement.css('color', 'White');
                                eventElement.css('font-family', 'Arial, Helvetica Neue, Helvetica, sans-serif');
                                eventElement.css('font-size', '.85em');
                            }
                        },
                        eventLimit: true, // for all non-TimeGrid views
                        views: { timeGrid: { eventLimit: 2 } },
                        editable: false,
                        droppable: false,
                        displayEventTime: false,
                        defaultView: 'month'
                        //})
                    })
                },
                error: function (r) { alert('Error : ' + r.responseText); },
                failure: function (r) { alert('failure'); }
            });

        });
    }

    function ClearCalender() {
        calendar.fullCalendar('removeEvents', function (e) {
            return true;
        });
    }
    // --------------------------------------------------------------------------
    $("#drpSummaryMonth").change(function () {
        //ClearCalender();
        var newDate = new Date();
        var m21 = parseInt($("#drpSummaryMonth").val());
        var y21 = parseInt($("#drpSummaryYear").val());
        var eid21 = parseInt($("#drpEmployee").val());
        newDate.setYear(y21);
        newDate.setMonth(m21);
        newDate.setDate(1);
        Cal.fullCalendar("gotoDate", newDate);
        loadCalendar(m21, y21, eid21);
    });

    $("#drpSummaryYear").change(function () {
        //ClearCalender();
        var newDate = new Date();
        var m21 = parseInt($("#drpSummaryMonth").val());
        var y21 = parseInt($("#drpSummaryYear").val());
        var eid21 = parseInt($("#drpEmployee").val());
        newDate.setYear(y21);
        newDate.setMonth(m21);
        newDate.setDate(1);
        Cal.fullCalendar("gotoDate", newDate);
        loadCalendar(m21, y21, eid21);
    });

    $("#drpEmployee").change(function () {
        //ClearCalender();
        var newDate = new Date();
        var m21 = parseInt($("#drpSummaryMonth").val());
        var y21 = parseInt($("#drpSummaryYear").val());
        var eid21 = parseInt($("#drpEmployee").val());
        newDate.setYear(y21);
        newDate.setMonth(m21);
        newDate.setDate(1);
        Cal.fullCalendar("gotoDate", newDate);
        loadCalendar(m21, y21, eid21);
    });

</script>
