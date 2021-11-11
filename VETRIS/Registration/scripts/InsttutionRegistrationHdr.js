var PHYSADD = "N"; var GsDlgConfAction = "";
var strRowID = "0";
function grdPhys_onCallbackComplete(sender, eventArgs) {
    grdPhys.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = document.getElementById("hdnCBErrPhys").value;
    if (strErr != "") {
        PopupMessage(RootDirectory, strForm, "grdPhys_onCallbackComplete()", strErr, "true");
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

    if (PHYSADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    PHYSADD = "N";
}