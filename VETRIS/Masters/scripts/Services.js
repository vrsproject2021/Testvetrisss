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
    ArrRecords[2] = ddlStatus.value; 
    ArrRecords[3] = UserID;
    ArrRecords[4] = MenuID;

    CallBackBrw.callback(ArrRecords);
}
function ResetRecord() {
    objtxtName.value = "";
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
        VRSServices.SaveRecord(ArrRecords, arrParams, ShowProcess);
       
       
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
        changed = gridItem.Data[7].toString();
        if (changed == "Y") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[1].toString();
            arrRecords[idx + 2] = gridItem.Data[2].toString();
            arrRecords[idx + 3] = gridItem.Data[3].toString();
            arrRecords[idx + 4] = gridItem.Data[4].toString();
            arrRecords[idx + 5] = gridItem.Data[5].toString();
            arrRecords[idx + 6] = gridItem.Data[6].toString();
            idx = idx + 7;
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

function btnAdd_OnClick() {
    strDtls = GetGridDetails();
    CallBackBrw.callback("A", strDtls);
}

function txtName_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
           
            gridItem.Data[2] = document.getElementById("txtName_" + RowId).value;
            gridItem.Data[7] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtCode_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            document.getElementById("txtCode_" + RowId).value = document.getElementById("txtCode_" + RowId).value.toUpperCase();
            gridItem.Data[3] = document.getElementById("txtCode_" + RowId).value;
            gridItem.Data[7] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlPriority_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("ddlPriority_" + RowId).value;
            gridItem.Data[7] = "Y";
            if (parseInt(gridItem.Data[4]) != 0) {
                document.getElementById("txtCode_" + RowId).value = "";
                document.getElementById("txtCode_" + RowId).readOnly = "readOnly";
                document.getElementById("txtCode_" + RowId).className = "GridTextBoxReadOnly";
            }
            else {
                document.getElementById("txtCode_" + RowId).readOnly = "";
                document.getElementById("txtCode_" + RowId).className = "GridTextBox";
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtGLCode_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            document.getElementById("txtGLCode_" + RowId).value = parent.Trim(document.getElementById("txtGLCode_" + RowId).value.toUpperCase());
            gridItem.Data[5] = document.getElementById("txtGLCode_" + RowId).value;
            gridItem.Data[7] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ChkStatus_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            if (document.getElementById("chkAct_" + RowId).checked) gridItem.Data[6] = "Y";
            else gridItem.Data[6] = "N";
            gridItem.Data[7] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}

function DeleteRow(ID) {
    strRowID = ID;
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function DeleteRecord() {
    var strDtls = "";
    strDtls = GetGridDetails();
    CallBackBrw.callback("D", strRowID, strDtls);
}
function GetGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString() + SecDivider;
        strDtls += gridItem.Data[5].toString() + SecDivider;
        strDtls += gridItem.Data[6].toString() + SecDivider;
        strDtls += gridItem.Data[7].toString() + SecDivider;
        strDtls += gridItem.Data[8].toString();
        itemIndex++;
    }
    return strDtls;
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
