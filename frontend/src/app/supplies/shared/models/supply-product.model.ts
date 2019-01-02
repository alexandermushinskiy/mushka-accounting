import { Product } from '../../../shared/models/product.model';
import { Size } from '../../../shared/models/size.model';

export class SupplyProduct {
  product: Product;
  quantity: number;
  costPerItem: number;

  productId: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
