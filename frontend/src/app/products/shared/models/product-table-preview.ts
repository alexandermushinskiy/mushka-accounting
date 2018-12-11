import { ProductSize } from '../../../shared/models/product-size.model';
import { DataTablePreview } from '../../../shared/models/data-table-preview.model';

export class ProductTablePreview extends DataTablePreview {
  name: string;
  code: string;
  createdOn: string;
  deliveriesCount: number;
  lastDeliveryDate: string;
  lastDeliveryCount: number;
  totalCount: number;
  sizes: ProductSize[];

  constructor(elem, index: number = 0) {
    super(elem, index);

    this.name = elem.name;
    this.code = elem.code;
    this.createdOn = elem.createdOn;
    this.deliveriesCount = elem.deliveriesCount || 0;
    this.lastDeliveryDate = elem.lastDeliveryDate;
    this.lastDeliveryCount = elem.lastDeliveryCount;
    this.totalCount = elem.totalCount;
    this.sizes = elem.sizes;
  }
}
