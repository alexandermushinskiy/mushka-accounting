import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';
import { Order } from '../../shared/models/order.model';
import { OrderList } from '../../orders/shared/models/order-list.model';
import { OrderProduct } from '../../shared/models/order-product.model';

@Injectable()
export class OrdersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/orders`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getAll(): Observable<OrderList[]>  {
    return this.http.get(this.endPoint)
      .map((res: any) => this.converterService.convertToOrdersList(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getById(productId: string): Observable<Order> {
    return this.http.get(`${this.endPoint}/${productId}`)
      .map((res: any) => this.converterService.convertToOrder(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getDefaultProducts(): Observable<OrderProduct[]> {
    return this.http.get(`${this.endPoint}/default-products`)
      .map((res: any) => this.converterService.convertToOrderProducts(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  create(order: Order): Observable<Order> {
    return this.http.post(this.endPoint, order)
      .map((res: any) => this.converterService.convertToOrder(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  update(orderId: string, order: Order): Observable<Order> {
    return this.http.put(`${this.endPoint}/${orderId}`, order)
      .map((res: any) => this.converterService.convertToOrder(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  delete(orderId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${orderId}`)
      .catch((res: any) => throwError(res.error.messages));
  }

  export(orderIds: string[]): Observable<Blob> {
    const requestBody = {
      orderIds: orderIds
    };
    return this.http.post(`${this.endPoint}/export`, requestBody, { responseType: 'blob', observe: 'response' })
      .map((res: any) => res.body)
      .catch((res: any) => throwError(res.error.messages));
  }

  validateOrderNumber(number: string ): Observable<boolean> {
    return this.http.get(`${this.endPoint}/validate-number/${number}`)
      .delay(1000)
      .map((res: any) => res.data.isValid)
      .catch((res: any) => throwError(res.error.messages));
  }
}
