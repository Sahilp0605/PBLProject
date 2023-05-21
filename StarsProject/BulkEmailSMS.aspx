<%@ Page Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="BulkEmailSMS.aspx.cs" Inherits="StarsProject.BulkEmailSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/app-file-manager.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <style type="text/css">
        .clDiv70 {
            height: 50px;
            vertical-align: middle;
        }

        .imgChart {
            vertical-align: middle;
            Height: 40px;
            width: 40px;
        }

        #drpSelectDepartment {
            margin: 7px 0px;
            font-family: Verdana,Arial;
            font-size: 12px;
            vertical-align: middle;
        }

        .clTrans {
            background-color: transparent;
        }

        .textureWodden {
            background: url(images/texture_wooden.jpg);
            background-size: cover;
        }

        .textureBubble {
            background: url(images/texture_bubble.jpg);
            background-size: cover;
            color: lightyellow;
        }

        .textureCardboard1 {
            background: url(images/texture_cardboard.jpg);
            background-size: cover;
        }

        .textureCardboard2 {
            background: url(images/texture_cardboard1.jpg);
            background-size: cover;
        }

        .textureGradient {
            background: url(images/texture_gradient1.jpg);
            background-size: cover;
        }

        .textureLight1 {
            background: url(images/texture_light1.jpg);
            background-size: cover;
        }

        .textureLight2 {
            background: url(images/texture_light2.jpg);
            background-size: cover;
        }

        .textureLight3 {
            background: url(images/texture_light3.jpg);
            background-size: cover;
        }

        .texturePaper1 {
            background: url(images/texture_paper.jpg);
            background-size: cover;
        }

        .texturePaper2 {
            background: url(images/texture_paper1.jpg);
            background-size: cover;
        }

        .texturePaper3 {
            background: url(images/texture_paper2.jpg);
            background-size: cover;
        }

        .lightDropdown {
            background-color: lightyellow;
            color: navy;
            font-size: 16px;
            font-weight: 400;
            border-radius: 4px;
            margin-top: 0px;
            padding: 0px 10px;
        }
    </style>

    <script type="text/javascript">
        window.onload = function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "Create & Schedule Campaign";
        }

        //Count Checked        
        function CheckAll() {
            jQuery("#spnSelected").text($("#contentwrapper .chkToCompare").find('input:checked').length);
        }

        //function SendSMS() {
        //    alert("You've clicked the link.");
        //    $.ajax({
        //        type: "POST",
        //        url: "http://sharvayainfotech.com:88/Common/SendSMS",
        //        data: JSON.stringify({ CompanyId:'12' ,mobileno: '7433029545', message: 'bb' }),
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            //location.reload();
        //            alert(response.d);
        //        },
        //    });
        //}
    </script>

    <style type="text/css">
        [type='checkbox']:not(:checked), [type='checkbox']:checked { 
            position:relative !important; 
            opacity:100;
            pointer-events:all;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="smCampaign" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="contentwrapper" class="contentwrapper">
                <div class="row">
                    <div class="input-field col m3">
                        <asp:DropDownList ID="drpCampaign" CssClass="select2-theme browser-default p-0 mr-2 center-align" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="drpCampaign_SelectedIndexChanged">
                            <asp:ListItem Text="Email" Value="Email" />
                            <asp:ListItem Text="SMS" Value="SMS" />
                        </asp:DropDownList>
                    </div>
                    <div class="input-field col m3">
                        <asp:DropDownList ID="drpCampaignFor" CssClass="select2-theme browser-default p-0 mr-2 center-align" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="drpCampaignFor_SelectedIndexChanged">
                            <asp:ListItem Text="Customer" Value="Customer" />
                            <asp:ListItem Text="Employee" Value="Employee" />
                            <asp:ListItem Text="External Leads" Value="External Leads" />
                        </asp:DropDownList>
                    </div>
                    <div class="input-field col m3">
                        <asp:DropDownList ID="drpCustomerType" CssClass="select2-theme browser-default p-0 mr-1 center-align" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="drpCustomerType_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="input-field col m3">
                        <asp:DropDownList ID="drpTemplate" CssClass="select2-theme browser-default p-0 mr-1 center-align" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="drpTemplate_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col m9" style="height: 450px; overflow-y: auto; box-shadow: -6px 8px 6px -6px black; padding-top: 0 2px 2px 2px !important;">
                        <table id="tblSender" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                            <asp:Repeater ID="grdCustomer" runat="server" ClientIDMode="Static" OnItemDataBound="grid_ItemDataBound">
                                <HeaderTemplate>
                                    <thead>
                                        <tr style="background-color: darkslateblue;">
                                            <th class="SelectAll text-center" style="font-weight: 600; width: 4%; color: navy;">Select<br />
                                                <asp:CheckBox CssClass="chkToCompare" ID="chkSelectAll" runat="server" ClientIDMode="Static" />
                                            </th>
                                            <th class="text-center" style="font-weight: 600; width: 30%; color: navy;">Sender Name</th>
                                            <th class="text-center" style="font-weight: 600; width: 30%; color: navy;">Email Address</th>
                                            <th class="text-center" style="font-weight: 600; width: 13%; color: navy;">Contact No</th>
                                            <th class="text-center" style="font-weight: 600; width: 23%; color: navy;">Last Campaign Sent On</th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="blueShed">
                                        <td class="singleCheckbox text-center">
                                            <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" Value='<%# Eval("CustomerID") %>' />
                                            <asp:HiddenField ID="hdnCustomerName" runat="server" ClientIDMode="Static" Value='<%# Eval("CustomerName") %>' />
                                            <asp:HiddenField ID="hdnCustomerMail" runat="server" ClientIDMode="Static" Value='<%# Eval("EmailAddress") %>' />
                                            <asp:HiddenField ID="hdnCustomerContact" runat="server" ClientIDMode="Static" Value='<%# Eval("ContactNo1") %>' />
                                            <asp:CheckBox CssClass="chkToCompare" ID="chkSelect" runat="server" ClientIDMode="Static" onclick="CheckAll()" />
                                        </td>
                                        <td class="text-left"><%#Eval("CustomerName") %></td>
                                        <td class="text-left">
                                            <asp:HyperLink ID="lnkEmail" runat="server" Text='<%# Eval("EmailAddress") %>' NavigateUrl='<%# Eval("EmailAddress", "mailto:{0}") %>' />
                                        </td>
                                        <td class="text-center"><%#Eval("ContactNo1") %></td>
                                        <td class="text-center"><%#Eval("CampaignSentOn") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="grdEmployee" runat="server" ClientIDMode="Static" OnItemDataBound="grid_ItemDataBound">
                                <HeaderTemplate>
                                    <thead>
                                        <tr style="background-color: darkslateblue;">
                                            <th class="singleCheckbox text-center" style="font-weight: 600; width: 5%; color: navy;">Select<br />
                                                <asp:CheckBox type="checkbox" ID="chkSelectAll" runat="server" ClientIDMode="Static" />
                                            </th>
                                            <th class="text-center" style="font-weight: 600; width: 30%; color: navy;">Sender Name</th>
                                            <th class="text-center" style="font-weight: 600; width: 30%; color: navy;">Email Address</th>
                                            <th class="text-center" style="font-weight: 600; width: 15%; color: navy;">Contact No</th>
                                            <th class="text-center" style="font-weight: 600; width: 20%; color: navy;">Last Campaign Sent On</th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                    <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" Value='<%# Eval("EmployeeName") %>' />
                                    <asp:HiddenField ID="hdnEmployeeMail" runat="server" ClientIDMode="Static" Value='<%# Eval("EmailAddress") %>' />
                                    <asp:HiddenField ID="hdnEmployeeContact" runat="server" ClientIDMode="Static" Value='<%# Eval("MobileNo") %>' />
                                    <tr class="blueShed">
                                        <td class="singleCheckbox text-center">
                                            <asp:CheckBox CssClass="chkToCompare" ID="chkSelect" runat="server" ClientIDMode="Static" onclick="CheckAll()" />
                                        </td>
                                        <td class="text-left"><%#Eval("EmployeeName") %></td>
                                        <td class="text-Left">
                                            <asp:HyperLink ID="lnkEmail" runat="server" Text='<%# Eval("EmailAddress") %>' NavigateUrl='<%# Eval("EmailAddress", "mailto:{0}") %>' />
                                        </td>
                                        <td class="text-center"><%#Eval("MobileNo") %></td>
                                        <%--<td class="text-center">&nbsp;</td>--%>
                                        <td class="text-center"><%#Eval("CampaignSentOn") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="grdExternal" runat="server" ClientIDMode="Static" OnItemDataBound="grid_ItemDataBound">
                                <HeaderTemplate>
                                    <thead>
                                        <tr style="background-color: darkslateblue;">
                                            <th class="singleCheckbox text-center" style="font-weight: 600; width: 5%; color: navy;">Select<br />
                                                <asp:CheckBox CssClass="chkToCompare" ID="chkSelectAll" runat="server" ClientIDMode="Static" />
                                            </th>
                                            <th class="text-center" style="font-weight: 600; width: 30%; color: navy;">Sender Name</th>
                                            <th class="text-center" style="font-weight: 600; width: 30%; color: navy;">Sender Email</th>
                                            <th class="text-center" style="font-weight: 600; width: 15%; color: navy;">Contact No</th>
                                            <th class="text-center" style="font-weight: 600; width: 20%; color: navy;">Last Campaign Sent On</th>
                                        </tr>
                                    </thead>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnSenderID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                    <asp:HiddenField ID="hdnSenderName" runat="server" ClientIDMode="Static" Value='<%# Eval("SenderName") %>' />
                                    <asp:HiddenField ID="hdnSenderMail" runat="server" ClientIDMode="Static" Value='<%# Eval("SenderMail") %>' />
                                    <asp:HiddenField ID="hdnSenderContact" runat="server" ClientIDMode="Static" Value='<%# Eval("PrimaryMobileNo") %>' />
                                    <tr class="blueShed">
                                        <td class="singleCheckbox text-center">
                                            <asp:CheckBox CssClass="chkToCompare" ID="chkSelect" runat="server" ClientIDMode="Static" onclick="CheckAll()" />
                                        </td>
                                        <td class="text-Left"><%#Eval("SenderName") %></td>
                                        <td class="text-Left">
                                            <asp:HyperLink ID="lnkEmail" runat="server" Text='<%# Eval("SenderMail") %>' NavigateUrl='<%# Eval("SenderMail", "mailto:{0}") %>' />
                                        </td>
                                        <td class="text-center"><%#Eval("PrimaryMobileNo") %></td>
                                        <td class="text-center">&nbsp;</td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                    <div class="col m3" style="box-shadow: -6px 8px 6px -6px black;">
                        <div class="card bg-info-gradient text-center" style="height: 100%; background: #ced3d4; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#ced3d4),color-stop(1,#89d8e4));">
                            <div class="card-header no-border" style="padding: 5px 10px;">
                                <h5 class="card-title"><i class="fa fa-area-chart ml-2 mr-2"></i><span id="spAnalysis1">Campaign Statistics</span></h5>
                            </div>
                            <div class="card-body" style="padding: 10px !important;">
                                <div class="badge badge-primary mb-2 w-100" style="font-size: 18px; font-weight: 400;">
                                    <span>Total Count</span>
                                    <span id="spnTotal" runat="server" clientidmode="Static" class="badge badge-light" style="min-width: 30px;"></span>
                                </div>
                                <div class="badge badge-primary mb-2 w-100" style="font-size: 18px; font-weight: 400;">
                                    <span>Selected Count</span>
                                    <span id="spnSelected" runat="server" clientidmode="Static" class="badge badge-light" style="min-width: 30px;"></span>
                                </div>
                            </div>
                            <div class="card-footer" style="text-align: center;">
                                <asp:Button CssClass="btn btn-success" ID="btnSendCampaign" runat="server" ClientIDMode="Static" Text="Send Campaign" OnClick="btnSendCampaign_Click"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <script type="text/javascript">
                $("#chkSelectAll").click(function () {
                    $("input[type=checkbox]:enabled").prop('checked', $(this).prop('checked'));
                });
            </script>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>



