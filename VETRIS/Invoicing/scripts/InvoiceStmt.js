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

function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSInvoiceStmt.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSInvoiceStmt.aspx';
        parent.PopupConfirm("028");
    }
}
function btnClose_OnClick() {
    Unlock = "N";
    btnBrwClose_Onclick();
}

function btnGen_OnClick() {
    
    var CycleID = objddlBillingCycle.value;
    var AccountID = objddlAccount.value;

    if (CycleID != "00000000-0000-0000-0000-000000000000") {
        parent.GsFileType = "PDF";
        parent.GsLaunchURL = "Invoicing/DocumentPrinting/VRSDocPrint.aspx?DocID=4&CYCLE=" + CycleID + "&ACCT=" + AccountID + "&UID=" + UserID;
        parent.PopupReportViewer();
    }
    else {
        parent.PopupMessage(RootDirectory, strForm, "btnGen_OnClick()", "229", "true");
    }
}



