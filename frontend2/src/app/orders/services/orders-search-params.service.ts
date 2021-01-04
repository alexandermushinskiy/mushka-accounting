import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TableSortOrder } from '../../shared/enums/table-sort-order.enum';

import { TablePagination } from '../../shared/interfaces/table-pagination.interface';
import { TableSort } from '../../shared/interfaces/table-sort.interface';
import { OrdersFiltersSchema } from '../shared/interfaces/orders-filters-schema.interface';

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

  setFilters(filters: OrdersFiltersSchema): void {
    const currentFilters = { ...this.defaultFilters };

    this.filters$.next({ ...filters || currentFilters });
  }

  resetPagination(): void {
    this.pagination$.next(this.defaultPagination);
  }

  resetSearchParams(): void {
    this.filters$.next(this.defaultFilters);
    this.sort$.next(this.defaultSort);
    this.resetPagination();
  }

  private get defaultFilters(): OrdersFiltersSchema {
    return {};
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
