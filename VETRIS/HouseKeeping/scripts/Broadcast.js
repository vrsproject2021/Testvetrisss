var strBroadcastFLAG = "E";
$(document).ready(function () {
    CheckError();
    //objrdoSMS.style.display = "none";
    $("#dvSMS").css('display', 'none');
   
});
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
    SearchRecord();
}
function ddlInstitution_OnChange(id) {

    if (id != "-1") {
        SearchRecord();

    }

}

function SearchRecord() {

    CallBackInst.callback(UserID, objddlInstitution.value, strBroadcastFLAG);
   // ConfigureGridColumn(strBroadcastFLAG);
}

function rdoMessage_OnCheckedChange(e) {
    strBroadcastFLAG = e;
    if (strBroadcastFLAG === 'E') {
        strBroadcastFLAG="E"
        $("#dvSMS").css('display', 'none');
        $("#dvEmail").css('display', 'block');
    }
    else if (strBroadcastFLAG === 'S') {
        strBroadcastFLAG = "S"
        $("#dvSMS").css('display', 'block');
        $("#dvEmail").css('display', 'none');
    }

    
    CallBackInst.callback(UserID, objddlInstitution.value, strBroadcastFLAG);
    ConfigureGridColumn(strBroadcastFLAG);
}

function chkSelect_Onclick(ID) {
    var RowId       = "00000000-0000-0000-0000-000000000000";
    var itemIndex   = 0;
    var gridItem;
    if (RowId != ID) {
        while (gridItem = grdInst.get_table().getRow(itemIndex)) {
            RowId = gridItem.get_cells()[0].get_value().toString();

            if (RowId == ID) {
                if (document.getElementById("chkSelect_" + RowId).checked)
                    gridItem.Data[1] = "Y";
                else
                    gridItem.Data[1] = "N";

                break;
            }
            itemIndex++;
        }
    }
    else if (RowId == ID) {
        if (document.getElementById("chkSelect").checked) {
            while (gridItem = grdInst.get_table().getRow(itemIndex)) {

                RowId = gridItem.get_cells()[0].get_value().toString();

                //document.getElementById("chkSelect_" + RowId).checked = true
                if (strBroadcastFLAG === 'E' && gridItem.get_cells()[3].get_value().toString() !="")
                    gridItem.Data[1] = "Y";
                else if (strBroadcastFLAG === 'S' && gridItem.get_cells()[4].get_value().toString() != "")
                    gridItem.Data[1] = "Y";

                itemIndex++;
            }
            document.getElementById("chkSelect").checked = true;
            ck="Y"
        }
        else {
            while (gridItem = grdInst.get_table().getRow(itemIndex)) {

                RowId = gridItem.get_cells()[0].get_value().toString();

                //document.getElementById("chkSelect_" + RowId).checked = true
                gridItem.Data[1] = "N";

                itemIndex++;
            }
            document.getElementById("chkSelect").checked = false;
            ck = "N";
        }
    }
}

function btnSend_OnClick() {

    parent.PopupProcess("Y");
    var ArrRecords = new Array();
    var arrParams = new Array();


    try {
        arrParams[0] = UserID;
        arrParams[1] = MenuID;
        arrParams[2] = objtxtEmailSubject.value;
        arrParams[3] = objtxtEmailBody.value;
        arrParams[4] = objtxtSMS.value;
        arrParams[5] = strBroadcastFLAG;

        ArrRecords = GetRecords();

        AjaxPro.timeoutPeriod = 1800000;
        VRSBroadcast.SendMessage(ArrRecords, arrParams, ShowProcess);


    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSend_OnClick()", expErr.message, "true");
    }

}

function GetRecords() {
    var itemIndex   = 0;
    var idx         = 0;
    var checked     = "";
    var gridItem;
    var arrRecords  = new Array(); 
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        checked = gridItem.Data[1].toString();
        if (checked == "Y") {
            arrRecords[idx]     = gridItem.Data[0].toString();
            arrRecords[idx + 1] = gridItem.Data[2].toString();
            arrRecords[idx + 2] = gridItem.Data[3].toString();
            arrRecords[idx + 3] = gridItem.Data[4].toString();
            idx = idx + 4;
        }
        itemIndex++;
    }
    return arrRecords;
}
function SendMessage(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    var cnt = 0;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveMessage()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveMessage()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.GsRetStatus = "false";
            parent.PopupMessage(RootDirectory, strForm, "SaveMessage()", arrRes[1], "false");
           
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SendMessage":
            SendMessage(Result);
            break;
    }

}

function btnReset_Onclick() {

    objtxtEmailSubject.value    = "";
    objtxtEmailBody.value       = "";
    objtxtSMS.value             = "";
}