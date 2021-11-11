$(document).ready($(function () {
    parent.adjusDataListFrameHeight();
    if (document.all) {
        objlblConfMsg.innerText = parent.GsText;
    }
    else {
        objlblConfMsg.textContent = parent.GsText;
    }
}))

function btnSubmit_OnClick() {
    var strOpt = "";

    if (objrdoSTAT.checked) strOpt = "10";
    else if (objrdoSTD.checked) strOpt = "20";
    else if (objrdoCanc.checked) strOpt = "0";

    if (strOpt == "") {
        objdivMsg.innerHTML = "<font color='red'>Please select one of the options </font>";
    }
    else {
        parent.HideDataList(strOpt);
    }
}