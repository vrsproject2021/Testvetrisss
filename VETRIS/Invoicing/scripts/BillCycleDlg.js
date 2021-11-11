var DEL_FLAG = ""; var strRowID = "0";

//$(document).ready($(function () {
//    imgFromDt.setAttribute("onclick", "javascript:SetSelectedDate('CalFromDate','imgFromDt');");
//    imgToDt.setAttribute("onclick", "javascript:SetSelectedDate('CalToDate','imgToDt');");
//}))
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
function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        btnBrwAddUI_Onclick('Invoicing/VRSBillCycleDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Invoicing/VRSBillCycleDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSBillCycleDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSBillCycleDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Invoicing/VRSBillCycleBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}

function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var strFromDt = ""; var strToDt = "";

    if (parent.Trim(objtxtFromDate.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtFromDate.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtFromDate.value, parent.GsDateFormat, parent.GsDateSep);
    }
    if (parent.Trim(objtxtFromDate.value) == "") strToDt = "01Jan1900";
    else {
        if (document.all)
            strToDt = parent.SetDateFormat(objtxtToDate.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strToDt = parent.SetDateFormat1(objtxtToDate.value, parent.GsDateFormat, parent.GsDateSep);
    }

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objtxtName.value;
        ArrRecords[2] = strFromDt;
        ArrRecords[3] = strToDt;
        ArrRecords[4] = UserID;
        ArrRecords[5] = MenuID;

        AjaxPro.timoutPeriod = 1800000;
        VRSBillCycleDlg.SaveRecord(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            objhdnID.value = arrRes[2];
            parent.GsRetStatus = "false";
            break;
    }
}

function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;

    }
}
