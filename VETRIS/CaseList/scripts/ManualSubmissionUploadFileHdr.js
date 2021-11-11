var grdRowID = "0"; var cb = "N"; var DOCADD = "N";

function grdSF_onCallbackComplete(sender, eventArgs) {
    grdSF.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
}
function grdSF_onRenderComplete(sender, eventArgs) {
   
    
}



