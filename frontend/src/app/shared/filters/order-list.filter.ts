import { FiltersBase } from './filter-base';
import { OrderList } from '../../orders/shared/models/order-list.model';

export class OrderListFilter extends FiltersBase {
  constructor(private searchKey: string) {
    super();
  }
  
  filter(order: OrderList): boolean {
    return this.containsSearchKey(order.customerName, this.searchKey)
      || this.containsSearchKey(order.number, this.searchKey);
  }
}
