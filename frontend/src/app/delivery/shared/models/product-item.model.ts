import { Product } from '../../../shared/models/product.model';
import { Size } from '../../../shared/models/size.model';

export class ProductItem {
  product: Product;
  amount: number;
  costPerItem: number;
  size: Size;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
