function grdInvoice_onCallbackComplete(sender, eventArgs) {
    grdInvoice.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    var strUser = parent.Trim(document.getElementById("hdnUsr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdInvoice_onCallbackComplete()", strErr, "true", strUser);
    }

}
function grdInvoice_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[1].get_value();
}
function grdInvoice_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem;
    var AcctRowId = ""; var InstRowId = ""; var DtlsRowId = "";
    var ApprovedAcct = ""; var ApprovedInst = ""; var ApprovedDtls = "";
    var AcctTot = 0; var InstTot = 0;
    var SYAmt = 0; var SVCAmt = 0; var SYTot = 0; var Billed = "";
    var ItemRecCountInst = 0; var ItemRecCountDtls = 0;
    var ProcAcctCount = 0; var AppvAcctCount = 0;

    while (gridItem = grdInvoice.get_table().getRow(itemIndex)) {
        AcctRowId = gridItem.get_cells()[0].get_value().toString();
        AcctTot = gridItem.get_cells()[5].get_value().toString();
        ApprovedAcct = gridItem.get_cells()[6].get_value().toString();
        ProcAcctCount = ProcAcctCount + 1;

        if (document.getElementById("txtAcctTot_" + AcctRowId) != null) {
            document.getElementById("txtAcctTot_" + AcctRowId).value = parent.SetDecimalFormat(AcctTot);
        }

        if (gridItem.Data.length > 10) {
            ItemRecCountInst = gridItem.Data[10].length;
            for (var i = 0; i < ItemRecCountInst; i++) {
                InstRowId = gridItem.Data[10][i][0].toString();
                InstTot = gridItem.Data[10][i][7].toString();
                ApprovedInst = gridItem.Data[10][i][8].toString();

                if (document.getElementById("txtInstTot_" + InstRowId) != null) {
                    document.getElementById("txtInstTot_" + InstRowId).value = parent.SetDecimalFormat(InstTot);
                }

                if (gridItem.Data[10][i].length > 12) {
                    ItemRecCountDtls = gridItem.Data[10][i][12].length;

                    for (var j = 0; j < ItemRecCountDtls; j++) {
                        DtlsRowId = gridItem.Data[10][i][12][j][0].toString();
                        SYAmt = gridItem.Data[10][i][12][j][14].toString();
                        SVCAmt = gridItem.Data[10][i][12][j][15].toString();
                        SYTot = gridItem.Data[10][i][12][j][16].toString();
                        Billed = gridItem.Data[10][i][12][j][17].toString();
                        ApprovedDtls = gridItem.Data[10][i][12][j][18].toString();

                        if (document.getElementById("txtSYAmt_" + DtlsRowId) != null) {
                            document.getElementById("txtSYAmt_" + DtlsRowId).value = parent.SetDecimalFormat(SYAmt);
                            document.getElementById("txtSVCAmt_" + DtlsRowId).value = parent.SetDecimalFormat(SVCAmt);
                            document.getElementById("txtSYTot_" + DtlsRowId).value = parent.SetDecimalFormat(SYTot);
                        }


                    }

                }

            }

        }

        itemIndex++;
    }

    if (grdInvoice.get_recordCount() > 0) objbtnUnfinal.style.display = "inline";

}
function grdInvoice_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdInvoice_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}