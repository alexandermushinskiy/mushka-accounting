export namespace ApiGetOrderDefaultProducts {
  export interface Response {
    items: OrderDefaultProduct[];
  }

  export interface OrderDefaultProduct {
    id: string;
    name: string;
    vendorCode: string;
    sizeName?: string;
    quantity: number;
    unitPrice: number;
    costPrice: number;
  }
}
