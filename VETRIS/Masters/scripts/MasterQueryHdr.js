function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    //var strUser = parent.Trim(document.getElementById("hdnUsr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true", strUser);
    }

}
function grdBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[0].get_value();
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdBrw_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdBrw_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}