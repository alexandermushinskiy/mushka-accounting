import { Injectable } from '@angular/core';

import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { ExhibitionList } from '../../../shared/models/exhibition-list.model';
import { ExhibitionProduct } from '../../../shared/models/exhibition-product.model';
import { Exhibition } from '../../../shared/models/exhibition.model';
import { Size } from '../../../shared/models/size.model';
import { ApiDescribeExhibition } from '../interfaces/api-describe-exhibition.interface';
import { ApiGetExhibitionDefaultProducts } from '../interfaces/api-get-exhibition-default-roducts.interface';
import { ApiSearchExhibitions } from '../interfaces/api-search-exhibitions.interface';
import { ApiExhibitionProduct } from '../types/exhibition-product.type';

@Injectable({
  providedIn: 'root'
})
export class ApiExhibitionsTransformService {
  fromSearchExhibitions(data: ApiSearchExhibitions.Response): ItemsList<ExhibitionList> {
    const items = data.items || [];

    return {
      items: items.map(item => this.toExhibitionList(item)),
      length: data.total
    };
  }

  fromDescribeExhibition({ exhibition, products }: ApiDescribeExhibition.Response): Exhibition {
    return new Exhibition({
      id: exhibition.id,
      name: exhibition.name,
      fromDate: exhibition.fromDate,
      toDate: exhibition.toDate,
      city: exhibition.city,
      participationCost: exhibition.participationCost,
      participationCostMethod: exhibition.participationCostMethod,
      accommodationCost: exhibition.accommodationCost,
      accommodationCostMethod: exhibition.accommodationCostMethod,
      fareCost: exhibition.fareCost,
      fareCostMethod: exhibition.fareCostMethod,
      notes: exhibition.notes,
      profit: exhibition.profit,
      products: products.map(product => this.toExhibitionProduct(product))
    });
  }

  fromGetDefaultExhibitionProducts({ products }: ApiGetExhibitionDefaultProducts.Response): ExhibitionProduct[] {
    return products.map(product => this.toExhibitionProduct(product));
  }

  private toExhibitionList(source: ApiSearchExhibitions.Exhibition): ExhibitionList {
    return new ExhibitionList({
      id: source.id,
      name: source.name,
      fromDate: source.fromDate,
      toDate: source.toDate,
      city: source.city,
      participationCost: source.participationCost,
      profit: source.profit,
      productsCount: source.productsCount
    });
  }

  private toExhibitionProduct(product: ApiExhibitionProduct): ExhibitionProduct {
    return new ExhibitionProduct({
      quantity: product.quantity,
      unitPrice: product.unitPrice,
      costPrice: product.costPrice,
      product: {
        id: product.id,
        name: product.name,
        vendorCode: product.vendorCode,
        size: !!product.sizeName ? new Size({ name: product.sizeName }) : null
      }
    });
  }
}
