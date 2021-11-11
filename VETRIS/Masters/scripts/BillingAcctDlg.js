var strRowID = "0"; var objItem;
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
        CallBackInst.callback(objhdnID.value);
        CallBackContact.callback("L", objhdnID.value);
        CallBackPhys.callback("L", objhdnID.value);
        CallBackMF.callback(objhdnID.value, "N");
        CallBackSF.callback(objhdnID.value, "N");
    }
    objhdnError.value = "";
    parent.adjustFrameHeight();
}
function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        if (objhdnCF.value == "") btnBrwAddUI_Onclick('Masters/VRSBillingAcctDlg.aspx');
        else btnBrwAddUI_Onclick('Masters/VRSBillingAcctDlg.aspx?cf=' + objhdnCF.value);
    else {
        parent.GsDlgConfAction = "NEWUI";
        if (objhdnCF.value == "") parent.GsNavURL = "Masters/VRSBillingAcctDlg.aspx";
        else parent.GsNavURL = 'Masters/VRSBillingAcctDlg.aspx?cf=' + objhdnCF.value;
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        if (objhdnCF.value == "") btnBrwEditUI_Onclick('Masters/VRSBillingAcctDlg.aspx');
        else btnBrwEditUI_Onclick('Masters/VRSBillingAcctDlg.aspx?cf=' + objhdnCF.value);
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        if (objhdnCF.value == "") parent.GsNavURL = "Masters/VRSBillingAcctDlg.aspx";
        else if (objhdnCF.value == "MQ") parent.GsNavURL = 'Masters/VRSBillingAcctDlg.aspx?cf=' + objhdnCF.value;
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    if (objhdnCF.value == "") parent.GsNavURL = "Masters/VRSBillingAcctBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
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
    var ArrInst = new Array();
    var ArrCont = new Array();
    var ArrPhys = new Array();
    var ArrModFees = new Array();
    var ArrSvcFees = new Array();

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
        //ArrRecords[10]  = objtxtEmailID.value;
        //ArrRecords[11]  = objtxtTel.value;
        //ArrRecords[12]  = objtxtFax.value;
        //ArrRecords[13]  = objtxtContPerson.value;
        //ArrRecords[14]  = objtxtContMobile.value;
        //ArrRecords[15]  = objtxtContEmail.value;
        ArrRecords[10] = objtxtLoginID.value;
        ArrRecords[11] = objtxtPwd.value;
        ArrRecords[12] = objtxtLoginEmail.value;
        ArrRecords[13] = objtxtUserMobile.value;
        ArrRecords[14] = "B"; if (objrdoEmail.checked) ArrRecords[14] = "E"; else if (objrdoSMS.checked) ArrRecords[14] = "S";
        ArrRecords[15] = objddlSalesPerson.value;
        ArrRecords[16] = objtxtCommission1stYr.value;
        ArrRecords[17] = objtxtCommission2ndYr.value;
        //ArrRecords[18] = objtxtDisc.value;
        ArrRecords[18] = "0";
        ArrRecords[19] = objtxtAccName.value;
        ArrRecords[20] = UserID;
        ArrRecords[21] = MenuID;
        ArrInst = GetInstitutions();
        ArrCont = GetContacts();
        ArrPhys = GetPhysicians();
        ArrModFees = GetModalityFees();
        ArrSvcFees = GetServiceFees();

        AjaxPro.timoutPeriod = 1800000;
        VRSBillingAcctDlg.SaveRecord(ArrRecords, ArrInst, ArrCont, ArrPhys, ArrModFees, ArrSvcFees, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetInstitutions() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0; var sel = "";
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[5].toString();
        if (sel == "Y") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[3].toString();
            arrRecords[idx + 2] = gridItem.Data[4].toString();
            idx = idx + 3;
        }
        itemIndex++;
    }
    return arrRecords;
}
function GetContacts() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdContact.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();

        arrRecords[idx] = gridItem.Data[1].toString();
        arrRecords[idx + 1] = gridItem.Data[3];
        arrRecords[idx + 2] = gridItem.Data[4];
        arrRecords[idx + 3] = gridItem.Data[5];
        arrRecords[idx + 4] = gridItem.Data[6];
        arrRecords[idx + 5] = gridItem.Data[7];
        idx = idx + 6;

        itemIndex++;
    }
    return arrRecords;
}
function GetPhysicians() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0; var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        if (gridItem.Data.length > 3) {
            ItemRecCount = gridItem.Data[3].length;
            for (var i = 0; i < ItemRecCount; i++) {
                arrRecords[idx] = gridItem.Data[3][i][0].toString();
                arrRecords[idx + 1] = gridItem.Data[3][i][1];
                arrRecords[idx + 2] = gridItem.Data[3][i][2];
                arrRecords[idx + 3] = gridItem.Data[3][i][3];
                arrRecords[idx + 4] = gridItem.Data[3][i][4];
                arrRecords[idx + 5] = gridItem.Data[3][i][5];
                arrRecords[idx + 6] = gridItem.Data[3][i][6];
                idx = idx + 7;
            }
        }

        itemIndex++;
    }
    return arrRecords;
}
function GetModalityFees() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        arrRecords[idx] = gridItem.Data[0].toString();
        arrRecords[idx + 1] = gridItem.Data[1].toString();
        arrRecords[idx + 2] = gridItem.Data[10].toString();
        arrRecords[idx + 3] = gridItem.Data[11].toString();
        arrRecords[idx + 4] = gridItem.Data[12].toString();
        idx = idx + 5;
        itemIndex++;
    }
    return arrRecords;
}
function GetServiceFees() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        arrRecords[idx] = gridItem.Data[0].toString();
        arrRecords[idx + 1] = gridItem.Data[1].toString();
        arrRecords[idx + 2] = gridItem.Data[10].toString();
        arrRecords[idx + 3] = gridItem.Data[11].toString();
        idx = idx + 4;
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
            CallBackInst.callback(objhdnID.value);
            CallBackContact.callback("L", objhdnID.value);
            CallBackMF.callback(objhdnID.value, "N");
            CallBackSF.callback(objhdnID.value, "N");
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
        VRSBillingAcctDlg.FetchStates(ArrRecords, ShowProcess);
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

/**************INSTITUTIONS***************/
function chkSelInst_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "";

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();

        if (RowId == ID) {
            if (document.getElementById("chkSelInst_" + RowId.toString()).checked) {
                gridItem.Data[5] = "Y";
                UpdateContact(ID);
                UpdatePhysicians(ID);
            }
            else {
                gridItem.Data[5] = "N";
                DeleteContact(ID);
                DeletePhysician(ID);
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function chkSelCons_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "";

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();

        if (RowId == ID) {
            if (document.getElementById("chkSelCons_" + RowId.toString()).checked) {
                gridItem.Data[3] = "Y";
            }
            else {
                gridItem.Data[3] = "N";
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function chkSelStore_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "";

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();

        if (RowId == ID) {
            if (document.getElementById("chkSelStore_" + RowId.toString()).checked) {
                gridItem.Data[4] = "Y";
            }
            else {
                gridItem.Data[4] = "N";
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
/**************INSTITUTIONS***************/

function ApplyDefaultFee(Type) {
    if (Type == "M") CallBackMF.callback(objhdnID.value, "Y");
    else if (Type == "S") CallBackSF.callback(objhdnID.value, "Y");
}


/**************CONTACTS***************/
function UpdateContact(InstitutionID) {
    var strDtls = "";
    strDtls = GetContactGridDetails();
    CallBackContact.callback("U", strDtls, InstitutionID);
}
function txtPhone_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdContact.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtPhone_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtFax_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdContact.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("txtFax_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtContPer_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdContact.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("txtContPer_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtMobile_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdContact.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("txtMobile_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtEmail_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdContact.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[7] = document.getElementById("txtEmail_" + RowId).value;

        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function DeleteContact(InstitutionID) {
    strDtls = GetContactGridDetails();
    CallBackContact.callback("D", InstitutionID, strDtls);

}
function GetContactGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdContact.get_table().getRow(itemIndex)) {
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
/**************CONTACTS***************/
/**************PHYSICIANS***************/
function UpdatePhysicians(InstitutionID) {
    var strInstDtls = GetInstGridDetails();
    var strPhysDtls = GetPhysicianGridDetails();
    CallBackPhys.callback("U", strInstDtls, strPhysDtls, InstitutionID);
}
function txtFname_OnChange(InstID, PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = ""; var RowId = "";
    var ItemRecCount = 0;

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();

        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if (RowId == PhysID) {
                        gridItem.Data[3][i][2] = document.getElementById("txtFname_" + RowId).value;
                        break;
                    }
                }

                break;
            }

        }
        itemIndex++;

    }
    parent.GsRetStatus = "true";
}
function txtLname_OnChange(InstID, PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = ""; var RowId = "";
    var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if (RowId == PhysID) {
                        gridItem.Data[3][i][3] = document.getElementById("txtLname_" + RowId).value;
                        break;
                    }
                }
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtCred_OnChange(InstID, PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = ""; var RowId = "";
    var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if (RowId == PhysID) {
                        gridItem.Data[3][i][4] = document.getElementById("txtCred_" + RowId).value;
                        break;
                    }
                }
            }
            break;
        }
        itemIndex++;
    }

    parent.GsRetStatus = "true";
}
function txtPhysEmail_OnChange(InstID, PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = ""; var RowId = "";
    var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if (RowId == PhysID) {
                        gridItem.Data[3][i][5] = document.getElementById("txtPhysEmail_" + RowId).value;
                        break;
                    }
                }
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtPhysMobile_OnChange(InstID, PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = ""; var RowId = "";
    var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if (RowId == PhysID) {
                        gridItem.Data[3][i][6] = document.getElementById("txtPhysMobile_" + RowId).value;
                        break;
                    }
                }
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function DeletePhysician(InstitutionID) {
    var strInstDtls = GetInstGridDetails();
    var strPhysDtls = GetPhysicianGridDetails();
    CallBackPhys.callback("D", InstitutionID, strInstDtls, strPhysDtls);
}
function GetInstGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString();
        itemIndex++;
    }
    return strDtls;
}
function GetPhysicianGridDetails() {
    var strDtls = "";
    var itemIndex = 0; var ItemRecCount = 0;
    var gridItem;

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        if (gridItem.Data.length > 3) {
            ItemRecCount = gridItem.Data[3].length;
            for (var i = 0; i < ItemRecCount; i++) {
                if (strDtls != "") strDtls = strDtls + SecDivider;
                strDtls += gridItem.Data[3][i][0].toString() + SecDivider;
                strDtls += gridItem.Data[3][i][1].toString() + SecDivider;
                strDtls += gridItem.Data[3][i][2].toString() + SecDivider;
                strDtls += gridItem.Data[3][i][3].toString() + SecDivider;
                strDtls += gridItem.Data[3][i][4].toString() + SecDivider;
                strDtls += gridItem.Data[3][i][5].toString() + SecDivider;
                strDtls += gridItem.Data[3][i][6].toString();
            }
        }

        itemIndex++;
    }
    return strDtls;
}
/**************PHYSICIANS***************/
/**************MODALITY FEES***************/
function txtMFees_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdMF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[10] = document.getElementById("txtMFees_" + RowId).value;
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
            gridItem.Data[11] = document.getElementById("txtAddOn_" + RowId).value;
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
            gridItem.Data[12] = document.getElementById("txtMaxFee_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
/**************MODALITY FEES***************/
/**************SERVICE FEES***************/
function txtSFees_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[10] = document.getElementById("txtSFees_" + RowId).value;
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
            gridItem.Data[11] = document.getElementById("txtSFeesAH_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
/**************SERVICE FEES***************/


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
function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

}