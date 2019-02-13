import { Product } from './product.model';

export class ExhibitionProduct {
  product: Product;
  quantity: number;
  unitPrice: number;
  costPrice: number;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
