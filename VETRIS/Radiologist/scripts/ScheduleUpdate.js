$(document).ready($(function () {
    SetPageValue();
}))

function SetPageValue() {
    objtxtNotes.value = parent.GsText;
    if (objhdnID.value == "00000000-0000-0000-0000-000000000000") objbtnDel.style.display = "none";
    parent.GenlHt = "375";
    parent.adjustGenlFrameHeight();
}

function btnClose_OnClick() {
    parent.HideGeneral("Y");
}
function btnSave_OnClick() {
    PopupProcess("Y");
    var ArrRecords = new Array();
    var strFromDt = "";
    var strFromHr = ""; var strTillHr = "";

    if (parent.Trim(objtxtFromDt.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if ((objddlFromTT.value == "PM") && (parseInt(objddlFromHr.value) < 12)) strFromHr = parent.padZeroPlaces(12 + parseInt(objddlFromHr.value));
    else if ((objddlFromTT.value == "AM") && (parseInt(objddlFromHr.value) == 12)) strFromHr = "00";
    else strFromHr = objddlFromHr.value;

    if ((objddlTillTT.value == "PM") && (parseInt(objddlTillHr.value) < 12)) strTillHr = parent.padZeroPlaces(12 + parseInt(objddlTillHr.value));
    else if ((objddlTillTT.value == "AM") && (parseInt(objddlTillHr.value) == 12)) strTillHr = "00";
    else strTillHr = objddlTillHr.value;

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objddlRadiologist.value;
        ArrRecords[2] = strFromDt;
        ArrRecords[3] = strFromHr + ":" + objddlFromMin.value + ":00";
        ArrRecords[4] = strTillHr + ":" + objddlTillMin.value + ":00";
        ArrRecords[5] = objtxtNotes.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSScheduleUpdate.SaveSchedule(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        if (document.all) objlblMsg.innerText = expErr.message;
        else objlblMsg.textContent = expErr.message;
    }
}
function btnDel_OnClick() {
    PopupProcess();
    try {
        AjaxPro.timeoutPeriod = 1800000;
        VRSScheduleUpdate.DeleteSchedule(objhdnID.value, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        if (document.all) objlblMsg.innerText = expErr.message;
        else objlblMsg.textContent = expErr.message;
    }
}
function ProcessUpdate(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "false":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            parent.HideGeneral("Y");
            break;
    }
}
function SetSelectedDate(objCtrlID, CalName, LsImgID) {
    var strDate = ""; var strClass = "";
    var dt;
    objCtrl = document.getElementById(objCtrlID.id); if (objCtrl == null) objCtrl = objCtrlID;
    strDate = document.getElementById(objCtrl).value;

    if (parent.Trim(strDate) != "") {
        if (document.all) {
            dt = new Date(parent.SetDateFormat(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
        else {
            dt = new Date(parent.SetDateFormat1(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
    }
    else
        dt = new Date();
    switch (CalName) {
        case "CalFrom":
            CalFrom.setSelectedDate(dt); CalFrom.show();
            break;
        case "CalTill":
            CalTill.setSelectedDate(dt); CalTill.show();
            break;
    }


}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}

function ShowProcess(Result, MethodName) {
    HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveSchedule":
        case "DeleteSchedule":
            ProcessUpdate(Result);
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