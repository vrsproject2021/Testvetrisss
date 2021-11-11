var strRowID = "0"; var DEL_FLAG = ""; var ErrFlag = 0; var SUBMIT_PRIORITY = "N"; var CHECK_DOB = "Y";
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    objtxtPFName.focus();
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
    objiframeUploadSF.src = "VRSUploadStudyFiles.aspx?uid=" + UserID+ "&th="+ selTheme;
    if (parent.GsGlobalTempValue.length > 0) LoadStudyDetails(); else { objddlDOBYear.value = "1900"; ddlDOBMonth_OnChange(); }
    if (parent.GsStoredValue.length > 0) LoadStudyFiles();
    LoadStudyTypes();
    if (objddlInstitution.value != "00000000-0000-0000-0000-000000000000") ddlInstitution_OnChange();

    parent.GsGlobalTempValue.length = 0;
    parent.GsStoredValue.length = 0;
    SetDOB();
}

/**************Common********************/
function SetPriorityList() {
    var arrSpcSvcAvbl = new Array();
    var arrSpcInst = new Array();
    var arrModSvcAvbl = new Array();
    var arrModInst = new Array();
    var SelPriorityID = 0; var PriorityID = 0; var ModalityID = 0; var SpeciesID = 0; var InstID = "";
    var Exception = "";
    //debugger;

    if (objhdnAfterHrs.value == "Y") {
        if (parent.Trim(objhdnSpcSvcAvblAH.value) != "") arrSpcSvcAvbl = objhdnSpcSvcAvblAH.value.split(Divider);
        if (parent.Trim(objhdnSpcSvcAvblAHExInst.value) != "") arrSpcInst = objhdnSpcSvcAvblAHExInst.value.split(Divider);
        if (parent.Trim(objhdnModSvcAvblAH.value) != "") arrModSvcAvbl = objhdnModSvcAvblAH.value.split(Divider);
        if (parent.Trim(objhdnModSvcAvblAHExInst.value) != "") arrModInst = objhdnModSvcAvblAHExInst.value.split(Divider);
    }
    else {
        if (parent.Trim(objhdnSpcSvcAvbl.value) != "") arrSpcSvcAvbl = objhdnSpcSvcAvbl.value.split(Divider);
        if (parent.Trim(objhdnSpcSvcAvblExInst.value) != "") arrSpcInst = objhdnSpcSvcAvblExInst.value.split(Divider);
        if (parent.Trim(objhdnModSvcAvbl.value) != "") arrModSvcAvbl = objhdnModSvcAvbl.value.split(Divider);
        if (parent.Trim(objhdnModSvcAvblExInst.value) != "") arrModInst = objhdnModSvcAvblExInst.value.split(Divider);
    }

    SpeciesID = parseInt(objddlSpecies.value);
    ModalityID = parseInt(objddlModality.value);
    SelPriorityID = parseInt(objddlPriority.value);
    InstID = objddlInstitution.value;

    for (var i = 0; i < objddlPriority.length; i++) {
        PriorityID = parseInt(objddlPriority.options[i].value);
        if (PriorityID > 0) {
            /************************************Set Priority for SPECIES************************************/
            if (SpeciesID > 0) {
                for (var j = 0; j < arrSpcSvcAvbl.length; j = j + 4) {

                    if ((parseInt(arrSpcSvcAvbl[j + 2]) == PriorityID) && (parseInt(arrSpcSvcAvbl[j + 1]) == SpeciesID)) {

                        if (arrSpcSvcAvbl[j + 3] == "N") {
                            Exception = "N";

                            for (var k = 0; k < arrSpcInst.length; k = k + 4) {
                                if ((arrSpcInst[k + 1] == SpeciesID) && (arrSpcInst[k + 2] == PriorityID) && (arrSpcInst[k + 3] == InstID)) {
                                    Exception = "Y";
                                    break;
                                }
                            }

                            if (Exception == "N") {
                                objddlPriority.options[i].style.color = "#bbb";

                                if (PriorityID == SelPriorityID) {
                                    objddlPriority.style.color = "#bbb";
                                }
                            }
                        }
                        else if (arrSpcSvcAvbl[j + 3] == "Y") {
                            Exception = "N";

                            for (var k = 0; k < arrSpcInst.length; k = k + 4) {
                                if ((arrSpcInst[k + 1] == SpeciesID) && (arrSpcInst[k + 2] == PriorityID) && (arrSpcInst[k + 3] == InstID)) {
                                    Exception = "Y";
                                    break;
                                }
                            }
                            if (Exception == "Y") {
                                objddlPriority.options[i].style.color = "#bbb";

                                if (PriorityID == SelPriorityID) {
                                    objddlPriority.style.color = "#bbb";
                                }
                            }
                        }
                        break;
                    }
                }
            }
            /************************************Set Priority for SPECIES************************************/
            /************************************Set Priority for MODALITY************************************/
            if (ModalityID > 0) {
                for (var j = 0; j < arrModSvcAvbl.length; j = j + 4) {
                    if ((parseInt(arrModSvcAvbl[j + 2]) == PriorityID) && (parseInt(arrModSvcAvbl[j + 1]) == ModalityID)) {
                        if (arrModSvcAvbl[j + 3] == "N") {
                            Exception = "N";

                            for (var k = 0; k < arrModInst.length; k = k + 4) {
                                if ((arrModInst[k + 1] == ModalityID) && (arrModInst[k + 2] == PriorityID) && (arrModInst[k + 3] == InstID)) {
                                    Exception = "Y";
                                    break;
                                }
                            }
                            if (Exception == "N") {
                                if (objddlPriority.options[i].style.color == "rgb(0, 0, 0)") {
                                    objddlPriority.options[i].style.color = "#bbb";

                                    if (PriorityID == SelPriorityID) {
                                        objddlPriority.style.color = "#bbb";
                                    }
                                }
                            }
                        }
                        else if (arrModSvcAvbl[j + 3] == "Y") {
                            Exception = "N";

                            for (var k = 0; k < arrModInst.length; k = k + 4) {
                                if ((arrModInst[k + 1] == ModalityID) && (arrModInst[k + 2] == PriorityID) && (arrModInst[k + 3] == InstID)) {
                                    Exception = "Y";
                                    break;
                                }
                            }
                            if (Exception == "Y") {
                                if (objddlPriority.options[i].style.color == "rgb(0, 0, 0)") {
                                    objddlPriority.options[i].style.color = "#bbb";

                                    if (PriorityID == SelPriorityID) {
                                        objddlPriority.style.color = "#bbb";
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            /************************************Set Priority for MODALITY************************************/

        }
    }
}
function ResetPriorityList() {
    if (selTheme == "DEFAULT") objddlPriority.style.color = "#000";
    else if (selTheme == "DARK") objddlPriority.style.color = "#fff";
    for (var i = 0; i < objddlPriority.length; i++) {
        if (selTheme == "DEFAULT") objddlPriority.options[i].style.color = "#000";
        else if (selTheme == "DARK") objddlPriority.options[i].style.color = "#fff";
    }
    SetPriorityList();
}
/**************Common********************/

function ddlPriority_OnChange() {
    var PriorityId = objddlPriority.value;
    var arrPriority = objhdnPriority.value.split(Divider);

    if (parseInt(PriorityId) > 0 && objhdnAfterHrs.value == "Y") {
        for (var i = 0; i <= arrPriority.length; i = i + 2) {
            if (parseInt(arrPriority[i]) == parseInt(PriorityId)) {
                if (arrPriority[i + 1] == "Y") document.getElementById("imgCheckSvc").style.display = "inline";
                else document.getElementById("imgCheckSvc").style.display = "none";
                break;
            }
        }
    }
    else {
        document.getElementById("imgCheckSvc").style.display = "none";
    }
    ResetPriorityList();
}
function imgCheckSvc_OnClick() {
    if (parseInt(objddlSpecies) > 0 && parseInt(objddlModality.value) > 0 && objddlInstitution.value != "00000000-0000-0000-0000-000000000000") {
        var ArrParams = new Array();

        try {
            parent.PopupProcess("N");
            ArrParams[0] = objddlSpecies.value;
            ArrParams[1] = objddlModality.value;
            ArrParams[2] = objddlInstitution.value;
            ArrParams[3] = objddlPriority.value.value;

            AjaxPro.timeoutPeriod = 1800000;
            VRSManualSubmission.GetServiceAvailabilityMessage(ArrParams, ShowProcess);
        }
        catch (expErr) {
            parent.HideProcess();
            parent.PopupMessage(RootDirectory, strForm, "imgCheckSvc_OnClick()", expErr.message, "true");
        }
    }
    else {
        parent.PopupMessage(RootDirectory, strForm, "imgCheckSvc_OnClick()", "496", "true");
    }
}
function ShowServiceMessage(Result) {
    var arrRes = new Array();
    parent.GsPopupText = "";
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ShowServiceMessage()", arrRes[1], "true");
            break;
        case "false":
            if (arrRes[1] == "495" || arrRes[1] == "466") {
                parent.PopupMessage(RootDirectory, strForm, "ShowServiceMessage()", arrRes[1], "true");
            }
            else {
                parent.GsText = arrRes[1];
                parent.PopupMessage(RootDirectory, strForm, "ShowServiceMessage()", "", "false");
                SUBMIT_PRIORITY = "Y";
            }
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            break;
    }
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
    var strDOB = ""; var dtDOB = new Date();

    SetModality(parent.GsGlobalTempValue[0]);
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
    //objtxtFromDt.value = parent.GsGlobalTempValue[7];

    if (document.all) strDOB = parent.SetDateFormat(parent.GsGlobalTempValue[7], parent.GsDateFormat, parent.GsDateSep);
    else strDOB = parent.SetDateFormat1(parent.GsGlobalTempValue[7], parent.GsDateFormat, parent.GsDateSep);

    dtDOB = new Date(strDOB);
    objddlDOBMonth.value = dtDOB.getMonth() + 1;
    objddlDOBYear.value = dtDOB.getFullYear();
    objhdnDOBDay.value = parent.padZeroPlaces(dtDOB.getDate());

    objddlSex.value = parent.GsGlobalTempValue[8]; if (objddlSex.value == null) objddlSex.value = "";
    objddlPriority.value = parent.GsGlobalTempValue[10]; if (objddlPriority.value == null) objddlPriority.value = "0";
    //txtFromDt_OnBlur();
    ddlDOBMonth_OnChange();
}
function SetModality(Modality) {
    var arrRecords = objhdnModality.value.split(Divider)
    var id = "0";
    for (var i = 0; i < arrRecords.length; i++) {
        var arrFields = arrRecords[i].split(SecDivider);
        id = arrFields[0];
        if (Modality == parent.Trim(arrFields[1])) { objddlModality.value = id; break;}
        else if (Modality == parent.Trim(arrFields[2])) { objddlModality.value = id; break; }
        else if (arrFields[3].indexOf(Modality) > -1) { objddlModality.value = id; break; }
    }
}

function btnSubmit_OnClick(WriteBack, MergeStatus) {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrImg = new Array();
    var arrDCM = new Array();
    var arrSTs = new Array();
    var arrDocs = new Array();
    var strDOS = ""; var strDOB = "";
    var sep = parent.GsDateSep;
    var dt = new Date();

    if (parent.Trim(objtxtDOS.value) == "") strDOS = "01Jan1900";
    else {
        if (document.all)
            strDOS = parent.SetDateFormat(objtxtDOS.value, parent.GsDateFormat, sep);
        else
            strDOS = parent.SetDateFormat1(objtxtDOS.value, parent.GsDateFormat, sep);
    }
    //if (parent.Trim(objtxtFromDt.value) == "") strDOB = "01Jan1900";
    //else {
    //    if (document.all)
    //        strDOB = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    //    else
    //        strDOB = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    //}

    if (document.all) strDOB = parent.SetDateFormat(objddlDOBMonth.value + sep + objddlDOBDay.value + sep + objddlDOBYear.value, "mm" + sep + "dd" + sep + "yyyy", sep);
    else strDOB = parent.SetDateFormat1(objddlDOBMonth.value + sep + objddlDOBDay.value + sep + objddlDOBYear.value, "mm" + sep + "dd" + sep + "yyyy", sep);

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
        ArrRecords[19] = objddlInstitution.value;
        ArrRecords[20] = objddlInstitution.options[objddlInstitution.selectedIndex].text;
        ArrRecords[21] = objddlPhys.value;
        ArrRecords[22] = "N"; if (objrdoConsY.checked) ArrRecords[26] = "Y";
        ArrRecords[23] = objddlCategory.value;
        ArrRecords[24] = objhdnDCMMODIFYEXEPATH.value;
        ArrRecords[25] = dt.getTimezoneOffset().toString();
        ArrRecords[26] = SUBMIT_PRIORITY;
        ArrRecords[27] = objddlCountry.value;
        ArrRecords[28] = objddlState.value;
        ArrRecords[29] = objtxtCity.value;
        ArrRecords[30] = CHECK_DOB;
        ArrRecords[31] = UserID;
        ArrRecords[32] = SessionID;

        arrSTs = GetStudyTypes();
        
        arrImg = GetImageList();
        arrDCM = GetDCMList();
        arrDocs = GetDocList();

        if (ErrFlag == 0) {
            if (objddlInstitution.value != "00000000-0000-0000-0000-000000000000") {
                AjaxPro.timeoutPeriod = 1800000;
                VRSManualSubmission.SaveRecord(ArrRecords, arrSTs,arrDCM, arrImg,arrDocs, ShowProcess);
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
    var itemIndex = 0; var fileType = "";
    var gridItem;
    var arrDCM = new Array(); var idx = 0;
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        fileType = gridItem.get_cells()[2].get_value().toString();
        if (fileType == "D") {
            arrDCM[idx] = gridItem.get_cells()[0].get_value().toString();
            arrDCM[idx + 1] = gridItem.get_cells()[1].get_value().toString();
            idx = idx + 2;
        }
        itemIndex++;
    }
    return arrDCM;
}
function GetImageList() {
    var itemIndex = 0;
    var gridItem;
    var arrImg = new Array(); var idx = 0;
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        fileType = gridItem.get_cells()[2].get_value().toString();
        if (fileType == "I") {
            arrImg[idx] = gridItem.get_cells()[0].get_value().toString();
            arrImg[idx + 1] = gridItem.get_cells()[1].get_value().toString();
            idx = idx + 2;
        }
        itemIndex++;
    }
    return arrImg;
}
function GetDocList() {
    var itemIndex = 0;
    var gridItem;
    var arrDocs = new Array(); var idx = 0; var fileType = "";
    while (gridItem = grdSF.get_table().getRow(itemIndex)) {
        fileType = gridItem.get_cells()[2].get_value().toString();
        if (fileType == "P") {
            arrDocs[idx] = gridItem.get_cells()[0].get_value().toString();
            arrDocs[idx + 1] = gridItem.get_cells()[1].get_value().toString();
            idx = idx + 2;
        }
        itemIndex++;
    }
    return arrDocs;
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
            else if (arrRes[1] == "427") {

                //parent.GiWidth = 600;
                //parent.GiTop = 25;
                //parent.GsLaunchURL = "CaseList/VRSBeyondOTSubmitConfirm.aspx?msg=427&ot=" + arrRes[3] + "&std=" + arrRes[4] + "&stat=" + arrRes[5] + "&tz=" + arrRes[7];
                //parent.GsText = arrRes[6];
                //strLoadPopup = "N";
                //parent.PopupDataList();
                parent.GsText = arrRes[5];
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", "", "false");
                SUBMIT_PRIORITY = "Y";
            }
            else if (arrRes[1] == "488") {
                parent.GsDlgConfAction = "SAV";
                parent.PopupConfirm(arrRes[1]);
            }
            else
            {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            }
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.GsNavURL = "CaseList/VRSInProgressBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=21";
            btnDlgClose_Onclick();
            //parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            //btnClose_OnClick();
            break;
    }
}
function ProcessSave(Args) {
    if (Args == "Y") {
        CHECK_DOB = "N";
        btnSubmit_OnClick(WRITE_BACK, "");
    }
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

    if (parent.Trim(objtxtFromDt.value) != "") {
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
function ddlDOBMonth_OnChange() {
    var arrParams = new Array();
    var arrRes = new Array();
    var strDOS = "";
    var Result;

    try {
        if (objddlDOBYear.value == "1900") {
            if (parent.Trim(objtxtDOS.value) == "") strDOS = "01Jan1900";
            else {
                if (document.all)
                    strDOS = parent.SetDateFormat(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
                else
                    strDOS = parent.SetDateFormat1(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
            }
            var dt = new Date(strDOS);
            objhdnDOBDay.value = dt.getDate();
            objddlDOBMonth.value = dt.getMonth() + 1;
            objddlDOBYear.value = dt.getFullYear();
        }

        arrParams[0] = objddlDOBMonth.value;
        arrParams[1] = objddlDOBYear.value;
        AjaxPro.timeoutPeriod = 1800000;
        Result = VRSManualSubmission.FetchLastDayOfMonth(arrParams);

        arrRes = Result.value;
        switch (arrRes[0]) {
            case "catch":
                parent.PopupMessage(RootDirectory, strForm, "ddlDOBMonth_OnChange()", arrRes[1], "true");
                break;
            case "true":
                objddlDOBDay.length = 0;
                for (var i = 1; i <= parseInt(arrRes[1]) ; i++) {
                    var op = document.createElement("option");
                    op.value = parent.padZeroPlaces(i);
                    op.text = parent.padZeroPlaces(i);
                    if (op.value == objhdnDOBDay.value) op.selected = true;
                    objddlDOBDay.add(op);
                }
                SetDOB();
                break;
        }

    }
    catch (expErr) {
        parent.PopupMessage(RootDirectory, strForm, "ddlDOBMonth_OnChange()", expErr.message, "true");
    }

}
function ddlDOBDay_OnChange() {
    SetDOB();
}
function ddlDOBYear_OnChange() {
    var arrParams = new Array();
    var arrRes = new Array();
    var Result;

    try {
        if (objddlDOBMonth.value == "2") {
            arrParams[0] = objddlDOBMonth.value;
            arrParams[1] = objddlDOBYear.value;
            AjaxPro.timeoutPeriod = 1800000;
            Result = VRSManualSubmission.FetchLastDayOfMonth(arrParams);

            arrRes = Result.value;
            switch (arrRes[0]) {
                case "catch":
                    parent.PopupMessage(RootDirectory, strForm, "ddlDOBMonth_OnChange()", arrRes[1], "true");
                    break;
                case "true":
                    objddlDOBDay.length = 0;
                    for (var i = 1; i <= parseInt(arrRes[1]) ; i++) {
                        var op = document.createElement("option");
                        op.value = parent.padZeroPlaces(i);
                        op.text = parent.padZeroPlaces(i);
                        if (op.value == objhdnDOBDay.value) op.selected = true;
                        objddlDOBDay.add(op);
                    }
                    SetDOB();
                    break;
            }
        }
        else
            SetDOB();

    }
    catch (expErr) {
        parent.PopupMessage(RootDirectory, strForm, "ddlDOBMonth_OnChange()", expErr.message, "true");
    }

}
function SetDOB() {
    var strDOB = "";
    var strDtToday = "";
    var dtFrom = new Date();
    var dtToday = new Date();
    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    var sep = parent.GsDateSep;


    if (document.all) strDOB = parent.SetDateFormat(objddlDOBMonth.value + sep + objddlDOBDay.value + sep + objddlDOBYear.value, "mm" + sep + "dd" + sep + "yyyy", sep);
    else strDOB = parent.SetDateFormat1(objddlDOBMonth.value + sep + objddlDOBDay.value + sep + objddlDOBYear.value, "mm" + sep + "dd" + sep + "yyyy", sep);

    //strDtToday = dtToday.getDate().toString() + monthName[dtToday.getMonth()] + dtToday.getFullYear();

    if (document.all) strDtToday = parent.SetDateFormat(objtxtDOS.value, parent.objhdnDateFormat.value, parent.objhdnDateSep.value);
    else strDtToday = parent.SetDateFormat1(objtxtDOS.value, parent.objhdnDateFormat.value, parent.objhdnDateSep.value);

    dtFrom = new Date(strDOB);
    dtToday = new Date(strDtToday);
    objhdnDOBDay.value = objddlDOBDay.value;

    if (dtFrom < dtToday) {
        CalculateAge(strDOB);
    }
    else {
        parent.PopupMessage(RootDirectory, strForm, "SetDOB()", "338", "true");
    }
}
function txtDOS_OnBlur() {
     SetDOB();
}
function CalculateAge(strDOB) {
    var strDtTill = "";
    var dtFrom = new Date(); var dtTill = new Date();
    var diff = 0; var ageYrs = 0; var ageMth = 0; var age = "";
    //if (parent.Trim(objtxtFromDt.value) != "") {
    //if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
    //else {
    //    if (document.all)
    //        strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    //    else
    //        strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    //}
    if (parent.Trim(objtxtDOS.value) == "") strDtTill = "01Jan1900";
    else {
        if (document.all)
            strDtTill = parent.SetDateFormat(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtTill = parent.SetDateFormat1(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
    }
    dtFrom = new Date(strDOB);
    dtTill = new Date(strDtTill);
    diff = dateDiffInDays(dtFrom, dtTill);

    //ageYrs = (diff / 365).toFixed(0);
    //ageMth = (diff / 12).toFixed(0) - ;
    //if (diff <= 0) {
    //    age = "0 year 0 month";
    //}
    //else {
        age = getAge(dtFrom, dtTill)
    //}
    objtxtAge.value = age;
    //objtxtAge.value = ageYrs.toString() + "Years " + ageMth.toString() + " Months";
    //}
    //else {
    //    objtxtAge.value = "0 year 0 month";
    //}
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
    //if ((parseInt(months) == 0) && (parseInt(days) > 0)) months = 1;
    var age = years + ' years ' + months + ' months ' + days + ' days';
    //var age = years + ' years ' + months + ' months ';
    return age;
}

function ddlCountry_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlCountry.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSManualSubmission.FetchStates(ArrRecords, ShowProcess);
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
            if (STATE_ID != "") {
                objddlState.value = STATE_ID;
                STATE_ID = "";
            }
            break;
    }

}
function ddlSpecies_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlSpecies.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSManualSubmission.FetchBreeds(ArrRecords, ShowProcess);
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
        objhdnTempPID.value = "";
        ArrRecords[0] = objddlInstitution.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSManualSubmission.FetchPhysicians(ArrRecords, ShowProcess);
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
            objhdnInstConsAppl.value = arrRes[arrRes.length - 3];
            if (objhdnInstConsAppl.value == "Y") document.getElementById("divCons").style.display = "inline";
            else document.getElementById("divCons").style.display = "none";
            objhdnTempPID.value = arrRes[arrRes.length - 2] + "-" + arrRes[arrRes.length - 1];
            if (grdSF.recordNumber != null) rc = grdSF.get_recordCount();
            else if (grdSF.RecordCount != null) rc = grdSF.get_recordCount();
            if (rc==0) {
                objtxtPID.value = objhdnTempPID.value;
            }
            break;
    }
    ResetPriorityList();
}

/********Study Types*************************/
function LoadStudyTypes() {
    CallBackST.callback(objhdnID.value, objddlModality.value, objddlInstitution.value);
}
function chkSel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = ""; var strInvBy = "";
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
            if (document.getElementById("hdnModInvBy") != null) strInvBy = document.getElementById("hdnModInvBy").value;
            CallBackSelST.callback("U", strDtls, strInvBy);
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

    if (parent.GsGlobalTempValue.length > 0) {
        strPID = parent.GsGlobalTempValue[1];
        strAccnNo = parent.GsGlobalTempValue[5];
    }

    CallBackSF.callback("L", strFileList, strPID, strAccnNo, UserID);
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
        objiframeUploadSF.src = "VRSUploadStudyFiles.aspx?uid=" + UserID;
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
    CallBackSF.callback("D", strRowID, strExistingDtls, objhdnTempFolder.value);
}
function ClearFiles() {
    var strExistingDtls = GetGridDetails();
    CallBackSF.callback("C", strExistingDtls, objhdnTempDCMFolder.value);
}
/********Study File Grid*****************************/

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
        case "FetchStates":
            PopulateStateList(Result);
            break;
        case "FetchBreeds":
            PopulateBreed(Result);
            break;
        case "FetchPhysicians":
            PopulatePhysicians(Result);
            break;
        case "GetServiceAvailabilityMessage":
            ShowServiceMessage(Result);
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
