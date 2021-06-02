import { Injectable } from '@angular/core';

import { ProductList } from '../../../shared/models/product-list.model';
import { ApiGetByCategory } from '../interfaces/api-get-by-category.interface';
import { DatetimeService } from '../../../core/datetime/datetime.service';
import { Size } from '../../../shared/models/size.model';
import { ApiGetProductSizes } from '../interfaces/api-get-product-sizes.interface';
import { Product } from '../../../shared/models/product.model';
import { ApiDescribeProduct } from '../interfaces/api-describe-product.interface';
import { Category } from '../../../shared/models/category.model';
import { ProductSize } from '../../../shared/models/product-size.model';
import { ApiCreateProduct } from '../interfaces/api-create-product.interface';
import { ApiUpdateProduct } from '../interfaces/api-update-product.interface';
import { SelectProduct } from '../../../shared/models/select-product.model';
import { ApiGetProductsForSale } from '../interfaces/api-get-products-for-sale.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiProductsTransformService {
  constructor(private datetimeService: DatetimeService) {
  }

  fromGetByCategory(response: ApiGetByCategory.Response): ProductList[] {
    return response.data.map(source => {
      return new ProductList({
        id: source.id,
        name: source.name,
        vendorCode: source.vendorCode,
        recommendedPrice: source.recommendedPrice,
        createdOn: source.createdOn,
        sizeName: source.sizeName,
        lastDeliveryDate: source.lastDeliveryDate,
        lastDeliveryCount: !!source.lastDeliveryCount ? source.lastDeliveryCount : 0,
        deliveriesCount: source.deliveriesCount,
        quantity: source.quantity
      });
    });
  }

  fromDescribeProduct(response: ApiDescribeProduct.Response) {
    return new Product({
      id: response.id,
      name: response.name,
      vendorCode: response.vendorCode,
      recommendedPrice: response.recommendedPrice,
      category: new Category({
        id: response.id,
        name: response.name
      }),
      createdOn: response.createdOn,
      size: this.toProductSize(response.size),
      isArchival: response.isArchival
    });
  }

  fromGetProductSizes(response: ApiGetProductSizes.Response): Size[] {
    return response.data.map(source =>
      new Size({ id: source.id, name: source.name })
    );
  }

  toCreateProduct(product: Product): ApiCreateProduct.Request {
    return {
      name: product.name,
      vendorCode: product.vendorCode,
      recommendedPrice: product.recommendedPrice,
      categoryId: product.category.id,
      sizeId: !!product.size ? product.size.id : null,
      isArchival: product.isArchival
    };
  }

  toUpdateProduct(product: Product): ApiUpdateProduct.Request {
    return {
      name: product.name,
      vendorCode: product.vendorCode,
      recommendedPrice: product.recommendedPrice,
      categoryId: product.category.id,
      sizeId: !!product.size ? product.size.id : null,
      isArchival: product.isArchival
    };
  }

  fromGetProductsForSale(response: ApiGetProductsForSale.Response): SelectProduct[] {
    return response.data.map(res => new SelectProduct({
      id: res.id,
      name: res.name,
      vendorCode: res.vendorCode,
      recommendedPrice: res.recommendedPrice,
      quantity: res.quantity,
      category: new Category({name: res.categoryName}),
      size: !!res.sizeName ? new Size({name: res.sizeName}) : null,
      disabled: res.isArchival
    })
  );
  }

  private toProductSize(source: ApiDescribeProduct.Size): ProductSize {
    return !!source ? new ProductSize({
      id: source.id,
      name: source.name,
      // quantity: source.quantity
    }) : null;
  }
}
