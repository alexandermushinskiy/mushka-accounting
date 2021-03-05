import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ItemsWithTotalCount } from '../../../shared/models/items-with-total-count.model';
import { SupplyList } from '../../../shared/models/supply-list.model';
import { Supply } from '../../../shared/models/supply.model';
import { ApiSearchSupplies } from '../interfaces/api-search-supplies.interface';
import { ApiSuppliesTransformService } from './api-supplies-transform.service';

@Injectable({
  providedIn: 'root'
})
export class ApiSuppliesService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/supplies`;

  constructor(private http: HttpClient,
              private transformService: ApiSuppliesTransformService) {
  }

  searchSupplies$(searchKey: string, productId: string): Observable<ItemsWithTotalCount<SupplyList>> {
    const url = `${this.endPoint}/search`;
    const body = this.transformService.toSearchOrders(searchKey, productId);

    return this.http.post(url, body)
      .pipe(
        map((data: ApiSearchSupplies.Response) => this.transformService.fromSearchOrders(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  describeSupply$(supplyId: string): Observable<Supply> {
    return this.http.get(`${this.endPoint}/${supplyId}/describe`)
      .pipe(
        map((data: any) => this.transformService.fromDescribeSupply(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  createSupply$(supply: Supply): Observable<void> {
    return this.http.post<void>(this.endPoint, supply)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  updateSupply$(supplyId: string, supply: Supply): Observable<void> {
    return this.http.put<void>(`${this.endPoint}/${supplyId}`, supply)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  deleteSupply$(supplyId: string): Observable<void> {
    return this.http.delete<void>(`${this.endPoint}/${supplyId}`)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  exportSupply$(supplyIds: string[], productIds: string[] = []): Observable<Blob> {
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
