import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';
import { PopularProduct } from '../../dashboard/shared/models/popular-product.model';
import { PopularCity } from '../../dashboard/shared/models/popular-city.model';
import { Balance } from '../../dashboard/shared/models/balance.model';

@Injectable()
export class AnalyticsService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/analytics`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getBalance(): Observable<Balance> {
    return this.http.get(`${this.endPoint}/balance`)
      .map((res: any) => this.convertToBalance(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getPopularProducts(): Observable<PopularProduct[]> {
    return this.http.get(`${this.endPoint}/popular/products`)
      .map((res: any) => this.convertToPopularProducts(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getPopularCities(): Observable<PopularCity[]> {
    return this.http.get(`${this.endPoint}/popular/cities`)
      .map((res: any) => this.convertToPopularCities(res.data))
      .catch((res: any) => throwError(res.error.messages));
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
}
