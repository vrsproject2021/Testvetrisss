$(document).ready($(function () {
    window.scrollTo(0, 0);
}))
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

function btnOkDR_Onclick() {
    var ArrParams = new Array();
    var strDtFrom = ""; var strDtTill = "";

    if (parent.Trim(objtxtFromDt.value) == "") strDtFrom = "01Jan1900";
    else {
        if (document.all)
            strDtFrom = parent.SetDateFormat(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtFrom = parent.SetDateFormat1(objtxtFromDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if (parent.Trim(objtxtTillDt.value) == "") strDtTill = "01Jan1900";
    else {
        if (document.all)
            strDtTill = parent.SetDateFormat(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtTill = parent.SetDateFormat1(objtxtTillDt.value, parent.GsDateFormat, parent.GsDateSep);
    }

    ArrParams[0] = objddlInstitution.value;
    ArrParams[1] = objddlService.value;
    ArrParams[2] = objddlType.value;
    ArrParams[3] = strDtFrom;
    ArrParams[4] = strDtTill;
    ArrParams[5] = UserID;

    CallBackBrw.callback(ArrParams);
}


function btnOKUA_Onclick() {
    var ArrParams = new Array();
    var strDtFrom = ""; var strDtTill = "";

    if (parent.Trim(objtxtFromDtUA.value) == "") strDtFrom = "01Jan1900";
    else {
        if (document.all)
            strDtFrom = parent.SetDateFormat(objtxtFromDtUA.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtFrom = parent.SetDateFormat1(objtxtFromDtUA.value, parent.GsDateFormat, parent.GsDateSep);
    }

    if (parent.Trim(objtxtTillDtUA.value) == "") strDtTill = "01Jan1900";
    else {
        if (document.all)
            strDtTill = parent.SetDateFormat(objtxtTillDtUA.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDtTill = parent.SetDateFormat1(objtxtTillDtUA.value, parent.GsDateFormat, parent.GsDateSep);
    }

    ArrParams[0] = objddlUserName.value;
    ArrParams[1] = objtxtActivity.value;
    ArrParams[2] = strDtFrom;
    ArrParams[3] = strDtTill;

    CallBackUA.callback(ArrParams);
}

function SetSelectedDate(objCtrlID, CalName, LsImgID) {
    var strDate = ""; var strClass = "";
    var dt;
    objCtrl = document.getElementById(objCtrlID.id); if (objCtrl == null) objCtrl = objCtrlID;
    strDate = document.getElementById(objCtrl).value;

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
    switch (CalName) {
        case "CalFrom":
            CalFrom.setSelectedDate(dt); CalFrom.show();
            break;
        case "CalTill":
            CalTill.setSelectedDate(dt); CalTill.show();
            break;
        case "CalFromUA":
            CalFromUA.setSelectedDate(dt); CalFromUA.show();
            break;
        case "CalTillUA":
            CalTillUA.setSelectedDate(dt); CalTillUA.show();
            break;
    }


}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}
