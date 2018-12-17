import { ProductItem } from './product-item.model';
import { ServiceItem } from './service-item.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class Delivery {
  id: string;
  supplier: string;
  requestDate: string;
  deliveryDate: string;
  paymentMethod: PaymentMethod;
  transferFee: number;
  cost: number;
  totalCost: number;
  productsAmount: number;

  products: ProductItem[];
  services: ServiceItem[];
  isDraft: boolean;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
