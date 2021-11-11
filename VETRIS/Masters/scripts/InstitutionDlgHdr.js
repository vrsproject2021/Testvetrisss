var grdRowID = "0"; var cb = "N"; var DEVADD = "N"; var PHYSADD = "N"; var SPADD = "N"; var USERADD = "N";
function grdTags_onCallbackComplete(sender, eventArgs) {
    grdTags.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrTags").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdTags_onCallbackComplete()", strErr, "true");
    }
}
function grdTags_onRenderComplete(sender, eventArgs) {
    grdTags.Width = "99%";
    //parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var selected = "";
   

    while (gridItem = grdTags.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        selected = gridItem.Data[1].toString();
        if (document.getElementById("chkSelTag_" + RowId) != null) {
            if (selected == "Y") document.getElementById("chkSelTag_" + RowId).checked = true;
            if (objrdoFmtYes.checked) document.getElementById("chkSelTag_" + RowId).disabled = false;
        }

        itemIndex++;
    }
    
}

function grdInsCategory_onCallbackComplete(sender, eventArgs) {
    grdInsCategory.Width = "99%";
    var strErr = parent.Trim(document.getElementById("hdnCBErrInsCtg").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdInsCategory_onCallbackComplete()", strErr, "true");
    }
}
function grdInsCategory_onRenderComplete(sender, eventArgs) {
    grdInsCategory.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var selected = "";

    while (gridItem = grdInsCategory.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        selected = gridItem.Data[1].toString();
        if (document.getElementById("chkSelInsCtg_" + RowId) != null) {
            if (selected == "Y") document.getElementById("chkSelInsCtg_" + RowId).checked = true;
            if (objrdoFmtYes.checked) document.getElementById("chkSelInsCtg_" + RowId).disabled = false;
        }

        itemIndex++;
    }

}

function grdDevice_onCallbackComplete(sender, eventArgs) {
    grdDevice.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrDev").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdDevice_onCallbackComplete()", strErr, "true");
    }
}
function grdDevice_onRenderComplete(sender, eventArgs) {
    //parent.adjustFrameHeight();
    if (DEVADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";

    SetDeviceGridValues();// Added on 2nd SEP 2019 @BK  
    DEVADD = "N";
}

//--Added on 2nd SEP 2019 @BK
function SetDeviceGridValues() {

    var gridItem;
    var itemIndex = 0;
    var RowId = "0";
    var uom = "";


    while (gridItem = grdDevice.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        uom = gridItem.Data[5].toString();

        if (document.getElementById('ddlWeightUOM_' + RowId) != null) {

            if (gridItem.get_cells()[1].get_value().toString() === "00000000-0000-0000-0000-000000000000") {

                document.getElementById('ddlWeightUOM_' + RowId).setAttribute("enabled", "true");
            }
            else {
                if (uom === "Lbs")
                    document.getElementById('ddlWeightUOM_' + RowId).value = 1;
                else
                    document.getElementById('ddlWeightUOM_' + RowId).value = 2;
                //document.getElementById('ddlWeightUOM_' + RowId).setAttribute("disabled", "true");
            }

        }
        itemIndex++;
    }
}
//--Added on 2nd SEP 2019 @BK

function grdPhys_onCallbackComplete(sender, eventArgs) {
    grdPhys.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrPhys").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPhys_onCallbackComplete()", strErr, "true");
    }
}
function grdPhys_onRenderComplete(sender, eventArgs) {
    grdPhys.Width = "99%";
    //parent.adjustFrameHeight();
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
    //grdPhys.beginUpdate();
    //grdPhys.CurrentPageIndex = grdPhys.PageCount - 1;
    //grdPhys.endUpdate();
    if (PHYSADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    PHYSADD = "N";
}

function grdCred_onCallbackComplete(sender, eventArgs) {
    grdCred.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrCred").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdCred_onCallbackComplete()", strErr, "true");
    }
}
function grdCred_onRenderComplete(sender, eventArgs) {
    grdCred.Width = "99%";
    //parent.adjustFrameHeight();
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

function grdSP_onCallbackComplete(sender, eventArgs) {
    grdSP.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSP").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSP_onCallbackComplete()", strErr, "true");
    }
}
function grdSP_onRenderComplete(sender, eventArgs) {
    grdSP.Width = "99%";
    //parent.adjustFrameHeight();
   

    if (SPADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    SPADD = "N";
}

function grdPromo_onCallbackComplete(sender, eventArgs) {
    grdPromo.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrPromo").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPromo_onCallbackComplete()", strErr, "true");
    }
}
function grdPromo_onRenderComplete(sender, eventArgs) {
    grdPromo.Width = "99%";
    //parent.adjustFrameHeight();
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
    var sel = "";

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[3].toString();
        if (document.getElementById("chkSelInst_" + RowId) != null) {
            if (sel == "Y") document.getElementById("chkSelInst_" + RowId).checked = true;
        }

        itemIndex++;
    }
}

