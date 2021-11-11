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
    var stat = ""; var Id = "0"; var SpeciesID = "0";
    var arrSpecies = new Array();

    if (parent.Trim(objhdnSpecies.value) != "") {
        if (parent.Trim(objhdnSpecies.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrSpecies = parent.Trim(objhdnSpecies.value).split(parent.objhdnDivider.value);
        }
        else
            arrSpecies[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }


    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Id = gridItem.Data[1].toString();
        SpeciesID = gridItem.Data[4].toString();
        stat = gridItem.Data[5].toString();

        if (Id == "00000000-0000-0000-0000-000000000000") {
            if (document.getElementById("btnDel_" + RowId) != null) {
                document.getElementById("btnDel_" + RowId).style.display = "inline";
            }
        }
        else {
            if (document.getElementById("txtCode_" + RowId) != null) {
                document.getElementById("txtCode_" + RowId).readOnly = "readOnly";
                document.getElementById("txtCode_" + RowId).className = "GridTextBoxReadOnly";
            }
        }

        if (document.getElementById("ddlSpecies_" + RowId) != null) {
            if (document.getElementById("ddlSpecies_" + RowId).length == 0) {
                for (var i = 0; i < arrSpecies.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrSpecies[i];
                    op.text = arrSpecies[i + 1];
                    document.getElementById("ddlSpecies_" + RowId).add(op);
                }
            }
            document.getElementById("ddlSpecies_" + RowId).value = SpeciesID;
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


