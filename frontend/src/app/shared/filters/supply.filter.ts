import { FiltersBase } from './filter-base';
import { Supply } from '../../supplies/shared/models/supply.model';


export class SupplyFilter extends FiltersBase {

  constructor(private searchKey: string) {
    super();
  }

  filter(delivery: Supply): boolean {
    return this.containsSearchKey(delivery.supplier, this.searchKey);
  }
}
