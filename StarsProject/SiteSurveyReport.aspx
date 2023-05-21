<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteSurveyReport.aspx.cs" Inherits="StarsProject.SiteSurveyReport"  MaintainScrollPositionOnPostback="true"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="app-assets/vendors/vendors.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/pages/form-select2.min.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/materialize.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/themes/vertical-modern-menu-template/style.css" />
    <link rel="stylesheet" type="text/css" href="app-assets/css/custom/custom.css" />

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src="app-assets/js/vendors.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <link rel="stylesheet" href="app-assets/vendors/select2/select2.min.css" type="text/css" />
    <link rel="stylesheet" href="app-assets/vendors/select2/select2-materialize.css" type="text/css" />
    <script type="text/javascript" src="app-assets/js/plugins.js"></script>

    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>
    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>

    <style type="text/css">
        table.responsive-table th {
            background-color: #6868d6 !important;
            color: white !important;
        }

        table.responsive-table th, table.responsive-table td {
            border-bottom: 1px solid silver !important;
        }

        #toast-container {
            background-color: aliceblue;
            min-width: 10%;
            top: 50%;
            right: 50%;
            transform: translateX(50%) translateY(50%);
            color: yellow;
            border: 1px solid blue !important;
            border-radius: 8px !important;
        }

        input[type=text], input[type=password], input[type=email], input[type=url], input[type=number], textarea :focus {
            border-color: #3C90BE;
        }

        #ulToast li::before {
            content: "\2022"; /* Add content: \2022 is the CSS Code/unicode for a bullet */
            color: white; /* Change the color */
            font-weight: bold; /* If you want it to be bold */
            display: inline-block; /* Needed to add space between the bullet and the text */
            width: 1em; /* Also needed for space (tweak if needed) */
            margin-left: -1em; /* Also needed for space (tweak if needed) */
        }

        /* ==================================== */
        /*****         Modal Popup        *****/
        /* ==================================== */
        .Popup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 96%;
            height: 96%;
        }

        .btnTopRightCorner {
            display: block;
            box-sizing: border-box;
            width: 30px;
            height: 30px;
            border-width: 3px;
            border-style: solid;
            border-color: red;
            border-radius: 100%;
            background: -webkit-linear-gradient(-45deg, transparent 0%, transparent 46%, white 46%, white 56%,transparent 56%, transparent 100%), -webkit-linear-gradient(45deg, transparent 0%, transparent 46%, white 46%, white 56%,transparent 56%, transparent 100%);
            background-color: red;
            box-shadow: 0px 0px 5px 2px rgba(0,0,0,0.5);
            transition: all 0.3s ease;
            position: relative;
            top: 12px;
            right: 15px;
        }
    </style>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('tabs').tabs();
            $('.timepicker').timepicker();
            $(function () {
                $(".tabs a").click(function () {
                    $("#hdnCurrTab").val($(this).attr("href").replace("#", ""));
                });
            });
            retainTabPosition();
            getAMCType();
        }

        function getAMCType() {
            $('#drpContractType, #txtStartDate').bind('change', function () {

                var date = new Date($('#txtStartDate').val());

                if ($('#drpContractType').val().toLowerCase() == '6 months') {
                    date.setMonth(date.getMonth() + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '1 year') {
                    date.setMonth(date.getMonth() + 11 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '2 years') {
                    date.setMonth(date.getMonth() + 22 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '3 years') {
                    date.setMonth(date.getMonth() + 33 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '4 years') {
                    date.setMonth(date.getMonth() + 44 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '5 years') {
                    date.setMonth(date.getMonth() + 55 + parseInt($('#drpContractType').val()));
                }

                else if ($('#drpContractType').val().toLowerCase() == '10 years') {
                    date.setMonth(date.getMonth() + 110 + parseInt($('#drpContractType').val()));
                }

                $("#txtEndDate").val(date.toISOString().slice(0, 10));

            }).trigger('change');
        }

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


        /*---------------------------------------------------------*/
        function openCustomerInfo(cat) {
            var keyid = (cat == 'view') ? jQuery('#hdnCustomerID').val() : "0";
            var pageUrl = (cat == 'view') ? "customers.aspx?mode=view&id=" + keyid : "customers.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }

        /*Add Product view on Product Name Vikram Rajput 15-07-2020*/
        function openProductInfo(cat) {
            var keyid = "0";
            var pageUrl = "Products.aspx?mode=view&id=0";
            $('#ifrModule').attr('src', pageUrl);
            $find("mpe").show();
        }
        /*---------------------------------------------------------*/
        function showErrorPopup(xTitle, xMsg) {
            if (xTitle == '')
                xTitle = 'Message';
            // -------------------------------------
            if (xMsg != '') {
                jQuery.confirm({
                    title: xTitle, content: xMsg, type: 'red', typeAnimated: true, width: 'auto',
                    buttons: { close: { text: 'Close', btnClass: 'btn-blue ConfirmClose', action: function () { } } }
                });
            }
        }

        function retainTabPosition() {

            var currTab = $("#hdnCurrTab").val();

            $("#lnkFisibility").removeClass("active");
            $("#lnkGenDe").removeClass("active");
            $("#lnkReefDe").removeClass("active");
            $("#lnkEquipDe").removeClass("active");
            $("#lnkSysAvailDe").removeClass("active");
            $("#lnkReqDe").removeClass("active");

            $("#pnl_Fisibility").css("display", "none");
            $("#pnl_GenDe").css("display", "none");
            $("#pnl_ReefDe").css("display", "none");
            $("#pnl_EquipDe").css("display", "none");
            $("#pnl_SysAvailDe").css("display", "none");
            $("#pnl_ReqDe").css("display", "none");

            if (currTab == 'pnl_Fisibility') {
                $("#lnkFisibility").addClass("active");
                $("#pnl_Fisibility").css("display", "block");
            }
            else if (currTab == 'pnl_GenDe') {
                $("#lnkTNC").addClass("active");
                $("#pnl_GenDe").css("display", "block");
            }
            else if (currTab == 'pnl_ReefDe') {
                $("#lnkReefDe").addClass("active");
                $("#pnl_ReefDe").css("display", "block");
            }
            else if (currTab == 'pnl_EquipDe') {
                $("#lnkEquipDe").addClass("active");
                $("#pnl_EquipDe").css("display", "block");
            }
            else if (currTab == 'pnl_SysAvailDe') {
                $("#lnkSysAvailDe").addClass("active");
                $("#pnl_SysAvailDe").css("display", "block");
            }
            else if (currTab == 'pnl_ReqDe') {
                $("#lnkReqDe").addClass("active");
                $("#pnl_ReqDe").css("display", "block");
            }
        }
    </script>

</head>
<body class="loginpage" style="background-color: transparent; overflow-x: hidden;">
    <form id="frmEntry" runat="server">
        <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
        <div id="contentwrapper" class="contentwrapper">
            <div class="widgetbox">
                <%-- Bootstrap CONTRACT INFO Modal Popup --%>
                <div class="clearall"></div>
                <asp:HiddenField ID="hdnParent" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnOrgCodeEmp" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustEmailAddress" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCurrTab" runat="server" ClientIDMode="Static" EnableViewState="true" Value="pnl_Fisibility" />
                <div id="myModal" style="display: block; width: 100%;">
                    <div class="modal-content border-0">
                        <div class="modal-body col m12">
                            
                            <div class="row">
                                <div class="col m12" style="border-right: 3px ridge silver;">
                                   <div class="row">
                                        <div class="input-field col m1.5">
                                            <label class="active" for="txtSurveyID">Site Survey ID</label>
                                            <asp:TextBox ID="txtSurveyID" runat="server" placeholder="" class="form-control" ClientIDMode="Static" TabIndex="1" ReadOnly="true" />
                                        </div>
                                        <div class="input-field col m2">
                                            <label class="active" for="txtVisitDate">Visit Date <span class="materialize-red-text font-weight-800">*</span> </label>
                                            <asp:TextBox ID="txtVisitDate" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" onkeypress="return false;" placeholder="" TextMode="Date" />
                                        </div>
                                       
                                        <div class="input-field col m4">
                                            <asp:HiddenField ID="hdnCustStateID" runat="server" ClientIDMode="Static" />
                                            <asp:TextBox ID="txtCustomerName" runat="server" ClientIDMode="Static" placeholder="" class="form-control" AutoPostBack="true" onKeyup="bindDDLTo('#txtCustomerName')" OnTextChanged="txtCustomerName_TextChanged" TabIndex="4" />
                                            <label class="active" for="txtCustomerName">
                                                Customer Name&nbsp;<small class="text-muted font-italic">(Minimum 3 chars to activate search)</small> <span class="materialize-red-text font-weight-800"></span>
                                                <a href="javascript:openCustomerInfo('view');">
                                                    <img src="images/registration.png" width="30" height="20" alt="Preview Customer Info" title="Preview Customer Info" style="display: inline-block;" tabindex="4" />
                                                </a>
                                                <a href="javascript:openCustomerInfo('add');">
                                                    <img src="images/addCustomer.png" width="30" height="20" alt="Add New Customer" title="Add New Customer" />
                                                </a>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col m12">
                                    <div class="row">
                                        <div class="col s12">
                                            <ul id="Ul1" class="tabs" runat="server" clientidmode="Static" enableviewstate="true" style="background-color: antiquewhite;">
                                                <li class="tab col m1.5 p-0"><a id="lnkFisibility" class="left active" href="#pnl_Fisibility">Pre-Fisibility Report</a></li>
                                                <li class="tab col m1.5 p-0"><a id="lnkGenDe" class="left" href="#pnl_GenDe">General Detail</a></li>
                                                <li class="tab col m1.5 p-0"><a id="lnkReefDe" class="left" href="#pnl_ReefDe">Roof Detail</a></li>
                                                <li class="tab col m2.5 p-0"><a id="lnkEquipDe" class="left" href="#pnl_EquipDe">Equipment Location With Distance</a></li>
                                                <li class="tab col m2 p-0"><a id="lnkSysAvailDe" class="left" href="#pnl_SysAvailDe">Existing System Availablity</a></li>
                                                <li class="tab col m2.5 p-0"><a id="lnkReqDe" class="left" href="#pnl_ReqDe">Detail Required For Engineering</a></li>
                                            </ul>
                                        </div>
                                       <div id="pnl_Fisibility" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display:block;" >
                                            <div class="row">
                                               <div class="input-field col m5">
                                                    <asp:DropDownList ID="drpPosition" runat="server" class="select2-theme browser-default" TabIndex="1" >
                                                        <asp:ListItem Text="---Select---" Value="" />
                                                        <asp:ListItem Text="Group mounted" Value="GroupMounted" />
                                                        <asp:ListItem Text="Rooftop" Value="Rooftop" />
                                                    </asp:DropDownList>
                                                    <label class="active" for="txtCategory">Solar System Position</label>
                                                </div>
                                                <div class="input-field col m7">
                                                    <label class="active" for="txtPosRemarks">Remarks</label>
                                                    <asp:TextBox ID="txtPosRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="81" placeholder="" />
                                                </div>
                                            </div>
                                            <div class="row">
                                               <div class="input-field col m5">
                                                    <asp:DropDownList ID="drpBuilding" runat="server" class="select2-theme browser-default" TabIndex="1" >
                                                        <asp:ListItem Text="---Select---" Value="" />
                                                        <asp:ListItem Text="Residential" Value="Residential" />
                                                        <asp:ListItem Text="Commercial" Value="Commercial" />
                                                        <asp:ListItem Text="School or Colleges" Value="SchoolorColleges" />
                                                        <asp:ListItem Text="IT Parks or Hospital" Value="ITParksorHospital" />
                                                        <asp:ListItem Text="Other" Value="Other" />
                                                    </asp:DropDownList>
                                                    <label class="active" for="drpBuilding">Type of Buildings</label>
                                                </div>
                                                <div class="input-field col m7">
                                                    <label class="active" for="txtbuildingRemarks">Remarks</label>
                                                    <asp:TextBox ID="txtbuildingRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="81" placeholder="" />
                                                </div>
                                            </div>
                                            <div class="row">
                                               <div class="input-field col m5">
                                                    <asp:DropDownList ID="drpRoof" runat="server" class="select2-theme browser-default" TabIndex="1" >
                                                        <asp:ListItem Text="---Select---" Value="" />
                                                        <asp:ListItem Text="RCC Roof" Value="RCCRoof" />
                                                        <asp:ListItem Text="Metal roof / Cement sheet" Value="MetalRoofCementSheet" />
                                                        <asp:ListItem Text="Other" Value="Other" />
                                                    </asp:DropDownList>
                                                    <label class="active" for="drpRoof">Type of Roof</label>
                                                </div>
                                                <div class="input-field col m7">
                                                    <label class="active" for="txtRoofRemarks">Remarks</label>
                                                    <asp:TextBox ID="txtRoofRemarks" runat="server" class="form-control" ClientIDMode="Static" TabIndex="81" placeholder="" />
                                                </div>
                                            </div>
                                           <div class="row">
                                                <div class="input-field col m7">
                                                    <label class="active" for="txtReq">Special Requirement by Client</label>
                                                    <asp:TextBox ID="txtReq" runat="server" class="form-control" ClientIDMode="Static" TabIndex="81" placeholder="" TextMode="MultiLine" MaxLength="1000" Rows="4"/>
                                                </div>
                                            </div>
                                        </div>
                                       <div id="pnl_GenDe" class="row" runat="server" clientidmode="Static" enableviewstate="true" style="display:none;">
                                            <div class="input-field col m12" style="height:400px; overflow-y:auto;">
                                                <div id="container1" runat="server" clientidmode="Static" class="container">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="row">
                                                            <div class="col m6 pr-0">
                                                                <div class="card small" style="height:350px;">
                                                                    <div class="card-title blue white-text" style="padding:10px;">
                                                                        <p class="no-margin">General Details</p>
                                                                    </div>
                                                                    <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m12">
                                                                                <asp:TextBox ID="txtSLoad" runat="server" class="form-control" Placeholder="" TabIndex="19" MaxLength="100"  ClientIDMode="Static" />
                                                                               <label class="active" for="txtSLoad">Sanction Load</label> <%--<span class="materialize-red-text font-weight-800">*</span></label>--%>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m12">
                                                                                <asp:TextBox ID="txtConsumption" runat="server" TabIndex="20" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                                                <label class="active" for="txtConsumption">Monthly Electricity Consumption from DISCOM</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m6">
                                                                                <asp:TextBox ID="txtinstallation" runat="server" TabIndex="20" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                                                <label class="active" for="txtinstallation">Total Area Available for Installation</label>
                                                                            </div>
                                                                        
                                                                            <div class="input-field col m6">
                                                                                <label class="active" for="chkBlock">In Case Of Lease Property</label>
                                                                                <div class = "switch"><label>No
                                                                                    <asp:CheckBox ID="chkBlock" runat="server" ClientIDMode="Static" Checked="true" TabIndex="5"/>
                                                                                    <span class="lever"></span>Yes</label>
                                                                                </div> 
                                                                            </div>
                                                                        </div>
                                                                        <div class="row" style="width:100%">
                                                                            <div class="input-field col m6">
                                                                                
                                                                                <asp:DropDownList ID="drpConnection" class="select2-theme browser-default" runat="server"  ClientIDMode="Static" TabIndex="23" >
                                                                                    <asp:ListItem Text="---Select---" Value="" />
                                                                                    <asp:ListItem Text="Single Phase 230" Value="SinglePhase230" />
                                                                                    <asp:ListItem Text="Three phase 415v" Value="Threephase415v" />
                                                                                 </asp:DropDownList>
                                                                                 <label class="active" for="drpConnection">Type Of Existing AC Connection</label>
                                                                            </div>
                                                                             <div class="input-field col m6">
                                                                                <asp:TextBox ID="txtConnRemarks" runat="server" TabIndex="20" class="form-control"  ClientIDMode="Static" Placeholder="" MaxLength="100"/>
                                                                                <label class="active" for="txtConnRemarks">Connection Remarks</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                             <div class="col m6 pr-0">
                                                                <div class="card small" style="height:350px;">
                                                                    <div class="card-title blue white-text" style="padding:10px;">
                                                                        <p class="no-margin">Attachments</p>
                                                                    </div>
                                                                    <div class="card-content" style="padding:10px 5px 5px 5px  !important;">
                                                                       
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                       <div id="pnl_ReefDe" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                            <asp:Panel runat="server" ID="pnlDetail">
                                                <div class="row">
                                                    <table id="tblRoofDetails" class="table responsive-table striped width-100">
                                                        <asp:Repeater ID="rptRoofDetails" runat="server" ClientIDMode="Static" OnItemCommand="rptRoofDetails_ItemCommand" OnItemDataBound="rptRoofDetails_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr>
                                                                        <th class="center-align">Building Name</th>
                                                                        <th class="center-align">Roof Type</th>
                                                                        <th class="center-align">Roof Area</th>
                                                                        <th class="center-align">Capacity Of Building</th>
                                                                        <th class="center-align">Action</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <asp:HiddenField ID="edRoofID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                    <td class="text-center">
                                                                        <asp:TextBox ID="edBuildingName" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("BuildingName") %>' AutoPostBack="true" OnTextChanged="editRoofDetail_TextChanged"  TabIndex="12" />
                                                                    </td>
                                                                    <td class="text-center">
                                                                        <asp:TextBox ID="edRoofType" MaxLength="15" runat="server" ClientIDMode="Static" Text='<%# Eval("RoofType") %>' AutoPostBack="true" OnTextChanged="editRoofDetail_TextChanged" TabIndex="12" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edRoofArea" runat="server" ClientIDMode="Static" Text='<%# Eval("RoofArea") %>' AutoPostBack="true" OnTextChanged="editRoofDetail_TextChanged" TabIndex="15" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edCapacity" runat="server" ClientIDMode="Static" Text='<%# Eval("CapacityOfBuilding") %>' AutoPostBack="true" OnTextChanged="editRoofDetail_TextChanged" TabIndex="16" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="27" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="background-color: #dde8f3; border-bottom: 5px solid navy;">
                                                                    <td class="left-align">
                                                                        <asp:HiddenField ID="hdnRoofID" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtBuildingType" runat="server" ClientIDMode="Static" class="form-control" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:TextBox ID="txtRoofType" runat="server" ClientIDMode="Static" TabIndex="29" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="txtRoofArea" runat="server" ClientIDMode="Static"  TabIndex="31"  />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="txtCapacity" runat="server" ClientIDMode="Static" TabIndex="32" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="110" Height="30" TabIndex="42" />
                                                                    </td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                         <div id="pnl_EquipDe" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                            <asp:Panel runat="server" ID="Panel1">
                                                <div class="row">
                                                    <table id="tblEquipmentLocationDetails" class="table responsive-table striped width-100">
                                                        <asp:Repeater ID="rptEquipmentLocationDetails" runat="server" ClientIDMode="Static" OnItemCommand="rptEquipmentLocationDetails_ItemCommand" OnItemDataBound="rptEquipmentLocationDetails_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr>
                                                                        <th class="center-align">Equipment</th>
                                                                        <th class="center-align">Distance</th>
                                                                        <th class="center-align">Con. Possibility</th>
                                                                        <th class="center-align">Client LT Panel Rating</th>
                                                                        <th class="center-align">Action</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <asp:HiddenField ID="edEquipID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                    <td class="text-center">
                                                                        <asp:TextBox ID="edEquipment" runat="server" ClientIDMode="Static" Text='<%# Eval("Equipment") %>' AutoPostBack="true" OnTextChanged="ediEquipmentLocation_TextChanged"  TabIndex="12" />
                                                                    </td>
                                                                    <td class="text-center">
                                                                        <asp:TextBox ID="edDistance" runat="server" ClientIDMode="Static" Text='<%# Eval("Distance") %>' AutoPostBack="true" OnTextChanged="ediEquipmentLocation_TextChanged" TabIndex="12" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edConnPossibility" runat="server" ClientIDMode="Static" Text='<%# Eval("ConnPossibility") %>' AutoPostBack="true" OnTextChanged="ediEquipmentLocation_TextChanged" TabIndex="15" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edClientRating" runat="server" ClientIDMode="Static" Text='<%# Eval("ClientRating") %>' AutoPostBack="true" OnTextChanged="ediEquipmentLocation_TextChanged" TabIndex="16" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="27" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="background-color: #dde8f3; border-bottom: 5px solid navy;">
                                                                    <td class="left-align">
                                                                        <asp:HiddenField ID="hdnEquipID" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtEquipment" runat="server" ClientIDMode="Static" class="form-control" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:TextBox ID="txtDistance" runat="server" ClientIDMode="Static" TabIndex="29" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="txtConnPossibility" runat="server" ClientIDMode="Static"  TabIndex="31"  />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="txtClientRating" runat="server" ClientIDMode="Static" TabIndex="32" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="110" Height="30" TabIndex="42" />
                                                                    </td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                         <div id="pnl_SysAvailDe" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                            <asp:Panel runat="server" ID="Panel2">
                                                <div class="row">
                                                    <table id="tblSysAvailablityDetails" class="table responsive-table striped width-100">
                                                        <asp:Repeater ID="rptSysAvailablityDetails" runat="server" ClientIDMode="Static" OnItemCommand="rptSysAvailablityDetails_ItemCommand" OnItemDataBound="rptSysAvailablityDetails_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr>
                                                                        <th class="center-align">Load Description</th>
                                                                        <th class="center-align">Capacity</th>
                                                                        <th class="center-align">Voltage</th>
                                                                        <th class="center-align">Quantity</th>
                                                                        <th class="center-align">Action</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <asp:HiddenField ID="edLoadID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                    <td class="text-center">
                                                                        <asp:TextBox ID="edLoadDesc" runat="server" ClientIDMode="Static" Text='<%# Eval("LoadDesc") %>' AutoPostBack="true" OnTextChanged="editSystemAvailablity_TextChanged"  TabIndex="12" />
                                                                    </td>
                                                                    <td class="text-center">
                                                                        <asp:TextBox ID="edCapacity" runat="server" ClientIDMode="Static" Text='<%# Eval("Capacity") %>' AutoPostBack="true" OnTextChanged="editSystemAvailablity_TextChanged" TabIndex="12" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edVoltage" runat="server" ClientIDMode="Static" Text='<%# Eval("Voltage") %>' AutoPostBack="true" OnTextChanged="editSystemAvailablity_TextChanged" TabIndex="15" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="edQuantity" runat="server" ClientIDMode="Static" Text='<%# Eval("Quantity") %>' AutoPostBack="true" OnTextChanged="editSystemAvailablity_TextChanged" TabIndex="16" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="27" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="background-color: #dde8f3; border-bottom: 5px solid navy;">
                                                                    <td class="left-align">
                                                                        <asp:HiddenField ID="hdnLoadID" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtLoadDesc" runat="server" ClientIDMode="Static" class="form-control" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:TextBox ID="txtCapacity" runat="server" ClientIDMode="Static" TabIndex="29" />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="txtVoltage" runat="server" ClientIDMode="Static"  TabIndex="31"  />
                                                                    </td>
                                                                    <td class="right-align">
                                                                        <asp:TextBox ID="txtQuantity" runat="server" ClientIDMode="Static" TabIndex="32" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="110" Height="30" TabIndex="42" />
                                                                    </td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                         <div id="pnl_ReqDe" class="col m12 mt-1" runat="server" clientidmode="Static" enableviewstate="true" style="display: block;">
                                            <asp:Panel runat="server" ID="Panel3">
                                                <div class="row">
                                                    <table id="tblRequiredEngDetails" class="table responsive-table striped width-100">
                                                        <asp:Repeater ID="rptRequiredEngDetails" runat="server" ClientIDMode="Static" OnItemCommand="rptRequiredEngDetails_ItemCommand" OnItemDataBound="rptRequiredEngDetails_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <thead>
                                                                    <tr>
                                                                        <th class="center-align">Description</th>
                                                                        <th class="center-align">Remarks</th>
                                                                        <th class="center-align">Action</th>
                                                                    </tr>
                                                                </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <asp:HiddenField ID="edDescID" runat="server" ClientIDMode="Static" Value='<%# Eval("pkID") %>' />
                                                                    <td class="text-center">
                                                                        <asp:TextBox ID="edDesc" runat="server" ClientIDMode="Static" Text='<%# Eval("Description") %>' AutoPostBack="true" OnTextChanged="editRequiredEng_TextChanged"  TabIndex="12" />
                                                                    </td>
                                                                    <td class="text-center">
                                                                        <asp:TextBox ID="edRemarks" runat="server" ClientIDMode="Static" Text='<%# Eval("Remarks") %>' AutoPostBack="true" OnTextChanged="editRequiredEng_TextChanged" TabIndex="12" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="ImgbtnDelete" runat="server" ClientIDMode="Static" ImageUrl="~/images/delete.png" ToolTip="Delete" CommandName="Delete" CommandArgument='<%# Eval("pkID") %>' Width="20" Height="20" TabIndex="27" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr style="background-color: #dde8f3; border-bottom: 5px solid navy;">
                                                                    <td class="left-align">
                                                                        <asp:HiddenField ID="hdnDescID" runat="server" ClientIDMode="Static" />
                                                                        <asp:TextBox ID="txtDesc" runat="server" ClientIDMode="Static" class="form-control" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" TabIndex="29" />
                                                                    </td>
                                                                    <td class="center-align">
                                                                        <asp:ImageButton ID="imgBtnSave" runat="server" ImageUrl="~/images/buttons/bt-add2.png" ToolTip="Add Item" CommandName="Save" CommandArgument='0' Width="110" Height="30" TabIndex="42" />
                                                                    </td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row padding-1 gradient-45deg-blue-grey-blue-grey vertical-align-bottom" style="margin-top: 10px;">
                <div class="col m12">
                    <button id="btnReset" type="button" runat="server" clientidmode="Static" class="btn orange left" onserverclick="btnReset_ServerClick" tabindex="11"><i class="material-icons left">backspace</i>Clear and Add new</button>
                    <button id="btnSave" type="button" runat="server" clientidmode="Static" class="btn cyan right mr-1" onserverclick="btnSave_ServerClick" tabindex="10"><i class="material-icons left">save</i>Save</button>
                </div>
            </div>
            <%--**************************** Customer Popup ***********************************--%>
            <asp:Button ID="Button1" CssClass="hide" runat="server" Text="Fill Form in Popup" />
            <cc1:ModalPopupExtender ID="myModalPopup" BehaviorID="mpe" runat="server" ClientIDMode="Static" PopupControlID="Panl1" TargetControlID="Button1" CancelControlID="Button2" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panl1" runat="server" CssClass="Popup" align="center" Style="display: none; border-radius: 10px; padding-right: 10px !important;">
                <div id="myModal123">
                    <div class="modal-header position-sticky gradient-45deg-light-blue-indigo m-0" style="margin-bottom: 10px !important;">
                        <h5 class=" z-depth-5" style="font-size: 2rem !important; font-weight: 800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
                            <i class="material-icons prefix">ac_unit</i>
                            <span id="spnModuleHeader" runat="server" clientidmode="static">Manage Module</span>
                            <asp:Button CssClass="btnTopRightCorner float-right" ID="Button2" runat="server" Text="" />
                        </h5>
                    </div>
                </div>
                <iframe id="ifrModule" src="about:blank" runat="server" clientidmode="static" scrolling="auto" style="border: 1px solid silver; background-color: transparent; width: 100%; height: 90%; padding: 5px;"></iframe>
                <br />
            </asp:Panel>
            <%-- *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-* --%>
            <script type="text/javascript">
                $("input").keypress(function (event) {
                    if (event.which == 13) {
                        event.preventDefault();
                        //$("form").submit();
                    }
                });

                $("#txtProductName").focusin(function () {
                    window.scrollBy(0, 150);
                });

                function bindDDLTo(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterCustomer",
                            data: '{pCustName:\'' + $(selector).val() + '\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                var sample = JSON.parse(data.d);
                                $(selector).autoComplete({
                                    minLength: 1,
                                    source: function (term, suggest) {
                                        term = term.toLowerCase();
                                        var choices = sample;
                                        suggest(choices);
                                    },
                                    renderItem: function (item, search) {
                                        $(".autocomplete-suggestion").remove();
                                        return '<div class="autocomplete-suggestion" data-langname="' + item.CustomerID + '" data-lang="' + item.CustomerName + '" data-val="' + search + '">' + item.CustomerName + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnCustomerID").val(item.data('langname'));
                                        $("#hdnBlockCustomer").val(item.data('BlockCustomer'));
                                        $("#drpInquiryStatus").focus();
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
                            $("#hdnCustomerID").val('');
                        }
                    }
                }

                function bindDDLProductTo(selector) {
                    if ($(selector).val().length >= 3) {
                        jQuery.ajax({
                            type: "POST",
                            url: "InquiryInfo.aspx/FilterProduct",
                            data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeQuotation\'}',
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                var sample = JSON.parse(data.d);
                                $(selector).autoComplete({
                                    minLength: 1,
                                    source: function (term, suggest) {
                                        term = term.toLowerCase();
                                        var choices = sample;
                                        suggest(choices);
                                    },
                                    renderItem: function (item, search) {
                                        $(".autocomplete-suggestion").remove();
                                        return '<div class="autocomplete-suggestion" data-langname="' + item.pkID + '" data-lang="' + item.ProductNameLong + '" data-val="' + search + '">' + item.ProductNameLong + '</div>';
                                    },
                                    onSelect: function (e, term, item) {
                                        console.log('Item "' + item.data('langname') + ' (' + item.data('lang') + ')" selected by ' + (e.type == 'keydown' ? 'pressing enter or tab' : 'mouse click') + '.');
                                        $(selector).val(item.html());
                                        $("#hdnProductID").val(item.data('langname'));
                                        $("#txtReferenceName").focus();
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
                            $("#hdnProductID").val('');
                        }
                    }
                }

                function clearProductField() {
                    $("#hdnProductID").val('');
                    $("#txtProductName").val('');
                    $("#txtUnitPrice").val('');
                    $("#txtQuantity").val('');
                    $("#lblAmount").val('');
                    $("#txtProductName").focus();
                }

                $('#btnReset').focusout(function () {
                    $('#txtInquiryNo').focus();
                    //alert('reset button focusout invoke');
                });

                $('#btnCloseModel').focusout(function () {
                    closeSetTime();
                });
            </script>
        </div>
    </form>
</body>
</html>
