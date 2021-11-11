// date range store
var currentDate = new Date().getDate() - 6;
var addDate = new Date().setDate(currentDate);
var todateRange = currentDate + 5;
var range = [new Date().setDate(todateRange - 29), new Date().setDate(todateRange)];
// default date format from setting compatible to Flatpicker
// if setting is 'dd-MM-yyyy' it will be 'd-m-Y'
// if setting is 'MM/dd/yyyy' it will be 'm/d/Y'

var dateFormat = parent.parent.GsDateFormat.replace(/y{1,}/, 'Y').replace(/M{1,}/, 'm').replace(/d{1,}/, 'd')
/*
* Initialize date controls from and to
*/
var fromDate, toDate;
var _y = (new Date()).getFullYear(), _fromD = new Date(_y, 0, 1), _toD = new Date();

fromDate = flatpickr("#fromDate", {
    defaultDate: range[0],
    enable: [{
        from: _fromD,
        to: range[1]
    }],
    dateFormat: dateFormat,

    onClose: function (selectedDates, dateStr, instance) {
        var from = selectedDates[0];
        if (toDate && from > toDate.selectedDates[0]) {
            toDate.setDate(from);
        }

        //$(".apply").trigger("click");
    }
});
toDate = flatpickr("#toDate", {
    defaultDate: range[1],
    dateFormat: dateFormat,
    enable: [{
        from: _fromD,
        to: range[1]
    }],
    onClose: function (selectedDates, dateStr, instance) {
        var to = selectedDates[0];
        if (to < fromDate.selectedDates[0]) {
            fromDate.setDate(to);
        }
    }
});
$('#rdoNumberName').prop('checked', true);
function callSlotTime() {
    var strDtFrom = moment(fromDate.selectedDates[0], 'date').format("DDMMMyyyy");
    var strDtTo = moment(toDate.selectedDates[0], 'date').format("DDMMMyyyy");
    var orderByQry = 'name';
    var ArrRecords = new Array();
    ArrRecords[0] = strDtFrom;
    ArrRecords[1] = strDtTo;
    ArrRecords[2] = $('#ddlModality option:selected').val();
    if ($('#rdoNumberCases').prop('checked'))
        orderByQry = '';
    ArrRecords[3] = orderByQry;
    VRSRadiologistProductivity.SearchRecord(ArrRecords, apply_callback);
}
function apply_callback(lineData) {
    if (lineData.value.Columns.length > 0) {
        var th = document.getElementById("dynamicColspan");
        th.setAttribute("colspan", lineData.value.Columns.length);
    }
    var setTh = '';
    for (var i = 0; i < lineData.value.Columns.length - 1; i++) {
        setTh += '<th>' + lineData.value.Columns[i + 1].Name + '</th>';
    }
    //setTh += '<th>Total</th>';
    $('#dynamicColumn').html(setTh);

    var setTr = '';
    var trTotal = 0;
    for (var i = 0; i < lineData.value.Rows.length; i++) {
        trTotal = 0;
        setTr += '<tr>' +
        '<td style="width:21%;">' + lineData.value.Rows[i].radiologist_name + '</td>' +
        '<td colspan="' + lineData.value.Columns.length + '" style="padding: 0; border-left: 1px solid #cecece;">' +
            '<table class="tbody-inner" style="width: 100%;">' +
            '<tr style="border-bottom: none;">';
        for (var j = 0; j < lineData.value.Columns.length - 1 ; j++) {
            if (lineData.value.Rows[i][lineData.value.Columns[j + 1].Name] != null) {
                if (lineData.value.Columns.length - 2 == j) {
                    setTr += '<td style="width:22%;">' + lineData.value.Rows[i][lineData.value.Columns[j + 1].Name] + '</td>';
                } else {
                    setTr += '<td style="width:18%;">' + lineData.value.Rows[i][lineData.value.Columns[j + 1].Name] + '</td>';
                }
                
            }
            else {
                setTr += '<td style="width:18%;">' + 0 + '</td>';
            }
        }

        setTr += '</tr>' +
            '</table>' +
            '</td>' +
        '</tr>';
    }
    
    if (lineData.value.Rows.length == 0) {
        setTr = '';
        setTr += '<tr>' +
        '<td>No Data Found</td>' +
        '</tr>';
    }
    $('.tbody-main').html(setTr);
}
callSlotTime();
if ($('#hdnreportTitle').val() === '')
    parent.$('#txtDescription').html('<span style="font-size:18px;">' + $('#hdnDesc').val() + '</span>');
else
    parent.$('#txtDescription').html('<span style="font-size:18px;">' + $('#hdnreportTitle').val() + '</span>');

$(document).ready(function () {
    if ($('#hdnIsRefreshBtn').val() == 'Y')
        parent.$('#btnReset').css('display', 'inline');
    else
        parent.$('#btnReset').css('display', 'none');
    parent.$('#btnReset').click(function () {
        callSlotTime();
    });
    if (parseInt($('#hdnRefreshTime').val()) > 0) {
        setInterval(function () {
            callSlotTime();
        }, $('#hdnRefreshTime').val() * 1000);
    }

});