var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
function grdST_onCallbackComplete(sender, eventArgs) {
    grdST.Width = "99%";
   // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdST_onCallbackComplete()", strErr, "true");
    }
}
function grdST_onRenderComplete(sender, eventArgs) {

    if (document.getElementById("hdnModTrackBy").value == "I") { objlblImgCnt.style.display = "inline"; objlblObjCnt.style.display = "none"; }
    else if (document.getElementById("hdnModTrackBy").value == "O") { objlblImgCnt.style.display = "none"; objlblObjCnt.style.display = "inline"; }
}
function grdDoc_onCallbackComplete(sender, eventArgs) {
    grdDoc.Width = "99%";
   // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrDoc").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdDoc_onCallbackComplete()", strErr, "true");
    }
}
function grdDoc_onRenderComplete(sender, eventArgs) {
   // parent.adjustFrameHeight();
    if (DOCADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
}


