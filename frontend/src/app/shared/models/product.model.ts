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
  deliveriesCount: number;
  lastDeliveryDate: string;
  lastDeliveryCount: number;
  totalCount: number;
  quantity = 0;
  size: Size;
  subproducts: Subproduct[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}

export class Subproduct {
  productId: string;
  product: Product;
  quantity: number;
  
  constructor(data: any) {
    Object.assign(this, data);
  }
}