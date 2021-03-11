import { ExpenseCategory } from '../../../expenses/enums/expense-category.enum';
import { PaymentMethod } from '../../../shared/enums/payment-method.enum';

export namespace ApiDescribeExpense {
  export interface Response {
    id: string;
    createdOn: string;
    cost: number;
    costMethod: PaymentMethod;
    category: ExpenseCategory;
    purpose: string;
    supplierName: string;
    notes: string;
  }
}
