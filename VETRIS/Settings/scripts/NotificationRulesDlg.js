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
    else {
        SetPageValue();
    }

    objhdnError.value = "";

}
function SetPageValue() {
    CallBackRadiologist.callback(objhdnID.value);
    CallBackMatrix.callback(objhdnID.value);
}
function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        btnBrwAdd_Onclick('Settings/VRSCaseNotificationRulesDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Settings/VRSCaseNotificationRulesDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEdit_Onclick('Settings/VRSCaseNotificationRulesDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Settings/VRSCaseNotificationRulesDlg.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    parent.GsNavURL = "Settings/VRSCaseNotificationRulesBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}

function NotifyByTime_OnClick() {
    if (objrdoNotifyE.checked) {
        objddlEllapseHr.disabled = false;
        objddlEllapseMin.disabled = false;
        objddlLeftHr.value = "00";
        objddlLeftMin.value = "00";
        objddlLeftHr.disabled = true;
        objddlLeftMin.disabled = true;
    }
    else if (objrdoNotifyL.checked) {
        objddlEllapseHr.value = "00";
        objddlEllapseMin.value = "00";
        objddlEllapseHr.disabled = true;
        objddlEllapseMin.disabled = true;
        objddlLeftHr.disabled = false;
        objddlLeftMin.disabled = false;
    }
}
function chkRadSchedule_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();

        if (RowId == ID) {
            if (document.getElementById("chkRadSchedule_" + RowId.toString()).checked) {
                gridItem.Data[3] = "Y"; 
            }
            else {
                gridItem.Data[3] = "N";
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function chkRadAlways_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();

        if (RowId == ID) {
            if (document.getElementById("chkRadAlways_" + RowId.toString()).checked) {
                gridItem.Data[4] = "Y";
            }
            else {
                gridItem.Data[4] = "N";
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}

function ChkSchedule_OnClick(ID) {
    var itemIndex = 0; var gridItem; var HdrRowId = "0";

    while (gridItem = grdMatrix.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.get_cells()[0].get_value().toString();

        if (HdrRowId == ID) {
            if (document.getElementById("chkSchedule_" + HdrRowId.toString()).checked) {
                gridItem.Data[3] = "Y";
                document.getElementById("chkNotifyAll_" + HdrRowId.toString()).disabled = true;
            }
            else {
                gridItem.Data[3] = "N";
                document.getElementById("chkNotifyAll_" + HdrRowId.toString()).disabled = false;
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function ChkNotifyAll_OnClick(ID) {
    var itemIndex = 0; var gridItem; var HdrRowId = "0";
    var ItemRecCount = 0; var strSel = ""; var RowId = "0"; var SelCount = 0;
    while (gridItem = grdMatrix.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.get_cells()[0].get_value().toString();

        if (HdrRowId == ID) {
            if (document.getElementById("chkNotifyAll_" + HdrRowId.toString()).checked) strSel = "Y"; else strSel = "N";
            gridItem.Data[4] = strSel;
            if (gridItem.Data.length > 6) {
                ItemRecCount = gridItem.Data[6].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    RowId = gridItem.Data[6][i][0].toString();
                    gridItem.Data[6][i][4] = strSel;
                    if (strSel == "Y") SelCount = SelCount + 1;
                    if (document.getElementById("chkSel_" + RowId.toString()) != null) {
                        if (strSel == "Y") 
                            document.getElementById("chkSel_" + RowId.toString()).checked = true;
                        else
                            document.getElementById("chkSel_" + RowId.toString()).checked = false;
                    }
                }
                if (document.all) document.getElementById("lnkCnt_" + HdrRowId).innerText = SelCount.toString();
                else document.getElementById("lnkCnt_" + HdrRowId).textContent = SelCount.toString();
                gridItem.Data[5] = SelCount;
            }
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function Count_OnClick(ID) {
    var itemIndex = 0; var gridItem; var HdrRowId = "0";

    while (gridItem = grdMatrix.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.get_cells()[0].get_value().toString();
        if (HdrRowId == ID) {
            grdMatrix.ToggleExpand(event, gridItem, itemIndex.toString())
            break;
        }
        itemIndex++;
    }
}
function ChkSel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var HdrRowId = "0";
    var ItemRecCount = 0; var strSel = ""; var RowId = "0"; var selCount = 0;
    while (gridItem = grdMatrix.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.get_cells()[0].get_value().toString();
        if (gridItem.Data.length > 6) {
            ItemRecCount = gridItem.Data[6].length;
            for (var i = 0; i < ItemRecCount; i++) {
                RowId = gridItem.Data[6][i][0].toString();
                if (RowId == ID) {
                    if (document.getElementById("chkSel_" + RowId.toString()).checked) {
                        gridItem.Data[6][i][4] = "Y";
                        for (var j = 0; j < ItemRecCount; j++) {
                            if (gridItem.Data[6][j][4].toString() == "N") break;
                            else selCount == selCount + 1;
                        }
                        if (selCount == ItemRecCount) {
                            if (document.getElementById("chkNotifyAll_" + HdrRowId.toString()) != null) {
                                document.getElementById("chkNotifyAll_" + HdrRowId.toString()).checked = true;
                            }
                            gridItem.Data[4] = "Y";
                        }
                    }
                    else {
                        gridItem.Data[6][i][4] = "N";
                        if (document.getElementById("chkNotifyAll_" + HdrRowId) != null) {
                            document.getElementById("chkNotifyAll_" + HdrRowId).checked = false;
                            gridItem.Data[4] = "N";
                        }
                    }

                    selCount = 0;
                    for (var j = 0; j < ItemRecCount; j++) {
                        if (gridItem.Data[6][j][4].toString() == "Y") selCount = selCount + 1;
                    }

                    if (document.all) document.getElementById("lnkCnt_" + HdrRowId).innerText = selCount.toString();
                    else document.getElementById("lnkCnt_" + HdrRowId).textContent = selCount.toString();


                    gridItem.Data[5] = selCount;
                    break;
                }
            }
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function CalculateMinutes(Flag) {
    var tot_mins = 0;
    if(Flag=="E")
        tot_mins = (parseInt(objddlEllapseHr.value) * 60) + parseInt(objddlEllapseMin.value);
    else if (Flag == "L")
        tot_mins = (parseInt(objddlLeftHr.value) * 60) + parseInt(objddlLeftMin.value);
    return tot_mins.toString();
}


function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrRadList = new Array();
    var arrOthRecList = new Array();

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objtxtDesc.value;
        ArrRecords[2] = objddlStudyStatus.value;
        ArrRecords[3] = objddlPriority.value;
        ArrRecords[4] = "E"; if (objrdoNotifyL.checked) ArrRecords[4] = "L";
        ArrRecords[5] = CalculateMinutes("E");
        ArrRecords[6] = CalculateMinutes("L");
        ArrRecords[7] = "Y"; if (objrdoStatNo.checked) ArrRecords[7] = "N";
        ArrRecords[8] = UserID;
        ArrRecords[9] = MenuID;

        arrRadList = GetRadiologistList();
        arrOthRecList = GetOtherRecepientList();
        AjaxPro.timoutPeriod = 1800000;
        VRSCaseNotificationRulesDlg.SaveRecord(ArrRecords,arrRadList, arrOthRecList, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetRadiologistList() {
    var arrList = new Array();
    var idx = 0;
    var itemIndex = 0; var gridItem; var RowId = "0";
    var user_id = "";var scheduled = "N"; var always = "N";


    try {

        while (gridItem = grdRad.get_table().getRow(itemIndex)) {
            RowId = gridItem.Data[0].toString();
            user_id = gridItem.Data[1].toString();
            scheduled = gridItem.Data[3].toString();
            always = gridItem.Data[4].toString();

            if ((scheduled == "Y") || (always == "Y")) {
                arrList[idx] = RowId;
                arrList[idx + 1] = user_id;
                arrList[idx + 2] = scheduled;
                arrList[idx + 3] = always;
                idx = idx + 4;
            }

            itemIndex++;
        }

    }
    catch (expErr) {
        parent.HideProcess();
        arrList.length = 0;
        parent.PopupMessage(RootDirectory, strForm, "GetRadiologistList()", expErr.message, "true");
    }

    return arrList;
}
function GetOtherRecepientList() {
    var arrList = new Array();
    var idx = 0;
    var itemIndex = 0; var gridItem; var HdrRowId = "0";
    var ItemRecCount = 0; var RowId = "0";
    var scheduled = "N"; var notify_all = "N";
    var user_id = ""; var selected = "";

    try {

        while (gridItem = grdMatrix.get_table().getRow(itemIndex)) {
            HdrRowId = gridItem.Data[0].toString();
            scheduled = gridItem.Data[3].toString();
            notify_all = gridItem.Data[4].toString();

            if (gridItem.Data.length > 6) {
                ItemRecCount = gridItem.Data[6].length;
                for (var i = 0; i < ItemRecCount; i++) {

                    RowId = gridItem.Data[6][i][0].toString();
                    user_id = gridItem.Data[6][i][1].toString();
                    selected = gridItem.Data[6][i][4].toString();

                    if (selected == "Y") {
                        arrList[idx] = HdrRowId;
                        arrList[idx + 1] = scheduled;
                        arrList[idx + 2] = notify_all;
                        arrList[idx + 3] = user_id;
                        idx = idx + 4;
                    }

                }
            }
            else {
                if (notify_all == "Y") {
                    arrList[idx] = HdrRowId;
                    arrList[idx + 1] = scheduled;
                    arrList[idx + 2] = notify_all;
                    arrList[idx + 3] = "00000000-0000-0000-0000-000000000000";
                    idx = idx + 4;
                }
            }

            itemIndex++;
        }

    }
    catch (expErr) {
        parent.HideProcess();
        arrList.length = 0;
        parent.PopupMessage(RootDirectory, strForm, "GetOtherRecepientList()", expErr.message, "true");
    }

    return arrList;
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
            objhdnID.value = arrRes[2];
            objtxtRuleNo.value = arrRes[2];
            parent.GsRetStatus = "false";
            break;
    }
}
function ddlRole_OnChange() {
    CallBackMatrix.callback('UR', objddlRole.value);
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;

    }
}