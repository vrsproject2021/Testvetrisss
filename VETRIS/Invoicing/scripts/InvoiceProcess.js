var Cid = ""; var Aid = ""; var Sid = ""; var Account = ""; var InvFile = ""; var InstId = "";
var SINGLE_SEL = "N";

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
    if (objddlBillingCycle.value != "00000000-0000-0000-0000-000000000000") ddlBillingCycle_OnChange();

}
function btnClose_OnClick() {
    Unlock = "Y";
    btnBrwClose_Onclick();
}
function ddlBillingCycle_OnChange() {
    CallBackBA.callback(objddlBillingCycle.value);
    CallBackBAProc.callback(objddlBillingCycle.value,"A", MenuID, UserID);
}

/*******************************************************************************/
/*PENDING PROCESSING LIST - START                                              */
/*******************************************************************************/
function btnSel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdBA.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            document.getElementById("btnCheck_" + RowId).style.display = "inline";
            document.getElementById("btnSel_" + RowId).style.display = "none";
            gridItem.Data[3] = "Y";
            //objbtnProcess.style.display = "inline";
            break;
        }
       
        itemIndex++;
    }
}
function btnCheck_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var rc = grdBA.get_recordCount();
    var cnt = 0; 

    while (gridItem = grdBA.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            document.getElementById("btnCheck_" + RowId).style.display = "none";
            document.getElementById("btnSel_" + RowId).style.display = "inline";
            gridItem.Data[3] = "N";
            cnt = cnt + 1;
        }
        else {
            if (gridItem.Data[3] == "N") {
                cnt = cnt + 1;
            }
        }

        itemIndex++;
    }

    //if (cnt == rc) { objbtnProcess.style.display = "none"; }
}
function btnProcBA_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowID = "";

    while (gridItem = grdBA.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        
        if (RowID == ID) {
            gridItem.Data[3] = "Y";
            SINGLE_SEL = "Y";
            break;
        }

        itemIndex++;
    }

    btnProcess_OnClick();
}
function btnProcess_OnClick() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    var ArrAcct = new Array();

    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = MenuID;
        ArrRecords[2] = UserID;
        ArrRecords[3] = SessionID;
        ArrAcct       = GetAccounts();

        AjaxPro.timoutPeriod = 1800000;
        VRSInvoiceProcess.ProcessInvoice(ArrRecords, ArrAcct,ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnProcess_OnClick()", expErr.message, "true");
    }
}
function GetAccounts() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var sel = ""; var idx = 0;

    while (gridItem = grdBA.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[3].toString();
        if (sel == "Y") {
            arrRecords[idx] = gridItem.Data[0].toString();
            idx = idx + 1;
            if (SINGLE_SEL == "Y") {
                SINGLE_SEL = "N";
                break;
            }
        }

        itemIndex++;
    }
    return arrRecords;
}
function ProcessInvoice(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessInvoice()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessInvoice()", arrRes[1], "true", arrRes[2]);
            ddlBillingCycle_OnChange();
            break;
        case "true":
            if(arrRes[1] != "") parent.PopupMessage(RootDirectory, strForm, "ProcessInvoice()", arrRes[1], "false");
            ddlBillingCycle_OnChange();
            parent.GsRetStatus = "false";
            break;
    }
}
/*******************************************************************************/
/*PENDING PROCESSING LIST -END                                                 */
/*******************************************************************************/

/*******************************************************************************/
/*PROCESSED LIST - START                                                       */
/*******************************************************************************/
function FilterApproved() {
    CallBackBAProc.callback(objddlBillingCycle.value, "Y", MenuID, UserID);
}
function FilterNotApproved() {
    CallBackBAProc.callback(objddlBillingCycle.value, "N", MenuID, UserID);
}
function btnPrintAcct_OnClick(AccountID) {
    var CycleID = objddlBillingCycle.value;
    parent.GsFileType = "PDF";
    parent.GsLaunchURL = "Invoicing/DocumentPrinting/VRSDocPrint.aspx?DocID=2&CYCLE=" + CycleID + "&ACCT=" + AccountID + "&UID=" + UserID;
    parent.PopupReportViewer();
}
function btnView_OnClick(AccountID) {
    parent.PopupProcess("N");
    parent.GsPopupText = "Checking record lock";
    var ArrRecords = new Array();

    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = AccountID;
        ArrRecords[2] = SessionID;
        ArrRecords[3] = UserID;
        ArrRecords[4] = MenuID;

        Aid = AccountID;

        AjaxPro.timoutPeriod = 1800000;
        VRSInvoiceProcess.CheckRecordLock(ArrRecords, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnView_OnClick()", expErr.message, "true");
    }
}
function ProcessCheckRecordLock(Result) {

    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessInvoice()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessInvoice()", arrRes[1], "true", arrRes[2]);
            ddlBillingCycle_OnChange();
            break;
        case "true":
            var CycleID = objddlBillingCycle.value;
            var CycleName = objddlBillingCycle.options[objddlBillingCycle.selectedIndex].text;
            var URL = "Invoicing/VRSInvoiceProcessView.aspx";
            parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=00000000-0000-0000-0000-000000000000&bcid=" + CycleID + "&cnm=" + CycleName + "&aid=" + Aid + "&sid=" + SessionID + "&th=" + selTheme;
            break;
    }
}
function Approve(ID)
{
    var itemIndex = 0; var gridItem; var RowID = "";

    while (gridItem = grdBAProc.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();

        if (RowID == ID) {
            gridItem.Data[7] = "Y";
            SINGLE_SEL = "Y";
            break;
        }

        itemIndex++;
    }

    btnApprove_OnClick();
}
function btnApprove_OnClick() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    var ArrAcct = new Array();

    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = UserID;
        ArrRecords[2] = MenuID;
        ArrRecords[3] = SessionID;
        ArrAcct = GetProcessedAccounts();

        AjaxPro.timoutPeriod = 1800000;
        VRSInvoiceProcess.ApproveInvoice(ArrRecords, ArrAcct, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnProcess_OnClick()", expErr.message, "true");
    }
}
function GetProcessedAccounts() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var sel = ""; var idx = 0;

    while (gridItem = grdBAProc.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[7].toString();
        if (sel == "Y") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[3][0].toString();
            idx = idx + 2;
            if (SINGLE_SEL == "Y") {
                SINGLE_SEL = "N";
                break;
            }
        }

        itemIndex++;
    }
    return arrRecords;
}
function ProcessApproveInvoice(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ApproveInvoice()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ApproveInvoice()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "ApproveInvoice()", arrRes[1], "false");
            ddlBillingCycle_OnChange();
            parent.GsRetStatus = "false";
            break;
    }
}
function btnSelProc_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdBAProc.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            document.getElementById("btnCheckProc_" + RowId).style.display = "inline";
            document.getElementById("btnSelProc_" + RowId).style.display = "none";
            gridItem.Data[7] = "Y";
            break;
        }

        itemIndex++;
    }
}
function btnCheckProc_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var rc = grdBA.get_recordCount();
    var cnt = 0;

    while (gridItem = grdBAProc.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            document.getElementById("btnCheckProc_" + RowId).style.display = "none";
            document.getElementById("btnSelProc_" + RowId).style.display = "inline";
            gridItem.Data[7] = "N";
            cnt = cnt + 1;
        }
        else {
            if (gridItem.Data[7] == "N") {
                cnt = cnt + 1;
            }
        }

        itemIndex++;
    }
}
function btnEmail_OnClick(AccountID, AccountName) {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    try {
        ArrRecords[0] = Cid = objddlBillingCycle.value;
        ArrRecords[1] = objddlBillingCycle.options[objddlBillingCycle.selectedIndex].text;
        ArrRecords[2] = Aid = AccountID;
        ArrRecords[3] = Account = AccountName;
        ArrRecords[4] = UserID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSInvoiceProcess.GenerateBillingAccountInvoice(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnEmail_OnClick()", expErr.message, "true");
    }
}
function ProcessAcctInvoice(Result) {
    var arrRes = new Array();
    var Cycle = objddlBillingCycle.options[objddlBillingCycle.selectedIndex].text;
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessAcctInvoice()", arrRes[1], "true");
            break;
        case "true":
            parent.GiWidth = 400;
            parent.GiTop = 15;
            InvFile = arrRes[2];
            parent.GsLaunchURL = "Invoicing/VRSMailInvoice.aspx?cycid=" + Cid + "&acctid=" + Aid + "&inv=" + arrRes[1] + "&type=A" + "&uid=" + UserID + "&th=" + selTheme;
            parent.PopupDataList();
            break;
    }
}
function ProcessDataList(Args) {
    try {
        var Result = VRSInvoiceProcess.DeleteInvoiceFile(InvFile);
    }
    catch (expErr) {; }
}
function btnReProcBA_OnClick(AccountID) {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    var ArrStudy = new Array();
    var ArrRates = new Array();

    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = AccountID;
        ArrRecords[2] = UserID;
        ArrRecords[3] = MenuID;

        AjaxPro.timoutPeriod = 1800000;
        VRSInvoiceProcess.ReprocessInvoice(ArrRecords, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnReProcBA_OnClick()", expErr.message, "true");
    }
}
function ReprocessInvoice(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ReprocessInvoice()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ReprocessInvoice()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            CallBackBAProc.callback(objddlBillingCycle.value, "A", MenuID, UserID);
            break;
    }
}
/*******************************************************************************/
/*PROCESSED LIST - END                                                         */
/*******************************************************************************/
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "ProcessInvoice":
            ProcessInvoice(Result);
            break;
        case "ApproveInvoice":
            ProcessApproveInvoice(Result);
            break;
        case "CheckRecordLock":
            ProcessCheckRecordLock(Result);
            break;
        case "GenerateBillingAccountInvoice":
            ProcessAcctInvoice(Result);
            break;
        case "ReprocessInvoice":
            ReprocessInvoice(Result);
            break;
    }
}