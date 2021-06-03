import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, Subscription, throwError } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';

import { ApiCategoriesService } from '../../api/categories/services/api-cateries.service';
import { NotificationsService } from '../../core/notifications/notifications.service';
import { ItemsList } from '../../shared/interfaces/items-list.interface';
import { Category } from '../../shared/models/category.model';
import { I18N } from '../constants/i18n.const';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  items$ = new BehaviorSubject<Category[]>([]);
  isLoading$ = new BehaviorSubject<boolean>(false);
  selectedItem$ = new BehaviorSubject<Category>(null);

  private readonly defaultError = I18N.errors.fetchCategoriesError;

  private fetchSubscription: Subscription;

  constructor(private apiCategoriesService: ApiCategoriesService,
              private notificationsService: NotificationsService) {
  }

  fetchCategories(): void {
    const sources$ = new Subject<ItemsList<Category>>();

    this.setStartFetchingState();

    this.fetchSubscription = this.apiCategoriesService.searchCategories$()
      .pipe(
        // map(result => result.items),
        finalize(() => this.setEndFetchingState()),
        tap((categories: ItemsList<Category>) => {
          this.items$.next(categories.items);
          if (categories.length > 0) {
            this.selectedItem$.next(categories.items[0]);
          }
        }),
        catchError((error) => {
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
