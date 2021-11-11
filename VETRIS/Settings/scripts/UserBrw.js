
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
}
function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objtxtCode.value = parent.GsFilter[0];
        objtxtName.value = parent.GsFilter[1];
        objddlStatus.value = parent.GsFilter[2];
        objddlRole.value = parent.GsFilter[3];
        objtxtLoginID.value = parent.GsFilter[4];
        objtxtInstitution.value = parent.GsFilter[5];
        objtxtBA.value = parent.GsFilter[6];
        objddlAllowMS.value = parent.GsFilter[7];
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objtxtCode.value;
    ArrRecords[1] = objtxtName.value;
    ArrRecords[2] = objddlStatus.value;
    ArrRecords[3] = objddlRole.value;
    ArrRecords[4] = objtxtLoginID.value;
    ArrRecords[5] = objtxtInstitution.value;
    ArrRecords[6] = objtxtBA.value;
    ArrRecords[7] = objddlAllowMS.value;
    ArrRecords[8] = UserID;
    ArrRecords[9] = MenuID;
    ArrRecords[10] = SessionID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtCode.value);
    parent.GsFilter[1] = parent.Trim(objtxtName.value);
    parent.GsFilter[2] = objddlStatus.value;
    parent.GsFilter[3] = objddlRole.value;
    parent.GsFilter[4] = parent.Trim(objtxtLoginID.value);
    parent.GsFilter[5] = parent.Trim(objtxtInstitution.value);
    parent.GsFilter[6] = parent.Trim(objtxtBA.value);
    parent.GsFilter[7] = objddlAllowMS.value;
}
function ResetRecord() {
    objtxtCode.value = "";
    objtxtName.value = "";
    objddlStatus.value = "X";
    objddlRole.value = "0";
    objtxtLoginID.value = "";
    objtxtInstitution.value = "";
    objtxtBA.value = "";
    objddlAllowMS.value = "A";
    SearchRecord();
}


