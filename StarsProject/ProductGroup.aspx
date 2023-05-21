<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ProductGroup.aspx.cs" Inherits="StarsProject.ProductGroup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link  href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="js/jquery-3.3.1.js"></script>
    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
    <link href="css/Registration.css" rel="stylesheet" type="text/css"/>
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css"/>

    <style type="text/css">
        /*.clDiv70 { height:50px; vertical-align:middle; }
        .imgChart { vertical-align:middle; Height:40px; width:40px }
        #drpSelectDepartment { margin:7px 0px; font-family: Verdana,Arial; font-size:12px; vertical-align:middle;}
        .clTrans { background-color: transparent; }*/

    </style>
    <style type="text/css">
        .required:after {
            content: " *";
            color: #f00;
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
            background-color:forestgreen;
        }
        /*---------------------------------------------------------------*/ 
         
    </style>
    <script type="text/javascript">
        // ------------------------------------------------------------------------
        //  Open Bootstrap Modal Popup 
        // ------------------------------------------------------------------------
        function validateForm() {
            var errmsg = "";
            var validflag = true;
            // ----------------------------------------
            if (jQuery("#txtProductGroupName").val() == "") {

                if (jQuery("#txtProductGroupName").val() == "")
                    errmsg += "Product Group Name is required</br>";

                jQuery("#divErrorMessage").html(errmsg).show();

                validflag = false;
            }
            // ----------------------------------------
            if (validflag == true) {
                btnCloseDialog();
                
            }
            return validflag;
        }
        // -------------------------------------------------------
        function openFormContainerNew() {
            clearFormData();
            // ------------------------------------------
            jQuery(".usermsg").empty();
            jQuery('#myModal').modal({ "show": true });
        }

        function openFormContainer() {
            jQuery('#myModal').modal({ "show": true });
        }

        function btnCloseDialog() {
            jQuery('#myModal').modal('hide'); 
            jQuery('#myModal .modal-backdrop').remove();
        };

        function clearFormData() {
            jQuery(".usermsg").empty();
            jQuery('#form1').find('input:text, input:password, input:file, select, textarea').val('');
            jQuery("#hdnpkID").val('');
        }
    </script>
</head>
<body class="loginpage">
    <form id="frmEntry" runat="server">
    <asp:ScriptManager ID="srcUser" runat="server"></asp:ScriptManager>

    <div id="contentwrapper" class="contentwrapper">
    
        <asp:UpdatePanel ID="upUserPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="widgetbox">
            <asp:HiddenField ID="hdnpkID" runat="server" ClientIDMode="Static" />
            <div id="myModal" style="display:block; width:100%;">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <div class="input-field col m">
                                <label class="active required" for="txtProductGroupName">Product Group Name</label>
                                <asp:TextBox ID="txtProductGroupName" runat="server" class="form-control" ClientIDMode="Static" TabIndex="1" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m">
                                <label class="form-check-label" for="chkActive">Active Status</label>
                                <div class="form-check p-0">
                                    <div class="checkbox-container yellow">
                                        <asp:CheckBox ID="chkActive" runat="server" ClientIDMode="Static" TabIndex="2" />
                                        <label for="chkActive"></label>
                                        <div class="active-circle"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m12">
                                <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" Text="Save" class="btn btn-primary" OnClientClick="return validateForm();" OnClick="btnSave_Click" TabIndex="3" />
                        <asp:Button ID="btnReset" runat="server" ClientIDMode="Static" Text="Reset" class="btn btn-primary" OnClick="btnReset_Click" TabIndex="4" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>