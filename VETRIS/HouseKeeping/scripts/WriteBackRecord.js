
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
    LoadStudies();
}
function LoadStudies() {
    var ArrParams = new Array();
    var strDtFrom = ""; var strDtTill = "";


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

       
        ArrParams[0] = strDtFrom;
        ArrParams[1] = strDtTill;
        CallBackBrw.callback(ArrParams);


}
function btnClose_OnClick() {
    parent.LoadHome();
}
function btnSearch_OnClick() {
    LoadStudies();
}
function UpdateWriteBack(ID,WriteBack) {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {
        ArrRecords[0] = ID;
        ArrRecords[1] = WriteBack;
        AjaxPro.timeoutPeriod = 1800000;
        VRSWriteBackRecord.UpdateWriteBack(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnWB_OnClick()", expErr.message, "true");
    }

}
function ProcessWriteBack(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessWriteBack()", arrRes[1], "true");
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ProcessWriteBack()", arrRes[1], "false");
            btnSearch_OnClick();
            break;
    }

    parent.GsRetStatus = "false";
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
        case "UpdateWriteBack":
            ProcessWriteBack(Result);
            break;
    }

}

