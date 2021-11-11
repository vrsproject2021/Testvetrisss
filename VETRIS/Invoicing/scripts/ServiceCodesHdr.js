var grdRowID = "0";

function grdCodes_onCallbackComplete(sender, eventArgs) {
    grdCodes.Width = "99%";
    parent.adjusDataListFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        objdivMsg.innerHTML = "<font color='red'>" + strErr + "</font>";
        parent.adjusDataListFrameHeight();
    }
}
function grdCodes_onRenderComplete(sender, eventArgs) {
    grdCodes.Width = "99%";
   

    var itemIndex = 0; var gridItem;
    var RowId = ""; var sel = "";

    while (gridItem = grdCodes.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        sel = gridItem.get_cells()[3].get_value().toString();

        if (sel == "Y") {
            if (document.getElementById("chkSel_" + RowId) != null) {
                document.getElementById("chkSel_" + RowId).checked = true;
            }
        }

        itemIndex++;
    }
}
