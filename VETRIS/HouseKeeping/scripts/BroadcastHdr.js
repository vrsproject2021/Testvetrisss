 var ck="N"
function grdInst_onCallbackComplete(sender, eventArgs) {
    grdInst.Width = "99%";
    parent.adjustFrameHeight();
    ConfigureGridColumn(strBroadcastFLAG);
}
function grdInst_onRenderComplete(sender, eventArgs) {
    parent.adjustFrameHeight();
    SetGridValues();
    ConfigureGridColumn(strBroadcastFLAG);

    if (ck === 'N') document.getElementById("chkSelect").checked = false;
    else if (ck === 'Y') document.getElementById("chkSelect").checked = true;
}
function grdInst_onColumnReorder(sender, eventArgs) {
    parent.adjustFrameHeight();
    
    ConfigureGridColumn(strBroadcastFLAG);
   
}
function SetGridValues() {
    var itemIndex = 0; var gridItem; var RowId = "0";
    var chk = "";
    
    while (gridItem = grdInst.get_table().getRow(itemIndex)) {
        
        RowId = gridItem.get_cells()[0].get_value().toString();
        chk = gridItem.get_cells()[1].get_value().toString();
        
        if (document.getElementById("chkSelect_" + RowId) != null) {
            if (chk == "Y") {
                
                document.getElementById("chkSelect_" + RowId).checked = true;
            }
            else {
                document.getElementById("chkSelect_" + RowId).checked = false;
            }
        }
        itemIndex++;
    }

}

function ConfigureGridColumn(x) {
    var gridItem;
    gridItem = grdInst.get_table();

    var gridCol;
    gridCol = gridItem.Columns[0]["Visible"];
    if (x === "E") {
        //gridItem.Columns["email_id"].Visible = true;
        //gridItem.Columns["mobile"].Visible = false;
        gridItem.Columns[3]["Visible"] = true;
        gridItem.Columns[4]["Visible"] = false;
    }
    else if (x === "S") {
        //gridItem.Columns["email_id"].Visible = false;
        gridItem.Columns[3]["Visible"] = false;
        gridItem.Columns[4]["Visible"] = true;
    }

    //CallBackInst.callback(UserID, objddlInstitution.value);
}
