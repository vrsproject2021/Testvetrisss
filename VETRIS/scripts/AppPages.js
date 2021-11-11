var UserID = parent.objhdnUserID.value; var UserName = parent.objhdnUserName.value; var UserRoleID = parent.objhdnUserRoleID.value; var UserRoleCode = parent.objhdnUserRoleCode.value;
var MenuID = parent.objhdnMenuID.value;  var Divider = parent.objhdnDivider.value; var SecDivider = parent.objhdnSecDivider.value;
var RootDirectory = parent.objhdnRootDirectory.value; var ServerPath = parent.objhdnServerPath.value; var SessionID = parent.objhdnSessionID.value;
var WSSessionID = parent.objhdnWS8Session.value; var APIVER = parent.objhdnAPIVER.value;
var WS8SRVIP = parent.objhdnWS8SRVIP.value; var WS8CLTIP = parent.objhdnWS8CLTIP.value; var WS8SRVUID = parent.objhdnWS8SRVUID.value; var WS8SRVPWD = parent.objhdnWS8SRVPWD.value;
var strValidate = "Y"; var Unlock = "N"; var UI = "00000000-0000-0000-0000-000000000000"; var strLoadPopup = "Y"; var selTheme = parent.GsTheme;
var PACSUID = parent.objhdnPACSUID.value; var PACSPwd = parent.objhdnPACSPwd.value; var InstCode = parent.objhdnInstCode.value; var InitialiseID = "N";


$(document).ready($(function () {
    parent.window.scrollTo(0, 0);
    window.history.forward();
    parent.adjustFrameHeight();
    parent.HideLoad();
    parent.GsNavURL = ""; parent.GsRetStatus = "false"; parent.GsConfirmAction = "";
    parent.FetchMenuRecordCount()

    //SetCss();
    CheckError();
    if ((parent.GsIsBrowser == "Y")) {
        SetFilterValues();
        SearchRecord();
        //SetCss();
    }
    else {
        //track changes made by user
        $("#form1 :input").change(function () {
            parent.GsRetStatus = "true";
            parent.GsIsBrowser = "N";

        });
    }
   
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0;// For Chrome, Firefox, IE and Opera
    
}))
Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};
// datepart: 'y', 'm', 'w', 'd', 'h', 'n', 's'
Date.dateDiff = function (datepart, fromdate, todate) {
    datepart = datepart.toLowerCase();
    var diff = todate - fromdate;
    var divideBy = {
        w: 604800000,
        d: 86400000,
        h: 3600000,
        n: 60000,
        s: 1000
    };

    return Math.floor(diff / divideBy[datepart]);
}
Date.prototype.getWeekOfMonth = function () {
    var dayOfMonth = this.getDay();
    var month = this.getMonth();
    var year = this.getFullYear();
    var checkDate = new Date(year, month, this.getDate());
    var checkDateTime = checkDate.getTime();
    var currentWeek = 0;

    for (var i = 1; i < 32; i++) {
        var loopDate = new Date(year, month, i);

        if (loopDate.getDay() == dayOfMonth) {
            currentWeek++;
        }

        if (loopDate.getTime() == checkDateTime) {
            return currentWeek;
        }
    }

    //var firstDay = new Date(this.getFullYear(), this.getMonth(), 1).getDay();
    //return Math.ceil((this.getDate() + firstDay) / 7);
};
Date.prototype.getWeeksInMonth = function () {
    var firstDay = new Date(this.setDate(1)).getDay();
    var totalDays = new Date(this.getFullYear(), this.getMonth() + 1, 0).getDate();
    return Math.ceil((firstDay + totalDays) / 7);
}
Date.prototype.getWeek = function () {
    var date = new Date(this.getTime());
    date.setHours(0, 0, 0, 0);
    // Thursday in current week decides the year.
    date.setDate(date.getDate() + 3 - (date.getDay() + 6) % 7);
    // January 4 is always in week 1.
    var week1 = new Date(date.getFullYear(), 0, 4);
    // Adjust to Thursday in week 1 and count number of weeks from date to week1.
    return 1 + Math.round(((date.getTime() - week1.getTime()) / 86400000
                          - 3 + (week1.getDay() + 6) % 7) / 7);
}

//function SetCss() {
//    $('input, textarea, select').each(
//  function () {

//      if (($(this).prop('readonly')) || ($(this).prop('type') == 'select-one') || ($(this).prop('type') == 'select-multiple')) {
//          $(this).addClass("has-content");
//      }
//      else {
//          var val = $(this).val().trim();
//          if (val.length) {
//              $(this).addClass("has-content");
//          }
//          else {
//              $(this).removeClass("has-content");
//          }
//      }
//  });
//}
function btnBrwEdit_Onclick(URL) {
    if (URL != null) {
        if (strValidate == "Y") {
            if (parseInt(objhdnID.value) == 0) {
                parent.PopupMessage(RootDirectory, strForm, "btnBrwEdit_Onclick()", "021", "true");
            }
            else {
                if(parent.GsIsBrowser == "Y") PreserveFilterValues();
                parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
                parent.PopupLoad();
                if (URL.indexOf("?") != -1) {
                    if (URL.indexOf("id") != -1)
                        parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&sid=" + SessionID + "&th=" + selTheme;
                    else
                        parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
                }
                else
                    parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
            }
        }
        else {
            if (parent.GsIsBrowser == "Y") PreserveFilterValues();
            parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
            parent.PopupLoad();
            if (URL.indexOf("?") != -1) {
                if (URL.indexOf("id") != -1)
                    parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID ;
                else
                    parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
            }
            else
                parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
        }
        
    }
    else {
        if(parent.GsIsBrowser == "Y") PreserveFilterValues();
        parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
        parent.PopupLoad();
        parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
    }
}
function btnBrwEditUI_Onclick(URL) {
    if (URL != null) {
        if (strValidate == "Y") {
            if (objhdnID.value == "00000000-0000-0000-0000-000000000000") {
                parent.PopupMessage(RootDirectory, strForm, "btnBrwEditUI_Onclick()", "021", "true");
            }
            else {
                if(parent.GsIsBrowser == "Y") PreserveFilterValues();
                parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
                parent.PopupLoad();
                if (URL.indexOf("?") != -1) {
                    if (URL.indexOf("id") != -1)
                        parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&sid=" + SessionID + "&th=" + selTheme;
                    else
                        parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
                }
                else
                    parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
            }
        }
        else {
            if (parent.GsIsBrowser == "Y") PreserveFilterValues();
            parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
            parent.PopupLoad();
            if (URL.indexOf("?") != -1) {
                if (URL.indexOf("id") != -1)
                    parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&sid=" + SessionID + "&th=" + selTheme;
                else
                    parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
            }
            else
                parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
        }
        
    }
    else {
        if(parent.GsIsBrowser == "Y") PreserveFilterValues();
        parent.GsIsBrowser = "N"; parent.GsRetStatus = "false";
        parent.PopupLoad();
        if (URL.indexOf("?") != -1) {
            if (URL.indexOf("id") != -1)
                parent.objiframePage.src = URL + + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&sid=" + SessionID;
            else
                parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
        }
        else
            parent.objiframePage.src = URL + "uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&sid=" + SessionID + "&th=" + selTheme;
    }
}
function btnBrwAdd_Onclick(URL) {
    if (URL != null) {
        if(parent.GsIsBrowser == "Y") PreserveFilterValues();
        parent.GsIsBrowser = "N";
        parent.PopupLoad();
        if (URL.indexOf("?") != -1) {
            parent.objiframePage.src = URL + +"&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=0&sid=" + SessionID + "&th=" + selTheme;
        }
        else
            parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=0&sid=" + SessionID + "&th=" + selTheme;
    }
}
function btnBrwAddUI_Onclick(URL) {
    if (URL != null) {
        if (parent.GsIsBrowser == "Y") PreserveFilterValues();
        parent.GsIsBrowser = "N";
        parent.PopupLoad();
        if (URL.indexOf("?") != -1) {
            parent.objiframePage.src = URL + "&uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=00000000-0000-0000-0000-000000000000&sid=" + SessionID + "&th=" + selTheme;
        }
        else
            parent.objiframePage.src = URL + "?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=00000000-0000-0000-0000-000000000000&sid=" + SessionID + "&th=" + selTheme;
    }
}
function btnBrwClose_Onclick() {
    parent.GsIsBrowser = "N"; parent.GsRetStatus = "false"; parent.GsFilter.length = 0;
    parent.PopupLoad();
    if (Unlock == "N") parent.LoadHome();
    else parent.LoadHome(UserID);
}
function btnDlgClose_Onclick() {
    parent.GsRetStatus = "false";
    if (parent.GsNavURL != "") {
        parent.GsIsBrowser = "Y";
        if (strLoadPopup == "Y") parent.PopupLoad();
        if (parent.GsNavURL.indexOf("?") != -1) parent.objiframePage.src = parent.GsNavURL + "&th=" + selTheme;
        else parent.objiframePage.src = parent.GsNavURL + "?th=" + selTheme;
    }
    else {
        parent.PopupLoad();
        parent.LoadHome();
    }
}
function ProcessConfirm(ArgsRet) {
    if (ArgsRet == "Y") {
        switch (parent.GsDlgConfAction) {
            case "NEW":
                btnBrwAdd_Onclick(parent.GsNavURL);
                break;
            case "NEWUI":
                btnBrwAddUI_Onclick(parent.GsNavURL);
                break;
            case "RES":
                strValidate = "N";
                if (InitialiseID == "Y") objhdnID.value = "0";
                btnBrwEdit_Onclick(parent.GsNavURL);
                break;
            case "RESUI":
                strValidate = "N";
                if (InitialiseID == "Y") objhdnID.value = "00000000-0000-0000-0000-000000000000";
                btnBrwEdit_Onclick(parent.GsNavURL);
                break;
            case "CLS":
                if (Unlock == "N") btnDlgClose_Onclick();
                else btnBrwClose_Onclick();
                break;
            case "FET":
                ProcessPostConfirm(ArgsRet);
                break;
            case "SAV":
                ProcessSave(ArgsRet);
                break;
            case "RELRPT":
                ReleaseReport(ArgsRet);
                break;
            case "APV":
                ApproveRecord();
                break;
            case "DEL":
                DeleteRecord();
                break;
            case "PROC":
                doProcessRecord(ArgsRet);
                break;
        }

    }
    else if (ArgsRet == "N") {
        switch (parent.GsDlgConfAction) {
            case "PROC":
                doProcessRecord(ArgsRet);
                break;
            case "SAV":
                ProcessSave(ArgsRet);
                break;
            case "FET":
                ProcessPostConfirm(ArgsRet);
                break;
        }
    }
    parent.GsDlgConfAction = "";
}
function ConvertToUpperCase(obj) {
    var objCtrlID = obj.id;
    document.getElementById(objCtrlID).value = parent.Trim(document.getElementById(objCtrlID).value.toUpperCase());
}
function ViewLog(menu_id) {
    if (parseInt(objhdnID.value) > 0) {
        parent.GsLaunchURL = "Common/TMSAuditTrail.aspx?record_id=" + objhdnID.value + "&mid=" + menu_id.toString();
        parent.PopupHelp();
    }
}
function ViewLogUI(menu_id) {
    if (objhdnID.value !="00000000-0000-0000-0000-000000000000") {
        parent.GsLaunchURL = "Common/TMSAuditTrailUI.aspx?record_id=" + objhdnID.value + "&mid=" + menu_id.toString();
        parent.PopupHelp();
    }
}
function DisableCode(objCtrl) {
    if (objCtrl == null) objCtrl = objtxtCode;
    if ((objhdnID.value != "0")) {
        objCtrl.readOnly = "readOnly";
        //objtxtCode.addClass("has-content");
        objCtrl.onkeydown = function (e) {
            if (!e) {
                e = event;
            }
            var Num;
            if (window.event) Num = e.keyCode;
            else if (e.which) Num = e.which;
            else Num = e.keyCode;

            if (Num == 8) {
                e.returnValue = false;
                e.cancelBubble = true;
                if (e.stopPropagation) { e.stopPropagation(); }
                if (e.preventDefault) { e.preventDefault();}
                return false;
            }
        };

    }
}
function DisableCodeUI(objCtrl) {
    if (objCtrl == null) objCtrl = objtxtCode;
    if ((objhdnID.value != "00000000-0000-0000-0000-000000000000")) {
        objCtrl.readOnly = "readOnly";
        //objtxtCode.addClass("has-content");
        objCtrl.onkeydown = function (e) {
            if (!e) {
                e = event;
            }
            var Num;
            if (window.event) Num = e.keyCode;
            else if (e.which) Num = e.which;
            else Num = e.keyCode;

            if (Num == 8) {
                e.returnValue = false;
                e.cancelBubble = true;
                if (e.stopPropagation) { e.stopPropagation(); }
                if (e.preventDefault) { e.preventDefault(); }
                return false;
            }
        };

    }
}