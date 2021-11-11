var StudyID = ""; var StudyUID = ""; var strRowID = ""; var SAVE_TYPE = "S"; var DlgFlag = "";

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
    if (objddlBillingCycle.value != "00000000-0000-0000-0000-000000000000") btnOk_OnClick();

    if (objhdnCF.value == "IP") {
        objddlBillingCycle.disabled = true;
    }
}

function btnSave_OnClick() {
    SAVE_TYPE="S";
    SaveAmendment();
}
function SaveAmendment()
{
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrStudy = new Array();
    var ArrRates = new Array();

    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = UserID;
        ArrRecords[2] = MenuID;
        ArrStudy = GetStudies();
        ArrRates = GetRates();

        AjaxPro.timoutPeriod = 1800000;
        VRSStudyAmend.SaveRecord(ArrRecords, ArrStudy,ArrRates, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "SaveAmendment()", expErr.message, "true");
    }
}
function GetStudies() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0; var changed = "";

    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        changed = gridItem.Data[10].toString();
        if (changed == "Y") {
            arrRecords[idx] = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[3].toString();
            arrRecords[idx + 2] = gridItem.Data[4].toString();
            arrRecords[idx + 3] = gridItem.Data[5].toString();
            arrRecords[idx + 4] = gridItem.Data[6].toString();
            arrRecords[idx + 5] = gridItem.Data[7].toString();
            arrRecords[idx + 6] = gridItem.Data[8].toString();
            idx = idx + 7;
        }

        itemIndex++;
    }
    return arrRecords;
}
function GetRates() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0; var changed = ""; var InstID = "";
    var ItemRecCount = 0; var RateRowID = ""; var RateChanged = "";

    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        changed = gridItem.Data[10].toString();
        if (changed == "Y") {
            InstID = gridItem.Data[4].toString();
            if (gridItem.Data.length > 13) {
                ItemRecCount = gridItem.Data[13].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RateRowID = gridItem.Data[13][i][0].toString();
                    RateChanged = gridItem.Data[13][i][9].toString();
                    if (RateChanged == "Y") {
                        arrRecords[idx] = InstID;
                        arrRecords[idx + 1] = gridItem.Data[13][i][1].toString();
                        arrRecords[idx + 2] = gridItem.Data[13][i][2].toString();
                        arrRecords[idx + 3] = gridItem.Data[13][i][3].toString();
                        arrRecords[idx + 4] = gridItem.Data[13][i][4].toString();
                        arrRecords[idx + 5] = gridItem.Data[13][i][8].toString();
                        idx = idx + 6;
                    }
                }
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
            if(SAVE_TYPE=="S") parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            btnOk_OnClick();
            parent.GsRetStatus = "false";
            break;
    }
}

function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSStudyAmend.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSStudyAmend.aspx?cf=IP';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    if (objhdnCF.value == "IP") {
        var URL = "Invoicing/VRSInvoiceProcessView.aspx";
        var CycleName = objddlBillingCycle.options[objddlBillingCycle.selectedIndex].text;
        parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=00000000-0000-0000-0000-000000000000&bcid=" + objhdnBCID.value + "&cnm=" + CycleName + "&aid=" + objhdnAID.value + "&th=" + selTheme;
    }
    else {
        Unlock = "Y";
        btnBrwClose_Onclick();
    }
}

function btnOk_OnClick() {
    var ArrRecords = new Array();
    ArrRecords[0] = objddlBillingCycle.value;
    ArrRecords[1] = objddlInstitution.value;
    ArrRecords[2] = objtxtName.value;
    ArrRecords[3] = objddlModality.value;
    ArrRecords[4] = objddlCategory.value;
    ArrRecords[5] = MenuID;
    ArrRecords[6] = UserID;
    CallBackStudy.callback(ArrRecords);
}
function ddlBillingCycle_OnChange() {
    var rc = grdStudy.get_recordCount();
    if (parseInt(rc) > 0) btnOk_OnClick();
}
function ddlInstitution_OnChange() {
    var rc = grdStudy.get_recordCount();
    if (parseInt(rc) > 0) btnOk_OnClick();
}
function DeleteStudy(ID, UID) {
    StudyID = ID; StudyUID = UID;
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function DeleteRecord() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();

    try {
        ArrRecords[0] = objddlBillingCycle.value;
        ArrRecords[1] = StudyID;
        ArrRecords[2] = UserID;
        ArrRecords[3] = MenuID;

        AjaxPro.timoutPeriod = 1800000;
        VRSStudyAmend.DeleteRecord(ArrRecords, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }

}
function ProcessDeleteStudy(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteStudy()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteStudy()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            DeleteRecordPACS();
            break;
    }
}

function DeleteRecordPACS() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    var ArrParams = new Array();
    var strURL = "";

    try {

        if (objhdnAPIVER.value == "7.2") {
            ArrRecords[0] = StudyID;
            ArrRecords[1] = StudyUID;
            ArrRecords[2] = UserID;

            strURL = objhdnStudyDelUrl.value + StudyUID;

            AjaxPro.timeoutPeriod = 1800000;
            VRSStudyAmend.DeleteStudyPACS_72(ArrRecords, strURL, ShowProcess);
        }
        else if (objhdnAPIVER.value == "8") {
            ArrRecords[0] = StudyID;
            ArrRecords[1] = StudyUID;
            ArrRecords[2] = UserID;

            ArrParams[0] = objhdnWS8SRVIP.value;
            ArrParams[1] = objhdnWS8CLTIP.value;
            ArrParams[2] = objhdnWS8SRVUID.value;
            ArrParams[3] = objhdnWS8SRVPWD.value;

            AjaxPro.timeoutPeriod = 1800000;
            VRSStudyAmend.DeleteStudyPACS_80(ArrRecords, ArrParams, ShowProcess);
        }
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "DeleteRecordPACS()", expErr.message, "true");
    }
}
function ProcessDeleteStudyPACS(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteStudyPACS()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteStudyPACS()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            btnOk_OnClick();
            break;
    }
}

function ddlInst_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("ddlInst_" + RowId).value;
            gridItem.Data[10] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlModality_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0; var RateRowID = ""; var HeadType = "";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("ddlModality_" + RowId).value;
            gridItem.Data[10] = "Y";
            if (gridItem.Data.length > 13) {
                ItemRecCount = gridItem.Data[13].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RateRowID = gridItem.Data[13][i][0].toString();
                    HeadType = gridItem.Data[13][i][4].toString();
                    
                    if (HeadType == "M") {
                        gridItem.Data[13][i][3] = document.getElementById("ddlModality_" + RowId).value;
                        gridItem.Data[13][i][5] = document.getElementById("ddlModality_" + RowId).options[document.getElementById("ddlModality_" + RowId).selectedIndex].text;
                        gridItem.Data[13][i][9] = "Y";
                        if (document.getElementById("txtHead_" + RateRowID) != null) document.getElementById("txtHead_" + RateRowID).value = document.getElementById("ddlModality_" + RowId).options[document.getElementById("ddlModality_" + RowId).selectedIndex].text;
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
function ddlPriority_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("ddlPriority_" + RowId).value;
            gridItem.Data[10] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ddlCategory_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[7] = document.getElementById("ddlCategory_" + RowId).value;
            gridItem.Data[10] = "Y";
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function btnEditSvc_OnClick(ID, Codes) {
    parent.GiWidth = 800;
    parent.GiTop = 30;
    strRowID = ID;
    parent.GsText = Codes;
    DlgFlag = "SVC";
    parent.GsLaunchURL = "Invoicing/VRSServiceCodes.aspx?id=" + ID + "&th=" + selTheme;
    parent.PopupDataList();

}
function btnEditBP_OnClick(ID, SID,HeadID) {
    parent.GiWidth = 100;
    parent.GiTop = 30;
    strRowID = ID;
    StudyID = SID;
    DlgFlag = "BPCNT";
    parent.GsLaunchURL = "Invoicing/VRSStudyAmendStudyTypes.aspx?id=" + SID + "&mod=" + HeadID + "&cycle=" + objddlBillingCycle.value + "&th=" + selTheme;
    parent.PopupDataList();

}
function ProcessDataList(Args) {
    
    var itemIndex = 0; var gridItem; var RowID = "0"; var Svc = "";
    var ItemRecCount = 0;var RateRowID="";
    switch (DlgFlag) {
        case "SVC":
            while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
                RowID = gridItem.Data[0].toString();
                if (RowID == strRowID) {
                    if (Args.length > 1) {
                        for (var i = 1; i < Args.length; i = i + 2) {
                            if (parent.Trim(Svc) != "") Svc = Svc + ",";
                            Svc += parent.Trim(Args[i]);
                        }
                        if (document.getElementById("txtService_" + RowID) != null) document.getElementById("txtService_" + RowID).value = Svc;
                        gridItem.Data[8] = Svc;
                        gridItem.Data[10] = "Y";
                        SAVE_TYPE = "L";
                        SaveAmendment();
                    }
                    else if (Args.length == 0) {
                        if (document.getElementById("txtService_" + RowID) != null) document.getElementById("txtService_" + RowID).value = "";
                        gridItem.Data[8] = "";
                        gridItem.Data[10] = "Y";
                        SAVE_TYPE = "L";
                        SaveAmendment();
                    }
                    break;
                }
                itemIndex++;
            }
            break;
        case "BPCNT":
            while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
                RowID = gridItem.Data[0].toString();
                if (RowID == StudyID) {
                    gridItem.Data[10] = "Y";
                    if (gridItem.Data.length > 13) {
                        ItemRecCount = gridItem.Data[13].length;
                        for (var i = 0; i < ItemRecCount; i++) {
                            RateRowID = gridItem.Data[13][i][0].toString();
                            if(RateRowID == strRowID)
                            {
                                if (document.getElementById("txtCnt_" + RateRowID) != null) {
                                    document.getElementById("txtCnt_" + RateRowID).value = Args.length.toString();
                                    gridItem.Data[13][i][6]=document.getElementById("txtCnt_" + RateRowID).value;
                                }
                                break;
                            }
                        }
                    }
                    break;
                }
                itemIndex++;
            }
    }
}

function btnDisc_OnClick(ID) {
    StudyID = ID;
    parent.GsLaunchURL = "CaseList/VRSApplyPromotion.aspx?id=" + ID + "&mid=" + MenuID + "&uid=" + UserID + "&sid=" + SessionID + "&th=" + selTheme;
    parent.PopupGeneralMedium();
}
function btnFree_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0; var RateRowID = ""; var HeadType = "";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[10] = "Y";
            if (gridItem.Data.length > 13) {
                ItemRecCount = gridItem.Data[13].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RateRowID = gridItem.Data[13][i][0].toString();
                    gridItem.Data[13][i][8] = 0;
                    gridItem.Data[13][i][9] = "Y";
                    if (document.getElementById("txtAmt_" + RateRowID) != null)
                        document.getElementById("txtAmt_" + RateRowID).value = parent.SetDecimalFormat("0");
                    else
                        grdStudy.ToggleExpand(event, gridItem, itemIndex.toString())
                }
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ProcessGeneralMedium(ArgsRet) {
    if (ArgsRet == "Y") {
        parent.PopupProcess("Y");
        var ArrRecords = new Array();
        var ArrStudy = new Array();
        var ArrRates = new Array();

        try {
            ArrRecords[0] = objddlBillingCycle.value;
            ArrRecords[1] = objhdnAID.value;
            ArrRecords[2] = UserID;
            ArrRecords[3] = MenuID;

            AjaxPro.timoutPeriod = 1800000;
            VRSStudyAmend.ReprocessInvoice(ArrRecords, ShowProcess);


        }
        catch (expErr) {
            parent.HideProcess();
            parent.PopupMessage(RootDirectory, strForm, "ProcessGeneralMedium()", expErr.message, "true");
        }
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
            btnOk_OnClick();
            parent.GsRetStatus = "false";
            break;
    }
}
function txtAmt_OnChange(StudyID, ID) {
    var itemIndex = 0; var gridItem; var RowId = "";
    var ItemRecCount = 0; var RateRowID = "";
    while (gridItem = grdStudy.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        if (RowId == StudyID) {
            if (gridItem.Data.length > 13) {
                ItemRecCount = gridItem.Data[13].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RateRowID = gridItem.Data[13][i][0].toString();
                    if (RateRowID == ID) {
                        gridItem.Data[13][i][8] = parent.SetDecimalFormat(document.getElementById("txtAmt_" + RateRowID).value);
                        document.getElementById("txtAmt_" + RateRowID).value = parent.SetDecimalFormat(document.getElementById("txtAmt_" + RateRowID).value);
                        gridItem.Data[13][i][9] = "Y";
                        gridItem.Data[10] = "Y";
                        break;
                    }
                }
            }
            break;
        }

        itemIndex++;
    }
}


function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "DeleteRecord":
            ProcessDeleteStudy(Result);
            break;
        case "SaveRecord":
            SaveRecord(Result);
            break;
        case "ReprocessInvoice":
            ReprocessInvoice(Result);
            break;
        case "DeleteStudyPACS_72":
        case "DeleteStudyPACS_80":
            ProcessDeleteStudyPACS(Result);
            break;

    }
}