var SecDivider = parent.objhdnSecDivider.value;

var DEL_FLAG = ""; var strRowID = "0"; var objItem; var VALIDATED = "N";

function btnRegister_OnClick() {
   
    var ArrRecords  = new Array();
    var ArrDevice   = new Array();
    var ArrPhys     = new Array();
    var ArrUser     = new Array();
    var ArrFees     = new Array();

    try {
        ArrRecords[0]   = objhdnID.value;
        ArrRecords[1]   = "";//--code
        ArrRecords[2]   = objtxtName.value;
        ArrRecords[3]   = "N";//--Status(Active?)
        ArrRecords[4]   = objtxtAddr1.value;
        ArrRecords[5]   = "";//--Addr2
        ArrRecords[6]   = objtxtCity.value;
        ArrRecords[7]   = objddlCountry.value;
        ArrRecords[8]   = objddlState.value;
        ArrRecords[9]   = objtxtZip.value;
        ArrRecords[10]  = objtxtEmailID.value;
        ArrRecords[11]  = objtxtTel.value;
        ArrRecords[12]  = objtxtMobile.value;
        ArrRecords[13]  = objtxtContPerson.value;
        ArrRecords[14]  = objtxtContMobile.value;
        ArrRecords[15]  = "00000000-0000-0000-0000-000000000000";//UserID;
        ArrRecords[16]  = "2";// MenuID;
        ArrRecords[17]  = "";//objhdnUsrUpdUrl.value;
        ArrRecords[18]  = objtxtLoginId.value;
        ArrRecords[19]  = objtxtPassword.value;
        ArrRecords[20]  = objtxtLoginEmailId.value;
        ArrPhys         = GetPhysicians();

        AjaxPro.timoutPeriod = 1800000;
        VRSInstitutionRegistration.SaveRecord(ArrRecords, ArrPhys,  ShowProcess);
       
    }
    catch (expErr) {
        alert("error");
        //parent.HideProcess();
        //parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function ddlCountry_OnChange() {
    //parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = objddlCountry.value;
        AjaxPro.timeoutPeriod = 1800000;
        VRSInstitutionRegistration.FetchStates(ArrRecords, ShowProcess);
        
    }
    catch (expErr) {
        alert("...?");
        //parent.HideProcess();
        //parent.PopupMessage(RootDirectory, strForm, "ddlCountry_OnChange()", expErr.message, "true");
    }
}

function PopulateStateList(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            //parent.PopupMessage(RootDirectory, strForm, "PopulateStateList()", arrRes[1], "true");
            break;
        case "true":
            objddlState.length = 0;
            for (var i = 1; i < arrRes.length; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlState.add(op);
            }
            break;
    }

}


function GetPhysicians() {
    var itemIndex = 0;
    var gridItem;
    var arrRecords = new Array(); var idx = 0;
    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        arrRecords[idx] = gridItem.Data[1].toString();
        arrRecords[idx + 1] = gridItem.Data[2].toString();
        arrRecords[idx + 2] = gridItem.Data[3].toString();
        arrRecords[idx + 3] = gridItem.Data[4].toString();
        arrRecords[idx + 4] = gridItem.Data[5].toString();
        arrRecords[idx + 5] = gridItem.Data[6].toString();
        idx = idx + 6;
        itemIndex++;
    }
    return arrRecords;
}
/**************PHYSICIANS***************/
function btnAddPhys_OnClick() {
    var strDtls = "";
    PHYSADD = "Y";
    strDtls = GetPhysicianGridDetails();
    CallBackPhys.callback("A", strDtls);
}
function txtFname_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[2] = document.getElementById("txtFname_" + RowId).value;
            //objItem = gridItem;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtLname_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtLname_" + RowId).value;
            //objItem = gridItem;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtCred_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[4] = document.getElementById("txtCred_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtEmail_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();

        if (RowId == ID) {
            gridItem.Data[5] = document.getElementById("txtEmail_" + RowId).value;

        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function txtMobile_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var arrParams = new Array();

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        strRowID = ID;

        if (RowId == ID) {
            gridItem.Data[6] = document.getElementById("txtMobile_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}
function DeletePhysicianRow(ID) {
    strRowID = ID;
    DEL_FLAG = "PHYSICIAN";
    //GsDlgConfAction = "DEL";
    //PopupConfirm("032");

    strDtls = GetPhysicianGridDetails();
    CallBackPhys.callback("D", strRowID, strDtls);
}
function GetPhysicianGridDetails() {
    var strDtls = "";
    var itemIndex = 0;
    var gridItem;

    while (gridItem = grdPhys.get_table().getRow(itemIndex)) {
        if (strDtls != "") strDtls = strDtls + SecDivider;
        strDtls += gridItem.Data[0].toString() + SecDivider;
        strDtls += gridItem.Data[1].toString() + SecDivider;
        strDtls += gridItem.Data[2].toString() + SecDivider;
        strDtls += gridItem.Data[3].toString() + SecDivider;
        strDtls += gridItem.Data[4].toString() + SecDivider;
        strDtls += gridItem.Data[5].toString() + SecDivider;
        strDtls += gridItem.Data[6].toString();
        itemIndex++;
    }
    return strDtls;
}
/**************PHYSICIANS***************/



function DeleteRecord() {
    var strDtls = "";
    switch (DEL_FLAG) {
        case "DEVICE":
            strDtls = GetDeviceGridDetails();
            CallBackDevice.callback("D", strRowID, strDtls);
            break;
        case "PHYSICIAN":
            strDtls = GetPhysicianGridDetails();
            CallBackPhys.callback("D", strRowID, strDtls);
            break;
        
    }

}

function ShowProcess(Result, MethodName) {
    //parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "FetchStates":
            PopulateStateList(Result);
            break;
        case "FetchPhysicianDetails":
            PopulatePhysicianDetails(Result);
            break;
        case "FetchUserDetails":
            PopulateUserDetails(Result);
            break;
        case "SaveRecord":
            SaveRecord(Result);
            break;

    }
}

function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            //objhdnID.value = arrRes[2];
            //objtxtCode.value = arrRes[3];
            CallBackPhys.callback("L", objhdnID.value);
            VALIDATED = "N";
            parent.GsRetStatus = "false";
            break;
    }
}

/**************POPUPS***************/
function PopupConfirm(argErrCode, argTxt1, argTxt2, argAction) {
    var sUrl = "../htmls/confirm.html";
    if (argTxt1 == null) argTxt1 = "";
    if (argTxt2 == null) argTxt2 = "";
    if (argAction != null) GsConfirmAction = argAction;
    GsLaunchURL = "../Common/VRSConfirm.aspx?ERRCODE=" + argErrCode + "&TEXT1=" + argTxt1 + "&TEXT2=" + argTxt2;
    $('#tblConf').surfOverlay('cfm', { url: sUrl, zIndex: 3000, imgLoading: false, center: true, bgClickToClose: false, closeOnEsc: false });
    return false;
}

function PopupMessage(argRootDirectory, argForm, argMethod, argErrCode, argShowErr, argsText1, argsText2, argsRet) {
    if (argsText1 == null) argsText1 = ""; if (argsText2 == null) argsText2 = "";
    if (argsRet == null) argsRet = ""; MsgFORM = argForm;
    GsLaunchURL = "../Common/VRSMessages.aspx?FORM=" + argForm + "&METHOD=" + argMethod + "&ERRCODE=" + argErrCode + "&ShowErr=" + argShowErr + "&TEXT1=" + argsText1 + "&TEXT2=" + argsText2 + "&RETVAL=" + argsRet;
    var sUrl = "../htmls/message.html";
    $('#tblMsg').surfOverlay('msg', { url: sUrl, zIndex: 4000, imgLoading: false, bgClickToClose: false, closeOnEsc: false });
    return false;
}
function HideMessage(ArgsRet) {
    $('#tblMsg').surfOverlay('msg', { zIndex: 100 });
    closepopup('msg');

    GsLaunchURL = "";
    GsText = "";

    if (MsgFORM == "VRSChangePwd") {
        if (GsLogout == "Y") {
            if (document.all)
                location.href = objhdnServerPath.value + "/VRSLogout.aspx?uid=" + objhdnUserID.value;
            else
                window.location.assign(objhdnServerPath.value + "/VRSLogout.aspx?uid=" + objhdnUserID.value);
        }
    }
    else if (ArgsRet != null) {
        if (typeof (objiframePage) != "undefined") {
            if (objiframePage.contentWindow)
                objiframePage.contentWindow.ProcessMessage(ArgsRet);
            else
                objiframePage.contentDocument.parentWindow.ProcessMessage(ArgsRet);
        }
    }
}

/**************POPUPS***************/
function CheckInteger(e) {
    try {
        var Num; var keyChar; var NumCheck;
        if (window.event) Num = e.keyCode;
        else if (e.which) Num = e.which;
        else if ((e.which) == undefined) Num = e.keyCode;
        keyChar = String.fromCharCode(Num);
        NumCheck = /\d/;
        return NumCheck.test(keyChar);
    }
    catch (err) {
        if (err.description == "undefined") return true;
        return false;
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
