export namespace ApiCreateProduct {
  export interface Request {
    name: string;
    vendorCode: string;
    recommendedPrice: number;
    categoryId: string;
    sizeId?: string;
    isArchival: boolean;
  }
}
