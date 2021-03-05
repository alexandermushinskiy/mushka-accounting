export namespace ApiSearchSupplies {
  export interface Request {
    searchKey: string;
    productId: string;
  }

  export interface Response {
    total: number;
    items: Supply[];
  }

  export interface Supply {
    id: string;
    supplierName: string;
    description: string;
    receivedDate: string;
    cost: number;
    totalCost: number;
    productsAmount: number;
  }
}
