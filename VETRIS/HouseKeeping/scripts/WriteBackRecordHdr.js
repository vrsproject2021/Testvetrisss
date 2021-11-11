function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }

}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var rec = ""; var wb = "";

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        wb = gridItem.Data[7].toString();

        if (wb == "Y") {

            if (document.getElementById("btnRevert_" + RowId) != null) {
                document.getElementById("btnRevert_" + RowId).style.display = "inline";
            }
        }
        else if (wb == "N") {

            if (document.getElementById("btnWB_" + RowId) != null) {
                document.getElementById("btnWB_" + RowId).style.display = "inline";
            }
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";
}
function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
function CalTill_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalTill.getSelectedDate());
    objtxtTillDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
