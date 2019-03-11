import { QuickFilter } from './quick-filter';
import { ProductList } from '../models/product-list.model';

export class ProductQuickFilter {
  filterFinished(product: ProductList): boolean {
    return product.quantity === 0;
  }

  filterAlmostFinished(product: ProductList): boolean {
    return product.quantity > 0 && product.quantity < 10;
  }

  filterWithoutDeliveries(product: ProductList): boolean {
    return product.deliveriesCount === 0;
  }

  getFilters(): QuickFilter[] {
    return [
      new QuickFilter(this.filterWithoutDeliveries, 'Без поставок'),
      new QuickFilter(this.filterAlmostFinished, 'Заканчивающиеся'),
      new QuickFilter(this.filterFinished, 'Нет в наличие')
    ];
  }
}
