export namespace ApiGetPopularProducts {
  export interface Response {
    data: {
      name: string;
      sizeName: string;
      vendorCode: string;
      quantity: number;
    }[];
  }
}
