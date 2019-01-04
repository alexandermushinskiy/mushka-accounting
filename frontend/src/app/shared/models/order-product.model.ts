import { Product } from './product.model';

export class OrderProduct {
  product: Product;
  quantity: number;
  costForItem: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
