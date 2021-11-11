google.charts.load('visualization', '1.0', { packages: ["corechart"] });

getServerTime();
//parent.$('body').css('overflow', 'scroll');
//parent.parent.$('body').css('overflow', 'hidden');
//parent.$('body').css('overflow-x', 'hidden');
$(document).ready(function () {
    if ($('#hdnIsRefreshBtn').val() == 'Y')
        parent.$('#btnReset').css('display', 'inline');
    else
        parent.$('#btnReset').css('display', 'none');
    parent.$('#btnReset').click(function () {
        cache_clear();
    });
    if (parseInt($('#hdnRefreshTime').val()) > 0) {
        setInterval(function () {
            cache_clear();
        }, $('#hdnRefreshTime').val() * 1000);
    }
});

function cache_clear() {
    getServerTime();
}
if ($('#hdnreportTitle').val() === '')
    parent.$('#txtDescription').html('<span style="font-size:18px;">' + $('#hdnDesc').val() + '</span>');
else
    parent.$('#txtDescription').html('<span style="font-size:18px;">' + $('#hdnreportTitle').val() + '</span>');

var chartData = [];
function getServerTime() {
    //VRSCases.GetDashboardOpenCase($('#hdnMenuId').val(), getServerTime_callback);
    VRSCases.GetDashboardOpenCase($('#hdnMenuId').val(), createArrayOfData_callback);
}

function getServerTime_callback(dataObj) {

    /*Normal case*/
    charts.pie('pieCaseNormal', 'Modality', dataObj.value.modality_open_case_normal)

    charts.pie('pieCaseStatusNormal', 'Status', dataObj.value.modality_status_normal)

    charts.pie('pieCaseTimeNormal', 'Elapsed Time(Minutes)', dataObj.value.elapsed_time_normal)

    /*Stat case*/

    charts.pie('pieCaseStat', 'Modality', dataObj.value.modality_open_case_stat)

    charts.pie('pieCaseStatusStat', 'Status', dataObj.value.modality_status_stat)

    charts.pie('pieCaseTimeStat', 'Elapsed Time(Minutes)', dataObj.value.elapsed_time_stat)

}

function createArrayOfData_callback(dataObj) {
    chartData = [];
    var data = {
        title: 'Modality',
        divId: 'pieCaseNormal',
        data: dataObj.value.modality_open_case_normal,
        fillColor: $('#hdnTheme').val() == 'DARK' ? '#2C2C2C' : '#fff',
        isDark: $('#hdnTheme').val() == 'DARK' ? true : false
    };
    chartData.push(data);
    data = {
        title: 'Status',
        divId: 'pieCaseStatusNormal',
        data: dataObj.value.modality_status_normal,
        fillColor: $('#hdnTheme').val() == 'DARK' ? '#2C2C2C' : '#fff',
        isDark: $('#hdnTheme').val() == 'DARK' ? true : false
};
    chartData.push(data);
    data = {
        title: 'Elapsed Time (Minutes)',
        divId: 'pieCaseTimeNormal',
        data: dataObj.value.elapsed_time_normal,
        fillColor: $('#hdnTheme').val() == 'DARK' ? '#2C2C2C' : '#fff',
        isDark: $('#hdnTheme').val() == 'DARK' ? true : false
    };
    chartData.push(data);
    data = {
        title: 'Modality',
        divId: 'pieCaseStat',
        data: dataObj.value.modality_open_case_stat,
        fillColor: $('#hdnTheme').val() == 'DARK' ? '#2C2C2C' : '#fff',
        isDark: $('#hdnTheme').val() == 'DARK' ? true : false
    };
    chartData.push(data);
    data = {
        title: 'Status',
        divId: 'pieCaseStatusStat',
        data: dataObj.value.modality_status_stat,
        fillColor: $('#hdnTheme').val() == 'DARK' ? '#2C2C2C' : '#fff',
        isDark: $('#hdnTheme').val() == 'DARK' ? true : false
    };
    chartData.push(data);
    data = {
        title: 'Elapsed Time (Minutes)',
        divId: 'pieCaseTimeStat',
        data: dataObj.value.elapsed_time_stat,
        fillColor: $('#hdnTheme').val() == 'DARK' ? '#2C2C2C' : '#fff',
        isDark: $('#hdnTheme').val() == 'DARK' ? true : false
    };
    chartData.push(data);
    for (var i = 0; i < chartData.length; i++) {
        loadGoogleChart(chartData[i]);
    }

}