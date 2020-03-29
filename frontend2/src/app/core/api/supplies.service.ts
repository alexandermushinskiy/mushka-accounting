import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { Supply } from '../../shared/models/supply.model';
import { ConverterService } from '../converter/converter.service';
import { environment } from '../../../environments/environment';
import { SupplyList } from '../../shared/models/supply-list.model';
import { ItemsWithTotalCount } from '../../shared/models/items-with-total-count.model';

@Injectable()
export class SuppliesService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/supplies`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  get(searchKey: string, productId: string): Observable<ItemsWithTotalCount<SupplyList>> {
    const params = this.converterService.convertToHttpParams({ searchKey, productId });

    return this.http.get(this.endPoint, { params }).pipe(
      map((res: any) => this.converterService.convertToSuppliesListWithCount(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getById(supplyId: string): Observable<Supply> {
    return this.http.get(`${this.endPoint}/${supplyId}`).pipe(
      map((res: any) => this.converterService.convertToSupply(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  create(supply: Supply): Observable<Supply> {
    return this.http.post(this.endPoint, supply).pipe(
      map((res: any) => this.converterService.convertToSupply(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  update(supplyId: string, supply: Supply): Observable<Supply> {
    return this.http.put(`${this.endPoint}/${supplyId}`, supply).pipe(
      map((res: any) => this.converterService.convertToSupply(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  delete(supplyId: string): Observable<any> {
    return this.http.delete(`${this.endPoint}/${supplyId}`).pipe(
      catchError((res: any) => throwError(res.error.messages)));
  }

  export(supplyIds: string[], productIds: string[] = []): Observable<Blob> {
    const requestBody = {
      supplyIds,
      productIds
    };
    return this.http.post(`${this.endPoint}/export`, requestBody, { responseType: 'blob', observe: 'response' })
      .pipe(
        map((res: any) => res.body),
        catchError((res: any) => throwError(res.error.messages))
      );
  }
}
