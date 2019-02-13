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
  notes: string;
  profit: number;
  products: ExhibitionProduct[];

  accommodationСost: number;
  accommodationСostMethod: PaymentMethod;
  fareCost: number;
  fareCostMethod: PaymentMethod;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
