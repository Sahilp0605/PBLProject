function validatetag(appno, tagid, tagbox, flg) {

    jQuery.ajax({
        type: "POST",
        url: "handler/ValidateTag.ashx",
        data: "TagID=" + tagid + "&flag=" + flg + "&app=" + appno,
        success: function (data) {
            //alert(data);
            if (data != 'valid') {
                jQuery('#' + tagbox).val('');
                jQuery('#' + tagbox).focus();
                alert(data);
            }
        },
        error: function () {
            //alert('Error Occur');
        }
    });
}
function showloadicon() {
    jQuery('#divloadbar').css('display', 'block');
    jQuery('#canvasloader-container').css('display', 'block');
}
function hideloadicon() {
    jQuery('#divloadbar').css('display', 'none');
    jQuery('#canvasloader-container').css('display', 'none');
}