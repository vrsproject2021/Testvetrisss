var strRowID = "0"; var DEL_FLAG = ""; var ErrFlag = 0; var LoggedRadiologistID = parent.objhdnRadiologistID.value;

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

//        timeout = window.setTimeout(function () {
//            console.log('timeout');
//            window.clearTimeout(CntInterval);
//            parent.HideMessage();
//            parent.PopupMessage(RootDirectory, strForm, "btnView1_OnClick()", "398", "false");
//            CntInterval = setTimeout(parent.HideMessage, 30000);
//            location.href = "DownloadDCMViewerInstaller/Setup.msi";
//        }, 1000);

//        location.href = getUrl();

//        parent.HideMessage();
//        parent.PopupMessage(RootDirectory, strForm, "btnView1_OnClick()", "396", "false");
//        CntInterval = setTimeout(parent.HideMessage, 30000);
//        e.preventDefault();
//    });
//    $("#btnView2").click(function (e) {

//        timeout = window.setTimeout(function () {
//            console.log('timeout');
//            window.clearTimeout(CntInterval);
//            parent.HideMessage();
//            parent.PopupMessage(RootDirectory, strForm, "btnView2_OnClick()", "398", "false");
//            CntInterval = setTimeout(parent.HideMessage, 30000);
//            location.href = "DownloadDCMViewerInstaller/Setup.msi";
//        }, 1000);

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

    SetPageValue();
    parent.adjustFrameHeight();
}
function SetPageValue() {
    var RadFnRights = objhdnRadFnRights.value;
    if (UserRoleCode == "RDL") {
        if (RadFnRights.indexOf("ACCLOCKSTUDY") > -1) {

            if (RadFnRights.indexOf("UPDFINALRPT") == -1) {
                objbtnSave1.style.display = "none";
                objbtnSave2.style.display = "none";
            }
        }
        else if (RadFnRights.indexOf("VWLOCKSTUDY") > -1) {
            if (RadFnRights.indexOf("UPDFINALRPT") == -1) {
                objbtnSave1.style.display = "none";
                objbtnSave2.style.display = "none";
            }
            if (objhdnAssnRadID.value != LoggedRadiologistID) {
                objbtnSave1.style.display = "none";
                objbtnSave2.style.display = "none";
            }

        }

        if (RadFnRights.indexOf("UPDFINALRPT") == -1) {
            objbtnSave1.style.display = "none";
            objbtnSave2.style.display = "none";
        }
        if (RadFnRights.indexOf("UNFINALRPT") > -1) {
            objdivFindings.style.display = "none";
            objdivAddendum.style.display = "none";
            if ((RadFnRights.indexOf("RPTONRTEDTR") == -1)) {
                document.getElementById("rptEditor").style.display = "none";
                document.getElementById("rptTB").style.display = "block";
                document.getElementById("addnEditor").style.display = "none";
                document.getElementById("addnTB").style.display = "block";
            }
            else {
                document.getElementById("rptEditor").style.display = "block";
                document.getElementById("rptTB").style.display = "none";
                document.getElementById("addnEditor").style.display = "block";
                document.getElementById("addnTB").style.display = "none";
            }
        }
    }
    else {
        objbtnSave1.style.display = "none";
        objbtnSave2.style.display = "none";
    }

    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();
    LoadStudyTypes();
    CallBackDoc.callback(objhdnID.value, UserID);
    CallBackAddn.callback(objhdnID.value);

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
        VRSCaseArchivelDlg.GetImageCountAPI_80(ArrParams, ShowProcess);


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
        btnBrwEditUI_Onclick('CaseList/VRSCaseArchivelDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'CaseList/VRSCaseArchivelDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "CaseList/VRSCaseFinalRptBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false") {
        parent.FetchMenuRecordCount();
        btnDlgClose_Onclick();
    }
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function btnDownload_OnClick() {
    parent.GsLaunchURL = "CaseList/VRSDownloadImg.aspx?id=" + objhdnID.value + "&mid=" + MenuID + "&uid=" + UserID + "&arch=Y";
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
    //var URL = objhdnVRSAPPLINK.value + "/CaseList/DownloadDCMViewerInstaller/Setup.msi";
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

    try {
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
function ddlDisclReason_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlDisclReason.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseArchivelDlg.FetchTypeWiseReportDisclaimerReasonDescription(ArrRecords, ShowProcess);
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

function btnEditST_OnClick() {
    parent.GiWidth = 100;
    parent.GiTop = 30;
    var StudyID = objhdnID.value;
    var HeadID = objhdnModalityID.value;
    parent.GsLaunchURL = "Invoicing/VRSStudyAmendStudyTypes.aspx?id=" + StudyID + "&mod=" + HeadID;
    parent.PopupDataList();
}
function ProcessDataList(Args) {

    LoadStudyTypes();
}

function btnSave_OnClick() {
   
    var ArrRecords = new Array();
    var objAddendum = document.getElementById("cke_contents_editorAddendum").children[0];

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

        if (SAVE_FLAG == "Y") {
            parent.PopupProcess("Y");
            ArrRecords[0] = objhdnID.value;
            ArrRecords[1] = objtxtReport.value;
            ArrRecords[2] = CKEDITOR.instances.editorFindings.getData();
            ArrRecords[3] = objtxtAddendum.value;
            ArrRecords[4] = objAddendum.contentDocument.body.innerHTML;
            ArrRecords[5] = objddlDisclReason.value;
            ArrRecords[6] = "N"; if (objchkTeach.checked) ArrRecords[6] = "Y";
            ArrRecords[7] = objtxtRptDisclText.value;
            ArrRecords[8] = UserID;
            ArrRecords[9] = MenuID;
            ArrRecords[10] = SessionID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSCaseArchivelDlg.SaveReportAddendum(ArrRecords, ShowProcess);
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
function SaveReportAddendum(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "297") {
                parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true");
                parent.GsRetStatus = "false";
                btnClose_OnClick();
            }
            else
                parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "false");
            CallBackAddn.callback(objhdnID.value);
            var objAddendum = document.getElementById("cke_contents_editorAddendum").children[0];
            objAddendum.contentDocument.body.innerHTML = "";
            break;
    }
}
function LoadStudyTypes() {
    CallBackSelST.callback(objhdnID.value, objhdnModalityID.value);
}
function btnViewAddn_OnClick(SerialNo) {
    parent.GiWidth = 400;
    parent.GiTop = 30;
    parent.GsLaunchURL = "CaseList/VRSAddendumView.aspx?id=" + objhdnID.value + "&srl=" + SerialNo + "&th=" + selTheme;
    parent.PopupDataList();
}
/********Delete Study*****************************/
function btnDel_OnClick() {
    DEL_FLAG = 'STUDY';
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("123");
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
            VRSCaseArchivelDlg.DeleteStudy_72(ArrRecords, strURL, ShowProcess);
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
            VRSCaseArchivelDlg.DeleteStudy_80(ArrRecords, ArrParams, ShowProcess);
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

function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveReportAddendum":
            SaveReportAddendum(Result);
            break;
        case "DeleteStudy_72":
        case "DeleteStudy_80":
            ProcessDeleteStudy(Result);
            break;
        case "FetchTypeWiseReportDisclaimerReasonDescription":
            PopulateReportDisclaimerReasonDescription(Result);
            break;
        case "GetImageCountAPI_80":
            GetImageCount(Result);
            break;
    }

}

