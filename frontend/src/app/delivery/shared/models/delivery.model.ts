import { ProductItem } from './product-item.model';
import { ServiceItem } from './service-item.model';
import { PaymentMethod } from '../enums/payment-method.enum';
import { Payment } from './payment.model';

export class Delivery {
  id: string;
  supplier: string;
  requestDate: string;
  receivedDate: string;
  cost: number;
  costMethod: PaymentMethod;
  transferFee: number;
  transferFeeMethod: PaymentMethod;
  bankFee: number;
  bankFeeMethod: PaymentMethod;
  prepayment: number;
  prepaymentMethod: PaymentMethod;
  totalCost: number;
  notes: string;

  products: ProductItem[];
  // services: ServiceItem[];
  // isDraft: boolean;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
