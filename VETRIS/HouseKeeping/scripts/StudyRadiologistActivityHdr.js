function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "100%";
    parent.adjustFrameHeight();
    var strErr = "";
    if (document.getElementById("hdnCBErr") != null) strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }

    if(document.getElementById("hdnPDtls") != null) document.getElementById("spnPNM").innerHTML = parent.Trim(document.getElementById("hdnPDtls").value);
}

