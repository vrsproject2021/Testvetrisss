function grdInvoiceBrw_onCallbackComplete(sender, eventArgs) {
    grdInvoiceBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdInvoiceBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdInvoiceBrw_onRenderComplete(sender, eventArgs) {
    grdInvoiceBrw.Width = "99%";
    parent.adjustFrameHeight();
    
    if (parent["__pg_done__"]===undefined) {
        getTotalAmount();
        var total = parseFloat(objtxtSelectedAmount.value);
        if (total > 0) {
            $("#btnPay").click();
        } else {
            objtxtSelectedAmount.value = "0.00";
        }
    }
}

function grdPaymentBrw_onCallbackComplete(sender, eventArgs) {
    grdInvoiceBrw.Width = "99%";
    parent.adjustFrameHeight();
}
function grdPaymentBrw_onRenderComplete(sender, eventArgs) {
    grdPaymentBrw.Width = "99%";
    parent.adjustFrameHeight();
}

function grdAllInvoiceBrw_onCallbackComplete(sender, eventArgs) {
    grdAllInvoiceBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBInvAllErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdAllInvoiceBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdAllInvoiceBrw_onRenderComplete(sender, eventArgs) {
    grdAllInvoiceBrw.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var balance = 0; 
    while (gridItem = grdAllInvoiceBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        balance = parseFloat(gridItem.Data[8].toString());

        if (balance >0) {

            if (document.getElementById("btnAllPay_" + RowId) != null) {
                document.getElementById("btnAllPay_" + RowId).style.display = "inline";
            }
        }

        itemIndex++;
    }
}
function grdAdjBrw_onCallbackComplete(sender, eventArgs) {
    if (grdAdjBrw)
        grdAdjBrw.Width = "99%";
    parent.adjustFrameHeight();
}
function grdAdjBrw_onRenderComplete(sender, eventArgs) {
    if (grdAdjBrw)
        grdAdjBrw.Width = "99%";
    parent.adjustFrameHeight();

}
function grdAllInvoiceBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    var InvID = item.Cells[0].get_value();
    var ArrRecords = new Array();
    ArrRecords[0] = "L";
    ArrRecords[1] = UserID;
    ArrRecords[2] = MenuID;
    ArrRecords[3] = InvID;
    CallBackAdjBrw.callback(ArrRecords);
}

