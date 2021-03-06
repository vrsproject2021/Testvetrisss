var DEL_FLAG = ""; var strRowID = "0"; var objItem;
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
        CallBackDevice.callback("L", objhdnID.value);
        CallBackPhys.callback("L", objhdnID.value);
        CallBackCred.callback("L", objhdnID.value);
    }
    objhdnError.value = "";

}
function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        btnBrwAddUI_Onclick('Masters/VRSInstitutionDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Masters/VRSInstitutionDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Masters/VRSInstitutionDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Masters/VRSInstitutionDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Masters/VRSInstitutionBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
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
    var ArrDevice = new Array();
    var ArrPhys = new Array();
    var ArrUser = new Array();

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
        ArrRecords[13] = objtxtContPerson.value;
        ArrRecords[14] = objtxtContMobile.value;
        ArrRecords[15] = objtxtNotes.value;
        ArrRecords[16] = objddlSalesPerson.value;
        ArrRecords[17] = UserID;
        ArrRecords[18] = MenuID;
        ArrDevice = GetDevices();
        ArrPhys = GetPhysicians();
        ArrUser = GetUsers();

        AjaxPro.timoutPeriod = 1800000;
        VRSInstitutionDlg.SaveRecord(ArrRecords, ArrDevice, ArrPhys, ArrUser,ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetDevices() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdDevice.get_table().getRow(itemIndex)) {

        arrRecords[idx] = gridItem.Data[1].toString();
        arrRecords[idx + 1] = gridItem.Data[2].toString();
        arrRecords[idx + 2] = gridItem.Data[3].toString();
        arrRecords[idx + 3] = gridItem.Data[4].toString();
        idx = idx + 4;
        itemIndex++;
    }
    return arrRecords;
}
function GetPhysicians() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[1].toString();
        arrRecords[idx + 1] = gridItem.Data[2].toString();
        arrRecords[idx + 2] = gridItem.Data[3].toString();
        arrRecords[idx + 3] = gridItem.Data[4].toString();
        arrRecords[idx + 4] = gridItem.Data[5].toString();
        arrRecords[idx + 5] = gridItem.Data[6].toString();
        idx = idx + 6;
        itemIndex++;
    }
    return arrRecords;
}
function GetUsers() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[1].toString();
        arrRecords[idx + 1] = gridItem.Data[2].toString();
        arrRecords[idx + 2] = gridItem.Data[3].toString();
        arrRecords[idx + 3] = gridItem.Data[4].toString();
        arrRecords[idx + 4] = gridItem.Data[5].toString();
        arrRecords[idx + 5] = gridItem.Data[6].toString();
        arrRecords[idx + 6] = gridItem.Data[7].toString();
        idx = idx + 7;
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
            CallBackDevice.callback("L", objhdnID.value);
            CallBackPhys.callback("L", objhdnID.value);
            CallBackCred.callback("L", objhdnID.value);
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
        VRSInstitutionDlg.FetchStates(ArrRecords, ShowProcess);
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

/**************DEVICES***************/
function btnAddDevice_OnClick() {
    var strDtls = "";
    DEVADD = "Y";
    strDtls = GetDeviceGridDetails();
    CallBackDevice.callback("A", strDtls);
}
function txtManf_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdDevice.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("txtManf_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtModality_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdDevice.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtModality_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtAETitle_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdDevice.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("txtAETitle_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function GetDeviceGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdDevice.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString();
        itemIndex++;
    }
    return strDtls;
}
function DeleteDeviceRow(ID) {
    strRowID = ID;
    DEL_FLAG = "DEVICE";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
/**************DEVICES***************/

/**************PHYSICIANS***************/
function btnAddPhys_OnClick() {
    var strDtls = "";
    PHYSADD = "Y";
    strDtls = GetPhysicianGridDetails();
    CallBackPhys.callback("A", strDtls);
}
function txtFname_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("txtFname_" + RowId).value;
            //objItem = gridItem;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtLname_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtLname_" + RowId).value;
            //objItem = gridItem;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtCred_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("txtCred_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtEmail_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("txtEmail_" + RowId).value;
           
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtMobile_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        strRowID = ID;

        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("txtMobile_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function DeletePhysicianRow(ID) {
    strRowID = ID;
    DEL_FLAG = "PHYSICIAN";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function GetPhysicianGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString() + SecDivider;
        strDtls += gridItem.Data[5].toString() + SecDivider;
        strDtls += gridItem.Data[6].toString();
        itemIndex++;
    }
    return strDtls;
}
/**************PHYSICIANS***************/

/**************USERS***************/
function btnAddCred_OnClick() {
    var strDtls = "";
    USERADD = "Y";
    strDtls = GetUserGridDetails();
    CallBackCred.callback("A", strDtls);
}   
function txtLoginID_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("txtLoginID_" + RowId).value;
            try {
                objItem = gridItem;
                strRowID = RowId;
                parent.PopupProcess("N");
                AjaxPro.timoutPeriod = 1800000;
                VRSInstitutionDlg.FetchUserDetails(document.getElementById("txtLoginID_" + RowId).value, ShowProcess);
            }
            catch (expErr) {
                parent.HideProcess();
                parent.PopupMessage(RootDirectory, strForm, "txtLoginID_OnChange()", expErr.message, "true");
            }

            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtPwd_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtPwd_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtPACSUser_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("txtPACSUser_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtPACSPwd_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("txtPACSPwd_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtUserEmail_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("txtUserEmail_" + RowId).value;

        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ChkStatus_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            if(document.getElementById("chkAct_" + RowId).checked) gridItem.Data[7] = "Y";
            else gridItem.Data[7] = "N";
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function DeleteUserRow(ID) {
    strRowID = ID;
    DEL_FLAG = "USER";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function PopulateUserDetails(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "PopulateUserDetails()", arrRes[1], "true");
            break;
        case "true":
            if (arrRes[1] != "00000000-0000-0000-0000-000000000000") {
                objItem.Data[1] = arrRes[1];
                objItem.Data[2] = arrRes[2]; document.getElementById("txtLoginID_" + strRowID).value = arrRes[2];
                objItem.Data[3] = arrRes[3]; document.getElementById("txtPwd_" + strRowID).value = arrRes[3];
                objItem.Data[4] = arrRes[4]; document.getElementById("txtPACSUser_" + strRowID).value = arrRes[4];
                objItem.Data[5] = arrRes[5]; document.getElementById("txtPACSPwd_" + strRowID).value = arrRes[5];
                objItem.Data[6] = arrRes[6]; document.getElementById("txtUserEmail_" + strRowID).value = arrRes[6];
                objItem.Data[7] = arrRes[7];
                document.getElementById("btnDelUser_" + strRowID).style.display = "none";
                document.getElementById("chkAct_" + strRowID).style.display = "inline";
                if (arrRes[6] == "Y") document.getElementById("chkAct_" + strRowID).checked = true; else document.getElementById("chkAct_" + strRowID).checked = false;
            }
            break;
    }

}
function GetUserGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString() + SecDivider;
        strDtls += gridItem.Data[5].toString() + SecDivider;
        strDtls += gridItem.Data[6].toString() + SecDivider;
        strDtls += gridItem.Data[7].toString();
        itemIndex++;
    }
    return strDtls;
}
/**************USER***************/

function DeleteRecord() {
    var strDtls = "";
    switch (DEL_FLAG) {
        case "DEVICE":
            strDtls = GetDeviceGridDetails();
            CallBackDevice.callback("D", strRowID, strDtls);
            break;
        case "PHYSICIAN":
            strDtls = GetPhysicianGridDetails();
            CallBackPhys.callback("D", strRowID, strDtls);
            break;
        case "USER":
            strDtls = GetUserGridDetails();
            CallBackCred.callback("D", strRowID, strDtls);
            break;
    }

}

function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "FetchStates":
            PopulateStateList(Result);
            break;
        case "FetchPhysicianDetails":
            PopulatePhysicianDetails(Result);
            break;
        case "FetchUserDetails":
            PopulateUserDetails(Result);
            break;
        case "SaveRecord":
            SaveRecord(Result);
            break;

    }
}