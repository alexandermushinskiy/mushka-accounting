import { Injectable } from '@angular/core';

import { Balance } from '../../../dashboard/models/balance.model';
import { OrdersCount } from '../../../dashboard/models/orders-count.model';
import { PopularCity } from '../../../dashboard/models/popular-city.model';
import { PopularProduct } from '../../../dashboard/models/popular-product.model';
import { SoldProductsCount } from '../../../dashboard/models/sold-products-count.model';
import { ApiGetBalance } from '../interfaces/api-get-balance.interface';
import { ApiGetOrdersCount } from '../interfaces/api-get-orders-count.interface';
import { ApiGetPopularCities } from '../interfaces/api-get-popular-cities.interface';
import { ApiGetPopularProducts } from '../interfaces/api-get-popular-products.interface';
import { ApiGetSoldProductsCount } from '../interfaces/api-get-sold-products-count.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiAnalyticsTransformService {
  fromGetBalance({ data }: ApiGetBalance.Response): Balance {
    return new Balance({
      expense: data.expense,
      profit: data.profit
    });
  }

  fromGetPopularProducts({ data }: ApiGetPopularProducts.Response): PopularProduct[] {
    return data.map(res => new PopularProduct({
      name: res.name,
      sizeName: res.sizeName,
      vendorCode: res.vendorCode,
      quantity: res.quantity,
    }));
  }

  fromGetPopularCities({ data }: ApiGetPopularCities.Response): PopularCity[] {
    return data.map(res => new PopularCity({
      city: res.city,
      quantity: res.quantity,
    }));
  }

  fromGetOrdersCount({ data }: ApiGetOrdersCount.Response): OrdersCount[] {
    return data.map(res => new OrdersCount({
      createdOn: res.createdOn, // this.datetimeService.toString(res.createdOn),
      quantity: res.quantity
    }));
  }

  toGetSoldProductsCount({ data }: ApiGetSoldProductsCount.Response): SoldProductsCount[] {
    return data.map(res => new SoldProductsCount({
      createdOn: res.createdOn, // this.datetimeService.toString(res.createdOn),
      quantity: res.quantity
    }));
  }
}
