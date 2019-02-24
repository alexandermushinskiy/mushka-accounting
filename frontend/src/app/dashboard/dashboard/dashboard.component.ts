import { Component, OnInit } from '@angular/core';

import { AnalyticsService } from '../../core/api/analytics.service';
import { PopularProduct } from '../shared/models/popular-product.model';
import { PopularCity } from '../shared/models/popular-city.model';
import { Balance } from '../shared/models/balance.model';

@Component({
  selector: 'mk-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  balanceData: Array<any> = [0, 0];
  balanceLabels: Array<string> = ['Потратили', 'Получили'];
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
 
  popularCitiesData: Array<any> = [{ data: [], label: ''}];
  popularCitiesLabels: Array<any> = [];
  popularCitiesColor: Array<any> = [{
    backgroundColor: 'rgb(255,127,14)'
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
  public lineChartLegend: boolean = true;
  public lineChartType: string = 'line';

  constructor(private analyticsService: AnalyticsService) { }

  ngOnInit() {
    this.loadBalance();
    this.loadPopularProducts();
    this.loadPopularCities();
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
  
  private loadPopularCities() {
    this.analyticsService.getPopularCities()
      .subscribe((res: PopularCity[]) => {
        this.popularCitiesLabels = res.map(pc => pc.city);
        this.popularCitiesData[0].data = res.map(pc => pc.quantity);
      });
  }
}
