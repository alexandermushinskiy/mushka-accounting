import { OrderProduct } from './order-product.model';
import { PaymentMethod } from '../enums/payment-method.enum';

export class Order {
  id: string;
  orderDate: string;
  number: string;
  cost: number;
  costMethod: PaymentMethod;
  profit: number;
  //client: Client;
  region: string;
  city: string;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  notes: string;
  products: OrderProduct[];

  constructor(data: any) {
    Object.assign(this, data);
  }
}
