import { Component, OnInit } from '@angular/core';

import { AnalyticsService } from '../../core/api/analytics.service';
import { PopularProduct } from '../shared/models/popular-product.model';
import { PopularCity } from '../shared/models/popular-city.model';
import { Balance } from '../shared/models/balance.model';
import { OrdersCount } from '../shared/models/orders-count.model';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { SoldProductsCount } from '../shared/models/sold-products-count.model';

@Component({
  selector: 'mk-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  defaultChartOptions: any = {
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
  };
  defaultPeriod = 12;

  popularityChartOptions: any = {
    responsive: true,
    scales: {
      xAxes: [{
        ticks: {
          suggestedMin: 15,
          autoSkip: false
        }
      }]
    }
  };

  periods = [
    { period: 3, desc: '3 месяца' },
    { period: 6, desc: '6 месяцев' },
    { period: 12, desc: '12 месяцев' }
  ];

  balanceData: Array<any> = [];
  balanceLabels: Array<string> = [];
  balanceColor: Array<any> = [{
      backgroundColor: ['rgba(255,127,14,0.5)', 'rgba(134,199,243,0.5)'],
      borderColor: ['rgba(255,127,14,1)', 'rgba(134,199,243,1)'],
      borderWidth: 1
  }];
  balanceOptions: any = {
    responsive: true,
    tooltips: {
      enabled: false
    },
    legend: {
      display: true,
      position: 'right'
    }
  };

  popularProductsData: Array<any> = [{ data: [], label: '' }];
  popularProductsLabels: Array<any> = [];
  popularProductsColor: Array<any> = [{
    backgroundColor: 'rgb(134,199,243,0.5)',
    borderColor: 'rgba(134,199,243,1)',
    borderWidth: 1
  }];

  unpopularProductsData: Array<any> = [{ data: [], label: '' }];
  unpopularProductsLabels: Array<any> = [];
  unpopularProductsColor: Array<any> = [{
    backgroundColor: 'rgba(255,120,149,0.5)',
    borderColor: 'rgba(255,120,149,1)',
    borderWidth: 1
  }];

  popularCitiesData: Array<any> = [{ data: [], label: '' }];
  popularCitiesLabels: Array<any> = [];
  popularCitiesColor: Array<any> = [{
    backgroundColor: 'rgba(163,116,255,0.5)',
    borderColor: 'rgba(163,116,255,1)',
    borderWidth: 1
  }];

  ordersData: Array<any> = [{ data: [], label: '' }];
  ordersLabels: Array<string> = [];
  ordersColor: Array<any> = [{
    backgroundColor: 'rgba(255,127,14,0.2)',
    borderColor: 'rgba(255,127,14,1)',
    pointBackgroundColor: 'rgba(255,127,14,1)',
    pointBorderColor: '#fff',
    pointHoverBackgroundColor: '#fff',
    pointHoverBorderColor: 'rgba(255,127,14,0.8)'
  }];

  soldProductsData: Array<any> = [{ data: [], label: '' }];
  soldProductsLabels: Array<string> = [];
  soldProductsColor: Array<any> = [{
    backgroundColor: 'rgba(165,223,223,0.5)',
    borderColor: 'rgba(165,223,223,1)',
    borderWidth: 1
  }];

  constructor(private analyticsService: AnalyticsService,
              private datetimeService: DatetimeService) { }

  ngOnInit() {
    this.loadBalance();
    this.loadPopularProducts();
    this.loadUnpopularProducts();
    this.loadPopularCities();
    this.loadOrdersCount(this.defaultPeriod);
    this.loadSoldProductsCount(this.defaultPeriod);
  }

  onPeriodSelected(period: number) {
    this.loadOrdersCount(period);
  }

  private loadBalance() {
    this.analyticsService.getBalance()
      .subscribe((res: Balance) => {
        this.balanceData = [res.expense, res.profit];
        this.balanceLabels[0] = `Потратили\t\t\t: ${this.addThousandsSeparator(res.expense)}`;
        this.balanceLabels[1] = `Заработали\t: ${this.addThousandsSeparator(res.profit)}`;
      });
  }

  private loadPopularProducts() {
    this.analyticsService.getPopularProducts()
      .subscribe((res: PopularProduct[]) => {
        this.popularProductsLabels = res.map(pp => `${pp.name} (${pp.sizeName})`);
        this.popularProductsData[0].data = res.map(pp => pp.quantity);
      });
  }

  private loadUnpopularProducts() {
    this.analyticsService.getUnpopularProducts()
      .subscribe((res: PopularProduct[]) => {
        this.unpopularProductsLabels = res.map(pp => `${pp.name} (${pp.sizeName})`);
        this.unpopularProductsData[0].data = res.map(pp => pp.quantity);
      });
  }

  private loadPopularCities() {
    this.analyticsService.getPopularCities()
      .subscribe((res: PopularCity[]) => {
        this.popularCitiesLabels = res.map(pc => pc.city);
        this.popularCitiesData[0].data = res.map(pc => pc.quantity);
      });
  }

  private loadOrdersCount(period: number) {
    this.analyticsService.getOrdersCount(period)
      .subscribe((res: OrdersCount[]) => {
        this.ordersLabels = res.map(pc => this.datetimeService.getMonthName(pc.createdOn));
        this.ordersData[0].data = res.map(pc => pc.quantity);
      });
  }

  private loadSoldProductsCount(period: number) {
    this.analyticsService.getSoldProductsCount(period)
      .subscribe((res: SoldProductsCount[]) => {
        this.soldProductsLabels = res.map(pc => this.datetimeService.getMonthName(pc.createdOn));
        this.soldProductsData[0].data = res.map(pc => pc.quantity);
      });
  }

  private addThousandsSeparator(value: number): string {
    return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
  }
}
