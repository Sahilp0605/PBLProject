<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchProduct.aspx.cs" Inherits="StarsProject.SearchProduct" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src='<%=ResolveUrl("js/plugins/jquery-1.7.min.js") %>'></script>
    <link  href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/bootstrap/js/bootstrap.min.js"></script>

    <link href="plugins/datatables/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="plugins/datatables/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://code.jquery.com/ui/1.11.2/jquery-ui.min.js" />

    <link href="css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="css/PageReSetup.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.auto-complete.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery.auto-complete.min.js"></script>

    <style type="text/css">
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
            background-color: #e8e8e8;
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
            width: 300px;
            height: 180px;
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
    </style>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <asp:HiddenField ID="hdnProductID" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnClientERPIntegration" runat="server" ClientIDMode="Static" />
        <div id="myModal" style="display:block; width:100%;">
            <div class="modal-content">
                <div class="modal-header" style="padding:10px 10px 0px 10px;">
                    <div class="row mb-0">
                        <div class="input-field col m12">
                            <label class="active" for="txtProductName" style="color:white !important;">Product Name  <small class="text-muted font-italic" style="color:springgreen !important;">(Minimum 3 chars to activate search)</small></label>
                            <asp:TextBox ID="txtProductName" runat="server" ClientIDMode="Static" class="form-control" AutoPostBack="true" onKeyup="bindDDLProductTo('#txtProductName')" OnTextChanged="txtProductName_TextChanged" TabIndex="3" style="width:600px;" />
                        </div>
                    </div>
                </div>
                <div class="modal-body" style="min-height:500px; padding:0px 5px;">
                    <div class="row">
                        <div id="showProduct" class="col m12 mt-2 p-1" style="color:navy; border-top-left-radius:0.5rem; border-bottom-left-radius:0.5rem; border-top-right-radius:0.5rem; border-bottom-right-radius:0.5rem;">
                            <div id="Product-content">
                                <div class="emp-profile" style="padding:10px;">
                                    <div class="row">
                                        <div class="col m4">
                                            <div class="profile-work">
                                                <h5 style="margin-bottom:18px;">Product Detail</h5>
                                                <section class="our-webcoderskull" style="height:400px; padding:10px; overflow-y:auto;">
                                                        <div class="row">
                                                            <div class="col m12"><label class="mb-0">Product Alias</label></div>
                                                            <div class="col m12">
                                                                <small class="badge badge-success" style="min-width: 100%;min-height: 20px;"><i class="fa fa-calendar-check-o"></i>
                                                                    <span id="lblProductAlias" runat="server" clientidmode="Static"></span>
                                                                </small>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col m12"><label class="mb-0">Product Brand</label></div>
                                                            <div class="col m12">
                                                                <small class="badge badge-primary" style="min-width: 100%;min-height: 20px;"><i class="fa fa-calendar-check-o"></i>
                                                                <span ID="lblProductBrand" runat="server" clientidmode="Static"></span>
                                                                </small>
                                                            </div>
                                                        </div>
                                                        <div class="row mb-1">
                                                            <div class="col m12"><label class="mb-0">Category/Group</label></div>
                                                            <div class="col m12">
                                                                <small class="badge badge-primary" style="min-width: 100%;min-height: 20px;"><i class="fa fa-calendar-check-o"></i>
                                                                    <span ID="lblProductGroup" runat="server" clientidmode="Static"></span>
                                                                </small>
                                                            </div>
                                                        </div>
                                                        <div class="row mb-1">
                                                            <div class="col m6"><label class="mb-0">Unit</label></div>
                                                            <div class="col m6">
                                                                <small class="badge badge-info" style="min-width: 100%;min-height: 20px;"><i class="fa fa-calendar-check-o"></i>
                                                                    <span ID="lblUnit" runat="server" clientidmode="Static"></span>
                                                                </small>
                                                            </div>
                                                        </div>
                                                        <div class="row mb-1">
                                                            <div class="col m6"><label class="mb-0">Unit Price</label></div>
                                                            <div class="col m6">
                                                                <small class="badge badge-info" style="min-width: 100%;min-height: 20px;"><i class="fa fa-calendar-check-o"></i>
                                                                    <span ID="lblUnitPrice" runat="server" clientidmode="Static"></span>
                                                                </small>
                                                            </div>
                                                        </div>
                                                        <div class="row mb-2">
                                                            <div class="col m6"><label class="mb-0">Tax %</label></div>
                                                            <div class="col m6">
                                                                <small class="badge badge-info" style="min-width: 100%;min-height: 20px;"><i class="fa fa-calendar-check-o"></i>
                                                                    <span ID="lblTaxRate" runat="server" clientidmode="Static"></span>
                                                                </small>
                                                            </div>
                                                        </div>
                                                        <div class="row mb-1">
                                                            <div class="col m12">
                                                                <label class="mb-0">Product Specification</label><br/>
                                                            </div>
                                                            <div class="col m12">
                                                                <small class="badge badge-primary" style="min-width: 100%;min-height: 20px;width: 40px; text-align: left;"><i class="fa fa-calendar-check-o"></i>
                                                                    <span ID="lblSpecification" runat="server" clientidmode="Static" style="white-space: normal;line-height: normal;"></span>
                                                                </small>
                                                            </div>
                                                        </div>
                                                    <% if (hdnClientERPIntegration.Value == "Yes")
                                                       { %>
                                                        <div class="row mb-2">
                                                            <div class="col m6"><label class="mb-0">Closing Stock</label></div>
                                                            <div class="col m6">
                                                                <small class="badge badge-info" style="min-width: 100%;min-height: 20px;"><i class="fa fa-calendar-check-o"></i>
                                                                    <span id="lblClosingStock" runat="server" clientidmode="Static"></span>
                                                                </small>
                                                            </div>
                                                        </div>
                                                    <% } %>
                                                    </section>
                                            </div>
                                        </div>
                                        <div class="col m8">
                                            <div class="profile-head">
                                                <ul class="nav nav-tabs" id="myTab" role="tablist">
                                                    <li class="nav-item">
                                                        <a class="nav-link active" id="home-tab" data-toggle="tab" href="#home" role="tab" aria-controls="home" aria-selected="true">Annual Statistics</a>
                                                    </li>
                                                </ul>
                                            </div>
                                            <div class="tab-content profile-tab" id="myTabContent">
                                                <div class="tab-pane fade show active" id="home" role="tabpanel" aria-labelledby="home-tab">
                                                    <section class="our-webcoderskull padding-lg" style="height:400px; overflow-y:auto;">
                                                      <div class="container" style="max-width:650px;">
                                                      </div>
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

        <script type="text/javascript">
            $("input").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    //$("form").submit();
                }
            });
            // -------------------------------------------------------------
            function bindDDLProductTo(selector) {
                if ($(selector).val().length >= 3) {
                    jQuery.ajax({
                        type: "POST",
                        url: "InquiryInfo.aspx/FilterProduct",
                        data: '{pProductName:\'' + $(selector).val() + '\', pSearchModule:\'ProductSearchTypeAll\'}',
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
                                    $("#txtQuantity").focus();
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

        </script>
    </form>
</body>
</html>
