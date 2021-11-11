function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var configList = "0"; var DataType = ""; var DataValStr = ""; var DataValNo = "";
    var ControlCode = ""; var UICtrl = ""; var UICtrlList = ""; var IsPwd = "";
  

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
       
        configList = gridItem.Data[2];
        for (var i = 0; i < configList.length; i = i + 1) {
            ControlCode = configList[i][0];
            DataType = configList[i][3].toString();
            DataValStr = configList[i][4].toString();
            DataValNo = configList[i][5].toString();
            IsPwd = configList[i][7].toString();
            UICtrl = configList[i][8].toString();
            UICtrlList = configList[i][9].toString();

            switch (DataType) {
                case "S":
                    if (document.getElementById("txtDataTypeStr_" + ControlCode) != null) {
                        document.getElementById("txtDataTypeNumber_" + ControlCode).readOnly = "readOnly"; document.getElementById("txtDataTypeNumber_" + ControlCode).className = "GridTextBoxReadOnly";
                        document.getElementById("txtDataTypeDecimal_" + ControlCode).readOnly = "readOnly"; document.getElementById("txtDataTypeDecimal_" + ControlCode).className = "GridTextBoxReadOnly";
                        ResetValueDecimal(document.getElementById("txtDataTypeDecimal_" + ControlCode));

                        if (IsPwd == "Y") {
                            document.getElementById("txtDataTypeStr_" + ControlCode).setAttribute('type', 'password');
                        }
                        else if (UICtrl == "ddl") {
                            document.getElementById("txtDataTypeStr_" + ControlCode).style.display = "none";
                            var arrList = UICtrlList.split("»");
                            var arrOpt = new Array();

                            for (var j = 0; j < arrList.length; j++) {
                                arrOpt = arrList[j].split("±");
                                var op = document.createElement("option");
                                op.value = arrOpt[0];
                                op.text = arrOpt[1];
                                document.getElementById("ddlDataTypeStr_" + ControlCode).add(op);
                            }
                            document.getElementById("ddlDataTypeStr_" + ControlCode).style.display = "inline";
                            document.getElementById("ddlDataTypeStr_" + ControlCode).value = DataValStr;
                        }
                    }
                    break;
                case "I":
                    if (document.getElementById("txtDataTypeNumber_" + ControlCode) != null) {
                        document.getElementById("txtDataTypeStr_" + ControlCode).readOnly = "readOnly"; document.getElementById("txtDataTypeStr_" + ControlCode).className = "GridTextBoxReadOnly";
                        document.getElementById("txtDataTypeDecimal_" + ControlCode).readOnly = "readOnly"; document.getElementById("txtDataTypeDecimal_" + ControlCode).className = "GridTextBoxReadOnly";
                        ResetValueDecimal(document.getElementById("txtDataTypeDecimal_" + ControlCode));
                        if (UICtrl == "ddl") {
                            document.getElementById("txtDataTypeNumber_" + ControlCode).style.display = "none";
                            var arrList = UICtrlList.split("»");
                            var arrOpt = new Array();

                            for (var j = 0; j < arrList.length; j++) {
                                arrOpt = arrList[j].split("±");
                                var op = document.createElement("option");
                                op.value = arrOpt[0];
                                op.text = arrOpt[1];
                                document.getElementById("ddlDataTypeNumber_" + ControlCode).add(op);
                            }
                            document.getElementById("ddlDataTypeNumber_" + ControlCode).style.display = "inline";
                            document.getElementById("ddlDataTypeNumber_" + ControlCode).value = DataValNo;
                        }
                    }
                    break;
                case "D":
                    if (document.getElementById("txtDataTypeDecimal_" + ControlCode) != null) {
                        document.getElementById("txtDataTypeStr_" + ControlCode).readOnly = "readOnly"; document.getElementById("txtDataTypeStr_" + ControlCode).className = "GridTextBoxReadOnly";
                        document.getElementById("txtDataTypeNumber_" + ControlCode).readOnly = "readOnly"; document.getElementById("txtDataTypeNumber_" + ControlCode).className = "GridTextBoxReadOnly";
                        ResetValueDecimal(document.getElementById("txtDataTypeDecimal_" + ControlCode));
                    }
                    break;
            }

            //if (configList[i][4] == '') {
            //    //var cellId = "txtDataTypeStr_" + configList[i][0] + '_' + configList[i][1];
            //    //$('#' + cellId).css('display', 'none');
            //}
            //if (parseFloat(configList[i][5]) == 0) {
            //    //var cellId = "txtDataTypeNumber_" + configList[i][0] + '_' + configList[i][1];
            //    //$('#' + cellId).css('display', 'none');
            //}
            //if (parseFloat(configList[i][6]) == 0) {
            //    //var cellId = "txtDataTypeDecimal_" + configList[i][0] + '_' + configList[i][1];
            //    //$('#' + cellId).css('display', 'none');
            //}
        }
        itemIndex++;
    }
    parent.GsRetStatus = "false"; 
    //SetGridValues();
}
function grdBrw_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdBrw_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}

function grdOT_onCallbackComplete(sender, eventArgs) {
    grdOT.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBOTErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdOT_onCallbackComplete()", strErr, "true");
    }
}
function grdOT_onRenderComplete(sender, eventArgs) {
    grdOT.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrTZ = new Array(); var TzID = "0";

    if (parent.Trim(objhdnTZ.value) != "") {
        if (parent.Trim(objhdnTZ.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrTZ = parent.Trim(objhdnTZ.value).split(parent.objhdnDivider.value);
        }
    }

    while (gridItem = grdOT.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        TzID = gridItem.Data[4].toString();

        if (document.getElementById("ddlTZ_" + RowId) != null) {
            if (document.getElementById("ddlTZ_" + RowId).length == 0) {
                for (var i = 0; i < arrTZ.length; i = i + 4) {
                    var op = document.createElement("option");
                    op.value = arrTZ[i];
                    op.text = arrTZ[i + 1];
                    document.getElementById("ddlTZ_" + RowId).add(op);
                }
            }
            document.getElementById("ddlTZ_" + RowId).value = TzID;
        }

        
        itemIndex++;
    }
    parent.GsRetStatus = "false";
}

function grdDASH_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdDASH_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdDASH_onCallbackComplete(sender, eventArgs) {
    grdDASH.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBODASHErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdDASH_onCallbackComplete()", strErr, "true");
    }
}
function grdDASH_onRenderComplete(sender, eventArgs) {
    grdDASH.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var active = 'N'; var isDefault = 'N'; var showRefresh = 'N'; var dashboardList = "0";
    var childId = "0";
    while (gridItem = grdDASH.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        dashboardList = gridItem.Data[2];
        for (var i = 0; i < dashboardList.length; i = i + 1) {
            childId = dashboardList[i][0].toString();
            active = dashboardList[i][7].toString();
            isDefault = dashboardList[i][8].toString();
            showRefresh = dashboardList[i][9].toString();
            if (active == "Y") {
                if (document.getElementById("chkIsEnabled_" + childId) != null) {
                    document.getElementById("chkIsEnabled_" + childId).checked = true;
                }
            }
            if (isDefault == "Y") {
                if (document.getElementById("chkIsDefault_" + childId) != null) {
                    document.getElementById("chkIsDefault_" + childId).checked = true;
                }
            }
            if (showRefresh == "Y") {
                if (document.getElementById("chkIsRefreshButton_" + childId) != null) {
                    document.getElementById("chkIsRefreshButton_" + childId).checked = true;
                }
            }
        }
        
       
        itemIndex++;
    }
    parent.GsRetStatus = "false";
}

function grdService_onCallbackComplete(sender, eventArgs) {
    grdService.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnError").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdService_onCallbackComplete()", strErr, "true");
    }
}
function grdService_onRenderComplete(sender, eventArgs) {
    grdService.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0; var ModRowID = "0"; var available = "";


    while (gridItem = grdService.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (gridItem.Data.length > 2) {
            ItemRecCount = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCount; i++) {
                ModRowID = gridItem.Data[2][i][0].toString();
                available = gridItem.Data[2][i][4].toString();
                if (available == "Y") {
                    if (document.getElementById("chkSvcAvailable_" + ModRowID.toString()) != null) {
                        document.getElementById("chkSvcAvailable_" + ModRowID.toString()).checked = true;
                    }
                }
            }
        }
       
        
        itemIndex++;
    }
    parent.GsRetStatus = "false";
}
function grdService_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdService_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}

function grdServiceSpc_onCallbackComplete(sender, eventArgs) {
    grdServiceSpc.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBSASpcErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdServiceSpc_onCallbackComplete()", strErr, "true");
    }
}
function grdServiceSpc_onRenderComplete(sender, eventArgs) {
    grdServiceSpc.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0; var SpeciesRowID = "0"; var available = "";


    while (gridItem = grdServiceSpc.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (gridItem.Data.length > 2) {
            ItemRecCount = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCount; i++) {
                SpeciesRowID = gridItem.Data[2][i][0].toString();
                available = gridItem.Data[2][i][4].toString();
                if (available == "Y") {
                    if (document.getElementById("chkSvcSpcAvailable_" + SpeciesRowID.toString()) != null) {
                        document.getElementById("chkSvcSpcAvailable_" + SpeciesRowID.toString()).checked = true;
                    }
                }
            }
        }


        itemIndex++;
    }
    parent.GsRetStatus = "false";
}
function grdServiceSpc_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdServiceSpc_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}

function grdSAAH_onCallbackComplete(sender, eventArgs) {
    grdSAAH.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnError").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSAAH_onCallbackComplete()", strErr, "true");
    }
}
function grdSAAH_onRenderComplete(sender, eventArgs) {
    grdSAAH.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0; var ModRowID = "0"; var available = "";


    while (gridItem = grdSAAH.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (gridItem.Data.length > 2) {
            ItemRecCount = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCount; i++) {
                ModRowID = gridItem.Data[2][i][0].toString();
                available = gridItem.Data[2][i][4].toString();
                if (available == "Y") {
                    if (document.getElementById("chkSAAH_" + ModRowID.toString()) != null) {
                        document.getElementById("chkSAAH_" + ModRowID.toString()).checked = true;
                    }
                }
            }
        }


        itemIndex++;
    }
    parent.GsRetStatus = "false";
}
function grdSAAH_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdSAAH_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}

function grdSASPAH_onCallbackComplete(sender, eventArgs) {
    grdSASPAH.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBSASPErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSASPAH_onCallbackComplete()", strErr, "true");
    }
}
function grdSASPAH_onRenderComplete(sender, eventArgs) {
    grdSASPAH.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    var ItemRecCount = 0; var SpeciesRowID = "0"; var available = "";


    while (gridItem = grdSASPAH.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        if (gridItem.Data.length > 2) {
            ItemRecCount = gridItem.Data[2].length;
            for (var i = 0; i < ItemRecCount; i++) {
                SpeciesRowID = gridItem.Data[2][i][0].toString();
                available = gridItem.Data[2][i][4].toString();
                if (available == "Y") {
                    if (document.getElementById("chkSASPAH_" + SpeciesRowID.toString()) != null) {
                        document.getElementById("chkSASPAH_" + SpeciesRowID.toString()).checked = true;
                    }
                }
            }
        }


        itemIndex++;
    }
    parent.GsRetStatus = "false";
}
function grdSASPAH_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdSASPAH_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}


function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

}