import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ApiOrdersService } from '../../core/api/orders/services/api-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { I18N } from '../constants/i18n.const';

@Injectable({
  providedIn: 'root'
})
export class DeleteOrderService {
  constructor(private apiOrdersService: ApiOrdersService,
              private notificationsService: NotificationsService) {
  }

  delete(orderId: string): Observable<any> {
    return this.apiOrdersService.delete$(orderId)
      .pipe(
        catchError((error: any) => this.showApiError$(error))
      );
  }

  private showApiError$(error: any): Observable<never> {
    this.notificationsService.error(I18N.errors.deleteOrderError);
    return throwError(error);
  }
}
