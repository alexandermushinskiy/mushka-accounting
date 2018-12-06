import { SizeItem } from '../../../shared/models/size-item.model';
import { DataTablePreview } from '../../../shared/models/data-table-preview';

export class ProductTablePreview extends DataTablePreview {
  name: string;
  code: string;
  createdOn: string;
  deliveriesCount: number;
  lastDeliveryDate: string;
  lastDeliveryCount: number;
  totalCount: number;
  sizes: SizeItem[];

  constructor(elem, index: number = 0) {
    super(elem, index);

    this.name = elem.name;
    this.code = elem.code;
    this.createdOn = elem.createdOn;
    this.deliveriesCount = elem.deliveriesCount || this.defaultValue;
    this.lastDeliveryDate = elem.lastDeliveryDate || this.defaultValue;
    this.lastDeliveryCount = elem.lastDeliveryCount || this.defaultValue;
    this.totalCount = elem.totalCount;
    this.sizes = elem.sizes;
  }
}
