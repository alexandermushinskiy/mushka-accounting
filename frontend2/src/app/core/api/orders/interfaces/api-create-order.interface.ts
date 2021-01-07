import { PaymentMethod } from '../../../../shared/enums/payment-method.enum';

export namespace ApiCreateOrder {
  export interface Request {
    orderDate: string;
    number: string;
    cost: number;
    costMethod: PaymentMethod;
    discount: number;
    region: string;
    city: string;
    isWholesale: boolean;
    notes: string;
    profit: number;
    customer: {
      id?: string;
      email?: string;
      firstName: string;
      lastName: string;
      phone?: string;
    };
    products: {
      quantity: number;
      unitPrice: number;
      costPrice: number;
      productId: string;
    }[];
  }

  export interface Response {
    id: string;
    orderDate: Date;
    number: string;
    cost: string;
    costMethod: string;
    discount: number;
    region: string;
    city: string;
    isWholesale: boolean;
    notes: string;
    customer: Customer;
    products: OrderProduct[];
  }

  export interface Customer {
    id: string;
    firstName: string;
    lastName: string;
    phone: string;
    email: string;
  }

  export interface OrderProduct {
    quantity: number;
    unitPrice: number;
    costPrice: number;
    product?: {
      id: string;
      name: string;
      vendorCode: string;
      sizeName?: string;
    };
  }
}
