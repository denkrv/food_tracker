export default {
    chart: {
        height: 350,
        type: 'bar',
        stacked: true,
        toolbar: {
            show: false
        }
    },
    plotOptions: {
        bar: {
            horizontal: true,
        },

    },
    stroke: {
        width: 1,
        colors: ['#fff']
    },
    title: {
        text: 'estimated daily values'
    },
    xaxis: {
        categories: ['protein', 'carbohydrates', 'fat'],
        labels: {
            formatter: function (val) {
                return val + " g"
            }
        }
    },
    yaxis: {
        title: {
            text: undefined
        },

    },
    tooltip: {
        y: {
            formatter: function (val) {
                return val + " g"
            }
        }
    },
    fill: {
        opacity: 1
    },

    legend: {
        position: 'top',
        horizontalAlign: 'left',
        offsetX: 40
    }
}