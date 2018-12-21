import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, of } from 'rxjs';

import { Delivery } from '../../delivery/shared/models/delivery.model';
import { Supplier } from '../../shared/models/supplier.model';
import { ProductItem } from '../../delivery/shared/models/product-item.model';
import { ServiceItem } from '../../delivery/shared/models/service-item.model';
import { PaymentMethod } from '../../delivery/shared/enums/payment-method.enum';
import { GuidGenerator } from '../guid-generator/guid.generator';
import { Product } from '../../shared/models/product.model';
import { ConverterService } from '../converter/converter.service';
import { environment } from '../../../environments/environment';

@Injectable()
export class DeliveriesService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/deliveries`;

  // private static fakeDeliveries: Delivery[];
  // private deliveries$: BehaviorSubject<Delivery[]> = new BehaviorSubject([]);

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
    //DeliveriesService.fakeDeliveries = this.getFakeDeliveries();
    //this.loadDeliveries();
  }

  getAll(): Observable<Delivery[]> {
    return this.http.get(this.endPoint)
      .map((res: any) => this.converterService.convertToDeliveries(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getById(deliveryId: string): Observable<Delivery> {
    return this.http.get(`${this.endPoint}/${deliveryId}`)
      .map((res: any) => this.converterService.convertToDelivery(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  create(delivery: Delivery): Observable<Delivery> {
    const requestModel = this.convertToRequestData(delivery);

    return this.http.post(this.endPoint, requestModel)
      .map((res: any) => this.converterService.convertToDelivery(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  update(deliveryId: string, delivery: Delivery): Observable<Delivery> {
    const requestModel = this.convertToRequestData(delivery);

    return this.http.put(`${this.endPoint}/${deliveryId}`, requestModel)
      .map((res: any) => this.converterService.convertToDelivery(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  delete(deliveryId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${deliveryId}`)
      .catch((res: any) => throwError(res.error.messages));
  }

  private convertToRequestData(delivery: Delivery): any {
    return {
    };
  }


  // create(delivery: Delivery): Observable<Delivery> {
  //   return this.addDeliveryInternal(delivery)
  //     .map((res: any) => res.data)
  //     .catch(() => Observable.throw('Ошибка при добавлении поступления'))
  //     .delay(2000)
  //     .finally(() => this.loadDeliveries());
  // }

  // update(delivery: Delivery): Observable<Delivery> {
  //   return this.updateDeliveryInternal(delivery)
  //     .map((res: any) => res.data)
  //     .catch(() => Observable.throw('Ошибка при редактировании поступления'))
  //     .delay(2000)
  //     .finally(() => this.loadDeliveries());
  // }

  // delete(deliveryId: string) {
  //   return this.deleteDeliveryInternal(deliveryId)
  //     .catch(() => Observable.throw('Ошибка при удалении черновика поступления'))
  //     .delay(2000)
  //     .finally(() => this.loadDeliveries());
  // }

  // private loadDeliveries() {
  //   Observable.of(DeliveriesService.fakeDeliveries)
  //     .subscribe(data => this.deliveries$.next(data));
  // }

  // private addDeliveryInternal(delivery: Delivery): Observable<any> {
  //   const newDelivery = new Delivery(Object.assign({}, delivery, {
  //     id: GuidGenerator.newGuid()
  //   }));

  //   Observable.of(DeliveriesService.fakeDeliveries)
  //     .delay(2000)
  //     .subscribe(data => data.push(newDelivery));

  //   return Observable.of({data: newDelivery});
  // }

  // private updateDeliveryInternal(delivery: Delivery): Observable<any> {
  //   let storedDelivery = DeliveriesService.fakeDeliveries.find(del => del.id === delivery.id);

  //   Observable.of(DeliveriesService.fakeDeliveries)
  //     .delay(2000)
  //     .subscribe(() => {
  //       storedDelivery = Object.assign(storedDelivery, delivery);
  //     });

  //   return Observable.of({data: storedDelivery});
  // }

  // private deleteDeliveryInternal(deliveryId: string) {
  //   Observable.of(DeliveriesService.fakeDeliveries)
  //     .delay(2000)
  //     .subscribe(data => {
  //       const index = data.findIndex(del => del.id === deliveryId);
  //       data.splice(index, 1);
  //     });

  //   return Observable.of({data: deliveryId});
  // }

  // private getFakeDeliveries(): Delivery[] {
  //   return [
  //     new Delivery({
  //       id: '2A689E03-8D4A-4397-9292-2ECBD1DAEEB3',
  //       requestDate: '2018-01-13',
  //       deliveryDate:  '2018-01-17',
  //       supplier: new Supplier({
  //         id: 'FE557110-FE4E-492E-933E-EACD6A31E22D',
  //         name: 'Вова-Зи-Львов',
  //         address: 'ул. Шевченка 41, г.Львов, УКРАИНА',
  //         phone: '+380(50)921-7654',
  //         email: 'hello@vova-zi-lvova.com',
  //         contactPerson: 'Сахаров Владимир Сергеевич',
  //         paymentConditions: 'Безналичный',
  //         services: 'Бирки',
  //         webSite: 'vova-zi-lvova.com.ua'
  //       }),
  //       paymentMethod:  PaymentMethod.CREDIT_CARD,
  //       batchNumber:  'D00001',
  //       transferFee: 10.00,
  //       deliveryCost: 55.30,
  //       totalCost: 723.10,
  //       products: [
  //         new ProductItem({ product: new Product({name: 'Galaxy (GLX01)'}), amount: 100, costPerItem: 27.00, notes: 'Два носка брака' }),
  //         new ProductItem({ product: new Product({name: 'Potato (PTT01)'}), amount: 320, costPerItem: 7.50, notes: 'Неправильно пришиты бирки и что-то там еще есть' })
  //       ],
  //       services: [
  //         new ServiceItem({ name: 'Услуги фотографа', cost: 250.00, notes: 'Студия "ОЛИМП"'})
  //       ],
  //       isDraft: false
  //     }),
  //     new Delivery({
  //       id: '3D62C4A9-6A0A-473C-B82D-FF3BDC0E14D5',
  //       requestDate: '2018-01-21',
  //       deliveryDate:  '2018-01-22',
  //       supplier: new Supplier({
  //         id: 'FE557110-FE4E-492E-933E-EACD6A31E22D',
  //         name: 'Вова-Зи-Львов',
  //         address: 'ул. Шевченка 41, г.Львов, УКРАИНА',
  //         phone: '+380(50)921-7654',
  //         email: 'hello@vova-zi-lvova.com',
  //         contactPerson: 'Сахаров Владимир Сергеевич',
  //         paymentConditions: 'Безналичный',
  //         services: 'Бирки',
  //         webSite: 'vova-zi-lvova.com.ua'
  //       }),
  //       paymentMethod:  PaymentMethod.TRANSFER_TO_CARD,
  //       batchNumber:  'D00331',
  //       transferFee: 23.00,
  //       deliveryCost: 23.70,
  //       totalCost: 812.10,
  //       products: [
  //         new ProductItem({ product: new Product({name: 'Football (FTB01)'}), amount: 25, costPerItem: 1234.55 })
  //       ],
  //       services: [],
  //       isDraft: false
  //     }),
  //     new Delivery({
  //       id: '4225E8B0-D56E-488B-B2AB-9B511D0AF22F',
  //       requestDate: '2018-02-05',
  //       deliveryDate:  '2018-02-11',
  //       supplier: new Supplier({
  //         id: 'FE557110-FE4E-492E-933E-EACD6A31E22D',
  //         name: 'Вова-Зи-Львов',
  //         address: 'ул. Шевченка 41, г.Львов, УКРАИНА',
  //         phone: '+380(50)921-7654',
  //         email: 'hello@vova-zi-lvova.com',
  //         contactPerson: 'Сахаров Владимир Сергеевич',
  //         paymentConditions: 'Безналичный',
  //         services: 'Бирки',
  //         webSite: 'vova-zi-lvova.com.ua'
  //       }),
  //       paymentMethod:  PaymentMethod.CASH,
  //       batchNumber:  'D00515',
  //       transferFee: 13.13,
  //       deliveryCost: 88.02,
  //       totalCost: 2323.20,
  //       products: [
  //         new ProductItem({ product: new Product({name: 'Galaxy (GLX01)'}), amount: 100, costPerItem: 27.00, notes: 'Два носка брака' })
  //       ],
  //       services: [],
  //       isDraft: false
  //     }),
  //     new Delivery({
  //       id: 'C1FECCED-9182-4B41-BF49-FA9B7FFABD79',
  //       requestDate: '2018-03-15',
  //       deliveryDate:  '2018-02-16',
  //       supplier: new Supplier({
  //         id: 'FE557110-FE4E-492E-933E-EACD6A31E22D',
  //         name: 'Вова-Зи-Львов',
  //         address: 'ул. Шевченка 41, г.Львов, УКРАИНА',
  //         phone: '+380(50)921-7654',
  //         email: 'hello@vova-zi-lvova.com',
  //         contactPerson: 'Сахаров Владимир Сергеевич',
  //         paymentConditions: 'Безналичный',
  //         services: 'Бирки',
  //         webSite: 'vova-zi-lvova.com.ua'
  //       }),
  //       paymentMethod:  PaymentMethod.CREDIT_CARD,
  //       batchNumber:  'D00775',
  //       transferFee: 10.00,
  //       deliveryCost: 70.00,
  //       totalCost: 1200.00,
  //       products: [
  //         new ProductItem({ product: new Product({name: 'Galaxy (GLX01)'}), amount: 100, costPerItem: 27.00, notes: 'Два носка брака' })
  //       ],
  //       services: [],
  //       isDraft: true
  //     })
  //   ];
  // }
}
