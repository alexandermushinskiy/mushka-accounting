import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, Subscription, throwError, combineLatest, timer } from 'rxjs';
import { catchError, finalize, map, tap } from 'rxjs/operators';

import { ApiOrdersService } from '../../api/orders/services/api-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { I18N } from '../constants/i18n.const';

@Injectable({
  providedIn: 'root'
})
export class ValidateOrderNumberService {
  isValid$ = new Subject<boolean>();
  isLoading$ = new BehaviorSubject<boolean>(false);

  private readonly fetchMinDelay = 500;
  private readonly defaultError = I18N.errors.validateOrderNumberError;

  private fetchSubscription: Subscription;

  constructor(private apiOrdersService: ApiOrdersService,
              private notificationsService: NotificationsService) {
  }

  validateOrderNumber$(orderNumber: string): Observable<boolean> {
    const sources$ = new Subject<boolean>();
    this.setStartFetchingState();

    this.fetchSubscription = combineLatest([
      timer(this.fetchMinDelay),
      this.apiOrdersService.validateOrderNumber$(orderNumber)
    ])
      .pipe(
        map(x => x[1]),
        finalize(() => this.setEndFetchingState()),
        tap((isValid: boolean) => {
          this.isValid$.next(isValid);
        }),
        catchError(error => {
          return this.showApiError$(error);
        })
      )
      .subscribe(sources$);

    return sources$;
  }

  private setStartFetchingState(): void {
    if (this.fetchSubscription) {
      this.fetchSubscription.unsubscribe();
    }
    this.isLoading$.next(true);
  }

  private setEndFetchingState(): void {
    this.fetchSubscription = null;
    this.isLoading$.next(false);
  }

  private showApiError$(error: any): Observable<never> {
    this.notificationsService.error(this.defaultError);
    return throwError(error);
  }
}
