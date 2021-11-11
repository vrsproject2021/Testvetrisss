function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    //parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }

}
function grdBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    var ID = item.Cells[0].get_value();
    var Menu_ID = item.Cells[10].get_value().toString();
    var Patient_Name = item.Cells[2].get_value();
    var InstID = item.Cells[8].get_value();

    parent.objhdnMenuID.value = Menu_ID;
    parent.GsFilter.length = 0;
    parent.PopupLoad();
    try {
        switch (Menu_ID) {
            case "20":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "";
                parent.GsFilter[2] = parent.Trim(InstID);
                if (objchkRecDt.checked) parent.GsFilter[3] = "Y"; else parent.GsFilter[3] == "N";
                parent.GsFilter[4] = objtxtFromDt.value;
                parent.GsFilter[5] = objtxtTillDt.value;
                parent.GsFilter[6] = "";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCaseRABrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + parent.GsTheme;
                break;
            case "21":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "-1";
                parent.GsFilter[7] = "0";
                parent.GsFilter[8] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[9] = "0";
                parent.GsFilter[10] = "0";
                parent.GsFilter[11] = "";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSInProgressBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + parent.GsTheme;
                break;
            case "22":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[7] = "0"
                parent.GsFilter[8] = "P";
                parent.GsFilter[9] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[10] = "";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCasePrelimBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + parent.GsTheme;
                break;
            case "23":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[7] = "0"
                parent.GsFilter[8] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[9] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[10] = "N";
                parent.GsFilter[11] = "00000000-0000-0000-0000-000000000000";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCaseFinalBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + parent.GsTheme;
                break;
            case "24":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                parent.GsFilter[2] = "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[7] = "0"
                parent.GsFilter[8] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[9] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[10] = "N";
                parent.GsFilter[11] = "00000000-0000-0000-0000-000000000000";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCaseArchiveBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + parent.GsTheme;
                break;
            case "76":
                parent.GsFilter[0] = parent.Trim(Patient_Name);
                parent.GsFilter[1] = "0";
                if (objchkRecDt.checked) parent.GsFilter[2] = "Y"; else parent.GsFilter[2] == "N";
                parent.GsFilter[3] = objtxtFromDt.value;
                parent.GsFilter[4] = objtxtTillDt.value;
                parent.GsFilter[5] = parent.Trim(InstID);
                parent.GsFilter[6] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[7] = "0"
                parent.GsFilter[8] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[9] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[10] = "N";
                parent.GsFilter[11] = "00000000-0000-0000-0000-000000000000";
                parent.GsFilter[12] = "N";
                parent.GsFilter[13] = "-1";
                parent.GsFilter[14] = "N";
                parent.GsFilter[15] = "";
                parent.GsIsBrowser = "Y";
                parent.objiframePage.src = "CaseList/VRSCaseFinalRptBrw.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + Menu_ID + "&th=" + parent.GsTheme;
                break;
        }
    }
    catch (expErr) {
        parent.HideLoad();
        parent.PopupMessage(RootDirectory, strForm, "btnNav_OnClick()", expErr.message, "true");
    }
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    var itemIndex = 0; var gridItem; var RowId = "0";
    parent.GsRetStatus = "false";
}
function CalFrom_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalFrom.getSelectedDate());
    objtxtFromDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
function CalTill_onSelectionChanged(sender, eventArgs) {
    var dt = new Date(CalTill.getSelectedDate());
    objtxtTillDt.value = parent.FormatDate(new Date(dt).getDate(), new Date(dt).getMonth() + 1, new Date(dt).getFullYear(), parent.GsDateFormat, parent.GsDateSep);
}
