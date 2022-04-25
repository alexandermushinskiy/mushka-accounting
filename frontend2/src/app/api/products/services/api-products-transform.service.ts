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
import { ItemsList } from '../../../shared/interfaces/items-list.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiProductsTransformService {

  fromGetByCategory(response: ApiGetByCategory.Response): ItemsList<ProductList> {
    const items = response.items || [];

    return {
      length: response.total,
      items: items.map(item => {
        return new ProductList({
          id: item.id,
          name: item.name,
          vendorCode: item.vendorCode,
          recommendedPrice: item.recommendedPrice,
          createdOn: item.createdOn,
          sizeName: item.sizeName,
          lastDeliveryDate: item.lastDeliveryDate,
          lastDeliveryCount: !!item.lastDeliveryCount ? item.lastDeliveryCount : 0,
          deliveriesCount: item.deliveriesCount,
          quantity: item.quantity
        });
      })
    };
  }

  fromDescribeProduct({ product, category, size }: ApiDescribeProduct.Response) {
    return new Product({
      id: product.id,
      name: product.name,
      vendorCode: product.vendorCode,
      recommendedPrice: product.recommendedPrice,
      createdOn: product.createdOn,
      isArchival: product.isArchival,
      category: new Category({
        id: category.id,
        name: category.name
      }),
      size: this.toProductSize(size)
    });
  }

  fromGetProductSizes(response: ApiGetProductSizes.Response): Size[] {
    return response.items.map(source =>
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

  fromGetProductsForSale(response: ApiGetProductsForSale.Response): ItemsList<SelectProduct> {
    const items = response.items || [];

    return {
      length: response.total,
      items: items.map(item => {
        return new SelectProduct({
          id: item.id,
          name: item.name,
          vendorCode: item.vendorCode,
          recommendedPrice: item.recommendedPrice,
          quantity: item.quantity,
          categoryName: item.categoryName,
          sizeName: item.sizeName,
          disabled: item.isArchival
        });
      })
    };
  }

  private toProductSize(source: ApiDescribeProduct.Size): ProductSize {
    return !!source ? new ProductSize({
      id: source.id,
      name: source.name,
      // quantity: source.quantity
    }) : null;
  }
}
