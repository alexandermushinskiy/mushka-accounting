export const chartOptions = {
  defaultPeriod: 12,
  periods: [
    { period: 3, desc: '3 месяца' },
    { period: 6, desc: '6 месяцев' },
    { period: 12, desc: '12 месяцев' }
  ],
  defaultOptions: {
    responsive: true,
    scales: {
      // yAxes: [{
      //   ticks: {
      //     beginAtZero: true
      //   }
      // }],
      xAxes: [{
        ticks: {
          autoSkip: false
        }
      }]
    }
  },
  popularityOptions: {
    responsive: true,
    scales: {
      xAxes: [{
        ticks: {
          suggestedMin: 15,
          autoSkip: false
        }
      }]
    }
  },
  balanceColor: [{
    backgroundColor: ['rgba(255,127,14,0.5)', 'rgba(134,199,243,0.5)'],
    borderColor: ['rgba(255,127,14,1)', 'rgba(134,199,243,1)'],
    borderWidth: 1
  }],
  balanceOptions: {
    responsive: true,
    tooltips: {
      enabled: false
    },
    legend: {
      display: true,
      position: 'right'
    }
  },
  popularProductsColor: [{
    backgroundColor: 'rgb(134,199,243,0.5)',
    borderColor: 'rgba(134,199,243,1)',
    borderWidth: 1
  }],
  unpopularProductsColor: [{
    backgroundColor: 'rgba(255,120,149,0.5)',
    borderColor: 'rgba(255,120,149,1)',
    borderWidth: 1
  }],
  popularCitiesColor: [{
    backgroundColor: 'rgba(163,116,255,0.5)',
    borderColor: 'rgba(163,116,255,1)',
    borderWidth: 1
  }],
  ordersColor: [{
    backgroundColor: 'rgba(255,127,14,0.2)',
    borderColor: 'rgba(255,127,14,1)',
    pointBackgroundColor: 'rgba(255,127,14,1)',
    pointBorderColor: '#fff',
    pointHoverBackgroundColor: '#fff',
    pointHoverBorderColor: 'rgba(255,127,14,0.8)'
  }],
  soldProductsColor: [{
    backgroundColor: 'rgba(165,223,223,0.5)',
    borderColor: 'rgba(165,223,223,1)',
    borderWidth: 1
  }]

};
