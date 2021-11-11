
$(document).ready($(function () {
    window.history.forward();
    parent.adjustGenlMedFrameHeight();
    CheckError();
   
}))
function CheckError() {
    if (parent.Trim(objhdnError.value) != "") {
        document.getElementById("divMsg").style.display = "inline-block";
    }
    objhdnError.value = "";
    SetPageValue();
}
function SetPageValue() {
    DiscountBy_OnClick();
    if (objhdnInvoiced.value == "Y") {
        objtxtDiscPer.readOnly = "readOnly";
        objtxtDiscAmt.readOnly = "readOnly";
        objrdoPer.disabled = true;
        objrdoAmt.disabled = true;
        objddlReason.disabled = true;
        objbtnApply1.style.display = "none";
        objbtnApply2.style.display = "none";
    }
    else if (parseFloat(objtxtDiscPer.value) > 0 || parseFloat(objtxtDiscAmt.value) > 0) {
        objbtnRevert1.style.display = "inline";
        objbtnRevert2.style.display = "inline";
    }
}
function btnClose_OnClick() {
    parent.HideGeneralMedium();
}
function DiscountBy_OnClick() {
    if (objrdoPer.checked) {
        objtxtDiscPer.readOnly = "";
        objtxtDiscAmt.readOnly = "readOnly";ResetValueDecimal(objtxtDiscAmt);
    }
    else if (objrdoAmt.checked) {
        objtxtDiscAmt.readOnly = "";
        objtxtDiscPer.readOnly = "readOnly"; ResetValueDecimal(objtxtDiscPer);
    }
}
function btnApply_OnClick() {

    PopupProcess("Y");
    var ArrRecords = new Array();


    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = "N"; if (objrdoPer.checked) ArrRecords[1] = "P"; else if (objrdoAmt.checked) ArrRecords[1] = "A";
        ArrRecords[2] = objtxtDiscPer.value;
        ArrRecords[3] = objtxtDiscAmt.value;
        ArrRecords[4] = objddlReason.value;
        ArrRecords[5] = objtxtCost.value;
        ArrRecords[6] = objhdnMenuID.value;
        ArrRecords[7] = objhdnUserID.value;
        ArrRecords[8] = parent.objhdnSessionID.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSApplyPromotion.ApplyDiscount(ArrRecords,ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "inline-block";
        parent.adjustGenlMedFrameHeight();
    }

}
function ApplyDiscount(Result) {
    var arrRes = new Array();
    if (parent.GsTheme == "DEFAULT") objlblMsg.style.color = "#000";
    else if (parent.GsTheme == "DARK") objlblMsg.style.color = "#fff";
    arrRes = Result.value;
    document.getElementById("divMsg").style.display = "inline-block";
    objlblMsg.innerHTML = "";
    switch (arrRes[0]) {
        case "catch":
        case "false":
            objlblMsg.innerHTML = arrRes[1]; 
            break;
        case "true":
            parent.GsRetStatus = "false";
            objlblMsg.innerHTML = arrRes[1];
            parent.HideGeneralMedium("Y");
            break;
    }
    parent.adjustGenlMedFrameHeight();
}
function btnRevert_OnClick() {

    PopupProcess("Y");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objhdnMenuID.value;
        ArrRecords[2] = objhdnUserID.value;
        ArrRecords[3] = parent.objhdnSessionID.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSApplyPromotion.RevertDiscount(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "inline-block";
        parent.adjustGenlMedFrameHeight();
    }

}
function RevertDiscount(Result) {
    var arrRes = new Array();
    if (parent.GsTheme == "DEFAULT") objlblMsg.style.color = "#000";
    else if (parent.GsTheme == "DARK") objlblMsg.style.color = "#fff";
    arrRes = Result.value;
    document.getElementById("divMsg").style.display = "inline-block";
    objlblMsg.innerHTML = "";
    switch (arrRes[0]) {
        case "catch":
        case "false":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            parent.GsRetStatus = "false";
            objlblMsg.innerHTML = arrRes[1];
            parent.HideGeneralMedium("Y");
            break;
    }
    parent.adjustGenlMedFrameHeight();
}
function ShowProcess(Result, MethodName) {
    HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "ApplyDiscount":
            ApplyDiscount(Result);
            break;
        case "RevertDiscount":
            RevertDiscount(Result);
            break;
       
    }

}
function ResetValueDecimal(objCtrlID, decimalPlace) {

    var objCtrl;
    if (decimalPlace == null) decimalPlace = parent.GiDecimal;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value.replace("-", ""), decimalPlace);

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