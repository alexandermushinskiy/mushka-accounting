import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';
import { Order } from '../../shared/models/order.model';
import { OrderList } from '../../shared/models/order-list.model';
import { OrderProduct } from '../../shared/models/order-product.model';
import { delay, map, catchError } from 'rxjs/operators';

@Injectable()
export class OrdersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/orders`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getAll(): Observable<OrderList[]>  {
    return this.http.get(this.endPoint).pipe(
      map((res: any) => this.converterService.convertToOrdersList(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getById(productId: string): Observable<Order> {
    return this.http.get(`${this.endPoint}/${productId}`).pipe(
      map((res: any) => this.converterService.convertToOrder(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getDefaultProducts(): Observable<OrderProduct[]> {
    return this.http.get(`${this.endPoint}/default-products`).pipe(
      map((res: any) => this.converterService.convertToOrderProducts(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  create(order: Order): Observable<Order> {
    return this.http.post(this.endPoint, order).pipe(
      map((res: any) => this.converterService.convertToOrder(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  update(orderId: string, order: Order): Observable<Order> {
    return this.http.put(`${this.endPoint}/${orderId}`, order).pipe(
      map((res: any) => this.converterService.convertToOrder(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  delete(orderId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${orderId}`).pipe(
      catchError((res: any) => throwError(res.error.messages)));
  }

  export(orderIds: string[]): Observable<Blob> {
    return this.http.post(`${this.endPoint}/export`, { orderIds }, { responseType: 'blob', observe: 'response' })
      .pipe(
        map((res: any) => res.body),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  validateOrderNumber(orderNumber: string ): Observable<boolean> {
    return this.http.get(`${this.endPoint}/validate-number/${orderNumber}`).pipe(
      delay(1000),
      map((res: any) => res.data.isValid),
      catchError((res: any) => throwError(res.error.messages)));
  }
}
