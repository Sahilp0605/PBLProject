<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="SearchEmployee.aspx.cs" Inherits="StarsProject.SearchEmployee" EnableEventValidation="false" %>
<%@ Register Src="~/myEmployeeExpnLedger.ascx" TagPrefix="uc1" TagName="myEmployeeExpnLedger" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/datatables/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://code.jquery.com/ui/1.11.2/jquery-ui.min.js" />

    <link href="css/Registration.css" rel="stylesheet" type="text/css" />

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <link href="css/jquery-te-1.4.0.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-te-1.4.0.min.js"></script>
    
    <link href="css/font-awesome-Editor.min.css" rel="stylesheet" />

    <link href="css/richtext.min.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.richtext.min.js"></script>


    <style type="text/css">
        .jqte { margin-top: 5px;}
        .jqte_editor, .jqte_source {
            min-height: 300px !important;
        }
        .required:after {
            content: " *";
            color: #f00;
        } 
        .text-input {
            position: relative;
            margin-top: 50px;
            input [type="text"];
        }

        input[type="text"] + label {
            position: absolute;
            top: 0;
            left: 0;
            bottom: 0;
            height: 40px;
            line-height: 40px;
            color: white;
            border-radius: 3px 0 0 3px;
            padding: 0 20px;
            background: #E03616;
            transform: translateZ(0) translateX(0);
            transition: all 0.3s ease-in;
            transition-delay: 0.2s;
        }

        input[type="text"]:focus + label {
            transform: translateY(-120%) translateX(0%);
            border-radius: 3px;
            transition: all 0.1s ease-out;
        }

        input[type="text"]:focus {
            padding: 10px;
            transition: all 0.3s ease-out;
            transition-delay: 0.2s;
        }

        }

        /* =================================================================================== */
        .row.heading h2 {
            color: #fff;
            font-size: 52.52px;
            line-height: 95px;
            font-weight: 400;
            text-align: center;
            margin: 0 0 40px;
            padding-bottom: 20px;
            text-transform: uppercase;
        }

        ul {
            margin: 0;
            padding: 0;
            list-style: none;
        }

        .heading.heading-icon {
            display: block;
        }

        .padding-lg {
            display: block;
            padding-top: 20px;
            padding-bottom: 20px;
        }

        .practice-area.padding-lg {
            padding-bottom: 55px;
            padding-top: 55px;
        }

        .practice-area .inner {
            border: 1px solid #999999;
            text-align: center;
            margin-bottom: 28px;
            padding: 40px 25px;
        }

        .our-webcoderskull .cnt-block:hover {
            box-shadow: 0px 0px 10px rgba(0,0,0,0.3);
            border: 0;
        }

        .practice-area .inner h3 {
            color: #3c3c3c;
            font-size: 24px;
            font-weight: 500;
            font-family: 'Poppins', sans-serif;
            padding: 10px 0;
        }

        .practice-area .inner p {
            font-size: 14px;
            line-height: 22px;
            font-weight: 400;
        }

        .practice-area .inner img {
            display: inline-block;
        }


        .our-webcoderskull {
            /*background: url("http://www.webcoderskull.com/img/right-sider-banner.png") no-repeat center top / cover;*/
            /*background-color: #e8e8e8;*/
        }

            .our-webcoderskull .cnt-block {
                float: left;
                width: 100%;
                background: #fff;
                padding: 30px 20px;
                text-align: center;
                border: 2px solid #d5d5d5;
                margin: 0 0 28px;
            }

                .our-webcoderskull .cnt-block figure {
                    width: 80px;
                    height: 80px;
                    border-radius: 100%;
                    display: inline-block;
                    margin-bottom: 15px;
                }

                .our-webcoderskull .cnt-block img {
                    width: 80px;
                    height: 80px;
                    border-radius: 100%;
                }

                .our-webcoderskull .cnt-block h3 {
                    color: #2a2a2a;
                    font-size: 20px;
                    font-weight: 500;
                    padding: 6px 0;
                    text-transform: uppercase;
                }

                    .our-webcoderskull .cnt-block h3 a {
                        text-decoration: none;
                        color: #2a2a2a;
                    }

                        .our-webcoderskull .cnt-block h3 a:hover {
                            color: #337ab7;
                        }

                .our-webcoderskull .cnt-block p {
                    color: #2a2a2a;
                    font-size: 13px;
                    line-height: 20px;
                    font-weight: 400;
                }

                .our-webcoderskull .cnt-block .follow-us {
                    margin: 20px 0 0;
                }

                    .our-webcoderskull .cnt-block .follow-us li {
                        display: inline-block;
                        width: auto;
                        margin: 0 5px;
                    }

                        .our-webcoderskull .cnt-block .follow-us li .fa {
                            font-size: 24px;
                            color: #767676;
                        }

                            .our-webcoderskull .cnt-block .follow-us li .fa:hover {
                                color: #025a8e;
                            }

        .flip-card {
            background-color: transparent;
            width: 400px;
            height: 346px;
            margin-bottom: 15px;
            perspective: 1000px;
        }

        .flip-card-inner {
            position: relative;
            width: 100%;
            height: 100%;
            text-align: center;
            transition: transform 0.6s;
            transform-style: preserve-3d;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
        }

        .flip-card:hover .flip-card-inner {
            transform: rotateY(180deg);
        }

        .flip-card-front, .flip-card-back {
            position: absolute;
            width: 100%;
            height: 100%;
            backface-visibility: hidden;
        }

        .flip-card-front {
            background-color: white;
            color: black;
            padding-top: 10px;
            border-radius: 4px;
        }

        .flip-card-back {
            background-color: #2980b9;
            color: white;
            transform: rotateY(180deg);
            border-radius: 4px;
        }

        .nav-tabs .nav-item.show .nav-link, .nav-tabs .nav-link.active {
            color: white;
            background-color: navy;
            border-color: #dee2e6 #dee2e6 #fff;
        }
        th {
            color: #e6e6f1 !important;
            background-color: #4a69d8 !important;
        }
        .nav-tabs {
            border-bottom: 1px solid #234b73;
        }
        .select2-theme { height:2.0rem !important; }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".content").richText();
        });

        function setEditor() {
            $(".content").richText();
        }
    </script>
</head>
<body style="overflow-x: hidden;">
    <form id="form1" runat="server" autocomplete="off">
        <asp:ScriptManager ID="srcSearchEmployee" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnEmpID" runat="server" ClientIDMode="Static" />
        <div id="myModal" style="display: block; width: 100%;">
            <div class="modal-content border-0">
                <div class="modal-header" style="background-color: #9696d6; border-bottom: 4px solid darkgrey;">
                    <div class="row mb-0">
                        <div class="col m4">
                            <label class="active white-text" for="txtEmployeeName" style="font-size: 18px;">Employee Name</label>
                            <asp:TextBox CssClass="white black-text" ID="txtEmployeeName" runat="server" ClientIDMode="Static" class="form-control mb-0" AutoPostBack="true" onKeyup="bindDDLEmployeeTo('#txtEmployeeName')" TabIndex="3" Style="width: 300px;" PlaceHolder="Min.3 Chars To Activate Search" />
                        </div>
                    </div>
                </div>
                <div class="modal-body" style="min-height: 500px; padding: 0px 5px;">
                    <div class="row">
                        <div id="showContacts" class="col m12 mt-2 p-1" style="color: navy; border-top-left-radius: 0.5rem; border-bottom-left-radius: 0.5rem; border-top-right-radius: 0.5rem; border-bottom-right-radius: 0.5rem;">
                            <div id="Contacts-content">
                                <div class="emp-profile" style="padding: 10px;">
                                    <div class="row">
                                        <div class="col m12">
                                            <div class="profile-head">
                                                <ul class="nav nav-tabs" id="myTab" role="tablist" runat="server" clientidmode="Static" enableviewstate="true">
                                                    <li class="nav-item">
                                                        <a class="nav-link active" id="primary-tab" data-toggle="tab" href="#primary" role="tab" aria-controls="home" aria-selected="true">Primary Information</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="leave-tab" data-toggle="tab" href="#leave" role="tab" aria-controls="leave" aria-selected="false">Leave Management</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="todotask-tab" data-toggle="tab" href="#todotask" role="tab" aria-controls="todotask" aria-selected="false">Task Management</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="payroll-tab" data-toggle="tab" href="#payroll" role="tab" aria-controls="payroll" aria-selected="false">Payroll Management</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="attendance-tab" data-toggle="tab" href="#attendance" role="tab" aria-controls="attendance" aria-selected="false">Attendance</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="notifify-tab" data-toggle="tab" href="#notification" role="tab" aria-controls="notification" aria-selected="false">Notification</a>
                                                    </li>
                                                    <li class="nav-item">
                                                        <a class="nav-link" id="expnledger-tab" data-toggle="tab" href="#expnledger" role="tab" aria-controls="expnledger" aria-selected="false">Expn.Ledger</a>
                                                    </li>
                                                </ul>
                                            </div>
                                            <div class="tab-content profile-tab" id="myTabContent">
                                                <div class="tab-pane fade show active" id="primary" role="tabpanel" aria-labelledby="primary-tab">
                                                    <section>
                                                        <ul class="row">
                                                            <li class="col m3">
                                                                <div class="flip-card" style="margin: 20px 0px 5px 0px">
                                                                    <div class="" style="position: relative; width: 100%; height: 100%; text-align: center; transition: transform 0.6s; transform-style: preserve-3d; box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2); box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);">
                                                                        <div class="">
                                                                            <asp:Image ID="lblImage" runat="server" ImageUrl="images/customer.png" Style="margin: 22px 0px 0px 0px;width: 70%; height: 300px;" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="flip-card" style="margin: -360px 0px 0px 440px;">
                                                                    <h5>Department</h5>
                                                                    <table>
                                                                        <tr>
                                                                            <td>Department/Division :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblOrgName" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Designation :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblDesignation" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Report To :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblReportTo" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Joining Date:</td>
                                                                            <td>
                                                                                <asp:Label ID="lblJoiningDate" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Confirmation Date:</td>
                                                                            <td>
                                                                                <asp:Label ID="lblConfirmationDate" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <br />
                                                                    <h5>Contact/Email Details</h5>
                                                                    <table style="width: 100%;">
                                                                        <tr>
                                                                            <td>Mobile # :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblMobile" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Email :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblEmail" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>BirthDate :</td>
                                                                            <td>
                                                                                <asp:Label ID="lblBirthDate" runat="server" ClientIDMode="Static" Font-Size="14px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="flip-card" style="margin: -350px 0px 0px 850px;">
                                                                    <h5>Authorized Signatory</h5>
                                                                    <div class="row">
                                                                        <div class="input-field col m6">
                                                                            <asp:TextBox ID="lblSignatory" Style="margin: -10px 0px 0px 0px; width: 98%; height: 180%;" runat="server" class="form-control" ReadOnly="true" ClientIDMode="Static" TextMode="MultiLine" MaxLength="250" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </li>
                                                        </ul>
                                                    </section>
                                                </div>
                                                <div class="tab-pane fade" id="leave" role="tabpanel" aria-labelledby="leave-tab">
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <asp:DropDownList ID="drpLeaveMonth" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpLeaveMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <asp:DropDownList ID="drpLeaveYear" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpLeaveMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <asp:UpdatePanel ID="updLeaveRequest" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <section class="our-webcoderskull padding-lg" style="height: 405px; overflow-y: auto;">
                                                                <table id="tblLeave" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                                    <asp:Repeater ID="rptLeave" runat="server">
                                                                        <HeaderTemplate>
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="text-center">Period From</th>
                                                                                    <th class="text-center">Period To</th>
                                                                                    <th class="text-center">Reason For Leave</th>
                                                                                    <th class="text-center">Approval Status</th>
                                                                                </tr>
                                                                            </thead>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr class="blueShed">
                                                                                <td class="text-center"><%# Eval("FromDate", "{0:dd/MM/yyyy}") %></td>
                                                                                <td class="text-center"><%# Eval("ToDate", "{0:dd/MM/yyyy}") %></td>
                                                                                <td class="text-center"><%# Eval("ReasonForLeave") %></td>
                                                                                <td class="text-center"><%# Eval("ApprovalStatus") %></td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </table>

                                                            </section>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="drpLeaveMonth" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="drpLeaveYear" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="tab-pane fade" id="todotask" role="tabpanel" aria-labelledby="todotask-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; overflow-y: auto;">
                                                        <table id="tbtodotask" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                            <asp:Repeater ID="rpttodotask" runat="server">
                                                                <HeaderTemplate>
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="text-center">Task Description</th>
                                                                            <th class="text-center">Priority</th>
                                                                            <th class="text-center">Period From</th>
                                                                            <th class="text-center">Period To</th>
                                                                            <th class="text-center">Completion Date</th>
                                                                            <th class="text-center">Status</th>
                                                                            <th class="text-center">Assigned To</th>
                                                                            <th class="text-center">Assigned From</th>
                                                                        </tr>
                                                                    </thead>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="blueShed">
                                                                        <td style="overflow: auto; max-width: 220px; overflow: hidden; word-wrap: break-word; white-space: nowrap;" class="text-center"><%# Eval("TaskDescription") %></td>
                                                                        <td class="text-center"><%# Eval("Priority") %></td>
                                                                        <td class="text-center"><%# Eval("StartDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td class="text-center"><%# Eval("DueDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td class="text-center"><%# Eval("CompletionDate", "{0:dd/MM/yyyy}") %></td>
                                                                        <td class="text-center"><%# Eval("TaskStatus") %></td>
                                                                        <td class="text-center"><%# Eval("EmployeeName") %></td>
                                                                        <td class="text-center"><%# Eval("FromEmployeeName") %></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </table>

                                                    </section>
                                                </div>
                                                <div class="tab-pane fade" id="payroll" role="tabpanel" aria-labelledby="payroll-tab">
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <asp:DropDownList ID="drpPayrollMonth" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpBindPayrollMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <asp:DropDownList ID="drpPayrollYear" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpBindPayrollMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <asp:UpdatePanel ID="updPayroll" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                                <table id="tblPayroll" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                                    <asp:Repeater ID="rptPayroll" runat="server">
                                                                        <HeaderTemplate>
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="text-center">Work.Days</th>
                                                                                    <th class="text-center">Pres.Days</th>
                                                                                    <th class="text-center">Net Pay</th>
                                                                                </tr>
                                                                            </thead>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr class="blueShed">
                                                                                <td class="text-center"><%# Eval("WDays") %></td>
                                                                                <td class="text-center"><%# Eval("PDays") %></td>
                                                                                <td class="text-center"><%# Eval("NetSalary") %></td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </table>
                                                            </section>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="drpPayrollMonth" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="drpPayrollYear" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="tab-pane fade" id="attendance" role="tabpanel" aria-labelledby="attendance-tab">
                                                    <div class="row">
                                                        <div class="input-field col m12">
                                                            <asp:DropDownList ID="drpAttendanceMonth" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpAttendanceMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                        <div class="input-field col m12">
                                                            <asp:DropDownList ID="drpAttendanceYear" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpAttendanceMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <asp:UpdatePanel ID="updAttendance" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                                <table id="tblAttendance" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                                    <asp:Repeater ID="rptAttendance" runat="server" ClientIDMode="Static" OnItemDataBound="rptAttendance_ItemDataBound">
                                                                        <HeaderTemplate>
                                                                            <tr>
                                                                                <th class="center-align">Attendance Date</th>
                                                                                <th class="center-align">Time IN</th>
                                                                                <th class="center-align">Time OUT</th>
                                                                                <th class="center-align">Working Hrs</th>
                                                                                <th class="center-align">Working Status</th>
                                                                                <th class="center-align">Day Status</th>
                                                                            </tr>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                            <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" Value='<%# Eval("EmployeeID") %>' />
                                                                            <asp:HiddenField ID="hdnWorkingTotalHrs" runat="server" ClientIDMode="Static" Value='<%# Eval("WorkingTotalHrs") %>' />
                                                                            <asp:HiddenField ID="hdnWorkingHrsFlag" runat="server" ClientIDMode="Static" Value='<%# Eval("WorkingHrsFlag") %>' />
                                                                            <asp:HiddenField ID="hdnDayStatus" runat="server" ClientIDMode="Static" Value='<%# Eval("DayStatus") %>' />
                                                                            <tr id="trItem" runat="server" class="blueShed">
                                                                                <td class="center-align"><%# Eval("PresenceDate", "{0:dd/MM/yyyy}") %></td>
                                                                                <td class="center-align"><%# Eval("TimeIn") %></td>
                                                                                <td class="center-align"><%# Eval("TimeOut") %></td>
                                                                                <td class="center-align"><%# Eval("WorkingTotalHrs") %></td>
                                                                                <td id="tdWorkingHrsFlag" runat="server" class="center-align"><%# Eval("WorkingHrsFlag") %></td>
                                                                                <td id="tdDayStatus" runat="server" class="center-align"><%# Eval("DayStatus") %></td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>

                                                                </table>
                                                            </section>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="drpAttendanceMonth" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="drpAttendanceYear" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="tab-pane fade" id="notification" role="tabpanel" aria-labelledby="attendance-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: none;">
                                                        <asp:UpdatePanel ID="updNotification" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col m6">
                                                                <table id="tblNotification" class="stdtable" style="width: 100%; font-size: 12px; border: 1px ridge silver;">
                                                                    <asp:Repeater ID="rptNotification" runat="server" OnItemCommand="rptNotification_ItemCommand">
                                                                        <HeaderTemplate>
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="text-center">Description</th>
                                                                                    <th class="text-center">Subject</th>
                                                                                    <th class="text-center">Action</th>
                                                                                </tr>
                                                                            </thead>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr class="blueShed">
                                                                                <td class="text-center"><%# Eval("Description") %></td>
                                                                                <td class="text-center"><%# Eval("Subject") %></td>
                                                                                <td class="text-center">
                                                                                    <asp:ImageButton ID="imgSendNotification" runat="server" ClientIDMode="Static" ImageUrl="images/MailBox.PNG" CommandName="SendNotification" CommandArgument='<%# Eval("TemplateID") %>' Width="40" Height="40" />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </table>
                                                            </div>
                                                            <div class="col m6">
                                                                <div class="row">
                                                                    <div class="input-field col m12">
                                                                        <asp:HiddenField ID="hdnTemplateID" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnPass" runat="server" ClientIDMode="Static" />
                                                                        <label class="active" for="txtFrom">From</label>
                                                                        <asp:TextBox ID="txtFrom" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" ReadOnly="true" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="input-field col m12">
                                                                        <label class="active" for="txtTo">To</label>
                                                                        <asp:TextBox ID="txtTo" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" ReadOnly="true" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="input-field col m12">
                                                                        <label class="active" for="txtCC">CC</label>
                                                                        <asp:TextBox ID="txtCC" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="input-field col m12">
                                                                        <label class="active" for="txtBCC">BCC</label>
                                                                        <asp:TextBox ID="txtBCC" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" ReadOnly="true" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="input-field col m12">
                                                                        <label class="active" for="txtSubject">Subject</label>
                                                                        <asp:TextBox ID="txtSubject" runat="server" class="form-control" MaxLength="20" ClientIDMode="Static" TabIndex="1" placeholder="" />
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="input-field col m12">
                                                                        <label for="txtEditor" class="active required">Email Content</label>
                                                                        <asp:TextBox ID="txtEditor" runat="server" TabIndex="6" class="content" TextMode="MultiLine" placeholder="" EnableViewState="true" ClientIDMode="Static"/>
                                                                    </div>
                                                                </div>
                                                                <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top:10px;"">
                                                                    <button id="btnSaveEmail" type="button" runat="server" clientidmode="Static" Text="Send Email" class="btn cyan right mr-1" onserverclick="btnSaveEmail_Click" TabIndex="9"><i class="material-icons left">email</i>Save - Send Email</button>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="rptNotification" />
                                                        </Triggers>
                                                        </asp:UpdatePanel>
                                                    </section>
                                                </div>
                                                <div class="tab-pane fade" id="expnledger" role="tabpanel" aria-labelledby="attendance-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height: 405px; padding: 10px; overflow-y: auto;">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-3">
                                                                    <label class="active" for="drpExpnMonth" style="font-size: 18px;">Select Month</label>
                                                                    <asp:DropDownList ID="drpExpnMonth" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpExpnMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-3">
                                                                    <label class="active" for="drpExpnYear" style="font-size: 18px;">Select Year</label>
                                                                    <asp:DropDownList ID="drpExpnYear" class="select2-theme browser-default" runat="server" ClientIDMode="Static" Style="font-size: 16px;" AutoPostBack="true" OnSelectedIndexChanged="drpExpnMonthYear_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="row mt-2">
                                                                <div class="col-8">
                                                                    <uc1:myEmployeeExpnLedger runat="server" id="myEmployeeExpnLedger" />
                                                                </div>
                                                                <div class="col-4">
                                                                <div id="divExpnSummary" runat="server" class="flip-card"></div>
                                                            </div>
                                                        </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </section>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:updateprogress id="UpdateProgress1" runat="server" clientidmode="Predictable" viewstatemode="Inherit">    
            <ProgressTemplate>    
                <div class="div1" style="margin-left: 160px">    
                    <img alt="" src="Images/Loaders/ajax-loader.gif" />    
                </div>    
            </ProgressTemplate>    
        </asp:updateprogress>        
        <script type="text/javascript">
            $("input").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    //$("form").submit();
                }
            });
            // -------------------------------------------------------------
            function bindDDLEmployeeTo(selector) {
                if ($(selector).val().length >= 2) {
                    jQuery.ajax({
                        type: "POST",
                        url: "OrgEmployee.aspx/FilterEmployee",
                        data: '{pEmpName:\'' + $(selector).val() + '\'}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            console.log(data);
                            //var resultdata = JSON.parse(data.d);
                            var sample = JSON.parse(data.d);
                            $(selector).autoComplete({
                                minChars: 1,
                                source: function (term, suggest) {
                                    term = term.toLowerCase();
                                    var choices = sample;
                                    suggest(choices);
                                },
                                renderItem: function (item, search) {
                                    $(".autocomplete-suggestion").remove();
                                    //search = search.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
                                    //var re = new RegExp("(" + search.split(' ').join('|') + ")", "gi");
                                    return '<div class="autocomplete-suggestion" data-langname="' + item.pkID + '" data-lang="' + item.EmployeeName + '" data-val="' + search + '">' + item.EmployeeName + '</div>';
                                },
                                onSelect: function (e, term, item) {
                                    console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                    $(selector).val(item.html());
                                    $("#hdnEmpID").val(item.data('langname'));
                                }
                            });

                        },
                        error: function (r) { alert('Error : ' + r.responseText); },
                        failure: function (r) { alert('failure'); }
                    });
                    return false;
                }
                else {
                    if ($(selector).val().length == 0) {
                        $("#hdnEmpID").val('');
                    }
                }
            }

            // -------------------------------------------------------------
            function ShowPDFfile(repFilename) {
                alert(repFilename)
                yhooWin = window.open(repFilename, "ywin", "width=1050,height=750");
                yhooWin.focus();
            }

            function ShowQuotationPreview(id) {
                jQuery.ajax({
                    type: "POST", url: 'Quotation.aspx/GetQuotationNoForPDF', data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8", success: function (data11) {
                        QuotationNoForPDF = (data11.d).replace("/", "-");
                        if (QuotationNoForPDF != "") {
                            ShowPDFfile('PDF/' + QuotationNoForPDF.toString() + '.pdf');
                        }
                        else {
                            alert('Quotation PDF Not Found !')
                        }
                    }
                });
            }

            function ShowSalesOrderPreview(id) {
                jQuery.ajax({
                    type: "POST", url: 'SalesOrder.aspx/GetSalesOrderNoForPDF', data: '{pkID:\'' + id + '\'}',
                    contentType: "application/json; charset=utf-8", success: function (data11) {
                        OrderNoForPDF = (data11.d).replace("/", "-");
                        if (OrderNoForPDF != "")
                            ShowPDFfile('PDF/' + OrderNoForPDF.toString() + '.pdf');
                        else
                            alert('Sales Order PDF Not Found !')
                    }
                });
            }

        </script>
    </form>
</body>
</html>
