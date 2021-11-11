$(document).ready($(function () {
    parent.window.scrollTo(0, 0);
}))

function CheckError() {
    var arrErr = new Array();
    if (parent.Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                parent.PopupMessage(RootDirectory, strForm, "OnLoad()", arrErr[1], "true");
                break;
        }
    }
    objhdnError.value = "";
}
function btndownload_Onclick() {
    
    try {

        if (parent.Trim(objddlInstitution.value) != "") {
            parent.PopupProcess("N");
            AjaxPro.timeoutPeriod = 1800000;
            VRSDownloadRouter.PrepareDownload(objhdnVer.value, objddlInstitution.value, UserID, ShowProcess);
        }
        else {
            parent.PopupMessage(RootDirectory, strForm, "btndownload_Onclick()","274", "true");
        }
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btndownload_Onclick()", expErr.message, "true");
    }
}
function ProcessDownload(Result) {
    var arrRes = new Array();
    var strFilePath = "";
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessDownload()", arrRes[1], "true");
            break;
        case "true":
            parent.GsRetStatus = "false";
            strFilePath = "/" + RootDirectory + arrRes[1];
            var win = window.open(strFilePath, '_blank');
            win.focus();
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "PrepareDownload":
            ProcessDownload(Result);
            break;

    }

}