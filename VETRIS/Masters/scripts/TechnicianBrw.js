
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
}
function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objtxtCode.value    = parent.GsFilter[0];
        objtxtName.value    = parent.GsFilter[1];
        objddlStatus.value  = parent.GsFilter[2];
        objddlCountry.value = parent.GsFilter[3];
        objddlState.value   = parent.GsFilter[4];
        objtxtCity.value    = parent.GsFilter[5];
        objtxtMobile.value  = parent.GsFilter[6];
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objtxtCode.value;
    ArrRecords[1] = objtxtName.value;
    ArrRecords[2] = objddlStatus.value;
    ArrRecords[3] = objddlCountry.value;
    ArrRecords[4] = objddlState.value;
    ArrRecords[5] = objtxtCity.value;
    ArrRecords[6] = objtxtMobile.value;
    ArrRecords[7] = UserID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtCode.value);
    parent.GsFilter[1] = parent.Trim(objtxtName.value);
    parent.GsFilter[2] = objddlStatus.value;
    parent.GsFilter[3] = objddlCountry.value;
    parent.GsFilter[4] = objddlState.value;
    parent.GsFilter[5] = parent.Trim(objtxtCity.value);
    parent.GsFilter[6] = parent.Trim(objtxtMobile.value);
}
function ResetRecord() {
    objtxtCode.value = "";
    objtxtName.value = "";
    objddlStatus.value = "X";
    objddlCountry.value = "0";
    objtxtCity.value = "";
    objtxtMobile.value = "";
    BindDDLBlank();
    SearchRecord();
}


function ddlCountry_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlCountry.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSTechnicianBrw.FetchStates(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "ddlCountry_OnChange()", expErr.message, "true");
    }
}
function PopulateStateList(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "PopulateStateList()", arrRes[1], "true");
            break;
        case "true":
            objddlState.length = 0;
            for (var i = 1; i < arrRes.length; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlState.add(op);
            }
            break;
    }

}
function BindDDLBlank() {
    var op = document.createElement("option");
    op.value = "0"; op.text = "Select One";
    objddlState.add(op);
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "FetchStates":
            PopulateStateList(Result);
            break;
    }
}

