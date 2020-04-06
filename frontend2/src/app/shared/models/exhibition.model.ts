import { ExhibitionProduct } from './exhibition-product.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class Exhibition {
  id: string;
  name: string;
  fromDate: string;
  toDate: string;
  city: string;
  participationCost: number;
  participationCostMethod: PaymentMethod;
  accommodationCost: number;
  accommodationCostMethod: PaymentMethod;
  fareCost: number;
  fareCostMethod: PaymentMethod;
  notes: string;
  profit: number;
  products: ExhibitionProduct[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}
