var grdRowID = "0"; var cb = "N";
function grdMF_onCallbackComplete(sender, eventArgs) {
    grdMF.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdMF_onCallbackComplete()", strErr, "true");
    }
}
function grdMF_onRenderComplete(sender, eventArgs) {
    grdMF.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var CtgID = ""; var Id = "0"; var ModID = "0";
    var arrMod = new Array(); var arrCtg = new Array();

    if (parent.Trim(objhdnModality.value) != "") {
        if (parent.Trim(objhdnModality.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrMod = parent.Trim(objhdnModality.value).split(parent.objhdnDivider.value);
        }
        else
            arrMod[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "");
    }
    else
        arrMod[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "");
    if (parent.Trim(objhdnCategory.value) != "") {
        if (parent.Trim(objhdnCategory.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrCtg = parent.Trim(objhdnCategory.value).split(parent.objhdnDivider.value);
        }
        else
            arrCtg[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }
    else
        arrCtg[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");


    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Id = gridItem.Data[1].toString();
        CtgID = gridItem.Data[2].toString();
        ModID = gridItem.Data[3].toString();

        if (document.getElementById("ddlCategory_" + RowId) != null) {
            document.getElementById("ddlCategory_" + RowId).length = 0;
            for (var i = 0; i < arrCtg.length; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrCtg[i];
                op.text = arrCtg[i + 1];
                document.getElementById("ddlCategory_" + RowId).add(op);
            }
            document.getElementById("ddlCategory_" + RowId).value = CtgID;
        }
        
        if (document.getElementById("ddlModality_" + RowId) != null) {
            document.getElementById("ddlModality_" + RowId).length = 0;
            
            for (var i = 0; i < arrMod.length; i = i + 3) {
                var op = document.createElement("option");
                op.value = arrMod[i];
                op.text = arrMod[i + 1];
                document.getElementById("ddlModality_" + RowId).add(op);
            }
            document.getElementById("ddlModality_" + RowId).value = ModID;
        }

        if (document.getElementById("txtMFees_" + RowId) != null) {
            document.getElementById("txtMFees_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtMFees_" + RowId).value);
            document.getElementById("txtAddOn_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtAddOn_" + RowId).value);
            document.getElementById("txtMaxFee_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtMaxFee_" + RowId).value);
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}
function grdSF_onCallbackComplete(sender, eventArgs) {
    grdSF.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBSvcErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSF_onCallbackComplete()", strErr, "true");
    }
}
function grdSF_onRenderComplete(sender, eventArgs) {
    grdSF.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var SvcID = ""; var Id = "0"; var ModID = "0";
    var arrMod = new Array(); var arrSvc = new Array();

    if (parent.Trim(objhdnModality.value) != "") {
        if (parent.Trim(objhdnModality.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrMod = parent.Trim(objhdnModality.value).split(parent.objhdnDivider.value);
        }
        else
            arrMod[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "");
    }
    else
        arrMod[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "");
    if (parent.Trim(objhdnServices.value) != "") {
        if (parent.Trim(objhdnServices.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrSvc = parent.Trim(objhdnServices.value).split(parent.objhdnDivider.value);
        }
        else
            arrSvc[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "0");
    }
    else
        arrSvc[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "0");


    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Id = gridItem.Data[1].toString();
        SvcID = gridItem.Data[2].toString();
        ModID = gridItem.Data[3].toString();

        if (document.getElementById("ddlService_" + RowId) != null) {
            document.getElementById("ddlService_" + RowId).length = 0;
            for (var i = 0; i < arrSvc.length; i = i + 3) {
                var op = document.createElement("option");
                op.value = arrSvc[i];
                op.text = arrSvc[i + 1];
                document.getElementById("ddlService_" + RowId).add(op);
            }
            document.getElementById("ddlService_" + RowId).value = SvcID;
        }

        if (document.getElementById("ddlSModality_" + RowId) != null) {
            document.getElementById("ddlSModality_" + RowId).length = 0;

            for (var i = 0; i < arrMod.length; i = i + 3) {
                var op = document.createElement("option");
                op.value = arrMod[i];
                op.text = arrMod[i + 1];
                document.getElementById("ddlSModality_" + RowId).add(op);
            }
            document.getElementById("ddlSModality_" + RowId).value = ModID;
        }

        if (document.getElementById("txtSFees_" + RowId) != null) {
            document.getElementById("txtSFees_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtSFees_" + RowId).value);
            document.getElementById("txtSFeesAH_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtSFeesAH_" + RowId).value);
        }

        itemIndex++;
    }
    parent.GsRetStatus = "false";

}