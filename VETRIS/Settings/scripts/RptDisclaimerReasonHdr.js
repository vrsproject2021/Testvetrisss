﻿function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[0].get_value();
    btnBrwEdit_Onclick("Settings/VRSRptDisclaimerReasonDlg.aspx");
}


