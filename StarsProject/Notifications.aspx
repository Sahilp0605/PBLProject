<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="StarsProject.Notifications" %>

<%@ Register Src="~/myNotifications.ascx" TagPrefix="uc1" TagName="myNotifications" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">

        .ImgbtnSelect { Width: 40px; Height: 40px; float: left;}
        .divInfo { float: left; margin: 3px; padding: 2px; border: 0px; width: 100% !important;}
        .divHead { height: 30px; float: left; padding:2px; border: 2px solid silver; background-color: #ddd; font-family: verdana,Arial,Helvetica,sans-serif; font-size: 14px; font-weight: bold; min-width: 99%; vertical-align: middle; text-align: center; }
        .stdtable { height: 168px; font-size:14px; color: Navy; font-weight: 400;}
        .stdtable th { text-align: center !important; }
        .stdtable td { text-align: center !important; }

        #map_canvas { text-align: center; float: center; min-width: 48% !important; height: 600px !important; }
        
        .stdtable tbody tr td:first-child { border-left: 0px !important; border-right: 0px !important; }
        .stdtable tbody tr td:last-child { border-left: 0px !important; border-right: 0px !important; }
      
        .boxClose { display:none; height:0px; }
        .boxOpen  { display:block;}

        .limited{
            white-space: nowrap;
            max-width:100px;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .progmodal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .progcenter
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .progcenter img
        {
            height: 128px;
            width: 128px;
        }
        .morris-hover{position:absolute;z-index:1000;}
        .morris-hover-point { color: greenyellow !important; }
        /* --------------------------------------------------------------- */
        #loader {
          position: absolute;
          left: 50%;
          top: 50%;
          z-index: 1;
          width: 150px;
          height: 150px;
          margin: -75px 0 0 -75px;
          border: 16px solid #f3f3f3;
          border-radius: 50%;
          border-top: 16px solid #3498db;
          width: 120px;
          height: 120px;
          -webkit-animation: spin 2s linear infinite;
          animation: spin 2s linear infinite;
        }

        @-webkit-keyframes spin {
          0% { -webkit-transform: rotate(0deg); }
          100% { -webkit-transform: rotate(360deg); }
        }

        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }

        /* Add animation to "page content" */
        .animate-bottom {
          position: relative;
          -webkit-animation-name: animatebottom;
          -webkit-animation-duration: 1s;
          animation-name: animatebottom;
          animation-duration: 1s
        }

        @-webkit-keyframes animatebottom {
          from { bottom:-100px; opacity:0 } 
          to { bottom:0px; opacity:1 }
        }

        @keyframes animatebottom { 
          from{ bottom:-100px; opacity:0 } 
          to{ bottom:0; opacity:1 }
        }

        #myDiv {
          display: none;
          text-align: center;
        }


    </style>


    <link type="text/css" href="css/font-awesome.css" rel="stylesheet" />
    <%--<link type="text/css" rel="stylesheet" href="dist/css/adminlte.min.css"/>--%>
    <!-- iCheck -->
    <link type="text/css" rel="stylesheet" href="plugins/iCheck/flat/blue.css"/>
    <!-- Morris chart -->
    <link type="text/css" rel="stylesheet" href="plugins/morris/morris.css"/>
    <script type="text/javascript" src="js/raphael-min.js"></script>
    <script type="text/javascript" src="plugins/morris/morris.min.js"></script>
    <script type="text/javascript" src="js/plugins/jquery.bxSlider.min.js"></script>
    <script type="text/javascript" src="plugins/knob/jquery.knob.js"></script>


     <link rel="stylesheet" href="css/style.default.css" type="text/css" />
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-ui-1.8.16.custom.min.js") %>'></script>
    <script type="text/javascript" src="js/plugins/jquery.cookie.js"></script>
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>

    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery.colorbox-min.js") %>'></script>
    <link href="css/plugins/colorbox.css" rel="stylesheet" type="text/css" />
    


</head>
<body>
    <form id="form1" runat="server">
        <div class="container"  style="min-width: 100%;">
            <div class="row mt-3">
                <div class="col m12">
                    <!-- Sales Target List -->
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--<div class="card" style="box-shadow: -6px 8px 6px -6px black;">
                                <div class="card-header" style="padding: 3px;">
                                    <h3 class="card-title" style="margin-bottom: 0px; font-size: 1.25rem; font-weight: 400;"><i class="fa fa-bullseye ml-2 mr-2"></i>
                                        Notification List
                                    </h3>
                                </div>
                                <!-- /.card-header -->
                                <div class="card-body" style="min-height:400px; overflow-y: auto;">--%>
                                    <uc1:myNotifications runat="server" ID="myNotifications"/>
                                <%--</div>
                            </div>--%>
                            <!-- /.card -->
                        </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="drpTargetType" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
