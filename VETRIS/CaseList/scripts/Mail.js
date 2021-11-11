
$(document).ready($(function () {
    parent.adjusDataListFrameHeight();
    CheckError();
}))
function CheckError() {
    var arrErr = new Array();
    if (parent.Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                objdivMsg.innerHTML = "<font color='red'>" + arrErr[1] + "</font>";
                break;
        }
    }

    objhdnError.value = "";
    parent.adjusDataListFrameHeight();
}