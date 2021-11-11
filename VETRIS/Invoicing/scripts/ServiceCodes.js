$(document).ready($(function () {
    parent.adjusDataListFrameHeight();
    CheckError();
}))
function CheckError() {
    var arrErr = new Array();
    if (parent.Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                objdivMsg.innerHTML = "<font color='red'>" + arrErr[1] + "</font>";
                parent.adjusDataListFrameHeight();
                break;
        }
    }
    else {
        CallBackCodes.callback(objhdnID.value, parent.GsText);
    }
    objhdnError.value = "";
}
function btnDone_OnClick()
{
    var itemIndex = 0;
    var gridItem;
    var arrRet = new Array();
    var idx = 0;
    while (gridItem = grdCodes.get_table().getRow(itemIndex)) {
        if (parent.Trim(gridItem.Data[3].toString()) == "Y") {
            //if (parent.Trim(strRet) != "") strRet = strRet + ",";
            //strRet += parent.Trim(gridItem.Data[1].toString());
            arrRet[idx] = gridItem.Data[0].toString();
            arrRet[idx + 1] = parent.Trim(gridItem.Data[1].toString());
            idx = idx + 2;
        }
        itemIndex++;
    }
    
    parent.HideDataList(arrRet);
}
function btnClose_OnClick() {
    var arrRet = new Array();
    arrRet[0] = "C";
    parent.HideDataList(arrRet);
}

function chkSel_OnClick(ID) {
    var itemIndex = 0; var gridItem;
    var RowId = "";

    while (gridItem = grdCodes.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();

        if (RowId == ID) {
            if (document.getElementById("chkSel_" + RowId).checked) gridItem.Data[3] = "Y";
            else gridItem.Data[3] = "N";
            break;
        }

        itemIndex++;
    }
}
