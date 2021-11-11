function grdBA_onCallbackComplete(sender, eventArgs) {
    grdBA.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBBAErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBA_onCallbackComplete()", strErr, "true");
    }

}
function grdBA_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowID = ""; var sel = "";

    while (gridItem = grdBA.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        sel = gridItem.Data[3].toString();

        if (sel == "Y") {
            if (document.getElementById("btnSel_" + RowID) != null) {
                document.getElementById("btnSel_" + RowID).style.display = "none";
                document.getElementById("btnCheck_" + RowID).style.display = "inline";
            }
        }

        itemIndex++;
    }
}
function grdBAProc_onCallbackComplete(sender, eventArgs) {
    grdBAProc.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBBAProcErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBAProc_onCallbackComplete()", strErr, "true");
    }
}
function grdBAProc_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowID = "";
    var sel = ""; var approved = ""; var locked = "";

    while (gridItem = grdBAProc.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        approved = gridItem.Data[4].toString();
        locked = gridItem.Data[5].toString();
        sel = gridItem.Data[7].toString();

        if (sel == "Y") {
            if (document.getElementById("btnSelProc_" + RowID) != null) {
                document.getElementById("btnSelProc_" + RowID).style.display = "none";
                document.getElementById("btnCheckProc_" + RowID).style.display = "inline";
            }
        }

        if (document.getElementById("btnPrint_" + RowID) != null) {
            if (approved == "Y") {
                if (locked == "N") {
                    document.getElementById("btnApprove_" + RowID).style.display = "inline";
                    document.getElementById("btnEmail_" + RowID).style.display = "inline";
                }
                else {
                    document.getElementById("btnLocked_" + RowID).style.display = "inline";
                }
            }
            else if (approved == "N") {
                if (locked == "N") {
                    document.getElementById("btnNotApprove_" + RowID).style.display = "inline";
                    document.getElementById("btnReProc_" + RowID).style.display = "inline";
                }
                else {
                    document.getElementById("btnLocked_" + RowID).style.display = "inline";
                    document.getElementById("btnView_" + RowID).style.display = "none";
                    document.getElementById("btnSelProc_" + RowID).style.display = "none";
                    document.getElementById("btnReProc_" + RowID).style.display = "none";
                }
            }
        }

        

        itemIndex++;
    }
}

