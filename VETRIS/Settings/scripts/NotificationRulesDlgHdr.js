function grdRad_onCallbackComplete(sender, eventArgs) {
    grdRad.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnRADCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdRad_onCallbackComplete()", strErr, "true");
    }

}
function grdRad_onRenderComplete(sender, eventArgs) {
    //parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var always = ""; var scheduled = ""; var rcpt_cnt = 0;
    

    while (gridItem = grdRad.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        scheduled = gridItem.get_cells()[3].get_value().toString();
        always = gridItem.get_cells()[4].get_value().toString();

        if (scheduled == "Y") {
            if (document.getElementById("chkRadSchedule_" + RowId.toString()) != null) {
                document.getElementById("chkRadSchedule_" + RowId.toString()).checked = true;
            }
        }
        if (always == "Y") {
            if (document.getElementById("chkRadAlways_" + RowId.toString()) != null) {
                document.getElementById("chkRadAlways_" + RowId.toString()).checked = true;
            }
        }
        itemIndex++;
    }
    parent.GsRetStatus = "false";


}


function grdMatrix_onCallbackComplete(sender, eventArgs) {
    grdMatrix.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdMatrix_onCallbackComplete()", strErr, "true");
    }

}
function grdMatrix_onRenderComplete(sender, eventArgs) {
    //parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var HdrRowId = "0";
    var Role = ""; var scheduled = ""; var rcpt_cnt = 0;
    var selected = ""; var selCount = 0;
    var ItemRecCount = 0; var RowId = "0";

    while (gridItem = grdMatrix.get_table().getRow(itemIndex)) {
        HdrRowId = gridItem.get_cells()[0].get_value().toString();
        Role = gridItem.get_cells()[1].get_value().toString();
        scheduled = gridItem.get_cells()[3].get_value().toString();
        rcpt_cnt = gridItem.get_cells()[5].get_value().toString();

        if (Role == "RDL") {
            if (document.getElementById("chkSchedule_" + HdrRowId.toString()) != null) {
                document.getElementById("chkSchedule_" + HdrRowId.toString()).style.display = "inline";
                if (scheduled == "Y") {
                    document.getElementById("chkSchedule_" + HdrRowId.toString()).checked = true;
                    document.getElementById("chkNotifyAll_" + HdrRowId.toString()).checked = false;
                    document.getElementById("chkNotifyAll_" + HdrRowId.toString()).disabled = true;
                }
            }
        }
        else {
            if (document.getElementById("chkSchedule_" + HdrRowId.toString()) != null) {
                document.getElementById("chkSchedule_" + HdrRowId.toString()).style.display = "none";
            }
        }

        //if (parseInt(rcpt_cnt) > 0) {
        //    if (document.getElementById("lnkCnt_" + HdrRowId.toString()) != null) {
        //        document.getElementById("lnkCnt_" + HdrRowId.toString()).style.display = "inline";
        //    }
            
        //}
        //else {
        //    if (document.getElementById("spnCnt_" + HdrRowId.toString()) != null) {
        //        document.getElementById("spnCnt_" + HdrRowId.toString()).style.display = "inline";
        //    }
        //}


        if (gridItem.Data.length > 6) {
            ItemRecCount = gridItem.Data[6].length;
            selCount = 0;
            for (var i = 0; i < ItemRecCount; i++) {

                RowId = gridItem.Data[6][i][0].toString();
                selected = gridItem.Data[6][i][4].toString();

                if (selected == "Y") {
                    selCount = selCount + 1;
                    if (document.getElementById("chkSel_" + RowId.toString()) != null) {
                        document.getElementById("chkSel_" + RowId.toString()).checked = true;
                    }
                }

            }
            if (selCount == ItemRecCount) {
                if (document.getElementById("chkNotifyAll_" + HdrRowId.toString()) != null) {
                    document.getElementById("chkNotifyAll_" + HdrRowId.toString()).checked = true;
                }
                gridItem.Data[4]="Y";
            }
        }
        itemIndex++;
    }
    parent.GsRetStatus = "false";


}
function grdMatrix_onItemExpand(sender, eventArgs) {
    parent.adjustFrameHeight();
}
function grdMatrix_onItemCollapse(sender, eventArgs) {
    parent.adjustFrameHeight();
}