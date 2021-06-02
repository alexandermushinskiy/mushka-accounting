export namespace ApiGetByCategory {
  export interface Response {
    data: {
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
    }[];
  }
}
