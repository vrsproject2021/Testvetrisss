
function callSlotTime() {
    
    var ArrRecords = new Array();
    ArrRecords[0] = $('#last_month option:selected').val();
    ArrRecords[1] = $('#ddlModality option:selected').val();
    VRSModalityRevenue.SearchRecord(ArrRecords, apply_callback);
}
function apply_callback(lineData) {
    data = {
        title: 'Time',
        divId: 'line_chart',
        data: lineData.value,
        xAxisTitle: '',
        fillColor: $('#hdnTheme').val() == 'DARK' ? '#2C2C2C' : '#fff',
        isDark: $('#hdnTheme').val() == 'DARK' ? true : false
        //chartType: $('#ctp option:selected').val()
    };
    monthlyRevenueChart(data);
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