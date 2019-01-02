import { Product } from '../../../shared/models/product.model';

export class SupplyProduct {
  product: Product;
  quantity: number;
  costForItem: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
