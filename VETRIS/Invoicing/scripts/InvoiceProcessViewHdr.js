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
        if (ApprovedAcct == "Y" || ApprovedAcct == "X") {
            AppvAcctCount = AppvAcctCount + 1;
            if (document.getElementById("btnNotApproveAcct_" + AcctRowId) != null) {
                document.getElementById("btnNotApproveAcct_" + AcctRowId).style.display = "none";
                document.getElementById("btnApproveAcct_" + AcctRowId).style.display = "inline";
                document.getElementById("btnEmailAcct_" + AcctRowId).style.display = "inline";
            }
        }
        else {
            if (document.getElementById("btnNotApproveAcct_" + AcctRowId) != null) {
                document.getElementById("btnNotApproveAcct_" + AcctRowId).style.display = "inline";
                document.getElementById("btnApproveAcct_" + AcctRowId).style.display = "none";
                document.getElementById("btnEmailAcct_" + AcctRowId).style.display = "none";
            }
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

                if (ApprovedInst == "Y" || ApprovedInst == "X") {
                    if (document.getElementById("btnNotApproveInst_" + InstRowId) != null) {
                        document.getElementById("btnNotApproveInst_" + InstRowId).style.display = "none";
                        document.getElementById("btnApproveInst_" + InstRowId).style.display = "inline";
                        document.getElementById("btnEmailInst_" + InstRowId).style.display = "inline";
                        document.getElementById("btnEditInst_" + InstRowId).style.display = "none";
                    }
                }
                else {
                    if (document.getElementById("btnNotApproveInst_" + InstRowId) != null) {
                        document.getElementById("btnNotApproveInst_" + InstRowId).style.display = "inline";
                        document.getElementById("btnApproveInst_" + InstRowId).style.display = "none";
                        document.getElementById("btnEmailInst_" + InstRowId).style.display = "none";
                        document.getElementById("btnEditInst_" + InstRowId).style.display = "inline";
                    }
                }

                if (gridItem.Data[10][i].length > 12) {
                    ItemRecCountDtls = gridItem.Data[10][i][12].length;

                    for (var j = 0; j < ItemRecCountDtls; j++) {
                        DtlsRowId = gridItem.Data[10][i][12][j][0].toString();
                        SYAmt = gridItem.Data[10][i][12][j][16].toString();
                        SVCAmt = gridItem.Data[10][i][12][j][17].toString();
                        SYTot = gridItem.Data[10][i][12][j][18].toString();
                        Billed = gridItem.Data[10][i][12][j][19].toString();
                        ApprovedDtls = gridItem.Data[10][i][12][j][20].toString();

                        if (document.getElementById("txtSYAmt_" + DtlsRowId) != null) {
                            document.getElementById("txtSYAmt_" + DtlsRowId).value = parent.SetDecimalFormat(SYAmt);
                            document.getElementById("txtSVCAmt_" + DtlsRowId).value = parent.SetDecimalFormat(SVCAmt);
                            document.getElementById("txtSYTot_" + DtlsRowId).value = parent.SetDecimalFormat(SYTot);
                        }
                        if (Billed == "Y") {
                            if (document.getElementById("chkBill_" + DtlsRowId) != null) {
                                document.getElementById("chkBill_" + DtlsRowId).checked = true;
                            }
                        }

                        if (ApprovedDtls == "Y" || ApprovedDtls == "X") {
                            if (document.getElementById("chkBill_" + DtlsRowId) != null) {
                                document.getElementById("chkBill_" + DtlsRowId).disabled = true;
                            }
                        }

                    }

                }

            }
        }

        itemIndex++;
    }

    if (ProcAcctCount == AppvAcctCount) {
        objbtnApprove.style.display = "none";
    }
    else if (ProcAcctCount != AppvAcctCount) {
        objbtnApprove.style.display = "inline";
    }
    if (ProcAcctCount > 0) objbtnPrint.style.display = "inline";
    else objbtnPrint.style.display = "none";

}
function grdInvoice_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdInvoice_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}