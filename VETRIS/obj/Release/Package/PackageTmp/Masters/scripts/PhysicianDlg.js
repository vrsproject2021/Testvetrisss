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
    else {

        CallBackInst.callback("L", objhdnID.value);
    }
    objhdnError.value = "";

}
function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        btnBrwAddUI_Onclick('Masters/VRSPhysicianDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Masters/VRSPhysicianDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Masters/VRSPhysicianDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Masters/VRSPhysicianDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Masters/VRSPhysicianBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
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
    var ArrInst = new Array();

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objtxtCode.value;
        ArrRecords[2] = objtxtName.value;
        ArrRecords[3] = "Y"; if (objrdoStatNo.checked) ArrRecords[3] = "N";
        ArrRecords[4] = objtxtAddr1.value;
        ArrRecords[5] = objtxtAddr2.value;
        ArrRecords[6] = objtxtCity.value;
        ArrRecords[7] = objddlCountry.value;
        ArrRecords[8] = objddlState.value;
        ArrRecords[9] = objtxtZip.value;
        ArrRecords[10] = objtxtEmailID.value;
        ArrRecords[11] = objtxtTel.value;
        ArrRecords[12] = objtxtMobile.value;
        ArrRecords[13] = UserID;
        ArrRecords[14] = MenuID;

        ArrInst = GetInstitutions();

        AjaxPro.timoutPeriod = 1800000;
        VRSPhysicianDlg.SaveRecord(ArrRecords, ArrInst, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}

function GetInstitutions() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[1].toString();
       
        idx = idx + 1;
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
            objhdnID.value = arrRes[2];
            CallBackInst.callback("L", objhdnID.value);
            parent.GsRetStatus = "false";
            break;
    }
}

function ddlCountry_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlCountry.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSPhysicianDlg.FetchStates(ArrRecords, ShowProcess);
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


function btnAddInst_OnClick() {
    var strDtls = "";
    INSTADD = "Y";
    strDtls = GetGridDetails();
    CallBackInst.callback("A", strDtls);
}
function ddlInst_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {

            gridItem.Data[1] = document.getElementById("ddlInst_" + RowId).value
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlUser_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {

            gridItem.Data[2] = document.getElementById("ddlUser_" + RowId).value
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
function GetGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString();
        itemIndex++;
    }
    return strDtls;
}

function DeleteRecord() {
    var strDtls = "";
    strDtls = GetGridDetails();
    CallBackInst.callback("D", strRowID, strDtls);
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "FetchStates":
            PopulateStateList(Result);
            break;
        case "SaveRecord":
            SaveRecord(Result);
            break;

    }
}