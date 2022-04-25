export namespace ApiGetByCategory {
  export interface Response {
    total: number;
    items: Product[];
  }

  export interface Product {
    id: string;
    name: string;
    vendorCode: string;
    recommendedPrice: number;
    createdOn: string;
    deliveriesCount: number;
    lastDeliveryDate: string;
    lastDeliveryCount: number;
    totalCount: number;
    quantity: number;
    sizeName: string;
  }
}
