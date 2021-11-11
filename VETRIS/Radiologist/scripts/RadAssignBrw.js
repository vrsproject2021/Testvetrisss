$(document).ready($(function () {
    window.scrollTo(0, 0);
}))
function view_Searchform() {
    $("#searchSection").slideToggle(800);
    $("#searchIcon").toggleClass("icon", 800);
}

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
    parent.adjustFrameHeight();
}
function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objtxtPatientName.value = parent.GsFilter[0];
        objddlModality.value = parent.GsFilter[1];
        if (parent.GsFilter[2] == "Y") objchkRecDt.checked = true;
        objtxtFromDt.value = parent.GsFilter[3];
        objtxtTillDt.value = parent.GsFilter[4];
        objddlInstitution.value = parent.GsFilter[5];
        objddlStatus.value = parent.GsFilter[6];
        objddlCategory.value = parent.GsFilter[7];
        objddlSpecies.value = parent.GsFilter[8];
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    var ArrRecords = new Array();
    var strDtFrom = ""; var strDtTill = "";

    if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
    else {
        if (document.all)
            strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if (parent.Trim(objtxtTillDt.value) == "") strDtTill = "01Jan1900";
    else {
        if (document.all)
            strDtTill = parent.SetDateFormat(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtTill = parent.SetDateFormat1(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    ArrRecords[0] = objtxtPatientName.value;
    ArrRecords[1] = objddlModality.value;
    ArrRecords[2] = "N"; if (objchkRecDt.checked) ArrRecords[2] = "Y";
    ArrRecords[3] = strDtFrom;
    ArrRecords[4] = strDtTill;
    ArrRecords[5] = objddlInstitution.value;
    ArrRecords[6] = objddlStatus.value;
    ArrRecords[7] = objddlCategory.value;
    ArrRecords[8] = objddlSpecies.value;
    ArrRecords[9] = UserID;
    ArrRecords[10] = UserRoleCode;

    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
    // view_Searchform();
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtPatientName.value);
    parent.GsFilter[1] = parent.Trim(objddlModality.value);
    if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
    parent.GsFilter[3] = objtxtFromDt.value;
    parent.GsFilter[4] = objtxtTillDt.value;
    parent.GsFilter[5] = parent.Trim(objddlInstitution.value);
    parent.GsFilter[6] = parent.Trim(objddlStatus.value);
    parent.GsFilter[7] = parent.Trim(objddlCategory.value);
    parent.GsFilter[8] = parent.Trim(objddlSpecies.value);
}
function ResetRecord() {
    var strDtFrom = ""; var strDtTill = "";

    var dtFrom = new Date(); var dtTill = new Date();
    dtFrom = dtFrom.addDays(-7);
    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDtFrom = dtFrom.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2);

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDtTill = dtTill.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2);

    objtxtPatientName.value = "";
    objddlModality.value = "0";
    objddlSpecies.value = "0";
    objddlCategory.value = "0";
    objtxtPatientName.value = "";
    objchkRecDt.checked = false;
    objtxtFromDt.value = strDtFrom;
    objtxtTillDt.value = strDtTill;
    objddlInstitution.value = "00000000-0000-0000-0000-000000000000";
    objddlStatus.value = "-1";
    SearchRecord();
}


function btnMultipleAssign_OnClick() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var statID = "0"; idx = 0;
    parent.GsStoredValue.length = 0;
    parent.GsStoredValue = new Array();
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (gridItem.Data[19] == "Y") {
            statID = gridItem.Data[13].toString();
            parent.GsStoredValue[idx] = RowId;
            idx = idx + 1;
        }
        itemIndex++;
    }

    parent.GiWidth = 500;
    parent.GiTop = 15;
    parent.GsLaunchURL = "Radiologist/VRSRadAssignMultiDlg.aspx?statID=" + statID + "&mid=" + MenuID + "&uid=" + UserID + "&th=" + selTheme + "&sid=" + SessionID;
    parent.PopupDataList();
}
function Assign(ID) {
    objhdnID.value = ID;
    
    parent.GiWidth = 200;
    parent.GiTop = 15;
    parent.GsLaunchURL = "Radiologist/VRSRadAssignDlg.aspx?id=" + ID + "&mid=" + MenuID + "&uid=" + UserID + "&th=" + selTheme + "&sid=" + SessionID;
    parent.PopupDataList();
}
function Unassign(ID) {
    objhdnID.value = ID;

    parent.GiWidth = 200;
    parent.GiTop = 15;
    parent.GsLaunchURL = "Radiologist/VRSRadUnassignDlg.aspx?id=" + ID + "&mid=" + MenuID + "&uid=" + UserID + "&th=" + selTheme + "&sid=" + SessionID;
    parent.PopupDataList();
}
function ChkAssign_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var rc = grdBrw.get_recordCount();
    var cnt1 = 0; var cnt2 = 0; var statID1 = "0"; var statID2 = "0";

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
           
            statID2 = gridItem.Data[13].toString();
            if (document.getElementById("chkAsn_" + ID) != null) {
                if (document.getElementById("chkAsn_" + ID).checked) {
                    cnt1 = cnt1 + 1;
                    cnt2 = cnt2 + 1;
                    if (cnt1 == 1) statID1 = gridItem.Data[13].toString();

                    if (statID1 == statID2) {
                        gridItem.Data[19] = "Y";
                    }
                    else {
                        document.getElementById("chkAsn_" + ID).checked = false;
                        parent.PopupMessage(RootDirectory, strForm, "ChkAssign_OnClick()", "409", "true");
                        break;
                    }
                }
                else {
                     gridItem.Data[19] = "N";
                }
            }
        }
        else {
            if (gridItem.Data[19] == "Y") {
                cnt1 = cnt1 + 1;
                cnt2 = cnt2 + 1;
                if (cnt1 == 1) statID1 = gridItem.Data[13].toString();
            }
        }
        
        itemIndex++;
    }

    if (cnt2 > 0) objbtnMultipleAssign.style.display = "inline"; else objbtnMultipleAssign.style.display = "none";
}
function ProcessDataList(Args) {
    SearchRecord();
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
        case "CalTill":
            CalTill.setSelectedDate(dt); CalTill.show();
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
        case "FetchPhysicians":
            PopulatePhysicians(Result);
            break;
        case "UpdatePriority":
            ProcessPriorityUpdate(Result);
            break;
    }

}
