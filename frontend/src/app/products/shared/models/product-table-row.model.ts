import { DataTablePreview } from '../../../shared/models/data-table-preview.model';
import { Size } from '../../../shared/models/size.model';

export class ProductTableRow extends DataTablePreview {
  name: string;
  vendorCode: string;
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
    this.createdOn = elem.createdOn;
    this.deliveriesCount = elem.deliveriesCount || 0;
    this.lastDeliveryDate = elem.lastDeliveryDate;
    this.lastDeliveryCount = elem.lastDeliveryCount;
    this.totalCount = elem.totalCount;
    this.quantity = elem.quantity;
    this.size = elem.size;
  }
}
