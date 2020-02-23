import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

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
    return this.http.get(`${this.endPoint}`).pipe(
      map((res: any) => this.converterService.convertToSuppliers(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  getById(supplierId: string): Observable<Supplier> {
    return this.http.get(`${this.endPoint}/${supplierId}`).pipe(
      map((res: any) => this.converterService.convertToSupplier(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  create(supplier: Supplier): Observable<Supplier> {
    return this.http.post(this.endPoint, supplier).pipe(
      map((res: any) => this.converterService.convertToSupplier(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  update(supplierId: string, supplier: Supplier): Observable<Supplier> {
    return this.http.put(`${this.endPoint}/${supplierId}`, supplier).pipe(
      map((res: any) => this.converterService.convertToSupplier(res.data)),
      catchError((res: any) => throwError(res.error.messages)));
  }

  delete(supplierId: string) {
    return this.http.delete(`${this.endPoint}/${supplierId}`).pipe(
      catchError((res: any) => throwError(res.error.messages)));
  }
}
