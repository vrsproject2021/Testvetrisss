function getPromoReasonList() {

    //var ArrRecords = new Array();
    //ArrRecords[0] = $('#last_month option:selected').val();
    //ArrRecords[1] = $('#ddlModality option:selected').val();
    VRSPromoDetailsParams.GetPromoReasonList(apply_callback);
}
function apply_callback(lineData) {
   
    //var options = '<option selected value="00000000-0000-0000-0000-000000000000">All</option>';
    var options = '';
    if (lineData.value.Rows.length > 0) {
        
        for (var i = 0; i < lineData.value.Rows.length; i++) {
            options += '<option value="' + lineData.value.Rows[i].id + '">' + lineData.value.Rows[i].reason + '</option>';
        }
        $('#multiselect').append(options);
        $('#multiselect').multiselect({
            buttonWidth: '200px',
            nonSelectedText: 'All',
            numberDisplayed: 1,
        });
        $('#multiselect').multiselect({
            includeSelectAllOption: true,
            nonSelectedText: 'All'
        });
        //debugger;
        //$("#multiselect").selectpicker('All');
    }
    
}

getPromoReasonList();
//mindate();
function mindate() {
    debugger;
    var d = new Date(new Date().getFullYear(), 0, 1);

    var month = d.getMonth() + 1;
    var day = d.getDate();
    var year = d.getFullYear();
    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    var minDate = year + '-' + month + '-' + day;

    //$('#txtFromDate').attr('min', minDate);
    document.getElementById("txtFromDate").min = minDate;
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
function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = objtxtFromDate.value;
    ArrRecords[1] = objtxtToDate.value;
    ArrRecords[2] = objddlType.value;
    ArrRecords[3] = UserID;
    ArrRecords[4] = MenuID;
    CallBackBrw.callback(ArrRecords);
}
function ResetRecord() {
    objtxtFromDate.value = "";
    objtxtToDate.value = "";
    objddlType.value = "A"
    SearchRecord();
}
function btnSearch_OnClick() {
    SearchRecord();
}
function btnExcel_OnClick() {
    var ArrRecords = new Array();
    try {
        parent.parent.PopupProcess("N");

        ArrRecords[0] = objtxtFromDate.value;
        ArrRecords[1] = objtxtToDate.value;
        ArrRecords[2] = $('#multiselect').val() == null ? '00000000-0000-0000-0000-000000000000' : $('#multiselect').val().join(',');
        ArrRecords[3] = $('#ddlInstitution option:selected').val();
        ArrRecords[4] = UserID;
        ArrRecords[5] = MenuID;
        AjaxPro.timeoutPeriod = 1800000;
        VRSPromoDetailsParams.GenerateExcel(ArrRecords, ShowProcess);

    }
    catch (expErr) {
        parent.parent.HideProcess();
        parent.parent.PopupMessage(RootDirectory, strForm, "btnExcel_OnClick()", expErr.message, "true");
    }

}
//function btnClose_OnClick() {
//    Unlock = "N";
//    btnBrwClose_Onclick();
//}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Reports/VRSPromoDetailsParams.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Reports/VRSPromoDetailsParams.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    Unlock = "N";
    btnBrwClose_Onclick();
}
function ShowProcess(Result, MethodName) {
    parent.parent.HideProcess();
    var strMethod = MethodName.method;
    debugger;
    switch (strMethod) {
        case "GenerateExcel":
            ProcessReport(Result);
            break;
    }
}
function ProcessReport(Result) {
    var arrRet = new Array();
    arrRet = Result.value;
    switch (arrRet[0]) {
        case "catch":
            parent.parent.PopupMessage(RootDirectory, strForm, "ProcessReport()", arrRet[1], "true");
            break;
        case "false":
            parent.parent.PopupMessage(RootDirectory, strForm, "ProcessReport()", arrRet[1], "true");
            break;
        case "true":
            parent.parent.GsLaunchURL = arrRet[1];
            parent.parent.GsFilePath = arrRet[2];
            parent.parent.GsFileType = "XLS";
            parent.parent.PopupReportViewer();
            break;
    }
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
