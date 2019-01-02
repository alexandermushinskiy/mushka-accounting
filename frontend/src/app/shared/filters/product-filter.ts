import { FiltersBase } from './filter-base';
import { Product } from '../models/product.model';

export class ProductFilter extends FiltersBase {

  constructor(private searchKey: string) {
    super();
  }

  filter(product: Product): boolean {
    return this.containsSearchKey(product.vendorCode, this.searchKey)
      || this.containsSearchKey(product.name, this.searchKey);
  }
}
