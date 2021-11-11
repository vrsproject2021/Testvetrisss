var strID = ""; var strSUID = ""; var IS_ARCHIVE = ""; var ROWID = ""; var RE_RATE = "";
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
    objhdnError.value = "";

    if (UserRoleCode == "SUPP" || UserRoleCode == "SYSADMIN") {
        document.getElementById("divApprovedBy").style.display = "inline";
        document.getElementById("divCategory").style.display = "inline";
        document.getElementById("divRAD1").style.display = "inline";
        document.getElementById("divRAD2").style.display = "inline";
        document.getElementById("divRptReason").style.display = "inline";
    }
    else if (UserRoleCode == "RDL") {
        document.getElementById("divApprovedBy").style.display = "inline";
        document.getElementById("divCategory").style.display = "inline";
        document.getElementById("divRAD1").style.display = "inline";
        document.getElementById("divRAD2").style.display = "inline";
        document.getElementById("divRptReason").style.display = "inline";
    }
    else document.getElementById("divCategory").style.display = "inline";

    if (UserRoleCode == "SUPP") {
        $('#divLblStudyUID').css('display', 'block');
        $('#divTxtStudyUID').css('display', 'block');
    } else {
        $('#divLblStudyUID').css('display', 'none');
        $('#divTxtStudyUID').css('display', 'none');
    }
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
        objddlStatus.value = parent.GsFilter[13];
        if (parent.GsFilter[14] == "Y") objchkMarkToTeach.checked = true;
        objtxtStudyUID.value = parent.GsFilter[15];
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
    ArrRecords[13] = objddlStatus.value;
    ArrRecords[14] = "A"; if (objchkMarkToTeach.checked) ArrRecords[14] = "Y";
    ArrRecords[15] = UserID;
    ArrRecords[16] = MenuID;
    ArrRecords[17] = SessionID;
    ArrRecords[18] = UserRoleCode;
    ArrRecords[19] = objhdnPageSize.value;
    ArrRecords[20] = objhdnPageNo.value;
    ArrRecords[21] = $('#ddlDatabase option:selected').val() == 'Current Year' ? 'vrsdb' : $('#ddlDatabase option:selected').val();
    ArrRecords[22] = $('#ddlDatabase option:selected').val() == 'Current Year' ? new Date().getFullYear() : $('#ddlDatabase option:selected').text();
    ArrRecords[23] = objtxtStudyUID.value;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function ddlDatabase_OnChange() {
    ResetRecord();
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
    parent.GsFilter[13] = parent.Trim(objddlStatus.value);
    if (objchkMarkToTeach.checked) parent.GsFilter[14] = "Y"; else parent.GsFilter[14] = "A";
    parent.GsFilter[15] = objtxtStudyUID.value;
}
function ResetRecord() {
    var strDtFrom = ""; var strDtTill = "";
    var selectedYear = new Date().getFullYear();
    if ($('#ddlDatabase option:selected').val() != 'Current Year') {
        selectedYear = $('#ddlDatabase option:selected').text();
    }
    var dtFrom = new Date(); var dtTill = new Date();
    dtFrom = dtFrom.addDays(-7);
    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + selectedYear;
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + selectedYear;
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDtFrom = selectedYear + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2);
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + selectedYear;

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + selectedYear;
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + selectedYear;
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDtTill = selectedYear + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2);
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + selectedYear;

    objtxtPatientName.value = "";
    objddlModality.value = "0";
    objtxtPatientName.value = "";
    objddlCategory.value = "0";
    objddlStatus.value = "-1";
    objchkRecDt.checked = false;
    objtxtFromDt.value = strDtFrom;
    objtxtTillDt.value = strDtTill;
    objddlInstitution.value = "00000000-0000-0000-0000-000000000000";
    objddlPhys.length = 0;
    BindDDLBlank();
    objddlApprovedBy.value = "00000000-0000-0000-0000-000000000000";
    objddlRadiologist.value = "00000000-0000-0000-0000-000000000000";
    objchkShowAbRpt.checked = false;
    objddlAbRptReason.value = "00000000-0000-0000-0000-000000000000";
    objchkPendRptRel.checked = false;
    objtxtStudyUID.value = "";

    objhdnPageNo.value = 1;
    SearchRecord();
}

function ddlInstitution_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlInstitution.value;
        ArrRecords[1] = "Y"; if (objhdnRadFnRights.value.indexOf("VWINSTINFO") == -1) ArrRecords[1] = "N";
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalRptBrw.FetchPhysicians(ArrRecords, ShowProcess);
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
function btnDV_OnClick() {

    try {
        if (parent.Trim(objhdnSelSUID.value) != "") {
            location.href = "vetrisepacs://open?uid=" + objhdnSelSUID.value + "&c=" + objhdnPACSCred.value;
        }
        else {
            parent.PopupMessage(RootDirectory, strForm, "btnDV_OnClick()", "471", "true");
        }
    }
    catch (expErr) {
        parent.HideMessage();
        parent.PopupMessage(RootDirectory, strForm, "btnDV_OnClick()", expErr.message, "true");

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

    

    try {
        parent.parent.PopupProcess("N");

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
        ArrRecords[13] = objddlStatus.value;
        ArrRecords[14] = UserID;
        ArrRecords[15] = UserRoleCode;
        ArrRecords[16] = $('#ddlDatabase option:selected').val() == 'Current Year' ? 'vrsdb' : $('#ddlDatabase option:selected').val();
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalRptBrw.GenerateExcel(ArrRecords, ShowProcess);

    }
    catch (expErr) {
        parent.parent.HideProcess();
        parent.parent.PopupMessage(RootDirectory, strForm, "btnExcel_OnClick()", expErr.message, "true");
    }

}
function ProcessReport(Result) {
    var arrRet = new Array();
    arrRet = Result.value;
    switch (arrRet[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessReport()", arrRet[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessReport()", arrRet[1], "true");
            break;
        case "true":
            parent.GsLaunchURL = arrRet[1];
            parent.GsFilePath = arrRet[2];
            parent.GsFileType = "XLS";
            parent.PopupReportViewer();
            break;
    }
}

function btnRpt_OnClick(ID, PatientName, CustomRpt,RptFmt) {
    parent.GsFileType = "PDF";
    if (CustomRpt == "N")
        parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=1&ID=" + ID + "&PNM=" + PatientName + "&UID=" + UserID + "&FMT=" + RptFmt;
    else
        parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=3&ID=" + ID + "&PNM=" + PatientName + "&UID=" + UserID + "&FMT=" + RptFmt;
    parent.PopupReportViewer();
}
function btnImg_OnClick(ID, StudyUID, AccnNo, PatientID, URL) {
    URL = URL.replace("#V1", AccnNo);
    URL = URL.replace("#V2", PatientID);
    URL = URL.replace("#V3", WSSessionID);
    URL = URL.replace("#V4", WS8SRVUID);
    URL = URL.replace("#V5", WS8SRVPWD);
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
    parent.GsLaunchURL = "CaseList/VRSForwardLinks.aspx?sid=" + ID + "&uid=" + UserID + "&th=" + selTheme ;
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
            ArrRecords[3] = SessionID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSCaseFinalRptBrw.ReleaseReport(ArrRecords, ShowProcess);

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
            parent.PopupMessage(RootDirectory, strForm, "ProcessReportRelease()", arrRes[1], "false", arrRes[2]);
            SearchRecord();
            break;
    }
}

/**************Procss Download ******************/
function btnDLImg_OnClick(ID, SUID, InstCode, PhysCode) {
    //parent.GsLaunchURL = "CaseList/VRSCaseFinalRptBrw.aspx?id=" + ID + "&mid=" + MenuID + "&uid=" + UserID + "&arch=N";
    //parent.PopupGeneralMedium();
    var InstName = ""; PatientName = "";
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            PatientName = gridItem.Data[2].toString();
            InstName = gridItem.Data[27].toString();
            break;
        }
        itemIndex++;
    }

    document.getElementById("btnDLImg_" + ID).style.display = "none";
    document.getElementById("imgDLProc_" + ID).style.display = "inline";
    CopyFolder(ID, SUID, InstCode, InstName, PhysCode, PatientName);
}
function CopyFolder(ID, SUID, InstCode, InstName, PhysCode, PatientName) {
    var ArrRecords = new Array();
    var arrRes = new Array();

    try {
        ArrRecords[0] = ID;
        ArrRecords[1] = SUID;
        ArrRecords[2] = InstCode;
        ArrRecords[3] = InstName;
        ArrRecords[4] = PhysCode;
        ArrRecords[5] = PatientName;
        ArrRecords[6] = objhdnPACSARCHIVEFLDR.value;
        ArrRecords[7] = objhdnPACSARCHALTFLDR.value;
        ArrRecords[8] = objhdnDCMMODIFYEXEPATH.value;
        ArrRecords[9] = "N"; if (objhdnRadFnRights.value.indexOf("VWINSTINFO") > -1) ArrRecords[8] = "Y";
        ArrRecords[10] = UserID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalRptBrw.CopyFolder(ArrRecords, ShowDownloadProcess);

    }
    catch (expErr) {
        document.getElementById("btnDLImg_" + ID).style.display = "inline";
        document.getElementById("imgDLProc_" + ID).style.display = "none";
        parent.PopupMessage(RootDirectory, strForm, "CopyFolder()", PatientName + "..." + expErr.message, "true");
    }

}
function ProcessCopy(Result) {
    var arrRes = new Array();
    var ID = ""; var PatientName = ""; var TgtFolder = "";
    arrRes = Result.value;

    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessCopy()", arrRes[1], "true");
            document.getElementById("btnDLImg_" + arrRes[2]).style.display = "inline";
            document.getElementById("imgDLProc_" + arrRes[2]).style.display = "none";
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessCopy()", arrRes[1], "true", arrRes[3]);
            document.getElementById("btnDLImg_" + arrRes[2]).style.display = "inline";
            document.getElementById("imgDLProc_" + arrRes[2]).style.display = "none";
            break;
        case "true":
            ID = arrRes[1];
            PatientName = arrRes[2];
            TgtFolder = arrRes[3];
            CompressFolder(ID, PatientName, TgtFolder);
            break;
    }
}
function CompressFolder(ID, PatientName, TgtFolder) {
    var ArrRecords = new Array();
    var arrRes = new Array();

    try {

        ArrRecords[0] = ID;
        ArrRecords[1] = PatientName;
        ArrRecords[2] = TgtFolder;
        ArrRecords[3] = UserID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalRptBrw.CompressFolder(ArrRecords, ShowDownloadProcess);
    }
    catch (expErr) {
        document.getElementById("btnDLImg_" + ID).style.display = "inline";
        document.getElementById("imgDLProc_" + ID).style.display = "none";
        parent.PopupMessage(RootDirectory, strForm, "CompressFolder()", PatientName + "..." + expErr.message, "true");
    }
}
function ProcessCompress(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;

    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessCopy()", arrRes[1], "true");
            document.getElementById("btnDLImg_" + arrRes[2]).style.display = "inline";
            document.getElementById("imgDLProc_" + arrRes[2]).style.display = "none";
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessCopy()", arrRes[1], "true", arrRes[3]);
            document.getElementById("btnDLImg_" + arrRes[2]).style.display = "inline";
            document.getElementById("imgDLProc_" + arrRes[2]).style.display = "none";
            break;
        case "true":
            location.href = "Temp/" + UserID + "/" + arrRes[1];
            document.getElementById("btnDLImg_" + arrRes[2]).style.display = "inline";
            document.getElementById("imgDLProc_" + arrRes[2]).style.display = "none";
            break;
    }
}
function btnDLImgDsbl_OnClick(ID, SUID, InstCode) {
    var InstName = ""; PatientName = "";
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            PatientName = gridItem.Data[2].toString();
            InstName = gridItem.Data[27].toString();
            break;
        }
        itemIndex++;
    }

    document.getElementById("btnDLImgDsbl_" + ID).style.display = "none";
    document.getElementById("imgDLProc_" + ID).style.display = "inline";
    CheckFileCount(ID, SUID, InstCode, InstName, PatientName);
}
function CheckFileCount(ID, SUID, InstCode, InstName, PatientName) {
    var ArrRecords = new Array();
    var arrRes = new Array();

    try {
        ROWID = ID;
        ArrRecords[0] = ID;
        ArrRecords[1] = SUID;
        ArrRecords[2] = InstCode;
        ArrRecords[3] = InstName;
        ArrRecords[4] = PatientName;
        ArrRecords[5] = objhdnPACSARCHIVEFLDR.value;
        ArrRecords[6] = objhdnPACSARCHALTFLDR.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalRptBrw.CheckFileCount(ArrRecords, ShowDownloadProcess);

    }
    catch (expErr) {
        document.getElementById("btnDLImgDsbl_" + ID).style.display = "inline";
        document.getElementById("imgDLProc_" + ID).style.display = "none";
        parent.PopupMessage(RootDirectory, strForm, "CheckFileCount()", PatientName + "..." + expErr.message, "true");
    }

}
function ProcessCount(Result) {
    var arrRes = new Array();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ID = ""; var PatientName = ""; var TgtFolder = "";
    arrRes = Result.value;

    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessCount()", arrRes[1], "true");
            document.getElementById("btnDLImgDsbl_" + arrRes[2]).style.display = "inline";
            document.getElementById("imgDLProc_" + arrRes[2]).style.display = "none";
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessCount()", arrRes[1], "true", arrRes[3]);
            document.getElementById("btnDLImgDsbl_" + arrRes[2]).style.display = "inline";
            document.getElementById("imgDLProc_" + arrRes[2]).style.display = "none";
            break;
        case "true":
            ID = arrRes[1];
            PatientName = arrRes[3];
            document.getElementById("imgDLProc_" + arrRes[2]).style.display = "none";
            
            if (arrRes[1] == "490") {
                parent.PopupMessage(RootDirectory, strForm, "ProcessCount()", arrRes[1], "true", arrRes[3],arrRes[4]);
                document.getElementById("btnDLImgDsbl_" + arrRes[2]).style.display = "inline";
            }
            else if (arrRes[1] == "491") {
                parent.PopupMessage(RootDirectory, strForm, "ProcessCount()", arrRes[1], "true", arrRes[3]);
                document.getElementById("btnDLImg_" + arrRes[2]).style.display = "inline";
                document.getElementById("btnDLImgDsbl_" + arrRes[2]).style.display = "none";

                while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
                    RowId = gridItem.Data[0].toString();
                    if (RowId == ROWID) {
                        gridItem.Data[29] = "Y";
                        break;
                    }
                    itemIndex++;
                }
            }

            break;
    }
}
function ShowDownloadProcess(Result, MethodName) {
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "CopyFolder":
            ProcessCopy(Result);
            break;
        case "CompressFolder":
            ProcessCompress(Result);
            break;
        case "CheckFileCount":
            ProcessCount(Result);
            break;
    }
}
/**************Procss Download ******************/

function btnEditRpt_OnClick(ID, IsArchive) {
    objhdnID.value = ID;

    if (UserRoleCode == "RDL") {
        IS_ARCHIVE = IsArchive;
        CheckRadiologistLock();
    }
    else {
        if (IsArchive == "N") btnBrwEditUI_Onclick("CaseList/VRSCaseFinalDlg.aspx");
        else if (IsArchive == "Y") btnBrwEditUI_Onclick("CaseList/VRSCaseArchivelDlg.aspx");
    }
}

function btnEditRateRpt_OnClick(ID) {
    objhdnID.value = ID;

    if (UserRoleCode == "RDL") {
        RE_RATE = "Y";
        CheckRadiologistLock();
    }
    
}
function btnActivity_OnClick(ID, SUID) {
    strValidate = "N";
    btnBrwEditUI_Onclick("HouseKeeping/VRSStudyRadiologistActivity.aspx?cf=" + MenuID + "&suid=" + SUID);
}
function btnRevert_OnClick(ID, StudyUID) {
    strID = ID; strSUID = StudyUID;
    parent.GsDlgConfAction = "SAV";
    parent.PopupConfirm("311");
}
function btnSel_OnClick(ID, StudyUID) {
    if (parent.Trim(objhdnSelSUID.value) != "") objhdnSelSUID.value += ";";
    objhdnSelSUID.value += StudyUID;

    document.getElementById("btnSel_" + ID).style.display = "none";
    document.getElementById("btnCheck_" + ID).style.display = "inline";
}
function btnCheck_OnClick(ID, StudyUID) {
    var suid = objhdnSelSUID.value;
    var arrSUID = new Array();

    if (parent.Trim(suid) != "") {
        if (suid.indexOf(";") >= 0) {
            arrSUID = suid.split(";");
            objhdnSelSUID.value = "";
            for (var i = 0; i < arrSUID.length; i++) {
                if (arrSUID[i] != StudyUID) {
                    if (parent.Trim(objhdnSelSUID.value) != "") objhdnSelSUID.value += ";";
                    objhdnSelSUID.value += arrSUID[i];
                }
            }
        }
        else {
            objhdnSelSUID.value = "";
        }
    }

    document.getElementById("btnSel_" + ID).style.display = "inline";
    document.getElementById("btnCheck_" + ID).style.display = "none";
}
function ProcessSave(ArgsRet) {
    if (ArgsRet == "Y") {
        parent.PopupProcess("N");
        var ArrRecords = new Array();

        try {

            ArrRecords[0] = strID;
            ArrRecords[1] = strSUID;
            ArrRecords[2] = UserID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSCaseArchiveBrw.RevertArchiveStudy(ArrRecords, ShowProcess);
        }
        catch (expErr) {
            parent.HideProcess();
            parent.PopupMessage(RootDirectory, strForm, "ProcessSave()", expErr.message, "true");
        }
    }
}
function RevertArchiveStudy(Result) {
    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "RevertArchiveStudy()", arrRes[1], "true");
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "RevertArchiveStudy()", arrRes[1], "false");
            parent.GsFilter.length = 0;
            parent.FetchMenuRecordCount();
            parent.objiframePage.src = "CaseList/VRSCaseRADlg.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value;
            break;
    }

}
function CheckRadiologistLock() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = "100";
        ArrRecords[2] = UserID;
        ArrRecords[3] = MenuID;
        ArrRecords[4] = SessionID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalRptBrw.CheckRadiologistLock(ArrRecords, ShowProcess);
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
            if (RE_RATE == "Y") {
                RE_RATE = "";
                btnBrwEditUI_Onclick("CaseList/VRSRateReportDlg.aspx");
            }
            else if (IS_ARCHIVE == "N") {
                if (user_name == "") btnBrwEditUI_Onclick("CaseList/VRSCaseFinalDlg.aspx");
                else btnBrwEditUI_Onclick("CaseList/VRSCaseFinalDlg.aspx?unm=" + user_name);
            }
            else if (IS_ARCHIVE == "Y") {
                if (user_name == "") btnBrwEditUI_Onclick("CaseList/VRSCaseArchivelDlg.aspx");
                else btnBrwEditUI_Onclick("CaseList/VRSCaseArchivelDlg.aspx?unm=" + user_name);
            }

            break;
    }


}
function btnDel_OnClick(ID, StudyUID, ViaDR) {
    strID = ID; strSUID = StudyUID; strViaDR = ViaDR;
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("123");
}
function DeleteRecord() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    var ArrParams = new Array();
    var strURL = "";

    try {


        ArrRecords[0] = strID;
        ArrRecords[1] = strSUID;
        ArrRecords[2] = strViaDR;
        ArrRecords[3] = UserID;
        ArrRecords[4] = MenuID;
        ArrRecords[5] = SessionID;

        ArrParams[0] = objhdnWS8SRVIP.value;
        ArrParams[1] = objhdnWS8CLTIP.value;
        ArrParams[2] = objhdnWS8SRVUID.value;
        ArrParams[3] = objhdnWS8SRVPWD.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseFinalRptBrw.DeleteStudy_80(ArrRecords, ArrParams, ShowProcess);

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
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteStudy()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            SearchRecord();
            break;
    }
}
function btnDisc_OnClick(ID) {
    parent.GsLaunchURL = "CaseList/VRSApplyPromotion.aspx?id=" + ID + "&mid=" + MenuID + "&uid=" + UserID + "&sid=" + SessionID + "&th=" + selTheme;
    parent.PopupGeneralMedium();
}
function ProcessGeneralMedium(ArgsRet) {
    if (ArgsRet == "Y") {
        SearchRecord();
    }
}
function btnCompare_OnClick(ID) {
    parent.OpenReportComparison(ID, UserID);
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
        case "CheckRadiologistLock":
            ProcessRadiologistLock(Result);
            break;
        case "DeleteStudy_80":
            ProcessDeleteStudy(Result);
            break;
        case "GenerateExcel":
            ProcessReport(Result);
            break;
        case "ReleaseReport":
            ProcessReportRelease(Result);
            break;
    }

}
