function CheckError() { }
function btnChangePwd_OnClick() {
    parent.PopupProcess("Y");
    var arrRecord = new Array();
   
    try {
        arrRecord[0] = objhdnID.value;;
        arrRecord[1] = objtxtPassword.value;
        arrRecord[2] = objtxtNewPassword.value;
        arrRecord[3] = objtxtConfirmPassword.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSChangePwd.ChangePassword(arrRecord, ShowProcess)
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnChangePwd_OnClick()", expErr.message, "true");
    }
}
function ChangePassword(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ChangePassword()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ChangePassword()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ChangePassword()", arrRes[1], "false");    
            parent.GsLogout = "Y";
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "ChangePassword":
            ChangePassword(Result);
            break;
    }
}
function btnClose_OnClick() {
    parent.LoadHome();
}
