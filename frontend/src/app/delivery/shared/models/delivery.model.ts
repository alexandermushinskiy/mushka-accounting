import { ProductItem } from './product-item.model';
import { ServiceItem } from './service-item.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class Delivery {
  id: string;
  supplier: string;
  requestDate: string;
  receivedDate: string;
  paymentMethod: PaymentMethod;
  transferFee: number;
  bankFee: number;
  cost: number;
  totalCost: number;
  productsAmount: number;
  prepayment: number;
  notes: string;

  products: ProductItem[];
  // services: ServiceItem[];
  // isDraft: boolean;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
