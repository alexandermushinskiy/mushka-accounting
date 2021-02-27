export namespace ApiCreateSupplier {
  export interface Request {
    supplier: Supplier;
    contactPersons: ContactPerson[];
    paymentCards: PaymentCard[];
  }

  export interface Supplier {
    name: string;
    address: string;
    email: string;
    webSite: string;
    notes: string;
    service: string;
  }

  export interface ContactPerson {
    id?: string;
    name: string;
    phones: string;
    email: string;
  }

  export interface PaymentCard {
    id?: string;
    number: string;
    owner: string;
  }
}
