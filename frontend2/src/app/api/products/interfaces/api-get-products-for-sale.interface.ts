export namespace ApiGetProductsForSale {
  export interface Response {
    total: number;
    items: {
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
