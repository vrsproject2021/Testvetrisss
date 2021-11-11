function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var stat = ""; var Id = "0"; var UserID = "0"; var Validate = "";
    var arrAdmins = new Array();
    var arrRads = new Array();

    if (parent.Trim(objhdnAdmins.value) != "") {
        if (parent.Trim(objhdnAdmins.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrAdmins = parent.Trim(objhdnAdmins.value).split(parent.objhdnDivider.value);
        }
        else
            arrAdmins[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }
    if (parent.Trim(objhdnRadiologists.value) != "") {
        if (parent.Trim(objhdnRadiologists.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrRads = parent.Trim(objhdnRadiologists.value).split(parent.objhdnDivider.value);
        }
        else
            arrRads[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        stat = gridItem.Data[1].toString();
        UserID = gridItem.Data[8].toString();

        if (document.getElementById("ddlUser_" + RowId) != null) {
            if (stat == "0") {
                if (document.getElementById("ddlUser_" + RowId).length == 0) {
                    for (var i = 0; i < arrAdmins.length; i = i + 2) {
                        var op = document.createElement("option");
                        op.value = arrAdmins[i];
                        op.text = arrAdmins[i + 1];
                        document.getElementById("ddlUser_" + RowId).add(op);
                    }
                }
            }
            else {
                if (document.getElementById("ddlUser_" + RowId).length == 0) {
                    for (var i = 0; i < arrRads.length; i = i + 2) {
                        var op = document.createElement("option");
                        op.value = arrRads[i];
                        op.text = arrRads[i + 1];
                        document.getElementById("ddlUser_" + RowId).add(op);
                    }

                }
            }
            document.getElementById("ddlUser_" + RowId).value = UserID;
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}


