function grdFreeCredit_onCallbackComplete(sender, eventArgs) {
    grdFreeCredit.Width = "99%";
    parent.adjustFrameHeight();
    //var strErr = parent.Trim(document.getElementById("hdnCBErrBy").value);
    //if (strErr != "") {
    //    parent.PopupMessage(RootDirectory, strForm, "grdFreeCredit_onCallbackComplete()", strErr, "true");
    //}

}
function grdFreeCredit_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[1].get_value();
}
function grdFreeCredit_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var HdrRowId = "0";
    var StatusID = ""; var Approved = ""; var ApprovedHdr = ""; var Overdue = ""; var DeadLine = ""; var ActivityCount = 0;
    var ItemRecCount = 0;

}
function grdFreeCredit_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdFreeCredit_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}