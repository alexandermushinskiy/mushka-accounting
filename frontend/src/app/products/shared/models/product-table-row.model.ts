import { DataTableRow } from '../../../shared/models/data-table-row.model';
import { Size } from '../../../shared/models/size.model';

export class ProductTableRow extends DataTableRow {
  name: string;
  vendorCode: string;
  recommendedPrice: number;
  createdOn: string;
  deliveriesCount: number;
  lastDeliveryDate: string;
  lastDeliveryCount: number;
  totalCount: number;
  quantity: number;
  size: Size;

  constructor(elem, index: number = 0) {
    super(elem, index);

    this.name = elem.name;
    this.vendorCode = elem.vendorCode;
    this.recommendedPrice = elem.recommendedPrice;
    this.createdOn = elem.createdOn;
    this.deliveriesCount = elem.deliveriesCount || 0;
    this.lastDeliveryDate = elem.lastDeliveryDate;
    this.lastDeliveryCount = elem.lastDeliveryCount;
    this.totalCount = elem.totalCount;
    this.quantity = elem.quantity;
    this.size = elem.size;
  }
}
