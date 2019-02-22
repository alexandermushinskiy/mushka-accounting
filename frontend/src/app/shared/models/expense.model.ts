import { PaymentMethod } from '../enums/payment-method.enum';
import { ExpenseCategory } from '../enums/expense-category.enum';

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
        return 'Реклама';
      
      case ExpenseCategory.EQUIPMENT:
        return 'Оборудование';

      case ExpenseCategory.PHOTOGRAPHY:
        return 'Фото';
    
      case ExpenseCategory.DESIGN:
        return 'Дизайн';

      case ExpenseCategory.WEBSITE:
        return 'Веб сайт';

      case ExpenseCategory.POLYGRAPHY:
        return 'Полиграфия';
      
      case ExpenseCategory.PROMO:
        return 'Промо';

      default:
        return 'Unknown category';
    }
  }
}
