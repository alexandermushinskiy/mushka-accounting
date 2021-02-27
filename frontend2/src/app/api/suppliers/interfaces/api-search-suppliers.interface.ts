export namespace ApiSearchSuppliers {
  export interface Response {
    total: number;
    items: Supplier[];
  }

  export interface Supplier {
    supplier: {
      id: string;
      name: string;
      address: string;
      email: string;
      webSite: string;
      notes: string;
      service: string;
      suppliesCount: number;
    };
    contactPersons: {
      id: string;
      name: string;
      position: string;
      city: string;
      phones: string;
      email: string;
    }[];
    paymentCards: {
      id: string;
      number: string;
      owner: string;
    }[];
  }
}
