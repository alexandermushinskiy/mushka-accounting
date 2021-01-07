export namespace ApiGetOrderDefaultProducts {
  export interface Response {
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
