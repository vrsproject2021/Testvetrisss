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
    LoadPayments();
}
function SetPageValue() {
    if (objhdnID.value != "00000000-0000-0000-0000-000000000000") {
        objbtnSave1.style.display = "none";
        objbtnSave2.style.display = "none";

        objddlAccount.disabled = true;
        objtxtAmount.readOnly = "readOnly";
        parent.GsStoredValue.length = 0;
    }
    else {
        if (parent.GsStoredValue.length > 0) {
            objtxtOurRefNo.value = parent.GsStoredValue[0];
            objtxtRefDate.value = parent.GsStoredValue[1];
            objtxtAmount.value = parent.GsStoredValue[3];
            parent.GsStoredValue.length = 0;
        }
    }
    if (objhdnAID.value != "00000000-0000-0000-0000-000000000000") {
        objddlAccount.value = objhdnAID.value;

    }
}
function LoadPayments() {
    var ArrRecords = new Array();
    ArrRecords[0] = "L";
    ArrRecords[1] = UserID;
    ArrRecords[2] = MenuID;
    ArrRecords[3] = objhdnID.value;
    ArrRecords[4] = objddlAccount.value;
    CallBackInvoiceBrw.callback(ArrRecords);
}
function ddlAccount_OnChange() {
    LoadPayments();
}

function btnNew_OnClick() {
    if (parent.GsRetStatus == "false")
        btnBrwAddUI_Onclick('Invoicing/VRSOnlineRefundDlg.aspx');
    else {
        parent.GsDlgConfAction = "NEWUI";
        parent.GsNavURL = "Invoicing/VRSOnlineRefundDlg.aspx";
        parent.PopupConfirm("028");
    }
}
function btnReset_OnClick() {

    if (parent.GsRetStatus == "false") {
        strValidate = "N";
        btnBrwEditUI_Onclick('Invoicing/VRSOnlineRefundDlg.aspx');
    }
    else {
        parent.GsDlgConfAction = "RESUI";
        parent.GsNavURL = 'Invoicing/VRSOnlineRefundDlg.aspx';
        parent.PopupConfirm("028");
    }
    showCheckBoxes();
}
function btnClose_OnClick() {
    parent.GsNavURL = "Invoicing/VRSRefunds.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
    if (parent.GsRetStatus == "false")
        btnDlgClose_Onclick();
    else {
        parent.GsDlgConfAction = "CLS";
        parent.PopupConfirm("028");
    }
}
function ValidateData() {
    var amount = parseFloat(objtxtAmount.value);
    var refNo = objtxtOurRefNo.value || "";
    var strErr = "";
    var ret = true;

    if (objddlAccount.value == "00000000-0000-0000-0000-000000000000") {
        strErr += "225";
    }

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
    //if (objddlPmtMode.value == "1") {
    if (!ValidateData()) return;
    //}

    if (parent.Trim(objtxtRefDate.value) == "") strDt = "01Jan1900";
    else {
        if (document.all)
            strDt = parent.SetDateFormat(objtxtRefDate.value, parent.GsDateFormat, parent.GsDateSep);
        else
            strDt = parent.SetDateFormat1(objtxtRefDate.value, parent.GsDateFormat, parent.GsDateSep);
    }

    try {

        ArrRecords[0] = objhdnID.value;
        ArrRecords[1] = objddlAccount.value;
        ArrRecords[2] = objtxtOurRefNo.value;
        ArrRecords[3] = strDt;
        ArrRecords[4] = "";//objtxtOurRefNo.value;
        ArrRecords[5] = objtxtAmount.value;
        ArrRecords[6] = "1";//objddlPmtMode.value;
        ArrRecords[7] = UserID;
        ArrRecords[8] = MenuID;
        ArrRecords[9] = objPaymentID.value;
        ArrRecords[10] = objPaymentRefID.value;

        AjaxPro.timoutPeriod = 1800000;
        VRSOnlineRefundDlg.SaveRecord(ArrRecords, ShowProcess);
    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnSave_OnClick()", expErr.message, "true");
    }
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
function onChange_txtAmount(ctrl) {
    var value = parseFloat($("#txtAmount").val() | 0);
    var max = parseFloat(objmaxVal.value | "0");
    if (value > max) value = max;
    if (value < 0) value = 0;
    objtxtAmount.value = value;
    ResetValueDecimal(objtxtAmount);
    var selectedRows = GetSelectedRows();
    grdInvoiceBrw.beginUpdate();
    selectedRows.forEach(function (r) {
        setColumnValue(r, "current_refund", value);
    });
    grdInvoiceBrw.endUpdate();

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
            setColumnValue(r, "current_refund", 0);
        }
    }
    if (update) grdInvoiceBrw.endUpdate();
    objtxtAmount.value = 0;
    ResetValueDecimal(objtxtAmount);
    $("input[id^='chkSel_']").removeAttr('checked');
    $("input[id^='chkSel_']").attr('unchecked', true);
};
function ToggleCheckbox(objCheckbox, itemID, e) {
    var row = grdInvoiceBrw.getItemFromClientId(itemID);

    grdInvoiceBrw.beginUpdate();
    if (objCheckbox.checked) {

        var selectedRows = GetSelectedRows();
        selectedRows.forEach(function (r) {
            ClearSelectedRows(false);
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
            return true;
        });
        var balance = getColumnValue(row, "refundable");
        setColumnValue(row, "current_refund", balance);
        setColumnValue(row, "selected", true);
        grdInvoiceBrw.endUpdate();
        objtxtAmount.value = parseFloat((balance | 0).toFixed(parent.GiDecimal || 2));
        
        objPaymentID.value = getColumnValue(row, "id");
        objPaymentRefID.value = getColumnValue(row, "processing_ref_no");
        $("#txtAmount").prop('disabled', false);
        objmaxVal.value = objtxtAmount.value;
    }
    else {
        setColumnValue(row, "selected", false);
        setColumnValue(row, "current_refund", 0);
        objtxtAmount.value = 0;
        objmaxVal.value = 0;
        objPaymentID.value = "";
        objPaymentRefID.value = "";
        $("#txtAmount").prop('disabled', true);
    }
    ResetValueDecimal(objtxtAmount);
    grdInvoiceBrw.endUpdate();
    e.cancelBubble = true;
    if (e.stopPropagation) e.stopPropagation();
    return true;
}

