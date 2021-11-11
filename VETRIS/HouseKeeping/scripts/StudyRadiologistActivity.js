$(document).ready($(function () {
    window.scrollTo(0, 0);
}))

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
    if (parent.Trim(objtxtSUID.value) != "") btnOK_Onclick();
}

function btnOK_Onclick() {
    var ArrRecords = new Array();
   
    ArrRecords[0] = objtxtSUID.value;
    ArrRecords[1] = UserID;
    ArrRecords[2] = SessionID;
    CallBackBrw.callback(ArrRecords);
}
function btnClose_Onclick()
{
    if (parent.Trim(objhdnCF.value) == "") {
        btnBrwClose_Onclick();
    }
    else {
        parent.GsRetStatus = "false";
        switch (objhdnCF.value) {
            case "21":
                parent.NavMenu("CaseList/VRSInProgressBrw.aspx", objhdnCF.value, "Y",1,"Y");
                break;
            case "22":
                parent.NavMenu("CaseList/VRSCasePrelimBrw.aspx", objhdnCF.value, "Y", 1, "Y");
                break;
            case "23":
                parent.NavMenu("CaseList/VRSCaseFinalBrw.aspx", objhdnCF.value, "Y", 1, "Y");
                break;
            case "24":
                parent.NavMenu("CaseList/VRSCaseArchiveBrw.aspx", objhdnCF.value, "Y", 1, "Y");
                break;
            case "76":
                parent.NavMenu("CaseList/VRSCaseFinalRptBrw.aspx", objhdnCF.value, "Y", 1, "Y");
                break;
        }
        
    }

}

