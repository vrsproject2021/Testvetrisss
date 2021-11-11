$(document).ready($(function () {
    parent.window.scrollTo(0, 0);
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
}
function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objtxtPatientName.value = parent.GsFilter[0];
        objtxtModality.value = parent.GsFilter[1];
        objddlInstitution.value = parent.GsFilter[2];
        if (parent.GsFilter[3] == "Y") objchkStudyDt.checked = true;
        objtxtFromDt.value = parent.GsFilter[4];
        objtxtTillDt.value = parent.GsFilter[5];
    }
    else
        parent.GsFilter.length = 0;
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
    ArrRecords[1] = objtxtModality.value;
    ArrRecords[2] = objddlInstitution.value;
    ArrRecords[3] = "N"; if (objchkStudyDt.checked) ArrRecords[2] = "Y";
    ArrRecords[4] = strDtFrom;
    ArrRecords[5] = strDtTill;
    ArrRecords[6] = UserID;

    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
   // view_Searchform();
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtPatientName.value);
    parent.GsFilter[1] = parent.Trim(objtxtModality.value);
    parent.GsFilter[2] = parent.Trim(objddlInstitution.value);
    if (objchkStudyDt.checked) parent.GsFilter[3] = "Y"; else parent.GsFilter[3] == "N";
    parent.GsFilter[4] = objtxtFromDt.value;
    parent.GsFilter[5] = objtxtTillDt.value;
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
    objtxtModality.value = "";
    objchkStudyDt.checked = false;
    objtxtFromDt.value = strDtFrom;
    objtxtTillDt.value = strDtTill;
    SearchRecord();
}
function btnEdit_OnClick(ID) {
    objhdnID.value = ID;
    btnBrwEditUI_Onclick("HouseKeeping/VRSDeleteStudyDlg.aspx");
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