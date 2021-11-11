$(document).ready($(function () {
    window.scrollTo(0, 0);
    window.alert = function () { };
}))
window.alert = function () { };
function view_Searchform() {
    $("#searchSection").slideToggle(800);
    $("#searchIcon").toggleClass("icon", 800);
    if (document.getElementById("searchSection").style.display != "none") CallBackRad.callback();
}
function CheckError() {
    var arrErr = new Array();
    if (parent.Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                if(parent.Trim(arrErr[1]) != "") parent.PopupMessage(RootDirectory, strForm, "OnLoad()", arrErr[1], "true");
                break;
        }
    }
    objhdnError.value = "";
    SetPageValue();
    LoadScheduler();
}
function SetPageValue() {
    if (UserRoleCode == "RDL") {
        document.getElementById("btnCreateCanc").style.display = "none";
        if (objhdnViewSchedule.value == "O") {
            objddlRadiologist.value = objhdnRadID.value;
            objddlRadiologist.disabled = true;
        }
    }
}
function LoadScheduler() {
    var strFromDt = ""; var strTillDt = "";

    if (parent.Trim(objtxtStartDt.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if (parent.Trim(objtxtEndDt.value) == "") strTillDt = "01Jan1900";
    else {
        if (document.all)
            strTillDt = parent.SetDateFormat(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strTillDt = parent.SetDateFormat1(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    //objhdnRADCALSTARTTIME.value
    CallBackSch.callback(strFromDt, strTillDt, objddlRadiologist.value, objhdnRADCALSTARTTIME.value);
}

function SetRadCss()
{
    parent.adjustFrameHeight();
    var arrCnt = new Array();
    var str = "";
    var strHTML = "";

    /******************************************************************************************/
    var strFromDt = ""; var strTillDt = ""; var strDt = "";
    var dtFrom = new Date(); var dt = new Date();
   
    if (document.all)
        strFromDt = parent.SetDateFormat(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);
    else
        strFromDt = parent.SetDateFormat1(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);

    if (document.all)
        strTillDt = parent.SetDateFormat(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);
    else
        strTillDt = parent.SetDateFormat1(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDt = dt.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2);

    if (document.all)
        strDt = parent.SetDateFormat(strDt, parent.GsDateFormat, parent.GsDateSep);
    else
        strDt = parent.SetDateFormat1(strDt, parent.GsDateFormat, parent.GsDateSep);

    dtFrom = new Date(strFromDt);
    dt = new Date(strDt);

    if (dtFrom <= dt) {
        objbtnPrev.style.display = "none";
    }
    else {
        objbtnPrev.style.display = "inline";
    }
    /******************************************************************************************/

    if (document.getElementById("hdnCBCnt") != null) {
        str = document.getElementById("hdnCBCnt").value;
        if (parent.Trim(str) !="") {
            if (str.indexOf(Divider) >= 0) 
                arrCnt = str.split(Divider);
            else
                arrCnt[0] = str;
        }

        for (var i = 0; i < arrCnt.length; i++) {
            var arr = new Array();
            arr = arrCnt[i].split(",");

            if (document.getElementById(arr[0]) != null) {
                var el = document.getElementById(arr[0]).getElementsByTagName("dt");
                el[0].style.backgroundColor = arr[1];
               
            }
        }
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "";
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function chkSel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    while (gridItem = grdRAD.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            if (document.getElementById("chkSel_" + RowId).checked) { gridItem.Data[2] = "Y"; }
            else { gridItem.Data[2] = "N"; }
            parent.GsRetStatus = "true";
            break;
        }
        itemIndex++;
    }
}
function chkNext_OnClick() {
    if (objchkNext.checked) {
        objtxtDays.readOnly = "";
        objtxtDays.value = "1";
        objtxtTillDt.readOnly = "readOnly";
        objimgTill.style.display = "none";
    }
    else {
        objtxtDays.readOnly = "readOnly";
        objtxtDays.value = "0";
        objtxtTillDt.readOnly = "";
        objimgTill.style.display = "inline";
    }
}
function btnCreate_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrRad = new Array();
    var arrWeek = new Array();
    var strFromDt = ""; var strFromHr = "";
    var strTillDt = ""; var strTillHr = "";

    if (parent.Trim(objtxtFromDt.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if (parent.Trim(objtxtTillDt.value) == "") strTillDt = "01Jan1900";
    else {
        if (document.all)
            strTillDt = parent.SetDateFormat(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strTillDt = parent.SetDateFormat1(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if ((objddlFromTT.value == "PM") && (parseInt(objddlFromHr.value) < 12)) strFromHr = parent.padZeroPlaces(12 + parseInt(objddlFromHr.value));
    else if ((objddlFromTT.value == "AM") && (parseInt(objddlFromHr.value) == 12)) strFromHr = "00";
    else strFromHr = objddlFromHr.value;

    if ((objddlTillTT.value == "PM") && (parseInt(objddlTillHr.value) < 12)) strTillHr = parent.padZeroPlaces(12 + parseInt(objddlTillHr.value));
    else if ((objddlTillTT.value == "AM") && (parseInt(objddlTillHr.value) == 12)) strTillHr = "00";
    else strTillHr = objddlTillHr.value;

    try {

        ArrRecords[0] = strFromDt;
        ArrRecords[1] = strTillDt;
        ArrRecords[2] = "N"; if (objchkNext.checked) ArrRecords[2] = "Y";
        ArrRecords[3] = objtxtDays.value;
        ArrRecords[4] = strFromHr + ":" + objddlFromMin.value + ":00";
        ArrRecords[5] = strTillHr + ":" + objddlTillMin.value + ":00";
        ArrRecords[6] = UserID;
        ArrRecords[7] = MenuID;

        arrRad = GetRadiologists();
        arrWeek = GetWeekdays();


        AjaxPro.timeoutPeriod = 1800000;
        VRSSchedule.CreateSchedule(ArrRecords, arrRad, arrWeek, ShowProcess);



    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnCreate_OnClick()", expErr.message, "true");
    }
}
function GetRadiologists() {
    var itemIndex = 0; var gridItem;
    var arrRad = new Array(); var idx = 0; var sel = "";

    while (gridItem = grdRAD.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[2].toString();
        if (sel == "Y") {
            arrRad[idx] = gridItem.Data[1].toString();
            idx = idx + 1;
        }
        itemIndex++;
    }


    return arrRad;
}
function GetWeekdays() {
    var arrWeek = new Array(); var idx = 0;
    if (objchkMon.checked) { arrWeek[idx] = "1"; idx = idx + 1; }
    if (objchkTue.checked) { arrWeek[idx] = "2"; idx = idx + 1; }
    if (objchkWed.checked) { arrWeek[idx] = "3"; idx = idx + 1; }
    if (objchkThu.checked) { arrWeek[idx] = "4"; idx = idx + 1; }
    if (objchkFri.checked) { arrWeek[idx] = "5"; idx = idx + 1; }
    if (objchkSat.checked) { arrWeek[idx] = "6"; idx = idx + 1; }
    if (objchkSun.checked) { arrWeek[idx] = "7"; idx = idx + 1; }
    return arrWeek;
}
function ProcessSchedule(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessSchedule()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessSchedule()", arrRes[1], "true");
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ProcessSchedule()", arrRes[1], "false");
            view_Searchform();
            LoadScheduler();
            break;
    }
}
function btnCancel_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrRad = new Array();
    var arrWeek = new Array();
    var strFromDt = ""; var strFromHr = "";
    var strTillDt = ""; var strTillHr = "";


    if (parent.Trim(objtxtFromDt.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if (parent.Trim(objtxtTillDt.value) == "") strTillDt = "01Jan1900";
    else {
        if (document.all)
            strTillDt = parent.SetDateFormat(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strTillDt = parent.SetDateFormat1(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if ((objddlFromTT.value == "PM") && (parseInt(objddlFromHr.value) < 12)) strFromHr = parent.padZeroPlaces(12 + parseInt(objddlFromHr.value));
    else strFromHr = objddlFromHr.value;

    if ((objddlTillTT.value == "PM") && (parseInt(objddlTillHr.value) < 12)) strTillHr = parent.padZeroPlaces(12 + parseInt(objddlTillHr.value));
    else strTillHr = objddlTillHr.value;

    try {

        ArrRecords[0] = strFromDt;
        ArrRecords[1] = strTillDt;
        ArrRecords[2] = "N"; if (objchkNext.checked) ArrRecords[2] = "Y";
        ArrRecords[3] = objtxtDays.value;
        ArrRecords[4] = strFromHr + ":" + objddlFromMin.value + ":00";
        ArrRecords[5] = strTillHr + ":" + objddlTillMin.value + ":00";
        ArrRecords[6] = UserID;
        ArrRecords[7] = MenuID;

        arrRad = GetRadiologists();
        arrWeek = GetWeekdays();


        AjaxPro.timeoutPeriod = 1800000;
        VRSSchedule.CancelSchedule(ArrRecords, arrRad, arrWeek, ShowProcess);



    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnCreate_OnClick()", expErr.message, "true");
    }
}
function btnReset_OnClick() {
    var strDtFrom = ""; var strDtTill = "";
    var dtFrom = new Date(); var dtTill = new Date();
    var strTillDt = ""; var strTillHr = "";
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdRAD.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        gridItem.Data[2] = "N";
        if (document.getElementById("chkSel_" + RowId) != null) {
            document.getElementById("chkSel_" + RowId).checked = false;
        }
        itemIndex++;
    }


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

    strFromHr = parent.padZeroPlaces(dtFrom.getHours());
    if (parseInt(strFromHr) < 12) objddlFromTT.value = "AM"; else objddlFromTT.value = "PM";
    if (parseInt(strFromHr) > 12) strFromHr = parent.padZeroPlaces(parseInt(strFromHr) - 12);

    strTillHr = parent.padZeroPlaces(dtTill.getHours());
    if (parseInt(strTillHr) < 12) objddlTillTT.value = "AM"; else objddlTillTT.value = "PM";
    if (parseInt(strTillHr) > 12) strTillHr = parent.padZeroPlaces(parseInt(strTillHr) - 12);

   
    objtxtFromDt.value = strDtFrom;
    objtxtTillDt.value = strDtTill; objtxtTillDt.readOnly = ""; objimgTill.style.display = "inline";
    objchkNext.checked = false;
    objtxtDays.readOnly = "readOnly";
    objtxtDays.value = "0";
    objddlFromHr.value = strFromHr;
    objddlTillHr.value = strTillHr;

    objchkMon.checked = false;
    objchkTue.checked = false;
    objchkWed.checked = false;
    objchkThu.checked = false;
    objchkFri.checked = false;
    objchkSat.checked = false;
    objchkSun.checked = false;
}
function btnPrev_OnClick() {
    var strFromDt = ""; var strTillDt = "";
    var dtFrom = new Date(); var dtTill = new Date();
    var dt = new Date(); var strDt = "";

    if (document.all)
        strFromDt = parent.SetDateFormat(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);
    else
        strFromDt = parent.SetDateFormat1(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);

    if (document.all)
        strTillDt = parent.SetDateFormat(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);
    else
        strTillDt = parent.SetDateFormat1(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);

    dtFrom = new Date(strFromDt);
    dtTill = new Date(strTillDt);
    dtFrom.addDays(-1);
    dtTill.addDays(-1);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strFromDt = parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strFromDt = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strFromDt = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strFromDt = dtFrom.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strTillDt = parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strTillDt = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strTillDt = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strTillDt = dtTill.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDt = dt.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2);

    if (document.all)
        strDt = parent.SetDateFormat(strDt, parent.GsDateFormat, parent.GsDateSep);
    else
        strDt = parent.SetDateFormat1(strDt, parent.GsDateFormat, parent.GsDateSep);

    objtxtStartDt.value = strFromDt;
    objtxtEndDt.value = strTillDt;
    dt = new Date(strDt);

    if (dtFrom >= dt) {
        LoadScheduler();
    }
    else {
        objbtnPrev.style.display = "none";
    }
    
}
function btnNext_OnClick() {
    var strFromDt = ""; var strTillDt = "";
    var dtFrom = new Date(); var dtTill = new Date();

    if (document.all)
        strFromDt = parent.SetDateFormat(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);
    else
        strFromDt = parent.SetDateFormat1(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);

    if (document.all)
        strTillDt = parent.SetDateFormat(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);
    else
        strTillDt = parent.SetDateFormat1(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);

    dtFrom = new Date(strFromDt);
    dtTill = new Date(strTillDt);
    dtFrom.addDays(1);
    dtTill.addDays(1);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strFromDt = parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strFromDt = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strFromDt = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strFromDt = dtFrom.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strTillDt = parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strTillDt = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strTillDt = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strTillDt = dtTill.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2);

    objtxtStartDt.value = strFromDt;
    objtxtEndDt.value = strTillDt;

    LoadScheduler();
}
function DateChanged() {
    var strFromDt = ""; var strTillDt = ""; var strDt = "";
    var dtFrom = new Date(); var dtTill = new Date(); var dt = new Date();
    var strErr="";

    if (document.all)
        strFromDt = parent.SetDateFormat(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);
    else
        strFromDt = parent.SetDateFormat1(objtxtStartDt.value, parent.GsDateFormat, parent.GsDateSep);

    if (document.all)
        strTillDt = parent.SetDateFormat(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);
    else
        strTillDt = parent.SetDateFormat1(objtxtEndDt.value, parent.GsDateFormat, parent.GsDateSep);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strDt = parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2) + parent.objhdnDateSep.value + dt.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDt = dt.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dt.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dt.getDate(), 2);

    if (document.all)   
        strDt = parent.SetDateFormat(strDt, parent.GsDateFormat, parent.GsDateSep);
    else
        strDt = parent.SetDateFormat1(strDt, parent.GsDateFormat, parent.GsDateSep);

    dtFrom = new Date(strFromDt);
    dtTill = new Date(strTillDt);
    dt = new Date(strDt);

    if (dtFrom.getFullYear() == 1900) strErr = "214";
    if (dtTill.getFullYear() == 1900) { if (parent.Trim(strErr) != "") strErr += Divider; strErr = "215"; }
    if (dtFrom > dtTill) { if (parent.Trim(strErr) != "") strErr += Divider; strErr = "216"; }

    if (parent.Trim(strErr) != "") {
        parent.PopupMessage(RootDirectory, strForm, "DateChanged()", strErr, "true");
    }
    else {
        LoadScheduler();
    }

}

function ShowScheduleDialog(sender, eventArgs, Title) {
    var dtFrom = new Date(); var dtTill = new Date();
    var strDtFrom = ""; var strDtTill = ""; var strDateFmt = "";
    var appointmentInstance = eventArgs.AppointmentInstance;
    var ID = appointmentInstance.get_appointmentID();
    var RadiologistID = appointmentInstance.get_tag();
    var Radiologist = appointmentInstance.get_title();
    var StartDt = appointmentInstance.get_period().get_startDateTime();
    var EndDt = appointmentInstance.get_period().get_endDateTime();
    var Desc = appointmentInstance.get_description();
    var DescTime = parent.Trim(Desc.substring(0, 15));

    if (UserRoleCode != "RDL") {
        Desc = parent.Trim(Desc.replace(DescTime, ''));

        if (ID == undefined) ID = "00000000-0000-0000-0000-000000000000";
        if (RadiologistID == undefined) RadiologistID = "00000000-0000-0000-0000-000000000000";
        dtFrom = new Date(StartDt);
        dtTill = new Date(EndDt);

        strDateFmt = "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy";
        strDtFrom = parent.padZeroPlaces(dtFrom.getDate()) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1)) + parent.objhdnDateSep.value + dtFrom.getFullYear();
        strDtTill = parent.padZeroPlaces(dtTill.getDate()) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1)) + parent.objhdnDateSep.value + dtTill.getFullYear();

        strDtFrom = parent.SetDateFormat(strDtFrom, strDateFmt, parent.objhdnDateSep.value); strDtFrom += " " + parent.padZeroPlaces(dtFrom.getHours()) + ":" + parent.padZeroPlaces(dtFrom.getMinutes());
        strDtTill = parent.SetDateFormat(strDtTill, strDateFmt, parent.objhdnDateSep.value); strDtTill += " " + parent.padZeroPlaces(dtTill.getHours()) + ":" + parent.padZeroPlaces(dtTill.getMinutes());

        parent.GsText = Desc;
        parent.GsLaunchURL = "Radiologist/VRSScheduleUpdate.aspx?id=" + ID + "&radid=" + RadiologistID + "&sdt=" + strDtFrom + "&edt=" + strDtTill + "&ttl=" + Title + "&uid=" + UserID + "&mid=" + MenuID + "&th=" + selTheme;
        parent.PopupGeneral();
    }
}
function ProcessGeneral(Args) {
    if (Args == "Y") {
        LoadScheduler();
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
        case "CalStart":
            CalStart.setSelectedDate(dt); CalStart.show();
            break;
        case "CalEnd":
            CalEnd.setSelectedDate(dt); CalEnd.show();
            break;
    }


}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}
function ResetValueInteger(objCtrlID, strDefaultValue) {
    var objCtrl;
    objCtrl = document.getElementById(objCtrlID.id);
    if (objCtrl == null) objCtrl = objCtrlID;
    if (strDefaultValue != null) {
        if (objCtrl.value == "" || parseInt(objCtrl.value) <= 0) {
            objCtrl.value = strDefaultValue;
        }
    }
    else {
        if (objCtrl.value == "") objCtrl.value = "0";
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "CreateSchedule":
        case "CancelSchedule":
            ProcessSchedule(Result);
            break;
    }

}