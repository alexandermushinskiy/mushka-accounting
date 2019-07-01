import { OrderProduct } from './order-product.model';
import { PaymentMethod } from '../enums/payment-method.enum';
import { Customer } from './customer.model';

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
  notes: string;
  isWholesale: boolean;
  products: OrderProduct[];
  customer: Customer;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
