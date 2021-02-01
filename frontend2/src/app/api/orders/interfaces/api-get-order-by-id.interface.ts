export namespace ApiGetOrderById {
  export interface Response {
    order: Order;
    customer: Customer;
    products: OrderProduct[];
  }

  export interface Order {
    id: string;
    orderDate: Date;
    number: string;
    cost: string;
    costMethod: string;
    discount: string;
    region: string;
    city: string;
    isWholesale: boolean;
    notes: string;
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
