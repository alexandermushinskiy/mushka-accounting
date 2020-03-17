import { FiltersBase } from './filter-base';
import { OrderList } from '../../shared/models/order-list.model';
import { DateRange } from '../models/date-range.model';

export class OrderListFilter extends FiltersBase {
  constructor(private searchKey: string, private dateRange: DateRange) {
    super();
  }

  filter(order: OrderList): boolean {
//debugger
    let isInDateRange = true;
    let foundBySearch = true;

    if (!!this.dateRange) {
      isInDateRange = this.isBetween(order.orderDate, this.dateRange);
    }

    if (!!this.searchKey) {
      foundBySearch = [order.customerName, order.orderNumber]
        .some(field => field.toLowerCase().includes(this.searchKey.toLowerCase()));
    }

    return isInDateRange && foundBySearch;

    // return (!this.dateRange || this.isBetween(order.orderDate, this.dateRange)) &&
    //   (this.containsSearchKey(order.customerName, this.searchKey) || this.containsSearchKey(order.orderNumber, this.searchKey));
  }
}
