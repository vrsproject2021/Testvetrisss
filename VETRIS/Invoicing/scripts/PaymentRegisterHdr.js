function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    //parent.adjustFrameHeight();
    //SetGridValues();
}

function CalFromDate_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFromDate.getSelectedDate());
    document.getElementById("txtFromDate").value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    //var to_dt = dt.setDate(dt.getDate() + 29);
    //document.getElementById("txtToDate").value = parent.FormatDate(new Date(to_dt).getDate(), new Date(to_dt).getMonth() + 1, new Date(to_dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);

}
function CalToDate_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalToDate.getSelectedDate());
    document.getElementById("txtToDate").value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    //var from_dt = dt.setDate(dt.getDate() - 29);
    //document.getElementById("txtFromDate").value = parent.FormatDate(new Date(from_dt).getDate(), new Date(from_dt).getMonth() + 1, new Date(from_dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}

function grdBrw_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdBrw_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}