import { ContactPerson } from './contact-person.model';
import { PaymentCard } from './payment-card.model';

export class Supplier {
  id: string;
  name: string;
  address: string;
  email: string;
  webSite: string;
  notes: string;
  contactPersons: ContactPerson[] = [];
  service: string;
  suppliesCount: number;
  paymentCards: PaymentCard[] = [];

  constructor(data: any) {
    Object.assign(this, data);
  }
}
