import { Injectable } from '@angular/core';

import { Supplier } from '../../shared/models/supplier.model';
import { ContactPerson } from '../../shared/models/contact-person.model';
import { Category } from '../../shared/models/category.model';
import { Product } from '../../shared/models/product.model';
import { DatetimeService } from '../datetime/datetime.service';
import { ProductSize } from '../../shared/models/product-size.model';
import { Size } from '../../shared/models/size.model';
import { Supply } from '../../shared/models/supply.model';
import { PaymentCard } from '../../shared/models/payment-card.model';
import { SupplyProduct } from '../../shared/models/supply-product.model';
import { Order } from '../../shared/models/order.model';
import { OrderProduct } from '../../shared/models/order-product.model';
import { Customer } from '../../shared/models/customer.model';
import { OrderList } from '../../shared/models/order-list.model';
import { SelectProduct } from '../../shared/models/select-product.model';
import { Exhibition } from '../../shared/models/exhibition.model';
import { ExhibitionProduct } from '../../shared/models/exhibition-product.model';
import { ExhibitionList } from '../../shared/models/exhibition-list.model';
import { Expense } from '../../shared/models/expense.model';
import { SupplyList } from '../../shared/models/supply-list.model';
import { ProductList } from '../../shared/models/product-list.model';
import { CorporateOrderList } from '../../shared/models/corporate-order-list.model';
import { CorporateOrder } from '../../shared/models/corporate-order.model';
import { CorporateOrderProduct } from '../../shared/models/corporate-order-product.model';

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
      isSizeRequired: source.isSizeRequired,
      isAdditional: source.isAdditional
    });
  }

  convertToSelectProducts(response: any[]): SelectProduct[] {
    return response.map(res => new SelectProduct({
        id: res.id,
        name: res.name,
        vendorCode: res.vendorCode,
        recommendedPrice: res.recommendedPrice,
        quantity: res.quantity,
        category: new Category({name: res.categoryName}),
        size: !!res.sizeName ? new Size({name: res.sizeName}) : null,
        disabled: res.isArchival
      })
    );
  }

  convertToProductsList(response: any[]): ProductList[] {
    return response.map(res => this.convertToProductList(res));
  }

  convertToProductList(source: any): ProductList {
    return new ProductList({
      id: source.id,
      name: source.name,
      vendorCode: source.vendorCode,
      recommendedPrice: source.recommendedPrice,
      createdOn: this.datetimeService.toString(source.createdOn),
      sizeName: source.sizeName,
      lastDeliveryDate: !!source.lastDeliveryDate ? this.datetimeService.toString(source.lastDeliveryDate) : null,
      lastDeliveryCount: !!source.lastDeliveryCount ? source.lastDeliveryCount : 0,
      deliveriesCount: source.deliveriesCount,
      quantity: source.quantity
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
      recommendedPrice: source.recommendedPrice,
      category: this.convertToCategory(source.category),
      categoryId: source.categoryId,
      createdOn: this.datetimeService.toString(source.createdOn),
      size: !!source.size ? this.convertToProductSize(source.size) : null,
      isArchival: source.isArchival
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
      suppliesCount: source.suppliesCount,
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

  convertToSuppliesList(response: any[]): SupplyList[] {
    return response.map(res => this.convertToSupplyList(res));
  }

  convertToSupplyList(source: any): SupplyList {
    return new SupplyList({
      id: source.id,
      supplierName: source.supplierName,
      description: source.description,
      receivedDate: this.datetimeService.toString(source.receivedDate),
      cost: source.cost,
      totalCost: source.totalCost,
      productsAmount: source.productsAmount,
    });
  }

  convertToSupplies(response: any[]): Supply[] {
    return response.map(res => this.convertToSupply(res));
  }

  convertToSupply(source: any): Supply {
    return new Supply({
      id: source.id,
      supplierId: source.supplierId,
      supplierName: source.supplierName,
      description: source.description,
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
      productsAmount: source.productsAmount,
      notes: source.notes,
      products: source.products.map((prod: any) => new SupplyProduct({
        quantity: prod.quantity,
        unitPrice: prod.unitPrice,
        product: {
          id: prod.product.id,
          name: prod.product.name,
          vendorCode: prod.product.vendorCode,
          size: !!prod.product.sizeName ? new Size({name: prod.product.sizeName}) : null
        }
      }))
    });
  }

  convertToOrders(response: any[]): Order[] {
    return response.map(res => this.convertToOrder(res));
  }

  convertToOrder(source: any): Order {
    return new Order({
      id: source.id,
      orderDate: this.datetimeService.toString(source.orderDate),
      number: source.number,
      cost: source.cost,
      costMethod: source.costMethod,
      discount: source.discount,
      region: source.region,
      city: source.city,
      isWholesale: source.isWholesale,
      notes: source.notes,
      customer: this.convertToCustomer(source.customer),
      products: this.convertToOrderProducts(source.products)
    });
  }

  convertToOrderProducts(response: any[]): OrderProduct[] {
    return response.map((orderProduct: any) => new OrderProduct({
      quantity: orderProduct.quantity,
      unitPrice: orderProduct.unitPrice,
      costPrice: orderProduct.costPrice,
      product: {
        id: orderProduct.id,
        name: orderProduct.name,
        vendorCode: orderProduct.vendorCode,
        size: !!orderProduct.sizeName ? new Size({name: orderProduct.sizeName}) : null
      }
    }));
  }

  convertToOrdersList(response: any[]): OrderList[] {
    return response.map(res => this.convertToOrderList(res));
  }

  convertToOrderList(source: any): OrderList {
    return new OrderList({
      id: source.id,
      orderDate: this.datetimeService.toString(source.orderDate),
      orderNumber: source.number,
      cost: source.cost,
      address: source.address,
      customerName: source.customerName,
      productsCount: source.productsCount,
      isWholesale: source.isWholesale
    });
  }

  convertToCustomers(response: any[]): Customer[] {
    return response.map(res => this.convertToCustomer(res));
  }

  convertToCustomer(source: any): Customer {
    return new Customer({
      id: source.id,
      firstName: source.firstName,
      lastName: source.lastName,
      phone: source.phone,
      email: source.email
    });
  }

  convertToCorporateOrdersList(response: any[]): CorporateOrderList[] {
    return response.map(res => {
      return new CorporateOrderList({
        id: res.id,
        orderNumber: res.number,
        createdOn: this.datetimeService.toString(res.createdOn),
        address: res.address,
        companyName: res.companyName,
      });
    });
  }

  convertToCorporateOrder(source: any): CorporateOrder {
    return new CorporateOrder({
      id: source.id,
      createdOn: this.datetimeService.toString(source.createdOn),
      orderNumber: source.number,
      cost: source.cost,
      costMethod: source.costMethod,
      prepayment: source.prepayment,
      prepaymentMethod: source.prepaymentMethod,
      deliveryCost: source.deliveryCost,
      deliveryCostMethod: source.deliveryCostMethod,
      tax: source.tax,
      profit: source.profit,
      companyName: source.companyName,
      contactPerson: source.contactPerson,
      phone: source.phone,
      email: source.email,
      notes: source.notes,
      region: source.region,
      city: source.city,
      products: source.products.map(prod => new CorporateOrderProduct({
        id: prod.id,
        name: prod.name,
        quantity: prod.quantity,
        unitPrice: prod.unitPrice,
        costPrice: prod.costPrice
      }))
    });
  }

  convertToExhibitions(response: any[]): Exhibition[] {
    return response.map(res => this.convertToExhibition(res));
  }

  convertToExhibition(source: any): Exhibition {
    return new Exhibition({
      id: source.id,
      name: source.name,
      fromDate: this.datetimeService.toString(source.fromDate),
      toDate: this.datetimeService.toString(source.toDate),
      city: source.city,
      participationCost: source.participationCost,
      participationCostMethod: source.participationCostMethod,
      accommodationCost: source.accommodationCost,
      accommodationCostMethod: source.accommodationCostMethod,
      fareCost: source.fareCost,
      fareCostMethod: source.fareCostMethod,
      notes: source.notes,
      profit: source.profit,
      products: this.convertToExhibitionProducts(source.products)
    });
  }

  convertToExhibitionProducts(response: any[]): ExhibitionProduct[] {
    return response.map((exhibitionProduct: any) => new ExhibitionProduct({
      quantity: exhibitionProduct.quantity,
      unitPrice: exhibitionProduct.unitPrice,
      costPrice: exhibitionProduct.costPrice,
      product: {
        id: exhibitionProduct.id,
        name: exhibitionProduct.name,
        vendorCode: exhibitionProduct.vendorCode,
        size: !!exhibitionProduct.sizeName ? new Size({name: exhibitionProduct.sizeName}) : null
      }
    }));
  }

  convertToExhibitionsList(response: any[]): ExhibitionList[] {
    return response.map(res => this.convertToExhibitionList(res));
  }

  convertToExhibitionList(source: any): ExhibitionList {
    return new ExhibitionList({
      id: source.id,
      name: source.name,
      fromDate: this.datetimeService.toString(source.fromDate),
      toDate: this.datetimeService.toString(source.toDate),
      city: source.city,
      participationCost: source.participationCost,
      profit: source.profit,
      productsCount: source.productsCount
    });
  }

  convertToExpenses(response: any[]): Expense[] {
    return response.map(res => this.convertToExpense(res));
  }

  convertToExpense(source: any): Expense {
    return new Expense({
      id: source.id,
      createdOn: this.datetimeService.toString(source.createdOn),
      cost: source.cost,
      costMethod: source.costMethod,
      category: source.category,
      purpose: source.purpose,
      supplierName: source.supplierName,
      notes: source.notes
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
