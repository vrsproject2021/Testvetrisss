$(document).ready($(function () {
    var dt = new Date();
    var config = parent.GsDlgConfAction;
    _restoreValues();

    if (objrdoCC.checked) {
        objfName.placeholder = "Card Holder's First Name*";
        objlName.placeholder = "Card Holder's Last Name";

        document.getElementById("cc_banner").style.display = "inline";
       
    }
    else if (objrdoACH.checked) {
        objfName.placeholder = "Account Holder's First Name*";
        objlName.placeholder = "Account Holder's Last Name";

        document.getElementById("cc_banner").style.display = "none";

    }
}))

$(".card-payment-option").click(function () {
    var el = $(this);
    var $radio = $(el).find("input[type=radio]");
    if ($($radio).is(':checked') === false) {
        $($radio).attr('checked', true).change();
    }
});
$(".form-check-input").change(function () {
    var that = this;
    $('.form-check-input').each(function () {
        // clean up
        var el = this;
        var eid = el.id.match(/\d+/)[0];
        var row = $("#input-" + eid);
        $(el.closest(".card-payment-option")).removeClass('d-bg-solid');
        $(row).addClass('box-hidden');
    });
    // addition
    var dest = $(that.closest(".card-payment-option"));
    var id = that.id.match(/\d+/)[0];
    var inputrow = $("#input-" + id);
    var place = $("#place-holder-" + id);
    var $input = $("#txtcvv");
    var $vault = $("#vaultSelected");
    $vault.val(that.value);
    $input.val("");
    place.append($input);
    inputrow.removeClass("box-hidden");
    dest.addClass('d-bg-solid');
    $input.focus();
});
$(document).ready(function () {
    $('.form-check-input').each(function () {

        var el = this;
        var selected = $(el).attr("data-selected") == "1";
        if (selected) {
            if ($(el).is(':checked') === false) {
                $(el).filter('[value=' + el.value + ']').prop('checked', true);
                var $vault = $("#vaultSelected");
                $vault.val(el.value);
            }
            var id = el.id.match(/\d+/)[0];
            var row = $("#input-" + id);
            var dest = $(el.closest(".card-payment-option"));
            var place = $("#place-holder-" + id);
            var $input = $("#txtcvv");
            place.append($input);
            row.removeClass("box-hidden");
            dest.addClass('d-bg-solid');

            $input.focus();
        }

    });
});

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
}
function SetPageValue() {
    if (objhdnID.value != "00000000-0000-0000-0000-000000000000") {
        //objddlType.disabled = true;
        //if ((objddlType.value == "F") || (objddlType.value == "")) { objtxtFromDate.disabled = true; objimgFromDt.style.display = "none"; }
        //else if (objddlType.value == "D") { objtxtFromDate.disabled = false; objimgFromDt.style.display = "inline"; }
        //CallBackOfflinePayment_Callback.callback("L", objhdnID.value, objddlAccount.value, objddlType.value);
    }

}

function btnPay_OnClick() {
    // do not call this when direct cc or ach
    if (["0", "1"].indexOf(objusingOption.value) > -1) return false;

    if (($("#vaultSelected").val() || "") == "") {
        parent.PopupMessage(RootDirectory, strForm, "btnPay_OnClick()", "Please select a card.", "true");
        return false;
    }
    if (($("#txtcvv").val() || "") == "") {
        parent.PopupMessage(RootDirectory, strForm, "btnPay_OnClick()", "Please enter CVV.", "true");
        return false;
    }
    if (!$("#txtcvv").val().match(/^\d{3,4}$/)) {
        parent.PopupMessage(RootDirectory, strForm, "btnPay_OnClick()", "Please enter CVV 3 to 4 digits long.", "true");
        return false;
    }
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    try {
        debugger;
        var billing_account_id = parent["__pg_billing_account_id__"];
        var data = parent["__pg_data__"];
        var invoices = [];
        if (data && data.invoices && data.invoices.length > 0) {
            invoices = data.invoices;
        }
        ArrRecords[0] = UserID;
        ArrRecords[1] = MenuID;
        ArrRecords[2] = objtxtAmount.value;
        ArrRecords[3] = objuserIp.value;
        ArrRecords[4] = JSON.stringify(invoices);
        ArrRecords[5] = "";
        ArrRecords[6] = "";
        ArrRecords[7] = "";
        ArrRecords[8] = "";
        ArrRecords[9] = "";
        ArrRecords[10] = "";
        ArrRecords[11] = "";
        ArrRecords[12] = "";
        ArrRecords[13] = "";
        ArrRecords[14] = "U";
        ArrRecords[15] = $("#vaultSelected").val();
        ArrRecords[16] = $("#txtcvv").val();
        ArrRecords[17] = objhdnHasVault.value;
        ArrRecords[18] = billing_account_id;
        AjaxPro.timoutPeriod = 1800000;
        VRSGetPaymentDlg.SaveRecord(ArrRecords, ShowProcess);

    }
    catch (expErr) {
        parent.HideProcess();
        parent.PopupMessage(RootDirectory, strForm, "btnPay_OnClick()", expErr.message, "true");
    }

}
function btnClose_OnClick() {
    delete parent["__pg_data__"];
    delete parent["__pg_billing_account_id__"];
    if (objhdnCF.value == "") {
        parent.GsNavURL = "MyPayment/VRSPayment.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID;
        if (parent.GsRetStatus == "false") {
            btnDlgClose_Onclick();
        }
        else {
            parent.GsDlgConfAction = "CLS";
            parent.PopupConfirm("028");
        }
    }
    else if (objhdnCF.value == "pp") {
        parent.GsNavURL = "Invoicing/VRSOfflinePaymentDlg.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=" + objhdnID.value + "&aid=" + objhdnAID.value;
        if (parent.GsRetStatus == "true") {
            parent.GsDlgConfAction = "CLS";
            parent.PopupConfirm("028");
        }
        else {
            delete parent["__pg_data__"];
            delete parent["__pg_billing_account_id__"];
            parent.PopupLoad();
            parent.objiframePage.src = parent.GsNavURL;
        }
    }
    
}
function btnSave_OnClick() {
    parent.PopupProcess("N");
    var ArrRecords = new Array();
    try {
        var billing_account_id = parent["__pg_billing_account_id__"];
        var data = parent["__pg_data__"];
        var invoices = [];
        if (data && data.invoices && data.invoices.length > 0) {
            invoices = data.invoices;
        }
        ArrRecords[0] = UserID;
        ArrRecords[1] = MenuID;
        ArrRecords[2] = objtxtAmount.value;
        ArrRecords[3] = objuserIp.value;
        ArrRecords[4] = JSON.stringify(invoices);
        ArrRecords[5] = objhdnToken.value;
        ArrRecords[6] = objfName.value;
        ArrRecords[7] = objlName.value;
        ArrRecords[8] = objAddress.value;
        ArrRecords[9] = objCity.value;
        ArrRecords[10] = objState.value;
        ArrRecords[11] = objCountry.value;
        ArrRecords[12] = objZip.value;
        ArrRecords[13] = objPhone.value;
        ArrRecords[14] = $("#checkSave").is(":checked") ? "TS" : null;
        ArrRecords[15] = objtrnType.value;
        ArrRecords[16] = objtrnItem1.value + "|" + objtrnItem2.value + "|" + objtrnItem3.value;
        ArrRecords[17] = objhdnHasVault.value;
        ArrRecords[18] = billing_account_id;
        AjaxPro.timoutPeriod = 1800000;
        VRSGetPaymentDlg.SaveRecord(ArrRecords, ShowProcess);

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
            objhdnID.value = arrRes[1];
            parent.GsRetStatus = "false";
            btnClose_OnClick();
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
function _restoreValues() {
    if (parent["__pg_data__"]) {
        var billing_account_id = parent["__pg_billing_account_id__"];
        var _data = parent["__pg_data__"];
        objhdnAID.value = billing_account_id;
        objtxtAmount.value = _data.amount.toFixed(parent.GiDecimal || 2);
        objlblAmount.innerText = "$" + _data.amount.toFixed(parent.GiDecimal || 2);
    }
}

function PreserveFilterValues() {

}