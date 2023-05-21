<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="DatabaseBackup.aspx.cs" Inherits="StarsProject.DatabaseBackup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="apple-touch-icon" href="app-assets/images/favicon/apple-touch-icon-152x152.png"/>
    <link rel="shortcut icon" type="image/x-icon" href="app-assets/images/favicon/favicon-32x32.png"/>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css"/>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/flag-icon/css/flag-icon.min.css"/>
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

    <style type="text/css">
        table.responsive-table th { background-color:#6868d6 !important; color:white !important; }
        table.responsive-table th, table.responsive-table td { border-bottom:1px solid silver !important; }
        #toast-container { background-color:aliceblue; min-width: 10%; top: 50%; right: 50%; transform: translateX(50%) translateY(50%); color:yellow; border:1px solid blue !important; border-radius:8px  !important; }
        input[type=text], input[type=password], input[type=email], input[type=url], input[type=number], textarea :focus {
            border-color: #3C90BE;
        }
        #ulToast li::before {
          content: "\2022";  /* Add content: \2022 is the CSS Code/unicode for a bullet */
          color: white; /* Change the color */
          font-weight: bold; /* If you want it to be bold */
          display: inline-block; /* Needed to add space between the bullet and the text */
          width: 1em; /* Also needed for space (tweak if needed) */
          margin-left: -1em; /* Also needed for space (tweak if needed) */
        }
    </style>
    <script type="text/javascript">
        //Sys.Application.add_init(appl_init);

        //function appl_init() {
        //    var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
        //    pgRegMgr.add_endRequest(EndHandler);
        //}

        //function EndHandler() {
        //    $("#dvStatus").hide();
        //}

        $(document).ready(function () {
            $('.datepicker').datepicker({ format: "dd/mm/yyyy" });
            $('.timepicker').timepicker();
        });
        //$(window).bind('beforeunload', function () {
        //    alert('helloe')
        //    setBackupStatus('asdfasdfsdfdddddddddd', 'off');
        //});
        function showErrorPopup(xMsg) {
            M.toast({ html: '<ul id="ulToast" style="list-style:none;">' + xMsg + '</ul>', displayLength: 4000 });
        }

        function setBackupStatus(xMsg, xMode) {
           alert(xMode)
            $("#lblMessage").val(xMsg);
            if (xMode == 'on') {
                $("#dvStatus").addClass("indeterminate");
            }
            else {
                $("#dvStatus").removeClass("indeterminate");
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager ID="srcUser" runat="server" ClientIDMode="Static" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
    <asp:HiddenField ID="hdnSelect" runat="server" ClientIDMode="Static" Value="full" />
    <div class="row">
        <div class="col s12">
            <div class="card">
                <h5 style="padding:5px; line-height:35px; margin: 0.42rem 0 .556rem 0; background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
                    <i id="pageIcon" onclick="javascript:showhideMyMenu();" class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Application Database Backup</span>
                </h5>
            </div>
        </div>
    </div>
    <div id="contentwrapper" class="contentwrapper">
        <div class="row">
            <div id="dvFull" class="col s12" style="height:300px; overflow-y:auto;">
                <div class="row">
                    <div class="input-field col m6">
                        <h5>Full Backup</h5>
                    </div>
                </div>
                <div class="row">
                    <div class="input-field col m6">
                        <asp:Label ID="lblMessage" runat="server" ClientIDMode="Static" Text="asdfsdf" />

                    </div>
                    <div class="progress">
                      <div id="dvStatus" runat="server" clientidmode="static"></div>
                    </div>
                </div>
                <div class="row vertical-align-bottom">
                    <div class="col s12 left">
                        <button id="btnGetBackup" runat="server" clientidmode="Static" class="btn cyan mr-1" onclick="setBackupStatus('test....','on');" onserverclick="btnGetBackup_Click"><i class="material-icons left">save</i>Start Backup</button>
                    </div>
                </div>
            </div>
        </div>

     </div> 

</asp:Content>

