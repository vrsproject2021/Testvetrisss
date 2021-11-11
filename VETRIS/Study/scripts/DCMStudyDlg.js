var SUBMITTED = "N"; var ErrFlag = 0;
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    objtxtPID.focus();
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
    SetPageValue();
}
function SetPageValue() {
    if (UserRoleID == "1") document.getElementById("divStudyUID").style.display = "block";
    if (objhdnAppv.value == "Y") {
        objbtnSave1.style.display = "none";
        objbtnSave2.style.display = "none";
        objbtnSubmit1.style.display = "none";
        objbtnSubmit2.style.display = "none";
    }
    CallBackFiles.callback(objhdnID.value);

}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Study/VRSDCMStudyDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Study/VRSDCMStudyDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Study/VRSDCMStudyBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}


function btnSubmit_OnClick(Submitted) {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var strFromDt = "";
    SUBMITTED = Submitted;

    if (parent.Trim(objtxtFromDt.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }


    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objhdnSUID.value;
        ArrRecords[2] = objtxtPID.value;
        ArrRecords[3] = objtxtPFName.value;
        ArrRecords[4] = objtxtPLName.value;
        ArrRecords[5] = strFromDt;
        ArrRecords[6] = Submitted;
        ArrRecords[7] = UserID;
        ArrRecords[8] = MenuID;


        AjaxPro.timeoutPeriod = 1800000;
        VRSDCMStudyDlg.SaveRecord(ArrRecords, ShowProcess);



    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSubmit_OnClick()", expErr.message, "true");
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
            if (SUBMITTED == "Y") {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
                btnClose_OnClick();
            }
            else {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            }
            break;
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
        case "SaveRecord":
            SaveRecord(Result);
            break;
    }

}

