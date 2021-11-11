var GiDecimal = parseInt(objhdnDecPlaces.value); var GsDateFormat = objhdnDateFormat.value; var GsDateSep = objhdnDateSep.value; var GsNavURL = "";
var GsIsBrowser = "Y"; var GsLaunchURL = ""; var GsFilePath = ""; var GsFileType = ""; 
var GsRetStatus = "false"; var GsHelpID = ""; var GsText = ""; var GsLogout = "N"; var GsPopupText = "";
var GcDivider = objhdnDivider.value; var GsFilter = new Array(); var GsConfirmAction = ""; var GsDlgConfAction = "";
var GsStoredValue = new Array(); var GsGlobalTempValue = new Array();
var GsText = ""; var GsTaskOverdue = "X"; var GsAssignType = "B"; var GsDayRange = ""; var GsStat = "ALL"; var GsApproved = "A"; 

function getElement(aID) {

    return (document.getElementById) ? document.getElementById(aID) : document.all[aID];
}
function getIFrameDocument(aID) {
    var rv = null;
    var frame = getElement(aID);
    // if contentDocument exists, W3C 

    //compliant(e.g.Mozilla)
    if (frame.contentDocument)
        rv = frame.contentDocument;
    else // bad Internet Explorer  ;)
        rv = document.frames[aID].document;
    return rv;
}
function adjustFrameHeight() {
    var frame = getElement("iframePage");
    var divBody = getElement("divMenu");
    var scrollHeight = divBody.scrollHeight;
    var frameDoc = getIFrameDocument("iframePage");
   
    //offsetHeight = offsetHeight -123;
    var offsetHeight = divBody.offsetHeight;
    //alert(offsetHeight);
    if (frameDoc.body.scrollHeight > scrollHeight) {
        frame.style.height = frameDoc.body.scrollHeight.toString() + "px";
        //divBody.height = frameDoc.body.offsetHeight + 20;
    }
    else {
        frame.style.height = scrollHeight.toString() + "px";
        //divBody.height = "550px";
    }
    //doLoad();
}

function Trim(str) {
    while (str.charAt(0) == (" ")) {
        str = str.substring(1);
    }
    while (str.charAt(str.length - 1) == " ") {
        str = str.substring(0, str.length - 1);
    }
    return str;
}
function CheckInteger(e) {
    try {
        var Num; var keyChar; var NumCheck;
        if (window.event) Num = e.keyCode;
        else if (e.which) Num = e.which;
        else if ((e.which) == undefined) Num = e.keyCode;
        keyChar = String.fromCharCode(Num);
        NumCheck = /\d/;
        return NumCheck.test(keyChar);
    }
    catch (err) {
        if (err.description == "undefined") return true;
        return false;
    }
}
function ResetValueInteger(objCtrlID, strDefaultValue) {
    var objCtrl;
    objCtrl = document.getElementById(objCtrlID.id);
    if (objCtrl == null) objCtrl = objCtrlID;
    if (strDefaultValue != null) {
        if (objCtrl.value == "" || parseInt(objCtrl.value) <= 0) {
            objCtrl.value = strDefaultValue;
        }
    }
    else {
        if (objCtrl.value == "") objCtrl.value = "0";
    }
}
function CheckDecimal(e) {
    try {
        var Num; var keyChar; var NumCheck;

        if (window.event) Num = e.keyCode;
        else if (e.which) Num = e.which;
        else Num = e.keyCode;
        //alert(Num);
        // keyChar = String.fromCharCode(Num);
        if ((Num >= 48 && Num <= 57) || (Num == 46) || (Num == 8) || (Num == 9)) {
            return true;
        }
        return false;
    }
    catch (err) {
        if (err.description == "undefined")
            return true;
        return false;
    }
}
function CheckDecimalNegative(e) {
    try {
        var Num; var keyChar; var NumCheck;

        if (window.event) Num = e.keyCode;
        else if (e.which) Num = e.which;
        else Num = e.keyCode;
        //alert(Num);
        // keyChar = String.fromCharCode(Num);
        if ((Num >= 48 && Num <= 57) || (Num == 45) || (Num == 46) || (Num == 8) || (Num == 9)) {
            return true;
        }
        return false;
    }
    catch (err) {
        if (err.description == "undefined")
            return true;
        return false;
    }
}
function ResetValueDecimal(objCtrlID) {

    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = SetDecimalFormat("0");
    else objCtrl.value = SetDecimalFormat(objCtrl.value);

}
function SetDecimalFormat(value,decimalPlace)//round the decimal 
{
    value = SetToNumber(value);
    if (decimalPlace == null) decimalPlace = 2; 

    return value.toFixed(decimalPlace);
}
function SetToNumber(ctrlValue)//Number Setting
{

    var value = ctrlValue.toString();
    value = value.replace(/,/g, ""); //replace [,]
    if (value == '' || isNaN(value)) value = '0';
    return parseFloat((value));
}
function padZeroPlaces(num) {
    return (num < 10) ? '0' + num : num;
}
function SetDateFormat(DateValue, DateFormat, DateSeperator)//Date Format Set [ddmmmyyyy for database save]
{

    DateFormat = DateFormat.toLowerCase(); var sReturn = "";

    var iDate = DateName(DateValue, DateFormat, DateSeperator, "d");
    var iMonth = DateName(DateValue, DateFormat, DateSeperator, "m");
    var iYear = DateName(DateValue, DateFormat, DateSeperator, "y");

    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    sReturn = padZeroPlaces(iDate).toString() + monthName[iMonth - 1] + iYear.toString();
    return sReturn;
}
function SetDateFormat1(DateValue, DateFormat, DateSeperator)//Date Format Set [ddmmmyyyy for database save]
{

    DateFormat = DateFormat.toLowerCase(); var sReturn = "";

    var iDate = DateName(DateValue, DateFormat, DateSeperator, "d");
    var iMonth = DateName(DateValue, DateFormat, DateSeperator, "m");
    var iYear = DateName(DateValue, DateFormat, DateSeperator, "y");

    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    sReturn = monthName[iMonth - 1] + " " + iDate.toString() + "," + iYear.toString();
    return sReturn;
}
function DateName(dtStr, dtFormat, sepChar, datePart) {
    if (Trim(dtStr) == '') return '';

    var daysInMonth = DaysArray(12);
    var dtCh = sepChar;

    var posF1 = dtFormat.indexOf(dtCh);
    var posF2 = dtFormat.indexOf(dtCh, posF1 + 1);

    var posD1 = dtStr.indexOf(dtCh);
    var posD2 = dtStr.indexOf(dtCh, posD1 + 1);

    var dtPart1 = dtFormat.substring(0, posF1);
    var dtPart2 = dtFormat.substring(posF1 + 1, posF2);
    var dtPart3 = dtFormat.substring(posF2 + 1);

    if (dtPart1.charAt(0).toLowerCase() == datePart)
        strValue = dtStr.substring(0, posD1);
    else if (dtPart2.charAt(0).toLowerCase() == datePart)
        strValue = dtStr.substring(posD1 + 1, posD2);
    else
        strValue = dtStr.substring(posD2 + 1);

    if (strValue.charAt(0) == "0" && strValue.length > 1) strValue = strValue.substring(1);

    if (datePart.toLowerCase() == "y")
        for (var i = 1; i <= 3; i++) {
            if (strValue.charAt(0) == "0" && strValue.length > 1) strValue = strValue.substring(1);
        }

    strValue = parseInt(strValue);

    return strValue;
}
function DaysArray(n) {
    for (var i = 1; i <= n; i++) {
        this[i] = 31
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
        if (i == 2) { this[i] = 29 }
    }
    return this
}
function AppDateFormat(DateValue, DateFormat, DateSeperator)//Change Date Format as per Culture
{
    DateFormat = DateFormat.toLowerCase();

    var iDate = DateName(DateValue, DateFormat, DateSeperator, "d");
    var iMonth = DateName(DateValue, DateFormat, DateSeperator, "m");
    var iYear = DateName(DateValue, DateFormat, DateSeperator, "y");

    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");

    var sTmp = DateFormat;
    sTmp = sTmp.replace("dd", "<e>");
    sTmp = sTmp.replace("d", "<d>");
    sTmp = sTmp.replace("<e>", padZeroPlaces(iDate));
    sTmp = sTmp.replace("<d>", iDate);
    sTmp = sTmp.replace("mmm", "<o>");
    sTmp = sTmp.replace("mm", "<n>");
    sTmp = sTmp.replace("m", "<m>");
    sTmp = sTmp.replace("<m>", iMonth);
    sTmp = sTmp.replace("<n>", padZeroPlaces(iMonth));
    sTmp = sTmp.replace("<o>", monthName[iMonth]);
    return sTmp.replace("yyyy", iYear);
}
function AppDateTimeFormat(DateValue, DateFormat, DateSeperator)//Change Date Format as per Culture
{
    DateFormat = DateFormat.toLowerCase();

    var iDate = DateName(DateValue, DateFormat, DateSeperator, "d");
    var iMonth = DateName(DateValue, DateFormat, DateSeperator, "m");
    var iYear = DateName(DateValue, DateFormat, DateSeperator, "y");

    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");

    var sTmp = DateFormat;
    sTmp = sTmp.replace("dd", "<e>");
    sTmp = sTmp.replace("d", "<d>");
    sTmp = sTmp.replace("<e>", padZeroPlaces(iDate));
    sTmp = sTmp.replace("<d>", iDate);
    sTmp = sTmp.replace("mmm", "<o>");
    sTmp = sTmp.replace("mm", "<n>");
    sTmp = sTmp.replace("m", "<m>");
    sTmp = sTmp.replace("<m>", iMonth);
    sTmp = sTmp.replace("<n>", padZeroPlaces(iMonth));
    sTmp = sTmp.replace("<o>", monthName[iMonth]);
    return sTmp.replace("yyyy", iYear) + " " + FormatTime(DateValue);
}
function FormatDate(iDate, iMonth, iYear, DateFormat, DateSeperator) {
    var monthName = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");

    var sTmp = DateFormat.toLowerCase();
    sTmp = sTmp.replace("dd", "<e>");
    sTmp = sTmp.replace("d", "<d>");
    sTmp = sTmp.replace("<e>", padZeroPlaces(iDate));
    sTmp = sTmp.replace("<d>", iDate);
    sTmp = sTmp.replace("mmm", "<o>");
    sTmp = sTmp.replace("mm", "<n>");
    sTmp = sTmp.replace("m", "<m>");
    sTmp = sTmp.replace("<m>", iMonth);
    sTmp = sTmp.replace("<n>", padZeroPlaces(iMonth));
    sTmp = sTmp.replace("<o>", monthName[iMonth - 1]);
    return sTmp.replace("yyyy", iYear);
}
function FormatTime(DateValue) {
    var dtDateTime = new Date(DateValue);
    var AMPM = "";
    var Hr = dtDateTime.getHours();
    //    if(LHr==0) LHr=12;else if(LHr>12) LHr=LHr-12;
    if (Hr > 12) AMPM = "PM"; else AMPM = "AM";
    var Min = dtDateTime.getMinutes();
    var Secs = dtDateTime.getSeconds();
    return padZeroPlaces(Hr) + ":" + padZeroPlaces(Min) + ":" + padZeroPlaces(Secs);
}
function CheckDate(LdtDate1, LdtDate2) {
    if (dtDate1 > dtDate2) return true; else return false;
}
function CheckTime(e) {
    try {
        var Num; var keyChar; var NumCheck;

        if (window.event) Num = e.keyCode;
        else if (e.which) Num = e.which;
        else Num = e.keyCode;
        //alert(Num);
        // keyChar = String.fromCharCode(Num);
        if ((Num >= 48 && Num <= 58) || (Num == 8) || (Num == 9)) {
            return true;
        }
        return false;
    }
    catch (err) {
        if (err.description == "undefined")
            return true;
        return false;
    }
}
function ValidateTimeInput(strInput) {
    var strRootDirectory = objhdnRootDirectory.value;
    var regs;
    // regular expression to match required time format
    var re = /^(\d{1,2}):(\d{2})([ap]m)?$/;
    if (Trim(strInput) != "") {
        if (regs = strInput.match(re)) {
            if (regs[3]) {
                // 12-hour value between 1 and 12
                if (regs[1] < 1 || regs[1] > 12) {
                    PopupMessage(strRootDirectory, "Common.js", "ValidateTimeInput()", "423", "true", "00-23", "", "TE");
                    return false;
                }
            } else {
                // 24-hour value between 0 and 23
                if (regs[1] > 23) {
                    PopupMessage(strRootDirectory, "Common.js", "ValidateTimeInput()", "423", "true", "00-23", "", "TE");
                    return false;
                }
            }
            // minute value between 0 and 59
            if (regs[2] > 59) {
                PopupMessage(strRootDirectory, "Common.js", "ValidateTimeInput()", "424", "true", "00-59", "", "TE");
                return false;
            }
        } 
        return true;
    }
    else {
        PopupMessage(strRootDirectory, "Common.js", "ValidateTimeInput()", "422", "true", "HH:MM", "", "TE");
        return false;
    }
}

