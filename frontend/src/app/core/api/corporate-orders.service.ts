import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError, delay } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';
import { CorporateOrderList } from '../../corporate-orders/shared/models/corporate-order-list.model';
import { CorporateOrder } from '../../corporate-orders/shared/models/corporate-order.model';

@Injectable()
export class CorporateOrdersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/corporate-orders`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getAll(): Observable<CorporateOrderList[]>  {
    return this.http.get(this.endPoint).pipe(
      map((res: any) => this.converterService.convertToCorporateOrdersList(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getById(productId: string): Observable<CorporateOrder> {
    return this.http.get(`${this.endPoint}/${productId}`).pipe(
      map((res: any) => this.converterService.convertToCorporateOrder(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  create(order: CorporateOrder): Observable<CorporateOrder> {
    return this.http.post(this.endPoint, order).pipe(
      map((res: any) => this.converterService.convertToCorporateOrder(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  update(orderId: string, order: CorporateOrder): Observable<CorporateOrder> {
    return this.http.put(`${this.endPoint}/${orderId}`, order).pipe(
      map((res: any) => this.converterService.convertToCorporateOrder(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  delete(orderId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${orderId}`).pipe(
      catchError((res: any) => throwError(res.error.messages)));
  }

  validateOrderNumber(number: string ): Observable<boolean> {
    return this.http.get(`${this.endPoint}/validate-number/${number}`).pipe(
      map((res: any) => res.data.isValid),
      catchError((res: any) => throwError(res.error.messages)),
      delay(1000));
  }
}
