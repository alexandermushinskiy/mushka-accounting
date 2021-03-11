import { FiltersBase } from './filter-base';
import { DateRange } from '../models/date-range.model';
import { Expense } from '../../expenses/models/expense.model';
import { ExpenseCategory } from '../../expenses/enums/expense-category.enum';

export class ExpenseListFilter extends FiltersBase {
  constructor(private searchKey: string,
              private category: ExpenseCategory,
              private dateRange: DateRange) {
    super();
  }

  filter(expense: Expense): boolean {
    let isInDateRange = true;
    let foundBySearch = true;
    let foundByCategory = true;

    if (!!this.dateRange) {
      isInDateRange = this.isBetween(expense.createdOn, this.dateRange);
    }

    if (!!this.category) {
      foundByCategory = expense.category === this.category;
    }

    if (!!this.searchKey) {
      foundBySearch = [expense.supplierName, expense.purpose]
        .some(field => field.toLowerCase().includes(this.searchKey.toLowerCase()));
    }

    return isInDateRange && foundByCategory && foundBySearch;
  }
}
