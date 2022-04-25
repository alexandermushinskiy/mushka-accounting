import { Injectable } from '@angular/core';

import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { OrderList } from '../../../shared/models/order-list.model';
import { DatetimeService } from '../../../core/datetime/datetime.service';
import { ApiSearchOrders } from '../interfaces/api-serach-orders.interface';
import { OrdersSearchParams } from '../../../orders/interfaces/orders-search-params.interface';
import { Order } from '../../../shared/models/order.model';
import { Customer } from '../../../shared/models/customer.model';
import { OrderProduct } from '../../../shared/models/order-product.model';
import { Size } from '../../../shared/models/size.model';
import { ApiGetOrderById } from '../interfaces/api-get-order-by-id.interface';
import { ApiGetOrderDefaultProducts } from '../interfaces/api-get-order-default-products.interface';
import { ApiCreateOrder } from '../interfaces/api-create-order.interface';
import { ApiValidateOrderNumber } from '../interfaces/api-validate-order-number.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiOrdersTransformService {
  constructor(private datetimeService: DatetimeService) {
  }

  toSearchOrders({ filters, sort, pagination }: OrdersSearchParams): ApiSearchOrders.Request {
    return {
      query: {
        searchKey: filters.searchKey,
        region: filters.region,
        fromDate: filters.fromDate,
        toDate: filters.toDate
      },
      sort,
      page: {
        from: pagination.offset.toString(),
        size: pagination.limit.toString()
      }
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
      id: source.order.id,
      orderDate: this.datetimeService.toString(source.order.orderDate),
      number: source.order.number,
      cost: source.order.cost,
      costMethod: source.order.costMethod,
      discount: source.order.discount,
      region: source.order.region,
      city: source.order.city,
      isWholesale: source.order.isWholesale,
      notes: source.order.notes,
      customer: this.toCustomer(source.customer),
      products: this.toOrderProducts(source.products)
    });
  }

  fromGetOrderDefaultProducts(response: ApiGetOrderDefaultProducts.Response): OrderProduct[] {
    return response.items.map((orderProduct: any) => new OrderProduct({
      productId: orderProduct.id,
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
          productId: orderProduct.productId,
          quantity: orderProduct.quantity,
          unitPrice: orderProduct.unitPrice,
          costPrice: orderProduct.costPrice,
        };
      })
    };
  }

  toValidateOrderNumber(orderNumber: string): ApiValidateOrderNumber.Request {
    return {
      orderNumber
    };
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

  private toOrderProducts(source: ApiGetOrderById.OrderProduct[]): OrderProduct[] {
    return source.map((orderProduct: any) => new OrderProduct({
      productId: orderProduct.id,
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
}
