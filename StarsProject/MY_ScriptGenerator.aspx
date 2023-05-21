<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="MY_ScriptGenerator.aspx.cs" Inherits="StarsProject.MY_ScriptGenerator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css"/>

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById('<%=((Label)Master.FindControl("lblModule")).ClientID %>').innerText = "Daily Work Log";
            $('.datepicker').datepicker({ format: "dd-mm-yyyy" });
            //$('.timepicker').timepicker();
        });
        /*---------------------------------------------------------*/
        function showcaseError(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:circle;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        function showcaseError(xMsg, xClass) {
            M.toast({ html: '<ul id="ulToast">' + xMsg + '</ul>', classes: xClass, displayLength: 4000 });
        }
        function showcaseMessage(xText, xIcon) {
            xText = (xText == '') ? 'Action Performed !' : xText;
            xIcon = (xIcon == '') ? 'Info' : xIcon;
            swal({ title: "Message", text: xText, icon: xIcon });
        }

        function showErrorMessage(strMess) {
            jQuery.confirm({ title: 'Data Validation', content: 'Are you sure, You want to delete record !', type: 'red', typeAnimated: true });
        }

        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }
        // -----------------------------------------------------
        function CheckAll() {
            jQuery("#spnSelected").text($("#contentwrapper .chkToCompare").find('input:checked').length);
        }
    </script>
    
    <style type="text/css">
        .stdtable thead th {
            font-size: 16px !important;
            font-weight:bold;
        }

        .stdtable tr td {
            font-size: 16px !important;
            color:black !important;
        }

        .form-control {
            font-size: 12px !important;
        }
        
        /*.checkbox-container input {
            visibility: hidden;
            position: absolute;
            z-index: 2;
        }*/

        .chkReminder { position: relative !important; pointer-events:all !important; opacity: 100 !important; width:20px !important; height:20px !important; top:3px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnMonth" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnYear" runat="server" ClientIDMode="Static" EnableViewState="true" />
    <asp:HiddenField ID="hdnDebit" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
    <asp:HiddenField ID="hdnCredit" runat="server" ClientIDMode="Static" EnableViewState="true" Value="0" />
    <div id="contentwrapper" class="contentwrapper">
        <div class="row">
            <div class="col m6">
                <div class="row">
                    <div class="input-field col m6">
                        <label class="active" for="txtTableName">Select Table Name</label>
                        <asp:TextBox ID="txtTableName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" />
                    </div>        
                    <div class="input-field col m3">
                        <button id="btnGetCols" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnGetCols_Click" TabIndex="8"><i class="material-icons left">save</i>Get Structure</button>
                    </div>        
                </div>
            </div>
            <div class="col m6">
                <div class="row">
                    <div class="input-field col m3 float-right">
                        <button id="btnGenerate" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnGenerate_Click" TabIndex="8"><i class="material-icons left">save</i>Generate Script</button>
                    </div>        
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col m2">
                <button id="btnSelectAll" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan center mr-1 float-left" onserverclick="btnSelectAll_ServerClick" tabindex="17">Select All</button>
            </div>
            <div class="col m2">
                <button id="btnDeSelectAll" type="button" runat="server" clientidmode="Static" text="Save" class="btn cyan center mr-1 float-left" onserverclick="btnDeSelectAll_ServerClick" tabindex="17">De-Select All</button>
            </div>
        </div>
        <div class="row">
            <div class="col m6">
                <table id="tblStructure" class="stdtable" cellpadding="0" cellspacing="0" border="0" width="100%">
                    <asp:Repeater ID="rptStructure" runat="server">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th class="center-align width-10">Select</th>
                                    <th class="center-align width-10">Position</th>
                                    <th class="center-align width-20">Column Name</th>
                                    <th class="center-align width-10">Column Type</th>
                                    <th class="center-align width-10">Is Nullable</th>
                                    <th class="center-align width-10">Width</th>
                                    <th class="center-align width-10">Scale</th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="blueShed">
                                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" Value='<%#Eval("pkID") %>' />
                                <asp:HiddenField ID="hdnColName" runat="server" ClientIDMode="Static" Value='<%#Eval("ColName") %>' />
                                <asp:HiddenField ID="hdnColType" runat="server" ClientIDMode="Static" Value='<%#Eval("ColType") %>' />
                                <asp:HiddenField ID="hdnColIsNull" runat="server" ClientIDMode="Static" Value='<%#Eval("ColIsNull") %>' />
                                <asp:HiddenField ID="hdnColWidth" runat="server" ClientIDMode="Static" Value='<%#Eval("ColWidth") %>' />
                                <asp:HiddenField ID="hdnColScale" runat="server" ClientIDMode="Static" Value='<%#Eval("ColScale") %>' />
                                <td class="center-align">
                                    <input class="chkReminder" id="chkSelect" runat="server" clientidmode="static" type="checkbox" />
                                </td>
                                <td class="center-align"><%# Eval("pkID") %></td>
                                <td class="center-align"><%# Eval("ColName") %></td>
                                <td class="center-align"><%# Eval("ColType") %></td>
                                <td class="center-align"><%# Eval("ColIsNull") %></td>
                                <td class="center-align"><%# Eval("ColWidth") %></td>
                                <td class="center-align"><%# Eval("ColScale") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="col m6">
                <div class="row">
                    <div class="input-field col m4">
                        <label class="active" for="txtSPList">SP - For List</label>
                        <asp:TextBox ID="txtSPList" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" />
                    </div> 
                    <div class="input-field col m4">
                        <label class="active" for="txtSPInsUpd">SP - For Ins/Upd</label>
                        <asp:TextBox ID="txtSPInsUpd" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" />
                    </div> 
                    <div class="input-field col m4">
                        <label class="active" for="txtSPDel">SP - For Del</label>
                        <asp:TextBox ID="txtSPDel" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" />
                    </div> 
                </div>
                <div class="row">
                    <div class="input-field col m3">
                        <label class="active" for="txtEntityName">Entity Name</label>
                        <asp:TextBox ID="txtEntityName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" />
                    </div> 
                    
                    <div class="input-field col m3">
                        <label class="active" for="txtBALList">SP - For List</label>
                        <asp:TextBox ID="txtBALList" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" />
                    </div> 
                    <div class="input-field col m3">
                        <label class="active" for="txtBALInsUpd">SP - For List</label>
                        <asp:TextBox ID="txtBALInsUpd" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" />
                    </div> 
                    <div class="input-field col m3">
                        <label class="active" for="txtBALDel">SP - For List</label>
                        <asp:TextBox ID="txtBALDel" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" />
                    </div> 
                </div>
            </div>
        </div>
    </div>
    <br /><br />
</asp:Content>
