import { Product } from '../models/product.model';
import { QuickFilter } from './quick-filter';

export class ProductQuickFilter {
  filterFinished(product: Product): boolean {
    return product.sizes.filter(ps => ps.quantity === 0).length > 0;
  }

  filterAlmostFinished(product: Product): boolean {
    return product.sizes.filter(ps => ps.quantity > 0 && ps.quantity < 10).length > 0;
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
