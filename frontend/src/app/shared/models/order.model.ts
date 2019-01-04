import { OrderProduct } from './order-product.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class Order {
  id: string;
  orderDate: string;
  cost: number;
  costMethod: PaymentMethod;
  notes: string;
  products: OrderProduct[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}
