$(document).ready(function () {
    SearchRecord();
});
var DateFmt = parent.objhdnDateFormat.value;
var DateSep = parent.objhdnDateSep.value;
var strControlCode = ""; var objCtrl;
var selected_rows = [];
var AccountID = parent.objhdnBillAcctID.value;

function CheckError() {
    var arrErr = new Array();
    if (parent.Trim(objhdnError.value) != "") {
        arrErr = objhdnError.value.split(Divider);
        switch (arrErr[0]) {
            case "catch":
            case "false":
                parent.PopupMessage(RootDirectory, strForm, "OnLoad()", arrErr[1], "true");
                break;
        }
    }
    else {
        CheckRights();
    }
    objhdnError.value = "";
}
function CheckRights() {
    //var strRights = objhdnRights.value;
    //if (strRights.substring(0, 1) == "Y") { objbtnSave1.style.display = "inline"; objbtnSave2.style.display = "inline"; }
}
function SaveRecord(Result) {
    var arrRes = new Array();
    arrRes = Result.value;
    switch (arrRes[0]) {
        case "catch":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true");
            break;
        case "false":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "true", arrRes[2]);
            break;
        case "true":
            parent.PopupMessage(RootDirectory, strForm, "SaveRecord()", arrRes[1], "false");
            parent.GsRetStatus = "false";
            btnClose_OnClick();
            break;
    }
}
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {
        case "SaveRecord":
            SaveRecord(Result);
            break;
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false")
        btnBrwEdit_Onclick('MyPayment/VRSPayment.aspx');
    else {
        parent.GsDlgConfAction = "RES";
        parent.GsNavURL = "MyPayment/VRSPayment.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
        parent.PopupConfirm("030");
    }

}
function btnClose_OnClick() {
    parent.GsNavURL = "VRSHome.aspx";
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("030");
    }
}
function btnPay_OnClick() {
    if (onPayNowTotal()) {
        btnBrwAddUI_Onclick('MyPayment/VRSGetPaymentDlg.aspx?aid=' + AccountID);
    }
}


function ResetValueDecimal(objCtrlID, decimalPlace) {

    var objCtrl;
    if (decimalPlace == null) decimalPlace = parent.GiDecimal;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0", decimalPlace);
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value, decimalPlace);

}
function ResetValueInteger(objCtrlID, strDefaultValue) {
    var objCtrl;
    objCtrl = document.getElementById(objCtrlID.id);
    if (objCtrl == null) objCtrl = objCtrlID;
    if (strDefaultValue != null) {
        if (objCtrl.value == "" || parseInt(objCtrl.value) <= 0) {
            objCtrl.value = strDefaultValue;
        }
    }
    else {
        if (objCtrl.value == "") objCtrl.value = "0";
    }
}
function ProcessMessage(ArgsRet) {
    if (ArgsRet == "TE") {

        objCtrl.focus();
    }
    objCtrl = null;
}

function btnPayRow_Click(sender, rowid, rowdata) {
    
    $("button[id^='btnPay_']").attr('disabled', true);
    var total = 0;
    var invDt = "";
    var cell = rowdata.Cells.find(function (i) { return i.Name == "balance"; });
    var inv_no_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_no"; });
    var inv_dt_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_date"; });
    invDt = inv_dt_cell.Text;

    if (document.all)
        invDt = parent.SetDateFormat(invDt, parent.GsDateFormat, parent.GsDateSep);
    else
        invDt = parent.SetDateFormat1(invDt, parent.GsDateFormat, parent.GsDateSep);


    if (cell) total = cell.Value;
    selected_rows = [{ invoice_header_id: rowid, invoice_no: inv_no_cell.Value, invoice_date: invDt, adj_amount: total }];
    objtxtSelectedAmount.value = total.toFixed(parent.GiDecimal || 2);
    if (total < 1) {
        $(objtxtSelectedAmount).removeAttr('readonly');
        $(objbtnPay).html('<i class="fa fa-credit-card"></i> Adhoc Pay now');
        $(objtxtSelectedAmount).focus();
    } else {
        $(objtxtSelectedAmount).attr('readonly', true);
        $(objbtnPay).html('<i class="fa fa-credit-card"></i> Pay now');
        btnPay_OnClick();
    }

}
function btnAllPayRow_Click(sender, rowid, rowdata) {
    
    $("button[id^='btnAllPay_']").attr('disabled', true);
    var adjusted = 0;
    var total = 0;
    var invDt = "";
    var cell = rowdata.Cells.find(function (i) { return i.Name == "balance"; });
    var inv_no_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_no"; });
    var inv_dt_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_date"; });
    invDt = inv_dt_cell.Text;

    if (document.all)
        invDt = parent.SetDateFormat(invDt, parent.GsDateFormat, parent.GsDateSep);
    else
        invDt = parent.SetDateFormat1(invDt, parent.GsDateFormat, parent.GsDateSep);


    if (cell) total = cell.Value;
    selected_rows = [{ invoice_header_id: rowid, invoice_no: inv_no_cell.Value, invoice_date: invDt, adj_amount: total }];
    if (selected_rows.length == 0) adjusted = CreateAdjustmentsFromAll(total, rowid);
    else {
        adjusted = selected_rows.map(function (i) { return i.adj_amount; }).reduce(function (a, b) { return a + b; });
    };
    if (total > adjusted) {
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", "Excess amount not allowed.", "true");
        return false;
    }
    parent["__pg_billing_account_id__"] = objhdnID.value;
    parent["__pg_data__"] = Object.assign({}, { invoices: selected_rows, amount: total });
    
    btnBrwAddUI_Onclick('MyPayment/VRSGetPaymentDlg.aspx?aid=' + AccountID);
}

function onPayNowTotal() {
    var total = parseFloat(objtxtSelectedAmount.value);
    var adjusted = 0;
    if (total <= 0) {
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", "Amount must be greater than zero.", "true");
        return false;
    }
    if (selected_rows.length == 0) adjusted = CreateAdjustments(total);
    else {
        adjusted=selected_rows.map(function (i) { return i.adj_amount; }).reduce(function (a, b) { return a + b; });
    };
    if (total > adjusted) {
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", "Excess amount not allowed.", "true");
        return false;
    }
    objtxtSelectedAmount.value = total.toFixed(parent.GiDecimal || 2);
    parent["__pg_billing_account_id__"] = objhdnID.value;
    parent["__pg_data__"] = Object.assign({}, { invoices: selected_rows, amount: total });
    return true;
}

function CreateAdjustments(total) {
    
    // prepare auto adjustments
    if (selected_rows.length == 0 && total > 0) {
        var tobeadjusted = total;
        var rowdata, index = 0;
        var invDt = "";
        selected_rows = [];
        while (rowdata = grdInvoiceBrw.get_table().getRow(index)) {
            var balance = 0;
            var cell = rowdata.Cells.find(function (i) { return i.Name == "balance"; });
            if (cell) balance = cell.Value;
            cell = rowdata.Cells.find(function (i) { return i.Name == "id"; });

            if (cell) {
                var invoice_header_id = cell.Value;
                var inv_no_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_no"; });
                var inv_dt_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_date"; });

                var invoice_no = inv_no_cell.Value;
                invDt = inv_dt_cell.Text;

                if (document.all)
                    invDt = parent.SetDateFormat(invDt, parent.GsDateFormat, parent.GsDateSep);
                else
                    invDt = parent.SetDateFormat1(invDt, parent.GsDateFormat, parent.GsDateSep);

                if (tobeadjusted == 0) break;
                if (balance >= tobeadjusted) {
                    selected_rows.push({ invoice_header_id: invoice_header_id, invoice_no: invoice_no, invoice_date: invDt, adj_amount: tobeadjusted });
                    tobeadjusted = 0;
                    break;
                } else {
                    selected_rows.push({ invoice_header_id: invoice_header_id, invoice_no: invoice_no, invoice_date: invDt, adj_amount: balance });
                    tobeadjusted = parseFloat((tobeadjusted - balance).toFixed(parent.GiDecimal || 2));
                }
            }
            index++;
        }
        
        return total - tobeadjusted;
        
    }
    return 0;
}
function CreateAdjustmentsFromAll(total, rowid) {
    
    // prepare auto adjustments
    if (selected_rows.length == 0 && total > 0) {
        var tobeadjusted = total;
        var rowdata, index = 0;
        var invDt = "";
        selected_rows = [];
        while (rowdata = grdAllInvoiceBrw.get_table().getRow(index)) {
            var balance = 0;
            var cell = rowdata.Cells.find(function (i) { return i.Name == "balance"; });
            if (cell) balance = cell.Value;
            cell = rowdata.Cells.find(function (i) { return i.Name == "id"; });

            if (cell) {
                var invoice_header_id = cell.Value;
                if (invoice_header_id == rowid) {
                    var inv_no_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_no"; });
                    var inv_dt_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_date"; });

                    var invoice_no = inv_no_cell.Value;
                    invDt = inv_dt_cell.Text;

                    if (document.all)
                        invDt = parent.SetDateFormat(invDt, parent.GsDateFormat, parent.GsDateSep);
                    else
                        invDt = parent.SetDateFormat1(invDt, parent.GsDateFormat, parent.GsDateSep);

                    if (tobeadjusted == 0) break;
                    if (balance >= tobeadjusted) {
                        selected_rows.push({ invoice_header_id: invoice_header_id, invoice_no: invoice_no, invoice_date: invDt, adj_amount: tobeadjusted });
                        tobeadjusted = 0;
                        break;
                    } else {
                        selected_rows.push({ invoice_header_id: invoice_header_id, invoice_no: invoice_no, invoice_date: invDt, adj_amount: balance });
                        tobeadjusted = parseFloat((tobeadjusted - balance).toFixed(parent.GiDecimal || 2));
                    }
                }
                break;
            }
            index++;
        }
        return total - tobeadjusted;
    }
    return 0;
}
function btnClearSelection_OnClick() {
    $("input[id^='chkSel_']").attr('checked', false);
    var total = 0;
    $("button[id^='btnPay_']").removeAttr('disabled');
    selected_rows = [];
    objtxtSelectedAmount.value = total.toFixed(parent.GiDecimal || 2);
    if (total < 1) {
        $(objtxtSelectedAmount).removeAttr('readonly');
        $(objbtnPay).html('<i class="fa fa-credit-card"></i> Adhoc Pay now');
        $(objtxtSelectedAmount).focus();
    } else {
        $(objtxtSelectedAmount).attr('readonly', true);
        $(objbtnPay).html('<i class="fa fa-credit-card"></i> Pay now');
    }
}

function getTotalAmount() {

    var total = 0;
    var rowdata, index = 0;
    while (rowdata = grdInvoiceBrw.get_table().getRow(index)) {
        var balance = 0;
        var cell = rowdata.Cells.find(function (i) { return i.Name == "balance"; });
        if (cell) balance = cell.Value;
        total += parseFloat(balance);
        index++;
    }
    objtxtSelectedAmount.value = total.toFixed(parent.GiDecimal || 2);
}
function ChkSelect_OnClick(sender, rowid, rowdata) {
    var row = selected_rows.find(function (x) { return x.invoice_header_id === rowid; });
    
    if (sender.checked) {
        if (!row) {
            var balance = 0;
            var cell = rowdata.Cells.find(function (i) { return i.Name == "balance"; });
            if (cell) balance = cell.Value;
            var inv_no_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_no"; });
            var inv_dt_cell = rowdata.Cells.find(function (i) { return i.Name == "invoice_date"; });
            var invDt = inv_dt_cell.Text;

            if (document.all)
                invDt = parent.SetDateFormat(invDt, parent.GsDateFormat, parent.GsDateSep);
            else
                invDt = parent.SetDateFormat1(invDt, parent.GsDateFormat, parent.GsDateSep);

            selected_rows.push({ invoice_header_id: rowid, invoice_no: inv_no_cell.Value, invoice_date: invDt, adj_amount: balance });
        }
    } else {
        if (row) {
            var index = selected_rows.indexOf(row);
            if (index >= 0) selected_rows.splice(index, 1);
        }
    }
    var total = 0;
    selected_rows.forEach(function (i) {
        total = (total + i.adj_amount);
    });
    objtxtSelectedAmount.value = total.toFixed(parent.GiDecimal || 2);
    if (total == 0)
        $("button[id^='btnPay_']").removeAttr('disabled');
    else
        $("button[id^='btnPay_']").attr('disabled', true);
    if (total < 1) {
        $(objtxtSelectedAmount).removeAttr('readonly');
        $(objbtnPay).html('<i class="fa fa-credit-card"></i> Adhoc Pay now');
        $(objtxtSelectedAmount).focus();
    } else {
        $(objtxtSelectedAmount).attr('readonly', true);
        $(objbtnPay).html('<i class="fa fa-credit-card"></i> Pay now');
    }

}
function ShowInvoice(CycleID) {
    var AccountID = objhdnID.value;
    parent.GsFileType = "PDF";
    parent.GsLaunchURL = "Invoicing/DocumentPrinting/VRSDocPrint.aspx?DocID=2&CYCLE=" + CycleID + "&ACCT=" + AccountID + "&UID=" + UserID;
    parent.PopupReportViewer();
}

function SearchRecord() {
    var ArrRecords = new Array();
    ArrRecords[0] = AccountID;
    ArrRecords[1] = UserID;
    ArrRecords[2] = MenuID;
    if (rdoOutstanding.checked) {
        CallBackInvoiceBrw.callback(ArrRecords);
        CallBackPaymentBrw.callback(ArrRecords);

    } else {
        CallBackAllInvoiceBrw.callback(ArrRecords);
        ArrRecords[3] = objhdnID.value;
        CallBackAdjBrw.callback(ArrRecords);
    }
}


function SetFilterValues() {

}
function PreserveFilterValues() {

}