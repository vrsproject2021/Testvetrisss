var grdRowID = "0"; var cb = "N"; var DOCADD = "N"; var REF_IMG = "N";
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
    var sel = ""; var strInvBy = "";

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

    if (document.getElementById("hdnModInvBy") != null) strInvBy = document.getElementById("hdnModInvBy").value;
    CallBackSelST.callback("L", objhdnID.value, objddlModality.value,objddlInstitution.value, strInvBy);

}
function grdSelST_onCallbackComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    if ((strErr != "") && (strErr != "UPDATE")) {
        parent.PopupMessage(RootDirectory, strForm, "grdSelST_onCallbackComplete()", strErr, "true");
    }
}
function grdSelST_onRenderComplete(sender, eventArgs) {
    grdSelST.Width = "99%";
    //var strUpdate = parent.Trim(document.getElementById("hdnCBErrSelST").value);
    //if (strUpdate == "UPDATE") {
    //    if(REF_IMG=="N") btnRefreshCount_OnClick();
    //}
    ResetPriorityList();
}
function grdSF_onCallbackComplete(sender, eventArgs) {
    grdSF.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
}
function grdSF_onRenderComplete(sender, eventArgs) {
    var rc = 0;
    if (grdSF.recordNumber != null) rc = grdSF.get_recordCount();
    else if (grdSF.RecordCount != null) rc = grdSF.get_recordCount();

    if (rc > 0) {
        var e = $('#aRUploadCollapse').closest(".searchSection"),
                     t = $('#aRUploadCollapse').find("i"),
                     n = e.find("#divUpload");
        if ($('#aRUploadCollapse>i').attr('class') == "fa fa-chevron-down") {
            e.attr("style") ? n.slideToggle(200, function () {
                e.removeAttr("style")
            }) : (n.slideToggle(200), e.css("height", "auto")), t.toggleClass("fa-chevron-up fa-chevron-down")
        }
    }
    else {
        var e = $('#aRUploadCollapse').closest(".searchSection"),
                     t = $('#aRUploadCollapse').find("i"),
                     n = e.find("#divUpload");
        if ($('#aRUploadCollapse>i').attr('class') == "fa fa-chevron-up") {
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

