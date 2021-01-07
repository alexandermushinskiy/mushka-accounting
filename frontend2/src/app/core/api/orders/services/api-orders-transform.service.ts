import { Injectable } from '@angular/core';

import { ItemsList } from '../../../../shared/interfaces/items-list.interface';
import { OrderList } from '../../../../shared/models/order-list.model';
import { DatetimeService } from '../../../datetime/datetime.service';
import { ApiSearchOrders } from '../interfaces/api-serach-orders.interface';
import { OrdersSearchParams } from '../../../../orders/shared/interfaces/orders-search-params.interface';
import { TableSort } from '../../../../shared/interfaces/table-sort.interface';
import { TablePagination } from '../../../../shared/interfaces/table-pagination.interface';
import { OrdersFiltersSchema } from '../../../../orders/shared/interfaces/orders-filters-schema.interface';
import { Order } from '../../../../shared/models/order.model';
import { Customer } from '../../../../shared/models/customer.model';
import { OrderProduct } from '../../../../shared/models/order-product.model';
import { Size } from '../../../../shared/models/size.model';
import { ApiGetOrderById } from '../interfaces/api-get-order-by-id.interface';
import { ApiGetOrderDefaultProducts } from '../interfaces/api-get-order-default-products.interface';
import { ApiCreateOrder } from '../interfaces/api-create-order.interface';

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

  fromGetOrder(source: ApiGetOrderById.Response): Order {
    return new Order({
      id: source.id,
      orderDate: this.datetimeService.toString(source.orderDate),
      number: source.number,
      cost: source.cost,
      costMethod: source.costMethod,
      discount: source.discount,
      region: source.region,
      city: source.city,
      isWholesale: source.isWholesale,
      notes: source.notes,
      customer: this.toCustomer(source.customer),
      products: this.toOrderProducts(source.products)
    });
  }

  fromGetOrderDefaultProducts(source: ApiGetOrderDefaultProducts.Response[]): OrderProduct[] {
    return this.toOrderProducts(source);
  }

  toModifyOrder(order: Order): ApiCreateOrder.Request {
    return {
      orderDate: order.orderDate,
      number: order.number,
      cost: order.cost,
      costMethod: order.costMethod,
      discount: order.discount,
      region: order.region,
      city: order.city,
      isWholesale: order.isWholesale,
      notes: order.notes,
      profit: order.profit,
      customer: {
        id: order.customer.id,
        email: order.customer.email,
        firstName: order.customer.firstName,
        lastName: order.customer.lastName,
        phone: order.customer.phone
      },
      products: order.products.map(orderProduct => {
        return {
          productId: orderProduct.product.id,
          quantity: orderProduct.quantity,
          unitPrice: orderProduct.unitPrice,
          costPrice: orderProduct.costPrice,
        };
      })
    };
  }

  fromModifyOrder(source: ApiCreateOrder.Response): Order {
    return new Order({
      id: source.id,
      orderDate: this.datetimeService.toString(source.orderDate),
      number: source.number,
      cost: source.cost,
      costMethod: source.costMethod,
      discount: source.discount,
      region: source.region,
      city: source.city,
      isWholesale: source.isWholesale,
      notes: source.notes,
      customer: this.toCustomer(source.customer),
      products: this.toOrderProducts(source.products)
    });
  }

  private toCustomer(source: ApiGetOrderById.Customer): Customer {
    return new Customer({
      id: source.id,
      firstName: source.firstName,
      lastName: source.lastName,
      phone: source.phone,
      email: source.email
    });
  }

  private toOrderProducts(source: ApiGetOrderDefaultProducts.Response[] | ApiGetOrderById.OrderProduct[]): OrderProduct[] {
    return source.map((orderProduct: any) => new OrderProduct({
      quantity: orderProduct.quantity,
      unitPrice: orderProduct.unitPrice,
      costPrice: orderProduct.costPrice,
      product: {
        id: orderProduct.id,
        name: orderProduct.name,
        vendorCode: orderProduct.vendorCode,
        size: !!orderProduct.sizeName ? new Size({name: orderProduct.sizeName}) : null
      }
    }));
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
      customer: {
        name: filters.customerName.valueAsCriteria
      },
      order: {
        orderDate: filters.orderDate.valueAsCriteria
      }
    };
  }
}
