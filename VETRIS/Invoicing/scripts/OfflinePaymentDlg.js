var selected_rows = [];
var headerChecked = false;

$(document).ready($(function () {
    var dt = new Date();
    var config = parent.GsDlgConfAction;


}))
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

    objhdnError.value = "";
    SetPageValue();
    LoadInvoices();
}
function SetPageValue() {
    if (objhdnID.value != "00000000-0000-0000-0000-000000000000") {
        objbtnSave1.style.display = "none";
        objbtnSave2.style.display = "none";
        objbtnAutoAdjust.style.display = "none";
        objbtnClearSelection.style.display = "none";

        objddlAccount.disabled = true;
        objtxtAmount.readOnly = "readOnly";
        objtxtRefNo.readOnly = "readOnly";
        objddlPmtMode.disabled = true;
        parent.GsStoredValue.length = 0;
    }
    else {
        if (parent.GsStoredValue.length > 0) {
            objtxtOurRefNo.value = parent.GsStoredValue[0];
            objtxtRefDate.value = parent.GsStoredValue[1];
            objtxtRefNo.value = parent.GsStoredValue[2];
            objtxtAmount.value = parent.GsStoredValue[3];
            objddlPmtMode.value = parent.GsStoredValue[4];
            if (objddlPmtMode.value == "1") { objtxtRefNo.readOnly = "readOnly"; }
            parent.GsStoredValue.length = 0;
        }
    }
    if (objhdnAID.value != "00000000-0000-0000-0000-000000000000") {
        objddlAccount.value = objhdnAID.value;
    }
}
function LoadInvoices() {
    var ArrRecords = new Array();
    ArrRecords[0] = "L";
    ArrRecords[1] = UserID;
    ArrRecords[2] = MenuID;
    ArrRecords[3] = objhdnID.value;
    ArrRecords[4] = objddlAccount.value;
    CallBackInvoiceBrw.callback(ArrRecords);
}
function ddlAccount_OnChange() {
    LoadInvoices();
}

function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        btnBrwAddUI_Onclick('Invoicing/VRSOfflinePaymentDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Invoicing/VRSOfflinePaymentDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSOfflinePaymentDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSOfflinePaymentDlg.aspx';
        parent.PopupConfirm("028");
    }
    showCheckBoxes();
}
function btnClose_OnClick() {
    parent.GsNavURL = "Invoicing/VRSPaymentProcess.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function ValidateData() {
    var amount = parseFloat(objtxtAmount.value);
    var refNo = objtxtRefNo.value || "";
    var strErr = "";
    var ret = true;

    if (objddlAccount.value == "00000000-0000-0000-0000-000000000000") {
        strErr += "225";
    }

    //if (parent.Trim(refNo) == "") {
    //    if (parent.Trim(strErr) != "") strErr += Divider;
    //    strErr += "316";
    //}
    if (amount <= 0) {

        if (parent.Trim(strErr) != "") strErr += Divider;
        strErr += "318";
    }

    if (parent.Trim(strErr) != "") {
        parent.PopupMessage(RootDirectory, strForm, "ValidateData()", strErr, "true");
        ret = false;
    }


    return true;
}
function btnSave_OnClick() {
    var ArrRecords = new Array();
    var strDt = ""; var ret = false;
    parent.PopupProcess("Y");
    if (objddlPmtMode.value == "1") {
        if (!ValidateData()) return;
    }

    if (parent.Trim(objtxtRefDate.value) == "") strDt = "01Jan1900";
    else {
        if (document.all)
            strDt = parent.SetDateFormat(objtxtRefDate.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDt = parent.SetDateFormat1(objtxtRefDate.value, parent.GsDateFormat, parent.GsDateSep);
    }
    ret = GetInvoices();

    if (objddlPmtMode.value == "1") {
        if (ret) {
            parent.GsStoredValue[0] = objtxtOurRefNo.value;
            parent.GsStoredValue[1] = objtxtRefDate.value;
            parent.GsStoredValue[2] = objtxtRefNo.value;
            parent.GsStoredValue[3] = objtxtAmount.value;
            parent.GsStoredValue[4] = objddlPmtMode.value;
            parent.HideProcess();
            btnBrwAddUI_Onclick('MyPayment/VRSGetPaymentDlg.aspx?cf=pp&aid=' + objddlAccount.value);
        }
    }
    else if (objddlPmtMode.value == "0") {
        if (ret) {
            

            try {
                var data = parent["__pg_data__"];
                var invoices = [];
                if (data.length > 0) {
                    invoices = data;
                }
                ArrRecords[0] = objhdnID.value;
                ArrRecords[1] = objddlAccount.value;
                ArrRecords[2] = objtxtOurRefNo.value;
                ArrRecords[3] = strDt;
                ArrRecords[4] = objtxtRefNo.value;
                ArrRecords[5] = objtxtAmount.value;
                ArrRecords[6] = objddlPmtMode.value;
                ArrRecords[7] = UserID;
                ArrRecords[8] = MenuID;
                ArrRecords[9] = JSON.stringify(invoices);

                AjaxPro.timoutPeriod = 1800000;
                VRSOfflinePaymentDlg.SaveRecord(ArrRecords, ShowProcess);
            }
            catch (expErr) {
                parent.HideProcess();
                parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
            }
        }

    }
    else {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", "317", "true");
    }

}
function GetInvoices() {
    var amount = parseFloat(objtxtAmount.value);
    var outstanding = GetTotalOutstandingAmount();
    var adjustedRows = [];
    var table = grdInvoiceBrw.get_table();
    var tableRows = table.getRowCount();

    if (tableRows > 0) {
        adjustedRows = GetAdjustedData();
        if (adjustedRows.length == 0) {
            AutoAdjust();
            adjustedRows = GetAdjustedData();
        } else {
            var adjamount = 0;
            adjustedRows.forEach(function (item) {
                adjamount = parseFloat((adjamount + item.adj_amount).toFixed(parent.GiDecimal || 2));
            });
            if (adjamount != amount && outstanding >= amount) {
                parent.PopupMessage(RootDirectory, strForm, "GetInvoices()", "320", "true");
                return false;
            }
        }

        parent["__pg_billing_account_id__"] = objddlAccount.value;
        if (objddlPmtMode.value == "0") parent["__pg_data__"] = adjustedRows;
        else if (objddlPmtMode.value == "1") {
            var total = parseFloat(objtxtAmount.value);
            parent["__pg_data__"] = Object.assign({}, { invoices: adjustedRows, amount: total });
        }
    }
    else {
        parent.PopupMessage(RootDirectory, strForm, "GetInvoices()", "319", "true");
        return false;
    }

   return true;
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
            objhdnID.value = arrRes[2];
            objtxtOurRefNo.value = arrRes[3];
            objtxtRefDate.value.value = arrRes[4];
            SetPageValue();
            ddlAccount_OnChange();
            parent.GsRetStatus = "false";
            break;
    }
}

function ResetValueDecimal(objCtrlID) {
    var objCtrl;
    objCtrl = document.getElementById(objCtrlID); if (objCtrl == null) objCtrl = objCtrlID;
    if (objCtrl.value == "") objCtrl.value = parent.SetDecimalFormat("0");
    else if (objCtrl.value == undefined) objCtrl.value = parent.SetDecimalFormat("0");
    else objCtrl.value = parent.SetDecimalFormat(objCtrl.value);

}
function ResetValueInteger(objCtrlID) {

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
function ShowProcess(Result, MethodName) {
    parent.HideProcess();
    var strMethod = MethodName.method;
    switch (strMethod) {

        case "SaveRecord":
            SaveRecord(Result);
            break;

    }
}

function GetSelectedRows() {
    var table = grdInvoiceBrw.get_table();
    var rows = [];
    for (var i = 0; i < table.getRowCount() ; i++) {
        var r = table.getRow(i);
        var selected = getColumnValue(r, "selected");
        if (selected == true) {
            rows.push(r);
        }
    }
    return rows;
};
function getColumn(row, name) {
    return row.Cells.find(function (i) { return i.Name == name; });
}
function setColumnValue(row, name, value) {
    var column = getColumn(row, name);
    var _value = row.Data[column.Column.ColumnNumber];
    if (Array.isArray(_value)) {
        _value[0] = value;
        _value[1] = value.toFixed(parent.GiDecimal || 2);
        return;
    }
    return row.Data[column.Column.ColumnNumber] = value;
}
function getColumnValue(row, name) {
    var column = getColumn(row, name);
    var value = row.Data[column.Column.ColumnNumber];
    if (Array.isArray(value)) return value[0];
    return value;
}
function getColumnFormatedValue(row, name) {
    var column = getColumn(row, name);
    var value = row.Data[column.Column.ColumnNumber][1];
    if (Array.isArray(value)) return value[0];
    return value;
}
function ClearSelectedRows(update) {
    var table = grdInvoiceBrw.get_table();
    var rows = [];
    if (update) grdInvoiceBrw.beginUpdate();
    for (var i = 0; i < table.getRowCount() ; i++) {
        var r = table.getRow(i);
        var selected = getColumnValue(r, "selected");
        grdInvoiceBrw.unSelect(r);
        if (selected) {
            setColumnValue(r, "selected", false);
            var balance = getColumnValue(r, "balance");
            setColumnValue(r, "adjusted", 0);
            setColumnValue(r, "current_balance", balance);
        }
    }
    if (update) grdInvoiceBrw.endUpdate();
    $("input[id^='chkSel_']").removeAttr('checked');
    $("input[id^='chkSel_']").attr('unchecked', true);
};
function ToggleCheckbox(objCheckbox, itemID, e) {
    var row = grdInvoiceBrw.getItemFromClientId(itemID);
    var tobeAdjusted = parseFloat(objtxtAmount.value);
    if (tobeAdjusted == 0) {
        grdInvoiceBrw.beginUpdate();
        var selected = getColumnValue(row, "selected");
        if (selected) {
            setColumnValue(row, "selected", false);
            var balance = getColumnValue(r, "balance");
            setColumnValue(row, "adjusted", 0);
            setColumnValue(row, "current_balance", balance);
        }
        $(objCheckbox).removeAttr("checked");
        $(objCheckbox).attr("unchecked", true);
        setColumnValue(row, "selected", false);
        grdInvoiceBrw.endUpdate();
        e.cancelBubble = true;
        if (e.stopPropagation) e.stopPropagation();
        return true;
    }
    grdInvoiceBrw.beginUpdate();
    if (objCheckbox.checked) {

        var selectedRows = GetSelectedRows();
        var totalAdjusted = 0;
        selectedRows.forEach(function (r) {
            var adjusted = getColumnValue(r, "adjusted");
            totalAdjusted = parseFloat((totalAdjusted + (adjusted || 0)).toFixed(parent.GiDecimal || 2));
        });
        if (totalAdjusted > tobeAdjusted) {
            ClearSelectedRows(false);
            $(objCheckbox).attr("unchecked", true);
            $(objCheckbox).removeAttr("checked");
            setColumnValue(row, "selected", false);
            grdInvoiceBrw.endUpdate();
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
            return true;
        }
        if (totalAdjusted == tobeAdjusted) {
            $(objCheckbox).attr("unchecked", true);
            $(objCheckbox).removeAttr("checked");
            setColumnValue(row, "selected", false);
            var balance = getColumnValue(r, "balance");
            setColumnValue(row, "adjusted", 0);
            setColumnValue(row, "current_balance", balance);
            grdInvoiceBrw.endUpdate();
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
            return true;
        }
        tobeAdjusted = tobeAdjusted - totalAdjusted;
        var adjusted = getColumnValue(row, "adjusted");
        var balance = getColumnValue(row, "balance");
        var current_balance = getColumnValue(row, "current_balance");
        var adjusted = 0;
        if (balance >= tobeAdjusted) {
            adjusted = tobeAdjusted;
        }
        else
            adjusted = balance;
        setColumnValue(row, "selected", true);
        setColumnValue(row, "adjusted", adjusted);
        setColumnValue(row, "current_balance", (balance - adjusted));
        grdInvoiceBrw.endUpdate();
    }
    else {
        setColumnValue(row, "selected", false);
        var balance = getColumnValue(row, "balance");
        setColumnValue(row, "adjusted", 0);
        setColumnValue(row, "current_balance", balance);

    }
    grdInvoiceBrw.endUpdate();
    e.cancelBubble = true;
    if (e.stopPropagation) e.stopPropagation();
    return true;
}
function AutoAdjust() {
    var tobeAdjusted = parseFloat(objtxtAmount.value);
    var selectedRows = GetSelectedRows();
    var totalAdjusted = 0;
    grdInvoiceBrw.beginUpdate();
    ClearSelectedRows(false);
    var table = grdInvoiceBrw.get_table();
    var rows = [];
    for (var i = 0; i < table.getRowCount() ; i++) {
        if (tobeAdjusted == 0) break;
        var r = table.getRow(i);
        var adjusted = 0, current_balance = 0;
        var balance = getColumnValue(r, "balance");
        if (balance >= tobeAdjusted) {
            adjusted = tobeAdjusted;
            tobeAdjusted = 0;
            current_balance = parseFloat((balance - adjusted).toFixed(parent.GiDecimal || 2));
        } else {
            adjusted = balance;
            current_balance = 0;
            tobeAdjusted = parseFloat((tobeAdjusted - balance).toFixed(parent.GiDecimal || 2));
        }
        setColumnValue(r, "selected", true);
        setColumnValue(r, "adjusted", adjusted);
        setColumnValue(r, "current_balance", current_balance);
        var chk = $("#Chk_" + getColumnValue(r, "id"))
        chk.attr("checked", true);
        chk.removeAttr("unchecked");
        grdInvoiceBrw.endUpdate();
    }
}
function GetAdjustedData() {
    var table = grdInvoiceBrw.get_table();
    var rows = [];
    for (var i = 0; i < table.getRowCount() ; i++) {
        var r = table.getRow(i);
        var selected = getColumnValue(r, "selected");
        if (selected == true) {
            var invDt = "";
            invDt = getColumnFormatedValue(r, "invoice_date");
            if (document.all)
                invDt = parent.SetDateFormat(invDt, parent.GsDateFormat, parent.GsDateSep);
            else
                invDt = parent.SetDateFormat1(invDt, parent.GsDateFormat, parent.GsDateSep);
            rows.push({
                invoice_header_id: getColumnValue(r, "id"),
                balance: getColumnValue(r, "balance"),
                invoice_no: getColumnValue(r, "invoice_no"),
                invoice_date: invDt,
                adj_amount: getColumnValue(r, "adjusted")
            });
        }
    }
    return rows;
}
function GetTotalOutstandingAmount() {
    var table = grdInvoiceBrw.get_table();
    var amount = 0;
    for (var i = 0; i < table.getRowCount() ; i++) {
        var r = table.getRow(i);
        var amount = parseFloat((amount + getColumnValue(r, "balance")).toFixed(parent.GiDecimal || 2));
    }
    return amount;
}

function ddlPmtMode_OnChange() {
    if (objddlPmtMode.value == "1") {
        objtxtRefNo.value = "";
        objtxtRefNo.readOnly = "readOnly";
    }
    else {
        objtxtRefNo.readOnly = "";
    }
    
}