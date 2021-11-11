function color_changed(sender, args) {
    if (sender.get_selectedColor() && sender.get_selectedColor().get_hex()) {
        var c = "#" + sender.get_selectedColor().get_hex();
        document.getElementById("MenuColor_0").style.backgroundColor = c;
        MenuColor.hide();
        objhdnColor.value = c;
        if (document.all) objlblColor.innerText = c;
        else objlblColor.textContent = c;
    }
}
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
    var PrelimFee = 0; var FinalFee = 0; var AddlFee = 0;

    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[2].toString();
        PrelimFee = gridItem.Data[4].toString();
        FinalFee = gridItem.Data[5].toString();
        AddlFee = gridItem.Data[6].toString();

        if (sel == "Y") {
            selCnt = selCnt + 1;
            if (document.getElementById("chkSel_" + RowId) != null) {
                document.getElementById("chkSel_" + RowId).checked = true;
            }
        }

        if (document.getElementById("txtPrelim_" + RowId) != null) {
            document.getElementById("txtPrelim_" + RowId).value = parent.SetDecimalFormat(PrelimFee); 
            document.getElementById("txtFinal_" + RowId).value = parent.SetDecimalFormat(FinalFee);
            document.getElementById("txtAddl_" + RowId).value = parent.SetDecimalFormat(AddlFee); 
            if (sel == "Y") {
                document.getElementById("txtPrelim_" + RowId).readOnly = "";
                document.getElementById("txtFinal_" + RowId).readOnly = "";
                document.getElementById("txtAddl_" + RowId).readOnly = "";
                document.getElementById("txtWU_" + RowId).readOnly = "";
            }
            else {
                document.getElementById("txtPrelim_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("txtFinal_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("txtAddl_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("txtWU_" + RowId).className = "GridTextBoxReadOnly";
            }
        }
        


        itemIndex++;
    }

    if (selCnt == itemIndex) document.getElementById("chkSelHdr").checked = true;
}


