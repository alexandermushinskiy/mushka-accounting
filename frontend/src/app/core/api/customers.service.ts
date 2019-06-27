import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { ConverterService } from '../converter/converter.service';
import { environment } from '../../../environments/environment';
import { Customer } from '../../shared/models/customer.model';

@Injectable()
export class CustomersService {
  private readonly endPoint = `${environment.apiEndpoint}/api/v1/customers`;

  constructor(private http: HttpClient,
              private converterService: ConverterService) {
  }

  getByName(name: string): Observable<Customer[]> {
    return this.http.get(`${this.endPoint}/filter?name=${name}`)
      .map((res: any) => this.converterService.convertToCustomers(res.data))
      .catch((res: any) => throwError(res.error.messages));
  }
}
