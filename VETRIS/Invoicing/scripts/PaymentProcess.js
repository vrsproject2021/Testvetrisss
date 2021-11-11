$(document).ready(function () {
    SearchRecord();
});

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

function btnClose_OnClick() {
	Unlock = "N";
	btnBrwClose_Onclick();
}

function btnFind_OnClick() {
	var AccountID = objddlAccount.value;
	parent.PopupMessage(RootDirectory, strForm, "btnFind_OnClick()", "229", "true");

}

function ddlAccount_OnChange() {
	//CallBackPromo.callback("L", objhdnID.value, objddlAccount.value, objddlType.value);
}

function SetFilterValues() {
    if (parent.GsFilter.length > 0) {
        objddlAccount.value = parent.GsFilter[0];
        objddlType.value = parent.GsFilter[1];
        objddlStatus.value = parent.GsFilter[2];
        objddlUser.value = parent.GsFilter[3];
        objtxtFromDate.value = parent.GsFilter[4];
        objtxtToDate.value = parent.GsFilter[5];
        objtxtPaymentRef.value = parent.GsFilter[6];
        objtxtExternalPaymentRef.value = parent.GsFilter[7];
    }
    else
        parent.GsFilter.length = 0;
}

function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objddlAccount.value;
    ArrRecords[1] = objddlType.value;
    ArrRecords[2] = objddlStatus.value;
    ArrRecords[3] = UserID;
    ArrRecords[4] = MenuID;
    ArrRecords[5] = objtxtFromDate.value;
    ArrRecords[6] = objtxtToDate.value;
    ArrRecords[7] = objtxtPaymentRef.value;
    ArrRecords[8] = objtxtExternalPaymentRef.value;

    PreserveFilterValues();
    CallBackBrw.callback(ArrRecords);
}

function PreserveFilterValues() {
    parent.GsFilter.length = 0;
    parent.GsFilter[0] = objddlAccount.value;
    parent.GsFilter[1] = objddlType.value;
    parent.GsFilter[2] = objddlStatus.value;
    parent.GsFilter[3] = objddlUser.value;
    parent.GsFilter[4] = objtxtFromDate.value;
    parent.GsFilter[5] = objtxtToDate.value;
    parent.GsFilter[6] = objtxtPaymentRef.value;
    parent.GsFilter[7] = objtxtExternalPaymentRef.value;
}

function ResetRecord() {
    var strDtFrom = ""; var strDtTill = "";
    
    var dtFrom = new Date();
    var dtTill = new Date();
    dtFrom = dtFrom.addDays(-30);
    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDtFrom = dtFrom.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2);
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strDtFrom = parent.padZeroPlaces((parseInt(dtFrom.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtFrom.getDate(), 2) + parent.objhdnDateSep.value + dtFrom.getFullYear();

    if (parent.GsDateFormat == "dd" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "MM" + parent.objhdnDateSep.value + "dd" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();
    else if (parent.GsDateFormat == "yyyy" + parent.objhdnDateSep.value + "MM" + parent.objhdnDateSep.value + "dd")
        strDtTill = dtTill.getFullYear() + parent.objhdnDateSep.value + parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2);
    else if (parent.GsDateFormat == "M" + parent.objhdnDateSep.value + "d" + parent.objhdnDateSep.value + "yyyy")
        strDtTill = parent.padZeroPlaces((parseInt(dtTill.getMonth()) + 1), 2) + parent.objhdnDateSep.value + parent.padZeroPlaces(dtTill.getDate(), 2) + parent.objhdnDateSep.value + dtTill.getFullYear();

    objddlAccount.value = "00000000-0000-0000-0000-000000000000";
    objddlUser.value = "00000000-0000-0000-0000-000000000000";
    objddlType.value = "A";
    objddlStatus.value = "A";
    objtxtPaymentRef.value = "";
    objtxtExternalPaymentRef.value = "";
    objtxtFromDate.value = strDtFrom;
    objtxtToDate.value = strDtTill;
    SearchRecord();
}

function SetSelectedDate(CalName, objImgID) {
    var strDate = ""; var strClass = "";
    var dt;
    objCtrl = document.getElementById(CalName); if (objCtrl == null) objCtrl = CalName;
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.Trim(strDate) != "") {
        if (document.all) {
            dt = new Date(parent.SetDateFormat(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
        else {
            dt = new Date(parent.SetDateFormat1(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
    }
    else
        dt = new Date();
    switch (objImgID) {
        case "CalFromDate":
            CalFromDate.setSelectedDate(dt); CalFromDate.show();
            break;
        case "CalToDate":
            CalToDate.setSelectedDate(dt); CalToDate.show();

            break;

    }


    parent.GsRetStatus = "true";
}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}
