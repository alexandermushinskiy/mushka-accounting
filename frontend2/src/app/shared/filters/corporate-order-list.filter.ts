import { FiltersBase } from './filter-base';
import { CorporateOrderList } from '../models/corporate-order-list.model';
import { DateRange } from '../models/date-range.model';

export class CorporateOrderListFilter extends FiltersBase {
  constructor(private searchKey: string,
              private dateRange: DateRange) {
    super();
  }

  filter(order: CorporateOrderList): boolean {
    let isInDateRange = true;
    let foundBySearch = true;

    if (!!this.dateRange) {
      isInDateRange = this.isBetween(order.createdOn, this.dateRange);
    }

    if (!!this.searchKey) {
      foundBySearch = [order.companyName, order.orderNumber]
        .some(field => field.toLowerCase().includes(this.searchKey.toLowerCase()));
    }

    return isInDateRange && foundBySearch;
  }
}
