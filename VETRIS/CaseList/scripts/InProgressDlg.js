var strRowID = "0"; var DEL_FLAG = ""; var WRITE_BACK = "N"; var RPT_RATING = "N"; var ErrFlag = 0; 
var LoggedRadiologistID = parent.objhdnRadiologistID.value;

//var timeout;
var CntInterval;

//window.addEventListener('blur', function (e) {
//    window.clearTimeout(timeout);
//});

//$(document).ready($(function () {
//    $("input:text").each(function () {
//        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
//    })
//    parent.adjustFrameHeight();

//    $("#btnView1").click(function (e) {

//        //timeout = window.setTimeout(function () {
            
//        //        console.log('timeout');
//        //        window.clearTimeout(CntInterval);
//        //        parent.HideMessage();
//        //        parent.PopupMessage(RootDirectory, strForm, "btnView1_OnClick()", "398", "false");
//        //        CntInterval = setTimeout(parent.HideMessage, 30000);
//        //        location.href = "DownloadDCMViewerInstaller/Setup.msi";
//        //}, 1000);

//        debugger;
//        location.href = getUrl();
        
//        parent.HideMessage();
//        parent.PopupMessage(RootDirectory, strForm, "btnView1_OnClick()", "396", "false");
//        CntInterval = setTimeout(parent.HideMessage, 30000);
//        e.preventDefault();
//    });
//    $("#btnView2").click(function (e) {

//        //timeout = window.setTimeout(function () {
//        //    console.log('timeout');
//        //    window.clearTimeout(CntInterval);
//        //    parent.HideMessage();
//        //    parent.PopupMessage(RootDirectory, strForm, "btnView2_OnClick()", "398", "false");
//        //    CntInterval = setTimeout(parent.HideMessage, 30000);
//        //    location.href = "DownloadDCMViewerInstaller/Setup.msi";
//        //}, 1000);

//        location.href = getUrl();
//        parent.HideMessage();
//        parent.PopupMessage(RootDirectory, strForm, "btnView2_OnClick()", "396", "false");
//        CntInterval = setTimeout(parent.HideMessage, 30000);
//        e.preventDefault();
//    });
//}));

//function getUrl() {
//    return "vetrisepacs://open?uid=" + objhdnSUID.value;
//}

function CheckError() {
    var arrErr = new Array();
    if (parent.Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                if (arrErr[1] == "094") {
                    parent.PopupMessage(RootDirectory, strForm, "CheckError()", arrErr[1], "true");
                    strLoadPopup = "N";
                    parent.GsRetStatus = "false";
                    btnClose_OnClick();
                }
                else
                    parent.PopupMessage(RootDirectory, strForm, "OnLoad()", arrErr[1], "true");
                break;
        }
    }
    objhdnError.value = "";
    //if (objhdnLockedUser.value != "") parent.PopupMessage(RootDirectory, strForm, "OnLoad()", "303", "false", objhdnLockedUser.value);
    parent.adjustFrameHeight();
    SetPageValue();
}
function SetPageValue() {
    var RadFnRights = objhdnRadFnRights.value;
    var recCnt = 0;
    if (objhdnLang.value == "en" || objhdnLang.value == "")
        objbtnTranslate.style.display = "none";
    else
        objbtnTranslate.style.display = "inline";

    if (objhdnShowTranslation.value == "Y") {
        $(divTranslate).css('display', 'block');
        ToggleTranslate();
    }

    if (UserRoleCode == "RDL") {
        if ((RadFnRights.indexOf("DICTRPT") == -1)) {
            document.getElementById("divStatDict").style.display = "none";
            document.getElementById("spnStatDict").style.display = "none";
        }
        if (RadFnRights.indexOf("UPDPRELIMRPT") == -1) {
            document.getElementById("divStatPrelim").style.display = "none";
            document.getElementById("spnStatPrelim").style.display = "none";
            //document.getElementById("divTeach").style.display = "block";
        }
        if (RadFnRights.indexOf("UPDFINALRPT") == -1) {
            document.getElementById("divStatFinal").style.display = "none";
            document.getElementById("spnStatFinal").style.display = "none";
        }
        if ((RadFnRights.indexOf("DICTRPT") == -1) && (RadFnRights.indexOf("UPDPRELIMRPT") == -1) && (RadFnRights.indexOf("UPDFINALRPT") == -1)) {
            document.getElementById("divStat").style.display = "none";
        }
        if ((RadFnRights.indexOf("RPTONRTEDTR") == -1)) {
            document.getElementById("rptEditor").style.display = "none";
            document.getElementById("rptTB").style.display = "block";
        }
        else {
            document.getElementById("rptEditor").style.display = "block";
            document.getElementById("rptTB").style.display = "none";
        }

        if ((RadFnRights.indexOf("VWLOCKSTUDY") > -1) || (RadFnRights.indexOf("ACCLOCKSTUDY") > -1)) {
            if (objhdnAssnRadID.value != "00000000-0000-0000-0000-000000000000") {
                if (objhdnAssnRadID.value != LoggedRadiologistID) {
                    if (RadFnRights.indexOf("ACCLOCKSTUDY") == -1) {
                        objbtnSave1.style.display = "none";
                        objbtnSave2.style.display = "none";
                    }
                }
            }
        }
        if (RadFnRights.indexOf("RATEOTHRPT") > -1) {
            if (LoggedRadiologistID != objhdnAssnRadID.value) {
                if (objhdnAssnRadID.value != "00000000-0000-0000-0000-000000000000") {
                    RPT_RATING = "Y";
                    document.getElementById("divRateRpt").style.display = "block";
                }
                else
                    objrdoNormal.checked = true;
            }
            else
                objrdoNormal.checked = true;
        }
        else
            objrdoNormal.checked = true;
    }
    else if (UserRoleCode == "TRS") {
        document.getElementById("divStat").style.display = "none";
        document.getElementById("divRateRpt").style.display = "none";
        objbtnSave1.style.display = "none";
        objbtnSave2.style.display = "none";
        objbtnSaveTrans1.style.display = "inline";
        objbtnSaveTrans2.style.display = "inline";
    }
    else {
        objbtnSave1.style.display = "none";
        objbtnSave2.style.display = "none";
        objbtnSaveTrans1.style.display = "none";
        objbtnSaveTrans2.style.display = "none";
        document.getElementById("rptEditor").style.display = "block";
        //document.getElementById("divTeach").style.display = "block";
    }

    if (objhdnCurrStatusID.value == "60") {
        objrdoDict.disabled = true;
        objrdoDict.checked = true;
    }

    Rating_OnClick();

    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();

    LoadStudyTypes();
    CallBackDoc.callback(objhdnID.value, UserID);
    CallBackPS.callback(objhdnID.value, objhdnPName.value, objhdnInstID.value, UserID);

    if (objhdnSyncMode.value == "PACS") GetImageCountAPI();
}

function GetImageCountAPI() {
    var arrRes = new Array();
    var strSUID = ""; var strURL = "";
    var ArrParams = new Array();
    parent.GsPopupText = "Checking PACS Img# / Obj#";
    parent.PopupProcess("N");

    try {

       
            ArrParams[0] = objhdnID.value;
            ArrParams[1] = objhdnSUID.value;
            ArrParams[2] = objhdnWS8SRVIP.value;
            ArrParams[3] = objhdnWS8CLTIP.value;
            ArrParams[4] = objhdnWS8SRVUID.value;
            ArrParams[5] = objhdnWS8SRVPWD.value;
            ArrParams[6] = UserID;
            ArrParams[7] = MenuID;
            ArrParams[8] = SessionID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSInProgressDlg.GetImageCountAPI_80(ArrParams, ShowProcess);
        

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "GetImageCountAPI()", expErr.message, "true");
        parent.GsPopupText = "";
    }

}
function GetImageCount(Result) {

    var arrRes = new Array();
    var Modality = "";
    parent.GsPopupText = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            if (arrRes.length == 2) {
                parent.PopupMessage(RootDirectory, strForm, "GetImageCount()", arrRes[1], "true");
                if (arrRes[1] == "094") {
                    parent.GsRetStatus = "false";
                    btnClose_OnClick();
                }
            }
            else if (arrRes.length == 4) {
                objtxtImgCnt.value = arrRes[1];
                objtxtObjCnt.value = arrRes[2];
                if (arrRes[4] == "094") {
                    parent.GsRetStatus = "false";
                    btnClose_OnClick();
                }
            }
            break;
        case "true":
            if (objhdnTrackBy == "I") {
                if (document.all) {
                    objlblCnt.innerText = arrRes[1];
                }
                else {
                    objlblCnt.textContent = arrRes[1];
                }
            }
            else if (objhdnTrackBy == "O") {
                if (document.all) {
                    objlblCnt.innerText = arrRes[2];
                }
                else {
                    objlblCnt.textContent = arrRes[2];
                }
            }

            break;
    }

}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('CaseList/VRSInProgressDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'CaseList/VRSInProgressDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "CaseList/VRSInProgressBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false") {
        if (WRITE_BACK == "Y") parent.FetchMenuRecordCount();

        btnDlgClose_Onclick();
    }
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function btnDownload_OnClick() {
    parent.GsLaunchURL = "CaseList/VRSDownloadImg.aspx?id=" + objhdnID.value + "&mid=" + MenuID + "&uid=" + UserID + "&arch=N&th=" + selTheme;
    parent.PopupGeneralMedium();
}
function createGuid() {
    //return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function ShowDocument(ID, FileName) {
    var strFilePath = "/" + RootDirectory + "/CaseList/Temp/" + UserID + "/" + FileName;
    var win = window.open(strFilePath, '_blank');
    win.focus();
}
function NavigateToPACS() {
    var URL = objhdnVRSAPPLINK.value + "/CaseList/DownloadDCMViewerInstaller/Setup.msi";
    //if (APIVER == "7.2") {
    //    URL = objhdnPACSURL.value;
    //    URL = URL.replace("#V1", objhdnSUID.value);
    //    URL = URL.replace("#V2", PACSUID);
    //    URL = URL.replace("#V3", PACSPwd);
    //}
    //else if (APIVER == "8") {
    //    URL = objhdnWS8IMGVWRURL.value;
    //    if (document.all) {
    //        URL = URL.replace("#V1", objlblAccnNo.innerText);
    //        URL = URL.replace("#V2", objlblPatientID.innerText);
    //    }
    //    else {
    //        URL = URL.replace("#V1", objlblAccnNo.textContent);
    //        URL = URL.replace("#V2", objlblPatientID.textContent);
    //    }

    //    URL = URL.replace("#V3", WSSessionID);
    //    URL = URL.replace("#V4", WS8SRVUID);
    //    URL = URL.replace("#V5", WS8SRVPWD);
    //}
    //parent.NavigatePACS(URL);
    //var win = window.open("vetrisepacs://open?uid=" + objhdnSUID.value, '_blank');
    //win.focus();

    try
    {
        location.href = "vetrisepacs://open?uid=" + objhdnSUID.value + "&c=" + objhdnPACSCred.value;
        //parent.PopupMessage(RootDirectory, strForm, "NavigateToPACS()", "396", "false", URL);
        //CntInterval = setTimeout(parent.HideMessage, 30000);
    }
    catch (expErr) {
        //window.clearTimeout(CntInterval);
        parent.HideMessage();
        parent.PopupMessage(RootDirectory, strForm, "NavigateToPACS()", expErr.message, "true");
       
    }

   
}
function Rating_OnClick() {
    if (objrdoNormal.checked) {
        objddlAbRptReason.value = "00000000-0000-0000-0000-000000000000";
        objddlAbRptReason.disabled = true;
    }
    else if (objrdoAbnormal.checked)
        objddlAbRptReason.disabled = false;
}
function ddlDisclReason_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlDisclReason.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSInProgressDlg.FetchTypeWiseReportDisclaimerReasonDescription(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "ddlDisclReason_OnChange()", expErr.message, "true");
    }
}
function PopulateReportDisclaimerReasonDescription(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "PopulateReportDisclaimerReasonDescription()", arrRes[1], "true");
            break;
        case "true":
            //var objDisclDesc = document.getElementById("cke_contents_rtbDisclText").children[0];
            //objdivRptDisc.innerHTML = arrRes[1];
            //objDisclDesc.contentDocument.body.innerHTML = arrRes[1];
            objtxtRptDisclText.value = arrRes[1];
            break;
    }

}

function btnTranslate_OnClick() {
    parent.PopupProcess("N");
    var text = CKEDITOR.instances.editorFindings.getData();

    try {
        AjaxPro.timeoutPeriod = 1800000;
        VRSInProgressDlg.doTranslation(text,objhdnGOOGLETRANSAPILINK.value,objhdnGOOGLETRANSAPIKEY.value, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnTranslate_OnClick()", expErr.message, "true");
    }
}
function setTranslationData(Result) {
    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "setTranslationData()", arrRes[1], "true");
            break;
        case "true":
            $(divTranslate).css('display', 'block')
            ToggleTranslate();
            if (arrRes.length > 1)
                CKEDITOR.instances.editorFindingsTranslated.setData(arrRes[1]);
            else
                CKEDITOR.instances.editorFindingsTranslated.setData("");
            parent.adjustFrameHeight();
            break;
    }
}
function ToggleTranslate() {
    var e = $('#aRTransCollapse').closest(".searchSection"),
                       t = $('#aRTransCollapse').find("i"),
                       n = e.find("#divTransRpt");
    if ($('#aRTransCollapse>i').attr('class') == "fa fa-chevron-down") {
        e.attr("style") ? n.slideToggle(200, function () {
            e.removeAttr("style")
        }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
    }
    document.getElementById('aRTransCollapse').style.display = "none";
}

function btnEditST_OnClick() {
    parent.GiWidth = 100;
    parent.GiTop = 30;
    var StudyID = objhdnID.value;
    var HeadID  = objhdnModalityID.value;
    parent.GsLaunchURL = "Invoicing/VRSStudyAmendStudyTypes.aspx?id=" + StudyID + "&mod=" + HeadID + "&th="+ selTheme;
    parent.PopupDataList();
}
function ProcessDataList(Args) {

    LoadStudyTypes();
}

function btnSave_OnClick() {
   
    var ArrRecords = new Array();
    var objFindings = document.getElementById("cke_contents_editorFindings").children[0];
    var objFindingsTranslated = document.getElementById("cke_contents_editorFindingsTranslated").children[0];
    var status = objhdnCurrStatusID.value; if (objrdoDict.checked) status = "60"; else if (objrdoPrelim.checked) status = "80"; else if (objrdoFinal.checked) status = "100";

    try {
        

        if (objhdnInvBy.value == "B") {
            if (SAVE_FLAG == "N" || SAVE_FLAG == "X") {
                var recCnt = grdSelST.get_recordCount();
                parent.GsDlgConfAction = "SAV";
                parent.PopupConfirm("464", recCnt.toString());
            }
        }
        else {
            SAVE_FLAG = "Y";
        }

        if(SAVE_FLAG=="Y")
        {
            parent.PopupProcess("Y");
            ArrRecords[0] = objhdnRptID.value;
            ArrRecords[1] = objhdnID.value;
            ArrRecords[2] = objtxtReport.value;
            if (parseInt(status) > 60) {
                if (objhdnLang.value == "en")
                    ArrRecords[3] = objFindings.contentDocument.body.innerHTML;
                else
                    ArrRecords[3] = objFindingsTranslated.contentDocument.body.innerHTML;
            }
            else
                ArrRecords[3] = objFindings.contentDocument.body.innerHTML;

            ArrRecords[4] = ""; if (objrdoAbnormal.checked) ArrRecords[4] = "A"; else if (objrdoNormal.checked) ArrRecords[4] = "N";
            ArrRecords[5] = status;
            ArrRecords[6] = RPT_RATING;
            ArrRecords[7] = objddlDisclReason.value;
            ArrRecords[8] = objddlAbRptReason.value;
            ArrRecords[9] = "N"; if (objchkTeach.checked) ArrRecords[9] = "Y";
            ArrRecords[10] = objtxtRptDisclText.value;
            ArrRecords[11] = UserID;
            ArrRecords[12] = MenuID;
            ArrRecords[13] = SessionID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSInProgressDlg.SaveReport(ArrRecords, ShowProcess);
        }

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }

}
function ProcessSave(ArgsRet) {
    if (ArgsRet == "Y") {
        SAVE_FLAG = "Y";
        btnSave_OnClick();
    }
    else if (ArgsRet == "N") {
        SAVE_FLAG = "X";
        parent.GiWidth = 100;
        parent.GiTop = 30;
        var StudyID = objhdnID.value;
        var HeadID = objhdnModalityID.value;
        parent.GsLaunchURL = "Invoicing/VRSStudyAmendStudyTypes.aspx?id=" + StudyID + "&mod=" + HeadID;
        parent.PopupDataList();
    }
}
function SaveReport(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "296" || arrRes[1] == "297") {
                parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true", arrRes[2]);
                parent.GsRetStatus = "false";
                btnClose_OnClick();
            }
            else
                parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "false");
            objhdnRptID.value = arrRes[2];
            btnClose_OnClick();
            break;
    }
}
function btnSaveTrans_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var objFindings = document.getElementById("cke_contents_editorFindings").children[0];
    var objFindingsTranslated = document.getElementById("cke_contents_editorFindingsTranslated").children[0];

    try {

        ArrRecords[0] = objhdnRptID.value;
        ArrRecords[1] = objhdnID.value;
        ArrRecords[2] = objhdnLang.value;
        if (objhdnLang.value == "en") ArrRecords[3] = objFindings.contentDocument.body.innerHTML; else ArrRecords[3] = objFindingsTranslated.contentDocument.body.innerHTML;
        ArrRecords[4] = objFindingsTranslated.contentDocument.body.innerHTML;
        ArrRecords[5] = objhdnGOOGLETRANSAPILINK.value;
        ArrRecords[6] = objhdnGOOGLETRANSAPIKEY.value;
        ArrRecords[7] = UserID;
        ArrRecords[8] = MenuID;
        ArrRecords[9] = SessionID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSInProgressDlg.SaveTranscription(ArrRecords, ShowProcess);

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }

}
function SaveTranscription(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveTranscription()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveTranscription()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "SaveTranscription()", arrRes[1], "false");
            objhdnRptID.value = arrRes[2];
            btnClose_OnClick();
            break;
    }
}
function LoadStudyTypes() {
    CallBackSelST.callback(objhdnID.value, objhdnModalityID.value);
}

/********Delete Study*****************************/
function btnDel_OnClick() {
    DEL_FLAG = 'STUDY';
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("123");
}
function DeleteRecord() {
    DeleteStudy();
}
function DeleteStudy() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    var ArrParams = new Array();
    var strSUID = ""; var strURL = "";

    try {

        if (objhdnAPIVER.value == "7.2") {
            ArrRecords[0] = objhdnID.value;
            ArrRecords[1] = objhdnSUID.value;
            ArrRecords[2] = objhdnRecByRouter.value;
            ArrRecords[3] = UserID;
            ArrRecords[4] = MenuID;

            strSUID = objhdnSUID.value;
            strURL = objhdnStudyDelUrl.value + strSUID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSInProgressDlg.DeleteStudy_72(ArrRecords, strURL, ShowProcess);
        }
        else if (objhdnAPIVER.value == "8") {
            ArrRecords[0] = objhdnID.value;
            ArrRecords[1] = objhdnSUID.value;
            ArrRecords[2] = objhdnRecByRouter.value;
            ArrRecords[3] = UserID;
            ArrRecords[4] = MenuID;

            ArrParams[0] = objhdnWS8SRVIP.value;
            ArrParams[1] = objhdnWS8CLTIP.value;
            ArrParams[2] = objhdnWS8SRVUID.value;
            ArrParams[3] = objhdnWS8SRVPWD.value;

            AjaxPro.timeoutPeriod = 1800000;
            VRSInProgressDlg.DeleteStudy_80(ArrRecords, ArrParams, ShowProcess);
        }
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "DeleteStudy()", expErr.message, "true");
    }
}
function ProcessDeleteStudy(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteStudy()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            btnClose_OnClick();
            break;
    }
}
/********Delete Study*****************************/

function btnRpt_OnClick(ID, PatientName,StatusID) {
    var CustomRpt = objhdnCustomRpt.value;
    parent.GsFileType = "PDF";
    if (StatusID == "80") {
        if (CustomRpt == "N")
            parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=2&ID=" + ID + "&PNM=" + PatientName + "&UID=" + UserID;
        else
            parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=4&ID=" + ID + "&PNM=" + PatientName + "&UID=" + UserID;
    }
    else {
        if (CustomRpt == "N")
            parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=1&ID=" + ID + "&PNM=" + PatientName + "&UID=" + UserID;
        else
            parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=3&ID=" + ID + "&PNM=" + PatientName + "&UID=" + UserID;
    }
    parent.PopupReportViewer();
}
function chkSelPS_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "";
    while (gridItem = grdPS.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        if (RowId == ID) {
            if (document.getElementById("chkSelPS_" + RowId).checked) gridItem.Data[14] = "Y";
            else gridItem.Data[14] = "N";
            break;
        }
        itemIndex++;
    }
}
function btnImg_OnClick() {
    var SUID = "";
    var itemIndex = 0; var gridItem; var RowId = "";
    try {

        if (objchkIncStudy.checked) SUID = objhdnSUID.value;
        while (gridItem = grdPS.get_table().getRow(itemIndex)) {
            RowId = gridItem.get_cells()[0].get_value().toString();
            if (gridItem.Data[14] == "Y") {
                if (SUID != "") SUID = SUID + ';';
                SUID = SUID + parent.Trim(gridItem.Data[1]);
            }

            itemIndex++;
        }
        if (parent.Trim(SUID) == "") {
            parent.PopupMessage(RootDirectory, strForm, "btnImg_OnClick()", "434", "true");
        }
        else {
            location.href = "vetrisepacs://open?uid=" + SUID + "&c=" + objhdnPACSCred.value;
        }

    }
    catch (expErr) {

        parent.HideMessage();
        parent.PopupMessage(RootDirectory, strForm, "btnImg_OnClick()", expErr.message, "true");

    }
}
function btnDLImg_OnClick() {
    var itemIndex = 0; var gridItem; var RowId = "";
    var i = 0;
    try {
        while (gridItem = grdPS.get_table().getRow(itemIndex)) {
            RowId = gridItem.get_cells()[0].get_value().toString();
            if (gridItem.Data[14] == "Y") {
                parent.GsStoredValue[i] = parent.Trim(gridItem.Data[1]);
                parent.GsStoredValue[i + 1] = parent.Trim(gridItem.Data[10]);
                i = i + 2;
            }
            
            itemIndex++;
        }
        if (objchkIncStudy.checked) {
            parent.GsStoredValue[i] = objhdnSUID.value;
            parent.GsStoredValue[i + 1] = objhdnPhysCode.value;
        }

        if (parent.GsStoredValue.length ==0) {
            parent.PopupMessage(RootDirectory, strForm, "btnDLImg_OnClick()", "486", "true");
        }
        else {
            parent.GsLaunchURL = "CaseList/VRSDownloadMultipleStudyImg.aspx?id=" + objhdnID.value + "&mid=" + MenuID + "&uid=" + UserID + "&arch=N&th="+selTheme;
            parent.PopupGeneralMedium();
        }

    }
    catch (expErr) {

        parent.HideMessage();
        parent.PopupMessage(RootDirectory, strForm, "btnImg_OnClick()", expErr.message, "true");

    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveReport":
            SaveReport(Result);
            break;
        case "SaveTranscription":
            SaveTranscription(Result);
            break;
        case "DeleteStudy_72":
        case "DeleteStudy_80":
            ProcessDeleteStudy(Result);
            break;
        case "doTranslation":
            setTranslationData(Result);
            break;
        case "FetchTypeWiseReportDisclaimerReasonDescription":
            PopulateReportDisclaimerReasonDescription(Result);
            break;
        case "GetImageCountAPI_80":
            GetImageCount(Result);
            break;
    }

}


