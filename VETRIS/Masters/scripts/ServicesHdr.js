var grdRowID = "0"; 
function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    //parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var stat = ""; var Id = "0"; var PriorityID = "0"; var SysDefined = "";

    var arrPriority = new Array();

    if (parent.Trim(objhdnPriorities.value) != "") {
        if (parent.Trim(objhdnPriorities.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrPriority = parent.Trim(objhdnPriorities.value).split(parent.objhdnDivider.value);
        }
        else
            arrPriority[0] = parent.Trim("0" + parent.objhdnDivider.value + "None");
    }

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Id = gridItem.Data[1].toString();
        PriorityID = gridItem.Data[4].toString();
        stat = gridItem.Data[6].toString();
        SysDefined = gridItem.Data[8].toString();

        if (Id == "0") {
            if (document.getElementById("btnDel_" + RowId) != null) {
                document.getElementById("btnDel_" + RowId).style.display = "inline";
            }
        }

        if (document.getElementById("ddlPriority_" + RowId) != null) {
            if (document.getElementById("ddlPriority_" + RowId).length == 0) {
                for (var i = 0; i < arrPriority.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrPriority[i];
                    op.text = arrPriority[i + 1];
                    document.getElementById("ddlPriority_" + RowId).add(op);
                }
            }
            document.getElementById("ddlPriority_" + RowId).value = PriorityID;
            if (parseInt(PriorityID) != "0") {
                if (document.getElementById("txtCode_" + RowId) != null) {
                    document.getElementById("txtCode_" + RowId).readOnly = "readOnly";
                    document.getElementById("txtCode_" + RowId).className = "GridTextBoxReadOnly";
                }
            }
        }
    
        if (stat == "Y") {

            if (document.getElementById("chkAct_" + RowId) != null) {
                document.getElementById("chkAct_" + RowId).checked = true;
            }
        }

        if (SysDefined == "Y") {
            if (document.getElementById("txtName_" + RowId) != null) {
                document.getElementById("txtName_" + RowId).readOnly = "readOnly";
                document.getElementById("txtName_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("txtCode_" + RowId).readOnly = "readOnly";
                document.getElementById("txtCode_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("ddlPriority_" + RowId).disabled = true;
                //document.getElementById("chkAct_" + RowId).disabled = true;
            }
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}


