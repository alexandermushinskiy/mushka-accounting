import { Category } from './category.model';

export class ProductList {
  id: string;
  name: string;
  vendorCode: string;
  recommendedPrice: number;
  // category: Category;
  // categoryId: string;
  createdOn: string;
  deliveriesCount: number;
  lastDeliveryDate: string;
  lastDeliveryCount: number;
  totalCount: number;
  quantity = 0;
  sizeName: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
