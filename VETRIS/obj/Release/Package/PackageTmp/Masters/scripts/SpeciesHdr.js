var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
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
    var stat = ""; var Id = "0";

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Id = gridItem.Data[1].toString();
        stat = gridItem.Data[4].toString();


        if (Id != "0") {
            if (document.getElementById("txtCode_" + RowId) != null) {
                document.getElementById("txtCode_" + RowId).readOnly = "readOnly";
                document.getElementById("txtCode_" + RowId).className = "GridTextBoxReadOnly";
            }
        }
        else {
            if (document.getElementById("btnDel_" + RowId) != null) {
                document.getElementById("btnDel_" + RowId).style.display = "inline";
            }
        }
        if (stat == "Y") {

            if (document.getElementById("chkAct_" + RowId) != null) {
                document.getElementById("chkAct_" + RowId).checked = true;
            }
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}


