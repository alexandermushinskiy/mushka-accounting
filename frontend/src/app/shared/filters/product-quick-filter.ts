import { Product } from '../models/product.model';
import { QuickFilter } from './quick-filter';

export class ProductQuickFilter {
  filterFinished(product: Product): boolean {
    return product.quantity === 0;
  }

  filterAlmostFinished(product: Product): boolean {
    return product.quantity > 0 && product.quantity < 10;
  }

  filterWithoutDeliveries(product: Product): boolean {
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
