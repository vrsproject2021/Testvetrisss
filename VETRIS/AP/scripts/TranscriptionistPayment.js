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

function btnApprove_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrTranscriptionist = new Array();

    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = UserID;
        ArrRecords[2] = MenuID;
        ArrTranscriptionist = GetTranscriptionists();


        AjaxPro.timoutPeriod = 1800000;
        VRSTranscriptionistPayment.SaveRecord(ArrRecords, ArrTranscriptionist, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetTranscriptionists() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0; var approved = "";

    while (gridItem = grdPayment.get_table().getRow(itemIndex)) {
        approved = gridItem.Data[5].toString();
        if (approved == "N") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[4].toString();
            arrRecords[idx + 2] = "Y";
            idx = idx + 3;
        }

        itemIndex++;
    }
    return arrRecords;
}


function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('AP/VRSTranscriptionistPayment.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'AP/VRSTranscriptionistPayment.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    Unlock = "Y";
    btnBrwClose_Onclick();
}

function btnOk_OnClick() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = objddlTranscriptionist.value;

        AjaxPro.timoutPeriod = 1800000;
        VRSTranscriptionistPayment.FetchProcessStatus(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnOk_OnClick()", expErr.message, "true");
    }

}
function ProcessStatus(Result) {
    var arrRes = new Array();
    var ProcCount = 0; var AppvCount = 0; var rc = 0;
    arrRes = Result.value;

    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessStatus()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessStatus()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            ProcCount = parseInt(arrRes[1]);
            AppvCount = parseInt(arrRes[2]);
            if (ProcCount == 0) {
                PopulateRecords("P");
            }
            else if (ProcCount == AppvCount) {
                parent.PopupMessage(RootDirectory, strForm, "btnOk_OnClick()", "407", "false");
                PopulateRecords("P");
            }
            else if (ProcCount != AppvCount) {
                parent.GsDlgConfAction = "PROC";
                parent.PopupConfirm("406");
            }
            break;
    }
}
function doProcessRecord(Args) {
    if (Args == "Y")
        PopulateRecords("P");
    else if (Args == "N")
        PopulateRecords("V");
}
function PopulateRecords(Op) {
    var ArrRecords = new Array();
    ArrRecords[0] = objddlBillingCycle.value;
    ArrRecords[1] = objddlTranscriptionist.value;
    ArrRecords[2] = MenuID;
    ArrRecords[3] = UserID;
    ArrRecords[4] = Op;
    CallBackPayment.callback(ArrRecords);
}
function ddlBillingCycle_OnChange() {
    var rc = grdPayment.get_recordCount();
    if (parseInt(rc) > 0) PopulateRecords("P");
}
function ddlTranscriptionist_OnChange() {
    var rc = grdPayment.get_recordCount();
    if (parseInt(rc) > 0) PopulateRecords("P");
}

function txtAdhoc_OnChange(TransID, SID) {
    var itemIndex = 0; var gridItem;
    var ItemRecCountStudy = 0; var StudyRowId = "";
    var TotAmt = 0; var AdAmt = 0; var RowID = "";


    while (gridItem = grdPayment.get_table().getRow(itemIndex)) {

        if (TransID == gridItem.Data[0].toString()) {
            TotAmt = parseFloat(gridItem.Data[5].toString());
            if (gridItem.Data.length > 8) {
                ItemRecCountStudy = gridItem.Data[8].length;
                for (var i = 0; i < ItemRecCountStudy; i++) {
                    if (SID == gridItem.Data[8][i][1].toString()) {
                        RowID = gridItem.Data[8][i][0].toString();
                        AdAmt = parseFloat(gridItem.Data[8][i][11].toString());
                        gridItem.Data[8][i][11] = document.getElementById("txtAdhoc_" + RowID).value;
                        TotAmt = (TotAmt - AdAmt) + parseFloat(document.getElementById("txtAdhoc_" + RowID).value);
                        gridItem.Data[5] = parent.SetDecimalFormat(TotAmt.toString());
                        if (document.getElementById("txtTotAmt_" + TransID) != null) document.getElementById("txtTotAmt_" + TransID).value = parent.SetDecimalFormat(TotAmt.toString());
                        break;
                    }
                }
            }
            break;
        }

        itemIndex++;
    }
}
function btnRpt_OnClick(ID, SID, PatientName, CustomRpt) {
    parent.GsFileType = "PDF";

    if (CustomRpt == "N")
        parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=5&ID=" + SID + "&PNM=" + PatientName + "&UID=" + UserID + "&FMT=1";
    else
        parent.GsLaunchURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=6&ID=" + SID + "&PNM=" + PatientName + "&UID=" + UserID + "&FMT=1";

    parent.PopupReportViewer();
}

/*******************************************************************************/
/*RADIOLOGIST PAYMENT HDR -START                                         */
/*******************************************************************************/
function SaveTranscriptionist(ID, Approved) {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrRad = new Array();


    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = Approved;
        ArrRecords[2] = UserID;
        ArrRecords[3] = MenuID;
        ArrRad = GetTranscriptionistDetails(ID);

        AjaxPro.timoutPeriod = 1800000;
        VRSTranscriptionistPayment.SaveRecord(ArrRecords, ArrRad, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "ApproveRadiologist()", expErr.message, "true");
    }
}
function GetTranscriptionistDetails(ID) {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0;
    var ItemRecCountStudy = 0; var StudyRowId = "";

    while (gridItem = grdPayment.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        if (RowID == ID) {
            if (gridItem.Data.length > 8) {
                ItemRecCountStudy = gridItem.Data[8].length;
                for (var i = 0; i < ItemRecCountStudy; i++) {
                    StudyRowId = gridItem.Data[8][i][0].toString();
                    arrRecords[idx] = gridItem.Data[8][i][2].toString();
                    arrRecords[idx + 1] = gridItem.Data[8][i][1].toString();
                    arrRecords[idx + 2] = gridItem.Data[8][i][4].toString();
                    arrRecords[idx + 3] = gridItem.Data[8][i][11].toString();
                    idx = idx + 4;
                }
            }

            break;
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
            PopulateRecords("P");
            parent.GsRetStatus = "false";
            break;
    }
}
function btnPrintTrans_OnClick(CycleID, TransID) {
    parent.GsFileType = "PDF";
    parent.GsLaunchURL = "AP/DocumentPrinting/VRSDocPrint.aspx?DocID=3&CYCLE=" + CycleID + "&TransID=" + TransID + "&UID=" + UserID;
    parent.PopupReportViewer();
}
/*******************************************************************************/
/*RADIOLOGIST PAYMENT HDR - BILLING ACCOUNT - END                                          */
/*******************************************************************************/
function btnPrint_OnClick() {

    var CycleID = objddlBillingCycle.value;
    var TransID = objddlTranscriptionist.value;

    if (CycleID != "00000000-0000-0000-0000-000000000000") {
        parent.GsFileType = "PDF";
        parent.GsLaunchURL = "AP/DocumentPrinting/VRSDocPrint.aspx?DocID=4&CYCLE=" + CycleID + "&TransID=" + TransID + "&UID=" + UserID;
        parent.PopupReportViewer();
    }
    else {
        parent.PopupMessage(RootDirectory, strForm, "btnPrint_OnClick()", "229", "true");
    }
}
function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

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
    }
}
