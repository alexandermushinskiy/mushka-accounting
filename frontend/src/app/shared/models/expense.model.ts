import { PaymentMethod } from '../enums/payment-method.enum';
import { ExpenseCategory } from '../enums/expense-category.enum';
import { expenceCategory } from '../constants/expence-category.const';

export class Expense {
  id: string;
  createdOn: string;
  cost: number;
  costMethod: PaymentMethod;
  category: ExpenseCategory;
  purpose: string;
  supplierName: string;
  notes: string;
  
  constructor(data: any) {
    Object.assign(this, data);
  }

  getCategoryDescription(): string {
    switch(this.category) {
      case ExpenseCategory.ADVERTISING:
        return expenceCategory.advertising;
      
      case ExpenseCategory.EQUIPMENT:
        return expenceCategory.equipment;

      case ExpenseCategory.PHOTOGRAPHY:
        return expenceCategory.photography;
    
      case ExpenseCategory.DESIGN:
        return expenceCategory.design;

      case ExpenseCategory.WEBSITE:
        return expenceCategory.website;

      case ExpenseCategory.POLYGRAPHY:
        return expenceCategory.polygraphy;
      
      case ExpenseCategory.PROMO:
        return expenceCategory.promo;

      case ExpenseCategory.OTHER:
        return expenceCategory.other;

      default:
        return 'Unknown category';
    }
  }
}
