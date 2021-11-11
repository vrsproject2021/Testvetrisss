var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
function grdFiles_onCallbackComplete(sender, eventArgs) {
    grdFiles.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrFiles").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdFiles_onCallbackComplete()", strErr, "true");
    }
}
function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
   txtFromDt_OnBlur();
}

