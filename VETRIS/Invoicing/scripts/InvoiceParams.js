
var strControlCode = ""; var objCtrl;
parent.adjusDataListFrameHeight();

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
    else {
        CheckRights();
    }
    objhdnError.value = "";
    parent.adjusDataListFrameHeight();
}
function CheckRights() {
    var strRights = objhdnRights.value;
    if (strRights.substring(0, 1) == "Y") { objbtnSave1.style.display = "inline"; objbtnSave2.style.display = "inline"; }
}
function btnSave_OnClick() {
    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrCtrlList = new Array();
    try {

        ArrRecords[0] = UserID;
        ArrRecords[1] = MenuID;

        arrCtrlList = GetControlList();

        AjaxPro.timeoutPeriod = 1800000;
        VRSInvoiceParams.SaveRecord(ArrRecords, arrCtrlList, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }

}
function GetControlList() {
    var objrtbINVMAILTXT = document.getElementById("cke_contents_rtbINVMAILTXT").children[0];
    var arrCtrl = new Array();
    arrCtrl[0]  = "COMPADDR";    arrCtrl[1]  = objtxtCOMPADDR.value;    arrCtrl[2]  = "0";                      arrCtrl[3]  = "0"; arrCtrl[4]  = "CHAR"; arrCtrl[5]  = "txt";
    arrCtrl[6]  = "COMPNAME";    arrCtrl[7]  = objtxtCOMPNAME.value;    arrCtrl[8]  = "0";                      arrCtrl[9]  = "0"; arrCtrl[10] = "CHAR"; arrCtrl[11] = "txt";
    arrCtrl[12] = "DUEDTDAYS";   arrCtrl[13] = "";                      arrCtrl[14] = objtxtDUEDTDAYS.value;    arrCtrl[15] = "0"; arrCtrl[16] = "INT";  arrCtrl[17] = "txt";
    arrCtrl[18] = "FOOTTXT";     arrCtrl[19] = objtxtFOOTTXT.value;     arrCtrl[20] = "0";                      arrCtrl[21] = "0"; arrCtrl[22] = "CHAR"; arrCtrl[23] = "txt";
    arrCtrl[24] = "INVPRFX";     arrCtrl[25] = objtxtINVPRFX.value;     arrCtrl[26] = "0";                      arrCtrl[27] = "0"; arrCtrl[28] = "CHAR"; arrCtrl[29] = "txt";
    arrCtrl[30] = "OLPMTPWD";    arrCtrl[31] = objtxtOLPMTPWD.value;    arrCtrl[32] = "0";                      arrCtrl[33] = "0"; arrCtrl[34] = "CHAR"; arrCtrl[35] = "txt";
    arrCtrl[36] = "OLPMTUID";    arrCtrl[37] = objtxtOLPMTUID.value;    arrCtrl[38] = "0";                      arrCtrl[39] = "0"; arrCtrl[40] = "CHAR"; arrCtrl[41] = "txt";
    arrCtrl[42] = "OLPMTURL";    arrCtrl[43] = objtxtOLPMTURL.value;    arrCtrl[44] = "0";                      arrCtrl[45] = "0"; arrCtrl[46] = "CHAR"; arrCtrl[47] = "txt";
    arrCtrl[48] = "PAYINST";     arrCtrl[49] = objtxtPAYINST.value;     arrCtrl[50] = "0";                      arrCtrl[51] = "0"; arrCtrl[52] = "CHAR"; arrCtrl[53] = "txt";
    arrCtrl[54] = "STARTINVSRL"; arrCtrl[55] = "";                      arrCtrl[56] = objtxtSTARTINVSRL.value;  arrCtrl[57] = "0"; arrCtrl[58] = "INT";  arrCtrl[59] = "txt";
    arrCtrl[60] = "RADPMTSRL";   arrCtrl[61] = "";                      arrCtrl[62] = objtxtRADPMTSRL.value;    arrCtrl[63] = "0"; arrCtrl[64] = "INT";  arrCtrl[65] = "txt";
    arrCtrl[66] = "DEFCCMAILID"; arrCtrl[67] = objtxtDEFCCMAILID.value; arrCtrl[68] = "0";                      arrCtrl[69] = "0"; arrCtrl[70] = "CHAR"; arrCtrl[71] = "txt";
    arrCtrl[72] = "SENDMAILID";  arrCtrl[73] = objtxtSENDMAILID.value;  arrCtrl[74] = "0";                      arrCtrl[75] = "0"; arrCtrl[76] = "CHAR"; arrCtrl[77] = "txt";
    arrCtrl[78] = "SENDMAILPWD"; arrCtrl[79] = objtxtSENDMAILPWD.value; arrCtrl[80] = "0";                      arrCtrl[81] = "0"; arrCtrl[82] = "CHAR"; arrCtrl[83] = "txt";
    arrCtrl[84] = "TNTKNKEY";    arrCtrl[85] = objtxtTNTKNKEY.value;    arrCtrl[86] = "0";                      arrCtrl[87] = "0"; arrCtrl[88] = "CHAR"; arrCtrl[89] = "txt";
    arrCtrl[90] = "TNAPIKEY";    arrCtrl[91] = objtxtTNAPIKEY.value;    arrCtrl[92] = "0";                      arrCtrl[93] = "0"; arrCtrl[94] = "CHAR"; arrCtrl[95] = "txt";
    arrCtrl[96] = "OPMAILCC";    arrCtrl[97] = objtxtOPMAILCC.value;    arrCtrl[98] = "0";                      arrCtrl[99] = "0"; arrCtrl[100] = "CHAR";arrCtrl[101] = "txt";
    arrCtrl[102] = "INVMAILTXT"; arrCtrl[103] = objrtbINVMAILTXT.contentDocument.body.innerHTML;                arrCtrl[104] = "0"; arrCtrl[105] = "0"; arrCtrl[106] = "CHAR"; arrCtrl[107] = "rtb";
    arrCtrl[108] = "REFUNDDAYS"; arrCtrl[109] = ""; arrCtrl[110] = objtxtREFUNDDAYS.value; arrCtrl[111] = "0"; arrCtrl[112] = "INT"; arrCtrl[113] = "txt";
    arrCtrl[114] = "CALCMINUTEFACT"; arrCtrl[115] = ""; arrCtrl[116] = txtCALCMINUTEFACT.value; arrCtrl[117] = "0"; arrCtrl[118] = "INT"; arrCtrl[119] = "txt";
    return arrCtrl;
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false")
        btnBrwEdit_Onclick('Invoicing/VRSInvoiceParams.aspx');
    else {
        parent.GsDlgConfAction = "RES";
        parent.GsNavURL = "Invoicing/VRSInvoiceParams.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
        parent.PopupConfirm("030");
    }

}
function btnClose_OnClick() {
    parent.GsNavURL = "VRSHome.aspx";
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function ResetValueDecimal(objCtrlID, decimalPlace) {

    var objCtrl;
    if (decimalPlace == null) decimalPlace = parent.GiDecimal;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value, decimalPlace);

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
function ProcessMessage(ArgsRet) {
    if (ArgsRet == "TE") {

        objCtrl.focus();
    }
    objCtrl = null;
}
