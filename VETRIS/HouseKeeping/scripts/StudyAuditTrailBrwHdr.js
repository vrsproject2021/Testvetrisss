function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[0].get_value();
    btnBrwEditUI_Onclick("HouseKeeping/VRSStudyAuditTrailDlg.aspx");
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    //parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var status_id = "";

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        status_id = gridItem.Data[12].toString();

        if (parseInt(status_id)>=80) {

            if (document.getElementById("btnAL_" + RowId) != null) {
                document.getElementById("btnRpt_" + RowId).style.display = "inline";
                document.getElementById("btnImg_" + RowId).style.display = "inline";
            }
        }

        itemIndex++;
    }
}
function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
function CalTill_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalTill.getSelectedDate());
    objtxtTillDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
