import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';

import { ApiOrdersService } from '../../api/orders/services/api-orders.service';
import { LanguageService } from '../../core/language/language.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { Order } from '../../shared/models/order.model';
import { I18N } from '../constants/i18n.const';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  isSaving$ = new BehaviorSubject<boolean>(false);

  constructor(private apiOrdersService: ApiOrdersService,
              private notificationsService: NotificationsService,
              private languageService: LanguageService) {
  }

  create$(order: Order): Observable<void> {
    this.isSaving$.next(true);

    return this.apiOrdersService.create$(order)
      .pipe(
        tap(() => this.showSuccessMessage(I18N.create.successMessage)),
        catchError((error: any) => this.showApiError$(error, I18N.errors.saveOrderError)),
        finalize(() => {
          this.isSaving$.next(false);
        })
      );
  }

  update$(orderId: string, order: Order): Observable<void> {
    this.isSaving$.next(true);

    return this.apiOrdersService.update$(orderId, order)
      .pipe(
        tap(() => this.showSuccessMessage(I18N.update.successMessage)),
        catchError((error: any) => this.showApiError$(error, I18N.errors.saveOrderError)),
        finalize(() => {
          this.isSaving$.next(false);
        })
      );
  }

  delete$(orderId: string): Observable<void> {
    return this.apiOrdersService.deleteOrder$(orderId)
      .pipe(
        catchError((error: any) => this.showApiError$(error, I18N.errors.deleteOrderError))
      );
  }

  showSuccessMessage(messageKey: string): void {
    const resultMessage = this.languageService.translate(messageKey);
    this.notificationsService.success(resultMessage);
  }

  private showApiError$(error: any, message: string): Observable<never> {
    this.notificationsService.error(message);
    return throwError(error);
  }
}
