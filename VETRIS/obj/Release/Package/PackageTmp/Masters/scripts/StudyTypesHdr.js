var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
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
    var stat = ""; var Id = "0"; var ModID = "0";
    var arrModality = new Array();

    if (parent.Trim(objhdnModalities.value) != "") {
        if (parent.Trim(objhdnModalities.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrModality = parent.Trim(objhdnModalities.value).split(parent.objhdnDivider.value);
        }
        else
            arrModality[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }


    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Id = gridItem.Data[1].toString();
        ModID = gridItem.Data[3].toString();
        stat = gridItem.Data[4].toString();

        if (Id == "00000000-0000-0000-0000-000000000000") {
            if (document.getElementById("btnDel_" + RowId) != null) {
                document.getElementById("btnDel_" + RowId).style.display = "inline";
            }
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
       
        if (stat == "Y") {

            if (document.getElementById("chkAct_" + RowId) != null) {
                document.getElementById("chkAct_" + RowId).checked = true;
            }
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}


