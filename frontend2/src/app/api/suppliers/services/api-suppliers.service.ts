import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { Supplier } from '../../../shared/models/supplier.model';
import { ApiDescribeSupplier } from '../interfaces/api-describe-supplier.interface';
import { ApiSearchSuppliers } from '../interfaces/api-search-suppliers.interface';
import { ApiSuppliersTransformService } from './api-suppliers-transform.service';

@Injectable({
  providedIn: 'root'
})
export class ApiSuppliersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/suppliers`;

  constructor(private http: HttpClient,
              private transformService: ApiSuppliersTransformService) {
  }

  searchSuppliers$(): Observable<ItemsList<Supplier>> {
    const url = `${this.endPoint}/search`;

    return this.http.post(url, {})
      .pipe(
        map((data: ApiSearchSuppliers.Response) => this.transformService.fromSearchSuppliers(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  describeSupplier$(supplierId: string): Observable<Supplier> {
    const url = `${this.endPoint}/${supplierId}/describe`;

    return this.http.get(url)
      .pipe(
        map((data: ApiDescribeSupplier.Response) => this.transformService.fromDescribeSupplier(data)),
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  createSupplier$(supplier: Supplier): Observable<void> {
    return this.http.post<void>(this.endPoint, this.transformService.toCreateSupplier(supplier))
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  updateSupplier$(supplierId: string, supplier: Supplier): Observable<void> {
    const url = `${this.endPoint}/${supplierId}`;

    return this.http.put<void>(url, this.transformService.toUpdateSupplier(supplier))
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }

  deleteSupplier$(supplierId: string): Observable<void> {
    return this.http.delete<void>(`${this.endPoint}/${supplierId}`)
      .pipe(
        catchError((res: any) => throwError(res.error.messages))
      );
  }
}
