var strRowID = "0"; var ErrFlag = 0; var HasDICOM = "";


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
    SetPageValue();
}
function SetPageValue() {
    objiframeUploadSF.src = "VRSUploadStudyFiles.aspx?uid=" + UserID + "&th=" + selTheme;
}
function btnClose_OnClick() {
    if (parent.GsRetStatus == "false") {
        Unlock = "N";
        btnBrwClose_Onclick();
    }
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function btnSubmit_OnClick() {

    parent.PopupLoad();
    try {
        GetFileList();

        if (ErrFlag == 0) {
            if (HasDICOM == "Y") {
                AjaxPro.timeoutPeriod = 1800000;
                VRSManualSubmissionUploadFile.GetStudyDetails(parent.GsStoredValue, UserID, ShowProcess);
            }
            else {
                parent.GsRetStatus = "false";
                parent.GsNavURL = "CaseList/VRSManualSubmission.aspx?uid=" + UserID + "&urid=" + UserRoleID.value + "&mid=0";
                btnDlgClose_Onclick();
            }
            
        }
    }
    catch (expErr) {
        parent.HideLoad();
        parent.PopupMessage(RootDirectory, strForm, "btnSubmit_OnClick()", expErr.message, "true");
    }

}
function GetFileList() {
    var itemIndex = 0; 
    var gridItem;
    var arrSF = new Array(); var idx = 0;
    

    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        arrSF[idx] = gridItem.Data[0].toString();
        arrSF[idx + 1] = gridItem.Data[1].toString();
        arrSF[idx + 2] = gridItem.Data[2].toString(); if (arrSF[idx + 2] == 'D') HasDICOM = 'Y';
        arrSF[idx + 3] = gridItem.Data[3].toString();
        idx = idx + 4;
        itemIndex++;
    }
    parent.GsStoredValue = arrSF;
    if (itemIndex <= 0) {
        ErrFlag = 1;
        parent.PopupMessage(RootDirectory, strForm, "GetFileList()", "336", "true");
    }
}
function ProcessStudyDetails(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.HideLoad();
            parent.PopupMessage(RootDirectory, strForm, "ProcessStudyDetails()", arrRes[1], "true");
            break;
        case "false":
            parent.HideLoad();
            parent.PopupMessage(RootDirectory, strForm, "ProcessStudyDetails()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            for (var i = 1; i < arrRes.length; i++) {
                parent.GsGlobalTempValue[i - 1] = arrRes[i];
            }
            parent.GsRetStatus = "false";
            parent.GsNavURL = "CaseList/VRSManualSubmission.aspx?uid=" +UserID + "&urid=" + UserRoleID.value + "&mid=0&th="+ selTheme;
            btnDlgClose_Onclick();
            break;
    }
}

/********Study File Grid*****************************/
function ProcessStudyFileUpload(ArgsRet) {
    var arrFiles = new Array();
    var strExistingDtls = ""; var strNewDtls = "";
    var strFileType = "";
    if (ArgsRet != null) {
        parent.GsRetStatus = "true";
        arrFiles = ArgsRet[0].split(Divider);
        strExistingDtls = GetGridDetails();
        for (var i = 0; i < arrFiles.length; i=i+2) {
            if (strNewDtls != "") strNewDtls += SecDivider;
            strNewDtls += arrFiles[i] + SecDivider;
            strNewDtls += arrFiles[i + 1]
        }
        objiframeUploadSF.src = "VRSUploadStudyFiles.aspx?uid=" + UserID+ "&th="+ selTheme;
        CallBackSF.callback("A", strExistingDtls, strNewDtls,UserID);

    }
}
function GetGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.get_cells()[0].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[1].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[2].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[3].get_value().toString();

        itemIndex++;
    }
    return strDtls;
}
function DeleteStudyFile(ID) {
    strRowID = ID;
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function DeleteRecord() {
    var strExistingDtls = "";
    strExistingDtls = GetGridDetails();
    CallBackSF.callback("D", strRowID, strExistingDtls, objhdnTempFolder.value);
}
function ClearFiles() {
    var strExistingDtls = GetGridDetails();
    CallBackSF.callback("C", strExistingDtls, objhdnTempFolder.value);
}
/********Study File Grid*****************************/
function ShowProcess(Result, MethodName) {
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "GetStudyDetails":
            ProcessStudyDetails(Result);
            break;
    }

}


