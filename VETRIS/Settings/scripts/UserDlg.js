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
    DisableCodeUI();
}
function SetPageValue() {
    CallBackRights.callback('U', objhdnID.value);
}
function btnNew_OnClick() {
    if (parent.GsRetStatus == "false") {
        if (objhdnCF.value == "") btnBrwAddUI_Onclick('Settings/VRSUserDlg.aspx');
        else btnBrwAddUI_Onclick('Settings/VRSUserDlg.aspx?cf=' + objhdnCF.value);
    }
    else {
        parent.GsDlgConfAction = "NEWUI";
        if (objhdnCF.value == "") parent.GsNavURL = "Settings/VRSUserDlg.aspx";
        else parent.GsNavURL = 'Settings/VRSUserDlg.aspx?cf=' + objhdnCF.value;
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        if (objhdnCF.value == "") btnBrwEditUI_Onclick('Settings/VRSUserDlg.aspx');
        else btnBrwEditUI_Onclick('Settings/VRSUserDlg.aspx?cf=' + objhdnCF.value);
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        if (objhdnCF.value == "") parent.GsNavURL = "Settings/VRSUserDlg.aspx";
        else parent.GsNavURL = 'Settings/VRSUserDlg.aspx?cf=' + objhdnCF.value;
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    if (objhdnCF.value == "") parent.GsNavURL = "Settings/VRSUserBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    else if (objhdnCF.value == "MQ") parent.GsNavURL = "Masters/VRSMasterQuery.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=71";

    if (parent.GsRetStatus == "false") {
        if (objhdnCF.value == "MQ") parent.objhdnMenuID.value = "71";
        btnDlgClose_Onclick();
    }
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrMenuList = new Array();

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objtxtCode.value;
        ArrRecords[2] = objtxtName.value;
        ArrRecords[3] = "Y"; if (objrdoStatNo.checked) ArrRecords[3] = "N";
        ArrRecords[4] = objddlRole.value;
        ArrRecords[5] = objtxtEmailID.value;
        ArrRecords[6] = objtxtContactNo.value;
        ArrRecords[7] = objtxtLoginID.value;
        ArrRecords[8] = objtxtPwd.value;
        ArrRecords[9] = objtxtPACSUserID.value;
        ArrRecords[10] = objtxtPACSPwd.value;
        ArrRecords[11] = "N"; if (objrdoMSYes.checked) ArrRecords[11] = "Y";
        ArrRecords[12] = "N"; if (objrdoDBYes.checked) ArrRecords[12] = "Y";
        ArrRecords[13] = UserID;
        ArrRecords[14] = MenuID;
        ArrRecords[15] = SessionID;

        arrMenuList = GetMenuList();


        AjaxPro.timoutPeriod = 1800000;
        VRSUserDlg.SaveRecord(ArrRecords, arrMenuList, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetMenuList() {
    var arrMenu = new Array();
    var idx = 0;
    var strType = "";
    try {
        //Level 0 Menu
        for (var rCount = 0; rCount < tvRights.Nodes(0).Nodes().length; rCount++) {
            if (tvRights.Nodes(0).Nodes(rCount).Checked) {
                arrMenu[idx] = tvRights.Nodes(0).Nodes(rCount).get_id();
                idx = idx + 1;
                if (tvRights.Nodes(0).Nodes(rCount).Nodes().length > 0) {
                    for (var rCount0 = 0; rCount0 < tvRights.Nodes(0).Nodes(rCount).Nodes().length; rCount0++) {
                        if (tvRights.Nodes(0).Nodes(rCount).Nodes(rCount0).Checked) {
                            arrMenu[idx] = tvRights.Nodes(0).Nodes(rCount).Nodes(rCount0).get_id();
                            idx = idx + 1;
                            //Level 1 Menu
                            //for (var rCount1 = 0; rCount1 < tvRights.Nodes(rCount).Nodes(rCount0).Nodes().length; rCount1++) {
                            //    if (tvRights.Nodes(0).Nodes(rCount).Nodes(rCount0).Nodes(rCount1).Checked) {

                            //        arrMenu[idx] = tvRights.Nodes(0).Nodes(rCount).Nodes(rCount0).Nodes(rCount1).get_id();
                            //        idx = idx + 1;
                            //    }
                            //}
                        }
                    }
                }
                
            }
        }
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "GetMenuList()", expErr.message, "true");
    }

    return arrMenu;
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
            parent.GsRetStatus = "false";
            DisableCodeUI(); SetPageValue();
            break;
    }
}
function ddlRole_OnChange() {
    CallBackRights.callback('UR', objddlRole.value);
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