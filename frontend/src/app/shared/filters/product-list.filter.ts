import { FiltersBase } from './filter-base';
import { ProductList } from '../models/product-list.model';

export class ProductListFilter extends FiltersBase {

  constructor(private searchKey: string) {
    super();
  }

  filter(product: ProductList): boolean {
    return this.containsSearchKey(product.vendorCode, this.searchKey)
      || this.containsSearchKey(product.name, this.searchKey);
  }
}
