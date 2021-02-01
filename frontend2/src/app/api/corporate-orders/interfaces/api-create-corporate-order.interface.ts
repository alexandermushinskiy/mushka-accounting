import { PaymentMethod } from '../../../shared/enums/payment-method.enum';

export namespace ApiCreateCorporateOrder {
  export interface Request {
    createdOn: string;
    orderNumber: string;
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
    products: {
      name: string;
      quantity: number;
      unitPrice: number;
      costPrice: number;
    }[];
  }
}
