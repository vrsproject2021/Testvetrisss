var strRowID = ""; var DELFLG = "";
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
    else {
        SetPageValue();
    }
    objhdnError.value = "";
}
function SetPageValue() {
    CallBackModality.callback("L");
    CallBackService.callback("L");
    CallBackNRH.callback();
    CallBackRC.callback();
}
function btnClose_OnClick() {
    Unlock = "Y";
    btnBrwClose_Onclick();
}
function btnReset_OnClick() {
    strValidate = "N";
    if (parent.GsRetStatus == "false")
        btnBrwEdit_Onclick('Settings/VRSGlCodeMap.aspx');
    else {
        parent.GsDlgConfAction = "RES";
        parent.GsNavURL = "Settings/VRSGlCodeMap.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=0";
        parent.PopupConfirm("030");
    }

}
function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrModalidtyList = new Array();
    var arrServiceList = new Array();
    var arrNRHList = new Array();
    var arrRCList = new Array();

    try {

        ArrRecords[0] = UserID;
        ArrRecords[1] = MenuID;

        arrModalidtyList = GetModalityList();
        arrServiceList = GetServiceList();
        arrNRHList = GetNonRevenueList();
        arrRCList = GetRadiologistChargeList();

        AjaxPro.timeoutPeriod = 1800000;
        VRSGlCodeMap.SaveRecord(ArrRecords, arrModalidtyList, arrServiceList, arrNRHList,arrRCList, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }

}
function GetModalityList() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        if (parseInt(gridItem.Data[1].toString()) > 0 && parseInt(gridItem.Data[3].toString()) > 0) {
            arrRecords[idx] = gridItem.Data[1].toString();
            arrRecords[idx + 1] = gridItem.Data[3].toString();
            arrRecords[idx + 2] = gridItem.Data[5].toString();
            idx = idx + 3;
        }
        itemIndex++;
    }
    return arrRecords;
}
function GetServiceList() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        if (parseInt(gridItem.Data[1].toString()) > 0) {
            arrRecords[idx] = gridItem.Data[1].toString();
            arrRecords[idx + 1] = gridItem.Data[2].toString();
            arrRecords[idx + 2] = gridItem.Data[3].toString();
            arrRecords[idx + 3] = gridItem.Data[4].toString();
            idx = idx + 4;
        }
        itemIndex++;
    }
    return arrRecords;
}
function GetNonRevenueList() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdNRH.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[1].toString();
        arrRecords[idx + 1] = gridItem.Data[3].toString();
        idx = idx + 2;
        itemIndex++;
    }
    return arrRecords;
}
function GetRadiologistChargeList() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdRC.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[1].toString();
        arrRecords[idx + 1] = gridItem.Data[3].toString();
        idx = idx + 2;
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
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            CallBackModality.callback("L");
            CallBackService.callback("L");
            CallBackNRH.callback();
            CallBackRC.callback();
            break;
    }
}

/**************MODALITY***************/
function btnAddModality_OnClick() {
    var strDtls = "";
    MODADD = "Y";
    strDtls = GetModalityGridDetails();
    CallBackModality.callback("A", strDtls);
}
function ddlCategory_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[1] = document.getElementById("ddlCategory_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlModality_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("ddlModality_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtGLCodeMod_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("txtGLCodeMod_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function GetModalityGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString() + SecDivider;
        strDtls += gridItem.Data[5].toString();
        itemIndex++;
    }
    return strDtls;
}
function DeleteModalityRow(ID) {
    strRowID = ID;
    DELFLG = "M";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
/**************MODALITY***************/

/**************SERVICE***************/
function btnAddService_OnClick() {
    var strDtls = "";
    SVCADD = "Y";
    strDtls = GetServiceGridDetails();
    CallBackService.callback("A", strDtls);
}
function ddlService_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[1] = document.getElementById("ddlService_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlModalitySvc_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("ddlModalitySvc_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtGLCodeSvc_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtGLCodeSvc_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtGLCodeAHSvc_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("txtGLCodeAHSvc_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function GetServiceGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString() 
        itemIndex++;
    }
    return strDtls;
}
function DeleteServiceRow(ID) {
    strRowID = ID;
    DELFLG = "S";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
/**************SERVICE***************/

function DeleteRecord() {
    var strDtls = "";
    switch (DELFLG) {
        case "M":
            strDtls = GetModalityGridDetails();
            CallBackModality.callback("D", strRowID, strDtls);
            break;
        case "S":
            strDtls = GetServiceGridDetails();
            CallBackService.callback("D", strRowID, strDtls);
            break;
    }
    
}
function txtGLCodeNRH_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdNRH.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtGLCodeNRH_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtGLCodeRC_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdRC.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtGLCodeRC_" + RowId).value;
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
        case "DeleteRecord":
            DeleteRecord(Result);
            break;
    }
}
