var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
parent.adjustFrameHeight();
function grdSelST_onCallbackComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSelST_onCallbackComplete()", strErr, "true");
    }
}
function grdDoc_onCallbackComplete(sender, eventArgs) {
    grdDoc.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrDoc").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdDoc_onCallbackComplete()", strErr, "true");
    }
}
function grdDoc_onRenderComplete(sender, eventArgs) {
    //parent.adjustFrameHeight();
    if (DOCADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
}


