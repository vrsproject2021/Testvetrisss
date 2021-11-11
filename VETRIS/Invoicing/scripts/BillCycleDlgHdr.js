function CalFromDate_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFromDate.getSelectedDate());
    document.getElementById("txtFromDate").value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    //PossessDate_OnChange(grdRowID);
}
function CalToDate_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalToDate.getSelectedDate());
    document.getElementById("txtToDate").value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    //TermDate_OnChange(grdRowID);
}
function SetSelectedDate(objCtrlID, CalName, LsImgID) {
    var strDate = ""; var strClass = "";
    var dt;
    objCtrl = document.getElementById(objCtrlID.id); if (objCtrl == null) objCtrl = objCtrlID;
    strDate = document.getElementById(objCtrl).value;

    if (parent.Trim(strDate) != "") {
        if (document.all) {
            dt = new Date(parent.SetDateFormat(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
        else {
            dt = new Date(parent.SetDateFormat1(strDate, parent.GsDateFormat, parent.GsDateSep));
        }
    }
    else
        dt = new Date();
    switch (CalName) {
        case "CalFromDate":
            CalFromDate.setSelectedDate(dt); CalFromDate.show();
            break;
        case "CalToDate":
            CalToDate.setSelectedDate(dt); CalToDate.show();
            break;
    }


}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}