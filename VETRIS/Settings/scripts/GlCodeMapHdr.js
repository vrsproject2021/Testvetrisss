var MODADD = "N"; var SVCADD = "N";
function grdModality_onCallbackComplete(sender, eventArgs) {
    grdModality.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBModErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdModality_onCallbackComplete()", strErr, "true");
    }
}
function grdModality_onRenderComplete(sender, eventArgs) {
    var itemIndex = 0; var gridItem;
    var RowId = "";
    var ModalityID = ""; var CategoryID = "";
    var arrModality = new Array();
    var arrCategory = new Array();

    if (parent.Trim(objhdnModality.value) != "") {
        if (parent.Trim(objhdnModality.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrModality = parent.Trim(objhdnModality.value).split(parent.objhdnDivider.value);
        }
        else
            arrModality[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }

    if (parent.Trim(objhdnCategory.value) != "") {
        if (parent.Trim(objhdnCategory.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrCategory = parent.Trim(objhdnCategory.value).split(parent.objhdnDivider.value);
        }
        else
            arrCategory[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }

    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        CategoryID = gridItem.get_cells()[1].get_value().toString();
        ModalityID = gridItem.get_cells()[3].get_value().toString();

        if (document.getElementById("ddlModality_" + RowId) != null) {
            if (document.getElementById("ddlModality_" + RowId).length == 0) {
                for (var i = 0; i < arrModality.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrModality[i];
                    op.text = arrModality[i + 1];
                    document.getElementById("ddlModality_" + RowId).add(op);
                }
            }
            document.getElementById("ddlModality_" + RowId).value = ModalityID;
        }

        if (document.getElementById("ddlCategory_" + RowId) != null) {
            if (document.getElementById("ddlCategory_" + RowId).length == 0) {
                for (var i = 0; i < arrCategory.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrCategory[i];
                    op.text = arrCategory[i + 1];
                    document.getElementById("ddlCategory_" + RowId).add(op);
                }
            }
            document.getElementById("ddlCategory_" + RowId).value = CategoryID;
        }
        itemIndex++;
    }
    if (MODADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    MODADD = "N";

}
function grdService_onCallbackComplete(sender, eventArgs) {
    grdService.Width = "99%";
    var strErr = parent.Trim(document.getElementById("hdnCBSvcErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdService_onCallbackComplete()", strErr, "true");
    }
}
function grdService_onRenderComplete(sender, eventArgs) {
    var itemIndex = 0; var gridItem;
    var RowId = "";
    var ModalityID = ""; var ServiceID = "";
    var arrModality = new Array();
    var arrService = new Array();

    if (parent.Trim(objhdnModality.value) != "") {
        if (parent.Trim(objhdnModality.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrModality = parent.Trim(objhdnModality.value).split(parent.objhdnDivider.value);
        }
        else
            arrModality[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }

    if (parent.Trim(objhdnService.value) != "") {
        if (parent.Trim(objhdnService.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrService = parent.Trim(objhdnService.value).split(parent.objhdnDivider.value);
        }
        else
            arrService[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }

    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        ServiceID = gridItem.get_cells()[1].get_value().toString();
        ModalityID = gridItem.get_cells()[2].get_value().toString();

        if (document.getElementById("ddlService_" + RowId) != null) {
            if (document.getElementById("ddlService_" + RowId).length == 0) {
                for (var i = 0; i < arrService.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrService[i];
                    op.text = arrService[i + 1];
                    document.getElementById("ddlService_" + RowId).add(op);
                }
            }
            document.getElementById("ddlService_" + RowId).value = ServiceID;
        }
        if (document.getElementById("ddlModalitySvc_" + RowId) != null) {
            if (document.getElementById("ddlModalitySvc_" + RowId).length == 0) {
                for (var i = 0; i < arrModality.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrModality[i];
                    op.text = arrModality[i + 1];
                    document.getElementById("ddlModalitySvc_" + RowId).add(op);
                }
            }
            document.getElementById("ddlModalitySvc_" + RowId).value = ModalityID;
        }

        
        itemIndex++;
    }
    if (SVCADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    SVCADD = "N";

}
function grdNRH_onCallbackComplete(sender, eventArgs) {
    grdNRH.Width = "99%";
    var strErr = parent.Trim(document.getElementById("hdnCBNRHErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdNRH_onCallbackComplete()", strErr, "true");
    }
}
function grdRC_onCallbackComplete(sender, eventArgs) {
    grdRC.Width = "99%";
    var strErr = parent.Trim(document.getElementById("hdnCBRCErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdRC_onCallbackComplete()", strErr, "true");
    }
}