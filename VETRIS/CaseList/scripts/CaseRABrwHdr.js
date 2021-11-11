var PROMO_CLICK = "N"; var IMG_CLICK = "N";
function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
   // parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
   // parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var rec = ""; var Id = "";

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        Id = gridItem.Data[1].toString();
        rec = gridItem.Data[7].toString();

        if (rec == "Y") {
            
            if (document.getElementById("imgDR_" + RowId) != null) {
                document.getElementById("imgDR_" + RowId).style.display = "inline";
                document.getElementById("btnImg_" + RowId).style.display = "none";
            }
        }
        else if (rec == "M") {

            if (document.getElementById("imgMS_" + RowId) != null) {
                document.getElementById("imgMS_" + RowId).style.display = "inline";
                document.getElementById("btnImg_" + RowId).style.display = "none";
            }
        }
        
        itemIndex++;
    }
    parent.GsRetStatus = "false";
}
function grdBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[0].get_value();
    if (PROMO_CLICK == "N" && IMG_CLICK == "N") {
        btnBrwEditUI_Onclick("CaseList/VRSRADlgNew.aspx");
    }
    else {
        PROMO_CLICK = "N";
        IMG_CLICK = "N";
    }
}
function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
function CalTill_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalTill.getSelectedDate());
    objtxtTillDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
