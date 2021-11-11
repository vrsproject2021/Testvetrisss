$(document).ready($(function () {
    LoadRecord();
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
function LoadRecord() {
    //parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] =  UserID;
        ArrRecords[1] = MenuID;
        CallBackChargesDiscount.callback(ArrRecords);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "LoadRecord()", expErr.message, "true");
    }
}

function txtDiscount_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdChargesDiscount.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtDiscount_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}

function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

}

function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrParams = new Array();
    try {

        ArrRecords= GetChargesDiscount(); 
        ArrParams[0]= UserID;
        ArrParams[1] = MenuID;


        AjaxPro.timoutPeriod = 1800000;
        VRSChargesDiscount.SaveRecord(ArrRecords, ArrParams, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
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
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            SearchRecord();
            break;
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "VRSHome.aspx";
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("030");
    }
}
function GetChargesDiscount() {
    var gridItem;
    var itemIndex   = 0;
    var RowID       = "00000000-0000-0000-0000-000000000000";
    var disc         = 0;
    var idx         = 0;
    var arrRecords = new Array();

    while (gridItem = grdChargesDiscount.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        disc = parseFloat(gridItem.Data[3].toString());
        if (disc > 0) {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[1].toString();
            arrRecords[idx + 2] = gridItem.Data[3].toString();
            idx = idx + 3;
        }

        itemIndex++;
    }
    return arrRecords;
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
