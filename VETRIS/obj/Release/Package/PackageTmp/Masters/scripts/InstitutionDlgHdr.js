var grdRowID = "0"; var cb = "N"; var DEVADD = "N"; var PHYSADD = "N"; var SPADD = "N"; var USERADD = "N";
function grdDevice_onCallbackComplete(sender, eventArgs) {
    grdDevice.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrDev").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdDevice_onCallbackComplete()", strErr, "true");
    }
}
function grdDevice_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    if (DEVADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";

    DEVADD = "N";
}
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
    //var itemIndex = 0; var gridItem; var RowId = "0";
    //var PhysID = "0";
    //var arrPhys = new Array();

    //if (parent.Trim(objhdnPhysicians.value) != "") {
    //    if (parent.Trim(objhdnPhysicians.value).indexOf(parent.objhdnDivider.value) != -1) {
    //        arrPhys = parent.Trim(objhdnPhysicians.value).split(parent.objhdnDivider.value);
    //    }
    //    else
    //        arrPhys[0] = parent.Trim(objhdnPhysicians.value);
    //}

    //while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
    //    RowId = gridItem.Data[0].toString();
    //    PhysID = gridItem.Data[1].toString();

    //    if (document.getElementById("ddlPhys_" + RowId) != null) {
    //        if (document.getElementById("ddlPhys_" + RowId).length == 0) {
    //            for (var i = 0; i < arrPhys.length; i = i + 2) {
    //                var op = document.createElement("option");
    //                op.value = arrPhys[i];
    //                op.text = arrPhys[i + 1];
    //                document.getElementById("ddlPhys_" + RowId).add(op);
    //            }
    //        }
    //        document.getElementById("ddlPhys_" + RowId).value = PhysID;
               
    //    }
      

    //    itemIndex++;
    //}

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
        isActive = gridItem.Data[7].toString();
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

function grdSP_onCallbackComplete(sender, eventArgs) {
    grdSP.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSP").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSP_onCallbackComplete()", strErr, "true");
    }
}
function grdSP_onRenderComplete(sender, eventArgs) {
    grdSP.Width = "99%";
    parent.adjustFrameHeight();
   

    if (SPADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    SPADD = "N";
}
