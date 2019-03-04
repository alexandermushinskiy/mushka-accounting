import { FiltersBase } from './filter-base';
import { SupplyList } from '../../supplies/shared/models/supply-list.model';

export class SupplyFilter extends FiltersBase {

  constructor(private searchKey: string) {
    super();
  }

  filter(supply: SupplyList): boolean {
    return this.containsSearchKey(supply.supplierName, this.searchKey) ||
           this.containsSearchKey(supply.description, this.searchKey);
  }
}
