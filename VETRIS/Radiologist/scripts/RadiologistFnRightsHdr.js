function grdRights_onCallbackComplete(sender, eventArgs) {
    grdRights.Width = "99%";
   // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdRights_onCallbackComplete()", strErr, "true");
    }
}
function grdRights_onRenderComplete(sender, eventArgs) {
    grdRights.Width = "99%";
   // parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = "";

    while (gridItem = grdRights.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[1].toString();
       
        if (sel == "Y") {
            if (document.getElementById("chkSelRight_" + RowId) != null) {
                document.getElementById("chkSelRight_" + RowId).checked = true;
            }
        }

        
        itemIndex++;
    }
}
function grdModality_onCallbackComplete(sender, eventArgs) {
    grdModality.Width = "99%";
    // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBModErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdModality_onCallbackComplete()", strErr, "true");
    }
}
function grdModality_onRenderComplete(sender, eventArgs) {
    grdModality.Width = "99%";
    // parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = "";

    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[1].toString();

        if (sel == "Y") {
            if (document.getElementById("chkSelModality_" + RowId) != null) {
                document.getElementById("chkSelModality_" + RowId).checked = true;
            }
        }


        itemIndex++;
    }
}
function grdSpecies_onCallbackComplete(sender, eventArgs) {
    grdSpecies.Width = "99%";
    var strErr = parent.Trim(document.getElementById("hdnCBSpeciesErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSpecies_onCallbackComplete()", strErr, "true");
    }
}
function grdSpecies_onRenderComplete(sender, eventArgs) {
    grdSpecies.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = "";

    while (gridItem = grdSpecies.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[1].toString();

        if (sel == "Y") {
            if (document.getElementById("chkSelSpecies_" + RowId) != null) {
                document.getElementById("chkSelSpecies_" + RowId).checked = true;
            }
        }


        itemIndex++;
    }
}
function grdInst_onCallbackComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrInst").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdInst_onCallbackComplete()", strErr, "true");
    }
}
function grdInst_onRenderComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    // parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = ""; 

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[1].toString();
        
        if (sel == "Y") {
            if (document.getElementById("chkSelInst_" + RowId) != null) {
                document.getElementById("chkSelInst_" + RowId).checked = true;
            }
        }
        itemIndex++;
    }
}
function grdSelInst_onCallbackComplete(sender, eventArgs) {
    grdSelInst.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelInst").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSelInst_onCallbackComplete()", strErr, "true");
    }
}
function grdST_onCallbackComplete(sender, eventArgs) {
    grdST.Width = "99%";
    // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdST_onCallbackComplete()", strErr, "true");
    }
}
function grdST_onRenderComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    // parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = "";

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[1].toString();

        if (sel == "Y") {
            if (document.getElementById("chkSelST_" + RowId) != null) {
                document.getElementById("chkSelST_" + RowId).checked = true;
            }
        }
        itemIndex++;
    }
}
function grdSelST_onCallbackComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSelST_onCallbackComplete()", strErr, "true");
    }
}
function grdRad_onCallbackComplete(sender, eventArgs) {
    grdRad.Width = "99%";
    // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrRad").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdRad_onCallbackComplete()", strErr, "true");
    }
}
function grdRad_onRenderComplete(sender, eventArgs) {
    grdRad.Width = "99%";
    // parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = "";

    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[1].toString();

        if (sel == "Y") {
            if (document.getElementById("chkSelRad_" + RowId) != null) {
                document.getElementById("chkSelRad_" + RowId).checked = true;
            }
        }
        itemIndex++;
    }
}
function grdSelRad_onCallbackComplete(sender, eventArgs) {
    grdSelRad.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelRad").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSelRad_onCallbackComplete()", strErr, "true");
    }
}
