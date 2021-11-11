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
        objhdnTotalRecords.value = gridItem.get_cells()[25].get_value().toString();
    }
    else {
        objhdnTotalRecords.value = "0";
    }
    if (document.all) document.getElementById("spnTotRecs").innerText = objhdnTotalRecords.value;
    else document.getElementById("spnTotRecs").textContent = objhdnTotalRecords.value;
    Render_Paging();
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    var itemIndex = 0; var gridItem;
    var RowId = ""; var Rating = ""; var FinalRptRel = ""; var FinalRptRelBy = "";
    var StatusID = 0; var RecViaDR = ""; var IsArchive = ""; var ShowDL = ""; var mTeach = ""; var LogAvailable = "";
    var Rad = ""; var ApprovedBy = "";
    var LoggedRadName = parent.objhdnUserName.value;
    var RadFnRights = objhdnRadFnRights.value;

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId         = gridItem.get_cells()[0].get_value().toString();
        Rad           = gridItem.get_cells()[8].get_value().toString();
        ApprovedBy    = gridItem.get_cells()[9].get_value().toString();
        StatusID      = parseInt(gridItem.Data[10].toString());
        Rating        = gridItem.get_cells()[19].get_value().toString();
        FinalRptRel   = gridItem.get_cells()[21].get_value().toString();
        FinalRptRelBy = gridItem.get_cells()[22].get_value().toString();
        RecViaDR      = gridItem.get_cells()[23].get_value().toString();
        IsArchive     = gridItem.get_cells()[24].get_value().toString();
        ShowDL        = gridItem.get_cells()[29].get_value().toString();
        mTeach        = gridItem.get_cells()[31].get_value().toString();
        LogAvailable  = gridItem.get_cells()[32].get_value().toString();

        if ((UserRoleCode == "RDL") || (UserRoleCode == "SYSADMIN") || (UserRoleCode == "SUPP")) {
            if (document.getElementById("btnImgViewer_" + RowId) != null) {
                document.getElementById("btnImgViewer_" + RowId).style.display = "inline";
                if (StatusID > 80)
                {
                    if (Rating == "A") {
                        if (document.getElementById("btnCompareRed_" + RowId) != null)
                            document.getElementById("btnCompareRed_" + RowId).style.display = "inline";
                    }
                    else {
                        if (document.getElementById("btnCompareGreen_" + RowId) != null)
                            document.getElementById("btnCompareGreen_" + RowId).style.display = "inline";
                    }
                }

                if (StatusID >=80) {
                    if ((UserRoleCode == "RDL")) {
                        if (RadFnRights.indexOf("UPDFINALRPT") == -1) {
                            if (document.getElementById("btnEditRpt_" + RowId) != null)
                                document.getElementById("btnEditRpt_" + RowId).style.display = "none";
                        }
                        if (RadFnRights.indexOf("RERATE") == -1) {
                            if (document.getElementById("spnRateReason_" + RowId) != null) {
                                document.getElementById("spnRateReason_" + RowId).style.display = "inline";
                                document.getElementById("btnEditRateRpt_" + RowId).style.display = "none";
                            }
                        }
                        else {
                            if (document.getElementById("spnRateReason_" + RowId) != null) {
                                if (Rad == ApprovedBy) {
                                    document.getElementById("spnRateReason_" + RowId).style.display = "inline";
                                    document.getElementById("btnEditRateRpt_" + RowId).style.display = "none";
                                }
                                else {
                                  
                                    if (LoggedRadName != ApprovedBy) {
                                        document.getElementById("spnRateReason_" + RowId).style.display = "inline";
                                        document.getElementById("btnEditRateRpt_" + RowId).style.display = "none";
                                       
                                    }
                                    else {
                                        document.getElementById("spnRateReason_" + RowId).style.display = "none";
                                        document.getElementById("btnEditRateRpt_" + RowId).style.display = "inline";
                                    }
                                }
                            }
                            
                        }
                    }
                    else {
                        if (document.getElementById("spnRateReason_" + RowId) != null) {
                            document.getElementById("spnRateReason_" + RowId).style.display = "inline";
                            document.getElementById("btnEditRateRpt_" + RowId).style.display = "none";
                        }
                    }
                }

                if (mTeach == "Y") {
                    if (document.getElementById("btnTeach_" + RowId) != null)
                        document.getElementById("btnTeach_" + RowId).style.display = "inline";
                }
                
            }
        }

        if (StatusID >0) {
            if ((UserRoleCode == "IU") || (UserRoleCode == "AU")) {
                if (document.getElementById("btnActivity_" + RowId) != null) document.getElementById("btnActivity_" + RowId).style.display = "none";
            }
            else {
                if (LogAvailable == "N") document.getElementById("btnActivity_" + RowId).style.display = "none";
            }
        }

        if (UserRoleCode != "RDL" && StatusID==100) {
            if (document.getElementById("btnFwd_" + RowId) != null) {
                if (FinalRptRel == "Y" || FinalRptRelBy != "00000000-0000-0000-0000-000000000000") document.getElementById("btnFwd_" + RowId).style.display = "inline";
                else document.getElementById("btnRelease_" + RowId).style.display = "inline";
            }
        }

        if ((StatusID < 80) && (IsArchive=="Y")) {
            if (document.getElementById("btnRpt_" + RowId) != null) {
                document.getElementById("btnRpt_" + RowId).style.display = "none";
                if (RecViaDR == "Y" || RecViaDR == "M") document.getElementById("btnImg_" + RowId).style.display = "none";
                document.getElementById("btnFwd_" + RowId).style.display = "none";
                document.getElementById("btnRevert_" + RowId).style.display = "inline";
                document.getElementById("btnEditRpt_" + RowId).style.display = "none";
            }
        }
        if (document.getElementById("btnDLImg_" + RowId) != null) {
            if (ShowDL == "N") document.getElementById("btnDLImgDsbl_" + RowId).style.display = "inline";
            else if (ShowDL == "Y") document.getElementById("btnDLImg_" + RowId).style.display = "inline";
        }
        //if (IsArchive == "Y") {
        //    if ((UserRoleCode == "SYSADMIN") || (UserRoleCode == "SUPP")) {
        //        if (document.getElementById("btnDel_" + RowId) != null)
        //            document.getElementById("btnDel_" + RowId).style.display = "inline";
        //    }
        //}
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