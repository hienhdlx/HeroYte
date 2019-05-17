        Highcharts.chart('highChartColumn', {
        chart: {
            type: 'column',
            height: 280
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: [
                'Tháng 1',
                'Tháng 2',
                'Tháng 3',
                'Tháng 4',
                'Tháng 5'
            ],
            crosshair: true
        },
        yAxis: {
            min: 0,
            title: {
                text: ''
            }
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: [{
            name: 'Khám bảo hiểm y tế',
            data: [250, 430, 355, 250, 420]

        }, {
            name: 'Khám dịch vụ',
            data: [105, 210, 115, 130, 106]

        }]
    });
