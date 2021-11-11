var strRowID = "0"; var strType = "";
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

    ArrRecords[0] = "L";
    ArrRecords[1] = UserID;
    ArrRecords[2] = MenuID;

    CallBackMF.callback(ArrRecords);
    CallBackSF.callback(ArrRecords);
}

function DeleteRow(ID, Type) {
    strRowID = ID;
    strType = Type;
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function DeleteRecord() {
    var strDtls = "";
    if (strType == "M") {
        strDtls = GetModalityGridDetails();
        CallBackMF.callback("D", strRowID, strDtls, UserID, MenuID);
    }
    else if (strType == "S") {
        strDtls = GetServiceGridDetails();
        CallBackSF.callback("D", strRowID, strDtls, UserID, MenuID);
    }
}

function btnClose_OnClick() {
    parent.GsNavURL = "";
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}

/**********************MODALITY GRID**********************************************/
function ddlCategory_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("ddlCategory_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlModality_OnChange(ID) {
    var arrMod = new Array();
    var itemIndex = 0; var gridItem; var RowId = "0";

    if (parent.Trim(objhdnModality.value) != "") {
        if (parent.Trim(objhdnModality.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrMod = parent.Trim(objhdnModality.value).split(parent.objhdnDivider.value);
        }
        else
            arrMod[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "");
    }
    else
        arrMod[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "");


    
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("ddlModality_" + RowId).value;
            for (var i = 0; i < arrMod.length; i = i + 3) {
                if (arrMod[i] == document.getElementById("ddlModality_" + RowId).value) {
                    gridItem.Data[4] = arrMod[i + 2];
                    if (arrMod[i + 2] == "B") document.getElementById("txtInvBy_" + RowId).value = "Body Part";
                    else if (arrMod[i + 2] == "I") document.getElementById("txtInvBy_" + RowId).value = "Image";
                    else if (arrMod[i + 2] == "M") document.getElementById("txtInvBy_" + RowId).value = "Minute";
                    gridItem.Data[5] = document.getElementById("txtInvBy_" + RowId).value;
                    break;
                }
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtMinVal_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("txtMinVal_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtMaxVal_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[7] = document.getElementById("txtMaxVal_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtMFees_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[8] = document.getElementById("txtMFees_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtAddOn_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[9] = document.getElementById("txtAddOn_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtMaxFee_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[10] = document.getElementById("txtMaxFee_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtGLMod_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[11] = document.getElementById("txtGLMod_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}

function AddModality() {
    strDtls = GetModalityGridDetails();
    CallBackMF.callback("A", strDtls);
}
function GetModalityGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString() + SecDivider;
        strDtls += gridItem.Data[5].toString() + SecDivider;
        strDtls += gridItem.Data[6].toString() + SecDivider;
        strDtls += gridItem.Data[7].toString() + SecDivider;
        strDtls += gridItem.Data[8].toString() + SecDivider;
        strDtls += gridItem.Data[9].toString() + SecDivider;
        strDtls += gridItem.Data[10].toString() + SecDivider;
        strDtls += gridItem.Data[11].toString();
        itemIndex++;
    }
    return strDtls;
}
function SaveModality() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrParams = new Array();


    try {
        arrParams[0] = UserID;
        arrParams[1] = MenuID;
        ArrRecords = GetModalityRecords();

        AjaxPro.timeoutPeriod = 1800000;
        VRSFeesTemplate.SaveModalityRecord(ArrRecords, arrParams, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "SaveModality()", expErr.message, "true");
    }
}
function GetModalityRecords() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[0].toString();
        arrRecords[idx + 1] = gridItem.Data[1].toString();
        arrRecords[idx + 2] = gridItem.Data[2].toString();
        arrRecords[idx + 3] = gridItem.Data[3].toString();
        arrRecords[idx + 4] = gridItem.Data[4].toString();
        arrRecords[idx + 5] = gridItem.Data[6].toString();
        arrRecords[idx + 6] = gridItem.Data[7].toString();
        arrRecords[idx + 7] = gridItem.Data[8].toString();
        arrRecords[idx + 8] = gridItem.Data[9].toString();
        arrRecords[idx + 9] = gridItem.Data[10].toString();
        arrRecords[idx + 10] = gridItem.Data[11].toString();
        idx = idx + 11;
        itemIndex++;
    }
    return arrRecords;
}
function ProcessModalitySave(Result) {
    var arrRes = new Array();
    var ArrRecords = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessModalitySave()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessModalitySave()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "ProcessModalitySave()", arrRes[1], "false");
            ArrRecords[0] = "L";
            ArrRecords[1] = UserID;
            ArrRecords[2] = MenuID;
            CallBackMF.callback(ArrRecords);
            break;
    }
}
/**********************MODALITY GRID**********************************************/

/**********************SERVICE GRID**********************************************/
function ddlService_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("ddlService_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlSModality_OnChange(ID) {
    var arrMod = new Array();
    var itemIndex = 0; var gridItem; var RowId = "0";

    if (parent.Trim(objhdnModality.value) != "") {
        if (parent.Trim(objhdnModality.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrMod = parent.Trim(objhdnModality.value).split(parent.objhdnDivider.value);
        }
        else
            arrMod[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "");
    }
    else
        arrMod[0] = parent.Trim("0" + parent.objhdnDivider.value + "Select One" + parent.objhdnDivider.value + "");

    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("ddlSModality_" + RowId).value;
            for (var i = 0; i < arrMod.length; i = i + 3) {
                if (arrMod[i] == document.getElementById("ddlSModality_" + RowId).value) {
                    gridItem.Data[4] = arrMod[i + 2];
                    if (arrMod[i + 2] == "B") document.getElementById("txtSInvBy_" + RowId).value = "Body Part";
                    else if (arrMod[i + 2] == "I") document.getElementById("txtSInvBy_" + RowId).value = "Image";
                    else if (arrMod[i + 2] == "M") document.getElementById("txtSInvBy_" + RowId).value = "Minute";
                    gridItem.Data[5] = document.getElementById("txtSInvBy_" + RowId).value;
                    break;
                }
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtSMinVal_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("txtSMinVal_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtSMaxVal_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[7] = document.getElementById("txtSMaxVal_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtSFees_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[8] = document.getElementById("txtSFees_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtSFeesAH_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[9] = document.getElementById("txtSFeesAH_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtGLSvc_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[10] = document.getElementById("txtGLSvc_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}

function AddService() {
    strDtls = GetServiceGridDetails();
    CallBackSF.callback("A", strDtls);
}
function GetServiceGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString() + SecDivider;
        strDtls += gridItem.Data[5].toString() + SecDivider;
        strDtls += gridItem.Data[6].toString() + SecDivider;
        strDtls += gridItem.Data[7].toString() + SecDivider;
        strDtls += gridItem.Data[8].toString() + SecDivider;
        strDtls += gridItem.Data[9].toString() + SecDivider;
        strDtls += gridItem.Data[10].toString();
        itemIndex++;
    }
    return strDtls;
}
function SaveService() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrParams = new Array();


    try {
        arrParams[0] = UserID;
        arrParams[1] = MenuID;
        ArrRecords = GetServiceRecords();

        AjaxPro.timeoutPeriod = 1800000;
        VRSFeesTemplate.SaveServiceRecord(ArrRecords, arrParams, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "SaveService()", expErr.message, "true");
    }
}
function GetServiceRecords() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[0].toString();
        arrRecords[idx + 1] = gridItem.Data[1].toString();
        arrRecords[idx + 2] = gridItem.Data[2].toString();
        arrRecords[idx + 3] = gridItem.Data[3].toString();
        arrRecords[idx + 4] = gridItem.Data[4].toString();
        arrRecords[idx + 5] = gridItem.Data[6].toString();
        arrRecords[idx + 6] = gridItem.Data[7].toString();
        arrRecords[idx + 7] = gridItem.Data[8].toString();
        arrRecords[idx + 8] = gridItem.Data[9].toString();
        arrRecords[idx + 9] = gridItem.Data[10].toString();
        idx = idx + 10;
        itemIndex++;
    }
    return arrRecords;
}
function ProcessServiceSave(Result) {
    var arrRes = new Array();
    var ArrRecords = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessServiceSave()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessServiceSave()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "ProcessServiceSave()", arrRes[1], "false");
            ArrRecords[0] = "L";
            ArrRecords[1] = UserID;
            ArrRecords[2] = MenuID;
            CallBackSF.callback(ArrRecords);
            break;
    }
}

/**********************SERVICE GRID**********************************************/

function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveModalityRecord":
            ProcessModalitySave(Result);
            break;
        case "SaveServiceRecord":
            ProcessServiceSave(Result);
            break;
    }

}
function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

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
