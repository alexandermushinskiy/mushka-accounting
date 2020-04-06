import { Component, OnInit } from '@angular/core';

import { AnalyticsService } from '../../core/api/analytics.service';
import { DatetimeService } from '../../core/datetime/datetime.service';
import { SoldProductsCount } from '../shared/models/sold-products-count.model';
import { OrdersCount } from '../shared/models/orders-count.model';
import { PopularCity } from '../shared/models/popular-city.model';
import { PopularProduct } from '../shared/models/popular-product.model';
import { Balance } from '../shared/models/balance.model';
import { chartOptions } from '../shared/constants/chart-options.const';

@Component({
  selector: 'mshk-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  chartOptions = chartOptions;
  defaultPeriod = 12;

  balanceData: Array<any> = [];
  balanceLabels: Array<string> = [];

  popularProductsData: Array<any> = [{ data: [], label: '' }];
  popularProductsLabels: Array<any> = [];

  unpopularProductsData: Array<any> = [{ data: [], label: '' }];
  unpopularProductsLabels: Array<any> = [];

  popularCitiesData: Array<any> = [{ data: [], label: '' }];
  popularCitiesLabels: Array<any> = [];

  ordersData: Array<any> = [{ data: [], label: '' }];
  ordersLabels: Array<string> = [];

  soldProductsData: Array<any> = [{ data: [], label: '' }];
  soldProductsLabels: Array<string> = [];

  constructor(private analyticsService: AnalyticsService,
              private datetimeService: DatetimeService) {
  }

  ngOnInit() {
    this.loadBalance();
    this.loadPopularProducts();
    this.loadUnpopularProducts();
    this.loadPopularCities();
    this.loadOrdersCount(chartOptions.defaultPeriod);
    this.loadSoldProductsCount(chartOptions.defaultPeriod);
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
