function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }
    // - Paging setup
    var gridItem = grdBrw.get_table().getRow(0);
    if (gridItem) {
        objhdnTotalRecords.value = gridItem.get_cells()[24].get_value().toString();
    }
    else {
        objhdnTotalRecords.value = "0";
    }
    Render_Paging();
}
function grdBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    objhdnID.value = item.Cells[0].get_value();
    btnBrwEditUI_Onclick("CaseList/VRSCasePrelimDlg.aspx");
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    var itemIndex = 0; var gridItem;
    var RowId = ""; var Rating = ""; var FinalRptRel = ""; var FinalRptRelBy = "";
    var RadFnRights = objhdnRadFnRights.value;

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.get_cells()[0].get_value().toString();
        Rating = gridItem.get_cells()[20].get_value().toString();
        FinalRptRel = gridItem.get_cells()[22].get_value().toString();
        FinalRptRelBy = gridItem.get_cells()[23].get_value().toString();

        if ((UserRoleCode == "RDL") || (UserRoleCode == "SYSADMIN") || (UserRoleCode == "SUPP")) {
            if (document.getElementById("btnImgViewer_" + RowId) != null) {
                document.getElementById("btnImgViewer_" + RowId).style.display = "inline";
                if (Rating == "A") {
                    if (document.getElementById("btnCompareRed_" + RowId) != null)
                        document.getElementById("btnCompareRed_" + RowId).style.display = "inline";
                }
                else {
                    if (document.getElementById("btnCompareGreen_" + RowId) != null)
                        document.getElementById("btnCompareGreen_" + RowId).style.display = "inline";
                }
                if ((UserRoleCode == "RDL") && (RadFnRights.indexOf("UPDFINALRPT") == -1)) {
                    if (document.getElementById("btnEditRpt_" + RowId) != null)
                        document.getElementById("btnEditRpt_" + RowId).style.display = "none";
                }
                if (UserRoleCode == "RDL") {
                    document.getElementById("btnArch_" + RowId).style.display = "none";
                }
            }
        }
        if ((UserRoleCode == "IU") || (UserRoleCode == "AU")) {
            if (document.getElementById("btnActivity_" + RowId) != null) document.getElementById("btnActivity_" + RowId).style.display = "none";
        }
        if (UserRoleCode != "RDL") {
            if (document.getElementById("btnFwd_" + RowId) != null) {
                if (FinalRptRel == "Y" || FinalRptRelBy != "00000000-0000-0000-0000-000000000000") document.getElementById("btnFwd_" + RowId).style.display = "inline";
                else document.getElementById("btnRelease_" + RowId).style.display = "inline";
            }
        }

        itemIndex++;
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

// - Paging --
function Render_Paging() {
    var ul = $(".pagination");
    ul.empty();
    var num_pages = parseInt(parseInt(objhdnTotalRecords.value) / parseInt(objhdnPageSize.value));
    var reminder = parseInt(objhdnTotalRecords.value) % parseInt(objhdnPageSize.value);
    if (reminder > 0) num_pages += 1;
    if (num_pages === 0) return;
    var current = parseInt(objhdnPageNo.value);
    var arr_pages = buildArr(current, num_pages);
    ul.append('<li class="page-item"><a class="page-link" href="javascript:loadPage(' + 1 +')">First</a></li>')
    arr_pages.forEach(item => {
        if (item !== "...") {
            ul.append('<li class="page-item ' + ((item === current) ? 'active' : '') +'"><a class="page-link" href="javascript:loadPage('+item+')">' + item + '</a></li>');
} else {

            ul.append('<li class="page-item"><a class="page-link" href="javascript:void">' + item + '</a></li>');
}
});
ul.append('<li class="page-item"><a class="page-link" href="javascript:loadPage(' + num_pages +')">Last</a></li>')

}

function loadPage(num) {
    objhdnPageNo.value = num.toString();
    SearchRecord();
}

function buildArr(c, n) {
    if (n <= 7) {
        return [...Array(n)].map((_, i) => i + 1);
    } else {
        if (c < 3 || c > n - 2) {
            return [1, 2, 3, "...", n - 2, n - 1, n];
        } else {
            return [1, "...", c - 1, c, c + 1, "...", n];
        }
    }
}
