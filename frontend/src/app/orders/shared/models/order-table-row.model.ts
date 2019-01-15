import { DataTableRow } from '../../../shared/models/data-table-row.model';
import { OrderList } from './order-list.model';

export class OrderTableRow extends DataTableRow {
  number: string;
  orderDate: string;
  cost: number;
  address: string;
  customerName: string;

  constructor(elem: OrderList, index: number = 0) {
    super(elem, index);

    this.number = elem.number;
    this.orderDate = elem.orderDate;
    this.address = elem.address;
    this.customerName = elem.customerName;
    this.cost = elem.cost;
  }
}
