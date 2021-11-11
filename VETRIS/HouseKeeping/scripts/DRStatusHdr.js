function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var Type = false;

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Type = gridItem.Data[1].toString();

        if (Type == "Y") {

            if (document.getElementById("imgOn_" + RowId) != null) {
                document.getElementById("imgOn_" + RowId).style.display = "inline";
                document.getElementById("imgOff_" + RowId).style.display = "none";
            }
        }
        else {
            if (document.getElementById("imgOff_" + RowId) != null) {
                document.getElementById("imgOn_" + RowId).style.display = "none";
                document.getElementById("imgOff_" + RowId).style.display = "inline";
            }
        }

        itemIndex++;
    }
}

