import { Injectable } from '@angular/core';

import { Expense } from '../../../expenses/models/expense.model';
import { ApiSearchExpenses } from '../interfaces/api-search-expenses.interface';
import { ItemsList } from '../../../shared/interfaces/items-list.interface';
import { ApiDescribeExpense } from '../interfaces/api-describe-expense.interface';

@Injectable({
  providedIn: 'root'
})
export class ApiExpensesTransformService {
  fromSearchExpenses(data: ApiSearchExpenses.Response): ItemsList<Expense> {
    const items = data.items || [];

    return {
      items: items.map(item => this.toExpense(item)),
      length: data.total
    };
  }

  fromDescribeExpense(response: ApiDescribeExpense.Response): Expense {
    return new Expense({
      id: response.id,
      createdOn: response.createdOn,
      cost: response.cost,
      category: response.category,
      purpose: response.purpose,
      supplierName: response.supplierName,
      notes: response.notes,
      costMethod: response.costMethod
    });
  }

  toExpense(source: ApiSearchExpenses.Expense): Expense {
    return new Expense({
      id: source.id,
      createdOn: source.createdOn,
      cost: source.cost,
      category: source.category,
      purpose: source.purpose,
      supplierName: source.supplierName
    });
  }

}
