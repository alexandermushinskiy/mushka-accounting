import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../../environments/environment';
import { OrdersSearchParams } from '../../../../orders/shared/interfaces/orders-search-params.interface';
import { ItemsList } from '../../../../shared/interfaces/items-list.interface';
import { OrderList } from '../../../../shared/models/order-list.model';
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

    return this.http.post(`${this.endPoint}/search`, body)
      .pipe(
        map((data: any) => this.transformService.fromSearchOrders(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  delete$(orderId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${orderId}`)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

}
