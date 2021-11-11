var strRowID = "0"; var DEL_FLAG = ""; var WRITE_BACK = "N"; var ErrFlag = 0;
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    parent.adjustFrameHeight();
}));

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
    parent.adjustFrameHeight();
    SetPageValue();
}
function SetPageValue() {
    
   
    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();
    LoadStudyTypes();
    CallBackDoc.callback(objhdnID.value, UserID);
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('CaseList/VRSWorkListDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'CaseList/VRSWorkListDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    if(objhdnCF.value=="IP")
        parent.GsNavURL = "CaseList/VRSInProgressBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    else
        parent.GsNavURL = "CaseList/VRSWorkListBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;

    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function createGuid() {
    //return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function NavigateToPACS() {
    var URL = "";
    if (APIVER == "7.2") {
        URL = objhdnPACSURL.value;
        URL = URL.replace("#V1", objhdnSUID.value);
        URL = URL.replace("#V2", PACSUID);
        URL = URL.replace("#V3", PACSPwd);
    }
    else if (APIVER == "8") {
        URL = objhdnWS8SYVWRURL.value;
        if (document.all) {
            URL = URL.replace("#V1", objlblAccnNo.innerText);
            URL = URL.replace("#V2", objlblPatientID.innerText);
        }
        else {
            URL = URL.replace("#V1", objlblAccnNo.textContent);
            URL = URL.replace("#V2", objlblPatientID.textContent);
        }
       
        URL = URL.replace("#V3", WSSessionID);
        URL = URL.replace("#V4", WS8SRVUID);
        URL = URL.replace("#V5", objhdnWS8SRVPWD.value);
    }
    parent.NavigatePACS(URL);
}
function btnSubmit_OnClick(WriteBack, MergeStatus) {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrSTs = new Array();
    var arrDocs = new Array();
    var strFromDt = "";
    WRITE_BACK = WriteBack;

    if (parent.Trim(objtxtFromDt.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }


    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objtxtPID.value;
        ArrRecords[2] = objtxtPFName.value;
        ArrRecords[3] = objtxtPLName.value;
        ArrRecords[4] = objtxtPWt.value;
        ArrRecords[5] = strFromDt;
        ArrRecords[6] = objtxtAge.value;
        ArrRecords[7] = objddlSex.value;
        ArrRecords[8] = objddlSN.value;
        ArrRecords[9] = objddlSpecies.value;
        ArrRecords[10] = objddlBreed.value;
        ArrRecords[11] = objtxtOwnerFN.value;
        ArrRecords[12] = objtxtOwnerLN.value;
        ArrRecords[13] = objtxtAccnNo.value;
        ArrRecords[14] = objddlModality.value;
        ArrRecords[15] = objtxtReason.value;
        ArrRecords[16] = objtxtImgCnt.value;
        ArrRecords[17] = objtxtObjCnt.value;
        ArrRecords[18] = "N"; if (objrdoConfYes.checked) ArrRecords[18] = "Y";
        ArrRecords[19] = objddlInstitution.value;
        ArrRecords[20] = objddlPhys.value;
        ArrRecords[21] = WriteBack;
        ArrRecords[22] = objddlUOM.value;
        ArrRecords[23] = objddlPriority.value;
        ArrRecords[24] = MergeStatus;
        ArrRecords[25] = objtxtPhysNote.value;
        ArrRecords[26] = "N"; if (objrdoConsY.checked) ArrRecords[26] = "Y";
        ArrRecords[27] = objddlCategory.value;
        ArrRecords[28] = UserID;
        ArrRecords[29] = MenuID;

        arrSTs = GetStudyTypes();
        arrDocs = GetDocList();

        if (ErrFlag == 0) {
            AjaxPro.timeoutPeriod = 1800000;
            VRSWorkListDlg.SaveRecord(ArrRecords, arrSTs, arrDocs, ShowProcess);
        }


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSubmit_OnClick()", expErr.message, "true");
    }

}
function GetStudyTypes() {
    var itemIndex = 0; var gridItem;
    var arrST = new Array(); var idx = 0; var sel = ""; var validate = ""; var strName = ""; var validate_sel = "";
    var cnt = 0;
    if (grdSelST.recordNumber != null) rc = grdSelST.get_recordCount();
    else if (grdSelST.RecordCount != null) rc = grdSelST.get_recordCount();

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[2].toString();
        if (validate != "Y") {
            validate = gridItem.Data[4].toString();
            strName = gridItem.Data[3].toString();
            validate_sel = sel;
        }

        if (sel == "Y") {
            cnt = cnt + 1;
            arrST[idx] = gridItem.Data[1].toString();
            idx = idx + 1;
        }
        itemIndex++;
    }

    if (validate == "Y" && validate_sel == "Y" && cnt == 1) {
        ErrFlag = 1;
        arrST.length = 0;
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "GetStudyTypes()", "176", "true", strName);
    }

    return arrST;
}
function GetDocList() {
    var itemIndex = 0;
    var gridItem;
    var arrDoc = new Array(); var idx = 0;
    while (gridItem = grdDoc.get_table().getRow(itemIndex)) {
        arrDoc[idx] = gridItem.get_cells()[0].get_value().toString();
        arrDoc[idx + 1] = gridItem.get_cells()[1].get_value().toString();
        arrDoc[idx + 2] = gridItem.get_cells()[2].get_value().toString();
        arrDoc[idx + 3] = gridItem.get_cells()[3].get_value().toString();
        arrDoc[idx + 4] = gridItem.get_cells()[4].get_value().toString();
        idx = idx + 5;
        itemIndex++;
    }
    return arrDoc;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "094") {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
                strLoadPopup = "N";
                parent.GsRetStatus = "false";
                btnClose_OnClick();
            }
            else if (arrRes[1] == "165") {
                parent.GsLaunchURL = "CaseList/VRSMergeConfirm.aspx";
                strLoadPopup = "N";
                parent.PopupGeneralSmall();
            }
            else
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            if (WRITE_BACK == "Y") {
                if (arrRes[2] != "") {
                    parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false", arrRes[2]);
                }
                btnClose_OnClick();
            }
            else {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false", arrRes[2]);
                ddlModality_OnChange();
            }
            break;
    }
}
function ProcessGeneralSmall(Args) {
    if (Args != null) btnSubmit_OnClick(WRITE_BACK, Args);
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
            VRSWorkListDlg.DeleteStudy_72(ArrRecords, strURL, ShowProcess);
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
            VRSWorkListDlg.DeleteStudy_80(ArrRecords, ArrParams, ShowProcess);
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
        case "SaveRecord":
            SaveRecord(Result);
            break;
        
        case "DeleteStudy_72":
        case "DeleteStudy_80":
            ProcessDeleteStudy(Result);
            break;
    }

}

