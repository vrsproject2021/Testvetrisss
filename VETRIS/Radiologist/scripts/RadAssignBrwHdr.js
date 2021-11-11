function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }

}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var PriorityID = "0"; var PrelimRadID = ""; var FinalRadID = "";
    var arrPriority = new Array();

    if (parent.Trim(objhdnPriority.value) != "") {
        if (parent.Trim(objhdnPriority.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrPriority = parent.Trim(objhdnPriority.value).split(parent.objhdnDivider.value);
        }

    }

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        PriorityID = gridItem.Data[9].toString();
        PrelimRadID = gridItem.Data[15].toString();
        FinalRadID = gridItem.Data[17].toString();

        if (document.getElementById("ddlPriority_" + RowId) != null) {
            if (document.getElementById("ddlPriority_" + RowId).length == 0) {
                for (var i = 0; i < arrPriority.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrPriority[i];
                    op.text = arrPriority[i + 1];
                    document.getElementById("ddlPriority_" + RowId).add(op);
                }
            }
            document.getElementById("ddlPriority_" + RowId).value = PriorityID;

        }
        if (document.getElementById("btnUnassign_" + RowId) != null) {
            if (PrelimRadID == "00000000-0000-0000-0000-000000000000" && FinalRadID == "00000000-0000-0000-0000-000000000000") {
                document.getElementById("btnUnassign_" + RowId).style.display = "none";
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
