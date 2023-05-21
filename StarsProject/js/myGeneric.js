function openDashboardBox(view, mode, id) {
    if (view == 'product') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Products";
        var pageUrl = "Products.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
    if (view == 'customer') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Customer";
        var pageUrl = "Customers.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
    if (view == 'todo') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Task Activity";
        var pageUrl = "ToDo.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
    if (view == 'followup') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Followup";
        var pageUrl = "Followup.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
    if (view == 'leave') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leave Request";
        var pageUrl = "LeaveRequest.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
    if (view == 'user') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Users";
        var pageUrl = "UserMgmt.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
    if (view == 'complaint') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Complaints";
        var pageUrl = "Complaint.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();

    }
    if (view == 'inquiry') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leads";
        var pageUrl = "InquiryInfo.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
    if (view == 'inquirybyno') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Leads";
        jQuery.ajax({
            type: "POST",
            url: 'InquiryInfo.aspx/GetInquiryNoPrimaryID',
            data: '{pInqNo:\'' + id + '\'}',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var resultdata = JSON.parse(data.d);
                var returnID = parseInt(data.d);
                if (returnID > 0) {
                    var pageUrl = "InquiryInfo.aspx?mode=view&id=" + returnID;
                    $('#ifrModule').attr('src', pageUrl);
                    $find("mpe").show();
                }
            },
            error: function (r) { alert('Error : ' + r.responseText); },
            failure: function (r) { alert('failure'); }
        });

    }
    if (view == 'quotation') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Quotation";
        var pageUrl = "Quotation.aspx?mode=" + mode + "&id=" + id;
        $('#ifrModule').attr('src', pageUrl);
        $find("mpe").show();
    }
    if (view == 'quotationbyno') {
        Sys.UI.DomElement.getElementById('<%= spnModuleHeader.ClientID%>').innerHTML = "Manage Quotation";
        jQuery.ajax({
            type: "POST",
            url: 'Quotation.aspx/GetQuotationNoPrimaryID',
            data: '{pQuotationNo:\'' + id + '\'}',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var id;
                var resultdata = JSON.parse(data.d);
                var returnID = parseInt(data.d);
                if (returnID > 0) {
                    var pageUrl = "Quotation.aspx?mode=view&id=" + returnID;
                    $('#ifrModule').attr('src', pageUrl);
                    $find("mpe").show();
                }
            },
            error: function (r) { alert('Error : ' + r.responseText); },
            failure: function (r) { alert('failure'); }
        });
    }
}



