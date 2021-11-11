var Divider = parent.objhdnDivider.value; var SecDivider = parent.objhdnSecDivider.value;
var RootDirectory = parent.objhdnRootDirectory.value; var ServerPath = parent.hdnServerPath.value;

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
                SlideDown();
                break;

        }
    }
    else
        CallBackStat.callback(objhdnID.value);

    objhdnError.value = "";
}
function SlideDown() {
    $('#divMsg').slideDown({
        complete: function () {
            parent.adjusDataListFrameHeight();
        }
    });
}
function SlideUp() {
    $('#divMsg').slideUp({
        complete: function () {
            parent.adjusDataListFrameHeight();
        }
    });
}
