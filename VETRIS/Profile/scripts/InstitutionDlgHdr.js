var grdRowID = "0"; var cb = "N"; var DEVADD = "N"; var PHYSADD = "N"; var SPADD = "N"; var USERADD = "N";


function grdPhys_onCallbackComplete(sender, eventArgs) {
    grdPhys.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrPhys").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPhys_onCallbackComplete()", strErr, "true");
    }
}
function grdPhys_onRenderComplete(sender, eventArgs) {
    grdPhys.Width = "99%";
    parent.adjustFrameHeight();
    
    if (PHYSADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    PHYSADD = "N";
}

function grdCred_onCallbackComplete(sender, eventArgs) {
    grdCred.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrCred").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdCred_onCallbackComplete()", strErr, "true");
    }
}
function grdCred_onRenderComplete(sender, eventArgs) {
    grdCred.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var isActive = ""; var ID = "";

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        ID = gridItem.Data[1].toString();
        isActive = gridItem.Data[8].toString();
        if (document.getElementById("chkAct_" + RowId) != null) {
            if (isActive == "Y") document.getElementById("chkAct_" + RowId).checked = true;
        }
        if (ID == "00000000-0000-0000-0000-000000000000") {
            if (document.getElementById("chkAct_" + RowId) != null) {
                document.getElementById("chkAct_" + RowId).style.display = "none";
                document.getElementById("btnDelUser_" + RowId).style.display = "inline";
            }
        }
        else {
            if (document.getElementById("chkAct_" + RowId) != null) {
                document.getElementById("chkAct_" + RowId).style.display = "inline";
                document.getElementById("btnDelUser_" + RowId).style.display = "none";
            }
        }
        itemIndex++;
    }

    if (USERADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    USERADD = "N";
}

function grdPromo_onCallbackComplete(sender, eventArgs) {
    grdPromo.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrPromo").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPromo_onCallbackComplete()", strErr, "true");
    }
}
function grdPromo_onRenderComplete(sender, eventArgs) {
    grdPromo.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = ""; var Type = ""; var dtFrom = "";

    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Type = gridItem.Data[1].toString();
        dtFrom = gridItem.Data[7][1].toString();
        if (Type == "D") {
            if (document.getElementById("spnDt_" + RowId) != null) {
                if (document.all) document.getElementById("spnDt_" + RowId).innerText = dtFrom;
                else document.getElementById("spnDt_" + RowId).textContent = dtFrom;
               
            }
        }
        itemIndex++;
    }
}
