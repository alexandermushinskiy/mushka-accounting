import { QuickFilter } from './quick-filter';

export class SupplyQuickFilter {
  getFilters(): QuickFilter[] {
    return [
      new QuickFilter(this.filterProducts, 'По товарам'),
    ];
  }

  filterProducts() {
  }
}
