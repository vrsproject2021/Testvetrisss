var GRID_SYNCH = "";

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
    //btnSearch_OnClick();
}

function btnClose_OnClick() {
    parent.LoadHome();
}
function btnSynch_OnClick() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    if (Validate()) {
        try {
            GRID_SYNCH = "N";
            ArrRecords[0] = objtxtSUID.value;
            if (objhdnAPIVER.value == "7.2") {
                ArrRecords[1] = objhdnURL.value;
                ArrRecords[2] = objhdnPACSUserID.value;
                ArrRecords[3] = objhdnPACSPwd.value;
                ArrRecords[4] = UserID;
                ArrRecords[5] = MenuID;
            }
            else if (objhdnAPIVER.value == "8") {
                ArrRecords[1] = objhdnWS8SRVIP.value;
                ArrRecords[2] = objhdnWS8CLTIP.value;
                ArrRecords[3] = objhdnWS8SRVUID.value;
                ArrRecords[4] = objhdnWS8SRVPWD.value;
                ArrRecords[5] = WSSessionID;
                ArrRecords[6] = UserID;
                ArrRecords[7] = MenuID;
            }
           

            AjaxPro.timeoutPeriod = 1800000;
            if (objhdnAPIVER.value == "7.2") VRSMissingStudy.SynchStudy_72(ArrRecords, ShowProcess);
            else if (objhdnAPIVER.value == "8") VRSMissingStudy.SynchStudy_80(ArrRecords, ShowProcess);
        }
        catch (expErr) {
            parent.HideProcess();
            parent.PopupMessage(RootDirectory, strForm, "btnSynch_OnClick()", expErr.message, "true");
        }
    }

}
function Validate() {
    var strErr = "";
    var bRet = true;

    if (parent.Trim(objtxtSUID.value) == "") {
        strErr = "161"
    }
    if (parent.Trim(objhdnPACSUserID.value) == "") {
        if (strErr != "") strErr = strErr + Divider;
        strErr += "159"
    }
    if (parent.Trim(objhdnPACSPwd.value) == "") {
        if (strErr != "") strErr = strErr + Divider;
        strErr += "160"
    }
    if (parent.Trim(objhdnURL.value) == "") {
        if (strErr != "") strErr = strErr + Divider;
        strErr += "158"
    }

    if (parent.Trim(strErr) != "") {
        bRet = false;
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSynch_OnClick()", strErr, "true");
    }

    return bRet;
}
function ProcessSynch(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessSynch()", arrRes[1], "true");
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ProcessSynch()", arrRes[1], "false");
            btnSearch_OnClick();
            break;
    }

    parent.GsRetStatus = "false";
}

function btnSearch_OnClick() {
    var ArrParams = new Array();
    var strDtFrom = ""; var strDtTill = "";

    if (ValidateSearch()) {

        if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
        else {
            if (document.all)
                strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
            else
                strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        }

        if (parent.Trim(objtxtTillDt.value) == "") strDtTill = "01Jan1900";
        else {
            if (document.all)
                strDtTill = parent.SetDateFormat(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
            else
                strDtTill = parent.SetDateFormat1(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
        }

        if (objhdnAPIVER.value == "7.2") {
            ArrParams[0] = objhdnStudyCheckURL.value;
            ArrParams[1] = objhdnPACSUserID.value;
            ArrParams[2] = objhdnPACSPwd.value;
            ArrParams[3] = strDtFrom;
            ArrParams[4] = strDtTill;
            ArrParams[5] = objhdnAPIVER.value;
        }
        else if (objhdnAPIVER.value == "8") {
            ArrParams[0] = objhdnWS8URLMSK.value;
            ArrParams[1] = objhdnWS8CLTIP.value;
            ArrParams[2] = objhdnWS8SRVUID.value;
            ArrParams[3] = objhdnWS8SRVPWD.value;
            ArrParams[4] = WSSessionID;
            ArrParams[5] = strDtFrom;
            ArrParams[6] = strDtTill;
            ArrParams[7] = objhdnAPIVER.value;
        }
        

        CallBackBrw.callback(ArrParams);

    }
}
function ValidateSearch() {
    var strErr = "";
    var bRet = true;


    if (parent.Trim(objhdnPACSUserID.value) == "") {
        strErr += "159"
    }
    if (parent.Trim(objhdnPACSPwd.value) == "") {
        if (strErr != "") strErr = strErr + Divider;
        strErr += "160"
    }
    if (parent.Trim(objhdnStudyCheckURL.value) == "") {
        if (strErr != "") strErr = strErr + Divider;
        strErr += "158"
    }

    if (parent.Trim(strErr) != "") {
        bRet = false;
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSynch_OnClick()", strErr, "true");
    }

    return bRet;
}
function btnSynch_Grid_OnClick(ID, StudyUID) {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {
        GRID_SYNCH = "Y";
        ArrRecords[0] = StudyUID;
        if (objhdnAPIVER.value == "7.2") {
            ArrRecords[1] = objhdnURL.value;
            ArrRecords[2] = objhdnPACSUserID.value;
            ArrRecords[3] = objhdnPACSPwd.value;
        }
        else if (objhdnAPIVER.value == "8") {
            ArrRecords[1] = objhdnWS8SRVIP.value;
            ArrRecords[2] = objhdnWS8CLTIP.value;
            ArrRecords[3] = objhdnWS8SRVUID.value;
        }
        ArrRecords[4] = UserID;
        ArrRecords[5] = MenuID;

        AjaxPro.timeoutPeriod = 1800000;
        if (objhdnAPIVER.value == "7.2") VRSMissingStudy.SynchStudy_72(ArrRecords, ShowProcess);
        else if (objhdnAPIVER.value == "8") VRSMissingStudy.SynchStudy_80(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSynch_Grid_OnClick()", expErr.message, "true");
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
        case "SynchStudy_72":
        case "SynchStudy_80":
            ProcessSynch(Result);
            break;
    }

}

