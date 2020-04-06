import { FiltersBase } from './filter-base';
import { ProductList } from '../models/product-list.model';
import { ProductsQuickFilter } from '../enums/products-quick-filter.enum';

export class ProductListFilter extends FiltersBase {

  constructor(private searchKey: string,
              private quickFilter: ProductsQuickFilter) {
    super();
  }

  filter(product: ProductList): boolean {
    let foundBySearch = true;
    const foundByQuickSearch = this.internalFilter(product);

    if (!!this.searchKey) {
      foundBySearch = [product.vendorCode, product.name]
        .some(field => field.toLowerCase().includes(this.searchKey.toLowerCase()));
    }

    return foundByQuickSearch && foundBySearch;
  }

  private internalFilter(product: ProductList): boolean {
    switch (this.quickFilter) {
      case ProductsQuickFilter.WITHOUT_DELIVERIES:
        return product.deliveriesCount === 0;

      case ProductsQuickFilter.ALMOST_FINISHED:
        return product.quantity > 0 && product.quantity < 10;

      case ProductsQuickFilter.OUT_OF_STOCK:
        return product.quantity === 0;

      default:
        return true;
    }
  }
}
