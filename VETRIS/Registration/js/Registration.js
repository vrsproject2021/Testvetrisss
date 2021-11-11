var RootDirectory = objhdnRootDir.value;
if (parseInt($('#ddlCountry').val()) > 0) {
    ddlCountry_OnChange();
}
function ddlCountry_OnChange() {
    PopupProcess("N");
    var ArrRecords = new Array();

    try {
        AjaxPro.timeoutPeriod = 1800000;
        VRSRegistration.FetchStates(objddlCountry.value, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        PopupMessage(RootDirectory, strForm, "ddlCountry_OnChange()", expErr.message, "true");
    }

   
}
function PopulateStateList(Result) {

    var arrRes = new Array(); var url = "";
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
             PopupMessage(RootDirectory, strForm, "PopulateStateList()", arrRes[1], "true");
            break;
        case "true":
            objddlState.length = 0;
            for (var i = 1; i < arrRes.length; i = i + 2) {
                var op = document.createElement("option");
                op.value = arrRes[i];
                op.text = arrRes[i + 1];
                objddlState.add(op);
            }
            break;
    }

}
function ShowProcess(Result, MethodName) {
    HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "FetchStates":
            PopulateStateList(Result);
            break;
        case "SaveRecord":
            SaveRecord(Result);
            break;
    }
}
function btnRegistration_OnClick() {
    PopupProcess("Y");
    var ArrRecords = new Array();
    var ArrInst = new Array();
    var ArrPhys = new Array();
    var ArrModalities = new Array();

    try {
        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objName.value;
        ArrRecords[2] = objAddress_1.value;
        ArrRecords[3] = objCity.value;
        ArrRecords[4] = objZip.value;
        //ArrRecords[5] = objddlPaymentMethod.value;
        ArrRecords[5] = objContact_person_name.value;
        ArrRecords[6] = objContact_person_mobile.value;
        ArrRecords[7] = objddlCountry.value;
        ArrRecords[8] = objddlState.value;
        ArrRecords[9] = objEmail_id.value;
        ArrRecords[10] = objPhone_no.value;
        ArrRecords[11] = objLogin_id.value;
        ArrRecords[12] = objLogin_password.value;
        //ArrRecords[14] = objLogin_email_id.value;
        //ArrRecords[15] = objLogin_mobile_no.value;
        ArrRecords[13] = checkIsModalitySelected();
        ArrRecords[14] = objddlState.selectedOptions[0].text;
        ArrRecords[15] = objddlCountry.selectedOptions[0].text;
        ArrRecords[16] = objtxtSubmitBy.value;
        ArrRecords[17] = objtxtImgSoftware.value;
        ArrPhys = GetPhysicians();
        ArrModalities = GetModalities();

        AjaxPro.timoutPeriod = 1800000;
        VRSRegistration.SaveRecord(ArrRecords, ArrPhys, ArrModalities, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
}
function GetPhysicians() {
    var arrRecords = new Array(); var idx = 0;
    $('#tblBody tr').each(function (e, val) {
        var id = $(this).index();
        var rowId = $(this).attr("id");
        if ($('#physician_fname_' + rowId.split('_')[1]).val() != '' || $('#physician_lname_' + rowId.split('_')[1]).val() != '') {
            arrRecords[idx] = "00000000-0000-0000-0000-000000000000";
            arrRecords[idx + 1] = $('#physician_fname_' + rowId.split('_')[1]).val();
            arrRecords[idx + 2] = $('#physician_lname_' + rowId.split('_')[1]).val();
            arrRecords[idx + 3] = $('#physician_credentials_' + rowId.split('_')[1]).val();
            arrRecords[idx + 4] = $('#physician_email_' + rowId.split('_')[1]).val();
            arrRecords[idx + 5] = $('#physician_mobile_' + rowId.split('_')[1]).val();
            idx = idx + 6;
        }
    });
    return arrRecords;
}
function GetModalities() {
    var arrRecords = new Array();
    var idx = 0;
    var chks = $("#chkModality input:checkbox");
    for (var i = 0; i < chks.length; i++) {
        if (chks[i].checked) {
            arrRecords[idx + 0] = chks[i].value;
            idx = idx + 1;
        }
    }
    return arrRecords;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            //PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            objhdnID.value = arrRes[2];
            GsRetStatus = "false";
            window.location.href = "/VETRIS/Registration/VRSRegConfirmation.aspx";
            break;
    }
}

function checkIsModalitySelected() {
    var hasChecked = 'N';
    var chks = $("#chkModality input:checkbox");
    for (var i = 0; i < chks.length; i++) {
        if (chks[i].checked) {
            hasChecked = 'Y';
            break;
        }
    }
    return hasChecked;
}

