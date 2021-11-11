var strRowID = "0"; 

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
    
    CallBackDoc.callback(objhdnID.value, UserID);
    CallBackST.callback(objhdnID.value, objhdnModalityID.value);
}
function btnClose_OnClick() {
    parent.GsNavURL = "HouseKeeping/VRSStudyAuditTrailBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function btnRefreshCount_OnClick() {
    parent.PopupProcess("N");
    var strSUID = ""; var strURL = "";

    try {
        strSUID = objhdnSUID.value;
        strURL = objhdnImgCntURL.value.replace("qf_SYUI=", "qf_SYUI=" + strSUID);
        strURL = strURL.replace("#V2", PACSUID);
        strURL = strURL.replace("#V3", PACSPwd);

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseRADlg.GetImageCount(strURL, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnRefreshCount_OnClick()", expErr.message, "true");
    }
}
function GetImageCount(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "GetImageCount()", arrRes[1], "true");
            break;
        case "true":
            objtxtImgCnt.value = arrRes[1];
            break;
    }

}
function ShowDocument(ID, FileName) {
    var strFilePath = "/" + RootDirectory + "/HouseKeeping/Temp/" + UserID + "/" + FileName;
    var win = window.open(strFilePath, '_blank');
    win.focus();
}

