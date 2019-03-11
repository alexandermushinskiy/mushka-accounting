import { DataTableRow } from '../../../shared/models/data-table-row.model';
import { ProductList } from '../../../shared/models/product-list.model';

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
  sizeName: string;

  constructor(elem: ProductList, index: number = 0) {
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
    this.sizeName = elem.sizeName;
  }
}
