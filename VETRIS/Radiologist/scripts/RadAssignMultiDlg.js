var strRowID = "0"; var DEL_FLAG = ""; var WRITE_BACK = "N"; var ErrFlag = 0;
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    parent.adjusDataListFrameHeight();
    CheckError();
}));

function CheckError() {
    var arrErr = new Array();
    if (parent.Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                if (arrErr[1] == "094") {
                    parent.PopupMessage(RootDirectory, strForm, "CheckError()", arrErr[1], "true");
                    strLoadPopup = "N";
                    parent.GsRetStatus = "false";
                    btnClose_OnClick();
                }
                else
                    parent.PopupMessage(RootDirectory, strForm, "OnLoad()", arrErr[1], "true");
                break;
        }
    }
    objhdnError.value = "";
    parent.adjusDataListFrameHeight();
    GetStudyIDs();
    SetPageValues();
   
    
}
function GetStudyIDs() {
    var Divider = parent.objhdnDivider.value;
    for (var i = 0; i < parent.GsStoredValue.length; i++) {
        if (objhdnStudy.value != "") objhdnStudy.value = objhdnStudy.value + Divider;
        objhdnStudy.value = objhdnStudy.value + parent.GsStoredValue[i];
    }
    parent.GsStoredValue.length = 0;
}
function SetPageValues() {
    var statusID = parseInt(objhdnStatusID.value);
    
    if (statusID > 50) {
        objrdoPrelim.disabled = true;
        objrdoFinal.checked = true;
    }
    LoadRadiologist();
}
function LoadRadiologist() {
    var strType = "";
    var strFilter = "";
    if (objrdoPrelim.checked) strType = "P"; else if (objrdoFinal.checked) strType = "F"; else strType = "";
    if (objrdoAll.checked) strFilter = "A";  else if (objrdoSchedule.checked) strFilter = "S";
    CallBackRad.callback(strType, strFilter,objhdnStudy.value,objhdnMenuID.value, objhdnUserID.value);
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Radiologist/VRSRadAssignMultiDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Radiologist/VRSRadAssignMultiDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {

    parent.HideDataList();
}
function rdoSel_OnClick(ID) {
    var rc = grdRad.get_recordCount();
    var itemIndex = 0; var gridItem; var RowId = "0"; 
    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {

            if (document.getElementById("rdoSel_" + RowId).checked) {
                gridItem.Data[2] = "Y";
            }
            else {
                gridItem.Data[2] = "N";
            }

        }
        else {
            if (gridItem.Data[1] == "Y") selCnt = selCnt + 1;
        }
        itemIndex++;
    }
   
    parent.GsRetStatus = "true";
}
function btnSave_OnClick() {
    PopupProcess("Y");
    var ArrRecords = new Array();
    var RadID = "00000000-0000-0000-0000-000000000000";

    try {
        
        if (objrdoPrelim.checked) ArrRecords[0] = "P"; else if (objrdoFinal.checked) ArrRecords[0] = "F"; else ArrRecords[0] = "";
        ArrRecords[1] = GetRadiologist();
        ArrRecords[2] = objhdnStudy.value;
        ArrRecords[3] = objhdnUserID.value;
        ArrRecords[4] = objhdnMenuID.value;
        ArrRecords[5] = objhdnSessionID.value;

        AjaxPro.timoutPeriod = 1800000;
        VRSRadAssignMultiDlg.SaveRecord(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "inline-block";
        
    }
}
function GetRadiologist() {
    var RadID = "00000000-0000-0000-0000-000000000000";
    var itemIndex = 0; var gridItem;
   

    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[2].toString();
        

        if (sel == "Y") {
            RadID = gridItem.Data[0].toString();
            break;
        }
        itemIndex++;
    }

    return RadID;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    objlblMsg.style.color = "black";
    document.getElementById("divMsg").style.display = "inline-block";
    objlblMsg.innerHTML = "";

    switch (arrRes[0]) {
        case "catch":
        case "false":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            parent.HideDataList("Y");
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

    //$('#tblProcess').surfOverlay('ps', { zIndex: 100 });
    closepopup('ps');
}
function ShowProcess(Result, MethodName) {
    HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
    }

}

