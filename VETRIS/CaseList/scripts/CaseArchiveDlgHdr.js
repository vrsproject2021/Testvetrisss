var grdRowID = "0"; var cb = "N"; var DOCADD = "N"; var SAVE_FLAG = "N";
parent.adjustFrameHeight(); 
function grdSelST_onCallbackComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSelST_onCallbackComplete()", strErr, "true");
    }
}
function grdSelST_onRenderComplete(sender, eventArgs) {
    if (objhdnInvBy.value == "B") {
        recCnt = grdSelST.get_recordCount();
        document.getElementById("spnSTCount").style.display = "inline";
        if (document.all) document.getElementById("spnSTCount").innerText = "Count : " + recCnt.toString();
        else document.getElementById("spnSTCount").textContent = "Count : " + recCnt.toString();
        objbtnEditST.style.display = "inline";
        if (SAVE_FLAG == "X") SAVE_FLAG = "Y";
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
function grdAddn_onCallbackComplete(sender, eventArgs) {
    grdAddn.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrAddn").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdAddn_onCallbackComplete()", strErr, "true");
    }
}
