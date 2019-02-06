import { Size } from './size.model';
import { Category } from './category.model';

export class SelectProduct {
  id: string;
  name: string;
  vendorCode: string;
  recommendedPrice: number;
  quantity: number;
  category: Category;
  size: Size;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
