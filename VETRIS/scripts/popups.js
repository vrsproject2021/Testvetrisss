var GsIsDepotInv = "N"; var GiWidth = 0; var GiHeight = 0; var GiTop = 0; var GiBuffHt = 0; var MsgFORM = "";
function PopupProcess(IsSaving) {
    if (IsSaving == null) IsSaving = "N";
    var sUrl = "";
    if (IsSaving == "N") sUrl = "htmls/" + GsTheme + "/processing.html";
    else sUrl = "htmls/" + GsTheme + "/saving.html";

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
function PopupScan() {
    var sUrl = "htmls/Scan.html";
    $('#tblScan').surfOverlay('scan', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideScan() {
    closepopup('scan');
}
function PopupMessage(argRootDirectory, argForm, argMethod, argErrCode, argShowErr, argsText1, argsText2, argsRet) {
    if (argsText1 == null) argsText1 = ""; if (argsText2 == null) argsText2 = "";
    if (argsRet == null) argsRet = ""; MsgFORM = argForm;
    GsLaunchURL = "Common/VRSMessages.aspx?FORM=" + argForm + "&METHOD=" + argMethod + "&ERRCODE=" + argErrCode + "&ShowErr=" + argShowErr + "&TEXT1=" + argsText1 + "&TEXT2=" + argsText2 + "&RETVAL=" + argsRet + "&TH=" + GsTheme;
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
    else if (MsgFORM == "VRSManualSubmissionTemp") {
        if (GsLogout == "Y") {
            if (document.all)
                location.href = objhdnServerPath.value + "/VRSLogin.aspx";
            else
                window.location.assign(objhdnServerPath.value + "/VRSLogin.aspx");
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
function PopupGeneral() {
    var sUrl = "htmls/General.html";
    if (GsLaunchURL.indexOf("VRSChangePassword.aspx") > -1)
        $('#tblGenl').surfOverlay('genl', { url: sUrl, zIndex: 3000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    else
        $('#tblGenl').surfOverlay('genl', { url: sUrl, zIndex: 2500, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideGeneral(ArgsRet) {
    closepopup('genl');

    var strURL = GsLaunchURL;
    GsLaunchURL = "";
    GsText = "";

    if (strURL.substring(0, strURL.indexOf(".")) == "VRSChangePassword") {
        if (GsLogout == "Y") {
            if (document.all)
                location.href = "VRSLogout.aspx?uid=" + objhdnUserID.value + "&cmp=" + objhdnCompanyID.value;
            else
                window.location.assign("VRSLogout.aspx?uid=" + objhdnUserID.value + "&cmp=" + objhdnCompanyID.value);
        }
    }
    else if (ArgsRet != null) {
        if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessGeneral(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessGeneral(ArgsRet);
        }
    }
}
function PopupGeneralMedium() {
    var sUrl = "htmls/GeneralMedium.html";
    if (GsLaunchURL.indexOf("VRSChangePassword.aspx") > -1)
        $('#tblGenlM').surfOverlay('genlM', { url: sUrl, zIndex: 3000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    else
        $('#tblGenlM').surfOverlay('genlM', { url: sUrl, zIndex: 2500, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideGeneralMedium(ArgsRet) {
    closepopup('genlM');

    var strURL = GsLaunchURL;
    GsLaunchURL = "";
    GsText = "";

   if (ArgsRet != null) {
        if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessGeneralMedium(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessGeneralMedium(ArgsRet);
        }
    }
}
function PopupGeneralBig() {
    var sUrl = "htmls/GeneralBig.html";
    $('#tblGenlBig').surfOverlay('genlBig', { url: sUrl, zIndex: 2500, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideGeneralBig(ArgsRet) {
    closepopup('genlBig');

    var strURL = GsLaunchURL;
    GsLaunchURL = "";

    
    //if (ArgsRet != null) {
    //    if (strURL.substring(0, strURL.indexOf(".")) == "AYWfrmChangePassword") {
    //        GsLogout = "Y";
    //        if (document.all)
    //            location.href = "VRSLogout.aspx?uid=" + objhdnUserID.value + "&pid=" + objhdnCompanyID.value + "&err=056";
    //        else
    //            window.location.assign("VRSLogout.aspx?uid=" + objhdnUserID.value + "&pid=" + objhdnCompanyID.value + "&err=056");
    //    }
    //    else if (typeof (objiframePage) != "undefined") {
    //        if (objiframePage.contentWindow)
    //            objiframePage.contentWindow.ProcessGridReturnValue(ArgsRet);
    //        else
    //            objiframePage.contentDocument.parentWindow.ProcessGridReturnValue(ArgsRet);
    //    }
    //    else
    //        ProcessGridReturnValue(ArgsRet);
    //}

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

    if(ArgsRet!=null) {
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
function PopupHelp() {
    var sUrl = "htmls/Help.html";
    $('#tblHelp').surfOverlay('help', { url: sUrl, zIndex: 2500, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideHelp(ArgsRet) {
    closepopup('help');

    var strURL = GsLaunchURL;
    GsLaunchURL = "";

    if (ArgsRet != null) {
       if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessHelp(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessHelp(ArgsRet);
        }
        else
           ProcessHelp(ArgsRet);
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
function PopupReportViewer() {
    var sUrl = "htmls/" + GsTheme + "/ReportViewer.html";
    $('#tblRptView').surfOverlay('rvw', { url: sUrl, zIndex:11000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideReportViewer() {
    closepopup('rvw');
    GsLaunchURL = "";
    try {

        if (Trim(GsFilePath) != "") {
            AjaxPro.timeoutPeriod = 1800000;
            VRSMain.DeleteFile(GsFilePath);
        }
    }
    catch (expErr) {
        //PopupMessage(strRootDirectory, strForm, "FetchMenu()", expErr.message, "true");
        ;
    }
   
}
function PopupEditor() {
    var sUrl = "htmls/Editor.html";
    $('#tblEditor').surfOverlay('editor', { url: sUrl, zIndex: 4500, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideEditor(ArgsRet) {
    closepopup('editor');

    var strURL = GsLaunchURL;
    GsLaunchURL = "";


    if (ArgsRet != null) {
        if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessEditor(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessEditor(ArgsRet);
        }
        else
            ProcessEditor(ArgsRet);
    }

}
function PopupMail() {
    var sUrl = "htmls/Mail.html";
    $('#tblMail').surfOverlay('ml', { url: sUrl, zIndex: 3500, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideMail() {
    closepopup('ml');
}
function PopupInvoiceCopy() {
    var sUrl = "htmls/InvoiceCopy.html";
    $('#tblInvCopy').surfOverlay('inv', { url: sUrl, zIndex: 2500, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideInvoiceCopy(ArgsRet) {
    closepopup('inv');

    var strURL = GsLaunchURL;
    GsLaunchURL = "";

    if (ArgsRet != null) {
        if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessInvoiceCopy(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessInvoiceCopy(ArgsRet);
        }
        else
            ProcessInvoiceCopy(ArgsRet);
    }
}
function PopupDataList() {
    var sUrl = "HTMLS/DataList.html";
    $('#tblDataList').surfOverlay('dtlst', { url: sUrl, zIndex: 10000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideDataList(ArgsRet) {
    closepopup('dtlst');
    GsLaunchURL = "";
    GiWidth = 0; GiHeight = 0; GiTop = 0;GiBuffHt = 0;

    if (ArgsRet != null) {
        if (typeof (objiframePage) != "undefined") {
            if (typeof (objiframeEdit) != "undefined") objiframeEdit = undefined;
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessDataList(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessDataList(ArgsRet);
        }

    }
    
}
function PopupMail() {
    var sUrl = "htmls/mail.html";
    $('#tblMail').surfOverlay('mail', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideMail() {
    closepopup('mail');
}
function PopupSMS() {
    var sUrl = "htmls/sms.html";
    $('#tblSMS').surfOverlay('sms', { url: sUrl, zIndex: 3000, imgLoading: false, gClickToClose: false, closeOnEsc: false });
    return false;
}
function HideSMS() {
    closepopup('sms');
}