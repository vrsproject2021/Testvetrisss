var Divider = parent.objhdnDivider.value; var SecDivider = parent.objhdnSecDivider.value;
var arrST = new Array();

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
        LoadStudyTypes();
    }
    objhdnError.value = "";
}
function btnClose_OnClick() {
    parent.HideDataList();
}

/********Study Types*************************/
function LoadStudyTypes() {
    CallBackST.callback(objhdnID.value, objhdnModalityID.value);
    CallBackSelST.callback("L", objhdnID.value, objhdnModalityID.value);
}
function chkSel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    var SelStudies = 0;
    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            SelStudies = grdSelST.get_recordCount();

            parent.GsRetStatus = "true";
            if (document.getElementById("chkSel_" + ID).checked) {
                if (SelStudies < 4) {
                    gridItem.Data[2] = "Y";
                    strDtls = GetStudyGridDetails();
                    CallBackSelST.callback("U", strDtls);
                }
                else
                    document.getElementById("chkSel_" + ID).checked = false
            }
            else {
                gridItem.Data[2] = "N";
                strDtls = GetStudyGridDetails();
                CallBackSelST.callback("U", strDtls);
            }
            break;
        }
        itemIndex++;
    }
}
function GetStudyGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;
    var strSel = "";

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        strSel = gridItem.Data[2].toString();

        if (strSel == "Y") {
            if (strDtls != "") strDtls = strDtls + SecDivider;
            strDtls += gridItem.get_cells()[1].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[3].get_value().toString();
        }
        itemIndex++;
    }
    return strDtls;
}
/********Study Types*************************/

function btnDone_OnClick() {

    PopupProcess("Y");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objhdnCycleID.value;
        ArrRecords[1] = objhdnID.value;
        ArrRecords[2] = parent.objhdnMenuID.value;
        ArrRecords[3] = parent.objhdnUserID.value;
        arrST = GetStudyTypes();

        AjaxPro.timeoutPeriod = 1800000;
        VRSStudyAmendStudyTypes.SaveStudyTypes(ArrRecords, arrST, ShowProcess);


    }
    catch (expErr) {
        HideProcess();
        objdivMsg.style.color = "red";
        if (document.all) objdivMsg.innerHTML = expErr.message; else objdivMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "inline-block";
        parent.adjusDataListFrameHeight();
    }

}
function GetStudyTypes()
{
    var arr = new Array();
    var itemIndex = 0;
    var gridItem;
    var idx = 0;

    while (gridItem = grdSelST.get_table().getRow(itemIndex)) {
        arr[idx] = gridItem.get_cells()[1].get_value().toString();
        idx = idx + 1;
        itemIndex++;
    }
    return arr;
}
function SaveStudyTypes(Result) {
    var arrRes = new Array();
    objdivMsg.style.color = "black";
    arrRes = Result.value;
    document.getElementById("divMsg").style.display = "inline-block";
    objdivMsg.innerHTML = "";
    switch (arrRes[0]) {
        case "catch":
        case "false":
            objdivMsg.innerHTML = arrRes[1];
            break;
        case "true":
            parent.GsRetStatus = "false";
            objdivMsg.innerHTML = arrRes[1];
            parent.HideDataList(arrST);
            break;
    }
}
function PopupProcess(IsSaving) {
    if (IsSaving == null) IsSaving = "N";
    var sUrl = "";
    if (IsSaving == "N") sUrl = "../htmls/processing.html";
    else sUrl = "../htmls/saving.html";

    $('#tblProcess').surfOverlay('ps', { url: sUrl, zIndex: 3000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });

    return false;
}
function HideProcess() {
    closepopup('ps');
}
function ShowProcess(Result, MethodName) {
    HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveStudyTypes":
            SaveStudyTypes(Result);
            break;
    }

}