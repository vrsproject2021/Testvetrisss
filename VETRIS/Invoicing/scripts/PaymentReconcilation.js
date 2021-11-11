
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
function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objtxtFromDate.value;
    ArrRecords[1] = objtxtToDate.value;
    ArrRecords[2] = UserID;
    ArrRecords[3] = MenuID;
    CallBackBrw.callback(ArrRecords);
}
function ResetRecord() {
    objtxtFromDate.value = "";
    objtxtToDate.value = "";
    SearchRecord();
}
function btnSearch_OnClick() {
    SearchRecord();
}
function btnExcel_OnClick() {
    var ArrRecords = new Array();
    try {
        parent.parent.PopupProcess("N");

        ArrRecords[0] = objtxtFromDate.value;
        ArrRecords[1] = objtxtToDate.value;
        ArrRecords[2] = UserID;
        ArrRecords[3] = MenuID;
        AjaxPro.timeoutPeriod = 1800000;
        VRSOnlinePmtReconcile.GenerateExcel(ArrRecords, ShowProcess);

    }
    catch (expErr) {
        parent.parent.HideProcess();
        parent.parent.PopupMessage(RootDirectory, strForm, "btnExcel_OnClick()", expErr.message, "true");
    }

}
function btnClose_OnClick() {
    Unlock = "N";
    btnBrwClose_Onclick();
}
function ShowProcess(Result, MethodName) {
    parent.parent.HideProcess();
    var strMethod = MethodName.method;
    debugger;
    switch (strMethod) {
        case "GenerateExcel":
            ProcessReport(Result);
            break;
    }
}
function ProcessReport(Result) {
    var arrRet = new Array();
    arrRet = Result.value;
    switch (arrRet[0]) {
        case "catch":
            parent.parent.PopupMessage(RootDirectory, strForm, "ProcessReport()", arrRet[1], "true");
            break;
        case "false":
            parent.parent.PopupMessage(RootDirectory, strForm, "ProcessReport()", arrRet[1], "true");
            break;
        case "true":
            parent.parent.GsLaunchURL = arrRet[1];
            parent.parent.GsFilePath = arrRet[2];
            parent.parent.GsFileType = "XLS";
            parent.parent.PopupReportViewer();
            break;
    }
}
function SetSelectedDate(CalName, objImgID) {
    var strDate = ""; var strClass = "";
    var dt;
    objCtrl = document.getElementById(CalName); if (objCtrl == null) objCtrl = CalName;
    strDate = document.getElementById(objCtrl.id).value;

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
    switch (objImgID) {
        case "CalFromDate":
            CalFromDate.setSelectedDate(dt); CalFromDate.show();
            break;
        case "CalToDate":
            CalToDate.setSelectedDate(dt); CalToDate.show();

            break;

    }


    parent.GsRetStatus = "true";
}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}
