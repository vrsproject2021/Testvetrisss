$(document).ready(function () {
    objddlAccount.value = "00000000-0000-0000-0000-000000000000";
    setTimeout(function () {
        ddlAccount_OnChange();
    }, 100)
    ddlAccount_OnChange();
});

function view_Searchform() {
    //$("#searchSection").slideToggle(800);
    //$("#searchIcon").toggleClass("icon", 800);
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

function btnClose_OnClick() {
	Unlock = "N";
	btnBrwClose_Onclick();
}

function btnFind_OnClick() {
	var AccountID = objddlAccount.value;
	parent.PopupMessage(RootDirectory, strForm, "btnFind_OnClick()", "229", "true");

}

function ddlAccount_OnChange() {
    var ArrRecords = new Array();
    ArrRecords[0] = objddlAccount.value;
    ArrRecords[1] = "A";
    ArrRecords[2] = "A";
    ArrRecords[3] = UserID;
    ArrRecords[4] = MenuID;
    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}

function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objddlAccount.value = parent.GsFilter[0];
    }
    else
        parent.GsFilter.length = 0;
}


function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = objddlAccount.value;
}
function ResetRecord() {
    objddlAccount.value = "00000000-0000-0000-0000-000000000000";
    
}

