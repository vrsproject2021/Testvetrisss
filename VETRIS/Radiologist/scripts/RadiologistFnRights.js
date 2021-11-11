var DEL_FLAG = ""; var strRowID = "0";
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
    CallBackRights.callback(objhdnID.value);
    CallBackModality.callback(objhdnID.value);
    CallBackSpecies.callback(objhdnID.value);
    LoadInstitutions();
    LoadStudyTypes();
    LoadRadiologists();
    parent.adjustFrameHeight();
}
function LoadInstitutions() {
    CallBackInst.callback(objhdnID.value);
    CallBackSelInst.callback("L", objhdnID.value);
}
function LoadStudyTypes() {
    CallBackST.callback(objhdnID.value);
    CallBackSelST.callback("L", objhdnID.value);
}
function LoadRadiologists() {
    CallBackRad.callback(objhdnID.value);
    CallBackSelRad.callback("L", objhdnID.value);
}

function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Radiologist/VRSRadiologistFnRights.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Radiologist/VRSRadiologistFnRights.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Masters/VRSRadiologistBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}

function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrRights = new Array();
    var arrMods = new Array();
    var arrSpes = new Array();
    var arrInst = new Array();
    var arrST = new Array();
    var arrRad = new Array();

    try {
        ArrRecords[0]   = objhdnID.value;
        ArrRecords[1]  = UserID;
        ArrRecords[2]  = MenuID;
        
        arrRights = GetRights();
        arrMods = GetModlities();
        arrSpes = GetSpecies();
        arrInst = GetInstitutions();
        arrST = GetStudyTypes();
        arrRad = GetRadiologists();

        AjaxPro.timoutPeriod = 1800000;
        VRSRadiologistFnRights.SaveRecord(ArrRecords, arrRights, arrMods,arrSpes, arrInst,arrST,arrRad, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetRights() {
    var itemIndex = 0; var gridItem;
    var arrRights = new Array(); var idx = 0; var sel = "";

    while (gridItem = grdRights.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[1].toString();

        if (sel == "Y") {
            arrRights[idx] = gridItem.Data[0].toString();
            idx = idx + 1;
        }
        itemIndex++;
    }
    return arrRights;
}
function GetModlities() {
    var itemIndex = 0; var gridItem;
    var arrMod = new Array(); var idx = 0; var sel = ""; 

    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[1].toString();
        
        if (sel == "Y") {
            arrMod[idx] = gridItem.Data[0].toString();
            idx = idx + 1;
        }
        itemIndex++;
    }
    return arrMod;
}
function GetSpecies() {
    var itemIndex = 0; var gridItem;
    var arrSpec = new Array(); var idx = 0; var sel = "";

    while (gridItem = grdSpecies.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[1].toString();

        if (sel == "Y") {
            arrSpec[idx] = gridItem.Data[0].toString();
            idx = idx + 1;
        }
        itemIndex++;
    }
    return arrSpec;
}
function GetInstitutions() {
    var itemIndex = 0; var gridItem;
    var arrInst = new Array(); var idx = 0;

    while (gridItem = grdSelInst.get_table().getRow(itemIndex)) {

            arrInst[idx] = gridItem.Data[0].toString();
            idx = idx + 1;
        itemIndex++;
    }
    return arrInst;
}
function GetStudyTypes() {
    var itemIndex = 0; var gridItem;
    var arrST = new Array(); var idx = 0;

    while (gridItem = grdSelST.get_table().getRow(itemIndex)) {

        arrST[idx] = gridItem.Data[0].toString();
        idx = idx + 1;
        itemIndex++;
    }
    return arrST;
}
function GetRadiologists() {
    var itemIndex = 0; var gridItem;
    var arrRad = new Array(); var idx = 0;

    while (gridItem = grdSelRad.get_table().getRow(itemIndex)) {

        arrRad[idx] = gridItem.Data[0].toString();
        idx = idx + 1;
        itemIndex++;
    }
    return arrRad;
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
            objhdnID.value = arrRes[2];
            CallBackRights.callback(objhdnID.value);
            CallBackModality.callback(objhdnID.value);
            CallBackST.callback(objhdnID.value);
            CallBackInst.callback(objhdnID.value);
            CallBackRad.callback(objhdnID.value);
            parent.GsRetStatus = "false";
            break;
    }
}

function chkSelRightHdr_OnClick() {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    while (gridItem = grdRights.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("chkSelRightHdr").checked) {
            if (document.getElementById("chkSelRight_" + RowId) != null) document.getElementById("chkSelRight_" + RowId).checked = true;
            gridItem.Data[1] = "Y";
        }
        else {
            if (document.getElementById("chkSelRight_" + RowId) != null) document.getElementById("chkSelRight_" + RowId).checked = false;
            gridItem.Data[1] = "N";

        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function chkSelRight_OnClick(ID) {
    var rc = grdRights.get_recordCount();
    var itemIndex = 0; var gridItem; var RowId = "0"; var selCnt = 0;
    while (gridItem = grdRights.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            if (document.getElementById("chkSelRight_" + RowId).checked) {
                gridItem.Data[1] = "Y";
                selCnt = selCnt + 1;
            }
            else
                gridItem.Data[1] = "N";
            
        }
        else {
            if (gridItem.Data[1] == "Y") selCnt = selCnt + 1;
        }
        itemIndex++;
    }
    if (selCnt == rc) document.getElementById("chkSelRightHdr").checked = true; else document.getElementById("chkSelRightHdr").checked = false;
    parent.GsRetStatus = "true";

}

function chkSelModHdr_OnClick() {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("chkSelModHdr").checked) {
            if (document.getElementById("chkSelModality_" + RowId) != null) document.getElementById("chkSelModality_" + RowId).checked = true;
            gridItem.Data[1] = "Y";
        }
        else {
            if (document.getElementById("chkSelModality_" + RowId) != null) document.getElementById("chkSelModality_" + RowId).checked = false;
            gridItem.Data[1] = "N";

        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function chkSelModality_OnClick(ID) {
    var rc = grdModality.get_recordCount();
    var itemIndex = 0; var gridItem; var RowId = "0"; var selCnt = 0;
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            if (document.getElementById("chkSelModality_" + RowId).checked) {
                gridItem.Data[1] = "Y";
                selCnt = selCnt + 1;
            }
            else
                gridItem.Data[1] = "N";
        }
        else {
            if (gridItem.Data[1] == "Y") selCnt = selCnt + 1;
        }
        itemIndex++;
    }
    if (selCnt == rc) document.getElementById("chkSelModHdr").checked = true; else document.getElementById("chkSelModHdr").checked = false;
    parent.GsRetStatus = "true";

}

function chkSelSpeciesHdr_OnClick() {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    while (gridItem = grdSpecies.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("chkSelSpeciesHdr").checked) {
            if (document.getElementById("chkSelSpecies_" + RowId) != null) document.getElementById("chkSelSpecies_" + RowId).checked = true;
            gridItem.Data[1] = "Y";
        }
        else {
            if (document.getElementById("chkSelSpecies_" + RowId) != null) document.getElementById("chkSelSpecies_" + RowId).checked = false;
            gridItem.Data[1] = "N";

        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function chkSelSpecies_OnClick(ID) {
    var rc = grdSpecies.get_recordCount();
    var itemIndex = 0; var gridItem; var RowId = "0"; var selCnt = 0;
    while (gridItem = grdSpecies.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            if (document.getElementById("chkSelSpecies_" + RowId).checked) {
                gridItem.Data[1] = "Y";
                selCnt = selCnt + 1;
            }
            else
                gridItem.Data[1] = "N";
        }
        else {
            if (gridItem.Data[1] == "Y") selCnt = selCnt + 1;
        }
        itemIndex++;
    }
    if (selCnt == rc) document.getElementById("chkSelSpeciesHdr").checked = true; else document.getElementById("chkSelSpeciesHdr").checked = false;
    parent.GsRetStatus = "true";

}

function chkSelInstHdr_OnClick() {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("chkSelInstHdr").checked) {
            if (document.getElementById("chkSelInst_" + RowId) != null) document.getElementById("chkSelInst_" + RowId).checked = true;
            gridItem.Data[1] = "Y";
        }
        else {
            if (document.getElementById("chkSelInst_" + RowId) != null) document.getElementById("chkSelInst_" + RowId).checked = false;
            gridItem.Data[1] = "N";

        }
        itemIndex++;
    }

    strDtls = GetInstitutionGridDetails();
    CallBackSelInst.callback("U", strDtls);
    parent.GsRetStatus = "true";
}
function chkSelInst_OnClick(ID) {
    var rc = grdInst.get_recordCount();
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = ""; var selCnt = 0;
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {

            if (document.getElementById("chkSelInst_" + RowId).checked) {
                gridItem.Data[1] = "Y";
                selCnt = selCnt + 1;
            }
            else {
                gridItem.Data[1] = "N";
            }

        }
        else {
            if (gridItem.Data[1] == "Y") selCnt = selCnt + 1;
        }
        itemIndex++;
    }
    if (selCnt == rc) document.getElementById("chkSelInstHdr").checked = true; else document.getElementById("chkSelInstHdr").checked = false;
    strDtls = GetInstitutionGridDetails();
    CallBackSelInst.callback("U", strDtls);
    parent.GsRetStatus = "true";
}
function GetInstitutionGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;
    var strSel = "";

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        strSel = gridItem.Data[1].toString();

        if (strSel == "Y") {
            if (strDtls != "") strDtls = strDtls + SecDivider;
            strDtls += gridItem.get_cells()[0].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[2].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[3].get_value().toString();
        }
        itemIndex++;
    }
    return strDtls;
}

function chkSelSTHdr_OnClick() {
   
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = ""; 
    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("chkSelSTHdr").checked) {
            if(document.getElementById("chkSelST_" + RowId) != null) document.getElementById("chkSelST_" + RowId).checked = true;
            gridItem.Data[1] = "Y";
        }
        else {
            if (document.getElementById("chkSelST_" + RowId) != null) document.getElementById("chkSelST_" + RowId).checked = false;
            gridItem.Data[1] = "N";
           
        }
        itemIndex++;
    }

    strDtls = GetStudyGridDetails();
    CallBackSelST.callback("U", strDtls);
    parent.GsRetStatus = "true";
}
function chkSelST_OnClick(ID) {
    var rc = grdST.get_recordCount();
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = ""; var selCnt = 0;
    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {

            if (document.getElementById("chkSelST_" + RowId).checked) {
                gridItem.Data[1] = "Y";
                selCnt = selCnt + 1;
            }
            else {
                gridItem.Data[1] = "N";
            }
        }
        else {
            if (gridItem.Data[1] == "Y") selCnt = selCnt + 1;
        }
        itemIndex++;
    }

    if (selCnt == rc) document.getElementById("chkSelSTHdr").checked = true; else document.getElementById("chkSelSTHdr").checked = false;
    strDtls = GetStudyGridDetails();
    CallBackSelST.callback("U", strDtls);
    parent.GsRetStatus = "true";
}
function GetStudyGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;
    var strSel = "";

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        strSel = gridItem.Data[1].toString();

        if (strSel == "Y") {
            if (strDtls != "") strDtls = strDtls + SecDivider;
            strDtls += gridItem.get_cells()[0].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[2].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[3].get_value().toString();
        }
        itemIndex++;
    }
    return strDtls;
}

function chkSelRadHdr_OnClick() {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("chkSelRadHdr").checked) {
            if (document.getElementById("chkSelRad_" + RowId) != null) document.getElementById("chkSelRad_" + RowId).checked = true;
            gridItem.Data[1] = "Y";
        }
        else {
            if (document.getElementById("chkSelRad_" + RowId) != null) document.getElementById("chkSelRad_" + RowId).checked = false;
            gridItem.Data[1] = "N";

        }
        itemIndex++;
    }

    strDtls = GetRadiologistGridDetails();
    CallBackSelRad.callback("U", strDtls);
    parent.GsRetStatus = "true";
}
function chkSelRad_OnClick(ID) {
    var rc = grdRad.get_recordCount();
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = ""; var selCnt = 0;
    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {

            if (document.getElementById("chkSelRad_" + RowId).checked) {
                gridItem.Data[1] = "Y";
                selCnt = selCnt + 1;
            }
            else {
                gridItem.Data[1] = "N";
            }

        }
        else {
            if (gridItem.Data[1] == "Y") selCnt = selCnt + 1;
        }
        itemIndex++;
    }
    if (selCnt == rc) document.getElementById("chkSelRadHdr").checked = true; else document.getElementById("chkSelRadHdr").checked = false;
    strDtls = GetRadiologistGridDetails();
    CallBackSelRad.callback("U", strDtls);
    parent.GsRetStatus = "true";
}
function GetRadiologistGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;
    var strSel = "";

    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        strSel = gridItem.Data[1].toString();

        if (strSel == "Y") {
            if (strDtls != "") strDtls = strDtls + SecDivider;
            strDtls += gridItem.get_cells()[0].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[2].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[3].get_value().toString();
        }
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
