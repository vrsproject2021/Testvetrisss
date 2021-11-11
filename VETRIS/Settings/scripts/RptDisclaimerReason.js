var strRowID = "0"; var objgridItem;
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
        objtxtType.value = parent.GsFilter[0];
        objddlStatus.value = parent.GsFilter[1];
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    var ArrRecords = new Array();
    
    ArrRecords[0] = objtxtType.value;
    ArrRecords[1] = ddlStatus.value; 
    ArrRecords[2] = UserID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtType.value);
    parent.GsFilter[1] = objddlStatus.value;
}
function ResetRecord() {
    objtxtType.value = "";
    objddlStatus.value = "X";
    SearchRecord();
}

function btnSave_OnClick() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrParams = new Array();


    try {
        arrParams[0] = UserID;
        arrParams[1] = MenuID;
        ArrRecords = GetRecords();

        AjaxPro.timeoutPeriod = 1800000;
        VRSRptDisclaimerReason.SaveRecord(ArrRecords, arrParams, ShowProcess);
       
       
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }

}
function GetRecords() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0; var changed = "";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        changed = gridItem.Data[4].toString();
        if (changed == "Y") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[1].toString();
            arrRecords[idx + 2] = gridItem.Data[2].toString();
            arrRecords[idx + 3] = gridItem.Data[3].toString();
            idx = idx + 4;
        }
        itemIndex++;
    }
    return arrRecords;
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
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            SearchRecord();
            break;
    }
}

