var grdRowID = "0"; var cb = "N";  var INSTADD = "N";

function grdInst_onCallbackComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    parent.adjustFrameHeight();
    var strErr = parent.Trim(document.getElementById("hdnCBErrInst").value);
    if (strErr != "") {
        parent.PopupMessage(RootDirectory, strForm, "grdInst_onCallbackComplete()", strErr, "true");
    }
}
function grdInst_onRenderComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    parent.adjustFrameHeight();
    var itemIndex = 0; var gridItem; var RowId = "0";
    var InstID = "00000000-0000-0000-0000-000000000000";
    var LinkedUser =  "00000000-0000-0000-0000-000000000000";
    var arrInst = new Array();
    var arrUser = new Array();

    if (parent.Trim(objhdnInstitutions.value) != "") {
        if (parent.Trim(objhdnInstitutions.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrInst = parent.Trim(objhdnInstitutions.value).split(parent.objhdnDivider.value);
        }
        else
            arrInst[0] = parent.Trim(objhdnInstitutions.value);
    }

    if (parent.Trim(objhdnUsers.value) != "") {
        if (parent.Trim(objhdnUsers.value).indexOf(parent.objhdnDivider.value) != -1) {
            arrUser = parent.Trim(objhdnUsers.value).split(parent.objhdnDivider.value);
        }
        else
            arrUser[0] = parent.Trim(objhdnUsers.value);
    }

    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        RowId = gridItem.Data[0].toString();
        InstID = gridItem.Data[1].toString();
        LinkedUser = gridItem.Data[2].toString();

        

        if (document.getElementById("ddlInst_" + RowId) != null) {
            if (document.getElementById("ddlInst_" + RowId).length == 0) {
                for (var i = 0; i < arrInst.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrInst[i];
                    op.text = arrInst[i + 1];
                    document.getElementById("ddlInst_" + RowId).add(op);
                }
            }
            document.getElementById("ddlInst_" + RowId).value = InstID;
               
        }

        if (document.getElementById("ddlUser_" + RowId) != null) {
            if (document.getElementById("ddlUser_" + RowId).length == 0) {
                for (var i = 0; i < arrInst.length; i = i + 2) {
                    var op = document.createElement("option");
                    op.value = arrUser[i];
                    op.text = arrUser[i + 1];
                    document.getElementById("ddlUser_" + RowId).add(op);
                }
            }
            document.getElementById("ddlUser_" + RowId).value = LinkedUser;

        }
      
        if (document.getElementById("txtCommission1Yr_" + RowId) != null) {
            document.getElementById("txtCommission1Yr_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtCommission1Yr_" + RowId).value);
        }
        if (document.getElementById("txtCommission2Yr_" + RowId) != null) {
            document.getElementById("txtCommission2Yr_" + RowId).value = parent.SetDecimalFormat(document.getElementById("txtCommission2Yr_" + RowId).value);
        }


        itemIndex++;
    }

    if (INSTADD == "N") parent.GsRetStatus = "false";
    else parent.GsRetStatus = "true";
    INSTADD = "N";
}
