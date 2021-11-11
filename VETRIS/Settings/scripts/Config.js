var strRowID = ""; var SvcID = ""; var SVC_FLAG = "";
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
    CallBackBrw.callback();
    CallBackOT.callback(UserID, MenuID);
    CallBackSMA.callback(UserID, MenuID);
    CallBackSSA.callback(UserID, MenuID);
    CallBackSAAH.callback(UserID, MenuID);
    CallBackSASPAH.callback(UserID, MenuID);
    CallBackDASH.callback(UserID, MenuID);
}
function btnClose_OnClick() {
    Unlock = "Y";
    btnBrwClose_Onclick();
}

/*******************General Settings*******************/
function txtDataTypeStr_OnChange(code, group_id, data_type) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = 0; var GroupID = "";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        GroupID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (GroupID == group_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCountInst = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCountInst; i++) {
                    CtrlCode = gridItem.Data[2][i][0].toString();
                    if (CtrlCode == code) {
                        if (document.getElementById("txtDataTypeStr_" + code) != null) {
                            gridItem.Data[2][i][4] = document.getElementById("txtDataTypeStr_" + code).value;
                        }
                        break;
                    }

                }
            }
        }

        itemIndex++;
    }
}
function ddlDataTypeStr_OnChange(code, group_id, data_type) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = 0; var GroupID = "";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        GroupID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (GroupID == group_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCountInst = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCountInst; i++) {
                    CtrlCode = gridItem.Data[2][i][0].toString();
                    if (CtrlCode == code) {
                        if (document.getElementById("ddlDataTypeStr_" + code) != null) {
                            gridItem.Data[2][i][4] = document.getElementById("ddlDataTypeStr_" + code).value;
                        }
                        break;
                    }

                }
            }
        }

        itemIndex++;
    }
}
function txtDataTypeNumber_OnChange(code, group_id, data_type) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = 0; var GroupID = "";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        GroupID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (GroupID == group_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCountInst = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCountInst; i++) {
                    CtrlCode = gridItem.Data[2][i][0].toString();
                    if (CtrlCode == code) {
                        if (document.getElementById("txtDataTypeNumber_" + code) != null) {
                            gridItem.Data[2][i][5] = document.getElementById("txtDataTypeNumber_" + code).value;
                        }
                        break;
                    }

                }
            }
        }

        itemIndex++;
    }
}
function ddlDataTypeNumber_OnChange(code, group_id, data_type) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = 0; var GroupID = "";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        GroupID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (GroupID == group_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCountInst = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCountInst; i++) {
                    CtrlCode = gridItem.Data[2][i][0].toString();
                    if (CtrlCode == code) {
                        if (document.getElementById("ddlDataTypeNumber_" + code) != null) {
                            gridItem.Data[2][i][5] = document.getElementById("ddlDataTypeNumber_" + code).value;
                        }
                        break;
                    }

                }
            }
        }

        itemIndex++;
    }
}
function txtDataTypeDecimal_OnChange(code, group_id, data_type) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = 0; var GroupID = "";
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        GroupID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (GroupID == group_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCountInst = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCountInst; i++) {
                    CtrlCode = gridItem.Data[2][i][0].toString();
                    if (CtrlCode == code) {
                        if (document.getElementById("txtDataTypeDecimal_" + code) != null) {
                            gridItem.Data[2][i][6] = document.getElementById("txtDataTypeDecimal_" + code).value;
                        }
                        break;
                    }

                }
            }
        }

        itemIndex++;
    }
}
function btnSave_OnClick() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrData = new Array();
    try {
        ArrRecords[0] = UserID;
        ArrRecords[1] = MenuID;
        ArrData = GetData();


        AjaxPro.timoutPeriod = 1800000;
        VRSConfig.SaveRecord(ArrRecords, ArrData, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetData() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0;
    var ItemRecCountInst = 0;
   
    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        if (gridItem.Data.length > 2) {
            ItemRecCountInst = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCountInst; i++) {
                arrRecords[idx] = gridItem.Data[2][i][0].toString();
                arrRecords[idx + 1] = gridItem.Data[2][i][4].toString();
                arrRecords[idx + 2] = gridItem.Data[2][i][5].toString();
                arrRecords[idx + 3] = gridItem.Data[2][i][6].toString();
                arrRecords[idx + 4] = gridItem.Data[2][i][7].toString();
                idx = idx + 5;
            }
        }
        itemIndex++;
    }
    return arrRecords;
}
/*******************General Settings*******************/

/*******************Dashboard*******************/
/* Onchange Dashboard Settings Checkbox Control*/
function IsDashboardMenuEnabled_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ChildRowId = "0";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        dashboardList = gridItem.Data[2];
        for (var i = 0; i < dashboardList.length; i = i + 1) {
            ChildRowId = dashboardList[i][0].toString();
            if (ChildRowId == ID) {
                if (document.getElementById("chkIsEnabled_" + ChildRowId).checked) gridItem.Data[2][i][7] = "Y";
                else gridItem.Data[2][i][7] = "N";
                break;
            }
        }

        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function IsDefault_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ChildRowId = "0";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        dashboardList = gridItem.Data[2];
        for (var i = 0; i < dashboardList.length; i = i + 1) {
            ChildRowId = dashboardList[i][0].toString();
            if (ChildRowId == ID) {
                for (var j = 0; j < gridItem.Data[2].length; j++) {
                    if (gridItem.Data[2][j][8] == 'Y' && gridItem.Data[2][j][0] != ChildRowId) {
                        gridItem.Data[2][j][8] = 'N';
                        $('#chkIsDefault_' + gridItem.Data[2][j][0]).prop('checked', false);
                    }
                }
                if (document.getElementById("chkIsDefault_" + ChildRowId).checked) gridItem.Data[2][i][8] = "Y";
                else gridItem.Data[2][i][8] = "N";
                break;
            }
        }

        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function IsRefreshButtonShow_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ChildRowId = "0";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        dashboardList = gridItem.Data[2];
        for (var i = 0; i < dashboardList.length; i = i + 1) {
            ChildRowId = dashboardList[i][0].toString();
            if (ChildRowId == ID) {
                if (document.getElementById("chkIsRefreshButton_" + ChildRowId).checked) gridItem.Data[2][i][9] = "Y";
                else gridItem.Data[2][i][9] = "N";
                break;
            }
        }

        itemIndex++;
    }
    parent.GsRetStatus = "true";
}

/* Onchange Dashboard Settings Control*/
function txtMenuDesc_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            ChildRowId = ItemRecCountInst[i][0].toString();
            if (ChildRowId == id) {
                if (document.getElementById("txtMenuDesc_" + ChildRowId) != null) {
                    gridItem.Data[2][i][2] = document.getElementById("txtMenuDesc_" + ChildRowId).value;
                }
            }
        }
        itemIndex++;
    }
}
function txtTitleDesc_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            ChildRowId = ItemRecCountInst[i][0].toString();
            if (ChildRowId == id) {
                if (document.getElementById("txtTitleDesc_" + ChildRowId) != null) {
                    gridItem.Data[2][i][10] = document.getElementById("txtTitleDesc_" + ChildRowId).value;
                }
            }
        }
        itemIndex++;
    }
}
function txtNavUrl_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            ChildRowId = ItemRecCountInst[i][0].toString();
            if (ChildRowId == id) {
                if (document.getElementById("txtNavUrl_" + ChildRowId) != null) {
                    gridItem.Data[2][i][3] = document.getElementById("txtNavUrl_" + ChildRowId).value;
                }
            }
        }
        itemIndex++;
    }
}
function txtIcon_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            ChildRowId = ItemRecCountInst[i][0].toString();
            if (ChildRowId == id) {
                if (document.getElementById("txtIcon_" + ChildRowId) != null) {
                    gridItem.Data[2][i][4] = document.getElementById("txtIcon_" + ChildRowId).value;
                }
            }
        }
        itemIndex++;
    }
}
function txtDisplayIndex_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            ChildRowId = ItemRecCountInst[i][0].toString();
            if (ChildRowId == id) {
                if (document.getElementById("txtDisplayIndex_" + ChildRowId) != null) {
                    gridItem.Data[2][i][5] = document.getElementById("txtDisplayIndex_" + ChildRowId).value;
                }
            }
        }
        itemIndex++;
    }
}
function txtRefreshTime_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            ChildRowId = ItemRecCountInst[i][0].toString();
            if (ChildRowId == id) {
                if (document.getElementById("txtRefreshTime_" + ChildRowId) != null) {
                    gridItem.Data[2][i][6] = document.getElementById("txtRefreshTime_" + ChildRowId).value;
                }
            }
        }
        itemIndex++;
    }
}
/*Next Level*/
function txtSlotCount_OnChange(id) {
    var slotCount = $('#txtSlotCount_' + id).val();
    if (slotCount < 2) {
        $('#txtSlotCount_' + id).val(2);
    }
    if (slotCount > 4) {
        $('#txtSlotCount_' + id).val(4);
    }
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    var childRows = [];
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            if (ItemRecCountInst[i].length > 11) {
                childRows = ItemRecCountInst[i][11];
                for (var j = 0; j < childRows.length; j++) {
                    var childRowId = childRows[j][0];
                    if (childRowId == id) {
                        if (document.getElementById("txtSlotCount_" + id) != null) {
                            gridItem.Data[2][i][11][j][3] = document.getElementById("txtSlotCount_" + id).value;
                        }
                    }
                }

            }
        }
        itemIndex++;
    }
}
function txtSlotCount_OnBlur(id) {
    var slotCount = $('#txtSlotCount_' + id).val();
    slotEnableDisable(slotCount, id);
}
function txtSlot4_OnBlur(id) {
    var slotCount = $('#txtSlotCount_' + id).val();
    slotEnableDisable(slotCount, id);
}
function slotEnableDisable(slotCount, id) {
    if (slotCount <= 2) {
        $('#txtSlot3_' + id).val('');
        $('#txtSlot3_' + id).prop('disabled', true);
    } else {
        $('#txtSlot3_' + id).prop('disabled', false);
    }
}
function txtSlot1_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    var childRows = [];
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            if (ItemRecCountInst[i].length > 11) {
                childRows = ItemRecCountInst[i][11];
                for (var j = 0; j < childRows.length; j++) {
                    var childRowId = childRows[j][0];
                    if (childRowId == id) {
                        if (document.getElementById("txtSlot1_" + id) != null) {
                            gridItem.Data[2][i][11][j][4] = document.getElementById("txtSlot1_" + id).value;
                        }
                    }
                }

            }
        }
        itemIndex++;
    }
}
function txtSlot2_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    var childRows = [];
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            if (ItemRecCountInst[i].length > 11) {
                childRows = ItemRecCountInst[i][11];
                for (var j = 0; j < childRows.length; j++) {
                    var childRowId = childRows[j][0];
                    if (childRowId == id) {
                        if (document.getElementById("txtSlot2_" + id) != null) {
                            gridItem.Data[2][i][11][j][5] = document.getElementById("txtSlot2_" + id).value;
                        }
                    }
                }

            }
        }
        itemIndex++;
    }
}
function txtSlot3_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    var childRows = [];
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            if (ItemRecCountInst[i].length > 11) {
                childRows = ItemRecCountInst[i][11];
                for (var j = 0; j < childRows.length; j++) {
                    var childRowId = childRows[j][0];
                    if (childRowId == id) {
                        if (document.getElementById("txtSlot3_" + id) != null) {
                            gridItem.Data[2][i][11][j][6] = document.getElementById("txtSlot3_" + id).value;
                        }
                    }
                }

            }
        }
        itemIndex++;
    }
}

function txtKeyDesc_OnChange(id) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCountInst = []; var GroupID = "";
    var childRows = [];
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        ItemRecCountInst = gridItem.Data[2];
        for (var i = 0; i < ItemRecCountInst.length; i++) {
            if (ItemRecCountInst[i].length > 11) {
                childRows = ItemRecCountInst[i][11];
                for (var j = 0; j < childRows.length; j++) {
                    var childRowId = childRows[j][0];
                    if (childRowId == id) {
                        if (document.getElementById("txtKeyDesc_" + id) != null) {
                            gridItem.Data[2][i][11][j][2] = document.getElementById("txtKeyDesc_" + id).value;
                        }
                    }
                }

            }
        }
        itemIndex++;
    }
}

/*Save dashboard settings*/
function btnSaveDashboard_OnClick() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrData = new Array();
    var ChildArrData = new Array();
    try {
        ArrRecords[0] = UserID;
        ArrRecords[1] = MenuID;
        ArrData = GetDashboardData();
        ChildArrData = GetDashboardChildData();

        AjaxPro.timoutPeriod = 1800000;
        VRSConfig.SaveDashboardRecord(ArrRecords, ArrData, ChildArrData, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSaveDashboard_OnClick()", expErr.message, "true");
    }
}
function GetDashboardData() {
    
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0;
    var ItemRecCountInst = 0;
    

    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        if (gridItem.Data.length > 2) {
            ItemRecCountInst = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCountInst; i++) {
                arrRecords[idx] = gridItem.Data[2][i][0].toString();
                arrRecords[idx + 1] = gridItem.Data[2][i][1].toString();
                arrRecords[idx + 2] = gridItem.Data[2][i][2].toString();
                arrRecords[idx + 3] = gridItem.Data[2][i][3].toString();
                arrRecords[idx + 4] = gridItem.Data[2][i][4].toString();
                arrRecords[idx + 5] = gridItem.Data[2][i][5].toString();
                arrRecords[idx + 6] = gridItem.Data[2][i][6].toString();
                arrRecords[idx + 7] = gridItem.Data[2][i][7].toString();
                arrRecords[idx + 8] = gridItem.Data[2][i][8].toString();
                arrRecords[idx + 9] = gridItem.Data[2][i][9].toString();
                arrRecords[idx + 10] = gridItem.Data[2][i][10].toString();
                idx = idx + 11;
            }
        }
        itemIndex++;
    }
    return arrRecords;
}
function GetDashboardChildData() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0;
    var ItemRecCountInst = 0;
    var childRows = [];
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        if (gridItem.Data.length > 2) {
            ItemRecCountInst = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCountInst; i++) {
                if (gridItem.Data[2][i].length > 11) {
                    childRows = gridItem.Data[2][i][11];
                    for (var j = 0; j < childRows.length; j++) {
                        arrRecords[idx] = gridItem.Data[2][i][11][j][0].toString();
                        arrRecords[idx + 1] = gridItem.Data[2][i][11][j][1].toString();
                        arrRecords[idx + 2] = gridItem.Data[2][i][11][j][2].toString();
                        arrRecords[idx + 3] = gridItem.Data[2][i][11][j][3].toString();
                        arrRecords[idx + 4] = gridItem.Data[2][i][11][j][4].toString();
                        arrRecords[idx + 5] = gridItem.Data[2][i][11][j][5].toString();
                        arrRecords[idx + 6] = gridItem.Data[2][i][11][j][6].toString();
                        idx = idx + 7;
                    }
                }
                
            }
        }
        itemIndex++;
    }
    return arrRecords;
}
function SaveDashboardRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveDashboardRecord()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveDashboardRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SaveDashboardRecord()", arrRes[1], "false");
            //PopulateRecords("V");
            parent.GsRetStatus = "false";
            break;
    }
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
            //PopulateRecords("V");
            parent.GsRetStatus = "false";
            break;
    }
}
/*******************Dashboard*******************/

/*******************Operation Hours*******************/
function txtFromTime_OnBlur(id) {
    objCtrl = document.getElementById("txtFromTime_" + id.toString());
    var arrTime = new Array(); var arrHrs = new Array();
    var itemIndex = 0; var gridItem; var strRowID = "";
    while (gridItem = grdOT.get_table().getRow(itemIndex)) {
        strRowID = gridItem.get_cells()[0].get_value();
        if (strRowID == id) {
            objCtrl = document.getElementById("txtFromTime_" + id.toString());
            if (parent.ValidateTimeInput(document.getElementById("txtFromTime_" + id.toString()).value)) {
                if (document.getElementById("txtFromTime_" + id.toString()).value.length < 5) {
                    if (document.getElementById("txtFromTime_" + id.toString()).value.indexOf(":") > 1) {
                        arrTime = document.getElementById("txtFromTime_" + id.toString()).value.split(":");
                        if (arrTime[0].length < 2) arrTime[0] = parent.padZeroPlaces(arrTime[0]);
                        if (arrTime[1].length < 2) arrTime[1] = parent.padZeroPlaces(arrTime[1]);
                        document.getElementById("txtFromTime_" + id.toString()).value = arrTime[0] + ":" + arrTime[1];
                    }
                    else
                        document.getElementById("txtFromTime_" + id.toString()).value = "00:00";
                }
            }
            else {
                document.getElementById("txtFromTime_" + id.toString()).value = "00:00";
            }
            arrHrs = document.getElementById("txtFromTime_" + id.toString()).value.split(":");
            gridItem.Data[2] = document.getElementById("txtFromTime_" + id.toString()).value;
            break;
        }
        itemIndex++;
    }
}
function txtTillTime_OnBlur(id) {
    objCtrl = document.getElementById("txtTillTime_" + id.toString());
    var arrTime = new Array(); var arrHrs = new Array();
    var itemIndex = 0; var gridItem; var strRowID = "";
    while (gridItem = grdOT.get_table().getRow(itemIndex)) {
        strRowID = gridItem.get_cells()[0].get_value();
        if (strRowID == id) {
            objCtrl = document.getElementById("txtTillTime_" + id.toString());
            if (parent.ValidateTimeInput(document.getElementById("txtTillTime_" + id.toString()).value)) {
                if (document.getElementById("txtTillTime_" + id.toString()).value.length < 5) {
                    if (document.getElementById("txtTillTime_" + id.toString()).value.indexOf(":") > 1) {
                        arrTime = document.getElementById("txtTillTime_" + id.toString()).value.split(":");
                        if (arrTime[0].length < 2) arrTime[0] = parent.padZeroPlaces(arrTime[0]);
                        if (arrTime[1].length < 2) arrTime[1] = parent.padZeroPlaces(arrTime[1]);
                        document.getElementById("txtTillTime_" + id.toString()).value = arrTime[0] + ":" + arrTime[1];
                    }
                    else
                        document.getElementById("txtTillTime_" + id.toString()).value = "00:00";
                }

            }
            else {
                document.getElementById("txtTillTime_" + id.toString()).value = "00:00";
            }

            arrHrs = document.getElementById("txtTillTime_" + id.toString()).value.split(":");
            gridItem.Data[3] = document.getElementById("txtTillTime_" + id.toString()).value;
            break;
        }
        itemIndex++;
    }
}
function ddlTZ_OnChange(id) {

    var itemIndex = 0; var gridItem; var RowID = "";
    while (gridItem = grdOT.get_table().getRow(itemIndex)) {
        RowID = gridItem.get_cells()[0].get_value();
        if (RowID == id) {
            gridItem.Data[4] = document.getElementById("ddlTZ_" + id.toString()).value;
            break;
        }
        itemIndex++;
    }
}
function btnMsg_OnClick(id) {
    strRowID = id;
    var itemIndex = 0; var gridItem; var RowID = "";
    while (gridItem = grdOT.get_table().getRow(itemIndex)) {
        RowID = gridItem.get_cells()[0].get_value();
        if (RowID == strRowID) {
            SVC_FLAG = "MSG";
            parent.GsPopupText = gridItem.Data[5];
            parent.GsLaunchURL = "Common/VRSNotes.aspx?hdr=Message To Display&hlp=Max. 500 characters&mc=500&th="+ selTheme;
            parent.PopupGeneralSmall();
            break;
        }
        itemIndex++;
    }

}
function btnSaveOT_OnClick() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrData = new Array();
    try {
        ArrRecords[0] = UserID;
        ArrRecords[1] = MenuID;
        ArrData = GetOperationTime();

        AjaxPro.timoutPeriod = 1800000;
        VRSConfig.SaveOperationTime(ArrRecords, ArrData, ShowProcess);

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetOperationTime() {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var arrRecords = new Array(); var idx = 0;

    while (gridItem = grdOT.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[0].toString();
        arrRecords[idx + 1] = gridItem.Data[2].toString();
        arrRecords[idx + 2] = gridItem.Data[3].toString();
        arrRecords[idx + 3] = gridItem.Data[4].toString();
        arrRecords[idx + 4] = gridItem.Data[5].toString();
        idx = idx + 5;
        itemIndex++;
    }
    return arrRecords;
}
function SaveOperationTime(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveOperationTime()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveOperationTime()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SaveOperationTime()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            break;
    }
}
/*******************Operation Hours*******************/

/*******************Service Available (Normal Hours)*******************/
function btnSaveSMA_OnClick() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrModData = new Array();
    var ArrSpeciesData = new Array();

    try {
        ArrRecords[0] = UserID;
        ArrRecords[1] = MenuID;
        ArrModData = GetServiceModalityAvailability();
        ArrSpeciesData = GetServiceSpeciesAvailability();

        AjaxPro.timoutPeriod = 1800000;
        VRSConfig.SaveServiceAvailability(ArrRecords, ArrModData,ArrSpeciesData, ShowProcess);

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetServiceModalityAvailability() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0;
    var arrRecords = new Array(); var idx = 0;

    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        if (gridItem.Data.length > 2) {
            ItemRecCount = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCount; i++) {
                arrRecords[idx] = gridItem.Data[2][i][1].toString();
                arrRecords[idx + 1] = gridItem.Data[2][i][2].toString();
                arrRecords[idx + 2] = gridItem.Data[2][i][4].toString();
                arrRecords[idx + 3] = gridItem.Data[2][i][5].toString();
                idx = idx + 4;
            }
        }

        itemIndex++;
    }
    return arrRecords;
}
function GetServiceSpeciesAvailability() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0;
    var arrRecords = new Array(); var idx = 0;

    while (gridItem = grdServiceSpc.get_table().getRow(itemIndex)) {
        if (gridItem.Data.length > 2) {
            ItemRecCount = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCount; i++) {
                arrRecords[idx] = gridItem.Data[2][i][1].toString();
                arrRecords[idx + 1] = gridItem.Data[2][i][2].toString();
                arrRecords[idx + 2] = gridItem.Data[2][i][4].toString();
                arrRecords[idx + 3] = gridItem.Data[2][i][5].toString();
                idx = idx + 4;
            }
        }

        itemIndex++;
    }
    return arrRecords;
}
function SaveServiceAvailability(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveServiceAvailability()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveServiceAvailability()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SaveServiceAvailability()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            break;
    }
}
function UpdateAvailability(service_id, ID) {
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var ModRowID = "0";
    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    ModRowID = gridItem.Data[2][i][0].toString();
                    if (ModRowID == ID) {
                        if (document.getElementById("chkSvcAvailable_" + ModRowID.toString()).checked)
                            gridItem.Data[2][i][4] = "Y";
                        else
                            gridItem.Data[2][i][4] = "N";
                        break;
                    }

                }
            }
        }

        itemIndex++;
    }
}
function btnSAMsg_OnClick(service_id, id) {
    strRowID = id;
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var ModRowID = "0";
    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.get_cells()[0].get_value();
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    ModRowID = gridItem.Data[2][i][0].toString();
                    if (ModRowID == id) {
                        strRowID = id;
                        SvcID = service_id;
                        SVC_FLAG = "MSGSVC";
                        parent.GsPopupText = gridItem.Data[2][i][5];
                        parent.GsLaunchURL = "Common/VRSNotes.aspx?hdr=Message To Display&hlp=Max. 500 characters&mc=500&th="+selTheme;
                        parent.PopupGeneralSmall();
                        break;
                    }
                }
            }
        }
        itemIndex++;
    }

}
function btnSAExInst_OnClick(service_id, modality_id, id) {
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var ModRowID = "0";
    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.get_cells()[0].get_value();
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    ModRowID = gridItem.Data[2][i][0].toString();
                    if (ModRowID == id) {
                        parent.GsStoredValue[0] = modality_id;
                        parent.GsStoredValue[1] = service_id;
                        parent.GsStoredValue[2] = gridItem.Data[2][i][4];//available
                        parent.GsStoredValue[3] = gridItem.Data[2][i][5];//display message
                        parent.GsStoredValue[4] = "N";
                        parent.GsStoredValue[5] = "MODALITY";
                        parent.GiWidth = 300;
                        parent.GiTop = 30;
                        parent.GsLaunchURL = "Settings/VRSExceptionInstitution.aspx?th=" + selTheme;
                        parent.PopupDataList();
                        break;
                    }
                }
            }
        }
        itemIndex++;
    }

}
function UpdateSpeciesAvailability(service_id, ID) {
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var SpeciesRowID = "0";
    while (gridItem = grdServiceSpc.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    SpeciesRowID = gridItem.Data[2][i][0].toString();
                    if (SpeciesRowID == ID) {
                        if (document.getElementById("chkSvcSpcAvailable_" + SpeciesRowID.toString()).checked)
                            gridItem.Data[2][i][4] = "Y";
                        else
                            gridItem.Data[2][i][4] = "N";
                        break;
                    }

                }
            }
            break;
        }

        itemIndex++;
    }
}
function btnSASPCMsg_OnClick(service_id, id) {
    strRowID = id;
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var SpeciesRowID = "0";
    while (gridItem = grdServiceSpc.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.get_cells()[0].get_value();
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    SpeciesRowID = gridItem.Data[2][i][0].toString();
                    if (SpeciesRowID == id) {
                        strRowID = id;
                        SvcID = service_id;
                        SVC_FLAG = "MSGSPCSVC";
                        parent.GsPopupText = gridItem.Data[2][i][5];
                        parent.GsLaunchURL = "Common/VRSNotes.aspx?hdr=Message To Display&hlp=Max. 500 characters&mc=500&th="+ selTheme;
                        parent.PopupGeneralSmall();
                        break;
                    }
                }
            }
        }
        itemIndex++;
    }

}
function btnSASPCExInst_OnClick(service_id, species_id, id) {
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var SpeciesRowID = "0";
    while (gridItem = grdServiceSpc.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.get_cells()[0].get_value();
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    SpeciesRowID = gridItem.Data[2][i][0].toString();
                    if (SpeciesRowID == id) {
                        parent.GsStoredValue[0] = species_id;
                        parent.GsStoredValue[1] = service_id;
                        parent.GsStoredValue[2] = gridItem.Data[2][i][4];//available
                        parent.GsStoredValue[3] = gridItem.Data[2][i][5];//display message
                        parent.GsStoredValue[4] = "N";
                        parent.GsStoredValue[5] = "SPECIES";
                        parent.GiWidth = 300;
                        parent.GiTop = 30;
                        parent.GsLaunchURL = "Settings/VRSExceptionInstitution.aspx?th=" + selTheme;
                        parent.PopupDataList();
                        break;
                    }
                }
            }
        }
        itemIndex++;
    }

}
/*******************Service Available (Normal Hours)*******************/

/*******************Service Available (After Hours)*******************/
function btnSaveSAAH_OnClick() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrModData = new Array();
    var ArrSpeciesData = new Array();

    try {
        ArrRecords[0]  = UserID;
        ArrRecords[1]  = MenuID;
        ArrModData     = GetServiceModalityAvailabilityAfterHours();
        ArrSpeciesData = GetServiceSpeciesAvailabilityAfterHours();

        AjaxPro.timoutPeriod = 1800000;
        VRSConfig.SaveServiceAvailabilityAfterHours(ArrRecords, ArrModData, ArrSpeciesData, ShowProcess);

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSaveSAAH_OnClick()", expErr.message, "true");
    }
}
function GetServiceModalityAvailabilityAfterHours() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0;
    var arrRecords = new Array(); var idx = 0;

    while (gridItem = grdSAAH.get_table().getRow(itemIndex)) {
        if (gridItem.Data.length > 2) {
            ItemRecCount = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCount; i++) {
                arrRecords[idx] = gridItem.Data[2][i][1].toString();
                arrRecords[idx + 1] = gridItem.Data[2][i][2].toString();
                arrRecords[idx + 2] = gridItem.Data[2][i][4].toString();
                arrRecords[idx + 3] = gridItem.Data[2][i][5].toString();
                idx = idx + 4;
            }
        }

        itemIndex++;
    }
    return arrRecords;
}
function GetServiceSpeciesAvailabilityAfterHours() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0;
    var arrRecords = new Array(); var idx = 0;

    while (gridItem = grdSASPAH.get_table().getRow(itemIndex)) {
        if (gridItem.Data.length > 2) {
            ItemRecCount = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCount; i++) {
                arrRecords[idx] = gridItem.Data[2][i][1].toString();
                arrRecords[idx + 1] = gridItem.Data[2][i][2].toString();
                arrRecords[idx + 2] = gridItem.Data[2][i][4].toString();
                arrRecords[idx + 3] = gridItem.Data[2][i][5].toString();
                idx = idx + 4;
            }
        }

        itemIndex++;
    }
    return arrRecords;
}
function SaveServiceAvailabilityAfterHours(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveServiceAvailabilityAfterHours()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveServiceAvailabilityAfterHours()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SaveServiceAvailabilityAfterHours()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            break;
    }
}
function UpdateAfterHourAvailability(service_id, ID) {
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var ModRowID = "0"; 
    while (gridItem = grdSAAH.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    ModRowID = gridItem.Data[2][i][0].toString();
                    if (ModRowID == ID) {
                        if (document.getElementById("chkSAAH_" + ModRowID.toString()).checked)
                            gridItem.Data[2][i][4] = "Y";
                        else
                            gridItem.Data[2][i][4] = "N";
                        break;
                    }

                }
            }
            break;
        }

        itemIndex++;
    }
}
function btnSAAHMsg_OnClick(service_id, id) {
    strRowID = id;
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var ModRowID = "0";
    while (gridItem = grdSAAH.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.get_cells()[0].get_value();
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    ModRowID = gridItem.Data[2][i][0].toString();
                    if (ModRowID == id) {
                        strRowID = id;
                        SvcID = service_id;
                        SVC_FLAG = "MSGSVCAH";
                        parent.GsPopupText = gridItem.Data[2][i][5];
                        parent.GsLaunchURL = "Common/VRSNotes.aspx?hdr=Message To Display&hlp=Max. 500 characters&mc=500&th="+selTheme;
                        parent.PopupGeneralSmall();
                        break;
                    }
                }
            }
        }
        itemIndex++;
    }

}
function btnSAExInstAH_OnClick(service_id, modality_id, id) {
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var ModRowID = "0";
    while (gridItem = grdSAAH.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.get_cells()[0].get_value();
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    ModRowID = gridItem.Data[2][i][0].toString();
                    if (ModRowID == id) {
                        parent.GsStoredValue[0] = modality_id;
                        parent.GsStoredValue[1] = service_id;
                        parent.GsStoredValue[2] = gridItem.Data[2][i][4];//available
                        parent.GsStoredValue[3] = gridItem.Data[2][i][5];//display message
                        parent.GsStoredValue[4] = "Y";
                        parent.GsStoredValue[5] = "MODALITY";
                        parent.GiWidth = 300;
                        parent.GiTop = 30;
                        parent.GsLaunchURL = "Settings/VRSExceptionInstitution.aspx?th=" + selTheme;
                        parent.PopupDataList();
                        break;
                    }
                }
            }
        }
        itemIndex++;
    }

}
function UpdateSpeciesAfterHourAvailability(service_id, ID) {
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var SpeciesRowID = "0";
    while (gridItem = grdSASPAH.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.Data[0].toString(); var CtrlCode = "";
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    SpeciesRowID = gridItem.Data[2][i][0].toString();
                    if (SpeciesRowID == ID) {
                        if (document.getElementById("chkSASPAH_" + SpeciesRowID.toString()).checked)
                            gridItem.Data[2][i][4] = "Y";
                        else
                            gridItem.Data[2][i][4] = "N";
                        break;
                    }

                }
            }
            break;
        }

        itemIndex++;
    }
}
function btnSASPAHMsg_OnClick(service_id, id) {
    strRowID = id;
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var SpeciesRowID = "0";
    while (gridItem = grdSASPAH.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.get_cells()[0].get_value();
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    SpeciesRowID = gridItem.Data[2][i][0].toString();
                    if (SpeciesRowID == id) {
                        strRowID = id;
                        SvcID = service_id;
                        SVC_FLAG = "MSGSVCAHSP";
                        parent.GsPopupText = gridItem.Data[2][i][5];
                        parent.GsLaunchURL = "Common/VRSNotes.aspx?hdr=Message To Display&hlp=Max. 500 characters&mc=500&th="+ selTheme;
                        parent.PopupGeneralSmall();
                        break;
                    }
                }
            }
        }
        itemIndex++;
    }

}
function btnSASPExInstAH_OnClick(service_id, species_id, id) {
    var itemIndex = 0; var gridItem; var ServiceID = "0";
    var ItemRecCount = 0; var SpeciesRowID = "0";
    while (gridItem = grdSASPAH.get_table().getRow(itemIndex)) {
        ServiceID = gridItem.get_cells()[0].get_value();
        if (ServiceID == service_id) {
            if (gridItem.Data.length > 2) {
                ItemRecCount = gridItem.Data[2].length;
                for (var i = 0; i < ItemRecCount; i++) {
                    SpeciesRowID = gridItem.Data[2][i][0].toString();
                    if (SpeciesRowID == id) {
                        parent.GsStoredValue[0] = species_id;
                        parent.GsStoredValue[1] = service_id;
                        parent.GsStoredValue[2] = gridItem.Data[2][i][4];//available
                        parent.GsStoredValue[3] = gridItem.Data[2][i][5];//display message
                        parent.GsStoredValue[4] = "Y";
                        parent.GsStoredValue[5] = "SPECIES";
                        parent.GiWidth = 300;
                        parent.GiTop = 30;
                        parent.GsLaunchURL = "Settings/VRSExceptionInstitution.aspx?th=" + selTheme;
                        parent.PopupDataList();
                        break;
                    }
                }
            }
        }
        itemIndex++;
    }

}
/*******************Service Available (After Hours)*******************/

function ProcessGeneralSmall(Args) {
    var itemIndex = 0; var gridItem; var RowID = "";
    var ItemRecCount = 0; var ModRowID = "0"; var SpeciesRowID = "0";
    if (Args != null) {
        switch (SVC_FLAG) {
            case "MSG":
                while (gridItem = grdOT.get_table().getRow(itemIndex)) {
                    RowID = gridItem.get_cells()[0].get_value();
                    if (RowID == strRowID) {
                        gridItem.Data[5] = Args;
                        break;
                    }
                    itemIndex++;
                }
                break;
            case "MSGSVC":
                while (gridItem = grdService.get_table().getRow(itemIndex)) {
                    ServiceID = gridItem.get_cells()[0].get_value();
                    if (ServiceID == SvcID) {
                        if (gridItem.Data.length > 2) {
                            ItemRecCount = gridItem.Data[2].length;
                            for (var i = 0; i < ItemRecCount; i++) {
                                ModRowID = gridItem.Data[2][i][0].toString();
                                if (ModRowID == strRowID) {
                                    gridItem.Data[2][i][5] = Args;
                                    break;
                                }
                            }
                        }
                    }
                    itemIndex++;
                }
                break;
            case "MSGSPCSVC":
                while (gridItem = grdServiceSpc.get_table().getRow(itemIndex)) {
                    ServiceID = gridItem.get_cells()[0].get_value();
                    if (ServiceID == SvcID) {
                        if (gridItem.Data.length > 2) {
                            ItemRecCount = gridItem.Data[2].length;
                            for (var i = 0; i < ItemRecCount; i++) {
                                SpeciesRowID = gridItem.Data[2][i][0].toString();
                                if (SpeciesRowID == strRowID) {
                                    gridItem.Data[2][i][5] = Args;
                                    break;
                                }
                            }
                        }
                    }
                    itemIndex++;
                }
                break;
            case "MSGSVCAH":
                while (gridItem = grdSAAH.get_table().getRow(itemIndex)) {
                    ServiceID = gridItem.get_cells()[0].get_value();
                    if (ServiceID == SvcID) {
                        if (gridItem.Data.length > 2) {
                            ItemRecCount = gridItem.Data[2].length;
                            for (var i = 0; i < ItemRecCount; i++) {
                                ModRowID = gridItem.Data[2][i][0].toString();
                                if (ModRowID == strRowID) {
                                    gridItem.Data[2][i][5] = Args;
                                    break;
                                }
                            }
                        }
                    }
                    itemIndex++;
                }
                break;
            case "MSGSVCAHSP":
                while (gridItem = grdSASPAH.get_table().getRow(itemIndex)) {
                    ServiceID = gridItem.get_cells()[0].get_value();
                    if (ServiceID == SvcID) {
                        if (gridItem.Data.length > 2) {
                            ItemRecCount = gridItem.Data[2].length;
                            for (var i = 0; i < ItemRecCount; i++) {
                                SpeciesRowID = gridItem.Data[2][i][0].toString();
                                if (SpeciesRowID == strRowID) {
                                    gridItem.Data[2][i][5] = Args;
                                    break;
                                }
                            }
                        }
                    }
                    itemIndex++;
                }
                break;

        }

    }

}
function ResetValueInteger(objCtrlID, strDefaultValue) {
    var objCtrl;
    objCtrl = document.getElementById(objCtrlID.id);
    if (objCtrl == null) objCtrl = objCtrlID;
    if (strDefaultValue != null) {
        if (objCtrl.value == "" || parseInt(objCtrl.value) <= 0) {
            objCtrl.value = strDefaultValue;
        }
    }
    else {
        if (objCtrl.value == "") objCtrl.value = "0";
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
        case "SaveOperationTime":
            SaveOperationTime(Result);
            break;
        case "SaveServiceAvailability":
            SaveServiceAvailability(Result);
            break;
        case "SaveServiceAvailabilityAfterHours":
            SaveServiceAvailabilityAfterHours(Result);
            break;
        case "SaveDashboardRecord":
            SaveDashboardRecord(Result);
            break;
    }
}345