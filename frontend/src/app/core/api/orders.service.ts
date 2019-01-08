import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';
import { Order } from '../../shared/models/order.model';
import { OrderList } from '../../orders/shared/models/order-list.model';

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

}
