var grdRowID = "0"; var cb = "N";
function grdInst_onCallbackComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrInst").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdInst_onCallbackComplete()", strErr, "true");
    }
}
function grdInst_onRenderComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    //parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = ""; var selCons = ""; var selStore = "";

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        selCons = gridItem.Data[3].toString();
        selStore = gridItem.Data[4].toString();
        sel = gridItem.Data[5].toString();


        if (document.getElementById("chkSelInst_" + RowId) != null) {
            if (selCons == "Y") document.getElementById("chkSelCons_" + RowId).checked = true;
            if (selStore == "Y") document.getElementById("chkSelStore_" + RowId).checked = true;
            if (sel == "Y") document.getElementById("chkSelInst_" + RowId).checked = true;
        }

        itemIndex++;
    }
}
function grdContact_onCallbackComplete(sender, eventArgs) {
    grdContact.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrCont").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdContact_onCallbackComplete()", strErr, "true");
    }
}
function grdContact_onRenderComplete(sender, eventArgs) {
    grdContact.Width = "99%";
    //parent.adjustFrameHeight();

}

function grdPhys_onCallbackComplete(sender, eventArgs) {
    grdPhys.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBPhysErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPhys_onCallbackComplete()", strErr, "true");
    }

}
function grdPhys_onRenderComplete(sender, eventArgs) {
    //parent.adjustFrameHeight();
    parent.GsRetStatus = "false";
}
function grdPhys_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdPhys_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}

function grdMF_onCallbackComplete(sender, eventArgs) {
    grdMF.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrMF").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdMF_onCallbackComplete()", strErr, "true");
    }
}
function grdMF_onRenderComplete(sender, eventArgs) {
    grdMF.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Id = gridItem.Data[1].toString();

        if (document.getElementById("txtMFees_" + RowId) != null) {
            document.getElementById("txtMFees_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtMFees_" + RowId).value);
            document.getElementById("txtAddOn_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtAddOn_" + RowId).value);
            document.getElementById("txtMaxFee_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtMaxFee_" + RowId).value);
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}
function grdSF_onCallbackComplete(sender, eventArgs) {
    grdSF.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSF").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSF_onCallbackComplete()", strErr, "true");
    }
}
function grdSF_onRenderComplete(sender, eventArgs) {
    grdSF.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (document.getElementById("txtSFees_" + RowId) != null) {
            document.getElementById("txtSFees_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtSFees_" + RowId).value);
            document.getElementById("txtSFeesAH_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtSFeesAH_" + RowId).value);
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}

