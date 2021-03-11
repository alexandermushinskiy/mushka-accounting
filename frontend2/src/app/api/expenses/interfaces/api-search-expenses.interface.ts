export namespace ApiSearchExpenses {
  export interface Response {
    total: number;
    items: Expense[];
  }

  export interface Expense {
    id: string;
    createdOn: string;
    cost: number;
    category: string;
    purpose: string;
    supplierName: string;
  }
}
