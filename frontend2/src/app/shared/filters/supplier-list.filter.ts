import { FiltersBase } from './filter-base';
import { Supplier } from '../models/supplier.model';

export class SupplierListFilter extends FiltersBase {
  constructor(private searchKey: string) {
    super();
  }

  filter(supplier: Supplier): boolean {
    let foundBySearch = true;

    if (!!this.searchKey) {
      foundBySearch = [supplier.name, supplier.service, supplier.webSite, supplier.email]
        .some(field => !!field && field.toLowerCase().includes(this.searchKey.toLowerCase()));
    }

    return foundBySearch;
  }
}
