import { Injectable } from '@angular/core';

import { Customer } from '../../../shared/models/customer.model';
import { ApiGetCustomerByName } from '../interfaces/api-get-customer-by-name.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiCustomersTransformService {
  fromGetCustomersByName({ customers }: ApiGetCustomerByName.Response): Customer[] {
    return customers.map(customer => this.toCustomer(customer));
  }

  private toCustomer(customer: ApiGetCustomerByName.Customer): Customer {
    return new Customer({
      id: customer.id,
      firstName: customer.firstName,
      lastName: customer.lastName,
      phone: customer.phone,
      email: customer.email
    });
  }
}
