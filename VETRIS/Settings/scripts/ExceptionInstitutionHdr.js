function grdInst_onCallbackComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    parent.adjusDataListFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        objlblMsg.innerHTML = "<font color='red'>" + strErr + "</font>";
    }
}
function grdInst_onRenderComplete(sender, eventArgs) {
    var itemIndex = 0; var gridItem;
    var RowId = ""; var sel = "";
   
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        sel = gridItem.Data[3].toString();
        if (document.getElementById("chkSel_" + RowId) != null) {
            if (sel == "Y") document.getElementById("chkSel_" + RowId).checked = true;
            else document.getElementById("chkSel_" + RowId).checked = false;
        }

        
        itemIndex++;
    }


}