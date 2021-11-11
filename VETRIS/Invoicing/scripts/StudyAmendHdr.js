var grdItem; var StudyUpdated = "N";
function grdStudy_onCallbackComplete(sender, eventArgs) {
    grdStudy.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdStudy_onCallbackComplete()", strErr, "true");
    }
}
function grdStudy_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem;
    var RowId = ""; 
    var Invoiced = ""; var InstID = ""; var ModalityID = ""; var PriorityID = ""; var CategoryID = "";
    var PromoID = ""; var del = ""; var ItemRecCount = 0; var RateRowID = ""; var RateAmt = 0; var HeadType = ""; var InvoiceBy = "";
    var arrInst = new Array();
    var arrModality = new Array(); 
    var arrPriority = new Array();
    var arrCategory = new Array();

    if (parent.Trim(objhdnInst.value) != "") {
        if (parent.Trim(objhdnInst.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrInst = parent.Trim(objhdnInst.value).split(parent.objhdnDivider.value);
        }
        else
            arrInst[0] = parent.Trim("00000000-0000-0000-0000-000000000000" + parent.objhdnDivider.value + "Select One");
    }

    if (parent.Trim(objhdnModality.value) != "") {
        if (parent.Trim(objhdnModality.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrModality = parent.Trim(objhdnModality.value).split(parent.objhdnDivider.value);
        }
        else
            arrModality[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }

    if (parent.Trim(objhdnPriority.value) != "") {
        if (parent.Trim(objhdnPriority.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrPriority = parent.Trim(objhdnPriority.value).split(parent.objhdnDivider.value);
        }
        else
            arrPriority[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }

    if (parent.Trim(objhdnCategory.value) != "") {
        if (parent.Trim(objhdnCategory.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrCategory = parent.Trim(objhdnCategory.value).split(parent.objhdnDivider.value);
        }
        else
            arrCategory[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One");
    }

    

    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        InstID = gridItem.get_cells()[4].get_value().toString();
        ModalityID = gridItem.get_cells()[5].get_value().toString();
        PriorityID = gridItem.get_cells()[6].get_value().toString();
        CategoryID = gridItem.get_cells()[7].get_value().toString();
        Invoiced = gridItem.get_cells()[9].get_value().toString();
        del = gridItem.get_cells()[11].get_value().toString();
        PromoID = gridItem.get_cells()[12].get_value().toString();

        if (document.getElementById("ddlInst_" + RowId) != null) {
            if (document.getElementById("ddlInst_" + RowId).length == 0) {
                for (var i = 0; i < arrInst.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrInst[i];
                    op.text = arrInst[i + 1];
                    document.getElementById("ddlInst_" + RowId).add(op);
                }
            }
            document.getElementById("ddlInst_" + RowId).value = InstID;
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
            document.getElementById("ddlModality_" + RowId).value = ModalityID;
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

        if(del=="N")
        {
            if (document.getElementById("btnDel_" + RowId) != null)
            {
                document.getElementById("btnDel_" + RowId).style.display = "none";
            }
        }
        if(Invoiced == "Y")
        {
            if (document.getElementById("ddlInst_" + RowId) != null) 
            {
                document.getElementById("ddlInst_" + RowId).disabled=true;
                document.getElementById("ddlModality_" + RowId).disabled=true;
                document.getElementById("ddlPriority_" + RowId).disabled = true;
                document.getElementById("ddlCategory_" + RowId).disabled = true;
                document.getElementById("btnDel_" + RowId).style.display = "none";
                document.getElementById("btnDisc_" + RowId).style.display = "none";
                document.getElementById("btnEditSvc_" + RowId).style.display = "none";
            }
        }
        else if (Invoiced == "N") {
            if (document.getElementById("btnDel_" + RowId) != null) {
                document.getElementById("btnDel_" + RowId).style.display = "inline";
            }
            if (PromoID == "00000000-0000-0000-0000-000000000000") {
                if (document.getElementById("btnDisc_" + RowId) != null) {
                    document.getElementById("btnDisc_" + RowId).style.display = "inline";
                }
            }
        }


        if (gridItem.Data.length > 13) {
            ItemRecCount = gridItem.Data[13].length;
            for (var i = 0; i < ItemRecCount; i++) {

                RateRowID = gridItem.Data[13][i][0].toString();
                HeadType = gridItem.Data[13][i][4].toString();
                RateAmt = gridItem.Data[13][i][8].toString();
                InvoiceBy = gridItem.Data[13][i][10].toString();

                if (document.getElementById("txtAmt_" + RateRowID) != null) {
                    document.getElementById("txtAmt_" + RateRowID).value = parent.SetDecimalFormat(RateAmt);
                    if (HeadType == "M" && InvoiceBy=="B") {
                        if (document.getElementById("btnEditBP_" + RateRowID) != null) { document.getElementById("btnEditBP_" + RateRowID).style.display="inline" }
                    }
                }
            }
        }

        itemIndex++;
    }

    
}
function grdStudy_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdStudy_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}
