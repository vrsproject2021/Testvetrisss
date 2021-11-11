function loadGoogleChart(data) {

    google.charts.setOnLoadCallback(function () { drawPieChart(data); });
}
function drawPieChart(chartData) {
    var mapdata = [[chartData.title, "No of cases"]];
    chartData.data
        .map(function (i) { return [i.name, i.value]; })
        .forEach(function (i) {
            mapdata.push(i);
        });
    var data = google.visualization.arrayToDataTable(mapdata);
    var options = {
        //title: chartData.title,
        pieHole: 0.4,
        legend: {
            position: 'top',
            alignment: 'start',
            textStyle: {
                fontSize: 10,
                color: chartData.isDark ? '#fff' : '#222222'
            },
            orientation: 'vertical',
            maxLines: 2
        },
        is3D: true,
        chartArea: {
            bottom: 0, width: '50%', height: '75%', backgroundColor: {
                fill: chartData.fillColor,
                fillOpacity: 0.1
            },
        },
        backgroundColor: {
            fill: chartData.fillColor,
            //fillOpacity: 0.8
        },
        //titleTextStyle: {
        //    textAlign: 'center',
        //    color: '#2185d0',
        //    fontSize: 12, 
        //    bold: true,  
        //}
    };

    var chart = new google.visualization.PieChart(document.getElementById(chartData.divId));

    chart.draw(data, options);

    $("text:contains(" + chartData.title + ")")
        .attr({ 'x': '130', 'y': '27.85' });
}

function loadGoogleLineChart(data) {
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(function () { drawLineChart(data); });
}
function drawLineChart(chartData) {
    if (chartData.data.datasets.length == 0) {
        $('#' + chartData.divId).html('No data found');
        return;
    }
    var data = new google.visualization.DataTable();
    // transpose of data
    var matrix = new Array(chartData.data.labels.length + 1).fill(); // rows + heading (1)
    matrix.forEach(function (_, row) {
        matrix[row] = new Array(chartData.data.datasets.length + 1).fill();
        if (row == 0) {//heading
            matrix[row][0] = "Hours";
            for (var col = 0; col < chartData.data.datasets.length; col++) {
                matrix[row][col + 1] = chartData.data.datasets[col].label;
            }
        }
        else {
            matrix[row][0] = chartData.data.labels[row - 1];

            for (var col = 0; col < chartData.data.datasets.length; col++) {
                matrix[row][col + 1] = chartData.data.datasets[col].data[row - 1];
            }
        }
    });


    for (var i = 0; i < matrix[0].length; i++) {
        data.addColumn(i == 0 ? 'string' : 'number', matrix[0][i]);
    }
    for (var i = 1; i < matrix.length; i++) {
        data.addRow(matrix[i]);
    }

    var options = {
        //chart: {
        //    title: 'Case - Time slot Frequency',

        //},
        //title: 'Case - Time-slot Frequency',
        backgroundColor: {
            fill: chartData.fillColor,
        },
        seriesType: chartData.chartType,
        hAxis: {
            title: chartData.xAxisTitle,
            textStyle: {
                fontSize: 8, // or the number you want
                color: chartData.isDark ? '#fff' : '#222222'
            },
            titleTextStyle: { color: '#2185d0', fontSize: 10, },
        },
        vAxis: {
            title: 'Number of Cases',
            baseline: 0,
            textStyle: { color: chartData.isDark ? '#fff' : '#222222' },
            titleTextStyle: { color: chartData.isDark ? '#fff' : '#222222' }
        },
        legend: {
            textStyle: { color: chartData.isDark ? '#fff' : '#222222' },
        },
    };

    var chart = new google.visualization.ComboChart(document.getElementById(chartData.divId));
    chart.draw(data, options);
    $("text:contains(" + chartData.title + ")").attr({ 'x': '120', 'y': '27.85' })
    $("text:contains(" + "Note : Only FINAL Cases are considered" + ")").attr({ 'x': '144', 'y': '370.5' })
}

function monthlyRevenueChart(data) {
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(function () { drawRevenueChart(data); });
}
function drawRevenueChart(chartData) {
    if (chartData.data == null || chartData.data.Rows.length==0) {
        $('#' + chartData.divId).html('No data found');
        return;
    }
    
    
    var barColors = new Array(chartData.data.Rows.length).fill().map(function (e, i) { return Colors.select(i).rgb; });
    var matrix = new Array(chartData.data.Rows.length + 1).fill(); // rows + heading (1)
    matrix.forEach(function (_, row) {
        matrix[row] = new Array(chartData.data.Columns.length).fill();
        if (row == 0) {//heading
            matrix[row] = chartData.data.Columns.map(function (i) { return i.Name });
        }
        else {
            matrix[row] = Object.keys(chartData.data.Rows[row - 1]).map(function (i) { return chartData.data.Rows[row - 1][i] });
        }
    });
    var data = new google.visualization.arrayToDataTable(matrix);
   

    var options = {
        backgroundColor: {
            fill: chartData.fillColor,
        },
        hAxis: {
            title: chartData.xAxisTitle,
            textStyle: {
                fontSize: 8.5, // or the number you want
                bold: true,
                color: chartData.isDark ? '#fff' : '#222222'
            },
            titleTextStyle: { color: '#2185d0', fontSize: 10, },
        },
        vAxis: {
            title: 'Revenue (US $)',
            baseline: 0,
            textStyle: {
                bold: true,
                color: chartData.isDark ? '#fff' : '#222222'
            },
            bold: true,
            titleTextStyle: { color: chartData.isDark ? '#fff' : '#222222' }
        },
        bars:'vertical',
        isStacked: true,
        legend: {
            textStyle: { color: chartData.isDark ? '#fff' : '#222222' },
        },
    };
    
    var chart = new google.visualization.ColumnChart(document.getElementById(chartData.divId));
    chart.draw(data, options);
    $("text:contains(" + chartData.title + ")").attr({ 'x': '120', 'y': '27.85' })
}

var charts = {

}
charts.pie = function (div, title, data) {
    var xValues = data.map(function (i) { return i.name });
    var yValues = data.map(function (i) { return i.value });
    var barColors = new Array(data.length).fill().map(function (e, i) { return Colors.select(i).rgb; });
    var options = {
        responsive: true,
        tooltips: {
            enabled: false
        },
        plugins: {
            datalabels: {
                formatter: function (value, ctx) {
                    let datasets = ctx.chart.data.datasets;
                    if (datasets.indexOf(ctx.dataset) === datasets.length - 1) {
                        let sum = datasets[0].data.reduce(function (a, b) { a + b, 0 });
                        let percentage = Math.round((value / sum) * 100) + '%';
                        return percentage;
                    } else {
                        return percentage;
                    }
                },
                color: '#fff',
            }
        },
        legend: {
            position: 'right',
            labels: {
                "fontSize": 10,
                "boxWidth": 10,

            },
            fontColor: '#fff',
        },
        title: {
            display: true,
            text: title
        }
    };
    new Chart(div, {
        type: "doughnut",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]

        },
        options: options
    });
};

charts.line = function (div, title, linedata) {
    var data = linedata.value;
    // prepare line data with color and tension
    data.datasets = data.datasets.map(function (d, i) {
        return Object.assign(d, {
            fill: false,
            borderColor: Colors.select(i).rgb,
            tension: 0.1
        });
    });

    new Chart(div, {
        type: "line",
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: title
                }
            },
            scales: {
                xAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: 'Hours'
                    }
                }],
                yAxes: [{
                    scaleLabel: {
                        display: true,
                        labelString: 'Number of Cases'
                    }
                }]
            }
        }
    });
};

Colors = {};
Colors.names = {
    or1: "#ff5722",
    b2: "#4bc0c0",
    g1: "#12a679",
    p1: "#ff6384",
    b1: "#36a2eb",
    yg1: "#cddc39",
    p2: "#e91e63",
    y3: "#ff9f40",
    g2: "#009688",
    green: "#0ea77d",
    red: "#eb4c2d",
    blue: "#3b69aa",
    yellow: "#f3b801",
    orange: "#ffc800",
    violet: "#803d8c",
    purple: "#800080",
    pink: "#e0115e",
    aqua: "#00ffff",
    azure: "#f0ffff",
    beige: "#f5f5dc",
    brown: "#a52a2a",
    cyan: "#00ffff",
    darkblue: "#00008b",
    darkcyan: "#008b8b",
    darkgrey: "#a9a9a9",
    darkgreen: "#006400",
    darkkhaki: "#bdb76b",
    darkmagenta: "#8b008b",
    darkolivegreen: "#556b2f",
    darkorange: "#ff8c00",
    darkorchid: "#9932cc",
    darkred: "#8b0000",
    darksalmon: "#e9967a",
    darkviolet: "#9400d3",
    gold: "#ffd700",
    indigo: "#4b0082",
    khaki: "#f0e68c",
    lightblue: "#add8e6",
    lightcyan: "#e0ffff",
    lightpink: "#ffb6c1",
    lightyellow: "#ffffe0",
    lime: "#00ff00",
    magenta: "#ff00ff",
    maroon: "#800000",
    navy: "#000080",
    olive: "#808000",

};
Colors.select = function (index) {
    var result = null;
    var count = 0;
    for (var prop in this.names) {
        if (count == index) {
            result = prop;
            break;
        }
        count++;
    }
    if (!result) result = "red";

    return { name: result, rgb: this.names[result] };
};