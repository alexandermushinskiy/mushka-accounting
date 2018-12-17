import { FiltersBase } from './filter-base';
import { Delivery } from '../../delivery/shared/models/delivery.model';


export class DeliveryFilter extends FiltersBase {

  constructor(private searchKey: string) {
    super();
  }

  filter(delivery: Delivery): boolean {
    return this.containsSearchKey(delivery.supplier, this.searchKey);
  }
}
