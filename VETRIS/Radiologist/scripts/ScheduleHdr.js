var RENDER = "N";

function grdRAD_onCallbackComplete(sender, eventArgs) {
    grdRAD.Width = "98%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBRADErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdRAD_onCallbackComplete()", strErr, "true");
    }
   
}

function rowHeaderCellHtml(schedulerView, rowHeaderCell) {
    var htmlBuilder = new Array();
    var time = rowHeaderCell.Time;
    if (time.getHours() == 12 && time.getMinutes() == 0) {
        htmlBuilder.push('<div class="rowHeaderHour">Noon</div>');
    }
    else {
        //htmlBuilder.push('<div class="rowHeaderAmPmDesignator">');
        //htmlBuilder.push(schedulerView.DateTimeFormatter.toString(rowHeaderCell.Time, 'tt'));
        htmlBuilder.push('<div class="rowHeaderHour">');
        htmlBuilder.push(schedulerView.DateTimeFormatter.toString(rowHeaderCell.Time, 'hh:mm'));
        htmlBuilder.push('</div><div class="rowHeaderAmPmDesignator">');
        htmlBuilder.push(schedulerView.DateTimeFormatter.toString(rowHeaderCell.Time, 'tt'));
        htmlBuilder.push('</div>');
    }
    return htmlBuilder.join('');
}

function grdRAD_onRenderComplete(sender, eventArgs) {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = "";

    while (gridItem = grdST.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[2].toString();

        if (sel == "Y") {

            if (document.getElementById("chkSel_" + RowId) != null) {
                document.getElementById("chkSel_" + RowId).checked = true;
            }
        }

        itemIndex++;
    }

}
function schRad_onCallbackComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "schRad_onCallbackComplete()", strErr, "true");
    }
    else
        SetRadCss();
}
function schRad_OnCallbackError(sender, eventArgs) {
    var x = "";
}

function scheduler_OnAppointmentBeforeModify(sender, eventArgs) {
    //LoadScheduler();
    alert("Before Modify");

}
function scheduler_OnAppointmentModify(sender, eventArgs) {
    //LoadScheduler();
    alert("Modify");

}
function scheduler_OnCallBackComplete(sender, eventArgs) {
    //LoadScheduler();
    alert("hello");

}

function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
function CalTill_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalTill.getSelectedDate());
    objtxtTillDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}

function CalStart_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalStart.getSelectedDate());
    objtxtStartDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    DateChanged();
}
function CalEnd_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalEnd.getSelectedDate());
    objtxtEndDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    DateChanged();
}

function viewRad_AppointmentAddOpen(sender, eventArgs) {
    var Title = "Add a schedule";
    ShowScheduleDialog(sender, eventArgs, Title);
}
function viewRad_AppointmentModifyOpen(sender, eventArgs) {
    var Title = "Modify schedule";
    ShowScheduleDialog(sender, eventArgs, Title);
}


