/// <reference path="../Common/Help/VRSHelp.aspx" />
var GsText = ""; var strRootDirectory = objhdnRootDirectory.value; var strDivider = objhdnDivider.value; var GsLaunchURL = ""; var GsConfirmAction = "";


if (Trim(objhdnError.value) != "") {
    var strError = objhdnError.value;
   // var strErrCodes = objhdnError.value.substring(objhdnError.value.indexOf(strDivider) + 1, objhdnError.value.length);
    var arrErr = new Array();
    if (strError.indexOf(strDivider) > -1)
        arrErr = strError.split(strDivider);
    else
        arrErr[0] = strError;

    if (arrErr[0] == "catch")
        PopupMessage(strRootDirectory, strForm, "OnLoad()", arrErr[1], "true");
    else if (arrErr[0] == "false") {
        if (arrErr[1] == "001") {
            var strAppVer = "";
            var strDBVer = objhdnDBVer.value;
            if (document.all)
                strAppVer = objlblVersion.innerText;
            else
                strAppVer = objlblVersion.textContent;
            PopupMessage(strRootDirectory, strForm, "OnLoad()", arrErr[1], "true", strDBVer, strAppVer);
            document.getElementById("btnChange").disabled = true;
        }
        else if (arrErr[1] == "004") {
            PopupConfirm(arrErr[1]);
            PopupMessage(strRootDirectory, strForm, "OnLoad()", arrErr[1], "true");
        }
        else {
            PopupMessage(strRootDirectory, strForm, "OnLoad()", arrErr[1], "true");
        }

    }
   
    objhdnError.value = "";
}

function txtPwd_OnKeyPress(e) {
    var Num; var keyChar; var NumCheck;
    if (window.event) Num = e.keyCode;
    else if (e.which) Num = e.which;
    else if ((e.which) == undefined) Num = e.keyCode;
    if (Num == 13) Login_Onclick();
}
function set_cookie(strName, strValue, strExp_y, strExp_m, strExp_d) {
    var strCookieString = strName + "=" + escape(strValue);

    if (strExp_y) {
        var strExpires = new Date(strExp_y, strExp_m, strExp_d);
        strCookieString += "; expires=" + strExpires.toGMTString();
    }

    document.cookie = strCookieString;
}
function read_cookie(cookieName) {
    var theCookie = "" + document.cookie;
    var ind = theCookie.indexOf(cookieName);
    if (ind == -1 || cookieName == "") return "";
    var ind1 = theCookie.indexOf(';', ind);
    if (ind1 == -1) ind1 = theCookie.length;
    return unescape(theCookie.substring(ind + cookieName.length + 1, ind1));
}
function PopupProcess(IsSaving) {
    if (IsSaving == null) IsSaving = "N";
    var sUrl = "";
    if (IsSaving == "N") sUrl = "htmls/processing.html";
    else sUrl = "htmls/saving.html";

    $('#tblProcess').surfOverlay('ps', { url: sUrl, zIndex: 3000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });

    return false;
}
function HideProcess() {

    //$('#tblProcess').surfOverlay('ps', { zIndex: 100 });
    closepopup('ps');
}
function PopupLoad() {
    var sUrl = "htmls/Loading.html";
    $('#tblProcess1').surfOverlay('ld', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideLoad() {
    closepopup('ld');
}
function PopupMessage(argRootDirectory, argForm, argMethod, argErrCode, argShowErr, argsText1, argsText2, argsRet) {
    if (argsText1 == null) argsText1 = ""; if (argsText2 == null) argsText2 = "";
    if (argsRet == null) argsRet = "";
    GsLaunchURL = "Common/VRSMessages.aspx?FORM=" + argForm + "&METHOD=" + argMethod + "&ERRCODE=" + argErrCode + "&ShowErr=" + argShowErr + "&TEXT1=" + argsText1 + "&TEXT2=" + argsText2 + "&RETVAL=" + argsRet;
    var sUrl = "htmls/message.html";
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
function PopupConfirm(argErrCode, argTxt1, argTxt2, argAction) {
    var sUrl = "htmls/confirm.html";
    if (argTxt1 == null) argTxt1 = "";
    if (argTxt2 == null) argTxt2 = "";
    if (argAction != null) GsConfirmAction = argAction;
    GsLaunchURL = "Common/VRSConfirm.aspx?ERRCODE=" + argErrCode + "&TEXT1=" + argTxt1 + "&TEXT2=" + argTxt2;
    $('#tblConf').surfOverlay('cfm', { url: sUrl, zIndex: 3000, imgLoading: false, center: true, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideConfirm(ArgsRet) {
    closepopup('cfm');
    GsLaunchURL = "";

    if (ArgsRet != null) {
        if (GsConfirmAction != "")
            ProcessConfirm(ArgsRet);
        else if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessConfirm(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessConfirm(ArgsRet);
        }

    }


}
function ProcessConfirm(Args) {
    if (Args == "Y") {
        PopupProcess("Y");
        var ArrRecords = new Array();
        try {
            ArrRecords[0] = objtxtLoginID.value;

            AjaxPro.timeoutPeriod = 1800000;
            VRSRegistrationChangePwd.UserUnlock(ArrRecords, ShowProcess);
        }
        catch (expErr) {
            HideProcess();
            PopupMessage(strRootDirectory, strForm, "UnlockUser()", expErr.message, "true");
        }
    }
}
function ProcessUserUnlock(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            PopupMessage(strRootDirectory, strForm, "ProcessUserUnlock()", arrRes[1], "true");
            break;
        case "false":
            PopupMessage(strRootDirectory, strForm, "ProcessUserUnlock()", arrRes[1], "true");
            break;
        case "true":
            PopupMessage(strRootDirectory, strForm, "ProcessUserUnlock()", arrRes[1], "false");
            parent.GsRetStatus = "false"; SearchRecord();
            break;
    }
}
function Trim(str) {
    while (str.charAt(0) == (" ")) {
        str = str.substring(1);
    }
    while (str.charAt(str.length - 1) == " ") {
        str = str.substring(0, str.length - 1);
    }
    return str;
}
function PopupGeneralSmall() {
    var sUrl = "htmls/GeneralSmall.html";
    $('#tblGenlSm').surfOverlay('genlSm', { url: sUrl, zIndex: 2500, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideGeneralSmall(ArgsRet) {
    closepopup('genlSm');

    var strURL = GsLaunchURL;
    GsLaunchURL = "";

    if (ArgsRet != null) {
        if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessGeneralSmall(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessGeneralSmall(ArgsRet);
        }
        else
            ProcessGeneralSmall(ArgsRet);
    }

}

function btnChangePwd_OnClick() {
    parent.PopupProcess("Y");
    var arrRecord = new Array();

    try {
        arrRecord[0] = objhdnTempInstID.value;
        arrRecord[1] = objhdnLoginId.value;;
        arrRecord[2] = objtxtPwd.value;
        arrRecord[3] = objtxtNewPwd.value;
        arrRecord[4] = objtxtConfirmPwd.value;
        arrRecord[5] = objhdnEmailId.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSRegistrationChangePwd.ChangePassword(arrRecord, ShowProcess)
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(strRootDirectory, strForm, "btnChangePwd_OnClick()", expErr.message, "true");
    }
}
function ChangePassword(Result) {
    debugger;
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(strRootDirectory, strForm, "ChangePassword()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(strRootDirectory, strForm, "ChangePassword()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            location.href = "VRSMain.aspx?ucd=" + objtxtUserName.value + "&unm=" + objtxtUserName.value + "&tmpid=" + objhdnTempInstID.value;
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

