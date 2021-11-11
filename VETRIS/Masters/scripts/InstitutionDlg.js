var DEL_FLAG = ""; var strRowID = "0"; var objItem; var VALIDATED = "N";
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
        if(objddlState.value==0) ddlCountry_OnChange();
        CallBackDevice.callback("L", objhdnID.value);
        CallBackPhys.callback("L", objhdnID.value);
        CallBackCred.callback("L", objhdnID.value);
        //CallBackFees.callback(objhdnID.value);
        CallBackTags.callback(objhdnID.value);
        CallBackInsCtg.callback(objhdnID.value);
        CallBackPromo.callback(objhdnID.value);
        CallBackInst.callback(objhdnID.value);
    }
    objhdnError.value = "";
    LinkExistingAccount();
}

function btnNew_OnClick() {
    if (parent.GsRetStatus == "false") {
        if (objhdnCF.value == "") btnBrwAddUI_Onclick('Masters/VRSInstitutionDlg.aspx');
        else btnBrwAddUI_Onclick('Masters/VRSInstitutionDlg.aspx?cf=' + objhdnCF.value);
    }
    else {
        parent.GsDlgConfAction = "NEWUI";
        if (objhdnCF.value == "") parent.GsNavURL = "Masters/VRSInstitutionDlg.aspx";
        else parent.GsNavURL = 'Masters/VRSInstitutionDlg.aspx?cf=' + objhdnCF.value;
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        if (objhdnCF.value == "") btnBrwEditUI_Onclick('Masters/VRSInstitutionDlg.aspx');
        else btnBrwEditUI_Onclick('Masters/VRSInstitutionDlg.aspx?cf=' + objhdnCF.value);
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        if (objhdnCF.value == "") parent.GsNavURL = "Masters/VRSInstitutionDlg.aspx";
        else parent.GsNavURL = 'Masters/VRSInstitutionDlg.aspx?cf=' + objhdnCF.value;
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    if (objhdnCF.value == "") parent.GsNavURL = "Masters/VRSInstitutionBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    else if (objhdnCF.value == "MQ") parent.GsNavURL = "Masters/VRSMasterQuery.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=71";

    if (parent.GsRetStatus == "false") {
        if (objhdnCF.value == "MQ") parent.objhdnMenuID.value = "71";
        btnDlgClose_Onclick();
    }
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
    var ArrFees = new Array();
    var ArrTags = new Array();
    var ArrCategories = new Array();

    var ArrAltNames = new Array();

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
        ArrRecords[12] = objtxtFax.value;
        ArrRecords[13] = objtxtContPerson.value;
        ArrRecords[14] = objtxtContMobile.value;
        ArrRecords[15] = objtxtNotes.value;
        ArrRecords[16] = "Y"; if (objrdoBANo.checked) ArrRecords[16] = "N";
        ArrRecords[17] = objddlBA.value;
        //ArrRecords[16]  = objddlSalesPerson.value;
        //ArrRecords[17]  = objtxtCommission1stYr.value;// Added on 4th SEP 2019 @BK
        //ArrRecords[18]  = objtxtCommission2ndYr.value;// Added on 4th SEP 2019 @BK
        //ArrRecords[19]  = objtxtDisc.value;
        //ArrRecords[19]  = objtxtAccName.value;// Added on 3rd SEP 2019 @BK
        ArrRecords[18] = objddlInfoSrc.value;
        ArrRecords[19] = "N"; if (objrdoFmtYes.checked) ArrRecords[19] = "Y";
        ArrRecords[20] = "N"; if (objrdoAuto.checked) ArrRecords[20] = "A"; else if (objrdoManual.checked) ArrRecords[20] = "M";
        ArrRecords[21] = objtxtRecPath.value;
        ArrRecords[22] = "N"; if (objrdoConsultY.checked) ArrRecords[22] = "Y";
        ArrRecords[23] = "N"; if (objrdoStoreY.checked) ArrRecords[23] = "Y";
        ArrRecords[24] = "N"; if (objrdoCustRptY.checked) ArrRecords[24] = "Y";
        ArrRecords[25] = "N"; if (objrdoCompXferY.checked) ArrRecords[25] = "Y";
        ArrRecords[26] = "N"; if (objrdoFaxRptY.checked) ArrRecords[26] = "Y";
        ArrRecords[27] = "P"; if (objrdoRFRTF.checked) ArrRecords[27] = "R"; else if (objrdoRFBoth.checked) ArrRecords[27] = "B";

        ArrRecords[28] = objhdnFilename.value;
        ArrRecords[29] = UserID;
        ArrRecords[30] = MenuID;
        ArrRecords[31] = objhdnUsrUpdUrl.value;

        ArrDevice = GetDevices();
        ArrPhys = GetPhysicians();
        ArrUser = GetUsers();
        // ArrFees         = GetFees();
        ArrTags = GetTags();
        ArrCategories = GetStudyTypeCategories();
        ArrAltNames = GetAlternateNames();

        //if (VALIDATED == "Y") {
        AjaxPro.timoutPeriod = 1800000;
        VRSInstitutionDlg.SaveRecord(ArrRecords, ArrDevice, ArrPhys, ArrUser, ArrTags,ArrCategories, ArrAltNames,ShowProcess);
        //}
        //else 
        //    parent.HideProcess();
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
        arrRecords[idx + 4] = gridItem.Data[5].toString();//Added on 2nd SEP 2019 @BK
        idx = idx + 5;
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
        arrRecords[idx + 7] = gridItem.Data[8].toString();
        idx = idx + 8;
        itemIndex++;
    }
    return arrRecords;
}
function GetTags() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0; var sel = "";
    while (gridItem = grdTags.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[1].toString();
        if (sel == "Y") {
            arrRecords[idx] = gridItem.Data[2].toString();
            arrRecords[idx + 1] = gridItem.Data[3].toString();
            arrRecords[idx + 2] = gridItem.Data[5].toString();
            arrRecords[idx + 3] = gridItem.Data[6].toString();
            idx = idx + 4;
        }
        itemIndex++;
    }
    return arrRecords;
}
function GetStudyTypeCategories() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0; var sel = "";
    while (gridItem = grdInsCategory.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[1].toString();
        if (sel == "Y") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = objhdnID.value.toString();
            idx = idx + 2;
        }
        itemIndex++;
    }
    return arrRecords;
}
function GetAlternateNames() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0; var sel = "";
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[3].toString();
        if (sel == "Y") {
            arrRecords[idx] = gridItem.Data[1].toString();
            arrRecords[idx + 1] = gridItem.Data[2].toString();
            idx = idx + 2;
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
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            objhdnID.value = arrRes[2];
            objtxtCode.value = arrRes[3];
            CallBackDevice.callback("L", objhdnID.value);
            CallBackPhys.callback("L", objhdnID.value);
            CallBackCred.callback("L", objhdnID.value);
            CallBackTags.callback(objhdnID.value);
            CallBackInsCtg.callback(objhdnID.value);
            CallBackInst.callback(objhdnID.value);
            VALIDATED = "N";
            FetchBillingAccounts();
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
function LinkExistingAccount() {
    if (objrdoBAYes.checked) objddlBA.disabled = false;
    else if (objrdoBANo.checked) {
        objddlBA.value = "00000000-0000-0000-0000-000000000000";
        objddlBA.disabled = true;
    }

}
function FetchBillingAccounts() {
    var arrRes = new Array();
    try {
        AjaxPro.timoutPeriod = 1800000;
        var Result = VRSInstitutionDlg.FetchBillingAccounts(objhdnID.value);
        arrRes = Result.value;

        switch (arrRes[0]) {
            case "catch":
                parent.PopupMessage(RootDirectory, strForm, "PopuplateBillingAccounts()", arrRes[1], "true");
                break;
            case "false":
                parent.PopupMessage(RootDirectory, strForm, "PopuplateBillingAccounts()", arrRes[1], "true", arrRes[2]);
                break;
            case "true":
                objddlBA.length = 0;
                var op = document.createElement("option");
                op.value = "00000000-0000-0000-0000-000000000000";
                op.text = "Select One";
                objddlBA.add(op);

                for (var i = 1; i < (arrRes.length - 1) ; i = i + 2) {

                    var op = document.createElement("option");
                    op.value = arrRes[i];
                    op.text = arrRes[i + 1];
                    objddlBA.add(op);
                }
                objddlBA.value = arrRes[arrRes.length - 1];
                objrdoBAYes.checked = true;
                LinkExistingAccount();
                break;
        }
    }
    catch (expErr) {
        parent.PopupMessage(RootDirectory, strForm, "FetchBillingAccounts()", expErr.message, "true");
    }
}
function UpdateTagFormat() {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdTags.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (document.getElementById("chkSelTag_" + RowId) != null) {
            if (objrdoFmtYes.checked) document.getElementById("chkSelTag_" + RowId).disabled = false;
            else {
                document.getElementById("chkSelTag_" + RowId).checked = false;
                document.getElementById("chkSelTag_" + RowId).disabled = true;
            }
        }
        itemIndex++;
    }

}

function chkSelInst_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            if (document.getElementById("chkSelInst_" + RowId).checked) gridItem.Data[3] = "Y";
            else gridItem.Data[3] = "N";
        }
        
        itemIndex++;
    }
    parent.GsRetStatus = "true";
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
        strDtls += gridItem.Data[4].toString() + SecDivider;
        strDtls += gridItem.Data[5].toString();//Added on 2nd SEP 2019 @BK
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
function ddlWeightUOM_OnChange(ID) {

    var gridItem;
    var itemIndex = 0;
    var RowId = "0";
    while (gridItem = grdDevice.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString()

        if (RowId == ID) {
            var uom = document.getElementById("ddlWeightUOM_" + ID);
            gridItem.Data[5] = uom.options[uom.selectedIndex].text;;
            break;
        }

        itemIndex++;
    }
}
/**************DEVICES***************/

/**************PHYSICIANS***************/
function btnAddPhys_OnClick() {
    var strDtls = "";
    PHYSADD = "Y";
    if (CheckBlankRow()) {
        strDtls = GetPhysicianGridDetails();
        CallBackPhys.callback("A", strDtls);
    }
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
function CheckBlankRow() {
    var bRet = true; var itemIndex = 0;var gridItem;
    var strFN = ""; var strLN = ""; var strEmailID = ""; var RowID = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowID = parseInt(gridItem.Data[0].toString());
        strFN = parent.Trim(gridItem.Data[2].toString());
        strLN = parent.Trim(gridItem.Data[3].toString());
        strEmailID = parent.Trim(gridItem.Data[5].toString());

        if ((strFN == "" && strLN == "") || strEmailID=="")
        {
            parent.PopupMessage(RootDirectory, strForm, "CheckBlankRow()", "351", "true",RowID.toString());
            bRet = false;
            break;
        }
        itemIndex++;
    }

    return bRet;
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
    var strItemVal;

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("txtLoginID_" + RowId).value;
            strItemVal = document.getElementById("txtLoginID_" + RowId).value;
            gridItem.Data[4] = strItemVal;
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
    var strItemVal;

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtPwd_" + RowId).value;
            strItemVal = document.getElementById("txtPwd_" + RowId).value;
            gridItem.Data[5] = strItemVal;
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
function txtUserContact_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdCred.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[7] = document.getElementById("txtUserContact_" + RowId).value;

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
            if (document.getElementById("chkAct_" + RowId).checked) gridItem.Data[8] = "Y";
            else gridItem.Data[8] = "N";
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
                objItem.Data[7] = arrRes[7]; document.getElementById("txtUserContact_" + strRowID).value = arrRes[7];
                objItem.Data[8] = arrRes[8];
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
        strDtls += gridItem.Data[7].toString() + SecDivider;
        strDtls += gridItem.Data[8].toString();
        itemIndex++;
    }
    return strDtls;
}
/**************USER***************/

/**************FEES***************/
function txtFees_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdFees.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[8] = document.getElementById("txtFees_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
/**************FEES***************/

/**************TAGS***************/
function chkSelTag_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdTags.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            if (document.getElementById("chkSelTag_" + RowId).checked)
                gridItem.Data[1] = "Y";
            else
                gridItem.Data[1] = "N";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtDefVal_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdTags.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("txtDefVal_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtJunkChar_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdTags.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("txtJunkChar_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
/**************TAGS***************/

function chkSelInsCtg_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdInsCategory.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            if (document.getElementById("chkSelInsCtg_" + RowId).checked)
                gridItem.Data[1] = "Y";
            else
                gridItem.Data[1] = "N";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}

function btnUploadLogo_OnClick() {
    var strDir = objhdnFilePath.value + "/Masters/Logo/Temp/";
    var imgId = createGuid();
    parent.parent.GsLaunchURL = "Common/VRSUploader.aspx?dir=" + strDir + "&t=IMG&fileID=" + UserID + "_Logo_" + imgId + "&th=" + selTheme;
    parent.parent.PopupUpload();
}
function btnDelLogo_OnClick() {
    var strDelFileName = objhdnFilename.value;
    objhdnFilename.value = "NoLogo.jpg";
    if (strDelFileName == objhdnFilename.value) strDelFileName = "";
    CallBackLogo.callback("~/Masters/Logo/Temp/NoLogo.jpg", strDelFileName);
}
function createGuid() {
    //return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function ProcessUpload(ArgsRet) {
    if (ArgsRet != null) {
        objhdnFilename.value = ArgsRet[0];
        CallBackLogo.callback("~/Masters/Logo/Temp/" + ArgsRet[0], "");
    }
   
}

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
function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

}