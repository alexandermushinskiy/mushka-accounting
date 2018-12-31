import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, of } from 'rxjs';

import { Supply } from '../../delivery/shared/models/supply.model';
import { Supplier } from '../../shared/models/supplier.model';
import { ProductItem } from '../../delivery/shared/models/product-item.model';
import { ServiceItem } from '../../delivery/shared/models/service-item.model';
import { PaymentMethod } from '../../delivery/shared/enums/payment-method.enum';
import { GuidGenerator } from '../guid-generator/guid.generator';
import { Product } from '../../shared/models/product.model';
import { ConverterService } from '../converter/converter.service';
import { environment } from '../../../environments/environment';

@Injectable()
export class SuppliesService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/supplies`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getAll(): Observable<Supply[]> {
    return this.http.get(this.endPoint)
      .map((res: any) => this.converterService.convertToSupplies(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  getById(supplyId: string): Observable<Supply> {
    return this.http.get(`${this.endPoint}/${supplyId}`)
      .map((res: any) => this.converterService.convertToSupply(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  create(supply: Supply): Observable<Supply> {
    //const requestModel = this.convertToRequestData(delivery);

    return this.http.post(this.endPoint, supply)
      .map((res: any) => this.converterService.convertToSupply(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  update(supplyId: string, supply: Supply): Observable<Supply> {
    const requestModel = this.convertToRequestData(supply);

    return this.http.put(`${this.endPoint}/${supplyId}`, requestModel)
      .map((res: any) => this.converterService.convertToSupply(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }

  delete(supplyId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${supplyId}`)
      .catch((res: any) => throwError(res.error.messages));
  }

  private convertToRequestData(supply: Supply): any {
    return {
      supplierId: supply.supplier,
      requestDate: supply.requestDate,
      receivedDate: supply.receivedDate,
    };
  }
}
