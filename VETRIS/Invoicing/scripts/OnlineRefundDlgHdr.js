function grdInvoiceBrw_onCallbackComplete(sender, eventArgs) {
    grdInvoiceBrw.Width = "99%";
    parent.adjustFrameHeight();
    showCheckBoxes();
    //var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    //if (strErr != "") {
    //    parent.PopupMessage(RootDirectory, strForm, "grdInvoiceBrw_onCallbackComplete()", strErr, "true");
    //}
}
function grdInvoiceBrw_onRenderComplete(sender, eventArgs) {
    grdInvoiceBrw.Width = "99%";
    //parent.adjustFrameHeight();
    showCheckBoxes();
}
function showCheckBoxes() {
    var disable = (objhdnID.value != "00000000-0000-0000-0000-000000000000" || objhdnID.value == "");
    if (disable)
        $("input[id^='chkSel_']").attr('disabled', true);
    else
        $("input[id^='chkSel_']").removeAttr('disabled');
}