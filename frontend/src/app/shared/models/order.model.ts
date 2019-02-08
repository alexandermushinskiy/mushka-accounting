import { OrderProduct } from './order-product.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class Order {
  id: string;
  orderDate: string;
  number: string;
  cost: number;
  costMethod: PaymentMethod;
  discount: number;
  profit: number;
  region: string;
  city: string;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  notes: string;
  isWholesale: boolean;
  products: OrderProduct[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}
