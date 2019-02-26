import { Component, OnInit } from '@angular/core';

import { AnalyticsService } from '../../core/api/analytics.service';
import { PopularProduct } from '../shared/models/popular-product.model';
import { PopularCity } from '../shared/models/popular-city.model';
import { Balance } from '../shared/models/balance.model';
import { OrdersCount } from '../shared/models/orders-count.model';
import { DatetimeService } from '../../core/datetime/datetime.service';

@Component({
  selector: 'mk-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  defaultPeriod = 12;
  periods = [
    { period: 3, desc: '3 месяца' },
    { period: 6, desc: '6 месяцев' },
    { period: 12, desc: '12 месяцев' }];

  balanceData: Array<any> = [0, 0];
  balanceLabels: Array<string> = ['Потратили', 'Заработали'];
  balanceColor: Array<any> = [
    { backgroundColor: [ 'rgb(255,127,14)', 'rgb(134,199,243)'] }
  ];
  balanceOptions: any = {
    responsive: true,
    legend: {
      display: true,
      position: 'right'
    }
  };

  popularProductsData: Array<any> = [{ data: [], label: '' }];
  popularProductsLabels: Array<any> = [];
  popularProductsColor: Array<any> = [{
    backgroundColor: 'rgb(134,199,243)'
  }];
 
  unpopularProductsData: Array<any> = [{ data: [], label: '' }];
  unpopularProductsLabels: Array<any> = [];
  unpopularProductsColor: Array<any> = [{
    backgroundColor: 'rgba(255,120,149,0.6)'
  }];
 
  popularCitiesData: Array<any> = [{ data: [], label: '' }];
  popularCitiesLabels: Array<any> = [];
  popularCitiesColor: Array<any> = [{
    backgroundColor: 'rgba(163,116,255,0.5)',
    borderColor: 'rgba(163,116,255,1)'
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

  public lineChartData:Array<any> = [
    {data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A'},
    {data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B'}
  ];
  public lineChartLabels:Array<any> = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль'];
  public lineChartOptions:any = {
    responsive: true
  };
  public lineChartColors:Array<any> = [
    { // grey
      backgroundColor: 'rgba(99,123,217,0.2)',
      borderColor: 'rgba(99,123,217,1)',
      pointBackgroundColor: 'rgba(99,123,217,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(99,123,217,0.8)'
    },
    { // dark grey
      backgroundColor: 'rgba(77,83,96,0.2)',
      borderColor: 'rgba(77,83,96,1)',
      pointBackgroundColor: 'rgba(77,83,96,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(77,83,96,1)'
    }
  ];

  constructor(private analyticsService: AnalyticsService,
              private datetimeService: DatetimeService) { }

  ngOnInit() {
    this.loadBalance();
    this.loadPopularProducts();
    this.loadUnpopularProducts();
    this.loadPopularCities();
    this.loadOrdersCount(this.defaultPeriod);
  }

  onPeriodSelected(period: number) {
    this.loadOrdersCount(period);
  }

  private loadBalance() {
    this.analyticsService.getBalance()
      .subscribe((res: Balance) => {
        this.balanceData = [res.expense, res.profit];
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
}
