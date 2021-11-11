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
    btnBrwEditUI_Onclick("Masters/VRSInstitutionDlg.aspx");
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    //parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var isNew = "";

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        isNew = gridItem.Data[10].toString();
        if (isNew == "Y") {
            if (document.getElementById("imgNew_" + RowId) != null) {
                document.getElementById("imgNew_" + RowId).style.display = "inline";
            }
        }
        itemIndex++;
    }
}