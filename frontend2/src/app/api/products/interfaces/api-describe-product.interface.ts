export namespace ApiDescribeProduct {
  export interface Response {
    id: string;
    name: string;
    vendorCode: string;
    recommendedPrice: number;
    createdOn: string;
    isArchival: boolean;
    category: Category;
    size?: Size;
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
