import { Category } from './category.model';
import { Size } from './size.model';

export class Product {
  id: string;
  name: string;
  vendorCode: string;
  recommendedPrice: number;
  category: Category;
  categoryId: string;
  createdOn: string;
  size: Size;
  subProducts: SubProduct[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}

export class SubProduct {
  productId: string;
  quantity: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
