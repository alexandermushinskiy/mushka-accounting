import { Injectable } from '@angular/core';

import { ItemsList } from '../../../../shared/interfaces/items-list.interface';
import { OrderList } from '../../../../shared/models/order-list.model';
import { DatetimeService } from '../../../datetime/datetime.service';
import { ApiSearchOrders } from '../interfaces/api-serach-orders.interface';
import { OrdersSearchParams } from '../../../../orders/shared/interfaces/orders-search-params.interface';
import { TableSort } from '../../../../shared/interfaces/table-sort.interface';
import { TablePagination } from '../../../../shared/interfaces/table-pagination.interface';
import { OrdersFiltersSchema } from '../../../../orders/shared/interfaces/orders-filters-schema.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiOrdersTransformService {
  constructor(private datetimeService: DatetimeService) {
  }

  toSearchOrders({ filters, sort, pagination }: OrdersSearchParams): ApiSearchOrders.Request {
    return {
      query: this.getSearchOrdersQuery(filters),
      navigation: this.toNavigation(sort, pagination)
    };
  }

  fromSearchOrders(data: ApiSearchOrders.Response): ItemsList<OrderList> {
    const items = data.items || [];

    return {
      items: items.map(item => this.toOrderList(item)),
      length: data.total
    };
  }

  private toOrderList(source: ApiSearchOrders.Order): OrderList {
    return new OrderList({
      id: source.id,
      orderDate: this.datetimeService.toString(source.orderDate),
      orderNumber: source.orderNumber,
      cost: source.cost,
      address: source.address,
      customerName: source.customerName,
      productsCount: source.productsCount,
      isWholesale: source.isWholesale
    });
  }

  private toNavigation(sort: TableSort, pagination: TablePagination): ApiSearchOrders.Navigation {
    return {
      sort: sort as ApiSearchOrders.Sort,
      page: {
        from: pagination.offset.toString(),
        size: pagination.limit.toString()
      }
    };
  }

  private getSearchOrdersQuery(filters: OrdersFiltersSchema): ApiSearchOrders.Query {
    if (!filters || Object.keys(filters).length === 0) {
      return {};
    }

    return {
      ...filters.criteria && { criteria: filters.criteria },
      ...filters.fromDate && { fromDate: filters.fromDate },
      ...filters.toDate && { toDate: filters.toDate }
    };
  }
}
