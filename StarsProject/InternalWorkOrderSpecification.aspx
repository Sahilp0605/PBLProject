<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternalWorkOrderSpecification.aspx.cs" Inherits="StarsProject.InternalWorkOrderSpecification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />

    <script type="text/javascript" src="js/jquery-3.3.1.js"></script>
    <script src="plugins/jQueryUI/jquery-ui.min.js"></script>
    <%--<link rel="stylesheet" type="text/css" href="https://code.jquery.com/ui/1.11.2/jquery-ui.min.js" />--%>
    <script type="text/javascript" src="plugins/daterangepicker/moment.js"></script>
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="plugins/datatables/jquery.dataTables.min.js"></script>

    <link href="css/font-awesome-Editor.min.css" rel="stylesheet" />
    <link href="css/richtext.min.css" rel="stylesheet" />
    <script src="js/jquery.richtext.min.js"></script>
    <style type="text/css">
        .jqte_editor { height:250px !important; }

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

        .autocomplete-suggestions {
            min-width: 600px !important;
        }

        .lblOther {
            font-size: 14px;
        }

        .drpOther {
            width: 100%;
            font-size: 13px;
        }

        .inputOther {
            width: 100px;
            font-size: 13px;
            text-align: right;
            height: 30px;
        }

        .inputCalc {
            width: 140px;
            text-align: right;
            height: 27px;
        }

        .active {
            font-size: 15px;
        }

        .radio-inline label {
            color: white !important;
            margin-left: 5px !important;
            margin-right: 20px !important;
        }

        /* ------------------------------------------------------- */
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
            padding-top: 5px;
            padding-bottom: 5px;
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
            background-color: #fbf5f5;
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

        .nav-tabs .nav-item.show .nav-link, .nav-tabs .nav-link.active {
            background-color: #6cd883;
            color: Navy;
        }

        #myTabContent {
            border-left: 1px solid navy;
            border-right: 1px solid navy;
            border-bottom: 1px solid navy;
        }
    </style>

        <script type="text/javascript">
            $(document).ready(function () {
                myTabControl();

                $(".content").richText();

            });
        </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="srcQuotSpec" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hdnDocNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnFinishProductID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnModule" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSpecRemark" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnSpecFormat" runat="server" ClientIDMode="Static" />
<%--        <asp:UpdatePanel ID="updProdSpecs" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
                <div id="myModal" style="display: block; width: 100%;">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row mt-2">
                                <div class="input-field col m12">
                                    <div class="profile-head">
                                        <ul class="nav nav-tabs" id="myTab" role="tablist">
                                            <li class="nav-item">
                                                <a class="nav-link active" id="scopeofwork-tab" data-toggle="tab" href="#scopeofwork" role="tab" aria-controls="scopeofwork" aria-selected="true">Scope Of Work</a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" id="scopeClient-tab" data-toggle="tab" href="#scopeclient" role="tab" aria-controls="scopeclient" aria-selected="false">Scope Of Work - Client</a>
                                            </li>
                                            <li class="nav-item">
                                                <a class="nav-link" id="prodremark-tab" data-toggle="tab" href="#prodremark" role="tab" aria-controls="prodremark" aria-selected="false">Product Remarks</a>
                                            </li>
                                            <%--<li class="nav-item">
                                                <a class="nav-link" id="prodspec-tab" data-toggle="tab" href="#prodspec" role="tab" aria-controls="prodspec" aria-selected="false">Product Specification</a>
                                            </li>--%>
                                        </ul>
                                    </div>
                                    <div class="tab-content profile-tab" id="myTabContent" style="height: 322px; overflow-y: auto;">
                                        <div class="tab-pane fade show active" id="scopeofwork" role="tabpanel" aria-labelledby="scopeofwork-tab">
                                            <section class="our-webcoderskull padding-lg" style="height: 320px; overflow-y: auto;">
                                                <div class="container" style="max-width: none; width: 98%; margin: 5px !important;">
                                                    <div id="divScope" runat="server" clientidmode="Static" class="input-field col m">
                                                        <asp:TextBox ID="txtScope" runat="server" class="content" ClientIDMode="Static" TabIndex="17" TextMode="MultiLine" Rows="7" MaxLength="500" style="height:7rem !important;" />
                                                    </div>
                                                </div>
                                            </section>
                                        </div>
                                    
                                        <div class="tab-pane fade " id="scopeclient" role="tabpanel" aria-labelledby="scopeClient-tab">
                                            <section class="our-webcoderskull padding-lg" style="height: 320px; overflow-y: auto;">
                                                <div class="container" style="max-width: none; width: 98%; margin: 5px !important;">
                                                    <div id="divScopeClient" runat="server" clientidmode="Static" class="input-field col m">
                                                        <asp:TextBox ID="txtScopeClient" runat="server" class="content" ClientIDMode="Static" TabIndex="17" TextMode="MultiLine" Rows="7" MaxLength="500" style="height:7rem !important;" />
                                                    </div>
                                                </div>
                                            </section>
                                        </div>
                                    
                                        <div class="tab-pane fade " id="prodremark" role="tabpanel" aria-labelledby="prodremark-tab">
                                            <section class="our-webcoderskull padding-lg" style="height: 320px; overflow-y: auto;">
                                                <div class="container" style="max-width: none; width: 98%; margin: 5px !important;">
                                                    <div id="divRemarks" runat="server" clientidmode="Static" class="input-field col m">
                                                        <asp:TextBox ID="txtRemarks" runat="server" class="content" ClientIDMode="Static" TabIndex="17" TextMode="MultiLine" Rows="7" MaxLength="500" style="height:7rem !important;" />
                                                    </div>
                                                </div>
                                            </section>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="input-field col m12">
                                <div id="divErrorMessage" class="usermsg" runat="server" clientidmode="Static"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="input-field col m12">
                                <center>
                                    <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/SaveData.png" ToolTip="Save Specification" Width="100" Height="35" TabIndex="104" OnClick="btnSave_Click" />
                                </center>
                            </div>
                        </div>
                    </div>
                </div>

                
<%--        </ContentTemplate>
        </asp:UpdatePanel>--%>
        <script type="text/javascript">
            function myTabControl() {
                //if ($('#hdnQuatSpecRemark').val().toLowerCase() === 'yes') {
                //    $('#prodspec').css('visibility', 'hidden');
                //    $('#prodspec-tab').css('visibility', 'hidden');
                //    $('#prodremark-tab').click();
                //}
                //else {
                //    $('#prodremark').css('visibility', 'hidden');
                //    $('#prodremark-tab').css('visibility', 'hidden');
                //    $('#prodspec-tab').click();
                //}
            }
        </script>
    </form>

</body>
</html>
