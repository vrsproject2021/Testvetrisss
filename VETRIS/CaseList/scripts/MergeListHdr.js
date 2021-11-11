
function grdStudy_onCallbackComplete(sender, eventArgs) {
    grdStudy.Width = "99%";
    //parent.adjustFrameHeight();
    //var strErr = parent.Trim(document.getElementById("hdnCBErrST").value);
    //if (strErr != "") {
    //    parent.PopupMessage(RootDirectory, strForm, "grdStudy_onCallbackComplete()", strErr, "true");
    //}
}
function grdStudy_onRenderComplete(sender, eventArgs) {
    grdStudy.Width = "99%";
    //parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var type = "";

    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        type = gridItem.Data[8].toString();

        if (document.getElementById("rdoMerge_" + RowId) != null) {
            if (type == "M") {
                document.getElementById("rdoMerge_" + RowId).checked = true;
            }
            else if (type == "C") {
                document.getElementById("rdoComp_" + RowId).checked = true;
            }
            else if (type == "N") {
                document.getElementById("rdoNone_" + RowId).checked = true;
            }
        }

        

        itemIndex++;
    }

    parent.adjusDataListFrameHeight();
}
