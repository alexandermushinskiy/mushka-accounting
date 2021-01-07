import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../../environments/environment';
import { OrdersSearchParams } from '../../../../orders/shared/interfaces/orders-search-params.interface';
import { ItemsList } from '../../../../shared/interfaces/items-list.interface';
import { OrderList } from '../../../../shared/models/order-list.model';
import { OrderProduct } from '../../../../shared/models/order-product.model';
import { Order } from '../../../../shared/models/order.model';
import { ApiOrdersTransformService } from './api-orders-transform.service';

@Injectable({
  providedIn: 'root'
})
export class ApiOrdersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/orders`;

  constructor(private http: HttpClient,
              private transformService: ApiOrdersTransformService) {
  }

  searchOrders$(searchParams: OrdersSearchParams): Observable<ItemsList<OrderList>> {
    const body = this.transformService.toSearchOrders(searchParams);
    const url = `${this.endPoint}/search`;

    return this.http.post(url, body)
      .pipe(
        map((data: any) => this.transformService.fromSearchOrders(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  deleteOrder$(orderId: string): Observable<void> {
    const url = `${this.endPoint}/${orderId}`;

    return this.http.delete<void>(url)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getOrder$(orderId: string): Observable<Order> {
    const url = `${this.endPoint}/${orderId}`;

    return this.http.get(url)
      .pipe(
        map((res: any) => this.transformService.fromGetOrder(res.data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  getDefaultProducts$(): Observable<OrderProduct[]> {
    const url = `${this.endPoint}/default-products`;

    return this.http.get(url)
      .pipe(
        map((res: any) => this.transformService.fromGetOrderDefaultProducts(res.data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  validateOrderNumber$(orderNumber: string ): Observable<boolean> {
    const url = `${this.endPoint}/validate-number/${orderNumber}`;

    return this.http.get(url)
      .pipe(
        map((res: any) => res.data),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  create$(order: Order): Observable<Order> {
    return this.http.post(this.endPoint, this.transformService.toModifyOrder(order))
      .pipe(
        map((res: any) => this.transformService.fromModifyOrder(res.data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  update$(orderId: string, order: Order): Observable<Order> {
    const url = `${this.endPoint}/${orderId}`;

    return this.http.put(url, this.transformService.toModifyOrder(order))
      .pipe(
        map((res: any) => this.transformService.fromModifyOrder(res.data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  export$(orderIds: string[]): Observable<Blob> {
    const url = `${this.endPoint}/export`;

    return this.http.post(url, { orderIds }, { responseType: 'blob', observe: 'response' })
      .pipe(
        map((res: any) => res.body),
        catchError((res: any) => throwError(res.error.messages))
      );
  }
}
