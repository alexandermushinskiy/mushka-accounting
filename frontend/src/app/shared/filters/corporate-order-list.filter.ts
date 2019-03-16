import { FiltersBase } from './filter-base';
import { CorporateOrderList } from '../../corporate-orders/shared/models/corporate-order-list.model';

export class CorporateOrderListFilter extends FiltersBase {
  constructor(private searchKey: string) {
    super();
  }

  filter(order: CorporateOrderList): boolean {
    return this.containsSearchKey(order.companyName, this.searchKey)
      || this.containsSearchKey(order.number, this.searchKey);
  }
}
