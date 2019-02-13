import { DataTableRow } from '../../../shared/models/data-table-row.model';
import { ExhibitionList } from './exhibition-list.model';

export class ExhibitionTableRow extends DataTableRow {
  name: string;
  fromDate: string;
  toDate: string;
  city: string;
  participationCost: number;
  profit: number;
  productsCount: number;

  constructor(elem: ExhibitionList, index: number = 0) {
    super(elem, index);

    this.name = elem.name;
    this.fromDate = elem.fromDate;
    this.toDate = elem.toDate;
    this.city = elem.city;
    this.participationCost = elem.participationCost;
    this.profit = elem.profit;
    this.productsCount = elem.productsCount;
  }
}
