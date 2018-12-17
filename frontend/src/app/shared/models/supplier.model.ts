import { ContactPerson } from './contact-person.model';

export class Supplier {
  id: string;
  name: string;
  address: string;
  email: string;
  webSite: string;
  notes: string;
  contactPersons: ContactPerson[] = [];
  service: string;
  deliveriesCount: number;

  //paymentConditions: string;

  constructor(data: any) {
    Object.assign(this, data);
  }
}
