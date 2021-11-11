function grdPayment_onCallbackComplete(sender, eventArgs) {
    grdPayment.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    var strUser = parent.Trim(document.getElementById("hdnUsr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPayment_onCallbackComplete()", strErr, "true", strUser);
    }

}
function grdPayment_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[1].get_value();
}
function grdPayment_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem;
    var RadRowId = "";
    var Approved = "";
    var ProcCount = 0; var AppvCount = 0;
    var ItemRecCountStudy = 0; var StudyRowId = ""; 

    while (gridItem = grdPayment.get_table().getRow(itemIndex)) {
        RadRowId = gridItem.get_cells()[0].get_value().toString();
        Approved = gridItem.get_cells()[6].get_value().toString();
        ProcCount = ProcCount + 1;

        if (document.getElementById("txtTotAmt_" + RadRowId) != null)
            document.getElementById("txtTotAmt_" + RadRowId).value = parent.SetDecimalFormat(document.getElementById("txtTotAmt_" + RadRowId).value);

        if (Approved == "Y") {
            AppvCount = AppvCount + 1;
            if (document.getElementById("btnNotApprove_" + RadRowId) != null) {
                document.getElementById("btnNotApprove_" + RadRowId).style.display = "none";
                document.getElementById("btnSave_" + RadRowId).style.display = "none";
                document.getElementById("btnApprove_" + RadRowId).style.display = "inline";
                //document.getElementById("btnEmailAcct_" + RadRowId).style.display = "inline";
            }
        }
        else {
            if (document.getElementById("btnNotApprove_" + RadRowId) != null) {
                document.getElementById("btnNotApprove_" + RadRowId).style.display = "inline";
                document.getElementById("btnApprove_" + RadRowId).style.display = "none";
                document.getElementById("btnSave_" + RadRowId).style.display = "inline";
                //document.getElementById("btnEmailAcct_" + RadRowId).style.display = "none";
            }
        }

        if (gridItem.Data.length > 8) {
            ItemRecCountStudy = gridItem.Data[8].length;
            for (var i = 0; i < ItemRecCountStudy; i++) {
                StudyRowId = gridItem.Data[8][i][0].toString();
                

                if (document.getElementById("txtAdhoc_" + StudyRowId) != null) {
                    document.getElementById("txtAdhoc_" + StudyRowId).value = parent.SetDecimalFormat(document.getElementById("txtAdhoc_" + StudyRowId).value);

                    
                }
            }
        }

       
        itemIndex++;
    }

    //if (ProcCount == AppvCount) {
    //    objbtnApprove.style.display = "none";
    //}
    //else if (ProcCount != AppvCount) {
    //    objbtnApprove.style.display = "inline";
    //}
    if (ProcCount > 0) objbtnPrint.style.display = "inline";
    else objbtnPrint.style.display = "none";

}
function grdPayment_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdPayment_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}