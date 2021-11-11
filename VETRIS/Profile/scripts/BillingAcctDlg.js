var strRowID = ""; var objItem; var PhysicianID = "";
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
    }
    objhdnError.value = "";

}

function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Profile/VRSBillingAcctDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Profile/VRSBillingAcctDlg.aspx';
        parent.PopupConfirm("028");
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

function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords  = new Array();
    var ArrCont = new Array();
    var ArrPhys = new Array();

    try {
        ArrRecords[0]   = objhdnID.value;
        ArrRecords[1] = objtxtLoginID.value;
        ArrRecords[2] = objtxtPwd.value;
        ArrRecords[3] = objtxtLoginEmail.value;
        ArrRecords[4] = objtxtUserMobile.value;
        ArrRecords[5] = "B"; if (objrdoEmail.checked) ArrRecords[5] = "E"; else if (objrdoSMS.checked) ArrRecords[5] = "S";
        ArrRecords[6]  = UserID;
        ArrRecords[7]  = MenuID;
        ArrCont         = GetContacts();
        ArrPhys         = GetPhysicians();

        
        AjaxPro.timoutPeriod = 1800000;
        VRSBillingAcctDlg.SaveRecord(ArrRecords, ArrCont,ArrPhys,ShowProcess);
        
        
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
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
            CallBackContact.callback("L", objhdnID.value);
            parent.GsRetStatus = "false";
            break;
    }
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
            itemIndex++;
        }
        parent.GsRetStatus = "true";
    }
}
function txtLname_OnChange(InstID,PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = "";var RowId = "";
    var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if(RowId == PhysID)
                    {
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
function txtCred_OnChange(InstID,PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = "";var RowId = "";
    var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if(RowId == PhysID)
                    {
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
function txtPhysEmail_OnChange(InstID,PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = "";var RowId = "";
    var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if(RowId == PhysID)
                    {
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
function txtPhysMobile_OnChange(InstID,PhysID) {
    var itemIndex = 0; var gridItem; var HdrRowId = "";var RowId = "";
    var ItemRecCount = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == InstID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if(RowId == PhysID)
                    {
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
function btnEditPhysEmail_OnClick(ID, PhysID) {

    var itemIndex = 0; var gridItem; var HdrRowId = ""; var RowId = "";
    var ItemRecCount = 0;

    parent.GiWidth = 800;
    parent.GiTop = 30;
    strRowID = ID;
    PhysicianID = PhysID;


    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.Data[0].toString();
        if (HdrRowId == ID) {
            if (gridItem.Data.length > 3) {
                ItemRecCount = gridItem.Data[3].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[3][i][1].toString();

                    if (RowId == PhysID) {
                        parent.GsText = gridItem.Data[3][i][5].toString();
                        parent.GsLaunchURL = "Profile/VRSPhysEmailID.aspx";
                        parent.PopupDataList();
                        break;
                    }
                }
            }
            break;
        }
        itemIndex++;
    }
}
/**************PHYSICIANS***************/
function ProcessDataList(Args) {
    if (Args != "") {
        var itemIndex = 0; var gridItem; var HdrRowId = ""; var RowId = "";
        var ItemRecCount = 0;
        while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
            HdrRowId = gridItem.Data[0].toString();
            if (HdrRowId == strRowID) {
                if (gridItem.Data.length > 3) {
                    ItemRecCount = gridItem.Data[3].length;
                    for (var i = 0; i < ItemRecCount; i++) {
                        RowId = gridItem.Data[3][i][1].toString();

                        if (RowId == PhysicianID) {
                            gridItem.Data[3][i][5] = Args;
                            if(document.getElementById("txtPhysEmail_" + RowId) != null) document.getElementById("txtPhysEmail_" + RowId).value = Args;
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
function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

}