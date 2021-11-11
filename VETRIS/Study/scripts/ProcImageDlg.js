var SUBMITTED = "N"; var DEL_FLAG = ""; var ErrFlag = 0; var FILE_ID = ""; var FILE_NAME = ""; var CategoryID = "0"; var SUBMIT_PRIORITY = "N"; STATE_ID = ""; var CHECK_DOB = "Y"; var MERGE_STATUS = "";
$(document).ready($(function () {
    $("input:text").each(function () {
        if (!($(this).prop('readonly'))) $(this).bind("focus", $(this).select());
    })
    objtxtDOS.focus();
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
    if ((UserRoleID == "1") || (UserRoleID == "2")) document.getElementById("divStudyUID").style.display = "inline";

    //if (objddlFiltInst.value == "00000000-0000-0000-0000-000000000000") {
    //    objddlFiltInst.style.display = "inline";
    //    objbtnFilter.style.display = "inline";
    //}

    if (objhdnAppv.value == "Y") {
        objbtnSave1.style.display = "none";
        objbtnSave2.style.display = "none";
        objbtnSubmit1.style.display = "none";
        objbtnSubmit2.style.display = "none";
    }
    //if (objhdnInstConsAppl.value == "Y") document.getElementById("divCons").style.display = "inline";
    btnFilter_OnClick();
    CategoryID = objddlCategory.value;
    var strDir = objhdnFilePath.value + "/CaseList/Temp/" + UserID + "/";
    var usrID = UserID.replace(/-/g, "");
    var tmpID = createGuid();
    objiframeUpload.src = "../CaseList/VRSUploadFiles.aspx?dir=" + strDir + "&fileID=" + usrID + "_" + tmpID + "&th=" + selTheme;
    ResetPriorityList();
}

/**************Common********************/
function SetPriorityList() {
    var arrSpcSvcAvbl = new Array();
    var arrSpcInst = new Array();
    var arrModSvcAvbl = new Array();
    var arrModInst = new Array();
    var SelPriorityID = 0; var PriorityID = 0; var ModalityID = 0; var SpeciesID = 0; var InstID = "";
    var Exception = "";
    debugger;

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
            VRSProcImageDlg.GetServiceAvailabilityMessage(ArrParams, ShowProcess);
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

function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Study/VRSProcImageDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Study/VRSProcImageDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Study/VRSProcImageBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function btnFilter_OnClick() {
    //if (objhdnID.value != "00000000-0000-0000-0000-000000000000")
    //objddlFiltInst.value = objhdnInstID.value;
    CallBackFiles.callback(objhdnID.value, objddlFiltInst.value, objhdnFTPDLFLDRTMP.value, UserID);
}
function ddlInstitution_OnChange() {
    objhdnInstID.value = objddlInstitution.value;
    Institution_OnChange();
}
function chkSel_OnClick(ID, InstID,CountryID,StateID,City) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ImgCnt = 0;

    ImgCnt = parseInt(objtxtImgCnt.value);
    while (gridItem = grdFiles.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            if (document.getElementById("chkSel_" + RowId).checked) {
                gridItem.Data[4] = "Y";
                ImgCnt = ImgCnt + 1;
                if (objhdnInstID.value == "00000000-0000-0000-0000-000000000000") {
                    objddlCountry.value = CountryID;
                    objtxtCity.value = City;
                    STATE_ID = StateID;
                    objhdnInstID.value = InstID;
                    objddlInstitution.value = InstID;
                }

                Institution_OnChange();
                ddlCountry_OnChange();
                objtxtImgCnt.value = ImgCnt.toString();
                
            }
            else {
                if (ImgCnt > 0) {
                    ImgCnt = ImgCnt - 1;
                    objtxtImgCnt.value = ImgCnt.toString();
                  
                }
            }
            break;
        }
        itemIndex++;
    }
}
function ShowImage(ID, FileName) {
    var strFilePath = "/" + RootDirectory + "/Study/Temp/" + UserID + "/" + FileName;
    var win = window.open(strFilePath, '_blank');
    win.focus();
}
function btnDelFile_OnClick(ID, FileName) {
    FILE_ID = ID;
    FILE_NAME = FileName;
    DEL_FLAG = "FILE";
    parent.GsDlgConfAction = "DEL";
    parent.PopupConfirm("203");
}
function DeleteFile() {
    parent.PopupProcess("N");
    ArrRecords = new Array();
    try {

        ArrRecords[0] = FILE_ID;
        ArrRecords[1] = objhdnFTPDLFLDRTMP.value;
        ArrRecords[2] = FILE_NAME;
        ArrRecords[3] = UserID;
        ArrRecords[4] = MenuID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSProcImageDlg.DeleteImageFile(ArrRecords,ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "DeleteFile()", expErr.message, "true");
    }
}
function ProcessDeleteFile(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteFile()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteFile()", arrRes[1], "true", arrRes[2]);
            btnFilter_OnClick();
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "ProcessDeleteFile()", arrRes[1], "false");
            parent.FetchMenuRecordCount();
            btnFilter_OnClick();
            break;
    }
}

function LoadStudyTypes() {
    CallBackST.callback(objhdnID.value, objddlModality.value, objddlInstitution.value);
}
function Consult_OnClick() {
    if (objrdoConsY.checked) {
        objddlCategory.value = "3";
    }
    else if (objrdoConsN.checked) {
        if (CategoryID != "0") objddlCategory.value = CategoryID;
        else objddlCategory.value = "1";
    }
}

function ddlSpecies_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlSpecies.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSProcImageDlg.FetchBreeds(ArrRecords, ShowProcess);
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
            ResetPriorityList();
            break;
    }

}
function Institution_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlInstitution.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSProcImageDlg.FetchPhysicians(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "Institution_OnChange()()", expErr.message, "true");
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
            for (var i = 1; i < (arrRes.length - 3) ; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlPhys.add(op);
            }
            objhdnInstConsAppl.value = arrRes[arrRes.length - 3];
            if (objhdnInstConsAppl.value == "Y") document.getElementById("divCons").style.display = "inline";
            else document.getElementById("divCons").style.display = "none";

            if (parent.Trim(objtxtPID.value) == "") {
                objtxtPID.value = arrRes[arrRes.length - 2] + "-" + arrRes[arrRes.length - 1];
            }
            LoadStudyTypes();
            break;
    }
    ResetPriorityList();
}

function ddlCountry_OnChange() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlCountry.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSProcImageDlg.FetchStates(ArrRecords, ShowProcess);
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


function txtDOS_OnBlur() {
    if (parent.Trim(objtxtDOS.value) != "") {
        if (ValidateDateEntered(objtxtDOS)) {

            SetDOB();
        }
    }
    else {
        SetDOB();
    }
}
function txtFromDt_OnBlur() {
   

    if (parent.Trim(objtxtFromDt.value) != "") {
        if (ValidateDateEntered(objtxtFromDt)) {

            CalculateAge();
        }
    }
    else {
        CalculateAge();
    }
}
function ddlDOBMonth_OnChange() {
    var arrParams = new Array();
    var arrRes = new Array();
    var Result;

    try {

        arrParams[0] = objddlDOBMonth.value;
        arrParams[1] = objddlDOBYear.value;
        AjaxPro.timeoutPeriod = 1800000;
        Result = VRSProcImageDlg.FetchLastDayOfMonth(arrParams);

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
            Result = VRSProcImageDlg.FetchLastDayOfMonth(arrParams);

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

function chkStudySel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0"; var strDtls = ""; var strInvBy = "";
    CategoryID = "0";
    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (RowId == ID) {
            if (document.getElementById("chkStudySel_" + RowId).checked) {
                if (CategoryID != "2") CategoryID = gridItem.Data[5].toString();
                gridItem.Data[2] = "Y";
                //if (CategoryID == "2") objddlCategory.value = "2";
            }
            else {
                gridItem.Data[2] = "N";
                //if (CategoryID == "2") {
                //    objddlCategory.value = "0";
                //    Consult_OnClick();
                //}
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


function btnSubmit_OnClick(Submitted, MergeStatus) {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrFiles = new Array();
    var arrSTs = new Array();
    var arrDocs = new Array();
    var strDOS = ""; var strDOB = "";
    var sep = parent.GsDateSep;
    var dt = new Date();
    SUBMITTED = Submitted;
    MERGE_STATUS = MergeStatus;

    if (parent.Trim(objtxtDOS.value) == "") strDOS = "01Jan1900";
    else {
        if (document.all)
            strDOS = parent.SetDateFormat(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDOS = parent.SetDateFormat1(objtxtDOS.value, parent.GsDateFormat, parent.GsDateSep);
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

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objhdnSUID.value;
        ArrRecords[2] = objtxtImgCnt.value;
        ArrRecords[3] = objddlInstitution.value;
        ArrRecords[4] = strDOS + " " + objddlHr.value + ":" + objddlMin.value + ":00";
        ArrRecords[5] = objddlModality.value;
        ArrRecords[6] = objtxtPID.value;
        ArrRecords[7] = objtxtPFName.value;
        ArrRecords[8] = objtxtPLName.value;
        ArrRecords[9] = objhdnSeriesUID.value;
        ArrRecords[10] = objhdnSeriesNo.value;
        ArrRecords[11] = objtxtPWt.value;
        ArrRecords[12] = objddlUOM.value;
        ArrRecords[13] = strDOB;
        ArrRecords[14] = objtxtAge.value;
        ArrRecords[15] = objddlSex.value;
        ArrRecords[16] = objddlSN.value;
        ArrRecords[17] = objddlSpecies.value;
        ArrRecords[18] = objddlBreed.value;
        ArrRecords[19] = objtxtOwnerFN.value;
        ArrRecords[20] = objtxtOwnerLN.value;
        ArrRecords[21] = objtxtAccnNo.value;
        ArrRecords[22] = objtxtReason.value;
        ArrRecords[23] = objddlPhys.value;
        ArrRecords[24] = objddlPriority.value;
        ArrRecords[25] = Submitted;
        ArrRecords[26] = MergeStatus;
        ArrRecords[27] = objtxtNotePhys.value;
        ArrRecords[28] = "N"; if (objrdoConsY.checked) ArrRecords[28] = "Y";
        ArrRecords[29] = objddlCategory.value;
        ArrRecords[30] = SUBMIT_PRIORITY;
        ArrRecords[31] = dt.getTimezoneOffset().toString();
        ArrRecords[32] = objddlCountry.value;
        ArrRecords[33] = objddlState.value;
        ArrRecords[34] = objtxtCity.value;
        ArrRecords[35] = CHECK_DOB;
        ArrRecords[36] = UserID;
        ArrRecords[37] = MenuID;
        ArrRecords[38] = SessionID;

        ArrFiles = GetFiles();
        arrSTs = GetStudyTypes();
        arrDocs = GetDocList();

        if (ErrFlag == 0) {
            AjaxPro.timeoutPeriod = 1800000;
            VRSProcImageDlg.SaveRecord(ArrRecords, ArrFiles, arrSTs, arrDocs, ShowProcess);
        }


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSubmit_OnClick()", expErr.message, "true");
    }

}
function GetFiles() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0; var sel = "";
    while (gridItem = grdFiles.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[4].toString();
        if (sel == "Y") {
            arrRecords[idx] = gridItem.Data[1].toString();
            arrRecords[idx + 1] = gridItem.Data[2].toString();
            idx = idx + 2;
        }
        itemIndex++;
    }
    return arrRecords;
}
function GetStudyTypes() {
    var itemIndex = 0; var gridItem;
    var arrST = new Array(); var idx = 0; var sel = ""; var validate = ""; var strName = "";
    var cnt = 0;
    if (grdSelST.recordNumber != null) rc = grdSelST.get_recordCount();
    else if (grdSelST.RecordCount != null) rc = grdSelST.get_recordCount();

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        sel = gridItem.Data[2].toString();
        if (validate != "Y") {
            validate = gridItem.Data[4].toString();
            strName = gridItem.Data[3].toString();
        }

        if (sel == "Y") {
            cnt = cnt + 1;
            arrST[idx] = gridItem.Data[1].toString();
            idx = idx + 1;
        }
        itemIndex++;
    }

    if (validate == "Y" && cnt == 1) {
        ErrFlag = 1;
        arrST.length = 0;
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "GetStudyTypes()", "176", "true", strName);
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
            if (arrRes[1] == "165") {
                parent.GsLaunchURL = "CaseList/VRSMergeConfirm.aspx";
                strLoadPopup = "N";
                parent.PopupGeneralSmall();
            }
            else if (arrRes[1] == "427") {

                //parent.GiWidth = 600;
                //parent.GiTop = 25;
                //parent.GsLaunchURL = "CaseList/VRSBeyondOTSubmitConfirm.aspx?msg=427&ot=" + arrRes[2] + "&std=" + arrRes[3] + "&stat=" + arrRes[4] + "&tz=" + arrRes[6];
                //parent.GsText = arrRes[5];
                //strLoadPopup = "N";
                //parent.PopupDataList();
                parent.GsText = arrRes[4];
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", "", "false");
                SUBMIT_PRIORITY = "Y";
            }
            else if (arrRes[1] == "488") {
                parent.GsDlgConfAction = "SAV";
                parent.PopupConfirm(arrRes[1]);
            }
            else
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            if (SUBMITTED == "Y") {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false","","","Y");
                objbtnSubmit1.style.display = "none"; objbtnSubmit2.style.display = "none";
                objbtnReset1.style.display = "none"; objbtnReset2.style.display = "none";
                objbtnClose1.style.display = "none"; objbtnClose2.style.display = "none";
            }
            else {
                parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
                objhdnID.value = arrRes[2];
                objhdnSUID.value = arrRes[3];
                objlblSUID.innerHTML = arrRes[3];
                objhdnSeriesUID.value = arrRes[4];
                objhdnSeriesNo.value = arrRes[5];
                btnFilter_OnClick();
            }
            break;
    }
}
function ProcessGeneralSmall(Args) {
    if (Args != null) btnSubmit_OnClick(SUBMITTED, Args);
}
function ProcessMessage(Args) {
    parent.PopupLoad();
    for (var i = 0; i <= 10000; i++);
    parent.HideLoad();
    parent.NavMenu("CaseList/VRSInProgressBrw.aspx", 21, "Y", 1);
}
function ProcessDataList(ArgsRet) {
    if (ArgsRet == "0") {
        parent.GsRetStatus = "false";
        btnClose_OnClick();
    }
    else {
        btnSubmit_OnClick("Y", "X", ArgsRet);
    }
}
function ProcessSave(Args) {
    if (Args == "Y") {
        CHECK_DOB = "N";
        btnSubmit_OnClick(WRITE_BACK, MERGE_STATUS);
    }
}

function ToggleDocUpload() {
    var e = $('#aRDOCCollapse').closest(".searchSection"),
                       t = $('#aRDOCCollapse').find("i"),
                       n = e.find("#divDocUpload");

    e.attr("style") ? n.slideToggle(200, function () {
        e.removeAttr("style")
    }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
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
        case "FILE":
            DeleteFile();
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
        case "CalDOS":
            CalDOS.setSelectedDate(dt); CalDOS.show();
            break;
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
        case "FetchStates":
            PopulateStateList(Result);
            break;
        case "SaveRecord":
            SaveRecord(Result);
            break;
        case "FetchBreeds":
            PopulateBreed(Result);
            break;
        case "FetchPhysicians":
            PopulatePhysicians(Result);
            break;
        case "DeleteImageFile":
            ProcessDeleteFile(Result);
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

