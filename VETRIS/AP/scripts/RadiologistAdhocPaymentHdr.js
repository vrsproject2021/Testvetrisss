var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
parent.adjusDataListFrameHeight();
function grdPmt_onCallbackComplete(sender, eventArgs) {
    grdPmt.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPmt_onCallbackComplete()", strErr, "true");
    }
}
function grdPmt_onRenderComplete(sender, eventArgs) {
    grdPmt.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdPmt.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (document.getElementById("txtAmt_" + RowId) != null) {
            document.getElementById("txtAmt_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtAmt_" + RowId).value);
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}


