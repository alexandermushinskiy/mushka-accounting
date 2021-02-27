export namespace ApiDescribeSupplier {
  export interface Response {
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
