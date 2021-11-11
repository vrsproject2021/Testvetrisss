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
    CallBackUserRole.callback();
    CallBackRights.callback(objhdnID.value, MenuID,UserID);
    CallBackAssign.callback(objhdnID.value, MenuID, UserID);
}
function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrMenuList = new Array(); 
    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = UserID;
        ArrRecords[2] = MenuID;

        arrMenuList = GetMenuList();
        

        AjaxPro.timeoutPeriod = 1800000;
        VRSAccessRights.SaveRecord(ArrRecords, arrMenuList, ShowProcess);
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
        for (var rCount = 0; rCount < tvRights.Nodes().length; rCount++) {
            if (tvRights.Nodes(rCount).Checked) {

                for (var rCount0 = 0; rCount0 < tvRights.Nodes(rCount).Nodes().length; rCount0++) {
                    if (tvRights.Nodes(rCount).Nodes(rCount0).Checked) {
                        arrMenu[idx] = tvRights.Nodes(rCount).Nodes(rCount0).get_id();
                        idx = idx + 1;
                        //Level 1 Menu
                        for (var rCount1 = 0; rCount1 < tvRights.Nodes(rCount).Nodes(rCount0).Nodes().length; rCount1++) {
                            if (tvRights.Nodes(rCount).Nodes(rCount0).Nodes(rCount1).Checked) {

                                arrMenu[idx] = tvRights.Nodes(rCount).Nodes(rCount0).Nodes(rCount1).get_id();
                                idx = idx + 1;
                            }
                        }
                    }
                }
            }
        }
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
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
            parent.GsRetStatus = "false";
            CallBackRights.callback(objhdnID.value, MenuID, UserID);
            CallBackAssign.callback(objhdnID.value, MenuID, UserID);
            break;
    }
}
function RemoveRights(strMenuID) {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = "0";
        ArrRecords[2] = strMenuID;
        ArrRecords[3] = UserID;
        ArrRecords[4] = MenuID;

        AjaxPro.timeoutPeriod = 1800000;
        VRSAccessRights.DeleteRecord(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "RemoveRights()", expErr.message, "true");
    }
}
function DeleteRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "DeleteRecord()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "DeleteRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "DeleteRecord()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            CallBackRights.callback(objhdnID.value, MenuID, UserID);
            CallBackAssign.callback(objhdnID.value, MenuID, UserID);
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
        case "DeleteRecord":
            DeleteRecord(Result);
            break;
    }
}
function btnReset_OnClick() {
    strValidate = "N";
    if (parent.GsRetStatus == "false")
        btnBrwEdit_Onclick('Settings/VRSAccessRights.aspx');
    else {
        parent.GsDlgConfAction = "RES";
        parent.GsNavURL = "Settings/VRSAccessRights.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=0";
        parent.PopupConfirm("030");
    }

}
