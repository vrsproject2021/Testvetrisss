$(document).ready($(function () {
    window.scrollTo(0, 0);
}))

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
    ArrRecords[6] = objddlCategory.value;
    ArrRecords[7] = UserID;
    ArrRecords[8] = UserRoleCode;
    ArrRecords[9] = MenuID;
    CallBackBrw.callback(ArrRecords);
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
    objddlCategory.value = "0";
    objtxtPatientName.value = "";
    objchkRecDt.checked = false;
    objtxtFromDt.value = strDtFrom;
    objtxtTillDt.value = strDtTill;
    objddlInstitution.value = "00000000-0000-0000-0000-000000000000";

    SearchRecord();
}
function btnNav_OnClick(ID, Menu_ID, Patient_Name, InstID) {
    parent.objhdnMenuID.value = Menu_ID;
    parent.GsFilter.length = 0;
    parent.PopupLoad();
    try{
        switch (Menu_ID) {
            case "20":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "";
                parent.GsFilter[2] = parent.Trim(InstID);
                if (objchkRecDt.checked) parent.GsFilter[3] = "Y"; else parent.GsFilter[3] == "N";
                parent.GsFilter[4] = objtxtFromDt.value;
                parent.GsFilter[5] = objtxtTillDt.value;
                parent.GsFilter[6] = "";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCaseRABrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + selTheme;
                break;
            case "21":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "-1";
                parent.GsFilter[7] = "0";
                parent.GsFilter[8] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[9] = "0";
                parent.GsFilter[10] = "0";
                parent.GsFilter[11] = "";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSInProgressBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + selTheme;
                break;
            case "22":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[7] = "0"
                parent.GsFilter[8] = "P"; 
                parent.GsFilter[9] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[10] = "";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCasePrelimBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + selTheme;
                break;
            case "23":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[7] = "0"
                parent.GsFilter[8] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[9] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[10] ="N";
                parent.GsFilter[11] = "00000000-0000-0000-0000-000000000000";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCaseFinalBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + selTheme;
                break;
            case "24":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[7] = "0"
                parent.GsFilter[8] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[9] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[10] = "N";
                parent.GsFilter[11] = "00000000-0000-0000-0000-000000000000";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCaseArchiveBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + selTheme;
                break;
        }
    }
    catch (expErr) {
        parent.HideLoad();
        parent.PopupMessage(RootDirectory, strForm, "btnNav_OnClick()", expErr.message, "true");
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


