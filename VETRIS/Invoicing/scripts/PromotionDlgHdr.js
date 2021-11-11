function grdPromo_onCallbackComplete(sender, eventArgs) {
    grdPromo.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPromo_onCallbackComplete()", strErr, "true");
    }
}
function grdPromo_onRenderComplete(sender, eventArgs) {
    grdPromo.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ModID = "0"; var InstId = "00000000-0000-0000-0000-000000000000";

    var arrInst = new Array();
    var arrModality = new Array();

    if (document.getElementById("hdnInst") != null) {
        if (document.getElementById("hdnInst").value != "") {
            if (parent.Trim(document.getElementById("hdnInst").value).indexOf(parent.objhdnDivider.value) != -1) {
                arrInst = parent.Trim(document.getElementById("hdnInst").value).split(parent.objhdnDivider.value);
            }
            else {
                arrInst[0] = "00000000-0000-0000-0000-000000000000";
                arrInst[1] = "Select One";
            }
        }
        else {
            arrInst[0] = "00000000-0000-0000-0000-000000000000";
            arrInst[1] = "Select One";
        }
    }
    else {
        arrInst[0] = "00000000-0000-0000-0000-000000000000";
        arrInst[1] = "Select One";
    }

    if (document.getElementById("hdnMod") != null) {
        if (document.getElementById("hdnMod").value != "") {
            if (parent.Trim(document.getElementById("hdnMod").value).indexOf(parent.objhdnDivider.value) != -1) {
                arrModality = parent.Trim(document.getElementById("hdnMod").value).split(parent.objhdnDivider.value);
            }
            else {
                arrModality[0] = "0";
                arrModality[1] = "Select One";
            }
        }
        else {
            arrModality[0] = "0";
            arrModality[1] = "Select One";
        }

    }
    else {
        arrModality[0] = "0";
        arrModality[1] = "Select One";
    }

    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        InstId = gridItem.Data[2].toString();
        ModID = gridItem.Data[3].toString();

        if (document.getElementById("ddlInst_" + RowId) != null) {
            if (document.getElementById("ddlInst_" + RowId).length == 0) {
                for (var i = 0; i < arrInst.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrInst[i];
                    op.text = arrInst[i + 1];
                    document.getElementById("ddlInst_" + RowId).add(op);
                }
            }
            if (InstId == "00000000-0000-0000-0000-000000000000") {
                if (document.getElementById("ddlInst_" + RowId).length == 2) {
                    document.getElementById("ddlInst_" + RowId).value = arrInst[arrInst.length - 2];
                    gridItem.Data[2] = document.getElementById("ddlInst_" + RowId).value;
                }
                else
                    document.getElementById("ddlInst_" + RowId).value = InstId;
            }
            else
                document.getElementById("ddlInst_" + RowId).value = InstId;
        }

        if (document.getElementById("ddlModality_" + RowId) != null) {
            if (document.getElementById("ddlModality_" + RowId).length == 0) {
                for (var i = 0; i < arrModality.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrModality[i];
                    op.text = arrModality[i + 1];
                    document.getElementById("ddlModality_" + RowId).add(op);
                }
            }
            document.getElementById("ddlModality_" + RowId).value = ModID;
        }
        if (document.getElementById("txtDisc_" + RowId) != null) {
            document.getElementById("txtDisc_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtDisc_" + RowId).value);
        }
        
        itemIndex++;
    }
}

function CalFromDate_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFromDate.getSelectedDate());
    document.getElementById("txtFromDate").value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);

}
function CalToDate_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalToDate.getSelectedDate());
    document.getElementById("txtToDate").value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);

}


