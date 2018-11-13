import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs';

import { Supplier } from '../../shared/models/supplier.model';
import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';

@Injectable()
export class SuppliersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/suppliers`;

  private suppliers$: BehaviorSubject<Supplier[]> = new BehaviorSubject([]);

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getAll(): Observable<Supplier[]> {
    return this.http.get(`${this.endPoint}`)
      .map((res: any) => this.converterService.convertToSuppliers(res.data))
      .catch((res: any) => Observable.throw(res.error.messages));
  }

  getById(supplierId: string): Observable<Supplier> {
    return this.http.get(`${this.endPoint}/${supplierId}`)
      .map((res: any) => this.converterService.convertToSupplier(res.data))
      .catch((res: any) => Observable.throw(res.error.messages));
  }

  create(supplier: Supplier): Observable<Supplier> {
    //const requestData = this.convertToRequestData(supplier);

    return this.http.post(this.endPoint, supplier)
      .map((res: any) => this.converterService.convertToSupplier(res.data))
      .catch((res: any) => Observable.throw(res.error.messages));
  }

  update(supplierId: string, supplier: Supplier): Observable<Supplier> {
    //const requestData = this.convertToRequestData(supplier);
debugger;
    return this.http.put(`${this.endPoint}/${supplierId}`, supplier)
      .map((res: any) => this.converterService.convertToSupplier(res.data))
      .catch((error) => Observable.throw(error.error.messages));
  }

  private convertToRequestData(supplier: Supplier): any {
    return {
      name: supplier.name,
      address: supplier.address,
      email: supplier.email,
      webSite: supplier.webSite,
      notes: supplier.notes,
      service: supplier.service,
      contactPersons: supplier.contactPersons.map(cp => {
        return {
          name: cp.name,
          phones: cp.phones,
          email: cp.email
        }
      })
    };
  }

  // getSuppliers(): Observable<Supplier[]> {
  //   return this.suppliers$.asObservable().delay(500);
  // }

  // addSupplier(supplier: Supplier): Observable<Supplier> {
  //   return this.addSupplierInternal(supplier)
  //     .map((res: any) => res.data)
  //     .catch(() => Observable.throw('Ошибка добавление поставщика'))
  //     .delay(2000)
  //     .finally(() => this.loadSuppliers());
  // }

  // private addSupplierInternal(supplier: Supplier): Observable<any> {
  //   const newSupplier = new Supplier(Object.assign({}, supplier, {
  //     id: '11111111-C9B6-4ACF-A478-5185A07C39BF',
  //     createdOn: '2018-04-05'
  //   }));

  //   Observable.of(SuppliersService.fakeSuppliers)
  //     .delay(2000)
  //     .subscribe(data => data.push(newSupplier));

  //   return Observable.of({data: newSupplier});
  // }

  // private loadSuppliers() {
  //   Observable.of(SuppliersService.fakeSuppliers)
  //     .subscribe(data => this.suppliers$.next(data));
  // }

  // private getFakeSuppliers(): Supplier[] {
  //   return [
  //     new Supplier({
  //       id: 'FE5570E0-FE4E-492E-933E-EACD6A31E22D',
  //       name: 'ТОВ "Новая Линия"',
  //       address: 'ул. Центральная 11/3, г.Тернополь, УКРАИНА',
  //       phone: '+380(98)412-1212',
  //       email: 'info@socks.com',
  //       webSite: 'socks.com.ua',
  //       contactPerson: 'Иванов Иван Иванович',
  //       paymentConditions: 'Наличный, безналичный',
  //       services: 'Носки',
  //       comments: ''
  //     }),
  //     new Supplier({
  //       id: 'FE557110-FE4E-492E-933E-EACD6A31E22D',
  //       name: 'Вова-Зи-Львов',
  //       address: 'ул. Шевченка 41, г.Львов, УКРАИНА',
  //       phone: '+380(50)921-7654',
  //       email: 'hello@vova-zi-lvova.com',
  //       webSite: 'vova-zi-lvova.com.ua',
  //       contactPerson: 'Сахаров Владимир Сергеевич',
  //       paymentConditions: 'Безналичный',
  //       services: 'Бирки',
  //       comments: ''
  //     })
  //   ];
  // }
}
