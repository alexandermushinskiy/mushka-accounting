export namespace ApiDescribeProduct {
  export interface Response {
    product: Product;
    category: Category;
    size?: Size;
  }

  export interface Product {
    id: string;
    name: string;
    vendorCode: string;
    recommendedPrice: number;
    createdOn: string;
    isArchival: boolean;
  }

  export interface Size {
    id: string;
    name: string;
  }

  export interface Category {
    id: string;
    name: string;
  }
}
