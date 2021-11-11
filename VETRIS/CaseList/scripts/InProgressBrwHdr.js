function grdBrw_onCallbackComplete(sender, eventArgs) {
    grdBrw.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErr").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdBrw_onCallbackComplete()", strErr, "true");
    }

}
function grdBrw_onItemSelect(sender, eventArgs) {
    var item = eventArgs.get_item();
    var URL = item.Cells[12].get_value();
    URL = URL.replace("#V2", PACSUID);
    URL = URL.replace("#V3", PACSPwd);
    parent.NavigatePACS(URL);
}
function grdBrw_onRenderComplete(sender, eventArgs) {
    grdBrw.Width = "99%"; var rc = 0;
    var itemIndex = 0; var gridItem; var RowId = "0";
    var PriorityID = "0"; var ModalityID = "0"; var Locked = ""; var ShowDL = "";
    var UnderTrans = ""; var ReqTrans = ""; var StatusID = 0; var RadID = ""; var LogAvailable = "";
    var arrPriority = new Array();
    var RadFnRights = objhdnRadFnRights.value;

    if (parent.Trim(objhdnPriority.value) != "") {
        if (parent.Trim(objhdnPriority.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrPriority = parent.Trim(objhdnPriority.value).split(parent.objhdnDivider.value);
        }
    }

    while (gridItem = grdBrw.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        PriorityID = gridItem.Data[9].toString();
        StatusID = parseInt(gridItem.Data[15].toString());
        Locked = gridItem.Data[23].toString();
        UnderTrans = gridItem.Data[24].toString();
        ReqTrans = gridItem.Data[25].toString();
        RadID = gridItem.Data[33].toString();
        ShowDL = gridItem.Data[35].toString();
        LogAvailable = gridItem.Data[36].toString();

        if(StatusID ==50) rc = rc + 1;

        if (document.getElementById("ddlPriority_" + RowId) != null) {
            if (document.getElementById("ddlPriority_" + RowId).length == 0) {
                for (var i = 0; i < arrPriority.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrPriority[i];
                    op.text = arrPriority[i + 1];
                    document.getElementById("ddlPriority_" + RowId).add(op);
                }
            }
            document.getElementById("ddlPriority_" + RowId).value = PriorityID;
            SetPriorityList(gridItem);
        }

        if (document.getElementById("btnEditRpt_" + RowId) != null) {
            if (ShowDL == "N") document.getElementById("btnDLImgDsbl_" + RowId).style.display = "inline";
            else if (ShowDL == "Y") document.getElementById("btnDLImg_" + RowId).style.display = "inline";

            if (UserRoleCode == "RDL" || UserRoleCode == "SUPP" || UserRoleCode == "SYSADMIN") {
                
                if (Locked == "Y") {
                    if (document.getElementById("btnEditRpt_" + RowId) != null) {
                        document.getElementById("btnEditRpt_" + RowId).style.display = "none";
                        document.getElementById("btnLocked_" + RowId).style.display = "inline";
                    }
                }
                else {
                    if (document.getElementById("btnEditRpt_" + RowId) != null) {
                        document.getElementById("btnEditRpt_" + RowId).style.display = "inline";
                        document.getElementById("btnLocked_" + RowId).style.display = "none";
                        if (ReqTrans == "Y") {
                            if (document.getElementById("btnTrans_" + RowId).style.display == "none") {
                                document.getElementById("btnTransReq_" + RowId).style.display = "inline";
                                document.getElementById("btnEditRpt_" + RowId).style.display = "none";
                            }
                        }
                    }
                }
                document.getElementById("btnActivity_" + RowId).style.display = "inline";

                if (UserRoleCode == "RDL") {
                    if ((Locked == "Y") || ((parent.objhdnRadiologistID.value != RadID) && (RadID != "00000000-0000-0000-0000-000000000000"))) {
                        if (document.getElementById("btnSel_" + RowId) != null) document.getElementById("btnSel_" + RowId).style.display = "none";
                    }

                    if ((StatusID == 50) && (parent.objhdnRadiologistID.value == RadID)) {
                        if (document.getElementById("btnRelCase_" + RowId) != null) document.getElementById("btnRelCase_" + RowId).style.display = "inline";
                    }
                }

            }
            else if (UserRoleCode == "TRS") {
                if (document.getElementById("btnEditRpt_" + RowId) != null) {
                    document.getElementById("btnEditRpt_" + RowId).style.display = "none";
                    document.getElementById("btnTransReq_" + RowId).style.display = "none";
                    if (Locked == "Y") document.getElementById("btnLocked_" + RowId).style.display = "inline";
                    else document.getElementById("btnTrans_" + RowId).style.display = "inline";
                }
            }

        }

        if (document.getElementById("btnActivity_" + RowId) != null) {
            if (UserRoleCode == "TRS") {
                document.getElementById("btnActivity_" + RowId).style.display = "none";
                if (Locked == "Y") {
                    document.getElementById("btnTrans_" + RowId).style.display = "none";
                    document.getElementById("btnLocked_" + RowId).style.display = "inline";
                }
                else {
                    document.getElementById("btnTrans_" + RowId).style.display = "inline";
                    document.getElementById("btnLocked_" + RowId).style.display = "none";
                }
            }
            else if ((UserRoleCode == "IU") || (UserRoleCode == "AU")) {
                document.getElementById("btnEditRpt_" + RowId).style.display = "none";
                document.getElementById("btnLocked_" + RowId).style.display = "none";
                document.getElementById("btnActivity_" + RowId).style.display = "none";
                document.getElementById("btnTransReq_" + RowId).style.display = "none";
                document.getElementById("btnTrans_" + RowId).style.display = "none";
            }
            else {
                if (LogAvailable == "N") document.getElementById("btnActivity_" + RowId).style.display = "none";
            }
        }

        if (objhdnSCHCASVCENBL.value == "Y") {
            if (document.getElementById("btnSel_" + RowId) != null) document.getElementById("btnSel_" + RowId).style.display = "none";
        }

        itemIndex++;
    }
    if (rc == 0) {
        if (UserRoleCode == "RDL") objbtnGetCase.style.display = "inline";
    }
    parent.GsRetStatus = "false";
}
function SetPriorityList(gridItem) {
    var arrSpcSvcAvbl = new Array();
    var arrSpcInst = new Array();
    var arrModSvcAvbl = new Array();
    var arrModInst = new Array();
    var SelPriorityID = 0; var PriorityID = 0; var ModalityID = 0; var InstID = ""; var SpeciesID = ""; var DefOptColor = "";
    var Exception = ""; var colour = "";
    var RowId = gridItem.Data[0].toString();

    if (parent.GsTheme == "DEFAULT") { for (var x = 0; x < document.getElementById("ddlPriority_" + RowId).length; x++) { document.getElementById("ddlPriority_" + RowId).options[x].style.color = "rgb(0, 0, 0)"; } }
    else if (parent.GsTheme == "DARK") { for (var x = 0; x < document.getElementById("ddlPriority_" + RowId).length; x++) { document.getElementById("ddlPriority_" + RowId).options[x].style.color = "rgb(255, 255, 255)"; } }

    //debugger;
    if (objhdnAfterHrs.value == "Y") {
        if (parent.Trim(objhdnSpcSvcAvblAH.value) != "") arrSpcSvcAvbl = objhdnSpcSvcAvblAH.value.split(Divider);
        if (parent.Trim(objhdnSpcSvcAvblAHExInst.value) != "") arrSpcInst = objhdnSpcSvcAvblAHExInst.value.split(Divider);
        if (parent.Trim(objhdnModSvcAvblAH.value) != "") arrModSvcAvbl = objhdnModSvcAvblAH.value.split(Divider);
        if (parent.Trim(objhdnModSvcAvblAHExInst.value) != "") arrModInst = objhdnModSvcAvblAHExInst.value.split(Divider);
    }
    else {
        if (parent.Trim(objhdnSpcSvcAvbl.value) != "") arrSpcSvcAvbl = objhdnSpcSvcAvbl.value.split(Divider);
        if (parent.Trim(objhdnSpcSvcAvblExInst.value) != "") arrSpcInst = objhdnSpcSvcAvblExInst.value.split(Divider);
        if (parent.Trim(objhdnModSvcAvbl.value) != "") arrModSvcAvbl = objhdnModSvcAvbl.value.split(Divider);
        if (parent.Trim(objhdnModSvcAvblExInst.value) != "") arrModInst = objhdnModSvcAvblExInst.value.split(Divider);
    }

    ModalityID = parseInt(gridItem.Data[29].toString());
    SelPriorityID = parseInt(gridItem.Data[9].toString());
    InstID = parseInt(gridItem.Data[34].toString());
    SpeciesID = parseInt(gridItem.Data[37].toString());

    if (parent.GsTheme == "DEFAULT") { document.getElementById("ddlPriority_" + RowId).style.color = "#000"; DefOptColor = "rgb(0,0,0)";}
    else if (parent.GsTheme == "DARK") { document.getElementById("ddlPriority_" + RowId).style.color = "#fff"; DefOptColor = "rgb(255,255,255)"; }

    for (var i = 0; i < document.getElementById("ddlPriority_" + RowId).length; i++) {

        //document.getElementById("ddlPriority_" + RowId).style.color = "rgb(0, 0, 0)";
        PriorityID = parseInt(document.getElementById("ddlPriority_" + RowId).options[i].value);
        if (PriorityID > 0) {
            /************************************Set Priority for SPECIES************************************/
            if (SpeciesID > 0) {
                for (var j = 0; j < arrSpcSvcAvbl.length; j = j + 4) {
                    if ((parseInt(arrSpcSvcAvbl[j + 2]) == PriorityID) && (parseInt(arrSpcSvcAvbl[j + 1]) == SpeciesID)) {
                        if (arrSpcSvcAvbl[j + 3] == "N") {
                            Exception = "N";
                            for (var k = 0; k < arrSpcInst.length; k = k + 4) {
                                if ((arrSpcInst[k + 1] == SpeciesID) && (arrSpcInst[k + 2] == PriorityID) && (arrSpcInst[k + 3] == InstID)) {
                                    Exception = "Y";
                                    break;
                                }
                            }
                            if (Exception == "N" && SelPriorityID != PriorityID) colour = "rgb(187, 187, 187)";
                            else if (Exception == "Y") {
                                if (parent.GsTheme == "DEFAULT") colour = "rgb(0, 0, 0)"; else if (parent.GsTheme == "DARK") colour = "rgb(255, 255, 255)";
                            }
                            document.getElementById("ddlPriority_" + RowId).options[i].style.color = colour;
                            if (PriorityID == SelPriorityID) {
                                document.getElementById("ddlPriority_" + RowId).style.color = colour;
                            }
                        }
                        else if (arrSpcSvcAvbl[j + 3] == "Y") {
                            Exception = "N";
                            for (var k = 0; k < arrSpcInst.length; k = k + 4) {
                                if ((arrSpcInst[k + 1] == ModalityID) && (arrInst[k + 2] == PriorityID) && (arrSpcInst[k + 3] == InstID)) {
                                    Exception = "Y";
                                    break;
                                }
                            }
                            if (Exception == "Y") colour = "rgb(187, 187, 187)";
                            else if (Exception == "N") {
                                if (parent.GsTheme == "DEFAULT") colour = "rgb(0, 0, 0)"; else if (parent.GsTheme == "DARK") colour = "rgb(255, 255, 255)";
                            }
                            document.getElementById("ddlPriority_" + RowId).options[i].style.color = colour;

                            if (PriorityID == SelPriorityID) {
                                document.getElementById("ddlPriority_" + RowId).style.color = colour;
                            }
                        }
                        break;
                    }
                }
            }
            /************************************Set Priority for SPECIES************************************/
            /************************************Set Priority for MODALITY************************************/
            if (ModalityID > 0) {
                for (var j = 0; j < arrModSvcAvbl.length; j = j + 4) {
                    if ((parseInt(arrModSvcAvbl[j + 2]) == PriorityID) && (parseInt(arrModSvcAvbl[j + 1]) == ModalityID)) {
                        if (arrModSvcAvbl[j + 3] == "N") {
                            Exception = "N";

                            for (var k = 0; k < arrModInst.length; k = k + 4) {
                                if ((arrModInst[k + 1] == ModalityID) && (arrModInst[k + 2] == PriorityID) && (arrModInst[k + 3] == InstID)) {
                                    Exception = "Y";
                                    break;
                                }
                            }
                            if (Exception == "N" && SelPriorityID != PriorityID) colour = "rgb(187, 187, 187)";
                            else if (Exception == "Y") {
                                if (parent.GsTheme == "DEFAULT") colour = "rgb(0, 0, 0)"; else if (parent.GsTheme == "DARK") colour = "rgb(255, 255, 255)";
                            }

                            if ((document.getElementById("ddlPriority_" + RowId).options[i].style.color == DefOptColor) && (colour == "rgb(187, 187, 187)")) {
                                document.getElementById("ddlPriority_" + RowId).options[i].style.color = colour;
                                if (PriorityID == SelPriorityID) {
                                    document.getElementById("ddlPriority_" + RowId).style.color = colour;
                                }
                            }
                        }
                        else if (arrModSvcAvbl[j + 3] == "Y") {
                            Exception = "N";

                            for (var k = 0; k < arrModInst.length; k = k + 4) {
                                if ((arrModInst[k + 1] == ModalityID) && (arrModInst[k + 2] == PriorityID) && (arrModInst[k + 3] == InstID)) {
                                    Exception = "Y";
                                    break;
                                }
                            }
                            if (Exception == "Y") colour = "rgb(187, 187, 187)";
                            else if (Exception == "N") {
                                if (parent.GsTheme == "DEFAULT") colour = "rgb(0, 0, 0)"; else if (parent.GsTheme == "DARK") colour = "rgb(255, 255, 255)";
                            }

                            if ((document.getElementById("ddlPriority_" + RowId).options[i].style.color == DefOptColor) && (colour == "rgb(187, 187, 187)")) {
                                document.getElementById("ddlPriority_" + RowId).options[i].style.color = colour;

                                if (PriorityID == SelPriorityID) {
                                    document.getElementById("ddlPriority_" + RowId).style.color = colour;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            /************************************Set Priority for MODALITY************************************/
        }
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
