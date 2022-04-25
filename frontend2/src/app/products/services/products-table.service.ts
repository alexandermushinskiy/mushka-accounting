import { Injectable } from '@angular/core';
import { BehaviorSubject, combineLatest, Observable, Subject, Subscription, throwError, timer } from 'rxjs';
import { catchError, finalize, map, tap } from 'rxjs/operators';

import { ApiProductsService } from '../../api/products/services/api-products.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { ItemsList } from '../../shared/interfaces/items-list.interface';
import { ProductList } from '../../shared/models/product-list.model';
import { I18N } from '../constants/i18n.const';

@Injectable({
  providedIn: 'root'
})
export class ProductsTableService {
  items$ = new BehaviorSubject<ProductList[]>([]);
  isLoading$ = new BehaviorSubject<boolean>(false);

  private readonly fetchMinDelay = 300;
  private readonly defaultError = I18N.errors.fetchProductsError;

  private fetchSubscription: Subscription;

  constructor(private apiProductsService: ApiProductsService,
              private notificationsService: NotificationsService) {
  }

  fetchProducts(categoryId: string): void {
    const sources$ = new Subject<ItemsList<ProductList>>();
    this.setStartFetchingState();

    this.fetchSubscription = combineLatest([
      timer(this.fetchMinDelay),
      this.apiProductsService.getProductsByCategory$(categoryId)
    ])
      .pipe(
        map(x => x[1]),
        finalize(() => this.setEndFetchingState()),
        tap((products: ItemsList<ProductList>) => {
          this.items$.next(products.items);
        }),
        catchError(error => {
          this.items$.next([]);
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
