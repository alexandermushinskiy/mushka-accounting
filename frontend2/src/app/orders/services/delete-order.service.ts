import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { ApiOrdersService } from '../../api/orders/services/api-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { I18N } from '../constants/i18n.const';

@Injectable({
  providedIn: 'root'
})
export class DeleteOrderService {
  constructor(private apiOrdersService: ApiOrdersService,
              private notificationsService: NotificationsService) {
  }

  deleteOrder$(orderId: string): Observable<void> {
    return this.apiOrdersService.deleteOrder$(orderId)
      .pipe(
        tap(() => this.showSuccessMessage()),
        catchError((error: any) => this.showApiError$(error))
      );
  }

  private showSuccessMessage(): void {
    this.notificationsService.success(I18N.messages.orderDeleted);
  }

  private showApiError$(error: any): Observable<never> {
    this.notificationsService.error(I18N.errors.deleteOrderError);

    return throwError(error);
  }
}
