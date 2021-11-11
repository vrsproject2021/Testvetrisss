var strRowID = "0"; var DEL_FLAG = ""; var WRITE_BACK = "N";
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    objtxtPName.focus();
}));

function CheckError() {
    var arrErr = new Array();
    if (parent.Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                if (arrErr[1] == "094") {
                    parent.PopupMessage(RootDirectory, strForm, "CheckError()", arrErr[1], "true");
                    strLoadPopup = "N";
                    parent.GsRetStatus = "false";
                    btnClose_OnClick();
                }
                else
                    parent.PopupMessage(RootDirectory, strForm, "OnLoad()", arrErr[1], "true");
                break;
        }
    }
    objhdnError.value = "";
    SetPageValue();
}
function SetPageValue() {
    if (document.all) {
        if (parent.Trim(objlblPatientID.innerText) == "") objtxtPID.readOnly = "";
        if (parent.Trim(objlblAccnNo.innerText) == "") objtxtAccnNo.readOnly = "";
    }
    else {
        if (parent.Trim(objlblPatientID.textContent) == "") objtxtPID.readOnly = "";
        if (parent.Trim(objlblAccnNo.textContent) == "") objtxtAccnNo.readOnly = "";
    }
    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();
    //objiframeUpload.src = "VRSUploadFiles.aspx?dir=" + strDir + "&t=IMG&fileID=" + usrID + "_" + tmpID;
    objiframeUpload.src = "VRSUploadFiles.aspx?dir=" + strDir + "&fileID=" + usrID + "_" + tmpID;
    ddlModality_OnChange();
    if ((UserRoleID != "1") && (UserRoleID != "2")) {
        objddlInstitution.disabled = true;
    }
    CallBackDoc.callback("L", objhdnID.value, UserID);
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('CaseList/VRSCaseRADlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'CaseList/VRSCaseRADlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "CaseList/VRSCaseRABrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}

function NavigateToImgViewer() {
    var URL = objhdnImgVwrURL.value;
    URL = URL.replace("#V1", objhdnSUID.value);
    URL = URL.replace("#V2", PACSUID);
    URL = URL.replace("#V3", PACSPwd);
    parent.NavigatePACS(URL);
}
function NavigateToPACS() {
    var URL = objhdnPACSURL.value;
    URL = URL.replace("#V1", objhdnSUID.value);
    URL = URL.replace("#V2", PACSUID);
    URL = URL.replace("#V3", PACSPwd);

    parent.NavigatePACS(URL);
}
function btnSubmit_OnClick(WriteBack) {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrSTs = new Array();
    var arrDocs = new Array();
    var strFromDt = "";
    WRITE_BACK = WriteBack;

    if (parent.Trim(objtxtFromDt.value) == "") strFromDt = "01Jan1900";
    else {
        if (document.all)
            strFromDt = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strFromDt = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }


    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objtxtPID.value;
        ArrRecords[2] = objtxtPName.value;
        ArrRecords[3] = objtxtPWt.value;
        ArrRecords[4] = strFromDt;
        ArrRecords[5] = objtxtAge.value;
        ArrRecords[6] = objddlSex.value;
        ArrRecords[7] = objddlSN.value;
        ArrRecords[8] = objddlSpecies.value;
        ArrRecords[9] = objddlBreed.value;
        ArrRecords[10] = objtxtOwnerFN.value;
        ArrRecords[11] = objtxtOwnerLN.value;
        ArrRecords[12] = objtxtAccnNo.value;
        ArrRecords[13] = objddlModality.value;
        ArrRecords[14] = objtxtReason.value;
        ArrRecords[15] = objtxtImgCnt.value;
        ArrRecords[16] = "N"; if (objrdoConfYes.checked) ArrRecords[16] = "Y";
        ArrRecords[17] = objddlInstitution.value;
        ArrRecords[18] = objddlPhys.value;
        ArrRecords[19] = WriteBack;
        ArrRecords[20] = objddlUOM.value;
        ArrRecords[21] = objddlPriority.value;
        ArrRecords[22] = UserID;
        ArrRecords[23] = MenuID;

        arrSTs = GetStudyTypes();
        arrDocs = GetDocList();

      
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseRADlg.SaveRecord(ArrRecords, arrSTs, arrDocs, ShowProcess);
       
       
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSubmit_OnClick()", expErr.message, "true");
    }

}
function GetStudyTypes() {
    var itemIndex = 0;
    var gridItem;
    var arrST = new Array(); var idx = 0;var sel="";

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[2].toString(); 
        if(sel=="Y")
        {
            arrST[idx] = gridItem.Data[1].toString();
            idx = idx + 1;
        }
        itemIndex++;
    }
    return arrST;
}
function GetDocList() {
    var itemIndex = 0;
    var gridItem;
    var arrDoc = new Array(); var idx = 0;
    while (gridItem = grdDoc.get_table().getRow(itemIndex)) {
        arrDoc[idx] = gridItem.get_cells()[0].get_value().toString();
        arrDoc[idx + 1] = gridItem.get_cells()[1].get_value().toString();
        arrDoc[idx + 2] = gridItem.get_cells()[2].get_value().toString();
        arrDoc[idx + 3] = gridItem.get_cells()[3].get_value().toString();
        arrDoc[idx + 4] = gridItem.get_cells()[4].get_value().toString();
        idx = idx + 5;
        itemIndex++;
    }
    return arrDoc;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "094") {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
                strLoadPopup = "N";
                parent.GsRetStatus = "false";
                btnClose_OnClick();
            }
            else
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            if (WRITE_BACK == "Y")
                btnClose_OnClick();
            else {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
                ddlModality_OnChange();
            }
            break;
    }
}

//function txtFromDt_OnBlur() {
//    var strDtFrom = ""; var strDtTill = "";
//    var dtFrom = new Date(); var dtTill = new Date();
//    var diff = 0; var age = 0;

//    if (ValidateDateEntered(objtxtFromDt)) {

//        if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
//        else {
//            if (document.all)
//                strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
//            else
//                strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
//        }

//        dtFrom = new Date(strDtFrom);
//        diff = dateDiffInDays(dtFrom, dtTill);

//        age = (diff / 365).toFixed(0);
//        objtxtAge.value = age.toString();
//    }
//}
function dateDiffInDays(dt1, dt2) {
    var _MS_PER_DAY = 1000 * 60 * 60 * 24;
    // Discard the time and time-zone information.
    var utc1 = Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate());
    var utc2 = Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate());

    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}
function ddlSpecies_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlSpecies.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseRADlg.FetchBreeds(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "ddlSpecies_OnChange()", expErr.message, "true");
    }
}
function PopulateBreed(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "PopulateBreed()", arrRes[1], "true");
            break;
        case "true":
            objddlBreed.length = 0;
            for (var i = 1; i < arrRes.length; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlBreed.add(op);
            }
            break;
    }

}
function ddlInstitution_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlInstitution.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseRADlg.FetchPhysicians(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "ddlInstitution_OnChange()", expErr.message, "true");
    }
}
function PopulatePhysicians(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "PopulatePhysicians()", arrRes[1], "true");
            break;
        case "true":
            objddlPhys.length = 0;
            for (var i = 1; i < arrRes.length; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlPhys.add(op);
            }
            break;
    }

}
function ddlModality_OnChange() {
    CallBackST.callback(objhdnID.value, objddlModality.value);
}
function chkSel_OnClick(ID)
{
    var itemIndex = 0; var gridItem; var RowId = "0";
    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if(RowId==ID)
        {
            if (document.getElementById("chkSel_" + RowId).checked) {
                gridItem.Data[2] = "Y";
                var op = document.createElement("option");
                op.value = gridItem.Data[1].toString();
                op.text = gridItem.Data[3].toString();
                objlbSelST.add(op);
            }
            else {
                gridItem.Data[2] = "N";
                for (var i = 0; i < objlbSelST.length; i++) {
                    if(objlbSelST.options[i].value == gridItem.Data[1].toString())
                    {
                        objlbSelST.options.remove(i);
                        break;
                    }
                }
            }

            parent.GsRetStatus = "true";
            break;
        }
        itemIndex++;
    }
}

function btnRefreshCount_OnClick() {
    parent.PopupProcess("N");
    var strSUID = ""; var strURL = "";

    try {
        strSUID = objhdnSUID.value;
        strURL = objhdnImgCntURL.value.replace("qf_SYUI=", "qf_SYUI=" + strSUID);
        strURL = strURL.replace("#V2", PACSUID);
        strURL = strURL.replace("#V3", PACSPwd);

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseRADlg.GetImageCount(strURL, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnRefreshCount_OnClick()", expErr.message, "true");
    }
}
function GetImageCount(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "GetImageCount()", arrRes[1], "true");
            break;
        case "true":
            objtxtImgCnt.value = arrRes[1];
            break;
    }

}

function btnDel_OnClick() {
    DEL_FLAG = 'STUDY';
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("123");
}
function DeleteStudy() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    var strSUID = ""; var strURL = "";

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objhdnSUID.value;
        ArrRecords[2] = UserID;
        ArrRecords[3] = MenuID;

        strSUID = objhdnSUID.value;
        strURL = objhdnStudyDelUrl.value + strSUID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSCaseRADlg.DeleteStudy(ArrRecords,strURL, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "DeleteStudy()", expErr.message, "true");
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
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            btnClose_OnClick();
            break;
    }
}

function btnUpload_OnClick() {
    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();
    parent.GsLaunchURL = "Common/VRSUploader.aspx?dir=" + strDir + "&t=IMG&fileID=" + usrID + "_" + tmpID;
    parent.PopupUpload();

}
function createGuid() {
    //return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
function ProcessUpload(ArgsRet) {
    var retArr = new Array();
    var strExistingDtls = ""; var strNewDtls = "";
    var strFileType = "";
    if (ArgsRet != null) {
        parent.GsRetStatus = "true";
        retArr = ArgsRet;

        if (!CheckDulipcateDocument(retArr[0])) {
            strExistingDtls = GetDocGridDetails("Y", retArr[0]);
            strFileType = retArr[0].substr(retArr[0].lastIndexOf(".") + 1, (retArr[0].length - retArr[0].lastIndexOf(".")));

            strNewDtls = "0" + SecDivider;
            strNewDtls += "00000000-0000-0000-0000-000000000000" + SecDivider;
            strNewDtls += retArr[1] + SecDivider;
            strNewDtls += retArr[0] + SecDivider;
            strNewDtls += strFileType;
            DOCADD = "Y";

            CallBackDoc.callback("A", strExistingDtls, strNewDtls);
        }

    }
}
function CheckDulipcateDocument(FileName) {
    var bRet = false;

    var itemIndex = 0;
    var gridItem;
    var srl = "";

    while (gridItem = grdDoc.get_table().getRow(itemIndex)) {
        if (parent.Trim(gridItem.get_cells()[3].get_value().toString()) == parent.Trim(FileName)) {
            srl = gridItem.get_cells()[0].get_value().toString();
            parent.PopupMessage(RootDirectory, strForm, "CheckDulipcateDocument()", "031", "true", srl);
            bRet = true;
            break;
        }
        itemIndex++;
    }
    return bRet;
}
function GetDocGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdDoc.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.get_cells()[0].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[1].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[2].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[3].get_value().toString() + SecDivider;
        strDtls += gridItem.get_cells()[4].get_value().toString();
        itemIndex++;
    }
    return strDtls;
}
function DeleteDocument(ID) {

    strRowID = ID;
    DEL_FLAG = "DOC";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("032");
}
function DeleteRecord() {
    var Flg = DEL_FLAG;
    switch (Flg) {
        case "DOC":
            var strExistingDtls = GetDocGridDetails();
            CallBackDoc.callback("D", strRowID, strExistingDtls);
            break;
        case "STUDY":
            DeleteStudy();
            break;
    }
   
    
}
function ShowDocument(ID, FileName) {
    var strFilePath = "/" + RootDirectory + "/CaseList/Temp/" + UserID + "/" + FileName;
    var win = window.open(strFilePath, '_blank');
    win.focus();
}

function SetSelectedDate(objCtrlID, CalName, LsImgID) {
    var strDate = ""; var strClass = "";
    var dt;
    objCtrl = document.getElementById(objCtrlID.id); if (objCtrl == null) objCtrl = objCtrlID;
    strDate = document.getElementById(objCtrl).value;

    if (parent.Trim(strDate) != "") {
        if (document.all) {
            dt = new Date(parent.SetDateFormat(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
        else {
            dt = new Date(parent.SetDateFormat1(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
    }
    else
        dt = new Date();
    switch (CalName) {
        case "CalFrom":
            CalFrom.setSelectedDate(dt); CalFrom.show();
            break;

    }


}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
        case "FetchBreeds":
            PopulateBreed(Result);
            break;
        case "FetchPhysicians":
            PopulatePhysicians(Result);
            break;
        case "GetImageCount":
            GetImageCount(Result);
            break;
        case "DeleteStudy":
            ProcessDeleteStudy(Result);
            break;
    }

}
function ResetValueDecimal(objCtrlID, decimalPlace) {

    var objCtrl;
    if (decimalPlace == null) decimalPlace = parent.GiDecimal;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value.replace("-", ""), decimalPlace);

}
