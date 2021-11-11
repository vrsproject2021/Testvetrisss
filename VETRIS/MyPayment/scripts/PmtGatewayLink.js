var DateFmt = parent.objhdnDateFormat.value;
var DateSep = parent.objhdnDateSep.value;
var strControlCode = ""; var objCtrl;
var selected_rows = [];
var arrData = new Array();

function CheckError() { FetchInvoiceData(); }

function onPayNowTotal() {
    var total = parseFloat(objhdnTotAmt.value);
    if (total <= 0) {
        parent.PopupMessage(RootDirectory, strForm, "onPayNowTotal()", "315", "false");
        parent.GsRetStatus = "false";
        parent.NavMenu("MyPayment/VRSPayment.aspx",62,"N",1);
        return false;
    }
    if (selected_rows.length == 0) CreateAdjustments(total);
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
        for (var i = 0; i < arrData.length; i = i + 4) {
            var balance = 0;

            var invoice_header_id = arrData[i];
            var invoice_no = arrData[i + 1];
            var invoice_date = arrData[i + 2];
            balance = parseFloat(parent.SetDecimalFormat(arrData[i + 3]));

            if (tobeadjusted == 0) break;
            if (balance >= tobeadjusted) {
                selected_rows.push({ invoice_header_id: invoice_header_id, invoice_no: invoice_no, invoice_date: invoice_date, adj_amount: tobeadjusted });
                tobeadjusted = 0;
                break;
            } else {
                selected_rows.push({ invoice_header_id: invoice_header_id, invoice_no: invoice_no, invoice_date: invoice_date, adj_amount: balance });
                tobeadjusted = parseFloat((tobeadjusted - balance).toFixed(parent.GiDecimal || 2));
            }
        }
    }
}

function FetchInvoiceData() {
    var Result;
    var arrRes = new Array();
    var idx = 0;

    try {
        AjaxPro.timeoutPeriod = 1800000;
        Result = VRSPmtGatewayLink.FetchInvoices(objhdnID.value, UserID);
        arrRes = Result.value;

        switch (arrRes[0]) {
            case "catch":
            case "false":
                document.getElementById("tdCalc").style.display = "none";
                document.getElementById("tdBlank").style.display = "inline-block";
                parent.PopupMessage(RootDirectory, strForm, "FetchInvoiceData()", arrRes[1], "true");
                break;
            case "true":
                parent.GsRetStatus = "false";
                document.getElementById("tdCalc").style.display = "none";
                document.getElementById("tdRedirect").style.display = "inline-block";
                for (var i = 1; i < arrRes.length - 1; i = i + 4) {
                    arrData[idx] = arrRes[i];
                    arrData[idx + 1] = arrRes[i + 1];
                    arrData[idx + 2] = arrRes[i + 2];
                    arrData[idx + 3] = arrRes[i + 3];
                    idx = idx + 4;
                }
                objhdnTotAmt.value = arrRes[arrRes.length - 1];
                
                if (onPayNowTotal()) {
                    parent.GsIsBrowser = "N";
                    parent.objiframePage.src = "MyPayment/VRSGetPaymentDlg.aspx?uid=" + UserID + "&urid=" + UserRoleID + "&mid=" + MenuID + "&id=00000000-0000-0000-0000-000000000000&aid=" + objhdnID.value;
                }
                break;
        }
    }
    catch (expErr) {
        parent.PopupMessage(RootDirectory, strForm, "FetchInvoiceData()", expErr.message, "true");
    }

}