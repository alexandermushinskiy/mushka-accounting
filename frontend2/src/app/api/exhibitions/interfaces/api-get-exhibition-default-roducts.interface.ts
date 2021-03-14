export namespace ApiGetExhibitionDefaultProducts {
  export interface Response {
    products: ExhibitionProduct[];
  }

  export interface ExhibitionProduct {
    id: string;
    name: string;
    vendorCode: string;
    sizeName: string;
    quantity: number;
    unitPrice: number;
    costPrice: number;
  }
}
