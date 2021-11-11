

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
}
function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objtxtBACode.value = parent.GsFilter[0];
        objtxtBAName.value = parent.GsFilter[1];
        objddlBAStatus.value = parent.GsFilter[2];
        objtxtInstCode.value = parent.GsFilter[3];
        objtxtInstName.value = parent.GsFilter[4];
        objddlInstStatus.value = parent.GsFilter[5];
        objtxtLoginID.value = parent.GsFilter[6];
        objddlUserStatus.value = parent.GsFilter[7];
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objtxtBACode.value;
    ArrRecords[1] = objtxtBAName.value;
    ArrRecords[2] = objddlBAStatus.value;
    ArrRecords[3] = objtxtInstCode.value;
    ArrRecords[4] = objtxtInstName.value;
    ArrRecords[5] = objddlInstStatus.value;
    ArrRecords[6] = objtxtLoginID.value;
    ArrRecords[7] = objddlUserStatus.value;
    ArrRecords[8] = UserID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtBACode.value);
    parent.GsFilter[1] = parent.Trim(objtxtBAName.value);
    parent.GsFilter[2] = objddlBAStatus.value;
    parent.GsFilter[3] = parent.Trim(objtxtInstCode.value);
    parent.GsFilter[4] = parent.Trim(objtxtInstName.value);
    parent.GsFilter[5] = objddlInstStatus.value;
    parent.GsFilter[6] = parent.Trim(objtxtLoginID.value);
    parent.GsFilter[7] = objddlUserStatus.value;
}
function ResetRecord() {
    objtxtBACode.value = "";
    objtxtBAName.value = "";
    objddlBAStatus.value = "X";
    objtxtInstCode.value = "";
    objddlInstStatus.value = "X";
    objtxtLoginID.value = "";
    objddlUserStatus.value = "X";
    SearchRecord();
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Masters/VRSMasterQuery.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Masters/VRSMasterQuery.aspx';
        parent.PopupConfirm("028");
    }
}

function btnEditBA_OnClick(ID) {
    var URL = "Masters/VRSBillingAcctDlg.aspx";
    if (parent.GsIsBrowser == "Y") PreserveFilterValues();
    parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
    parent.PopupLoad();
    parent.objhdnMenuID.value = "42";
    parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=42&id=" + ID + "&cf=MQ&th=" + selTheme;
}

function btnEditInst_OnClick(ID) {
    var URL = "Masters/VRSInstitutionDlg.aspx";
    if (parent.GsIsBrowser == "Y") PreserveFilterValues();
    parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
    parent.PopupLoad();
    parent.objhdnMenuID.value = "18";
    parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=18&id=" + ID + "&cf=MQ&th=" + selTheme;
}
function btnEditUser_OnClick(ID) {
    var URL = "Settings/VRSUserDlg.aspx";
    if (parent.GsIsBrowser == "Y") PreserveFilterValues();
    parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
    parent.PopupLoad();
    parent.objhdnMenuID.value = "12";
    parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=12&id=" + ID + "&cf=MQ&th=" + selTheme;
}