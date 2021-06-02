export namespace ApiUpdateProduct {
  export interface Request {
    name: string;
    vendorCode: string;
    recommendedPrice: number;
    categoryId: string;
    sizeId?: string;
    isArchival: boolean;
  }
}
