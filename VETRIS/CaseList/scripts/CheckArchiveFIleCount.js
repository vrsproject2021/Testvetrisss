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
        document.getElementById("divCount").style.display = "block";
        CheckFileCount();
    }
    
}

function CheckFileCount() {

    var ArrRecords = new Array();
    var arrRes = new Array();

    try {
        document.getElementById("spnCountMsg").innerHTML = "Checking the file sync count...";
        document.getElementById("divCountLoad").style.display = "inline";
        document.getElementById("divCountInfo").style.display = "none";

        ArrRecords[0] = objhdnError.value;
        ArrRecords[0] = objhdnArchFolderPath.value;
        ArrRecords[1] = objhdnArchAltFolderPath.value;


        AjaxPro.timeoutPeriod = 1800000;
        VRSCheckArchiveFileCount.CheckFileCount(ArrRecords, ShowProcess);
        

    }
    catch (expErr) {
        document.getElementById("divCountLoad").style.display = "none";
        document.getElementById("divCountInfo").style.display = "none";
        document.getElementById("divCountErr").style.display = "inline";
        document.getElementById("spnCountMsg").innerHTML = expErr.message;
    }

}
function ProcessCount(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            document.getElementById("divCountLoad").style.display = "none";
            document.getElementById("divCountInfo").style.display = "none";
            document.getElementById("divCountErr").style.display = "none";
            document.getElementById("spnCountMsg").innerHTML = arrRes[1];
            break;
        case "true":
            document.getElementById("divCountLoad").style.display = "none";
            document.getElementById("divCountErr").style.display = "none";
            document.getElementById("divCountInfo").style.display = "none";
            document.getElementById("spnCountMsg").innerHTML = arrRes[1];

            break;
    }
}

function ShowProcess(Result, MethodName) {
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "CheckFileCount":
            ProcessCount(Result);
            break;
    }
}