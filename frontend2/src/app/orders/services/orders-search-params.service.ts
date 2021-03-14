import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { TableSortOrder } from '../../shared/enums/table-sort-order.enum';
import { TablePagination } from '../../shared/interfaces/table-pagination.interface';
import { TableSort } from '../../shared/interfaces/table-sort.interface';
import { BetweenCriteriaFilter } from '../../shared/models/filtering/between-criteria-filter.model';
import { LikeCriteriaFilter } from '../../shared/models/filtering/like-criteria-filter.model';
import { OrdersFiltersSchema } from '../interfaces/orders-filters-schema.interface';

@Injectable({
  providedIn: 'root'
})
export class OrdersSearchParamsService {
  filters$ = new BehaviorSubject<OrdersFiltersSchema>(this.defaultFilters);
  pagination$ = new BehaviorSubject<TablePagination>(this.defaultPagination);
  sort$ = new BehaviorSubject<TableSort>(this.defaultSort);

  private readonly defaultPaginationLimit = 13;

  getPageLimit(): number {
    return this.defaultPaginationLimit;
  }

  setFilters(filters: Partial<Record<string, any>>): void {
    const currentFilters = { ...this.defaultFilters };

    Object.entries(currentFilters)
      .forEach(([filterName, filter]) => {
        filter.value = filters[filterName];
      });

    this.filters$.next(currentFilters);
  }

  resetPagination(): void {
    this.pagination$.next(this.defaultPagination);
  }

  resetSearchParams(): void {
    this.filters$.next(this.defaultFilters);
    this.sort$.next(this.defaultSort);
    this.resetPagination();
  }

  hasActiveFilters$(): Observable<boolean> {
    return this.filters$.asObservable()
      .pipe(
        map(filter => {
          return !(filter.customerName.isEmpty() && filter.orderDate.isEmpty());
        })
      );
  }

  private get defaultFilters(): OrdersFiltersSchema {
    return {
      customerName: new LikeCriteriaFilter(),
      orderDate: new BetweenCriteriaFilter()
    };
  }

  private get defaultSort(): TableSort {
    return {
      key: 'orderDate',
      order: TableSortOrder.Desc
    };
  }

  private get defaultPagination(): TablePagination {
    return {
      offset: 0,
      limit: this.defaultPaginationLimit
    };
  }
}
