var strID = ""; var strSUID = "";
$(document).ready($(function () {
    window.scrollTo(0, 0);
}))
function view_Searchform() {
    $("#searchSection").slideToggle(800);
    $("#searchIcon").toggleClass("icon", 800);
}

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
    if (UserRoleCode == "SUPP" || UserRoleCode == "SYSADMIN") {
        document.getElementById("divApprovedBy").style.display = "inline";
        document.getElementById("divCategory").style.display = "inline";
        document.getElementById("divRAD1").style.display = "inline";
        document.getElementById("divRAD2").style.display = "inline";
        document.getElementById("divRptReason").style.display = "inline";
    }
    else if (UserRoleCode == "RDL" || UserRoleCode == "SUPP" || UserRoleCode == "SYSADMIN") {
        document.getElementById("divApprovedBy").style.display = "inline";
        document.getElementById("divCategory").style.display = "inline";
        document.getElementById("divRAD1").style.display = "inline";
        document.getElementById("divRAD2").style.display = "inline";
    }
    else document.getElementById("divCategory").style.display = "inline";
    objhdnError.value = "";
    chkShowAbRpt_OnClick();
}
function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objtxtPatientName.value = parent.GsFilter[0];
        objddlModality.value = parent.GsFilter[1];
        if (parent.GsFilter[2] == "Y") objchkRecDt.checked = true;
        objtxtFromDt.value = parent.GsFilter[3];
        objtxtTillDt.value = parent.GsFilter[4];
        objddlInstitution.value = parent.GsFilter[5];
        objddlPhys.value = parent.GsFilter[6];
        objddlCategory.value = parent.GsFilter[7];
        objddlRadiologist.value = parent.GsFilter[8];
        objddlApprovedBy.value = parent.GsFilter[9];
        if (parent.GsFilter[10] == "Y") objchkShowAbRpt.checked = true;
        objddlAbRptReason.value = parent.GsFilter[11];
        if (parent.GsFilter[12] == "Y") objchkPendRptRel.checked = true;
    }
    else
        parent.GsFilter.length = 0;
}
function chkShowAbRpt_OnClick() {
    if (objchkShowAbRpt.checked) {
        objddlAbRptReason.disabled = false;
    }
    else {
        objddlAbRptReason.value = "00000000-0000-0000-0000-000000000000";
        objddlAbRptReason.disabled = true;
    }
}
function onSearch() {
    objhdnPageNo.value = 1;
    SearchRecord();
    view_Searchform();
}
function SearchRecord() {
    var ArrRecords = new Array();
    var strDtFrom = ""; var strDtTill = "";

    if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
    else {
        if (document.all)
            strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if (parent.Trim(objtxtTillDt.value) == "") strDtTill = "01Jan1900";
    else {
        if (document.all)
            strDtTill = parent.SetDateFormat(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtTill = parent.SetDateFormat1(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    ArrRecords[0] = objtxtPatientName.value;
    ArrRecords[1] = objddlModality.value;
    ArrRecords[2] = "N"; if (objchkRecDt.checked) ArrRecords[2] = "Y";
    ArrRecords[3] = strDtFrom;
    ArrRecords[4] = strDtTill;
    ArrRecords[5] = objddlInstitution.value;
    ArrRecords[6] = objddlPhys.value;
    ArrRecords[7] = objddlCategory.value;
    ArrRecords[8] = objddlApprovedBy.value;
    ArrRecords[9] = objddlRadiologist.value;
    ArrRecords[10] = "N"; if (objchkShowAbRpt.checked) ArrRecords[10] = "Y";
    ArrRecords[11] = objddlAbRptReason.value;
    ArrRecords[12] = "X"; if (objchkPendRptRel.checked) ArrRecords[12] = "Y";
    ArrRecords[13] = UserID;
    ArrRecords[14] = UserRoleCode;
    ArrRecords[15] = objhdnPageSize.value;
    ArrRecords[16] = objhdnPageNo.value;

    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtPatientName.value);
    parent.GsFilter[1] = parent.Trim(objddlModality.value);
    if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
    parent.GsFilter[3] = objtxtFromDt.value;
    parent.GsFilter[4] = objtxtTillDt.value;
    parent.GsFilter[5] = parent.Trim(objddlInstitution.value);
    parent.GsFilter[6] = parent.Trim(objddlPhys.value);
    parent.GsFilter[7] = parent.Trim(objddlCategory.value);
    parent.GsFilter[8] = parent.Trim(objddlRadiologist.value);
    parent.GsFilter[9] = parent.Trim(objddlApprovedBy.value);
    if (objchkShowAbRpt.checked) parent.GsFilter[10] = "Y"; else parent.GsFilter[10] = "N";
    parent.GsFilter[11] = parent.Trim(objddlAbRptReason.value);
    if (objchkPendRptRel.checked) parent.GsFilter[12] = "Y"; else parent.GsFilter[12] = "X";
}
function ResetRecord() {
    var strDtFrom = ""; var strDtTill = "";

    var dtFrom = new Date(); var dtTill = new Date();
    dtFrom = dtFrom.addDays(-7);
    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDtFrom = dtFrom.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDtTill = dtTill.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2);

    objtxtPatientName.value = "";
    objddlModality.value = "0";
    objtxtPatientName.value = "";
    objddlCategory.value = "0";
    objchkRecDt.checked = false;
    objtxtFromDt.value = strDtFrom;
    objtxtTillDt.value = strDtTill;
    objddlInstitution.value = "00000000-0000-0000-0000-000000000000";
    objddlPhys.length = 0;
    objddlApprovedBy.value = "00000000-0000-0000-0000-000000000000";
    objddlRadiologist.value = "00000000-0000-0000-0000-000000000000";
    //if (objddlRadiologist.length == 2) objddlRadiologist.selectedIndex = 1;
    //else objddlRadiologist.value = "00000000-0000-0000-0000-000000000000";
    //if (objddlApprovedBy.length == 2) objddlApprovedBy.selectedIndex = 1;
    //else objddlApprovedBy.value = "00000000-0000-0000-0000-000000000000";
    objhdnPageNo.value = 1;
    SearchRecord();
}
function btnWL_OnClick(ID, URL) {
    URL = URL.replace("#V2", PACSUID);
    URL = URL.replace("#V3", PACSPwd);
    parent.NavigatePACS(URL);
}
function btnRpt_OnClick(ID, URL, PatientName, CustomRpt) {
    //URL = URL.replace("#V2", PACSUID);
    //URL = URL.replace("#V3", PACSPwd);
    //parent.NavigatePACS(URL);
    parent.GsFileType = "PDF";
    if (CustomRpt == "N")
        parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=1&ID=" + ID + "&PNM=" + PatientName + "&UID=" + UserID;
    else
        parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=3&ID=" + ID + "&PNM=" + PatientName + "&UID=" + UserID;
    parent.PopupReportViewer();
}
function btnImg_OnClick(ID, StudyUID, AccnNo, PatientID, URL) {
    if (APIVER == "7.2") {
        URL = URL.replace("#V1", StudyUID);
        URL = URL.replace("#V2", PACSUID);
        URL = URL.replace("#V3", PACSPwd);
    }
    else if (APIVER == "8") {
        URL = URL.replace("#V1", AccnNo);
        URL = URL.replace("#V2", PatientID);
        URL = URL.replace("#V3", WSSessionID);
        URL = URL.replace("#V4", WS8SRVUID);
        URL = URL.replace("#V5", WS8SRVPWD);

    }

    parent.NavigatePACS(URL);
}
function btnImgViewer_OnClick(StudyUID) {

    try {
        location.href = "vetrisepacs://open?uid=" + StudyUID + "&c=" + objhdnPACSCred.value;
    }
    catch (expErr) {
        parent.HideMessage();
        parent.PopupMessage(RootDirectory, strForm, "btnImgViewer_OnClick()", expErr.message, "true");

    }
}
function btnFwd_OnClick(ID, StudyUID) {
    parent.GiWidth = 400;
    parent.GiTop = 15;
    parent.GsLaunchURL = "CaseList/VRSForwardLinks.aspx?sid=" + ID + "&uid=" + UserID;
    parent.PopupDataList();
}
function btnRelease_OnClick(ID) {
    strID = ID;
    parent.GsDlgConfAction = "RELRPT";
    parent.PopupConfirm("451");
}
function ReleaseReport(ArgsRet) {
    if (ArgsRet == "Y") {
        var ArrRecords = new Array();

        try {
            ArrRecords[0] = strID;
            ArrRecords[1] = UserID;
            ArrRecords[2] = MenuID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSCaseFinalBrw.ReleaseReport(ArrRecords, ShowProcess);

        }
        catch (expErr) {
            parent.HideProcess();
            parent.PopupMessage(RootDirectory, strForm, "ReleaseReport()", expErr.message, "true");
        }
    }
}
function ProcessReportRelease(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessReportRelease()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessReportRelease()", arrRes[1], "true");
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ProcessReportRelease()", arrRes[1], "false",arrRes[2]);
            SearchRecord();
            break;
    }
}
function btnDLImg_OnClick(ID) {
    parent.GsLaunchURL = "CaseList/VRSDownloadImg.aspx?id=" + ID + "&mid=" + MenuID + "&uid=" + UserID + "&arch=N";
    parent.PopupGeneralMedium();
}
function btnEditRpt_OnClick(ID) {
    objhdnID.value = ID;
    if (UserRoleCode == "RDL") {
        CheckRadiologistLock();
    }
    else {
        btnBrwEditUI_Onclick("CaseList/VRSCaseFinalDlg.aspx");
    }
}
function btnActivity_OnClick(ID, SUID) {
    strValidate = "N";
    btnBrwEditUI_Onclick("HouseKeeping/VRSStudyRadiologistActivity.aspx?cf=" + MenuID + "&suid=" + SUID);
}
function CheckRadiologistLock() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = "100";
        ArrRecords[2] = UserID;
        ArrRecords[3] = MenuID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalBrw.CheckRadiologistLock(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "CheckRadiologistLock()", expErr.message, "true");
    }
}
function ProcessRadiologistLock(Result) {

    var arrRes = new Array(); var url = ""; var refresh = "N"; var user_name = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessRadiologistLock()", arrRes[1], "true");
            break;
        case "false":
            refresh = arrRes[1];
            parent.PopupMessage(RootDirectory, strForm, "ProcessRadiologistLock()", arrRes[2], "true", arrRes[3]);
            if (refresh == "Y") {
                SearchRecord();
                parent.FetchMenuRecordCount();
            }
            break;
        case "true":
            refresh = arrRes[1];
            user_name = arrRes[3];
            if (refresh == "Y") {
                SearchRecord();
                parent.FetchMenuRecordCount();
            }
            if (user_name == "") btnBrwEditUI_Onclick("CaseList/VRSCaseFinalDlg.aspx");
            else btnBrwEditUI_Onclick("CaseList/VRSCaseFinalDlg.aspx?unm=" + user_name);
            //if (parent.Trim(arrRes[3]) != "") parent.PopupMessage(RootDirectory, strForm, "ProcessRadiologistLock()", arrRes[2], "false", arrRes[3]);
            break;
    }


}
function btnCompare_OnClick(ID) {
    parent.OpenReportComparison(ID, UserID);
}

function ddlInstitution_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlInstitution.value;
        ArrRecords[1] = "Y"; if (objhdnRadFnRights.value.indexOf("VWINSTINFO") == -1) ArrRecords[1] = "N";
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalBrw.FetchPhysicians(ArrRecords, ShowProcess);
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
            for (var i = 1; i < arrRes.length; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlPhys.add(op);
            }
            break;
    }

}
function BindDDLBlank() {
    var op1 = document.createElement("option");
    op1.value = "00000000-0000-0000-0000-000000000000"; op1.text = "Select One";
    objddlPhys.add(op1);
}
function btnArch_OnClick(ID, StudyUID) {
    strID = ID; strSUID = StudyUID;
    parent.GsDlgConfAction = "SAV";
    parent.PopupConfirm("073");
}
function ProcessSave(ArgsRet) {
    if (ArgsRet == "Y") {
        parent.PopupProcess("N");
        var ArrRecords = new Array();

        try {

            ArrRecords[0] = strID;
            ArrRecords[1] = strSUID;
            ArrRecords[2] = UserID;
            ArrRecords[3] = MenuID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSCaseFinalBrw.ArchiveStudy(ArrRecords, ShowProcess);
        }
        catch (expErr) {
            parent.HideProcess();
            parent.PopupMessage(RootDirectory, strForm, "ProcessSave()", expErr.message, "true");
        }
    }
}
function ArchiveStudy(Result) {
    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ArchiveStudy()", arrRes[1], "true");
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ArchiveStudy()", arrRes[1], "false");
            SearchRecord();
            parent.FetchMenuRecordCount();
            break;
    }

}
function btnDisc_OnClick(ID) {
    parent.GsLaunchURL = "CaseList/VRSApplyPromotion.aspx?id=" + ID + "&mid=" + MenuID + "&uid=" + UserID;
    parent.PopupGeneralMedium();
}
function ProcessGeneralMedium(ArgsRet) {
    if (ArgsRet == "Y") {
        SearchRecord();
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
        case "CalTill":
            CalTill.setSelectedDate(dt); CalTill.show();
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
        case "FetchPhysicians":
            PopulatePhysicians(Result);
            break;
        case "ArchiveStudy":
            ArchiveStudy(Result);
            break;
        case "CheckRadiologistLock":
            ProcessRadiologistLock(Result);
            break;
        case "GenerateExcel":
            ProcessReport(Result);
            break;
        case "ReleaseReport":
            ProcessReportRelease(Result);
            break;
    }

}
function ProcessReport(Result) {
    var arrRet = new Array();
    arrRet = Result.value;
    switch (arrRet[0]) {
        case "catch":
            parent.parent.PopupMessage(RootDirectory, strForm, "ProcessReport()", arrRet[1], "true");
            break;
        case "false":
            parent.parent.PopupMessage(RootDirectory, strForm, "ProcessReport()", arrRet[1], "true");
            break;
        case "true":
            parent.parent.GsLaunchURL = arrRet[1];
            parent.parent.GsFilePath = arrRet[2];
            parent.parent.GsFileType = "XLS";
            parent.parent.PopupReportViewer();
            break;
    }
}
function btnExcel_OnClick() {
    var ArrRecords = new Array();
    var strDtFrom = ""; var strDtTill = "";

    if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
    else {
        if (document.all)
            strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if (parent.Trim(objtxtTillDt.value) == "") strDtTill = "01Jan1900";
    else {
        if (document.all)
            strDtTill = parent.SetDateFormat(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtTill = parent.SetDateFormat1(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    ArrRecords[0] = objtxtPatientName.value;
    ArrRecords[1] = objddlModality.value;
    ArrRecords[2] = "N"; if (objchkRecDt.checked) ArrRecords[2] = "Y";
    ArrRecords[3] = strDtFrom;
    ArrRecords[4] = strDtTill;
    ArrRecords[5] = objddlInstitution.value;
    ArrRecords[6] = objddlPhys.value;
    ArrRecords[7] = objddlCategory.value;
    ArrRecords[8] = objddlApprovedBy.value;
    ArrRecords[9] = objddlRadiologist.value;
    ArrRecords[10] = "N"; if (objchkShowAbRpt.checked) ArrRecords[10] = "Y";
    ArrRecords[11] = objddlAbRptReason.value;
    ArrRecords[12] = UserID;
    ArrRecords[13] = UserRoleCode;
    try {
        parent.parent.PopupProcess("N");


        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalBrw.GenerateExcel(ArrRecords, ShowProcess);

    }
    catch (expErr) {
        parent.parent.HideProcess();
        parent.parent.PopupMessage(RootDirectory, strForm, "btnExcel_OnClick()", expErr.message, "true");
    }

}