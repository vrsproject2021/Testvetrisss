var grdRowID = "0"; var cb = "N";
function grdInst_onCallbackComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrInst").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdInst_onCallbackComplete()", strErr, "true");
    }
}
function grdInst_onRenderComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    parent.adjustFrameHeight();
    
}
function grdContact_onCallbackComplete(sender, eventArgs) {
    grdContact.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrCont").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdContact_onCallbackComplete()", strErr, "true");
    }
}
function grdContact_onRenderComplete(sender, eventArgs) {
    grdContact.Width = "99%";
    parent.adjustFrameHeight();
   
}

function grdPhys_onCallbackComplete(sender, eventArgs) {
    grdPhys.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBPhysErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdPhys_onCallbackComplete()", strErr, "true");
    }

}
function grdPhys_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    parent.GsRetStatus = "false";
}
function grdPhys_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdPhys_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}

