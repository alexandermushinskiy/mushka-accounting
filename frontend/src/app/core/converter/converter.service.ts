import { Injectable } from '@angular/core';

import { Supplier } from '../../shared/models/supplier.model';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { Category } from '../../shared/models/category.model';
import { Product } from '../../shared/models/product.model';
import { DatetimeService } from '../datetime/datetime.service';
import { ProductSize } from '../../shared/models/product-size.model';
import { Size } from '../../shared/models/size.model';
import { Supply } from '../../supplies/shared/models/supply.model';
import { PaymentCard } from '../../shared/models/payment-card.model';
import { copyStyles } from '@angular/animations/browser/src/util';

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
      isSizeRequired: source.isSizeRequired
    });
  }

  convertToProducts(response: any[]): Product[] {
    return response.map(res => this.convertToProduct(res));
  }

  convertToProduct(source: any): Product {
    return new Product({
      id: source.id,
      name: source.name,
      vendorCode: source.vendorCode,
      category: this.convertToCategory(source.category),
      categoryId: source.categoryId,
      createdOn: this.datetimeService.toString(source.createdOn),
      size: this.convertToProductSize(source.size),
      lastDeliveryDate: !!source.lastDeliveryDate ? this.datetimeService.toString(source.lastDeliveryDate) : null,
      lastDeliveryCount: !!source.lastDeliveryCount ? source.lastDeliveryCount : 0,
      deliveriesCount: source.deliveriesCount,
      quantity: source.quantity
    });
  }

  convertToProductSizes(response: any[]): ProductSize[] {
    return response.map(res => this.convertToProductSize(res));
  }

  convertToProductSize(source: any): ProductSize {
    return new ProductSize({
      id: source.id,
      name: source.name,
      quantity: source.quantity
    });
  }

  convertToSizes(response: any[]): Size[] {
    return response.map(res => this.convertToSize(res));
  }

  convertToSize(source: any): Size {
    return new Size({
      id: source.id,
      name: source.name
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
      deliveriesCount: source.deliveriesCount,
      contactPersons: this.convertToContactPersons(source.contactPersons),
      paymentCards: this.convertToPaymentCards(source.paymentCards)
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

  convertToSupplies(response: any[]): Supply[] {
    return response.map(res => this.convertToSupply(res));
  }

  convertToSupply(source: any): Supply {
    return new Supply({
      id: source.id,
      supplier: source.supplierName,
      requestDate: this.datetimeService.toString(source.requestDate),
      receivedDate: this.datetimeService.toString(source.receivedDate),
      prepayment: source.prepayment,
      prepaymentMethod: source.prepaymentMethod,
      cost: source.cost,
      costMethod: source.costMethod,
      deliveryCost: source.deliveryCost,
      deliveryCostMethod: source.deliveryCostMethod,
      bankFee: source.bankFee,
      totalCost: source.totalCost,
      productsAmount: source.productsAmount
    });
  }

  private convertToPaymentCards(response: any[]): PaymentCard[] {
    return response.map(res => this.convertToPaymentCard(res));
  }

  private convertToPaymentCard(source: any): PaymentCard {
    return new PaymentCard({
      id: source.id,
      number: source.number,
      owner: source.owner
    });
  }
}
