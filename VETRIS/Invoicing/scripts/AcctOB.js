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

function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSAcctOB.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSAcctOB.aspx';
        parent.PopupConfirm("028");
    }
}
function ResetRecord() {
    SearchRecord();
}

function btnClose_OnClick() {
    Unlock = "N";
    btnBrwClose_Onclick();
}
function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objddlAccount.value = parent.GsFilter[0];
        objddlYear.value = parent.GsFilter[1];

    }
    else
        parent.GsFilter.length = 0;
}
function SearchRecord() {

    var ArrRecords = new Array();
    ArrRecords[0] = objddlAccount.value;
    ArrRecords[1] = objddlYear.value;
    ArrRecords[2] = UserID;
    ArrRecords[3] = MenuID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}

function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = parent.Trim(objddlAccount.value);
    parent.GsFilter[1] = parent.Trim(objddlYear.value);
}

function btnFind_OnClick() {
    var AccountID = objddlAccount.value;
    parent.PopupMessage(RootDirectory, strForm, "btnFind_OnClick()", "229", "true");
   
}

function ddlAccount_OnChange() {
    //CallBackPromo.callback("L", objhdnID.value, objddlAccount.value, objddlType.value);
}



function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}

