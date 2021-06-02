import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { Balance } from '../../../dashboard/models/balance.model';
import { OrdersCount } from '../../../dashboard/models/orders-count.model';
import { PopularCity } from '../../../dashboard/models/popular-city.model';
import { PopularProduct } from '../../../dashboard/models/popular-product.model';
import { SoldProductsCount } from '../../../dashboard/models/sold-products-count.model';
import { ApiGetBalance } from '../interfaces/api-get-balance.interface';
import { ApiAnalyticsTransformService } from './api-analytics-transform.service';

@Injectable({
  providedIn: 'root'
})
export class ApiAnalyticsService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/analytics`;

  constructor(private http: HttpClient,
              private transformService: ApiAnalyticsTransformService) {
  }

  getBalance$(): Observable<Balance> {
    return this.http.get(`${this.endPoint}/balance`)
      .pipe(
        map((res: ApiGetBalance.Response) => this.transformService.fromGetBalance(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getPopularProducts$(): Observable<PopularProduct[]> {
    return this.http.get(`${this.endPoint}/popular-products`)
      .pipe(
        map((res: any) => this.transformService.fromGetPopularProducts(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getUnpopularProducts$(): Observable<PopularProduct[]> {
    return this.http.get(`${this.endPoint}/unpopular-products`)
      .pipe(
        map((res: any) => this.transformService.fromGetPopularProducts(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getPopularCities$(): Observable<PopularCity[]> {
    return this.http.get(`${this.endPoint}/popular-cities`)
      .pipe(
        map((res: any) => this.transformService.fromGetPopularCities(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getOrdersCount$(period: number): Observable<OrdersCount[]> {
    return this.http.get(`${this.endPoint}/orders?period=${period}`)
      .pipe(
        map((res: any) => this.transformService.fromGetOrdersCount(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getSoldProductsCount$(period: number): Observable<SoldProductsCount[]> {
    return this.http.get(`${this.endPoint}/sold-products?period=${period}`)
      .pipe(
        map((res: any) => this.transformService.toGetSoldProductsCount(res)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }
}
