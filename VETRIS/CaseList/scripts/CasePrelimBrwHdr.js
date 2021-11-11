function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
   // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var RadFnRights = objhdnRadFnRights.value; var ShowDL = ""; var LogAvailable = "";

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        ShowDL = gridItem.get_cells()[24].get_value().toString();
        LogAvailable = gridItem.Data[25].toString();

        if (document.getElementById("btnActivity_" + RowId) != null) {

            if (ShowDL == "N") document.getElementById("btnDLImgDsbl_" + RowId).style.display = "inline";
            else if (ShowDL == "Y") document.getElementById("btnDLImg_" + RowId).style.display = "inline";

            if ((UserRoleCode == "IU") || (UserRoleCode == "AU")) {
                document.getElementById("btnActivity_" + RowId).style.display = "none";
            }
            else {
                if (LogAvailable == "N") document.getElementById("btnActivity_" + RowId).style.display = "none";
            }
        }
        if ((UserRoleCode == "RDL") && (RadFnRights.indexOf("UPDFINALRPT") == -1)) {
            if (document.getElementById("btnEditRpt_" + RowId) != null)
                document.getElementById("btnEditRpt_" + RowId).style.display = "none";
        }
        if ((UserRoleCode == "RDL") || (UserRoleCode == "SYSADMIN") || (UserRoleCode == "SUPP")) {
            if (document.getElementById("btnImgViewer_" + RowId) != null) {
                document.getElementById("btnImgViewer_" + RowId).style.display = "inline";
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
