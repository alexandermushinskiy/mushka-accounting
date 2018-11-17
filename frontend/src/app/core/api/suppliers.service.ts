import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Supplier } from '../../shared/models/supplier.model';
import { environment } from '../../../environments/environment';
import { ConverterService } from '../converter/converter.service';

@Injectable()
export class SuppliersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/suppliers`;

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
    return this.http.post(this.endPoint, supplier)
      .map((res: any) => this.converterService.convertToSupplier(res.data))
      .catch((res: any) => Observable.throw(res.error.messages));
  }

  update(supplierId: string, supplier: Supplier): Observable<Supplier> {
    return this.http.put(`${this.endPoint}/${supplierId}`, supplier)
      .map((res: any) => this.converterService.convertToSupplier(res.data))
      .catch((error) => Observable.throw(error.error.messages));
  }
}
