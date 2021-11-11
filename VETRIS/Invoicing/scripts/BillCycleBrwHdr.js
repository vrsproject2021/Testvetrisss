function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    SetGridValues();
}
function grdBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[0].get_value();
    btnBrwEditUI_Onclick("Invoicing/VRSBillCycleDlg.aspx");
}
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
function SetSelectedDate(CalName, objImgID) {
    var strDate = ""; var strClass = ""; var dt;
    if (CalName == "CalFromDate") var objCtrl = document.getElementById("txtFromDate");
    else if (CalName == "CalToDate") var objCtrl = document.getElementById("txtToDate");


    strDate = objCtrl.value;
    // grdRowID = RowId;
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
            CalFromDate.beginUpdate();
            CalFromDate.set_popUpExpandControlId(objImgID);
            CalFromDate.endUpdate();
            CalFromDate.SetSelectedDate(dt); CalFromDate.show();
            break;
        case "CalToDate":
            CalToDate.beginUpdate();
            CalToDate.set_popUpExpandControlId(objImgID);
            CalToDate.endUpdate();
            CalToDate.SetSelectedDate(dt); CalToDate.show();
            break;
        case "CalBillDate":
            CalBillDate.beginUpdate();
            CalBillDate.set_popUpExpandControlId(objImgID);
            CalBillDate.endUpdate();
            CalBillDate.SetSelectedDate(dt); CalBillDate.show();
            break;


    }
    parent.GsRetStatus = "true";
}

function SetGridValues() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var dtFrom = new Date(); var strDtFrom = "";
    var objtbFrom; var objimgDtFrom;
    var dtTill = new Date(); var strDtTill = "";
    var objtbTill; var objimgDtTill;
    var act = "";


    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        act = gridItem.get_cells()[4].get_value().toString();
        if (act == "Y") {
            document.getElementById("lock_" + RowId).classList.add('fa-lock');
        }
        else {
            document.getElementById("lock_" + RowId).classList.add('fa-unlock');
        }

        itemIndex++;
    }

    cb = "Y";

}
function ValidateDateEntered(objCtrl) {
    var strDate = "";
    strDate = document.getElementById(objCtrl.id).value;

    if (parent.ValidateDate(strDate))
        return true;
}