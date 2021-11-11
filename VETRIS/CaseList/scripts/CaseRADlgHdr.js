var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
function grdST_onCallbackComplete(sender, eventArgs) {
    grdST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdST_onCallbackComplete()", strErr, "true");
    }
}
function grdST_onRenderComplete(sender, eventArgs) {
    grdST.Width = "99%";
    //parent.adjustFrameHeight();
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

    if (document.getElementById("hdnModTrackBy").value == "I") { objtxtImgCnt.style.display = "inline"; objtxtObjCnt.style.display = "none"; }
    else if (document.getElementById("hdnModTrackBy").value == "O") { objtxtImgCnt.style.display = "none"; objtxtObjCnt.style.display = "inline"; }
    else { objtxtImgCnt.style.display = "inline"; objtxtObjCnt.style.display = "inline"; }
}
function grdSelST_onCallbackComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSelST_onCallbackComplete()", strErr, "true");
    }
}
function grdDCM_onCallbackComplete(sender, eventArgs) {
    grdDCM.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrDCM").value);
}
function grdDCM_onRenderComplete(sender, eventArgs) {
    if (grdDCM.recordNumber != null) rc = grdDCM.get_recordCount();
    else if (grdDCM.RecordCount != null) rc = grdDCM.get_recordCount();

    if (rc > 0) {
        var e = $('#aRDCMCollapse').closest(".searchSection"),
                     t = $('#aRDCMCollapse').find("i"),
                     n = e.find("#divDCMUpload");
        if ($('#aRDCMCollapse>i').attr('class') == "fa fa-chevron-down") {
            e.attr("style") ? n.slideToggle(200, function () {
                e.removeAttr("style")
            }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
        }
    }
    else {
        var e = $('#aRDCMCollapse').closest(".searchSection"),
                     t = $('#aRDCMCollapse').find("i"),
                     n = e.find("#divDCMUpload");
        if ($('#aRDCMCollapse>i').attr('class') == "fa fa-chevron-up") {
            e.attr("style") ? n.slideToggle(200, function () {
                e.removeAttr("style")
            }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
        }
    }
}
function grdDoc_onCallbackComplete(sender, eventArgs) {
    grdDoc.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrDoc").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdDoc_onCallbackComplete()", strErr, "true");
    }
}
function grdDoc_onRenderComplete(sender, eventArgs) {
    //parent.adjustFrameHeight();
    var rc = 0;
    if (DOCADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";

    if (grdDoc.recordNumber != null) rc = grdDoc.get_recordCount();
    else if (grdDoc.RecordCount != null) rc = grdDoc.get_recordCount();

    if (rc > 0) {
        var e = $('#aRDOCCollapse').closest(".searchSection"),
                     t = $('#aRDOCCollapse').find("i"),
                     n = e.find("#divDocUpload");
        if ($('#aRDOCCollapse>i').attr('class') == "fa fa-chevron-down") {
            e.attr("style") ? n.slideToggle(200, function () {
                e.removeAttr("style")
            }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
        }
    }
    else {
        var e = $('#aRDOCCollapse').closest(".searchSection"),
                     t = $('#aRDOCCollapse').find("i"),
                     n = e.find("#divDocUpload");
        if ($('#aRDOCCollapse>i').attr('class') == "fa fa-chevron-up") {
            e.attr("style") ? n.slideToggle(200, function () {
                e.removeAttr("style")
            }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
        }
    }
}

function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    CalculateAge();
}

