var Cid = ""; var Aid = ""; var Sid = ""; var Account = ""; var InvFile = ""; var InstId = "";

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

}



function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSUnfinalInvoice.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSUnfinalInvoice.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    Unlock = "Y";
    btnBrwClose_Onclick();
}

function btnOk_OnClick() {
    var ArrRecords = new Array();
    ArrRecords[0] = objddlBillingCycle.value;
    ArrRecords[1] = objddlAccount.value;
    ArrRecords[2] = MenuID;
    ArrRecords[3] = UserID;
    ArrRecords[4] = "V";
    CallBackInvoice.callback(ArrRecords);
    
}
function ddlBillingCycle_OnChange() {
    var rc = grdInvoice.get_recordCount();
    if (parseInt(rc) > 0) btnOk_OnClick();
}
function ddlAccount_OnChange() {
    var rc = grdInvoice.get_recordCount();
    if (parseInt(rc) > 0) btnOk_OnClick();
}
function btnUnfinal_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = UserID;
        ArrRecords[2] = MenuID;
        ArrAcct = GetAccounts();

        AjaxPro.timoutPeriod = 1800000;
        VRSUnfinalInvoice.BulkUnfinal(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnOk_OnClick()", expErr.message, "true");
    }

}
function ProcessBulkUnfinal(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessBulkUnfinal()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessBulkUnfinal()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ProcessBulkUnfinal()", arrRes[1], "false");
            btnOk_OnClick();
            parent.GsRetStatus = "false";
            break;
    }
}

/*******************************************************************************/
/*INVOICE HDR - BILLING ACCOUNT -START                                         */
/*******************************************************************************/
function UnfinalAccount(ID,Amt) {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrAcct = new Array();
    var ArrInst = new Array();
    var ArrStudy = new Array();

    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = UserID;
        ArrRecords[2] = MenuID;

        ArrAcct[0] = ID.toString();
        ArrAcct[1] = Amt.toString();

        AjaxPro.timoutPeriod = 1800000;
        VRSUnfinalInvoice.BulkUnfinal(ArrRecords, ArrAcct, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}

/*******************************************************************************/
/*INVOICE HDR - BILLING ACCOUNT - END                                          */
/*******************************************************************************/

/*******************************************************************************/
/*INVOICE INSTITUTION HDR - INSTITUTION -START                                 */
/*******************************************************************************/
//function UnfinalInstitution(AcctID,InstID) {
//    parent.PopupProcess("Y");
//    var ArrRecords = new Array();
//    var ArrAcct = new Array();
//    var ArrInst = new Array();
//    var ArrStudy = new Array();

//    try {
//        ArrRecords[0] = objddlBillingCycle.value;
//        ArrRecords[1] = UserID;
//        ArrRecords[2] = MenuID;
//        ArrAcct = GetAccountsUnfinal(AcctID);
//        ArrInst = GetInstitutionUnfinal(AcctID, InstID);
//        ArrStudy = GetStudiesUnfinal(AcctID, InstID);

//        AjaxPro.timoutPeriod = 1800000;
//        VRSUnfinalInvoice.SaveRecord(ArrRecords, ArrAcct, ArrInst, ArrStudy, ShowProcess);

//    }
//    catch (expErr) {
//        parent.HideProcess();
//        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
//    }
//}
//function GetInstitutionUnfinal(AcctID,InstID) {
//    var itemIndex = 0; var gridItem; var RowID = ""; var InstitutionID = "";
//    var arrRecords = new Array(); var idx = 0; var approved = "";
//    var ItemRecCountInst = 0;

//    while (gridItem = grdInvoice.get_table().getRow(itemIndex)) {
//        RowID = gridItem.Data[0].toString();
//        if (RowID == AcctID) {
//            if (gridItem.Data.length > 10) {
//                ItemRecCountInst = gridItem.Data[10].length;
//                for (var i = 0; i < ItemRecCountInst; i++) {
//                    InstitutionID = gridItem.Data[10][i][0].toString();
//                    if (InstitutionID == InstID) {
//                        arrRecords[idx] = gridItem.Data[10][i][0].toString();
//                        arrRecords[idx + 1] = gridItem.Data[10][i][1].toString();
//                        arrRecords[idx + 2] = gridItem.Data[10][i][7].toString();
//                        arrRecords[idx + 3] = "Y";
//                        idx = idx + 4;
//                        break;
//                    }
//                }
//            }
//            break;
//        }
//        itemIndex++;
//    }
//    return arrRecords;
//}
//function GetStudiesUnfinal(AcctID, InstID) {
//    var itemIndex = 0; var gridItem; var RowID = ""; var InstitutionID = "";
//    var arrRecords = new Array(); var idx = 0; var approved = "";
//    var ItemRecCountInst = 0; var ItemRecCountDtls = 0;

//    while (gridItem = grdInvoice.get_table().getRow(itemIndex)) {
//        RowID = gridItem.Data[0].toString();
//        if (RowID == AcctID) {
//            if (gridItem.Data.length > 10) {
//                ItemRecCountInst = gridItem.Data[10].length;
//                for (var i = 0; i < ItemRecCountInst; i++) {
//                    InstitutionID = gridItem.Data[10][i][0].toString();
//                    if (InstitutionID == InstID) {
//                        if (gridItem.Data[10][i].length > 10) {
//                            ItemRecCountDtls = gridItem.Data[10][i][12].length;
//                            for (var j = 0; j < ItemRecCountDtls; j++) {
//                                arrRecords[idx] = gridItem.Data[10][i][12][j][0].toString();
//                                arrRecords[idx + 1] = gridItem.Data[10][i][12][j][1].toString();
//                                arrRecords[idx + 2] = gridItem.Data[10][i][12][j][3].toString();
//                                arrRecords[idx + 3] = gridItem.Data[10][i][12][j][7].toString();
//                                arrRecords[idx + 4] = gridItem.Data[10][i][12][j][17].toString();
//                                arrRecords[idx + 5] = "Y";
//                                idx = idx + 6;
                                
//                            }
//                        }
//                        break;
//                    }

//                }
//            }
//            break;
//        }

//        itemIndex++;
//    }
//    return arrRecords;
//}
/*******************************************************************************/
/*INVOICE INSTITUTION HDR - INSTITUTION - END                                  */
/*******************************************************************************/

function GetAccounts() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0; var approved = "";

    while (gridItem = grdInvoice.get_table().getRow(itemIndex)) {
        approved = gridItem.Data[6].toString();
        if (approved == "N") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[5].toString();
            idx = idx + 2;
        }

        itemIndex++;
    }
    return arrRecords;
}



function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "FetchProcessStatus":
            ProcessStatus(Result);
            break;
        case "SaveRecord":
            SaveRecord(Result);
            break;
        case "BulkUnfinal":
            ProcessBulkUnfinal(Result);
            break;
    }
}