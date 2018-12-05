import { Injectable } from '@angular/core';

import { Supplier } from '../../shared/models/supplier.model';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { Category } from '../../shared/models/category.model';
import { Product } from '../../shared/models/product.model';
import { DatetimeService } from '../datetime/datetime.service';
import { SizeItem } from '../../shared/models/size-item.model';

@Injectable()
export class ConverterService {
  constructor(private datetimeService: DatetimeService) {
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

  convertToProducts(response: any[]): Product[] {
    return response.map(res => this.convertToProduct(res));
  }

  convertToProduct(source: any): Product {
    return new Product({
      id: source.id,
      name: source.name,
      code: source.code,
      createdOn: this.datetimeService.toString(source.createdOn),
      sizes: this.convertToSizes(source.sizes)
    });
  }

  convertToSizes(response: any[]): SizeItem[] {
    return response.map(res => this.convertToSize(res));
  }

  convertToSize(source: any): SizeItem {
    return new SizeItem({
      id: source.id,
      name: source.name,
      quantity: source.quantity
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
