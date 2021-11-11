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
        objddlAccount.value = parent.GsFilter[0];
        objddlType.value    = parent.GsFilter[1];
        objddlUser.value    = parent.GsFilter[2];
        objddlStatus.value  = parent.GsFilter[3];
        objddlReason.value  = parent.GsFilter[4];
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objddlAccount.value;
    ArrRecords[1] = objddlType.value;
    ArrRecords[2] = objddlUser.value;
    ArrRecords[3] = objddlStatus.value;
    ArrRecords[4] = objddlReason.value;
    ArrRecords[5] = UserID;
    ArrRecords[6] = MenuID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = objddlAccount.value;
    parent.GsFilter[1] = objddlType.value;
    parent.GsFilter[2] = objddlUser.value;
    parent.GsFilter[3] = objddlStatus.value;
    parent.GsFilter[4] = objddlReason.value;
}
function ResetRecord() {
    objddlAccount.value = "00000000-0000-0000-0000-000000000000";
    objddlUser.value = "00000000-0000-0000-0000-000000000000";
    objddlReason.value = "00000000-0000-0000-0000-000000000000";
    objddlType.value = "A";
    objddlStatus.value  = "Y";
    SearchRecord();
}