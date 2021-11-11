var Divider = parent.objhdnDivider.value; var SecDivider = parent.objhdnSecDivider.value;
$(document).ready($(function () {
    CallBackStudy.callback(objhdnID.value, objhdnSUID.value);
}));

function UpdateTypeAll(Type) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (Type == "M") {
            gridItem.Data[8] = "M";
            if (document.getElementById("rdoMerge_" + RowId) != null) {
                document.getElementById("rdoMerge_" + RowId).checked = true;
                document.getElementById("rdoComp_" + RowId).checked = false;
                document.getElementById("rdoNone_" + RowId).checked = false;
            }
        }
        else if (Type == "C") {
            gridItem.Data[8] = "C";
            if (document.getElementById("rdoComp_" + RowId) != null) {
                document.getElementById("rdoComp_" + RowId).checked = false;
                document.getElementById("rdoComp_" + RowId).checked = true;
                document.getElementById("rdoNone_" + RowId).checked = false;
            }
        }
        else if (Type == "N") {
            gridItem.Data[8] = "N";
            if (document.getElementById("rdoNone_" + RowId) != null) {
                document.getElementById("rdoMerge_" + RowId).checked = false;
                document.getElementById("rdoComp_" + RowId).checked = false;
                document.getElementById("rdoNone_" + RowId).checked = true;
            }
        }
        
        itemIndex++;
    }
}
function UpdateType(ID,Type) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[8] = Type;
            if (Type=="M") {
                document.getElementById("rdoMerge_" + RowId).checked = true;
                document.getElementById("rdoComp_" + RowId).checked = false;
                document.getElementById("rdoNone_" + RowId).checked = false;
            }
            else if (Type == "C") {
                document.getElementById("rdoComp_" + RowId).checked = false;
                document.getElementById("rdoComp_" + RowId).checked = true;
                document.getElementById("rdoNone_" + RowId).checked = false;
            }
            else if (Type == "N") {
                document.getElementById("rdoMerge_" + RowId).checked = false;
                document.getElementById("rdoComp_" + RowId).checked = false;
                document.getElementById("rdoNone_" + RowId).checked = true;
            }
            break;
        }
        itemIndex++;
    }
}
function ReturnStudies() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var RetArgs = ""; var ErrFlg = "0";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        if (gridItem.Data[8].toString() == "") {
            ErrFlg = "1";
            document.getElementById("divErr").style.display = "block";
            document.getElementById("divMsg").innerHTML = "One of the studies is neither marked as Merged nor Compare or Ignore.Please check...";
            break;
        }
        else {
            if (RetArgs != "") RetArgs += Divider;
            RetArgs += gridItem.Data[0].toString() + Divider + gridItem.Data[1].toString() + Divider + gridItem.Data[6].toString() + Divider + gridItem.Data[8].toString();
            itemIndex++;
        }
    }
    if(ErrFlg=="0") parent.HideDataList(RetArgs);
}