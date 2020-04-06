import { Product } from './product.model';

export class OrderProduct {
  product: Product;
  quantity: number;
  unitPrice: number;
  costPrice: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
