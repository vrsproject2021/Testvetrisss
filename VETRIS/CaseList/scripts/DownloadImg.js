$(document).ready($(function () {

    parent.adjustGenlMedFrameHeight();
    CheckError();

}));

function adjustFrameHeight() {
    var frame = parent.objiframeDL;
    var frameDoc = getFrameDocument(frame);
    var ht = frameDoc.body.scrollHeight;
    frame.style.height = ht.toString() + "px";
}
function getFrameDocument(frame) {
    var rv = null;
   
    if (frame.contentDocument)
        rv = frame.contentDocument;
    else // bad Internet Explorer  ;)
        rv = document.frames[aID].document;
    return rv;
}
function getElement(aID) {

    return (document.getElementById) ? document.getElementById(aID) : document.all[aID];
}
function CheckError() {
    if (objhdnArchFolderPath.value == "") {
        document.getElementById("divMsg").style.display = "block";
    }
    else {
        document.getElementById("divDL").style.display = "block";
    }
    //parent.adjustGenlMedFrameHeight();
    //adjustFrameHeight();
    CopyFolder();
}

function CopyFolder() {

    var ArrRecords = new Array();
    var arrRes = new Array();

    try {
        document.getElementById("spnCopyMsg").innerHTML = "Copying folder from archive...";
        document.getElementById("divCopyLoad").style.display = "inline";
        document.getElementById("divCopyInfo").style.display = "none";

        ArrRecords[0] = objhdnArchFolderPath.value;
        ArrRecords[1] = objhdnArchAltFolderPath.value;
        ArrRecords[2] = objhdnArchFolderName.value;
        ArrRecords[3] = objhdnTgtFolderName.value;
        ArrRecords[4] = objhdnDCMMODIFYEXEPATH.value;
        ArrRecords[5] = "N"; if (objhdnRadFnRights.value.indexOf("VWINSTINFO") > -1) ArrRecords[4] = "Y";
        ArrRecords[6] = objhdnInstCode.value;
        ArrRecords[7] = objhdnPhysCode.value;
        ArrRecords[8] = objhdnSuffix.value;
        ArrRecords[9] = objhdnUserID.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSDownloadImg.CopyFolder(ArrRecords, ShowProcess);
        

    }
    catch (expErr) {
        document.getElementById("divCopyLoad").style.display = "none";
        document.getElementById("divCopyInfo").style.display = "none";
        document.getElementById("divCopyErr").style.display = "inline";
        document.getElementById("spnCopyMsg").innerHTML = expErr.message;
    }

}
function ProcessCopy(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            document.getElementById("divCopyLoad").style.display = "none";
            document.getElementById("divCopyInfo").style.display = "none";
            document.getElementById("divCopyErr").style.display = "none";
            document.getElementById("spnCopyMsg").innerHTML = arrRes[1];
            break;
        case "true":
            document.getElementById("divCopyLoad").style.display = "none";
            document.getElementById("divCopyErr").style.display = "none";
            document.getElementById("divCopyInfo").style.display = "none";
            document.getElementById("spnCopyMsg").innerHTML = arrRes[1];
            CompressFolder();
            break;
    }
}
function CompressFolder() {
    var ArrRecords = new Array();
    var arrRes = new Array();

    try {
        document.getElementById("spnZipMsg").innerHTML = "Compressing the copied folder...";
        document.getElementById("divZipLoad").style.display = "inline";
        document.getElementById("divZipInfo").style.display = "none";
        document.getElementById("divZipErr").style.display = "none";
        ArrRecords[0] = objhdnTgtFolderName.value;
        ArrRecords[1] = objhdnUserID.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSDownloadImg.CompressFolder(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        document.getElementById("divZipLoad").style.display = "none";
        document.getElementById("divZipInfo").style.display = "none";
        document.getElementById("divZipErr").style.display = "inline";
        document.getElementById("spnZipMsg").innerHTML = expErr.message;
    }
}
function ProcessCompress(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            document.getElementById("divZipLoad").style.display = "none";
            document.getElementById("divZipInfo").style.display = "none";
            document.getElementById("divZipErr").style.display = "none";
            document.getElementById("spnZipMsg").innerHTML = arrRes[1];
            break;
        case "true":
            document.getElementById("divZipLoad").style.display = "none";
            document.getElementById("divZipErr").style.display = "none";
            document.getElementById("divZipInfo").style.display = "none";
            document.getElementById("spnZipMsg").innerHTML = arrRes[1];
            location.href = "Temp/" + objhdnUserID.value + "/" + arrRes[2];
            break;
    }
}
function ShowProcess(Result, MethodName) {
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "CopyFolder":
            ProcessCopy(Result);
            break;
        case "CompressFolder":
            ProcessCompress(Result);
            break;
       
    }
}