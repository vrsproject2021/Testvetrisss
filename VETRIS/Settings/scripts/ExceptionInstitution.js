parent.adjusDataListFrameHeight();
function SetPageValue() {
    objhdnRecordID.value = parent.GsStoredValue[0];
    objhdnSvcID.value = parent.GsStoredValue[1];
    objhdnAvbl.value = parent.GsStoredValue[2];
    objhdnDispMsg.value = parent.GsStoredValue[3];
    objhdnAfterHrs.value = parent.GsStoredValue[4];
    objhdnFilter.value = parent.GsStoredValue[5];

    if (parent.GsStoredValue[4] == "Y") {
        if (document.all) document.getElementById("spnAfterHrs").innerHTML = "(After Hours)"
        else document.getElementById("spnAfterHrs").textContent = "(After Hours)"
    }
    else if (parent.GsStoredValue[4] == "N") {
        if (document.all) document.getElementById("spnAfterHrs").innerHTML = "(Normal Hours)"
        else document.getElementById("spnAfterHrs").textContent = "(Normal Hours)"
    }
    LoadInstitutions();
}

function LoadInstitutions() {
    var arrParams = new Array();
    arrParams[0] = objhdnFilter.value;
    arrParams[1] = objhdnRecordID.value;
    arrParams[2] = objhdnSvcID.value;
    arrParams[3] = objhdnAfterHrs.value;
    arrParams[4] = parent.objhdnMenuID.value;
    arrParams[5] = parent.objhdnUserID.value;
    CallBackInst.callback(arrParams);
}

function chkSel_OnClick(ID) {
    var itemIndex = 0; var gridItem; var RowID = "0";
    var ItemRecCount = 0;
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowID = gridItem.Data[0].toString();
        if (ID == RowID) {
            if (document.getElementById("chkSel_" + RowID).checked) gridItem.Data[3] = "Y";
            else gridItem.Data[3] = "N";

            break;
        }

        itemIndex++;
    }
}

function btnClose_OnClick() {
    parent.HideDataList();
}

function btnOk_OnClick() {
    PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrData = new Array();
    try {
        ArrRecords[0] = objhdnSvcID.value;
        ArrRecords[1] = objhdnRecordID.value;
        ArrRecords[2] = objhdnAfterHrs.value;
        ArrRecords[3] = objhdnAvbl.value;
        ArrRecords[4] = objhdnDispMsg.value;
        ArrRecords[5] = objhdnFilter.value;
        ArrRecords[6] = parent.objhdnUserID.value;
        ArrRecords[7] = parent.objhdnMenuID.value;
        ArrData = GetInstitutions();

        
        AjaxPro.timoutPeriod = 1800000;
        VRSExceptionInstitution.SaveRecord(ArrRecords, ArrData, ShowProcess);
      

    }
    catch (expErr) {
        HideProcess();
        objlblMsg.style.color = "red";
        if (document.all) objlblMsg.innerHTML = expErr.message; else objlblMsg.textContent = expErr.message;
        document.getElementById("divMsg").style.display = "block";
    }

}
function GetInstitutions() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrRecords = new Array(); var idx = 0;

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        if (gridItem.Data[3] == "Y") {
            arrRecords[idx] = gridItem.Data[0];
            idx = idx + 1;
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
        case "false":
            objlblMsg.innerHTML = arrRes[1];
            break;
        case "true":
            parent.HideDataList();
            break;
    }
}
function ShowProcess(Result, MethodName) {
    HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
    }
}
function PopupProcess(IsSaving) {
    if (IsSaving == null) IsSaving = "N";
    var sUrl = "";
    if (IsSaving == "N") sUrl = "../htmls/processing.html";
    else sUrl = "../htmls/saving.html";

    $('#tblProcess').surfOverlay('ps', { url: sUrl, zIndex: 3000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });

    return false;
}
function HideProcess() {

    //$('#tblProcess').surfOverlay('ps', { zIndex: 100 });
    closepopup('ps');
}
