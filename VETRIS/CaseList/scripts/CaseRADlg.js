var strRowID = "0"; var DEL_FLAG = ""; var WRITE_BACK = "N"; var ErrFlag = 0; var CategoryID = "0"; var IMG_COUNT_CONF = 0;
var imgCntInterval;
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    objtxtPFName.focus();
    //imgCntInterval = setTimeout(btnRefreshCount_OnClick, 5000);
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
    
    SetPageValue();
}
function SetPageValue() {
    if (UserRoleID == "1") document.getElementById("divStudyUID").style.display = "inline";
    if (document.all) {
        if (parent.Trim(objlblPatientID.innerText) == "") objtxtPID.readOnly = "";
        if (parent.Trim(objlblAccnNo.innerText) == "") objtxtAccnNo.readOnly = "";
    }
    else {
        if (parent.Trim(objlblPatientID.textContent) == "") objtxtPID.readOnly = "";
        if (parent.Trim(objlblAccnNo.textContent) == "") objtxtAccnNo.readOnly = "";
    }
    CategoryID = objddlCategory.value;
    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();
    //objiframeUpload.src = "VRSUploadFiles.aspx?dir=" + strDir + "&t=IMG&fileID=" + usrID + "_" + tmpID;
    objiframeUpload.src = "VRSUploadFiles.aspx?dir=" + strDir + "&fileID=" + usrID + "_" + tmpID;
    objiframeUploadDCM.src = "VRSUploadDCMFiles.aspx?uid=" + UserID;
    LoadStudyTypes();
    //if ((UserRoleID != "1") && (UserRoleID != "2")) {
    //if(objddlInstitution.length == 2) objddlInstitution.disabled = true;
    //}
    if (objhdnInstConsAppl.value == "Y") document.getElementById("divCons").style.display = "inline";
    CallBackDoc.callback("L", objhdnID.value, UserID);
    CallBackDCM.callback("L", objhdnID.value, UserID);
    btnRefreshCount_OnClick();
   
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('CaseList/VRSCaseRADlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'CaseList/VRSCaseRADlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "CaseList/VRSCaseRABrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}

function NavigateToImgViewer() {
    var URL = objhdnImgVwrURL.value;
    URL = URL.replace("#V1", objhdnSUID.value);
    URL = URL.replace("#V2", PACSUID);
    URL = URL.replace("#V3", PACSPwd);
    parent.NavigatePACS(URL);
}
function NavigateToPACS() {
    var URL = objhdnPACSURL.value;
    URL = URL.replace("#V1", objhdnSUID.value);
    URL = URL.replace("#V2", PACSUID);
    URL = URL.replace("#V3", PACSPwd);

    parent.NavigatePACS(URL);
}

function btnSubmit_OnClick(WriteBack, MergeStatus) {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrSTs = new Array();
    var arrDocs = new Array();
    var arrDCM = new Array();
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
        ArrRecords[28] = objhdnSUID.value;
        ArrRecords[29] = objhdnFTPABSPATH.value;
        ArrRecords[30] = objhdnTempDCMFolder.value;
        ArrRecords[31] = objhdnDCMMODIFYEXEPATH.value;
        ArrRecords[32] = objddlInstitution.options[objddlInstitution.selectedIndex].text;
        ArrRecords[33] = UserID;
        ArrRecords[34] = MenuID;

        arrSTs = GetStudyTypes();
        arrDocs = GetDocList();
        arrDCM = GetDCMList();

        if (ErrFlag == 0) {
            if (objddlInstitution.value != "00000000-0000-0000-0000-000000000000") {
                AjaxPro.timeoutPeriod = 1800000;
                VRSCaseRADlg.SaveRecord(ArrRecords, arrSTs, arrDocs, arrDCM, ShowProcess);
            }
            else {
                parent.HideProcess();
                parent.PopupMessage(RootDirectory, strForm, "SubmitStudy()", "055", "true");
            }
        }


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "SubmitStudy()", expErr.message, "true");
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
function GetDCMList() {
    var itemIndex = 0;
    var gridItem;
    var arrDCM = new Array(); var idx = 0;
    while (gridItem = grdDCM.get_table().getRow(itemIndex)) {
        arrDCM[idx] = gridItem.get_cells()[0].get_value().toString();
        arrDCM[idx + 1] = gridItem.get_cells()[1].get_value().toString();
        arrDCM[idx + 2] = gridItem.get_cells()[2].get_value().toString();
        idx = idx + 3;
        itemIndex++;
    }
    return arrDCM;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    parent.GsPopupText = "";
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "094") {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
                parent.GsRetStatus = "false";
                btnClose_OnClick();
            }
            else if (arrRes[1] == "165") {
                parent.GsLaunchURL = "CaseList/VRSMergeConfirm.aspx";
                strLoadPopup = "N";
                parent.PopupGeneralSmall();
            }
            else if (arrRes[1] == "331") {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2], arrRes[3]);
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
    if (Args != null) 
        btnSubmit_OnClick(WRITE_BACK,Args);
}

function txtFromDt_OnBlur() {
    var strFromDt = "";
    var strDtToday = "";
    var dtFrom = new Date();
    var dtToday = new Date();
    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");

    if (parent.Trim(objtxtFromDt.value) != "") {
        if (ValidateDateEntered(objtxtFromDt)) {

            if (document.all) strFromDt = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
            else strFromDt = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);

            strDtToday = dtToday.getDate().toString() + monthName[dtToday.getMonth()] + dtToday.getFullYear();

            dtFrom = new Date(strFromDt);
            dtToday = new Date(strDtToday);

            if (dtFrom < dtToday) {
                CalculateAge();
            }
            else {
                parent.PopupMessage(RootDirectory, strForm, "txtFromDt_OnBlur()", "338", "true");
            }
        }
    }
    else
        CalculateAge();
}
function CalculateAge() {

    var strDtFrom = ""; var strDtTill = "";
    var dtFrom = new Date(); var dtTill = new Date();
    var diff = 0; var ageYrs = 0; var ageMth = 0; var age = "";
    if (parent.Trim(objtxtFromDt.value) != "") {
        if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
        else {
            if (document.all)
                strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
            else
                strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        }
        if (parent.Trim(objtxtStudyDt.value) == "") strDtTill = "01Jan1900";
        else {
            if (document.all)
                strDtTill = parent.SetDateFormat(objtxtStudyDt.value, parent.GsDateFormat, parent.GsDateSep);
            else
                strDtTill = parent.SetDateFormat1(objtxtStudyDt.value, parent.GsDateFormat, parent.GsDateSep);
        }
        dtFrom = new Date(strDtFrom);
        dtTill = new Date(strDtTill);
        diff = dateDiffInDays(dtFrom, dtTill);

        //ageYrs = (diff / 365).toFixed(0);
        //ageMth = (diff / 12).toFixed(0) - ;
        if (diff <= 0) {
            age = "0 year 0 month";
        }
        else {
            age = getAge(dtFrom, dtTill)
        }
        objtxtAge.value = age;
        //objtxtAge.value = ageYrs.toString() + "Years " + ageMth.toString() + " Months";
    }
    else {
        objtxtAge.value = "0 year 0 month";
    }
}

function dateDiffInDays(dt1, dt2) {
    var _MS_PER_DAY = 1000 * 60 * 60 * 24;
    // Discard the time and time-zone information.
    var utc1 = Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate());
    var utc2 = Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate());

    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}
function getAge(dtFrom, dtTill) {
    var today = new Date(dtTill);
    var DOB = new Date(dtFrom);
    var totalMonths = (today.getFullYear() - DOB.getFullYear()) * 12 + (today.getMonth() + 1) - (DOB.getMonth() + 1);
    totalMonths += today.getDay() < DOB.getDay() ? -1 : 0;
    var years = today.getFullYear() - DOB.getFullYear();
    if (DOB.getMonth() > today.getMonth())
        years = years - 1;
    else if (DOB.getMonth() === today.getMonth())
        if (DOB.getDate() > today.getDate())
            years = years - 1;

    var days;
    var months;

    if (DOB.getDate() > today.getDate()) {
        months = (totalMonths % 12);
        if (months == 0)
            months = 11;
        var x = today.getMonth() + 1;
        switch (x) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12: {
                var a = DOB.getDate() - today.getDate();
                days = 31 - a;
                break;
            }
            default: {
                var a = DOB.getDate() - today.getDate();
                days = 30 - a;
                break;
            }
        }

    }
    else {
        days = today.getDate() - DOB.getDate();
        if (DOB.getMonth() === today.getMonth())
            months = (totalMonths % 12);
        else
            months = (totalMonths % 12) + 1;
    }
    if ((parseInt(months) == 0) && (parseInt(days) > 0)) months = 1;
    //var age = years + ' years ' + months + ' months ' + days + ' days';
    var age = years + ' years ' + months + ' months ';
    return age;
}

function ddlSpecies_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlSpecies.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseRADlg.FetchBreeds(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "ddlSpecies_OnChange()", expErr.message, "true");
    }
}
function PopulateBreed(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "PopulateBreed()", arrRes[1], "true");
            break;
        case "true":
            objddlBreed.length = 0;
            for (var i = 1; i < arrRes.length; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlBreed.add(op);
            }
            break;
    }

}
function ddlInstitution_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlInstitution.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseRADlg.FetchPhysicians(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "ddlInstitution_OnChange()", expErr.message, "true");
    }
}
function PopulatePhysicians(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "PopulatePhysicians()", arrRes[1], "true");
            break;
        case "true":
            objddlPhys.length = 0;
            for (var i = 1; i < (arrRes.length - 1) ; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlPhys.add(op);
            }
            objhdnInstConsAppl.value = arrRes[arrRes.length - 1];
            if (objhdnInstConsAppl.value == "Y") document.getElementById("divCons").style.display = "inline";
            else document.getElementById("divCons").style.display = "none";
            break;
    }

}

/********Study Types*************************/
function LoadStudyTypes() {
    CallBackST.callback(objhdnID.value, objddlModality.value, objddlCategory.value);
    CallBackSelST.callback("L", objhdnID.value, objddlModality.value);
}
function chkSel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    CategoryID = "0";
    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {

            if (document.getElementById("chkSel_" + RowId).checked) {
                if (CategoryID != "2") CategoryID = gridItem.Data[5].toString();
                gridItem.Data[2] = "Y";
                //if (CategoryID == "2") objddlCategory.value = "2";
            }
            else {
                gridItem.Data[2] = "N";
                //if (CategoryID == "2") {
                //    objddlCategory.value = "0";
                //    Consult_OnClick();
                //}
            }
            objddlCategory.value = CategoryID.toString();
            Consult_OnClick();
            parent.GsRetStatus = "true";
            strDtls = GetStudyGridDetails();
            CallBackSelST.callback("U", strDtls);
            break;
        }
        itemIndex++;
    }
}
function GetStudyGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;
    var strSel = "";

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        strSel = gridItem.Data[2].toString();

        if (strSel == "Y") {
            if (strDtls != "") strDtls = strDtls + SecDivider;
            strDtls += gridItem.get_cells()[1].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[3].get_value().toString();
        }
        itemIndex++;
    }
    return strDtls;
}
/********Study Types*************************/

function Consult_OnClick() {
    //if (objddlCategory.value != "2") {
    if (objrdoConsY.checked) {
        objddlCategory.value = "3";
    }
    else if (objrdoConsN.checked) {
        if (CategoryID != "0") objddlCategory.value = CategoryID;
        else objddlCategory.value = "1";
    }

    //}
}

/********Image/Object Count*************************/
function btnRefreshCount_OnClick() {
    if ((objhdnRecByRouter.value == "Y") || (objhdnRecByRouter.value == "M"))
        GetImageCountManual();
    else
        GetImageCountAPI();
}
function GetImageCountManual() {
    var arrRes = new Array();
    var Result;

    try {
        AjaxPro.timeoutPeriod = 1800000;
        //VRSCaseRADlg.GetImageCount(objhdnID.value, ShowProcess);
        Result = VRSCaseRADlg.GetImageCount(objhdnID.value);
        arrRes = Result.value;

        switch (arrRes[0]) {
            case "catch":
            case "false":
                //parent.PopupMessage(RootDirectory, strForm, "GetImageCount()", arrRes[1], "true");
                break;
            case "true":
                objtxtImgCnt.value = arrRes[1];
                objtxtObjCnt.value = arrRes[2];
                //if (objddlModality.value != "0") Modality = objddlModality.options[objddlModality.selectedIndex].text;
                break;
        }
    }
    catch (expErr) {
        //parent.HideProcess();
        //parent.PopupMessage(RootDirectory, strForm, "GetImageCountManual()", expErr.message, "true");
        //parent.GsPopupText = "";
    }
    imgCntInterval = setTimeout(btnRefreshCount_OnClick, 5000);
}
function GetImageCountAPI() {
    var arrRes = new Array();
    var Result;
    var strSUID = ""; var strURL = "";
    var ArrParams = new Array();

    try {

        if (objhdnAPIVER.value == "7.2") {
            strSUID = objhdnSUID.value;
            strURL = objhdnImgCntURL.value.replace("qf_SYUI=", "qf_SYUI=" + strSUID);
            strURL = strURL.replace("#V2", PACSUID);
            strURL = strURL.replace("#V3", PACSPwd);

            AjaxPro.timeoutPeriod = 1800000;
            //VRSCaseRADlg.GetImageCountAPI_72(strURL, ShowProcess);
            Result = VRSCaseRADlg.GetImageCountAPI_72(strURL);
        }
        if (objhdnAPIVER.value == "8") {
            ArrParams[0] = objhdnSUID.value;
            ArrParams[1] = objhdnWS8SRVIP.value;
            ArrParams[2] = objhdnWS8CLTIP.value;
            ArrParams[3] = objhdnWS8SRVUID.value;
            ArrParams[4] = objhdnWS8SRVPWD.value;

            AjaxPro.timeoutPeriod = 1800000;
            //VRSCaseRADlg.GetImageCountAPI_80(ArrParams, ShowProcess);
            Result = VRSCaseRADlg.GetImageCountAPI_80(ArrParams)
        }

        arrRes = Result.value;

        switch (arrRes[0]) {
            case "catch":
            case "false":
                //parent.PopupMessage(RootDirectory, strForm, "GetImageCountAPI()", arrRes[1], "true");
                break;
            case "true":
                objtxtImgCnt.value = arrRes[1];
                objtxtObjCnt.value = arrRes[2];
                //if (objddlModality.value != "0") Modality = objddlModality.options[objddlModality.selectedIndex].text;
                break;
        }
    }
    catch (expErr) {
        //parent.HideProcess();
        //parent.PopupMessage(RootDirectory, strForm, "GetImageCountAPI()", expErr.message, "true");
        //parent.GsPopupText = "";
    }
    imgCntInterval = setTimeout(btnRefreshCount_OnClick, 5000);
}
function GetImageCount(Result) {

    var arrRes = new Array(); 
    var Modality = "";
    parent.GsPopupText = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "GetImageCount()", arrRes[1], "true");
            break;
        case "true":
            objtxtImgCnt.value = arrRes[1];
            objtxtObjCnt.value = arrRes[2];
            //if (objddlModality.value != "0") Modality = objddlModality.options[objddlModality.selectedIndex].text;
            break;
    }

}

/********Image/Object Count*************************/

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
            VRSCaseRADlg.DeleteStudy_72(ArrRecords, strURL, ShowProcess);
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
            VRSCaseRADlg.DeleteStudy_80(ArrRecords, ArrParams, ShowProcess);
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

/********Doc. Grid*****************************/
function ToggleDocUpload() {
    var e = $('#aRDOCCollapse').closest(".searchSection"),
                       t = $('#aRDOCCollapse').find("i"),
                       n = e.find("#divDocUpload");

    e.attr("style") ? n.slideToggle(200, function () {
        e.removeAttr("style")
    }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
}
function btnUpload_OnClick() {
    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();
    parent.GsLaunchURL = "Common/VRSUploader.aspx?dir=" + strDir + "&t=IMG&fileID=" + usrID + "_" + tmpID;
    parent.PopupUpload();

}
function createGuid() {
    //return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function ProcessUpload(ArgsRet) {
    var retArr = new Array();
    var strExistingDtls = ""; var strNewDtls = "";
    var strFileType = "";
    if (ArgsRet != null) {
        parent.GsRetStatus = "true";
        retArr = ArgsRet;

        if (!CheckDulipcateDocument(retArr[0])) {
            strExistingDtls = GetDocGridDetails("Y", retArr[0]);
            strFileType = retArr[0].substr(retArr[0].lastIndexOf(".") + 1, (retArr[0].length - retArr[0].lastIndexOf(".")));

            strNewDtls = "0" + SecDivider;
            strNewDtls += "00000000-0000-0000-0000-000000000000" + SecDivider;
            strNewDtls += retArr[1] + SecDivider;
            strNewDtls += retArr[0] + SecDivider;
            strNewDtls += strFileType;
            DOCADD = "Y";

            CallBackDoc.callback("A", strExistingDtls, strNewDtls);
        }

    }
}
function CheckDulipcateDocument(FileName) {
    var bRet = false;

    var itemIndex = 0;
    var gridItem;
    var srl = "";

    while (gridItem = grdDoc.get_table().getRow(itemIndex)) {
        if (parent.Trim(gridItem.get_cells()[3].get_value().toString()) == parent.Trim(FileName)) {
            srl = gridItem.get_cells()[0].get_value().toString();
            parent.PopupMessage(RootDirectory, strForm, "CheckDulipcateDocument()", "031", "true", srl);
            bRet = true;
            break;
        }
        itemIndex++;
    }
    return bRet;
}
function GetDocGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdDoc.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.get_cells()[0].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[1].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[2].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[3].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[4].get_value().toString();
        itemIndex++;
    }
    return strDtls;
}
function DeleteDocument(ID) {

    strRowID = ID;
    DEL_FLAG = "DOC";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function ShowDocument(ID, FileName) {
    var strFilePath = "/" + RootDirectory + "/CaseList/Temp/" + UserID + "/" + FileName;
    var win = window.open(strFilePath, '_blank');
    win.focus();
}
/********Doc. Grid*****************************/

/********DCM Grid*****************************/
function ToggleDCMUpload() {
    var e = $('#aRDCMCollapse').closest(".searchSection"),
                       t = $('#aRDCMCollapse').find("i"),
                       n = e.find("#divDCMUpload");

    e.attr("style") ? n.slideToggle(200, function () {
        e.removeAttr("style")
    }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
}
function ProcessDCMUpload(ArgsRet) {
    var arrFiles = new Array();
    var strExistingDtls = ""; var strNewDtls = "";
    var strFileType = "";
    if (ArgsRet != null) {
        parent.GsRetStatus = "true";
        arrFiles = ArgsRet[0].split(Divider);
        strExistingDtls = GetDCMGridDetails();
        for (var i = 0; i < arrFiles.length; i++) {
            if (strNewDtls != "") strNewDtls += SecDivider;
            strNewDtls += "00000000-0000-0000-0000-000000000000" + SecDivider;
            strNewDtls += arrFiles[i];
        }
        objiframeUploadDCM.src = "VRSUploadDCMFiles.aspx?uid=" + UserID;
        CallBackDCM.callback("A", strExistingDtls, strNewDtls);

    }
}
function GetDCMGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdDCM.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.get_cells()[0].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[1].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[2].get_value().toString();

        itemIndex++;
    }
    return strDtls;
}
function DeleteDCM(ID) {

    strRowID = ID;
    DEL_FLAG = "DCM";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
/********DCM Grid*****************************/

function DeleteRecord() {
    var Flg = DEL_FLAG;
    var strExistingDtls = "";
    switch (Flg) {
        case "DOC":
            strExistingDtls = GetDocGridDetails();
            CallBackDoc.callback("D", strRowID, strExistingDtls);
            break;
        case "DCM":
            strExistingDtls = GetDCMGridDetails();
            CallBackDCM.callback("D", strRowID, strExistingDtls, objhdnTempDCMFolder.value);
            break;
        case "STUDY":
            DeleteStudy();
            break;
    }


}

function SetSelectedDate(objCtrlID, CalName, LsImgID) {
    var strDate = ""; var strClass = "";
    var dt;
    objCtrl = document.getElementById(objCtrlID.id); if (objCtrl == null) objCtrl = objCtrlID;
    strDate = document.getElementById(objCtrl).value;

    if (parent.Trim(strDate) != "") {
        if (document.all) {
            dt = new Date(parent.SetDateFormat(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
        else {
            dt = new Date(parent.SetDateFormat1(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
    }
    else
        dt = new Date();
    switch (CalName) {
        case "CalFrom":
            CalFrom.setSelectedDate(dt); CalFrom.show();
            break;

    }


}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
        case "FetchBreeds":
            PopulateBreed(Result);
            break;
        case "FetchPhysicians":
            PopulatePhysicians(Result);
            break;
        case "GetImageCountAPI_72":
        case "GetImageCountAPI_80":
        case "GetImageCount":
            GetImageCount(Result);
            break;
        case "DeleteStudy_72":
        case "DeleteStudy_80":
            ProcessDeleteStudy(Result);
            break;
    }

}
function ResetValueDecimal(objCtrlID, decimalPlace) {

    var objCtrl;
    if (decimalPlace == null) decimalPlace = parent.GiDecimal;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value.replace("-", ""), decimalPlace);

}
