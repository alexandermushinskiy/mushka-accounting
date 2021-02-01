import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ApiCorporateOrdersTransformService } from './api-corporate-orders-transform.service';
import { CorporateOrderList } from '../../../shared/models/corporate-order-list.model';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { ApiSearchCorporateOrders } from '../interfaces/api-search-corporate-orders.interface';
import { CorporateOrder } from '../../../shared/models/corporate-order.model';

@Injectable({
  providedIn: 'root'
})
export class ApiCorporateOrdersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/corporate-orders`;

  constructor(private http: HttpClient,
              private transformService: ApiCorporateOrdersTransformService) {
  }

  searchOrders$(): Observable<ItemsList<CorporateOrderList>>  {
    const url = `${this.endPoint}/search`;

    return this.http.post(url, {})
      .pipe(
        map((data: ApiSearchCorporateOrders.Response) => this.transformService.fromSearchOrders(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getOrder$(orderId: string): Observable<CorporateOrder> {
    return this.http.get(`${this.endPoint}/${orderId}`)
      .pipe(
        map((data: any) => this.transformService.fromGetOrder(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  validateOrderNumber$(orderNumber: string): Observable<boolean> {
    const url = `${this.endPoint}/validate-number`;

    return this.http.post(url, this.transformService.toValidateOrderNumber(orderNumber))
      .pipe(
        map((data: any) => data.isValid),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  deleteOrder$(orderId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${orderId}`)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  createOrder$(order: CorporateOrder): Observable<void> {
    return this.http.post<void>(this.endPoint, this.transformService.toCreateOrder(order))
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  updateOrder$(orderId: string, order: CorporateOrder): Observable<void> {
    return this.http.put<void>(`${this.endPoint}/${orderId}`, this.transformService.toUpdateOrder(order))
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }
}
