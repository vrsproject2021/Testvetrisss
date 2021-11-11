/// <reference path="../Common/Help/VRSHelp.aspx" />
var GsText = ""; var strRootDirectory = objhdnRootDirectory.value; var strDivider = objhdnDivider.value; var GsLaunchURL = ""; var GsConfirmAction = "";
var GsTheme = objhdnTheme.value;

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
            document.getElementById("btnLogin").disabled = true;
        }
        else if (arrErr[1] == "004") {
            PopupConfirm(arrErr[1]);
            //PopupMessage(strRootDirectory, strForm, "OnLoad()", arrErr[1], "true");
        }

    }
   
    objhdnError.value = "";
}
else {

    if (Trim(objhdnUID.value) != "") {
        objtxtLoginID.value = Trim(objhdnUID.value);
        Login_Onclick();
    }
    else if (Trim(objhdnTempInstID.value) != "00000000-0000-0000-0000-000000000000") {
        RegistrationLogin_Onclick(Trim(objhdnTempInstID.value));
    }
}
function txtPwd_OnKeyPress(e) {
    var Num; var keyChar; var NumCheck;
    if (window.event) Num = e.keyCode;
    else if (e.which) Num = e.which;
    else if ((e.which) == undefined) Num = e.keyCode;
    if (Num == 13) Login_Onclick();
}
function Login_Onclick() {
    GsText = "Validating";
    PopupLoad();

    //var brwDtls = getBrowserDtls();

    var ArrRecords = new Array();
    try {
     
        ArrRecords[0] = objtxtLoginID.value;
        ArrRecords[1] = objtxtPwd.value;
        if (objchkRemember.checked) ArrRecords[2] = "Y"; else ArrRecords[2] = "N";

        AjaxPro.timeoutPeriod = 1800000;
        VRSLogin.ValidateLogin(ArrRecords,ShowProcess);
    }
    catch (expErr) {
        HideLoad();
        PopupMessage(strRootDirectory, strForm, "Login_Onclick()", expErr.message, "true");
    }

}
function getBrowserDtls() {
    var navUserAgent = navigator.userAgent;
    var browserName = navigator.appName;
    var browserVersion = '' + parseFloat(navigator.appVersion);
    var majorVersion = parseInt(navigator.appVersion, 10);
    var tempNameOffset, tempVersionOffset, tempVersion;


    if ((tempVersionOffset = navUserAgent.indexOf("Opera")) != -1) {
        browserName = "Opera";
        browserVersion = navUserAgent.substring(tempVersionOffset + 6);
        if ((tempVersionOffset = navUserAgent.indexOf("Version")) != -1)
            browserVersion = navUserAgent.substring(tempVersionOffset + 8);
    } else if ((tempVersionOffset = navUserAgent.indexOf("MSIE")) != -1) {
        browserName = "Microsoft Internet Explorer";
        browserVersion = navUserAgent.substring(tempVersionOffset + 5);
    } else if ((tempVersionOffset = navUserAgent.indexOf("Chrome")) != -1) {
        browserName = "Chrome";
        browserVersion = navUserAgent.substring(tempVersionOffset + 7);
    } else if ((tempVersionOffset = navUserAgent.indexOf("Safari")) != -1) {
        browserName = "Safari";
        browserVersion = navUserAgent.substring(tempVersionOffset + 7);
        if ((tempVersionOffset = navUserAgent.indexOf("Version")) != -1)
            browserVersion = navUserAgent.substring(tempVersionOffset + 8);
    } else if ((tempVersionOffset = navUserAgent.indexOf("Firefox")) != -1) {
        browserName = "Firefox";
        browserVersion = navUserAgent.substring(tempVersionOffset + 8);
    } else if ((tempNameOffset = navUserAgent.lastIndexOf(' ') + 1) < (tempVersionOffset = navUserAgent.lastIndexOf('/'))) {
        browserName = navUserAgent.substring(tempNameOffset, tempVersionOffset);
        browserVersion = navUserAgent.substring(tempVersionOffset + 1);
        if (browserName.toLowerCase() == browserName.toUpperCase()) {
            browserName = navigator.appName;
        }
    }

    // trim version
    if ((tempVersion = browserVersion.indexOf(";")) != -1)
        browserVersion = browserVersion.substring(0, tempVersion);
    if ((tempVersion = browserVersion.indexOf(" ")) != -1)
        browserVersion = browserVersion.substring(0, tempVersion);

    //alert("BrowserName = " + browserName + "\n" + "Version = " + browserVersion);
    return browserName + " - " + browserVersion;


}
function ValidateLogin(Result) {

    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            PopupMessage(strRootDirectory, strForm, "ValidateLogin()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "004") {
                GsConfirmAction = "UNLOCK";
                PopupConfirm(arrRes[1]);
            }
            else
                PopupMessage(strRootDirectory, strForm, "ValidateLogin()", arrRes[1], "true", arrRes[2], arrRes[3]);
            break;
        case "true":
            PopupLoad();
            if (objchkRemember.checked) {
                set_cookie("vrsrememberid", "1", arrRes[4], arrRes[5], arrRes[6]);
                set_cookie("vrsLoginId", Trim(objtxtLoginID.value.toLowerCase()), arrRes[4], arrRes[5], arrRes[6]);
            }
            else {
                set_cookie("vrsrememberid", "0", arrRes[4], arrRes[5], arrRes[6]);
                set_cookie("vrsLoginId", "", arrRes[4], arrRes[5], arrRes[6]);
            }
            location.href = "VRSMain.aspx?uid=" + arrRes[1] + "&ucd=" + arrRes[2] + "&unm=" + arrRes[3] + "&mid=" + objhdnMenuID.value + "&sid=" + arrRes[7];
            break;
    }

}

function RegistrationLoginValidate(Result) {

    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            PopupMessage(strRootDirectory, strForm, "ValidateLogin()", arrRes[1], "true");
            break;
        case "false":

            PopupMessage(strRootDirectory, strForm, "ValidateLogin()", arrRes[1], "true", arrRes[2], arrRes[3]);
            break;
        case "true":
            PopupLoad();
            if (objchkRemember.checked) {
                set_cookie("vrsrememberid", "1", arrRes[4], arrRes[5], arrRes[6]);
                set_cookie("vrsLoginId", Trim(objtxtLoginID.value.toLowerCase()), arrRes[4], arrRes[5], arrRes[6]);
            }
            else {
                set_cookie("vrsrememberid", "0", arrRes[4], arrRes[5], arrRes[6]);
                set_cookie("vrsLoginId", "", arrRes[4], arrRes[5], arrRes[6]);
            }
            location.href = "VRSMain.aspx?ucd=" + arrRes[1] + "&unm=" + arrRes[2] + "&tmpid=" + objhdnTempInstID.value;
            break;
    }

}

function RegistrationLogin_Onclick(val) {
    GsText = "Validating";
    PopupLoad();
    try {
        AjaxPro.timeoutPeriod = 1800000;
        VRSLogin.RegistrationLoginValidate(objhdnTempInstID.value, ShowProcess);
    }
    catch (expErr) {
        HideLoad();
        PopupMessage(strRootDirectory, strForm, "Login_Onclick()", expErr.message, "true");
    }

}

function PWDHelp() {
    GsLaunchURL = "VRSForgotPwd.aspx?id=" + objtxtLoginID.value + "&th=" + GsTheme;
    PopupGeneralSmall();
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
    var sUrl = "htmls/" + GsTheme + "/Loading.html";
    $('#tblProcess1').surfOverlay('ld', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideLoad() {
    closepopup('ld');
}
function PopupMessage(argRootDirectory, argForm, argMethod, argErrCode, argShowErr, argsText1, argsText2, argsRet) {
    if (argsText1 == null) argsText1 = ""; if (argsText2 == null) argsText2 = "";
    if (argsRet == null) argsRet = "";
    GsLaunchURL = "Common/VRSMessages.aspx?FORM=" + argForm + "&METHOD=" + argMethod + "&ERRCODE=" + argErrCode + "&ShowErr=" + argShowErr + "&TEXT1=" + argsText1 + "&TEXT2=" + argsText2 + "&RETVAL=" + argsRet + "&TH=" + GsTheme;
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
    GsLaunchURL = "Common/VRSConfirm.aspx?ERRCODE=" + argErrCode + "&TEXT1=" + argTxt1 + "&TEXT2=" + argTxt2 + "&TH=" + GsTheme;
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
function ShowProcess(Result, MethodName) {
    GsText = "";
    
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "ValidateLogin":
            HideLoad();
            ValidateLogin(Result);
            break;
        case "UserUnlock":
            HideProcess();
            ProcessUserUnlock(Result);
            break;
        case "MailPassWord":
            PopMailPassWOrd(Result);
            break;
        case "RegistrationLoginValidate":
            HideLoad();
            RegistrationLoginValidate(Result);
            break;

    }
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
function ProcessConfirm(Args) {
    if (Args == "Y") {
        PopupProcess("Y");
        var ArrRecords = new Array();
        try {
            ArrRecords[0] = objtxtLoginID.value;

            AjaxPro.timeoutPeriod = 1800000;
            VRSLogin.UserUnlock(ArrRecords, ShowProcess);
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

