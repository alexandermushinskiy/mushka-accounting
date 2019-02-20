import { PaymentMethod } from '../enums/payment-method.enum';

export class Expense {
  id: string;
  createdOn: string;
  cost: number;
  costMethod: PaymentMethod;
  
  constructor(data: any) {
    Object.assign(this, data);
  }
}
