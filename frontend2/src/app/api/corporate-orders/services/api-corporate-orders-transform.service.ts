import { Injectable } from '@angular/core';

import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { CorporateOrderList } from '../../../shared/models/corporate-order-list.model';
import { CorporateOrderProduct } from '../../../shared/models/corporate-order-product.model';
import { CorporateOrder } from '../../../shared/models/corporate-order.model';
import { DatetimeService } from '../../../core/datetime/datetime.service';
import { ApiCreateCorporateOrder } from '../interfaces/api-create-corporate-order.interface';
// import { ApiCreateCorporateOrder } from '../interfaces/api-create-corporate-order.interface';
import { ApiGetCorporateOrder } from '../interfaces/api-get-corporate-order.interface';
import { ApiSearchCorporateOrders } from '../interfaces/api-search-corporate-orders.interface';
import { ApiUpdateCorporateOrder } from '../interfaces/api-update-corporate-order.interface';
import { ApiValidateCorporateOrderNumber } from '../interfaces/api-validate-corporate-order-number.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiCorporateOrdersTransformService {
  constructor(private datetimeService: DatetimeService) {
  }

  fromSearchOrders(data: ApiSearchCorporateOrders.Response): ItemsList<CorporateOrderList> {
    const items = data.items || [];

    return {
      length: data.total,
      items: items.map(item => this.toCorporateOrderList(item))
    };
  }

  fromGetOrder({ order, products }: ApiGetCorporateOrder.Response): CorporateOrder {
    return new CorporateOrder({
      id: order.id,
      createdOn: this.datetimeService.toString(order.createdOn),
      orderNumber: order.orderNumber,
      cost: order.cost,
      costMethod: order.costMethod,
      prepayment: order.prepayment,
      prepaymentMethod: order.prepaymentMethod,
      deliveryCost: order.deliveryCost,
      deliveryCostMethod: order.deliveryCostMethod,
      tax: order.tax,
      profit: order.profit,
      companyName: order.companyName,
      contactPerson: order.contactPerson,
      phone: order.phone,
      email: order.email,
      notes: order.notes,
      region: order.region,
      city: order.city,
      products: products.map(prod => new CorporateOrderProduct({
        name: prod.name,
        quantity: prod.quantity,
        unitPrice: prod.unitPrice,
        costPrice: prod.costPrice
      }))
    });
  }

  toValidateOrderNumber(orderNumber: string): ApiValidateCorporateOrderNumber.Request {
    return {
      orderNumber
    };
  }

  toCreateOrder(order: CorporateOrder): ApiCreateCorporateOrder.Request {
    return this.toCreateUpdateOrder(order);
  }

  toUpdateOrder(order: CorporateOrder): ApiUpdateCorporateOrder.Request {
    return this.toCreateUpdateOrder(order);
  }

  private toCorporateOrderList(source: ApiSearchCorporateOrders.CorporateOrder): CorporateOrderList {
    return new CorporateOrderList({
      id: source.id,
      orderNumber: source.orderNumber,
      createdOn: this.datetimeService.toString(source.createdOn),
      address: source.address,
      companyName: source.companyName,
    });
  }

  private toCreateUpdateOrder(order: CorporateOrder): ApiCreateCorporateOrder.Request | ApiUpdateCorporateOrder.Request {
    return {
      createdOn: order.createdOn,
      orderNumber: order.orderNumber,
      cost: order.cost,
      costMethod: order.costMethod,
      prepayment: order.prepayment,
      prepaymentMethod: order.prepaymentMethod,
      deliveryCost: order.deliveryCost,
      deliveryCostMethod: order.deliveryCostMethod,
      tax: order.tax,
      profit: order.profit,
      region: order.region,
      city: order.city,
      companyName: order.companyName,
      contactPerson: order.contactPerson,
      phone: order.phone,
      email: order.email,
      notes: order.notes,
      products: order.products.map(product => {
        return {
          name: product.name,
          quantity: product.quantity,
          unitPrice: product.unitPrice,
          costPrice: product.costPrice,
        };
      })
    };
  }
}
