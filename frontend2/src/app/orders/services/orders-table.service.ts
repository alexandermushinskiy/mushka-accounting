import { Injectable } from '@angular/core';
import { BehaviorSubject, combineLatest, Observable, Subject, Subscription, throwError, timer } from 'rxjs';
import { catchError, finalize, map, tap } from 'rxjs/operators';

import { ApiOrdersService } from '../../core/api/orders/services/api-orders.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { ItemsList } from '../../shared/interfaces/items-list.interface';
import { OrderList } from '../../shared/models/order-list.model';
import { I18N } from '../constants/i18n.const';
import { OrdersSearchParams } from '../shared/interfaces/orders-search-params.interface';

@Injectable({
  providedIn: 'root'
})
export class OrdersTableService {
  items$ = new BehaviorSubject<OrderList[]>([]);
  totalItems$ = new BehaviorSubject<number>(0);
  pageIndex$ = new BehaviorSubject<number>(0);
  isLoading$ = new BehaviorSubject<boolean>(false);

  private readonly fetchMinDelay = 500;
  private readonly defaultError = I18N.errors.searchOrdersError;

  private fetchSubscription: Subscription;

  constructor(private apiOrdersService: ApiOrdersService,
              private notificationsService: NotificationsService) {
  }

  fetchOrders(searchParams: OrdersSearchParams): void {
    const sources$ = new Subject<ItemsList<OrderList>>();
    this.setStartFetchingState();

    this.fetchSubscription = combineLatest([
      timer(this.fetchMinDelay),
      this.apiOrdersService.searchOrders$(searchParams)
    ])
      .pipe(
        map(x => x[1]),
        finalize(() => this.setEndFetchingState()),
        tap((orders: ItemsList<OrderList>) => {
          const pagination = searchParams.pagination;

          this.items$.next(orders.items);
          this.totalItems$.next(orders.length);
          this.pageIndex$.next(pagination.limit > 0 ? Math.floor(pagination.offset / pagination.limit) : 0);
        }),
        catchError(error => {
          this.items$.next([]);
          this.totalItems$.next(0);
          this.pageIndex$.next(0);

          return this.showApiError$(error);
        })
      )
      .subscribe(sources$);
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
