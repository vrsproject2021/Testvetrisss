var DEL_FLAG = ""; var strRowID = "0"; var objItem; var VALIDATED = "N";
var Divider = objhdnDivider.value; var SecDivider = objhdnSecDivider.value;
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

        CallBackEmail.callback("L", parent.GsText);
    }
    objhdnError.value = "";
}

function btnDone_OnClick()
{
    var itemIndex = 0;
    var gridItem;
    var strRet = "";
    while (gridItem = grdEmail.get_table().getRow(itemIndex)) {
        if (parent.Trim(gridItem.Data[1].toString()) != "") {
            if (parent.Trim(strRet) != "") strRet = strRet + ";";
            strRet += parent.Trim(gridItem.Data[1].toString());
        }
        itemIndex++;
    }
    parent.HideDataList(strRet);
}
function btnClose_OnClick() {
    parent.HideDataList();
}
function btnAdd_OnClick() {
    var strDtls = "";
    EMAILADD = "Y";
    strDtls = GetEmailGridDetails();
    CallBackEmail.callback("A", strDtls);
}
function txtEmail_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdEmail.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[1] = document.getElementById("txtEmail_" + RowId).value;

        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function DeleteRow(ID) {
    strRowID = ID;
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function GetEmailGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdEmail.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString();
        itemIndex++;
    }
    return strDtls;
}
function DeleteRecord() {

    strDtls = GetEmailGridDetails();
    CallBackEmail.callback("D", strRowID, strDtls);


}
