<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ManageRole.aspx.cs" Inherits="StarsProject.ManageRole" %>

<%@ Register Src="~/PaggingUserControl.ascx" TagName="page" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.4.1.js"></script>

    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />

    <link type="text/css" href="http://cdn.kendostatic.com/2013.2.918/styles/kendo.common.min.css" rel="stylesheet" />
    <link type="text/css" href="http://cdn.kendostatic.com/2013.2.918/styles/kendo.default.min.css" rel="stylesheet" />
    <script type="text/javascript" src="http://cdn.kendostatic.com/2013.2.918/js/kendo.all.min.js"></script>


    <style type="text/css">
        .grid-cntNav {
            height: auto;
            left: 5px;
            margin: 10px 0px;
            padding: 0;
            top: 0;
            width: auto;
        }

        .flRight {
            float: right;
        }

        .pagging {
            color: #949494;
            float: right;
            height: 20px;
            line-height: 19px;
            padding: 0 10px;
        }

        .pagging a {
            background: none repeat scroll 0 0 #DDE0E8;
            border: 1px solid #D5D5D5;
            color: #949494;
            float: left;
            height: 20px;
            margin-left: 5px;
            padding: 0 8px;
            text-decoration: none;
        }

        .pagging span {
            float: none;
            margin-left: 5px;
            padding-top: 2px;
        }

        /* ---------------------------------------------------------- */
        .checkbox-container {
            display: inline-block;
            position: relative;
        }

        .checkbox-container label {
            background-color: lightgray;
            border: 1px solid #fff;
            border-radius: 20px;
            display: inline-block;
            position: relative;
            transition: all 0.3s ease-out;
            width: 45px;
            height: 25px;
            z-index: 2;
        }

        .checkbox-container label::after {
            content: ' ';
            background-color: red;
            border-radius: 50%;
            position: absolute;
            top: 1.5px;
            left: 1px;
            transform: translateX(0);
            transition: transform 0.3s linear;
            width: 20px;
            height: 20px;
            z-index: 3;
        }

            .checkbox-container input {
                visibility: hidden;
                position: absolute;
                z-index: 2;
            }

                /*.checkbox-container input:checked + label + .active-circle {
	        transform: translate(-50%, -50%) scale(15);
        }*/

        .checkbox-container input:checked + label::after {
            transform: translateX(calc(100% + 0.5px));
            background-color: forestgreen;
        }
        input[type=checkbox] { margin-right:5px; }
        /*---------------------------------------------------------------*/
        .groupchk {
            padding: 2px 2px 2px 2px;
        }

        .groupcheck {
            padding: 2px 2px 2px 2px;
        }
    </style>
    <script type="text/javascript">
        window.localStorage.setItem("post", "get");
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                window.localStorage.setItem("post", "post");
                __doPostBack("", "");
            }
        }
    </script>
</head>
<body>
    <form id="frmEntry" runat="server">
        <asp:ValidationSummary ID="valEvent" runat="server" ShowSummary="false" ShowMessageBox="true" ValidationGroup="RoleGroup" />
        <asp:ScriptManager ID="srcRole" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnAccess" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnNotAccess" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
        <div id="contentwrapper" class="contentwrapper">
            <asp:UpdatePanel ID="upRolePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="myModal" style="display: block; width: 100%;">
                        <div class="modal-content">
                            <div class="modal-body" style="min-height: 500px;">
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtRoleName" style="">Role Name</label>
                                        <asp:TextBox ID="txtRoleName" runat="server" TabIndex="1" class="shortinptfld" MaxLength="255" Width="80%"></asp:TextBox>
                                    </div>
                                    <div class="input-field col m6">
                                        <label class="active" for="txtRoleName">Role Code</label>
                                        <asp:TextBox ID="txtRoleID" runat="server" class="shortinptfld" TabIndex="2" MaxLength="20" Width="80%"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m6">
                                        <label class="active" for="txtComments">Comments</label>
                                        <asp:TextBox ID="txtComments" runat="server" TabIndex="3" class="shortinptfld" MaxLength="255" Width="80%"></asp:TextBox>
                                    </div>
                                    <div class="input-field col m">
                                        <label class="form-check-label" for="chkActive">Active Status</label>
                                        <div class="form-check p-0">
                                            <div class="checkbox-container yellow">
                                                <asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" TabIndex="4" />
                                                <label for="chkActive"></label>
                                                <div class="active-circle"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="input-field col m3">
                                        <div class="card">
                                            <div class="card-header" style="background-color: navy;color: wheat;">
                                                <h5 class="card-title mb-0">Assign Menu Privilege</h5>
                                            </div>
                                            <asp:UpdatePanel ID="upRolePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <div class="card-body" id="treeview">
                                                    <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All" ExpandDepth="0" OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged" TabIndex="5">
                                                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                                        <NodeStyle Font-Names="Tahoma" Font-Size="12pt" ForeColor="Black" HorizontalPadding="2px"
                                                            NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="true" HorizontalPadding="0px"
                                                            VerticalPadding="0px" Font-Bold="true" />
                                                    </asp:TreeView>
                                                </div>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="input-field col m3">
                                        <div class="card">
                                            <div class="card-header" style="background-color: navy;color: wheat;">
                                                <h5 class="card-title mb-0">Assign Reports Privilege</h5>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <div class="card-body" id="treeviewrep">
                                                    <asp:TreeView ID="TreeView2" runat="server" ShowCheckBoxes="All" ExpandDepth="0" OnTreeNodeCheckChanged="TreeView2_TreeNodeCheckChanged" TabIndex="5">
                                                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                                        <NodeStyle Font-Names="Tahoma" Font-Size="12pt" ForeColor="Black" HorizontalPadding="2px"
                                                            NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="true" HorizontalPadding="0px"
                                                            VerticalPadding="0px" Font-Bold="true" />
                                                    </asp:TreeView>
                                                </div>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="input-field col m3">
                                        <div class="card">
                                            <div class="card-header" style="background-color: navy;color: wheat;">
                                                <h5 class="card-title mb-0">Assign Widgets Privilege</h5>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <div class="card-body" id="treeviewicon">
                                                    <asp:TreeView ID="TreeView3" runat="server" ShowCheckBoxes="All" ExpandDepth="0" OnTreeNodeCheckChanged="TreeView2_TreeNodeCheckChanged" TabIndex="5">
                                                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                                        <NodeStyle Font-Names="Tahoma" Font-Size="12pt" ForeColor="Black" HorizontalPadding="2px"
                                                            NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="true" HorizontalPadding="0px"
                                                            VerticalPadding="0px" Font-Bold="true" />
                                                    </asp:TreeView>
                                                </div>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="input-field col m3">
                                        <div class="card">
                                            <div class="card-header" style="background-color: navy;color: wheat;">
                                                <h5 class="card-title mb-0">Assign General Master Privilege</h5>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <div class="card-body" id="treeviewGenMaster">
                                                    <asp:TreeView ID="TreeView4" runat="server" ShowCheckBoxes="All" ExpandDepth="0" OnTreeNodeCheckChanged="TreeView4_TreeNodeCheckChanged" TabIndex="6">
                                                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                                        <NodeStyle Font-Names="Tahoma" Font-Size="12pt" ForeColor="Black" HorizontalPadding="2px"
                                                            NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="true" HorizontalPadding="0px"
                                                            VerticalPadding="0px" Font-Bold="true" />
                                                    </asp:TreeView>
                                                </div>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="input-field col m12">
                                        <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static">
                                            <ul id="wowList" runat="server" class="text-center" clientidmode="Static"></ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary" ToolTip="Save" ValidationGroup="UserMgmt" OnClick="btnSave_Click" TabIndex="7" />
                                <asp:Button ID="btnReset" runat="server" Text="Reset" class="btn btn-primary" ToolTip="Reset" OnClick="btnReset_Click" TabIndex="8" />
                            </div>

                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <script type="text/javascript" language="javascript">
            function checkedNodeIds(nodes, checkedNodes) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].checked) {
                        checkedNodes.push(nodes[i].id);
                    }
                    if (nodes[i].hasChildren) {
                        checkedNodeIds(nodes[i].children.view(), checkedNodes);
                    }
                }
            }

            function useraccess() {
                jQuery('.pageheader').next().val('');
                jQuery('.pageheader').next().next().val('');
                var hdnaccess = jQuery('.pageheader').next();
                var hdnnoaccess = jQuery('.pageheader').next().next();
                jQuery("#treeview input:checkbox").each(function () {
                    var text = jQuery(this).parents('.k-checkbox').next().find('span').attr('class').split(' ')[1];
                    if (jQuery(this).is(':checked')) {
                        hdnaccess.val(hdnaccess.val() + ',' + text);
                    }
                    else {
                        hdnnoaccess.val(hdnnoaccess.val() + ',' + text);
                    }
                });
                jQuery("#treeviewright input:checkbox").each(function () {
                    var text = jQuery(this).parents('.k-checkbox').next().find('span').attr('class').split(' ')[1];
                    if (jQuery(this).is(':checked')) {
                        hdnaccess.val(hdnaccess.val() + ',' + text);
                    }
                    else {
                        hdnnoaccess.val(hdnnoaccess.val() + ',' + text);
                    }
                });

                jQuery.ajax({
                    type: "POST",
                    url: "handler/Accessfield.ashx",
                    data: "Access=" + hdnaccess.val() + "&NoAccess=" + hdnnoaccess.val(),
                    success: function (data) {
                        jQuery('.bsave').click();
                    },
                    error: function () {
                        alert('Error Occur');
                        return false;
                    }
                });
            }

            function showmessage(msg) {
                jQuery('.usermsg').html(msg);
                jQuery(".usermsg").fadeIn(3000);
                jQuery(".usermsg").delay(5000).fadeOut(3000);
            }

            function changecolor() {
                jQuery('.stdtable tbody tr').hover(function () {
                    jQuery(this).attr('style', 'background:#7EB8CE;');
                }, function () {
                    jQuery(this).attr('style', 'background:#FFFFFF;');
                });
            }

            $(document).ready(function () {

                changecolor();


                console.log('<%=Session["0"].ToString() %>');
                console.log('<%=Session["1"].ToString() %>');
                var i = 0;
                var reset = '<%=Session["reset"].ToString() %>';
                var jsVariable = '<%=Session["0"].ToString() %>';
                var users = '<%=Session["1"].ToString() %>';

                console.log(window.localStorage.getItem("post"));
                var roleid = '<%=hdnpkID.Value.ToString() %>';

                if (roleid === "admin") {

                    if (window.localStorage.getItem("post") === "post") {
                        console.log('this is  postback')
                        $("#treeview #disabledTreeviewNode").closest("td").attr("class", "groupchk");
                        $("#treeview td.groupchk input:checkbox").attr("class", "chkgroup");
                        if (reset === "reset") {
                            if ($("#treeview input.chkgroup").prop("checked") === true) {
                                //do nothing
                            }
                            else {
                                $("#treeview input.chkgroup").attr("checked", false);
                                $("#treeview input.chkgroup").attr("disabled", false);
                            }
                           
                        }
                        else {
                            $("#treeview input.chkgroup").attr("checked", jsVariable);
                            $("#treeview input.chkgroup").attr("disabled", jsVariable);
                        }


                        $("#treeview #disabledTreeviewNode1").closest("td").attr("class", "groupcheck");
                        $("#treeview td.groupcheck input:checkbox").attr("class", "checkgroup");
                        if (reset === "reset") {
                            if ($("#treeview input.checkgroup").prop("checked") === true) {
                                //do nothing
                            }
                            else {
                                $("#treeview input.checkgroup").attr("checked", false);
                                $("#treeview input.checkgroup").attr("disabled", false);
                            }
                           
                        }
                        else {
                            $("#treeview input.checkgroup").attr("checked", users);
                            $("#treeview input.checkgroup").attr("disabled", users);

                        }

                        //window.localStorage.setItem("post", "get");
                    }
                    else {
                        console.log('this is not postback')
                        $(document).ready(function () {
                            $("#treeview #disabledTreeviewNode").closest("td").attr("class", "groupchk");
                            $("#treeview td.groupchk input:checkbox").attr("class", "chkgroup");

                            if ($("#treeview input.chkgroup").prop("checked") === true) {
                                $("#treeview input.chkgroup").attr("disabled", true);
                            }

                            $("#treeview #disabledTreeviewNode1").closest("td").attr("class", "groupcheck");
                            $("#treeview td.groupcheck input:checkbox").attr("class", "checkgroup");
                            if ($("#treeview input.checkgroup").prop("checked") === true) {
                                $("#treeview input.checkgroup").attr("disabled", true);
                            }

                        });
                    }
                }


            });

            $('#myModal').on('hidden', function () {
                window.localStorage.clear();
            })
        </script>

    </form>
</body>
</html>
