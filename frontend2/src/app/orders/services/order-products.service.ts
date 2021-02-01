import { Injectable } from '@angular/core';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ApiOrdersService } from '../../api/orders/services/api-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { OrderProduct } from '../../shared/models/order-product.model';
import { I18N } from '../constants/i18n.const';

@Injectable({
  providedIn: 'root'
})
export class OrderProductsService {
  constructor(private apiOrdersService: ApiOrdersService,
              private notificationsService: NotificationsService) {
  }

  fetchOrderDefaultProducts$(): Observable<OrderProduct[]> {
    const source$ = new Subject<OrderProduct[]>();

    this.apiOrdersService.getDefaultProducts$()
      .pipe(
        catchError(error => {
          return this.showApiError$(error);
        })
      )
      .subscribe(source$);

    return source$;
  }

  private showApiError$(error: any): Observable<never> {
    this.notificationsService.error(I18N.errors.fetchOrderDefaultProductsError);
    return throwError(error);
  }
}
