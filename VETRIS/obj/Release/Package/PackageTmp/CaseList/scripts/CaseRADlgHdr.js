var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
function grdST_onCallbackComplete(sender, eventArgs) {
    grdST.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdST_onCallbackComplete()", strErr, "true");
    }
}
function grdST_onRenderComplete(sender, eventArgs) {
    grdST.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = "";

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[2].toString();

        if (sel == "Y") {

            if (document.getElementById("chkSel_" + RowId) != null) {
                document.getElementById("chkSel_" + RowId).checked = true;
            }
        }

        itemIndex++;
    }


}
function grdDoc_onCallbackComplete(sender, eventArgs) {
    grdDoc.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrDoc").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdDoc_onCallbackComplete()", strErr, "true");
    }
}
function grdDoc_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    if (DOCADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
}
function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
   // txtFromDt_OnBlur();
}

