var grdRowID = "0"; var cb = "N"; var DOCADD = "N";
function grdFiles_onCallbackComplete(sender, eventArgs) {
    grdFiles.Width = "98%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrFiles").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdFiles_onCallbackComplete()", strErr, "true");
    }
    
}
function grdFiles_onRenderComplete(sender, eventArgs) {
    grdFiles.Width = "99%";
    ////parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var sel = ""; var ImgCnt = 0;

    while (gridItem = grdFiles.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        sel = gridItem.Data[4].toString();

        if (sel == "Y") {

            if (document.getElementById("chkSel_" + RowId) != null) {
                document.getElementById("chkSel_" + RowId).checked = true;
            }
            ImgCnt = ImgCnt + 1;
        }

        itemIndex++;
    }
    //if (objtxtImgCnt != null) {
    //    objtxtImgCnt.value = ImgCnt.toString();
    //    if (ImgCnt == 0) {
    //        objhdnInstID.value = "00000000-0000-0000-0000-000000000000";
    //        objtxtInstName.value = "";
    //    }
    //}

}
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

            if (document.getElementById("chkStudySel_" + RowId) != null) {
                document.getElementById("chkStudySel_" + RowId).checked = true;
            }
        }

        itemIndex++;
    }

    if (document.getElementById("hdnModInvBy") != null) strInvBy = document.getElementById("hdnModInvBy").value;
    CallBackSelST.callback("L", objhdnID.value, objddlModality.value,objddlInstitution.value, strInvBy);
}
function grdSelST_onCallbackComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdSelST_onCallbackComplete()", strErr, "true");
    }
}
function grdSelST_onRenderComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    ResetPriorityList();
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

function CalDOS_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalDOS.getSelectedDate());
    objtxtDOS.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    CalculateAge();
}
function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
    CalculateAge();
}


