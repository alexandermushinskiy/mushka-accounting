import { Product } from '../../../shared/models/product.model';
import { Size } from 'src/app/shared/models/size.model';

export class ProductItem {
  // id: string;
  product: Product;
  amount: number;
  costPerItem: number;
  totalCost: number;
  notes: string;

  size: Size;
  name: string;
  hasSizes: boolean;

  constructor(data: any) {
    Object.assign(this, data);
    this.totalCost = Math.round((this.costPerItem * this.amount) * 100) / 100;
  }
}
