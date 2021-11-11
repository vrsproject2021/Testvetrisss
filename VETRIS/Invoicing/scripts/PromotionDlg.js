$(document).ready($(function () {
    var dt = new Date();
    var config = parent.GsDlgConfAction;
    if (parent.GsDlgConfAction == 'NEWUI') {
        $("#dvDiscount").css("display", "block");
        $("#dvFreeCredit").css("display", "none");
        LoadModality('D', objddlAccount.value);

    }
    else {
        if (objddlType.value == 'D') {
            $("#dvDiscount").css("display", "block");
            $("#dvFreeCredit").css("display", "none");
        }
        else {
            $("#dvDiscount").css("display", "none");
            $("#dvFreeCredit").css("display", "block");
        }

    }

}))
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
    SetPageValue();
}
function SetPageValue() {
    if (objhdnID.value != "00000000-0000-0000-0000-000000000000") {
        objddlType.disabled = true;
        if ((objddlType.value == "F") || (objddlType.value == "")) { objtxtFromDate.disabled = true; objimgFromDt.style.display = "none"; }
        else if (objddlType.value == "D") { objtxtFromDate.disabled = false; objimgFromDt.style.display = "inline"; }
        CallBackPromo.callback("L", objhdnID.value, objddlAccount.value, objddlType.value);
    }

}
function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        btnBrwAddUI_Onclick('Invoicing/VRSPromotionDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Invoicing/VRSPromotionDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSPromotionDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSPromotionDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Invoicing/VRSPromotionBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
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
    var ArrPromo = new Array();
    var strFromDt = ""; var strToDt = "";

    if (parent.Trim(objtxtFromDate.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtFromDate.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtFromDate.value, parent.GsDateFormat, parent.GsDateSep);
    }
    if (parent.Trim(objtxtFromDate.value) == "") strToDt = "01Jan1900";
    else {
        if (document.all)
            strToDt = parent.SetDateFormat(objtxtToDate.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strToDt = parent.SetDateFormat1(objtxtToDate.value, parent.GsDateFormat, parent.GsDateSep);
    }

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objddlType.value;
        ArrRecords[2] = objddlAccount.value;
        ArrRecords[3] = strFromDt;
        ArrRecords[4] = strToDt;
        ArrRecords[5] = "Y"; if (objrdoStatNo.checked) ArrRecords[5] = "N";
        ArrRecords[6] = objddlReason.value;
        ArrRecords[7] = UserID;
        ArrRecords[8] = MenuID;

        ArrPromo = GetPromotions();
        if (ArrPromo.length > 0) {
            AjaxPro.timoutPeriod = 1800000;
            VRSPromotionDlg.SaveRecord(ArrRecords, ArrPromo, ShowProcess);
        }
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetPromotions() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    var RowId = "0"; var ModID = "0"; var InstId = "00000000-0000-0000-0000-000000000000";

    if (ValidateRows()) {
        while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
            RowId = gridItem.Data[0].toString();
            InstId = gridItem.Data[2].toString();
            ModID = gridItem.Data[3].toString();
            if (IsDuplicate(RowId, InstId, ModID) == false) {
                arrRecords[idx] = gridItem.Data[0].toString();
                arrRecords[idx + 1] = gridItem.Data[1].toString();
                arrRecords[idx + 2] = gridItem.Data[2].toString();
                arrRecords[idx + 3] = gridItem.Data[3].toString();
                arrRecords[idx + 4] = gridItem.Data[4].toString();
                arrRecords[idx + 5] = gridItem.Data[5].toString();
                idx = idx + 6;
            }
            else {
                arrRecords.length = 0;
                break;
            }
            itemIndex++;
        }
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
            SetPageValue();
            parent.GsRetStatus = "false";
            break;
    }
}


function IsDuplicate(ID, InstitutionID, ModalityID) {
    var itemIndex = 0;
    var gridItem;
    var bRet = false;
    var RowId = "0"; var ModID = "0"; var InstId = "00000000-0000-0000-0000-000000000000";

    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        InstId = gridItem.Data[2].toString();
        ModID = gridItem.Data[3].toString();

        if (RowId != ID) {
            if ((InstitutionID == InstId) && (ModalityID == ModID)) {
                parent.PopupMessage(RootDirectory, strForm, "IsDuplicate()", "258", "true", RowId);
                bRet = true;
                break;
            }
        }

        itemIndex++;
    }

    return bRet;
}
function btnAddPromo_OnClick() {
    var strDtls = "";
    if (objddlAccount.value != "00000000-0000-0000-0000-000000000000") {
        if (ValidateRows()) {
            strDtls = GetPromoGridDetails();
            CallBackPromo.callback("A", strDtls, document.getElementById("hdnInst").value, document.getElementById("hdnMod").value, objddlType.value);
        }
    }
    else {
        parent.PopupMessage(RootDirectory, strForm, "btnAddPromo_OnClick()", "225", "true");
    }
}
function ValidateRows() {
    var itemIndex = 0;
    var gridItem;
    var bRet = true;
    var type = objddlType.value;
    var RowId = "0"; var ModID = "0"; var InstId = "00000000-0000-0000-0000-000000000000";
    var disc = 0; var credit = 0; var strErr = "";

    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        InstId = gridItem.Data[2].toString();
        ModID = gridItem.Data[3].toString();
        disc = parseFloat(gridItem.Data[4].toString());
        credit = parseFloat(gridItem.Data[5].toString());

        if (InstId == "00000000-0000-0000-0000-000000000000") strErr = "254";
        if (ModID == "0") { if (strErr != "") strErr += Divider; strErr += "255"; }
        if (type == "D") {
            if (disc == 0) {
                if (strErr != "") strErr += Divider; strErr += "256";
            }
        }
        else if (type == "F") {
            if (credit == 0) {
                if (strErr != "") strErr += Divider; strErr += "257";
            }
        }

        if (parent.Trim(strErr) != "") {
            parent.PopupMessage(RootDirectory, strForm, "ValidateRow()", strErr, "true", RowId);
            bRet = false;
            break;
        }
        itemIndex++;
    }

    return bRet;
}
function DeleteRow(ID) {
    strRowID = ID;
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function DeleteRecord() {

    strDtls = GetPromoGridDetails();
    CallBackPromo.callback("D", strRowID, strDtls, document.getElementById("hdnInst").value, document.getElementById("hdnMod").value, objddlType.value);

}
function GetPromoGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
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
function ddlInstitution_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("ddlInst_" + RowId).value;
            break;
        }
        itemIndex++;
    }
}
function ddlModality_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("ddlModality_" + RowId).value;
            break;
        }
        itemIndex++;
    }
}
function txtDisc_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("txtDisc_" + RowId).value;
            break;
        }
        itemIndex++;
    }
}
function txtCredit_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdPromo.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("txtCredit_" + RowId).value;
            break;
        }
        itemIndex++;
    }
}

function ddlType_OnChange() {
    if (objhdnID.value == "00000000-0000-0000-0000-000000000000") {
        if (objddlType.value == "") { objddlAccount.value = "00000000-0000-0000-0000-000000000000"; objddlAccount.disabled = true; }
        else { objddlAccount.disabled = false; }
    }
    if ((objddlType.value == "F") || (objddlType.value == "")) { objtxtFromDate.disabled = true; objimgFromDt.style.display = "none"; }
    else if (objddlType.value == "D") { objtxtFromDate.disabled = false; objimgFromDt.style.display = "inline"; }
    CallBackPromo.callback("L", objhdnID.value, objddlAccount.value, objddlType.value);
}
function ddlAccount_OnChange() {
    CallBackPromo.callback("L", objhdnID.value, objddlAccount.value, objddlType.value);
}


function SetSelectedDate(objCtrlID, CalName, LsImgID) {
    var strDate = ""; var strClass = "";
    var dt;
    objCtrl = document.getElementById(objCtrlID.id); if (objCtrl == null) objCtrl = objCtrlID;
    strDate = document.getElementById(objCtrl).value;

    if (parent.Trim(strDate) != "") {
        if (document.all) {
            dt = new Date(parent.SetDateFormat(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
        else {
            dt = new Date(parent.SetDateFormat1(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
    }
    else
        dt = new Date();
    switch (CalName) {
        case "CalCreatedDate":
            CalCreatedDate.setSelectedDate(dt); CalCreatedDate.show();
            break;
        case "CalFromDate":
            CalFromDate.setSelectedDate(dt); CalFromDate.show();
            break;
        case "CalToDate":
            CalToDate.setSelectedDate(dt); CalToDate.show();
            break;
    }


}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}
function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

}
function ResetValueInteger(objCtrlID) {

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

function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {

        case "SaveRecord":
            SaveRecord(Result);
            break;

    }
}