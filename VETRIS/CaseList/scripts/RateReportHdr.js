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
function grdPS_onCallbackComplete(sender, eventArgs) {
    grdPS.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrStudy").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPS_onCallbackComplete()", strErr, "true");
    }
}
function grdPS_onRenderComplete(sender, eventArgs) {
    var itemIndex = 0; var gridItem;
    var RowId = ""; var StatusID = 0;

    while (gridItem = grdPS.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        StatusID = parseInt(gridItem.get_cells()[8].get_value().toString());
        if (StatusID > 60) {
            if (document.getElementById("btnRpt_" + RowId) != null) document.getElementById("btnRpt_" + RowId).style.display = "inline";
        }

        itemIndex++;
    }
}

