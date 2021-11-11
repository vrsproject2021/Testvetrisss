var strRowID = "0"; 
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
function SearchRecord() {
    var ArrRecords = new Array();
    
    ArrRecords[0] = "L";
    ArrRecords[1] = objtxtName.value;
    ArrRecords[2] = UserID;
    ArrRecords[3] = MenuID;

    CallBackBrw.callback(ArrRecords);
}
function ResetRecord() {
    
    objtxtName.value = "";
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
        VRSGroups.SaveRecord(ArrRecords, arrParams, ShowProcess);
       
       
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
            arrRecords[idx + 2] = gridItem.Data[3].toString();
            idx = idx + 3;
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
function txtDispOrd_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtDispOrd_" + RowId).value;
            gridItem.Data[4] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
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
