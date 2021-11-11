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
    PopulateRecords();

}
function PopulateRecords() {
    var ArrRecords = new Array();
    ArrRecords[0] = objhdnBCID.value;
    ArrRecords[1] = objhdnAID.value;
    ArrRecords[2] = MenuID;
    ArrRecords[3] = UserID;
    ArrRecords[4] = SessionID;
    CallBackInvoice.callback(ArrRecords);
}
function btnClose_OnClick() {
    var CycleID = objhdnBCID.value;
    var URL = "Invoicing/VRSInvoiceProcess.aspx";
    parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=00000000-0000-0000-0000-000000000000&bcid=" + CycleID + "&sid=" + SessionID + "&th=" + selTheme;
}

/*******************************************************************************/
/*INVOICE HDR - BILLING ACCOUNT -START                                         */
/*******************************************************************************/
function ApproveAcct(ID) {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrAcct = new Array();

    try {
        ArrRecords[0] = objhdnBCID.value;
        ArrRecords[1] = UserID;
        ArrRecords[2] = MenuID;
        ArrRecords[3] = SessionID;
        ArrAcct = GetAccountsApproved(ID);


        AjaxPro.timoutPeriod = 1800000;
        VRSInvoiceProcessView.ApproveInvoice(ArrRecords, ArrAcct,  ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetAccountsApproved(ID) {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0; var approved = "";

    while (gridItem = grdInvoice.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        if (RowID == ID) {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[5].toString();
            idx = idx + 2;
            break;
        }

        itemIndex++;
    }
    return arrRecords;
}
function btnPrintAcct_OnClick(CycleID, AccountID) {
    parent.GsFileType = "PDF";
    parent.GsLaunchURL = "Invoicing/DocumentPrinting/VRSDocPrint.aspx?DocID=2&CYCLE=" + CycleID + "&ACCT=" + AccountID + "&UID=" + UserID;
    parent.PopupReportViewer();
}
function btnAcctEmail_OnClick(CycleID, AccountID, AccountName) {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    try {
        ArrRecords[0] = Cid = CycleID;
        ArrRecords[1] = objddlBillingCycle.options[objddlBillingCycle.selectedIndex].text;
        ArrRecords[2] = Aid = AccountID;
        ArrRecords[3] = Account = AccountName;
        ArrRecords[4] = UserID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSInvoiceProcessView.GenerateBillingAccountInvoice(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnAcctEmail_OnClick()", expErr.message, "true");
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
/*******************************************************************************/
/*INVOICE HDR - BILLING ACCOUNT - END                                          */
/*******************************************************************************/

/*******************************************************************************/
/*INVOICE INSTITUTION HDR - INSTITUTION -START                                 */
/*******************************************************************************/
function ApproveInstitution(AcctID, InstID) {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrAcct = new Array();

    try {
        ArrRecords[0] = objhdnBCID.value;
        ArrRecords[1] = UserID;
        ArrRecords[2] = MenuID;
        ArrRecords[3] = SessionID;
        ArrAcct = GetAccountsApproved(AcctID);
        

        AjaxPro.timoutPeriod = 1800000;
        VRSInvoiceProcessView.ApproveInvoice(ArrRecords, ArrAcct, ArrInst, ArrStudy, ShowProcess);

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function btnPrintInst_OnClick(CycleID, InstitutionID) {
    parent.GsFileType = "PDF";
    parent.GsLaunchURL = "Invoicing/DocumentPrinting/VRSDocPrint.aspx?DocID=1&CYCLE=" + CycleID + "&INST=" + InstitutionID + "&UID=" + UserID;
    parent.PopupReportViewer();
}
function btnInstEmail_OnClick(CycleID, AccountID, InstitutionID, InstitutionName) {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {
        ArrRecords[0] = Cid = CycleID;
        ArrRecords[1] = objddlBillingCycle.options[objddlBillingCycle.selectedIndex].text;
        ArrRecords[2] = Aid = AccountID;
        ArrRecords[3] = InstId = InstitutionID;
        ArrRecords[4] = InstitutionName;
        ArrRecords[5] = UserID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSInvoiceProcessView.GenerateInstitutionInvoice(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnEmail_OnClick()", expErr.message, "true");
    }
}
function ProcessInstInvoice(Result) {
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
            parent.GsLaunchURL = "Invoicing/VRSMailInvoice.aspx?cycid=" + Cid + "&acctid=" + Aid + "&instid=" + InstId + "&inv=" + arrRes[1] + "&type=I" + "&uid=" + UserID;
            parent.PopupDataList();
            break;
    }
}
function btnEditInst_OnClick(AcctID, InstID) {
    var URL = "Invoicing/VRSStudyAmend.aspx";
    parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=00000000-0000-0000-0000-000000000000&bcid=" + objhdnBCID.value + "&aid=" + AcctID + "&iid=" + InstID + "&cf=IP" + "&th=" + selTheme;
}
/*******************************************************************************/
/*INVOICE INSTITUTION HDR - INSTITUTION - END                                  */
/*******************************************************************************/
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
            PopulateRecords();
            parent.GsRetStatus = "false";
            break;
    }
}
function ProcessDataList(Args) {
    try {
        var Result = VRSInvoiceProcessView.DeleteInvoiceFile(InvFile);
    }
    catch (expErr) {; }
}
function SendEmail(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SendEmail()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SendEmail()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SendEmail()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "ApproveInvoice":
            ProcessApproveInvoice(Result);
            break;
        case "GenerateBillingAccountInvoice":
            ProcessAcctInvoice(Result);
            break;
        case "GenerateInstitutionInvoice":
            ProcessInstInvoice(Result);
            break;
    }
}