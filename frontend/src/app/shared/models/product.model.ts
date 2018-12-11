import { ProductSize } from './product-size.model';
import { Category } from './category.model';

export class Product {
  id: string;
  name: string;
  code: string;
  category: Category;
  categoryId: string;
  createdOn: string;
  deliveriesCount: number;
  lastDeliveryDate: string;
  lastDeliveryCount: number;
  totalCount: number;
  sizes: ProductSize[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}
