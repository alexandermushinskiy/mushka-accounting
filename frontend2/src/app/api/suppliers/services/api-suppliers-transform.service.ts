import { Injectable } from '@angular/core';

import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { ContactPerson } from '../../../shared/models/contact-person.model';
import { PaymentCard } from '../../../shared/models/payment-card.model';
import { Supplier } from '../../../shared/models/supplier.model';
import { ApiCreateSupplier } from '../interfaces/api-create-supplier.interface';
import { ApiDescribeSupplier } from '../interfaces/api-describe-supplier.interface';
import { ApiSearchSuppliers } from '../interfaces/api-search-suppliers.interface';
import { ApiUpdateSupplier } from '../interfaces/api-update-supplier.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiSuppliersTransformService {
  fromSearchSuppliers(response: ApiSearchSuppliers.Response): ItemsList<Supplier> {
    const items = response.items || [];

    return {
      items: items.map(item => this.toSupplier(item)),
      length: response.total
    };
  }

  fromDescribeSupplier(response: ApiDescribeSupplier.Response): Supplier {
    return this.toSupplier(response);
  }

  toCreateSupplier(supplier: Supplier): ApiCreateSupplier.Request {
    return {
      supplier: {
        name: supplier.name,
        address: supplier.address,
        email: supplier.email,
        webSite: supplier.webSite,
        notes: supplier.notes,
        service: supplier.service
      },
      contactPersons: supplier.contactPersons.map(cp => {
        return {
          name: cp.name,
          phones: cp.phones,
          email: cp.email
        };
      }),
      paymentCards: supplier.paymentCards.map(pc => {
        return {
          number: pc.number,
          owner: pc.owner
        };
      })
    };
  }

  toUpdateSupplier(supplier: Supplier): ApiUpdateSupplier.Request {
    return {
      supplier: {
        name: supplier.name,
        address: supplier.address,
        email: supplier.email,
        webSite: supplier.webSite,
        notes: supplier.notes,
        service: supplier.service
      },
      contactPersons: supplier.contactPersons.map(cp => {
        return {
          id: cp.id,
          name: cp.name,
          phones: cp.phones,
          email: cp.email
        };
      }),
      paymentCards: supplier.paymentCards.map(pc => {
        return {
          id: pc.id,
          number: pc.number,
          owner: pc.owner
        };
      })
    };
  }

  private toSupplier({ supplier, contactPersons, paymentCards }: ApiSearchSuppliers.Supplier | ApiDescribeSupplier.Response): Supplier {
    return new Supplier({
      id: supplier.id,
      name: supplier.name,
      address: supplier.address,
      email: supplier.email,
      webSite: supplier.webSite,
      notes: supplier.notes,
      service: supplier.service,
      suppliesCount: supplier.suppliesCount,
      contactPersons: contactPersons.map(data => this.convertToContactPerson(data)),
      paymentCards: paymentCards.map(data => this.convertToPaymentCard(data))
    });
  }

  private convertToContactPerson(source: any): ContactPerson {
    return new ContactPerson({
      id: source.id,
      name: source.name,
      phones: source.phones,
      email: source.email,
      position: source.position,
      city: source.city
    });
  }

  private convertToPaymentCard(source: any): PaymentCard {
    return new PaymentCard({
      id: source.id,
      number: source.number,
      owner: source.owner
    });
  }
}
