import { PaymentMethod } from '../enums/payment-method.enum';
import { CorporateOrderProduct } from './corporate-order-product.model';

export class CorporateOrder {
  id: string;
  createdOn: string;
  number: string;
  cost: number;
  costMethod: PaymentMethod;
  prepayment: number;
  prepaymentMethod: PaymentMethod;
  deliveryCost: number;
  deliveryCostMethod: PaymentMethod;
  tax: number;
  profit: number;
  region: string;
  city: string;
  companyName: string;
  contactPerson: string;
  phone: string;
  email: string;
  notes: string;
  products: CorporateOrderProduct[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}
