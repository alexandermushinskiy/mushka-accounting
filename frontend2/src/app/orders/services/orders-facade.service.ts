import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TablePagination } from '../../shared/interfaces/table-pagination.interface';
import { TableSort } from '../../shared/interfaces/table-sort.interface';
import { OrderList } from '../../shared/models/order-list.model';
import { OrdersFiltersSchema } from '../shared/interfaces/orders-filters-schema.interface';
import { DeleteOrderService } from './delete-order.service';

import { OrdersSearchParamsService } from './orders-search-params.service';
import { OrdersTableService } from './orders-table.service';

@Injectable({
  providedIn: 'root'
})
export class OrdersFacadeService {
  constructor(private ordersSearchParamsService: OrdersSearchParamsService,
              private ordersTableService: OrdersTableService,
              private deleteOrderService: DeleteOrderService
              ) {
  }

  searchOrders(filters?: OrdersFiltersSchema): void {
    this.ordersSearchParamsService.setFilters(filters);
    this.ordersSearchParamsService.resetPagination();
    this.fetchOrders();
  }

  sortOrders(sort: TableSort): void {
    this.ordersSearchParamsService.sort$.next(sort);
    this.fetchOrders();
  }

  paginateOrders(pagination: TablePagination): void {
    this.ordersSearchParamsService.pagination$.next(pagination);
    this.fetchOrders();
  }

  deleteOrder$(orderId: string): Observable<void> {
    return this.deleteOrderService.delete(orderId);
  }

  getTableItems$(): Observable<OrderList[]> {
    return this.ordersTableService.items$.asObservable();
  }

  getTotalTableItems$(): Observable<number> {
    return this.ordersTableService.totalItems$.asObservable();
  }

  getTableLoadingFlag$(): Observable<boolean> {
    return this.ordersTableService.isLoading$.asObservable();
  }

  getPageIndex$(): Observable<number> {
    return this.ordersTableService.pageIndex$.asObservable();
  }

  getPageLimit(): number {
    return this.ordersSearchParamsService.getPageLimit();
  }

  getSort$(): Observable<TableSort> {
    return this.ordersSearchParamsService.sort$.asObservable();
  }

  getPagination(): TablePagination {
    return this.ordersSearchParamsService.pagination$.value;
  }

  fetchOrders(): void {
    const params = {
      filters: this.ordersSearchParamsService.filters$.value,
      sort: this.ordersSearchParamsService.sort$.value,
      pagination: this.ordersSearchParamsService.pagination$.value
    };

    this.ordersTableService.fetchOrders(params);
  }
}
