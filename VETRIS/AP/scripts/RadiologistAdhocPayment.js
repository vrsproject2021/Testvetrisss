var strRowID = "0"; var DEL_FLAG = ""; var WRITE_BACK = "N"; var ErrFlag = 0;
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    parent.adjusDataListFrameHeight();
    CheckError();
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
    parent.adjusDataListFrameHeight();
    CallBackPmt.callback(objhdnRadID.value, objhdnCycleID.value);
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Radiologist/VRSRadUnassignDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Radiologist/VRSRadUnassignDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {

    parent.HideDataList();
}
function txtAmt_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdPmt.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("txtAmt_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtRemarks_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdPmt.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtRemarks_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function btnSave_OnClick() {
    PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrPmts = new Array();

    try {

        ArrRecords[0] = objhdnRadID.value;
        ArrRecords[1] = objhdnCycleID.value;
        ArrRecords[2] = objhdnUserID.value;
        ArrRecords[3] = objhdnMenuID.value;
        ArrPmts = GetPayment();

        AjaxPro.timoutPeriod = 1800000;
        VRSRadiologistAdhocPayment.SaveRecord(ArrRecords,ArrPmts, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "inline-block";
        
    }
}
function GetPayment() {
    var itemIndex = 0; var gridItem;
    var arrPmt = new Array();
    var idx = 0;

    while (gridItem = grdPmt.get_table().getRow(itemIndex)) {
        arrPmt[idx] = gridItem.Data[0].toString();
        arrPmt[idx + 1] = gridItem.Data[2].toString();
        arrPmt[idx + 2] = gridItem.Data[3];
        idx = idx + 3;

        itemIndex++;
    }

    return arrPmt;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    objlblMsg.style.color = "black";
    document.getElementById("divMsg").style.display = "inline-block";
    objlblMsg.innerHTML = "";

    switch (arrRes[0]) {
        case "catch":
        case "false":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            parent.HideDataList("Y");
            break;
    }

}
function PopupProcess(IsSaving) {
    if (IsSaving == null) IsSaving = "N";
    var sUrl = "";
    if (IsSaving == "N") sUrl = "../htmls/processing.html";
    else sUrl = "../htmls/saving.html";

    $('#tblProcess').surfOverlay('ps', { url: sUrl, zIndex: 3000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });

    return false;
}
function HideProcess() {

    //$('#tblProcess').surfOverlay('ps', { zIndex: 100 });
    closepopup('ps');
}
function ShowProcess(Result, MethodName) {
    HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
    }

}
function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

}

