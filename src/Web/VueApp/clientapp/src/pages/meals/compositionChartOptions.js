export default {
    colors: ['#87CEEB', '#FA8072', '#F0E68C'],
    dataLabels: {
        style: {
            colors: ['#333', '#333', '#333']
        },
        dropShadow: {
            enabled: false
        }
    },
    labels: ['protein', 'carbohydrates', 'fat'],
    tooltip: {
        theme: 'light',
        style: {
            colors: ['#333', '#333', '#333']
        }
    },
    yaxis: {
        labels: {
            formatter: function (val) {
                return val + " kcal"
            }
        }
    },
    plotOptions: {
        pie: {
            donut: {
                labels: {
                    show: true,
                    total: {
                        show: true,
                        showAlways: true,
                        label: 'Total',
                        color: '#373d3f',
                        formatter(w) {
                            return w.globals.seriesTotals.reduce((a, b) => a + b, 0) + ' kcal'
                        }
                    }
                }
            }
        },

    }
}