
function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    //var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    //if (strErr != "") {
    //    parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    //}
}
function grdBrw_onItemSelect(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
}

function grdBrw_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdBrw_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}
