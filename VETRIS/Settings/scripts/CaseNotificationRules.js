var strRowID = "0"; 

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
    LoadRecords();
}
function LoadRecords() {
    var ArrRecords = new Array();
    ArrRecords[0] = UserID;
    ArrRecords[1] = MenuID;
    CallBackBrw.callback(ArrRecords);
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
        VRSCaseNotificationRules.SaveRecord(ArrRecords, arrParams, ShowProcess);
       
       
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
        changed = gridItem.Data[9].toString();
        if (changed == "Y") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[5].toString();
            arrRecords[idx + 2] = gridItem.Data[8].toString();
            idx = idx + 3;
        }
        itemIndex++;
    }
    return arrRecords;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
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
            LoadRecords();
            break;
    }
}

function txtTime_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            if (parent.Trim(document.getElementById("txtTime_" + RowId).value) == "")
                gridItem.Data[5] = "0";
            else 
                gridItem.Data[5] = document.getElementById("txtTime_" + RowId).value;

            gridItem.Data[9] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlUser_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[8] = document.getElementById("ddlUser_" + RowId).value;
            gridItem.Data[9] = "Y";
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
function ResetValueInteger(objCtrlID, strDefaultValue) {
    var objCtrl;
    objCtrl = document.getElementById(objCtrlID.id);
    if (objCtrl == null) objCtrl = objCtrlID;
    if (strDefaultValue != null) {
        if (objCtrl.value == "" || parseInt(objCtrl.value) <= 0) {
            objCtrl.value = strDefaultValue;
        }
    }
    else {
        if (objCtrl.value == "") objCtrl.value = "0";
    }
}
