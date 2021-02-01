export namespace ApiSearchCorporateOrders {
  export interface Response {
    total: number;
    items?: CorporateOrder[];
  }

  export interface CorporateOrder {
    id: string;
    createdOn: Date;
    orderNumber: string;
    companyName: string;
    address: string;
  }
}
