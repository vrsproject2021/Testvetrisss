var grdRowID = "0";

function grdST_onCallbackComplete(sender, eventArgs) {
    grdST.Width = "99%";
    var strErr = parent.Trim(document.getElementById("hdnCBErrST").value);
    if (strErr != "") {
        objdivMsg.innerHTML = "<font color='red'>" + strErr + "</font>";
        parent.adjusDataListFrameHeight();
    }
}
function grdST_onRenderComplete(sender, eventArgs) {
    grdST.Width = "99%";
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
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    if ((strErr != "") && (strErr != "UPDATE")) {
        objdivMsg.innerHTML = "<font color='red'>" + strErr + "</font>";
        parent.adjusDataListFrameHeight();
    }
}
function grdSelST_onRenderComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    var strUpdate = parent.Trim(document.getElementById("hdnCBErrSelST").value);
}
