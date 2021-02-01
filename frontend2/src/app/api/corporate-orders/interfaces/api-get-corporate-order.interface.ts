import { PaymentMethod } from '../../../shared/enums/payment-method.enum';

export namespace ApiGetCorporateOrder {
  export interface Response {
    order: Order;
    products: OrderProduct[];
  }

  export interface Order {
    id: string;
    createdOn: Date;
    orderNumber: string;
    cost: number;
    costMethod: PaymentMethod;
    prepayment?: number;
    prepaymentMethod?: PaymentMethod;
    deliveryCost?: number;
    deliveryCostMethod?: PaymentMethod;
    tax: number;
    profit: number;
    companyName: string;
    contactPerson: string;
    phone: string;
    email: string;
    notes: string;
    region: string;
    city: string;
  }

  export interface OrderProduct {
    name: string;
    quantity: number;
    unitPrice: number;
    costPrice: number;
  }
}
