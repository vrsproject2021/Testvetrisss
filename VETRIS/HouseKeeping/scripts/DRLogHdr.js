function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "100%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var Type = false;

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Type = gridItem.Data[1].toString();

        if (Type == "true") {

            if (document.getElementById("imgErr_" + RowId) != null) {
                document.getElementById("imgErr_" + RowId).style.display = "inline";
                document.getElementById("imgInfo_" + RowId).style.display = "none";
            }
        }
        else {
            if (document.getElementById("imgInfo_" + RowId) != null) {
                document.getElementById("imgErr_" + RowId).style.display = "none";
                document.getElementById("imgInfo_" + RowId).style.display = "inline";
            }
        }

        itemIndex++;
    }
}
function grdUA_onCallbackComplete(sender, eventArgs) {
    grdUA.Width = "100%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBUAErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdUA_onCallbackComplete()", strErr, "true");
    }
}
function CalFromUA_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFromUA.getSelectedDate());
    objtxtFromDtUA.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
function CalTillUA_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalTillUA.getSelectedDate());
    objtxtTillDtUA.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
function CalTill_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalTill.getSelectedDate());
    objtxtTillDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
