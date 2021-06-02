export namespace ApiGetOrdersCount {
  export interface Response {
    data: {
      createdOn: string;
      quantity: number;
    }[];
  }
}
