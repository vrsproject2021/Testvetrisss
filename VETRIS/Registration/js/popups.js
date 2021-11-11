var GiWidth = 0; var GiHeight = 0; var GiTop = 0; var GiBuffHt = 0; var MsgFORM = "";
function PopupProcess(IsSaving) {
    if (IsSaving == null) IsSaving = "N";
    var sUrl = "";
    if (IsSaving == "N") sUrl = "../htmls/processing.html";
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
    if (argsRet == null) argsRet = ""; MsgFORM = argForm;
    GsLaunchURL = "../Common/VRSMessages.aspx?FORM=" + argForm + "&METHOD=" + argMethod + "&ERRCODE=" + argErrCode + "&ShowErr=" + argShowErr + "&TEXT1=" + argsText1 + "&TEXT2=" + argsText2 + "&RETVAL=" + argsRet;
    var sUrl = "htmls/message.html";
    $('#tblMsg').surfOverlay('msg', { url: sUrl, zIndex: 4000,imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideMessage(ArgsRet) {
    $('#tblMsg').surfOverlay('msg', { zIndex: 100 });
    closepopup('msg');

    GsLaunchURL = "";
    GsText = "";

    if (MsgFORM == "VRSChangePwd") {
        if (GsLogout == "Y") {
            if (document.all)
                location.href = objhdnServerPath.value +"/VRSLogout.aspx?uid=" + objhdnUserID.value;
            else
                window.location.assign(objhdnServerPath.value + "/VRSLogout.aspx?uid=" + objhdnUserID.value);
        }
    }
    else if (ArgsRet != null) {
        if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessMessage(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessMessage(ArgsRet);
        }
    }
}
function PopupConfirm(argErrCode, argTxt1, argTxt2,argAction) {
    var sUrl = "htmls/confirm.html";
    if (argTxt1 == null) argTxt1 = "";
    if (argTxt2 == null) argTxt2 = "";
    if (argAction != null) GsConfirmAction = argAction;
    GsLaunchURL = "../Common/VRSConfirm.aspx?ERRCODE=" + argErrCode + "&TEXT1=" + argTxt1 + "&TEXT2=" + argTxt2;
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
function PopupUpload() {
    var sUrl = "HTMLS/uploader.html";
    $('#tblUpload').surfOverlay('lupld', { url: sUrl, zIndex: 2000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideUpload(ArgsRet) {
    closepopup('lupld');
    GsLaunchURL = "";

    if (ArgsRet != null) {
        //if (typeof (objiframeEdit) != "undefined") {
        //    if (objiframeEdit != null) {
        //        if (objiframeEdit.contentWindow) {
        //            objiframeEdit.contentWindow.ProcessUpload(ArgsRet);
        //            return;
        //        }
        //        else {
        //            objiframePage.contentDocument.parentWindow.ProcessUpload(ArgsRet);
        //            return;
        //        }
        //    }
        //}
        if (typeof (objiframePage) != "undefined") {
            if (typeof (objiframeEdit) != "undefined") objiframeEdit = undefined;
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessUpload(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessUpload(ArgsRet);
        }


        //else
        //    ProcessUpload(ArgsRet);
    }
    //else
    //    if (typeof (objiframeEdit) != "undefined") objiframeEdit = undefined;
}
