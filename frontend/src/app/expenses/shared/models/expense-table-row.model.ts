import { DataTableRow } from '../../../shared/models/data-table-row.model';
import { Expense } from '../../../shared/models/expense.model';

export class ExpenseTableRow extends DataTableRow  {
  createdOn: string;
  cost: number;
  category: string;
  purpose: string;
  supplierName: string;

  constructor(elem: Expense, index: number = 0) {
    super(elem, index);

    this.createdOn = elem.createdOn;
    this.cost = elem.cost;
    this.category = elem.getCategoryDescription();
    this.purpose = elem.purpose;
    this.cost = elem.cost;
    this.supplierName = elem.supplierName;
  }
}
