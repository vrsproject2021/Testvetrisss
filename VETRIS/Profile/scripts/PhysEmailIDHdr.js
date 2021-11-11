var grdRowID = "0"; var cb = "N"; var DEVADD = "N"; var EMAILADD = "N"; var SPADD = "N"; var USERADD = "N";


function grdEmail_onCallbackComplete(sender, eventArgs) {
    grdEmail.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        objdivMsg.innerHTML = "<font color='red'>" + strErr + "</font>";
        parent.adjusDataListFrameHeight();
    }
}
function grdEmail_onRenderComplete(sender, eventArgs) {
    grdEmail.Width = "99%";
    parent.adjusDataListFrameHeight();

    var rc = grdEmail.get_recordCount();

    if (rc >= 3) objbtnAdd.style.display = "none";
    else objbtnAdd.style.display = "inline";
    
    if (EMAILADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    EMAILADD = "N";
}
