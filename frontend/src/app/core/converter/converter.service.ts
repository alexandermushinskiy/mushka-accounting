import { Injectable } from '@angular/core';

import { Supplier } from '../../shared/models/supplier.model';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { Category } from '../../shared/models/category.model';

@Injectable()
export class ConverterService {
  constructor() {
  }

  convertToCategories(response: any[]): Category[] {
    return response.map(res => this.convertToCategory(res));
  }

  convertToCategory(source: any): Category {
    return new Category({
      id: source.id,
      name: source.name,
    });
  }

  convertToSuppliers(response: any[]): Supplier[] {
    return response.map(res => this.convertToSupplier(res));
  }

  convertToSupplier(source: any): Supplier {
    return new Supplier({
      id: source.id,
      name: source.name,
      address: source.address,
      email: source.email,
      webSite: source.webSite,
      notes: source.notes,
      service: source.service,
      contactPersons: this.convertToContactPersons(source.contactPersons)
    });
  }

  convertToContactPersons(response: any[]): ContactPerson[] {
    return response.map(res => this.convertToContactPerson(res));
  }

  convertToContactPerson(source: any): ContactPerson {
    return new ContactPerson({
      id: source.id,
      name: source.name,
      phones: source.phones,
      email: source.email,
      position: source.position,
      city: source.city
    });
  }
}
