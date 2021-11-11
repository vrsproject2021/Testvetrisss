var Divider = objhdnDivider.value;
var RootDirectory = objhdnRootDirectory.value;
var GsPopupText = "";

$(document).ready($(function () {
    CheckError();
}));
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
    var arrErr = new Array();
    if (Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                PopupMessage(RootDirectory, strForm, "OnLoad()", arrErr[1], "true");
                break;
        }
    }
    objhdnError.value = "";
    if (objhdnTrans.value == "Y") {
        document.getElementById("div3rdframe").style.display = "block";
    }
    if (document.all) {
        if (objlblAbRptReason.inneText != "") objdivReason.style.display = "block";
    }
    else {
        if (objlblAbRptReason.textContent != "") objdivReason.style.display = "block";
    }

    GenerateSourceReport();
}

function GenerateSourceReport() {
    GsPopupText = "Generating Source Report";
    PopupProcess();
    try {

        AjaxPro.timeoutPeriod = 1800000;
        VRSCompareDocuments.GenerateSourceReport(objhdnID.value, objhdnTrans.value, objhdnPName.value, objhdnFolder.value, objhdnUserID.value, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        PopupMessage(RootDirectory, strForm, "GenerateSourceReport()", expErr.message, "true");
    }
}
function ProcessSourceReport(Result) {
    var arrRes = new Array();
    GsPopupText = "";
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            PopupMessage(RootDirectory, strForm, "ProcessSourceReport()", arrRes[1], "true");
            break;
        case "true":
            objhdnFileName1.value = arrRes[1];
            GenerateFinalReport();
            break;
    }
}
function GenerateFinalReport() {
    GsPopupText = "Generating Final Report";
    PopupProcess();
    try {

        AjaxPro.timeoutPeriod = 1800000;
        VRSCompareDocuments.GenerateFinalReport(objhdnID.value, objhdnPName.value, objhdnFolder.value, objhdnUserID.value, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        PopupMessage(RootDirectory, strForm, "GenerateFinalReport()", expErr.message, "true");
    }
}
function ProcessFinalReport(Result) {
    var arrRes = new Array();
    GsPopupText = "";
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            PopupMessage(RootDirectory, strForm, "ProcessFinalReport()", arrRes[1], "true");
            break;
        case "true":
            objhdnFileName2.value = arrRes[1];
            if (objhdnTrans.value == "N") CompareReports();
            else {
                objiframeDoc4.src = "DocPrint/CompareTemp/" + objhdnUserID.value + "/" + objhdnFileName1.value
                GenerateRadiologistReport();
            }
            break;
    }
}
function GenerateRadiologistReport() {
    GsPopupText = "Generating Radiologist Report";
    PopupProcess();
    try {

        AjaxPro.timeoutPeriod = 1800000;
        VRSCompareDocuments.GenerateRadiologistReport(objhdnID.value, objhdnPName.value, objhdnFolder.value, objhdnUserID.value, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        PopupMessage(RootDirectory, strForm, "GenerateFinalReport()", expErr.message, "true");
    }
}
function ProcessRadiologistReport(Result) {
    var arrRes = new Array();
    GsPopupText = "";
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            PopupMessage(RootDirectory, strForm, "ProcessRadiologistReport()", arrRes[1], "true");
            break;
        case "true":
            objhdnFileName3.value = arrRes[1];
            objiframeDoc3.src = "DocPrint/CompareTemp/" + objhdnUserID.value + "/" + arrRes[1];
            CompareReports();
            break;
    }
}

function CompareReports() {
    GsPopupText = "Comparing Reports";
    PopupProcess();
    try {

        AjaxPro.timeoutPeriod = 1800000;
        VRSCompareDocuments.CompareReports(objhdnFileName1.value, objhdnFileName2.value, objhdnFolder.value, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        PopupMessage(RootDirectory, strForm, "CompareReports()", expErr.message, "true");
    }
}
function ProcessCompareReport(Result) {
    var arrRes = new Array();
    GsPopupText = "";
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            PopupMessage(RootDirectory, strForm, "ProcessCompareReport()", arrRes[1], "true");
            break;
        case "true":
            objiframeDoc1.src = "DocPrint/CompareTemp/" + objhdnUserID.value + "/" + arrRes[1];
            objiframeDoc2.src = "DocPrint/CompareTemp/" + objhdnUserID.value + "/" + arrRes[2];
            break;
    }
}
function btnClose_OnClick() {
    window.close();
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
function PopupMessage(argRootDirectory, argForm, argMethod, argErrCode, argShowErr, argsText1, argsText2, argsRet) {
    if (argsText1 == null) argsText1 = ""; if (argsText2 == null) argsText2 = "";
    if (argsRet == null) argsRet = "";
    GsLaunchURL = "../Common/VRSMessages.aspx?FORM=" + argForm + "&METHOD=" + argMethod + "&ERRCODE=" + argErrCode + "&ShowErr=" + argShowErr + "&TEXT1=" + argsText1 + "&TEXT2=" + argsText2 + "&RETVAL=" + argsRet;
    var sUrl = "../htmls/message.html";
    $('#tblMsg').surfOverlay('msg', { url: sUrl, zIndex: 4000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideMessage(ArgsRet) {
    $('#tblMsg').surfOverlay('msg', { zIndex: 100 });
    closepopup('msg');
    GsLaunchURL = "";

    if (ArgsRet != null) {
        if (ArgsRet == "068")
            ProcessMessage(ArgsRet);
        else if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessMessage(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessMessage(ArgsRet);
        }
    }
}
function ShowProcess(Result, MethodName) {
    GsPopupText = "";
    HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "GenerateSourceReport":
            ProcessSourceReport(Result);
            break;
        case "GenerateFinalReport":
            ProcessFinalReport(Result);
            break;
        case "GenerateRadiologistReport":
            ProcessRadiologistReport(Result);
            break;
        case "CompareReports":
            ProcessCompareReport(Result);
            break;

    }
}