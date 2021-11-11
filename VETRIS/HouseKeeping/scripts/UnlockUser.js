
function view_Searchform() {
    $("#searchSection").slideToggle(800);
    $("#searchIcon").toggleClass("icon", 800);
}
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
    SearchRecord();
}
function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objtxtCode.value = parent.GsFilter[0];
        objtxtName.value = parent.GsFilter[1];
        
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    CallBackBrw.callback(objhdnRights.value, objtxtCode.value, objtxtName.value, UserID);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtCode.value);
    parent.GsFilter[1] = parent.Trim(objtxtName.value);
}
function ResetRecord() {
    objtxtCode.value = "";
    objtxtName.value = "";
    SearchRecord();
}
function UnlockUser(ID,SessID) {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    try {
        ArrRecords[0] = ID;
        ArrRecords[1] = SessID;
        ArrRecords[2] = UserID;
        ArrRecords[3] = SessionID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSUnlockUser.UserUnlock(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "UnlockUser()", expErr.message, "true");
    }
}
function ProcessUserUnlock(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessUserUnlock()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessUserUnlock()", arrRes[1], "true");
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ProcessUserUnlock()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            SearchRecord();
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "UserUnlock":
            ProcessUserUnlock(Result);
            break;
    }
}