var strDivider = objhdnDivider.value; var strRootDirectory = objhdnRootDirectory.value; var ServerPath = objhdnServerPath.value;
var GsTheme = objhdnPrefTheme.value; var PageURL = "";var PrevTheme ="";
var strNavMenuID = ""; var strMenuURL = ""; var strMenuIsBrw = ""; var ActFlg = 0;
$(window).resize(function () {
    adjustFrameHeight();
});
$(document).ready($(function () {
    window.history.forward();
    adjustFrameHeight();
    HideLoad();
    CheckError();
    if (objhdnTempInstID.value != "00000000-0000-0000-0000-000000000000") {
        LoadManualSubmissionTemp();
    }
    else if (objhdnMenuID.value == "0") LoadHome(objhdnUserID.value);
    else LoadPage();
    adjustFrameHeight();
}))
$(function () {
    $('#ChangeToggle').click(function () {
        $('#navbar-hamburger').toggleClass('hidden');
        $('#navbar-close').toggleClass('hidden');
    });
});
function CheckError() {
    if (Trim(objhdnError.value) != "") {
        var strError = objhdnError.value;
        var strErrCodes = objhdnError.value.substring(objhdnError.value.indexOf(strDivider) + 1, objhdnError.value.length);
        var arrErr = new Array();
        if (strError.indexOf(strDivider) > -1)
            arrErr = strError.split(strDivider);
        else
            arrErr[0] = strError;

        if (arrErr[0] == "catch")
            PopupMessage(strRootDirectory, strForm, "OnLoad()", arrErr[1], "true");
        else if (arrErr[0] == "false") {
            PopupMessage(strRootDirectory, strForm, "OnLoad()", arrErr[1], "true");
        }

        objhdnError.value = "";
    }
    if (objhdnAllowMS.value == "Y") document.getElementById("msIcon").style.display = "inline";
    if (objhdnAllowDB.value == "Y") document.getElementById("dashboard").style.display = "inline";
    if (GsTheme == "DARK") {
        document.getElementById("spnModeDef").style.display = "none";
        document.getElementById("spnModeDark").style.display = "inline";
    }
    else if (GsTheme == "DEFAULT") {
        document.getElementById("spnModeDef").style.display = "inline";
        document.getElementById("spnModeDark").style.display = "none";
    }

}

function SlideDown() {
    if (document.getElementById("divMenu").style.display == "none")
        $('#divMenu').slideDown("slow");
    else
        $('#divMenu').slideUp("slow");
}
function SlideUp() {
    if (document.getElementById("divMenu").style.display != "none")
        $('#divMenu').slideUp("slow");
}
function getOffset(el) {
    var _x = 0;
    var _y = 0;
    while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
        _x += el.offsetLeft - el.scrollLeft;
        _y += el.offsetTop - el.scrollTop;
        el = el.offsetParent;
    }
    return { top: _y, left: _x };
}

function FetchMenuRecordCount() {
    var ArrRecords = new Array();
    var arrRes = new Array();
    var MenuID = "";
    var Count = "0";

    try {

        ArrRecords[0] = objhdnUserID.value;

        AjaxPro.timeoutPeriod = 1800000;
        Result = VRSMain.FetchMenuRecordCount(ArrRecords);
        arrRes = Result.value;
        switch (arrRes[0]) {
            case "catch":
            case "false":
                PopupMessage(strRootDirectory, strForm, "FetchMenuRecordCount()", arrRes[1], "true");
                break;
            case "true":
                if (arrRes.length > 1) {
                    for (var i = 1; i < arrRes.length; i++) {
                        MenuID = arrRes[i];
                        Count = arrRes[i + 1];

                        if (document.getElementById("spnCnt_" + MenuID) != null) {
                            if (document.all) document.getElementById("spnCnt_" + MenuID).innerText = padZeroPlaces(Count);
                            else document.getElementById("spnCnt_" + MenuID).textContent = padZeroPlaces(Count);
                        }

                        i = i + 1;
                    }
                }
                break;
        }

    }
    catch (expErr) {
        HideProcess();
        PopupMessage(strRootDirectory, strForm, "FetchMenuRecordCount()", expErr.message, "true");
    }
}
function NavigatePACS(URL) {
    var win = window.open(URL, '_blank');
    win.focus();
}

function NavMenu(url, menu_id, is_browser, level, keepFilter) {
    objhdnMenuID.value = menu_id;
    if (keepFilter == null) keepFilter = "N";

    if (GsRetStatus == "false") {
        if (keepFilter == "N") GsFilter.length = 0;
        if (url != "") {
            if (is_browser == null) is_browser = "Y";
            GsIsBrowser = is_browser;
            PopupLoad();
            objiframePage.src = "";
            if (url.indexOf("?") != -1)
                objiframePage.src = url + "&uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=" + menu_id + "&sid=" + objhdnSessionID.value + "&th=" + GsTheme;
            else
                objiframePage.src = url + "?uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=" + menu_id + "&sid=" + objhdnSessionID.value + "&th=" + GsTheme;
            objhdnMenuID.value = menu_id;
        }

        else {
            GsConfirmAction = "MC"; strNavMenuID = menu_id; strMenuURL = url; strMenuIsBrw = is_browser;
            PopupConfirm("028");
        }
    }
    else {
        GsConfirmAction = "MC"; strNavMenuID = menu_id; strMenuURL = url; strMenuIsBrw = is_browser;
        PopupConfirm("028");
    }
}
function PopupApplication(menu_id, menu_desc) {
    $("#reportcontainer").css("display", "");
    if (objhdnRPTEGNURL.value != "") {
        objreportserver.src = objhdnRPTEGNURL.value + "?cachebuster="+(+new Date())+"&token=" + objhdnUserID.value + "#/tabular/" + encodeURI(menu_desc);
    }
    $("#reportcontainer").height(window.innerHeight);
}
function closereports() {
    objreportserver.src = "";
    $("#reportcontainer").css("display", "none");
}
function DashboardPopupApplication(menu_id, menu_desc) {
    $("#dashboardreportcontainer").css("display", "");
    if (objhdnRPTEGNURL.value != "") {
        objdashboardreport.src = objhdnRPTEGNURL.value + "?token=" + objhdnUserID.value + "#/tabular/" + encodeURI(menu_desc);
    }
    $("#reportcontainer").height(window.innerHeight);
}

function LoadHome(UserID) {
    objiframePage.src = "";
    GsFilter.length = 0;
    GsIsBrowser = "N";
    if (UserID == undefined) objiframePage.src = "VRSHome.aspx?th=" + objhdnPrefTheme.value;
    else objiframePage.src = "VRSHome.aspx?uid=" + UserID + "&sid=" + objhdnSessionID.value + "&th=" + objhdnPrefTheme.value;
    adjustFrameHeight();
}
function Logout() {
    if (GsRetStatus == "false") {
        window.location = "VRSLogout.aspx?uid=" + objhdnUserID.value + "&sid=" + objhdnSessionID.value + "&th=" + GsTheme;
    }
    else {
        GsConfirmAction = "LO";
        PopupConfirm("028");
    }
}

function LoadChangePwd() {

    var userID = objhdnUserID.value;
    var userName = objhdnUserName.value;
    GsLaunchURL = "VRSChangePwd.aspx?uid=" + userID + "&unm=" + userName + "&th=" + GsTheme;
    objiframePage.src = GsLaunchURL;
}
function LoadPage() {
    var MenuID = objhdnMenuID.value;
    var PreDefMenuExists = objhdnPreDefMenu.value;
    adjustFrameHeight();

    if (PreDefMenuExists == "Y") {
        switch (MenuID) {
            case "20":
                NavMenu("CaseList/VRSCaseRABrw.aspx", 20, "Y", 1);
                break;
            case "37":
                NavMenu("Study/VRSProcImageDlg.aspx", 37, "N", 1);
                break;
            case "62":
                NavMenu("MyPayment/VRSPmtGatewayLink.aspx?aid=" + objhdnBillAcctID.value + "&th=" + GsTheme, 62, "N", 1);
                break;
        }
    }
    else {
        PopupMessage(strRootDirectory, strForm, "LoadPage()", "314", "true");
    }
}
function LoadManualSubmission() {

    var url = "CaseList/VRSManualSubmissionUploadFile.aspx";
    if (GsRetStatus == "false") {
        PopupLoad();
        GsIsBrowser = "N";
        objiframePage.src = "";
        objiframePage.src = url + "?uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=0&sid=" + objhdnSessionID.value + "&th=" + GsTheme;
        objhdnMenuID.value = "0";
    }
    else {
        GsConfirmAction = "MC"; strNavMenuID = 0; strMenuURL = url; strMenuIsBrw = "N";
        PopupConfirm("028");
    }
}
function Dashboard_OnClick() {
    var ArrRecords = new Array();
    if (objhdnUserRoleCode.value === 'RDL') {
        var d = new Date();
        //var zoneName = d.toString().match(".*(\\((.*)\\))")[2];
        var zoneName = objhdnRadiologistTimeZone.value;
        $('#divHeader').html(objhdnUserName.value + ': Productivity Status as on ' + d.toLocaleDateString() + ', time : ' + d.toLocaleTimeString([], {timeStyle: 'short'})+' ('+ zoneName+')');
        $('#dashboardradiologistcontainer').css({ "display": "block" });
        $("#overlay").css({ "display": "block" });
        document.getElementById("myModal").style.display = "block";
        ArrRecords[0] = objhdnRadiologistID.value;
        ArrRecords[1] = objhdnUserID.value;
        var objData = VRSMain.FetchRadiologistProductivity(ArrRecords)
        setProductivity(objData);
    } else {
        $('#dashboardradiologistcontainer').css({ "display": "none" });
        if ($('#hdnDashboardId').val() === '')
            return;
        var url = "Dashboard/VRSDashboard.aspx";
        $("#dashboardreportcontainer").css("display", "");

        objdashboardreport.src = url + "?uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&dashId=" + $('#hdnDashboardId').val() + "&th=" + GsTheme;

        $("#dashboardreportcontainer").height(window.innerHeight);
    }
}

function setProductivity(result) {
    var strHtml = '';
    var barColors = new Array(result.value.length).fill().map(function (e, i) { return Colors.select(i).rgb; });
    
    for (var i = 0; i < result.value.length; i++) {
        strHtml += '<tr>'+
                        '<td style="text-align: left;">' + result.value[i].modality + '</td>' +
                        '<td>'+result.value[i].assigned_count+'</td>'+
                        '<td>'+result.value[i].work_progress_count+'</td>'+
                        '<td>'+result.value[i].today_count+'</td>'+
                        '<td>'+result.value[i].this_month_count+'</td>'+
                        '<td>'+result.value[i].last_month_count+'</td>'+
                        '<td>' + result.value[i].this_year_count + '</td>' +
                    '</tr>';
       
    }
    $('#data tbody').html(strHtml);
    var sumResult = result.value.reduce(function (previousValue, currentValue) {
        return {
            assigned_count: previousValue.assigned_count + currentValue.assigned_count,
            work_progress_count: previousValue.work_progress_count + currentValue.work_progress_count,
            today_count: previousValue.today_count + currentValue.today_count,
            this_month_count: previousValue.this_month_count + currentValue.this_month_count,
            last_month_count: previousValue.last_month_count + currentValue.last_month_count,
            this_year_count: previousValue.this_year_count + currentValue.this_year_count,
        }
    });
    strHtml = '<tr>' +
                        '<th style="text-align:right;">Total</th>' +
                        '<td>' + sumResult.assigned_count + '</td>' +
                        '<td>' + sumResult.work_progress_count + '</td>' +
                        '<td>' + sumResult.today_count + '</td>' +
                        '<td>' + sumResult.this_month_count + '</td>' +
                        '<td>' + sumResult.last_month_count + '</td>' +
                        '<td>' + sumResult.this_year_count + '</td>' +
                    '</tr>';
    $('#data tfoot').html(strHtml);
}
// Close the radiologist productivity
function closeModal() {
    $('#dashboardradiologistcontainer').css({ "display": "none" });
    document.getElementById("myModal").style.display = "none";
}
function closedashboardreport() {
    objdashboardreport.src = "";
    $("#dashboardreportcontainer").css("display", "none");
}
function LoadManualSubmissionTemp() {
    var url = "CaseList/VRSManualSubmissionUploadFile.aspx";
    if (GsRetStatus == "false") {
        document.getElementById("loguser").style.display = "none";
        PopupLoad();
        GsIsBrowser = "N";
        objiframePage.src = "";
        objiframePage.src = url + "?InstID=" + hdnTempInstID.value + "&uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=0";
        objhdnMenuID.value = "0";
    }
    else {
        GsConfirmAction = "MC"; strNavMenuID = 0; strMenuURL = url; strMenuIsBrw = "N";
        PopupConfirm("028");
    }
}
function OpenReportComparison(ID, UserID) {
    var url = "VRSCompareDocuments.aspx?id=" + ID + "&uid=" + UserID + "&th=" + GsTheme;
    window.open(url);
}
function ProcessConfirm(ArgsRet) {
    if (ArgsRet == "Y") {
        GsRetStatus = "false";
        GsFilter.length = 0;
        switch (GsConfirmAction) {
            case "LO":
                location.href = "../VRSLogout.aspx?uid=" + objhdnUserID.value + "&th=" + GsTheme;
                break;
            case "MC":
                if (strMenuURL != "") {
                    parent.GsIsBrowser = strMenuIsBrw;
                    PopupLoad();
                    objiframePage.src = "";
                    if (strMenuURL.indexOf("?") != -1)
                        objiframePage.src = strMenuURL + "&uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=" + strNavMenuID + "&th=" + GsTheme;
                    else
                        objiframePage.src = strMenuURL + "?uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=" + strNavMenuID + "&th=" + GsTheme;
                    objhdnMenuID.value = strNavMenuID;
                }
                break;
        }
    }

    GsConfirmAction = "";
}
function SetMobleMainMenu() {
    $('#navbar-hamburger').toggleClass('hidden');
    $('#navbar-close').toggleClass('hidden');
    objmobile_menu.className = "navbar-collapse collapse";

}
function ToggleMobileMenu(GroupID) {


    for (var i = 1; i <= document.getElementById('hdnDDMenuCnt').value; i++) {
        if (GroupID == i.toString()) document.getElementById("mobmenugrp_" + i.toString()).style.display = "block";
        else document.getElementById("mobmenugrp_" + i.toString()).style.display = "none";
    }
}

//function ToggleTheme(theme) {
//    var ArrParams = new Array();
//    PrevTheme = GsTheme;

//    if (theme == "DARK") {
//        document.getElementById("spnModeDef").style.display = "none";
//        document.getElementById("spnModeDark").style.display = "inline";
//    }
//    else if (theme == "DEFAULT") {
//        document.getElementById("spnModeDef").style.display = "inline";
//        document.getElementById("spnModeDark").style.display = "none";
//    }
//    GsTheme = theme;

   
//    try {
//        PopupProcess("N");
//        ArrParams[0] = theme;
//        ArrParams[1] = objhdnUserID.value;

//        AjaxPro.timeoutPeriod = 1800000;
//        VRSMain.UpdatePreferedTheme(ArrParams, ShowProcess);
//    }
//    catch (expErr) {
//        HideProcess();
//        PopupMessage(strRootDirectory, strForm, "Login_Onclick()", expErr.message, "true");
//    }
//}
function ToggleTheme() {
    var ArrParams = new Array();
    PrevTheme = GsTheme;

    if (GsTheme == "DARK") {
        document.getElementById("spnModeDef").style.display = "inline";
        document.getElementById("spnModeDark").style.display = "none";
        GsTheme = "DEFAULT";
    }
    else if (GsTheme == "DEFAULT") {
        document.getElementById("spnModeDef").style.display = "none";
        document.getElementById("spnModeDark").style.display = "inline";
        GsTheme = "DARK";
    }
    


    try {
        PopupProcess("N");
        ArrParams[0] = GsTheme;
        ArrParams[1] = objhdnUserID.value;

        AjaxPro.timeoutPeriod = 1800000;
        VRSMain.UpdatePreferedTheme(ArrParams, ShowProcess);
    }
    catch (expErr) {
        HideProcess();
        PopupMessage(strRootDirectory, strForm, "Login_Onclick()", expErr.message, "true");
    }
}
function UpdatePreferedTheme(Result) {

    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
        case "false":
            PopupMessage(strRootDirectory, strForm, "UpdatePreferedTheme()", arrRes[1], "true");
            GsTheme = PrevTheme;
            break;
        case "true":
            PopupLoad();
            set_cookie("vrsTheme", GsTheme, arrRes[1], arrRes[2], arrRes[3]);
            location.href = "VRSMain.aspx?uid=" + objhdnUserID.value + "&ucd=" + objhdnUserCode.value + "&unm=" + objhdnUserName.value + "&sid=" + objhdnSessionID.value;
            //"&mid=" + objhdnMenuID.value + 
            
            
            break;
    }

}
function ShowProcess(Result, MethodName) {
    HideProcess(); 
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "UpdatePreferedTheme":
            UpdatePreferedTheme(Result);
            break;
    }
}
function set_cookie(strName, strValue, strExp_y, strExp_m, strExp_d) {
    var strCookieString = strName + "=" + escape(strValue);

    if (strExp_y) {
        var strExpires = new Date(strExp_y, strExp_m, strExp_d);
        strCookieString += "; expires=" + strExpires.toGMTString();
    }

    document.cookie = strCookieString;
}
