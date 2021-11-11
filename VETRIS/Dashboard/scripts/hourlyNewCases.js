// date range store
var currentDate = new Date().getDate() - 6;
var addDate = new Date().setDate(currentDate);
var range = [new Date(addDate), new Date()];
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
        to: _toD
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
        to: _toD
    }],
    onClose: function (selectedDates, dateStr, instance) {
        var to = selectedDates[0];
        if (to < fromDate.selectedDates[0]) {
            fromDate.setDate(to);
        }
    }
});
function callSlotTime() {
    var strDtFrom = moment(fromDate.selectedDates[0], 'date').format("DDMMMyyyy");
    var strDtTo = moment(toDate.selectedDates[0], 'date').format("DDMMMyyyy");

    var ArrRecords = new Array();
    ArrRecords[0] = strDtFrom;
    ArrRecords[1] = strDtTo;
    ArrRecords[2] = $('#ddlModality option:selected').val();
    VRSHourlyNewCases.SearchRecord(ArrRecords, apply_callback);
}
function apply_callback(lineData) {
    data = {
        title: 'Time',
        divId: 'line_chart',
        data: lineData.value,
        xAxisTitle: '',
        chartType: $('#ctp option:selected').val(),
        fillColor: $('#hdnTheme').val() == 'DARK' ? '#2C2C2C' : '#fff',
        isDark: $('#hdnTheme').val() == 'DARK' ? true : false
    };
    loadGoogleLineChart(data);
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