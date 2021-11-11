function grdModality_onCallbackComplete(sender, eventArgs) {
    grdModality.Width = "99%";
   // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdModality_onCallbackComplete()", strErr, "true");
    }
}
function grdModality_onRenderComplete(sender, eventArgs) {
    grdModality.Width = "99%";
   // parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = ""; var selCnt = 0;
    var DefaultFee = 0; var AddlFee = 0;

    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[2].toString();
        DefaultFee = gridItem.Data[4].toString();
        AddlFee = gridItem.Data[5].toString();

        if (sel == "Y") {
            selCnt = selCnt + 1;
            if (document.getElementById("chkSel_" + RowId) != null) {
                document.getElementById("chkSel_" + RowId).checked = true;
            }
        }

        if (document.getElementById("txtDefFee_" + RowId) != null) {
            document.getElementById("txtDefFee_" + RowId).value = parent.SetDecimalFormat(DefaultFee); 
            document.getElementById("txtAddl_" + RowId).value = parent.SetDecimalFormat(AddlFee); 
            if (sel == "Y") {
                document.getElementById("txtDefFee_" + RowId).readOnly = "";
                document.getElementById("txtAddl_" + RowId).readOnly = "";
            }
            else {
                document.getElementById("txtDefFee_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("txtAddl_" + RowId).className = "GridTextBoxReadOnly";
            }
        }
        
        itemIndex++;
    }

    if (selCnt == itemIndex) document.getElementById("chkSelHdr").checked = true;
}
