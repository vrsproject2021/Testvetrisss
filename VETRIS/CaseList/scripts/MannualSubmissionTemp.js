var strRowID = "0"; var DEL_FLAG = ""; var ErrFlag = 0;
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    objtxtPFName.focus();
    ddlInstitution_OnChange();
}));

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
    parent.adjustFrameHeight();
    objiframeUploadSF.src = "VRSUploadStudyFiles.aspx?uid=" + UserID;
    if (parent.GsGlobalTempValue.length > 0) LoadStudyDetails();
    if (parent.GsStoredValue.length > 0) LoadStudyFiles();
    
    LoadStudyTypes();
    parent.GsGlobalTempValue.length = 0;
    parent.GsStoredValue.length = 0;
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
function LoadStudyDetails() {
    var arr = new Array();
    objtxtPID.value = parent.GsGlobalTempValue[1];
    objtxtPFName.value = parent.GsGlobalTempValue[2];
    objtxtPLName.value = parent.GsGlobalTempValue[3]; objtxtOwnerLN.value = parent.GsGlobalTempValue[3];
    arr = parent.GsGlobalTempValue[4].split(' ');
    objtxtDOS.value = arr[0];
    arr = arr[1].split(":");
    objddlHr.value = arr[0];
    objddlMin.value = arr[1];
    objtxtAccnNo.value = parent.GsGlobalTempValue[5];
    objtxtReason.value = parent.GsGlobalTempValue[6];
    objtxtFromDt.value = parent.GsGlobalTempValue[7];
    objddlPriority.value = parent.GsGlobalTempValue[10];
    txtFromDt_OnBlur();
    
}
function txtPLName_OnChange() {
    if (parent.Trim(objtxtOwnerLN.value) == "") {
        objtxtOwnerLN.value = objtxtPLName.value;
    }
}
function txtFromDt_OnBlur() {
    var strFromDt = "";
    var strDtToday = "";
    var dtFrom = new Date();
    var dtToday = new Date();
    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");

    if (objtxtFromDt.value != "") {
        if (ValidateDateEntered(objtxtFromDt)) {

            if (document.all) strFromDt = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
            else strFromDt = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);

            strDtToday = dtToday.getDate().toString() + monthName[dtToday.getMonth()] + dtToday.getFullYear();

            dtFrom = new Date(strFromDt);
            dtToday = new Date(strDtToday);

            if (dtFrom <= dtToday) {
                CalculateAge();
            }
            else {
                parent.PopupMessage(RootDirectory, strForm, "txtFromDt_OnBlur()", "338", "true");
            }
        }
    }
    else
        CalculateAge();
}
function txtDOS_OnBlur() {
    var strFromDt = "";
    var strDtToday = "";
    var dtFrom = new Date();
    var dtToday = new Date();
    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");

    if (objtxtFromDt.value != "") {
        if (ValidateDateEntered(objtxtDOS)) {

            if (document.all) strFromDt = parent.SetDateFormat(txtDOS.value, parent.GsDateFormat, parent.GsDateSep);
            else strFromDt = parent.SetDateFormat1(txtDOS.value, parent.GsDateFormat, parent.GsDateSep);

            strDtToday = dtToday.getDate().toString() + monthName[dtToday.getMonth()] + dtToday.getFullYear();

            dtFrom = new Date(strFromDt);
            dtToday = new Date(strDtToday);

            if (dtFrom <= dtToday) {
                CalculateAge();
            }
            else {
                parent.PopupMessage(RootDirectory, strForm, "txtFromDt_OnBlur()", "338", "true");
            }
        }
    }
    else
        CalculateAge();
}
function CalculateAge() {

    var strDtFrom = ""; var strDtTill = "";
    var dtFrom = new Date(); var dtTill = new Date();
    var diff = 0; var ageYrs = 0; var ageMth = 0; var age = "";
    if (objtxtFromDt.value != "") {
        if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
        else {
            if (document.all)
                strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
            else
                strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        }
        if (parent.Trim(objtxtDOS.value) == "") strDtTill = "01Jan1900";
        else {
            if (document.all)
                strDtTill = parent.SetDateFormat(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
            else
                strDtTill = parent.SetDateFormat1(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
        }
        dtFrom = new Date(strDtFrom);
        dtTill = new Date(strDtTill);
        diff = dateDiffInDays(dtFrom, dtTill);

        //ageYrs = (diff / 365).toFixed(0);
        //ageMth = (diff / 12).toFixed(0) - ;
        if (diff <= 0) {
            age = "0 year 0 month";
        }
        else {
            age = getAge(dtFrom, dtTill)
        }
        objtxtAge.value = age;
        //objtxtAge.value = ageYrs.toString() + "Years " + ageMth.toString() + " Months";
    }
    else {
        objtxtAge.value = "0 year 0 month";
    }
}
function dateDiffInDays(dt1, dt2) {
    var _MS_PER_DAY = 1000 * 60 * 60 * 24;
    // Discard the time and time-zone information.
    var utc1 = Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate());
    var utc2 = Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate());

    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}
function getAge(dtFrom, dtTill) {
    var today = new Date(dtTill);
    var DOB = new Date(dtFrom);
    var totalMonths = (today.getFullYear() - DOB.getFullYear()) * 12 + (today.getMonth() + 1) - (DOB.getMonth() + 1);
    totalMonths += today.getDay() < DOB.getDay() ? -1 : 0;
    var years = today.getFullYear() - DOB.getFullYear();
    if (DOB.getMonth() > today.getMonth())
        years = years - 1;
    else if (DOB.getMonth() === today.getMonth())
        if (DOB.getDate() > today.getDate())
            years = years - 1;

    var days;
    var months;

    if (DOB.getDate() > today.getDate()) {
        months = (totalMonths % 12);
        if (months == 0)
            months = 11;
        var x = today.getMonth() + 1;
        switch (x) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12: {
                var a = DOB.getDate() - today.getDate();
                days = 31 - a;
                break;
            }
            default: {
                var a = DOB.getDate() - today.getDate();
                days = 30 - a;
                break;
            }
        }

    }
    else {
        days = today.getDate() - DOB.getDate();
        if (DOB.getMonth() === today.getMonth())
            months = (totalMonths % 12);
        else
            months = (totalMonths % 12) + 1;
    }
    if ((parseInt(months) == 0) && (parseInt(days) > 0)) months = 1;
    //var age = years + ' years ' + months + ' months ' + days + ' days';
    var age = years + ' years ' + months + ' months ';
    return age;
}
function ddlSpecies_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlSpecies.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSManualSubmissionTemp.FetchBreeds(ArrRecords, ShowProcess);
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
    //parent.PopupProcess("N");
    try {
        objhdnTempPID.value = "";
        AjaxPro.timeoutPeriod = 1800000;
        VRSManualSubmissionTemp.FetchRegPhysicians(objRegInstitutionId.value, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "ddlInstitution_OnChange()", expErr.message, "true");
    }
}
function PopulatePhysicians(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    var rc = 0;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "PopulatePhysicians()", arrRes[1], "true");
            break;
        case "true":
            objddlPhys.length = 0;
            for (var i = 1; i < (arrRes.length - 3) ; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlPhys.add(op);
            }
            //objhdnInstConsAppl.value = arrRes[arrRes.length - 3];
            objRegInstitutionName.value = arrRes[arrRes.length - 3];
            if (objhdnInstConsAppl.value == "Y") document.getElementById("divCons").style.display = "inline";
            else document.getElementById("divCons").style.display = "none";
            objhdnTempPID.value = arrRes[arrRes.length - 2] + "-" + arrRes[arrRes.length - 1];
            if (grdDCM.recordNumber != null) rc = grdDCM.get_recordCount();
            else if (grdDCM.RecordCount != null) rc = grdDCM.get_recordCount();
            if (rc == 0) {
                objtxtPID.value = ''; //objhdnTempPID.value;
            }
            break;
    }

}

/********Study Types*************************/
function LoadStudyTypes() {
    CallBackST.callback(objhdnID.value, objddlModality.value, objddlCategory.value);
    CallBackSelST.callback("L", objhdnID.value, objddlModality.value);
}
function chkSel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = "";
    CategoryID = "0";
    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {

            if (document.getElementById("chkSel_" + RowId).checked) {
                if (CategoryID != "2") CategoryID = gridItem.Data[5].toString();
                gridItem.Data[2] = "Y";
            }
            else {
                gridItem.Data[2] = "N";
            }
            objddlCategory.value = CategoryID.toString();
            Consult_OnClick();
            parent.GsRetStatus = "true";
            strDtls = GetStudyGridDetails();
            CallBackSelST.callback("U", strDtls);
            break;
        }
        itemIndex++;
    }
}
function GetStudyGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;
    var strSel = "";

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        strSel = gridItem.Data[2].toString();

        if (strSel == "Y") {
            if (strDtls != "") strDtls = strDtls + SecDivider;
            strDtls += gridItem.get_cells()[1].get_value().toString() + SecDivider;
            strDtls += gridItem.get_cells()[3].get_value().toString();
        }
        itemIndex++;
    }
    return strDtls;
}
/********Study Types*************************/

function Consult_OnClick() {
    //if (objddlCategory.value != "2") {
    if (objrdoConsY.checked) {
        objddlCategory.value = "3";
    }
    else if (objrdoConsN.checked) {
        if (CategoryID != "0") objddlCategory.value = CategoryID;
        else objddlCategory.value = "1";
    }

    //}
}

/********Study File Grid*****************************/
function LoadStudyFiles() {
    var strFileList = "";
    var strPID = "";
    var strAccnNo = "";
    for (var i = 0; i < parent.GsStoredValue.length; i++) {
        if (strFileList != "") strFileList += SecDivider;
        strFileList += parent.GsStoredValue[i];
    }

    if (parent.GsGlobalTempValue.length > 0)
    {
        strPID = objtxtPID.value;
        strAccnNo = objtxtAccnNo.value;
    }

    CallBackSF.callback("L", strFileList, strPID, strAccnNo,UserID);
}
function ProcessStudyFileUpload(ArgsRet) {
    var arrFiles = new Array();
    var strExistingDtls = ""; var strNewDtls = "";
    var strFileType = "";
    if (ArgsRet != null) {
        parent.GsRetStatus = "true";
        arrFiles = ArgsRet[0].split(Divider);
        strExistingDtls = GetGridDetails();
        for (var i = 0; i < arrFiles.length; i = i + 2) {
            if (strNewDtls != "") strNewDtls += SecDivider;
            strNewDtls += arrFiles[i] + SecDivider;
            strNewDtls += arrFiles[i + 1]
        }
        objiframeUploadDCM.src = "VRSUploadStudyFiles.aspx?uid=" + UserID;
        CallBackSF.callback("A", strExistingDtls, strNewDtls, UserID);

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
    CallBackSF.callback("D", strRowID, strExistingDtls, objhdnTempDCMFolder.value);
}
function ClearFiles() {
    var strExistingDtls = GetGridDetails();
    CallBackSF.callback("C", strExistingDtls, objhdnTempDCMFolder.value);
}
/********Study File Grid*****************************/

function btnSubmit_OnClick(WriteBack, MergeStatus) {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrImg = new Array();
    var arrDCM = new Array();
    var arrSTs = new Array();
    var arrDocs = new Array();
    var strDOS = ""; var strDOB = "";

    if (parent.Trim(objtxtDOS.value) == "") strDOS = "01Jan1900";
    else {
        if (document.all)
            strDOS = parent.SetDateFormat(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDOS = parent.SetDateFormat1(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
    }
    if (parent.Trim(objtxtFromDt.value) == "") strDOB = "01Jan1900";
    else {
        if (document.all)
            strDOB = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDOB = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    try {
        ArrRecords[0] = objtxtPFName.value;
        ArrRecords[1] = objtxtPLName.value;
        ArrRecords[2] = objddlSex.value;
        ArrRecords[3] = objtxtOwnerFN.value;
        ArrRecords[4] = objtxtOwnerLN.value;
        ArrRecords[5] = objddlSN.value;
        ArrRecords[6] = objtxtPWt.value;
        ArrRecords[7] = objddlUOM.value;
        ArrRecords[8] = strDOB;
        ArrRecords[9] = objtxtAge.value;
        ArrRecords[10] = objddlSpecies.value;
        ArrRecords[11] = objddlBreed.value;
        ArrRecords[12] = strDOS + " " + objddlHr.value + ":" + objddlMin.value;
        ArrRecords[13] = objtxtPID.value;
        ArrRecords[14] = objtxtAccnNo.value;
        ArrRecords[15] = objtxtReason.value;
        ArrRecords[16] = objtxtPhysNote.value;
        ArrRecords[17] = objddlModality.value;
        ArrRecords[18] = objddlPriority.value;
        ArrRecords[19] = objRegInstitutionId.value; //objddlInstitution.value;
        ArrRecords[20] = objRegInstitutionName.value; //objddlInstitution.options[objddlInstitution.selectedIndex].text;
        ArrRecords[21] = objddlPhys.value;
        ArrRecords[22] = "N"; if (objrdoConsY.checked) ArrRecords[26] = "Y";
        ArrRecords[23] = objddlCategory.value;
        ArrRecords[24] = objhdnDCMMODIFYEXEPATH.value;
        ArrRecords[25] = UserID;

        arrSTs = GetStudyTypes();
        //arrDocs = GetDocList();
        arrImg = GetImageList();
        arrDCM = GetDCMList();

        if (ErrFlag == 0) {
            if (objRegInstitutionId.value != "00000000-0000-0000-0000-000000000000") {
                AjaxPro.timeoutPeriod = 1800000;
                VRSManualSubmissionTemp.SaveRecord(ArrRecords, arrSTs, arrDCM, arrImg, ShowProcess);
            }
            else {
                parent.HideProcess();
                parent.PopupMessage(RootDirectory, strForm, "btnSubmit_OnClick()", "055", "true");
            }
        }


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSubmit_OnClick()", expErr.message, "true");
    }

}
function GetStudyTypes() {
    var itemIndex = 0; var gridItem;
    var arrST = new Array(); var idx = 0; var sel = ""; var validate = ""; var strName = ""; var validate_sel = "";
    var cnt = 0;
    if (grdSelST.recordNumber != null) rc = grdSelST.get_recordCount();
    else if (grdSelST.RecordCount != null) rc = grdSelST.get_recordCount();

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[2].toString();
        //validate = gridItem.Data[4].toString();

        if (validate != "Y") {
            validate = gridItem.Data[4].toString();
            strName = gridItem.Data[3].toString();
            validate_sel = sel;
        }

        if (sel == "Y") {
            cnt = cnt + 1;
            arrST[idx] = gridItem.Data[1].toString();
            idx = idx + 1;
        }
        itemIndex++;
    }

    if (validate == "Y" && validate_sel == "Y" && cnt == 1) {
        ErrFlag = 1;
        arrST.length = 0;
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "GetStudyTypes()", "176", "true", strName);
    }

    return arrST;
}
function GetDCMList() {
    var itemIndex = 0;
    var gridItem;
    var arrDCM = new Array(); var idx = 0;
    while (gridItem = grdDCM.get_table().getRow(itemIndex)) {
        arrDCM[idx] = gridItem.get_cells()[0].get_value().toString();
        arrDCM[idx + 1] = gridItem.get_cells()[1].get_value().toString();
        idx = idx + 2;
        itemIndex++;
    }
    return arrDCM;
}
function GetImageList() {
    var itemIndex = 0;
    var gridItem;
    var arrImg = new Array(); var idx = 0;
    while (gridItem = grdImg.get_table().getRow(itemIndex)) {
        arrImg[idx] = gridItem.get_cells()[0].get_value().toString();
        arrImg[idx + 1] = gridItem.get_cells()[1].get_value().toString();
        idx = idx + 2;
        itemIndex++;
    }
    return arrImg;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "331") {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2], arrRes[3]);
            }
            else {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            }
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            parent.GsLogout = "Y";
            break;
    }
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
        case "CalDOS":
            CalDOS.setSelectedDate(dt); CalDOS.show();
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
        case "FetchRegPhysicians":
            PopulatePhysicians(Result);
            break;

    }

}
function ResetValueDecimal(objCtrlID, decimalPlace) {

    var objCtrl;
    if (decimalPlace == null) decimalPlace = parent.GiDecimal;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    //else objCtrl.value = parent.SetDecimalFormat(objCtrl.value.replace("-", ""), decimalPlace);

}
