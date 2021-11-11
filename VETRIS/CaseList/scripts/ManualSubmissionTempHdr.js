var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
function grdST_onCallbackComplete(sender, eventArgs) {
    grdST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdST_onCallbackComplete()", strErr, "true");
    }
}
function grdST_onRenderComplete(sender, eventArgs) {
    grdST.Width = "99%";
    //parent.adjustFrameHeight();
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
function grdSelST_onCallbackComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSelST_onCallbackComplete()", strErr, "true");
    }
}
function grdSF_onCallbackComplete(sender, eventArgs) {
    grdSF.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
}
function grdSF_onRenderComplete(sender, eventArgs) {
    var rc = 0;
    if (grdSF.recordNumber != null) rc = grdSF.get_recordCount();
    else if (grdSF.RecordCount != null) rc = grdSF.get_recordCount();

    if (document.getElementById("hdnAccNo") != null) {
        if (parent.Trim(document.getElementById("hdnAccNo").value) != "")
            objtxtAccnNo.value = parent.Trim(document.getElementById("hdnAccNo").value);
        else
            objtxtAccnNo.value = objhdnTempAccNo.value;

        if (parent.Trim(document.getElementById("hdnPid").value) != "")
            objtxtPID.value = parent.Trim(document.getElementById("hdnPid").value);
        else
            objtxtPID.value = objhdnTempPID.value;
    }

    
}


function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    CalculateAge();
}
function CalDOS_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalDOS.getSelectedDate());
    objtxtDOS.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    CalculateAge();
}


