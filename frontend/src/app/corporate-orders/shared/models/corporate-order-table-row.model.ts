import { DataTableRow } from '../../../shared/models/data-table-row.model';
import { CorporateOrderList } from './corporate-order-list.model';

export class CorporateOrderTableRow extends DataTableRow {
  number: string;
  createdOn: string;
  address: string;
  companyName: string;

  constructor(elem: CorporateOrderList, index: number = 0) {
    super(elem, index);

    this.number = elem.number;
    this.createdOn = elem.createdOn;
    this.address = elem.address;
    this.companyName = elem.companyName;
  }
}