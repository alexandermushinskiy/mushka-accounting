export namespace ApiGetProductsForSale {
  export interface Response {
    data: {
      id: string;
      name: string;
      vendorCode: string;
      recommendedPrice: number;
      quantity: number;
      categoryName: string;
      sizeName?: string;
      isArchival: boolean;
    }[];
  }
}
