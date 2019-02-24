import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';
import { PopularProduct } from '../../shared/models/popular-product.model';

@Injectable()
export class AnalyticsService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/analytics`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getPopular(): Observable<PopularProduct[]> {
    return this.http.get(`${this.endPoint}/popular`)
      .map((res: any) => this.convertToPopularProducts(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  private convertToPopularProducts(response: any[]): PopularProduct[] {
    return response.map(res => new PopularProduct({
      name: res.name,
      sizeName: res.sizeName,
      vendorCode: res.vendorCode,
      quantity: res.quantity,
    }));
  }
}
