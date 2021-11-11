
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
        objddlStudyStatus.value = parent.GsFilter[0];
        objddlPriority.value = parent.GsFilter[1];
        objddlStatus.value = parent.GsFilter[2];
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objddlStudyStatus.value;
    ArrRecords[1] = objddlPriority.value;
    ArrRecords[2] = objddlStatus.value;
    ArrRecords[3] = UserID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objddlStudyStatus.value);
    parent.GsFilter[1] = parent.Trim(objddlPriority.value);
    parent.GsFilter[2] = objddlStatus.value;
}
function ResetRecord() {
    objddlStudyStatus.value = "-999";
    objddlPriority.value = "0";
    objddlStatus.value = "X";

    SearchRecord();
}


