import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { TablePagination } from '../../shared/interfaces/table-pagination.interface';
import { TableSort } from '../../shared/interfaces/table-sort.interface';
import { OrderList } from '../../shared/models/order-list.model';
import { OrderProduct } from '../../shared/models/order-product.model';
import { Order } from '../../shared/models/order.model';
import { OrderDetailsService } from './order-details.service';
import { OrderProductsService } from './order-products.service';
import { OrderService } from './order.service';
import { OrdersSearchParamsService } from './orders-search-params.service';
import { OrdersTableService } from './orders-table.service';
import { ValidateOrderNumberService } from './validate-order-number.service';

@Injectable({
  providedIn: 'root'
})
export class OrdersFacadeService {
  constructor(private ordersSearchParamsService: OrdersSearchParamsService,
              private ordersTableService: OrdersTableService,
              private orderService: OrderService,
              private orderDetailsService: OrderDetailsService,
              private orderProductsService: OrderProductsService,
              private validateOrderNumberService: ValidateOrderNumberService
              ) {
  }

  searchOrders(filters: Partial<Record<string, any>>): void {
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

  loadOrder$(orderId: string): Observable<Order> {
    return this.orderDetailsService.fetchOrder$(orderId);
  }

  getOrderDefaultProducts$(): Observable<OrderProduct[]> {
    return this.orderProductsService.fetchOrderDefaultProducts$();
  }

  createOrder$(order: Order): Observable<Order> {
    return this.orderService.create$(order);
  }

  updateOrder$(orderId: string, order: Order): Observable<Order> {
    return this.orderService.update$(orderId, order);
  }

  getSaveOrderLoadingFlag$(): Observable<boolean> {
    return this.orderService.isSaving$.asObservable();
  }

  deleteOrder$(orderId: string): Observable<void> {
    return this.orderService.delete$(orderId);
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

  hasActiveFilters$(): Observable<boolean> {
    return this.ordersSearchParamsService.hasActiveFilters$();
  }

  fetchOrders(): void {
    const params = {
      filters: this.ordersSearchParamsService.filters$.value,
      sort: this.ordersSearchParamsService.sort$.value,
      pagination: this.ordersSearchParamsService.pagination$.value
    };

    this.ordersTableService.fetchOrders(params);
  }

  validateOrderNumber$(orderNumber: string): Observable<boolean> {
    return this.validateOrderNumberService.validateOrderNumber$(orderNumber);
  }

  getValidateOrderNumberLoadingFlag$(): Observable<boolean> {
    return this.validateOrderNumberService.isLoading$.asObservable();
  }
}
