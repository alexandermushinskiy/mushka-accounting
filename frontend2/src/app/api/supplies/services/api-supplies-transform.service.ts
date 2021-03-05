import { Injectable } from '@angular/core';

import { ItemsWithTotalCount } from '../../../shared/models/items-with-total-count.model';
import { Size } from '../../../shared/models/size.model';
import { SupplyList } from '../../../shared/models/supply-list.model';
import { SupplyProduct } from '../../../shared/models/supply-product.model';
import { Supply } from '../../../shared/models/supply.model';
import { ApiDescribeSupply } from '../interfaces/api-describe-supply.interface';
import { ApiSearchSupplies } from '../interfaces/api-search-supplies.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiSuppliesTransformService {
  toSearchOrders(searchKey: string, productId: string): ApiSearchSupplies.Request {
    return {
      searchKey,
      productId
    };
  }

  fromSearchOrders(response: ApiSearchSupplies.Response): ItemsWithTotalCount<SupplyList> {
    return new ItemsWithTotalCount<SupplyList>(
      response.items.map(res => this.toSupplyList(res)),
      response.total
    );
  }

  fromDescribeSupply({ supply, products}: ApiDescribeSupply.Response): Supply {
    return new Supply({
      id: supply.id,
      supplierId: supply.supplierId,
      supplierName: supply.supplierName,
      description: supply.description,
      requestDate: supply.requestDate,
      receivedDate: supply.receivedDate,
      prepayment: supply.prepayment,
      prepaymentMethod: supply.prepaymentMethod,
      cost: supply.cost,
      costMethod: supply.costMethod,
      deliveryCost: supply.deliveryCost,
      deliveryCostMethod: supply.deliveryCostMethod,
      bankFee: supply.bankFee,
      totalCost: supply.totalCost,
      productsAmount: supply.productsAmount,
      notes: supply.notes,
      products: products.map((prod: any) => new SupplyProduct({
        quantity: prod.quantity,
        unitPrice: prod.unitPrice,
        product: {
          id: prod.product.id,
          name: prod.product.name,
          vendorCode: prod.product.vendorCode,
          size: !!prod.product.sizeName ? new Size({name: prod.product.sizeName}) : null
        }
      }))
    });
  }

  private toSupplyList(source: ApiSearchSupplies.Supply): SupplyList {
    return new SupplyList({
      id: source.id,
      supplierName: source.supplierName,
      description: source.description,
      receivedDate: source.receivedDate,
      cost: source.cost,
      totalCost: source.totalCost,
      productsAmount: source.productsAmount
    });
  }
}
