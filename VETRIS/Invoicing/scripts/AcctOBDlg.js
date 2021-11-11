var DEL_FLAG = ""; var strRowID = "0";

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
        btnBrwAddUI_Onclick('Invoicing/VRSAcctOBDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Invoicing/VRSAcctOBDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSAcctOBDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSAcctOBDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Invoicing/VRSAcctOB.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
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
    var strOpBalDt = "";

    if (parent.Trim(objtxtOpBalDate.value) == "") strOpBalDt = "01Jan"+objddlYear.value;
    else {
        if (document.all)
            strOpBalDt = parent.SetDateFormat(objtxtOpBalDate.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strOpBalDt = parent.SetDateFormat1(objtxtOpBalDate.value, parent.GsDateFormat, parent.GsDateSep);
    }
    

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objddlAccount.value;
        ArrRecords[2] = strOpBalDt;
        ArrRecords[3] = objtxtInvoiceNo.value;
        ArrRecords[4] = objtxtAmount.value;
        ArrRecords[5] = UserID;
        ArrRecords[6] = MenuID;

        AjaxPro.timoutPeriod = 1800000;
        VRSAcctOBDlg.SaveRecord(ArrRecords, ShowProcess);
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
            btnClose_OnClick();

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
function ResetValueDecimal(objCtrlID, decimalPlace) {

    var objCtrl;
    if (decimalPlace == null) decimalPlace = parent.GiDecimal;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value.replace("-", ""), decimalPlace);

}