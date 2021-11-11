function grdChargesDiscount_onCallbackComplete(sender, eventArgs) {
    grdChargesDiscount.Width = "99%";
    parent.adjustFrameHeight();
    //var strErr = parent.Trim(document.getElementById("hdnCBErrBy").value);
    //if (strErr != "") {
    //    parent.PopupMessage(RootDirectory, strForm, "grdChargesDiscount_onCallbackComplete()", strErr, "true");
    //}

}
function grdChargesDiscount_onRenderComplete(sender, eventArgs) {
    grdChargesDiscount.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; 
    var RowId = "0";

    while (gridItem = grdChargesDiscount.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("txtDiscount_" + RowId) != null) {
            document.getElementById("txtDiscount_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtDiscount_" + RowId).value);
        }
        itemIndex++;
    }

}