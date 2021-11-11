$(document).ready($(function () {
    LoadRecord();
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

}
function LoadRecord() {
    //parent.PopupProcess("N");
    var ArrRecords = new Array();

    try {

        ArrRecords[0] = MenuID;
        ArrRecords[1] = UserID;
        CallBackFreeCredit.callback(ArrRecords);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "LoadRecord()", expErr.message, "true");
    }
}

function txtCredit_OnChange(ID) {
    var itemIndex = 0; var gridItem; var RowId = "0";

    while (gridItem = grdFreeCredit.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[1].toString();

        if (RowId == ID) {
            gridItem.Data[3] = document.getElementById("txtCredit_" + RowId).value;
            break;
        }
        itemIndex++;
    }
    parent.GsRetStatus = "true";
}

function btnClose_OnClick() {
    Unlock = "Y";
    btnBrwClose_Onclick();
}