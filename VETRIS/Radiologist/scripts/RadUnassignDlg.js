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
    SetPageValues();
    
}
function SetPageValues() {
    var statusID = parseInt(objhdnStatusID.value);
    if (objhdnPrelimRadID.value != "00000000-0000-0000-0000-000000000000") {
        if (statusID <= 50) { objbtnUnassignPrelim.style.display = "inline";}
    }
    if (objhdnFinalRadID.value != "00000000-0000-0000-0000-000000000000") { objbtnUnassignFinal.style.display = "inline"; }

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

function btnUnassign_OnClick(RadType) {
    PopupProcess("Y");
    var ArrRecords = new Array();
    var RadID = "00000000-0000-0000-0000-000000000000";

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = RadType;
        if (RadType == "P") ArrRecords[2] = objhdnPrelimRadID.value; else if (RadType == "F") ArrRecords[2] = objhdnFinalRadID.value;
        ArrRecords[3] = objhdnUserID.value;
        ArrRecords[4] = objhdnMenuID.value;
        ArrRecords[5] = objhdnSessionID.value;

        AjaxPro.timoutPeriod = 1800000;
        VRSRadUnassignDlg.SaveRecord(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "inline-block";
        
    }
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

