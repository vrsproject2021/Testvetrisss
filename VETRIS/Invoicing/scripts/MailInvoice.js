$(document).ready($(function () {
    window.history.forward();
    parent.adjusDataListFrameHeight();
    CheckError();

}))

function CheckError() {
    if (parent.Trim(objhdnError.value) != "") {
        document.getElementById("divMsg").style.display = "inline-block";
        parent.adjusDataListFrameHeight();
    }
    objhdnError.value = "";
    parent.adjusDataListFrameHeight();
    parent.adjusDataListFrameHeight();
}

function ShowInvoice() {
    var strFilePath = "/" + objhdnRootDir.value + "/Invoicing/MailTemp/" + objhdnInvFile.value;
    var win = window.open(strFilePath, '_blank');
    win.focus();
}

function btnSend_OnClick() {
    PopupMail();
    var ArrRecords = new Array();
    var objrtbMailText = document.getElementById("rtbMailText");

    try {
        ArrRecords[0] = objhdnMAILSVRNAME.value;
        ArrRecords[1] = objhdnMAILSVRPORT.value;
        ArrRecords[2] = objhdnMAILSSLENABLED.value;
        ArrRecords[3] = objhdnMAILSVRUSRCODE.value;
        ArrRecords[4] = objhdnMAILSVRUSRPWD.value;
        ArrRecords[5] = objhdnSENDMAILID.value;
        ArrRecords[6] = objtxtTo.value;
        ArrRecords[7] = objtxtCC.value;
        ArrRecords[8] = objtxtSubject.value;
        ArrRecords[9] = objrtbMailText.value;
        ArrRecords[10] = objhdnInvFilePath.value;
        ArrRecords[11] = objhdnInvFile.value;

        AjaxPro.timoutPeriod = 1800000;
        VRSMailInvoice.SendMail(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        HideMail();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "inline-block";
        parent.adjusDataListFrameHeight();
    }
}
function ProcessMail(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    if (parent.GsTheme == "DEFAULT") objlblMsg.style.color = "#000";
    else if (parent.GsTheme == "DARK") objlblMsg.style.color = "#fff";
    document.getElementById("divMsg").style.display = "inline-block";
    objlblMsg.innerHTML = "";

    switch (arrRes[0]) {
        case "catch":
        case "false":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            objlblMsg.innerHTML = arrRes[1];
            break;
    }

    parent.adjusDataListFrameHeight();
}
function ShowProcess(Result, MethodName) {
    HideMail();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SendMail":
            ProcessMail(Result);
            break;
        
    }
}
function PopupMail() {
    var sUrl = "../htmls/mail.html";
    $('#tblMail').surfOverlay('mail', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideMail() {
    closepopup('mail');
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