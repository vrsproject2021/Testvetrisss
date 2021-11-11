var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
parent.adjusDataListFrameHeight();
function grdRad_onCallbackComplete(sender, eventArgs) {
    grdRad.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdRad_onCallbackComplete()", strErr, "true");
    }
}



