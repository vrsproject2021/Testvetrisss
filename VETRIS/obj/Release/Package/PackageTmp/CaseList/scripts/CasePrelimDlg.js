var strRowID = "0"; var DEL_FLAG = ""; var WRITE_BACK = "N";
$(document).ready($(function () {
    $("input:text").each(function () {
        $(this).bind("focus", $(this).select());
    })
}));

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

function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('CaseList/VRSCasePrelimDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'CaseList/VRSCasePrelimDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "CaseList/VRSCasePrelimBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}

function NavigateToImgViewer() {
    var URL = objhdnImgVwrURL.value;
    URL = URL.replace("#V1", objhdnSUID.value);
    URL = URL.replace("#V2", PACSUID);
    URL = URL.replace("#V3", PACSPwd);
    parent.NavigatePACS(URL);
}
function NavigateToPACS() {
    var URL = objhdnPACSURL.value;
    URL = URL.replace("#V1", objhdnSUID.value);
    URL = URL.replace("#V2", PACSUID);
    URL = URL.replace("#V3", PACSPwd);

    parent.NavigatePACS(URL);
}
function btnEmail_OnClick() {
    parent.GiWidth = 400;
    parent.GiTop = 15;
    parent.GiHeight = 600;
    parent.GsLaunchURL = "CaseList/VRSMail.aspx?id=" + objhdnID.value + "&uid=" + UserID;
    parent.PopupDataList();
}
function btnSave_OnClick() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var obj = document.getElementById("cke_contents_CKEditor1").children[0];


    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objhdnSUID.value;
        ArrRecords[2] = obj.contentDocument.body.innerHTML;
        ArrRecords[3] = UserID;
        ArrRecords[4] = MenuID;
        AjaxPro.timeoutPeriod = 1800000;
        VRSCasePrelimDlg.SaveReport(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSubmit_OnClick()", expErr.message, "true");
    }

}
function SaveReport(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "false");
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveReport":
            SaveReport(Result);
            break;
    }

}

