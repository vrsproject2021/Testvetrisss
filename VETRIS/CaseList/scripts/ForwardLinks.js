$(document).ready($(function () {
    window.history.forward();
    parent.adjusDataListFrameHeight();
    CheckError();

}))
var ServerPath = parent.objhdnServerPath.value; var GsPopupText = "";
function Trim(str) {
    while (str.charAt(0) == (" ")) {
        str = str.substring(1);
    }
    while (str.charAt(str.length - 1) == " ") {
        str = str.substring(0, str.length - 1);
    }
    return str;
}
function CheckError() {
    if (parent.Trim(objhdnError.value) != "") {
        document.getElementById("divMsg").style.display = "inline-block";
        parent.adjusDataListFrameHeight();
    }
    objhdnError.value = "";
    ToggleNotification();
}

function ToggleNotification() {
    document.getElementById("divMsg").style.display = "none";
    objlblMsg.innerHTML = "";
  
    if (objrdoEmail.checked) {
        document.getElementById("divEmail").style.display = "block";
        document.getElementById("divSMS").style.display = "none";
        document.getElementById("divFax").style.display = "none";
        GenerateReport();
    }
    else if (objrdoSMS.checked) {
        document.getElementById("divEmail").style.display = "none";
        document.getElementById("divSMS").style.display = "block";
        document.getElementById("divFax").style.display = "none";
    }
    else if (objrdoFax.checked) {
        document.getElementById("divEmail").style.display = "none";
        document.getElementById("divSMS").style.display = "none";
        document.getElementById("divFax").style.display = "block";
        GenerateReport();
    }

    if (objhdnFAXENABLE.value == "Y") {
        if (objhdnFaxRpt.value == "N") objrdoFax.disabled = true;
        else objrdoFax.disabled = false;
    }
    else {
        objrdoFax.disabled = true;
    }

    chkRpt_OnClick();chkImg_OnClick();
    parent.adjusDataListFrameHeight();
}
function chkRpt_OnClick() {

    if (document.getElementById("spnEmailRptLink") == null) {
        objlblEmailPrev.innerHTML += "<br/><span id='spnEmailRptLink'></span><br/>";
    }
    if (document.getElementById("spnSMSRptLink") == null) {
        objlblSMSPrev.innerHTML += "<br/><span id = 'spnSMSRptLink'></span><br/>";
    }

    if (objrdoEmail.checked) {
        if(objchkRpt.checked)
            document.getElementById("spnEmailRptLink").innerHTML = "<a href='" + objhdnVRSPACSLINKURL.value + "&vw=rpt&fmt=" + objhdnFmt.value + "&target='blank'>Click here to view the report</a>";
        else
            document.getElementById("spnEmailRptLink").innerHTML = "";
    }
    else if (objrdoSMS.checked) {
        if(objchkRpt.checked)
            document.getElementById("spnSMSRptLink").innerHTML = "Click " + objhdnVRSPACSLINKURL.value + "&vw=rpt&fmt=" + objhdnFmt.value + " to view the report";
        else
            document.getElementById("spnSMSRptLink").innerHTML = "";
    } 
    
        
}
function chkImg_OnClick() {
    if (document.getElementById("spnEmailImgLink") == null) {
        objlblEmailPrev.innerHTML += "<br/><span id = 'spnEmailImgLink'></span><br/>";
    }
    if (document.getElementById("spnSMSImgLink") == null) {
        objlblSMSPrev.innerHTML += "<br/><span id = 'spnSMSImgLink'></span><br/>";
    }

    if (objrdoEmail.checked) {
        if(objchkImg.checked)
            document.getElementById("spnEmailImgLink").innerHTML = "<a href='" + objhdnVRSPACSLINKURL.value + "&vw=img' target='blank'>Click here to view the image(s)</a>";
        else
            document.getElementById("spnEmailImgLink").innerHTML ="";
    }
    else if (objrdoSMS.checked) {
        if(objchkRpt.checked)
            document.getElementById("spnSMSImgLink").innerHTML = "Click " + objhdnVRSPACSLINKURL.value + "&vw=img to view the image(s)";
        else
            document.getElementById("spnSMSImgLink").innerHTML = "";
    }
}

function GenerateReport() {
    PopupProcess("N");

    try{
        AjaxPro.timoutPeriod = 1800000;
        if (objhdnStudyStatus.value == "100") {
            if (objhdnCustomRpt.value == "Y") {
                VRSForwardLinks.PrintCustomFinalReport(objhdnStudyID.value, objhdnPName.value, objhdnUserID.value, ShowProcess);
            }
            else {
                VRSForwardLinks.PrintFinalReport(objhdnStudyID.value, objhdnPName.value, objhdnUserID.value, ShowProcess);
            }
        }
        else if (objhdnStudyStatus.value == "80") {
            if (objhdnCustomRpt.value == "Y") {
                VRSForwardLinks.PrintCustomPreliminaryReport(objhdnStudyID.value, objhdnPName.value, objhdnUserID.value, ShowProcess);
            }
            else {
                VRSForwardLinks.PrintPreliminaryReport(objhdnStudyID.value, objhdnPName.value, objhdnUserID.value, ShowProcess);
            }
        }
    }
    catch (expErr) {
        HideProcess();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "block";
        parent.adjusDataListFrameHeight();
    }
    
}
function ProcessReport(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    if (parent.GsTheme == "DEFAULT") objlblMsg.style.color = "black";
    else if (parent.GsTheme == "DARK") objlblMsg.style.color = "white";
    document.getElementById("divMsg").style.display = "inline";
    objlblMsg.innerHTML = "";

    switch (arrRes[0]) {
        case "catch":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            
            if (objrdoFax.checked) {
                objhdnFaxFilePath.value = arrRes[1];
                if (document.all) objlblFileName.innerText = arrRes[2]; else objlblFileName.textContent = arrRes[2];
            }
            if (objrdoEmail.checked) {
                objhdnEmailFilePath.value = arrRes[1];
                objlblAttachment.innerHTML = "<a href='#' onclick='javascript:ShowReport();'>" +  arrRes[2] + "</a>";
               
            }
            objhdnFileName.value = arrRes[2];
            objhdnRptServerPath.value = arrRes[3];
            break;
    }

    parent.adjusDataListFrameHeight();
}
function ShowReport() {
    var strFilePath = objhdnRptServerPath.value;
    var win = window.open(strFilePath, '_blank');
    win.focus();
}
function btnSend_OnClick() {
    if (objrdoEmail.checked) SendMail();
    else if (objrdoSMS.checked) SendSMS();
    else if (objrdoFax.checked) SendFax();
}
function SendMail() {
    PopupMail();
    var ArrRecords = new Array();
    var mail_text = "";

    try {
        mail_text = objlblEmailPrev.innerHTML;
        mail_text += "<br/>" + objtxtBody.value;
        ArrRecords[0] = objhdnMAILSVRNAME.value;
        ArrRecords[1] = objhdnMAILSVRPORT.value;
        ArrRecords[2] = objhdnMAILSSLENABLED.value;
        ArrRecords[3] = objhdnMAILSVRUSRCODE.value;
        ArrRecords[4] = objhdnMAILSVRUSRPWD.value;
        ArrRecords[5] = objtxtTo.value;
        ArrRecords[6] = objtxtCC.value;
        ArrRecords[7] = objtxtSubject.value;
        ArrRecords[8] = mail_text;
        ArrRecords[9] = objhdnEmailFilePath.value;
        ArrRecords[10] = objhdnFileName.value;

        AjaxPro.timoutPeriod = 1800000;
        VRSForwardLinks.SendMail(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        HideMail();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "block";
        parent.adjusDataListFrameHeight();
    }
}
function ProcessMail(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    if (parent.GsTheme == "DEFAULT") objlblMsg.style.color = "black";
    else if (parent.GsTheme == "DARK") objlblMsg.style.color = "white";
    document.getElementById("divMsg").style.display = "inline";
    objlblMsg.innerHTML = "";

    switch (arrRes[0]) {
        case "catch":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            objlblMsg.innerHTML = arrRes[1];
            break;
    }

    parent.adjusDataListFrameHeight();
}
function SendSMS() {
    PopupSMS();
    var ArrParams = new Array();
    var ArrText = new Array();

    try {

        ArrParams[0] = objhdnSMSACCTSID.value;
        ArrParams[1] = objhdnSMSAUTHTKNNO.value;
        ArrParams[2] = objhdnSMSSENDERNO.value;
        ArrParams[3] = objtxtMobileNo.value;

        ArrText[0] = document.getElementById("spnSMShdr").innerHTML;
        ArrText[1] = document.getElementById("spnSMSRptLink").innerHTML;
        ArrText[2] = document.getElementById("spnSMSImgLink").innerHTML;

        AjaxPro.timoutPeriod = 1800000;
        VRSForwardLinks.SendSMS(ArrParams,ArrText, ShowProcess);
    }
    catch (expErr) {
        HideSMS();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "block";
        parent.adjusDataListFrameHeight();
    }
}
function ProcessSMS(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    if (parent.GsTheme == "DEFAULT") objlblMsg.style.color = "black";
    else if (parent.GsTheme == "DARK") objlblMsg.style.color = "white";
    document.getElementById("divMsg").style.display = "inline";
    objlblMsg.innerHTML = "";

    switch (arrRes[0]) {
        case "catch":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "false":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            objlblMsg.innerHTML = "SMS Sent successfully";
            break;
    }

    parent.adjusDataListFrameHeight();
}
function SendFax() {
    PopupFax();
    var ArrParams = new Array();

    try {

        ArrParams[0] = objhdnFAXAPIURL.value;
        ArrParams[1] = objhdnFAXAUTHUSERID.value;
        ArrParams[2] = objhdnFAXAUTHPWD.value;
        ArrParams[3] = objhdnFAXCSID.value;
        ArrParams[4] = objhdnFAXREFTEXT.value;
        ArrParams[5] = objhdnFAXREPADDR.value;
        ArrParams[6] = objhdnFAXCONTACT.value;
        ArrParams[7] = objhdnFAXRETRY.value;
        ArrParams[8] = objhdnFaxFilePath.value;
        ArrParams[9] = objtxtFaxNo.value;

        
        AjaxPro.timoutPeriod = 1800000;
        VRSForwardLinks.SendFax(ArrParams, ShowProcess);
    }
    catch (expErr) {
        HideFax();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "block";
        parent.adjusDataListFrameHeight();
    }
}
function ProcessFax(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    if (parent.GsTheme == "DEFAULT") objlblMsg.style.color = "black";
    else if (parent.GsTheme == "DARK") objlblMsg.style.color = "white";
    document.getElementById("divMsg").style.display = "inline";
    objlblMsg.innerHTML = "";

    switch (arrRes[0]) {
        case "catch":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "false":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            objlblMsg.innerHTML = "Fax Sent successfully";
            break;
    }

    parent.adjusDataListFrameHeight();
}
function ShowProcess(Result, MethodName) {
    HideMail(); HideSMS(); HideProcess(); HideFax();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SendMail":
            ProcessMail(Result);
            break;
        case "SendSMS":
            ProcessSMS(Result);
            break;
        case "SendFax":
            ProcessFax(Result);
            break;
        case "PrintCustomFinalReport":
        case "PrintFinalReport":
        case "PrintCustomPreliminaryReport":
        case "PrintPreliminaryReport":
            ProcessReport(Result);
            break;
        
    }
}
function PopupMail() {
    var sUrl = "../htmls/" + parent.GsTheme + "/mail.html";
    $('#tblMail').surfOverlay('mail', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideMail() {
    closepopup('mail');
}
function PopupSMS() {
    var sUrl = "../htmls/" + parent.GsTheme + "/sms.html";
    $('#tblSMS').surfOverlay('sms', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideSMS() {
    closepopup('sms');
}
function PopupFax() {
    var sUrl = "../htmls/" + parent.GsTheme + "/fax.html";
    $('#tblFax').surfOverlay('fax', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideFax() {
    closepopup('fax');
}
function PopupProcess(IsSaving) {
    
    if (IsSaving == null) IsSaving = "N";
    var sUrl = "";
    if (IsSaving == "N") sUrl = "../htmls/" + parent.GsTheme + "/processing.html";
    else sUrl = "../htmls/" + parent.GsTheme + "/saving.html";

    $('#tblProcess').surfOverlay('ps', { url: sUrl, zIndex: 3000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });

    return false;
}
function HideProcess() {

    //$('#tblProcess').surfOverlay('ps', { zIndex: 100 });
    closepopup('ps');
}