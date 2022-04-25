import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { Customer } from '../../../shared/models/customer.model';
import { ApiCustomersTransformService } from './api-customers-transform.service';

@Injectable({
  providedIn: 'root'
})
export class ApiCustomersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/customers`;

  constructor(private http: HttpClient,
              private transformService: ApiCustomersTransformService) {
  }

  getCustomerByName$(name: string): Observable<Customer[]> {
    return this.http.get(`${this.endPoint}/filter?name=${name}`)
      .pipe(
        map((res: any) => this.transformService.fromGetCustomersByName(res)),
        catchError((error: any) => throwError(error))
      );
  }
}
