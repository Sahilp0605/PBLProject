<%@ Page Title="" Language="C#" MasterPageFile="~/StarsSite.Master" AutoEventWireup="true" CodeBehind="CustomerSearch.aspx.cs" Inherits="StarsProject.CustomerSearch" %>
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
<%--    <script type="text/javascript" src="js/dataValidation.js"></script>
    <script type="text/javascript" src="js/myGeneric.js"></script>--%>
    <script type="text/javascript" src="js/jquerysession.js"></script>
    <style type="text/css">
        .dataTables_filter { display:none; }
    </style>
    <script type="text/javascript">
        function openCustomerInfo(cat, keyid) {
            if (keyid != '' && keyid != undefined) {
                jQuery.colorbox({ width: "100%", height: "100%", iframe: true, href: "customers.aspx?mode=" + cat + "&id=" + keyid });
            }
        }

        function loadCustomer(xCustName, xType, xSource, xContact, xEmail, xState, xCity) 
        {
            var viewmode = 'view';
            var dataurl = "/Services/NagrikService.asmx/webCustomerSearchInfo?pCustName=" + xCustName + "&pType=" + xType + "&pSource=" + xSource + "&pContact=" + xContact + "&pEmail=" + xEmail + "&pState=" + xState + "&pCity=" + xCity;
            var ClientERPIntegration = jQuery.trim(jQuery("#hdnClientERPIntegration").val());
            if (ClientERPIntegration == "Yes") {
                colstructure = [
                {
                    title: "Customer Name", data: "CustomerName", className: "text-left", width: "300",
                    render: function (data, type, row, meta) {
                        data = '<a href="javascript:gridAction(\'view\',\'' + row.CustomerID + '\');">' + data + '</a>';
                        return data;
                    }
                },
                { title: "Category", data: "CustomerType", className: "text-center", width: "130" },
                { title: "Source", data: "CustomerSourceName", className: "text-center", width: "160" },
                { title: "City", data: "CityName", className: "text-center", width: "110" },
                { title: "State", data: "StateName", className: "text-center", width: "110" },
                { title: "Contact #1", data: "ContactNo1", className: "text-center", width: "110" },
                { title: "Contact #2", data: "ContactNo2", className: "text-center", width: "110" },
                { title: "Email Address", data: "EmailAddress", className: "text-center", width: "150" },
                { title: "Closing", data: "ErpClosing", className: "text-right" },
                { title: "Edit", className: "text-center", width: "30", mRender: function (data, type, row) {
                    return '<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',' + row.CustomerID + ');"/>';
                }},
                { title: "Delete", className: "text-center", width: "30", mRender: function (data, type, row) {
                    return '<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',' + row.CustomerID + ');"/>';
                }}
                ];
            }
            else {
                colstructure = [
                {
                    title: "Customer Name", data: "CustomerName", className: "text-left", width: "300",
                    render: function (data, type, row, meta) {
                        data = '<a href="javascript:gridAction(\'view\',\'' + row.CustomerID + '\');">' + data + '</a>';
                        return data;
                    }
                },
                { title: "Category", data: "CustomerType", className: "text-center", width: "130" },
                { title: "Source", data: "CustomerSourceName", className: "text-center", width: "160" },
                { title: "City", data: "CityName", className: "text-center", width: "110" },
                { title: "State", data: "StateName", className: "text-center", width: "110" },
                { title: "Contact #1", data: "ContactNo1", className: "text-center", width: "110" },
                { title: "Contact #2", data: "ContactNo2", className: "text-center", width: "110" },
                { title: "Email Address", data: "EmailAddress", className: "text-center", width: "150" },
                { title: "Edit", className: "text-center", width: "30", mRender: function (data, type, row) {
                        return '<img src="images/edit.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'edit\',' + row.CustomerID + ');"/>';
                    }},
                { title: "Delete", className: "text-center", width: "30", mRender: function (data, type, row) {
                        return '<img src="images/delete.png" alt="" style="width:15px; height:15px;" onclick="gridAction(\'delete\',' + row.CustomerID + ');"/>';
                    }}
                ];
            }
            colstructure.push();

            colstructure.push();
            // -------------------------------------------------------------------------
            // Design and Load Table as per above configuration 
            // -------------------------------------------------------------------------
            jQuery.ajax({
                url: dataurl,
                type: "get",
                success: function (data) {
                    var resultdata = JSON.parse(data.getElementsByTagName("string")[0].innerHTML);
                    var sortingDef = [[0, "asc"]];
                    // --------------------------------------------------
                    // Loading and Drawing Datatable .... From Database
                    // --------------------------------------------------
                    $('.clWaitingModal').show();
                    jQuery('#tblView')
                    .on('page.dt', function () {
                        var info = jQuery("#tblView").DataTable().page.info();
                        $.session.set('tblCurrPage', (info.page + 1));
                    })
                    .DataTable({
                        order: sortingDef,
                        pageLength: parseFloat(jQuery("#currPageSize").val()),
                        data: resultdata,
                        columns: colstructure,
                        createdRow: function (row, data, dataIndex) {
                            setTableRowFormat(viewmode, row, data, dataIndex)
                        }
                    });
                    $('.clWaitingModal').hide();
                    // ----------------------------------------------
                    var currPage = $.session.get('tblCurrPage');
                    if (typeof currPage === 'undefined')        // Set default to First Page
                        jQuery('#tblView').dataTable().fnPageChange(0, true);
                    else
                        jQuery('#tblView').dataTable().fnPageChange(currPage - 1, true);
                },
                error: function (r) {
                    alert('Error : ' + r.responseText);
                },
                failure: function (r) {
                    alert('failure');
                }
            });
        }
        // -----------------------------------------------------
        function setTableRowFormat(viewmode, row, data, dataIndex) {
        }
        // -----------------------------------------------------
        function gridAction(mode, id)
        {
            var cbWidth = "90%";
            var cbHeight = "90%";
            var varView = jQuery("#hdnView").val();
            var pageUrl = '', deleteUrl = '', deletePara = '';
            // -------------------------------------------------------------
            pageUrl = "Customers.aspx?mode=" + mode + "&id=" + id;
            deleteUrl = 'Customers.aspx/DeleteCustomer';
            deletePara = '{pkID:' + id + '}';
            // -------------------------------------------------------------
            // Section : Call page for ADD / EDIT
            // -------------------------------------------------------------
            if (mode == 'add' || mode == 'edit' || mode == 'view') {
                $.colorbox({
                    width: cbWidth, height: cbHeight, iframe: true, href: pageUrl,
                    onClosed: function () {
                        //if (mode != 'view') { location.reload(false); }
                    }
                });
            }
            // -------------------------------------------------------------
            // Section : Call page for DELETE 
            // -------------------------------------------------------------
            if (mode == 'delete') {
                jQuery.confirm({
                    title: 'Delete Record',
                    content: 'Are you sure, You want to delete record !',
                    type: 'red',
                    typeAnimated: true,
                    buttons: {
                        tryAgain: {
                            text: 'Yes',
                            btnClass: 'btn-red',
                            action: function () {
                                jQuery.ajax({
                                    type: "POST",
                                    url: deleteUrl,
                                    data: deletePara,
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        var resultdata = JSON.parse(data.d);
                                        alert(resultdata[0].ReturnMsg)
                                        // -----------------------------------
                                        location.reload();
                                    },
                                    error: function (r) { alert('Error : ' + r.responseText); },
                                    failure: function (r) { alert('failure'); }
                                });
                            }
                        },
                        close: {
                            text: 'No',
                            btnClass: 'btn-blue',
                            action: function () { }
                        }
                    }
                });

            }
 

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card bg-info-gradient" style="background: #17a2b8; background: -webkit-gradient(linear,left bottom,left top,color-stop(0,#17a2b8),color-stop(1,#89d8e4));">
        <div class="card-header no-border" style="padding: 5px 10px;">
            <h5>
                <i class="fa fa-th mr-1"></i><span id="spnPageHeader" runat="server" clientidmode="Static" class="text-top" style="color: white; margin-left: 15px;">Manage Customer</span>
                <asp:Button ID="btnDownload" runat="server" ClientIDMode="Static" CssClass="btn btn-danger float-right ml-1 mr-1" style="padding: 3px 25px !important;" Text="Download" OnClick="btnDownload_Click" />
                <asp:Button ID="btnUpload" runat="server" ClientIDMode="Static" CssClass="btn btn-danger float-right ml-1 mr-1" style="padding: 3px 25px !important;" Text="Upload" OnClick="btnUpload_Click" />
                <input id="myAddNew" type="button" class="btn btn-danger float-right" style="padding: 3px 25px !important;" value="Add New" onclick="gridAction('add', 0);" />
            </h5>
        </div>
    </div>
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>
    <div id="contentwrapper" class="contentwrapper">
        <asp:HiddenField ID="hdnLoginUserID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnActionFlag" runat="server" ClientIDMode="Static" Value="false" />
        <asp:HiddenField ID="hdnView" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnKeyID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTitle" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPara1" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPara2" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPara3" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnClientERPIntegration" runat="server" ClientIDMode="Static" />

        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="input-field col m">
                    <label id="lblCustomerName" runat="server" clientidmode="Static" for="txtCustomerName" class="required">Customer Name </label>
                    <asp:TextBox ID="txtCustomerName" runat="server" class="form-control" ClientIDMode="Static" MaxLength="100" TabIndex="1" />
                </div>
                <div class="input-field col m">
                    <label class="active" for="drpCustomerType">Category</label>
                    <asp:DropDownList ID="drpCustomerType" runat="server" ClientIDMode="Static" class="form-control" MaxLength="50" TabIndex="2" style="height:inherit;"></asp:DropDownList>
                </div>
                <div class="input-field col m">
                    <label class="active" for="drpCustomerSource">Source</label>
                    <asp:DropDownList ID="drpCustomerSource" runat="server" ClientIDMode="Static" class="form-control" MaxLength="50" TabIndex="2" style="height:inherit;"></asp:DropDownList>
                </div>
                <div class="input-field col m">
                    <label id="Label2" runat="server" clientidmode="Static" for="txtEmail" class="required">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" class="form-control" ClientIDMode="Static" MaxLength="100" TabIndex="1" />
                </div>
                <div class="input-field col m">
                    <label id="Label1" runat="server" clientidmode="Static" for="txtContact" class="required">Contact #</label>
                    <asp:TextBox ID="txtContact" runat="server" class="form-control" ClientIDMode="Static" MaxLength="100" TabIndex="1" />
                </div>
                <div class="input-field col m">
                    <label class="active" for="drpState">State</label>
                    <asp:DropDownList ID="drpState" runat="server"  ClientIDMode="Static" TabIndex="16" class="form-control" style="height:inherit;" AutoPostBack="true" OnSelectedIndexChanged="drpState_SelectedIndexChanged"/>
                </div>
                <div class="input-field col m">
                    <label class="active" for="drpCity">City</label>
                    <asp:DropDownList ID="drpCity" runat="server"  ClientIDMode="Static" TabIndex="16" class="form-control" style="height:inherit;" />
                </div>
            </div>
            <div class="row">
                <div class="input-field col m">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-primary" ToolTip="Save" onclick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" class="btn btn-danger" ToolTip="Reset" onclick="btnReset_Click" />
                </div>
            </div>
            <%--*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*---%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ClientIDMode="Static">
            <ContentTemplate>
                    <asp:HiddenField ID="currPageSize" runat="server" ClientIDMode="Static" EnableViewState="true" Value="10" />
                    <table id="tblView" class="stdtable" width="100%"></table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
