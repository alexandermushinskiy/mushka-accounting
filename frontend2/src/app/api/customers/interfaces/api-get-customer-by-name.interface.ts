export namespace ApiGetCustomerByName {
  export interface Response {
    customers: Customer[];
  }

  export interface Customer {
    id: string;
    firstName: string;
    lastName: string;
    phone: string;
    email: string;
  }
}
