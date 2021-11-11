var strDivider = objhdnDivider.value; var strRootDirectory = objhdnRootDirectory.value; var ServerPath = objhdnServerPath.value;
var strNavMenuID = ""; var strMenuURL = ""; var strMenuIsBrw = ""; var ActFlg = 0;
$(window).resize(function () {
    adjustFrameHeight();
});
$(document).ready($(function () {
    window.history.forward();
    adjustFrameHeight();
    HideLoad();
    CheckError();
    LoadHome();
    adjustFrameHeight();
}))
$(function () {
    $('#ChangeToggle').click(function () {
        $('#navbar-hamburger').toggleClass('hidden');
        $('#navbar-close').toggleClass('hidden');
    });
});
function CheckError() 
{
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
}
function SlideDown() {
    if(document.getElementById("divMenu").style.display=="none")
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


function ShowProcess(Result, MethodName) {
    //HideProcess(); HideMail();
    //var strMethod = MethodName.method;
    //switch (strMethod) {
    //    case "FetchMenuRecordCount":
    //        populateMenu(Result);
    //        break;
    //}
}
function NavMenu(url, menu_id, is_browser,level) {
    objhdnMenuID.value = menu_id;
    
    
    if (GsRetStatus == "false") {
        GsFilter.length = 0;
        if (url != "") {
            if (is_browser == null) is_browser = "Y";
            GsIsBrowser = is_browser;
            PopupLoad();
            objiframePage.src = "";
            if (url.indexOf("?") != -1)
                objiframePage.src = url + "&uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=" + menu_id;
            else
                objiframePage.src = url + "?uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=" + menu_id;
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
function LoadHome() {
    objiframePage.src = "";
    GsFilter.length = 0;
    GsIsBrowser = "N";
    objiframePage.src = "VRSHome.aspx";
    adjustFrameHeight();
}
function Logout() {
    if (GsRetStatus == "false") {
        window.location = "VRSLogout.aspx?uid=" + objhdnUserID.value;
    }
    else {
        GsConfirmAction = "LO";
        PopupConfirm("028");
    }
}
function LoadChangePwd() {

    var userID = objhdnUserID.value;
    var userName = objhdnUserName.value;
    GsLaunchURL = "VRSChangePwd.aspx?uid=" + userID + "&unm=" + userName;
    objiframePage.src = GsLaunchURL;
}

function ProcessConfirm(ArgsRet) {
    if (ArgsRet == "Y") {
        GsRetStatus = "false";
        GsFilter.length = 0;
        switch (GsConfirmAction) {
            case "LO":
                location.href = "../VRSLogout.aspx?uid=" + objhdnUserID.value;
                break;
            case "MC":
                if (strMenuURL != "") {
                    parent.GsIsBrowser = strMenuIsBrw;
                    PopupLoad();
                    objiframePage.src = "";
                    if (strMenuURL.indexOf("?") != -1)
                        objiframePage.src = strMenuURL + "&uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=" + strNavMenuID;
                    else
                        objiframePage.src = strMenuURL + "?uid=" + objhdnUserID.value + "&urid=" + objhdnUserRoleID.value + "&mid=" + strNavMenuID;
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
    

    for(var i=1;i<=document.getElementById('hdnDDMenuCnt').value;i++)
    {
        if (GroupID == i.toString()) document.getElementById("mobmenugrp_" + i.toString()).style.display = "block";
        else document.getElementById("mobmenugrp_" + i.toString()).style.display = "none";
    }
}
