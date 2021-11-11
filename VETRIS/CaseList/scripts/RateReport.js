var strRowID = "0"; var DEL_FLAG = ""; var WRITE_BACK = "N"; var RPT_RATING = "N"; var ErrFlag = 0; 
var LoggedRadiologistID = parent.objhdnRadiologistID.value;

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
    parent.adjustFrameHeight();
    SetPageValue();
}
function SetPageValue() {
    var RadFnRights = objhdnRadFnRights.value;

    if (UserRoleCode == "RDL") {
        if ((RadFnRights.indexOf("DICTRPT") == -1)) {
            document.getElementById("divStatDict").style.display = "none";
            document.getElementById("spnStatDict").style.display = "none";
        }
        if (RadFnRights.indexOf("UPDPRELIMRPT") == -1) {
            document.getElementById("divStatPrelim").style.display = "none";
            document.getElementById("spnStatPrelim").style.display = "none";
            //document.getElementById("divTeach").style.display = "block";
        }
        if (RadFnRights.indexOf("UPDFINALRPT") == -1) {
            document.getElementById("divStatFinal").style.display = "none";
            document.getElementById("spnStatFinal").style.display = "none";
        }
        if ((RadFnRights.indexOf("DICTRPT") == -1) && (RadFnRights.indexOf("UPDPRELIMRPT") == -1) && (RadFnRights.indexOf("UPDFINALRPT") == -1)) {
            document.getElementById("divStat").style.display = "none";
        }
        

        if ((RadFnRights.indexOf("VWLOCKSTUDY") > -1) || (RadFnRights.indexOf("ACCLOCKSTUDY") > -1)) {
            if (objhdnAssnRadID.value != "00000000-0000-0000-0000-000000000000") {
                if (objhdnAssnRadID.value != LoggedRadiologistID) {
                    if (RadFnRights.indexOf("ACCLOCKSTUDY") == -1) {
                        objbtnSave1.style.display = "none";
                        objbtnSave2.style.display = "none";
                    }
                }
            }
        }
        if (RadFnRights.indexOf("RATEOTHRPT") > -1) {
            if (LoggedRadiologistID != objhdnAssnRadID.value) {
                if (objhdnAssnRadID.value != "00000000-0000-0000-0000-000000000000") {
                    RPT_RATING = "Y";
                    document.getElementById("divRateRpt").style.display = "block";
                }
                else
                    objrdoNormal.checked = true;
            }
            else
                objrdoNormal.checked = true;
        }
        else
            objrdoNormal.checked = true;
        $('#ddlDisclReason').prop('disabled', true);
        $('#txtRptDisclText').prop('readonly', true);
        $('#txtReport').prop('readonly', true);

    }
    else {
        objbtnSave1.style.display = "none";
        objbtnSave2.style.display = "none";
    }

    if (objhdnCurrStatusID.value == "60") {
        objrdoDict.disabled = true;
        objrdoDict.checked = true;
    }

    Rating_OnClick();

    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();
    LoadStudyTypes();
    CallBackDoc.callback(objhdnID.value, UserID);
    CallBackPS.callback(objhdnID.value, objhdnPName.value, objhdnInstID.value, UserID);

}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('CaseList/VRSRateReportDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'CaseList/VRSRateReportDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "CaseList/VRSCaseFinalRptBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false") {
        parent.FetchMenuRecordCount();
        btnDlgClose_Onclick();
    }
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function createGuid() {
    //return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function ShowDocument(ID, FileName) {
    var strFilePath = "/" + RootDirectory + "/CaseList/Temp/" + UserID + "/" + FileName;
    var win = window.open(strFilePath, '_blank');
    win.focus();
}
function Rating_OnClick() {
    if ($('#hdnAbnormalReportId').val() != '00000000-0000-0000-0000-000000000000') {
        objrdoAbnormal.checked = true;
        objddlAbRptReason.value = $('#hdnAbnormalReportId').val();
        objddlAbRptReason.disabled = false;
    }

    if (objrdoNormal.checked) {
        objddlAbRptReason.value = "00000000-0000-0000-0000-000000000000";
        objddlAbRptReason.disabled = true;
    }
    else if (objrdoAbnormal.checked)
        objddlAbRptReason.disabled = false;
}
function btnSave_OnClick() {
   
    var ArrRecords = new Array();
    
    try {

        
            parent.PopupProcess("Y");
            ArrRecords[0] = objhdnRptID.value;
            ArrRecords[1] = objhdnID.value;
            ArrRecords[2] = objddlAbRptReason.value;
            ArrRecords[3] = UserID;
            ArrRecords[4] = MenuID;
            ArrRecords[5] = SessionID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSRateReportDlg.SaveReRate(ArrRecords, ShowProcess);

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }

}
function SaveReRate(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "296" || arrRes[1] == "297") {
                parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true", arrRes[2]);
                parent.GsRetStatus = "false";
                btnClose_OnClick();
            }
            else
                parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "SaveReport()", arrRes[1], "false");
            break;
    }
}
function LoadStudyTypes() {
    CallBackSelST.callback(objhdnID.value, objhdnModalityID.value);
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveReRate":
            SaveReRate(Result);
            break;
    }

}


