import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { PopularProduct } from '../../dashboard/shared/models/popular-product.model';
import { PopularCity } from '../../dashboard/shared/models/popular-city.model';
import { Balance } from '../../dashboard/shared/models/balance.model';
import { OrdersCount } from '../../dashboard/shared/models/orders-count.model';
import { DatetimeService } from '../datetime/datetime.service';
import { SoldProductsCount } from '../../dashboard/shared/models/sold-products-count.model';

@Injectable()
export class AnalyticsService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/analytics`;

  constructor(private http: HttpClient,
              private datetimeService: DatetimeService) {
  }

  getBalance(): Observable<Balance> {
    return this.http.get(`${this.endPoint}/balance`).pipe(
      map((res: any) => this.convertToBalance(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getPopularProducts(): Observable<PopularProduct[]> {
    return this.http.get(`${this.endPoint}/popular/products`).pipe(
      map((res: any) => this.convertToPopularProducts(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getUnpopularProducts(): Observable<PopularProduct[]> {
    return this.http.get(`${this.endPoint}/unpopular/products`).pipe(
      map((res: any) => this.convertToPopularProducts(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getPopularCities(): Observable<PopularCity[]> {
    return this.http.get(`${this.endPoint}/popular/cities`).pipe(
      map((res: any) => this.convertToPopularCities(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getOrdersCount(period: number): Observable<OrdersCount[]> {
    return this.http.get(`${this.endPoint}/orders?period=${period}`).pipe(
      map((res: any) => this.convertToOrdersCount(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getSoldProductsCount(period: number): Observable<SoldProductsCount[]> {
    return this.http.get(`${this.endPoint}/sold-products?period=${period}`).pipe(
      map((res: any) => this.convertToSoldProductsCount(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  private convertToBalance(response: any): Balance {
    return new Balance({
      expense: response.expense,
      profit: response.profit
    });
  }

  private convertToPopularProducts(response: any[]): PopularProduct[] {
    return response.map(res => new PopularProduct({
      name: res.name,
      sizeName: res.sizeName,
      vendorCode: res.vendorCode,
      quantity: res.quantity,
    }));
  }

  private convertToPopularCities(response: any[]): PopularCity[] {
    return response.map(res => new PopularCity({
      city: res.city,
      quantity: res.quantity,
    }));
  }

  private convertToOrdersCount(response: any[]): OrdersCount[] {
    return response.map(res => new OrdersCount({
      createdOn: this.datetimeService.toString(res.createdOn),
      quantity: res.quantity
    }));
  }

  private convertToSoldProductsCount(response: any[]): SoldProductsCount[] {
    return response.map(res => new SoldProductsCount({
      createdOn: this.datetimeService.toString(res.createdOn),
      quantity: res.quantity
    }));
  }
}
