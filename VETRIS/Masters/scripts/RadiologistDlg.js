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
    if (document.getElementById("MenuColor_0") != null) document.getElementById("MenuColor_0").style.backgroundColor = objhdnColor.value;
    objhdnError.value = "";
    CallBackModality.callback(objhdnID.value);
    parent.adjustFrameHeight();
}
function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        btnBrwAddUI_Onclick('Masters/VRSRadiologistDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Masters/VRSRadiologistDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Masters/VRSRadiologistDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Masters/VRSRadiologistDlg.aspx';
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
    var arrMods = new Array();
    var arrWU = new Array();
    var obj = document.getElementById("cke_contents_CKEditor1").children[0];

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objtxtFName.value;
        ArrRecords[2] = objtxtLName.value;
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
        ArrRecords[13] = objhdnLoginUserID.value;
        ArrRecords[14] = objtxtLoginID.value;
        ArrRecords[15] = objtxtPwd.value;
        ArrRecords[16] = objtxtPACSLoginID.value;
        ArrRecords[17] = objtxtPACSPwd.value;
        ArrRecords[18] = objtxtCredentials.value;// Added on 12th SEP 2019 @BK
        ArrRecords[19] = objhdnColor.value;
        ArrRecords[20] = "B"; if (objrdoEmail.checked) ArrRecords[20] = "E"; else if (objrdoSMS.checked) ArrRecords[20] = "S";
        ArrRecords[21] = obj.contentDocument.body.innerHTML;
        ArrRecords[22] = "O"; if (objrdoSVA.checked) ArrRecords[22] = "A";
        ArrRecords[23] = objtxtNotes.value;
        ArrRecords[24] = "N"; if (objrdoTransYes.checked) ArrRecords[24] = "Y";
        ArrRecords[25] = objddlAcctGroup.value;
        ArrRecords[26] = "N"; if (objrdoAsnYes.checked) ArrRecords[26] = "Y";
        ArrRecords[27] = objddlTimeZone.value;
        ArrRecords[28] = objtxtMaxWU.value;
        ArrRecords[29] = UserID;
        ArrRecords[30] = MenuID;

        arrMods = GetModlities();

        AjaxPro.timoutPeriod = 1800000;
        VRSRadiologistDlg.SaveRecord(ArrRecords, arrMods, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetModlities() {
    var itemIndex = 0; var gridItem;
    var arrMod = new Array(); var idx = 0; var sel = "";

    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[2].toString();

        if (sel == "Y") {
            arrMod[idx] = gridItem.Data[1].toString();
            arrMod[idx + 1] = gridItem.Data[4].toString();
            arrMod[idx + 2] = gridItem.Data[5].toString();
            arrMod[idx + 3] = gridItem.Data[6].toString();
            arrMod[idx + 4] = gridItem.Data[7].toString();
            idx = idx + 5;
        }
        itemIndex++;
    }
    return arrMod;
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
            CallBackModality.callback(objhdnID.value);
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
        VRSRadiologistDlg.FetchStates(ArrRecords, ShowProcess);
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
function rdoReaderType_OnCheckedChanged(e) {

    if (e == "P") {
        document.getElementById('txtFeePrelim').Enabled = true;
        document.getElementById('txtFeeFinal').Enabled = false;
        $("#txtFeePrelim").prop("disabled", false);
        $("#txtFeeFinal").prop("disabled", true);

        objtxtFeeFinal.value = "0.00";
    }
    else if (e == "F") {
        document.getElementById('txtFeePrelim').Enabled = false;
        document.getElementById('txtFeeFinal').Enabled = true;
        $("#txtFeePrelim").prop("disabled", true);
        $("#txtFeeFinal").prop("disabled", false);
        objtxtFeePrelim.value = "0.00";
    }
    else if (e == "B") {
        $("#txtFeePrelim").prop("disabled", false);
        $("#txtFeeFinal").prop("disabled", false);

    }

}


function chkSel_OnClick(ID) {
    var rc = grdModality.get_recordCount();
    var itemIndex = 0; var gridItem; var RowId = "0"; var selCnt = 0;
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            if (document.getElementById("chkSel_" + RowId).checked) {
                gridItem.Data[2] = "Y";
                document.getElementById("txtPrelim_" + RowId).readOnly = ""; document.getElementById("txtPrelim_" + RowId).className = "GridTextBox";
                document.getElementById("txtFinal_" + RowId).readOnly = ""; document.getElementById("txtFinal_" + RowId).className = "GridTextBox";
                document.getElementById("txtAddl_" + RowId).readOnly = ""; document.getElementById("txtAddl_" + RowId).className = "GridTextBox";
                document.getElementById("txtAddl_" + RowId).readOnly = ""; document.getElementById("txtAddl_" + RowId).className = "GridTextBox";
                document.getElementById("txtWU_" + RowId).readOnly = ""; document.getElementById("txtWU_" + RowId).className = "GridTextBox";
                selCnt = selCnt + 1;
            }
            else {
                gridItem.Data[2] = "N";
                document.getElementById("chkSelHdr").checked = false;
                document.getElementById("txtPrelim_" + RowId).value = parent.SetDecimalFormat("0"); document.getElementById("txtPrelim_" + RowId).readOnly = "readOnly"; document.getElementById("txtPrelim_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("txtFinal_" + RowId).value = parent.SetDecimalFormat("0"); document.getElementById("txtFinal_" + RowId).readOnly = "readOnly"; document.getElementById("txtFinal_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("txtAddl_" + RowId).value = parent.SetDecimalFormat("0"); document.getElementById("txtAddl_" + RowId).readOnly = "readOnly"; document.getElementById("txtAddl_" + RowId).className = "GridTextBoxReadOnly";
                document.getElementById("txtWU_" + RowId).value = "0"; document.getElementById("txtWU_" + RowId).readOnly = "readOnly"; document.getElementById("txtWU_" + RowId).className = "GridTextBoxReadOnly";
                gridItem.Data[4] = document.getElementById("txtPrelim_" + RowId).value;
                gridItem.Data[5] = document.getElementById("txtFinal_" + RowId).value;
                gridItem.Data[6] = document.getElementById("txtAddl_" + RowId).value;
                gridItem.Data[7] = document.getElementById("txtWU_" + RowId).value;
            }

        }
        else {
            if (gridItem.Data[2] == "Y") selCnt = selCnt + 1;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
    if (selCnt == rc) document.getElementById("chkSelHdr").checked = true; else document.getElementById("chkSelHdr").checked = false;
}
function txtPrelim_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var selCnt = 0;
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("txtPrelim_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtFinal_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var selCnt = 0;
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("txtFinal_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtAddl_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var selCnt = 0;
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("txtAddl_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtWU_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var selCnt = 0;
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            gridItem.Data[7] = document.getElementById("txtWU_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function chkSelHdr_OnClick() {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    while (gridItem = grdModality.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("chkSelHdr").checked) {
            document.getElementById("chkSel_" + RowId).checked = true;
            gridItem.Data[2] = "Y";
            document.getElementById("txtPrelim_" + RowId).readOnly = ""; document.getElementById("txtPrelim_" + RowId).className = "GridTextBox";
            document.getElementById("txtFinal_" + RowId).readOnly = ""; document.getElementById("txtFinal_" + RowId).className = "GridTextBox";
            document.getElementById("txtAddl_" + RowId).readOnly = ""; document.getElementById("txtAddl_" + RowId).className = "GridTextBox";
            document.getElementById("txtWU_" + RowId).readOnly = ""; document.getElementById("txtWU_" + RowId).className = "GridTextBox";
        }
        else {
            document.getElementById("chkSel_" + RowId).checked = false;
            gridItem.Data[2] = "N";
            document.getElementById("chkSelHdr").checked = false;
            document.getElementById("txtPrelim_" + RowId).value = parent.SetDecimalFormat("0"); document.getElementById("txtPrelim_" + RowId).readOnly = "readOnly"; document.getElementById("txtPrelim_" + RowId).className = "GridTextBoxReadOnly";
            document.getElementById("txtFinal_" + RowId).value = parent.SetDecimalFormat("0"); document.getElementById("txtPrelim_" + RowId).readOnly = "readOnly"; document.getElementById("txtFinal_" + RowId).className = "GridTextBoxReadOnly";
            document.getElementById("txtAddl_" + RowId).value = parent.SetDecimalFormat("0"); document.getElementById("txtPrelim_" + RowId).readOnly = "readOnly"; document.getElementById("txtAddl_" + RowId).className = "GridTextBoxReadOnly";
            document.getElementById("txtWU_" + RowId).value = "0"; document.getElementById("txtWU_" + RowId).readOnly = "readOnly"; document.getElementById("txtWU_" + RowId).className = "GridTextBoxReadOnly";
            gridItem.Data[4] = document.getElementById("txtPrelim_" + RowId).value;
            gridItem.Data[5] = document.getElementById("txtFinal_" + RowId).value;
            gridItem.Data[6] = document.getElementById("txtAddl_" + RowId).value;
            gridItem.Data[7] = document.getElementById("txtWU_" + RowId).value;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
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
function ResetValueDecimal(objCtrlID, decimalPlace) {

    var objCtrl;
    if (decimalPlace == null) decimalPlace = parent.GiDecimal;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value.replace("-", ""), decimalPlace);

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