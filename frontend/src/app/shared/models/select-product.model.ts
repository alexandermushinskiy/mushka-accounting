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
  disabled: boolean;

  get nameWithVendorCode(): string {
    return `${this.name} (${this.vendorCode})`;
  }

  constructor(data: any) {
    Object.assign(this, data);
  }
}

export class Subproduct {
  id: string;
  name: string;
  vendorCode: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
