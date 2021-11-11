
function view_Searchform() {
    $("#searchSection").slideToggle(800);
    $("#searchIcon").toggleClass("icon", 800);
    imgFromDt.setAttribute("onclick", "javascript:SetSelectedDate('CalFromDate','imgFromDt');");
    imgToDt.setAttribute("onclick", "javascript:SetSelectedDate('CalToDate','imgToDt');");
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
        objtxtName.value = parent.GsFilter[0];
    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objtxtName.value;
    ArrRecords[1] = UserID;
    ArrRecords[2] = MenuID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}
function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objtxtName.value);
}
function ResetRecord() {
    objtxtCode.value = "";
    objtxtName.value = "";
    SearchRecord();
}