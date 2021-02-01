import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, throwError } from 'rxjs';

import { Order } from '../../shared/models/order.model';
import { I18N } from '../constants/i18n.const';
import { ApiOrdersService } from '../../api/orders/services/api-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OrderDetailsService {
  order$ = new BehaviorSubject<Order>(null);

  constructor(private apiOrdersService: ApiOrdersService,
              private notificationsService: NotificationsService) {
  }

  fetchOrder$(orderId: string): Observable<Order> {
    const source$ = new Subject<Order>();

    this.apiOrdersService.getOrder$(orderId)
      .pipe(
        tap(order => {
          this.order$.next(order);
        }),
        catchError(error => {
          this.order$.next(null);
          return this.showApiError$(error);
        })
      )
      .subscribe(source$);

    return source$;
  }

  private showApiError$(error: any): Observable<never> {
    this.notificationsService.error(I18N.errors.fetchOrderError);
    return throwError(error);
  }
}
