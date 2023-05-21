<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myNotificationPanel.ascx.cs" Inherits="StarsProject.myNotificationPanel" %>

<style type="text/css">
    #chkSendEmail {
        position: absolute !important;
        pointer-events: all !important;
        opacity: 100 !important;
        width: 20px !important;
        height: 20px !important;
    }
    #chkSendSMS {
        position: absolute !important;
        pointer-events: all !important;
        opacity: 100 !important;
        width: 20px !important;
        height: 20px !important;
    }
</style>
<script type="text/javascript">
    $(document).ready(function() {
        $("#chkSendEmail").change(function () {
            $("#hdnSendEmail").val($("#chkSendEmail").is(':checked'));
            if ($("#chkSendEmail").is(':checked') == false)
            {
                $("#txtEmailTo").attr('disabled', 'disabled');
                $("#txtEmailCC").attr('disabled', 'disabled');
                $("#txtEmailBCC").attr('disabled', 'disabled');
            }
            else {
                $("#txtEmailTo").removeAttr('disabled');
                $("#txtEmailCC").removeAttr('disabled');
                $("#txtEmailBCC").removeAttr('disabled');
            }
        });

        $("#chkSendSMS").click(function () {
            
            $("#hdnSendSMS").val($("#chkSendSMS").is(':checked'));

            if ($("#chkSendSMS").is(':checked') == false) {
                $("#txtSMSTo").attr('disabled', 'disabled');
                $("#txtSMSCC").attr('disabled', 'disabled');
            }
            else {
                $("#txtSMSTo").removeAttr('disabled');
                $("#txtSMSCC").removeAttr('disabled');
            }

        });

        $("#chkWhatsApp").change(function () {
            $("#hdnSendWhatsapp").val($("#chkWhatsApp").is(':checked'));
        });
    });
</script>
<asp:HiddenField ID="hdnNotiCustomerID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnSendEmail" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnSendSMS" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnSendWhatsapp" runat="server" ClientIDMode="Static" />
<div class="modal-content">
    <h5 class=" z-depth-5" style="position: fixed;top: 0; left:0px; width:100%; font-size: 2rem !important; font-weight:800; line-height: 50px; margin: 0; text-align: left; padding-left: 15px;">
        <i class="material-icons prefix">ac_unit</i>
        <span id="Span1" runat="server" clientidmode="static">Save & Send Notification</span>
        <asp:Button CssClass="btnTopRightCorner float-right" ID="ucNotificationClose" runat="server" Text=""/>
    </h5>
    <div class="row mt-10">
        <div class="col m12">
            <div class="col s18 m6 l6">
                <div class="col m8"><h5 class="blue-text">Email Configuration</h5></div>
                <div class="col m3"><label class="right"><input id="chkSendEmail" type="checkbox" /></label></div>
                <div class="card animate fadeLeft col m12">
                    <div class="card-badge"><a class="white-text"><b>On Offer</b> </a></div>
                    <div class="card-content">
                        <div class="input-field col m12">
                            <label class="active" for="txtEmailTo">Email To,</label>
                            <asp:TextBox ID="txtEmailTo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" Enabled="false" />
                        </div>
                        <div class="input-field col m12">
                            <label class="active" for="txtEmailCC">Email CC,</label>
                            <asp:TextBox ID="txtEmailCC" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" Enabled="false"  />
                        </div>
                        <div class="input-field col m12">
                            <label class="active" for="txtEmailBCC">Email BCC</label>
                            <asp:TextBox ID="txtEmailBCC" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" Enabled="false" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col s18 m6 l6">
                <div class="col m8"><h5 class="blue-text">SMS Configuration</h5></div>
                <div class="col m3"><label class="right"><input id="chkSendSMS" type="checkbox" /></label></div>
                <div class="card animate fadeLeft col m12">
                    <div class="card-badge"><a class="white-text"><b>On Offer</b> </a></div>
                    <div class="card-content">
                        <div class="input-field col m12">
                            <label class="active" for="txtSMSTo">To</label>
                            <asp:TextBox ID="txtSMSTo" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" Enabled="false" />
                        </div>
                        <div class="input-field col m12">
                            <label class="active" for="txtSMSCC">Additional</label>
                            <asp:TextBox ID="txtSMSCC" runat="server" class="form-control" ClientIDMode="Static" TabIndex="2" placeholder="" Enabled="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <hr class="mt-1 mb-1">
    <div class="row">
        <div class="col m12">
            <%--<a class="waves-effect waves-light btn gradient-45deg-deep-purple-blue mt-2 mr-2">Send Notification</a>--%>
            <button ID="btnFSaveEmail" type="button" runat="server" clientidmode="Static" text="Save" class="waves-effect waves-green btn-flat right" onserverclick="btnFSaveEmail_Click" tabindex="12"><i class="material-icons left">email</i>Save & Send Notification</button>
        </div>
    </div>
</div>