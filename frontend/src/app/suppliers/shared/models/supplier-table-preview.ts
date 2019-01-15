import { DataTableRow } from '../../../shared/models/data-table-row.model';
import { ContactPerson } from '../../../shared/models/contact-person.model';

export class SupplierTablePreview extends DataTableRow {
  name: string;
  address: string;
  email: string;
  webSite: string;
  service: string;
  contactPersons: ContactPerson[];
  paymentConditions: string;
  notes: string;
  suppliesCount: number;

  constructor(elem, index: number = 0) {
    super(elem, index);

    this.name = elem.name;
    this.address = elem.address;
    this.email = elem.email || this.defaultValue;
    this.webSite = elem.webSite || this.defaultValue;
    this.notes = elem.notes;
    this.contactPersons = elem.contactPersons;
    this.paymentConditions = elem.paymentConditions;
    this.service = elem.service;
    this.suppliesCount = elem.suppliesCount;
  }
}
